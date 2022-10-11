namespace OAI_PMH.Models.SGI.GruposInvestigacion
{
    public class RolProyecto
    {
        /// <summary>
        /// Id del rol.
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// Abreviatura del rol del proyecto.
        /// </summary>
        public string abreviatura { get; set; }
        /// <summary>
        /// Nombre del rol del proyecto.
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Descripción del rol del proyecto.
        /// </summary>
        public string descripcion { get; set; }
        /// <summary>
        /// Rol principal.
        /// </summary>
        public bool? rolPrincipal { get; set; }
        /// <summary>
        /// Orden.
        /// </summary>
        public string orden { get; set; }
        /// <summary>
        /// Equipo.
        /// </summary>
        public string equipo { get; set; }
        /// <summary>
        /// Rol activo o no.
        /// </summary>
        public bool? activo { get; set; }
    }
}
