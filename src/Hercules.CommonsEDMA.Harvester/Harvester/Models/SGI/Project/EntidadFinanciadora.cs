namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Entidad financiadora
    /// </summary>
    public class EntidadFinanciadora
    {
        /// <summary>
        /// Referencia de la entidad dentro del Sistema de gestión de empresas corporativo asociado al dato recogido sobre el campo Entidades financiadoras del CVN.
        /// </summary>
        public string EntidadFinanciadoraRef { get; set; }
        /// <summary>
        /// Nombre de la entidad recogido sobre el campo Entidades financiadoras del CVN.
        /// </summary>
        public string DatosEntidadFinanciadora { get; set; }
    }
}
