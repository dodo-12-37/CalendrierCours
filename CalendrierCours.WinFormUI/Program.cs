using CalendrierCours.Entites;
using CalendrierCours.BL;
using CalendrierCours.DAL.SiteInternet;

namespace CalendrierCours.WinFormUI
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            IProprietes? proprietes = null;
            IDepotCours? depotCours = null;
            Traitement? traitement = null;
            List<Cohorte>? cohortes = null;

            bool estCharge;
            bool chargerSuivant;

            //fChargement chargement = new fChargement();

            //System.Windows.Forms.Application.OpenForms.AsParallel();
            //System.Windows.Forms.Application.Run(chargement);

            proprietes = ChargerProprietes();
            estCharge = proprietes is not null;

            if (estCharge)
            {
                depotCours = ChargerDepotCours(proprietes);
                estCharge = depotCours is not null;
            }

            if (estCharge)
            {
                traitement = ChargerTraitement(depotCours);
                estCharge = traitement is not null;
            }

            if (estCharge)
            {
                cohortes = ChargerCohortes(depotCours, proprietes);
            }

            Application application = new Application(proprietes, depotCours, traitement, cohortes);
            application.Run();
        }

        private static IProprietes RetournerProprietes()
        {
            return new SinglotonProprietes();
        }
        private static IProprietes? ChargerProprietes()
        {
            IProprietes retour;

            try
            {
                retour = RetournerProprietes();
            }
            catch (Exception)
            {
                retour = null;
            }
            return retour;
        }

        private static IDepotCours RetournerDepotCours(IProprietes p_proprietes)
        {
            return new DepotSiteInternet(p_proprietes);
        }
        private static IDepotCours? ChargerDepotCours(IProprietes p_proprietes)
        {
            IDepotCours retour;

            try
            {
                retour = RetournerDepotCours(p_proprietes);
            }
            catch (Exception)
            {
                retour = null;
            }

            return retour;
        }

        private static List<Cohorte>? ChargerCohortes(IDepotCours p_depot, IProprietes p_proprietes)
        {
            List<Cohorte> retour;

            try
            {
                retour = p_depot.RecupererCohortes();
            }
            catch (Exception)
            {
                retour = null;
            }

            if (retour is not null && bool.Parse(p_proprietes["chargerCoursDemarrage"]))
            {
                try
                {
                    retour.ForEach(c => p_depot.RecupererCours(c));
                }
                catch (Exception)
                {
                    retour = null;
                }
            }

            return retour;
        }

        private static Traitement RetournerTraitement(IDepotCours p_depot)
        {
            return new Traitement(p_depot);
        }
        private static Traitement? ChargerTraitement(IDepotCours p_depot)
        {
            Traitement retour;

            try
            {
                retour = RetournerTraitement(p_depot);
            }
            catch (Exception)
            {
                retour = null;
            }
            return retour;
        }




    }
}