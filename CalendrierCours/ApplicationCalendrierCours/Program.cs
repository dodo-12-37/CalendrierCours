

using DepotSiteInternet;
using Entites;
using System.Globalization;
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

    private static List<CoursInternetDTO> TransformerLignesEnCours(List<string> p_liste)
    {
        List<CoursInternetDTO> listeRetour = new List<CoursInternetDTO>();

        Regex regexHeures = new Regex("(?<heure>[0-9]{1,2}):(?<minute>[0-9]{2})");
        Regex regexSemaines = new Regex("(?<semaine>[0-9]{4}-[0-9]{2}-[0-9]{2})");
        Regex regexCours = new Regex("ligne1");
        Regex regexVide = new Regex("class=\"vide\"");
        Regex taille = new Regex("(?<taille>[0-9]{2,3})px");

        int compteurHeure = -1;
        int tailleHeure = -1;
        DateTime premierJourSemaine = new DateTime();
        List<DateTime> horaires = new List<DateTime>();

        foreach (string l in p_liste)
        {
            Match matchSemaine = regexSemaines.Match(l);
            Match matchHeures = regexHeures.Match(l);
            Match matchCours = regexCours.Match(l);
            Match matchVide = regexVide.Match(l);

            if (matchSemaine.Success)
            {
                premierJourSemaine = DateTime.Parse(matchSemaine.Groups["semaine"].Value);
                horaires = new List<DateTime>();
                compteurHeure = 0;
                tailleHeure = 0;
            }
            else if (matchHeures.Success)
            {
                compteurHeure++;
                double heure = double.Parse(matchHeures.Groups["heure"].Value);
                double minute = double.Parse(matchHeures.Groups["minute"].Value);
                horaires.Add(premierJourSemaine.AddHours(heure).AddMinutes(minute));
                if (tailleHeure == 0)
                {
                    tailleHeure = int.Parse(taille.Match(l).Groups["taille"].Value);
                }
            }
            else if (matchVide.Success)
            {
                compteurHeure--;
            }
            else if (matchCours.Success)
            {
                DateTime dateDebut = horaires[horaires.Count - compteurHeure];
                int tempsCours = int.Parse(taille.Match(l).Groups["taille"].Value) / tailleHeure;
                DateTime dateFin = horaires[horaires.Count - compteurHeure + tempsCours];

                CoursInternetDTO nvCours = TransformerLigneNouvelleSeance(l, dateDebut, dateFin);

                CoursInternetDTO? coursExistant = listeRetour.SingleOrDefault(c => c.Equals(nvCours));

                if (coursExistant != default)
                {
                    listeRetour.Single(c => c.Equals(nvCours)).Seances.Add(nvCours.Seances.First());
                }
                else
                {
                    listeRetour.Add(nvCours);
                }

                compteurHeure -= tempsCours;
            }

            if ((matchCours.Success || matchVide.Success) && compteurHeure == 0)
            {
                List<DateTime> nvHoraires = horaires
                    .Select(h => 
                        {
                            DateTime dt = h.AddDays(1);
                            return dt;
                        })
                    .ToList();
                horaires = nvHoraires;
                compteurHeure = horaires.Count;
            }

        }

        return listeRetour;
    }

    private static CoursInternetDTO TransformerLigneNouvelleSeance(string p_ligne, DateTime p_dateDebut, DateTime p_dateFin)
    {
        Regex regexSeance = new Regex("(?<infos>[A-Z]{1}.+)");
        Regex regexNumeroCours = new Regex("(?<cours>[0-9]{3}-[A-Z]{1}[0-9]{2}-SF)");
        int positionIntitule = 0, positionNumero = 1, positionProf = 2, positionSalle = 3;
        int PositionNomProf = 0, positionPrenomProf = 1;

        string info = regexSeance.Match(p_ligne).Groups["infos"].Value;

        string[] infos = info.Split("<br>");
        infos[positionSalle] = infos[positionSalle].Replace("</td></tr>", "");

        string nomCours = regexNumeroCours.Match(infos[positionNumero]).Groups["cours"].Value + " " + infos[positionIntitule];
        string[] professeur = infos[positionProf].Split(", ");

        ProfesseurInternetDTO nvProf = new ProfesseurInternetDTO(professeur[PositionNomProf], professeur[positionPrenomProf]);
        CoursInternetDTO nvCours = new CoursInternetDTO(nvProf, nomCours);
        SeanceInternetDTO nvSeance = new SeanceInternetDTO(p_dateDebut, p_dateFin, infos[positionSalle]);
        nvCours.Seances.Add(nvSeance);

        return nvCours;
    }

}