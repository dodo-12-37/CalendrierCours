using CalendrierCours.BL;
using CalendrierCours.ConsoleUI;
using CalendrierCours.DAL.SiteInternet;
using CalendrierCours.Entites;
using System.ComponentModel;
using System.Reflection;

public class Program
{
    private static void Main(string[] args)
    {
        UtilitaireDepotCours utilitaireDepot = new UtilitaireDepotCours();
        UtilitaireExportCours utilitaireExport = new UtilitaireExportCours();
 
        try
        {
            DepotSiteInternet depot = utilitaireDepot.CreerDepot() as DepotSiteInternet;
            Traitement traitement = new Traitement(depot);
            Application application = new Application(traitement);
            application.Run();
        }
        catch (InvalidDepotException)
        {
            Console.Error.WriteLine("Erreur du fichier de configuration du dépot !");
        }
        catch(Exception) 
        {
            Console.Error.WriteLine("Erreur dans l'initialisation du programme !");
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



