using CalendrierCours.Entites;
using System.Globalization;
using System.Text;

namespace CalendrierCours.WinFormUI
{
    public class CohorteViewModelWinForm
    {
        #region Membres
        private List<CoursViewModelWinForm> m_listeCours;
        private string m_numero;
        #endregion

        #region Ctor
        public CohorteViewModelWinForm(string p_numero)
        {
            if (String.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_numero));
            }

            Numero = p_numero;
            m_listeCours = new List<CoursViewModelWinForm>();
        }
        public CohorteViewModelWinForm(List<CoursViewModelWinForm> p_listeCours, string p_numero)
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
        public CohorteViewModelWinForm(Cohorte p_cohorte)
            : this(p_cohorte.Cours.Select(c => new CoursViewModelWinForm(c)).ToList(), p_cohorte.Numero)
        { }
        #endregion

        #region Proprietes
        public List<CoursViewModelWinForm> ListeCours
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
        public override string ToString()
        {
            int positionPeriode = 1, positionNumero = 2;
            string[] elementsCohorte = m_numero.Split('_');

            return $"Cohorte n° {elementsCohorte[positionNumero]} - période : {elementsCohorte[positionPeriode]}";
        }
        public override bool Equals(object? obj)
        {
            return obj is CohorteViewModelWinForm cohorte
                && cohorte.Numero == m_numero;
        }
        #endregion
    }
    public class CoursViewModelWinForm
    {
        #region Membres
        private ProfesseurViewModelWinForm m_enseignant;
        private List<SeanceViewModelWinForm> m_seances;
        private string m_intitule;
        private string m_numero;
        private string m_categorie;
        #endregion

        #region Ctor
        public CoursViewModelWinForm(ProfesseurViewModelWinForm p_enseignant, string p_numero, string p_intitule)
            : this(p_enseignant, p_numero, p_intitule, new List<SeanceViewModelWinForm>())
        { }
        public CoursViewModelWinForm(ProfesseurViewModelWinForm p_enseignant, string p_numero, string p_intitule
            , List<SeanceViewModelWinForm> p_seances)
        {
            if (p_enseignant is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_enseignant));
            }
            if (p_intitule is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_intitule));
            }
            if (p_seances is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_seances));
            }

            m_enseignant = p_enseignant;
            m_intitule = p_intitule;
            m_seances = p_seances;
            m_numero = p_numero;
            m_categorie = "";
        }
        public CoursViewModelWinForm(Cours p_cours)
            : this(new ProfesseurViewModelWinForm(p_cours.Enseignant), p_cours.Numero, p_cours.Intitule
                  , p_cours.Seances.Select(s => new SeanceViewModelWinForm(s)).ToList())
        { }
        #endregion

        #region Proprietes
        public ProfesseurViewModelWinForm Enseignant
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
        public string Categorie
        {
            get { return m_categorie; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Ne doit pas etre null", nameof(value));
                }

                m_categorie = value;
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
        public List<SeanceViewModelWinForm> Seances
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

            Cours cours = new Cours(m_enseignant.VersEntite(), m_numero, m_intitule, Seances);
            cours.Categorie = m_categorie;

            return cours;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            if (m_numero == String.Empty)
            {
                sb.Append($"Cours {m_intitule}");
            }
            else
            {
                sb.Append($"Cours n° {m_numero} - {m_intitule}");
            }

            return sb.ToString();
        }
        public override bool Equals(object? obj)
        {
            return obj is CoursViewModelWinForm cours
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
    public class SeanceViewModelWinForm
    {
        #region Membres
        private DateTime m_dateDebut;
        private DateTime m_dateFin;
        private string m_salle;
        private Guid m_uid;
        #endregion

        #region Ctor
        public SeanceViewModelWinForm(DateTime p_dateDebut, DateTime p_dateFin, string p_salle, Guid p_uid)
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
        public SeanceViewModelWinForm(Seance p_seance) :
            this(p_seance.DateDebut, p_seance.DateFin, p_seance.Salle, p_seance.UID)
        { }
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
        public override string ToString()
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("fr-CA");
            StringBuilder sb = new StringBuilder();

            sb.Append($"Le {m_dateDebut.ToString("dd-MM-yyyy", cultureInfo)} ");
            sb.Append($"de {m_dateDebut.ToString("HH:mm", cultureInfo)}");
            sb.Append($" à {m_dateFin.ToString("HH:mm", cultureInfo)}");

            return sb.ToString();
        }
        public override bool Equals(object? obj)
        {
            return obj is SeanceViewModelWinForm seance
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
    public class ProfesseurViewModelWinForm
    {
        #region Membres
        private string m_nom;
        private string m_prenom;
        #endregion

        #region Ctor
        public ProfesseurViewModelWinForm(string p_nom, string p_prenom)
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
        public ProfesseurViewModelWinForm(Professeur p_enseignant) : this(p_enseignant.Nom, p_enseignant.Prenom) { }
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
        public override string ToString()
        {
            return $"{m_prenom} {m_nom}";
        }
        public override bool Equals(object? obj)
        {
            return obj is ProfesseurViewModelWinForm professeur &&
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
