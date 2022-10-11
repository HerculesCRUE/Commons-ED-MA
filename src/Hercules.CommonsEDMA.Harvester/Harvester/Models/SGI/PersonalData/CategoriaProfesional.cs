using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Categoría profesional
    /// </summary>
    public class CategoriaProfesional
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// Nombre de la categoría profesional de la persona dentro de la universidad
        /// </summary>
        public string nombre { get; set; }
    }
}