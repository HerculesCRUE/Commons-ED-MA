extern alias ApiWrapper;


using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Web;
using Gnoss.Web.Login.SAML.Models;
using Es.Riam.Gnoss.Util.General;
using Es.Riam.Gnoss.AD.EntityModel;
using Es.Riam.Gnoss.Util.Configuracion;
using Es.Riam.Gnoss.CL;
using Es.Riam.Gnoss.AD.Virtuoso;
using Microsoft.AspNetCore.Hosting;
using Es.Riam.Gnoss.AD.EntityModelBASE;
using Es.Riam.Gnoss.Logica.ServiciosGenerales;
using Es.Riam.Gnoss.Logica.Usuarios;
using Es.Riam.AbstractsOpen;
using Es.Riam.Util;
using ApiWrapper::Gnoss.ApiWrapper;
using ApiWrapper::Gnoss.ApiWrapper.ApiModel;
using ApiWrapper::Gnoss.ApiWrapper.Model;
using Gnoss.Web.Login.Open;

namespace Gnoss.Web.Login.SAML
{
    [Controller]
    [Route("[controller]")]
    public class LoginSAMLController : ControllerBaseLogin
    {
        private static readonly ResourceApi mResourceApi = new ResourceApi($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config/ConfigOAuth/OAuthV3.config");
        private static readonly CommunityApi mCommunityApi = new CommunityApi($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config/ConfigOAuth/OAuthV3.config");
        private static readonly UserApi mUserApi = new UserApi($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config/ConfigOAuth/OAuthV3.config");
        readonly ConfigServiceLogin mConfigServiceSAML;

        public LoginSAMLController(LoggingService loggingService, IHttpContextAccessor httpContextAccessor, EntityContext entityContext, ConfigService configService, RedisCacheWrapper redisCacheWrapper, GnossCache gnossCache, VirtuosoAD virtuosoAD, IHostingEnvironment env, EntityContextBASE entityContextBASE, IServicesUtilVirtuosoAndReplication servicesUtilVirtuosoAndReplication, ConfigServiceLogin configServiceSAML)
             : base(loggingService, httpContextAccessor, entityContext, configService, redisCacheWrapper, gnossCache, virtuosoAD, env, entityContextBASE, servicesUtilVirtuosoAndReplication)
        {
            mConfigServiceSAML = configServiceSAML;
        }

        [HttpGet, HttpPost]
        public IActionResult Index(string returnUrl = null,string token=null)
        {
            mResourceApi.Log.Info($"0.-LoginSAMLController Intento de login returnUrl: {returnUrl} token: {token}");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (User != null && User.Claims.Count() > 0)
                {
                    mResourceApi.Log.Info($"1.-LoginSAMLController Intento de login returnUrl: {returnUrl} token: {token}");
                    //Loguear
                    Response.Redirect(LoguearUsuario(User, returnUrl,token));
                }
                else
                {
                    mResourceApi.Log.Info($"2.-LoginSAMLController Intento de login returnUrl: {returnUrl} token: {token}");
                    //Si no hay usuario redirigimos al login
                    Response.Redirect(Url.Content(@$"~/{mConfigServiceSAML.GetUrlServiceInDomain()}Auth/Login") + "?returnUrl=" + returnUrl +"&token="+ token);
                    
                }
            }

            return View();
        }

        private string LoguearUsuario(ClaimsPrincipal pUser,string pReturnUrl,string pToken)
        {
            string email = "";

            mCommunityApi.Log.Info("3.-numClaims:" + pUser.Claims.ToList());

            //mail http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
            foreach (Claim claim in pUser.Claims.ToList())
            {
                mCommunityApi.Log.Info("4.-CLAIM TYPE: '" + claim.Type + "' CLAIMVALUE: '" + claim.Value.Trim().ToLower() + "'");
                if(claim.Type== "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")
                {
                    email = claim.Value.Trim();
                }            
            }

            //comprobamos si existe la persona del email
            string person=mResourceApi.VirtuosoQuery("select ?person", @$"where{{
                                        ?person <http://w3id.org/roh/isActive> 'true'.
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        ?person <https://www.w3.org/2006/vcard/ns#email> '{email}'.
                        }}", "person").results.bindings.FirstOrDefault()?["person"].value;
            
            string UrlLogout = pReturnUrl + "/"+ UtilIdiomas.GetText("URLSEM","DESCONECTAR");

            if (string.IsNullOrEmpty(person))
            {
                //No existe ninguna persona aociada al correo
                mCommunityApi.Log.Info("5.-Redirigir a la página avisando de que no existe ningún usuario con ese correo");
                return pReturnUrl+"/notexistsmail?email="+email;
            }
            else
            {
                mCommunityApi.Log.Info("6.-LOGUEAMOS");
                //Comprobamos si existe usuario para la persona
                string user = mResourceApi.VirtuosoQuery("select ?user", @$"where{{
                                        <{person}> <http://w3id.org/roh/gnossUser> ?user.
                        }}", "person").results.bindings.FirstOrDefault()?["user"].value;

                User usuario = null;
                if (!string.IsNullOrEmpty(user))
                {                    
                    //Obtenemos el usuario
                    Guid userID = new Guid(user.Substring(user.LastIndexOf("/") + 1));
                    usuario = mUserApi.GetUserById(userID);
                }

                if (usuario==null)
                {
                    usuario = new User();
                    usuario.email = email;
                    usuario.password = CreatePassword();
                    usuario.name = "name";
                    usuario.last_name = "last_name";
                    usuario = mUserApi.CreateUser(usuario);
                    //Modificamos persona para asignar ususario

                    //Insertamos
                    Dictionary<Guid, List<TriplesToInclude>> triples = new() { { mResourceApi.GetShortGuid(person), new List<TriplesToInclude>() } };
                    TriplesToInclude t = new();
                    t.Predicate = "http://w3id.org/roh/gnossUser";
                    t.NewValue = "http://gnoss/" + usuario.user_id.ToString().ToUpper();
                    triples[mResourceApi.GetShortGuid(person)].Add(t);
                    mResourceApi.InsertPropertiesLoadedResources(triples);
                }

                string logoutUrl = UrlLogout;
                mCommunityApi.Log.Info("7.-logoutUrl:" + logoutUrl);
                mCommunityApi.Log.Info("8.-urlRedirect:" + pReturnUrl);
                Uri url = new Uri(pReturnUrl);

                PersonaCN personaCN = new PersonaCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
                Es.Riam.Gnoss.AD.EntityModel.Models.PersonaDS.Persona filaPersona = personaCN.ObtenerPersonaPorUsuario(usuario.user_id).ListaPersona.FirstOrDefault();
                UsuarioCN usuarioCN=new UsuarioCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
                Es.Riam.Gnoss.AD.EntityModel.Models.UsuarioDS.Usuario filaUsuario = usuarioCN.ObtenerUsuarioCompletoPorID(usuario.user_id).ListaUsuario.FirstOrDefault();

                mUsuarioID = filaUsuario.UsuarioID;
                mPersonaID = filaPersona.PersonaID;
                mLogin = filaUsuario.Login;
                mIdioma = filaPersona.Idioma;
                mNombreCorto = filaUsuario.NombreCorto;
                mMantenerConectado = false;

                base.LoguearUsuario(filaUsuario.UsuarioID, mPersonaID, mNombreCorto, mLogin, mIdioma);

                return EnviarCookies(url.Scheme+"://"+url.Host, pReturnUrl, pToken);
            }
        }

