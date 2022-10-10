using Es.Riam.AbstractsOpen;
using Es.Riam.Gnoss.AD.EntityModel;
using Es.Riam.Gnoss.AD.EntityModelBASE;
using Es.Riam.Gnoss.AD.ParametroAplicacion;
using Es.Riam.Gnoss.AD.Virtuoso;
using Es.Riam.Gnoss.CL;
using Es.Riam.Gnoss.Util.Configuracion;
using Es.Riam.Gnoss.Util.General;
using Es.Riam.Web.Util;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Gnoss.Web.Login.Open.Controllers
{
    [Controller]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class RefrescarCookieController : ControllerBaseLogin
    {
        public RefrescarCookieController(LoggingService loggingService, IHttpContextAccessor httpContextAccessor, EntityContext entityContext, ConfigService configService, RedisCacheWrapper redisCacheWrapper, GnossCache gnossCache, VirtuosoAD virtuosoAD, IHostingEnvironment env, EntityContextBASE entityContextBASE, IServicesUtilVirtuosoAndReplication servicesUtilVirtuosoAndReplication)
            : base(loggingService, httpContextAccessor, entityContext, configService, redisCacheWrapper, gnossCache, virtuosoAD, env, entityContextBASE, servicesUtilVirtuosoAndReplication)
        {
        }
        [HttpGet, HttpPost]
        public void Index()
        {
            if (mHttpContextAccessor.HttpContext.Request.Cookies.ContainsKey("_UsuarioActual"))
            {
                //establezco la validez inicial de la cookie que será de 1 día o indefinida si el usuario quiere mantener su sesión activa
                DateTime caduca = ObtenerValidezCookieUsuario();
                //obtengo las cookies
                Dictionary<string, string> cookie = UtilCookiesHercules.FromLegacyCookieString(Request.Cookies["_UsuarioActual"], mEntityContext);
                Response.Cookies.Append("_UsuarioActual", UtilCookiesHercules.ToLegacyCookieString(cookie, mEntityContext), new CookieOptions { Expires = caduca });
            }
        }
        /// <summary>
        /// Obtiene la validez inicial de la cookie del usuario
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private DateTime ObtenerValidezCookieUsuario()
        {
            //establezco la validez inicial de la cookie que será de 1 día o indefinida si el usuario quiere mantener su sesión activa
            DateTime caduca = DateTime.Now.AddDays(1);
            List<ParametroAplicacion> filas = ParametrosAplicacionDS.Where(parametroApp => parametroApp.Parametro.Equals(TiposParametrosAplicacion.DuracionCookieUsuario)).ToList();
            if (filas != null && filas.Count > 0)
            {
                string duracion = (string)ParametrosAplicacionDS.Find(parametroApp => parametroApp.Parametro.Equals(TiposParametrosAplicacion.DuracionCookieUsuario)).Valor;
                if (!string.IsNullOrEmpty(duracion))
                {
                    string letra = duracion.Substring(duracion.Length - 1).ToLower();
                    string digitos = duracion.Substring(0, duracion.Length - 1);
                    int cantidad;
                    if (int.TryParse(digitos, out cantidad) && cantidad > 0)
                    {
                        switch (letra)
                        {
                            case "d":
                                caduca = DateTime.Now.AddDays(cantidad);
                                break;
                            case "h":
                                caduca = DateTime.Now.AddHours(cantidad);
                                break;
                            case "m":
                                caduca = DateTime.Now.AddMinutes(cantidad);
                                break;
                            default:
                                caduca = DateTime.Now.AddDays(1);
                                break;
                        }
                    }
                }
            }
            return caduca;
        }

        
    }
}