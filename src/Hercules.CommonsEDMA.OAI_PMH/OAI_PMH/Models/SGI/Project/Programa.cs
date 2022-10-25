namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Programa
    /// </summary>
    public class Programa : SGI_Base
    {
        /// <summary>
        /// Identificador del programa.
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
        /// Nombre del programa
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Descripción del programa.
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Entidad que representa el programa padre.
        /// </summary>
        public Programa Padre { get; set; }
        /// <summary>
        /// Flag con el que se da cobertura al borrado lógico de los registros de esta tabla.
        /// </summary>
        public bool Activo { get; set; }
    }
}