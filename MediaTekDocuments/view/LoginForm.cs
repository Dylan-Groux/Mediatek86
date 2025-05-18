using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MediaTekDocuments.controller;
using Newtonsoft.Json;

namespace MediaTekDocuments.view
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
        }

        private readonly FrmMediatekController controller;

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs.");
                return;
            }

            var user = await controller.LoginUtilisateur(username: txtUsername.Text, password: txtPassword.Text);

            if (user != null)
            {
                // Assurer que le rôle de l'utilisateur existe et qu'il a un username valide
                if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Role))
                {
                    MessageBox.Show("Utilisateur trouvé mais des informations manquent.");
                    return;
                }

                // Sinon, l'utilisateur est valide
                string UserRole = user.Role;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Identifiants incorrects.");
                Application.Exit(); // ou juste return si tu veux rester sur le form
            }
        }
    }
}
