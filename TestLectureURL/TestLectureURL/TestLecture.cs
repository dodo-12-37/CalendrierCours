using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TestLectureURL
{
    public static class TestLecture
    {
        public static void Lecture()
        {
            string urlEntre = "https://externe5.csfoy.ca/horairecohorte/index.php";

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string contenuWeb;

            using (WebClient client = new WebClient())
            {
                contenuWeb = client.DownloadString(urlEntre);
            }

            string[] lignesContenuWeb = contenuWeb.Split("\r\n");
            List<string> listeProgrammes = new List<string>();

            listeProgrammes = lignesContenuWeb
                .Where(str => str.Contains("option value"))
                .Select(str =>
                {
                    string[] valeurs = str.Split("\"");
                    return valeurs[1];
                })
                .Where(str => str.Length > 0)
                .ToList();

            foreach (var str in listeProgrammes)
            {
                Console.WriteLine(str);
            }

            string url = "https://externe5.csfoy.ca/horairecohorte/index.php?cohorte="+"LEAD4_H22_4394";

            string contenuCohorte;

            using (WebClient client = new WebClient())
            {
                contenuCohorte = client.DownloadString(url);
            }

            List<string> contenuTR = contenuCohorte.Split("\r\n").ToList()
                .Where(ln => ln.Contains("<tr>") || ln.Contains("<td>"))
                .Where(ln => ln.Length > 0)
                .Select(ln => ln.Trim())
                .ToList();

            foreach (var ln in contenuTR)
            {
                Console.WriteLine(ln);
            }

        }

        
    }
}
