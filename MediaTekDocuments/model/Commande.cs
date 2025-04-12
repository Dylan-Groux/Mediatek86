using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class Commande
    {
        public string Id { get; set; } // Id de la commande (correspond à la clé primaire de la table)
        public DateTime DateCommande { get; set; } // Date et heure de la commande
        public int Montant { get; set; } // Montant de la commande

        // Liste des suivis associés à cette commande (relation avec la table "suivi")
        public List<Suivi> Suivis { get; set; } = new List<Suivi>();

        // Constructeur pour initialiser la commande
        public Commande(string id, DateTime dateCommande, int montant)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }
    }
}
