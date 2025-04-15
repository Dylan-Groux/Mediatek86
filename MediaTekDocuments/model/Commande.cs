using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MediaTekDocuments.model
{
    public class Commande
    {
        [JsonProperty("id")]         // Correspond à "id" dans le JSON
        public string Id { get; set; }

        [JsonProperty("dateCommande")]  // Correspond à "dateCommande" dans le JSON
        public DateTime DateCommande { get; set; }

        [JsonProperty("montant")]   // Correspond à "montant" dans le JSON
        public int Montant { get; set; }

        // Constructeur pour initialiser la commande
        public Commande(string id, DateTime dateCommande, int montant)
        {
            this.Id = id;
            this.DateCommande = dateCommande;
            this.Montant = montant;
        }

        public Commande() { }
    }
}
