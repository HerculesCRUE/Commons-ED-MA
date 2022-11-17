using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class NotificationController : ControllerBase
    {

        [HttpPost("ReadNotifications")]
        public IActionResult ReadNotifications([FromForm] string[] notificacionesID)
        {
            AccionesNotificaciones acciones = new AccionesNotificaciones();
            foreach (string notificacion in notificacionesID)
            {
                acciones.readNotification(notificacion);
            }

            return Ok();
        }
        [HttpGet("hasNotificationsUnRead")]
        public IActionResult HasNotificationsUnRead(string userID)
        {
            AccionesNotificaciones acciones = new AccionesNotificaciones();
            return Ok(acciones.hasNotificationsUnRead(userID));
        }
    }
}
