using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Datos Academicos
    /// </summary>
    public class DatosAcademicos
    {
        /// <summary>
        /// Nivel Academico
        /// </summary>
        public NivelAcademico NivelAcademico { get; set; }
        /// <summary>
        /// Fecha obtención del nivel académico.
        /// </summary>
        public string Fecha { get; set; }
    }
}
