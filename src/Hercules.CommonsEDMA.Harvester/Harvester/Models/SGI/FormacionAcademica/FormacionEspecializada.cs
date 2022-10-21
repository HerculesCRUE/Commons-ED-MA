using OAI_PMH.Models.SGI.OrganicStructure;
using System;

namespace OAI_PMH.Models.SGI.FormacionAcademica
{
    /// <summary>
    /// Formación especializada, continuada, técnica, profesionalizada, de reciclaje y actualización (distinta a la formación académica reglada y a la sanitaria)
    /// </summary>
    public class FormacionEspecializada
    {
        /// <summary>
        /// Identificador de la formación.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del título
        /// </summary>
        public string NombreTitulo { get; set; }
        /// <summary>
        /// Fecha de obtención del título
        /// </summary>
        public DateTime? FechaTitulacion { get; set; }
        /// <summary>
        /// Horas de formación
        /// </summary>
        public float? DuracionTitulacion { get; set; }
        /// <summary>
        /// Entidad que concede el título
        /// </summary>
        public Entidad EntidadTitulacion { get; set; }
        /// <summary>
        /// Ciudad en la que se ha obtenido el título
        /// </summary>
        public string CiudadEntidadTitulacion { get; set; }
        /// <summary>
        /// País en el que se ha obtenido el título
        /// </summary>
        public Pais PaisEntidadTitulacion { get; set; }
        /// <summary>
        /// Región/comunidad autónoma en la que se ha obtenido el título
        /// </summary>
        public ComunidadAutonoma CcaaRegionEntidadTitulacion { get; set; }
        /// <summary>
        /// Especialidad, Extensión universitaria, Master, Posgrado
        /// </summary>
        public TipoFormacion TipoFormacion { get; set; }
        /// <summary>
        /// Objetivos de la formación, podría ser una descripción que acompañe al curso
        /// </summary>
        public string Objetivos { get; set; }
        /// <summary>
        /// Nombre y apellidos de la persona responsable, no se identifica a la persona con un investigador existente
        /// </summary>
        public PersonaSecundaria ResponsableFormacion { get; set; }
    }
}
