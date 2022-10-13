namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Tipo de origen de la fuente de financiación
    /// </summary>
    public class TipoOrigenFuenteFinanciacion
    {
        /// <summary>
        /// Identificador del tipo de origen de fuente de financiación
        /// </summary>
        public static long? Id { get; set; }
        /// <summary>
        /// Nombre del usuario que ha creado la entidad.
        /// </summary>
        public static string CreatedBy { get; set; }
        /// <summary>
        /// Fecha de la creación de la entidad.
        /// </summary>
        public static string CreationDate { get; set; }
        /// <summary>
        /// Nombre del usuario que ha modificado por última vez la entidad.
        /// </summary>
        public static string LastModifiedBy { get; set; }
        /// <summary>
        /// Fecha de la última modificación de la entidad.
        /// </summary>
        public static string LastModifiedDate { get; set; }
        /// <summary>
        /// Nombre del tipo de origen de fuente de financiación.
        /// </summary>
        public static string Nombre { get; set; }
        /// <summary>
        /// Flag a través del que se implementa el borrado lógico de los registros de esta tabla.
        /// </summary>
        public static bool? Activo { get; set; }
    }
}
