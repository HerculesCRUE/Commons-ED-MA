namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Proyecto entidad financiadora
    /// </summary>
    public class ProyectoEntidadFinanciadora : SGI_Base
    {
        /// <summary>
        /// Identificador de la entidad financiadora del proyecto.
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// Nombre del usuario que ha creado la entidad.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Fecha de la creación de la entidad.
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// Nombre del usuario que ha modificado por última vez la entidad.
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// Fecha de la última modificación de la entidad.
        /// </summary>
        public string LastModifiedDate { get; set; }
        /// <summary>
        /// Identifcador del proyecto.
        /// </summary>
        public long? ProyectoId { get; set; }
        /// <summary>
        /// Referencia o Identificador de la entidad en el sistema externo de Empresas.
        /// </summary>
        public string EntidadRef { get; set; }
        /// <summary>
        /// Entidad que representa una fuente de financiación.
        /// </summary>
        public FuenteFinanciacion FuenteFinanciacion { get; set; }
        /// <summary>
        /// Entidad que representa un tipo de financiación.
        /// </summary>
        public TipoFinanciacion TipoFinanciacion { get; set; }
        /// <summary>
        /// Porcentaje de financiación de la entidad financiadora.
        /// </summary>
        public double? PorcentajeFinanciacion { get; set; }
        /// <summary>
        /// Importe de financiación de la entidad.
        /// </summary>
        public double? ImporteFinanciacion { get; set; }
        /// <summary>
        /// Indica si se trata de una entidad financiadora ajena a la convocatoria o no.
        /// </summary>
        public bool? Ajena { get; set; }
    }
}
