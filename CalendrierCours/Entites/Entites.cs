namespace Entites
{
    public class Cohorte
    {
        #region Membres
        private List<Cours> m_listeCours;
        private string m_numero;
        #endregion

        #region Ctors
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

            this.m_listeCours = p_listeCours;
            this.m_numero = p_numero;
        }
        #endregion

        #region Proprietes
        public List<Cours> ListeCours
        {
            get
            {
                Cours[] listeRetour = new Cours[this.m_listeCours.Count];
                this.m_listeCours.CopyTo(listeRetour);
                return listeRetour.ToList();
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

        #endregion
    }

    public class Cours
    {
        #region Membres
        private Professeur m_enseignant;
        private string m_intitule;
        private List<Seance> m_seances;
        #endregion

        #region Ctor
        public Cours(Professeur p_enseignant, string p_intitule)
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
            this.m_seances = new List<Seance>();
        }
        public Cours(Professeur p_enseignant, string p_intitule, List<Seance> p_seances)
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

            this.m_enseignant = p_enseignant;
            this.m_intitule = p_intitule;
            this.m_seances = p_seances;
        }
        #endregion

        #region Proprietes
        public Professeur Enseignant
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
        public List<Seance> Seances
        {
            get
            {
                Seance[] seanceRetour = new Seance[this.m_seances.Count];
                this.m_seances.CopyTo(seanceRetour);
                return seanceRetour.ToList();
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
        public override bool Equals(object? obj)
        {
            return obj is Cours cours
                && this.Enseignant.Equals(cours.Enseignant)
                && Intitule == cours.Intitule;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Enseignant, Intitule);
        }
        #endregion
    }

    public class Seance
    {
        #region Membres
        private DateTime m_dateDebut;
        private DateTime m_dateFin;
        private string m_salle;
        #endregion

        #region Ctor
        public Seance(DateTime p_dateDebut, DateTime p_dateFin, string p_salle)
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
        public override bool Equals(object? obj)
        {
            return obj is Seance seance
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

    public class Professeur
    {
        #region Membres
        private string m_nom;
        private string m_prenom;
        #endregion

        #region Ctor
        public Professeur(string p_nom, string p_prenom)
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