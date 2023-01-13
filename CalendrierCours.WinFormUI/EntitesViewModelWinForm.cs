using CalendrierCours.Entites;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

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

            this.Numero = p_numero;
            this.m_listeCours = new List<CoursViewModelWinForm>();
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

            this.m_listeCours = p_listeCours;
            this.m_numero = p_numero;
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
            return obj is CohorteViewModelWinForm cohorte
                && cohorte.Numero == this.m_numero;
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
            if (String.IsNullOrWhiteSpace(p_intitule))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_intitule));
            }
            if (p_seances is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_seances));
            }

            Regex formatNumero = new Regex(this.RecupereFormatNumero());
            if (!formatNumero.IsMatch(p_numero))
            {
                throw new FormatException("Le numero n'est pas au bon format");
            }

            this.m_enseignant = p_enseignant;
            this.m_intitule = p_intitule;
            this.m_seances = p_seances;
            this.m_numero = p_numero;
            this.m_categorie = "";
        }
        public CoursViewModelWinForm(Cours p_cours)
            : this(new ProfesseurViewModelWinForm(p_cours.Enseignant), p_cours.Numero, p_cours.Intitule
                  , p_cours.Seances.Select(s => new SeanceViewModelWinForm(s)).ToList())
        { }
        #endregion

        #region Proprietes
        public ProfesseurViewModelWinForm Enseignant
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
        public string Categorie
        {
            get { return this.m_categorie; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(value));
                }

                this.m_categorie = value;
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

                Regex formatNumero = new Regex(this.RecupereFormatNumero());
                if (!formatNumero.IsMatch(value))
                {
                    throw new FormatException("Le numero n'est pas au bon format");
                }

                this.m_numero = value;
            }
        }
        public List<SeanceViewModelWinForm> Seances
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

            Cours cours = new Cours(this.m_enseignant.VersEntite(), this.m_numero, this.m_intitule, Seances);
            cours.Categorie = this.m_categorie;

            return cours;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"Cours n° {this.Numero} - {this.Intitule} ");

            return sb.ToString();
        }
        public override bool Equals(object? obj)
        {
            return obj is CoursViewModelWinForm cours
                && this.Enseignant.Equals(cours.Enseignant)
                && Numero == cours.Numero
                && Intitule == cours.Intitule;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Enseignant, Intitule, Numero);
        }

        private IConfigurationRoot LireFichierConfig()
        {
            IConfigurationRoot? configuration;

            try
            {
                configuration =
                    new ConfigurationBuilder()
                      .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                      .AddJsonFile("appsettings.json", false)
                      .Build();
            }
            catch (Exception e)
            {
                throw new InvalidDepotException("Le fichier de configuration est corrompu", e);
            }

            return configuration;
        }
        private string RecupereFormatNumero()
        {
            string? retour;
            IConfigurationRoot configuration = this.LireFichierConfig();

            if (configuration is null)
            {
                throw new Exception("Erreur dans la lecture du fichier de configuration");
            }

            retour = configuration["formatNumeroCours"];

            if (retour is null)
            {
                throw new Exception("Erreur dans la lecture du fichier de configuration");
            }

            return retour;
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
            if (String.IsNullOrWhiteSpace(p_salle))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide");
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
        public SeanceViewModelWinForm(Seance p_seance) :
            this(p_seance.DateDebut, p_seance.DateFin, p_seance.Salle, p_seance.UID)
        { }
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
        public Guid UID { get { return this.m_uid; } }
        #endregion

        #region Methodes
        public Seance VersEntite()
        {
            return new Seance(this.m_dateDebut, this.m_dateFin, this.m_salle, this.m_uid);
        }
        public override string ToString()
        {
            CultureInfo cultureInfo = CultureInfo.CreateSpecificCulture("fr-CA");
            StringBuilder sb = new StringBuilder();

            sb.Append($"Le {this.m_dateDebut.ToString("dd-MM-yyyy", cultureInfo)} ");
            sb.Append($"de {this.m_dateDebut.ToString("HH:mm", cultureInfo)}");
            sb.Append($" à {this.m_dateFin.ToString("HH:mm", cultureInfo)}");

            return sb.ToString();
        }
        public override bool Equals(object? obj)
        {
            return obj is SeanceViewModelWinForm seance
                && seance.UID == this.UID
                && seance.DateDebut == this.DateDebut
                && seance.DateFin == this.DateFin
                && seance.Salle == this.Salle;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(this.m_uid, this.m_dateDebut, this.m_dateFin, this.m_salle);
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
        public ProfesseurViewModelWinForm(Professeur p_enseignant) : this(p_enseignant.Nom, p_enseignant.Prenom) { }
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
