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
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            fChargement chargement = new fChargement();

            System.Windows.Forms.Application.OpenForms.AsParallel();
            System.Windows.Forms.Application.Run(chargement);

            if (chargement.EstCharge)
            {
                Application application = new Application(chargement.Proprietes, chargement.DepotCours, chargement.Traitement, chargement.Cohortes);
                application.Run();
            }
        }
    }
}