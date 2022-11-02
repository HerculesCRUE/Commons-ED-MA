using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.IO;

namespace Hercules.CommonsEDMA.Journals.Config
{
    public class ConfigService
    {
        // Archivo de configuración.
        public static readonly IConfigurationRoot configuracion = new ConfigurationBuilder().AddJsonFile($@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}appsettings.json").Build();

        private string RutaDatos { get; set; }

        /// <summary>
        /// Obtiene la ruta de lectura de los ficheros.
        /// </summary>
        /// <returns>Ruta de lectura.</returns>
        public string GetRutaDatos()
        {
            if (string.IsNullOrEmpty(RutaDatos))
            {
                string connectionString;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("RutaDatos"))
                {
                    connectionString = environmentVariables["RutaDatos"] as string;
                }
                else
                {
                    connectionString = configuracion["RutaDatos"];
                }

                RutaDatos = connectionString;
            }

            return RutaDatos;
        }
    }
}
