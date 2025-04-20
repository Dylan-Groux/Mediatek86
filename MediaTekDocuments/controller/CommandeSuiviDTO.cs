using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaTekDocuments.model;

namespace MediaTekDocuments.controller
{
    public class CommandeSuiviDTO
    {
        public string CommandeId { get; set; }
        public DateTime DateCommande { get; set; }
        public int Montant { get; set; }

        public string SuiviId { get; set; }
        public int StatutSuivi { get; set; }
        public DateTime? DateSuivi { get; set; }
        public CommandesDocuments LiaisonCommandeDocument { get; set; }

        public string LibelleStatutSuivi
        {
            get
            {
                switch (StatutSuivi)
                {
                    case 1: return "En cours";
                    case 2: return "Livré";
                    case 3: return "Disponible en points relais";
                    case 4: return "Annulé";
                    default: return "Inconnu";
                }
            }
        }
    }
}
