using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Hercules.CommonsEDMA.ServicioExterno.Models;
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
            DataGraficaPublicaciones datosPublicaciones = null;
            try
            {
                AccionesPublicaciones accionesPublicaciones = new AccionesPublicaciones();
                datosPublicaciones = accionesPublicaciones.GetDatosGraficaPublicaciones(pParametros);
            }
            catch (Exception)
            {
                throw;
            }
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
            GraficasProyectos datosProyectos = null;
            try
            {
                AccionesProyecto accionesProyecto = new AccionesProyecto();
                datosProyectos = accionesProyecto.GetDatosGraficaProyectos(pParametros);
            }
            catch (Exception)
            {
                throw;
            }

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
            DataGraficaAreasTags datos = null;

            try
            {
                AccionesAreasTematicas accionesAreasTematicas = new AccionesAreasTematicas();
                datos = accionesAreasTematicas.DatosGraficaAreasTematicas(pId, pType, pAnioInicio, pAnioFin);
            }
            catch (Exception)
            {
                throw;
            }

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
            List<DataItemRelacion> datos = null;

            try
            {
                AccionesAreasTematicas accionesAreasTematicas = new AccionesAreasTematicas();
                datos = accionesAreasTematicas.DatosGraficaAreasTematicasArania(pId, pType, pAnioInicio, pAnioFin, pNumAreas);
            }
            catch (Exception)
            {
                throw;
            }

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
            List<DataItemRelacion> datosGrupo = null;

            try
            {
                AccionesGroup accionGrupo = new AccionesGroup();
                datosGrupo = accionGrupo.DatosGraficaMiembrosGrupo(pIdGrupo, pParametros);
            }
            catch (Exception)
            {
                throw;
            }

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
            List<DataItemRelacion> datosGrupo = null;

            try
            {
                AccionesGroup accionGrupo = new AccionesGroup();
                datosGrupo = accionGrupo.DatosGraficaColaboradoresGrupo(pIdGrupo, pParametros, pMax);
            }
            catch (Exception)
            {
                throw;
            }

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
            Dictionary<string, int> datosCabeceraFichas = null;

            try
            {
                AccionesProyecto accionProyecto = new AccionesProyecto();
                datosCabeceraFichas = accionProyecto.GetDatosCabeceraProyecto(pIdProyecto);
            }
            catch (Exception)
            {
                throw;
            }

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
            List<DataItemRelacion> datosRedColaboradores = null;

            try
            {
                AccionesProyecto accionProyecto = new AccionesProyecto();
                datosRedColaboradores = accionProyecto.DatosGraficaRedMiembros(pIdProyecto, pParametros);
            }
            catch (Exception)
            {
                throw;
            }

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
            List<DataItemRelacion> datosRedColaboradores = null;

            try
            {
                AccionesProyecto accionProyecto = new AccionesProyecto();
                datosRedColaboradores = accionProyecto.GetDatosGraficaRedColaboradores(pIdProyecto, pParametros, pMax);
            }
            catch (Exception)
            {
                throw;
            }

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
            DataGraficaAreasTags datosPublicaciones = null;

            try
            {
                AccionesProyecto accionProyecto = new AccionesProyecto();
                datosPublicaciones = accionProyecto.GetDatosGraficaPublicacionesHorizontal(pIdProyecto, pParametros);
            }
            catch (Exception)
            {
                throw;
            }

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
            Dictionary<string, int> datosCabeceraFichas = null;

            try
            {
                AccionesPersona accionPersona = new AccionesPersona();
                datosCabeceraFichas = accionPersona.GetDatosCabeceraPersona(pIdPersona);
            }
            catch (Exception)
            {
                throw;
            }

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
            List<string> datosPublicacionesPersona = null;

            try
            {
                AccionesPersona accionPersona = new AccionesPersona();
                datosPublicacionesPersona = accionPersona.GetGrupoInvestigacion(pIdPersona);
            }
            catch (Exception)
            {
                throw;
            }

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
            List<Dictionary<string, string>> datosPublicacionesPersona = null;

            try
            {
                AccionesPersona accionPersona = new AccionesPersona();
                datosPublicacionesPersona = accionPersona.GetTopicsPersona(pIdPersona);
            }
            catch (Exception)
            {
                throw;
            }

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
            DataGraficaAreasTags datosPublicacionesPersona = null;

            try
            {
                AccionesPersona accionPersona = new AccionesPersona();
                datosPublicacionesPersona = accionPersona.GetDatosGraficaProyectosPersonaHorizontal(pIdPersona, pParametros);
            }
            catch (Exception)
            {
                throw;
            }

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
            List<DataItemRelacion> datosPublicacionesPersona = null;

            try
            {
                AccionesPersona accionPersona = new AccionesPersona();
                datosPublicacionesPersona = accionPersona.GetDatosGraficaRedColaboradoresPersonas(pIdPersona, pParametros, pMax);
            }
            catch (Exception)
            {
                throw;
            }

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
            Dictionary<string, int> datosCabeceraFichas = null;

            try
            {
                AccionesPublicaciones accionDocumento = new AccionesPublicaciones();
                datosCabeceraFichas = accionDocumento.GetDatosCabeceraDocumento(pIdDocumento);
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(datosCabeceraFichas);

        }

        /// <summary>
        /// Controlador para obtener los datos de las fuentes de RO.
        /// </summary>
        /// <param name="pIdGnossUser">Usuario de gnoss.</param>
        /// <returns>Diccionario con los datos.</returns>
        [HttpGet("GetDatosRedesUsuario")]
        public IActionResult GetDatosRedesUsuario(string pIdGnossUser)
        {

            if (!Security.CheckUser(new Guid(pIdGnossUser), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            Dictionary<string, string> datosRedesUsuario = null;
            try
            {
                AccionesRedesUsuario accionDocumento = new AccionesRedesUsuario();
                //datosRedesUsuario = accionDocumento.GetDataRedesUsuario(pIdGnossUser);
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(datosRedesUsuario);
        }

        /// <summary>
        /// Controlador para modificar los datos de las fuentes de RO.
        /// </summary>
        /// <param name="pIdGnossUser">Usuario de gnoss.</param>
        /// <param name="pDicDatosAntiguos">Datos antiguos a modificar.</param>
        /// <param name="pDicDatosNuevos">Datos nuevos a modificar.</param>
        /// <returns>Diccionario con los datos.</returns>
        [HttpGet("SetDatosRedesUsuario")]
        public IActionResult SetDatosRedesUsuario(string pIdGnossUser, string pDicDatosAntiguos, string pDicDatosNuevos)
        {
            if (!Security.CheckUser(new Guid(pIdGnossUser), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            try
            {
                AccionesRedesUsuario accionDocumento = new AccionesRedesUsuario();
                //accionDocumento.SetDataRedesUsuario(pIdGnossUser, pDicDatosAntiguos, pDicDatosNuevos);
            }
            catch (Exception)
            {
                throw;
            }

            return Ok();
        }




        /// <summary>
        /// Controlador para obtener la información del usuario en la home de ED.
        /// </summary>
        /// <param name="pIdGnossUser">Usuario de gnoss.</param>
        /// <returns>Diccionario con los datos.</returns>
        [HttpGet("GetInfoHomeEdUser")]
        public IActionResult GetInfoHomeEdUser(string pIdGnossUser)
        {
            Dictionary<string, string> datosUsuario = null;
            if (!Security.CheckUser(new Guid(pIdGnossUser), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }
            try
            {
                AccionesPersona accionP = new AccionesPersona();
                datosUsuario = accionP.GetInfoHomeEdUser(pIdGnossUser);
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(datosUsuario);
        }
    }
}