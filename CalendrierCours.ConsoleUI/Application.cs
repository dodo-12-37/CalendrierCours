using CalendrierCours.BL;
using CalendrierCours.Entites;
using System;

namespace CalendrierCours.ConsoleUI
{
    public class Application
    {
        #region Membres
        private Traitement m_traitement;
        private List<CohorteViewModelConsole> m_cohortes;
        private CohorteViewModelConsole m_cohorteActive;
        private Dictionary<int, string> m_menuPrincipal;
        private Dictionary<int, string> m_menuExportCours;
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

            this.InitialiserMenus();
        }
        #endregion

        #region Proprietes

        #endregion

        #region Methodes
        public void Run()
        {
            int choix = -1;
            this.InitialisationAffichage();

            do
            {
                Console.Clear();
                Console.WriteLine("Récupération des horaires de cours\n");
                if (this.m_cohorteActive is null)
                {
                    Console.WriteLine("Pas de cohorte choisie.\n");
                }
                else
                {
                    Console.WriteLine(this.m_cohorteActive.ToString() + "\n");
                }

                this.AfficherMenu(this.m_menuPrincipal);

                choix = this.RetournerChoixUtilisateur("Que voulez-vous faire ?", this.m_menuPrincipal.Keys.ToList());

                switch (choix)
                {
                    case 1:
                        this.ChoisirCohorte();
                        break;
                    case 2:
                        this.AfficherCours();
                        break;
                    case 3:
                        this.ModifierCours();
                        break;
                    case 4:
                        this.ModifierEnseignant();
                        break;
                    case 5:
                        this.ExporterCours();
                        break;
                    case 0:
                        Console.WriteLine("\nAu revoir !");
                        break;
                    default:
                        break;
                }









            } while (choix != 0);

        }
        private void InitialiserMenus()
        {
            this.m_menuPrincipal = new Dictionary<int, string>
            {
                { 1, "Choisir une cohorte" },
                { 2, "Afficher les cours de la cohorte" },
                { 3, "Modifier l'intitulé d'un cours" },
                { 4, "Modifier un enseignant" },
                { 5, "Exporter des cours" },
                { 0, "Quitter" }
            };
            this.m_menuExportCours = new Dictionary<int, string>()
            {
                {1, "Format ICalendar (cvs)" }
            };
        }
        private void InitialisationAffichage()
        {
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
        }

        private void AfficherMenu(Dictionary<int, string> p_menu)
        {
            foreach (KeyValuePair<int, string> ligne in p_menu)
            {
                Console.WriteLine($"{ligne.Key} - {ligne.Value}");
            }
        }
        private void AfficherCohortes()
        {
            int compteur = 1;
            this.m_cohortes.ForEach(cvm => Console.WriteLine($"{compteur++} - {cvm.ToString()}"));
        }
        private void AfficherCours()
        {
            Console.Clear();

            if (this.VerifierCohorteActive())
            {
                Console.WriteLine($"Cours de la cohorte {this.m_cohorteActive.ToString()} :");
                this.m_cohorteActive.ListeCours.ForEach(cvm => Console.WriteLine(cvm.ToString()));
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadLine();
            }
        }

