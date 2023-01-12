using CalendrierCours.BL;
using CalendrierCours.DAL.SiteInternet;
using CalendrierCours.Entites;

namespace CalendrierCours.WinFormUI
{
    public partial class fChargement : Form
    {
        public IProprietes Proprietes {get; set;}
        public IDepotCours DepotCours {get; set;}
        public Traitement Traitement {get; set;}
        public List<Cohorte> Cohortes {get; set;}

        public bool EstCharge { get; private set; }

        public fChargement()
        {
            InitializeComponent();
        }

        public void ModifierLabelChargement(string p_text)
        {
            this.lChargement.Text = p_text;
        }
        
        private void fChargement_Load(object sender, EventArgs e)
        {
            this.ModifierLabelChargement("Chargement des propriétés...");
            bool chargerSuivant;
            this.KeyPress -= new System.Windows.Forms.KeyPressEventHandler(this.fChargement_KeyPress);

            this.EstCharge = this.ChargerProprietes();
            chargerSuivant = this.EstCharge;
            if (!this.EstCharge)
            {
                this.ModifierLabelChargement("Erreur dans le chargement des propriétés ! Appuyez sur une touche...");
            }

            if (chargerSuivant)
            {
                this.ModifierLabelChargement("Chargement du dépôt des cours...");
                this.EstCharge = this.EstCharge && this.ChargerDepotCours();
                chargerSuivant = this.EstCharge;
                if (!this.EstCharge)
                {
                    this.ModifierLabelChargement("Erreur dans le chargement du dépôt des cours ! Appuyez sur une touche...");
                }
            }

            if (chargerSuivant)
            {
                this.ModifierLabelChargement("Chargement du traitement...");
                this.EstCharge = this.EstCharge && this.ChargerTraitement();
                chargerSuivant = this.EstCharge;
                if (!this.EstCharge)
                {  
                    this.ModifierLabelChargement("Erreur dans le chargement du traitement ! Appuyez sur une touche...");
                }
            }

            if (chargerSuivant)
            {
                this.ModifierLabelChargement("Chargement des cohortes...");
                this.EstCharge = this.EstCharge && this.ChargerCohortes();
                if (!this.EstCharge)
                {
                    this.ModifierLabelChargement("Erreur dans le chargement des cours ! Appuyez sur une touche...");
                }
            }

            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.fChargement_KeyPress);

            if (this.EstCharge)
            {
                this.ModifierLabelChargement("Chargement réussi ! Appuyez sur une touche...");
            }
        }
        private IProprietes RetournerProprietes()
        {
            return new SinglotonProprietes();
        }
        private bool ChargerProprietes()
        {
            bool estValide;
            try
            {
                this.Proprietes = this.RetournerProprietes();
                estValide = true;
            }
            catch (Exception)
            {
                estValide = false;
            }
            return estValide;
        }

        private IDepotCours RetournerDepotCours()
        {
            return new DepotSiteInternet(this.Proprietes);
        }
        private bool ChargerDepotCours()
        {
            bool estValide;

            try
            {
                this.DepotCours = this.RetournerDepotCours();
                estValide = true;
            }
            catch (Exception)
            {
                estValide = false;
            }

            return estValide;
        }

        private bool ChargerCohortes()
        {
            bool estValide = false;

            try
            {
                this.Cohortes = this.DepotCours.RecupererCohortes();
                estValide = true;
            }
            catch (Exception)
            {
                estValide = false;
                this.Cohortes = new List<Cohorte>();
            }

            if (estValide && bool.Parse(this.Proprietes["chargerCoursDemarrage"]))
            {
                try
                {
                    this.Cohortes.ForEach(c => this.DepotCours.RecupererCours(c));
                    estValide = true;
                }
                catch (Exception)
                {
                    estValide = false;
                }
            }
                
            return estValide;
        }

        private Traitement RetournerTraitement()
        {
            return new Traitement(this.DepotCours);
        }
        private bool ChargerTraitement()
        {
            bool estValide;

            try
            {
                this.Traitement = this.RetournerTraitement();
                estValide = true;
            }
            catch (Exception)
            {
                estValide = false;
            }
            return estValide;
        }

        private void fChargement_KeyPress(object sender, KeyPressEventArgs e)
        {
            this.Close();
        }
    }
}