using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Rol de proyecto
    /// </summary>
    public class RolProyecto : SGI_Base
    {
        /// <summary>
        /// Identificador del rol proyecto.
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
        /// Abreviatura identificativa del rol.
        /// </summary>
        public string Abreviatura { get; set; }
        /// <summary>
        /// Nombre identificativo del rol.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Campo de texto de introducción libre para descripción ampliada.
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Indica si el rol es el rol principal.
        /// </summary>
        public bool? RolPrincipal { get; set; }
        /// <summary>
        /// Tipo de Orden. 
        /// </summary>
        public string Orden { get; set; }
        /// <summary>
        /// Tipo Equipo Proyecto. 
        /// </summary>
        public string Equipo { get; set; }
        /// <summary>
        /// Indica si esta activo o no. 
        /// </summary>
        public bool? Activo { get; set; }
    }
}