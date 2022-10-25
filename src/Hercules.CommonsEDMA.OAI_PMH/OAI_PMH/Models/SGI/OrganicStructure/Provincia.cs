namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Provincia
    /// </summary>
    public class Provincia : SGI_Base
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
        /// Identificador de la comunidad autónoma
        /// </summary>
        public string ComunidadAutonomaId { get; set; }
    }
}
