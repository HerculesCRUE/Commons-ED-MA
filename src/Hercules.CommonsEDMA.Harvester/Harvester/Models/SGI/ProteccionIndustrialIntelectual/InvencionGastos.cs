namespace OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual
{
    /// <summary>
    /// InvencionGasto
    /// </summary>
    public class InvencionGastos
    {
        /// <summary>
        /// Id.
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// InvencionId
        /// </summary>
        public long? invencionId { get; set; }
        /// <summary>
        /// GastoRef
        /// </summary>
        public string gastoRef { get; set; }
        /// <summary>
        /// Estado.
        /// </summary>
        public string estado { get; set; }
        /// <summary>
        /// ImportePendienteDeducir
        /// </summary>
        public float? importePendienteDeducir { get; set; }
        /// <summary>
        /// SolicitudProteccion 
        /// </summary>
        public long? solicitudProteccion { get; set; }
    }
}
