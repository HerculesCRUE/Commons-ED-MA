namespace OAI_PMH.Models.SGI.OrganicStructure
{
    /// <summary>
    /// Director
    /// </summary>
    public class Director : SGI_Base
    {
        /// <summary>
        /// Identificador de la entidad director/a o co-director/a
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// Identificador o Referencia de la persona.
        /// </summary>
        public string PersonaRef { get; set; }
        /// <summary>
        /// Modalidad de participación en el congreso.
        /// </summary>
        public TipoParticipacion TipoDireccion { get; set; }
    }
}
