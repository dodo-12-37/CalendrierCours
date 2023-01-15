using CalendrierCours.Entites;

namespace CalendrierCours.WinFormUI
{
    public partial class fPrincipal : Form
    {
        private Application m_appli;
        private IProprietes m_proprietes;
        private Dictionary<int, CohorteViewModelWinForm> m_cohortes;
        private Dictionary<int, CoursViewModelWinForm> m_coursCohorteCourante;
        private CoursViewModelWinForm m_coursCourant;

        private Color m_couleurMajOk = Color.LimeGreen;
        private Color m_couleurMajAValider = Color.Tomato;

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

            m_appli = p_appli;
            m_proprietes = p_proprietes;
            m_coursCohorteCourante = new Dictionary<int, CoursViewModelWinForm>();
            m_cohortes = new Dictionary<int, CohorteViewModelWinForm>(p_cohortes.Count);
            m_coursCourant = new CoursViewModelWinForm();
            int compt = 0;
            p_cohortes.ForEach(c => m_cohortes.Add(compt++, c));

            InitializeComponent();
        }

        private void fPrincipal_Load(object sender, EventArgs e)
        {
            lInformation.Text = MessageInformation;

            foreach (KeyValuePair<int, CohorteViewModelWinForm> cohorte in m_cohortes)
            {
                lbCohortes.Items.Add(cohorte.Value.ToString());
            }
        }
        private void tsmiQuitter_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void lbCohortes_Click(object sender, EventArgs e)
        {
            CohorteViewModelWinForm cohorte;

            try
            {
                cohorte = m_cohortes[lbCohortes.SelectedIndex];

            }
            catch (Exception)
            {
                cohorte = null;
            }

            if (cohorte is not null)
            {
                cohorte.ListeCours = m_appli.RecupererCours(cohorte);

                if (!MessageInformation.ToLower().Contains("erreur"))
                {
                    MiseAJoursCoursCourant(cohorte);
                    MiseAjourAffichageCours();
                    MessageInformation = "Récupération des cours réussie !";
                }
            }

            lInformation.Text = MessageInformation;
        }
        private void lbCours_Click(object sender, EventArgs e)
        {
            CoursViewModelWinForm cours;

            try
            {
                cours = m_coursCohorteCourante[lbCours.SelectedIndex];
                m_coursCourant = cours;
            }
            catch (Exception)
            {
                cours = m_coursCourant;
            }

            if (cours is not null)
            {
                MiseAJourAfficheInformationCours(cours);
                MiseAjourAffichageSeances(cours);
            }
        }
        private void tbCours_TextChanged(object sender, EventArgs e)
        {
            ProfesseurViewModelWinForm enseignantModifie =
                new ProfesseurViewModelWinForm(tbNom.Text, tbPrenom.Text);
            CoursViewModelWinForm coursModifie =
                new CoursViewModelWinForm(enseignantModifie, tbNumero.Text, tbIntitule.Text);
            coursModifie.Categorie = tbCategorie.Text;

            if (!m_coursCourant.Equals(coursModifie) || m_coursCourant.Categorie != coursModifie.Categorie)
            {
                bmajCours.BackColor = m_couleurMajAValider;
            }
            else
            {
                bmajCours.BackColor = m_couleurMajOk;
            }
        }
        private void bmajCours_Click(object sender, EventArgs e)
        {
            m_coursCourant.Numero = tbNumero.Text;
            m_coursCourant.Intitule = tbIntitule.Text;
            m_coursCourant.Categorie = tbCategorie.Text;
            m_coursCourant.Enseignant.Nom = tbNom.Text;
            m_coursCourant.Enseignant.Prenom = tbPrenom.Text;

            bmajCours.BackColor = m_couleurMajOk;
        }
        private void bExportCours_Click(object sender, EventArgs e)
        {
            List<CoursViewModelWinForm> cours = new List<CoursViewModelWinForm>() { m_coursCourant };
            ExporterListeCours(cours);
        }
        private void bExportTousLesCours_Click(object sender, EventArgs e)
        {
            List<CoursViewModelWinForm> coursExport = new List<CoursViewModelWinForm>(m_coursCohorteCourante.Count);

            foreach (KeyValuePair<int, CoursViewModelWinForm> cours in m_coursCohorteCourante)
            {
                coursExport.Add(cours.Value);
            }

            ExporterListeCours(coursExport);
        }

        private void MiseAJoursCoursCourant(CohorteViewModelWinForm p_cohorte)
        {
            int cmpt = 0;
            m_coursCohorteCourante.Clear();

            p_cohorte.ListeCours.ForEach(c => m_coursCohorteCourante.Add(cmpt++, c));
        }
        private void MiseAjourAffichageCours()
        {
            lbCours.Items.Clear();

            foreach (KeyValuePair<int, CoursViewModelWinForm> cours in m_coursCohorteCourante)
            {
                lbCours.Items.Add(cours.Value.ToString());
            }
        }
        private void MiseAJourAfficheInformationCours(CoursViewModelWinForm p_cours)
        {
            tbNumero.Text = p_cours.Numero;
            tbIntitule.Text = p_cours.Intitule;
            tbNom.Text = p_cours.Enseignant.Nom;
            tbPrenom.Text = p_cours.Enseignant.Prenom;
            tbCategorie.Text = p_cours.Categorie;
        }
        private void MiseAjourAffichageSeances(CoursViewModelWinForm p_cours)
        {
            lbSeances.Items.Clear();
            p_cours.Seances.ForEach(s => lbSeances.Items.Add(s.ToString()));
        }

        private DialogResult RecupererCheminExport(out string chemin)
        {
            chemin = String.Empty;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Fichier ICalendar (*.ics)|*.ics";
            sfd.FileName = "Cours";

            DialogResult dr = sfd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                chemin = sfd.FileName;
            }

            return dr;
        }
        private void ExporterListeCours(List<CoursViewModelWinForm> p_cours)
        {
            string chemin;
            bool estExporte;

            if (RecupererCheminExport(out chemin) != DialogResult.Cancel)
            {
                estExporte = m_appli.ExporterCours(p_cours, chemin);

                if (estExporte)
                {
                    lInformation.Text = "Exportation réussie !";
                }
                else
                {
                    lInformation.Text = "Erreur dans l'exportation !";
                }
            }
            else
            {
                lInformation.Text = "Exportation annulée !";
            }
        }
    }
}
