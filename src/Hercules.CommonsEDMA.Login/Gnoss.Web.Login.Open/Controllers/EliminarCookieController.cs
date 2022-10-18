using Es.Riam.AbstractsOpen;
using Es.Riam.Gnoss.AD.EntityModel;
using Es.Riam.Gnoss.AD.EntityModelBASE;
using Es.Riam.Gnoss.AD.Virtuoso;
using Es.Riam.Gnoss.CL;
using Es.Riam.Gnoss.Util.Configuracion;
using Es.Riam.Gnoss.Util.General;
using Es.Riam.Web.Util;
using Gnoss.Web.Login.Open;
using Gnoss.Web.Login.Open.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Gnoss.Web.Login
{

    /// <summary>
    /// Página que elimina las cookies de todos los dominios en los que el usuario ha estado
    /// </summary>
    [Controller]
    [Route("[controller]")]
    public class EliminarCookieController : ControllerBaseLogin
    {
        readonly ConfigServiceLogin mConfigServiceSAML;
        public EliminarCookieController(LoggingService loggingService, IHttpContextAccessor httpContextAccessor, EntityContext entityContext, ConfigService configService, RedisCacheWrapper redisCacheWrapper, GnossCache gnossCache, VirtuosoAD virtuosoAD, IHostingEnvironment env, EntityContextBASE entityContextBASE, IServicesUtilVirtuosoAndReplication servicesUtilVirtuosoAndReplication, ConfigServiceLogin configServiceSAML)
            : base(loggingService, httpContextAccessor, entityContext, configService, redisCacheWrapper, gnossCache, virtuosoAD, env, entityContextBASE, servicesUtilVirtuosoAndReplication)
        {
            mConfigServiceSAML = configServiceSAML;
        }

        #region Métodos de eventos

        /// <summary>
        /// Método page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [HttpGet, HttpPost]
        public void Index()
        {
            string cookieUsuarioKey = "_UsuarioActual";
            string cookieEnvioKey = "_Envio";
            bool hayIframes = false;
            if (!Request.Headers.ContainsKey("eliminar") && !Request.Query.ContainsKey("eliminar"))
            {
                string dominio = "";

                if (Request.Headers.ContainsKey("dominio"))
                {
                    dominio = Request.Headers["dominio"];
                }
                else if (Request.Query.ContainsKey("dominio"))
                {
                    dominio = Request.Query["dominio"];
                }

                if (Request.Cookies.ContainsKey(cookieEnvioKey) || Request.Headers.ContainsKey("nuevoEnvio"))
                {
                    //Creo una cookie para saber que el resto de dominios ya han sido notificados
                    mHttpContextAccessor.HttpContext.Response.Cookies.Append(cookieEnvioKey, "true", new CookieOptions { Expires = DateTime.Now.AddDays(1) });

                    //El usuario se acaba de conectar, si habia estado en otros dominios, elimino su sesión
                    hayIframes = EliminarCookieRestoDominios(dominio);
                }

            }
            else
            {
                //Elimino la cookie del usuario actual conectado
                if (Request.Cookies.ContainsKey(cookieUsuarioKey))
                {
                    Response.Cookies.Append(cookieUsuarioKey, "0", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
                }

                string cookieRewriteKey = "_rewrite";

                //Elimino la cookie de rewrite
                if (Request.Cookies.ContainsKey(cookieRewriteKey))
                {
                    Response.Cookies.Append(cookieRewriteKey, "0", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
                }

                if (Request.Cookies.ContainsKey(cookieEnvioKey))
                {
                    Response.Cookies.Append(cookieEnvioKey, "0", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
                }

                if (Request.Cookies.ContainsKey("redireccion"))
                {
                    Response.Cookies.Append("redireccion", "0", new CookieOptions { Expires = DateTime.Now.AddDays(-1) });
                }

                string cookieTokenKey = "tokenDeVuelta";
                if (Request.Cookies.ContainsKey(cookieTokenKey))
                {
                    Response.Cookies.Append(cookieTokenKey, "0", new CookieOptions() { Expires = DateTime.Now.AddDays(-1) });
                }

                string usuarioLogueadoKey = "UsuarioLogueado";
                if (Request.Cookies.ContainsKey(usuarioLogueadoKey))
                {
                    CookieOptions cookieUsuarioLogueadoOptions = new CookieOptions
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };

                    string dominio = DominioAplicacion;
                    if (DominioAplicacion.IndexOf('.', 1) >= 0)
                    {
                        dominio = DominioAplicacion.Substring(DominioAplicacion.IndexOf('.', 1));
                    }
                    cookieUsuarioLogueadoOptions.Domain = dominio;

                    if (mHttpContextAccessor.HttpContext.Request.Scheme.Equals("https"))
                    {
                        cookieUsuarioLogueadoOptions.Secure = true;
                    }

                    Response.Cookies.Append(usuarioLogueadoKey, "0", cookieUsuarioLogueadoOptions);
                }

                Request.Path.ToString();
                string dominioPeticion = "";
                if (Request.Headers.ContainsKey("dominio"))
                {
                    dominioPeticion = Request.Headers["dominio"];
                }
                else if (Request.Query.ContainsKey("dominio"))
                {
                    dominioPeticion = Request.Query["dominio"];
                }
                hayIframes = EliminarCookieRestoDominios(dominioPeticion);
            }

            IDictionary environmentVariables = Environment.GetEnvironmentVariables();
            if (environmentVariables.Contains("Saml2_IdPMetadata"))
            {
                Response.Redirect(Url.Content(@$"~/{mConfigServiceSAML.GetUrlServiceInDomain()}Auth/Logout"));
            }
            else
            {
                if ((Request.Query.ContainsKey("redirect") || Request.Headers.ContainsKey("redirect")) && !hayIframes)
                {
                    if (Request.Headers.ContainsKey("redirect"))
                    {
                        Response.Redirect(Request.Headers["redirect"]);
                    }
                    else if (Request.Query.ContainsKey("redirect"))
                    {
                        Response.Redirect(Request.Query["redirect"]);
                    }

                }
            }
        }

        #endregion

        #region Métodos generales

        /// <summary>
        /// Método que elimina las cookies de todos los dominios
        /// </summary>
        [NonAction]
        private bool EliminarCookieRestoDominios(string pDominio)
        {
            bool hayIframes = false;
            Dictionary<string, string> dominios = (Dictionary<string, string>)UtilCookiesHercules.FromLegacyCookieString(Request.Cookies["_Dominios"], mEntityContext);

            if (pDominio.Contains("//www."))
            {
                pDominio = pDominio.Replace("//www.", "//");
            }
            string dominioSinHTTPS = pDominio.Replace("https://", "http://");
            string dominioConHTTPS = pDominio.Replace("http://", "https://");

            if ((dominios != null) && (dominios.Values.Count > 0))
            {
                //Recorre todos los dominios que hay en la cookie dominios y accede a la página eliminarCookie.aspx de cada uno de ellos, que elimina sus cookies
                foreach (string dominio in dominios.Values)
                {
                    if (string.IsNullOrEmpty(pDominio) || !(dominio.Equals(dominioSinHTTPS) || dominio.Equals(dominioConHTTPS)))
                    {
                        AgregarIframe(dominio);
                        hayIframes = true;
                    }
                }
            }

            if (Request.Headers.ContainsKey("eliminar") && Request.Headers["eliminar"].Equals("true"))
            {
                string cookieDominioLogoutExternoKey = "_DominioLogoutExterno";
                if (Request.Cookies.ContainsKey(cookieDominioLogoutExternoKey) && !string.IsNullOrEmpty(Request.Cookies[cookieDominioLogoutExternoKey]) && Uri.IsWellFormedUriString(Request.Cookies[cookieDominioLogoutExternoKey], UriKind.Absolute))
                {
                    // Si hay un dominio externo de login, la redirección se hará cuando se finalice la desconexión en este dominio
                    string onload = "";
                    if (Request.Headers.ContainsKey("redirect"))
                    {
                        onload = $"onload=\"document.location = '{Request.Headers["redirect"]}'\"";
                    }

                    Response.WriteAsync($"<IFRAME style='WIDTH:1px;HEIGHT:1px' src='{Request.Cookies[cookieDominioLogoutExternoKey]}' {onload} frameBorder='0'></IFRAME>");
                    hayIframes = true;
                }
            }
            return hayIframes;
        }
        [NonAction]
        private void AgregarIframe(string pUrl)
        {
            if (!string.IsNullOrEmpty(pUrl))
            {
                if (!pUrl.EndsWith("/"))
                {
                    pUrl += "/";
                }

                string parametros = "";
                string separador = "?";
                if (!Request.Headers.ContainsKey("eliminar"))
                {
                    parametros += "?login=1";
                    separador = "&";
                }

                if (Request.Headers.ContainsKey("usuarioID"))
                {
                    parametros += separador + "usuarioID=" + Request.Headers["usuarioID"];
                }

                Response.WriteAsync("<IFRAME style='WIDTH:1px;HEIGHT:1px' src='" + pUrl + "eliminarCookie.aspx" + parametros + "' frameBorder='0'></IFRAME>");
            }
        }

        #endregion
    }
}
