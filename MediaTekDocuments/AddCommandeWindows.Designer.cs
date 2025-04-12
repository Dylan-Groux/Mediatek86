namespace MediaTekDocuments
{
    partial class AddCommandeWindows
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.PB_COMMANDE_PHOTO = new System.Windows.Forms.PictureBox();
            this.LB_ID_LIVRE = new System.Windows.Forms.Label();
            this.LB_NUM_COMMANDE = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.LB_GP_ADD_COMMANDE = new System.Windows.Forms.Label();
            this.TB_NUM_COMMANDE = new System.Windows.Forms.MaskedTextBox();
            this.TB_ID_LIVRE = new System.Windows.Forms.MaskedTextBox();
            this.BT_ADD_ONE_COMMANDE = new System.Windows.Forms.Button();
            this.BT_ADD_IMG_COMMANDE = new System.Windows.Forms.Button();
            this.TB_TITLE_LIVRE = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_DESC_LIVRE = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CB_GP_COMMANDE = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.PB_COMMANDE_PHOTO)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // PB_COMMANDE_PHOTO
            // 
            this.PB_COMMANDE_PHOTO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PB_COMMANDE_PHOTO.Location = new System.Drawing.Point(12, 12);
            this.PB_COMMANDE_PHOTO.Name = "PB_COMMANDE_PHOTO";
            this.PB_COMMANDE_PHOTO.Size = new System.Drawing.Size(296, 229);
            this.PB_COMMANDE_PHOTO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.PB_COMMANDE_PHOTO.TabIndex = 1;
            this.PB_COMMANDE_PHOTO.TabStop = false;
            // 
            // LB_ID_LIVRE
            // 
            this.LB_ID_LIVRE.Location = new System.Drawing.Point(3, 39);
            this.LB_ID_LIVRE.Name = "LB_ID_LIVRE";
            this.LB_ID_LIVRE.Size = new System.Drawing.Size(149, 23);
            this.LB_ID_LIVRE.TabIndex = 5;
            this.LB_ID_LIVRE.Text = "Id du livre :  ";
            this.LB_ID_LIVRE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // LB_NUM_COMMANDE
            // 
            this.LB_NUM_COMMANDE.Location = new System.Drawing.Point(3, 12);
            this.LB_NUM_COMMANDE.Name = "LB_NUM_COMMANDE";
            this.LB_NUM_COMMANDE.Size = new System.Drawing.Size(149, 23);
            this.LB_NUM_COMMANDE.TabIndex = 4;
            this.LB_NUM_COMMANDE.Text = "Numéro de commande :  ";
            this.LB_NUM_COMMANDE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CB_GP_COMMANDE);
            this.panel1.Controls.Add(this.TB_DESC_LIVRE);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.TB_TITLE_LIVRE);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TB_ID_LIVRE);
            this.panel1.Controls.Add(this.TB_NUM_COMMANDE);
            this.panel1.Controls.Add(this.LB_GP_ADD_COMMANDE);
            this.panel1.Controls.Add(this.LB_ID_LIVRE);
            this.panel1.Controls.Add(this.LB_NUM_COMMANDE);
            this.panel1.Location = new System.Drawing.Point(12, 253);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(295, 259);
            this.panel1.TabIndex = 6;
            // 
            // LB_GP_ADD_COMMANDE
            // 
            this.LB_GP_ADD_COMMANDE.Location = new System.Drawing.Point(3, 66);
            this.LB_GP_ADD_COMMANDE.Name = "LB_GP_ADD_COMMANDE";
            this.LB_GP_ADD_COMMANDE.Size = new System.Drawing.Size(149, 23);
            this.LB_GP_ADD_COMMANDE.TabIndex = 6;
            this.LB_GP_ADD_COMMANDE.Text = "Groupe : ";
            this.LB_GP_ADD_COMMANDE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LB_GP_ADD_COMMANDE.Click += new System.EventHandler(this.label1_Click);
            // 
            // TB_NUM_COMMANDE
            // 
            this.TB_NUM_COMMANDE.Location = new System.Drawing.Point(158, 16);
            this.TB_NUM_COMMANDE.Name = "TB_NUM_COMMANDE";
            this.TB_NUM_COMMANDE.Size = new System.Drawing.Size(123, 20);
            this.TB_NUM_COMMANDE.TabIndex = 7;
            // 
            // TB_ID_LIVRE
            // 
            this.TB_ID_LIVRE.Location = new System.Drawing.Point(158, 42);
            this.TB_ID_LIVRE.Name = "TB_ID_LIVRE";
            this.TB_ID_LIVRE.Size = new System.Drawing.Size(123, 20);
            this.TB_ID_LIVRE.TabIndex = 8;
            // 
            // BT_ADD_ONE_COMMANDE
            // 
            this.BT_ADD_ONE_COMMANDE.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BT_ADD_ONE_COMMANDE.Location = new System.Drawing.Point(0, 533);
            this.BT_ADD_ONE_COMMANDE.Name = "BT_ADD_ONE_COMMANDE";
            this.BT_ADD_ONE_COMMANDE.Size = new System.Drawing.Size(323, 23);
            this.BT_ADD_ONE_COMMANDE.TabIndex = 10;
            this.BT_ADD_ONE_COMMANDE.Text = "Ajouter une commande";
            this.BT_ADD_ONE_COMMANDE.UseVisualStyleBackColor = true;
            this.BT_ADD_ONE_COMMANDE.Click += new System.EventHandler(this.BT_ADD_ONE_COMMANDE_Click);
            // 
            // BT_ADD_IMG_COMMANDE
            // 
            this.BT_ADD_IMG_COMMANDE.Location = new System.Drawing.Point(12, 218);
            this.BT_ADD_IMG_COMMANDE.Name = "BT_ADD_IMG_COMMANDE";
            this.BT_ADD_IMG_COMMANDE.Size = new System.Drawing.Size(295, 23);
            this.BT_ADD_IMG_COMMANDE.TabIndex = 11;
            this.BT_ADD_IMG_COMMANDE.Text = "Ajouter une image";
            this.BT_ADD_IMG_COMMANDE.UseVisualStyleBackColor = true;
            this.BT_ADD_IMG_COMMANDE.Click += new System.EventHandler(this.button1_Click);
            // 
            // TB_TITLE_LIVRE
            // 
            this.TB_TITLE_LIVRE.Location = new System.Drawing.Point(158, 95);
            this.TB_TITLE_LIVRE.Name = "TB_TITLE_LIVRE";
            this.TB_TITLE_LIVRE.Size = new System.Drawing.Size(123, 20);
            this.TB_TITLE_LIVRE.TabIndex = 11;
            this.TB_TITLE_LIVRE.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.maskedTextBox1_MaskInputRejected);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Titre du livre";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // TB_DESC_LIVRE
            // 
            this.TB_DESC_LIVRE.Location = new System.Drawing.Point(11, 143);
            this.TB_DESC_LIVRE.Name = "TB_DESC_LIVRE";
            this.TB_DESC_LIVRE.Size = new System.Drawing.Size(260, 20);
            this.TB_DESC_LIVRE.TabIndex = 13;
            this.TB_DESC_LIVRE.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.maskedTextBox2_MaskInputRejected);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "Description du livre";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // CB_GP_COMMANDE
            // 
            this.CB_GP_COMMANDE.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_GP_COMMANDE.FormattingEnabled = true;
            this.CB_GP_COMMANDE.Location = new System.Drawing.Point(158, 68);
            this.CB_GP_COMMANDE.Name = "CB_GP_COMMANDE";
            this.CB_GP_COMMANDE.Size = new System.Drawing.Size(123, 21);
            this.CB_GP_COMMANDE.TabIndex = 14;
            this.CB_GP_COMMANDE.SelectedIndexChanged += new System.EventHandler(this.CB_GP_COMMANDE_SelectedIndexChanged);
            // 
            // AddCommandeWindows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(323, 556);
            this.Controls.Add(this.BT_ADD_IMG_COMMANDE);
            this.Controls.Add(this.BT_ADD_ONE_COMMANDE);
            this.Controls.Add(this.PB_COMMANDE_PHOTO);
            this.Controls.Add(this.panel1);
            this.Name = "AddCommandeWindows";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ajouter une commande";
            ((System.ComponentModel.ISupportInitialize)(this.PB_COMMANDE_PHOTO)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox PB_COMMANDE_PHOTO;
        private System.Windows.Forms.Label LB_ID_LIVRE;
        private System.Windows.Forms.Label LB_NUM_COMMANDE;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label LB_GP_ADD_COMMANDE;
        private System.Windows.Forms.MaskedTextBox TB_ID_LIVRE;
        private System.Windows.Forms.MaskedTextBox TB_NUM_COMMANDE;
        private System.Windows.Forms.Button BT_ADD_ONE_COMMANDE;
        private System.Windows.Forms.Button BT_ADD_IMG_COMMANDE;
        private System.Windows.Forms.MaskedTextBox TB_TITLE_LIVRE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox TB_DESC_LIVRE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox CB_GP_COMMANDE;
    }
}