using Entites;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;

namespace DepotSiteInternet
{
    public class DepotCoursInternet
        : IDepotCours
    {
        #region Membres
        private const string URL_SITE_CSFOY = "https://externe5.csfoy.ca/horairecohorte/index.php";

        #endregion

        #region Ctor

        #endregion

        #region Proprietes

        #endregion

        #region Methodes

        public List<Cohorte> RecupererCohortes()
        {
            string? contenuInternet = this.RecupererContenu();

            if (String.IsNullOrEmpty(contenuInternet))
            {
                throw new WebException("Site internet non accessible");
            }

            List<string> lignesContenuInternet = this.CouperLignesTexte(contenuInternet);
            List<Cohorte> listeRetour = new List<Cohorte>();

            listeRetour = lignesContenuInternet
                .Where(str => str.Contains("option value"))
                .Select(str =>
                {
                    string[] valeurs = str.Split("\"");
                    return valeurs[1];
                })
                .Where(str => str.Length > 0)
                .Select(l => new CohorteInternetDTO(l).VersEntite())
                .ToList();

            return listeRetour;
        }
        public List<Cours> RecupererCours(Cohorte p_cohorte)
        {
            string url = URL_SITE_CSFOY + "?cohorte=" + p_cohorte.Numero;
            Regex regexHeures = new Regex("[0-9]{1,2}:[0-9]{2}");
            Regex regexSemaines = new Regex("[0-9]{2}-[0-9]{2}-[0-9]{2}");
            Regex regexJours = new Regex("([0-9]{2,3}px)+.*(ligne1|vide){1}");


            string? contenuInternet = RecupererContenu(url);

            if (String.IsNullOrEmpty(contenuInternet))
            {
                throw new WebException("Site internet non accessible");
            }

            List<string> lignesContenuInternet = CouperLignesTexte(contenuInternet);
            List<Cours> listeRetour = new List<Cours>();
            lignesContenuInternet = lignesContenuInternet
                .Select(l => l.Trim())
                .Where(l => regexJours.IsMatch(l) || regexSemaines.IsMatch(l) || regexHeures.IsMatch(l))
                .ToList();

            return TransformerLignesEnCours(lignesContenuInternet);
        }
        private string? RecupererContenu(string? p_url = null)
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
        private List<string> CouperLignesTexte(string p_texte)
        {
            if (p_texte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_texte));
            }

            return p_texte.Split("\r\n").ToList();
        }
        private List<Cours> TransformerLignesEnCours(List<string> p_liste)
        {
            Regex regexHeures = new Regex("[0-9]{1,2}:[0-9]{2}");
            Regex regexSemaines = new Regex("(?<semaine>[0-9]{2}-[0-9]{2}-[0-9]{2})");
            Regex regexCours = new Regex("ligne1");

            int compteur = 0;
            DateTime premierJourSemaine;
            List<DateTime> horaires = new List<DateTime>();

            foreach (string l in p_liste)
            {
                Match matchSemaine = regexSemaines.Match(l);

                if (matchSemaine.Success)
                {
                    string res = matchSemaine.Groups["semaine"].Value;
                    Console.WriteLine(res);
                }




            }


            throw new NotImplementedException();
        }

        #endregion

    }
}