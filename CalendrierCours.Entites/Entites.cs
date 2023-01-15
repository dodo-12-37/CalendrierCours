namespace CalendrierCours.Entites
{
    public class Cohorte
    {
        #region Membres
        private List<Cours> m_listeCours;
        private string m_numero;
        #endregion

        #region Ctors
        public Cohorte(string p_numero)
        {
            if (String.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_numero));
            }

            m_numero = p_numero;
            m_listeCours = new List<Cours>();
        }
        public Cohorte(List<Cours> p_listeCours, string p_numero)
        {
            if (p_listeCours is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_listeCours));
            }
            if (String.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_numero));
            }

            m_listeCours = p_listeCours;
            m_numero = p_numero;
        }
        #endregion

        #region Proprietes
        public List<Cours> Cours
        {
            get
            {
                Cours[] listeRetour = new Cours[m_listeCours.Count];
                m_listeCours.CopyTo(listeRetour);
                return listeRetour.ToList();
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
        public override bool Equals(object? obj)
        {
            return obj is Cohorte cohorte
                && cohorte.m_numero == m_numero;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(m_numero);
        }
        #endregion
    }
    public class Cours
    {
        #region Membres
        private Professeur m_enseignant;
        private List<Seance> m_seances;
        private string m_intitule;
        private string m_numero;
        private string? m_description;
        private string? m_categorie;
        #endregion

        #region Ctor
        public Cours(Professeur p_enseignant, string p_numero, string p_intitule) : this(p_enseignant, p_numero, p_intitule, new List<Seance>())
        { }
        public Cours(Professeur p_enseignant, string p_numero, string p_intitule, List<Seance> p_seances)
        {
            if (p_enseignant is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_enseignant));
            }
            if (p_intitule is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_intitule));
            }
            if (p_numero is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_numero));
            }
            if (p_seances is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_seances));
            }

            m_enseignant = p_enseignant;
            m_seances = p_seances;
            m_intitule = p_intitule;
            m_numero = p_numero;
            m_description = null;
            m_categorie = null;
        }
        #endregion

        #region Proprietes
        public Professeur Enseignant
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
        public List<Seance> Seances
        {
            get
            {
                Seance[] seanceRetour = new Seance[m_seances.Count];
                m_seances.CopyTo(seanceRetour);
                return seanceRetour.ToList();
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

                m_intitule = value;
            }
        }
        public string? Description { get { return m_description; } set { m_description = value; } }
        public string? Categorie { get { return m_categorie; } set { m_categorie = value; } }
        #endregion

        #region Methodes
        public override bool Equals(object? obj)
        {
            return obj is Cours cours
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
    public class Seance
    {
        #region Membres
        private DateTime m_dateDebut;
        private DateTime m_dateFin;
        private string m_salle;
        private Guid m_uid;
        #endregion

        #region Ctor
        public Seance(DateTime p_dateDebut, DateTime p_dateFin, string p_salle, Guid p_uid)
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
        public override bool Equals(object? obj)
        {
            return obj is Seance seance
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
    public class Professeur
    {
        #region Membres
        private string m_nom;
        private string m_prenom;
        #endregion

        #region Ctor
        public Professeur(string p_nom, string p_prenom)
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
        public override bool Equals(object? obj)
        {
            return obj is Professeur professeur &&
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