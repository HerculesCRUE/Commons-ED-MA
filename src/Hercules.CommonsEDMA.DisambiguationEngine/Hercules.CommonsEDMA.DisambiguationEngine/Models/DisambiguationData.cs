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

        /// <summary>
        /// Constructor vacio
        /// </summary>
        public DisambiguationDataConfig()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="score">Score</param>
        public DisambiguationDataConfig(DisambiguationDataConfigType type, float score)
        {
            this.type = type;
            this.score = score;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type</param>
        /// <param name="score">Score</param>
        /// <param name="scoreMinus">ScoreMinus</param>
        public DisambiguationDataConfig(DisambiguationDataConfigType type, float score, float scoreMinus)
        {
            this.type = type;
            this.score = score;
            this.scoreMinus = scoreMinus;
        }
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

        /// <summary>
        /// Constructor vacio
        /// </summary>
        public DisambiguationData()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">DisambiguationDataConfig</param>
        /// <param name="property">Property</param>
        /// <param name="value">Value</param>
        /// <param name="values">Values</param>
        public DisambiguationData(DisambiguationDataConfig config, string property, string value, HashSet<string> values)
        {
            this.config = config;
            this.property = property;
            this.value = value;
            this.values = values;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">DisambiguationDataConfig</param>
        /// <param name="property">Property</param>
        /// <param name="value">Value</param>
        public DisambiguationData(DisambiguationDataConfig config, string property, string value)
        {
            this.config = config;
            this.property = property;
            this.value = value;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="config">DisambiguationDataConfig</param>
        /// <param name="property">Property</param>
        /// <param name="value">Value</param>
        public DisambiguationData(DisambiguationDataConfig config, string property, HashSet<string> values)
        {
            this.config = config;
            this.property = property;
            this.values = values;
        }
    }
}
