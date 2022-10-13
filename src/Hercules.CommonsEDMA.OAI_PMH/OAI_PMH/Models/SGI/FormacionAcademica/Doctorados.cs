using OAI_PMH.Models.SGI.OrganicStructure;
using System;

namespace OAI_PMH.Models.SGI.FormacionAcademica
{
    /// <summary>
    /// Doctorados
    /// </summary>
    public class Doctorados : SGI_Base
    {
        /// <summary>
        /// Identificador del doctorado
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Programa de doctorado
        /// </summary>
        public string ProgramaDoctorado { get; set; }
        /// <summary>
        /// Fecha de obtención del título
        /// </summary>
        public DateTime? FechaTitulacion { get; set; }
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
        /// Fecha de obtención del título Diploma de títulos avanzados
        /// </summary>
        public DateTime? FechaTitulacionDEA { get; set; }
        /// <summary>
        /// Entidad que concede el título Diploma de títulos avanzados
        /// </summary>
        public Entidad EntidadTitulacionDEA { get; set; }
        /// <summary>
        /// Título de la tesis
        /// </summary>
        public string TituloTesis { get; set; }
        /// <summary>
        /// Valor de la calificación obtenida
        /// </summary>
        public string CalificacionObtenida { get; set; }
        /// <summary>
        /// Nombre del/ de la director/a que ha intervenido como director/a de la tesis.
        /// </summary>
        public string DirectorTesis { get; set; }
        /// <summary>
        /// Nombre de los/las co-directores/as que han intervenido
        /// </summary>
        public Director CoDirectorTesis { get; set; }
        /// <summary>
        /// Indica si se ha recibido una mención europea
        /// </summary>
        public bool? DoctoradoEuropeo { get; set; }
        /// <summary>
        /// Fecha de la mención del doctorado europeo
        /// </summary>
        public DateTime? FechaMencionDoctoradoEuropeo { get; set; }
        /// <summary>
        /// Indica si el dotorado ha recibido una mención de calidad
        /// </summary>
        public bool? MencionCalidad { get; set; }
        /// <summary>
        /// Indica si se ha recibido un premio extraordinario de doctorado
        /// </summary>
        public bool? PremioExtraordinarioDoctor { get; set; }
        /// <summary>
        /// Fecha del premio extraordinario de doctorado
        /// </summary>
        public DateTime? FechaPremioExtraordinarioDoctor { get; set; }
        /// <summary>
        /// Indica si se trata de un título extranjero homologado
        /// </summary>
        public bool? TituloHomologado { get; set; }
        /// <summary>
        /// Fecha de la homologación del doctorado extranjero
        /// </summary>
        public DateTime? FechaHomologacion { get; set; }
    }
}
