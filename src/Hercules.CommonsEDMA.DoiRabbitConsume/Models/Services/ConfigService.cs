using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gnoss.Web.ReprocessData.Models.Services
{
    public class ConfigService
    {
        private IConfiguration _configuration { get; set; }
        private string RabbitConnectionString { get; set; }
        private string rutaDirectorioEscritura { get; set; }
        private string DoiQueueRabbit { get; set; }
        private string urlPublicacion { get; set; }
        private string LogPath { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigService(IConfiguration configuration)
        {
            _configuration = configuration;
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
        /// Obtiene la el nombre de la cola Rabbit de configuración.
        /// </summary>
        /// <returns>Nombre de la cola Rabbit.</returns>
        public string GetQueueRabbit()
        {
            if (string.IsNullOrEmpty(DoiQueueRabbit))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                string queue = string.Empty;
                if (environmentVariables.Contains("DoiQueueRabbit"))
                {
                    queue = environmentVariables["DoiQueueRabbit"] as string;
                }
                else
                {
                    queue = _configuration["DoiQueueRabbit"];
                }
                DoiQueueRabbit = queue;
            }
            return DoiQueueRabbit;
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
        /// Obtiene la URL del API de Publicacion que ha sido configurada.
        /// </summary>
        /// <returns>URI del API de Publicacion.</returns>
        public string GetUrlPublicacion()
        {
            if (string.IsNullOrEmpty(urlPublicacion))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                string queue = string.Empty;
                if (environmentVariables.Contains("UrlPublicacion"))
                {
                    queue = environmentVariables["UrlPublicacion"] as string;
                }
                else
                {
                    queue = _configuration["UrlPublicacion"];
                }
                urlPublicacion = queue;
            }
            return urlPublicacion;
        }
    }
}
