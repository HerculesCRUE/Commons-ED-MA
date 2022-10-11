using Es.Riam.AbstractsOpen;
using Es.Riam.Gnoss.AD.EncapsuladoDatos;
using Es.Riam.Gnoss.AD.EntityModel;
using Es.Riam.Gnoss.AD.EntityModelBASE;
using Es.Riam.Gnoss.AD.ParametroAplicacion;
using Es.Riam.Gnoss.AD.Usuarios;
using Es.Riam.Gnoss.AD.Virtuoso;
using Es.Riam.Gnoss.CL;
using Es.Riam.Gnoss.CL.ParametrosAplicacion;
using Es.Riam.Gnoss.Elementos.Identidad;
using Es.Riam.Gnoss.Elementos.ParametroAplicacion;
using Es.Riam.Gnoss.Logica.ServiciosGenerales;
using Es.Riam.Gnoss.Logica.Usuarios;
using Es.Riam.Gnoss.Recursos;
using Es.Riam.Gnoss.Util.Configuracion;
using Es.Riam.Gnoss.Util.General;
using Es.Riam.Gnoss.Web.Controles;
using Es.Riam.Gnoss.Web.Controles.ServiciosGenerales;
using Es.Riam.Util;
using Es.Riam.Web.Util;
using Gnoss.Web.Login.Open.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Web;
using System.Xml;

namespace Gnoss.Web.Login
{

    /// <summary>
    /// Clase base para las páginas
    /// </summary>
    public class ControllerBaseLogin : Controller
    {

        #region Miembros

        private string mDominio = null;

        private UtilIdiomas mUtilIdiomas;

        private string mUrlStatics = null;

        /// <summary>
        /// Fila de parámetros de aplicación
        /// </summary>
        private List<ParametroAplicacion> mParametrosAplicacionDS;

        /// <summary>
        /// Obtiene si en este ecosistema hay que validar el registro o no.
        /// </summary>
        private bool? mRegistroAutomaticoEcosistema = null;

        protected LoggingService mLoggingService;
        protected IHttpContextAccessor mHttpContextAccessor;
        protected EntityContext mEntityContext;
        protected ConfigService mConfigService;
        protected RedisCacheWrapper mRedisCacheWrapper;
        protected ControladorBase mControladorBase;
        protected IHostingEnvironment mEnv;
        protected GnossCache mGnossCache;
        protected EntityContextBASE mEntityContextBASE;
        protected VirtuosoAD mVirtuosoAD;
        protected IServicesUtilVirtuosoAndReplication mServicesUtilVirtuosoAndReplication;

        #endregion

        public ControllerBaseLogin(LoggingService loggingService, IHttpContextAccessor httpContextAccessor, EntityContext entityContext, ConfigService configService, RedisCacheWrapper redisCacheWrapper, GnossCache gnossCache, VirtuosoAD virtuosoAD, IHostingEnvironment env, EntityContextBASE entityContextBASE, IServicesUtilVirtuosoAndReplication servicesUtilVirtuosoAndReplication)
        {
            mLoggingService = loggingService;
            mHttpContextAccessor = httpContextAccessor;
            mConfigService = configService;
            mRedisCacheWrapper = redisCacheWrapper;
            mEntityContext = entityContext;
            mEnv = env;
            mGnossCache = gnossCache;
            mEntityContextBASE = entityContextBASE;
            mVirtuosoAD = virtuosoAD;
            mServicesUtilVirtuosoAndReplication = servicesUtilVirtuosoAndReplication;
            mControladorBase = new ControladorBase(loggingService, configService, entityContext, redisCacheWrapper, gnossCache, virtuosoAD, httpContextAccessor, mServicesUtilVirtuosoAndReplication);
        }

        #region Metodos de eventos

