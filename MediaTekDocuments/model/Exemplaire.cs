using System;

namespace MediaTekDocuments.model
{
    public class Exemplaire
    {
        public int Numero { get; set; }
        public string Photo { get; set; }
        public DateTime DateAchat { get; set; }
        public string IdEtat { get; set; }
        public string IdLivre { get; set; }  // Pour lier l'exemplaire à un livre

        public Exemplaire(int numero, DateTime dateAchat, string photo, string idEtat, string idLivre)
        {
            this.Numero = numero;
            this.DateAchat = dateAchat;
            this.Photo = photo;
            this.IdEtat = idEtat;
            this.IdLivre = idLivre;
        }
    }
}