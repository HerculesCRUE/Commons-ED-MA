using System;
using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.GruposInvestigacion
{
    /// <summary>
    /// Grupo.
    /// </summary>
    public class Grupo : SGI_Base
    {
        /// <summary>
        /// Identificador.
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// Nombre del grupo.
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Feha de inicio del grupo.
        /// </summary>
        public DateTime? fechaInicio { get; set; }
        /// <summary>
        /// Fecha de fin del grupo.
        /// </summary>
        public DateTime? fechaFin { get; set; }
        /// <summary>
        /// Identificador en el SGE.
        /// </summary>
        public string proyectoSgeRef { get; set; }
        /// <summary>
        /// ID de solicitud.
        /// </summary>
        public int? solicitudId { get; set; }
        /// <summary>
        /// Código del grupo.
        /// </summary>
        public string codigo { get; set; }
        /// <summary>
        /// Tipo del grupo.
        /// </summary>
        public string tipo { get; set; }
        /// <summary>
        /// Especial Investigacion
        /// </summary>
        public bool? especialInvestigacion { get; set; }
        /// <summary>
        /// Indica si el grupo está acitvo o no.
        /// </summary>
        public bool? activo { get; set; }
        /// <summary>
        /// Miembros del equipo del grupo.
        /// </summary>
        public List<GrupoEquipo> equipo { get; set; }
        /// <summary>
        /// Palabras clave del grupo.
        /// </summary>
        public List<GrupoPalabraClave> palabrasClave { get; set; }
        /// <summary>
        /// Lineas de clasificacion del grupo.
        /// </summary>
        public List<LineaClasificacion> lineasClasificacion { get; set; }
        /// <summary>
        /// Lineas de investigación del grupo.
        /// </summary>
        public List<LineaInvestigacion> lineasInvestigacion { get; set; }
        /// <summary>
        /// Investigadores principales del grupo.
        /// </summary>
        public List<string> investigadoresPrincipales { get; set; }
        /// <summary>
        /// Investigadores que más han participado en el grupo.
        /// </summary>
        public List<string> investigadoresPrincipalesMaxParticipacion { get; set; }
    }
}
