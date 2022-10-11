using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Tipo de finalidad
    /// </summary>
    public class TipoFinalidad : SGI_Base
    {
        /// <summary>
        /// Identificador del tipo de finalidad.
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
        /// Nombre del tipo de finalidad.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Descripción del tipo de finalidad.
        /// </summary>
        public string Descripcion { get; set; }
        /// <summary>
        /// Flag con el que se da cobertura al borrado lógico de los registros de esta tabla.
        /// </summary>
        public string Activo { get; set; }
        /// <summary>
        /// Iva
        /// </summary>
        public double? Iva { get; set; }
        /// <summary>
        /// Fecha de inicio
        /// </summary>
        public string FechaInicio { get; set; }
        /// <summary>
        /// Fecha de fin
        /// </summary>
        public string FechaFin { get; set; }
    }
}