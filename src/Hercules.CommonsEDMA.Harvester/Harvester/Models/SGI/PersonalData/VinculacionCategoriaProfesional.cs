using System;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Vinculación categoría profesional
    /// </summary>
    public class VinculacionCategoriaProfesional 
    {
        /// <summary>
        /// Categoría profesional de la persona
        /// </summary>
        public CategoriaProfesional categoriaProfesional { get; set; }
        /// <summary>
        /// Fecha de obtención de categoría profesional de la persona
        /// </summary>
        public DateTime? fechaObtencion { get; set; }
    }
}
