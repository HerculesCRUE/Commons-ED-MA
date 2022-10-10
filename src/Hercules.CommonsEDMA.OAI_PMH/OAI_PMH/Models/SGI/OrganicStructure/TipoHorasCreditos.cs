namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Tipo horas/creditos
    /// </summary>
    public class TipoHorasCreditos : SGI_Base
    {
        /// <summary>
        /// Identificador del tipo de horas o créditos
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Tipo de horas o créditos.
        /// </summary>
        public string Nombre { get; set; }
    }
}
