using CalendrierCours.BL;
using CalendrierCours.ConsoleUI;
using CalendrierCours.DAL.SiteInternet;
using CalendrierCours.Entites;
using System.ComponentModel;
using System.Net;
using System.Reflection;

public class Program
{
    private static void Main(string[] args)
    {
        UtilitaireDepotCours utilitaireDepot = new UtilitaireDepotCours();
        UtilitaireExportCours utilitaireExport = new UtilitaireExportCours();

        Application application = null;
        bool estInitialise = false;

        try
        {
            Console.Out.WriteLine("Initialisation du dépot...");
            DepotSiteInternet depot = utilitaireDepot.CreerDepot() as DepotSiteInternet;
            Console.Out.WriteLine("Initialisation du traitement...");
            Traitement traitement = new Traitement(depot);
            Console.Out.WriteLine("Initialisation de l'application...");
            application = new Application(traitement);
            Console.Out.WriteLine("Initialisation terminée avec succès !");
            estInitialise = true;
        }
        catch (WebException)
        {
            Console.Error.WriteLine("Erreur de connexion internet !");
        }
        catch (InvalidDepotException)
        {
            Console.Error.WriteLine("Erreur du fichier de configuration du dépot !");
        }
        catch (Exception)
        {
            Console.Error.WriteLine("Erreur dans l'initialisation du programme !");
        }

        if (estInitialise)
        {
            Console.Clear();
            application.Run();
        }
    }

}

public class UtilitaireDepotCours
{
    public static IEnumerable<CreateurDepot> RechercherDepotCours()
    {
        Type typesDepots = typeof(IDepotCours);

        Console.WriteLine(AppDomain.CurrentDomain.GetAssemblies());

        List<Type> traitements = AppDomain.CurrentDomain.GetAssemblies()
            .ToList()
            .SelectMany(e => e.GetTypes())
            .Where(t => typesDepots.IsAssignableFrom(t)
                && t.GetCustomAttribute<DescriptionAttribute>() != null)
            .ToList();


        return traitements.Select(t => new CreateurDepot() { Type = t });
    }

    public IDepotCours CreerDepot()
    {
        return new DepotSiteInternet();
    }
}
public class CreateurDepot
{
    public Type Type { get; set; }
    public IDepotCours Creer()
    {
        return (IDepotCours)Activator.CreateInstance(this.Type);
    }
}
public class UtilitaireExportCours
{
    public static IEnumerable<CreateurExport> RechercherDepotCours()
    {
        Type typesDepots = typeof(IExportFichier);

        Console.WriteLine(AppDomain.CurrentDomain.GetAssemblies());

        List<Type> traitements = AppDomain.CurrentDomain.GetAssemblies()
            .ToList()
            .SelectMany(e => e.GetTypes())
            .Where(t => typesDepots.IsAssignableFrom(t)
                && t.GetCustomAttribute<DescriptionAttribute>() != null)
            .ToList();


        return traitements.Select(t => new CreateurExport() { Type = t });
    }

    public IDepotCours CreerDepot()
    {
        throw new NotImplementedException();
        return new DepotSiteInternet();
    }
}
public class CreateurExport
{
    public Type Type { get; set; }
    public IExportFichier Creer()
    {
        return (IExportFichier)Activator.CreateInstance(this.Type);
    }
}



