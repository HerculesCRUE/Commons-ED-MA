using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Departamento
    /// </summary>
    public class Departamento : SGI_Base
    {
        /// <summary>
        /// Identificador del departamento.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del departamento.
        /// </summary>
        public string Nombre { get; set; }
    }
}
