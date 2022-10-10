using System;

namespace OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual
{
    /// <summary>
    /// SolicitudProteccion
    /// </summary>
    public class SolicitudProteccion : SGI_Base
    {
        /// <summary>
        /// Identificador único de la solicitud de protección.
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// Invención a la que pertenece la solicitud de protección.
        /// </summary>
        public Invencion invencion { get; set; }
        /// <summary>
        /// Título de la solicitud de protección.
        /// </summary>
        public string titulo { get; set; }
        /// <summary>
        /// Fecha de la solicitud o fecha de prioridad.
        /// </summary>
        public DateTime? fechaPrioridadSolicitud { get; set; }
        /// <summary>
        /// Fecha de finalización de la prioridad de la solicitud o de fin del plazo de presentación de solicitudes en fases nacionales/regionales.
        /// </summary>
        public DateTime? fechaFinPriorPresFasNacRec { get; set; }
        /// <summary>
        /// Vía de protección.
        /// </summary>
        public ViaProteccion viaProteccion { get; set; }
        /// <summary>
        /// Este campo estará informado en las vías de protección con países asociados (País específico p.ej.).
        /// </summary>
        public string paisProteccionRef { get; set; }
        /// <summary>
        /// Número de la solicitud que es comunicada por el organismo donde se solicita.
        /// </summary>
        public string numeroSolicitud { get; set; }
        /// <summary>
        /// Número del registro que es comunicada por el organismo que concede la protección.
        /// </summary>
        public string numeroRegistro { get; set; }
        /// <summary>
        /// Estado de la solicitud.
        /// </summary>
        public string estado { get; set; }
        /// <summary>
        /// Fecha de publicación de la solicitud de invención.
        /// </summary>
        public DateTime? fechaPublicacion { get; set; }
        /// <summary>
        /// Número de la publicación que es comunicada por el organismo donde se publica.
        /// </summary>
        public string numeroPublicacion { get; set; }
        /// <summary>
        /// Porcentaje de participación del titular en la invención.
        /// </summary>
        public DateTime? fechaConcesion { get; set; }
        /// <summary>
        /// Número de la concesión que es comunicada por el organismo que concede la protección.
        /// </summary>
        public string numeroConcesion { get; set; }
        /// <summary>
        /// fechaCaducidad
        /// </summary>
        public DateTime? fechaCaducidad { get; set; }
        /// <summary>
        /// Identificador en los sistemas de la Universidad de la entidad/empresa que actúa como agente de la propiedad asociado a la solicitud de protección.
        /// </summary>
        public string agentePropiedadRef { get; set; }
        /// <summary>
        /// Tipo de caducidad de la solicitud de invención.
        /// </summary>
        public string tipoCaducidad { get; set; }
        /// <summary>
        /// Comentarios a la solicitud de protección.
        /// </summary>
        public string comentarios { get; set; }
        /// <summary>
        /// Indicador de si la solicitud de protección está activa o no en el SGI.
        /// </summary>
        public bool? activo { get; set; }
    }
}
