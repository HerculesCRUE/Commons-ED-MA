using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Proyecto IVA
    /// </summary>
    public class ProyectoIVA
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
        /// Identificador del proyecto
        /// </summary>
        public long? ProyectoId { get; set; }
        /// <summary>
        /// Valor del porcentaje del IVA.
        /// </summary>
        public double? Iva { get; set; }
        /// <summary>
        /// Fecha de inicio en la que empieza a aplicarse el porcentaje de IVA al que refiere el registro.
        /// </summary>
        public string FechaInicio { get; set; }
        /// <summary>
        /// Fecha de fin en la que deja de aplicarse el porcentaje de IVA al que refiere el registro.
        /// </summary>
        public string FechaFin { get; set; }
    }
}