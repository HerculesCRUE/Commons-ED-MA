namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Tipo ambito geografico
    /// </summary>
    public class TipoAmbitoGeografico : SGI_Base
    {
        /// <summary>
        /// Identificador del tipo de ámbito geográfico.
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
        /// Nombre del ámbito geográfico
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Campo utilizado para dar soporte al borrado lógico de los registros de esta entidad.
        /// </summary>
        public bool? Activo { get; set; }
    }
}