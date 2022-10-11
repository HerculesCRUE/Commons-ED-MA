namespace OAI_PMH.Models.SGI.GruposInvestigacion
{
    /// <summary>
    /// Clase principal de Grupo Equipo.
    /// </summary>
    public class GrupoEquipo
    {
        /// <summary>
        /// Id del equipo.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Id del grupo.
        /// </summary>
        public int grupoId { get; set; }
        /// <summary>
        /// PersonaRef.
        /// </summary>
        public string personaRef { get; set; }
        /// <summary>
        /// Rol del equipo.
        /// </summary>
        public RolProyecto rol { get; set; }
        /// <summary>
        /// Fecha de inicio del equipo.
        /// </summary>
        public string fechaInicio { get; set; }
        /// <summary>
        /// Fechade fin del equipo.
        /// </summary>
        public string fechaFin { get; set; }
        /// <summary>
        /// Dedicación del grupo.
        /// </summary>
        public string dedicacion { get; set; }
        /// <summary>
        /// Participacion del grupo.
        /// </summary>
        public float participacion { get; set; }
    }
}
