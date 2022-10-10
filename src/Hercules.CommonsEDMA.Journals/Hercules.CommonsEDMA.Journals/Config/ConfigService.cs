using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.Journals.Config
{
    public class ConfigService
    {
        // Archivo de configuración.
        public static IConfigurationRoot configuracion;

        private string rutaDatos { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConfigService()
        {
            configuracion = new ConfigurationBuilder().AddJsonFile($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config/appsettings.json").Build();
        }

        /// <summary>
        /// Obtiene la ruta de lectura de los ficheros.
        /// </summary>
        /// <returns>Ruta de lectura.</returns>
        public string GetRutaDatos()
        {
            if (string.IsNullOrEmpty(rutaDatos))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("RutaDatos"))
                {
                    connectionString = environmentVariables["RutaDatos"] as string;
                }
                else
                {
                    connectionString = configuracion["RutaDatos"];
                }

                rutaDatos = connectionString;
            }

            return rutaDatos;
        }
    }
}
