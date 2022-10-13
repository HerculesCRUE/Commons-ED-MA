namespace OAI_PMH.Models.SGI.FormacionAcademica
{
    /// <summary>
    /// Conocimiento de idiomas
    /// </summary>
    public class ConocimientoIdiomas
    {
        /// <summary>
        /// Identificador del idioma
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del idioma
        /// </summary>
        public string NombreIdioma { get; set; }
        /// <summary>
        /// Comprensión auditiva
        /// </summary>
        public string CompresionAuditiva { get; set; }
        /// <summary>
        /// Comprensión lectora
        /// </summary>
        public string CompresionLectora { get; set; }
        /// <summary>
        /// Interacción oral
        /// </summary>
        public string InteraccionOral { get; set; }
        /// <summary>
        /// Expresión oral
        /// </summary>
        public string ExpresionOral { get; set; }
        /// <summary>
        /// Expresión escrita
        /// </summary>
        public string ExpresionEscrita { get; set; }
    }
}
