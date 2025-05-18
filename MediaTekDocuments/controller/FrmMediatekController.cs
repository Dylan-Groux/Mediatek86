using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System.Linq;
using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using MediaTekDocuments.view;
using System.Threading;

namespace MediaTekDocuments.controller
{
    /// <summary>
    /// Contrôleur lié à FrmMediatek
    /// </summary>
    class FrmMediatekController
    {
        /// <summary>
        /// Objet d'accès aux données
        /// </summary>
        private readonly Access access;

        /// <summary>
        /// Récupération de l'instance unique d'accès aux données
        /// </summary>
        public FrmMediatekController()
        {
            
            access = Access.GetInstance();
        }

        /// <summary>
        /// getter sur la liste des genres
        /// </summary>
        /// <returns>Liste d'objets Genre</returns>
        public async Task<List<Categorie>> GetAllGenres()
        {
            
            return await access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public async Task<List<Livre>> GetAllLivres()
        {
            
            return await access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public async Task<List<Dvd>> GetAllDvd()
        {
            
            return await access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public async Task<List<Revue>> GetAllRevues()
        {
            
            return await access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public async Task<List<Categorie>> GetAllRayons()
        {
            
            return await access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public async Task<List<Categorie>> GetAllPublics()
        {
            
            return await access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public async Task<List<Exemplaire>> GetExemplairesRevue(string idDocuement)
        {
            
            return await access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public async Task<bool> CreerExemplaire(Exemplaire exemplaire)
        {
            
            return await access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// getter sur la liste des Commandes
        /// </summary>
        /// <returns>Liste d'objets Commandes</returns>
        public async Task<List<Commande>> GetAllCommandes()
        {
            
            var commandes = access.GetAllCommandes();

            //string allIds = string.Join(", ", commandes.Select(c => c.Id));
            //MessageBox.Show("GetAllCommandes(): " + allIds);

            return await commandes;
        }


        /// <summary>
        /// getter sur la liste des Suivi
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public async Task<List<Suivi>> GetAllSuivi()
        {
            
            var suivis = access.GetAllSuivi();

           // if (suivis == null)
           //{
           //     MessageBox.Show("Erreur : La liste des suivis est vide.");
           //     return new List<Suivi>(); // Retourne une liste vide si null
           // }

            return await suivis;
        }

        public async Task<Commande> GetCommandeAvecSuivis(string commandeId)
        {
            var commandes = await GetAllCommandes();
            var commande = commandes.FirstOrDefault(c => c.Id == commandeId);

            if (commande != null)
            {
                var suivis = await GetAllSuivi();
                var suivisDeLaCommande = suivis.Where(s => s.IdCommande == commande.Id).ToList();
            }

            return commande;
        }


        public async Task<List<CommandeSuiviDTO>> GetCommandesSuivisDTO()
        {
            
            var commandes = await GetAllCommandes();
            var suivis = await GetAllSuivi();
            var commandesdocuments = await GetAllCommnadesDocuments();

            // Log pour vérifier la taille des listes
            // MessageBox.Show($"Commandes : {commandes.Count}, Suivis : {suivis.Count}");

            // La jointure entre Commande et Suivi
            var result = from c in commandes
                         join s in suivis on c.Id equals s.IdCommande into csGroup
                         from s in csGroup.DefaultIfEmpty()  // Utilisation de DefaultIfEmpty pour gérer les suivis nuls
                         let doc = commandesdocuments.FirstOrDefault(cd => cd.id_commande == c.Id)
                         select new CommandeSuiviDTO
                         {
                             CommandeId = c.Id,
                             DateCommande = c.DateCommande,
                             Montant = c.Montant,
                             // Vérifier si 's' est null avant d'accéder à ses propriétés
                             SuiviId = s != null ? s.id_suivi : null,
                             DateSuivi = s?.DateSuivi,
                             StatutSuivi = s?.Status ?? -1,// Gestion du cas où 's' est null
                             LiaisonCommandeDocument = doc
                         };

            // Retourner le résultat sous forme de liste
            return result.ToList();
        }

        public async Task<List<CommandesDocuments>> GetAllCommnadesDocuments()
        {
            
            return await access.GetAllCommnadesDocuments();
        }


        /// <summary>
        /// Crée une nouvelle commande dans la BDD
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public async Task<bool> CreerCommande(Commande commande)
        {
            
            return await access.CreerCommande(commande);
        }




        /// <summary>
        /// Crée une nouvelle donnée de suivi dans la BDD
        /// </summary>
        /// <param name="suivi"></param>
        /// <returns></returns>
        public async Task<bool> CreerSuivi(Suivi suivi)
        {
            
            return await access.CreerSuivi(suivi);
        }




        /// <summary>
        /// Crée une nouvelle donnée de CommandesDocuments dans la BDD
        /// </summary>
        /// <param name="commandedocument"></param>
        /// <returns></returns>
        public async Task<bool> CreerCommandeDocument(CommandesDocuments commandedocument)
        {
            return await access.CreerCommandeDocument(commandedocument);
        }

        /// <summary>
        /// Génère un ID pour la table commande id
        /// <desc>
        /// On récupère ici la totalité des commandes via un appel APi direct, on le stock dans une varaible allCommandes,
        /// On exploite cette variable en la triant par ordre décroissant en triant par l'ID, stock dans une variable lastCommandeId
        /// Si null dans ce cas on affiche C001 car ce serait la première commande, sinon on extrait le préfix et la partie numérique dans deux variables
        /// dans notre cas on force le préfix "C" => contrôle saisit utilisateur au cas ou.
        /// on vérifie que l'exactration de la partie numérique est valide (int) et on transmet cela dans la variable lastNumber
        /// on incrémente de 1 la partie numérique puis on recolle le tout avec le nouveau nombre et le préfix, on ajoute "D3" pour s'assurer du bon format.
        /// </desc>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public async Task<string> GenerateCommandeId()
        {
            
            var allCommandes = await access.GetAllCommandes();
            var lastCommandeId = allCommandes
                .OrderByDescending(c => c.Id) 
                .FirstOrDefault()?.Id;

            //MessageBox.Show("Dernier ID trouvé : " + lastCommandeId);

            if (allCommandes == null || allCommandes.Count == 0)
            {
                MessageBox.Show("Aucune commande trouvée dans GenerateCommandeId()");
            }
            else
            {
                string allIds = string.Join(", ", allCommandes.Select(c => c.Id));
                //MessageBox.Show("Commandes récupérées dans GenerateCommandeId(): " + allIds);
            }

            if (string.IsNullOrEmpty(lastCommandeId))
            {
                return "C001";
            }

            // Extraire la partie numérique de l'ID, en excluant le préfixe (par exemple, "C001" -> "001", "MD000" -> "000")
            string prefix = new string(lastCommandeId.TakeWhile(char.IsLetter).ToArray()); // Extrait le préfixe, par exemple "C"
            string numberPart = new string(lastCommandeId.SkipWhile(char.IsLetter).ToArray()); // Extrait la partie numérique

            // Si le préfixe n'est pas "C", forcer "C" comme préfixe pour les nouvelles commandes
            if (prefix != "C")
            {
                prefix = "C";
            }

            // Vérifie que la partie numérique est un nombre valide
            if (!int.TryParse(numberPart, out int lastNumber))
            {
                throw new FormatException($"Le suffixe de l'ID de la commande n'est pas un nombre valide : {numberPart}");
            }

            // Incrémenter de 1
            int newNumber = lastNumber + 1;

            // Formater l'ID avec le préfixe et un padding de 3 chiffres (par exemple, "C001" si le préfixe est "C")
            string newCommandeId = prefix + newNumber.ToString("D3"); // Utilise "D3" pour s'assurer que le nombre a toujours 3 chiffres

            return newCommandeId;


        }


        /// <summary>
        /// Génère un ID pour la table commande id
        /// <desc>erdfhjnjjjjjjjjjjjjjjjjjjjjjjjjjjjjjeds&ééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééééét(-------
        /// tgyhioxkc/// On récupère ici la totalité des commandes via un appel APi direct, on le stock dans une varaible allCommandes,
        /// On exploite cette variable en la triant par ordre décroissant en triant par l'ID, stock dans une variable lastCommandeId
        /// Si null dans ce cas on affiche C001 car ce serait la première commande, sinon on extrait le préfix et la partie numérique dans deux variables
        /// dans notre cas on force le préfix "C" => contrôle saisit utilisateur au cas ou.
        /// on vérifie que l'exactration de la partie numérique est valide (int) et on transmet cela dans la variable lastNumber
        /// on incrémente de 1 la partie numérique puis on recolle le tout avec le nouveau nombre et le préfix, on ajoute "D3" pour s'assurer du bon format.
        /// </desc>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public async Task<string> GenerateSuiviId()
        {
            
            var allSuivis = await access.GetAllSuivi();
            var lastSuiviId = allSuivis
                .OrderByDescending(c => c.id_suivi)
                .FirstOrDefault()?.id_suivi;

            //MessageBox.Show("Dernier ID trouvé : " + lastSuiviId);

            if (allSuivis == null || allSuivis.Count == 0)
            {
                MessageBox.Show("Aucune Suivi trouvée dans GenerateSuiviId()");
            }
            else
            {
                string allIds = string.Join(", ", allSuivis.Select(c => c.id_suivi));
                //MessageBox.Show("Suivis récupérées dans GenerateSuiviId(): " + allIds);
            }

            if (string.IsNullOrEmpty(lastSuiviId))
            {
                return "S001";
            }

            // Extraire la partie numérique de l'ID, en excluant le préfixe (par exemple, "C001" -> "001", "MD000" -> "000")
            string prefix = new string(lastSuiviId.TakeWhile(char.IsLetter).ToArray()); // Extrait le préfixe, par exemple "C"
            string numberPart = new string(lastSuiviId.SkipWhile(char.IsLetter).ToArray()); // Extrait la partie numérique

            // Si le préfixe n'est pas "S", forcer "S" comme préfixe pour les nouvelles Suivis
            if (prefix != "S")
            {
                prefix = "S";
            }

            // Vérifie que la partie numérique est un nombre valide
            if (!int.TryParse(numberPart, out int lastNumber))
            {
                throw new FormatException($"Le suffixe de l'ID de la Suivi n'est pas un nombre valide : {numberPart}");
            }

            // Incrémenter de 1
            int newNumber = lastNumber + 1;

            // Formater l'ID avec le préfixe et un padding de 3 chiffres (par exemple, "C001" si le préfixe est "C")
            string newSuiviId = prefix + newNumber.ToString("D3"); // Utilise "D3" pour s'assurer que le nombre a toujours 3 chiffres

            return newSuiviId;


        }



        /// <summary>
        /// Génère un ID pour la table commande id
        /// <desc>
        /// On récupère ici la totalité des commandes via un appel APi direct, on le stock dans une varaible allCommandes,
        /// On exploite cette variable en la triant par ordre décroissant en triant par l'ID, stock dans une variable lastCommandeId
        /// Si null dans ce cas on affiche C001 car ce serait la première commande, sinon on extrait le préfix et la partie numérique dans deux variables
        /// dans notre cas on force le préfix "C" => contrôle saisit utilisateur au cas ou.
        /// on vérifie que l'exactration de la partie numérique est valide (int) et on transmet cela dans la variable lastNumber
        /// on incrémente de 1 la partie numérique puis on recolle le tout avec le nouveau nombre et le préfix, on ajoute "D3" pour s'assurer du bon format.
        /// </desc>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public async Task<string> GenerateCommandeDocumentId()
        {
            
            var allCommandeDocuments = await access.GetAllCommandesDocuments();
            var lastCommandeDocumentId = allCommandeDocuments
                .OrderByDescending(c => c.id_commandedocument)
                .FirstOrDefault()?.id_commandedocument;

            //MessageBox.Show("Dernier ID trouvé : " + lastCommandeDocumentId);

            if (allCommandeDocuments == null || allCommandeDocuments.Count == 0)
            {
                MessageBox.Show("Aucune CommandeDocument trouvée dans GenerateCommandeDocumentId()");
            }
            else
            {
                string allIds = string.Join(", ", allCommandeDocuments.Select(c => c.id_commandedocument));
                //MessageBox.Show("CommandeDocuments récupérées dans GenerateCommandeDocumentId(): " + allIds);
            }

            if (string.IsNullOrEmpty(lastCommandeDocumentId))
            {
                return "CD001";
            }

            // Extraire la partie numérique de l'ID, en excluant le préfixe (par exemple, "C001" -> "001", "MD000" -> "000")
            string prefix = new string(lastCommandeDocumentId.TakeWhile(char.IsLetter).ToArray()); // Extrait le préfixe, par exemple "C"
            string numberPart = new string(lastCommandeDocumentId.SkipWhile(char.IsLetter).ToArray()); // Extrait la partie numérique

            // Si le préfixe n'est pas "S", forcer "S" comme préfixe pour les nouvelles CommandeDocuments
            if (prefix != "CD")
            {
                prefix = "CD";
            }

            // Vérifie que la partie numérique est un nombre valide
            if (!int.TryParse(numberPart, out int lastNumber))
            {
                throw new FormatException($"Le suffixe de l'ID de la CommandeDocument n'est pas un nombre valide : {numberPart}");
            }

            // Incrémenter de 1
            int newNumber = lastNumber + 1;

            // Formater l'ID avec le préfixe et un padding de 3 chiffres (par exemple, "C001" si le préfixe est "C")
            string newCommandeDocumentId = prefix + newNumber.ToString("D3"); // Utilise "D3" pour s'assurer que le nombre a toujours 3 chiffres

            return newCommandeDocumentId;


        }

        /// <summary>
        /// Gère la mise à jour du statut d'une commande
        /// </summary>
        /// <param name="commandeId">ID de la commande</param>
        /// <param name="nouveauStatut">Le nouveau statut à appliquer</param>
        /// <returns>Retourne true si la mise à jour a réussi</returns>
        public async Task<bool> ModifierStatutCommande(string idSuivi, string commandeId, int nouveauStatut)
        {
            
            return await access.UpdateStatutSuivi(idSuivi, commandeId, nouveauStatut);
        }

        public async Task<bool> SupprimerCommande(string commandeId)
        {
            
            //MessageBox.Show(commandeId);
            var commande = new Commande { Id = commandeId };
            return await access.SupprimerCommande(commande);
        }

        public async Task<bool> SupprimerCommandeDocument(string commandedocumentId)
        {
            
            //MessageBox.Show(commandedocumentId);
            var commandedocument = new CommandesDocuments { id_commandedocument = commandedocumentId  };
            return await access.SupprimerCommandeDocument(commandedocument);
        }

        public async Task<bool> SupprimerNbExemplaire(string commandedocumentId)
        {
            
            //MessageBox.Show(commandedocumentId);
            var commandedocument = new CommandesDocuments { id_commandedocument = commandedocumentId };
            return await access.SupprimerCommandeDocument(commandedocument);
        }


        public async Task<bool> SupprimerSuivi(string idSuivi)
        {
            
            //MessageBox.Show(idSuivi);
            var suivi = new Suivi { id_suivi = idSuivi };
            return await access.SupprimerSuivi(suivi);
        }

        #region DocumentUnitaire
        public async Task<List<DocumentUnitaire>> GetAllDocumentUnitaires()
        {
            
            var documentunitaire = access.GetAllDocumentUnitaires();

            return await documentunitaire;
        }


        /// <summary>
        /// Crée une nouvelle donnée de suivi dans la BDD
        /// </summary>
        /// <param name="suivi"></param>
        /// <returns></returns>
        public async Task<bool> CreerDocumentUnitaire(DocumentUnitaire documentUnitaire)
        {
            
            return await access.CreerDocumentUnitaire(documentUnitaire);
        }


        public async Task<string> GenerateDocumentUnitaireId()
        {
            
            var allDocumentUnitaire = await access.GetAllDocumentUnitaires();
            var lastCommandeDocumentId = allDocumentUnitaire
                .OrderByDescending(c => c.Id)
                .FirstOrDefault()?.Id;

            //MessageBox.Show("Dernier ID trouvé : " + lastCommandeDocumentId);

            if (allDocumentUnitaire == null || allDocumentUnitaire.Count == 0)
            {
                //MessageBox.Show("Aucune CommandeDocument trouvée dans GenerateCommandeDocumentId()");
            }
            else
            {
                string allIds = string.Join(", ", allDocumentUnitaire.Select(c => c.Id));
                //MessageBox.Show("DocumentUnitaire récupérées dans GenerateCommandeDocumentId(): " + allIds);
            }

            if (string.IsNullOrEmpty(lastCommandeDocumentId))
            {
                return "DU00001";
            }

            // Extraire la partie numérique de l'ID, en excluant le préfixe (par exemple, "C001" -> "001", "MD000" -> "000")
            string prefix = new string(lastCommandeDocumentId.TakeWhile(char.IsLetter).ToArray()); // Extrait le préfixe, par exemple "C"
            string numberPart = new string(lastCommandeDocumentId.SkipWhile(char.IsLetter).ToArray()); // Extrait la partie numérique

            // Si le préfixe n'est pas "S", forcer "S" comme préfixe pour les nouvelles DocumentUnitaire
            if (prefix != "DU")
            {
                prefix = "DU";
            }

            // Vérifie que la partie numérique est un nombre valide
            if (!int.TryParse(numberPart, out int lastNumber))
            {
                throw new FormatException($"Le suffixe de l'ID de la CommandeDocument n'est pas un nombre valide : {numberPart}");
            }

            // Incrémenter de 1
            int newNumber = lastNumber + 1;

            // Formater l'ID avec le préfixe et un padding de 3 chiffres (par exemple, "C001" si le préfixe est "C")
            string newCommandeDocumentId = prefix + newNumber.ToString("D5"); // Utilise "D5" pour s'assurer que le nombre a toujours 3 chiffres
            //MessageBox.Show(newCommandeDocumentId);
            return newCommandeDocumentId;
        }

        public async Task GenererDocumentUnitairesPourCommande(CommandesDocuments commande)
        {
            
            for (int i = 0; i < commande.nbExemplaire; i++)
            {
                DocumentUnitaire nouveauDocumentUnitaire = new DocumentUnitaire
                {
                    Id = await GenerateDocumentUnitaireId(), // tu vas créer cette méthode juste en dessous
                    IdDocument = commande.id_document,
                    Etat = 1, // état "neuf"
                    DateAchat = DateTime.Now,
                    IdCommande = commande.id_commande,
                };

                bool success = await CreerDocumentUnitaire(nouveauDocumentUnitaire);
                if (!success)
                {
                    MessageBox.Show($"Erreur lors de la création de l'exemplaire {i + 1} pour le document {commande.id_document}");
                }
            }
        }

        /// <summary>
        /// Gère la mise à jour du statut d'une commande
        /// </summary>
        /// <param name="commandeId">ID de la commande</param>
        /// <param name="nouveauStatut">Le nouveau statut à appliquer</param>
        /// <returns>Retourne true si la mise à jour a réussi</returns>
        public async Task<bool> ModifierEtatDocumentUnitaire(string Id, int nouveauStatut)
        {
             // pause de 2 secondes (200 ms)
            return await access.UpdateEtatDocumentUnitaire(Id, nouveauStatut);
        }

        public async Task<bool> SupprimerDocumentUnitaire(string Id)
        {
             // pause de 2 secondes (200 ms)
            //MessageBox.Show(Id);
            var documentUnitaire = new DocumentUnitaire { Id = Id };
            return await access.SupprimerDocumentUnitaire(documentUnitaire);
        }
        #endregion
        #region Login

        public async Task<User> LoginUtilisateur(string username, string password)
        {
            return await access.LoginUtilisateur(username, password);
        }
        #endregion
    }
}
