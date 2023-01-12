using System.Net;
using System.Text.RegularExpressions;
using CalendrierCours.Entites;

namespace CalendrierCours.DAL.SiteInternet
{
    public class DepotSiteInternet
        : IDepotCours
    {
        #region Membres
        private const string PROPRIETE_URL_SITE_INTERNET = "urlSiteInternet";
        private const string PROPRIETE_URL_SITE_AVEC_COHORTE = "urlAvecCohorte";
        private const string PROPRIETE_FORMAT_LISTE_COHORTE = "formatListeCohorte";
        private const string PROPRIETE_FORMAT_NUMERO_COURS = "formatNumeroCours";
        private const string PROPRIETE_FORMAT_HAUTEUR_CASE = "formatHauteurCase";
        private const string PROPRIETE_FORMAT_SEPARATEUR_HEURE = "formatSeparateurHeure";
        private const string PROPRIETE_FORMAT_HEURES = "formatHeures";
        private const string PROPRIETE_FORMAT_MINUTES = "formatMinutes";
        private const string PROPRIETE_FORMAT_SEMAINE = "formatSemaine";
        private const string PROPRIETE_FORMAT_LIGNE_COURS = "formatLigneCours";
        private const string PROPRIETE_FORMAT_LIGNE_VIDE = "formatLignevide";
        private const string PROPRIETE_FORMAT_LIGNE_SEANCE = "formatLigneSeance";
        private const string PROPRIETE_FICHIER_LISTE_COURS = "fichierListeCours";
        private const string PROPRIETE_SEPARATEUR_LISTE_COURS = "separateurListeCours";

        private IProprietes m_proprietes;

        private string m_urlSiteCsfoy;
        private string m_urlSiteCsfoyCohorte;
        #endregion

        #region Ctor
        public DepotSiteInternet(IProprietes p_proprietes)
        {
            if (p_proprietes is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_proprietes));
            }

            this.m_proprietes = p_proprietes;

            this.m_urlSiteCsfoy = this.m_proprietes[PROPRIETE_URL_SITE_INTERNET];
            this.m_urlSiteCsfoyCohorte = this.m_proprietes[PROPRIETE_URL_SITE_AVEC_COHORTE];
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
                .Where(str => str.Contains(this.m_proprietes[PROPRIETE_FORMAT_LISTE_COHORTE]))
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
            List<CoursInternetDTO> listeCours = new List<CoursInternetDTO>();

            try
            {
                listeCours = this.TransformerLignesEnCoursInternetDTO(lignesContenuInternet);
            }
            catch (Exception e)
            {
                throw new InvalidDepotException("Erreur dans l'interprétation du contenu du site", e);
            }

            listeCours = this.ChangerIntitules(listeCours);

            return listeCours
                .Select(cDTO => cDTO.VersEntites())
                .ToList();
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

        private List<CoursInternetDTO> ChangerIntitules(List<CoursInternetDTO> p_liste)
        {
            Dictionary<string, string> listeCours = this.RecupererListeCours();

            foreach (CoursInternetDTO c in p_liste)
            {
                if (listeCours.ContainsKey(c.Numero))
                {
                    c.Intitule = listeCours[c.Numero];
                }
            }

            return p_liste;
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

            Regex formatHeures =
                new Regex($"(?<{gpHeure}>{this.m_proprietes[PROPRIETE_FORMAT_HEURES]})" +
                $"{this.m_proprietes[PROPRIETE_FORMAT_SEPARATEUR_HEURE]}(?<{gpMinute}>{this.m_proprietes[PROPRIETE_FORMAT_MINUTES]})");
            Regex formatSemaines = new Regex($"(?<{gpSemaine}>{this.m_proprietes[PROPRIETE_FORMAT_SEMAINE]})");
            Regex formatLigneCours = new Regex(this.m_proprietes[PROPRIETE_FORMAT_LIGNE_COURS]);
            Regex formatLigneVide = new Regex(this.m_proprietes[PROPRIETE_FORMAT_LIGNE_VIDE]);
            Regex formatHauteurCase = new Regex($"(?<{gpTaille}>{this.m_proprietes[PROPRIETE_FORMAT_HAUTEUR_CASE]})px");

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

            Regex regexSeance = new Regex($"\">(?<{gpInfos}>{this.m_proprietes[PROPRIETE_FORMAT_LIGNE_SEANCE]})");
            Regex regexNumeroCours = new Regex($"(?<{gpNumero}>{this.m_proprietes[PROPRIETE_FORMAT_NUMERO_COURS]})");
            int positionIntitule = 0, positionNumero = 1, positionProf = 2, positionSalle = 3;
            int PositionNomProf = 0, positionPrenomProf = 1;
            
            ProfesseurInternetDTO nvProf;
            CoursInternetDTO nvCours;
            SeanceInternetDTO nvSeance;

            string info = regexSeance.Match(p_ligne).Groups[gpInfos].Value;
            string[] infos = info.Split("<br>");

            string intituleCours = infos[positionIntitule];

            if (infos.Length == 4)
            {
                infos[positionSalle] = infos[positionSalle].Replace("</td></tr>", String.Empty);

                string numeroCours = regexNumeroCours.Match(infos[positionNumero]).Groups[gpNumero].Value;

                string[] professeur = infos[positionProf].Split(", ");


                if (professeur.Length == 2)
                {
                    nvProf = new ProfesseurInternetDTO(professeur[PositionNomProf], professeur[positionPrenomProf]);
                }
                else
                {
                    nvProf = new ProfesseurInternetDTO("-", "-");
                }

                nvCours = new CoursInternetDTO(nvProf, numeroCours, intituleCours);
                nvSeance = new SeanceInternetDTO(p_dateDebut, p_dateFin, infos[positionSalle], Guid.NewGuid());
            }
            else
            {
                nvProf = new ProfesseurInternetDTO("-", "-");
                nvCours = new CoursInternetDTO(nvProf, "000-000-SF", intituleCours);
                nvSeance = new SeanceInternetDTO(p_dateDebut, p_dateFin, "-", Guid.NewGuid());
            }

            nvCours.Seances.Add(nvSeance);

            return nvCours;
        }

        private Dictionary<string, string> RecupererListeCours()
        {
            Dictionary<string, string> retour = new Dictionary<string, string>();
            string contenu;

            string fichier = this.m_proprietes[PROPRIETE_FICHIER_LISTE_COURS];
            string separateur = this.m_proprietes[PROPRIETE_SEPARATEUR_LISTE_COURS];

            if (File.Exists(fichier))
            {
                using (StreamReader sr = new StreamReader(fichier))
                {
                    contenu = sr.ReadToEnd();
                }

                List<string> contenuCoupe = this.CouperLignesTexte(contenu);
                contenuCoupe.ForEach(l =>
                {
                    string[] infos = l.Split(separateur);
                    retour.Add(infos[0], infos[1]);
                });
            }

            return retour;
        }
        #endregion
    }
}