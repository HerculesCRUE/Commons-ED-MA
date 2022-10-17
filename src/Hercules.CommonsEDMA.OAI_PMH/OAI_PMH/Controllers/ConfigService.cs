using Microsoft.Extensions.Configuration;
using System;
using System.Collections;

namespace OAI_PMH.Controllers
{
    /// <summary>
    /// ConfigService
    /// </summary>
    public class ConfigService
    {
        /// <summary>
        /// Archivo de configuración.
        /// </summary>
        public static IConfigurationRoot configuracion;

        // Credenciales
        private string UsernameToken { get; set; }
        private string PasswordToken { get; set; }
        private string UsernameTokenPII { get; set; }
        private string PasswordTokenPII { get; set; }

        // URLs
        private string url_sgi { get; set; }
        private string ConfigUrl { get; set; }
        private string UrlBaseToken { get; set; }
        private string UrlBaseProyecto { get; set; }
        private string UrlBasePersona { get; set; }
        private string UrlBaseOrganizacion { get; set; }
        private string UrlBaseEstructuraOrganica { get; set; }
        private string UrlBaseFormacionAcademica { get; set; }
        private string UrlBaseActividadDocente { get; set; }
        private string UrlBaseProduccionCientifica { get; set; }
        private string UrlBaseAutorizacion { get; set; }
        private string UrlBaseInvenciones { get; set; }
        private string UrlBaseGrupos { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConfigService()
        {
            configuracion = new ConfigurationBuilder().AddJsonFile($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}appsettings.json").Build();
        }

        /// <summary>
        /// Obtiene la URL base del API del SGI configurada.
        /// </summary>
        /// <returns></returns>
        public string GetConfigSGI()
        {
            if (string.IsNullOrEmpty(url_sgi))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("url_sgi"))
                {
                    connectionString = environmentVariables["url_sgi"] as string;
                }
                else
                {
                    connectionString = configuracion["url_sgi"];
                }

                url_sgi = connectionString;
            }

            return url_sgi;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención del OAI-PMH que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetConfigUrl()
        {
            if (string.IsNullOrEmpty(ConfigUrl))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("ConfigUrl"))
                {
                    connectionString = environmentVariables["ConfigUrl"] as string;
                }
                else
                {
                    connectionString = configuracion["ConfigUrl"];
                }

                ConfigUrl = connectionString;
            }

            return ConfigUrl;
        }

        /// <summary>
        /// Obtiene el user del token del API que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUsernameToken()
        {
            if (string.IsNullOrEmpty(UsernameToken))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UsernameToken"))
                {
                    connectionString = environmentVariables["UsernameToken"] as string;
                }
                else
                {
                    connectionString = configuracion["UsernameToken"];
                }

                UsernameToken = connectionString;
            }

            return UsernameToken;
        }

        /// <summary>
        /// Obtiene el password del token del API que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetPasswordToken()
        {
            if (string.IsNullOrEmpty(PasswordToken))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("PasswordToken"))
                {
                    connectionString = environmentVariables["PasswordToken"] as string;
                }
                else
                {
                    connectionString = configuracion["PasswordToken"];
                }

                PasswordToken = connectionString;
            }

            return PasswordToken;
        }

        /// <summary>
        /// Obtiene el user del token del API que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUsernamePIIToken()
        {
            if (string.IsNullOrEmpty(UsernameTokenPII))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UsernameTokenPII"))
                {
                    connectionString = environmentVariables["UsernameTokenPII"] as string;
                }
                else
                {
                    connectionString = configuracion["UsernameTokenPII"];
                }

                UsernameTokenPII = connectionString;
            }

            return UsernameTokenPII;
        }

        /// <summary>
        /// Obtiene el password del token del API que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetPasswordPIIToken()
        {
            if (string.IsNullOrEmpty(PasswordTokenPII))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("PasswordTokenPII"))
                {
                    connectionString = environmentVariables["PasswordTokenPII"] as string;
                }
                else
                {
                    connectionString = configuracion["PasswordTokenPII"];
                }

                PasswordTokenPII = connectionString;
            }

            return PasswordTokenPII;
        }
    }
}
