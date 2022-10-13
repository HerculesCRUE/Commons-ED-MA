using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Contiene información del contexto del proyecto.
    /// </summary>
    public class ContextoProyecto
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
        /// Texto libre para recoger cuales serán los objetivos del proyecto.
        /// </summary>
        public string Objetivos { get; set; }
        /// <summary>
        /// Texto libre para recoger los motivos que justifican el desarrollo del proyecto y/o los intereses del mismo.
        /// </summary>
        public string Intereses { get; set; }
        /// <summary>
        /// Texto libre para recoger los resultados esperados del proyecto.
        /// </summary>
        public string ResultadosPrevistos { get; set; }
        /// <summary>
        /// Permitirá recoger quién es el propietario de los resultados que se generen a raíz de la ejecución del proyecto.
        /// </summary>
        public string PropiedadResultados { get; set; }
        /// <summary>
        /// Entidad que representa el área temática elegida en la convocatoria en caso de que el proyecto este relacionado con una convocatoria del SGI.
        /// </summary>
        public AreaTematica AreaTematicaConvocatoria { get; set; }
        /// <summary>
        /// Entidad que representa el área temática en la que se enmarca el proyecto.
        /// </summary>
        public AreaTematica AreaTematica { get; set; }
    }
}
