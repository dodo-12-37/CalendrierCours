using CalendrierCours.Entites;

namespace CalendrierCours.DAL.ExportCoursICS
{
    public class CoursICSDTO
    {
        #region Membres
        private string m_intitule;
        private string m_numero;
        private string? m_description;
        private string? m_categorie;
        private ProfesseurICSDTO m_enseignant;
        private List<SeanceICSDTO> m_seances;
        #endregion

        #region Ctor
        public CoursICSDTO(ProfesseurICSDTO p_enseignant, string p_numero, string p_intitule)
            : this(p_enseignant, p_numero, p_intitule, null, null, new List<SeanceICSDTO>()) { }
        public CoursICSDTO
            (ProfesseurICSDTO p_enseignant, string p_numero, string p_intitule
            , string? p_description, string? p_categorie, List<SeanceICSDTO> p_seances)
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

            m_intitule = p_intitule;
            m_numero = p_numero;
            m_enseignant = p_enseignant;
            m_seances = p_seances;
            m_categorie = p_categorie;
            m_description = p_description;
        }
        public CoursICSDTO(Cours p_cours)
            : this(new ProfesseurICSDTO(p_cours.Enseignant)
                  , p_cours.Numero
                  , p_cours.Intitule
                  , p_cours.Description
                  , p_cours.Categorie
                  , p_cours.Seances.Select(s => new SeanceICSDTO(s)).ToList())
        { }
        #endregion

        #region Proprietes
        public ProfesseurICSDTO Enseignant
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
        public List<SeanceICSDTO> Seances
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
        public string Intitule
        {
            get { return m_intitule; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(value));
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
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(value));
                }

                m_intitule = value;
            }
        }
        public string? Description { get { return m_description; } set { m_description = value; } }
        public string? Categorie { get { return m_categorie; } set { m_categorie = value; } }
        #endregion

        #region Methodes
        public Cours VersEntites()
        {
            List<Seance> Seances = m_seances.Select(s => s.VersEntite()).ToList();

            return new Cours(m_enseignant.VersEntite(), Numero, m_intitule, Seances);
        }
        public override bool Equals(object? obj)
        {
            return obj is CoursICSDTO cours
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
    public class SeanceICSDTO
    {
        #region Membres
        private DateTime m_dateDebut;
        private DateTime m_dateFin;
        private string m_salle;
        private Guid m_uid;
        #endregion

        #region Ctor
        public SeanceICSDTO(DateTime p_dateDebut, DateTime p_dateFin, string p_salle, Guid p_uid)
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
            m_uid = p_uid;
        }
        public SeanceICSDTO(Seance p_seance) : this(p_seance.DateDebut, p_seance.DateFin, p_seance.Salle, p_seance.UID) { }
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
            return obj is SeanceICSDTO seance
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
    public class ProfesseurICSDTO
    {
        #region Membres
        private string m_nom;
        private string m_prenom;
        #endregion

        #region Ctor
        public ProfesseurICSDTO(string p_nom, string p_prenom)
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
        public ProfesseurICSDTO(Professeur p_enseigant) : this(p_enseigant.Nom, p_enseigant.Prenom) { }
        #endregion

        #region Proprietes
        public string Nom
        {
            get { return m_nom; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(value));
                }

                m_nom = value;
            }
        }
        public string Prenom
        {
            get { return m_prenom; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(value));
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
            return obj is ProfesseurICSDTO professeur &&
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
