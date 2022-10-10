using Es.Riam.AbstractsOpen;
using Es.Riam.Gnoss.AD.EncapsuladoDatos;
using Es.Riam.Gnoss.AD.EntityModel;
using Es.Riam.Gnoss.AD.EntityModel.Models.PersonaDS;
using Es.Riam.Gnoss.AD.EntityModel.Models.UsuarioDS;
using Es.Riam.Gnoss.AD.EntityModelBASE;
using Es.Riam.Gnoss.AD.ParametroAplicacion;
using Es.Riam.Gnoss.AD.Usuarios;
using Es.Riam.Gnoss.AD.Virtuoso;
using Es.Riam.Gnoss.CL;
using Es.Riam.Gnoss.CL.ParametrosAplicacion;
using Es.Riam.Gnoss.Elementos.Identidad;
using Es.Riam.Gnoss.Elementos.ParametroAplicacion;
using Es.Riam.Gnoss.Logica.ParametroAplicacion;
using Es.Riam.Gnoss.Logica.ServiciosGenerales;
using Es.Riam.Gnoss.Logica.Usuarios;
using Es.Riam.Gnoss.Recursos;
using Es.Riam.Gnoss.Util.Configuracion;
using Es.Riam.Gnoss.Util.General;
using Es.Riam.Gnoss.Web.Controles;
using Es.Riam.Gnoss.Web.Controles.ServiciosGenerales;
using Es.Riam.Util;
using Es.Riam.Web.Util;
using Microsoft.AspNetCore.Cors;
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
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Web;
using System.Xml;

namespace Gnoss.Web.Login
{
    [Controller]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ObtenerTokenSharepoint : ControllerBaseLogin
    {
        public ObtenerTokenSharepoint(LoggingService loggingService, IHttpContextAccessor httpContextAccessor, EntityContext entityContext, ConfigService configService, RedisCacheWrapper redisCacheWrapper, GnossCache gnossCache, VirtuosoAD virtuosoAD, IHostingEnvironment env, EntityContextBASE entityContextBASE, IServicesUtilVirtuosoAndReplication servicesUtilVirtuosoAndReplication) : base(loggingService, httpContextAccessor, entityContext, configService, redisCacheWrapper, gnossCache, virtuosoAD, env, entityContextBASE, servicesUtilVirtuosoAndReplication)
        {
        }

        [HttpGet()]
        public ActionResult Index(string code)
        {
            ParametroAplicacionCN parametroAplicacionCN = new ParametroAplicacionCN(mEntityContext, mLoggingService, mConfigService, mServicesUtilVirtuosoAndReplication);
            string urlServicioLogin = mConfigService.ObtenerUrlServicioLogin();
            string clientID = parametroAplicacionCN.ObtenerParametroAplicacion("SharepointClientID");
            string tenantID = parametroAplicacionCN.ObtenerParametroAplicacion("SharepointTenantID");
            string clientSecret = parametroAplicacionCN.ObtenerParametroAplicacion("SharepointClientSecret");
            string peticion = $"https://login.microsoftonline.com/{tenantID}/oauth2/v2.0/token";
            string redirectUri = HttpUtility.UrlEncode($"{urlServicioLogin}/ObtenerTokenSharepoint");
            string scope = "offline_access " + HttpUtility.UrlEncode("https://graph.microsoft.com/.default");
            string requestParameters = $"client_id={clientID}&redirect_uri={redirectUri}&scope={scope}&grant_type=authorization_code&code={code}&client_secret={clientSecret}";
            byte[] byteData = Encoding.UTF8.GetBytes(requestParameters);
            string response = UtilGeneral.WebRequest("POST", peticion, byteData);
            dynamic tokenObj = JsonConvert.DeserializeObject(response);
            string token = tokenObj.access_token;
            string refresh_token = tokenObj.refresh_token;

            Guid usuarioID = new Guid(mHttpContextAccessor.HttpContext.Request.Cookies["usuID"]);
            UsuarioVinculadoLoginRedesSociales existeUsuarioVinculado = mEntityContext.UsuarioVinculadoLoginRedesSociales.Where(parametroApp => parametroApp.UsuarioID.Equals(usuarioID) && parametroApp.TipoRedSocial == 6).FirstOrDefault();

            //si el usuario existe ya tiene un token lo modificamos, sino lo guardamos
            if (existeUsuarioVinculado != null)
            {
                UsuarioVinculadoLoginRedesSociales usuarioRefresco = mEntityContext.UsuarioVinculadoLoginRedesSociales.Where(parametroApp => parametroApp.UsuarioID.Equals(usuarioID) && parametroApp.TipoRedSocial == 7).FirstOrDefault();

                existeUsuarioVinculado.IDenRedSocial = token;
                usuarioRefresco.IDenRedSocial = refresh_token;
                mEntityContext.UsuarioVinculadoLoginRedesSociales.Update(existeUsuarioVinculado);
                mEntityContext.UsuarioVinculadoLoginRedesSociales.Update(usuarioRefresco);
            }
            else
            {
                UsuarioVinculadoLoginRedesSociales usuario = new UsuarioVinculadoLoginRedesSociales();
                usuario.UsuarioID = usuarioID;
                usuario.TipoRedSocial = (short)TipoRedSocialLogin.Sharepoint;
                usuario.IDenRedSocial = token;

                UsuarioVinculadoLoginRedesSociales usuarioRefresh = new UsuarioVinculadoLoginRedesSociales();
                usuarioRefresh.UsuarioID = usuarioID;
                usuarioRefresh.TipoRedSocial = (short)TipoRedSocialLogin.SharepointRefresh;
                usuarioRefresh.IDenRedSocial = refresh_token;

                mEntityContext.UsuarioVinculadoLoginRedesSociales.Add(usuario);
                mEntityContext.UsuarioVinculadoLoginRedesSociales.Add(usuarioRefresh);
            }
            mEntityContext.SaveChanges();

            string urlRedirect = $"{urlServicioLogin}/Redireccion";

            return Redirect(urlRedirect);
        }

    }
}
