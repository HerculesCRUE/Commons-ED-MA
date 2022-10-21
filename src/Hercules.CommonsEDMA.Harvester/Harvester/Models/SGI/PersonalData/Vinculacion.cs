using OAI_PMH.Models.SGI.OrganicStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Contiene los datos de vinculación de una persona a la estructura organizativa Universitaria.
    /// </summary>
    public class Vinculacion
    {
        /// <summary>
        /// Categoría profesional
        /// </summary>
        public CategoriaProfesional CategoriaProfesional { get; set; }

        /// <summary>
        /// Fecha obtención categoría
        /// </summary>
        public DateTime? fechaObtencionCategoria { get; set; }
        /// <summary>
        /// Departamento de la Universidad
        /// </summary>
        public Departamento Departamento { get; set; }
        /// <summary>
        /// Centro de la Universidad
        /// </summary>
        public Centro centro { get; set; }
        /// <summary>
        /// Area de conocimiento de la Universidad
        /// </summary>
        public AreaConocimiento AreaConocimiento { get; set; }
        /// <summary>
        /// Empresa externa relacionada en la actualidad con la persona
        /// </summary>
        public string EmpresaRef { get; set; }
        /// <summary>
        /// Indica si es personal de la Universidad o no
        /// </summary>
        public bool? PersonalPropio { get; set; }
        /// <summary>
        /// Identificador de la entidad que representa a la universidad
        /// </summary>
        public string EntidadPropiaRef { get; set; }
    }
}
