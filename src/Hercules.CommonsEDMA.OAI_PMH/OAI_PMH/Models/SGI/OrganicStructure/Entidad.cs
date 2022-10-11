namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Entidad
    /// </summary>
    public class Entidad : SGI_Base
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Referencia o Identificador de la entidad en el sistema externo de Empresas.
        /// </summary>
        public string EntidadRef { get; set; }
        /// <summary>
        /// Referencia o Identificador de la entidad en el sistema externo de Empresas.
        /// </summary>
        public TipoEntidadOrganizadoraEvento TipoEntidad{get;set;}

    }
}
