using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Fotografía
    /// </summary>
    public class Fotografia : SGI_Base
    {
        /// <summary>
        /// Contenido de la fotografía digital de la persona (en bytes y en base 64).
        /// </summary>
        public string Contenido { get; set; }
        /// <summary>
        /// MIMEType
        /// </summary>
        public string MIMEType { get; set; }
    }
}
