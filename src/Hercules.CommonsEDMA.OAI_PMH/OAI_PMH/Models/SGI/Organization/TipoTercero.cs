using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Organization
{
    /// <summary>
    /// Tipo de tercero
    /// </summary>
    public class TipoTercero : SGI_Base
    {
        /// <summary>
        /// Identificador de la entidad TipoTercero.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Clase de tercero.
        /// </summary>
        public string Clase { get; set; }
    }
}