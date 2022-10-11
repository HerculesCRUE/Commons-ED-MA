using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Comunidad autónoma
    /// </summary>
    public class ComunidadAutonoma : SGI_Base
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Identificador del país
        /// </summary>
        public string PaisId { get; set; }
    }
}
