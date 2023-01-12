using CalendrierCours.BL;
using CalendrierCours.DAL.SiteInternet;
using CalendrierCours.Entites;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;

namespace CalendrierCours.WinFormUI
{
    public class Application
    {
        private IProprietes m_proprietes;
        private IDepotCours m_depotCours;
        private Traitement m_traitement;
        private List<CohorteViewModelConsole> m_cohortes;

        public Application(IProprietes p_proprietes, IDepotCours p_depot, Traitement p_traitement, List<Cohorte> p_cohortes)
        {
            if (p_proprietes is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_proprietes));
            }
            if (p_depot is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_depot));
            }
            if (p_traitement is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_traitement));
            }
            if (p_cohortes is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohortes));
            }

            this.m_proprietes = p_proprietes;
            this.m_depotCours = p_depot;
            this.m_traitement = p_traitement;
            this.m_cohortes = p_cohortes.Select(c => new CohorteViewModelConsole(c)).ToList();
        }




        public void Run()
        {
            fPrincipal fenetre = new fPrincipal();
            
            System.Windows.Forms.Application.Run(fenetre);
        }
    }
}
