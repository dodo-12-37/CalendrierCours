using CalendrierCours.BL;
using CalendrierCours.DAL.ExportCoursICS;
using CalendrierCours.Entites;

namespace CalendrierCours.WinFormUI
{
    public class Application
    {
        private IProprietes? m_proprietes;
        private Traitement? m_traitement;
        private List<CohorteViewModelWinForm> m_cohortes;
        private List<CohorteViewModelWinForm> m_cohortesCoursCharges;
        private bool m_coursCharges;

        private fPrincipal m_fenetrePrincipale;

        public Application(IProprietes? p_proprietes, Traitement? p_traitement, List<Cohorte>? p_cohortes)
        {
            if (p_proprietes is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_proprietes));
            }
            if (p_traitement is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_traitement));
            }
            if (p_cohortes is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohortes));
            }

            m_proprietes = p_proprietes;
            m_traitement = p_traitement;
            m_coursCharges = bool.Parse(p_proprietes["chargerCoursDemarrage"]);
            m_cohortes = new List<CohorteViewModelWinForm>();
            m_cohortesCoursCharges = new List<CohorteViewModelWinForm>();

            if (p_cohortes is not null)
            {
                if (m_coursCharges)
                {
                    p_cohortes.ForEach(c => m_cohortesCoursCharges.Add(new CohorteViewModelWinForm(c)));

                }
                else
                {
                    p_cohortes.ForEach(c => m_cohortes.Add(new CohorteViewModelWinForm(c)));
                }
            }

            m_fenetrePrincipale = new fPrincipal(this, m_proprietes, m_cohortes);
        }

        public void Run(string p_messageChargement)
        {
            if (String.IsNullOrEmpty(p_messageChargement))
            {
                p_messageChargement = "";
            }

            m_fenetrePrincipale.MessageInformation = p_messageChargement;

            System.Windows.Forms.Application.Run(m_fenetrePrincipale);
        }

        public List<CoursViewModelWinForm> RecupererCours(CohorteViewModelWinForm p_cohorte)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }

            List<CoursViewModelWinForm> cours;

            if (!m_coursCharges)
            {
                CohorteViewModelWinForm? cohorte = m_cohortesCoursCharges.SingleOrDefault(c => c.Equals(p_cohorte));

                if (cohorte is null)
                {
                    bool estValide;
                    cohorte = m_cohortes.Single(c => c.Equals(p_cohorte));

                    try
                    {
                        cours = m_traitement.ListerCours(p_cohorte.VersEntite())
                            .Select(c => new CoursViewModelWinForm(c)).ToList();
                        estValide = true;
                    }
                    catch (Exception)
                    {
                        cours = new List<CoursViewModelWinForm>();
                        m_fenetrePrincipale.MessageInformation = "Erreur dans la récupération des cours !";
                        estValide = false;
                    }

                    if (estValide)
                    {
                        cohorte.ListeCours = cours;
                        m_cohortesCoursCharges.Add(cohorte);
                        m_cohortes.Remove(cohorte);
                    }

                }
                else
                {
                    cours = cohorte.ListeCours;
                }

            }
            else
            {
                cours = m_cohortes.Single(c => c.Equals(p_cohorte)).ListeCours;
            }

            return cours;
        }

        public bool ExporterCours(List<CoursViewModelWinForm> p_cours, string p_chemin)
        {
            if (p_cours is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cours));
            }
            if (String.IsNullOrEmpty(p_chemin))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_chemin));
            }

            bool estExporte;
            List<Cours> coursAExporter = p_cours.Select(cDTO => cDTO.VersEntites()).ToList();
            IExportFichier export = RetournerExportFichier();

            try
            {
                m_traitement.ExporterSeances(coursAExporter, export, p_chemin);
                estExporte = true;
            }
            catch (Exception)
            {
                estExporte = false;
            }

            return estExporte;
        }

        private IExportFichier RetournerExportFichier()
        {
            return new ExportCoursICS(m_proprietes);
        }
    }
}
