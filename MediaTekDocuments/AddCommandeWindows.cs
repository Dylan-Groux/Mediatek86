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
using MediaTekDocuments.controller;
using MediaTekDocuments.dal;
using MediaTekDocuments.model;

namespace MediaTekDocuments
{
    public partial class AddCommandeWindows : Form
    {
        public AddCommandeWindows()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
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
            // Vérification des champs obligatoires avant de continuer
            if (string.IsNullOrEmpty(TB_NUM_COMMANDE.Text) || string.IsNullOrEmpty(TB_MONTANT_COMMANDE.Text) || string.IsNullOrEmpty(TB_ID_LIVRE.Text))
            {
                MessageBox.Show("Tous les champs doivent être remplis.");
                return;
            }

            if (controller == null)
            {
                MessageBox.Show("Le contrôleur est non initialisé.");
                return;
            }
            // Information pour la table Commande
            string idCommande = TB_NUM_COMMANDE.Text;
            DateTime dateCommande = DateTime.Now;
            int montant;

            // Assure-toi que TB_MONTANT_COMMANDE.Text contient bien un nombre
            if (!int.TryParse(TB_MONTANT_COMMANDE.Text, out montant))
            {
                MessageBox.Show("Le montant de la commande est invalide.");
                return;
            }

            // Information pour la table CommandeDocument
            string idDocument = TB_ID_LIVRE.Text;
            int nbExemplaire;

            // Assure-toi que NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE contient bien un nombre
            if (!int.TryParse(NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE.Text, out nbExemplaire))
            {
                MessageBox.Show("Le nombre d'exemplaires est invalide.");
                return;
            }

            // Création de la commande pour la table "Commande"
            Commande nouvelleCommande = new Commande(idCommande, dateCommande, montant);
            bool commandeCree = controller.CreerCommande(nouvelleCommande);

            if (string.IsNullOrEmpty(idCommande))
            {
                MessageBox.Show("L'ID de la commande est invalide.");
                return;
            }

            if (montant <= 0)
            {
                MessageBox.Show("Le montant de la commande doit être supérieur à zéro.");
                return;
            }

            if (!commandeCree)
            {
                MessageBox.Show("Erreur lors de la création de la commande.");
                return;
            }

            // Création de la commande pour la table "Suivi"
            Suivi nouveauSuivi = new Suivi
            {
                IdCommande = idCommande,
                Status = 0,
                DateSuivi = DateTime.Now
            };
            bool commandeSuiviCree = controller.CreerSuivi(nouveauSuivi);

            if (!commandeSuiviCree)
            {
                MessageBox.Show("Erreur lors de la création du suivi de la commande");
                return;
            }

            // Création de la commande pour la table "CommandesDocuments"
            CommandesDocuments liaison = new CommandesDocuments
            {
                id_document = idDocument,
                nbExemplaire = nbExemplaire,
                idLivreDvd = "00000",
                id_commande = idCommande
            };
            bool liaisonCree = controller.CreerCommandeDocument(liaison);

            if (commandeSuiviCree && liaisonCree)
            {
                MessageBox.Show("Commande, suivi et liaison créés avec succès !");
            }
            else
            {
                MessageBox.Show("Commande créée mais erreur sur le suivi ou la liaison.");
            }
        }
    }
}

