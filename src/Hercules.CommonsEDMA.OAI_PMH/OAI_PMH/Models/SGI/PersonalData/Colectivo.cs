using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Listado de colectivos que tiene la Universidad
    /// </summary>
    public class Colectivo : SGI_Base
    {
        /// <summary>
        /// Identificador de la entidad Colectivo.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del colectivo
        /// </summary>
        public string Nombre { get; set; }
    }
}
