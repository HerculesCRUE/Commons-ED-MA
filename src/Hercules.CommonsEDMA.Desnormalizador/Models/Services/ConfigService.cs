using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.Desnormalizador.Models.Services
{
    public class ConfigService
    {
        private IConfiguration _configuration { get; set; }
        private string RabbitConnectionString { get; set; }
        private string DenormalizerQueueRabbit { get; set; }
        private string rutaDirectorioEscritura { get; set; }
        private string denormalizerCronExpression { get; set; }
        private string UrlSimilarity { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Contruye el objeto de lectura con la configuración del JSON.
        /// </summary>
        /// <returns></returns>
        public static IConfiguration GetBuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            return builder.Build();
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
                    rabbitConnectionString = _configuration.GetConnectionString("RabbitMQ");
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
                    queue = _configuration["DenormalizerQueueRabbit"];
                }
                DenormalizerQueueRabbit = queue;
            }
            return DenormalizerQueueRabbit;
        }

        /// <summary>
        /// Obtiene la ruta de escritura de los ficheros.
        /// </summary>
        /// <returns>Ruta de escritura.</returns>
        public string GetRutaDirectorioEscritura()
        {
            if (string.IsNullOrEmpty(rutaDirectorioEscritura))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("DirectorioEscritura"))
                {
                    connectionString = environmentVariables["DirectorioEscritura"] as string;
                }
                else
                {
                    connectionString = _configuration["DirectorioEscritura"];
                }

                rutaDirectorioEscritura = connectionString;
            }

            return rutaDirectorioEscritura;
        }

        /// <summary>
        /// Obtiene la expresión CRON para la ejecución del desnormalizador
        /// </summary>
        /// <returns>Ruta de escritura.</returns>
        public string GetDenormalizerCronExpression()
        {
            if (string.IsNullOrEmpty(denormalizerCronExpression))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("DenormalizerCronExpression"))
                {
                    connectionString = environmentVariables["DenormalizerCronExpression"] as string;
                }
                else
                {
                    connectionString = _configuration["DenormalizerCronExpression"];
                }

                denormalizerCronExpression = connectionString;
            }

            return denormalizerCronExpression;
        }

        /// <summary>
        /// Obtiene la url del servicio de similaridad
        /// </summary>
        /// <returns>Ruta de escritura.</returns>
        public string GetUrlSimilarity()
        {
            if (string.IsNullOrEmpty(UrlSimilarity))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlSimilarity"))
                {
                    connectionString = environmentVariables["UrlSimilarity"] as string;
                }
                else
                {
                    connectionString = _configuration["UrlSimilarity"];
                }

                UrlSimilarity = connectionString;
            }

            return UrlSimilarity;
        }
    }
}
