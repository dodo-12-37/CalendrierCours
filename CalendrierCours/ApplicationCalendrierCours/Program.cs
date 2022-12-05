

using DepotSiteInternet;
using Entites;

public class Program
{
    private static void Main(string[] args)
    {
        DepotCoursInternet depot = new DepotCoursInternet();

        List<Cohorte> cohortes = depot.RecupererCohortes();

        foreach (var co in cohortes)
        {
            Console.WriteLine(co.Numero);
        }
    }
}