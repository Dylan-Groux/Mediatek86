using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments.model
{
    public class CommandeSuivie : Commande
    {
        public int Status { get; set; }

        public CommandeSuivie(string id, DateTime dateCommande, int Montant, int status)
            : base(id, dateCommande, Montant)
        {
            this.Status = status;
        }
    }
}
