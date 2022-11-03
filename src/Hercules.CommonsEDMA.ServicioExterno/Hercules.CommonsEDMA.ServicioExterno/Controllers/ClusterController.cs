using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Hercules.CommonsEDMA.ServicioExterno.Models.Cluster;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ClusterController : ControllerBase
    {
        /// <summary>
        /// Controlador para obtener los thesaurus usados en el cluster.
        /// </summary>
        /// <param name="listThesaurus">Elemento padre que define el thesaurus</param>
        /// <returns>Diccionario con los datos.</returns>
        [HttpGet("GetThesaurus")]
        public IActionResult GetThesaurus(string listThesaurus = "")
        {
            AccionesCluster cluster = new AccionesCluster();
            Dictionary<string, List<ThesaurusItem>> datosThesaurus = cluster.GetListThesaurus(listThesaurus);

            return Ok(datosThesaurus);
        }

        /// <summary>
        /// Controlador para crear/actualizar los datos del cluster.
        /// </summary>
        /// <param name="pIdGnossUser">Usuario de gnoss.</param>
        /// <param name="pDataCluster">Datos a añadir / modificar.</param>
        /// <returns>Id del cluster creado o modificado.</returns>
        [HttpPost("SaveCluster")]
        [Produces("application/json")]
        public IActionResult SaveCluster([FromForm] string pIdGnossUser, [FromForm] Cluster pDataCluster)
        {
            if (!Security.CheckUser(new Guid(pIdGnossUser), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            AccionesCluster accionCluster = new AccionesCluster();
            string idClusterRes = accionCluster.SaveCluster(pIdGnossUser, pDataCluster);
            return Ok(idClusterRes);
        }


        /// <summary>
        /// Controlador para cargar los datos de un cluster.
        /// </summary>
        /// <param name="pIdClusterId">Id del cluster.</param>
        /// <returns>Objeto con el contenido del cluster.</returns>
        [HttpGet("LoadCluster")]
        public IActionResult LoadCluster([Required] string pIdClusterId)
        {
            AccionesCluster accionCluster = new AccionesCluster();
            bool flag = false;
            foreach (string pIdGnossUser in accionCluster.memberListFromCluser(pIdClusterId))
            {
                string a = pIdGnossUser.Split("/")[pIdGnossUser.Split("/").Length - 1];
                if (Security.CheckUser(new Guid(pIdGnossUser.Split("/")[pIdGnossUser.Split("/").Length - 1]), Request))
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            Cluster idClusterRes = AccionesCluster.LoadCluster(pIdClusterId);
            return Ok(idClusterRes);
        }

        /// <summary>
        /// Controlador para cargar los perfiles de cada investigador sugerido del cluster.
        /// </summary>
        /// <param name="pDataCluster">Datos del cluster.</param>
        /// <param name="pPersons">Listado de personas sobre los que pedir información.</param>
        /// <returns>Diccionario con los datos necesarios para cada persona por cluster.</returns>
        [HttpPost("LoadProfiles")]
        public IActionResult LoadProfiles([FromForm] Cluster pDataCluster, [FromForm] List<string> pPersons)
        {
            AccionesCluster accionCluster = new AccionesCluster();
            return Ok(accionCluster.LoadProfiles(pDataCluster, pPersons));
        }

        /// <summary>
        /// Controlador para cargar la configuración de los perfiles de todos los clusters de un usuario de la web.
        /// </summary>
        /// <param name="pIdUser">Id del usuario.</param>
        /// <param name="loadSavedProfiles">Booleano que determina si cargamos los investigadores de cada perfil.</param>
        /// <returns>Listado con los datos necesarios de los clusters y sus perfiles.</returns>
        [HttpGet("LoadSavedProfiles")]
        public IActionResult LoadSavedProfiles([Required] Guid pIdUser, bool loadSavedProfiles = false)
        {

            if (!Security.CheckUser(pIdUser, Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            AccionesCluster accionCluster = new AccionesCluster();
            return Ok(accionCluster.loadSavedProfiles(pIdUser, loadSavedProfiles));
        }


        /// <summary>
        /// Controlador que obtiene el objeto para crear la gráfica tipo araña de las relaciones entre los perfiles seleccionados en el cluster
        /// </summary>
        /// <param name="pCluster">Cluster con los datos de las personas sobre las que realizar el filtrado de áreas temáticas.</param>
        /// <param name="pPersons">Personas sobre las que realizar el filtrado de áreas temáticas (Por si se envía directamente).</param>
        /// <param name="seleccionados">Determina si se envía el listado de personas desde el cluster o desde las personas</param>
        /// <returns>Objeto que se trata en JS para construir la gráfica.</returns>
        [HttpPost("DatosGraficaColaboradoresCluster")]
        public IActionResult DatosGraficaColaboradoresCluster([FromForm] Cluster pCluster, [FromForm] List<string> pPersons, [FromForm] bool seleccionados)
        {
            AccionesCluster accionCluster = new AccionesCluster();
            return Ok(accionCluster.DatosGraficaColaboradoresCluster(pCluster, pPersons, seleccionados));
        }

        /// <summary>
        /// Controlador que obtiene los datos para crear la gráfica de áreas temáticas
        /// </summary>
        /// <param name="pPersons">Listado de personas.</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpPost("DatosGraficaAreasTematicasCluster")]
        public IActionResult DatosGraficaAreasTematicasCluster([FromForm] List<string> pPersons)
        {
            AccionesCluster accionCluster = new AccionesCluster();
            DataGraficaAreasTags datos = accionCluster.DatosGraficaAreasTematicas(pPersons);

            return Ok(datos);
        }


        /// <summary>
        /// Controlador que borra un cluster
        /// </summary>
        /// <param name="pIdClusterId">Id del Cluster a borrar.</param>
        /// <returns>Un booleano si ha sido borrado.</returns>
        [HttpPost("BorrarCluster")]
        public IActionResult BorrarCluster([FromForm] string pIdClusterId)
        {
            AccionesCluster accionCluster = new AccionesCluster();
            string pUserId = accionCluster.getOwnerFromCluser(pIdClusterId);
            if (!Security.CheckUser(new Guid(pUserId.Split("/")[pUserId.Split("/").Length - 1]), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            bool borrado = accionCluster.BorrarCluster(pIdClusterId);

            return Ok(borrado);
        }



        /// <summary>
        /// Controlador que sugiere etiquetas con la búsqueda dada
        /// </summary>
        /// <param name="tagInput">Texto para la búsqueda de etiquetas.</param>
        /// <returns>Listado de las etiquetas de resultado.</returns>
        [HttpGet("SearchTags")]
        public IActionResult SearchTags([Required] string tagInput)
        {
            AccionesCluster accionCluster = new AccionesCluster();
            List<string> idClusterRes = accionCluster.SearchTags(tagInput);

            return Ok(idClusterRes);
        }

    }
}
