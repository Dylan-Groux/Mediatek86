using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using MediaTekDocuments.controller;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments
{
    public partial class AddCommandeWindows : Form
    {
        private string idCommande;
        private string idSuivi;
        private string idCommandeDocument;
        public Commande CommandeCreee { get; set; }
        public Suivi SuiviCree { get; set; }
        public CommandesDocuments LiaisonCreee { get; set; }

        public AddCommandeWindows(string idCommande, string idSuivi, string idCommandeDocument)
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            this.idCommande = idCommande;
            this.idSuivi = idSuivi;
            this.idCommandeDocument = idCommandeDocument;
        }

        private readonly FrmMediatekController controller;


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void CB_GP_COMMANDE_SelectedIndexChanged(object sender, EventArgs e)
        {
        
        }
        private void BT_ADD_ONE_COMMANDE_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TB_MONTANT_COMMANDE.Text) || string.IsNullOrEmpty(TB_ID_LIVRE.Text))
            {
                MessageBox.Show("Tous les champs doivent être remplis.");
                return;
            }

            if (!int.TryParse(TB_MONTANT_COMMANDE.Text, out int montant) ||
                !int.TryParse(NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE.Text, out int nbExemplaire))
            {
                MessageBox.Show("Montant ou nombre d'exemplaires invalide.");
                return;
            }

            // Utilise l'ID passé depuis FrmMediatek
            DateTime dateCommande = DateTime.Now;

            CommandeCreee = new Commande(idCommande, dateCommande, montant);
            SuiviCree = new Suivi { id_suivi = idSuivi, IdCommande = idCommande, Status = 1, DateSuivi = DateTime.Now };
            LiaisonCreee = new CommandesDocuments
            {
                id_commandedocument = idCommandeDocument,
                id_document = TB_ID_LIVRE.Text,
                nbExemplaire = nbExemplaire,
                idLivreDvd = null,
                id_commande = idCommande
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

    }
}

