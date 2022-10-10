using System.Collections.Generic;

namespace Hercules.CommonsEDMA.DisambiguationEngine.Models
{
    /// <summary>
    /// Clase abstracta para representar a una entidad
    /// </summary>
    public abstract class DisambiguableEntity
    {
        /// <summary>
        /// Obtiene los datos de desambiguación de la entidad
        /// </summary>
        /// <returns>Datos de desambiguación</returns>
        public abstract List<DisambiguationData> GetDisambiguationData();

        /// <summary>
        /// Identificador de la entidad
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Identificadores de otras entidades que son diferentes a la actual
        /// </summary>
        public HashSet<string> distincts { get; set; }

        /// <summary>
        /// Entidad bloqueada
        /// </summary>
        public bool block { get; set; }
    }
}
