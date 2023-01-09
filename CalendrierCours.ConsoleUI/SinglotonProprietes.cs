using CalendrierCours.Entites;
using Microsoft.Extensions.Configuration;
using System;

namespace CalendrierCours.ConsoleUI
{
    public class SinglotonProprietes
        : IProprietes
    {
        private static SinglotonProprietes _instance;
        private static object _lock = new object();
        private IConfigurationRoot? m_proprietes = null;

        public SinglotonProprietes()
        {
            if (_instance is null)
            {
                lock (_lock)
                {
                    if (_instance is null)
                    {
                        _instance = this;
                        _instance.m_proprietes = _instance.LireFichierConfig();
                    }
                }
            }
        }

        public string this[string p_nomPropriete]
        {
            get
            {
                if (String.IsNullOrWhiteSpace(p_nomPropriete))
                {
                    throw new ArgumentNullException("Ne doit pas etre null ou vide", nameof(p_nomPropriete));
                }

                string retour;

                lock (_lock)
                {
                    retour = _instance.m_proprietes[p_nomPropriete];
                }

                if (retour is null)
                {
                    throw new Exception($"Le parametre \"{p_nomPropriete}\" n'est pas dans le fichier de configuration");
                }

                return retour;
            }
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
    }
}
