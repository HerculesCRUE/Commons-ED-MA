namespace OAI_PMH.Models.SGI.Autorizacion
{
    /// <summary>
    /// Contiene los datos de la solicitud de autorización para participar en un proyecto externo
    /// </summary>
    public class Autorizacion
    {
        /// <summary>
        /// Identificador de la solicitud de autorización
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// Investigador a quien se le ha concedido la autorización. 
        /// </summary>
        public string solicitanteRef { get; set; }
        /// <summary>
        /// Título del proyecto.
        /// </summary>
        public string tituloProyecto { get; set; }
        /// <summary>
        /// Referencia a la entidad en la que se desarrollará el proyecto, recogida en la solicitud de autorización.
        /// </summary>
        public string entidadRef { get; set; }
        /// <summary>
        /// Referencia del Investigador Principal del proyecto externo
        /// </summary>
        public string responsableRef { get; set; }
        /// <summary>
        /// Nombre y apellidos de la persona que se ha indicado que actuará como IP del proyecto externo. 
        /// </summary>
        public string datosResponsable { get; set; }
        /// <summary>
        /// Horas de dedicación al proyecto por parte del solicitante.
        /// </summary>
        public long? horasDedicacion { get; set; }
        /// <summary>
        /// Nombre de la entidad en la que se desarrollará el proyecto.
        /// </summary>
        public string datosEntidad { get; set; }
        /// <summary>
        /// Identificador de la convocatoria del proyecto en caso de que la convocatoria exista dentro del SGI.
        /// </summary>
        public long? convocatoriaId { get; set; }
        /// <summary>
        /// Nombre de la convocatoria del proyecto en caso de que la convocatoria no existan dentro del SGI.
        /// </summary>
        public string datosConvocatoria { get; set; }
        /// <summary>
        /// Observaciones aportadas a la autorización.
        /// </summary>
        public string observaciones { get; set; }
    }
}
