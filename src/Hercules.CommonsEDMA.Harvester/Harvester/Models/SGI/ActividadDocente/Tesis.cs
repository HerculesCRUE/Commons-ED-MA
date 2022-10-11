using OAI_PMH.Models.SGI.OrganicStructure;
using System;
using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.ActividadDocente
{
    /// <summary>
    /// Dirección de tesis doctorales y/o proyectos fin de carrera
    /// </summary>
    public class Tesis
    {
        /// <summary>
        /// Identificador del doctorado
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Título del trabajo
        /// </summary>
        public string TituloTrabajo { get; set; }
        /// <summary>
        /// Fecha de obtención del título
        /// </summary>
        public DateTime? FechaDefensa { get; set; }
        /// <summary>
        /// Nombre del alumno
        /// </summary>
        public string Alumno { get; set; }
        /// <summary>
        /// Entidad que concede el título
        /// </summary>
        public Entidad EntidadRealizacion { get; set; }
        /// <summary>
        /// Ciudad en la que se ha obtenido el título
        /// </summary>
        public string CiudadEntidadRealizacion { get; set; }
        /// <summary>
        /// País en el que se ha obtenido el título
        /// </summary>
        public Pais PaisEntidadRealizacion { get; set; }
        /// <summary>
        /// Región/comunidad autónoma en la que se ha obtenido el título
        /// </summary>
        public ComunidadAutonoma CcaaRegionEntidadRealizacion { get; set; }
        /// <summary>
        /// Tipo del trabajo dirigido
        /// </summary>
        public TipoTrabajoDirigido TipoProyecto { get; set; }
        /// <summary>
        /// Valor de la calificación obtenida
        /// </summary>
        public string CalificacionObtenida { get; set; }
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
        /// Fecha de la homologación del doctorado extranjero
        /// </summary>
        public DateTime? FechaMencionCalidad { get; set; }
        /// <summary>
        /// Indica si el dotorado ha recibido un mención internacional
        /// </summary>
        public bool MencionInternacional { get; set; }
        /// <summary>
        /// Indica si el dotorado ha recibido un mención industrial
        /// </summary>
        public bool MencionIndustrial { get; set; }
        /// <summary>
        /// Compendio
        /// </summary>
        public bool Compendio { get; set; }

    }
}
