using System;
using System.Collections.Generic;


namespace MediaTekDocuments
{
    public class Groupes
    {
        //Variables 
        private string name;
        private string description;
        private List<SuiviCommande> commandesList;

        //Properties
        public string Name { get => name; set => name = value; }
        public string Description { get => name; set => name = value; }
        public List<SuiviCommande> suiviCommandes { get => commandesList; set => commandesList = value; }

        //Constructor
        public Groupes() 
        {
            this.Name = "Undefined";
            this.Description = "Undefined";
            this.commandesList = new List<SuiviCommande>();

        }

        public Groupes(string groupName, string groupDescription)
        {
            this.Name = groupName;
            this.Description = groupDescription;
            this.commandesList = new List<SuiviCommande>();

        }

        public Groupes(string groupName, string groupDescription, List<SuiviCommande> commandesList)
        {
            this.Name = groupName;
            this.Description = groupDescription;
            this.commandesList =commandesList;

        }

        public override string ToString()
        {
            return this.Name;
        }

    }
}
