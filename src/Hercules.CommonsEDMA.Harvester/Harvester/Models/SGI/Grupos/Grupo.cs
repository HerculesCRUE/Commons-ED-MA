using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Harvester;
using Harvester.Models.RabbitMQ;
using OAI_PMH.Models.SGI.PersonalData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace OAI_PMH.Models.SGI.GruposInvestigacion
{
    /// <summary>
    /// Grupo.
    /// </summary>
    public class Grupo : SGI_Base
    {
        /// <summary>
        /// Crea el objeto ComplexOntologyResource para ser cargado.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <param name="pRabbitConf"></param>
        /// <param name="pFusionarPersona"></param>
        /// <param name="pIdPersona"></param>
        /// <returns></returns>
        public override ComplexOntologyResource ToRecurso(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, bool pFusionarPersona = false, string pIdPersona = null)
        {
            GroupOntology.Group grupo = CrearGroupOntology(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas, pRabbitConf);
            pResourceApi.ChangeOntoly("group");
            return grupo.ToGnossApiResource(pResourceApi, null);
        }

        /// <summary>
        /// Obtiene los IDs de BBDD mediante el crisidentifier.
        /// </summary>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public override string ObtenerIDBBDD(ResourceApi pResourceApi)
        {
            Dictionary<string, string> respuesta = ObtenerGruposBBDD(new HashSet<string>() { id.ToString() }, pResourceApi);
            if (respuesta.ContainsKey(id.ToString()) && !string.IsNullOrEmpty(respuesta[id.ToString()]))
            {
                return respuesta[id.ToString()];
            }
            return null;
        }

        public override void ToRecursoAdicional(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, string pIdGnoss)
        {
            // No es necesario para esta clase.
        }

        /// <summary>
        /// Obtiene los IDs de los Grupos de BBDD mediante el crisidentifier.
        /// </summary>
        /// <param name="pListaIds"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ObtenerGruposBBDD(HashSet<string> pListaIds, ResourceApi pResourceApi)
        {
            List<List<string>> listasPersonas = SplitList(pListaIds.ToList(), 1000).ToList();
            Dictionary<string, string> dicPersonasBBDD = new();
            foreach (string persona in pListaIds)
            {
                dicPersonasBBDD[persona] = "";
            }
            foreach (List<string> listaItem in listasPersonas)
            {
                List<string> listaAux = new();
                foreach (string item in listaItem)
                {
                    if (item.Contains("_"))
                    {
                        listaAux.Add(item.Split("_")[1]);
                    }
                    else
                    {
                        listaAux.Add(item);
                    }
                }
                string selectPerson = $@"SELECT DISTINCT ?s ?crisIdentifier ";
                string wherePerson = $@"WHERE {{ 
                            ?s <http://w3id.org/roh/crisIdentifier> ?crisIdentifier. 
                            FILTER(?crisIdentifier in ('{string.Join("', '", listaAux.Select(x => x))}')) }}";
                SparqlObject resultadoQueryPerson = pResourceApi.VirtuosoQuery(selectPerson, wherePerson, "group");
                if (resultadoQueryPerson != null && resultadoQueryPerson.results != null && resultadoQueryPerson.results.bindings != null && resultadoQueryPerson.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPerson.results.bindings)
                    {
                        dicPersonasBBDD[fila["crisIdentifier"].value] = fila["s"].value;
                    }
                }
            }

            return dicPersonasBBDD;
        }

        /// <summary>
        /// Obtiene los datos de los grupos del SGI.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pId"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public static Grupo GetGrupoSGI(IHarvesterServices pHarvesterServices, ReadConfig pConfig, string pId, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            // Obtención de datos en bruto.
            Grupo grupo = new();
            string xmlResult = pHarvesterServices.GetRecord(pId, pConfig);

            if (string.IsNullOrEmpty(xmlResult))
            {
                return null;
            }

            XmlSerializer xmlSerializer = new(typeof(Grupo));
            using (StringReader sr = new(xmlResult))
            {
                grupo = (Grupo)xmlSerializer.Deserialize(sr);
            }

            return grupo;
        }

        /// <summary>
        /// Crea el objeto Grupo para cargarlo.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <param name="pRabbitConf"></param>
        /// <returns></returns>
        public GroupOntology.Group CrearGroupOntology(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf)
        {
            GroupOntology.Group groupOntology = new();

            HashSet<string> listaIdsPersonas = new();
            if (this.equipo != null && this.equipo.Any())
            {
                foreach (GrupoEquipo grupoEquipo in this.equipo)
                {
                    listaIdsPersonas.Add(grupoEquipo.personaRef);
                }
            }

            Dictionary<string, string> dicPersonasBBDD = Persona.ObtenerPersonasBBDD(listaIdsPersonas, pResourceApi);

            // Crisidentifier.
            groupOntology.Roh_crisIdentifier = this.id.ToString();

            // Validate.
            groupOntology.Roh_isValidated = true;

            // Nombre del grupo.
            groupOntology.Roh_title = this.nombre;

            // Código interno del grupo.
            groupOntology.Roh_normalizedCode = this.codigo;

            // Fecha de inicio del grupo.
            groupOntology.Roh_foundationDate = this.fechaInicio;

            // Duración.
            if (this.fechaInicio != null && this.fechaFin != null)
            {
                Tuple<string, string, string> duracion = RestarFechas((DateTime)this.fechaInicio, (DateTime)this.fechaFin);
                groupOntology.Roh_durationYears = duracion.Item1;
                groupOntology.Roh_durationMonths = duracion.Item2;
                groupOntology.Roh_durationDays = duracion.Item3;
            }

            Dictionary<string, string> dicPersonassCargadas = new();
            foreach (KeyValuePair<string, string> item in dicPersonasBBDD)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    Persona personaAux = Persona.GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + item.Key, pDicRutas);
                    if (personaAux != null)
                    {
                        string idGnoss = personaAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "person", pDicIdentificadores, pDicRutas, pRabbitConf, true);
                        pDicIdentificadores["person"].Add(idGnoss);
                        dicPersonassCargadas[item.Key] = idGnoss;
                    }
                    // TODO: RUTA
                }
                else
                {
                    dicPersonassCargadas[item.Key] = item.Value;
                }
            }

            List<GroupOntology.BFO_0000023> listaPersonas = new();
            foreach (GrupoEquipo grupoEquipo in this.equipo)
            {
                GroupOntology.BFO_0000023 persona = new();
                if (dicPersonassCargadas.ContainsKey(grupoEquipo.personaRef))
                {
                    persona.IdRoh_roleOf = dicPersonasBBDD[grupoEquipo.personaRef];

                    // IP principal
                    persona.Roh_isIP = grupoEquipo.rol.abreviatura == "IP";

                    // Fecha de inicio de la persona en el grupo.
                    if (!string.IsNullOrEmpty(grupoEquipo.fechaInicio))
                    {
                        persona.Vivo_start = Convert.ToDateTime(grupoEquipo.fechaInicio);
                    }

                    // Fecha de fin de la persona en el grupo.
                    if (!string.IsNullOrEmpty(grupoEquipo.fechaFin))
                    {
                        persona.Vivo_end = Convert.ToDateTime(grupoEquipo.fechaFin);
                    }

                    listaPersonas.Add(persona);
                }
            }

            // Relates.
            groupOntology.Vivo_relates = listaPersonas;

            return groupOntology;
        }

        /// <summary>
        /// Identificador.
        /// </summary>
        public int? id { get; set; }
        /// <summary>
        /// Nombre del grupo.
        /// </summary>
        public string nombre { get; set; }
        /// <summary>
        /// Feha de inicio del grupo.
        /// </summary>
        public DateTime? fechaInicio { get; set; }
        /// <summary>
        /// Fecha de fin del grupo.
        /// </summary>
        public DateTime? fechaFin { get; set; }
        /// <summary>
        /// Identificador en el SGE.
        /// </summary>
        public string proyectoSgeRef { get; set; }
        /// <summary>
        /// ID de solicitud.
        /// </summary>
        public int? solicitudId { get; set; }
        /// <summary>
        /// Código del grupo.
        /// </summary>
        public string codigo { get; set; }
        /// <summary>
        /// Tipo del grupo.
        /// </summary>
        public string tipo { get; set; }
        /// <summary>
        /// Especial Investigacion
        /// </summary>
        public bool? especialInvestigacion { get; set; }
        /// <summary>
        /// Indica si el grupo está acitvo o no.
        /// </summary>
        public bool? activo { get; set; }
        /// <summary>
        /// Miembros del equipo del grupo.
        /// </summary>
        public List<GrupoEquipo> equipo { get; set; }
        /// <summary>
        /// Palabras clave del grupo.
        /// </summary>
        public List<GrupoPalabraClave> palabrasClave { get; set; }
        /// <summary>
        /// Lineas de clasificacion del grupo.
        /// </summary>
        public List<LineaClasificacion> lineasClasificacion { get; set; }
        /// <summary>
        /// Lineas de investigación del grupo.
        /// </summary>
        public List<LineaInvestigacion> lineasInvestigacion { get; set; }
        /// <summary>
        /// Investigadores principales del grupo.
        /// </summary>
        public List<string> investigadoresPrincipales { get; set; }
        /// <summary>
        /// Investigadores que más han participado en el grupo.
        /// </summary>
        public List<string> investigadoresPrincipalesMaxParticipacion { get; set; }
    }
}
