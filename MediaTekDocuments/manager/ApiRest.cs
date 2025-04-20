using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MediaTekDocuments.manager
{
    /// <summary>
    /// Classe indépendante d'accès à une api rest avec éventuellement une "basic authorization"
    /// </summary>
    class ApiRest
    {
        /// <summary>
        /// unique instance de la classe
        /// </summary>
        private static ApiRest instance = null;
        /// <summary>
        /// Objet de connexion à l'api
        /// </summary>
        private readonly HttpClient httpClient;
        /// <summary>
        /// Canal http pour l'envoi du message et la récupération de la réponse
        /// </summary>
        private HttpResponseMessage httpResponse;

        /// <summary>
        /// Constructeur privé pour préparer la connexion (éventuellement sécurisée)
        /// </summary>
        /// <param name="uriApi">adresse de l'api</param>
        /// <param name="authenticationString">chaîne d'authentification</param>
        private ApiRest(String uriApi, String authenticationString = "")
        {
            httpClient = new HttpClient() { BaseAddress = new Uri(uriApi) };
            // prise en compte dans l'url de l'authentificaiton (basic authorization), si elle n'est pas vide
            if (!String.IsNullOrEmpty(authenticationString))
            {
                String base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));
                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + base64EncodedAuthenticationString);
            }
        }

        /// <summary>
        /// Crée une instance unique de la classe
        /// </summary>
        /// <param name="uriApi">adresse de l'api</param>
        /// <param name="authenticationString">chaîne d'authentificatio (login:pwd)</param>
        /// <returns></returns>
        public static ApiRest GetInstance(String uriApi)
        {
            if (instance == null)
            {
                instance = new ApiRest(uriApi);
            }
            return instance;
        }

        /// <summary>
        /// Envoi une demande à l'API et récupère la réponse
        /// </summary>
        /// <param name="methode">verbe http (GET, POST, PUT, DELETE)</param>
        /// <param name="message">message à envoyer dans l'URL</param>
        /// <param name="parametres">contenu de variables à mettre dans body</param>
        /// <returns>liste d'objets (select) ou liste vide (ok) ou null si erreur</returns>
        public JObject RecupDistant(string methode, string message, String parametres)
        {
            // transformation des paramètres pour les mettre dans le body
            StringContent content = null;
            if (!(parametres is null))
            {
                content = new StringContent(parametres, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            }
            // envoi du message et attente de la réponse
            try
            {
                switch (methode)
                {
                    case "GET":
                        httpResponse = httpClient.GetAsync(message).Result;
                        break;
                    case "POST":
                        httpResponse = httpClient.PostAsync(message, content).Result;
                        break;
                    case "PUT":
                        httpResponse = httpClient.PutAsync(message, content).Result;
                        break;
                    case "DELETE":
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Delete,
                            RequestUri = new Uri(httpClient.BaseAddress + message),
                            Content = content
                        };
                        httpResponse = httpClient.SendAsync(request).Result;
                        break;
                    // methode incorrecte
                    default:
                        return new JObject();
                }

                // Lire le contenu de la réponse HTTP en tant que chaîne
                string responseContent = httpResponse.Content.ReadAsStringAsync().Result;

                // Vérifier si la réponse est du JSON valide
                if (responseContent.StartsWith("{") || responseContent.StartsWith("["))
                {
                    return JsonConvert.DeserializeObject<JObject>(responseContent);
                }
                else
                {
                    throw new Exception("La réponse de l'API n'est pas du JSON valide.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'accès à l'API : {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Envoi une demande GET à l'API et récupère la réponse de manière asynchrone
        /// </summary>
        /// <typeparam name="T">Type de l'objet attendu en réponse</typeparam>
        /// <param name="url">URL de l'API</param>
        /// <returns>Objet désérialisé de type T</returns>
        public async Task<T> GetApiResponseAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // Vérifier si la réponse est du JSON valide
                if (responseBody.StartsWith("{") || responseBody.StartsWith("["))
                {
                    return JsonConvert.DeserializeObject<T>(responseBody);
                }
                else
                {
                    throw new Exception("La réponse de l'API n'est pas du JSON valide.");
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
