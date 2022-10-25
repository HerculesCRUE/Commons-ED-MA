namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Proyecto equipo
    /// </summary>
    public class ProyectoEquipo : SGI_Base
    {
        /// <summary>
        /// Identificador de la entidad proyecto equipo.
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
        /// Identificador o Referencia de la persona miembro del equipo de proyecto.
        /// </summary>
        public string PersonaRef { get; set; }
        /// <summary>
        /// Entidad que representa el rol.
        /// </summary>
        public RolProyecto RolProyecto { get; set; }
        /// <summary>
        /// Fecha de inicio para la participación del miembro del equipo de proyecto con el rol seleccionado.
        /// </summary>
        public string FechaInicio { get; set; }
        /// <summary>
        /// Fecha de inicio para la participación del miembro del equipo de proyecto con el rol seleccionado.
        /// </summary>
        public string FechaFin { get; set; }
        /// <summary>
        /// Horas totales de dedicación al proyecto.
        /// </summary>
        public float? HorasDedicacion { get; set; }
    }
}
