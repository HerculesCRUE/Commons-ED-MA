using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Harvester;
using Harvester.Models.ModelsBBDD;
using Harvester.Models.RabbitMQ;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OAI_PMH.Models.SGI.Organization
{
    /// <summary>
    /// Empresa
    /// </summary>
    public class Empresa : SGI_Base
    {
        public override ComplexOntologyResource ToRecurso(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, bool pFusionarPersona = false, string pIdPersona = null)
        {
            OrganizationOntology.Organization proyecto = CrearOrganizationOntology(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            return proyecto.ToGnossApiResource(pResourceApi, null);
        }

        public override string ObtenerIDBBDD(ResourceApi pResourceApi)
        {
            Dictionary<string, string> respuesta = ObtenerEmpresaBBDD(new HashSet<string>() { Id.ToString() }, pResourceApi);
            if (respuesta.ContainsKey(Id.ToString()) && !string.IsNullOrEmpty(respuesta[Id.ToString()]))
            {
                return respuesta[Id.ToString()];
            }
            return null;
        }

        public static Dictionary<string, string> ObtenerOrganizacionesBBDD(HashSet<string> pListaIds, ResourceApi pResourceApi)
        {
            List<List<string>> listasPersonas = SplitList(pListaIds.ToList(), 1000).ToList();
            Dictionary<string, string> dicOrganizacionesBBDD = new Dictionary<string, string>();
            foreach (string organizacion in pListaIds)
            {
                dicOrganizacionesBBDD[organizacion] = "";
            }
            foreach (List<string> listaItem in listasPersonas)
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
                SparqlObject resultadoQueryPerson = pResourceApi.VirtuosoQuery(selectPerson, wherePerson, "organization");
                if (resultadoQueryPerson != null && resultadoQueryPerson.results != null && resultadoQueryPerson.results.bindings != null && resultadoQueryPerson.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPerson.results.bindings)
                    {
                        dicOrganizacionesBBDD[fila["crisIdentifier"].value] = fila["s"].value;
                    }
                }
            }

            return dicOrganizacionesBBDD;
        }

        public static Empresa GetOrganizacionSGI(IHarvesterServices pHarvesterServices, ReadConfig pConfig, string pId, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            // Obtención de datos en bruto.
            Empresa empresa = new Empresa();
            string xmlResult = pHarvesterServices.GetRecord(pId, pConfig);

            if (string.IsNullOrEmpty(xmlResult))
            {
                return null;
            }

            XmlSerializer xmlSerializer = new(typeof(Empresa));
            using (StringReader sr = new(xmlResult))
            {
                empresa = (Empresa)xmlSerializer.Deserialize(sr);
            }

            return empresa;
        }

        public static OrganizacionBBDD GetOrganizacionBBDD(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, string pIdGnoss)
        {
            SparqlObject resultadoQuery = null;
            StringBuilder select = new StringBuilder(), where = new StringBuilder();

            // Consulta sparql.
            select.Append("SELECT ?crisIdentifier ?titulo ?localidad ");
            where.Append("WHERE { ");
            where.Append($@"<{pIdGnoss}> <http://w3id.org/roh/crisIdentifier> ?crisIdentifier. ");
            where.Append($@"<{pIdGnoss}> <http://w3id.org/roh/title> ?titulo. ");
            where.Append($@"OPTIONAL{{<{pIdGnoss}> <https://www.w3.org/2006/vcard/ns#locality> ?localidad. }}");
            where.Append("} ");

            resultadoQuery = pResourceApi.VirtuosoQuery(select.ToString(), where.ToString(), "organization");

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    OrganizacionBBDD organizacion = new OrganizacionBBDD();
                    if (!string.IsNullOrEmpty(fila["crisIdentifier"].value))
                    {
                        organizacion.crisIdentifier = fila["crisIdentifier"].value;
                    }
                    if (!string.IsNullOrEmpty(fila["titulo"].value))
                    {
                        organizacion.title = fila["titulo"].value;
                    }
                    if (fila.ContainsKey("localidad") && !string.IsNullOrEmpty(fila["localidad"].value))
                    {
                        organizacion.locality = fila["localidad"].value;
                    }
                    return organizacion;
                }
            }

            return null;
        }

        public override void ToRecursoAdicional(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, string pIdGnoss)
        {
        }

        public static Dictionary<string, string> ObtenerEmpresaBBDD(HashSet<string> pListaIds, ResourceApi pResourceApi)
        {
            List<List<string>> listaEmpresas = SplitList(pListaIds.ToList(), 1000).ToList();
            Dictionary<string, string> dicEmpresasBBDD = new Dictionary<string, string>();
            foreach (string empresa in pListaIds)
            {
                if (empresa.Contains("_"))
                {
                    dicEmpresasBBDD[empresa.Split("_")[1]] = "";
                }
                else
                {
                    dicEmpresasBBDD[empresa] = "";
                }
            }
            foreach (List<string> listaItem in listaEmpresas)
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
                SparqlObject resultadoQueryPerson = pResourceApi.VirtuosoQuery(selectPerson, wherePerson, "organization");
                if (resultadoQueryPerson != null && resultadoQueryPerson.results != null && resultadoQueryPerson.results.bindings != null && resultadoQueryPerson.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPerson.results.bindings)
                    {
                        dicEmpresasBBDD[fila["crisIdentifier"].value] = fila["s"].value;
                    }
                }
            }

            return dicEmpresasBBDD;
        }
                
        public OrganizationOntology.Organization CrearOrganizationOntology(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            OrganizationOntology.Organization organization = new OrganizationOntology.Organization();
            organization.Roh_crisIdentifier = this.Id;
            organization.Roh_title = this.Nombre;
            organization.Vcard_locality = this.DatosContacto?.Direccion;
            return organization;
        }
                
        /// <summary>
        /// Identificador de la empresa.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre de la empresa.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Razón social de la empresa.
        /// </summary>
        public string RazonSocial { get; set; }
        /// <summary>
        /// Tipo de identificador fiscal de la empresa.
        /// </summary>
        public TipoIdentificador TipoIdentificador { get; set; }
        /// <summary>
        /// Número de identificación fiscal de la empresa del tipo indicado en "tipoIdentificador".
        /// </summary>
        public string NumeroIdentificacion { get; set; }
        /// <summary>
        /// Indicador de si se trata de una empresa con datos económicos o sin datos económicos
        /// </summary>
        public bool? DatosEconomicos { get; set; }
        /// <summary>
        /// Identificador de la empresa padre o entidad principal.
        /// </summary>
        public string PadreId { get; set; }
        /// <summary>
        /// Datos de contacto
        /// </summary>
        public DatosContacto DatosContacto { get; set; }
    }
}
