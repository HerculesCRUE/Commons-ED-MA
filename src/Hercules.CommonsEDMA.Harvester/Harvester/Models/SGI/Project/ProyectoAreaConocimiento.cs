using System;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Proyecto area conocimiento
    /// </summary>
    public class ProyectoAreaConocimiento
    {
        /// <summary>
        /// Identificador único del área de conocimiento del proyecto.
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// Referencia al proyecto.
        /// </summary>
        public long? proyectoId { get; set; }
        /// <summary>
        /// Referencia al área de conocimiento en los sistemas de la Universidad.
        /// </summary>
        public string areaConocimientoRef { get; set; }
        /// <summary>
        /// Referencia al usuario creador del registro en los sistemas de la Universidad.
        /// </summary>
        public string createdBy { get; set; }
        /// <summary>
        /// Fecha y hora de creación del registro.
        /// Formato UTC.
        /// </summary>
        public DateTime? creationDate { get; set; }
        /// <summary>
        /// Referencia al último usuario que modificó el registro en los sistemas de la Universidad.
        /// </summary>
        public string lastModifiedBy { get; set; }
        /// <summary>
        /// Fecha y hora de creación del registro.
        /// Formato UTC.
        /// </summary>
        public DateTime? lastModifiedDate { get; set; }
    }
}
