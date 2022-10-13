namespace OAI_PMH.Models.SGI.ProduccionCientifica
{
    /// <summary>
    /// Indice de impacto
    /// </summary>
    public class IndiceImpacto
    {
        /// <summary>
        /// Tipo de la fuente de impacto. 
        /// </summary>
        public string FuenteImpacto { get; set; }
        /// <summary>
        /// Valor de la fuente de impacto cuando es un número
        /// </summary>
        public float? Indice { get; set; }
        /// <summary>
        /// Valor de la fuente de impacto cuando se trata de un ranking
        /// </summary>
        public string Ranking { get; set; }
        /// <summary>
        /// Año en el que se estable el valor del índice para la fuente de impacto.
        /// </summary>
        public string Anio { get; set; }
        /// <summary>
        /// En el caso de que en tipo tenga la opción OTROS, sería la descripción de la fuente de impacto
        /// </summary>
        public string OtraFuenteImpacto { get; set; }
        /// <summary>
        /// Indica la posición que ocupa la revista dentro de su categoría
        /// </summary>
        public float? PosicionPublicacion { get; set; }
        /// <summary>
        /// Numero de revistas
        /// </summary>
        public float? NumeroRevistas { get; set; }
        /// <summary>
        /// Indica si se está dentro del 25% de la revista
        /// </summary>
        public bool Revista25 { get; set; }
    }
}
