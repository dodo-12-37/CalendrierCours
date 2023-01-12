namespace CalendrierCours.WinFormUI
{
    partial class fChargement
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lChargement = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lChargement
            // 
            this.lChargement.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lChargement.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lChargement.Location = new System.Drawing.Point(12, 22);
            this.lChargement.Name = "lChargement";
            this.lChargement.Size = new System.Drawing.Size(388, 22);
            this.lChargement.TabIndex = 0;
            this.lChargement.Text = "Chargement des propriétés...";
            this.lChargement.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // fChargement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 62);
            this.Controls.Add(this.lChargement);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "fChargement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CalendrierCours";
            this.Load += new System.EventHandler(this.fChargement_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Label lChargement;
    }
}