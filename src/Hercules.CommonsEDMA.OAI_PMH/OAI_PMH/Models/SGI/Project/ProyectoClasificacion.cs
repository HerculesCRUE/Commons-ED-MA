namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Contiene la referencia a la clasificación.
    /// </summary>
    public class ProyectoClasificacion : SGI_Base
    {
        /// <summary>
        /// Identificador de la entidad clasificación proyecto
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
        /// Identificador del proyecto.
        /// </summary>
        public long? ProyectoId { get; set; }
        /// <summary>
        /// Identificador de la clasificación.
        /// </summary>
        public string ClasificacionRef { get; set; }
    }
}
