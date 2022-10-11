using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Entidad que representa un modelo de ejecución.
    /// </summary>
    public class ModeloEjecucion
    {
        /// <summary>
        /// Identificador del modelo de ejecución
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
        /// Nombre del modelo de ejecución
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Descripción del modelo de ejecución
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Indica si el registro está activo o no. 
        /// </summary>
        public bool? Activo { get; set; }
        /// <summary>
        /// Flag que distingue los modelos de ejecución que representan proyectos externos.
        /// </summary>
        public bool? Externo { get; set; }
        /// <summary>
        /// Flag que distingue los modelos de ejecución basados en facturación (contratos).
        /// </summary>
        public bool? Contrato { get; set; }
    }
}