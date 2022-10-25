namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Clasificación
    /// </summary>
    public class Clasificacion : SGI_Base
    {
        /// <summary>
        /// Identificador de la clasificación.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Código de la clasificación.
        /// </summary>
        public string Codigo { get; set; }
        /// <summary>
        /// Nombre de la clasificación.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Identificador de la entidad padre de la clasificación
        /// </summary>
        public string PadreId { get; set; }
    }
}
