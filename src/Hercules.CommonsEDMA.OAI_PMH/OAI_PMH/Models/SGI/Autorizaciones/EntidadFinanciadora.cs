namespace OAI_PMH.Models.SGI.Autorizacion
{
    /// <summary>
    /// Entidad financiadora
    /// </summary>
    public class EntidadFinanciadora
    {
        /// <summary>
        /// Referencia de la entidad dentro del Sistema de gestión de empresas
        /// </summary>
        public string entidadFinanciadoraRef { get; set; }
        /// <summary>
        /// Nombre de la entidad recogido sobre el campo Entidades financiadoras del CVN
        /// </summary>
        public string datosEntidadFinanciadora { get; set; }
    }
}
