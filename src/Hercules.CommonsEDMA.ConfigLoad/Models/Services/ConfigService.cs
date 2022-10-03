using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.IO;

namespace Hercules.CommonsEDMA.ConfigLoad.Models.Services
{
    public class ConfigService
    {
        private IConfiguration _configuration { get; set; }

        private string loginAdminEcosistema = "admin";
        private string nombreCortoComunidad = "hercules";

        private Guid metaProyectoID = new Guid("11111111-1111-1111-1111-111111111111");
        private string urlAPI { get; set; }
        private string urlAPIDespliegues { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public ConfigService()
        {
            _configuration = GetBuildConfiguration();
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

        public string ObtenerUrlAPI()
        {
            if (string.IsNullOrEmpty(urlAPI))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlAPI"))
                {
                    urlAPI = environmentVariables["urlAPI"] as string;
                }
                else
                {
                    urlAPI = _configuration["urlAPI"];
                }
            }
            return urlAPI;
        }

        public string ObtenerUrlAPIDespliegues()
        {
            if (string.IsNullOrEmpty(urlAPIDespliegues))
            {
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("urlAPIDespliegues"))
                {
                    urlAPIDespliegues = environmentVariables["urlAPIDespliegues"] as string;
                }
                else
                {
                    urlAPIDespliegues = _configuration["urlAPIDespliegues"];
                }
            }
            return urlAPIDespliegues;
        }



        public string ObtenerLoginAdminEcosistema()
        {
            return loginAdminEcosistema;
        }

        public Guid ObtenerMetaProyectoID()
        {
            return metaProyectoID;
        }

        public string ObtenerNombreCortoComunidad()
        {            
            return nombreCortoComunidad;
        }                
    }
}
