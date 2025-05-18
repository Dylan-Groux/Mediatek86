using System;
using System.Windows.Forms;
using MediaTekDocuments.model;
using MediaTekDocuments.controller;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using MediaTekDocuments.dal;
using System.ComponentModel.Design;
using System.Xml.Linq;
using MediaTekDocuments;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Security.Cryptography;

namespace MediaTekDocuments.view

{
    /// <summary>
    /// Classe d'affichage
    /// </summary>
    public partial class FrmMediatek : Form
    {
        #region Commun
        private readonly FrmMediatekController controller;
        private readonly BindingSource bdgGenres = new BindingSource();
        private readonly BindingSource bdgPublics = new BindingSource();
        private readonly BindingSource bdgRayons = new BindingSource();
        private readonly BindingSource bdgSuivis = new BindingSource();

        /// <summary>
        /// Constructeur : création du contrôleur lié à ce formulaire
        /// </summary>
        internal FrmMediatek()
        {
            InitializeComponent();
            this.controller = new FrmMediatekController();
            MettreAJourInfosUtilisateur();
        }

        /// <summary>
        /// Rempli un des 3 combo (genre, public, rayon)
        /// </summary>
        /// <param name="lesCategories">liste des objets de type Genre ou Public ou Rayon</param>
        /// <param name="bdg">bindingsource contenant les informations</param>
        /// <param name="cbx">combobox à remplir</param>
        public void RemplirComboCategorie(List<Categorie> lesCategories, BindingSource bdg, ComboBox cbx)
        {
            bdg.DataSource = lesCategories;
            cbx.DataSource = bdg;
            if (cbx.Items.Count > 0)
            {
                cbx.SelectedIndex = -1;
            }
        }
        private void DATAGRID_COMMANDES_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // Ignore proprement l’erreur sans popup
            e.ThrowException = false;
            e.Cancel = true;

            // (Optionnel) Tu peux logger si tu veux déboguer plus tard
            // Console.WriteLine($"Erreur silencieuse DataGridView : {e.Exception.Message}");
        }
        #endregion

        #region Onglet Livres
        private readonly BindingSource bdgLivresListe = new BindingSource();
        private List<Livre> lesLivres = new List<Livre>();


