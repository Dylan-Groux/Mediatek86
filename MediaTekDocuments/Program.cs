using MediaTekDocuments.view;
using System;
using System.Windows.Forms;

namespace MediaTekDocuments
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Gestion des exceptions non gérées
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.Show($"Une erreur non gérée s'est produite : {e.ExceptionObject.ToString()}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Afficher la fenêtre de login
                LoginForm loginForm = new LoginForm();
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Si login réussi → ouvrir le form principal
                    Application.Run(new FrmMediatek());
                }
                else
                {
                    Console.WriteLine("Fin du traitement. Appuyez sur une touche pour quitter.");
                    Console.Read();
                    // Sinon quitter proprement
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur dans le programme : {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}