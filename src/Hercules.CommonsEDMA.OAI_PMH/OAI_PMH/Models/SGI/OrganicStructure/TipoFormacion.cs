namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Tipo de formación
    /// </summary>
    public class TipoFormacion : SGI_Base
    {
        /// <summary>
        /// Identificador del tipo de titulación de formación especializada, continuada,...
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Tipo de formación.
        /// </summary>
        public string Nombre { get; set; }
    }
}
