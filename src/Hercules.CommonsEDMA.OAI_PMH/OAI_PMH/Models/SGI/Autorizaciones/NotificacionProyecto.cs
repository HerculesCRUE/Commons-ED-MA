using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.Autorizacion
{
    /// <summary>
    /// Contiene los datos del proyecto/contrato creado por un investigador es su CVN
    /// </summary>
    public class NotificacionProyecto
    {
        /// <summary>
        /// Identificador único o referencia del proyecto/contrato registrado en el CVN.
        /// </summary>
        public string proyectoCVNId { get; set; }
        /// <summary>
        /// Referencia de la persona dentro del sistema de gestión de personas corporativo a la que pertenece el ítem proyecto/contrato registrado en CVN.
        /// </summary>
        public string solicitanteRef { get; set; }
        /// <summary>
        /// Identificador de la solicitud de autorización previamente remitido por el SGI
        /// </summary>
        public long? autorizacionId { get; set; }
        /// <summary>
        /// Campo título del ítem proyecto/contrato registrado en CVN.
        /// </summary>
        public string titulo { get; set; }
        /// <summary>
        /// Referencia que se le ha dado el proyecto en la entidad convocante/financiadora recogida en CVN.
        /// </summary>
        public string codExterno { get; set; }
        /// <summary>
        /// Fecha de inicio del proyecto/contrato recogido en CVN, en formato UTC.
        /// </summary>
        public string fechaInicio { get; set; }
        /// <summary>
        /// Fecha de fin del proyecto/contrato recogido en CVN, en formato UTC.
        /// </summary>
        public string fechaFin { get; set; }
        /// <summary>
        /// Ámbito geográfico del proyecto/contrato recogido en CVN.
        /// </summary>
        public string ambitoGeografico { get; set; }
        /// <summary>
        /// Grado de contribución del proyecto/contrato recogido en CVN.
        /// </summary>
        public string gradoContribucion { get; set; }
        /// <summary>
        /// Referencia de la persona dentro del Sistema de gestión de personas corporativo asociado al dato recogido sobre el campo IP de CVN.
        /// </summary>
        public string responsableRef { get; set; }
        /// <summary>
        /// Nombre de la persona que ocupa el cargo de IP del proyecto, recogida en CVN. 
        /// </summary>
        public string datosResponsable { get; set; }
        /// <summary>
        /// Referencia de la entidad dentro del Sistema de gestión de empresas corporativo asociado al dato recogido sobre el campo Entidad donde se desarrolla del CVN.
        /// </summary>
        public string entidadParticipacionRef { get; set; }
        /// <summary>
        /// Nombre de la entidad recogido sobre el campo Entidad donde se desarrolla del CVN.
        /// </summary>
        public string datosEntidadParticipacion { get; set; }
        /// <summary>
        /// Nombre del programa de financiación recogido en el CVN.
        /// </summary>
        public string nombrePrograma { get; set; }
        /// <summary>
        /// Importe total del proyecto/programa recogido en el CVN.
        /// </summary>
        public float? importeTotal { get; set; }
        /// <summary>
        /// Porcentaje subvencionado recogido en el CVN.
        /// </summary>
        public float? porcentajeSubvencion { get; set; }
        /// <summary>
        /// Identificador del documento acreditativo de la concesión del proyecto
        /// </summary>
        public string documentoRef { get; set; }
        /// <summary>
        /// URL acreditativa en repositorios ajenos a la Universidad
        /// </summary>
        public string urlAcreditativa { get; set; }
        /// <summary>
        /// Listado de entidades financiadoras del CVN.
        /// </summary>
        public List<EntidadFinanciadora> entidadesFinanciadoras { get; set; }
    }
}
