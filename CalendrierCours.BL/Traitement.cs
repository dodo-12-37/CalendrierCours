using CalendrierCours.Entites;
using System.Net;
using System.Text.RegularExpressions;

namespace CalendrierCours.BL
{
    public class Traitement
    {
        #region Membres
        private IDepotCours m_depot;
        #endregion

        #region Ctor
        public Traitement(IDepotCours p_depot)
        {
            if (p_depot is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_depot));
            }

            this.m_depot = p_depot;
        }
        #endregion

        #region Proprietes

        #endregion

        #region Methodes
        public void ExporterSeances(Cohorte p_cohorte, IExportFichier p_typeExport, string p_chemin, DateTime? p_date = null)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_typeExport is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_typeExport));
            }
            if (String.IsNullOrWhiteSpace(p_chemin))
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_chemin));
            }

            this.MiseAJourCache(p_cohorte);

            if (p_date is null)
            {
                p_date = DateTime.Now;
            }

            List<Cours> listeExport = this.ListerCours(p_cohorte)
                .Select(c => new Cours(c.Enseignant, c.Intitule, c.Seances
                    .Where(s => s.DateDebut >= p_date)
                    .ToList()))
                .ToList();

            p_typeExport.ExporterVersFichier(listeExport, p_chemin);
        }
        public void ExporterSeances(Cohorte p_cohorte, IExportFichier p_typeExport, Professeur p_professeur, string p_chemin, DateTime? p_date = null)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_typeExport is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_typeExport));
            }
            if (p_professeur is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_professeur));
            }
            if (String.IsNullOrWhiteSpace(p_chemin))
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_chemin));
            }

            this.MiseAJourCache(p_cohorte);

            if (p_date is null)
            {
                p_date = DateTime.Now;
            }

            List<Cours> listeExport = this.ListerCours(p_cohorte)
                .Where(c => c.Enseignant.Equals(p_professeur))
                .Select(c => new Cours(c.Enseignant, c.Intitule, c.Seances
                    .Where(s => s.DateDebut >= p_date)
                    .ToList()))
                .ToList();

            p_typeExport.ExporterVersFichier(listeExport, p_chemin);
        }
        public List<Cohorte> ListerCohorte()
        {
            List<Cohorte> listeCohortes = new List<Cohorte>();

            try
            {
                listeCohortes = this.m_depot.RecupererCohortes();
            }
            catch (Exception)
            {
                throw;
            }

            return listeCohortes;
        }
        public List<Cours> ListerCours(Cohorte p_cohorte)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }

            this.MiseAJourCache(p_cohorte);

            return Cache.Cohorte.Cours;
        }
        public List<Seance> ListerSeances(Cohorte p_cohorte, Cours p_cours)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_cours is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cours));
            }

            this.MiseAJourCache(p_cohorte);

            List<Seance> seancesRetour = new List<Seance>();
            this.ListerCours(p_cohorte);

            if (Cache.Cohorte.Cours.Contains(p_cours))
            {
                seancesRetour = Cache.Cohorte.Cours.Single(c => c.Equals(p_cours)).Seances;
            }

            return seancesRetour;
        }
        public List<Seance> ListerSeances(Cohorte p_cohorte, Cours p_cours, DateTime? p_date = null)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_cours is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cours));
            }

            this.MiseAJourCache(p_cohorte);

            if (p_date is null)
            {
                p_date = DateTime.Now;
            }

            List<Seance> seancesRetour = this.ListerSeances(p_cohorte, p_cours);

            return seancesRetour.Where(s => s.DateDebut >= p_date).ToList();
        }
        public List<Seance> ListerSeances(Cohorte p_cohorte, Professeur p_professeur, DateTime? p_date = null)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_professeur is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_professeur));
            }

            this.MiseAJourCache(p_cohorte);

            if (p_date is null)
            {
                p_date = DateTime.Now;
            }

            List<Cours> cours = this.ListerCours(p_cohorte).Where(c => c.Enseignant.Equals(p_professeur)).ToList();
            List<Seance> seancesRetour = cours
                .SelectMany(c => c.Seances
                    .Where(s => s.DateDebut >= p_date))
                .ToList();

            return seancesRetour;
        }
        public void ModifierIntituleCours(Cohorte p_cohorte, Cours p_cours, string p_intitule)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_cours is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cours));
            }
            if (String.IsNullOrWhiteSpace(p_intitule))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_intitule));
            }

            this.MiseAJourCache(p_cohorte);

            Cours coursAChanger = Cache.Cohorte.Cours.SingleOrDefault(c => c.Equals(p_cours));

            if (coursAChanger is null)
            {
                throw new ArgumentException("Le cours ne fait pas partis de la cohorte", nameof(p_cours));
            }
            else
            {
                Regex regexNumeroCours = new Regex("(?<cours>[0-9]{3}-[A-Z]{1}[0-9]{2}-SF)");
                string numeroCours = regexNumeroCours.Match(p_cours.Intitule).Groups["cours"].Value;

                coursAChanger.Intitule = $"{numeroCours} - {p_intitule}";
            }
        }
        public void ModifierNomProfesseur(Cohorte p_cohorte, Professeur p_enseignant, string p_nom)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_enseignant is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_enseignant));
            }
            if (String.IsNullOrWhiteSpace(p_nom))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_nom));
            }

            this.MiseAJourCache(p_cohorte);

            List<Cours> cours = Cache.Cohorte.Cours.Where(c => c.Enseignant.Equals(p_enseignant)).ToList();

            cours.ForEach(c => c.Enseignant.Nom = p_nom);
        }
        public void ModifierPrenomProfesseur(Cohorte p_cohorte, Professeur p_enseignant, string p_prenom)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_enseignant is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_enseignant));
            }
            if (String.IsNullOrWhiteSpace(p_prenom))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_prenom));
            }

            this.MiseAJourCache(p_cohorte);

            List<Cours> cours = Cache.Cohorte.Cours.Where(c => c.Enseignant.Equals(p_enseignant)).ToList();

            cours.ForEach(c => c.Enseignant.Prenom = p_prenom);
        }
        private void MiseAJourCache(Cohorte p_cohorte)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }

            if (Cache.Cohorte is null || !Cache.Cohorte.Equals(p_cohorte))
            {
                Cache.Cohorte = p_cohorte;
                try
                {
                    Cache.Cohorte.Cours = this.m_depot.RecupererCours(p_cohorte);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        #endregion
    }
}
