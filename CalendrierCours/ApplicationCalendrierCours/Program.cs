

using DepotSiteInternet;
using Entites;
using System.Net;
using System.Text.RegularExpressions;

public class Program
{
    private const string URL_SITE_CSFOY = "https://externe5.csfoy.ca/horairecohorte/index.php";
    private static void Main(string[] args)
    {
        DepotCoursInternet depot = new DepotCoursInternet();

        List<Cohorte> cohortes = depot.RecupererCohortes();

        foreach (var co in cohortes)
        {
            Console.WriteLine(co.Numero);
        }

        Cohorte cohorte = new Cohorte("LEAD4_H22_4394");

        List<string> ls = RecupererCours(cohorte);

        foreach (var l in ls)
        {
            Console.WriteLine(l);
        }
    }

    public static List<string> RecupererCours(Cohorte p_cohorte)
    {
        string url = URL_SITE_CSFOY + "?cohorte=" + p_cohorte.Numero;
        Regex regexHeures = new Regex("[0-9]{1,2}:[0-9]{2}");
        Regex regexSemaines = new Regex("[0-9]{2}-[0-9]{2}-[0-9]{2}");
        Regex regexCours = new Regex("([0-9]{2,3}px)+.*(ligne1|vide){1}");


        string? contenuInternet = RecupererContenu(url);

        if (String.IsNullOrEmpty(contenuInternet))
        {
            throw new WebException("Site internet non accessible");
        }

        List<string> lignesContenuInternet = CouperLignesTexte(contenuInternet);
        List<Cours> listeRetour = new List<Cours>();
        lignesContenuInternet = lignesContenuInternet
            .Select(l => l.Trim())
            .Where(l => regexCours.IsMatch(l) || regexSemaines.IsMatch(l) || regexHeures.IsMatch(l))
            .ToList();

        TransformerLignesEnCours(lignesContenuInternet);


        return lignesContenuInternet;
    }
    private static string? RecupererContenu(string? p_url = null)
    {
        string? retour = null;
        string url = "";

        if (p_url is null)
        {
            url = URL_SITE_CSFOY;
        }
        else
        {
            url = p_url;
        }

        ServicePointManager.Expect100Continue = true;
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

        using (WebClient client = new WebClient())
        {
            retour = client.DownloadString(url);
        }

        return retour;
    }
    private static List<string> CouperLignesTexte(string p_texte)
    {
        if (p_texte is null)
        {
            throw new ArgumentNullException("Ne doit pas etre null", nameof(p_texte));
        }

        return p_texte.Split("\n").ToList();
    }

    private static List<Cours> TransformerLignesEnCours(List<string> p_liste)
    {
        Regex regexHeures = new Regex("(?<heure>[0-9]{1,2}:[0-9]{2})");
        Regex regexSemaines = new Regex("(?<semaine>[0-9]{4}-[0-9]{2}-[0-9]{2})");
        Regex regexCours = new Regex("ligne1");

        int compteurHeure = -1;
        int compteurJours = -1;
        DateTime premierJourSemaine;
        List<string> horaires = new List<string>();

        foreach (string l in p_liste)
        {
            Match matchSemaine = regexSemaines.Match(l);
            Match matchHeures = regexHeures.Match(l);

            if (matchSemaine.Success)
            {
                string res = matchSemaine.Groups["semaine"].Value;
                premierJourSemaine = DateTime.Parse(res);
                compteurHeure = 0;
                compteurJours = 0;
            }
            else if (matchHeures.Success && compteurJours == 0)
            {
                Regex taille = new Regex("(?<taille>[0-9]{2})px");
                horaires.Add(matchHeures.Groups["heure"].Value);
                compteurHeure += int.Parse(taille.Match(l).Groups["taille"].Value);
            }




        }


        throw new NotImplementedException();
    }

}