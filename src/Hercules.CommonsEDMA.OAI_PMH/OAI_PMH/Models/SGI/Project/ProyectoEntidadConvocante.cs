using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Contiene los datos de una entidad convocante. 
    /// </summary>
    public class ProyectoEntidadConvocante : SGI_Base
    {
        /// <summary>
        /// Identificador de la entidad convocante del proyecto.
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
        /// <summary>
        /// Entidad que representa el Programa al que se asocia la entidad convocante en la Convocatoria. 
        /// </summary>
        public Programa ProgramaConvocatoria { get; set; }
        /// <summary>
        /// Entidad que representa el del Programa al que se asocia la entidad convocante en el proyecto.
        /// </summary>
        public Programa Programa { get; set; }
    }
}
