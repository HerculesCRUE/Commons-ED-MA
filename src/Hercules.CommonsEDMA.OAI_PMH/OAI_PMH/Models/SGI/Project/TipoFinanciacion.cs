namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Tipo de financiación
    /// </summary>
    public class TipoFinanciacion : SGI_Base
    {
        /// <summary>
        /// Identificador del tipo de financiación.
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
        /// Nombre del tipo de financiación.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Descripción del tipo de financiación.
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Flag a través del que se implementa el borrado lógico de los registros de esta tabla.
        /// </summary>
        public bool? Activo { get; set; }
    }
}