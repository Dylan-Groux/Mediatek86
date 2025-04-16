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
            this.panel1 = new System.Windows.Forms.Panel();
            this.NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TB_MONTANT_COMMANDE = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TB_ID_LIVRE = new System.Windows.Forms.MaskedTextBox();
            this.LB_ID_LIVRE = new System.Windows.Forms.Label();
            this.BT_ADD_ONE_COMMANDE = new System.Windows.Forms.Button();
            this.BT_ADD_IMG_COMMANDE = new System.Windows.Forms.Button();
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
            // panel1
            // 
            this.panel1.Controls.Add(this.NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.TB_MONTANT_COMMANDE);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.TB_ID_LIVRE);
            this.panel1.Controls.Add(this.LB_ID_LIVRE);
            this.panel1.Location = new System.Drawing.Point(12, 253);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(295, 259);
            this.panel1.TabIndex = 6;
            // 
            // NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE
            // 
            this.NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE.Location = new System.Drawing.Point(158, 95);
            this.NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE.Name = "NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE";
            this.NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE.Size = new System.Drawing.Size(123, 20);
            this.NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 23);
            this.label2.TabIndex = 12;
            this.label2.Text = "Nombres d\'Exemplaires :";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TB_MONTANT_COMMANDE
            // 
            this.TB_MONTANT_COMMANDE.Location = new System.Drawing.Point(158, 43);
            this.TB_MONTANT_COMMANDE.Name = "TB_MONTANT_COMMANDE";
            this.TB_MONTANT_COMMANDE.Size = new System.Drawing.Size(123, 20);
            this.TB_MONTANT_COMMANDE.TabIndex = 11;
            this.TB_MONTANT_COMMANDE.MaskInputRejected += new System.Windows.Forms.MaskInputRejectedEventHandler(this.maskedTextBox1_MaskInputRejected);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Montant de la commande :";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label1.Click += new System.EventHandler(this.label1_Click_1);
            // 
            // TB_ID_LIVRE
            // 
            this.TB_ID_LIVRE.Location = new System.Drawing.Point(158, 69);
            this.TB_ID_LIVRE.Name = "TB_ID_LIVRE";
            this.TB_ID_LIVRE.Size = new System.Drawing.Size(123, 20);
            this.TB_ID_LIVRE.TabIndex = 8;
            // 
            // LB_ID_LIVRE
            // 
            this.LB_ID_LIVRE.Location = new System.Drawing.Point(3, 66);
            this.LB_ID_LIVRE.Name = "LB_ID_LIVRE";
            this.LB_ID_LIVRE.Size = new System.Drawing.Size(149, 23);
            this.LB_ID_LIVRE.TabIndex = 5;
            this.LB_ID_LIVRE.Text = "Id du livre :  ";
            this.LB_ID_LIVRE.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
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
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BT_ADD_ONE_COMMANDE;
        private System.Windows.Forms.Button BT_ADD_IMG_COMMANDE;
        private System.Windows.Forms.MaskedTextBox TB_MONTANT_COMMANDE;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MaskedTextBox NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MaskedTextBox TB_ID_LIVRE;
        private System.Windows.Forms.Label LB_ID_LIVRE;
    }
}