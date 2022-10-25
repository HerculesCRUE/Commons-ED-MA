using OAI_PMH.Models.SGI.OrganicStructure;
using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Contiene los datos de contacto de una persona
    /// </summary>
    public class DatosContacto : SGI_Base
    {
        /// <summary>
        /// Pais de contacto
        /// </summary>
        public Pais PaisContacto { get; set; }
        /// <summary>
        /// Comunidad autónoma de contaco
        /// </summary>
        public ComunidadAutonoma ComAutonomaContacto { get; set; }
        /// <summary>
        /// Provincia de contacto
        /// </summary>
        public Provincia ProvinciaContacto { get; set; }
        /// <summary>
        /// Ciudad de contacto
        /// </summary>
        public string CiudadContacto { get; set; }
        /// <summary>
        /// Dirección de contacto
        /// </summary>
        public string DireccionContacto { get; set; }
        /// <summary>
        /// Código postal de contacto
        /// </summary>
        public string CodigoPostalContacto { get; set; }
        /// <summary>
        /// Emails
        /// </summary>
        public List<Email> Emails { get; set; }
        /// <summary>
        /// Telefonos
        /// </summary>
        public List<string> Telefonos { get; set; }
        /// <summary>
        /// Moviles
        /// </summary>
        public List<string> Moviles { get; set; }
    }
}
