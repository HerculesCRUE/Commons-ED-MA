using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.IO;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    public class ConfigService
    {
        // Archivo de configuración.
        public static IConfigurationRoot configuracion;
        private string RabbitConnectionString { get; set; }
        private string FuentesExternasQueueRabbit { get; set; }
        private string DenormalizerQueueRabbit { get; set; }
        private string DoiQueueRabbit { get; set; }
        private string UrlSimilarity { get; set; }
        private string UrlPublicacion { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConfigService()
        {
            configuracion = new ConfigurationBuilder().AddJsonFile($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}appsettings.json").Build();
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
        public string GetRabbitConnectionString()
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
        /// Obtiene la el nombre de la cola Rabbit de configuración.
        /// </summary>
        /// <returns>Nombre de la cola Rabbit.</returns>
        public string GetFuentesExternasQueueRabbit()
        {
            if (string.IsNullOrEmpty(FuentesExternasQueueRabbit))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                string queue = "";
                if (environmentVariables.Contains("FuentesExternasQueueRabbit"))
                {
                    queue = environmentVariables["FuentesExternasQueueRabbit"] as string;
                }
                else
                {
                    queue = configuracion["FuentesExternasQueueRabbit"];
                }
                FuentesExternasQueueRabbit = queue;
            }
            return FuentesExternasQueueRabbit;
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

        /// <summary>
        /// Obtiene la el nombre de la cola Rabbit de configuración.
        /// </summary>
        /// <returns>Nombre de la cola Rabbit.</returns>
        public string GetDoiQueueRabbit()
        {
            if (string.IsNullOrEmpty(DoiQueueRabbit))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                string queue = "";
                if (environmentVariables.Contains("DoiQueueRabbit"))
                {
                    queue = environmentVariables["DoiQueueRabbit"] as string;
                }
                else
                {
                    queue = configuracion["DoiQueueRabbit"];
                }
                DoiQueueRabbit = queue;
            }
            return DoiQueueRabbit;
        }

        /// <summary>
        /// Obtiene la URL del servicio de similitud
        /// </summary>
        /// <returns>Url del servicio</returns>
        public string GetUrlSimilarity()
        {
            if (string.IsNullOrEmpty(UrlSimilarity))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                string url = "";
                if (environmentVariables.Contains("UrlSimilarity"))
                {
                    url = environmentVariables["UrlSimilarity"] as string;
                }
                else
                {
                    url = configuracion["UrlSimilarity"];
                }
                UrlSimilarity = url;
            }
            return UrlSimilarity;
        }

        /// <summary>
        /// Obtiene la URL del servicio de similitud
        /// </summary>
        /// <returns>Url del servicio</returns>
        public string GetUrlPublicacion()
        {
            if (string.IsNullOrEmpty(UrlPublicacion))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                string url = "";
                if (environmentVariables.Contains("UrlPublicacion"))
                {
                    url = environmentVariables["UrlPublicacion"] as string;
                }
                else
                {
                    url = configuracion["UrlPublicacion"];
                }
                UrlPublicacion = url;
            }
            return UrlPublicacion;
        }
    }
}
