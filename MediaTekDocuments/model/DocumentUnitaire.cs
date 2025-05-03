using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MediaTekDocuments.model
{
    public class DocumentUnitaire
    {
        [JsonProperty("id")] 
        public string Id { get; set; }

        [JsonProperty("id_document")] 
        public string IdDocument { get; set; }

        [JsonProperty("etat")] 
        public int Etat { get; set; }

        [JsonProperty("dateAchat")]
        public DateTime DateAchat { get; set; }

        [JsonProperty("id_commande")]
        public string IdCommande { get; set; }

        [JsonIgnore]
        public string LibelleEtatDocument
        {
            get
            {
                switch (Etat)
                {
                    case 1: return "Neuf";
                    case 2: return "Très bon";
                    case 3: return "Moyen";
                    case 4: return "Endommagé";
                    case 5: return "Illisible";
                    default: return "Inconnu";
                }
            }
        }

        public DocumentUnitaire(string Id, string IdDocument, int Etat, DateTime DateAchat, string iddeCommande)
        {
            this.Id = Id;
            this.IdDocument = IdDocument;
            this.Etat = Etat;
            this.DateAchat = DateAchat;
            this.IdCommande = iddeCommande;
        }
        public DocumentUnitaire() { }
    }
}
