using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.ProduccionCientifica
{
    /// <summary>
    /// Producción científica
    /// </summary>
    public class ProduccionCientifica : SGI_Base
    {
        /// <summary>
        /// Identificador del item de producción científica en el sistema origen.
        /// </summary>
        public string IdRef { get; set; }
        /// <summary>
        /// Código de la Fecyt para identificar el apartado.
        /// </summary>
        public string EpigrafeCVN { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        public string Estado { get; set; }
        /// <summary>
        /// Lista con los campos definidos en el item
        /// </summary>
        public List<CampoProduccionCientifica> Campos { get; set; }
        /// <summary>
        /// Lista con los autores del item.
        /// </summary>
        public List<Autor> Autores { get; set; }
        /// <summary>
        /// Lista con los índices de impacto del item.
        /// </summary>
        public List<IndiceImpacto> IndicesImpacto { get; set; }
        /// <summary>
        /// Lista con los identificadores de los proyectos SGI con los que se relaciona el item de producción científica
        /// </summary>
        public List<float> Proyectos { get; set; }
        /// <summary>
        /// Lista con las url y/o documentos que acreditan el item.
        /// </summary>
        public List<Acreditacion> Acreditaciones { get; set; }
    }
}
