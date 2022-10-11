using System;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Dirección de tesis
    /// </summary>
    public class DireccionTesis : SGI_Base
    {
        /// <summary>
        /// Identificador del doctorado
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Identificador de la persona que es el Director de la tesis
        /// </summary>
        public string PersonaRef { get; set; }
        /// <summary>
        /// Título del trabajo
        /// </summary>
        public string TituloTrabajo { get; set; }
        /// <summary>
        /// Fecha de obtención del título
        /// </summary>
        public DateTime? FechaDefensa { get; set; }
        /// <summary>
        /// Identificador en el SGP si existe el alumno y sino nombre y apellidos del alumno.
        /// </summary>
        public string Alumno { get; set; }
        /// <summary>
        /// Tipo del trabajo dirigido
        /// </summary>
        public TipoTrabajoDirigido TipoProyecto { get; set; }
        /// <summary>
        /// Calificación obtenida
        /// </summary>
        public string CalificacionObtenida { get; set; }
        /// <summary>
        /// Identificador del co-director/a que ha intervenido
        /// </summary>
        public string CoDirectorTesisRef { get; set; }
        /// <summary>
        /// Indica si se ha recibido una mención europea
        /// </summary>
        public bool? DoctoradoEuropeo { get; set; }
        /// <summary>
        /// Fecha de la mención del doctorado europeo.
        /// </summary>
        public DateTime? FechaMencionDoctoradoEuropeo { get; set; }
        /// <summary>
        /// Indica si el dotorado ha recibido una mención de calidad
        /// </summary>
        public bool? MencionCalidad { get; set; }
        /// <summary>
        /// Fecha de la homologación del doctorado extranjero.
        /// </summary>
        public DateTime? FechaMencionCalidad { get; set; }
        /// <summary>
        /// Indica si tiene mención internacional o no
        /// </summary>
        public bool? MencionInternacional { get; set; }
        /// <summary>
        /// Indica si tiene mención industrial o no
        /// </summary>
        public bool? MencionIndustrial { get; set; }
    }
}
