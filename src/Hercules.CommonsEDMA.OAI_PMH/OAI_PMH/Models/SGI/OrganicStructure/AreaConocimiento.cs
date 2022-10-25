namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Area de conocimiento de la Universidad
    /// </summary>
    public class AreaConocimiento : SGI_Base
    {
        /// <summary>
        /// Identificador del área de conocimiento
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del área de conocimiento.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Identificador de la entidad padre del área de conocimiento.
        /// </summary>
        public string PadreId { get; set; }
    }
}
