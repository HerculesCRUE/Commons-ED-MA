namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Tipo de docente
    /// </summary>
    public class TipoDocente : SGI_Base
    {
        /// <summary>
        /// Identificador del tipo de labor de docente
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Tipo de labor de docente.
        /// </summary>
        public string Nombre { get; set; }
    }
}
