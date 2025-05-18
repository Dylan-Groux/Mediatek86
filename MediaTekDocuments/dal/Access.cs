using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using MediaTekDocuments.manager;
using MediaTekDocuments.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

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
        private static readonly string uriApi = "http://localhost/testrest/rest_mediatekdocuments/";
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
        public async Task<List<Categorie>> GetAllGenres()
        {
            IEnumerable<Genre> lesGenres = await TraitementRecup<Genre>(GET, "genre", null);
            return new List<Categorie>(lesGenres);
        }

        /// <summary>
        /// Retourne tous les rayons à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public async Task<List<Categorie>> GetAllRayons()
        {
            IEnumerable<Rayon> lesRayons = await TraitementRecup<Rayon>(GET, "rayon", null);
            return new List<Categorie>(lesRayons);
        }

        /// <summary>
        /// Retourne toutes les catégories de public à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public async Task<List<Categorie>> GetAllPublics()
        {
            IEnumerable<Public> lesPublics = await TraitementRecup<Public>(GET, "public", null);
            return new List<Categorie>(lesPublics);
        }

        /// <summary>
        /// Retourne toutes les livres à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public async Task<List<Livre>> GetAllLivres()
        {
            List<Livre> lesLivres = await TraitementRecup<Livre>(GET, "livre", null);
            return lesLivres;
        }

        /// <summary>
        /// Retourne toutes les dvd à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Dvd</returns>
        public async Task<List<Dvd>> GetAllDvd()
        {
            List<Dvd> lesDvd = await TraitementRecup<Dvd>(GET, "dvd", null);
            return lesDvd;
        }

        /// <summary>
        /// Retourne toutes les revues à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public async Task<List<Revue>> GetAllRevues()
        {
            List<Revue> lesRevues = await TraitementRecup<Revue>(GET, "revue", null);
            return lesRevues;
        }


        /// <summary>
        /// Retourne les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocument">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public async Task<List<Exemplaire>> GetExemplairesRevue(string idDocument)
        {
            String jsonIdDocument = convertToJson("id", idDocument);
            List<Exemplaire> lesExemplaires = await TraitementRecup<Exemplaire>(GET, "exemplaire/" + jsonIdDocument, null);
            return lesExemplaires;
        }

        /// <summary>
        /// ecriture d'un exemplaire en base de données
        /// </summary>
        /// <param name="exemplaire">exemplaire à insérer</param>
        /// <returns>true si l'insertion a pu se faire (retour != null)</returns>
        public async Task<bool> CreerExemplaire(Exemplaire exemplaire)
        {
            String jsonExemplaire = JsonConvert.SerializeObject(exemplaire, new CustomDateTimeConverter());
            try
            {
                List<Exemplaire> liste = await TraitementRecup<Exemplaire>(POST, "exemplaire", "champs=" + jsonExemplaire);
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
        private async Task<List<T>> TraitementRecup<T>(string methode, string message, string parametres)
        {
            List<T> liste = new List<T>();
            try
            {
                JObject retour = await api.RecupDistant(methode, message, parametres);
                string code = (string)retour["code"];
                if (code.Equals("200") && methode.Equals(GET))
                {
                    string resultString = JsonConvert.SerializeObject(retour["result"]);
                    liste = JsonConvert.DeserializeObject<List<T>>(resultString, new CustomBooleanJsonConverter());
                }
                else
                {
                    Console.WriteLine("code erreur = " + code + " message = " + (string)retour["message"]);
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
        private async Task<bool> TraitementDelete(string table, string jsonChamps)
        {
            try
            {
                string postData = "champs=" + WebUtility.UrlEncode(jsonChamps);

                // Log de debug : ce qui est envoyé
                //MessageBox.Show("Tentative suppression\nTable : " + table + "\nData : " + postData, "DEBUG - DELETE");

                JObject retour = await api.RecupDistant(DELETE, table, postData);

                string code = (string)retour["code"];
                string retourMessage = (string)retour["message"];

                // MessageBox.Show("Code retour : " + code + "\nMessage : " + retourMessage, "DEBUG - Résultat API");

                return code == "200";
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception DELETE : " + ex.Message, "DEBUG - Exception");
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
        public async Task<List<Commande>> GetAllCommandes()
        {
            var commandes = await TraitementRecup<Commande>(GET, "commande", null);

            string ids = string.Join(", ", commandes.Select(c => c.Id));
            //MessageBox.Show("API a renvoyé : " + ids);

            return commandes;
        }

        /// <summary>
        /// Insertion d'une commande en base de données
        /// </summary>
        /// <param name="commande">Commande à insérer</param>
        /// <returns>true si l'insertion réussit</returns>
        public async Task<bool> CreerCommande(Commande commande)
        {
            string jsonCommande = JsonConvert.SerializeObject(commande, new CustomDateTimeConverter());
            //MessageBox.Show("JSON Commande envoyée : " + jsonCommande);
            try
            {
                List<Commande> liste = await TraitementRecup<Commande>(POST, "commande", "champs=" + jsonCommande);
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
        public async Task<List<Suivi>> GetAllSuivi()
        {
            List<Suivi> lesSuivi = await TraitementRecup<Suivi>(GET, "suivi", null);
            return lesSuivi;
        }


        /// <summary>
        /// Insertion d'un suivi en base de données
        /// </summary>
        /// <param name="suivi">Suivi à insérer</param>
        /// <returns>true si l'insertion réussit</returns>
        public async Task<bool> CreerSuivi(Suivi suivi)
        {
            string jsonSuivi = JsonConvert.SerializeObject(suivi, new CustomDateTimeConverter());
            //MessageBox.Show("JSON Commande envoyée : " + jsonSuivi);
            try
            {
                List<Suivi> liste = await TraitementRecup<Suivi>(POST, "suivi", "champs=" + jsonSuivi);
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
        public async Task<bool> UpdateStatutSuivi(string idSuivi, string idCommande, int nouveauStatut)
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
                    var response = await client.PutAsync($"http://localhost/rest_mediatekdocuments/suivi/{idSuivi}", content);

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
        public async Task<List<CommandesDocuments>> GetAllCommnadesDocuments()
        {
            List<CommandesDocuments> lesCommandesDocuments = await TraitementRecup<CommandesDocuments>(GET, "commandedocument", null);
            return lesCommandesDocuments;
        }

        /// <summary>
        /// Insertion d'une donnée de CommandesDocuments en base de données
        /// </summary>
        /// <param name="commandedocument">CommandesDocuments à insérer</param>
        /// <returns>true si l'insertion réussit</returns>
        public async Task<bool> CreerCommandeDocument(CommandesDocuments commandedocument)
        {
            string jsonCommandesDocuments = JsonConvert.SerializeObject(commandedocument, new CustomDateTimeConverter());
            //MessageBox.Show("JSON Commande envoyée : " + jsonCommandesDocuments);
            try
            {
                List<CommandesDocuments> liste = await TraitementRecup<CommandesDocuments>(POST, "commandedocument", "champs=" + jsonCommandesDocuments);
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
        public async Task<List<CommandesDocuments>> GetAllCommandesDocuments()
        {
            return await TraitementRecup<CommandesDocuments>("GET", "commandedocument", null);
        }
        #endregion

        #endregion

        public async Task<bool> SupprimerSuivi(Suivi suivi)
        {
            string jsonSuiviDelete = JsonConvert.SerializeObject(new { id = suivi.id_suivi });

            //MessageBox.Show("Tentative de suppression du suivi.\nJSON envoyé :\n" + jsonSuiviDelete, "DEBUG");

            try
            {
                return await TraitementDelete("suivi", jsonSuiviDelete);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du suivi : " + ex.Message, "DEBUG - Exception");
            }

            return false;
        }

        public async Task<bool> SupprimerCommandeDocument(CommandesDocuments commandedocument)
        {
            string jsonCommandeDocumentDelete = JsonConvert.SerializeObject(new { id = commandedocument.id_commandedocument });

            // Affichage du JSON préparé
            //MessageBox.Show("Tentative de suppression du CommandeDocument.\nDonnées envoyées :\n" + jsonCommandeDocumentDelete, "DEBUG - JSON CommandeDocument");

            try
            {
                return await TraitementDelete("commandedocument", jsonCommandeDocumentDelete);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du CommandeDocument : " + ex.Message, "DEBUG - Exception");
                //MessageBox.Show("Données envoyées : " + jsonCommandeDocumentDelete, "DEBUG - JSON Envoyé");
            }

            return false;
        }


        public async Task<bool> SupprimerCommande(Commande commande)
        {
            string jsonCommandeDelete = JsonConvert.SerializeObject(new { id = commande.Id });

            // Affichage du JSON préparé
            //MessageBox.Show("Tentative de suppression du Commande.\nDonnées envoyées :\n" + jsonCommandeDelete, "DEBUG - JSON Commande");

            try
            {
                return await TraitementDelete("commande", jsonCommandeDelete);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du Commande : " + ex.Message, "DEBUG - Exception");
                //MessageBox.Show("Données envoyées : " + jsonCommandeDelete, "DEBUG - JSON Envoyé");
            }

            return false;
        }

        #region DocumentUnitaire

        private List<DocumentUnitaire> documentunitaire;
        /// <summary>
        /// Retourne toutes les DocumentUnitaires à partir de la BDD
        /// </summary>
        /// <returns>Liste d'objets DocumentUnitaires</returns>
        public async Task<List<DocumentUnitaire>> GetAllDocumentUnitaires()
        {
            List<DocumentUnitaire> documentunitaire = await TraitementRecup<DocumentUnitaire>(GET, "documentunitaire", null);
            return documentunitaire;
        }

        /// <summary>
        /// Insertion d'une DocumentUnitaire en base de données
        /// </summary>
        /// <param name="DocumentUnitaire">DocumentUnitaire à insérer</param>
        /// <returns>true si l'insertion réussit</returns>
        public async Task<bool> CreerDocumentUnitaire(DocumentUnitaire documentunitaire)
        {
            string jsonDocumentUnitaire = JsonConvert.SerializeObject(documentunitaire, new CustomDateTimeConverter());
            MessageBox.Show("JSON DocumentUnitaire envoyée : " + jsonDocumentUnitaire);
            try
            {
                List<DocumentUnitaire> liste = await TraitementRecup<DocumentUnitaire>(POST, "documentunitaire", "champs=" + jsonDocumentUnitaire);
                return (liste != null);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'insertion de la DocumentUnitaire : " + ex.Message);
            }
            return false;
        }

        /// <summary>
        /// Met à jour l'état d'un document unitaire dans la base de données via l'API
        /// </summary>
        /// <param name="Id">ID du document unitaire</param>
        /// <param name="nouveauStatut">Le nouvel état à appliquer</param>
        /// <returns>Retourne true si la mise à jour a réussi, false sinon</returns>
        public async Task<bool> UpdateEtatDocumentUnitaire(string Id, int nouveauStatut)
        {
            // Prépare les données à envoyer
            var data = new Dictionary<string, string>
            {
                { "champs", JsonConvert.SerializeObject(new
                    {
                        id = Id,
                        etat = nouveauStatut,
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
                    var response = await client.PutAsync($"http://localhost/rest_mediatekdocuments/documentunitaire/{Id}", content);

                    return response.IsSuccessStatusCode;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la mise à jour de l'état du document unitaire : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> SupprimerDocumentUnitaire(DocumentUnitaire documentUnitaire)
        {
            string jsonDocUnitaireDelete = JsonConvert.SerializeObject(new { id = documentUnitaire.Id });

            // Affichage du JSON préparé
            //MessageBox.Show("Tentative de suppression du Commande.\nDonnées envoyées :\n" + jsonDocUnitaireDelete, "DEBUG - JSON Commande");

            try
            {
                return await TraitementDelete("documentunitaire", jsonDocUnitaireDelete);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la suppression du Commande : " + ex.Message, "DEBUG - Exception");
                //MessageBox.Show("Données envoyées : " + jsonCommandeDelete, "DEBUG - JSON Envoyé");
            }

            return false;
        }

        #endregion
        #region Login 
        public async Task<User> LoginUtilisateur(string username, string password)
        {
            try
            {
                // Récupération de tous les utilisateurs via GET
                List<User> users = await TraitementRecup<User>("GET", "user", null);

                if (users != null && users.Count > 0)
                {
                    // Parcours de la liste pour vérifier si un utilisateur correspond
                    foreach (var user in users)
                    {
                        if (user.Username == username && user.Password == password)
                        {
                            if (string.IsNullOrEmpty(user.Role))
                                user.Role = "salarie";

                            Session.CurrentUser = user;
                            MessageBox.Show("Connexion réussie !");
                            return user;
                        }
                    }
                }

                // Si aucun utilisateur trouvé
                MessageBox.Show("Identifiants incorrects.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la connexion : " + ex.Message);
            }

            return null;
        }
        #endregion
    }
}