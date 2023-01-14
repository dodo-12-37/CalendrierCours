namespace CalendrierCours.WinFormUI
{
    partial class fPrincipal
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
            this.lbCohortes = new System.Windows.Forms.ListBox();
            this.msPrincipal = new System.Windows.Forms.MenuStrip();
            this.tsmiFichier = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiQuitter = new System.Windows.Forms.ToolStripMenuItem();
            this.lCohortes = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bExportTousLesCours = new System.Windows.Forms.Button();
            this.lInformation = new System.Windows.Forms.Label();
            this.tbCategorie = new System.Windows.Forms.TextBox();
            this.bExportCours = new System.Windows.Forms.Button();
            this.lCategorie = new System.Windows.Forms.Label();
            this.tbPrenom = new System.Windows.Forms.TextBox();
            this.lPrenom = new System.Windows.Forms.Label();
            this.tbNom = new System.Windows.Forms.TextBox();
            this.bmajCours = new System.Windows.Forms.Button();
            this.lNom = new System.Windows.Forms.Label();
            this.tbIntitule = new System.Windows.Forms.TextBox();
            this.lIntitule = new System.Windows.Forms.Label();
            this.tbNumero = new System.Windows.Forms.TextBox();
            this.lNumero = new System.Windows.Forms.Label();
            this.lbCours = new System.Windows.Forms.ListBox();
            this.lCours = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbSeances = new System.Windows.Forms.ListBox();
            this.lSeances = new System.Windows.Forms.Label();
            this.msPrincipal.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbCohortes
            // 
            this.lbCohortes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbCohortes.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbCohortes.FormattingEnabled = true;
            this.lbCohortes.HorizontalScrollbar = true;
            this.lbCohortes.ItemHeight = 17;
            this.lbCohortes.Location = new System.Drawing.Point(3, 26);
            this.lbCohortes.Name = "lbCohortes";
            this.lbCohortes.ScrollAlwaysVisible = true;
            this.lbCohortes.Size = new System.Drawing.Size(235, 293);
            this.lbCohortes.TabIndex = 1;
            this.lbCohortes.Click += new System.EventHandler(this.lbCohortes_Click);
            this.lbCohortes.SelectedIndexChanged += new System.EventHandler(this.lbCohortes_Click);
            // 
            // msPrincipal
            // 
            this.msPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiFichier});
            this.msPrincipal.Location = new System.Drawing.Point(0, 0);
            this.msPrincipal.Name = "msPrincipal";
            this.msPrincipal.Size = new System.Drawing.Size(1084, 24);
            this.msPrincipal.TabIndex = 0;
            this.msPrincipal.Text = "menuStrip1";
            // 
            // tsmiFichier
            // 
            this.tsmiFichier.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPreferences,
            this.toolStripSeparator1,
            this.tsmiQuitter});
            this.tsmiFichier.Name = "tsmiFichier";
            this.tsmiFichier.Size = new System.Drawing.Size(54, 20);
            this.tsmiFichier.Text = "&Fichier";
            // 
            // tsmiPreferences
            // 
            this.tsmiPreferences.Name = "tsmiPreferences";
            this.tsmiPreferences.Size = new System.Drawing.Size(153, 22);
            this.tsmiPreferences.Text = "&Préférences";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(150, 6);
            // 
            // tsmiQuitter
            // 
            this.tsmiQuitter.Name = "tsmiQuitter";
            this.tsmiQuitter.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.tsmiQuitter.Size = new System.Drawing.Size(153, 22);
            this.tsmiQuitter.Text = "&Quitter";
            this.tsmiQuitter.Click += new System.EventHandler(this.tsmiQuitter_Click);
            // 
            // lCohortes
            // 
            this.lCohortes.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lCohortes.Location = new System.Drawing.Point(0, 0);
            this.lCohortes.Name = "lCohortes";
            this.lCohortes.Size = new System.Drawing.Size(238, 23);
            this.lCohortes.TabIndex = 2;
            this.lCohortes.Text = "Cohortes";
            this.lCohortes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Controls.Add(this.lbCohortes);
            this.panel1.Controls.Add(this.lCohortes);
            this.panel1.Location = new System.Drawing.Point(12, 27);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.panel1.Size = new System.Drawing.Size(241, 333);
            this.panel1.TabIndex = 4;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel2.BackColor = System.Drawing.Color.Gainsboro;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel2.Controls.Add(this.bExportTousLesCours);
            this.panel2.Controls.Add(this.lInformation);
            this.panel2.Controls.Add(this.tbCategorie);
            this.panel2.Controls.Add(this.bExportCours);
            this.panel2.Controls.Add(this.lCategorie);
            this.panel2.Controls.Add(this.tbPrenom);
            this.panel2.Controls.Add(this.lPrenom);
            this.panel2.Controls.Add(this.tbNom);
            this.panel2.Controls.Add(this.bmajCours);
            this.panel2.Controls.Add(this.lNom);
            this.panel2.Controls.Add(this.tbIntitule);
            this.panel2.Controls.Add(this.lIntitule);
            this.panel2.Controls.Add(this.tbNumero);
            this.panel2.Controls.Add(this.lNumero);
            this.panel2.Controls.Add(this.lbCours);
            this.panel2.Controls.Add(this.lCours);
            this.panel2.Location = new System.Drawing.Point(256, 27);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.panel2.Size = new System.Drawing.Size(581, 333);
            this.panel2.TabIndex = 5;
            // 
            // bExportTousLesCours
            // 
            this.bExportTousLesCours.AutoSize = true;
            this.bExportTousLesCours.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.bExportTousLesCours.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExportTousLesCours.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bExportTousLesCours.Location = new System.Drawing.Point(421, 244);
            this.bExportTousLesCours.Name = "bExportTousLesCours";
            this.bExportTousLesCours.Size = new System.Drawing.Size(155, 33);
            this.bExportTousLesCours.TabIndex = 10;
            this.bExportTousLesCours.Text = "Exporter les cours";
            this.bExportTousLesCours.UseVisualStyleBackColor = true;
            // 
            // lInformation
            // 
            this.lInformation.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lInformation.Location = new System.Drawing.Point(258, 280);
            this.lInformation.Name = "lInformation";
            this.lInformation.Size = new System.Drawing.Size(318, 44);
            this.lInformation.TabIndex = 15;
            this.lInformation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbCategorie
            // 
            this.tbCategorie.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbCategorie.Location = new System.Drawing.Point(343, 170);
            this.tbCategorie.Name = "tbCategorie";
            this.tbCategorie.Size = new System.Drawing.Size(235, 25);
            this.tbCategorie.TabIndex = 7;
            this.tbCategorie.TextChanged += new System.EventHandler(this.tbCours_TextChanged);
            // 
            // bExportCours
            // 
            this.bExportCours.AutoSize = true;
            this.bExportCours.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.bExportCours.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bExportCours.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bExportCours.Location = new System.Drawing.Point(260, 244);
            this.bExportCours.Name = "bExportCours";
            this.bExportCours.Size = new System.Drawing.Size(155, 33);
            this.bExportCours.TabIndex = 9;
            this.bExportCours.Text = "Exporter le cours";
            this.bExportCours.UseVisualStyleBackColor = true;
            this.bExportCours.Click += new System.EventHandler(this.bExportCours_Click);
            // 
            // lCategorie
            // 
            this.lCategorie.AutoSize = true;
            this.lCategorie.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lCategorie.Location = new System.Drawing.Point(260, 173);
            this.lCategorie.Name = "lCategorie";
            this.lCategorie.Size = new System.Drawing.Size(77, 21);
            this.lCategorie.TabIndex = 13;
            this.lCategorie.Text = "Catégorie";
            // 
            // tbPrenom
            // 
            this.tbPrenom.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbPrenom.Location = new System.Drawing.Point(343, 135);
            this.tbPrenom.Name = "tbPrenom";
            this.tbPrenom.Size = new System.Drawing.Size(235, 25);
            this.tbPrenom.TabIndex = 6;
            this.tbPrenom.TextChanged += new System.EventHandler(this.tbCours_TextChanged);
            // 
            // lPrenom
            // 
            this.lPrenom.AutoSize = true;
            this.lPrenom.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lPrenom.Location = new System.Drawing.Point(260, 138);
            this.lPrenom.Name = "lPrenom";
            this.lPrenom.Size = new System.Drawing.Size(65, 21);
            this.lPrenom.TabIndex = 11;
            this.lPrenom.Text = "Prénom";
            // 
            // tbNom
            // 
            this.tbNom.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbNom.Location = new System.Drawing.Point(343, 100);
            this.tbNom.Name = "tbNom";
            this.tbNom.Size = new System.Drawing.Size(235, 25);
            this.tbNom.TabIndex = 5;
            this.tbNom.TextChanged += new System.EventHandler(this.tbCours_TextChanged);
            // 
            // bmajCours
            // 
            this.bmajCours.AutoSize = true;
            this.bmajCours.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.bmajCours.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bmajCours.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.bmajCours.Location = new System.Drawing.Point(352, 205);
            this.bmajCours.Name = "bmajCours";
            this.bmajCours.Size = new System.Drawing.Size(147, 33);
            this.bmajCours.TabIndex = 8;
            this.bmajCours.Text = "&Mettre à jour";
            this.bmajCours.UseVisualStyleBackColor = false;
            this.bmajCours.Click += new System.EventHandler(this.bmajCours_Click);
            // 
            // lNom
            // 
            this.lNom.AutoSize = true;
            this.lNom.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lNom.Location = new System.Drawing.Point(260, 103);
            this.lNom.Name = "lNom";
            this.lNom.Size = new System.Drawing.Size(45, 21);
            this.lNom.TabIndex = 9;
            this.lNom.Text = "Nom";
            // 
            // tbIntitule
            // 
            this.tbIntitule.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbIntitule.Location = new System.Drawing.Point(343, 65);
            this.tbIntitule.Name = "tbIntitule";
            this.tbIntitule.Size = new System.Drawing.Size(235, 25);
            this.tbIntitule.TabIndex = 4;
            this.tbIntitule.TextChanged += new System.EventHandler(this.tbCours_TextChanged);
            // 
            // lIntitule
            // 
            this.lIntitule.AutoSize = true;
            this.lIntitule.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lIntitule.Location = new System.Drawing.Point(260, 68);
            this.lIntitule.Name = "lIntitule";
            this.lIntitule.Size = new System.Drawing.Size(58, 21);
            this.lIntitule.TabIndex = 7;
            this.lIntitule.Text = "Intitulé";
            // 
            // tbNumero
            // 
            this.tbNumero.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.tbNumero.Location = new System.Drawing.Point(343, 30);
            this.tbNumero.Name = "tbNumero";
            this.tbNumero.Size = new System.Drawing.Size(235, 25);
            this.tbNumero.TabIndex = 3;
            this.tbNumero.TextChanged += new System.EventHandler(this.tbCours_TextChanged);
            // 
            // lNumero
            // 
            this.lNumero.AutoSize = true;
            this.lNumero.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lNumero.Location = new System.Drawing.Point(260, 33);
            this.lNumero.Name = "lNumero";
            this.lNumero.Size = new System.Drawing.Size(68, 21);
            this.lNumero.TabIndex = 3;
            this.lNumero.Text = "Numéro";
            this.lNumero.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCours
            // 
            this.lbCours.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbCours.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbCours.FormattingEnabled = true;
            this.lbCours.HorizontalScrollbar = true;
            this.lbCours.ItemHeight = 17;
            this.lbCours.Location = new System.Drawing.Point(3, 26);
            this.lbCours.Name = "lbCours";
            this.lbCours.Size = new System.Drawing.Size(251, 293);
            this.lbCours.TabIndex = 2;
            this.lbCours.Click += new System.EventHandler(this.lbCours_Click);
            this.lbCours.SelectedIndexChanged += new System.EventHandler(this.lbCours_Click);
            // 
            // lCours
            // 
            this.lCours.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lCours.Location = new System.Drawing.Point(0, 0);
            this.lCours.Name = "lCours";
            this.lCours.Size = new System.Drawing.Size(581, 23);
            this.lCours.TabIndex = 2;
            this.lCours.Text = "Cours";
            this.lCours.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.BackColor = System.Drawing.Color.Gainsboro;
            this.panel3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel3.Controls.Add(this.lbSeances);
            this.panel3.Controls.Add(this.lSeances);
            this.panel3.Location = new System.Drawing.Point(843, 27);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(0, 0, 0, 2);
            this.panel3.Size = new System.Drawing.Size(229, 333);
            this.panel3.TabIndex = 6;
            // 
            // lbSeances
            // 
            this.lbSeances.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbSeances.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbSeances.FormattingEnabled = true;
            this.lbSeances.ItemHeight = 17;
            this.lbSeances.Location = new System.Drawing.Point(3, 30);
            this.lbSeances.Name = "lbSeances";
            this.lbSeances.Size = new System.Drawing.Size(223, 293);
            this.lbSeances.TabIndex = 11;
            // 
            // lSeances
            // 
            this.lSeances.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lSeances.Location = new System.Drawing.Point(0, 0);
            this.lSeances.Name = "lSeances";
            this.lSeances.Size = new System.Drawing.Size(226, 23);
            this.lSeances.TabIndex = 2;
            this.lSeances.Text = "Séances";
            this.lSeances.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1084, 366);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.msPrincipal);
            this.MaximumSize = new System.Drawing.Size(1100, 405);
            this.MinimumSize = new System.Drawing.Size(1100, 405);
            this.Name = "fPrincipal";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "fPrincipal";
            this.Load += new System.EventHandler(this.fPrincipal_Load);
            this.msPrincipal.ResumeLayout(false);
            this.msPrincipal.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MenuStrip msPrincipal;
        private ToolStripMenuItem tsmiFichier;
        private ToolStripMenuItem tsmiPreferences;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmiQuitter;
        private ListBox lbCohortes;
        private Label lCohortes;
        private Panel panel1;
        private Panel panel2;
        private ListBox lbCours;
        private Label lCours;
        private Panel panel3;
        private ListBox lbSeances;
        private Label lSeances;
        private Label lNumero;
        private TextBox tbNumero;
        private TextBox tbIntitule;
        private Label lIntitule;
        private TextBox tbPrenom;
        private Label lPrenom;
        private TextBox tbNom;
        private Label lNom;
        private Button bmajCours;
        private Label lInformation;
        private TextBox tbCategorie;
        private Label lCategorie;
        private Button bExportTousLesCours;
        private Button bExportCours;
    }
}