        /// <summary>
        /// Método page load
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected virtual void Page_Load()
        {
            //TODO Migrar a .net core
            //this.Error += new EventHandler(GnossServicioLoginPage_Error);

            //Cabeceras para poder recibir cookies de terceros
            mHttpContextAccessor.HttpContext.Response.Headers.Add("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");
        }
        protected string ObtenerDominioIP()
        {
            string path = null;
            if (Request.Path.HasValue)
            {
                path = Request.Path.Value.Substring(0, Request.Path.Value.LastIndexOf("/"));
            }

            string dominio = $"https://{Request.Host}{path}";
            return dominio;
        }
        /// <summary>
        /// Controlador de los errores
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void GnossServicioLoginPage_Error(object sender, EventArgs e)
        {
            //Captura el error y lo escribe en el log
            //TODO Migrar a .net core
            /*Exception ex = this.Context.Error;

            string error = "Programa:   " + ex.Source + Environment.NewLine + Environment.NewLine + "Espacio de nombres:   " + ex.TargetSite.DeclaringType.Namespace + Environment.NewLine + Environment.NewLine + "Metodo:   " + ex.TargetSite.Name + Environment.NewLine + Environment.NewLine + "Tipo de excepción: " + ex.GetType().ToString() + Environment.NewLine + Environment.NewLine + "Mensaje:   " + ex.Message + Environment.NewLine + Environment.NewLine + "Pila de llamadas:" + Environment.NewLine + ex.StackTrace;

            mLoggingService.AgregarEntrada("Error:" + this.Context.Error.Message + " Pila: " + this.Context.Error.StackTrace.Replace("\r\n", " "));
            mLoggingService.GuardarTraza(ObtenerRutaTraza());

            //Guardo el error en el log
            mLoggingService.GuardarLogError(error);*/
        }

        #endregion

        #region Metodos generales

        protected Guid mPersonaID = Guid.Empty;
        protected string mNombreCorto = "";
        protected Guid mUsuarioID = Guid.Empty;
        protected bool mMantenerConectado = false;
        protected string mLogin = "";
        protected string mIdioma = "";
        protected bool mEsProyectoPrivado = false;

        /// <summary>
        /// Valida nombre, contraseña y codigo de verificación del usuario
        /// </summary>
        /// <param name="pNombre">Nombre del usuario</param>
        /// <param name="pContrasenia">Contraseña de la cuenta</param>
        /// /// <param name="pCodigoVerificacion">Código verificación para la validación en 2 pasos</param>
        /// <returns>TRUE si usuario, contraseña y codigo de verificación  son correctos, FALSE en caso contrario</returns>
        [NonAction]
        public bool ValidarUsuario(string pNombre, string pContrasenia, string pCodigoVerificacion, bool estavalidado = true)
        {
            return ValidarUsuario(pNombre, pContrasenia, pCodigoVerificacion, false, estavalidado);
        }

