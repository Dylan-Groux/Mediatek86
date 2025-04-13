using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MediaTekDocuments.model
{
    public class CommandesDocuments
    {
        [JsonProperty("id")] // Correspond à "id" dans le JSON
        public string id_commandedocument { get; set; }

        [JsonProperty("id_document")] // Correspond à "status" dans le JSON
        public string id_document { get; set; }

        [JsonProperty("nbExemplaire")] // Correspond à "id_commande" dans le JSON
        public int nbExemplaire { get; set; }

        [JsonProperty("idLivreDvd")] // Correspond à "date_suivi" dans le JSON
        public string idLivreDvd { get; set; }

        [JsonProperty("id_commande")] // Correspond à "date_suivi" dans le JSON
        public string id_commande { get; set; }

        public CommandesDocuments(string id_commandedocument, string id_document, int nbExemplaire, string idLivreDvd, string id_commande)
        {
            this.id_commandedocument = id_commandedocument;
            this.id_document = id_document;
            this.nbExemplaire = nbExemplaire;
            this.idLivreDvd = idLivreDvd;
            this.id_commande = id_commande;
        }
    }
}
