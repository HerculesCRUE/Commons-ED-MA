using OAI_PMH.Models.SGI.OrganicStructure;

namespace OAI_PMH.Models.SGI.ActividadDocente
{
    /// <summary>
    /// Formación académica impartida
    /// </summary>
    public class FormacionAcademicaImpartida : SGI_Base
    {
        /// <summary>
        /// Identificador
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Titulación universitaria
        /// </summary>
        public string TitulacionUniversitaria { get; set; }
        /// <summary>
        /// Nombre de la asignatura del curso
        /// </summary>
        public string NombreAsignaturaCurso { get; set; }
        /// <summary>
        /// Fecha de inicio
        /// </summary>
        public string FechaInicio { get; set; }
        /// <summary>
        /// Fecha de finalización
        /// </summary>
        public string FechaFinalizacion { get; set; }
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
        /// 	Tipo de la labor de docente
        /// </summary>
        public TipoDocente TipoDocente { get; set; }
        /// <summary>
        /// Numero de horas/creditos
        /// </summary>
        public double? NumHorasCreditos { get; set; }
        /// <summary>
        /// Si la formación se imparte periódicamente, indique el número de veces que ha impartido esta materia.
        /// </summary>
        public double? FrecuenciaActividad { get; set; }
        /// <summary>
        /// Tipo de programa
        /// </summary>
        public TipoPrograma TipoPrograma { get; set; }
        /// <summary>
        /// Tipo de docencia
        /// </summary>
        public TipoDocencia TipoDocencia { get; set; }
        /// <summary>
        /// Departamento
        /// </summary>
        public string Departamento { get; set; }
        /// <summary>
        /// Tipo de asignatura
        /// </summary>
        public TipoAsignatura TipoAsignatura { get; set; }
        /// <summary>
        /// Curso
        /// </summary>
        public string Curso { get; set; }
        /// <summary>
        /// Tipo de horas o créditos: Horas, Créditos
        /// </summary>
        public TipoHorasCreditos TipoHorasCreditos { get; set; }
        /// <summary>
        /// Idioma
        /// </summary>
        public string Idioma { get; set; }
        /// <summary>
        /// Competencias
        /// </summary>
        public string Competencias { get; set; }
        /// <summary>
        /// Categoria profesional
        /// </summary>
        public string CategoriaProfesional { get; set; }
    }
}
