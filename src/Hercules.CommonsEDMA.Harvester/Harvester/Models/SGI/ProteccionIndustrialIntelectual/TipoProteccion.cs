namespace OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual
{
    /// <summary>
    /// TipoProteccion
    /// </summary>
    public class TipoProteccion
    {
        /// <summary>
        /// Id.
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// Nombre.
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Descripcion.
        /// </summary>
        public string descripcion { get; set; }
        /// <summary>
        /// PadreId.
        /// </summary>
        public long? padreId { get; set; }
        /// <summary>
        /// TipoPropiedad.
        /// </summary>
        public string tipoPropiedad { get; set; }   
        /// <summary>
        /// Activo.
        /// </summary>
        public bool? activo { get; set; }
    }
}