        /// <summary>
        /// Valida nombre, contraseña y codigo de verificación del usuario
        /// </summary>
        /// <param name="pNombre">Nombre del usuario</param>
        /// <param name="pContrasenia">Contraseña de la cuenta</param>
        /// <param name="pCodigoVerificacion">Código verificación para la validación en 2 pasos</param>
        /// <returns>TRUE si usuario y contraseña y codigo de verificación son correctos, FALSE en caso contrario</returns>
        [NonAction]
        public bool ValidarUsuario(string pNombre, string pContrasenia, string pCodigoVerificacion, bool pIgnorarPassword, bool estavalidado)
        {
            try
            {
                // Autenticamos el login para la organización (autenticación parcial)
                Configuracion.ObtenerDesdeFicheroConexion = true;
                UsuarioCN usuarioCN = new UsuarioCN("acid", mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
                DataWrapperUsuario dataWrapperUsuario = new DataWrapperUsuario();

                if (!string.IsNullOrEmpty(pNombre))
                {
                    dataWrapperUsuario = usuarioCN.AutenticarLogin(pNombre, false);
                }

                Es.Riam.Gnoss.AD.EntityModel.Models.UsuarioDS.Usuario filaUsuario = null;

                if (dataWrapperUsuario.ListaUsuario.Count > 0)
                {
                    filaUsuario = dataWrapperUsuario.ListaUsuario.First();
                }

                //Autenticamos la password (autenticación completa)
                if (!pIgnorarPassword && !usuarioCN.ValidarPasswordUsuarioSinActivar(filaUsuario, pContrasenia))
                {
                    return false;
                }


                PersonaCN personaCN = new PersonaCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
                Es.Riam.Gnoss.AD.EntityModel.Models.PersonaDS.Persona filaPersona = personaCN.ObtenerPersonaPorUsuario(filaUsuario.UsuarioID).ListaPersona.FirstOrDefault();

                mUsuarioID = filaUsuario.UsuarioID;
                mPersonaID = filaPersona.PersonaID;
                mLogin = filaUsuario.Login;
                mIdioma = filaPersona.Idioma;
                mNombreCorto = filaUsuario.NombreCorto;
                mMantenerConectado = false;

                if (estavalidado)
                {
                    LoguearUsuario(filaUsuario.UsuarioID, mPersonaID, mNombreCorto, mLogin, mIdioma);
                }


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        [NonAction]
        protected void LoguearUsuario(Guid pUsuarioID, Guid pPersonaID, string pNombreCorto, string pLogin, string pIdioma)
        {
            UsuarioCN usuarioCN = new UsuarioCN("acid", mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
            // Usuario logueado correctamente, actualizo el contador de accesos
            usuarioCN.ActualizarContadorUsuarioNumAccesos(pUsuarioID);

            usuarioCN.Dispose();

            CrearCookieUsuarioActual(pUsuarioID.ToString(), pLogin, pIdioma, DominioAplicacion);
            CrearCookiePerfiles(pPersonaID, pNombreCorto, DominioAplicacion);
        }


        private ControladorIdentidades mControladorIdentidades;

        protected ControladorIdentidades ControladorIdentidades
        {
            get
            {
                if (mControladorIdentidades == null)
                {
                    mControladorIdentidades = new ControladorIdentidades(new GestionIdentidades(new DataWrapperIdentidad(), mLoggingService, mEntityContext, mConfigService, mServicesUtilVirtuosoAndReplication), mLoggingService, mEntityContext, mConfigService, mRedisCacheWrapper, mGnossCache, mEntityContextBASE, mVirtuosoAD, mHttpContextAccessor, mServicesUtilVirtuosoAndReplication);
                }
                return mControladorIdentidades;
            }
        }


        /// <summary>
        /// Valida nombre y contraseña del usuario (pendiente de activar)
        /// </summary>
        /// <param name="pNombre">Nombre del usuario</param>
        /// <param name="pContrasenia">Contraseña de la cuenta</param>
        /// <returns>TRUE si usuario y contraseña son correctos, FALSE en caso contrario</returns>
        [NonAction]
        public bool ValidarUsuarioPendienteDeActivar(string pNombre, string pContrasenia)
        {
            UsuarioCN usuarioCN = new UsuarioCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);

            Es.Riam.Gnoss.AD.EntityModel.Models.UsuarioDS.Usuario filaUsuario = usuarioCN.ObtenerFilaUsuarioPorLoginOEmail(pNombre);
            GestorParametroAplicacion gestorParametroAplicacion = new GestorParametroAplicacion();
            gestorParametroAplicacion.ParametroAplicacion = ParametrosAplicacionDS;
            JsonEstado estado = ControladorIdentidades.AccionEnServicioExternoEcosistema(TipoAccionExterna.LoginConRegistro, Guid.Empty, filaUsuario.UsuarioID, "", "", pNombre, pContrasenia, gestorParametroAplicacion, null, null, null, null);

            if (estado != null)
            {
                try
                {
                    //Si no se loguea en el servicio pero devuelve usuario (está sin activar)
                    JsonUsuario jsonNuevoUsuario = JsonConvert.DeserializeObject<JsonUsuario>(estado.InfoExtra);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                if (filaUsuario != null && usuarioCN.ValidarPasswordUsuarioSinActivar(filaUsuario, pContrasenia))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        [NonAction]
        protected void RedireccionarADominioDeOrigen(string pUrlVuelta, string pRedirect, string pToken, string pEliminarCookie)
        {
            string query = "login=0";

            bool estaLogueado = false;

            try
            {
                string queryRerwrite = "";
                if (Request.Query.Count > 1)
                {
                    //Parte de la cookie de perfiles
                    if (!mPersonaID.Equals(Guid.Empty))
                    {
                        queryRerwrite = "&personaID=" + mPersonaID + "&nombreCorto=" + HttpUtility.UrlEncode(mNombreCorto);
                    }

                    //Enviamos las cookies
                    if (!string.IsNullOrEmpty(pUrlVuelta))
                    {
                        if (!mUsuarioID.Equals(Guid.Empty))
                        {
                            //Obtengo los valores de la cookie de usuario actual
                            query = "usuarioID=" + mUsuarioID + "&MantenerConectado=" + mMantenerConectado + "&loginUsuario=" + HttpUtility.UrlEncode(mLogin) + "&idioma=" + mIdioma + queryRerwrite;

                            estaLogueado = true;
                        }

                        while (pUrlVuelta.EndsWith("/"))
                        {
                            pUrlVuelta = pUrlVuelta.Remove(pUrlVuelta.Length - 1);
                        }


                        if (string.IsNullOrEmpty(pRedirect))
                        {
                            pRedirect = pUrlVuelta;
                        }

                        query += "&token=" + pToken;

                        if (!string.IsNullOrEmpty(pEliminarCookie))
                        {
                            query += pEliminarCookie;
                        }

                        query += "&redirect=" + HttpUtility.UrlEncode(pRedirect);
                    }
                }
            }
            catch (Exception ex)
            {
                mLoggingService.GuardarLogError(ex);
            }

            if (!string.IsNullOrEmpty(pUrlVuelta) && !pUrlVuelta.EndsWith("/"))
            {
                pUrlVuelta += "/";
            }

            AgregarDominio(pUrlVuelta);

            if (estaLogueado)
            {
                //La envío al otro dominio
                Response.Redirect(pUrlVuelta + "crearCookie?" + query);
            }
            else
            {
                Response.Redirect(pRedirect);
            }
        }

        /// <summary>
        /// Agrega el dominio a la cookie de dominios
        /// </summary>
        /// <param name="pDominio">Dominio nuevo</param>
        [NonAction]
        private void AgregarDominio(string pDominio)
        {
            CookieOptions options = new CookieOptions();
            Dictionary<string, string> cookieValues = new Dictionary<string, string>();

            //Cabeceras para poder recibir cookies de terceros
            mHttpContextAccessor.HttpContext.Response.Headers.Add("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

            if (!string.IsNullOrEmpty(pDominio) && UtilCookiesHercules.FromLegacyCookieString(Request.Cookies["_Dominios"], mEntityContext).ContainsKey("pDominio"))
            {
                //Quito www.
                if (pDominio.Contains("//www."))
                {
                    pDominio = pDominio.Replace("//www.", "//");
                }

                cookieValues.Add(pDominio, "conectado");
            }

            //establezco la validez de la cookie que será de 1 día
            options.Expires = DateTime.Now.AddDays(1);
            mHttpContextAccessor.HttpContext.Response.Cookies.Append("_Dominios", UtilCookiesHercules.ToLegacyCookieString(cookieValues, mEntityContext), options);
        }

        /// <summary>
        /// Método para crear la cookie del usuario que se acaba de registrar en un dominio
        /// </summary>
        /// <param name="pUsuarioID">Identificador del usuario</param>
        /// <param name="pLogin">0 si el usuario se ha desconectado, 1 si está logueado</param>
        /// <param name="pIdioma">Idioma con el que se conecta el usuario</param>
        /// <param name="pHashQuery">Diccionario de los parámetros de la petición actual</param>
        [NonAction]
        protected bool CrearCookieUsuarioActual(string pUsuarioID, string pLogin, string pIdioma, string pDominioAplicacion)
        {
            bool existe = true;
            //Creo la cookie para este usuario
            CookieOptions cookieUsuarioOptions = new CookieOptions();

            if (!mHttpContextAccessor.HttpContext.Request.Cookies.ContainsKey("_UsuarioActual"))
            {
                existe = false;
            }

            //establezco la validez inicial de la cookie que será de 1 día o indefinida si el usuario quiere mantener su sesión activa
            DateTime caduca = ObtenerValidezCookieUsuario();

            //Cabeceras para poder recibir cookies de terceros
            mHttpContextAccessor.HttpContext.Response.Headers.Add("p3p", "CP=\"IDC DSP COR ADM DEVi TAIi PSA PSD IVAi IVDi CONi HIS OUR IND CNT\"");

            Dictionary<string, string> cookieUsuarioValues = new Dictionary<string, string>();
            cookieUsuarioValues.Add("usuarioID", pUsuarioID);
            cookieUsuarioValues.Add("loginUsuario", pLogin);
            cookieUsuarioValues.Add("MantenerConectado", false.ToString());
            cookieUsuarioValues.Add("login", "1");
            cookieUsuarioValues.Add("idioma", pIdioma);

            //Añado la cookie al navegador
            cookieUsuarioOptions.Expires = caduca;
            cookieUsuarioOptions.SameSite = SameSiteMode.None;
            cookieUsuarioOptions.Secure = true;
            mHttpContextAccessor.HttpContext.Response.Cookies.Append("_UsuarioActual", UtilCookiesHercules.ToLegacyCookieString(cookieUsuarioValues, mEntityContext), cookieUsuarioOptions);

            CookieOptions usuarioLogueadoOptions = new CookieOptions();

            // Creo la cookie para que accedan todos los subdominios del dominio principal. 
            // Ej: servicios.didactalia.net -> .didactalia.net, servicios.gnoss.com -> .gnoss.com
            // De esta manera la cookie llega a didactalia.net, red.didactalia.net, gnoss.com, red.gnoss.com, redprivada.gnoss.com...
            string dominio = pDominioAplicacion;
            if (pDominioAplicacion.IndexOf('.', 1) >= 0)
            {
                dominio = pDominioAplicacion.Substring(pDominioAplicacion.IndexOf('.', 1));
            }
            usuarioLogueadoOptions.Domain = dominio;
            usuarioLogueadoOptions.Expires = caduca;

            if (mHttpContextAccessor.HttpContext.Request.Scheme.Equals("https"))
            {
                usuarioLogueadoOptions.Secure = true;
            }

            mHttpContextAccessor.HttpContext.Response.Cookies.Append("UsuarioLogueado", "0", usuarioLogueadoOptions);

            return existe;
        }

        /// <summary>
        /// Crea la cookie de perfiles y de rewrite
        /// </summary>
        /// <param name="pPersonaID">Identificador de la persona asociada al usuario logueado</param>
        /// <param name="pNombreCortoUsuario">Nombre corto del usuario</param>
        [NonAction]
        protected void CrearCookiePerfiles(Guid pPersonaID, string pNombreCortoUsuario, string pDominioAplicacion)
        {
            //Creo las cookies de perfiles y rewrite
            RiamDiccionarioSerializable<Guid, Guid> mListaIdentidadProyecto = new RiamDiccionarioSerializable<Guid, Guid>();
            RiamDiccionarioSerializable<string, Guid> mListaOrganizaciones = new RiamDiccionarioSerializable<string, Guid>();
            RiamDiccionarioSerializable<Guid, Guid> mListaIdentidadOrgMyGnoss = new RiamDiccionarioSerializable<Guid, Guid>();

            CookieOptions cookieRewriteoptions = new CookieOptions();
            cookieRewriteoptions.Expires = ObtenerValidezCookieUsuario();

            Dictionary<string, string> cookieRewriteValues = new Dictionary<string, string>();
            cookieRewriteValues.Add("personaID", pPersonaID.ToString());
            cookieRewriteValues.Add("nombreCorto", pNombreCortoUsuario);

            StringWriter sw = new StringWriter();
            XmlTextWriter xmlWriter = new XmlTextWriter(sw);
            mListaIdentidadProyecto.WriteXml(xmlWriter);
            xmlWriter.Flush();
            cookieRewriteValues.Add("ListaIdentidadProyecto", sw.ToString());
            xmlWriter.Close();
            xmlWriter = null;
            sw = null;

            sw = new StringWriter();
            xmlWriter = new XmlTextWriter(sw);
            mListaOrganizaciones.WriteXml(xmlWriter);
            xmlWriter.Flush();
            cookieRewriteValues.Add("ListaOrganizaciones", sw.ToString());
            xmlWriter.Close();
            xmlWriter = null;
            sw = null;

            sw = new StringWriter();
            xmlWriter = new XmlTextWriter(sw);
            mListaIdentidadOrgMyGnoss.WriteXml(xmlWriter);
            xmlWriter.Flush();
            cookieRewriteValues.Add("ListaIdentidadOrgMyGnoss", sw.ToString());
            xmlWriter.Close();
            xmlWriter = null;
            sw = null;

            //Actualizo la cookie de rewrite
            mHttpContextAccessor.HttpContext.Response.Cookies.Append("_rewrite", UtilCookiesHercules.ToLegacyCookieString(cookieRewriteValues, mEntityContext), cookieRewriteoptions);
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


        #endregion

        #region Propiedades

        public UtilIdiomas UtilIdiomas
        {
            get
            {
                if (mUtilIdiomas == null)
                {
                    mUtilIdiomas = new UtilIdiomas("es", mLoggingService, mEntityContext, mConfigService);
                }
                return mUtilIdiomas;
            }
        }

        /// <summary>
        /// Obtiene la URL de la aplicación principal para este servicio de login
        /// </summary>
        public string UrlAplicacionPrincipal
        {
            get
            {
                string url = null;

                try
                {
                    url = mConfigService.ObtenerUrlBase();
                }
                catch (Exception) { }

                return url;
            }
        }

        public string BaseURLIdioma
        {
            get
            {
                string url = UrlAplicacionPrincipal;
                if (UtilIdiomas.LanguageCode != "es")
                {
                    url += UtilIdiomas.LanguageCode;
                }
                return url;
            }
        }

        /// <summary>
        /// Obtiene el dominio de esta aplicación
        /// </summary>
        public string DominioAplicacion
        {
            get
            {
                if (mDominio == null)
                {
                    mDominio = UtilDominios.ObtenerDominioUrl($"{mHttpContextAccessor.HttpContext.Request.Scheme}://{mHttpContextAccessor.HttpContext.Request.Host.Value}", false);
                }

                return mDominio;
            }
        }

        public string UrlStatics
        {
            get
            {
                if (mUrlStatics == null)
                {
                    mUrlStatics = mConfigService.ObtenerUrlServicio("urlStatic");
                }
                return mUrlStatics;
            }
        }

        public bool RegistroAutomaticoEcosistema
        {
            get
            {
                if (!mRegistroAutomaticoEcosistema.HasValue)
                {
                    List<ParametroAplicacion> listaParametrosAplicacion = ParametrosAplicacionDS.Where(parametroApp => parametroApp.Parametro.Equals("RegistroAutomaticoEcosistema")).ToList();
                    mRegistroAutomaticoEcosistema = listaParametrosAplicacion.Count > 0 && bool.Parse(listaParametrosAplicacion[0].Valor);
                }
                return mRegistroAutomaticoEcosistema.Value;
            }
        }

        /// <summary>
        /// Obtiene el dataset de parámetros de aplicación
        /// </summary>
        public List<ParametroAplicacion> ParametrosAplicacionDS
        {
            get
            {
                if (mParametrosAplicacionDS == null)
                {
                    ParametroAplicacionCL paramCL = new ParametroAplicacionCL(mEntityContext, mLoggingService, mRedisCacheWrapper, mConfigService, mServicesUtilVirtuosoAndReplication);
                    mParametrosAplicacionDS = paramCL.ObtenerParametrosAplicacionPorContext();
                }
                return mParametrosAplicacionDS;
            }
        }

        #endregion

        #region Miembros de ICallbackEventHandler

        /*TODO Migrar a .net core

        string _callbackArg = null;

        void ICallbackEventHandler.RaiseCallbackEvent(string eventArgument)
        {
            _callbackArg = eventArgument;
        }

        string ICallbackEventHandler.GetCallbackResult()
        {
            try
            {
                string resultado = RaiseCallbackEvent(_callbackArg);
                mHttpContextAccessor.HttpContext.Session.Remove("listaResultados");

                mLoggingService.GuardarTraza(ObtenerRutaTraza());

                return resultado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected string ObtenerRutaTraza()
        {
            string ruta = Path.Combine(mEnv.ContentRootPath, "trazas");
            //string ruta = this.Server.MapPath(Page.Request.ApplicationPath + "\\trazas");

            if (!string.IsNullOrEmpty(mControladorBase.DominoAplicacion))
            {
                ruta += "\\" + mControladorBase.DominoAplicacion;
                if (!Directory.Exists(ruta))
                {
                    Directory.CreateDirectory(ruta);
                }
            }

            ruta += "\\traza_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            return ruta;
        }

        public string RaiseCallbackEvent(string eventArgument)
        {
            return eventArgument;
        }
        */
        #endregion
    }
}
