using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.manager;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Net;

namespace MediaTekDocuments.dal
{
    /// <summary>
    /// Classe d'accès aux données
    /// </summary>
    public class Access
    {
        /// <summary>
        /// adresse de l'API
        /// </summary>
        private static readonly string uriApi = "http://localhost/rest_mediatekdocuments/";
        /// <summary>
        /// instance unique de la classe
        /// </summary>
        private static Access instance = null;
        /// <summary>
        /// instance de ApiRest pour envoyer des demandes vers l'api et recevoir la réponse
        /// </summary>
        private readonly ApiRest api = null;
        /// <summary>
        /// méthode HTTP pour select
        /// </summary>
        private const string GET = "GET";
        /// <summary>
        /// méthode HTTP pour insert
        /// </summary>
        private const string POST = "POST";
        /// <summary>
        /// méthode HTTP pour update
        private const string DELETE = "DELETE";
        /// <summary>
        /// Méthode privée pour créer un singleton
        /// initialise l'accès à l'API
        /// </summary>
        public Access()
        {
            String authenticationString;
            try
            {
                authenticationString = "admin:adminpwd";
                api = ApiRest.GetInstance(uriApi);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Création et retour de l'instance unique de la classe
        /// </summary>
        /// <returns>instance unique de la classe</returns>
        public static Access GetInstance()
        {
            if (instance == null)
            {
                instance = new Access();
            }
            return instance;
        }

        /// <summary>
        /// Retourne tous les genres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public List<Categorie> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            List<Livre> lesLivres = TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            List<Dvd> lesDvd = TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            List<Revue> lesRevues = TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Traitement de la récupération du retour de l'api, avec conversion du json en liste pour les select (GET)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="methode">verbe HTTP (GET, POST, PUT, DELETE)</param>
        /// <param name="message">information envoyée dans l'url</param>
        /// <param name="parametres">paramètres à envoyer dans le body, au format "chp1=val1&chp2=val2&..."</param>
        /// <returns>liste d'objets récupérés (ou liste vide)</returns>
        private List<T> TraitementRecup<T>(String methode, String message, String parametres)
        {
            // trans
            List<T> liste = new List<T>();
            try
            {
                JObject retour = api.RecupDistant(methode, message, parametres);
                // extraction du code retourné
                String code = (String)retour["code"];
                if (code.Equals("200"))
                {
                    // dans le cas du GET (select), récupération de la liste d'objets
                    if (methode.Equals(GET))
                    {
                        String resultString = JsonConvert.SerializeObject(retour["result"]);
                        // construction de la liste d'objets à partir du retour de l'api
                        liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                    }
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (String)retour["message"]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur lors de l'accès à l'API : " + e.Message);
                MessageBox.Show($"Erreur lors de l'accès à l'API : {e.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            return liste;
        }

        /// <summary>
        /// Envoie une requête DELETE à l'API avec les paramètres spécifiés
        /// </summary>
        /// <param name="message">Nom de la table</param>
        /// <param name="parametres">Paramètres de suppression au format "champs={...}"</param>
        /// <returns>true si la suppression a réussi, false sinon</returns>
        private bool TraitementDelete(string table, string jsonChamps)
        {
            try
            {
                string postData = "champs=" + WebUtility.UrlEncode(jsonChamps);

                // Log de debug : ce qui est envoyé
                MessageBox.Show("Tentative suppression\nTable : " + table + "\nData : " + postData, "DEBUG - DELETE");

                JObject retour = api.RecupDistant(DELETE, table, postData);

                string code = (string)retour["code"];
                string retourMessage = (string)retour["message"];

                MessageBox.Show("Code retour : " + code + "\nMessage : " + retourMessage, "DEBUG - Résultat API");

                return code == "200";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception DELETE : " + ex.Message, "DEBUG - Exception");
                return false;
            }
        }



        /// <summary>
        /// Convertit en json un couple nom/valeur
        /// </summary>
        /// <param name="nom"></param>
        /// <param name="valeur"></param>
        /// <returns>couple au format json</returns>
        private String convertToJson(Object nom, Object valeur)
        {
            Dictionary<Object, Object> dictionary = new Dictionary<Object, Object>();
            dictionary.Add(nom, valeur);
            return JsonConvert.SerializeObject(dictionary);
        }

        /// <summary>
        /// Modification du convertisseur Json pour gérer le format de date
        /// </summary>
        private sealed class CustomDateTimeConverter : IsoDateTimeConverter
        {
            public CustomDateTimeConverter()
            {
                base.DateTimeFormat = "yyyy-MM-dd";
            }
        }

        /// <summary>
        /// Modification du convertisseur Json pour prendre en compte les booléens
        /// classe trouvée sur le site :
        /// https://www.thecodebuzz.com/newtonsoft-jsonreaderexception-could-not-convert-string-to-boolean/
        /// </summary>
        private sealed class CustomBooleanJsonConverter : JsonConverter<bool>
        {
            public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
            {
                return Convert.ToBoolean(reader.ValueType == typeof(string) ? Convert.ToByte(reader.Value) : reader.Value);
            }

            public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, value);
            }
        }

        #region Commandes

        private List<Commande> commandes;
        /// <summary>
        /// Retourne toutes les commandes à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Commandes</returns>
        public List<Commande> GetAllCommandes()
        {
            var commandes = TraitementRecup<Commande>(GET, "commande", null);

            string ids = string.Join(", ", commandes.Select(c => c.Id));
            //MessageBox.Show("API a renvoyé : " + ids);

            return commandes;
        }

        /// <summary>
        /// Insertion d'une commande en base de données
        /// </summary>
        /// <param name="commande">Commande à insérer</param>
        /// <returns>true si l'insertion réussit</returns>
        public bool CreerCommande(Commande commande)
        {
            string jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            //MessageBox.Show("JSON Commande envoyée : " + jsonCommande);
            try
            {
                List<Commande> liste = TraitementRecup<Commande>(POST, "commande", "champs=" + jsonCommande);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'insertion de la commande : " + ex.Message);
            }
            return false;
        }


        #endregion

        #region Suivi
        /// <summary>
        /// Retourne tous les suivi à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivi()
        {
            List<Suivi> lesSuivi = TraitementRecup<Suivi>(GET, "suivi", null);
            return lesSuivi;
        }


        /// <summary>
        /// Insertion d'un suivi en base de données
        /// </summary>
        /// <param name="suivi">Suivi à insérer</param>
        /// <returns>true si l'insertion réussit</returns>
        public bool CreerSuivi(Suivi suivi)
        {
            string jsonSuivi = JsonConvert.SerializeObject(suivi, new CustomDateTimeConverter());
            //MessageBox.Show("JSON Commande envoyée : " + jsonSuivi);
            try
            {
                List<Suivi> liste = TraitementRecup<Suivi>(POST, "suivi", "champs=" + jsonSuivi);
                return (liste != null);
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Erreur lors de l'insertion du suivi : " + ex.Message);
                // MessageBox.Show("Données envoyées : " + jsonSuivi);
            }
            return false;
        }

        /// <summary>
        /// Met à jour le statut d'une commande dans la base de données via l'API
        /// </summary>
        /// <param name="commandeId">ID de la commande</param>
        /// <param name="nouveauStatut">Le nouveau statut à appliquer</param>
        /// <returns>Retourne true si la mise à jour a réussi</returns>
        public bool UpdateStatutSuivi(string idSuivi, string idCommande, int nouveauStatut)
        {
            // Prépare les données à envoyer
            var data = new Dictionary<string, string>
            {
                { "champs", JsonConvert.SerializeObject(new
                    {
                        id_commande = idCommande,
                        status = nouveauStatut,
                        date_suivi = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    })
                }
            };

            try
            {
                // Crée le client HTTP
                using (var client = new HttpClient())
                {
                    var content = new FormUrlEncodedContent(data);

                    // Appel à l'API avec PUT
                    var response = client.PutAsync($"http://localhost/rest_mediatekdocuments/suivi/{idSuivi}", content).Result;

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour du suivi : " + ex.Message);
                return false;
            }
        }



        #endregion
        #region CommandesDocuments

        //Pour l'exploitation de la donnée par la suite (DGV - CBX  ...) 
        public List<CommandesDocuments> GetAllCommnadesDocuments()
        {
            List<CommandesDocuments> lesCommandesDocuments = TraitementRecup<CommandesDocuments>(GET, "commandedocument", null);
            return lesCommandesDocuments;
        }

        /// <summary>
        /// Insertion d'une donnée de CommandesDocuments en base de données
        /// </summary>
        /// <param name="commandedocument">CommandesDocuments à insérer</param>
        /// <returns>true si l'insertion réussit</returns>
        public bool CreerCommandeDocument(CommandesDocuments commandedocument)
        {
            string jsonCommandesDocuments = JsonConvert.SerializeObject(commandedocument, new CustomDateTimeConverter());
            //MessageBox.Show("JSON Commande envoyée : " + jsonCommandesDocuments);
            try
            {
                List<CommandesDocuments> liste = TraitementRecup<CommandesDocuments>(POST, "commandedocument", "champs=" + jsonCommandesDocuments);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'insertion de la commande : " + ex.Message);
            }
            return false;
        }

            #region Test de la table
            // Pour le test de la table 
            public List<CommandesDocuments> GetAllCommandesDocuments()
            {
                return TraitementRecup<CommandesDocuments>("GET", "commandedocument", null);
            }
        #endregion

        #endregion

        public bool SupprimerSuivi(Suivi suivi)
        {
            string jsonSuiviDelete = JsonConvert.SerializeObject(new { id = suivi.id_suivi });

            //MessageBox.Show("Tentative de suppression du suivi.\nJSON envoyé :\n" + jsonSuiviDelete, "DEBUG");

            try
            {
                return TraitementDelete("suivi", jsonSuiviDelete);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du suivi : " + ex.Message, "DEBUG - Exception");
            }

            return false;
        }

        public bool SupprimerCommandeDocument(CommandesDocuments commandedocument)
        {
            string jsonCommandeDocumentDelete = JsonConvert.SerializeObject(new { id = commandedocument.id_commandedocument });

            // Affichage du JSON préparé
            //MessageBox.Show("Tentative de suppression du CommandeDocument.\nDonnées envoyées :\n" + jsonCommandeDocumentDelete, "DEBUG - JSON CommandeDocument");

            try
            {
                return TraitementDelete("commandedocument", jsonCommandeDocumentDelete);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du CommandeDocument : " + ex.Message, "DEBUG - Exception");
                //MessageBox.Show("Données envoyées : " + jsonCommandeDocumentDelete, "DEBUG - JSON Envoyé");
            }

            return false;
        }


        public bool SupprimerCommande(Commande commande)
        {
            string jsonCommandeDelete = JsonConvert.SerializeObject(new { id = commande.Id });

            // Affichage du JSON préparé
            //MessageBox.Show("Tentative de suppression du Commande.\nDonnées envoyées :\n" + jsonCommandeDelete, "DEBUG - JSON Commande");

            try
            {
                return TraitementDelete("commande", jsonCommandeDelete);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du Commande : " + ex.Message, "DEBUG - Exception");
                //MessageBox.Show("Données envoyées : " + jsonCommandeDelete, "DEBUG - JSON Envoyé");
            }

            return false;
        }
    }
}
