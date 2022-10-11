namespace OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual
{
    /// <summary>
    /// ViaProteccion
    /// </summary>
    public class ViaProteccion 
    {
        /// <summary>
        /// Identificador.
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// Nombre
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Descripción
        /// </summary>
        public string descripcion { get; set; }
        /// <summary>
        /// TipoPropiedad
        /// </summary>
        public int? tipoPropiedad { get; set; }
        /// <summary>
        /// MesesPrioridad
        /// </summary>
        public int? mesesPrioridad { get; set; }
        /// <summary>
        /// PaisEspecifico
        /// </summary>
        public bool? paisEspecifico { get; set; }
        /// <summary>
        /// ExtensionInternacional
        /// </summary>
        public bool? extensionInternacional { get; set; }
        /// <summary>
        /// VariosPaises
        /// </summary>
        public bool? variosPaises { get; set; }
        /// <summary>
        /// Activo
        /// </summary>
        public bool? activo { get; set; }
    }
}
