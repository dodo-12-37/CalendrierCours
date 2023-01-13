using CalendrierCours.BL;
using CalendrierCours.DAL.SiteInternet;
using CalendrierCours.Entites;

namespace CalendrierCours.WinFormUI
{
    public partial class fErreur : Form
    {

        public fErreur(string p_message)
        {
            if (String.IsNullOrEmpty(p_message))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_message));
            }
            InitializeComponent();
            this.lMessage.Text = p_message;
        }

        private void fChargement_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Close();
        }
    }
}