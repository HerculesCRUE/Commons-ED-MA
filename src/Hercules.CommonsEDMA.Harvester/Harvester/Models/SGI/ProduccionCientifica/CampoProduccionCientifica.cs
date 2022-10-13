using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.ProduccionCientifica
{
    /// <summary>
    /// Campo producción cientifica
    /// </summary>
    public class CampoProduccionCientifica
    {
        /// <summary>
        /// Código de la Fecyt para identificar el campo del item.
        /// </summary>
        public string CodigoCVN { get; set; }
        /// <summary>
        /// Lista con los valores indicados en codigoCVN. 
        /// </summary>
        public List<string> Valores { get; set; }
    }
}
