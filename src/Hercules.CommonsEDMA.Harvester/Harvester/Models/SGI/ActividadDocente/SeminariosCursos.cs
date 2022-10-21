using OAI_PMH.Models.SGI.OrganicStructure;
using System;

namespace OAI_PMH.Models.SGI.ActividadDocente
{
    /// <summary>
    /// Cursos y seminarios impartidos orientados a la formación docente universitaria
    /// </summary>
    public class SeminariosCursos
    {
        /// <summary>
        /// Identificador de la formación
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del evento
        /// </summary>
        public string NombreEvento { get; set; }
        /// <summary>
        /// Congreso, Jornada, Seminario, Otros
        /// </summary>
        public TipoEvento TipoEvento { get; set; }
        /// <summary>
        /// Fecha de obtención del título
        /// </summary>
        public DateTime? FechaTitulacion { get; set; }
        /// <summary>
        /// Entidad que concede el título
        /// </summary>
        public Entidad EntidadOrganizacionEvento { get; set; }
        /// <summary>
        /// Tipo de entidad organizadora del evento
        /// </summary>
        public TipoEntidadOrganizadora TipoEntidad { get; set; }
        /// <summary>
        /// Ciudad en la que se ha obtenido el título
        /// </summary>
        public string CiudadEntidadOrganizacionEvento { get; set; }
        /// <summary>
        /// País en el que se ha obtenido el título
        /// </summary>
        public Pais PaisEntidadOrganizacionEvento { get; set; }
        /// <summary>
        /// Región/comunidad autónoma en la que se ha obtenido el título
        /// </summary>
        public ComunidadAutonoma CcaaRegionEntidadOrganizacionEvento { get; set; }
        /// <summary>
        /// Descripción del curso
        /// </summary>
        public string ObjetivosCurso { get; set; }
        /// <summary>
        /// Idioma del curso
        /// </summary>
        public string Idioma { get; set; }
        /// <summary>
        /// Código ISBN
        /// </summary>
        public string ISBN { get; set; }
        /// <summary>
        /// Código ISSN
        /// </summary>
        public string ISSN { get; set; }
        /// <summary>
        /// Indica si se es autor de correspondencia
        /// </summary>
        public bool? AutorCorrespondencia { get; set; }
        /// <summary>
        /// DOI, Handle, PMID, Otros
        /// </summary>
        public TipoIdentificador IdentificadoresPublicacion { get; set; }
        /// <summary>
        /// Descripción del perfil de destinatarios
        /// </summary>
        public string PerfilDestinatarios { get; set; }
        /// <summary>
        /// Ver valores en la definición de la estructura, más abajo
        /// </summary>
        public TipoParticipacion TipoParticipacion { get; set; }
    }
}
