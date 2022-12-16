using CalendrierCours.Entites;
using System.Globalization;
using System.Text;

namespace CalendrierCours.DAL.ExportCoursVCS
{
    public class ExportCoursVCS
        : IExportFichier
    {
        public void ExporterVersFichier(List<Cours> p_cours, string p_chemin)
        {
            if (p_cours is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cours));
            }
            if (String.IsNullOrWhiteSpace(p_chemin))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_chemin));
            }

            List<CoursVCSDTO> coursEport = p_cours.Select(c => new CoursVCSDTO(c)).ToList();

            string fichier = this.RetournerNomFichier(p_chemin);
            string contenu = "";
            coursEport.ForEach(c => c.Seances.ForEach(s => contenu += this.EcrireSeance(c, s)));

            this.EcrireFichier(fichier, contenu);
        }

        private string RetournerNomFichier(string p_chemin)
        {
            bool estDisponible = true;
            string fichier = p_chemin + "\\cours.vcs";

            do
            {
                int compteur = 1;
                if (!File.Exists(fichier))
                {
                    estDisponible = true;
                }
                else
                {
                    fichier = p_chemin + "\\cours" + compteur + ".vcs";
                    compteur++;
                }

            } while (!estDisponible);

            return fichier;
        }

        private string EcrireSeance(CoursVCSDTO p_cours, SeanceVCSDTO p_seance)
        {
            DateTime dateDebut = p_seance.DateDebut.AddHours(5);
            DateTime dateFin = p_seance.DateFin.AddHours(5);
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("BEGIN:VEVENT");
            sb.Append("DTSTART:");
            sb.Append($"{dateDebut.ToString("yyyyMMdd")}T");
            sb.AppendLine($"{dateDebut.ToString("HHmmss")}Z");
            sb.Append("DTEND:");
            sb.Append($"{dateFin.ToString("yyyyMMdd")}T");
            sb.AppendLine($"{dateFin.ToString("HHmmss")}Z");
            sb.AppendLine($"LOCATION:{p_seance.Salle}");
            sb.AppendLine("DESCRIPTION;ENCODING=QUOTED-PRINTABLE:=");
            sb.AppendLine($"Cours donné par {p_cours.Enseignant.Prenom} {p_cours.Enseignant.Nom}=0D=0A");
            sb.AppendLine($"SUMMARY:{p_cours.Numero} - {p_cours.Intitule}");
            sb.AppendLine($"END:VEVENT");
            sb.AppendLine($"END:VCALENDAR");

            return sb.ToString();
        }
        private void EcrireFichier(string p_fichier, string p_contenu)
        {
            FileStream fs = new FileStream(p_fichier, FileMode.Create);
            fs.Dispose();

            using (StreamWriter sw = new StreamWriter(p_fichier))
            {
                try
                {
                    sw.Write(p_contenu);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

    }
}