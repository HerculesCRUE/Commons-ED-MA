namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Email
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Dirección de email
        /// </summary>
        public string email { get; set; }
        /// <summary>
        /// Indicador de si el email es principal o no. 
        /// </summary>
        public bool? Principal { get; set; }
    }
}
