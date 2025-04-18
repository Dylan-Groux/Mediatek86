using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System.Linq;
using System;
using System.Windows.Forms;
using System.Threading.Tasks;

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
        public List<Categorie> GetAllGenres()
        {
            return access.GetAllGenres();
        }

        /// <summary>
        /// getter sur la liste des livres
        /// </summary>
        /// <returns>Liste d'objets Livre</returns>
        public List<Livre> GetAllLivres()
        {
            return access.GetAllLivres();
        }

        /// <summary>
        /// getter sur la liste des Dvd
        /// </summary>
        /// <returns>Liste d'objets dvd</returns>
        public List<Dvd> GetAllDvd()
        {
            return access.GetAllDvd();
        }

        /// <summary>
        /// getter sur la liste des revues
        /// </summary>
        /// <returns>Liste d'objets Revue</returns>
        public List<Revue> GetAllRevues()
        {
            return access.GetAllRevues();
        }

        /// <summary>
        /// getter sur les rayons
        /// </summary>
        /// <returns>Liste d'objets Rayon</returns>
        public List<Categorie> GetAllRayons()
        {
            return access.GetAllRayons();
        }

        /// <summary>
        /// getter sur les publics
        /// </summary>
        /// <returns>Liste d'objets Public</returns>
        public List<Categorie> GetAllPublics()
        {
            return access.GetAllPublics();
        }


        /// <summary>
        /// récupère les exemplaires d'une revue
        /// </summary>
        /// <param name="idDocuement">id de la revue concernée</param>
        /// <returns>Liste d'objets Exemplaire</returns>
        public List<Exemplaire> GetExemplairesRevue(string idDocuement)
        {
            return access.GetExemplairesRevue(idDocuement);
        }

        /// <summary>
        /// Crée un exemplaire d'une revue dans la bdd
        /// </summary>
        /// <param name="exemplaire">L'objet Exemplaire concerné</param>
        /// <returns>True si la création a pu se faire</returns>
        public bool CreerExemplaire(Exemplaire exemplaire)
        {
            return access.CreerExemplaire(exemplaire);
        }

        /// <summary>
        /// getter sur la liste des Commandes
        /// </summary>
        /// <returns>Liste d'objets Commandes</returns>
        public List<Commande> GetAllCommandes()
        {
            var commandes = access.GetAllCommandes();

            //string allIds = string.Join(", ", commandes.Select(c => c.Id));
            //MessageBox.Show("GetAllCommandes(): " + allIds);

            return commandes;
        }


        /// <summary>
        /// getter sur la liste des Suivi
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivi()
        {
            var suivis = access.GetAllSuivi();

           // if (suivis == null)
           //{
           //     MessageBox.Show("Erreur : La liste des suivis est vide.");
           //     return new List<Suivi>(); // Retourne une liste vide si null
           // }

            return suivis;
        }

        public Commande GetCommandeAvecSuivis(string commandeId)
        {
            // Supposons que tu récupères une commande par son Id
            var commande = GetAllCommandes().FirstOrDefault(c => c.Id == commandeId);

            if (commande != null)
            {
                // Récupérer tous les suivis associés à cette commande (tu peux le faire via une jointure)
                var suivis = GetAllSuivi().Where(s => s.IdCommande == commande.Id).ToList();
            }

            return commande;
        }

        public List<CommandeSuiviDTO> GetCommandesSuivisDTO()
        {
            var commandes = GetAllCommandes();
            var suivis = GetAllSuivi();

            // Log pour vérifier la taille des listes
            // MessageBox.Show($"Commandes : {commandes.Count}, Suivis : {suivis.Count}");

            // La jointure entre Commande et Suivi
            var result = from c in commandes
                         join s in suivis on c.Id equals s.IdCommande into csGroup
                         from s in csGroup.DefaultIfEmpty()  // Utilisation de DefaultIfEmpty pour gérer les suivis nuls
                         select new CommandeSuiviDTO
                         {
                             CommandeId = c.Id,
                             DateCommande = c.DateCommande,
                             Montant = c.Montant,
                             // Vérifier si 's' est null avant d'accéder à ses propriétés
                             SuiviId = s != null ? s.id_suivi : null,
                             DateSuivi = s?.DateSuivi,
                             StatutSuivi = s?.Status ?? -1 // Gestion du cas où 's' est null
                         };

            // Retourner le résultat sous forme de liste
            return result.ToList();
        }

        public List<CommandesDocuments> GetAllCommnadesDocuments()
        {
            return access.GetAllCommnadesDocuments();
        }


        /// <summary>
        /// Crée une nouvelle commande dans la BDD
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public bool CreerCommande(Commande commande)
        {
            return access.CreerCommande(commande);
        }




        /// <summary>
        /// Crée une nouvelle donnée de suivi dans la BDD
        /// </summary>
        /// <param name="suivi"></param>
        /// <returns></returns>
        public bool CreerSuivi(Suivi suivi)
        {
            return access.CreerSuivi(suivi);
        }




        /// <summary>
        /// Crée une nouvelle donnée de CommandesDocuments dans la BDD
        /// </summary>
        /// <param name="commandedocument"></param>
        /// <returns></returns>
        public bool CreerCommandeDocument(CommandesDocuments commandedocument)
        {
            return access.CreerCommandeDocument(commandedocument);
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
        public string GenerateCommandeId()
        {
            var allCommandes = access.GetAllCommandes();
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
        public string GenerateSuiviId()
        {
            var allSuivis = access.GetAllSuivi();
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
        public string GenerateCommandeDocumentId()
        {
            var allCommandeDocuments = access.GetAllCommandesDocuments();
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
        public bool ModifierStatutCommande(string idSuivi, string commandeId, int nouveauStatut)
        {
            return access.UpdateStatutSuivi(idSuivi, commandeId, nouveauStatut);
        }
    }
}
