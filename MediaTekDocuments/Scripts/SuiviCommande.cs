using System;
using System.Drawing;
using System.Drawing.Text;

namespace MediaTekDocuments
{
    public class SuiviCommande
    {
        //Variable
        private int id;
        private int numCommande;
        private string livreTitle;
        private string livreDescription;
        private Image photo;

        //Properties
        public int Id { get => id ; private set => id = value; }
        public int NumCommande { get => numCommande; private set => numCommande = value; }
        public string LivreTitle { get => livreTitle; private set => livreTitle = value; }
        public string LivreDescription { get => livreDescription; private set => livreDescription = value; }
        public Image Photo { get => photo; private set => photo = value; }

        //Constructors
        public SuiviCommande()
        {

        }

        public SuiviCommande(int id, int numCommande, string livreTitle, string livreDescription, Image photo)
        {
            this.Id = id;
            this.NumCommande = numCommande;
            this.LivreDescription = livreDescription;
            this.Photo = photo;
            this.LivreTitle = livreTitle;

        }


        public override string ToString()
        {
            return base.ToString();
        }



    }
}
