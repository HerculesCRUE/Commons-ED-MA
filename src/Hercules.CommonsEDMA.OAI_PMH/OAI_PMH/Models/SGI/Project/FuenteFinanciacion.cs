using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Fuente de financiación
    /// </summary>
    public class FuenteFinanciacion : SGI_Base
    {
        /// <summary>
        /// Identificador de la fuente de financiación
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
        /// Nombre identificativo de la fuente de financiación.
        /// </summary>
        public static string Nombre { get; set; }
        /// <summary>
        /// Descripción de la fuente de financiación.
        /// </summary>
        public static string Descripcion { get; set; }
        /// <summary>
        /// Flag que identifica si la fuente de financiación procede o no de fondos estructurales europeos.
        /// </summary>
        public static bool? FondoEstructural { get; set; }
        /// <summary>
        /// Entidad que representa un ámbito geográfico.
        /// </summary>
        public static TipoAmbitoGeografico TipoAmbitoGeografico { get; set; }
        /// <summary>
        /// Entidad que representa un tipo de origen de fuente de financiación.
        /// </summary>
        public static TipoOrigenFuenteFinanciacion TipoOrigenFuenteFinanciacion { get; set; }
        /// <summary>
        /// Flag con el que se da cobertura al borrado lógico de los registros de esta tabla.
        /// </summary>
        public static bool? Activo { get; set; }
    }
}