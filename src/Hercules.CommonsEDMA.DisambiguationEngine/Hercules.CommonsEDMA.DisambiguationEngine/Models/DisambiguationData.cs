using System.Collections.Generic;

namespace Hercules.CommonsEDMA.DisambiguationEngine.Models
{
    /// <summary>
    /// Tipos datos de desambiguación
    /// </summary>
    public enum DisambiguationDataConfigType
    {
        /// <summary>
        /// Identificadores
        /// </summary>
        equalsIdentifiers,
        /// <summary>
        /// Títulos
        /// </summary>
        equalsTitle,
        /// <summary>
        /// Items
        /// </summary>
        equalsItem,
        /// <summary>
        /// Listado de items
        /// </summary>
        equalsItemList,
        /// <summary>
        /// Nombres de personas
        /// </summary>
        algoritmoNombres
    }

    /// <summary>
    /// Configuración de un dato de desambiguación
    /// </summary>
    public class DisambiguationDataConfig
    {
        /// <summary>
        /// Tipo datos de desambiguación
        /// </summary>
        public DisambiguationDataConfigType type { get; set; }
        /// <summary>
        /// Score positivo (0-1)
        /// </summary>
        public float score { get; set; }
        /// <summary>
        /// Score negativo (0-1)
        /// </summary>
        public float scoreMinus { get; set; }
    }

    /// <summary>
    /// Dato de desambiguación
    /// </summary>
    public class DisambiguationData
    {
        /// <summary>
        /// Configuración del dato de desambiguación
        /// </summary>
        public DisambiguationDataConfig config { get; set; }
        /// <summary>
        /// Nombre de la propiedad
        /// </summary>
        public string property { get; set; }
        /// <summary>
        /// Valor de la propiedad
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// Valores de la propiedad
        /// </summary>
        public HashSet<string> values { get; set; }
    }
}