        /// <summary>
        /// Ouverture de l'onglet Livres : 
        /// appel des méthodes pour remplir le datagrid des livres et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void TabLivres_Enter(object sender, EventArgs e)
        {
            lesLivres = await controller.GetAllLivres();
            RemplirComboCategorie(await controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(await controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(await controller.GetAllRayons(), bdgRayons, cbxLivresRayons);
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="livres">liste de livres</param>
        private void RemplirLivresListe(List<Livre> livres)
        {
            bdgLivresListe.DataSource = livres;
            dgvLivresListe.DataSource = bdgLivresListe;
            dgvLivresListe.Columns["isbn"].Visible = false;
            dgvLivresListe.Columns["idRayon"].Visible = false;
            dgvLivresListe.Columns["idGenre"].Visible = false;
            dgvLivresListe.Columns["idPublic"].Visible = false;
            dgvLivresListe.Columns["image"].Visible = false;
            dgvLivresListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvLivresListe.Columns["id"].DisplayIndex = 0;
            dgvLivresListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage du livre dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbLivresNumRecherche.Text.Equals(""))
            {
                txbLivresTitreRecherche.Text = "";
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                Livre livre = lesLivres.Find(x => x.Id.Equals(txbLivresNumRecherche.Text));
                if (livre != null)
                {
                    List<Livre> livres = new List<Livre>() { livre };
                    RemplirLivresListe(livres);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirLivresListeComplete();
                }
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TxbLivresTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbLivresTitreRecherche.Text.ToLower().Equals(""))
            {
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
                txbLivresNumRecherche.Text = "";
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(txbLivresTitreRecherche.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxLivresGenres.SelectedIndex < 0 && cbxLivresPublics.SelectedIndex < 0 && cbxLivresRayons.SelectedIndex < 0
                    && txbLivresNumRecherche.Text.Equals(""))
                {
                    RemplirLivresListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfos(Livre livre)
        {
            txbLivresAuteur.Text = livre.Auteur;
            txbLivresCollection.Text = livre.Collection;
            txbLivresImage.Text = livre.Image;
            txbLivresIsbn.Text = livre.Isbn;
            txbLivresNumero.Text = livre.Id;
            txbLivresGenre.Text = livre.Genre;
            txbLivresPublic.Text = livre.Public;
            txbLivresRayon.Text = livre.Rayon;
            txbLivresTitre.Text = livre.Titre;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations du livre
        /// </summary>
        private void VideLivresInfos()
        {
            txbLivresAuteur.Text = "";
            txbLivresCollection.Text = "";
            txbLivresImage.Text = "";
            txbLivresIsbn.Text = "";
            txbLivresNumero.Text = "";
            txbLivresGenre.Text = "";
            txbLivresPublic.Text = "";
            txbLivresRayon.Text = "";
            txbLivresTitre.Text = "";
            pcbLivresImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresGenres.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Genre genre = (Genre)cbxLivresGenres.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresPublics.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Public lePublic = (Public)cbxLivresPublics.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirLivresListe(livres);
                cbxLivresRayons.SelectedIndex = -1;
                cbxLivresGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbxLivresRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxLivresRayons.SelectedIndex >= 0)
            {
                txbLivresTitreRecherche.Text = "";
                txbLivresNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxLivresRayons.SelectedItem;
                List<Livre> livres = lesLivres.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirLivresListe(livres);
                cbxLivresGenres.SelectedIndex = -1;
                cbxLivresPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations du livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLivresListe.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfos(livre);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des livres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnLivresAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirLivresListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des livres
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirLivresListeComplete()
        {
            RemplirLivresListe(lesLivres);
            VideLivresZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideLivresZones()
        {
            cbxLivresGenres.SelectedIndex = -1;
            cbxLivresRayons.SelectedIndex = -1;
            cbxLivresPublics.SelectedIndex = -1;
            txbLivresNumRecherche.Text = "";

        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DgvLivresListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }
        #endregion

        #region Onglet Revues
        private readonly BindingSource bdgRevuesListe = new BindingSource();
        private List<Revue> lesRevues = new List<Revue>();

        /// <summary>
        /// Ouverture de l'onglet Revues : 
        /// appel des méthodes pour remplir le datagrid des revues et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void tabRevues_Enter(object sender, EventArgs e)
        {
            lesRevues = await controller.GetAllRevues();
            RemplirComboCategorie(await controller.GetAllGenres(), bdgGenres, cbxRevuesGenres);
            RemplirComboCategorie(await controller.GetAllPublics(), bdgPublics, cbxRevuesPublics);
            RemplirComboCategorie(await controller.GetAllRayons(), bdgRayons, cbxRevuesRayons);
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Remplit le dategrid avec la liste reçue en paramètre
        /// </summary>
        /// <param name="revues"></param>
        private void RemplirRevuesListe(List<Revue> revues)
        {
            bdgRevuesListe.DataSource = revues;
            dgvRevuesListe.DataSource = bdgRevuesListe;
            dgvRevuesListe.Columns["idRayon"].Visible = false;
            dgvRevuesListe.Columns["idGenre"].Visible = false;
            dgvRevuesListe.Columns["idPublic"].Visible = false;
            dgvRevuesListe.Columns["image"].Visible = false;
            dgvRevuesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvRevuesListe.Columns["id"].DisplayIndex = 0;
            dgvRevuesListe.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Recherche et affichage de la revue dont on a saisi le numéro.
        /// Si non trouvé, affichage d'un MessageBox.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesNumRecherche_Click(object sender, EventArgs e)
        {
            if (!txbRevuesNumRecherche.Text.Equals(""))
            {
                txbRevuesTitreRecherche.Text = "";
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbRevuesNumRecherche.Text));
                if (revue != null)
                {
                    List<Revue> revues = new List<Revue>() { revue };
                    RemplirRevuesListe(revues);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                    RemplirRevuesListeComplete();
                }
            }
            else
            {
                RemplirRevuesListeComplete();
            }
        }

        /// <summary>
        /// Recherche et affichage des revues dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbRevuesTitreRecherche_TextChanged(object sender, EventArgs e)
        {
            if (!txbRevuesTitreRecherche.Text.Equals(""))
            {
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
                txbRevuesNumRecherche.Text = "";
                List<Revue> lesRevuesParTitre;
                lesRevuesParTitre = lesRevues.FindAll(x => x.Titre.ToLower().Contains(txbRevuesTitreRecherche.Text.ToLower()));
                RemplirRevuesListe(lesRevuesParTitre);
            }
            else
            {
                // si la zone de saisie est vide et aucun élément combo sélectionné, réaffichage de la liste complète
                if (cbxRevuesGenres.SelectedIndex < 0 && cbxRevuesPublics.SelectedIndex < 0 && cbxRevuesRayons.SelectedIndex < 0
                    && txbRevuesNumRecherche.Text.Equals(""))
                {
                    RemplirRevuesListeComplete();
                }
            }
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionné
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheRevuesInfos(Revue revue)
        {
            txbRevuesPeriodicite.Text = revue.Periodicite;
            txbRevuesImage.Text = revue.Image;
            txbRevuesDateMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbRevuesNumero.Text = revue.Id;
            txbRevuesGenre.Text = revue.Genre;
            txbRevuesPublic.Text = revue.Public;
            txbRevuesRayon.Text = revue.Rayon;
            txbRevuesTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbRevuesImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbRevuesImage.Image = null;
            }
        }

        /// <summary>
        /// Vide les zones d'affichage des informations de la reuve
        /// </summary>
        private void VideRevuesInfos()
        {
            txbRevuesPeriodicite.Text = "";
            txbRevuesImage.Text = "";
            txbRevuesDateMiseADispo.Text = "";
            txbRevuesNumero.Text = "";
            txbRevuesGenre.Text = "";
            txbRevuesPublic.Text = "";
            txbRevuesRayon.Text = "";
            txbRevuesTitre.Text = "";
            pcbRevuesImage.Image = null;
        }

        /// <summary>
        /// Filtre sur le genre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesGenres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesGenres.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Genre genre = (Genre)cbxRevuesGenres.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Genre.Equals(genre.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur la catégorie de public
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesPublics_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesPublics.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Public lePublic = (Public)cbxRevuesPublics.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Public.Equals(lePublic.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesRayons.SelectedIndex = -1;
                cbxRevuesGenres.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Filtre sur le rayon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxRevuesRayons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxRevuesRayons.SelectedIndex >= 0)
            {
                txbRevuesTitreRecherche.Text = "";
                txbRevuesNumRecherche.Text = "";
                Rayon rayon = (Rayon)cbxRevuesRayons.SelectedItem;
                List<Revue> revues = lesRevues.FindAll(x => x.Rayon.Equals(rayon.Libelle));
                RemplirRevuesListe(revues);
                cbxRevuesGenres.SelectedIndex = -1;
                cbxRevuesPublics.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sur la sélection d'une ligne ou cellule dans le grid
        /// affichage des informations de la revue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRevuesListe.CurrentCell != null)
            {
                try
                {
                    Revue revue = (Revue)bdgRevuesListe.List[bdgRevuesListe.Position];
                    AfficheRevuesInfos(revue);
                }
                catch
                {
                    VideRevuesZones();
                }
            }
            else
            {
                VideRevuesInfos();
            }
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulPublics_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulRayons_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Sur le clic du bouton d'annulation, affichage de la liste complète des revues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRevuesAnnulGenres_Click(object sender, EventArgs e)
        {
            RemplirRevuesListeComplete();
        }

        /// <summary>
        /// Affichage de la liste complète des revues
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirRevuesListeComplete()
        {
            RemplirRevuesListe(lesRevues);
            VideRevuesZones();
        }

        /// <summary>
        /// vide les zones de recherche et de filtre
        /// </summary>
        private void VideRevuesZones()
        {
            cbxRevuesGenres.SelectedIndex = -1;
            cbxRevuesRayons.SelectedIndex = -1;
            cbxRevuesPublics.SelectedIndex = -1;
            txbRevuesNumRecherche.Text = "";
            txbRevuesTitreRecherche.Text = "";
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRevuesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideRevuesZones();
            string titreColonne = dgvRevuesListe.Columns[e.ColumnIndex].HeaderText;
            List<Revue> sortedList = new List<Revue>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesRevues.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesRevues.OrderBy(o => o.Titre).ToList();
                    break;
                case "Periodicite":
                    sortedList = lesRevues.OrderBy(o => o.Periodicite).ToList();
                    break;
                case "DelaiMiseADispo":
                    sortedList = lesRevues.OrderBy(o => o.DelaiMiseADispo).ToList();
                    break;
                case "Genre":
                    sortedList = lesRevues.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesRevues.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesRevues.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirRevuesListe(sortedList);
        }
        #endregion

        #region Onglet Paarutions
        private readonly BindingSource bdgExemplairesListe = new BindingSource();
        private List<Exemplaire> lesExemplaires = new List<Exemplaire>();
        const string ETATNEUF = "00001";

        /// <summary>
        /// Ouverture de l'onglet : récupère le revues et vide tous les champs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void tabReceptionRevue_Enter(object sender, EventArgs e)
        {
            lesRevues = await controller.GetAllRevues();
            txbReceptionRevueNumero.Text = "";
        }

        /// <summary>
        /// Remplit le dategrid des exemplaires avec la liste reçue en paramètre
        /// </summary>
        /// <param name="exemplaires">liste d'exemplaires</param>
        private void RemplirReceptionExemplairesListe(List<Exemplaire> exemplaires)
        {
            if (exemplaires != null)
            {
                bdgExemplairesListe.DataSource = exemplaires;
                dgvReceptionExemplairesListe.DataSource = bdgExemplairesListe;
                dgvReceptionExemplairesListe.Columns["idEtat"].Visible = false;
                dgvReceptionExemplairesListe.Columns["id"].Visible = false;
                dgvReceptionExemplairesListe.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvReceptionExemplairesListe.Columns["numero"].DisplayIndex = 0;
                dgvReceptionExemplairesListe.Columns["dateAchat"].DisplayIndex = 1;
            }
            else
            {
                bdgExemplairesListe.DataSource = null;
            }
        }

        /// <summary>
        /// Recherche d'un numéro de revue et affiche ses informations
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionRechercher_Click(object sender, EventArgs e)
        {
            if (!txbReceptionRevueNumero.Text.Equals(""))
            {
                Revue revue = lesRevues.Find(x => x.Id.Equals(txbReceptionRevueNumero.Text));
                if (revue != null)
                {
                    AfficheReceptionRevueInfos(revue);
                }
                else
                {
                    MessageBox.Show("numéro introuvable");
                }
            }
        }

        /// <summary>
        /// Si le numéro de revue est modifié, la zone de l'exemplaire est vidée et inactive
        /// les informations de la revue son aussi effacées
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txbReceptionRevueNumero_TextChanged(object sender, EventArgs e)
        {
            txbReceptionRevuePeriodicite.Text = "";
            txbReceptionRevueImage.Text = "";
            txbReceptionRevueDelaiMiseADispo.Text = "";
            txbReceptionRevueGenre.Text = "";
            txbReceptionRevuePublic.Text = "";
            txbReceptionRevueRayon.Text = "";
            txbReceptionRevueTitre.Text = "";
            pcbReceptionRevueImage.Image = null;
            RemplirReceptionExemplairesListe(null);
            AccesReceptionExemplaireGroupBox(false);
        }

        /// <summary>
        /// Affichage des informations de la revue sélectionnée et les exemplaires
        /// </summary>
        /// <param name="revue">la revue</param>
        private void AfficheReceptionRevueInfos(Revue revue)
        {
            // informations sur la revue
            txbReceptionRevuePeriodicite.Text = revue.Periodicite;
            txbReceptionRevueImage.Text = revue.Image;
            txbReceptionRevueDelaiMiseADispo.Text = revue.DelaiMiseADispo.ToString();
            txbReceptionRevueNumero.Text = revue.Id;
            txbReceptionRevueGenre.Text = revue.Genre;
            txbReceptionRevuePublic.Text = revue.Public;
            txbReceptionRevueRayon.Text = revue.Rayon;
            txbReceptionRevueTitre.Text = revue.Titre;
            string image = revue.Image;
            try
            {
                pcbReceptionRevueImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbReceptionRevueImage.Image = null;
            }
            // affiche la liste des exemplaires de la revue
            AfficheReceptionExemplairesRevue();
        }

        /// <summary>
        /// Récupère et affiche les exemplaires d'une revue
        /// </summary>
        private async Task AfficheReceptionExemplairesRevue()
        {
            string idDocuement = txbReceptionRevueNumero.Text;
            lesExemplaires = await controller.GetExemplairesRevue(idDocuement);
            RemplirReceptionExemplairesListe(lesExemplaires);
            AccesReceptionExemplaireGroupBox(true);
        }

        /// <summary>
        /// Permet ou interdit l'accès à la gestion de la réception d'un exemplaire
        /// et vide les objets graphiques
        /// </summary>
        /// <param name="acces">true ou false</param>
        private void AccesReceptionExemplaireGroupBox(bool acces)
        {
            grpReceptionExemplaire.Enabled = acces;
            txbReceptionExemplaireImage.Text = "";
            txbReceptionExemplaireNumero.Text = "";
            pcbReceptionExemplaireImage.Image = null;
            dtpReceptionExemplaireDate.Value = DateTime.Now;
        }

        /// <summary>
        /// Recherche image sur disque (pour l'exemplaire à insérer)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReceptionExemplaireImage_Click(object sender, EventArgs e)
        {
            string filePath = "";
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // positionnement à la racine du disque où se trouve le dossier actuel
                InitialDirectory = Path.GetPathRoot(Environment.CurrentDirectory),
                Filter = "Files|*.jpg;*.bmp;*.jpeg;*.png;*.gif"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
            }
            txbReceptionExemplaireImage.Text = filePath;
            try
            {
                pcbReceptionExemplaireImage.Image = Image.FromFile(filePath);
            }
            catch
            {
                pcbReceptionExemplaireImage.Image = null;
            }
        }

        /// <summary>
        /// Enregistrement du nouvel exemplaire
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnReceptionExemplaireValider_Click(object sender, EventArgs e)
        {
            if (!txbReceptionExemplaireNumero.Text.Equals(""))
            {
                try
                {
                    int numero = int.Parse(txbReceptionExemplaireNumero.Text);
                    DateTime dateAchat = dtpReceptionExemplaireDate.Value;
                    string photo = txbReceptionExemplaireImage.Text;
                    string idEtat = ETATNEUF;
                    string idDocument = txbReceptionRevueNumero.Text;
                    Exemplaire exemplaire = new Exemplaire(numero, dateAchat, photo, idEtat, idDocument);
                    if (await controller.CreerExemplaire(exemplaire))
                    {
                        AfficheReceptionExemplairesRevue();
                    }
                    else
                    {
                        MessageBox.Show("numéro de publication déjà existant", "Erreur");
                    }
                }
                catch
                {
                    MessageBox.Show("le numéro de parution doit être numérique", "Information");
                    txbReceptionExemplaireNumero.Text = "";
                    txbReceptionExemplaireNumero.Focus();
                }
            }
            else
            {
                MessageBox.Show("numéro de parution obligatoire", "Information");
            }
        }

        /// <summary>
        /// Tri sur une colonne
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvExemplairesListe_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = dgvReceptionExemplairesListe.Columns[e.ColumnIndex].HeaderText;
            List<Exemplaire> sortedList = new List<Exemplaire>();
            switch (titreColonne)
            {
                case "Numero":
                    sortedList = lesExemplaires.OrderBy(o => o.Numero).Reverse().ToList();
                    break;
                case "DateAchat":
                    sortedList = lesExemplaires.OrderBy(o => o.DateAchat).Reverse().ToList();
                    break;
                case "Photo":
                    sortedList = lesExemplaires.OrderBy(o => o.Photo).ToList();
                    break;
            }
            RemplirReceptionExemplairesListe(sortedList);
        }

        /// <summary>
        /// affichage de l'image de l'exemplaire suite à la sélection d'un exemplaire dans la liste
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReceptionExemplairesListe_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvReceptionExemplairesListe.CurrentCell != null)
            {
                Exemplaire exemplaire = (Exemplaire)bdgExemplairesListe.List[bdgExemplairesListe.Position];
                string image = exemplaire.Photo;
                try
                {
                    pcbReceptionExemplaireRevueImage.Image = Image.FromFile(image);
                }
                catch
                {
                    pcbReceptionExemplaireRevueImage.Image = null;
                }
            }
            else
            {
                pcbReceptionExemplaireRevueImage.Image = null;
            }
        }
        #endregion

        #region Onglet Commandes

        //Variables
        private List<Suivi> lesSuivi = new List<Suivi>();
        private List<CommandesDocuments> lesCommandesDocuments = new List<CommandesDocuments>();
        private List<Commande> lesCommandes = new List<Commande>();
        private bool isAscending = true; //Gestion d'état dynamique pour triage

        private List<CommandeSuiviDTO> commandeSuivis = new List<CommandeSuiviDTO>();
        private List<CommandeSuiviDTO> commandeSuivisBase;


        /// <summary>
        /// Ouverture de l'onglet Commandes : 
        /// appel des méthodes pour remplir le datagrid des commandes et des combos (genre, rayon, public)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task TAB_COMMANDE_Enter(object sender, EventArgs e)
        {
            lesCommandes = await controller.GetAllCommandes();
            lesSuivi = await controller.GetAllSuivi();
            lesCommandesDocuments = await controller.GetAllCommnadesDocuments();

            commandeSuivisBase = await controller.GetCommandesSuivisDTO();
            commandeSuivis = new List<CommandeSuiviDTO>(commandeSuivisBase);
            RemplirCommandesListeComplete();

            lesLivres = await controller.GetAllLivres();
            RemplirComboCategorie( await controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(await controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(await controller.GetAllRayons(), bdgRayons, cbxLivresRayons);

        }

        /// <summary>
        /// Affichage de la liste complète des Commandes
        /// et annulation de toutes les recherches et filtres
        /// </summary>
        private void RemplirCommandesListeComplete()
        {
            commandeSuivis = new List<CommandeSuiviDTO>(commandeSuivisBase);
            RemplirCommandeAvecSuivi(commandeSuivis);
            VideLivresZones();
            //Remplissage de la DataGridView des Livres
            RemplirLivresListeCommandes(lesLivres);
        }


        #region "Liste des commandes en cours"

        /// <summary>
        /// Méthode de remplissge et gestion de l'affichage de la Dgv
        /// </summary>
        /// <param name="commandes"></param>
        private void RemplirCommandeAvecSuivi(List<CommandeSuiviDTO> commandes)
        {
            commandeSuivis = commandes;

            // 🔁 Reset propre
            DATAGRID_COMMANDES.DataSource = null;
            DATAGRID_COMMANDES.Columns.Clear();

            // 🔧 Création des colonnes visibles
            var colId = new DataGridViewTextBoxColumn { Name = "CommandeId", HeaderText = "N° de la commande", DataPropertyName = "CommandeId" };
            var colDateCmd = new DataGridViewTextBoxColumn { Name = "DateCommande", HeaderText = "Date de la commande", DataPropertyName = "DateCommande" };
            var colMontant = new DataGridViewTextBoxColumn { Name = "Montant", HeaderText = "Montant (€)", DataPropertyName = "Montant" };
            var colDateSuivi = new DataGridViewTextBoxColumn { Name = "DateSuivi", HeaderText = "Date de changement de suivi", DataPropertyName = "DateSuivi" };
            var colLibelle = new DataGridViewTextBoxColumn { Name = "LibelleStatutSuivi", HeaderText = "Statut de suivi", DataPropertyName = "LibelleStatutSuivi" };

            var statuts = new List<Statut>
            {
                new Statut { Value = 1, Libelle = "En cours" },
                new Statut { Value = 2, Libelle = "Livré" },
                new Statut { Value = 3, Libelle = "Disponible en points relais" },
                new Statut { Value = 4, Libelle = "Annulé" }
            };

            var comboCol = new DataGridViewComboBoxColumn
            {
                Name = "ColonneStatutCombo",
                HeaderText = "Modifier le statut",
                DataPropertyName = "StatutSuivi",
                DataSource = statuts,
                DisplayMember = "Libelle",
                ValueMember = "Value",
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
            };

            var btnCol = new DataGridViewButtonColumn
            {
                Name = "ColonneSuppression",
                HeaderText = "Supprimer une commande",
                Text = "❌ Supprimer",
                UseColumnTextForButtonValue = true
            };

            // ⛓️ Ajout manuel des colonnes
            DATAGRID_COMMANDES.Columns.AddRange(colId, colDateCmd, colMontant, colDateSuivi, colLibelle, comboCol, btnCol);


            if (Session.CurrentUser != null)
            {
                if (Session.CurrentUser.Role == "ADMIN")
                {
                    // Afficher la colonne de suppression
                    DATAGRID_COMMANDES.Columns["ColonneSuppression"].Visible = true;
                }
                else
                {
                    // Masquer la colonne de suppression pour les autres
                    DATAGRID_COMMANDES.Columns["ColonneSuppression"].Visible = false;
                }
            }

            // 🧷 Paramètres globaux
            DATAGRID_COMMANDES.AutoGenerateColumns = false;
            DATAGRID_COMMANDES.SelectionMode = DataGridViewSelectionMode.CellSelect;
            DATAGRID_COMMANDES.MultiSelect = false;
            DATAGRID_COMMANDES.RowHeadersVisible = false;
            DATAGRID_COMMANDES.AllowUserToAddRows = false;
            DATAGRID_COMMANDES.AllowUserToDeleteRows = false;
            DATAGRID_COMMANDES.AllowUserToResizeColumns = false;
            DATAGRID_COMMANDES.AllowUserToResizeRows = false;

            // 🧊 Rendre tout ReadOnly sauf statut combo
            DATAGRID_COMMANDES.ReadOnly = false;
            foreach (DataGridViewColumn col in DATAGRID_COMMANDES.Columns)
                col.ReadOnly = col.Name != "ColonneStatutCombo";

            // 🧯 Tri désactivé sur les colonnes sensibles
            foreach (DataGridViewColumn col in DATAGRID_COMMANDES.Columns)
            {
                if (col.Name == "ColonneStatutCombo" || col.Name == "ColonneSuppression")
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            // 🎯 Définir la source de données maintenant
            DATAGRID_COMMANDES.DataSource = commandeSuivis;
        }


        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DATAGRID_COMMANDES_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = DATAGRID_COMMANDES.Columns[e.ColumnIndex].HeaderText;
            List<CommandeSuiviDTO> sortedList = new List<CommandeSuiviDTO>();
            switch (titreColonne)
            {
                case "Supprimer une commande":
                    {
                        RemplirCommandeAvecSuivi(commandeSuivisBase);
                    }
                    return;

                case "N° de la commande":
                    if (isAscending)
                    {
                        sortedList = commandeSuivis.OrderBy(o => o.CommandeId).ToList();
                    }
                    else
                    {
                        sortedList = commandeSuivis.OrderByDescending(o => o.CommandeId).ToList();
                    }
                    break;

                case "Date de la commande":
                    if (isAscending)
                    {
                        sortedList = commandeSuivis.OrderBy(o => o.DateCommande).ToList();
                    }
                    else
                    {
                        sortedList = commandeSuivis.OrderByDescending(o => o.DateCommande).ToList();
                    }
                    break;

                case "Montant (€)":
                    if (isAscending)
                    {
                        sortedList = commandeSuivis.OrderBy(o => o.Montant).ToList();
                    }
                    else
                    {
                        sortedList = commandeSuivis.OrderByDescending(o => o.Montant).ToList();
                    }
                    break;

                case "SuiviId":
                    if (isAscending)
                    {
                        sortedList = commandeSuivis.OrderBy(o => o.SuiviId).ToList();
                    }
                    else
                    {
                        sortedList = commandeSuivis.OrderByDescending(o => o.SuiviId).ToList();
                    }
                    break;

                case "Statut de suivi":
                case "Modifier le statut":
                    if (isAscending)
                    {
                        sortedList = commandeSuivis.OrderBy(o => o.LibelleStatutSuivi).ToList();
                    }
                    else
                    {
                        sortedList = commandeSuivis.OrderByDescending(o => o.LibelleStatutSuivi).ToList();
                    }
                    break;
                case "Date de changement de suivi":
                    if (isAscending)
                    {
                        sortedList = commandeSuivis.OrderBy(o => o.DateSuivi).ToList();
                    }
                    else
                    {
                        sortedList = commandeSuivis.OrderByDescending(o => o.DateSuivi).ToList();
                    }
                    break;
            }

            isAscending = !isAscending;

            RemplirCommandeAvecSuivi(sortedList);
        }


        /// <summary>
        /// Permet de recharger complètement la liste des commandes
        /// </summary>
        private async Task LoadCommandes()
        {
            List<CommandeSuiviDTO> commandes = await controller.GetCommandesSuivisDTO();
            RemplirCommandeAvecSuivi(commandes);
        }

        /// <summary>
        /// Permet de forcer le rechargement de la Liste CommandesDTO dans sa liste de base
        /// </summary>
        private async Task ReloadCommandesDTOListBase()
        {
            commandeSuivisBase = await controller.GetCommandesSuivisDTO();
        }

        /// <summary>
        /// Permet de recherche par ID de commande dans la TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TXB_SEARCH_NUM_COMMANDE_TextChanged(object sender, EventArgs e)
        {
            ReloadCommandesDTOListBase();
            string saisie = TXB_SEARCH_NUM_COMMANDE.Text.Trim().ToLower();

            if (!string.IsNullOrEmpty(saisie))
            {
                var commandesTrouvees = commandeSuivisBase
                    .FindAll(x => x.CommandeId.ToLower().Contains(saisie));

                RemplirCommandeAvecSuivi(commandesTrouvees);
            }
            else
            {
                RemplirCommandesListeComplete();
            }
        }

        /// <summary>
        /// Supprime le text dans la TextBox 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BT_CLEAR_CHOICE_COMMANDES_ID_Click(object sender, EventArgs e)
        {
            this.TXB_SEARCH_NUM_COMMANDE.Text = "";
            RemplirCommandeAvecSuivi(commandeSuivis);
        }

        /// <summary>
        ///  Supprime le text dans la TextBox quand le champs est cliqué
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TXB_SEARCH_NUM_COMMANDE_Click(object sender, EventArgs e)
        {
            this.TXB_SEARCH_NUM_COMMANDE.Text = "";
            RemplirCommandeAvecSuivi(commandeSuivis);
        }

        /// <summary>
        /// Gère l'affichage et la mise à jour de la ComboBox dans la cellule de statut lors de l'édition d'une ligne dans le DataGrid.
        /// Cette méthode est déclenchée lorsque l'utilisateur entre en mode édition sur la colonne "ColonneStatutCombo" ( = à Modifier le suivi ) du DataGrid.
        /// Elle met à jour dynamiquement les options de statut disponibles en fonction du statut actuel de la commande.
        /// Seules les valeurs de statut supérieures ou égales au statut actuel sont proposées dans la liste.
        /// En cas de statut actuel non valide dans la liste, cette valeur est ajoutée temporairement pour permettre la sélection.
        /// </summary>
        /// <param name="sender">L'objet émetteur de l'événement (le DataGridView).</param>
        /// <param name="e">Les arguments de l'événement contenant les informations sur le contrôle d'édition en cours.</param>
        private async void DATAGRID_COMMANDES_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (DATAGRID_COMMANDES.CurrentCell.ColumnIndex == DATAGRID_COMMANDES.Columns["ColonneStatutCombo"].Index)
            {
                ComboBox combo = e.Control as ComboBox;
                if (combo != null)
                {
                    // Récupère la ligne et le statut actuel
                    var row = DATAGRID_COMMANDES.CurrentRow?.DataBoundItem as CommandeSuiviDTO;
                    if (row != null)
                    {
                        int statutActuel = row.StatutSuivi;

                        // Liste des statuts possible pour l'état d'un livre
                        var statutsDisponibles = new List<Statut>
                        {
                            new Statut { Value = 1, Libelle = "En cours" },
                            new Statut { Value = 2, Libelle = "Livré" },
                            new Statut { Value = 3, Libelle = "Disponible en points relais" },
                            new Statut { Value = 4, Libelle = "Annulé" }
                        };

                        // Nouvelle Liste en doublons pour gérer la partie admin
                        List<Statut> statutsAutorises;
                        
                        if  (Session.CurrentUser.Role == "ADMIN")
                        {
                            statutsAutorises = statutsDisponibles; 
                        }
                        else
                        {
                            statutsAutorises = statutsDisponibles
                                 .Where(s => s.Value >= statutActuel)
                                 .ToList();
                        };

                        // Retarder la mise à jour du DataSource pour éviter les conflits
                        await Task.Delay(100);

                        // Liaison à la comboBox
                        combo.DataSource = null; // important avant de le changer
                        combo.DisplayMember = "Libelle";
                        combo.ValueMember = "Value";
                        combo.DataSource = statutsAutorises;

                        // Si la valeur actuelle n'est pas valide dans la liste, on l'ajoute temporairement
                        if (!statutsAutorises.Any(s => s.Value == statutActuel))
                        {
                            // Ajoute la valeur actuelle (même si elle ne fait pas partie de la liste) pour la maintenir
                            statutsAutorises.Insert(0, new Statut { Value = statutActuel, Libelle = GetStatutLibelle(statutActuel) });
                            combo.SelectedValue = statutActuel;
                        }
                        else
                        {
                            // Si c'est valide, on sélectionne la valeur dans la ComboBox
                            combo.SelectedValue = statutActuel;
                        }

                        // Ajoute un gestionnaire d'événement pour valider lors de la sélection
                        combo.Validating -= ComboBox_Validating;
                        combo.Validating += ComboBox_Validating;
                    }
                }
            }
        }

        /// <summary>
        /// Récupération des informations changé dans la cellule et envois auprès de la BDD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DATAGRID_COMMANDES_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (DATAGRID_COMMANDES.Columns[e.ColumnIndex].Name == "ColonneStatutCombo")
            {
                var row = DATAGRID_COMMANDES.Rows[e.RowIndex].DataBoundItem as CommandeSuiviDTO;
                if (row != null)
                {
                    int nouveauStatut = (int)DATAGRID_COMMANDES.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                    string idSuivi = row.SuiviId;
                    string idCommande = row.CommandeId;

                    // Création de l'objet Suivi avec la nouvelle valeur
                    Suivi suiviModifie = new Suivi
                    {
                        id_suivi = idSuivi,
                        IdCommande = idCommande,
                        Status = nouveauStatut,
                        DateSuivi = DateTime.Now
                    };

                    // Envoi à l'API via la méthode existante
                    bool success = await controller.ModifierStatutCommande(row.SuiviId, row.CommandeId, nouveauStatut);

                    if (success)
                    {
                        MessageBox.Show("Statut modifié avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification du statut.");
                    }
                }
            }
        }

        #endregion

        #region "Liste des livres disponibles"
        /// <summary>
        /// Rempli la Dgv de "Liste des livres disponibles" dans la "section commandes des livres"
        /// </summary>
        /// <param name="livresCommandes"></param>
        private void RemplirLivresListeCommandes(List<Livre> livresCommandes)
        {
            bdgLivresListe.DataSource = livresCommandes;
            DATAGRID_LIST_COMMANDE_LIVRE.DataSource = bdgLivresListe;
            DATAGRID_LIST_COMMANDE_LIVRE.Columns["isbn"].Visible = false;
            DATAGRID_LIST_COMMANDE_LIVRE.Columns["idRayon"].Visible = false;
            DATAGRID_LIST_COMMANDE_LIVRE.Columns["idGenre"].Visible = false;
            DATAGRID_LIST_COMMANDE_LIVRE.Columns["idPublic"].Visible = false;
            DATAGRID_LIST_COMMANDE_LIVRE.Columns["image"].Visible = false;
            DATAGRID_LIST_COMMANDE_LIVRE.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DATAGRID_LIST_COMMANDE_LIVRE.Columns["id"].DisplayIndex = 0;
            DATAGRID_LIST_COMMANDE_LIVRE.Columns["titre"].DisplayIndex = 1;
        }

        /// <summary>
        /// Tri sur les colonnes de la Dgv
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DATAGRID_LIST_COMMANDE_LIVRE_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            VideLivresZones();
            string titreColonne = dgvLivresListe.Columns[e.ColumnIndex].HeaderText;
            List<Livre> sortedList = new List<Livre>();
            switch (titreColonne)
            {
                case "Id":
                    sortedList = lesLivres.OrderBy(o => o.Id).ToList();
                    break;
                case "Titre":
                    sortedList = lesLivres.OrderBy(o => o.Titre).ToList();
                    break;
                case "Collection":
                    sortedList = lesLivres.OrderBy(o => o.Collection).ToList();
                    break;
                case "Auteur":
                    sortedList = lesLivres.OrderBy(o => o.Auteur).ToList();
                    break;
                case "Genre":
                    sortedList = lesLivres.OrderBy(o => o.Genre).ToList();
                    break;
                case "Public":
                    sortedList = lesLivres.OrderBy(o => o.Public).ToList();
                    break;
                case "Rayon":
                    sortedList = lesLivres.OrderBy(o => o.Rayon).ToList();
                    break;
            }
            RemplirLivresListe(sortedList);
        }

        /// <summary>
        /// Recherche et affichage des livres dont le titre matche acec la saisie.
        /// Cette procédure est exécutée à chaque ajout ou suppression de caractère
        /// dans le textBox de saisie.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TXB_SEARCH_TITILE_LIVRE_TextChanged(object sender, EventArgs e)
        {
            if (!TXB_SEARCH_TITILE_LIVRE.Text.ToLower().Equals(""))
            {
                List<Livre> lesLivresParTitre;
                lesLivresParTitre = lesLivres.FindAll(x => x.Titre.ToLower().Contains(TXB_SEARCH_TITILE_LIVRE.Text.ToLower()));
                RemplirLivresListe(lesLivresParTitre);
            }
            else
            {
                RemplirLivresListeComplete();
            }
        }

        /// <summary>
        /// Syteme de recherche par titre d'un livre
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DATAGRID_LIST_COMMANDE_LIVRE_SelectionChanged(object sender, EventArgs e)
        {
            if (DATAGRID_LIST_COMMANDE_LIVRE.CurrentCell != null)
            {
                try
                {
                    Livre livre = (Livre)bdgLivresListe.List[bdgLivresListe.Position];
                    AfficheLivresInfosCommandes(livre);
                    AfficheCommandesInfos(livre.Id);
                }
                catch
                {
                    VideLivresZones();
                }
            }
            else
            {
                VideLivresInfos();
            }
        }

        #endregion

        #region "Informations des commandes en cours pour le livre"
        //Variables
        private string currentIdDocument; // à définir dans AfficheCommandesInfos

        /// <summary>
        /// Affichage et remplissage de la Dgv 
        /// </summary>
        /// <param name="idDocument"></param>
        private async Task AfficheCommandesInfos(string idDocument)
        {
            currentIdDocument = idDocument;

            List<CommandesDocuments> commandesDocuments = await controller.GetAllCommnadesDocuments();
            // Récupérer toutes les commandes associées à l'id_document
            commandesDocuments = await controller.GetAllCommnadesDocuments();

            // Filtrer les commandes pour récupérer celles qui correspondent à l'id_document
            var commandesFiltrees = commandesDocuments.Where(c => c.id_document == idDocument).ToList();

            // Afficher ces commandes dans un DataGridView ou autres contrôles
            DATAGRID_COMMANDES_DOCUMENTS.DataSource = commandesFiltrees;

            DATAGRID_COMMANDES_DOCUMENTS.Columns["id_commande"].HeaderText = "N° de la commande";
            DATAGRID_COMMANDES_DOCUMENTS.Columns["id_commandedocument"].Visible = false;
            DATAGRID_COMMANDES_DOCUMENTS.Columns["id_document"].HeaderText = "N° du document";
            DATAGRID_COMMANDES_DOCUMENTS.Columns["nbExemplaire"].HeaderText = "Nombres d'exemplaires";
            DATAGRID_COMMANDES_DOCUMENTS.Columns["idLivreDvd"].Visible = false;

            DATAGRID_COMMANDES_DOCUMENTS.Columns["id_commande"].DisplayIndex = 1;
            DATAGRID_COMMANDES_DOCUMENTS.Columns["id_document"].DisplayIndex = 2;
            DATAGRID_COMMANDES_DOCUMENTS.Columns["nbExemplaire"].DisplayIndex = 3;
        }

        private async Task<List<CommandesDocuments>> GetCommandesFiltreesEtTriees(string idDocument, string colonne, bool triAscendant)
        {
            // Récupérer toutes les commandes
            var toutesLesCommandes = await controller.GetAllCommnadesDocuments();

            // Filtrer par id_document
            var commandesFiltrees = toutesLesCommandes
                .Where(c => c.id_document == idDocument)
                .ToList();

            // Appliquer le tri selon la colonne
            switch (colonne)
            {
                case "N° de la commande":
                    return triAscendant
                        ? commandesFiltrees.OrderBy(c => c.id_commande).ToList()
                        : commandesFiltrees.OrderByDescending(c => c.id_commande).ToList();

                case "N° du document":
                    return triAscendant
                        ? commandesFiltrees.OrderBy(c => c.id_document).ToList()
                        : commandesFiltrees.OrderByDescending(c => c.id_document).ToList();

                case "Nombres d'exemplaires":
                    return triAscendant
                        ? commandesFiltrees.OrderBy(c => c.nbExemplaire).ToList()
                        : commandesFiltrees.OrderByDescending(c => c.nbExemplaire).ToList();

                default:
                    return commandesFiltrees; // tri non pris en charge
            }
        }

        /// <summary>
        /// Tri sur les colonnes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DATAGRID_COMMANDES_DOCUMENTS_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string titreColonne = DATAGRID_COMMANDES_DOCUMENTS.Columns[e.ColumnIndex].HeaderText;

            var listeTriee = GetCommandesFiltreesEtTriees(currentIdDocument, titreColonne, isAscending);
            isAscending = !isAscending;

            // Met à jour le DataGridView
            DATAGRID_COMMANDES_DOCUMENTS.DataSource = listeTriee;
        }

        /// <summary>
        /// Affichage des informations du livre sélectionné
        /// </summary>
        /// <param name="livre">le livre</param>
        private void AfficheLivresInfosCommandes(Livre livre)
        {
            txbLivresCommandesCollection.Text = livre.Collection;
            txbLivresCommandesIsbn.Text = livre.Isbn;
            txbLivresCommandesNumero.Text = livre.Id;
            txbLivresCommandesGenre.Text = livre.Genre;
            txbLivresCommandesPublic.Text = livre.Public;
            txbLivresCommandesRayon.Text = livre.Rayon;
            txbLivresCommandesTitre.Text = livre.Titre;
            txbLivresCommandesAuteur.Text = livre.Auteur;
            string image = livre.Image;
            try
            {
                pcbLivresImage.Image = Image.FromFile(image);
            }
            catch
            {
                pcbLivresImage.Image = null;
            }
        }
        #endregion

        #region Test méthode de l'onglet "Commandes des livres"
        //Debug Boutton pour la table Suivi
        private void TEST_GETTALLCOMMANDESDOCUMENTS_Click(object sender, EventArgs e)
        {
            var testCommandes = new TestCommandesDocuments();
            testCommandes.TesterRecuperationCommandes();
        }
        #endregion

        #region Gestion du DATAERROR
        /// <summary>
        /// Gestonnaire d'évènement pour ComboBox
        /// <desc>
        /// Permet de forcer l'utilisateur a choisir une option valide d'une ComboBox
        /// avant de passer à autre chose.
        /// </desc>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_Validating(object sender, CancelEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            if (combo != null)
            {
                if (!combo.Items.Cast<Statut>().Any(s => s.Value == (int)combo.SelectedValue))
                {
                    e.Cancel = true;
                }
            }
        }
        #endregion

        #region Gestion des nouvelles commandes
        /// <summary>
        /// Gère l'événement de clic sur le bouton pour ajouter une nouvelle commande.
        /// Cette méthode génère des ID pour la commande, le suivi, et la liaison, puis ouvre un formulaire de création de commande.
        /// Si l'utilisateur valide la création dans le formulaire, elle enregistre la commande, le suivi et la liaison dans la base de données.
        /// Ensuite, elle actualise la liste des commandes et affiche un message de succès ou d'erreur en fonction du résultat.
        /// </summary>
        /// <param name="sender">L'objet émetteur de l'événement (le bouton cliqué).</param>
        /// <param name="e">Les arguments de l'événement (données liées à l'événement de clic).</param>
        private async void BT_ADD_NEW_COMMANDE_Click(object sender, EventArgs e)
        {
            controller.GetAllCommandes();
            // Générez l'ID dans FrmMediatek
            string idCommande = await controller.GenerateCommandeId();
            string idSuivi = await controller.GenerateSuiviId();
            string idCommandeDocument = await controller.GenerateCommandeDocumentId();

            var addForm = new AddCommandeWindows(idCommande, idSuivi, idCommandeDocument);  // Passe l'ID généré à la fenêtre AddCommandeWindows

            if (addForm.ShowDialog() == DialogResult.OK)
            {
                var commande = addForm.CommandeCreee;
                var suivi = addForm.SuiviCree;
                var liaison = addForm.LiaisonCreee;

                if (commande != null && await controller.CreerCommande(commande))
                {
                    bool suiviOK = await controller.CreerSuivi(suivi);
                    bool liaisonOK = await controller.CreerCommandeDocument(liaison);

                    if (suiviOK && liaisonOK)
                    {
                        await controller.GenererDocumentUnitairesPourCommande(liaison);
                        MessageBox.Show("Commande, suivi, liaison et documents unitaires créés avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Commande créée mais erreur sur le suivi ou la liaison.");
                    }
                    LoadCommandes();  // Actualise la liste des commandes
                    LoadExemplaire();
                }
                else
                {
                    MessageBox.Show("Erreur lors de la création de la commande.");
                }
            }
            else
            {
                MessageBox.Show("Commande annulée.");
            }
        }

        #endregion

        #region Helper
        // Helper pour obtenir le libellé du statut
        private string GetStatutLibelle(int statut)
        {
            switch (statut)
            {
                case 1: return "En cours";
                case 2: return "Livré";
                case 3: return "Disponible en points relais";
                case 4: return "Annulé";
                default: return "Inconnu";
            }
        }

        #endregion

        #endregion

        private async void DATAGRID_COMMANDES_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (DATAGRID_COMMANDES.Columns[e.ColumnIndex].Name == "ColonneSuppression" && e.RowIndex >= 0)
            {
                var row = DATAGRID_COMMANDES.Rows[e.RowIndex].DataBoundItem as CommandeSuiviDTO;

                if (row != null && row.StatutSuivi >= 3)
                {
                    string idSuivi = row.SuiviId;
                    string idCommande = row.CommandeId;
                    string commandedocumentId = row.LiaisonCommandeDocument?.id_commandedocument;

                    bool successS = await controller.SupprimerSuivi(idSuivi);
                    bool successCD = await controller.SupprimerCommandeDocument(commandedocumentId);
                    bool successC = await controller.SupprimerCommande(idCommande);
                    if (successC && successS && successCD)
                    {
                        MessageBox.Show("Commande supprimé avec succès !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCommandes();
                    }
                    else
                    {
                        MessageBox.Show("Suppression échoué.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (Session.CurrentUser.Role == "ADMIN" && row != null)
                {
                    string idSuivi = row.SuiviId;
                    string idCommande = row.CommandeId;
                    string commandedocumentId = row.LiaisonCommandeDocument?.id_commandedocument;

                    bool successS = await controller.SupprimerSuivi(idSuivi);
                    bool successCD = await controller.SupprimerCommandeDocument(commandedocumentId);
                    bool successC = await controller.SupprimerCommande(idCommande);

                    if (successC && successS && successCD)
                    {
                        MessageBox.Show("Commande supprimé avec succès !", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadCommandes();
                    }
                    else
                    {
                        MessageBox.Show("Suppression échoué.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }

                if (DATAGRID_COMMANDES.Columns[e.ColumnIndex].Name == "ColonneSuppression" && e.RowIndex > 0)
                {
                    if (row != null && row.StatutSuivi <= 2 && Session.CurrentUser.Role != "ADMIN")
                        MessageBox.Show("Suppression impossible.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        #region Onglet Livres disponibles 
        //Variables 
        List<Livre> livresdisponibles = new List<Livre>();
        private List<DocumentUnitaire> documentUnitaires = new List<DocumentUnitaire>();

        private async void TAB_COMMANDE_Enter_1(object sender, EventArgs e)
        {
            lesCommandes = await controller.GetAllCommandes();
            lesSuivi = await controller.GetAllSuivi();
            lesCommandesDocuments = await controller.GetAllCommnadesDocuments();

            commandeSuivisBase = await controller.GetCommandesSuivisDTO();
            commandeSuivis = new List<CommandeSuiviDTO>(commandeSuivisBase);
            RemplirCommandesListeComplete();

            lesLivres = await controller.GetAllLivres();
            RemplirComboCategorie(await controller.GetAllGenres(), bdgGenres, cbxLivresGenres);
            RemplirComboCategorie(await controller.GetAllPublics(), bdgPublics, cbxLivresPublics);
            RemplirComboCategorie(await controller.GetAllRayons(), bdgRayons, cbxLivresRayons);

            documentUnitaires = await controller.GetAllDocumentUnitaires();

            RemplirListeLivresDisponibleComplete();
            RemplirLivresListeDisponible(livresdisponibles);
        }

        private void RemplirListeLivresDisponibleComplete()
        {
            VideLivresZones();
            //Remplissage de la DataGridView des Livres
            RemplirLivresListeCommandes(lesLivres);
        }

        /// <summary>
        /// Permet de recharger complètement la liste des commandes
        /// </summary>
        private async Task LoadDocumentUnitaire()
        {
            List<DocumentUnitaire> documentUnitaires = await controller.GetAllDocumentUnitaires();
            ConfigurerColonnesDgvExemplaires(documentUnitaires);
        }

        /// <summary>
        /// Rempli la Dgv de "Liste des livres disponibles" dans la "section commandes des livres"
        /// </summary>
        /// <param name="livresCommandes"></param>
        private void RemplirLivresListeDisponible(List<Livre> livresCommandes)
        {
            bdgLivresListe.DataSource = livresCommandes;
            DATAGRID_LIVRES_DISPONIBLES.DataSource = bdgLivresListe;
            DATAGRID_LIVRES_DISPONIBLES.Columns["isbn"].Visible = false;
            DATAGRID_LIVRES_DISPONIBLES.Columns["idRayon"].Visible = false;
            DATAGRID_LIVRES_DISPONIBLES.Columns["idGenre"].Visible = false;
            DATAGRID_LIVRES_DISPONIBLES.Columns["idPublic"].Visible = false;
            DATAGRID_LIVRES_DISPONIBLES.Columns["image"].Visible = false;
            DATAGRID_LIVRES_DISPONIBLES.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            DATAGRID_LIVRES_DISPONIBLES.Columns["id"].DisplayIndex = 0;
            DATAGRID_LIVRES_DISPONIBLES.Columns["titre"].DisplayIndex = 1;
        }


        #endregion

        private void DATAGRID_LIVRES_DISPONIBLES_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                var selectedLivre = DATAGRID_LIVRES_DISPONIBLES.Rows[e.RowIndex].DataBoundItem as Livre;
                if (selectedLivre != null)
                {
                    string idLivre = selectedLivre.Id;

                    //MessageBox.Show(documentUnitaires.Count().ToString());
                    //Filtrage de la liste
                    var exemplaires = documentUnitaires
                        .Where(doc=> doc.IdDocument == idLivre)
                        .ToList();

                    ConfigurerColonnesDgvExemplaires(exemplaires);
                }
            }
        }

        private void ConfigurerColonnesDgvExemplaires(List<DocumentUnitaire> exemplaires)
            {
                Debug.WriteLine("▶ Début de ConfigurerColonnesDgvExemplaires");

                // Vérification des objets critiques
                if (documentUnitaires == null)
                {
                    Debug.WriteLine("❌ documentUnitaires est null, arrêt de la méthode.");
                    return;
                }

                if (NOMBRE_EXEMPLE_LIVRES == null)
                {
                    Debug.WriteLine("❌ NOMBRE_EXEMPLE_LIVRES est null, arrêt de la méthode.");
                    return;
                }

                Debug.WriteLine($"✅ {documentUnitaires.Count} exemplaires à afficher.");

                // Reset propre du DataGridView
                Debug.WriteLine("ℹ️ Reset des colonnes et datasource du DataGridView.");
                NOMBRE_EXEMPLE_LIVRES.DataSource = null;
                NOMBRE_EXEMPLE_LIVRES.Columns.Clear();

                // Préparation des colonnes
                Debug.WriteLine("ℹ️ Création des colonnes...");

                var colIdCommande = new DataGridViewTextBoxColumn
                {
                    Name = "IdCommande",
                    HeaderText = "N° de la commande",
                    DataPropertyName = "IdCommande"
                };

                var colIdExemplaire = new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    HeaderText = "Identifiant",
                    DataPropertyName = "Id"
                };

                var colLibelleEtatLivre = new DataGridViewTextBoxColumn
                {
                    Name = "LibelleEtatDocument",
                    HeaderText = "Etat du livre",
                    DataPropertyName = "LibelleEtatDocument"
                };

                var colDateAt = new DataGridViewTextBoxColumn
                {
                    Name = "DateAchat",
                    HeaderText = "Date de l'achat",
                    DataPropertyName = "DateAchat"
                };

                var btnColSupression = new DataGridViewButtonColumn
                {
                    Name = "ColonneSuppression",
                    HeaderText = "Supprimer un livre",
                    Text = "❌ Supprimer",
                    UseColumnTextForButtonValue = true
                };

                var statuts = new List<Statut>
                {
                    new Statut { Value = 1, Libelle = "Neuf" },
                    new Statut { Value = 2, Libelle = "Très bon" },
                    new Statut { Value = 3, Libelle = "Moyen" },
                    new Statut { Value = 4, Libelle = "Endommagé" },
                    new Statut { Value = 5, Libelle = "Illisible" }
                };

                var comboCol = new DataGridViewComboBoxColumn
                {
                    Name = "ColonneStatutCombo",
                    HeaderText = "Modifier le statut",
                    DataPropertyName = "Etat",
                    DataSource = statuts,
                    DisplayMember = "Libelle",
                    ValueMember = "Value",
                    DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton
                };

                Debug.WriteLine("✅ Colonnes créées avec succès.");

                Debug.WriteLine("ℹ️ Ajout des colonnes au DataGridView...");
                // ⛓️ Ajout manuel des colonnes
                NOMBRE_EXEMPLE_LIVRES.Columns.AddRange(colIdExemplaire, colIdCommande, colDateAt, colLibelleEtatLivre, comboCol, btnColSupression);

                Debug.WriteLine("ℹ️ Implémentation des paramètre globaux au DataGridView...");
                // 🧷 Paramètres globaux
                NOMBRE_EXEMPLE_LIVRES.AutoGenerateColumns = false;
                NOMBRE_EXEMPLE_LIVRES.SelectionMode = DataGridViewSelectionMode.CellSelect;
                NOMBRE_EXEMPLE_LIVRES.MultiSelect = false;
                NOMBRE_EXEMPLE_LIVRES.RowHeadersVisible = false;
                NOMBRE_EXEMPLE_LIVRES.AllowUserToAddRows = false;
                NOMBRE_EXEMPLE_LIVRES.AllowUserToDeleteRows = false;
                NOMBRE_EXEMPLE_LIVRES.AllowUserToResizeColumns = false;
                NOMBRE_EXEMPLE_LIVRES.AllowUserToResizeRows = false;

                Debug.WriteLine("✅ Colonnes ajoutées.");


                // 🧊 Rendre tout ReadOnly sauf statut combo
                NOMBRE_EXEMPLE_LIVRES.ReadOnly = false;
                foreach (DataGridViewColumn col in NOMBRE_EXEMPLE_LIVRES.Columns)
                    col.ReadOnly = col.Name != "ColonneStatutCombo";

                // 🧯 Tri désactivé sur les colonnes sensibles
                foreach (DataGridViewColumn col in NOMBRE_EXEMPLE_LIVRES.Columns)
                {
                    if (col.Name == "ColonneStatutCombo" || col.Name == "ColonneSuppression")
                        col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                // Assignation de la datasource
                 Debug.WriteLine("ℹ️ Affectation de la DataSource au DataGridView.");
                NOMBRE_EXEMPLE_LIVRES.DataSource = exemplaires;

                // Masquage des colonnes inutiles si elles existent
                Debug.WriteLine("ℹ️ Tentative de masquage des colonnes Etat et IdDocument (si présentes).");
                if (NOMBRE_EXEMPLE_LIVRES.Columns.Contains("Etat"))
                {
                    NOMBRE_EXEMPLE_LIVRES.Columns["Etat"].Visible = false;
                }
                if (NOMBRE_EXEMPLE_LIVRES.Columns.Contains("IdDocument"))
                {
                    NOMBRE_EXEMPLE_LIVRES.Columns["IdDocument"].Visible = false;
                }


            Debug.WriteLine("✅ DataSource affectée avec succès.");
                Debug.WriteLine("▶ Fin de ConfigurerColonnesDgvExemplaires");
            // Vérifie le rôle de l'utilisateur connecté

                if (Session.CurrentUser != null)
                {
                    if (Session.CurrentUser.Role == "ADMIN")
                    {
                        // Afficher la colonne de suppression
                        NOMBRE_EXEMPLE_LIVRES.Columns["ColonneSuppression"].Visible = true;
                    }
                    else
                    {
                        // Masquer la colonne de suppression pour les autres
                        NOMBRE_EXEMPLE_LIVRES.Columns["ColonneSuppression"].Visible = false;
                    }
                }
        }

        private async void NOMBRE_EXEMPLE_LIVRES_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (NOMBRE_EXEMPLE_LIVRES.CurrentCell.ColumnIndex == NOMBRE_EXEMPLE_LIVRES.Columns["ColonneStatutCombo"].Index)
            {
                ComboBox combo = e.Control as ComboBox;
                if (combo != null)
                {
                    // Récupère la ligne et le statut actuel
                    var row = NOMBRE_EXEMPLE_LIVRES.CurrentRow?.DataBoundItem as DocumentUnitaire;
                    if (row != null)
                    {
                        int statutActuel = row.Etat;

                        // Liste dynamique des statuts autorisés (>= statut actuel)
                        var statutsDisponibles = new List<Statut>
                        {
                            new Statut { Value = 1, Libelle = "Neuf" },
                            new Statut { Value = 2, Libelle = "Très bon" },
                            new Statut { Value = 3, Libelle = "Moyen" },
                            new Statut { Value = 4, Libelle = "Endommagé" },
                            new Statut { Value = 5, Libelle = "Illisible" }
                        };

                        List<Statut> statutsAutorises;

                        if (Session.CurrentUser.Role == "ADMIN")
                        {
                            statutsAutorises = statutsDisponibles;
                        }
                        else
                        {
                            statutsAutorises = statutsDisponibles
                                .Where(s => s.Value >= statutActuel)
                                .ToList();
                        }

                        // Retarder la mise à jour du DataSource pour éviter les conflits
                        await Task.Delay(100);

                        // Liaison à la comboBox
                        combo.DataSource = null; // important avant de le changer
                        combo.DisplayMember = "Libelle";
                        combo.ValueMember = "Value";
                        combo.DataSource = statutsAutorises;

                        // Si la valeur actuelle n'est pas valide dans la liste, on l'ajoute temporairement
                        if (!statutsAutorises.Any(s => s.Value == statutActuel))
                        {
                            // Ajoute la valeur actuelle (même si elle ne fait pas partie de la liste) pour la maintenir
                            statutsAutorises.Insert(0, new Statut { Value = statutActuel, Libelle = GetStatutLibelle(statutActuel) });
                            combo.SelectedValue = statutActuel;
                        }
                        else
                        {
                            // Si c'est valide, on sélectionne la valeur dans la ComboBox
                            combo.SelectedValue = statutActuel;
                        }

                        // Ajoute un gestionnaire d'événement pour valider lors de la sélection
                        combo.Validating -= ComboBox_Validating;
                        combo.Validating += ComboBox_Validating;
                    }
                }
            }
        }

        private async void NOMBRE_EXEMPLE_LIVRES_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (NOMBRE_EXEMPLE_LIVRES.Columns[e.ColumnIndex].Name == "ColonneStatutCombo")
            {
                var row = NOMBRE_EXEMPLE_LIVRES.Rows[e.RowIndex].DataBoundItem as DocumentUnitaire;
                if (row != null)
                {
                    int nouveauStatut = (int)NOMBRE_EXEMPLE_LIVRES.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                    string Id = row.Id;

                    // Création de l'objet Suivi avec la nouvelle valeur
                    DocumentUnitaire suiviModifie = new DocumentUnitaire
                    {
                        Id = row.Id,
                        IdDocument = row.IdDocument,
                        Etat = nouveauStatut,
                        DateAchat = row.DateAchat,
                        IdCommande = row.IdCommande
                    };

                    // Envoi à l'API via la méthode existante
                    bool success = await controller.ModifierEtatDocumentUnitaire(Id, nouveauStatut);

                    if (success)
                    {
                        MessageBox.Show("Statut modifié avec succès !");
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification du statut.");
                    }
                }
            }
        }

        private async void NOMBRE_EXEMPLE_LIVRES_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (NOMBRE_EXEMPLE_LIVRES.Columns[e.ColumnIndex].Name == "ColonneSuppression" && e.RowIndex >= 0)
            {
                var row = NOMBRE_EXEMPLE_LIVRES.Rows[e.RowIndex].DataBoundItem as DocumentUnitaire;

                if (Session.CurrentUser.Role == "ADMIN" && row != null)
                {
                    string Id = row.Id;


                    bool success = await controller.SupprimerDocumentUnitaire(Id);
                    if (success)
                    {
                        MessageBox.Show("Livre supprimé avec succès !");
                        LoadDocumentUnitaire(); // Recharge la DGV si tu veux la MAJ directe
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la suppression du Livre !");
                    }
                }
                else
                {

                    if (row != null && row.Etat >= 4)
                    {
                        string Id = row.Id;


                        bool success = await controller.SupprimerDocumentUnitaire(Id);
                        if (success)
                        {
                            MessageBox.Show("Livre supprimé avec succès !");
                            LoadDocumentUnitaire(); // Recharge la DGV si tu veux la MAJ directe
                        }
                        else
                        {
                            MessageBox.Show("Erreur lors de la suppression du Livre !");
                        }

                    }

                    if (row != null && row.Etat <= 3)
                        MessageBox.Show("Suppression impossible.", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void MettreAJourInfosUtilisateur()
        {
            if (Session.CurrentUser != null)
            {
                lblInfosUtilisateurs.Text = $"Connecté : {Session.CurrentUser.Username} ({Session.CurrentUser.Role})";
            }
        }

        private void TAB_COMMANDE_SelectedIndexChanged(object sender, EventArgs e)
        {
            MettreAJourInfosUtilisateur();
        }

        /// <summary>
        /// Permet de recharger complètement la liste des exemplaires
        /// </summary>
        private async Task LoadExemplaire()
        {
            List<DocumentUnitaire> exemplaires = await controller.GetAllDocumentUnitaires();
            ConfigurerColonnesDgvExemplaires(exemplaires);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadExemplaire();
        }
    }
}
