

using DepotSiteInternet;
using Entites;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

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

        Cohorte cohorte = new Cohorte("LEAD4_H22_4394");

        depot.RecupererCours(cohorte);
    }

    

}