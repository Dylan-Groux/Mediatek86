using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaTekDocuments
{
    public static class Global
    {

        //Variables
        public static List<Groupes> suiviGroupes = new List<Groupes>()
        {
            new Groupes("Livres", "Livre"),
            new Groupes("DvD", "DvD"),

        };

    }
}
