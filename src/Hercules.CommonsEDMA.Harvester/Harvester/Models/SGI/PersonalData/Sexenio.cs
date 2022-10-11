namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Sexenio
    /// </summary>
    public class Sexenio
    {
        /// <summary>
        /// Número de sexenios reconocidos
        /// </summary>
        public string Numero { get; set; }
        /// <summary>
        /// País del reconocimiento
        /// </summary>
        public string PaisRef { get; set; }
        /// <summary>
        /// Identificador de la persona que tiene el sexenio
        /// </summary>
        public string PersonaRef { get; set; }
    }
}
