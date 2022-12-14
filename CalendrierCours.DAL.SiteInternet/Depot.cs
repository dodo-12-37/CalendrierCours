using System.Net;
using System.Text.RegularExpressions;
using CalendrierCours.Entites;
using Microsoft.Extensions.Configuration;

namespace CalendrierCours.DAL.SiteInternet
{
    public class DepotSiteInternet
        : IDepotCours
    {
        #region Membres
        private string m_urlSiteCsfoy;
        private string m_urlSiteCsfoyCohorte;
        #endregion

        #region Ctor
        public DepotSiteInternet()
        {
            this.LireFichierConfig();
        }
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
            string url = m_urlSiteCsfoyCohorte + p_cohorte.Numero;
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

            try
            {
                listeRetour = this.TransformerLignesEnCoursInternetDTO(lignesContenuInternet)
                    .Select(cDTO => cDTO.VersEntites())
                    .ToList();
            }
            catch (Exception e)
            {
                throw new InvalidDepotException("Erreur dans l'interprétation du contenu du site", e);
            }

            return listeRetour;
        }
        private string? RecupererContenu(string? p_url = null)
        {
            string? retour = null;
            string url = "";

            if (p_url is null)
            {
                url = m_urlSiteCsfoy;
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
        private List<CoursInternetDTO> TransformerLignesEnCoursInternetDTO(List<string> p_liste)
        {
            List<CoursInternetDTO> listeRetour = new List<CoursInternetDTO>();

            Regex regexHeures = new Regex("(?<heure>[0-9]{1,2}):(?<minute>[0-9]{2})");
            Regex regexSemaines = new Regex("(?<semaine>[0-9]{4}-[0-9]{2}-[0-9]{2})");
            Regex regexCours = new Regex("([0-9]{2,3}px)+.*ligne1");
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

                    CoursInternetDTO nvCours = this.TransformerLigneNouvelleSeance(l, dateDebut, dateFin);
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
                        .Select(h => h.AddDays(1))
                        .ToList();
                    horaires = nvHoraires;
                    compteurHeure = horaires.Count;
                }
            }

            return listeRetour;
        }
        private CoursInternetDTO TransformerLigneNouvelleSeance(string p_ligne, DateTime p_dateDebut, DateTime p_dateFin)
        {
            Regex regexSeance = new Regex("(?<infos>[A-Z]{1}.+)");
            Regex regexNumeroCours = new Regex("(?<cours>[0-9]{3}-[A-Z]{1}[0-9]{2}-SF)");
            int positionIntitule = 0, positionNumero = 1, positionProf = 2, positionSalle = 3;
            int PositionNomProf = 0, positionPrenomProf = 1;

            string info = regexSeance.Match(p_ligne).Groups["infos"].Value;
            string[] infos = info.Split("<br>");
            infos[positionSalle] = infos[positionSalle].Replace("</td></tr>", "");
            string nomCours = regexNumeroCours.Match(infos[positionNumero]).Groups["cours"].Value + " - " + infos[positionIntitule];
            string[] professeur = infos[positionProf].Split(", ");

            ProfesseurInternetDTO nvProf = new ProfesseurInternetDTO(professeur[PositionNomProf], professeur[positionPrenomProf]);
            CoursInternetDTO nvCours = new CoursInternetDTO(nvProf, nomCours);
            SeanceInternetDTO nvSeance = new SeanceInternetDTO(p_dateDebut, p_dateFin, infos[positionSalle]);
            nvCours.Seances.Add(nvSeance);

            return nvCours;
        }
        private void LireFichierConfig()
        {
            try
            {
                IConfigurationRoot configuration =
                    new ConfigurationBuilder()
                      .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                      .AddJsonFile("appsettings.json", false)
                      .Build();
                
                this.m_urlSiteCsfoy = configuration["UrlSiteInternet"];
                this.m_urlSiteCsfoyCohorte = configuration["UrlAvecCohorte"];
            }
            catch (Exception e)
            {
                throw new InvalidDepotException("Le fichier de configuration est corrompu", e);
            }

        }
        #endregion

    }
}