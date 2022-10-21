namespace OAI_PMH.Models.SGI.OrganicStructure
{
    public class PersonaSecundaria : SGI_Base
    {
        /// <summary>
        /// Identificador de la persona
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre de la persona
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Primer apellido
        /// </summary>
        public string PrimerApellido { get; set; }
        /// <summary>
        /// Segundo apellido
        /// </summary>
        public string SegundoApellido { get; set; }
    }
}
