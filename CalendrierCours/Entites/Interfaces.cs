namespace Entites
{
    public interface IInterfaceUtilisateur
    {
        void AfficherCohorte();
        void AfficherCours(Cohorte p_cohorte);
        void AfficherSeances(Cohorte p_cohorte, DateTime p_date);
    }

    public interface IExportFichier
    {
        void ExporterVersFichier(string p_chemin);
    }

    public interface IDepotCours 
    {
        List<Cohorte> RecupererCohortes();
        List<Cours> RecupererCours(Cohorte p_cohorte);
    }

}
