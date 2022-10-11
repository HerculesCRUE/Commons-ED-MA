using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Entidad que representa un estado del proyecto.
    /// </summary>
    public class EstadoProyecto
    {
        /// <summary>
        /// Identificador del estado del proyecto
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
        /// 	Fecha de la última modificación de la entidad.
        /// </summary>
        public string LastModifiedDate { get; set; }
        /// <summary>
        /// Identificador del proyecto.
        /// </summary>
        public long? ProyectoId { get; set; }
        /// <summary>
        /// Estado del proyecto. 
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// Fecha en la que se alcanzó el estado.
        /// </summary>
        public string FechaEstado { get; set; }
        /// <summary>
        /// Comentario que se pude añadir cuando se produce el cambio de estado para recoger cualquier observación.
        /// </summary>
        public string Comentario { get; set; }
    }
}