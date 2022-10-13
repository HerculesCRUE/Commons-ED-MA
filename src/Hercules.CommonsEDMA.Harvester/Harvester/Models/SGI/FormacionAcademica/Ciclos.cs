using OAI_PMH.Models.SGI.OrganicStructure;
using System;

namespace OAI_PMH.Models.SGI.FormacionAcademica
{
    /// <summary>
    /// Estudios de 1º y 2º ciclo, y antiguos ciclos
    /// </summary>
    public class Ciclos
    {
        /// <summary>
        /// Identificador de la formación.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre del título de la formación
        /// </summary>
        public string NombreTitulo { get; set; }
        /// <summary>
        /// Fecha de obtención del título
        /// </summary>
        public DateTime? FechaTitulacion { get; set; }
        /// <summary>
        /// Doctor, titulado medio, titulado superior, otros
        /// </summary>
        public TipoTitulacion TitulacionUniversitaria { get; set; }
        /// <summary>
        /// Entidad que concede el título
        /// </summary>
        public Entidad EntidadTitulacion { get; set; }
        /// <summary>
        /// Ciudad en la que se ha obtenido el título
        /// </summary>
        public string CiudadEntidadTitulacion { get; set; }
        /// <summary>
        /// Pais en el que se ha obtenido el título
        /// </summary>
        public Pais PaisEntidadTitulacion { get; set; }
        /// <summary>
        /// Región/comunidad autónoma en la que se ha obtenido el título
        /// </summary>
        public ComunidadAutonoma CcaaRegionEntidadTitulacion { get; set; }
        /// <summary>
        /// Nombre oficial del título expedido por una institución de educación superior extranjera.
        /// </summary>
        public string TituloExtranjero { get; set; }
        /// <summary>
        /// Homologación del titulo
        /// </summary>
        public bool? TituloHomologado { get;set; }
        /// <summary>
        ///  Fecha de la homologación
        /// </summary>
        public DateTime? FechaHomologacion { get; set; }
        /// <summary>
        /// Nota media del expediente
        /// </summary>
        public double? NotaMediaExpediente { get; set; }
        /// <summary>
        /// Premio extraordinario de licenciatura, Premio fin de carrera, Otros
        /// </summary>
        public Premio Premio { get; set; }
    }
}
