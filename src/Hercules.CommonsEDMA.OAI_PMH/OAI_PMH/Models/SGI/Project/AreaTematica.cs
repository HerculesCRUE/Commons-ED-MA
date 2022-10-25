namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Area temática
    /// </summary>
    public class AreaTematica : SGI_Base
    {
        /// <summary>
        /// Identificador del área temática
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
        /// Nombre del área temática. El nombre del nodo raíz del árbol da el nombre al listado de áreas temáticas.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Descripción del área temática.
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Entidad que representa el área temática padre.
        /// </summary>
        public AreaTematica Padre { get; set; }
        /// <summary>
        /// Flag a través del que se implementa el borrado lógico de los registros de esta tabla.
        /// </summary>
        public bool? Activo { get; set; }
    }
}