        private void ChoisirCohorte()
        {
            Console.Clear();
            Console.WriteLine("Voici les cohortes disponibles :");
            this.AfficherCohortes();
            int choix = this.RetournerChoixUtilisateur($"Quelle cohorte voulez-vous ? (0/{this.m_cohortes.Count})", 0, m_cohortes.Count);

            if (choix != -1)
            {
                try
                {
                    Console.WriteLine("\nRécupération de la cohorte...");
                    this.MiseAJourCohorteActiveDepuisDepot(this.m_cohortes[choix - 1]);
                    Console.WriteLine("Récupération réussie !");
                    Console.WriteLine("Appuyez sur une touche pour continuer...");
                    Console.ReadLine();
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Erreur lors de la récupération de la cohorte.");
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }

        }
        private void ModifierCours()
        {
            Console.Clear();

            if (this.VerifierCohorteActive())
            {
                int choix, compteur = 1;
                Console.WriteLine($"Cours présents dans la cohorte {this.m_cohorteActive.ToString()} :");

                this.m_cohorteActive.ListeCours
                    .ForEach(cvm => Console.WriteLine($"{compteur++} - {cvm.Intitule}"));

                choix = this.RetournerChoixUtilisateur("Quel intitulé de cours voulez-vous modifier ?", 1, this.m_cohorteActive.ListeCours.Count);

                if (choix != -1)
                {
                    CoursViewModelConsole coursAModifier = this.m_cohorteActive.ListeCours[choix - 1];
                    Console.WriteLine($"\nChangement de l'intitulé du cours {coursAModifier.Intitule}");
                    string nvIntilute = this.RetournerSaisieUtilisateur("Quel est le nouvel intitulé ?");

                    Console.WriteLine("Modifications en cours...");
                    this.m_traitement.ModifierIntituleCours(this.m_cohorteActive.VersEntite(), coursAModifier.VersEntites(), nvIntilute);
                    bool estReussi = this.MiseAJoursCohorteActiveDepuisTraitement();

                    if (estReussi)
                    {
                        Console.WriteLine("Modifications effectuées !");
                    }

                    Console.WriteLine("Appuyez sur une toucher pour continuer...");
                    Console.ReadLine();
                }
            }
        }
        private void ModifierEnseignant()
        {
            Console.Clear();

            if (this.VerifierCohorteActive())
            {
                int choix, compteur = 1;
                Console.WriteLine($"Enseignants présents dans la cohorte {this.m_cohorteActive.ToString()}");

                List<ProfesseurViewModelConsole> profs = this.m_cohorteActive.ListeCours
                    .Select(c => c.Enseignant)
                    .Distinct()
                    .ToList();

                profs.ForEach(p => Console.WriteLine($"{compteur++} - {p.ToString()}"));

                choix = this.RetournerChoixUtilisateur("Quel enseignant voulez-vous modifier ?", 1, profs.Count);

                if (choix != -1)
                {
                    ProfesseurViewModelConsole profAModifier = profs[choix - 1];
                    Console.WriteLine($"\nModification de : {profAModifier.ToString()}");
                    Console.WriteLine("1 - Nom\n2 - Prénom");
                    choix = this.RetournerChoixUtilisateur("Que voulez-vous modifier ?", 1, 2);

                    if (choix != -1)
                    {
                        string modification = "";
                        bool estReussi;

                        if (choix == 1)
                        {
                            modification = this.RetournerSaisieUtilisateur("\nQuel est le nouveau nom ?");

                            Console.WriteLine("Modification en cours...");
                            this.m_traitement.ModifierNomProfesseur(this.m_cohorteActive.VersEntite(), profAModifier.VersEntite(), modification);
                            estReussi = this.MiseAJoursCohorteActiveDepuisTraitement();
                        }
                        else
                        {
                            modification = this.RetournerSaisieUtilisateur("\nQuel est le nouveau prénom ?");

                            Console.WriteLine("Modification en cours...");
                            this.m_traitement.ModifierPrenomProfesseur(this.m_cohorteActive.VersEntite(), profAModifier.VersEntite(), modification);
                            estReussi = this.MiseAJoursCohorteActiveDepuisTraitement();
                        }

                        if (estReussi)
                        {
                            Console.WriteLine("Modifications effectuées !");
                        }

                        Console.WriteLine("Appuyez sur une toucher pour continuer...");
                        Console.ReadLine();
                    }
                }
            }
        }

        private void ExporterCours()
        {
            Console.Clear();

            if (this.VerifierCohorteActive())
            {
                int choix;
                Console.WriteLine($"Exportation des cours de la cohorte {this.m_cohorteActive.ToString()}");

                choix = this.RetournerChoixUtilisateur("Dans quel format voulez-vous exporter ?", this.m_menuExportCours.Keys.ToList());

                if (choix != -1)
                {
                    
                    string chemin = this.RetournerSaisieUtilisateur("À quel endroit voulez-vous enregistrez le fichier ?");
                    
                   

                    if (choix == 1)
                    {
                        Console.WriteLine("Exportation en cours...");
                    }

                    Console.WriteLine("Appuyez sur une toucher pour continuer...");
                    Console.ReadLine();
                }
            }
        }

        private bool MiseAJoursCohorteActiveDepuisTraitement()
        {
            bool estReussi = false;
            CohorteViewModelConsole cohorteTemp = new CohorteViewModelConsole(this.m_cohorteActive.ListeCours, this.m_cohorteActive.Numero);
            try
            {
                cohorteTemp.ListeCours = this.m_traitement.ListerCours(this.m_cohorteActive.VersEntite())
                    .Select(c => new CoursViewModelConsole(c))
                    .ToList();
                estReussi = true;
            }
            catch (Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Erreur lors de la modification !");
                Console.ForegroundColor = ConsoleColor.Black;
            }

            if (estReussi)
            {
                this.m_cohorteActive.ListeCours.Clear();
                this.m_cohorteActive.ListeCours = cohorteTemp.ListeCours;
            }

            return estReussi;
        }
        private void MiseAJourCohorteActiveDepuisDepot(CohorteViewModelConsole p_cohorte)
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
                    this.m_cohorteActive.ListeCours = this.m_traitement.ListerCours(this.m_cohorteActive.VersEntite())
                        .Select(c => new CoursViewModelConsole(c))
                        .ToList();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        private bool VerifierCohorteActive()
        {
            bool estNull = this.m_cohorteActive is null;

            if (estNull)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Veuillez choisir une cohorte avant d'afficher les cours !");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Appuyez sur une touche pour continuer...");
                Console.ReadLine();
            }

            return !estNull;
        }
        private int RetournerChoixUtilisateur(string p_message, int? p_min = null, int? p_max = null)
        {
            int retour = -1;
            string choix = "";
            bool estValide = false;

            do
            {
                Console.WriteLine(p_message + " Appuyez sur 'Entrer' pour valider, 'Echap' pour annuler...");
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
                } while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

                if (keyInfo.Key != ConsoleKey.Escape)
                {
                    estValide = int.TryParse(choix, out retour);

                    if (estValide && p_min is not null)
                    {
                        estValide = retour >= p_min;
                    }
                    if (estValide && p_max is not null)
                    {
                        estValide = retour <= p_max;
                    }
                    if (!estValide)
                    {
                        choix = "";
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Error.WriteLine("\nSaisie incorrecte !");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                }
                else
                {
                    estValide = true;
                }
            } while (!estValide);

            return retour;
        }
        private int RetournerChoixUtilisateur(string p_message, List<int> p_choixPossibles)
        {
            int retour = -1;
            string choix = "";
            bool estValide = false;

            do
            {
                Console.WriteLine(p_message + " Appuyez sur 'Entrer' pour valider, 'Echap' pour annuler...");
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
                } while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape);

                if (keyInfo.Key != ConsoleKey.Escape)
                {
                    estValide = int.TryParse(choix, out retour);
                    estValide = estValide && p_choixPossibles.Contains(retour);

                    if (!estValide)
                    {
                        choix = "";
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Error.WriteLine("\nSaisie incorrecte !");
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                }
                else
                {
                    estValide = true;
                }
            } while (!estValide);

            return retour;
        }
        private string RetournerSaisieUtilisateur(string p_message)
        {
            bool estValide = false;
            string retour = "";

            do
            {
                Console.WriteLine(p_message);
                retour = Console.ReadLine().Trim();

                if (String.IsNullOrWhiteSpace(retour))
                {
                    estValide = false;
                }
                else
                {
                    estValide = true;
                }

                if (!estValide)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("\nSaisie incorrecte !");
                    Console.ForegroundColor = ConsoleColor.Black;
                }

            } while (!estValide);

            return retour;
        }
        #endregion
    }
}
