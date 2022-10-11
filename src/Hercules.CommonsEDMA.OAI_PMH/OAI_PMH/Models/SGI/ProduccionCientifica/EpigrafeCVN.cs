using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.ProduccionCientifica
{
    /// <summary>
    /// Epigrafe CVN
    /// </summary>
    public class EpigrafeCVN : SGI_Base
    {
        /// <summary>
        /// Código de uno de lo apartados del CVN que forman parte de la Producción científica y que necesita validación
        /// </summary>
        public string Codigo { get; set; }
        /// <summary>
        /// Códigos de los campos del apartado del CVN que se tienen que enviar a Producción científica para su validación
        /// </summary>
        public List<string> Campos { get; set; }
    }
}
