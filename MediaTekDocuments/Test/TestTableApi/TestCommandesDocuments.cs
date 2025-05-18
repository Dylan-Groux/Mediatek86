using System;
using System.Collections.Generic;
using MediaTekDocuments.model;
using MediaTekDocuments.dal;
using System.Windows.Forms;
using System.Threading.Tasks;

public class TestCommandesDocuments
{
    public async Task TesterRecuperationCommandes()
    {
        Access access = new Access();
        List<CommandesDocuments> toutesLesCommandes = await access.GetAllCommandesDocuments();

        if (toutesLesCommandes != null && toutesLesCommandes.Count > 0)
        {
            foreach (var commande in toutesLesCommandes)
            {
                MessageBox.Show($"ID CommandeDoc: {commande.id_commandedocument}, ID Doc: {commande.id_document}, NbExemplaire: {commande.nbExemplaire}, ID Livre: {commande.idLivreDvd}, ID Commande: {commande.id_commande}");
            }
        }
        else
        {
            MessageBox.Show("Aucune commande trouvée.");
        }
    }
}

