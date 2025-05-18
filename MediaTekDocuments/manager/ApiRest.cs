using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediaTekDocuments.manager
{
    /// <summary>
    /// Classe indépendante d'accès à une API REST avec éventuellement une "basic authorization"
    /// </summary>
    class ApiRest
    {
        private static ApiRest instance = null;
        private readonly HttpClient httpClient;
        private HttpResponseMessage httpResponse;

        // Verrou et gestion du délai entre appels
        private static readonly object _lock = new object();
        private static DateTime _lastApiCall = DateTime.MinValue;
        private static readonly TimeSpan _minDelayBetweenCalls = TimeSpan.FromSeconds(2); // x2 = 2 secondes

        private ApiRest(string uriApi, string authenticationString = "")
        {
            httpClient = new HttpClient() { BaseAddress = new Uri(uriApi) };

            if (!string.IsNullOrEmpty(authenticationString))
            {
                string base64Auth = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64Auth);
            }

            httpClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static ApiRest GetInstance(string uriApi)
        {
            if (instance == null)
            {
                instance = new ApiRest(uriApi);
            }
            return instance;
        }

        /// <summary>
        /// Envoi une demande à l'API en synchronisé avec gestion du délai entre appels
        /// </summary>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">URL partielle</param>
        /// <param name="parametres">paramètres JSON dans body</param>
        /// <returns>JSON résultat (objet ou dernier élément si tableau), ou null si erreur</returns>
        public async Task<JObject> RecupDistant(string methode, string message, string parametres)
        {
            int maxRetries = 15;
            int retryDelaySeconds = 45; // 3 minutes en secondes

            Console.WriteLine("Lancement de la méthode RécupDistant");
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    if (message.StartsWith("/"))
                        message = message.Substring(1);

                    var fullUrl = new Uri(httpClient.BaseAddress, message);
                    Console.WriteLine($"Appel API: méthode={methode}, url={fullUrl}");

                    StringContent content = null;
                    if (!string.IsNullOrEmpty(parametres))
                        content = new StringContent(parametres, System.Text.Encoding.UTF8, "application/json");

                    switch (methode.ToUpper())
                    {
                        case "GET":
                            httpResponse = await httpClient.GetAsync(message);
                            break;
                        case "POST":
                            httpResponse = await httpClient.PostAsync(message, content);
                            break;
                        case "PUT":
                            httpResponse = await httpClient.PutAsync(message, content);
                            break;
                        case "DELETE":
                            var request = new HttpRequestMessage
                            {
                                Method = HttpMethod.Delete,
                                RequestUri = fullUrl,
                                Content = content
                            };
                            httpResponse = await httpClient.SendAsync(request);
                            break;
                        default:
                            Console.WriteLine($"Méthode HTTP inconnue: {methode}");
                            return null;
                    }

                    Console.WriteLine($"Status code: {httpResponse.StatusCode}");

                    // Si on reçoit un 429 : Too Many Requests
                    if (httpResponse.StatusCode == (HttpStatusCode)429)
                    {
                        Console.WriteLine($"Trop de requêtes — attente de {retryDelaySeconds} secondes avant de réessayer (tentative {attempt}/{maxRetries})");
                        Thread.Sleep(retryDelaySeconds * 1000);
                        continue; // repartir dans la boucle et réessayer
                    }

                    string responseContent = await httpResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"Réponse brute : {responseContent}");

                    if (string.IsNullOrWhiteSpace(responseContent))
                    {
                        Console.WriteLine("Réponse vide.");
                        return null;
                    }

                    if (responseContent.StartsWith("["))
                    {
                        JArray jsonArray = JArray.Parse(responseContent);
                        var jsonObject = new JObject();
                        jsonObject["result"] = jsonArray;
                        jsonObject["code"] = "200";
                        return jsonObject;
                    }
                    else if (responseContent.StartsWith("{"))
                    {
                        Console.WriteLine("Tentative de parsing JSON objet...");
                        try
                        {
                            JObject jsonObject = JObject.Parse(responseContent);
                            Console.WriteLine("Parsing réussi.");
                            return jsonObject;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erreur de parsing JSON : {ex.Message}");
                            Console.WriteLine($"Contenu problématique : {responseContent}");
                            throw;
                        }
                    }

                    else
                    {
                        Console.WriteLine("La réponse n'est pas un JSON valide.");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception dans RecupDistant : {ex.ToString()}");
                    return null;
                }
            }

            Console.WriteLine($"Echec après {maxRetries} tentatives.");
            return null;
        }


        /// <summary>
        /// Envoi une demande GET asynchrone à l'API avec délai entre appels
        /// </summary>
        /// <typeparam name="T">Type de retour</typeparam>
        /// <param name="url">URL partielle</param>
        /// <returns>Objet désérialisé de type T</returns>
        public async Task<T> GetApiResponseAsync<T>(string url)
        {
            // Gestion délai entre appels en async
            while (true)
            {
                lock (_lock)
                {
                    var now = DateTime.UtcNow;
                    var elapsed = now - _lastApiCall;
                    if (elapsed >= _minDelayBetweenCalls)
                    {
                        _lastApiCall = DateTime.UtcNow;
                        break;
                    }
                }
                var waitTime = _minDelayBetweenCalls - (DateTime.UtcNow - _lastApiCall);
                Console.WriteLine($"Attente {waitTime.TotalMilliseconds} ms avant appel API asynchrone");
                await Task.Delay(waitTime);
            }

            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                if (responseBody.StartsWith("{") || responseBody.StartsWith("["))
                {
                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
                else
                {
                    throw new Exception("La réponse de l'API n'est pas un JSON valide.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'accès à l'API : {ex.Message}");
                throw;
            }
        }
    }
}
