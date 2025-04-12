using System;
using System.Collections.Generic;
using System.Linq;
using MediaTekDocuments.model;

namespace MediaTekDocuments.Services
{
    public class ExemplaireService
    {
        // Méthode pour trier les exemplaires par date d'achat (ordre inverse)
        public List<Exemplaire> TrierParDateAchat(List<Exemplaire> exemplaires)
        {
            return exemplaires.OrderByDescending(e => e.DateAchat).ToList();
        }

        // Méthode pour changer l'état d'un exemplaire
        public void ChangerEtat(Exemplaire exemplaire, string nouvelEtat)
        {
            exemplaire.IdEtat = nouvelEtat;
        }
    }
}
