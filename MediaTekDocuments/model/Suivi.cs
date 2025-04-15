using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MediaTekDocuments.model
{
    public class Suivi
    {
        [JsonProperty("id")]         // Correspond à "id" dans le JSON
        public string id_suivi { get; set; }

        [JsonProperty("status")]     // Correspond à "status" dans le JSON
        public int Status { get; set; }

        [JsonProperty("id_commande")] // Correspond à "id_commande" dans le JSON
        public string IdCommande { get; set; }

        [JsonProperty("date_suivi")] // Correspond à "date_suivi" dans le JSON
        public DateTime DateSuivi { get; set; }

        public Suivi(string id_suivi, int status, string id_commande, DateTime date_suivi)
        {
            this.id_suivi = id_suivi;
            this.Status = status;
            this.IdCommande = id_commande;
            this.DateSuivi = date_suivi;
        }

        public Suivi() { }  
    }
}