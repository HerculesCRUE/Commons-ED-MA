namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Anualidades de un Proyecto
    /// </summary>
    public class ProyectoAnualidadResumen : SGI_Base
    {
        /// <summary>
        /// Identificador de la anualidad del proyecto.
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// Año de la anualidad
        /// </summary>
        public long? Anio { get; set; }
        /// <summary>
        /// Fecha de inicio de la anualidad
        /// </summary>
        public string FechaInicio { get; set; }
        /// <summary>
        /// Fecha de fin de la anualidad
        /// </summary>
        public string FechaFin { get; set; }
        /// <summary>
        /// Total de los gastos presupuestados de la anualidad.
        /// </summary>
        public double TotalGastosPresupuesto { get; set; }
        /// <summary>
        /// Total de los gastos concedidos de la anualidad.
        /// </summary>
        public double TotalGastosConcedido { get; set; }
        /// <summary>
        /// Total de los ingresos de la anualidad.
        /// </summary>
        public double TotalIngresos { get; set; }
        /// <summary>
        /// Indica si es necesario notificar al sistema de gestión económica el presupuesto de la anualidad. 
        /// </summary>
        public bool? Presupuestar { get; set; }
        /// <summary>
        /// Indica si el presupuesto de la anualidad ha sido notificado o no al sistema de gestión económica.
        /// </summary>
        public bool? EnviadoSGE { get; set; }
    }
}
