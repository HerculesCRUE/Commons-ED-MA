namespace OAI_PMH.Models.SGI.GruposInvestigacion
{
    /// <summary>
    /// LineaInvestigacion
    /// </summary>
    public class LineaInvestigacion
    {
        /// <summary>
        /// Id.
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// Nombre de la linea.
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Línea activa o no.
        /// </summary>
        public bool? activo { get; set; }
    }
}
