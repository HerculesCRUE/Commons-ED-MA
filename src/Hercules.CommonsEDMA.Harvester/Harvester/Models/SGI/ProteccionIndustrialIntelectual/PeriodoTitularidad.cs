using System;

namespace OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual
{
    /// <summary>
    /// PeriodoTitularidad
    /// </summary>
    public class PeriodoTitularidad
    {
        /// <summary>
        /// Id.
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// InvencionId.
        /// </summary>
        public long? invencionId { get; set; }
        /// <summary>
        /// FechaInicio
        /// </summary>
        public DateTime? fechaInicio { get; set; }
        /// <summary>
        /// FechaFin
        /// </summary>
        public DateTime? fechaFin { get; set; }
    }
}
