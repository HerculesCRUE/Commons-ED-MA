using Es.Riam.AbstractsOpen;
using Es.Riam.Gnoss.AD.EntityModel;
using Es.Riam.Gnoss.AD.Virtuoso;
using Es.Riam.Gnoss.CL;
using Es.Riam.Gnoss.Util.Configuracion;
using Es.Riam.Gnoss.Util.General;
using Es.Riam.Gnoss.Web.Controles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServicioCargaResultadosMVC.Controllers
{
    [Controller]
    [Route("[controller]")]
    public class TrazaController : Controller
    {

        private ControladorBase mControladorBase;
        private EntityContext mEntityContext;
        private LoggingService mLoggingService;
        private RedisCacheWrapper mRedisCacheWrapper;
        private ConfigService mConfigService;
        private IServicesUtilVirtuosoAndReplication mServicesUtilVirtuosoAndReplication;

        public TrazaController(EntityContext entityContext, LoggingService loggingService, RedisCacheWrapper redisCacheWrapper, ConfigService configService, GnossCache gnossCache, VirtuosoAD virtuosoAD, IHttpContextAccessor httpContextAccessor, IServicesUtilVirtuosoAndReplication servicesUtilVirtuosoAndReplication)
        {
            mConfigService = configService;
            mEntityContext = entityContext;
            mLoggingService = loggingService;
            mRedisCacheWrapper = redisCacheWrapper;
            mServicesUtilVirtuosoAndReplication = servicesUtilVirtuosoAndReplication;
            mControladorBase = new ControladorBase(loggingService, configService, entityContext, redisCacheWrapper, gnossCache, virtuosoAD, httpContextAccessor, mServicesUtilVirtuosoAndReplication);
        }

        [HttpGet]
        public ActionResult Index(string txtNombre, string txtPassword)
        {
            if (Request.Method.Equals("GET"))
            {
                EstablecerTextos();

                return View("Index");
            }
            else
            {
                if ((txtNombre.ToLower().Equals("admintrazas")) && (txtPassword.ToLower().Equals("riam123")))
                {
                    HabilitarTraza();
                }
                else
                {
                    ViewBag.Error = "Usuario o contraseña incorrectas, intentalo de nuevo";
                }

                EstablecerTextos();

                return View("Index");
            }
        }
        [NonAction]
        private void HabilitarTraza()
        {
            LoggingService.TrazaHabilitada = !LoggingService.TrazaHabilitada;

            GnossCacheCL gnossCacheCL = new GnossCacheCL(mEntityContext, mLoggingService, mRedisCacheWrapper, mConfigService, mServicesUtilVirtuosoAndReplication);
            if (LoggingService.TrazaHabilitada)
            {
                gnossCacheCL.AgregarACache("traza" + mControladorBase.DominoAplicacion, true, 60 * 60 * 72);//72horas
            }
            else
            {
                gnossCacheCL.InvalidarDeCache("traza" + mControladorBase.DominoAplicacion);
            }
        }
        [NonAction]
        private void EstablecerTextos()
        {
            string estadoTraza = "deshabilitada";
            string botonTraza = "Habilitar traza";

            if (LoggingService.TrazaHabilitada)
            {
                estadoTraza = "HABILITADA";
                botonTraza = "Deshabilitar traza";
            }

            ViewBag.Estado = "La traza está " + estadoTraza;
            ViewBag.Accion= botonTraza;
        }
    }
}