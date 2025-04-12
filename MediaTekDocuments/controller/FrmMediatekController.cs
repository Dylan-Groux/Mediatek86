using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System.Linq;
using System;
using System.Windows.Forms;

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
            return access.GetAllCommandes();
        }


        /// <summary>
        /// getter sur la liste des Suivi
        /// </summary>
        /// <returns>Liste d'objets Suivi</returns>
        public List<Suivi> GetAllSuivi()
        {
            var suivis = access.GetAllSuivi();

            if (suivis == null)
            {
                MessageBox.Show("Erreur : La liste des suivis est vide.");
                return new List<Suivi>(); // Retourne une liste vide si null
            }

            return suivis;
        }

        public Commande GetCommandeAvecSuivis(string commandeId)
        {
            // Supposons que tu récupères une commande par son Id
            var commande = GetAllCommandes().FirstOrDefault(c => c.Id == commandeId);

            if (commande != null)
            {
                // Récupérer tous les suivis associés à cette commande (tu peux le faire via une jointure)
                commande.Suivis = GetAllSuivi().Where(s => s.IdCommande == commande.Id).ToList();
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
                             DateSuivi = s.DateSuivi,
                             StatutSuivi = s.Status // Gestion du cas où 's' est null
                         };

            // Retourner le résultat sous forme de liste
            return result.ToList();
        }
    }
}
