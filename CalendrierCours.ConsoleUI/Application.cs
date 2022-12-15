using CalendrierCours.BL;
using CalendrierCours.Entites;

namespace CalendrierCours.ConsoleUI
{
    public class Application
    {
        #region Membres
        private Traitement m_traitement;
        private List<CohorteViewModelConsole> m_cohortes;
        private CohorteViewModelConsole m_cohorteActive;
        private List<CoursViewModelConsole> m_CoursActifs;
        #endregion

        #region Ctor
        public Application(Traitement p_traitement)
        {
            if (p_traitement is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_traitement));
            }

            this.m_traitement = p_traitement;
            this.m_cohortes = this.m_traitement.ListerCohorte()
                .Select(c => new CohorteViewModelConsole(c))
                .ToList();
        }
        #endregion

        #region Proprietes

        #endregion

        #region Methodes
        public void Run()
        {
            this.InitialisationAffichage();

            this.AfficherCohorte();

            int choix = this.RetournerChoixUtilisateur($"Quelle cohorte ? (0/{this.m_cohortes.Count})", 0, m_cohortes.Count);

            this.AfficherCours(this.m_cohortes[choix - 1]);


        }
        private void AfficherCohorte()
        {
            int compteur = 1;
            this.m_cohortes.ForEach(cvm => Console.WriteLine($"{compteur++} - {cvm.ToString()}"));
        }
        private void AfficherCours(CohorteViewModelConsole p_cohorte)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }

            this.MiseAJourCohorteEtCoursActifs(p_cohorte);

            this.m_CoursActifs.ForEach(cvm => Console.WriteLine(cvm.ToString()));
        }
        private void AfficherSeances(Cohorte p_cohorte, DateTime p_date)
        {

        }

        private void InitialisationAffichage()
        {

        }

        private void MiseAJourCohorteEtCoursActifs(CohorteViewModelConsole p_cohorte)
        {
            if (p_cohorte is null)
            {
                throw new ArgumentNullException("Ne doit pas etre null", nameof(p_cohorte));
            }

            if (!p_cohorte.Equals(this.m_cohorteActive))
            {
                this.m_cohorteActive = p_cohorte;

                try
                {
                    this.m_CoursActifs = this.m_traitement.ListerCours(this.m_cohorteActive.VersEntite())
                        .Select(c => new CoursViewModelConsole(c))
                        .ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private int RetournerChoixUtilisateur(string p_message, int? p_min = null, int? p_max = null)
        {
            int retour;
            string choix = "";
            bool estValide = false;

            do
            {
                Console.WriteLine(p_message + " Appuyez sur 'Entrer' pour valider...");
                ConsoleKeyInfo keyInfo;
                do
                {
                    keyInfo = Console.ReadKey(true);
                    char saisie = keyInfo.KeyChar;

                    if (saisie >= '0' && saisie <= '9')
                    {
                        choix += saisie;
                        Console.Write(saisie);
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace && Console.CursorLeft > 0 && choix.Length > 0)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(new String(' ', Console.BufferWidth));
                        choix = choix.Remove(choix.Length - 1, 1);
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(choix);
                    }
                } while (keyInfo.Key != ConsoleKey.Enter);

                estValide = int.TryParse(choix, out retour);

                if (estValide && p_min is not null)
                {
                    estValide = retour > p_min;
                }
                if (estValide && p_max is not null)
                {
                    estValide = retour < p_max;
                }
                if (!estValide)
                {
                    Console.Out.WriteLine("\nSaisie incorrecte !");
                }
            } while (!estValide);

            return retour;
        }
        #endregion
    }
}
