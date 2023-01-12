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
        private IProprietes? m_proprietes;
        private IDepotCours? m_depotCours;
        private Traitement? m_traitement;
        private List<CohorteViewModelConsole> m_cohortes;

        public Application(IProprietes? p_proprietes, IDepotCours? p_depot, Traitement? p_traitement, List<Cohorte>? p_cohortes)
        {
            this.m_proprietes = p_proprietes;
            this.m_depotCours = p_depot;
            this.m_traitement = p_traitement;
            if (p_cohortes is not null)
            {
                this.m_cohortes = p_cohortes.Select(c => new CohorteViewModelConsole(c)).ToList();
            }
            else
            {
                this.m_cohortes = new List<CohorteViewModelConsole>();
            }
        }




        public void Run()
        {
            fPrincipal fenetre = new fPrincipal();

            if (this.m_proprietes is null)
            {
                fenetre.MessageInformation = "Erreur dans le chargement des propriétés !";
            }
            else if (this.m_depotCours is null)
            {
                fenetre.MessageInformation = "Erreur dans le chargement du dépôt des cours !";
            }
            else if (this.m_traitement is null)
            {
                fenetre.MessageInformation = "Erreur dans le chargement du traitement !";
            }
            else if (this.m_cohortes.Count == 0)
            {
                fenetre.MessageInformation = "Erreur dans le chargement des cours !";
            }
            else
            {
                fenetre.MessageInformation = "Bienvenue !";
            }

            System.Windows.Forms.Application.Run(fenetre);
        }
    }
}
