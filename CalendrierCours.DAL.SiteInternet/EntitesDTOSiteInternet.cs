using CalendrierCours.Entites;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace CalendrierCours.DAL.SiteInternet
{
    public class CohorteInternetDTO
    {
        #region Membres
        private List<CoursInternetDTO> m_listeCours;
        private string m_numero;
        #endregion

        #region Ctor
        public CohorteInternetDTO(string p_numero)
        {
            if (String.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_numero));
            }

            this.Numero = p_numero;
            this.m_listeCours = new List<CoursInternetDTO>();
        }
        public CohorteInternetDTO(List<Cours> p_listeCours, string p_numero)
        {
            if (p_listeCours is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_listeCours));
            }
            if (String.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_numero));
            }

            this.m_listeCours = p_listeCours.Select(c => new CoursInternetDTO(c)).ToList();
            this.m_numero = p_numero;
        }
        public CohorteInternetDTO(Cohorte p_cohorte) : this(p_cohorte.Cours, p_cohorte.Numero) { }
        #endregion

        #region Proprietes
        public List<CoursInternetDTO> ListeCours
        {
            get
            {
                return this.m_listeCours;
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }
                this.m_listeCours = value;
            }
        }
        public string Numero
        {
            get { return this.m_numero; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide");
                }
                this.m_numero = value;
            }
        }
        #endregion

        #region Methodes
        public Cohorte VersEntite()
        {
            List<Cours> listeCours = this.m_listeCours.Select(cDTo => cDTo.VersEntites()).ToList();

            return new Cohorte(listeCours, this.Numero);
        }
        #endregion
    }
    public class CoursInternetDTO
    {
        #region Membres
        private ProfesseurInternetDTO m_enseignant;
        private List<SeanceInternetDTO> m_seances;
        private string m_intitule;
        private string m_numero;
        #endregion

        #region Ctor
        public CoursInternetDTO(ProfesseurInternetDTO p_enseignant, string p_numero, string p_intitule)
            : this(p_enseignant, p_numero, p_intitule, new List<SeanceInternetDTO>())
        { }
        public CoursInternetDTO(ProfesseurInternetDTO p_enseignant, string p_numero, string p_intitule, List<SeanceInternetDTO> p_seances)
        {
            if (p_enseignant is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_enseignant));
            }
            if (p_intitule is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_intitule));
            }
            if (p_numero is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_numero));
            }
            if (p_seances is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_seances));
            }

            this.m_enseignant = p_enseignant;
            this.m_intitule = p_intitule;
            this.m_seances = p_seances;
            this.m_numero = p_numero;
        }
        public CoursInternetDTO(Cours p_cours)
            : this(new ProfesseurInternetDTO(p_cours.Enseignant), p_cours.Numero, p_cours.Intitule, p_cours.Seances.Select(s => new SeanceInternetDTO(s)).ToList())
        { }
        #endregion

        #region Proprietes
        public ProfesseurInternetDTO Enseignant
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
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                this.m_intitule = value;
            }
        }
        public string Numero
        {
            get { return this.m_numero; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                this.m_numero = value;
            }
        }
        public List<SeanceInternetDTO> Seances
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

            return new Cours(this.m_enseignant.VersEntite(), this.m_numero, this.m_intitule, Seances);
        }

        public override bool Equals(object? obj)
        {
            return obj is CoursInternetDTO cours
                && this.Enseignant.Equals(cours.Enseignant)
                && Numero == cours.Numero
                && Intitule == cours.Intitule;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Enseignant, Intitule, Numero);
        }
        #endregion
    }
    public class SeanceInternetDTO
    {
        #region Membres
        private DateTime m_dateDebut;
        private DateTime m_dateFin;
        private string m_salle;
        private Guid m_uid;
        #endregion

        #region Ctor
        public SeanceInternetDTO(DateTime p_dateDebut, DateTime p_dateFin, string p_salle, Guid p_uid)
        {
            if (p_dateDebut >= p_dateFin)
            {
                throw new ArgumentException("La date de debut doit etre inferieur a la date de fin");
            }
            if (p_salle is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null");
            }

            if (p_uid == Guid.Empty)
            {
                this.m_uid = Guid.NewGuid();
            }
            else
            {
                this.m_uid = p_uid;
            }

            this.m_dateDebut = p_dateDebut;
            this.m_dateFin = p_dateFin;
            this.m_salle = p_salle;
        }
        public SeanceInternetDTO(Seance p_seance) : this(p_seance.DateDebut, p_seance.DateFin, p_seance.Salle, p_seance.UID) { }
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
                if (value is null)
                {
                    throw new ArgumentException("Ne doit pas etre null");
                }

                this.m_salle = value;
            }
        }
        public Guid UID { get { return this.m_uid; } }
        #endregion

        #region Methodes
        public Seance VersEntite()
        {
            return new Seance(this.m_dateDebut, this.m_dateFin, this.m_salle, this.m_uid);
        }
        public override bool Equals(object? obj)
        {
            return obj is SeanceInternetDTO seance
                && seance.UID == this.UID
                && seance.DateDebut == this.DateDebut
                && seance.DateFin == this.DateFin
                && seance.Salle == this.Salle;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(m_uid, m_dateDebut, m_dateFin, m_salle);
        }
        #endregion
    }
    public class ProfesseurInternetDTO
    {
        #region Membres
        private string m_nom;
        private string m_prenom;
        #endregion

        #region Ctor
        public ProfesseurInternetDTO(string p_nom, string p_prenom)
        {
            if (p_nom is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_nom));
            }
            if (p_prenom is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null");
            }

            this.m_nom = p_nom;
            this.m_prenom = p_prenom;
        }
        public ProfesseurInternetDTO(Professeur p_enseigant) : this(p_enseigant.Nom, p_enseigant.Prenom) { }
        #endregion

        #region Proprietes
        public string Nom
        {
            get { return this.m_nom; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                this.m_nom = value;
            }
        }
        public string Prenom
        {
            get { return this.m_prenom; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
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
            return obj is ProfesseurInternetDTO professeur &&
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
