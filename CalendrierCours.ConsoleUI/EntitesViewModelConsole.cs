using CalendrierCours.Entites;
using System.Globalization;
using System.Text;

namespace CalendrierCours.ConsoleUI
{
    public class CohorteViewModelConsole
    {
        #region Membres
        private List<CoursViewModelConsole> m_listeCours;
        private string m_numero;
        #endregion

        #region Ctor
        public CohorteViewModelConsole(string p_numero)
        {
            if (String.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_numero));
            }

            this.Numero = p_numero;
            this.m_listeCours = new List<CoursViewModelConsole>();
        }
        public CohorteViewModelConsole(List<CoursViewModelConsole> p_listeCours, string p_numero)
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
        public CohorteViewModelConsole(Cohorte p_cohorte) 
            : this(p_cohorte.Cours.Select(c => new CoursViewModelConsole(c)).ToList(), p_cohorte.Numero)
        { }
        #endregion

        #region Proprietes
        public List<CoursViewModelConsole> ListeCours
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
        public override string ToString()
        {
            int positionPeriode = 1, positionNumero = 2;
            string[] elementsCohorte = this.m_numero.Split('_');

            return $"Cohorte n° {elementsCohorte[positionNumero]} - période : {elementsCohorte[positionPeriode]}";
        }
        public override bool Equals(object? obj)
        {
            return obj is CohorteViewModelConsole cohorte
                && cohorte.Numero == this.m_numero;
        }
        #endregion
    }
    public class CoursViewModelConsole
    {
        #region Membres
        private ProfesseurViewModelConsole m_enseignant;
        private string m_intitule;
        private List<SeanceViewModelConsole> m_seances;
        #endregion

        #region Ctor
        public CoursViewModelConsole(ProfesseurViewModelConsole p_enseignant, string p_intitule)
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
            this.m_seances = new List<SeanceViewModelConsole>();
        }
        public CoursViewModelConsole(ProfesseurViewModelConsole p_enseignant, string p_intitule, List<SeanceViewModelConsole> p_seances)
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
        public CoursViewModelConsole(Cours p_cours) 
            : this(new ProfesseurViewModelConsole(p_cours.Enseignant), p_cours.Intitule, p_cours.Seances.Select(s => new SeanceViewModelConsole(s)).ToList()) 
        { }
        #endregion

        #region Proprietes
        public ProfesseurViewModelConsole Enseignant
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
        public List<SeanceViewModelConsole> Seances
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
        public override string ToString()
        {
            int positionNumero = 0, positionIntitule = 1;
            string[] elementsIntitule = this.m_intitule.Split(" - ");
            StringBuilder sb = new StringBuilder();

            sb.Append($"Cours n° {elementsIntitule[positionNumero]} - {elementsIntitule[positionIntitule]} ");
            sb.Append($"donné par {this.Enseignant.ToString()} ");
            sb.AppendLine($"- Nombre de séances : {this.Seances.Count}");
            this.m_seances.ForEach(s => sb.AppendLine($"\t{s.ToString()}"));

            return sb.ToString();
        }
        public override bool Equals(object? obj)
        {
            return obj is CoursViewModelConsole cours
                && this.Enseignant.Equals(cours.Enseignant)
                && Intitule == cours.Intitule;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Enseignant, Intitule);
        }
        #endregion
    }
    public class SeanceViewModelConsole
    {
        #region Membres
        private DateTime m_dateDebut;
        private DateTime m_dateFin;
        private string m_salle;
        #endregion

        #region Ctor
        public SeanceViewModelConsole(DateTime p_dateDebut, DateTime p_dateFin, string p_salle)
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
        public SeanceViewModelConsole(Seance p_seance) : this(p_seance.DateDebut, p_seance.DateFin, p_seance.Salle) { }
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
        public override string ToString()
        {
            CultureInfo cultureInfo= CultureInfo.CreateSpecificCulture("fr-CA");
            StringBuilder sb = new StringBuilder();

            sb.Append($"Le {this.m_dateDebut.ToString("dddd dd MMMM yyyy", cultureInfo)} ");
            sb.Append($"de {this.m_dateDebut.ToString("HH:mm", cultureInfo)}" );
            sb.Append($" à {this.m_dateFin.ToString("HH:mm", cultureInfo)}" );

            return sb.ToString();
        }
        public override bool Equals(object? obj)
        {
            return obj is SeanceViewModelConsole seance
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
    public class ProfesseurViewModelConsole
    {
        #region Membres
        private string m_nom;
        private string m_prenom;
        #endregion

        #region Ctor
        public ProfesseurViewModelConsole(string p_nom, string p_prenom)
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
        public ProfesseurViewModelConsole(Professeur p_enseignant) : this(p_enseignant.Nom, p_enseignant.Prenom) { }
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
        public override string ToString()
        {
            return $"{this.m_prenom} {this.m_nom}";
        }
        public override bool Equals(object? obj)
        {
            return obj is ProfesseurViewModelConsole professeur &&
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
