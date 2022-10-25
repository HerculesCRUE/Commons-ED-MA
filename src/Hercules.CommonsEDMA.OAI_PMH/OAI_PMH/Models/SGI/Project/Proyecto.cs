using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Proyecto
    /// </summary>
    public class Proyecto : SGI_Base
    {
        /// <summary>
        /// Identificador del proyecto.
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// Nombre del usuario que ha creado la entidad.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Fecha de la creación de la entidad.
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// Nombre del usuario que ha modificado por última vez la entidad.
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// Fecha de la última modificación de la entidad.
        /// </summary>
        public string LastModifiedDate { get; set; }
        /// <summary>
        /// Identificador de la convocatoria del proyecto en caso de que la convocatoria este registrada en el SGI.
        /// </summary>
        public string ConvocatoriaId { get; set; }
        /// <summary>
        /// Identificador de la solicitud de convocatoria que dió lugar al proyecto.
        /// </summary>
        public string SolicitudId { get; set; }
        /// <summary>
        /// Estado del proyecto.
        /// </summary>
        public EstadoProyecto Estado { get; set; }
        /// <summary>
        /// Título del proyecto.
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// Identificador corto del proyecto.
        /// </summary>
        public string Acronimo { get; set; }
        /// <summary>
        /// Código o referencia con el que se identifica el proyecto en la entidad convocante externa
        /// </summary>
        public string CodigoExterno { get; set; }
        /// <summary>
        /// Fecha de inicio del proyecto.
        /// </summary>
        public string FechaInicio { get; set; }
        /// <summary>
        /// Fecha de fin del proyecto.
        /// </summary>
        public string FechaFin { get; set; }
        /// <summary>
        /// Fecha de fin definitiva del proyecto.
        /// </summary>
        public string FechaFinDefinitiva { get; set; }
        /// <summary>
        /// Identificador de la Unidad de gestión
        /// </summary>
        public string UnidadGestionRef { get; set; }
        /// <summary>
        /// Entidad que representa un modelo de ejecución.
        /// </summary>
        public ModeloEjecucion ModeloEjecucion { get; set; }
        /// <summary>
        /// Entidad que representa la finalidad del proyecto.
        /// </summary>
        public TipoFinalidad Finalidad { get; set; }
        /// <summary>
        /// Permite mostrar o recoger la identificación externa de la convocatoria, dependiendo si el proyecto se asocia o no a una convocatoria registrada en el SGI.
        /// </summary>
        public string ConvocatoriaExterna { get; set; }
        /// <summary>
        /// Entidad que representa el ámbito geográfico.
        /// </summary>
        public TipoAmbitoGeografico AmbitoGeografico { get; set; }
        /// <summary>
        /// Indica si el proyecto es confidencial.
        /// </summary>
        public bool? Confidencial { get; set; }
        /// <summary>
        /// Indica el apartado del CVN al que correspondería el proyecto. 
        /// </summary>
        public string ClasificacionCVN { get; set; }
        /// <summary>
        /// Indica si el proyecto se realizará de forma coordinada con otros socios.
        /// </summary>
        public bool? Coordinado { get; set; }
        /// <summary>
        /// Indica si un proyecto coordinado es además colaborativo.
        /// </summary>
        public bool? Colaborativo { get; set; }
        /// <summary>
        /// Indica quién actúa como coordinador del proyecto.
        /// Un valor "false" indica que es la propia universidad quien actúa en calidad de coordinador del proyecto. 
        /// </summary>
        public bool? CoordinadorExterno { get; set; }
        /// <summary>
        /// Indica si el proyecto requiere gestión de Timesheet.
        /// </summary>
        public bool? Timesheet { get; set; }
        /// <summary>
        /// Indica si el proyecto requiere gestión de paquetes de trabajo en los Timesheet.
        /// </summary>
        public bool? PermitePaquetesTrabajo { get; set; }
        /// <summary>
        /// Indica si el proyecto requerirá realizar el cálculo de coste de hora de personal.
        /// </summary>
        public bool? CosteHora { get; set; }
        /// <summary>
        /// Indica el criterio de las horas anuales para el cálculo del coste/hora.
        /// </summary>
        public string TipoHorasAnuales { get; set; }
        /// <summary>
        /// IVA del proyecto.
        /// </summary>
        public ProyectoIVA Iva { get; set; }
        /// <summary>
        /// Indica la causa de exención de IVA.
        /// </summary>
        public string CausaExencion { get; set; }
        /// <summary>
        /// Observaciones de carácter interno del proyecto.
        /// </summary>
        public string Observaciones { get; set; }
        /// <summary>
        /// Indica si en el presupuesto se va a introducir por anualidades o no.
        /// </summary>
        public bool? Anualidades { get; set; }
        /// <summary>
        /// Importe presupuesto correspondiente al proyecto a desarrollar por la Universidad
        /// </summary>
        public double? ImportePresupuesto { get; set; }
        /// <summary>
        /// Importe concedido correspondiente al proyecto a desarrollar por la Universidad
        /// </summary>
        public double? ImporteConcedido { get; set; }
        /// <summary>
        /// Importe total presupuestado por todos los socios (adicionales a la Universidad) que participan en el proyecto
        /// </summary>
        public double? ImportePresupuestoSocios { get; set; }
        /// <summary>
        /// Importe total concedido por todos los socios (adicionales a la Universidad) que participan en el proyecto
        /// </summary>
        public double? ImporteConcedidoSocios { get; set; }
        /// <summary>
        /// Importe total presupuestado del proyecto (Universidad y socios)
        /// </summary>
        public double? TotalImportePresupuesto { get; set; }
        /// <summary>
        /// Importe total concedido del proyecto (Universidad y socios)
        /// </summary>
        public double? TotalImporteConcedido { get; set; }
        /// <summary>
        /// Indica si esta activa o no. 
        /// </summary>
        public bool? Activo { get; set; }
        /// <summary>
        /// Contexto
        /// </summary>
        public ContextoProyecto Contexto { get; set; }
        /// <summary>
        /// Equipo
        /// </summary>
        public List<ProyectoEquipo> Equipo { get; set; }
        /// <summary>
        /// Entidades gestoras
        /// </summary>
        public List<ProyectoEntidadGestora> EntidadesGestoras { get; set; }
        /// <summary>
        /// Entidades convocantes
        /// </summary>
        public List<ProyectoEntidadConvocante> EntidadesConvocantes { get; set; }
        /// <summary>
        /// Entidades financiadoras
        /// </summary>
        public List<ProyectoEntidadFinanciadora> EntidadesFinanciadoras { get; set; }
        /// <summary>
        /// Resumen anualidades
        /// </summary>
        public List<ProyectoAnualidadResumen> ResumenAnualidades { get; set; }
        /// <summary>
        /// Presupuestos totales
        /// </summary>
        public ProyectoPresupuestoTotales PresupuestosTotales { get; set; }
        /// <summary>
        /// Presupuestos clasificación
        /// </summary>
        public List<ProyectoClasificacion> ProyectoClasificacion { get; set; }
        /// <summary>
        /// Notificación de proyecto externo CVN
        /// </summary>
        public List<NotificacionProyectoExternoCVN> NotificacionProyectoExternoCVN { get; set; }
        /// <summary>
        /// Areas de conocimiento
        /// </summary>
        public List<ProyectoAreaConocimiento> AreasConocimiento { get; set; }
        /// <summary>
        /// Palabras clave
        /// </summary>
        public List<PalabraClave> PalabrasClaves { get; set; }
        /// <summary>
        /// Histórico de estados.
        /// </summary>
        public List<EstadoProyecto> historicoProyectos { get; set; }
    }
}
