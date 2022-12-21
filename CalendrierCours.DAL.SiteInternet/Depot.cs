using System;
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
            IConfigurationRoot configuration = this.LireFichierConfig();

            this.m_urlSiteCsfoy = this.AffecterStringDepuisFichierConfig("urlSiteInternet");
            this.m_urlSiteCsfoyCohorte = this.AffecterStringDepuisFichierConfig("urlAvecCohorte");
        }
        #endregion

        #region Methodes
        public List<Cohorte> RecupererCohortes()
        {
            string? contenuInternet = this.RecupererContenuSite();

            if (String.IsNullOrEmpty(contenuInternet))
            {
                throw new WebException("Site internet non accessible");
            }

            List<string> lignesContenuInternet = this.CouperLignesTexte(contenuInternet);
            List<Cohorte> listeRetour = new List<Cohorte>();

            listeRetour = lignesContenuInternet
                .Where(str => str.Contains(this.AffecterStringDepuisFichierConfig("formatListeCohorte")))
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

            string? contenuInternet = RecupererContenuSite(url);

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
        private string? RecupererContenuSite(string? p_url = null)
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

            string gpHeure = "h", gpMinute = "m", gpSemaine = "s", gpTaille = "t";
            string configHeures = "formatHeures", configMinutes = "formatMinutes", configSemaines = "formatSemaine";
            string configLigneCours = "formatLigneCours", configLigneVide = "formatLignevide", configHauteurCases = "formatHeuteurCase";

            Regex formatHeures = 
                new Regex($"(?<{gpHeure}>{this.AffecterStringDepuisFichierConfig(configHeures)})" +
                $":(?<{gpMinute}>{this.AffecterStringDepuisFichierConfig(configMinutes)})");
            Regex formatSemaines = new Regex($"(?<{gpSemaine}>{this.AffecterStringDepuisFichierConfig(configSemaines)})");
            Regex formatLigneCours = new Regex(this.AffecterStringDepuisFichierConfig(configLigneCours));
            Regex formatLigneVide = new Regex(this.AffecterStringDepuisFichierConfig(configLigneVide));
            Regex formatHauteurCase = new Regex($"(?<{gpTaille}>{this.AffecterStringDepuisFichierConfig(configHauteurCases)})px");

            int compteurHeure = -1;
            int tailleHeure = -1;

            DateTime premierJourSemaine = new DateTime();
            List<DateTime> horaires = new List<DateTime>();

            foreach (string ligne in p_liste)
            {
                Match matchSemaine = formatSemaines.Match(ligne);
                Match matchHeures = formatHeures.Match(ligne);
                Match matchCours = formatLigneCours.Match(ligne);
                Match matchVide = formatLigneVide.Match(ligne);

                if (matchSemaine.Success)
                {
                    premierJourSemaine = DateTime.Parse(matchSemaine.Groups[gpSemaine].Value);
                    horaires = new List<DateTime>();
                    compteurHeure = 0;
                    tailleHeure = 0;
                }
                else if (matchHeures.Success)
                {
                    compteurHeure++;
                    double heure = double.Parse(matchHeures.Groups[gpHeure].Value);
                    double minute = double.Parse(matchHeures.Groups[gpMinute].Value);
                    horaires.Add(premierJourSemaine.AddHours(heure).AddMinutes(minute));
                    if (tailleHeure == 0)
                    {
                        tailleHeure = int.Parse(formatHauteurCase.Match(ligne).Groups[gpTaille].Value);
                    }
                }
                else if (matchVide.Success)
                {
                    compteurHeure--;
                }
                else if (matchCours.Success)
                {
                    DateTime dateDebut = horaires[horaires.Count - compteurHeure];
                    int tempsCours = int.Parse(formatHauteurCase.Match(ligne).Groups[gpTaille].Value) / tailleHeure;
                    DateTime dateFin = horaires[horaires.Count - compteurHeure + tempsCours];

                    CoursInternetDTO nvCours = this.TransformerLigneNouvelleSeance(ligne, dateDebut, dateFin);
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
            string gpInfos = "i", gpNumero = "n";
            string configSeance = "formatLigneSeance", configNumero = "formatNumeroCours";

            Regex regexSeance = new Regex($"(?<{gpInfos}>{this.AffecterStringDepuisFichierConfig(configSeance)})");
            Regex regexNumeroCours = new Regex($"(?<{gpNumero}>{this.AffecterStringDepuisFichierConfig(configNumero)})");
            int positionIntitule = 0, positionNumero = 1, positionProf = 2, positionSalle = 3;
            int PositionNomProf = 0, positionPrenomProf = 1;

            string info = regexSeance.Match(p_ligne).Groups[gpInfos].Value;
            string[] infos = info.Split("<br>");
            infos[positionSalle] = infos[positionSalle].Replace("</td></tr>", String.Empty);

            string intituleCours = infos[positionIntitule];
            string numeroCours = regexNumeroCours.Match(infos[positionNumero]).Groups[gpNumero].Value;

            string[] professeur = infos[positionProf].Split(", ");

            ProfesseurInternetDTO nvProf = null;

            if (professeur.Length == 2)
            {
                nvProf = new ProfesseurInternetDTO(professeur[PositionNomProf], professeur[positionPrenomProf]);
            }
            else
            {
                nvProf = new ProfesseurInternetDTO("-", "-");
            }

            CoursInternetDTO nvCours = new CoursInternetDTO(nvProf, numeroCours, intituleCours);
            SeanceInternetDTO nvSeance = new SeanceInternetDTO(p_dateDebut, p_dateFin, infos[positionSalle]);
            nvCours.Seances.Add(nvSeance);

            return nvCours;
        }
        private IConfigurationRoot LireFichierConfig()
        {
            IConfigurationRoot? configuration;

            try
            {
                configuration =
                    new ConfigurationBuilder()
                      .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                      .AddJsonFile("appsettings.json", false)
                      .Build();
            }
            catch (Exception e)
            {
                throw new InvalidDepotException("Le fichier de configuration est corrompu", e);
            }

            return configuration;
        }
        private string AffecterStringDepuisFichierConfig(string p_nomParametre)
        {
            string? retour;
            IConfigurationRoot configuration = this.LireFichierConfig();

            if (configuration is null)
            {
                throw new Exception("Erreur dans la lecture du fichier de configuration");
            }

            retour = configuration[p_nomParametre];

            if (retour is null)
            {
                throw new Exception("Erreur dans la lecture du fichier de configuration");
            }

            return retour;
        }
        #endregion
    }
}