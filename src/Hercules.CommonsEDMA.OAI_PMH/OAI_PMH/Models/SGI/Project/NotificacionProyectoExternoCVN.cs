using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Notificación de un proyecto externo del CVN asociado a un Proyecto del SGI
    /// </summary>
    public class NotificacionProyectoExternoCVN : SGI_Base
    {
        /// <summary>
        /// Identificador de la entidad NotificacionProyectoExternoCVN
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// Identificador del proyecto.
        /// </summary>
        public long? ProyectoId { get; set; }
        /// <summary>
        /// Identificador único o referencia del proyecto/contrato registrado en el CVN
        /// </summary>
        public string ProyectoCVNId { get; set; }
        /// <summary>
        /// Referencia de la persona dentro del sistema de gestión de personas corporativo a la que pertenece el ítem proyecto/contrato registrado en CVN.
        /// </summary>
        public string SolicitanteRef { get; set; }
        /// <summary>
        /// Identificador de la solicitud de autorización previamente remitido por el SGI
        /// </summary>
        public long? AutorizacionId { get; set; }
        /// <summary>
        /// Campo título del ítem proyecto/contrato registrado en CVN.
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// Referencia que se le ha dado el proyecto en la entidad convocante/financiadora recogida en CVN.
        /// </summary>
        public string CodExterno { get; set; }
        /// <summary>
        /// Fecha de inicio del proyecto/contrato recogido en CVN.
        /// </summary>
        public string FechaInicio { get; set; }
        /// <summary>
        /// Fecha de fin del proyecto/contrato recogido en CVN.
        /// </summary>
        public string FechaFin { get; set; }
        /// <summary>
        /// Ámbito geográfico del proyecto/contrato recogido en CVN.
        /// </summary>
        public string AmbitoGeografico { get; set; }
        /// <summary>
        /// Referencia de la persona dentro del Sistema de gestión de personas corporativo asociado al dato recogido sobre el campo IP de CVN.
        /// </summary>
        public string ResponsableRef { get; set; }
        /// <summary>
        /// Nombre de la persona que ocupa el cargo de IP del proyecto, recogida en CVN.
        /// </summary>
        public string DatosResponsable { get; set; }
        /// <summary>
        /// Referencia de la entidad dentro del Sistema de gestión de empresas corporativo asociado al dato recogido sobre el campo Entidad donde se desarrolla del CVN.
        /// </summary>
        public string EntidadParticipacionRef { get; set; }
        /// <summary>
        /// Nombre de la entidad recogido sobre el campo Entidad donde se desarrolla del CVN.
        /// </summary>
        public string DatosEntidadParticipacion { get; set; }
        /// <summary>
        /// Nombre del programa de financiación recogido en el CVN.
        /// </summary>
        public string NombrePrograma { get; set; }
        /// <summary>
        /// Importe total del proyecto/programa recogido en el CVN.
        /// </summary>
        public float? ImporteTotal { get; set; }
        /// <summary>
        /// Porcentaje subvencionado recogido en el CVN.
        /// </summary>
        public float? PorcentajeSubvencion { get; set; }
        /// <summary>
        /// Identificador del documento acreditativo de la concesión del proyecto
        /// </summary>
        public string DocumentoRef { get; set; }
        /// <summary>
        /// URL acreditativa en repositorios ajenos a la Universidad
        /// </summary>
        public string UrlAcreditativa { get; set; }
        /// <summary>
        /// Listado de entidades financiadoras del CVN.
        /// </summary>
        public List<EntidadFinanciadora> EntidadesFinanciadoras { get; set; }
    }
}
