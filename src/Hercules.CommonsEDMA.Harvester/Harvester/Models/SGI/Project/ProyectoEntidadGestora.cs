using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Proyecto entidad gestora
    /// </summary>
    public class ProyectoEntidadGestora
    {
        /// <summary>
        /// Identificador de la entidad gestora del proyecto.
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
        /// Referencia o Identificador de la entidad en el sistema externo de Empresas.
        /// </summary>
        public string EntidadRef { get; set; }
    }
}
