namespace OAI_PMH.Models.SGI.ProduccionCientifica
{
    /// <summary>
    /// Autor
    /// </summary>
    public class Autor : SGI_Base
    {
        /// <summary>
        /// Identificador único de la persona dentro del sistema de gestión de personas de la Universidad
        /// </summary>
        public string PersonaRef { get; set; }
        /// <summary>
        /// Firma del autor.
        /// </summary>
        public string Firma { get; set; }
        /// <summary>
        /// Nombre de la persona.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Apellidos de la persona.
        /// </summary>
        public string Apellidos { get; set; }
        /// <summary>
        /// Indica la posición del autor dentro del listado de todos los autores
        /// </summary>
        public float? Orden { get; set; }
        /// <summary>
        /// Identificador ORCID
        /// </summary>
        public string OrcidId { get; set; }
        /// <summary>
        /// Indica si el autor es un Investigador Principal o no
        /// </summary>
        public bool? Ip { get; set; }
    }
}
