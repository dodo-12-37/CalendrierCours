using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entites;

namespace BL
{
    public class Traitement
    {
        #region Membres
        IDepotCours m_depot;
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
        public void ExporterSeances(Cohorte p_cohorte, IExportFichier p_typeExport, DateTime? p_date = null)
        {
            throw new NotImplementedException();
        }
        public void ExporterSeances(Cohorte p_cohorte, IExportFichier p_typeExport, Professeur p_professeur, DateTime? p_date = null)
        {
            throw new NotImplementedException();
        }
        public List<Cohorte> ListerCohorte()
        {
            return this.m_depot.RecupererCohortes();
        }
        public List<Cours> ListerCours(Cohorte p_cohorte)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }

            if (!Cache.Cohorte.Equals(p_cohorte))
            {
                Cache.Cohorte = p_cohorte;
                Cache.Cours = this.m_depot.RecupererCours(p_cohorte);
            }

            return Cache.Cours;
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

            List<Seance> seancesRetour = new List<Seance>();
            this.ListerCours(p_cohorte);

            if (Cache.Cours.Contains(p_cours))
            {
                seancesRetour = Cache.Cours.Single(c => c.Equals(p_cours)).Seances;
            }

            return seancesRetour;
        }
        public List<Seance> ListerSeances(Cohorte p_cohorte, Cours p_cours, DateTime p_date)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_cours is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cours));
            }

            List<Seance> seancesRetour = this.ListerSeances(p_cohorte, p_cours);

            return seancesRetour.Where(s => s.DateDebut >= p_date).ToList();
        }
        public List<Seance> ListerSeances(Cohorte p_cohorte, Professeur p_professeur, DateTime p_date)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }
            if (p_professeur is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_professeur));
            }

            List<Cours> cours = this.ListerCours(p_cohorte).Where(c => c.Enseignant.Equals(p_professeur)).ToList();
            List<Seance> seancesRetour = cours
                .SelectMany(c => c.Seances
                    .Select(s => s.DateDebut >= p_date))
                .ToList();

            throw new NotImplementedException();
        }
        public void ModifierIntituleCours(string p_intitule)
        {
            throw new NotImplementedException();
        }
        public void ModifierNomProfesseur(string p_nom)
        {
            throw new NotImplementedException();
        }
        public void ModifierPrenomProfesseur(string p_prenom)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