        private string CreatePassword()
        {
            int length = 5;
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string validNum = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
                res.Append(validNum[rnd.Next(validNum.Length)]);
            }
            return res.ToString();
        }

        /// <summary>
        /// Envia las cookies al dominio que hizo la petición por medio de una redirección a obtenercookie.aspx
        /// </summary>
        /// <param name="pDominioDeVuelta">Dominio que hizo la petición</param>
        /// <param name="pRedirect">URL a la que hay que redirigir al usuario una vez finalice el login</param>
        /// <param name="pToken">Token generado por el dominio de origen</param>
        [NonAction]
        private string EnviarCookies(string pDominioDeVuelta, string pRedirect, string pToken)
        {
            string dominioRedireccion = string.Empty;
            try
            {
                Uri uriRedireccion = new Uri(pRedirect);
                dominioRedireccion = UtilDominios.ObtenerDominioUrl(uriRedireccion, true);
            }
            catch (Exception ex)
            {
                mLoggingService.GuardarLogError(ex, "Error al obtener el dominio de la redirección");
            }

            string dominio = ObtenerDominioIP();


            string query = "urlVuelta=" + pDominioDeVuelta + "&redirect=" + HttpUtility.UrlEncode(pRedirect) + "&token=" + pToken;

            return dominio + "/obtenerCookie?" + query;
        }
    }
}
