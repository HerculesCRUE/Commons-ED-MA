using OAI_PMH.Models.SGI.OrganicStructure;
using System;

namespace OAI_PMH.Models.SGI.FormacionAcademica
{
    /// <summary>
    /// Otra formación universitaria de posgrado
    /// </summary>
    public class Posgrado
    {
        /// <summary>
        /// Identificador de la formación.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del título de posgrado
        /// </summary>
        public string NombreTituloPosgrado { get; set; }
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
        /// Especialidad, Extensión universitaria, Master, Posgrado
        /// </summary>
        public TipoFormacionHomologada TipoFormacionHomologada { get; set; }
        /// <summary>
        /// Valor de la calificación obtenida
        /// </summary>
        public string CalificacionObtenida { get; set; }
        /// <summary>
        /// Fecha de la homologación
        /// </summary>
        public DateTime? FechaHomologacion { get; set; }
        /// <summary>
        /// Indica si el título está homologado
        /// </summary>
        public bool? TituloHomologado { get; set; }
    }
}
