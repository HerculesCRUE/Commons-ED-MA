namespace OAI_PMH.Models.SGI.Organization
{
    /// <summary>
    /// Empresa
    /// </summary>
    public class Empresa : SGI_Base
    {
        /// <summary>
        /// Identificador de la empresa.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre de la empresa.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Razón social de la empresa.
        /// </summary>
        public string RazonSocial { get; set; }
        /// <summary>
        /// Tipo de identificador fiscal de la empresa.
        /// </summary>
        public TipoIdentificador TipoIdentificador { get; set; }
        /// <summary>
        /// Número de identificación fiscal de la empresa del tipo indicado en "tipoIdentificador".
        /// </summary>
        public string NumeroIdentificacion { get; set; }
        /// <summary>
        /// Indicador de si se trata de una empresa con datos económicos o sin datos económicos
        /// </summary>
        public bool? DatosEconomicos { get; set; }
        /// <summary>
        /// Identificador de la empresa padre o entidad principal.
        /// </summary>
        public string PadreId { get; set; }
        /// <summary>
        /// Datos de contacto
        /// </summary>
        public DatosContacto DatosContacto { get; set; }
    }
}
