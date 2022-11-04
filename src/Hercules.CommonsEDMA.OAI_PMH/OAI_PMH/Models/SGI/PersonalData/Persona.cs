using OAI_PMH.Models.SGI.ActividadDocente;
using OAI_PMH.Models.SGI.FormacionAcademica;
using System.Collections.Generic;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Persona
    /// </summary>
    public class Persona : SGI_Base
    {
        /// <summary>
        /// Identificador de la persona.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre de la persona.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Apellidos de la persona.
        /// </summary>
        public string Apellidos { get; set; }
        /// <summary>
        /// Se devuelve la entidad Sexo con todos sus campos.
        /// </summary>
        public Sexo Sexo { get; set; }
        /// <summary>
        /// Número de documento de identificación personal.
        /// </summary>
        public string NumeroDocumento { get; set; }
        /// <summary>
        /// Se devuelve la entidad TipoDocumento con todos sus campos.
        /// </summary>
        public TipoDocumento TipoDocumento { get; set; }
        /// <summary>
        /// Se devuelve el identificador/referencia de la entidad Empresa.
        /// </summary>
        public string EmpresaRef { get; set; }
        /// <summary>
        /// Indica si es personal de la Universidad o no (a día de hoy).
        /// </summary>
        public bool? PersonalPropio { get; set; }
        /// <summary>
        /// Se devuelve el identificador/referencia de la entidad que representa a la UM
        /// </summary>
        public string EntidadPropiaRef { get; set; }
        /// <summary>
        /// Lista con los emails de la persona 
        /// </summary>
        public List<Email> Emails { get; set; }
        /// <summary>
        /// Indica si la persona esta activa o no
        /// </summary>
        public bool? Activo { get; set; }
        /// <summary>
        /// Datos personales
        /// </summary>
        public DatosPersonales DatosPersonales { get; set; }
        /// <summary>
        /// Datos de contacto
        /// </summary>
        public DatosContacto DatosContacto { get; set; }
        /// <summary>
        /// Vinculación
        /// </summary>
        public Vinculacion Vinculacion { get; set; }
        /// <summary>
        /// Datos academicos
        /// </summary>
        public DatosAcademicos DatosAcademicos { get; set; }
        /// <summary>
        /// Colectivo
        /// </summary>
        public Colectivo Colectivo { get; set; }
        /// <summary>
        /// Fotografía
        /// </summary>
        public Fotografia Fotografia { get; set; }
        /// <summary>
        /// Sexenios
        /// </summary>
        public Sexenio Sexenios { get; set; }
        /// <summary>
        /// Posgrado
        /// </summary>
        public List<Posgrado> Posgrado { get; set; }
        /// <summary>
        /// Ciclos
        /// </summary>
        public List<Ciclos> Ciclos { get; set; }
        /// <summary>
        /// Doctorados
        /// </summary>
        public List<Doctorados> Doctorados { get; set; }
        /// <summary>
        /// Formación especializada
        /// </summary>
        public List<FormacionEspecializada> FormacionEspecializada { get; set; }
        /// <summary>
        /// Tesis
        /// </summary>
        public List<Tesis> Tesis { get; set; }
        /// <summary>
        /// Seminarios/Cursos
        /// </summary>
        public List<SeminariosCursos> SeminariosCursos { get; set; }
        /// <summary>
        /// Formación academica impartida
        /// </summary>
        public List<FormacionAcademicaImpartida> FormacionAcademicaImpartida { get; set; }
    }
}
