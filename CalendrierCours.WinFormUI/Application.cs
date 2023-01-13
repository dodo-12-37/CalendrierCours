using CalendrierCours.BL;
using CalendrierCours.DAL.SiteInternet;
using CalendrierCours.Entites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

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
            if (p_cohortes is null || p_cohortes.Count == 0)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohortes));
            }

            this.m_proprietes = p_proprietes;
            this.m_traitement = p_traitement;
            this.m_coursCharges = bool.Parse(p_proprietes["chargerCoursDemarrage"]);
            this.m_cohortes = new List<CohorteViewModelWinForm>();
            this.m_cohortesCoursCharges = new List<CohorteViewModelWinForm>();

            if (p_cohortes is not null)
            {
                if (m_coursCharges)
                {
                    p_cohortes.ForEach(c => this.m_cohortesCoursCharges.Add(new CohorteViewModelWinForm(c)));

                }
                else
                {
                    p_cohortes.ForEach(c => this.m_cohortes.Add(new CohorteViewModelWinForm(c)));
                }
            }

            this.m_fenetrePrincipale = new fPrincipal(this, this.m_proprietes, this.m_cohortes);
        }

        public void Run(string p_messageChargement)
        {
            if (String.IsNullOrEmpty(p_messageChargement))
            {
                p_messageChargement = "";
            }

            this.m_fenetrePrincipale.MessageInformation = p_messageChargement;

            System.Windows.Forms.Application.Run(this.m_fenetrePrincipale);
        }

        public List<CoursViewModelWinForm> RecupererCours(CohorteViewModelWinForm p_cohorte)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }

            List<CoursViewModelWinForm> cours;

            if (!this.m_coursCharges)
            {
                CohorteViewModelWinForm? cohorte = this.m_cohortesCoursCharges.SingleOrDefault(c => c.Equals(p_cohorte));

                if (cohorte is null)
                {
                    bool estValide;
                    cohorte = this.m_cohortes.Single(c => c.Equals(p_cohorte));

                    try
                    {
                        cours = this.m_traitement.ListerCours(p_cohorte.VersEntite())
                            .Select(c => new CoursViewModelWinForm(c)).ToList();
                        estValide = true;
                    }
                    catch (Exception)
                    {
                        cours = new List<CoursViewModelWinForm>();
                        this.m_fenetrePrincipale.MessageInformation = "Erreur dans la récupération des cours !";
                        estValide = false;
                    }

                    if (estValide)
                    {
                        cohorte.ListeCours = cours;
                        this.m_cohortesCoursCharges.Add(cohorte);
                        this.m_cohortes.Remove(cohorte);
                    }

                }
                else
                {
                    cours = cohorte.ListeCours;
                }

            }
            else
            {
                cours = this.m_cohortes.Single(c => c.Equals(p_cohorte)).ListeCours;
            }

            return cours;
        }




    }
}
