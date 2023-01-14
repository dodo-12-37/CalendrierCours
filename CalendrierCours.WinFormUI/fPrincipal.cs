using CalendrierCours.Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalendrierCours.WinFormUI
{
    public partial class fPrincipal : Form
    {
        private Application m_appli;
        private IProprietes m_proprietes;
        private Dictionary<int, CohorteViewModelWinForm> m_cohortes;
        private Dictionary<int, CoursViewModelWinForm> m_coursCohorteCourante;
        private CoursViewModelWinForm m_coursCourant;

        private System.Drawing.Color m_couleurMajOk = System.Drawing.Color.LimeGreen;
        private System.Drawing.Color m_couleurMajAValider = System.Drawing.Color.Tomato;

        public string MessageInformation { get; set; }

        public fPrincipal(Application p_appli, IProprietes p_proprietes, List<CohorteViewModelWinForm> p_cohortes)
        {
            if (p_appli is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_appli));
            }
            if (p_proprietes is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_proprietes));
            }
            if (p_cohortes is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohortes));
            }

            InitializeComponent();

            this.m_appli = p_appli;
            this.m_proprietes = p_proprietes;
            this.m_coursCohorteCourante = new Dictionary<int, CoursViewModelWinForm>();
            this.m_cohortes = new Dictionary<int, CohorteViewModelWinForm>(p_cohortes.Count);
            this.m_coursCourant = null;
            int compt = 0;
            p_cohortes.ForEach(c => this.m_cohortes.Add(compt++, c));
        }

        private void fPrincipal_Load(object sender, EventArgs e)
        {
            this.lInformation.Text = MessageInformation;

            foreach (KeyValuePair<int, CohorteViewModelWinForm> cohorte in this.m_cohortes)
            {
                this.lbCohortes.Items.Add(cohorte.Value.ToString());
            }
        }
        private void tsmiQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void lbCohortes_Click(object sender, EventArgs e)
        {
            CohorteViewModelWinForm cohorte;

            try
            {
                cohorte = this.m_cohortes[this.lbCohortes.SelectedIndex];

            }
            catch (Exception)
            {
                cohorte = null;
            }

            if (cohorte is not null )
            {
                cohorte.ListeCours = this.m_appli.RecupererCours(cohorte);

                if (!this.MessageInformation.ToLower().Contains("erreur"))
                {
                    this.MiseAJoursCoursCourant(cohorte);
                    this.MiseAjourAffichageCours();
                    this.MessageInformation = "Récupération des cours réussie !";
                }
            }

            this.lInformation.Text = MessageInformation;
        }
        private void lbCours_Click(object sender, EventArgs e)
        {
            CoursViewModelWinForm cours;

            try
            {
                cours = this.m_coursCohorteCourante[this.lbCours.SelectedIndex];
                this.m_coursCourant = cours;
            }
            catch (Exception)
            {
                cours = this.m_coursCourant;
            }

            if (cours is not null)
            {
                this.MiseAJourAfficheInformationCours(cours);
                this.MiseAjourAffichageSeances(cours);
            }
        }
        private void tbCours_TextChanged(object sender, EventArgs e)
        {
            ProfesseurViewModelWinForm enseignantModifie = 
                new ProfesseurViewModelWinForm(this.tbNom.Text, this.tbPrenom.Text);
            CoursViewModelWinForm coursModifie =
                new CoursViewModelWinForm(enseignantModifie, this.tbNumero.Text, this.tbIntitule.Text);
            coursModifie.Categorie = this.tbCategorie.Text;

            if (!this.m_coursCourant.Equals(coursModifie) || this.m_coursCourant.Categorie != coursModifie.Categorie)
            {
                this.bmajCours.BackColor = this.m_couleurMajAValider;
            }
            else
            {
                this.bmajCours.BackColor = this.m_couleurMajOk;
            }
        }
        private void bmajCours_Click(object sender, EventArgs e)
        {
            this.m_coursCourant.Numero = this.tbNumero.Text;
            this.m_coursCourant.Intitule= this.tbIntitule.Text;
            this.m_coursCourant.Categorie= this.tbCategorie.Text;
            this.m_coursCourant.Enseignant.Nom = this.tbNom.Text;
            this.m_coursCourant.Enseignant.Prenom = this.tbPrenom.Text;

            this.bmajCours.BackColor = this.m_couleurMajOk;
        }
        private void bExportCours_Click(object sender, EventArgs e)
        {
            string chemin;
            bool estExporte;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Fichier ICalendar (*.ics)|*.ics";
            sfd.FileName = "Cours";

            DialogResult dr = sfd.ShowDialog();

            if (dr != DialogResult.Cancel)
            {
                chemin = sfd.FileName;
                estExporte = this.m_appli.ExporterCours(new List<CoursViewModelWinForm>() { this.m_coursCourant }, chemin);
            
                if (estExporte)
                {
                    this.lInformation.Text = "Exportation réussie !";
                }
                else
                {
                    this.lInformation.Text = "Erreur dans l'exportation !";
                }
            }
            else
            {
                this.lInformation.Text = "Exportation annulée !";
            }
        }
        private void MiseAJoursCoursCourant(CohorteViewModelWinForm p_cohorte)
        {
            int cmpt = 0;
            this.m_coursCohorteCourante.Clear();

            p_cohorte.ListeCours.ForEach(c => this.m_coursCohorteCourante.Add(cmpt++, c));
        }
        private void MiseAjourAffichageCours()
        {
            this.lbCours.Items.Clear();

            foreach (KeyValuePair<int, CoursViewModelWinForm> cours in this.m_coursCohorteCourante)
            {
                this.lbCours.Items.Add(cours.Value.ToString());
            }
        }
        private void MiseAJourAfficheInformationCours(CoursViewModelWinForm p_cours)
        {
            this.tbNumero.Text = p_cours.Numero;
            this.tbIntitule.Text = p_cours.Intitule;
            this.tbNom.Text = p_cours.Enseignant.Nom;
            this.tbPrenom.Text = p_cours.Enseignant.Prenom;
            this.tbCategorie.Text = p_cours.Categorie;
        }
        private void MiseAjourAffichageSeances(CoursViewModelWinForm p_cours)
        {
            this.lbSeances.Items.Clear();
            p_cours.Seances.ForEach(s => this.lbSeances.Items.Add(s.ToString()));
        }
        
    }
}
