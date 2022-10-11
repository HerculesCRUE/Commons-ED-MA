using OAI_PMH.Models.SGI.OrganicStructure;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Vinculacion departamentos
    /// </summary>
    public class VinculacionDepartamentos : SGI_Base
    {
        /// <summary>
        /// Departamento al que está asociada la persona en su calidad de PAS o PDI
        /// </summary>
        public Departamento Departamento { get; set; }
        /// <summary>
        /// Tipo de personal. 
        /// </summary>
        public string TipoPersonal { get; set; }
    }
}
