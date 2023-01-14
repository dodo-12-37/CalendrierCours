using CalendrierCours.Entites;
using System.Text;

namespace CalendrierCours.DAL.ExportCoursICS
{
    public class ExportCoursICS
        : IExportFichier
    {
        private const string NOM_FICHIER_ENREGISTREMENT = "cours";
        private const string PROPRIETE_VTIMEZONE = "TZID";
        private const string PROPRIETE_LANGAGE = "LANGUAGE";

        private IProprietes m_proprietes;

        public ExportCoursICS(IProprietes p_proprietes)
        {
            if (p_proprietes is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_proprietes));
            }

            m_proprietes = p_proprietes;
        }

        public void ExporterVersFichier(List<Cours> p_cours, string p_chemin)
        {
            if (p_cours is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cours));
            }
            if (p_chemin is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_chemin));
            }

            List<CoursICSDTO> coursEport = p_cours.Select(c => 
                {
                    CoursICSDTO cours = new CoursICSDTO(c);
                    cours.Categorie = c.Categorie;
                    return cours;
                })
                .ToList();
            string fichier;

            if (p_chemin == String.Empty)
            {
                fichier = this.RetournerNomFichier();
            }
            else
            {
                fichier = p_chemin;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.EcrireEntete());

            coursEport.ForEach(c => c.Seances.ForEach(s => sb.AppendLine(this.EcrireSeance(c, s))));
            
            sb.AppendLine("END:VCALENDAR");

            this.EcrireFichier(fichier, sb.ToString());
        }

        private string RetournerNomFichier()
        {
            bool estDisponible = true;
            string chemin = Directory.GetParent(AppContext.BaseDirectory).FullName;
            string fichier = chemin + $"\\{NOM_FICHIER_ENREGISTREMENT}.ics";

            do
            {
                int compteur = 1;
                if (!File.Exists(fichier))
                {
                    estDisponible = true;
                }
                else
                {
                    fichier = chemin + $"\\{NOM_FICHIER_ENREGISTREMENT}" + compteur + ".ics";
                    compteur++;
                }

            } while (!estDisponible);

            return fichier;
        }

        private string EcrireEntete()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("BEGIN:VTIMEZONE");
            sb.AppendLine($"TZID:{this.m_proprietes[PROPRIETE_VTIMEZONE]}");
            sb.AppendLine("END:VTIMEZONE");

            return sb.ToString();
        }
        private string EcrireSeance(CoursICSDTO p_cours, SeanceICSDTO p_seance)
        {
            DateTime dateDebut = p_seance.DateDebut;
            DateTime dateFin = p_seance.DateFin;
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("BEGIN:VEVENT");

            if (!String.IsNullOrEmpty(p_cours.Categorie))
            {
                sb.AppendLine($"CATEGORIES:{p_cours.Categorie}");
            }

            sb.Append($"DTSTART;TZID=\"{this.m_proprietes[PROPRIETE_VTIMEZONE]}\":");
            sb.Append($"{dateDebut.ToString("yyyyMMdd")}T");
            sb.AppendLine($"{dateDebut.ToString("HHmmss")}");
            sb.Append($"DTEND;TZID=\"{this.m_proprietes[PROPRIETE_VTIMEZONE]}\":");
            sb.Append($"{dateFin.ToString("yyyyMMdd")}T");
            sb.AppendLine($"{dateFin.ToString("HHmmss")}");
            sb.AppendLine($"LOCATION:{p_seance.Salle}");

            sb.Append("DESCRIPTION:");
            if (!String.IsNullOrEmpty(p_cours.Enseignant.Nom) || !String.IsNullOrEmpty(p_cours.Enseignant.Prenom))
            {
                sb.Append($"Cours donné par {p_cours.Enseignant.Prenom} {p_cours.Enseignant.Nom}");
            }
            if (p_cours.Description is not null)
            {
                sb.AppendLine(p_cours.Description);
            }
            else
            {
                sb.AppendLine();
            }

            if (String.IsNullOrEmpty(p_cours.Numero))
            {
                sb.AppendLine($"SUMMARY;LANGUAGE={this.m_proprietes[PROPRIETE_LANGAGE]}:{p_cours.Intitule}");
            }
            else
            {
                sb.AppendLine($"SUMMARY;LANGUAGE={this.m_proprietes[PROPRIETE_LANGAGE]}:{p_cours.Numero} - {p_cours.Intitule}");
            }
            sb.AppendLine($"UID:{p_seance.UID.ToString()}");
            sb.AppendLine($"END:VEVENT");

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