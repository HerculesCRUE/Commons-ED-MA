namespace OAI_PMH.Models.SGI.ProduccionCientifica
{
    /// <summary>
    /// Producción cientifica estado
    /// </summary>
    public class ProduccionCientificaEstado : SGI_Base
    {
        /// <summary>
        /// Identificador del item de producción científica en el sistema origen.
        /// </summary>
        public string idRef { get; set; }
        /// <summary>
        /// Código de la Fecyt para identificar el apartado.
        /// </summary>
        public string epigrafeCVN { get; set; }
        /// <summary>
        /// Estado
        /// </summary>
        public string estado { get; set; }
    }
}
