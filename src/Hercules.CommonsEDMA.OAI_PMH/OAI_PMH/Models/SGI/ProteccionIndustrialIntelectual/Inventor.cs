namespace OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual
{
    /// <summary>
    /// Inventor
    /// </summary>
    public class Inventor
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
        /// InventorRef.
        /// </summary>
        public string inventorRef { get; set; }
        /// <summary>
        /// Participacion.
        /// </summary>
        public float? participacion { get; set; }
        /// <summary>
        /// RepartoUniversidad
        /// </summary>
        public bool? repartoUniversidad { get; set; }
        /// <summary>
        /// Activo
        /// </summary>
        public bool? activo { get; set; }
    }
}
