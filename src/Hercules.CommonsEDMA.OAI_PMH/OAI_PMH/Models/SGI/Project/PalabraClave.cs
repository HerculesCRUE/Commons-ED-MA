namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Palabra clave
    /// </summary>
    public class PalabraClave : SGI_Base
    {
        /// <summary>
        /// Identificador único autogenerado de la palabra clave.
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// Identificador del proyecto al que se asocia la palabra clave.
        /// </summary>
        public long? proyectoId { get; set; }
        /// <summary>
        /// Referencia a la palabra clave. 
        /// </summary>
        public string palabraClaveRef { get; set; }
    }
}
