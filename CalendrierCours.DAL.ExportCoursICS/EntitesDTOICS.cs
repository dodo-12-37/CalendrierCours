using CalendrierCours.Entites;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.RegularExpressions;

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
            : this(p_enseignant, p_numero, p_intitule, null, null, new List<SeanceICSDTO>())
        { }
        public CoursICSDTO
            (ProfesseurICSDTO p_enseignant, string p_numero, string p_intitule, string? p_description, string? p_categorie, List<SeanceICSDTO> p_seances)
        {
            if (p_enseignant is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_enseignant));
            }
            if (String.IsNullOrWhiteSpace(p_intitule))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_intitule));
            }
            if (String.IsNullOrWhiteSpace(p_numero))
            {
                throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_numero));
            }
            if (p_seances is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_seances));
            }

            Regex regexNumeroCours = new Regex(this.RecupereFormatNumero());

            if (!regexNumeroCours.IsMatch(p_numero))
            {
                throw new FormatException("Le format du numero ne correspond pas");
            }

            this.m_intitule = p_intitule;
            this.m_numero = p_numero;
            this.m_enseignant = p_enseignant;
            this.m_seances = p_seances;
            this.m_categorie = p_categorie;
            this.m_description = p_description;
        }
        public CoursICSDTO(Cours p_cours)
            : this(
                  new ProfesseurICSDTO(p_cours.Enseignant)
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
        public List<SeanceICSDTO> Seances
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

                Regex format = new Regex(this.RecupereFormatNumero());
                if (!format.IsMatch(value))
                {
                    throw new FormatException("Le numero n'est pas au bon format");
                }

                this.m_intitule = value;
            }
        }
        public string? Description { get { return this.m_description; } set { this.m_description = value; } }
        public string? Categorie { get { return this.m_categorie; } set { this.m_categorie = value; } }
        #endregion

        #region Methodes
        public Cours VersEntites()
        {
            List<Seance> Seances = this.m_seances.Select(s => s.VersEntite()).ToList();

            return new Cours(this.m_enseignant.VersEntite(), this.Numero, this.m_intitule, Seances);
        }
        public override bool Equals(object? obj)
        {
            return obj is CoursICSDTO cours
                && this.Enseignant.Equals(cours.Enseignant)
                && Numero == cours.Numero
                && Intitule == cours.Intitule;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Enseignant, Intitule);
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
    public class SeanceICSDTO
    {
        #region Membres
        private DateTime m_dateDebut;
        private DateTime m_dateFin;
        private string m_salle;
        private Guid m_uid;
        #endregion

        #region Ctor
        public SeanceICSDTO(DateTime p_dateDebut, DateTime p_dateFin, string p_salle)
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
            this.m_uid = Guid.NewGuid();
        }
        public SeanceICSDTO(Seance p_seance) : this(p_seance.DateDebut, p_seance.DateFin, p_seance.Salle) { }
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
            return new Seance(this.m_dateDebut, this.m_dateFin, this.m_salle);
        }
        public override bool Equals(object? obj)
        {
            return obj is SeanceICSDTO seance
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
    public class ProfesseurICSDTO
    {
        #region Membres
        private string m_nom;
        private string m_prenom;
        #endregion

        #region Ctor
        public ProfesseurICSDTO(string p_nom, string p_prenom)
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
        public ProfesseurICSDTO(Professeur p_enseigant) : this(p_enseigant.Nom, p_enseigant.Prenom) { }
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
