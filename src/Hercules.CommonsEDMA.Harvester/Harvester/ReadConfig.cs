using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester
{
    public class ReadConfig
    {
        // Archivo de configuración.
        public static IConfigurationRoot configuracion;

        private string urlOaipmh { get; set; }

        // Rutas.
        private string dirLogCargas { get; set; }
        private string lastUpdateDateFile { get; set; }
        private string cronExpression { get; set; }

        // Configuración Rabbit para el desnormalizador
        private string RabbitConnectionString { get; set; }
        private string DenormalizerQueueRabbit { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReadConfig()
        {
            configuracion = new ConfigurationBuilder().AddJsonFile($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}appsettings.json").Build();
        }

        /// <summary>
        /// Obtiene la URL del servidor OAI-PMH.
        /// </summary>
        /// <returns></returns>
        public string GetUrlOaiPmh()
        {
            if (string.IsNullOrEmpty(urlOaipmh))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("Url_OAI-PMH"))
                {
                    connectionString = environmentVariables["Url_OAI-PMH"] as string;
                }
                else
                {
                    connectionString = configuracion["Url_OAI-PMH"];
                }

                urlOaipmh = connectionString;
            }

            return urlOaipmh;
        }

        /// <summary>
        /// Obtiene la ruta dónde se van a almacenar los IDs cargados.
        /// </summary>
        /// <returns>Ruta.</returns>
        public string GetLogCargas()
        {
            if (string.IsNullOrEmpty(dirLogCargas))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("DirLogCarga"))
                {
                    connectionString = environmentVariables["DirLogCarga"] as string;
                }
                else
                {
                    connectionString = configuracion["DirLogCarga"];
                }

                dirLogCargas = connectionString;
            }

            return dirLogCargas;
        }

        /// <summary>
        /// Obtiene la ruta del fichero de la última fecha de modificación.
        /// </summary>
        /// <returns>Ruta.</returns>
        public string GetLastUpdateDate()
        {
            if (string.IsNullOrEmpty(lastUpdateDateFile))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("LastUpdateDateFile"))
                {
                    connectionString = environmentVariables["LastUpdateDateFile"] as string;
                }
                else
                {
                    connectionString = configuracion["LastUpdateDateFile"];
                }

                lastUpdateDateFile = connectionString;
            }

            return lastUpdateDateFile;
        }

        /// <summary>
        /// Obtiene la expresión cron configurada.
        /// </summary>
        /// <returns>Ruta.</returns>
        public string GetCronExpression()
        {
            if (string.IsNullOrEmpty(cronExpression))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("CronExpression"))
                {
                    connectionString = environmentVariables["CronExpression"] as string;
                }
                else
                {
                    connectionString = configuracion["CronExpression"];
                }

                cronExpression = connectionString;
            }

            return cronExpression;
        }

        /// <summary>
        /// Obtiene la cadena de conexión de Rabbit configurada.
        /// </summary>
        /// <returns>Cadena de conexión de Rabbit.</returns>
        public string GetrabbitConnectionString()
        {
            if (string.IsNullOrEmpty(RabbitConnectionString))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                string rabbitConnectionString = string.Empty;
                if (environmentVariables.Contains("RabbitMQ"))
                {
                    rabbitConnectionString = environmentVariables["RabbitMQ"] as string;
                }
                else
                {
                    rabbitConnectionString = configuracion.GetConnectionString("RabbitMQ");
                }
                RabbitConnectionString = rabbitConnectionString;
            }
            return RabbitConnectionString;
        }

        /// <summary>
        /// Obtiene la el nombre de la cola Rabbit de desnormalización de configuración.
        /// </summary>
        /// <returns>Nombre de la cola Rabbit.</returns>
        public string GetDenormalizerQueueRabbit()
        {
            if (string.IsNullOrEmpty(DenormalizerQueueRabbit))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                string queue = string.Empty;
                if (environmentVariables.Contains("DenormalizerQueueRabbit"))
                {
                    queue = environmentVariables["DenormalizerQueueRabbit"] as string;
                }
                else
                {
                    queue = configuracion["DenormalizerQueueRabbit"];
                }
                DenormalizerQueueRabbit = queue;
            }
            return DenormalizerQueueRabbit;
        }
    }
}
