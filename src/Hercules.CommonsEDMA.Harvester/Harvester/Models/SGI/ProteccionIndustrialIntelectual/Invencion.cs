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

namespace OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual
{
    /// <summary>
    /// Invencion (PII)
    /// </summary>
    public class Invencion : SGI_Base
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
            PatentOntology.Patent patente = CrearPatentOntology(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas, pRabbitConf);
            pResourceApi.ChangeOntoly("patent");
            return patente.ToGnossApiResource(pResourceApi, null);
        }

        /// <summary>
        /// Obtiene los IDs de BBDD mediante el crisidentifier.
        /// </summary>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public override string ObtenerIDBBDD(ResourceApi pResourceApi)
        {
            Dictionary<string, string> respuesta = ObtenerPatenteBBDD(new HashSet<string>() { id.ToString() }, pResourceApi);
            if (respuesta.ContainsKey(id.ToString()) && !string.IsNullOrEmpty(respuesta[id.ToString()]))
            {
                return respuesta[id.ToString()];
            }
            return null;
        }

        public override void ToRecursoAdicional(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, string pIdGnoss)
        {     
            // No necesario para esta clase.
        }

        /// <summary>
        /// Crea el objeto Patent para ser cargado.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <param name="pRabbitConf"></param>
        /// <returns></returns>
        public PatentOntology.Patent CrearPatentOntology(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf)
        {
            HashSet<string> listaIdsPersonas = new HashSet<string>();
            if (this.inventores != null && this.inventores.Any())
            {
                foreach (Inventor item in this.inventores)
                {
                    // Obtención de personas de BBDD con los IDs obtenidos por el SGI.
                    listaIdsPersonas.Add(item.inventorRef);
                }
            }

            Dictionary<string, string> dicPersonasBBDD = Persona.ObtenerPersonasBBDD(listaIdsPersonas, pResourceApi);

            PatentOntology.Patent patente = new PatentOntology.Patent();

            // CrisIdentifier
            patente.Roh_crisIdentifier = this.id.ToString();

            // Validate.
            patente.Roh_isValidated = true;

            // Título
            patente.Roh_title = this.titulo;

            // Fecha
            patente.Dct_issued = this.fechaComunicacion;

            // Descripción
            patente.Roh_qualityDescription = this.descripcion;

            Dictionary<string, string> dicPersonasCargadas = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in dicPersonasBBDD)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    Persona personaAux = Persona.GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + item.Key, pDicRutas);
                    if (personaAux != null)
                    {
                        string idGnoss = personaAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "person", pDicIdentificadores, pDicRutas, pRabbitConf);
                        pDicIdentificadores["person"].Add(idGnoss);
                        dicPersonasCargadas[item.Key] = idGnoss;
                    }
                    // TODO: RUTA
                }
                else
                {
                    dicPersonasCargadas[item.Key] = item.Value;
                }
            }

            // Autores
            List<PatentOntology.PersonAux> listaPersonas = new List<PatentOntology.PersonAux>();

            foreach (Inventor inventor in this.inventores)
            {    
                PatentOntology.PersonAux persona = new PatentOntology.PersonAux();
                if (dicPersonasCargadas.ContainsKey(inventor.inventorRef) && !string.IsNullOrEmpty(dicPersonasCargadas[inventor.inventorRef]))
                {
                    persona.IdRdf_member = dicPersonasCargadas[inventor.inventorRef];
                    listaPersonas.Add(persona);
                } 
            }

            patente.Bibo_authorList = listaPersonas;

            return patente;
        }

        /// <summary>
        /// Obtiene los IDs de los recursos de invenciones cargados en BBDD mediante el crisidentifier.
        /// </summary>
        /// <param name="pListaIds"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ObtenerPatenteBBDD(HashSet<string> pListaIds, ResourceApi pResourceApi)
        {
            List<List<string>> listaPatentes = SplitList(pListaIds.ToList(), 1000).ToList();
            Dictionary<string, string> dicPatentesBBDD = new Dictionary<string, string>();
            foreach (string patente in pListaIds)
            {
                if (patente.Contains("_"))
                {
                    dicPatentesBBDD[patente.Split("_")[1]] = "";
                }
                else
                {
                    dicPatentesBBDD[patente] = "";
                }
            }
            foreach (List<string> listaItem in listaPatentes)
            {
                List<string> listaAux = new List<string>();
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
                SparqlObject resultadoQueryPerson = pResourceApi.VirtuosoQuery(selectPerson, wherePerson, "project");
                if (resultadoQueryPerson != null && resultadoQueryPerson.results != null && resultadoQueryPerson.results.bindings != null && resultadoQueryPerson.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPerson.results.bindings)
                    {
                        dicPatentesBBDD[fila["crisIdentifier"].value] = fila["s"].value;
                    }
                }
            }

            return dicPatentesBBDD;
        }

        /// <summary>
        /// Obtiene los datos de las invenciones del SGI.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pId"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public static Invencion GetInvencionSGI(IHarvesterServices pHarvesterServices, ReadConfig pConfig, string pId, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            // Obtención de datos en bruto.
            Invencion invencion = new Invencion();
            string xmlResult = pHarvesterServices.GetRecord(pId, pConfig);

            if (string.IsNullOrEmpty(xmlResult))
            {
                return null;
            }

            XmlSerializer xmlSerializer = new(typeof(Invencion));
            using (StringReader sr = new(xmlResult))
            {
                invencion = (Invencion)xmlSerializer.Deserialize(sr);
            }

            return invencion;
        }

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
