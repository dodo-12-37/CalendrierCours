using System.Globalization;

namespace CalendrierCours.Entites
{
    public interface IExportFichier
    {
        void ExporterVersFichier(List<Cours> p_cours, string p_chemin);
    }

    public interface IDepotCours
    {
        List<Cohorte> RecupererCohortes();
        List<Cours> RecupererCours(Cohorte p_cohorte);
    }

    public interface IProprietes
    {
        string this[string p_nomPropriete] {get; }
    }
}
