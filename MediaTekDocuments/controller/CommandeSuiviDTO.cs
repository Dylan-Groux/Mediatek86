using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }

}
