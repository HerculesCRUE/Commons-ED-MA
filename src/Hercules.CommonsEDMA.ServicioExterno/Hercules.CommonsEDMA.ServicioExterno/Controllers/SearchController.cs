using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Hercules.CommonsEDMA.ServicioExterno.Models.Buscador;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class SearchController : ControllerBase
    {

        /// <summary>
        /// Inicia una búsqueda en los elementos seleccionados
        /// </summary>
        /// <param name="stringSearch">String de bíusqueda.</param>
        /// <param name="lang">Idioma</param>
        /// <returns>JSON con los datos necesarios para el JS.</returns>
        [HttpGet("DoMetaSearch")]
        public IActionResult DoMetaSearch(string stringSearch, string lang = "es")
        {
            Dictionary<string, KeyValuePair<bool, List<ObjectSearch>>> resultBusqueda = null; 

            try
            {
                AccionesMetaBusqueda accionBusqueda = new AccionesMetaBusqueda();
                resultBusqueda = accionBusqueda.Busqueda(stringSearch, lang);
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(resultBusqueda);
        }

        /// <summary>
        /// Inicia una búsqueda en los elementos seleccionados
        /// </summary>
        /// <returns>JSON con los un diccionario de 'tipo de items' => 'número de items'.</returns>
        [HttpGet("GetNumItems")]
        public IActionResult GetNumItems()
        {
            Dictionary<string, int> result = null;

            try
            {
                AccionesMetaBusqueda accionBusqueda = new AccionesMetaBusqueda();
                result = accionBusqueda.GetNumItems();
            }
            catch (Exception)
            {
                throw;
            }

            return Ok(result);
        }
    }
}