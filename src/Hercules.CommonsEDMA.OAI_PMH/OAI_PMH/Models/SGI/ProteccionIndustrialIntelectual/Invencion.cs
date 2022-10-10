using System;
using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual
{
    /// <summary>
    /// Invencion (PII)
    /// </summary>
    public class Invencion : SGI_Base
    {
        /// <summary>
        /// Id.
        /// </summary>
        public long? id { get; set; }
        /// <summary>
        /// Título.
        /// </summary>
        public string titulo { get; set; }
        /// <summary>
        /// FechaComunicacion.
        /// </summary>
        public DateTime? fechaComunicacion { get; set; }
        /// <summary>
        /// Descripción.
        /// </summary>
        public string descripcion { get; set; }
        /// <summary>
        /// TipoProtecciónId.
        /// </summary>
        public TipoProteccion tipoProteccion { get; set; }
        /// <summary>
        /// ProyectoRef.
        /// </summary>
        public string proyectoRef { get; set; }
        /// <summary>
        /// Comentarios.
        /// </summary>
        public string comentarios { get; set; }
        /// <summary>
        /// Activo.
        /// </summary>
        public bool? activo { get; set; }
        /// <summary>
        /// SectoresAplicacion.
        /// </summary>
        public List<SectorAplicacion> sectoresAplicacion { get; set; }
        /// <summary>
        /// InvencionDocumentos.
        /// </summary>
        public List<InvencionDocumento> invencionDocumentos { get; set; }
        /// <summary>
        /// Gastos.
        /// </summary>
        public List<InvencionGastos> gastos { get; set; }
        /// <summary>
        /// PalabrasClave.
        /// </summary>
        public List<PalabraClave> palabrasClave { get; set; }
        /// <summary>
        /// Inventores.
        /// </summary>
        public List<Inventor> inventores { get; set; }
        /// <summary>
        /// AreaConocimiento.
        /// </summary>
        public List<AreaConocimiento> areasConocimiento { get; set; }
        /// <summary>
        /// PeriodoTitularidad
        /// </summary>
        public List<PeriodoTitularidad> periodosTitularidad { get; set; }
        /// <summary>
        /// Titulares.
        /// </summary>
        public List<Titular> titulares { get; set; }
        /// <summary>
        /// Solicitudes de protección.
        /// </summary>
        public List<SolicitudProteccion> solicitudes { get; set; }
    }
}
