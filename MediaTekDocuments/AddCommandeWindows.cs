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

namespace MediaTekDocuments
{
    public partial class AddCommandeWindows : Form
    {
        public AddCommandeWindows()
        {
            InitializeComponent();

            loadGroupes();
        }

        private void loadGroupes()
        {
            this.CB_GP_COMMANDE.Items.Clear();
            this.CB_GP_COMMANDE.Items.AddRange(Global.suiviGroupes.ToArray());

            //Permet la préselection du premier élements dans la liste déroulante.
            if (this.CB_GP_COMMANDE.Items.Count > 0)
            {
                this.CB_GP_COMMANDE.SelectedIndex = 0;
            }
        }

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
            //Récupérer les champs saisit.
            int numCommande = 0;
            string livreTitle = this.TB_TITLE_LIVRE.Text;
            string livreDescription = this.TB_DESC_LIVRE.Text;
            string textId = this.TB_ID_LIVRE.Text;
            int id;
            bool idConverstion = int.TryParse(textId, out id);



            if (idConverstion)
            {
                Console.WriteLine("L'ID du livre est : " + id);
            } else {
                MessageBox.Show("Veuillez entrer un numéro valide.");
            }

            //Vérifier que les champs sont saisit

            //Création d'une nouvelle commande

            // Ajout d'une commande dans le groupe 
        }
    }
}
