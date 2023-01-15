using CalendrierCours.Entites;

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

            Numero = p_numero;
            m_listeCours = new List<CoursInternetDTO>();
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

            m_listeCours = p_listeCours.Select(c => new CoursInternetDTO(c)).ToList();
            m_numero = p_numero;
        }
        public CohorteInternetDTO(Cohorte p_cohorte) : this(p_cohorte.Cours, p_cohorte.Numero) { }
        #endregion

        #region Proprietes
        public List<CoursInternetDTO> ListeCours
        {
            get
            {
                return m_listeCours;
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }
                m_listeCours = value;
            }
        }
        public string Numero
        {
            get { return m_numero; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide");
                }
                m_numero = value;
            }
        }
        #endregion

        #region Methodes
        public Cohorte VersEntite()
        {
            List<Cours> listeCours = m_listeCours.Select(cDTo => cDTo.VersEntites()).ToList();

            return new Cohorte(listeCours, Numero);
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

            m_enseignant = p_enseignant;
            m_intitule = p_intitule;
            m_seances = p_seances;
            m_numero = p_numero;
        }
        public CoursInternetDTO(Cours p_cours)
            : this(new ProfesseurInternetDTO(p_cours.Enseignant), p_cours.Numero, p_cours.Intitule, p_cours.Seances.Select(s => new SeanceInternetDTO(s)).ToList())
        { }
        #endregion

        #region Proprietes
        public ProfesseurInternetDTO Enseignant
        {
            get { return m_enseignant; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null");
                }

                m_enseignant = value;
            }
        }
        public string Intitule
        {
            get { return m_intitule; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                m_intitule = value;
            }
        }
        public string Numero
        {
            get { return m_numero; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                m_numero = value;
            }
        }
        public List<SeanceInternetDTO> Seances
        {
            get
            {
                return m_seances;
            }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                m_seances = value;
            }
        }
        #endregion

        #region Methodes
        public Cours VersEntites()
        {
            List<Seance> Seances = m_seances.Select(s => s.VersEntite()).ToList();

            return new Cours(m_enseignant.VersEntite(), m_numero, m_intitule, Seances);
        }

        public override bool Equals(object? obj)
        {
            return obj is CoursInternetDTO cours
                && Enseignant.Equals(cours.Enseignant)
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
                m_uid = Guid.NewGuid();
            }
            else
            {
                m_uid = p_uid;
            }

            m_dateDebut = p_dateDebut;
            m_dateFin = p_dateFin;
            m_salle = p_salle;
        }
        public SeanceInternetDTO(Seance p_seance) : this(p_seance.DateDebut, p_seance.DateFin, p_seance.Salle, p_seance.UID) { }
        #endregion

        #region Proprietes
        public DateTime DateDebut
        {
            get { return m_dateDebut; }
            set
            {
                if (value >= m_dateFin)
                {
                    throw new ArgumentException("La date de debut doit etre inferieur a la date de fin");
                }

                m_dateDebut = value;
            }
        }
        public DateTime DateFin
        {
            get { return m_dateFin; }
            set
            {
                if (value <= m_dateDebut)
                {
                    throw new ArgumentException("La date de debut doit etre inferieur a la date de fin");
                }

                m_dateFin = value;
            }
        }
        public string Salle
        {
            get { return m_salle; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentException("Ne doit pas etre null");
                }

                m_salle = value;
            }
        }
        public Guid UID { get { return m_uid; } }
        #endregion

        #region Methodes
        public Seance VersEntite()
        {
            return new Seance(m_dateDebut, m_dateFin, m_salle, m_uid);
        }
        public override bool Equals(object? obj)
        {
            return obj is SeanceInternetDTO seance
                && seance.UID == UID
                && seance.DateDebut == DateDebut
                && seance.DateFin == DateFin
                && seance.Salle == Salle;
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

            m_nom = p_nom;
            m_prenom = p_prenom;
        }
        public ProfesseurInternetDTO(Professeur p_enseigant) : this(p_enseigant.Nom, p_enseigant.Prenom) { }
        #endregion

        #region Proprietes
        public string Nom
        {
            get { return m_nom; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                m_nom = value;
            }
        }
        public string Prenom
        {
            get { return m_prenom; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                m_prenom = value;
            }
        }
        #endregion

        #region Methodes
        public Professeur VersEntite()
        {
            return new Professeur(m_nom, m_prenom);
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
