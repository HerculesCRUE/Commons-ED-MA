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
using Gnoss.Web.Login.SAML.Models.PersonOntology;
using Es.Riam.Gnoss.Elementos.ServiciosGenerales;
using Es.Riam.Gnoss.Web.Controles.Proyectos;

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
        public IActionResult Index(string returnUrl = null, string token = null)
        {
            mResourceApi.Log.Info($"0.-LoginSAMLController Intento de login returnUrl: {returnUrl} token: {token}");
            if (!string.IsNullOrEmpty(returnUrl))
            {
                if (User != null && User.Claims.Count() > 0)
                {
                    mResourceApi.Log.Info($"1.-LoginSAMLController Intento de login returnUrl: {returnUrl} token: {token}");
                    //Loguear
                    Response.Redirect(LoguearUsuario(User, returnUrl, token));
                }
                else
                {
                    mResourceApi.Log.Info($"2.-LoginSAMLController Intento de login returnUrl: {returnUrl} token: {token}");
                    //Si no hay usuario redirigimos al login
                    Response.Redirect(Url.Content(@$"~/{mConfigServiceSAML.GetUrlServiceInDomain()}Auth/Login") + "?returnUrl=" + returnUrl + "&token=" + token);

                }
            }

            return View();
        }

        private string LoguearUsuario(ClaimsPrincipal pUser, string pReturnUrl, string pToken)
        {
            string email = "";

            mCommunityApi.Log.Info("-numClaims:" + pUser.Claims.ToList());

            string claimMail = mConfigServiceSAML.GetClaimMail();
            string claimGroups = mConfigServiceSAML.GetClaimGroups();

            bool gestorOtri = true;
            bool adminIndicadores = true;
            bool admin = true;

            foreach (Claim claim in pUser.Claims.ToList())
            {
                mCommunityApi.Log.Info("-CLAIM TYPE: '" + claim.Type + "' CLAIMVALUE: '" + claim.Value.Trim().ToLower() + "'");
                if (claim.Type == claimMail)
                {
                    email = claim.Value.Trim();
                }
                if (claim.Type == claimGroups && claim.Value.Trim() == mConfigServiceSAML.GetGroupAdmin())
                {
                    admin = true;
                }
                if (claim.Type == claimGroups && claim.Value.Trim() == mConfigServiceSAML.GetGroupAdminIndicadores())
                {
                    adminIndicadores = true;
                }
                if (claim.Type == claimGroups && claim.Value.Trim() == mConfigServiceSAML.GetGroupGestorOtri())
                {
                    gestorOtri = true;
                }
            }

            User usuario = ObtenerUsuario(email, gestorOtri, adminIndicadores, admin);

            if (usuario != null)
            {
                string logoutUrl = pReturnUrl + "/" + UtilIdiomas.GetText("URLSEM", "DESCONECTAR");
                mCommunityApi.Log.Info("-logoutUrl:" + logoutUrl);
                mCommunityApi.Log.Info("-urlRedirect:" + pReturnUrl);
                Uri url = new Uri(pReturnUrl);

                PersonaCN personaCN = new PersonaCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
                Es.Riam.Gnoss.AD.EntityModel.Models.PersonaDS.Persona filaPersona = personaCN.ObtenerPersonaPorUsuario(usuario.user_id).ListaPersona.FirstOrDefault();
                UsuarioCN usuarioCN = new UsuarioCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
                Es.Riam.Gnoss.AD.EntityModel.Models.UsuarioDS.Usuario filaUsuario = usuarioCN.ObtenerUsuarioCompletoPorID(usuario.user_id).ListaUsuario.FirstOrDefault();

                mUsuarioID = filaUsuario.UsuarioID;
                mPersonaID = filaPersona.PersonaID;
                mLogin = filaUsuario.Login;
                mIdioma = filaPersona.Idioma;
                mNombreCorto = filaUsuario.NombreCorto;
                mMantenerConectado = false;

                base.LoguearUsuario(filaUsuario.UsuarioID, mPersonaID, mNombreCorto, mLogin, mIdioma);

                return EnviarCookies(url.Scheme + "://" + url.Host, pReturnUrl, pToken);
            }
            else
            {
                //No tiene email
                mCommunityApi.Log.Info("-Redirigir a la página avisando de que no existe ningún usuario con ese correo");
                return pReturnUrl + "/notexistsmail?email=" + email;
            }
        }

        private User ObtenerUsuario(string email, bool gestorOtri, bool adminIndicadores, bool admin)
        {
            if (!string.IsNullOrEmpty(email))
            {
                //comprobamos si existe la persona del email (primero investigador principal, después sincronizado, después resto)
                SparqlObject datosPersona = mResourceApi.VirtuosoQuery("select ?person ?active ?crisIdentifier", @$"where{{
                                        OPTIONAL{{?person <http://w3id.org/roh/isActive> ?active.}}
                                        OPTIONAL{{?person <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.}}
                                        OPTIONAL{{?person <http://xmlns.com/foaf/0.1/firstName> ?firstName}}
                                        OPTIONAL{{?person <http://xmlns.com/foaf/0.1/lastName> ?lastName}}
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        ?person <https://www.w3.org/2006/vcard/ns#email> '{email}'.
                        }}order by desc(?active) desc(?crisIdentifier)", "person");

                string person = datosPersona.results.bindings.FirstOrDefault()?["person"].value;
                bool active = datosPersona.results.bindings.FirstOrDefault()?["active"].value == "true";
                string crisIdentifier = datosPersona.results.bindings.FirstOrDefault()?["crisIdentifier"].value;
                string firstName = datosPersona.results.bindings.FirstOrDefault()?["firstName"].value;
                string lastName = datosPersona.results.bindings.FirstOrDefault()?["lastName"].value;
                if (string.IsNullOrEmpty(firstName))
                {
                    firstName = email;
                }
                if (string.IsNullOrEmpty(lastName))
                {
                    lastName = email;
                }
                mCommunityApi.Log.Info($"-Obtenemos datos del usuario person:'{person}' active:'{active}' crisIdentifier:'{crisIdentifier}' firstName:'{firstName}' lastName:'{lastName}' ");


                //Si no tiene ningún tipo de administración sólo consideramos a los investigadores
                if ((!gestorOtri && !adminIndicadores && !admin) && (string.IsNullOrEmpty(person) || !active))
                {
                    return null;
                }
                else
                {
                    mCommunityApi.Log.Info("-LOGUEAMOS");
                    User usuario = null;

                    //Existe una persona con ese email
                    if (!string.IsNullOrEmpty(person))
                    {
                        //Comprobamos si existe usuario para la persona
                        string user = mResourceApi.VirtuosoQuery("select ?user", @$"where{{
                                        <{person}> <http://w3id.org/roh/gnossUser> ?user.
                        }}", "person").results.bindings.FirstOrDefault()?["user"].value;

                        if (!string.IsNullOrEmpty(user))
                        {
                            //Obtenemos el usuario
                            Guid userID = new Guid(user.Substring(user.LastIndexOf("/") + 1));
                            usuario = mUserApi.GetUserById(userID);
                        }
                    }

                    //Si no existe el usuario lo creamos
                    if (usuario == null)
                    {
                        mCommunityApi.Log.Info($"-Creamos usuario porque no existe ");
                        usuario = new User();
                        usuario.email = email;
                        usuario.password = CreatePassword();
                        usuario.name = firstName;
                        usuario.community_short_name = "hercules";
                        usuario.last_name = lastName;
                        usuario = mUserApi.CreateUser(usuario);
                    }

                    //Si no existe la persona se crea 
                    if (string.IsNullOrEmpty(person))
                    {
                        Person personObject = new();
                        personObject.Vcard_email = new List<string>() { email };
                        personObject.Foaf_firstName = firstName;
                        personObject.Foaf_lastName = lastName;

                        ComplexOntologyResource resource = personObject.ToGnossApiResource(mResourceApi, null);
                        int numIntentos = 0;
                        while (!resource.Uploaded)
                        {
                            numIntentos++;
                            if (numIntentos > 5)
                            {
                                break;
                            }
                            person = mResourceApi.LoadComplexSemanticResource(resource);
                        }
                    }

                    mCommunityApi.Log.Info($"-Aplicamos permisos ");

                    //Aplicamos permisos en la persona
                    //-Asignamos usuario                   
                    //-Asignamos adminIndicadores http://w3id.org/roh/isGraphicmanager
                    //-Asignamos gestorOtri http://w3id.org/roh/isOtriManager
                    //Asignamos admin

                    //Obtenemos los datos de la persona
                    SparqlObject datosPermisosPersona = mResourceApi.VirtuosoQuery("select ?person ?isGraphicmanager ?isOtriManager ?gnossUser", @$"where{{
                                        OPTIONAL{{?person <http://w3id.org/roh/isGraphicmanager> ?isGraphicmanager.}}
                                        OPTIONAL{{?person <http://w3id.org/roh/isOtriManager> ?isOtriManager.}}
                                        OPTIONAL{{?person <http://w3id.org/roh/gnossUser> ?gnossUser}}
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        FILTER(?person=<{person}>)
                        }}", "person");

                    string isGraphicmanager = datosPermisosPersona.results.bindings.FirstOrDefault()?["isGraphicmanager"].value;
                    string isOtriManager = datosPermisosPersona.results.bindings.FirstOrDefault()?["isOtriManager"].value;
                    string gnossUser = datosPermisosPersona.results.bindings.FirstOrDefault()?["gnossUser"].value;
                    bool isAdmin = EsAdministradorComunidad("hercules", usuario.user_id);

                    mCommunityApi.Log.Info($"-Permisos actuales isGraphicmanager:'{isGraphicmanager}' isOtriManager:'{isOtriManager}' gnossUser:'{gnossUser}' isAdmin:'{isAdmin}' ");

                    Dictionary<Guid, List<TriplesToInclude>> triplesInsertar = new() { { mResourceApi.GetShortGuid(person), new List<TriplesToInclude>() } };
                    Dictionary<Guid, List<TriplesToModify>> triplesModificar = new() { { mResourceApi.GetShortGuid(person), new List<TriplesToModify>() } };
                    Dictionary<Guid, List<RemoveTriples>> triplesEliminar = new() { { mResourceApi.GetShortGuid(person), new List<RemoveTriples>() } };

                    //gnossUser
                    if (string.IsNullOrEmpty(gnossUser))
                    {
                        mCommunityApi.Log.Info($"-Insertamos Gnossuser ");
                        TriplesToInclude t = new();
                        t.Predicate = "http://w3id.org/roh/gnossUser";
                        t.NewValue = "http://gnoss/" + usuario.user_id.ToString().ToUpper();
                        triplesInsertar[mResourceApi.GetShortGuid(person)].Add(t);
                    }
                    else if (gnossUser != "http://gnoss/" + usuario.user_id.ToString().ToUpper())
                    {
                        mCommunityApi.Log.Info($"-Modificamos Gnossuser ");
                        TriplesToModify t = new();
                        t.OldValue = gnossUser;
                        t.Predicate = "http://w3id.org/roh/gnossUser";
                        t.NewValue = "http://gnoss/" + usuario.user_id.ToString().ToUpper();
                        triplesModificar[mResourceApi.GetShortGuid(person)].Add(t);
                    }

                    //adminIndicadores
                    if (adminIndicadores && (string.IsNullOrEmpty(isGraphicmanager) || isGraphicmanager != "true"))
                    {
                        if (string.IsNullOrEmpty(isGraphicmanager))
                        {
                            mCommunityApi.Log.Info($"-Damos de alta isGraphicmanager ");
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/isGraphicmanager";
                            t.NewValue = "true";
                            triplesInsertar[mResourceApi.GetShortGuid(person)].Add(t);
                        }
                        else
                        {
                            mCommunityApi.Log.Info($"-Modificamos isGraphicmanager a 'true'");
                            TriplesToModify t = new();
                            t.OldValue = isGraphicmanager;
                            t.Predicate = "http://w3id.org/roh/isGraphicmanager";
                            t.NewValue = "true";
                            triplesModificar[mResourceApi.GetShortGuid(person)].Add(t);
                        }
                    }
                    if (!adminIndicadores && !string.IsNullOrEmpty(isGraphicmanager))
                    {
                        mCommunityApi.Log.Info($"-Eliminamos isGraphicmanager ");
                        RemoveTriples t = new();
                        t.Predicate = "http://w3id.org/roh/isGraphicmanager";
                        t.Value = isGraphicmanager;
                        triplesEliminar[mResourceApi.GetShortGuid(person)].Add(t);
                    }

                    //gestorOtri
                    if (gestorOtri && (string.IsNullOrEmpty(isOtriManager) || isOtriManager != "true"))
                    {
                        if (string.IsNullOrEmpty(isOtriManager))
                        {
                            mCommunityApi.Log.Info($"-Damos de alta isGraphicmanager ");
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/isOtriManager";
                            t.NewValue = "true";
                            triplesInsertar[mResourceApi.GetShortGuid(person)].Add(t);
                        }
                        else
                        {
                            mCommunityApi.Log.Info($"-Modificamos isGraphicmanager a 'true'");
                            TriplesToModify t = new();
                            t.OldValue = isOtriManager;
                            t.Predicate = "http://w3id.org/roh/isOtriManager";
                            t.NewValue = "true";
                            triplesModificar[mResourceApi.GetShortGuid(person)].Add(t);
                        }
                    }
                    if (!gestorOtri && !string.IsNullOrEmpty(isOtriManager))
                    {
                        mCommunityApi.Log.Info($"-Eliminamos isGraphicmanager ");
                        RemoveTriples t = new();
                        t.Predicate = "http://w3id.org/roh/isOtriManager";
                        t.Value = isOtriManager;
                        triplesEliminar[mResourceApi.GetShortGuid(person)].Add(t);
                    }

                    //Admin
                    if (admin && !isAdmin)
                    {
                        mCommunityApi.Log.Info($"-Damos de alta como admin ");
                        //Meter como admin
                        AltaAdministradorDeComunidad("hercules", usuario.user_id);
                    }
                    else if (!admin && isAdmin)
                    {
                        mCommunityApi.Log.Info($"-Eliminamos como admin ");
                        //Sacar como admin
                        EliminarPermisosAdministrador("hercules", usuario.user_id);
                    }

                    return usuario;

                }
            }
            return null;
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

        public bool EsAdministradorComunidad(string community_short_name, Guid user_id)
        {
            bool esAdministrador = false;
            ProyectoCN proyCN = new ProyectoCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
            Guid proyectoID = proyCN.ObtenerProyectoIDPorNombre(community_short_name);
            if (!proyectoID.Equals(Guid.Empty))
            {
                GestionProyecto gestorProyecto = new GestionProyecto(proyCN.ObtenerProyectoPorID(proyectoID), mLoggingService, mEntityContext);
                gestorProyecto.CargarGestor();
                Proyecto proyecto = gestorProyecto.ListaProyectos[proyectoID];
                esAdministrador = proyecto.EsAdministradorUsuario(user_id);

            }
            proyCN.Dispose();
            return esAdministrador;
        }

        public void AltaAdministradorDeComunidad(string community_short_name, Guid user_id)
        {
            ProyectoCN proyCN = new ProyectoCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
            Guid proyectoID = proyCN.ObtenerProyectoIDPorNombre(community_short_name);
            if (!proyectoID.Equals(Guid.Empty))
            {
                GestionProyecto gestorProyecto = new GestionProyecto(proyCN.ObtenerProyectoPorID(proyectoID), mLoggingService, mEntityContext);
                gestorProyecto.CargarGestor();
                Proyecto proyecto = gestorProyecto.ListaProyectos[proyectoID];

                List<Guid> listaUsuarios = new List<Guid>();
                listaUsuarios.Add(user_id);

                string error = new ControladorProyecto(mLoggingService, mEntityContext, mConfigService, mRedisCacheWrapper, mGnossCache, mEntityContextBASE, mVirtuosoAD, mHttpContextAccessor, mServicesUtilVirtuosoAndReplication).AgregarAdministradoresAComunidad(proyecto.FilaProyecto.OrganizacionID, proyecto.Clave, listaUsuarios, false);

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception("Could not add the member as administrator");
                }
            }
            else
            {
                throw new Exception("The community does not exists");
            }
            proyCN.Dispose();
        }

        public void EliminarPermisosAdministrador(string community_short_name, Guid user_id)
        {

            ProyectoCN proyCN = new ProyectoCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
            Guid proyectoID = proyCN.ObtenerProyectoIDPorNombre(community_short_name);

            if (!proyectoID.Equals(Guid.Empty))
            {
                GestionProyecto gestorProyecto = new GestionProyecto(proyCN.ObtenerProyectoPorID(proyectoID), mLoggingService, mEntityContext);
                gestorProyecto.CargarGestor();
                Proyecto proyecto = gestorProyecto.ListaProyectos[proyectoID];

                string error = new ControladorProyecto(mLoggingService, mEntityContext, mConfigService, mRedisCacheWrapper, mGnossCache, mEntityContextBASE, mVirtuosoAD, mHttpContextAccessor, mServicesUtilVirtuosoAndReplication)
                    .EliminarAdministradorComunidad(proyecto.FilaProyecto.OrganizacionID, proyecto.Clave, user_id, false);

                if (!string.IsNullOrEmpty(error))
                {
                    throw new Exception("Could not delete the member as administrator");
                }
            }
            else
            {
                throw new Exception("The community does not exists");
            }

            proyCN.Dispose();
        }
    }
}
