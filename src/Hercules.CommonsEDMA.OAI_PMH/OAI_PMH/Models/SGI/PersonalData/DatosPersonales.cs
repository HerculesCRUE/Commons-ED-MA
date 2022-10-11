using OAI_PMH.Models.SGI.OrganicStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Contiene los datos personales de una persona
    /// </summary>
    public class DatosPersonales : SGI_Base
    {
        /// <summary>
        /// Fecha de nacimiento
        /// </summary>
        public DateTime? FechaNacimiento { get; set; }
        /// <summary>
        /// País de nacimiento
        /// </summary>
        public Pais PaisNacimiento { get; set; }
        /// <summary>
        /// Comunidad autónoma de nacimiento
        /// </summary>
        public ComunidadAutonoma ComAutonomaNacimiento { get; set; }
        /// <summary>
        /// Ciudad de nacimiento
        /// </summary>
        public string CiudadNacimiento { get; set; }
    }
}
