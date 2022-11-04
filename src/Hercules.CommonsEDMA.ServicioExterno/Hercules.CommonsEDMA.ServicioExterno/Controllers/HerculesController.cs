using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaProyectos;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaPublicaciones;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataItemRelacion;
using Hercules.CommonsEDMA.ServicioExterno.Models.RedesUsuario;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class HerculesController : ControllerBase
    {
        #region Comunes
        /// <summary>
        /// Obtiene los datos para crear la gráfica de las publicaciones.
        /// </summary>
        /// <param name="pParametros">Filtros aplicados</param>
        /// <returns>Objeto con todos los datos necesarios para crear la gráfica en el JS.</returns>
        [HttpGet("DatosGraficaPublicaciones")]
        public IActionResult DatosGraficaPublicaciones(string pParametros)
        {
            AccionesPublicaciones accionesPublicaciones = new AccionesPublicaciones();
            DataGraficaPublicaciones datosPublicaciones = accionesPublicaciones.GetDatosGraficaPublicaciones(pParametros);

            return Ok(datosPublicaciones);
        }

        /// <summary>
        /// Obtiene los datos para crear la gráfica de los proyectos.
        /// </summary>
        /// <param name="pParametros">Filtros aplicados</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaProyectos")]
        public IActionResult DatosGraficaProyectos(string pParametros)
        {
            AccionesProyecto accionesProyecto = new AccionesProyecto();
            GraficasProyectos datosProyectos = accionesProyecto.GetDatosGraficaProyectos(pParametros);

            return Ok(datosProyectos);
        }

        /// <summary>
        /// Obtiene los datos para crear la gráfica de áreas temáticas
        /// </summary>
        /// <param name="pId">ID del elemento en cuestión.</param>
        /// <param name="pType">Tipo del elemento.</param>
        /// <param name="pAnioInicio">Año de inicio</param>
        /// <param name="pAnioFin">Año de fin</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaAreasTematicas")]
        public IActionResult DatosGraficaAreasTematicas(string pId, string pType, string pAnioInicio, string pAnioFin)
        {
            AccionesAreasTematicas accionesAreasTematicas = new AccionesAreasTematicas();
            DataGraficaAreasTags datos = accionesAreasTematicas.DatosGraficaAreasTematicas(pId, pType, pAnioInicio, pAnioFin);

            return Ok(datos);
        }

        /// <summary>
        /// Obtiene los datos para crear la gráfica de áreas temáticas
        /// </summary>
        /// <param name="pId">ID del elemento en cuestión.</param>
        /// <param name="pType">Tipo del elemento.</param>
        /// <param name="pAnioInicio">Año de inicio</param>
        /// <param name="pAnioFin">Año de fin</param>
        /// <param name="pNumAreas">Nº de areas</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaAreasTematicasArania")]
        public IActionResult DatosGraficaAreasTematicasArania(string pId, string pType, string pAnioInicio, string pAnioFin, int pNumAreas)
        {
            AccionesAreasTematicas accionesAreasTematicas = new AccionesAreasTematicas();
            List<DataItemRelacion> datos = accionesAreasTematicas.DatosGraficaAreasTematicasArania(pId, pType, pAnioInicio, pAnioFin, pNumAreas);

            return Ok(datos);
        }

        #endregion

        #region Grupo
        /// <summary>
        /// Obtiene los datos para crear la gráfica de miembros de un grupo
        /// </summary>
        /// <param name="pIdGrupo">ID del grupo en cuestión.</param>
        /// <param name="pParametros">Filtros de las facetas.</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaMiembrosGrupo")]
        public IActionResult DatosGraficaMiembrosGrupo(string pIdGrupo, string pParametros)
        {
            AccionesGroup accionGrupo = new AccionesGroup();
            List<DataItemRelacion> datosGrupo = accionGrupo.DatosGraficaMiembrosGrupo(pIdGrupo, pParametros);

            return Ok(datosGrupo);
        }

        /// <summary>
        /// Obtiene los datos para crear la gráfica de colaboradores de un grupo
        /// </summary>
        /// <param name="pIdGrupo">ID del grupo en cuestión.</param>
        /// <param name="pParametros">Filtros de la búsqueda.</param>
        /// <param name="pMax">Nº máximo para pintar</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaColaboradoresGrupo")]
        public IActionResult DatosGraficaColaboradoresGrupo(string pIdGrupo, string pParametros, int pMax)
        {
            AccionesGroup accionGrupo = new AccionesGroup();
            List<DataItemRelacion> datosGrupo = accionGrupo.DatosGraficaColaboradoresGrupo(pIdGrupo, pParametros, pMax);

            return Ok(datosGrupo);
        }

        #endregion

        /// <summary>
        /// Controlador para obtener los datos del proyecto en la cabecera de la ficha.
        /// </summary>
        /// <param name="pIdProyecto">ID del proyecto en cuestión.</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosFichaProyecto")]
        public IActionResult DatosFichaProyecto(string pIdProyecto)
        {
            AccionesProyecto accionProyecto = new AccionesProyecto();
            Dictionary<string, int> datosCabeceraFichas = accionProyecto.GetDatosCabeceraProyecto(pIdProyecto);

            return Ok(datosCabeceraFichas);
        }

        /// <summary>
        /// Controlador para obtener los datos de la gráfica de red de colaboradores.
        /// </summary>
        /// <param name="pIdProyecto">ID del proyecto en cuestión.</param>
        /// <param name="pParametros">Filtros de las facetas.</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaRedMiembros")]
        public IActionResult DatosGraficaRedMiembros(string pIdProyecto, string pParametros)
        {
            AccionesProyecto accionProyecto = new AccionesProyecto();
            List<DataItemRelacion> datosRedColaboradores = accionProyecto.DatosGraficaRedMiembros(pIdProyecto, pParametros);

            return Ok(datosRedColaboradores);
        }

        /// <summary>
        /// Controlador para obtener los datos de la gráfica de red de colaboradores.
        /// </summary>
        /// <param name="pIdProyecto">ID del proyecto en cuestión.</param>
        /// <param name="pParametros">Filtros de las facetas.</param>
        /// <param name="pMax"></param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaRedColaboradores")]
        public IActionResult DatosGraficaRedColaboradores(string pIdProyecto, string pParametros, int pMax)
        {
            AccionesProyecto accionProyecto = new AccionesProyecto();
            List<DataItemRelacion> datosRedColaboradores = accionProyecto.GetDatosGraficaRedColaboradores(pIdProyecto, pParametros, pMax);

            return Ok(datosRedColaboradores);
        }

        /// <summary>
        /// Controlador para obtener los datos de la gráfica de publicaciones (horizontal).
        /// </summary>
        /// <param name="pIdProyecto">ID del proyecto en cuestión.</param>
        /// <param name="pParametros">Filtros de las facetas.</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaPublicacionesHorizontal")]
        public IActionResult DatosGraficaPublicacionesHorizontal(string pIdProyecto, string pParametros)
        {
            AccionesProyecto accionProyecto = new AccionesProyecto();
            DataGraficaAreasTags datosPublicaciones = accionProyecto.GetDatosGraficaPublicacionesHorizontal(pIdProyecto, pParametros);

            return Ok(datosPublicaciones);
        }

        /// <summary>
        /// Controlador para obtener los datos de la persona en la cabecera de la ficha.
        /// </summary>
        /// <param name="pIdPersona">ID de la persona en cuestión.</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosFichaPersona")]
        public IActionResult DatosFichaPersona(string pIdPersona)
        {
            AccionesPersona accionPersona = new AccionesPersona();
            Dictionary<string, int> datosCabeceraFichas = accionPersona.GetDatosCabeceraPersona(pIdPersona);

            return Ok(datosCabeceraFichas);
        }

        /// <summary>
        /// Controlador para obtener los datos de los grupos.
        /// </summary>
        /// <param name="pIdPersona">ID de la persona en cuestión.</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGruposPersona")]
        public IActionResult DatosGruposPersona(string pIdPersona)
        {
            AccionesPersona accionPersona = new AccionesPersona();
            List<string> datosPublicacionesPersona = accionPersona.GetGrupoInvestigacion(pIdPersona);

            return Ok(datosPublicacionesPersona);
        }

        /// <summary>
        /// Controlador para obtener los datos de las categorias.
        /// </summary>
        /// <param name="pIdPersona">ID de la persona en cuestión.</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosCategoriasPersona")]
        public IActionResult DatosCategoriasPersona(string pIdPersona)
        {
            AccionesPersona accionPersona = new AccionesPersona();
            List<Dictionary<string, string>> datosPublicacionesPersona = accionPersona.GetTopicsPersona(pIdPersona);

            return Ok(datosPublicacionesPersona);
        }

        /// <summary>
        /// Controlador para obtener los datos de la gráfica en horizontal de personas.
        /// </summary>
        /// <param name="pIdPersona">ID de la persona en cuestión.</param>
        /// <param name="pParametros"></param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaHorizontalPersonas")]
        public IActionResult DatosGraficaHorizontalPersonas(string pIdPersona, string pParametros)
        {
            AccionesPersona accionPersona = new AccionesPersona();
            DataGraficaAreasTags datosPublicacionesPersona = accionPersona.GetDatosGraficaProyectosPersonaHorizontal(pIdPersona, pParametros);

            return Ok(datosPublicacionesPersona);
        }

        /// <summary>
        /// Controlador para obtener los datos de la gráfica de nodos de personas.
        /// </summary>
        /// <param name="pIdPersona">ID de la persona en cuestión.</param>
        /// <param name="pParametros">Filtros de la búsqueda.</param>
        /// <param name="pMax">Nº máximo para pintar</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DatosGraficaColaboradoresPersonas")]
        public IActionResult DatosGraficaColaboradoresPersonas(string pIdPersona, string pParametros, int pMax)
        {
            AccionesPersona accionPersona = new AccionesPersona();
            List<DataItemRelacion> datosPublicacionesPersona = accionPersona.GetDatosGraficaRedColaboradoresPersonas(pIdPersona, pParametros, pMax);

            return Ok(datosPublicacionesPersona);
        }

        /// <summary>
        /// Controlador para obtener los datos del documento en la cabecera de la ficha.
        /// </summary>
        /// <param name="pIdDocumento">ID del documento en cuestión.</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("GetDatosCabeceraDocumento")]
        public IActionResult GetDatosCabeceraDocumento(string pIdDocumento)
        {
            AccionesPublicaciones accionDocumento = new AccionesPublicaciones();
            Dictionary<string, int> datosCabeceraFichas = accionDocumento.GetDatosCabeceraDocumento(pIdDocumento);

            return Ok(datosCabeceraFichas);

        }
    }
}