namespace OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual
{
    /// <summary>
    /// SectorAplicacion
    /// </summary>
    public class SectorAplicacion
    {
        /// <summary>
        /// Id.
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// invencionId
        /// </summary>
        public long? invencionId { get; set; }
        /// <summary>
        /// sectorAplicacion
        /// </summary>
        public Sector sectorAplicacion { get; set; }
    }
}
