using CalendrierCours.Entites;
using System.Text;
using System.Text.RegularExpressions;

namespace CalendrierCours.DAL.ExportCoursVCS
{
    public class CoursVCSDTO
    {
        #region Membres
        private string m_intitule;
        private string m_numero;
        private ProfesseurVCSDTO m_enseignant;
        private List<SeanceVCSDTO> m_seances;
        #endregion

        #region Ctor
        public CoursVCSDTO(ProfesseurVCSDTO p_enseignant, string p_intitule)
        {
            if (p_enseignant is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_enseignant));
            }
            if (String.IsNullOrWhiteSpace(p_intitule))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_intitule));
            }

            this.m_enseignant = p_enseignant;
            this.m_intitule = p_intitule;
            this.m_seances = new List<SeanceVCSDTO>();
        }
        public CoursVCSDTO(ProfesseurVCSDTO p_enseignant, string p_intitule, List<SeanceVCSDTO> p_seances)
        {
            if (p_enseignant is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_enseignant));
            }
            if (String.IsNullOrWhiteSpace(p_intitule))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_intitule));
            }
            if (p_seances is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_seances));
            }

            Regex regexNumeroCours = new Regex("(?<cours>[0-9]{3}-[A-Z]{1}[0-9]{2}-SF)");

            string numeroCours = "";

            if (regexNumeroCours.IsMatch(p_intitule))
            {
                numeroCours = regexNumeroCours.Match(p_intitule).Groups["cours"].Value;
            }

            string intituleCours = p_intitule.Replace(numeroCours + " - ", string.Empty);

            this.m_intitule = intituleCours;
            this.m_numero = numeroCours;
            this.m_enseignant = p_enseignant;
            this.m_seances = p_seances;
        }
        public CoursVCSDTO(Cours p_cours)
            : this(new ProfesseurVCSDTO(p_cours.Enseignant), p_cours.Intitule, p_cours.Seances.Select(s => new SeanceVCSDTO(s)).ToList())
        { }
        #endregion

        #region Proprietes
        public ProfesseurVCSDTO Enseignant
        {
            get { return this.m_enseignant; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null");
                }

                this.m_enseignant = value;
            }
        }
        public string Intitule
        {
            get { return this.m_intitule; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(value));
                }

                this.m_intitule = value;
            }
        }
        public string Numero
        {
            get { return this.m_numero; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(value));
                }

                this.m_intitule = value;
            }
        }
        public List<SeanceVCSDTO> Seances
        {
            get
            {
                return this.m_seances;
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                this.m_seances = value;
            }
        }
        #endregion

        #region Methodes
        public Cours VersEntites()
        {
            List<Seance> Seances = this.m_seances.Select(s => s.VersEntite()).ToList();

            return new Cours(this.m_enseignant.VersEntite(), this.m_intitule, Seances);
        }
        public override bool Equals(object? obj)
        {
            return obj is CoursVCSDTO cours
                && this.Enseignant.Equals(cours.Enseignant)
                && Intitule == cours.Intitule;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Enseignant, Intitule);
        }
        #endregion
    }
    public class SeanceVCSDTO
    {
        #region Membres
        private DateTime m_dateDebut;
        private DateTime m_dateFin;
        private string m_salle;
        #endregion

        #region Ctor
        public SeanceVCSDTO(DateTime p_dateDebut, DateTime p_dateFin, string p_salle)
        {
            if (p_dateDebut >= p_dateFin)
            {
                throw new ArgumentException("La date de debut doit etre inferieur a la date de fin");
            }
            if (String.IsNullOrWhiteSpace(p_salle))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide");
            }

            this.m_dateDebut = p_dateDebut;
            this.m_dateFin = p_dateFin;
            this.m_salle = p_salle;
        }
        public SeanceVCSDTO(Seance p_seance) : this(p_seance.DateDebut, p_seance.DateFin, p_seance.Salle) { }
        #endregion

        #region Proprietes
        public DateTime DateDebut
        {
            get { return this.m_dateDebut; }
            set
            {
                if (value >= this.m_dateFin)
                {
                    throw new ArgumentException("La date de debut doit etre inferieur a la date de fin");
                }

                this.m_dateDebut = value;
            }
        }
        public DateTime DateFin
        {
            get { return this.m_dateFin; }
            set
            {
                if (value <= this.m_dateDebut)
                {
                    throw new ArgumentException("La date de debut doit etre inferieur a la date de fin");
                }

                this.m_dateFin = value;
            }
        }
        public string Salle
        {
            get { return this.m_salle; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Ne doit pas etre null ou vide");
                }

                this.m_salle = value;
            }
        }
        #endregion

        #region Methodes
        public Seance VersEntite()
        {
            return new Seance(this.m_dateDebut, this.m_dateFin, this.m_salle);
        }
        public override bool Equals(object? obj)
        {
            return obj is SeanceVCSDTO seance
                && seance.DateDebut == this.DateDebut
                && seance.DateFin == this.DateFin
                && seance.Salle == this.Salle;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(m_dateDebut, m_dateFin, m_salle);
        }
        #endregion
    }
    public class ProfesseurVCSDTO
    {
        #region Membres
        private string m_nom;
        private string m_prenom;
        #endregion

        #region Ctor
        public ProfesseurVCSDTO(string p_nom, string p_prenom)
        {
            if (String.IsNullOrWhiteSpace(p_nom))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_nom));
            }
            if (String.IsNullOrWhiteSpace(p_prenom))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide");
            }

            this.m_nom = p_nom;
            this.m_prenom = p_prenom;
        }
        public ProfesseurVCSDTO(Professeur p_enseigant) : this(p_enseigant.Nom, p_enseigant.Prenom) { }
        #endregion

        #region Proprietes
        public string Nom
        {
            get { return this.m_nom; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(value));
                }

                this.m_nom = value;
            }
        }
        public string Prenom
        {
            get { return this.m_prenom; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(value));
                }

                this.m_prenom = value;
            }
        }
        #endregion

        #region Methodes
        public Professeur VersEntite()
        {
            return new Professeur(this.m_nom, this.m_prenom);
        }
        public override bool Equals(object? obj)
        {
            return obj is ProfesseurVCSDTO professeur &&
                   Nom == professeur.Nom &&
                   Prenom == professeur.Prenom;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Nom, Prenom);
        }
        #endregion
    }
}
