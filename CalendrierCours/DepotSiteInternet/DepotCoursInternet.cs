using Entites;
using System.Net;
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

            //TextReader textReader = new StringReader(contenuInternet);
            //XmlReader xmlReader = XmlReader.Create(textReader);
            //while (xmlReader.Read())
            //{
            //    if (xmlReader.Name == "option")
            //    {
            //        lignesContenuInternet.Add(xmlReader.ReadInnerXml());
            //    }
            //}
            //lignesContenuInternet = lignesContenuInternet
            //    .Where(str => str.Contains("option value"))
            //    .Select(str =>
            //    {
            //        string[] valeurs = str.Split("\"");
            //        return valeurs[1];
            //    })
            //    .Where(str => str.Length > 0)
            //    .ToList();

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
            throw new NotImplementedException();
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
        #endregion

    }
}