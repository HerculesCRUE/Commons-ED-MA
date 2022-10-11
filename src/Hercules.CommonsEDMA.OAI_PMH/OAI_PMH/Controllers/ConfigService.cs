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
        /// Obtiene la URL base del API de obtención de Actividad Docente que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseToken()
        {
            if (string.IsNullOrEmpty(UrlBaseToken))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseToken"))
                {
                    connectionString = environmentVariables["UrlBaseToken"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseToken"];
                }

                UrlBaseToken = connectionString;
            }

            return UrlBaseToken;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención de Proyectos que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseProyecto()
        {
            if (string.IsNullOrEmpty(UrlBaseProyecto))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseProyecto"))
                {
                    connectionString = environmentVariables["UrlBaseProyecto"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseProyecto"];
                }

                UrlBaseProyecto = connectionString;
            }

            return UrlBaseProyecto;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención de Personas que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBasePersona()
        {
            if (string.IsNullOrEmpty(UrlBasePersona))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBasePersona"))
                {
                    connectionString = environmentVariables["UrlBasePersona"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBasePersona"];
                }

                UrlBasePersona = connectionString;
            }

            return UrlBasePersona;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención de Organizaciones que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseOrganizacion()
        {
            if (string.IsNullOrEmpty(UrlBaseOrganizacion))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseOrganizacion"))
                {
                    connectionString = environmentVariables["UrlBaseOrganizacion"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseOrganizacion"];
                }

                UrlBaseOrganizacion = connectionString;
            }

            return UrlBaseOrganizacion;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención de Estructura Organica que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseEstructuraOrganica()
        {
            if (string.IsNullOrEmpty(UrlBaseEstructuraOrganica))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseEstructuraOrganica"))
                {
                    connectionString = environmentVariables["UrlBaseEstructuraOrganica"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseEstructuraOrganica"];
                }

                UrlBaseEstructuraOrganica = connectionString;
            }

            return UrlBaseEstructuraOrganica;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención de Formacion Academica que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseFormacionAcademica()
        {
            if (string.IsNullOrEmpty(UrlBaseFormacionAcademica))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseFormacionAcademica"))
                {
                    connectionString = environmentVariables["UrlBaseFormacionAcademica"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseFormacionAcademica"];
                }

                UrlBaseFormacionAcademica = connectionString;
            }

            return UrlBaseFormacionAcademica;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención de Actividad Docente que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseActividadDocente()
        {
            if (string.IsNullOrEmpty(UrlBaseActividadDocente))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseActividadDocente"))
                {
                    connectionString = environmentVariables["UrlBaseActividadDocente"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseActividadDocente"];
                }

                UrlBaseActividadDocente = connectionString;
            }

            return UrlBaseActividadDocente;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención de Producción Científica que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseProduccionCientifica()
        {
            if (string.IsNullOrEmpty(UrlBaseProduccionCientifica))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseProduccionCientifica"))
                {
                    connectionString = environmentVariables["UrlBaseProduccionCientifica"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseProduccionCientifica"];
                }

                UrlBaseProduccionCientifica = connectionString;
            }

            return UrlBaseProduccionCientifica;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención de Autorizacines que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseAutorizacion()
        {
            if (string.IsNullOrEmpty(UrlBaseAutorizacion))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseAutorizacion"))
                {
                    connectionString = environmentVariables["UrlBaseAutorizacion"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseAutorizacion"];
                }

                UrlBaseAutorizacion = connectionString;
            }

            return UrlBaseAutorizacion;
        }


        /// <summary>
        /// Obtiene la URL base del API de obtención de Invenciones que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseInvenciones()
        {
            if (string.IsNullOrEmpty(UrlBaseInvenciones))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseInvenciones"))
                {
                    connectionString = environmentVariables["UrlBaseInvenciones"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseInvenciones"];
                }

                UrlBaseInvenciones = connectionString;
            }

            return UrlBaseInvenciones;
        }

        /// <summary>
        /// Obtiene la URL base del API de obtención de Grupos que ha sido configurada.
        /// </summary>
        /// <returns></returns>
        public string GetUrlBaseGrupos()
        {
            if (string.IsNullOrEmpty(UrlBaseGrupos))
            {
                string connectionString = string.Empty;
                IDictionary environmentVariables = Environment.GetEnvironmentVariables();
                if (environmentVariables.Contains("UrlBaseGrupos"))
                {
                    connectionString = environmentVariables["UrlBaseGrupos"] as string;
                }
                else
                {
                    connectionString = configuracion["UrlBaseGrupos"];
                }

                UrlBaseGrupos = connectionString;
            }

            return UrlBaseGrupos;
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
