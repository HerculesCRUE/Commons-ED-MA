using Hercules.CommonsEDMA.Desnormalizador.Models.Services;
using Hercules.CommonsEDMA.Desnormalizador.Models.Similarity;
using Es.Riam.Gnoss.Web.MVC.Models.IntegracionContinua;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores
{
    /// <summary>
    /// Clase para actualizar propiedades de documentos
    /// </summary>
    class ActualizadorDocument : ActualizadorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pResourceApi">API Wrapper de GNOSS</param>
        public ActualizadorDocument(ResourceApi pResourceApi) : base(pResourceApi)
        {
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/isValidated de los http://purl.org/ontology/bibo/Document
        /// los documentos validados (son los documentos oficiales, es decir, los que tienen http://w3id.org/roh/crisIdentifier o validados por la universidad)
        /// Esta propiedad se utilizará como filtro en el bucador general de publicaciones
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarDocumentosValidados(List<string> pDocuments = null)
        {
            HashSet<string> filtersDocumentosValidados = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersDocumentosValidados.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersDocumentosValidados.Count == 0)
            {
                filtersDocumentosValidados.Add("");
            }
            //Eliminamos los duplicados
            EliminarDuplicados("document", $"{GetUrlPrefix("bibo")}Document", "http://w3id.org/roh/isValidated");

            foreach (string filter in filtersDocumentosValidados)
            {
                while (true)
                {
                    int limitDocumentosValidados = 500;
                    String selectDocumentosValidados = @"select ?document ?isValidatedCargado ?isValidatedCargar";
                    String whereDocumentosValidados = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                MINUS
                                {{
                                    ?document <http://w3id.org/roh/isValidated> 'true'.
                                }}
                                OPTIONAL
                                {{
                                    ?document <http://w3id.org/roh/isValidated> ?isValidatedCargado.
                                }}
                                {{
                                    select distinct ?document IF(BOUND(?isValidatedCargar),?isValidatedCargar,'false')  as ?isValidatedCargar
                                    Where
                                    {{
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        OPTIONAL
                                        {{
                                            ?document <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                            BIND('true' as ?isValidatedCargar)
                                        }}
                                        OPTIONAL
                                        {{
                                            ?document <http://w3id.org/roh/validationStatusPRC> 'validado'.
                                            BIND('true' as ?isValidatedCargar)
                                        }} 
                                    }}
                                }}
                                FILTER(?isValidatedCargado!= ?isValidatedCargar OR !BOUND(?isValidatedCargado) )
                            }} limit {limitDocumentosValidados}";
                    SparqlObject resultadoDocumentosValidados = mResourceApi.VirtuosoQuery(selectDocumentosValidados, whereDocumentosValidados, "document");

                    Parallel.ForEach(resultadoDocumentosValidados.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string isValidatedCargar = fila["isValidatedCargar"].value;
                        string isValidatedCargado = "";
                        if (fila.ContainsKey("isValidatedCargado"))
                        {
                            isValidatedCargado = fila["isValidatedCargado"].value;
                        }
                        ActualizadorTriple(document, $"{GetUrlPrefix("roh")}isValidated", isValidatedCargado, isValidatedCargar);
                    });

                    if (resultadoDocumentosValidados.results.bindings.Count != limitDocumentosValidados)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/isProducedBy de los http://purl.org/ontology/bibo/Document 
        /// los grupos a los que pertenecen los autores de los documentos oficiales (http://w3id.org/roh/isValidated) en el momento de la publicación del documento
        /// Esta propiedad se utilizará para mostrar en las fichas de los grupos el listado de sus publicaciones oficiales
        /// No tiene dependencias
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        /// <param name="pDocuments">IDs de los documentos</param>
        public void ActualizarPertenenciaGrupos(List<string> pGroups = null, List<string> pDocuments = null)
        {
            HashSet<string> filtersPertenenciaGrupos = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersPertenenciaGrupos.Add($" FILTER(?grupo in (<{string.Join(">,<", pGroups)}>))");
            }
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersPertenenciaGrupos.Add($" FILTER(?doc in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersPertenenciaGrupos.Count == 0)
            {
                filtersPertenenciaGrupos.Add("");
            }

            foreach (string filter in filtersPertenenciaGrupos)
            {
                while (true)
                {
                    //Añadimos a documentos
                    int limitInsercionPertenenciaGrupos = 500;
                    String selectInsercionPertenenciaGrupos = @"select distinct ?doc ?grupo  ";
                    String whereInsercionPertenenciaGrupos = @$"where{{
                                    {filter}
                                    {{
                                        {{
                                            select distinct ?grupo ?doc
                                            Where{{
                                                ?grupo a <http://xmlns.com/foaf/0.1/Group>.
                                                ?grupo <http://vivoweb.org/ontology/core#relates> ?members.
                                                ?members <http://w3id.org/roh/roleOf> ?person.
                                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                                ?members <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.
                                                ?members <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.
                                                BIND(xsd:integer(?fechaPersonaEnd) as ?fechaPersonaEndAux)
												BIND(xsd:integer(?fechaPersonaInit) as ?fechaPersonaInitAux)
                                                ?doc a <http://purl.org/ontology/bibo/Document>.
                                                ?doc <http://w3id.org/roh/isValidated> 'true'.
                                                ?doc <http://purl.org/ontology/bibo/authorList> ?autores.
                                                ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                ?doc <http://purl.org/dc/terms/issued> ?fechaPublicacion.
                                                FILTER(xsd:integer(?fechaPublicacion)>= ?fechaPersonaInitAux AND xsd:integer(?fechaPublicacion)<= ?fechaPersonaEndAux)
                                            }}
                                        }}UNION
                                        {{
                                            select distinct ?grupo ?doc
                                            Where{{
                                                ?grupo a <http://xmlns.com/foaf/0.1/Group>.
                                                ?grupo <http://vivoweb.org/ontology/core#relates> ?members.
                                                ?members <http://w3id.org/roh/roleOf> ?person.
                                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                                ?members <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.
												MINUS{{?members <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
												BIND(xsd:integer(?fechaPersonaInit) as ?fechaPersonaInitAux)
                                                ?doc a <http://purl.org/ontology/bibo/Document>.
                                                ?doc <http://w3id.org/roh/isValidated> 'true'.
                                                ?doc <http://purl.org/ontology/bibo/authorList> ?autores.
                                                ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                ?doc <http://purl.org/dc/terms/issued> ?fechaPublicacion.
                                                FILTER(xsd:integer(?fechaPublicacion)>= ?fechaPersonaInitAux)
                                            }}
                                        }}UNION
                                        {{
                                            select distinct ?grupo ?doc
                                            Where{{
                                                ?grupo a <http://xmlns.com/foaf/0.1/Group>.
                                                ?grupo <http://vivoweb.org/ontology/core#relates> ?members.
                                                ?members <http://w3id.org/roh/roleOf> ?person.
                                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                                MINUS{{?members <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
												?members <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.
												BIND(xsd:integer(?fechaPersonaEnd) as ?fechaPersonaEndAux)
                                                ?doc a <http://purl.org/ontology/bibo/Document>.
                                                ?doc <http://w3id.org/roh/isValidated> 'true'.
                                                ?doc <http://purl.org/ontology/bibo/authorList> ?autores.
                                                ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                ?doc <http://purl.org/dc/terms/issued> ?fechaPublicacion.
                                                FILTER( xsd:integer(?fechaPublicacion)<= ?fechaPersonaEndAux)
                                            }}
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        ?grupo a <http://xmlns.com/foaf/0.1/Group>.
                                        ?doc a <http://purl.org/ontology/bibo/Document>.
                                        ?doc <http://w3id.org/roh/isProducedBy> ?grupo.
                                    }}
                                }}order by desc(?doc) limit {limitInsercionPertenenciaGrupos}";
                    SparqlObject resultadoInsercionPertenenciaGrupos = mResourceApi.VirtuosoQueryMultipleGraph(selectInsercionPertenenciaGrupos, whereInsercionPertenenciaGrupos, new List<string>() { "document", "curriculumvitae", "person", "group" });
                    InsercionMultiple(resultadoInsercionPertenenciaGrupos.results.bindings, $"{GetUrlPrefix("roh")}isProducedBy", "doc", "grupo");
                    if (resultadoInsercionPertenenciaGrupos.results.bindings.Count != limitInsercionPertenenciaGrupos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Eliminamos de documentos
                    int limitEliminacionPertenenciaGrupos = 500;
                    String selectEliminacionPertenenciaGrupos = @"select distinct ?doc ?grupo  ";
                    String whereEliminacionPertenenciaGrupos = @$"where{{
                                    {filter}
                                    {{
                                        ?grupo a <http://xmlns.com/foaf/0.1/Group>.
                                        ?doc a <http://purl.org/ontology/bibo/Document>.
                                        ?doc <http://w3id.org/roh/isProducedBy> ?grupo.                                                                   
                                    }}
                                    MINUS
                                    {{
                                        {{
                                            select distinct ?grupo ?doc
                                            Where{{
                                                ?grupo a <http://xmlns.com/foaf/0.1/Group>.
                                                ?grupo <http://vivoweb.org/ontology/core#relates> ?members.
                                                ?members <http://w3id.org/roh/roleOf> ?person.
                                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                                ?members <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.
                                                ?members <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.
                                                BIND(xsd:integer(?fechaPersonaEnd) as ?fechaPersonaEndAux)
												BIND(xsd:integer(?fechaPersonaInit) as ?fechaPersonaInitAux)
                                                ?doc a <http://purl.org/ontology/bibo/Document>.
                                                ?doc <http://w3id.org/roh/isValidated> 'true'.
                                                ?doc <http://purl.org/ontology/bibo/authorList> ?autores.
                                                ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                ?doc <http://purl.org/dc/terms/issued> ?fechaPublicacion.
                                                FILTER(xsd:integer(?fechaPublicacion)>= ?fechaPersonaInitAux AND xsd:integer(?fechaPublicacion)<= ?fechaPersonaEndAux)
                                            }}
                                        }}UNION
                                        {{
                                            select distinct ?grupo ?doc
                                            Where{{
                                                ?grupo a <http://xmlns.com/foaf/0.1/Group>.
                                                ?grupo <http://vivoweb.org/ontology/core#relates> ?members.
                                                ?members <http://w3id.org/roh/roleOf> ?person.
                                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                                ?members <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.
												MINUS{{?members <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
												BIND(xsd:integer(?fechaPersonaInit) as ?fechaPersonaInitAux)
                                                ?doc a <http://purl.org/ontology/bibo/Document>.
                                                ?doc <http://w3id.org/roh/isValidated> 'true'.
                                                ?doc <http://purl.org/ontology/bibo/authorList> ?autores.
                                                ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                ?doc <http://purl.org/dc/terms/issued> ?fechaPublicacion.
                                                FILTER(xsd:integer(?fechaPublicacion)>= ?fechaPersonaInitAux)
                                            }}
                                        }}UNION
                                        {{
                                            select distinct ?grupo ?doc
                                            Where{{
                                                ?grupo a <http://xmlns.com/foaf/0.1/Group>.
                                                ?grupo <http://vivoweb.org/ontology/core#relates> ?members.
                                                ?members <http://w3id.org/roh/roleOf> ?person.
                                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                                MINUS{{?members <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
												?members <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.
												BIND(xsd:integer(?fechaPersonaEnd) as ?fechaPersonaEndAux)
                                                ?doc a <http://purl.org/ontology/bibo/Document>.
                                                ?doc <http://w3id.org/roh/isValidated> 'true'.
                                                ?doc <http://purl.org/ontology/bibo/authorList> ?autores.
                                                ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                ?doc <http://purl.org/dc/terms/issued> ?fechaPublicacion.
                                                FILTER( xsd:integer(?fechaPublicacion)<= ?fechaPersonaEndAux)
                                            }}
                                        }}
                                    }}
                                }}order by desc(?doc) limit {limitEliminacionPertenenciaGrupos}";
                    SparqlObject resultadoEliminacionPertenenciaGrupos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminacionPertenenciaGrupos, whereEliminacionPertenenciaGrupos, new List<string>() { "document", "curriculumvitae", "person", "group" });
                    EliminacionMultiple(resultadoEliminacionPertenenciaGrupos.results.bindings, $"{GetUrlPrefix("roh")}isProducedBy", "doc", "grupo");
                    if (resultadoEliminacionPertenenciaGrupos.results.bindings.Count != limitEliminacionPertenenciaGrupos)
                    {
                        break;
                    }
                }

            }
        }


        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/citationCount de los http://purl.org/ontology/bibo/Document
        /// el nº máximo de citas con el que cuenta
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">IDs de documentos</param>
        public void ActualizarNumeroCitasMaximas(List<string> pDocuments = null)
        {
            HashSet<string> filtersNumeroCitasMaximas = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersNumeroCitasMaximas.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersNumeroCitasMaximas.Count == 0)
            {
                filtersNumeroCitasMaximas.Add("");
            }

            //Eliminamos los duplicados
            EliminarDuplicados("document", $"{GetUrlPrefix("bibo")}Document", $"{GetUrlPrefix("roh")}citationCount");

            foreach (string filter in filtersNumeroCitasMaximas)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limitNumeroCitasMaximas = 500;
                    String selectNumeroCitasMaximas = @"select ?document ?numCitasCargadas IF (BOUND (?numCitasACargar), ?numCitasACargar, 0 ) as ?numCitasACargar";
                    String whereNumeroCitasMaximas = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                OPTIONAL
                                {{
                                  ?document <http://w3id.org/roh/citationCount> ?numCitasCargadasAux. 
                                  BIND(xsd:int( ?numCitasCargadasAux) as  ?numCitasCargadas)
                                }}
                                {{
                                  select ?document max(xsd:int( ?numCitas)) as ?numCitasACargar
                                  Where{{
                                    ?document a <http://purl.org/ontology/bibo/Document>.
                                    OPTIONAL{{
                                        {{
                                            ?document <http://w3id.org/roh/wosCitationCount> ?numCitas.
                                        }}
                                        UNION{{
                                            ?document <http://w3id.org/roh/scopusCitationCount> ?numCitas.
                                        }}
                                        UNION{{
                                            ?document <http://w3id.org/roh/semanticScholarCitationCount> ?numCitas.
                                        }}
                                    }}                                
                                  }}
                                }}
                                FILTER(?numCitasCargadas!= ?numCitasACargar OR !BOUND(?numCitasCargadas) )
                            }} limit {limitNumeroCitasMaximas}";
                    SparqlObject resultadoNumeroCitasMaximas = mResourceApi.VirtuosoQuery(selectNumeroCitasMaximas, whereNumeroCitasMaximas, "document");

                    Parallel.ForEach(resultadoNumeroCitasMaximas.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string numCitasACargar = fila["numCitasACargar"].value;
                        string numCitasCargadas = "";
                        if (fila.ContainsKey("numCitasCargadas"))
                        {
                            numCitasCargadas = fila["numCitasCargadas"].value;
                        }
                        ActualizadorTriple(document, "http://w3id.org/roh/citationCount", numCitasCargadas, numCitasACargar);
                    });

                    if (resultadoNumeroCitasMaximas.results.bindings.Count != limitNumeroCitasMaximas)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/hasKnowledgeArea de los http://purl.org/ontology/bibo/Document 
        /// las áreas del documento (obtenido de varias propiedades en las que están las áreas en función de su origen)
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarAreasDocumentos(List<string> pDocuments = null)
        {
            //Categorías
            //unificada-->http://w3id.org/roh/hasKnowledgeArea
            //usuario-->http://w3id.org/roh/userKnowledgeArea
            //external-->http://w3id.org/roh/externalKnowledgeArea
            //enriched-->http://w3id.org/roh/enrichedKnowledgeArea

            HashSet<string> filtersAreasDocumentos = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersAreasDocumentos.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersAreasDocumentos.Count == 0)
            {
                filtersAreasDocumentos.Add("");
            }

            foreach (string filter in filtersAreasDocumentos)
            {
                //Eliminamos las categorías duplicadas
                while (true)
                {
                    int limit = 500;
                    String selectEliminacionAreasDocumentos = @"select ?document ?categoryNode ";
                    String whereEliminacionAreasDocumentos = @$"where{{
                                select distinct ?document ?hasKnowledgeAreaAux  ?categoryNode
                                where{{
                                    {filter}
                                    ?document a <http://purl.org/ontology/bibo/Document>.
                                    ?document  <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaAux.
                                    ?hasKnowledgeAreaAux <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                               }}
                            }}group by ?document ?categoryNode HAVING (COUNT(*) > 1) limit {limit}";
                    SparqlObject resultadoEliminacionAreasDocumentos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminacionAreasDocumentos, whereEliminacionAreasDocumentos, new List<string>() { "document", "taxonomy" });

                    Parallel.ForEach(resultadoEliminacionAreasDocumentos.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string categoryNode = fila["categoryNode"].value;
                        string selectEliminacionAreasDocumentosIn = @"select ?document ?hasKnowledgeArea ?categoryNode ";
                        string whereEliminacionAreasDocumentosIn = @$"where{{
                                    FILTER(?document=<{document}>)
                                    FILTER(?categoryNode =<{categoryNode}>)
                                    {{ 
                                        select distinct ?document ?hasKnowledgeArea  ?categoryNode
                                        where{{
                                            ?document a <http://purl.org/ontology/bibo/Document>.
                                            ?document  <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                                            ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?categoryNode.
                                            MINUS{{
                                                ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                            }}
                                        }}
                                    }}
                                }}";
                        SparqlObject resultadoEliminacionAreasDocumentosIn = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminacionAreasDocumentosIn, whereEliminacionAreasDocumentosIn, new List<string>() { "document", "taxonomy" });
                        List<RemoveTriples> triplesRemove = new();
                        foreach (string hasKnowledgeArea in resultadoEliminacionAreasDocumentosIn.results.bindings.GetRange(1, resultadoEliminacionAreasDocumentosIn.results.bindings.Count - 1).Select(x => x["hasKnowledgeArea"].value).ToList())
                        {
                            triplesRemove.Add(new RemoveTriples()
                            {
                                Predicate = $"{GetUrlPrefix("roh")}hasKnowledgeArea",
                                Value = hasKnowledgeArea
                            }); ;
                        }
                        if (triplesRemove.Count > 0)
                        {
                            var resultadox = mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.RemoveTriples>>() { { mResourceApi.GetShortGuid(document), triplesRemove } });
                        }
                    });


                    if (resultadoEliminacionAreasDocumentos.results.bindings.Count != limit)
                    {
                        break;
                    }
                }



                //Cargamos el tesauro
                Dictionary<string, string> dicAreasBroader = new();
                String selectDicAreasBroader = @"select distinct * ";
                String whereDicAreasBroader = @$"where{{
                            ?concept a <http://www.w3.org/2008/05/skos#Concept>.
                            ?concept <http://purl.org/dc/elements/1.1/source> 'researcharea'
                            OPTIONAL{{?concept <http://www.w3.org/2008/05/skos#broader> ?broader}}
                        }}";
                SparqlObject resultadoDicAreasBroader = mResourceApi.VirtuosoQuery(selectDicAreasBroader, whereDicAreasBroader, "taxonomy");

                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoDicAreasBroader.results.bindings)
                {
                    string concept = fila["concept"].value;
                    string broader = "";
                    if (fila.ContainsKey("broader"))
                    {
                        broader = fila["broader"].value;
                    }
                    dicAreasBroader.Add(concept, broader);
                }


                while (true)
                {
                    int limitInsertamosAreasDocumentos = 500;
                    //INSERTAMOS
                    String selectInsertamosAreasDocumentos = @"select ?document ?categoryNode ";
                    String whereInsertamosAreasDocumentos = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                {{
                                    select  distinct ?document ?hasKnowledgeAreaDocument ?categoryNode where{{
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        ?document ?props ?hasKnowledgeAreaDocument.
                                        FILTER(?props in (<http://w3id.org/roh/userKnowledgeArea>,<http://w3id.org/roh/externalKnowledgeArea>,<http://w3id.org/roh/enrichedKnowledgeArea>))
                                        ?hasKnowledgeAreaDocument <http://w3id.org/roh/categoryNode> ?categoryNode.
                                        MINUS{{
                                            ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                        }}
                                    }}
                                }}
                                MINUS{{
                                    select distinct ?document ?hasKnowledgeAreaDocumentAux ?categoryNode 
                                    where{{
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        ?document <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaDocumentAux.
                                        ?hasKnowledgeAreaDocumentAux <http://w3id.org/roh/categoryNode> ?categoryNode
                                        MINUS{{
                                            ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                        }}
                                    }}
                                }}
                            }}order by (?document) limit {limitInsertamosAreasDocumentos}";
                    SparqlObject resultadoInsertamosAreasDocumentos = mResourceApi.VirtuosoQueryMultipleGraph(selectInsertamosAreasDocumentos, whereInsertamosAreasDocumentos, new List<string>() { "document", "taxonomy" });
                    InsertarCategorias(resultadoInsertamosAreasDocumentos, dicAreasBroader, mResourceApi.GraphsUrl, "document", $"{GetUrlPrefix("roh")}hasKnowledgeArea");
                    if (resultadoInsertamosAreasDocumentos.results.bindings.Count != limitInsertamosAreasDocumentos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limitEliminamosAreasDocumentos = 500;
                    //ELIMINAMOS
                    String selectEliminamosAreasDocumentos = @"select ?document ?hasKnowledgeArea ";
                    String whereEliminamosAreasDocumentos = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                {{
                                    select distinct ?document ?hasKnowledgeArea ?categoryNode 
                                    where{{
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        ?document <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                                        ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?categoryNode
                                        MINUS{{
                                            ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                        }}
                                    }}                               
                                }}
                                MINUS{{
                                    select  distinct ?document ?hasKnowledgeAreaDocument ?categoryNode where{{
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        ?document ?props ?hasKnowledgeAreaDocument.
                                        FILTER(?props in (<http://w3id.org/roh/userKnowledgeArea>,<http://w3id.org/roh/externalKnowledgeArea>,<http://w3id.org/roh/enrichedKnowledgeArea>))
                                        ?hasKnowledgeAreaDocument <http://w3id.org/roh/categoryNode> ?categoryNode.
                                        MINUS{{
                                            ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                        }}
                                    }}
                                 
                                }}
                            }} limit {limitEliminamosAreasDocumentos}";
                    SparqlObject resultadoEliminamosAreasDocumentos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminamosAreasDocumentos, whereEliminamosAreasDocumentos, new List<string>() { "document", "taxonomy" });
                    EliminarCategorias(resultadoEliminamosAreasDocumentos, "document", "http://w3id.org/roh/hasKnowledgeArea");
                    if (resultadoEliminamosAreasDocumentos.results.bindings.Count != limitEliminamosAreasDocumentos)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos en la propiedad http://vivoweb.org/ontology/core#freeTextKeyword de los http://purl.org/ontology/bibo/Document 
        /// los tags (obtenido de varias propiedades en las que están los tags en función de su origen)
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarTagsDocumentos(List<string> pDocuments = null)
        {
            //Etiquetas
            //unificada-->http://vivoweb.org/ontology/core#freeTextKeyword
            //usuario-->http://w3id.org/roh/userKeywords
            //external-->http://w3id.org/roh/externalKeywords
            //enriched-->http://w3id.org/roh/enrichedKeywords@@@http://w3id.org/roh/title

            HashSet<string> filtersTagsDocumentos = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersTagsDocumentos.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersTagsDocumentos.Count == 0)
            {
                filtersTagsDocumentos.Add("");
            }
            foreach (string filter in filtersTagsDocumentos)
            {
                while (true)
                {
                    int limitInsertamosTagsDocumentos = 500;
                    //INSERTAMOS
                    String selectInsertamosTagsDocumentos = @"select ?document ?tag";
                    String whereInsertamosTagsDocumentos = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                {{
                                    {{
                                        select  distinct ?document ?tag where{{
                                            ?document a <http://purl.org/ontology/bibo/Document>.
                                            ?document <http://w3id.org/roh/enrichedKeywords> ?aux.
                                            ?aux <http://w3id.org/roh/title> ?tag.                                       
                                        }}
                                    }}
                                    UNION
                                    {{
                                        select  distinct ?document ?tag where{{
                                            ?document ?props ?tag.
                                            FILTER(?props in (<http://w3id.org/roh/userKeywords>,<http://w3id.org/roh/externalKeywords>))
                                        }}
                                    }}
                                }}
                                MINUS{{
                                    select distinct ?document ?tag
                                    where{{
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        ?document <http://vivoweb.org/ontology/core#freeTextKeyword> ?tagAux.
                                        ?tagAux <http://w3id.org/roh/title> ?tag.
                                    }}
                                }}
                            }}order by (?document) limit {limitInsertamosTagsDocumentos}";
                    SparqlObject resultadoInsertamosTagsDocumentos = mResourceApi.VirtuosoQuery(selectInsertamosTagsDocumentos, whereInsertamosTagsDocumentos, "document");
                    InsercionMultipleTags(resultadoInsertamosTagsDocumentos.results.bindings, "document", "tag");
                    if (resultadoInsertamosTagsDocumentos.results.bindings.Count != limitInsertamosTagsDocumentos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limitEliminamosTagsDocumentos = 500;
                    //ELIMINAMOS
                    String selectEliminamosTagsDocumentos = @"select ?document ?tagAux";
                    String whereEliminamosTagsDocumentos = @$"where{{
                            ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                {{
                                    select distinct ?document ?tagAux ?tag
                                    where{{
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        ?document <http://vivoweb.org/ontology/core#freeTextKeyword> ?tagAux.
                                        ?tagAux <http://w3id.org/roh/title> ?tag.
                                    }}                               
                                }}
                                MINUS{{
                                    {{
                                        select  distinct ?document ?tag where{{
                                            ?document a <http://purl.org/ontology/bibo/Document>.
                                            ?document <http://w3id.org/roh/enrichedKeywords> ?aux.
                                            ?aux <http://w3id.org/roh/title> ?tag.                                       
                                        }}
                                    }}
                                    UNION
                                    {{
                                        select  distinct ?document ?tag where{{
                                            ?document ?props ?tag.
                                            FILTER(?props in (<http://w3id.org/roh/userKeywords>,<http://w3id.org/roh/externalKeywords>))
                                        }}
                                    }}
                                }}
                            }} limit {limitEliminamosTagsDocumentos}";
                    SparqlObject resultadoEliminamosTagsDocumentos = mResourceApi.VirtuosoQuery(selectEliminamosTagsDocumentos, whereEliminamosTagsDocumentos, "document");
                    EliminacionMultiple(resultadoEliminamosTagsDocumentos.results.bindings, "http://vivoweb.org/ontology/core#freeTextKeyword", "document", "tagAux");
                    if (resultadoEliminamosTagsDocumentos.results.bindings.Count != limitEliminamosTagsDocumentos)
                    {
                        break;
                    }
                }

                //Eliminar duplicados
                while (true)
                {
                    int limitEliminamosDuplicados = 500;
                    String selectEliminamosDuplicados = @"select ?document count(?tag) ?tag ";
                    String whereEliminamosDuplicados = @$"where
                                {{
                                    ?document a <http://purl.org/ontology/bibo/Document>.
                                    {filter}
                                    ?document <http://vivoweb.org/ontology/core#freeTextKeyword> ?freeTextKeyword. 
                                    ?freeTextKeyword <http://w3id.org/roh/title> ?tag. 
                                }}group by (?document) (?tag) HAVING (COUNT(?tag) > 1) limit {limitEliminamosDuplicados}";
                    SparqlObject resultadoEliminamosDuplicados = mResourceApi.VirtuosoQuery(selectEliminamosDuplicados, whereEliminamosDuplicados, "document");

                    Parallel.ForEach(resultadoEliminamosDuplicados.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string tag = fila["tag"].value;
                        String selectEliminamosDuplicadosIn = @"select ?document ?data ";
                        String whereEliminamosDuplicadosIn = @$"where
                            {{                            
                                ?document <http://vivoweb.org/ontology/core#freeTextKeyword> ?data. 
                                ?data <http://w3id.org/roh/title> '{tag.Replace("'", "\\'")}'. 
                                FILTER(?document=<{document}>)
                            }}";
                        SparqlObject resultadoEliminamosDuplicadosIn = mResourceApi.VirtuosoQuery(selectEliminamosDuplicadosIn, whereEliminamosDuplicadosIn, "document");

                        foreach (Dictionary<string, SparqlObject.Data> filaEliminamosDuplicadosIn in resultadoEliminamosDuplicadosIn.results.bindings.GetRange(1, resultadoEliminamosDuplicadosIn.results.bindings.Count - 1))
                        {
                            string value = filaEliminamosDuplicadosIn["data"].value;
                            ActualizadorTriple(document, "http://vivoweb.org/ontology/core#freeTextKeyword", value, "");
                        }
                    });
                    if (resultadoEliminamosDuplicados.results.bindings.Count != limitEliminamosDuplicados)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/year de los http://purl.org/ontology/bibo/Document públicos 
        /// el año de publicación
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarAnios(List<string> pDocuments = null)
        {
            HashSet<string> filtersAnios = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersAnios.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersAnios.Count == 0)
            {
                filtersAnios.Add("");
            }

            EliminarDuplicados("document", $"{GetUrlPrefix("bibo")}Document", "http://w3id.org/roh/year");
            foreach (string filter in filtersAnios)
            {
                //Inserciones
                while (true)
                {
                    int limitActualizarAnios = 500;
                    String selectActualizarAnios = @"select distinct * where{select ?document ?yearCargado ?yearCargar  ";
                    String whereActualizarAnios = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                OPTIONAL{{
	                                ?document <http://purl.org/dc/terms/issued> ?fechaDoc.
	                                BIND(substr(str(?fechaDoc),0,4) as ?yearCargar).
                                }}
                                OPTIONAL{{
                                    ?document <http://w3id.org/roh/year> ?yearCargado.      
                                }}
                                
                                FILTER(?yearCargado!= ?yearCargar)

                            }}}} limit {limitActualizarAnios}";
                    SparqlObject resultadoActualizarAnios = mResourceApi.VirtuosoQuery(selectActualizarAnios, whereActualizarAnios, "document");

                    Parallel.ForEach(resultadoActualizarAnios.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string yearCargar = "";
                        if (fila.ContainsKey("yearCargar"))
                        {
                            yearCargar = fila["yearCargar"].value;
                        }
                        string yearCargado = "";
                        if (fila.ContainsKey("yearCargado"))
                        {
                            yearCargado = fila["yearCargado"].value;
                        }
                        ActualizadorTriple(document, "http://w3id.org/roh/year", yearCargado, yearCargar);
                    });

                    if (resultadoActualizarAnios.results.bindings.Count != limitActualizarAnios)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/genderIP de los http://purl.org/ontology/bibo/Document 
        /// el gender del primer autor (si esta validado) y '-' si el primer autor no está validado o está validado pero no tiene gender
        /// No tiene dependencias
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarGenderAutorPrincipal(List<string> pDocuments = null)
        {
            HashSet<string> filtersGenderAutorPrincipal = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersGenderAutorPrincipal.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersGenderAutorPrincipal.Count == 0)
            {
                filtersGenderAutorPrincipal.Add("");
            }

            //Eliminamos los duplicados
            EliminarDuplicados("document", $"{GetUrlPrefix("bibo")}Document", "http://w3id.org/roh/genderIP");

            foreach (string filter in filtersGenderAutorPrincipal)
            {
                while (true)
                {
                    int limitGenderAutorPrincipal = 500;
                    String selectGenderAutorPrincipal = @"select ?document ?genderIPCargado ?genderIPACargar ";
                    String whereGenderAutorPrincipal = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                OPTIONAL
                                {{
                                  ?document <http://w3id.org/roh/genderIP> ?genderIPCargado.
                                }}
                                OPTIONAL{{
                                  select ?document ?genderIPACargar
                                  Where{{
                                    ?document a <http://purl.org/ontology/bibo/Document>.                                    
                                    ?author <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                    ?person <http://xmlns.com/foaf/0.1/gender> ?genderIPACargar.
                                    ?person <http://w3id.org/roh/isActive>  'true'.
                                    {{
		                                select ?document  min(?orden) as ?orden sample(?author) as ?author where
		                                {{
			                                select ?document  ?orden ?author where
			                                {{
				                                ?document a <http://purl.org/ontology/bibo/Document>.
				                                ?document <http://purl.org/ontology/bibo/authorList> ?author.
				                                ?author <http://www.w3.org/1999/02/22-rdf-syntax-ns#comment> ?ordenAux.
				                                BIND(xsd:int(?ordenAux) as ?orden)
			                                }}order by asc(?orden) 
		                                }}group by (?document)
	                                }}  
                                  }}
                                }}
                                
                                FILTER(
                                    #Hay que cambiarlo
                                    (BOUND(?genderIPCargado) AND BOUND(?genderIPACargar) AND ?genderIPCargado!= ?genderIPACargar) OR
                                    #Hay que insertarlo
                                    (!BOUND(?genderIPCargado) AND BOUND(?genderIPACargar)) OR
                                    #Hay que eliminarlo
                                    (BOUND(?genderIPCargado) AND !BOUND(?genderIPACargar)) 
                                    )
                            }} limit {limitGenderAutorPrincipal}";
                    SparqlObject resultadoGenderAutorPrincipal = mResourceApi.VirtuosoQueryMultipleGraph(selectGenderAutorPrincipal, whereGenderAutorPrincipal, new List<string>() { "document", "person" });

                    Parallel.ForEach(resultadoGenderAutorPrincipal.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string genderIPACargar = "";
                        if (fila.ContainsKey("genderIPACargar"))
                        {
                            genderIPACargar = fila["genderIPACargar"].value;
                        }
                        string genderIPCargado = "";
                        if (fila.ContainsKey("genderIPCargado"))
                        {
                            genderIPCargado = fila["genderIPCargado"].value;
                        }
                        ActualizadorTriple(document, "http://w3id.org/roh/genderIP", genderIPCargado, genderIPACargar);
                    });

                    if (resultadoGenderAutorPrincipal.results.bindings.Count != limitGenderAutorPrincipal)
                    {
                        break;
                    }
                }

            }
        }

        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/positionIP de los http://purl.org/ontology/bibo/Document 
        /// el position del primer autor (si esta validado) y '-' si el primer autor no está validado o está validado pero no tiene position
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarPositionAutorPrincipal(List<string> pDocuments = null)
        {
            HashSet<string> filtersPositionAutorPrincipal = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersPositionAutorPrincipal.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersPositionAutorPrincipal.Count == 0)
            {
                filtersPositionAutorPrincipal.Add("");
            }

            //Eliminamos los duplicados
            EliminarDuplicados("document", $"{GetUrlPrefix("bibo")}Document", "http://w3id.org/roh/positionIP");

            foreach (string filter in filtersPositionAutorPrincipal)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limitPositionAutorPrincipal = 500;
                    String selectPositionAutorPrincipal = @"select ?document ?positionIPCargado ?positionIPACargar ";
                    String wherePositionAutorPrincipal = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                OPTIONAL
                                {{
                                  ?document <http://w3id.org/roh/positionIP> ?positionIPCargado.
                                }}
                                OPTIONAL{{
                                  select ?document IF (BOUND (?positionIPACargar), ?positionIPACargar, '-' ) as ?positionIPACargar
                                  Where{{
                                    ?document a <http://purl.org/ontology/bibo/Document>.
                                    OPTIONAL{{
                                        ?author <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                        ?person <http://w3id.org/roh/hasPosition> ?positionIPACargar.
                                        ?person <http://w3id.org/roh/isActive>  'true'.
                                        {{
		                                    select ?document  min(?orden) as ?orden sample(?author) as ?author where
		                                    {{
			                                    select ?document  ?orden ?author where
			                                    {{
				                                    ?document a <http://purl.org/ontology/bibo/Document>.
				                                    ?document <http://purl.org/ontology/bibo/authorList> ?author.
				                                    ?author <http://www.w3.org/1999/02/22-rdf-syntax-ns#comment> ?ordenAux.
				                                    BIND(xsd:int(?ordenAux) as ?orden)
			                                    }}order by asc(?orden) 
		                                    }}group by (?document)
	                                    }}                                    
                                    }}

                                  }}
                                }}
                                FILTER(?positionIPCargado!= ?positionIPACargar OR !BOUND(?positionIPCargado) )
                            }} limit {limitPositionAutorPrincipal}";
                    SparqlObject resultadoPositionAutorPrincipal = mResourceApi.VirtuosoQueryMultipleGraph(selectPositionAutorPrincipal, wherePositionAutorPrincipal, new List<string>() { "document", "person" });

                    Parallel.ForEach(resultadoPositionAutorPrincipal.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string positionIPACargar = "";
                        if (fila.ContainsKey("positionIPACargar"))
                        {
                            positionIPACargar = fila["positionIPACargar"].value;
                        }
                        string positionIPCargado = "";
                        if (fila.ContainsKey("positionIPCargado"))
                        {
                            positionIPCargado = fila["positionIPCargado"].value;
                        }
                        ActualizadorTriple(document, "http://w3id.org/roh/positionIP", positionIPCargado, positionIPACargar);
                    });

                    if (resultadoPositionAutorPrincipal.results.bindings.Count != limitPositionAutorPrincipal)
                    {
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Eliminamos los http://purl.org/ontology/bibo/Document que no tengan autores activos
        /// </summary>
        /// <param name="pDocuments">ID de documentos</param>
        public void EliminarDocumentosSinAutoresSGI(List<string> pDocuments = null)
        {
            HashSet<string> filtersDocumentosSinAutoresSGI = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersDocumentosSinAutoresSGI.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersDocumentosSinAutoresSGI.Count == 0)
            {
                filtersDocumentosSinAutoresSGI.Add("");
            }
            foreach (string filter in filtersDocumentosSinAutoresSGI)
            {
                while (true)
                {
                    int limitDocumentosSinAutoresSGI = 500;
                    String selectDocumentosSinAutoresSGI = @"select ?document  ";
                    String whereDocumentosSinAutoresSGI = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                MINUS
                                {{
                                    ?document <http://purl.org/ontology/bibo/authorList> ?autores.
                                    ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                    ?person <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                }}
                            }} limit {limitDocumentosSinAutoresSGI}";
                    SparqlObject resultadoDocumentosSinAutoresSGI = mResourceApi.VirtuosoQueryMultipleGraph(selectDocumentosSinAutoresSGI, whereDocumentosSinAutoresSGI, new List<string>() { "document", "person" });

                    Parallel.ForEach(resultadoDocumentosSinAutoresSGI.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        try
                        {
                            mResourceApi.PersistentDelete(mResourceApi.GetShortGuid(fila["document"].value));
                        }
                        catch (Exception) { }
                    });

                    if (resultadoDocumentosSinAutoresSGI.results.bindings.Count != limitDocumentosSinAutoresSGI)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Modifica los nombres de las revistas desnormalizadas 
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">IDs de los documentos</param>
        public void ModificarNombreRevistaDesnormalizado(List<string> pDocuments = null)
        {
            HashSet<string> filtersNombreRevistaDesnormalizado = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersNombreRevistaDesnormalizado.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersNombreRevistaDesnormalizado.Count == 0)
            {
                filtersNombreRevistaDesnormalizado.Add("");
            }

            foreach (string filter in filtersNombreRevistaDesnormalizado)
            {
                while (true)
                {
                    int limitNombreRevistaDesnormalizado = 500;
                    String selectNombreRevistaDesnormalizado = @$"select *  ";
                    String whereNombreRevistaDesnormalizado = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                OPTIONAL{{?document <http://w3id.org/roh/hasPublicationVenueJournalText> ?nombreDesnormalizadoRevista.}}
                                ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista.
                                ?revista <http://w3id.org/roh/title> ?nombreRevista.
                                FILTER(?nombreDesnormalizadoRevista!=?nombreRevista)
                            }}limit {limitNombreRevistaDesnormalizado}";
                    SparqlObject resultadoNombreRevistaDesnormalizado = mResourceApi.VirtuosoQueryMultipleGraph(selectNombreRevistaDesnormalizado, whereNombreRevistaDesnormalizado, new List<string>() { "document", "maindocument" });
                    Parallel.ForEach(resultadoNombreRevistaDesnormalizado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        //Entidad principal
                        string mainEntity = fila["document"].value;
                        //Predicado
                        string predicado = "http://w3id.org/roh/hasPublicationVenueJournalText";
                        //Valor antiguo
                        string valorAntiguo = "";
                        if (fila.ContainsKey("nombreDesnormalizadoRevista"))
                        {
                            valorAntiguo = fila["nombreDesnormalizadoRevista"].value;
                        }
                        //Valor nuevo
                        string valorNuevo = fila["nombreRevista"].value;
                        ActualizadorTriple(mainEntity, predicado, valorAntiguo, valorNuevo);
                    });
                    if (resultadoNombreRevistaDesnormalizado.results.bindings.Count != limitNombreRevistaDesnormalizado)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Modifica los nombres de las editoriales de las revistas desnormalizadas 
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">IDs de los documentos</param>
        public void ModificarEditorialRevistaDesnormalizado(List<string> pDocuments = null)
        {
            HashSet<string> filtersEditorialRevistaDesnormalizado = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersEditorialRevistaDesnormalizado.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersEditorialRevistaDesnormalizado.Count == 0)
            {
                filtersEditorialRevistaDesnormalizado.Add("");
            }

            foreach (string filter in filtersEditorialRevistaDesnormalizado)
            {
                while (true)
                {
                    int limitEditorialRevistaDesnormalizado = 500;
                    String selectEditorialRevistaDesnormalizado = @$"select * ";
                    String whereEditorialRevistaDesnormalizado = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                OPTIONAL{{?document <http://purl.org/ontology/bibo/publisher> ?nombreDesnormalizadoEditorial.}}
                                ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista.
                                ?revista <http://purl.org/ontology/bibo/editor> ?nombreEditorial.
                                FILTER(?nombreDesnormalizadoEditorial!=?nombreEditorial)
                            }}limit {limitEditorialRevistaDesnormalizado}";
                    SparqlObject resultadoEditorialRevistaDesnormalizado = mResourceApi.VirtuosoQueryMultipleGraph(selectEditorialRevistaDesnormalizado, whereEditorialRevistaDesnormalizado, new List<string>() { "document", "maindocument" });
                    Parallel.ForEach(resultadoEditorialRevistaDesnormalizado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        //Entidad principal
                        string mainEntity = fila["document"].value;
                        //Predicado
                        string predicado = "http://purl.org/ontology/bibo/publisher";
                        //Valor antiguo
                        string valorAntiguo = "";
                        if (fila.ContainsKey("nombreDesnormalizadoEditorial"))
                        {
                            valorAntiguo = fila["nombreDesnormalizadoEditorial"].value;
                        }
                        //Valor nuevo
                        string valorNuevo = fila["nombreEditorial"].value;
                        ActualizadorTriple(mainEntity, predicado, valorAntiguo, valorNuevo);
                    });
                    if (resultadoEditorialRevistaDesnormalizado.results.bindings.Count != limitEditorialRevistaDesnormalizado)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Modifica los ISSN de las revistas desnormalizadas 
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">IDs de los documentos</param>
        public void ModificarISSNRevistaDesnormalizado(List<string> pDocuments = null)
        {
            HashSet<string> filtersISSNRevistaDesnormalizado = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersISSNRevistaDesnormalizado.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersISSNRevistaDesnormalizado.Count == 0)
            {
                filtersISSNRevistaDesnormalizado.Add("");
            }

            foreach (string filter in filtersISSNRevistaDesnormalizado)
            {
                while (true)
                {
                    int limitISSNRevistaDesnormalizado = 500;
                    String selectISSNRevistaDesnormalizado = @$"select * ";
                    String whereISSNRevistaDesnormalizado = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                OPTIONAL{{?document <http://purl.org/ontology/bibo/issn> ?issnDesnormalizado.}}
                                ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista.
                                ?revista <http://purl.org/ontology/bibo/issn> ?issn.
                                FILTER(?issnDesnormalizado!=?issn)
                            }}limit {limitISSNRevistaDesnormalizado}";
                    SparqlObject resultadoISSNRevistaDesnormalizado = mResourceApi.VirtuosoQueryMultipleGraph(selectISSNRevistaDesnormalizado, whereISSNRevistaDesnormalizado, new List<string>() { "document", "maindocument" });
                    Parallel.ForEach(resultadoISSNRevistaDesnormalizado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        //Entidad principal
                        string mainEntity = fila["document"].value;
                        //Predicado
                        string predicado = "http://purl.org/ontology/bibo/issn";
                        //Valor antiguo
                        string valorAntiguo = "";
                        if (fila.ContainsKey("issnDesnormalizado"))
                        {
                            valorAntiguo = fila["issnDesnormalizado"].value;
                        }
                        //Valor nuevo
                        string valorNuevo = fila["issn"].value;
                        ActualizadorTriple(mainEntity, predicado, valorAntiguo, valorNuevo);
                    });
                    if (resultadoISSNRevistaDesnormalizado.results.bindings.Count != limitISSNRevistaDesnormalizado)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza indices de impacto
        /// </summary>
        /// <param name="pDocuments">IDs de documentos</param>
        public void ActualizarIndicesImpacto(List<string> pDocuments = null)
        {
            HashSet<string> filtersIndicesImpacto = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersIndicesImpacto.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersIndicesImpacto.Count == 0)
            {
                filtersIndicesImpacto.Add("");
            }

            foreach (string filter in filtersIndicesImpacto)
            {
                //Insertamos las auxiliares http://w3id.org/roh/ImpactIndex con categorías
                while (true)
                {
                    int limitInsertAuxImpactIndex = 500;

                    String selectInsertAuxImpactIndex = @"select distinct ?document ?impactSource ?categoryTitle ?impactCategory ?impactIndexInYear ";
                    String whereInsertAuxImpactIndex = @$"where{{          
    ?document a <http://purl.org/ontology/bibo/Document>.
    {filter}
    {{
        #Deseables
        ?document <http://purl.org/dc/terms/issued> ?fechaDoc.
        ?document<http://w3id.org/roh/year> ?anio.
        ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista. 
        ?revista <http://w3id.org/roh/impactIndex> ?impactIndex.
        ?impactIndex <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYear.
        ?impactIndex <http://w3id.org/roh/year> ?anio.     
        ?impactIndex  <http://w3id.org/roh/impactSource> ?impactSource.          
        ?impactIndex <http://w3id.org/roh/impactCategory> ?impactCategory .     
        ?impactCategory <http://w3id.org/roh/title> ?categoryTitle.

        #Doc
		?document <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaDoc.
		?hasKnowledgeAreaDoc <http://w3id.org/roh/categoryNode> ?categoryNode.
		?categoryNode <http://w3id.org/roh/sourceDescriptor> ?sourceDescriptor.
        ?sourceDescriptor <http://w3id.org/roh/impactSource> ?impactSource.
		?sourceDescriptor <http://w3id.org/roh/impactSourceCategory> ?titleCategoryCat.
		FILTER(lcase(?titleCategoryCat)=lcase(?categoryTitle))

    }}MINUS
    {{
        #Actuales
        ?document <http://w3id.org/roh/impactIndex> ?impactIndexDoc.        
        ?impactIndexDoc  <http://w3id.org/roh/impactSource> ?impactSource.
        ?impactIndexDoc <http://w3id.org/roh/impactIndexCategory> ?categoryTitle.
        ?impactIndexDoc <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYear.
    }}
}}order by (?document) limit {limitInsertAuxImpactIndex}";
                    SparqlObject resultadoInsertAuxImpactIndex = mResourceApi.VirtuosoQueryMultipleGraph(selectInsertAuxImpactIndex, whereInsertAuxImpactIndex, new List<string>() { "document", "maindocument", "taxonomy" });

                    InsercionIndicesImpacto(resultadoInsertAuxImpactIndex.results.bindings);

                    if (resultadoInsertAuxImpactIndex.results.bindings.Count != limitInsertAuxImpactIndex)
                    {
                        break;
                    }
                }

                //Eliminamos las auxiliares http://w3id.org/roh/ImpactIndex con categorías
                while (true)
                {
                    int limit = 500;

                    String selectDeleteAuxImpactIndex = @"select distinct ?document ?impactIndexDoc ";
                    String whereDeleteAuxImpactIndex = @$"where{{   
    ?document a <http://purl.org/ontology/bibo/Document>.
    {filter}
    {{
        #Actuales
        ?document <http://w3id.org/roh/impactIndex> ?impactIndexDoc.    
        ?impactIndexDoc  <http://w3id.org/roh/impactSource> ?impactSource.
        ?impactIndexDoc <http://w3id.org/roh/impactIndexCategoryEntity> ?impactCategoryAux.
        BIND(uri(?impactCategoryAux) as ?impactCategory)
    }}MINUS
    {{
        #Deseables
        ?document <http://purl.org/dc/terms/issued> ?fechaDoc.
        ?document<http://w3id.org/roh/year> ?anio.
        ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista. 
        ?revista <http://w3id.org/roh/impactIndex> ?impactIndex.
        ?impactIndex <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYear.
        ?impactIndex <http://w3id.org/roh/year> ?anio.     
        ?impactIndex  <http://w3id.org/roh/impactSource> ?impactSource.          
        ?impactIndex <http://w3id.org/roh/impactCategory> ?impactCategory .     
        ?impactCategory <http://w3id.org/roh/title> ?categoryTitle.

        #Doc
		?document <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaDoc.
		?hasKnowledgeAreaDoc <http://w3id.org/roh/categoryNode> ?categoryNode.
		?categoryNode <http://w3id.org/roh/sourceDescriptor> ?sourceDescriptor.
        ?sourceDescriptor <http://w3id.org/roh/impactSource> ?impactSource.
		?sourceDescriptor <http://w3id.org/roh/impactSourceCategory> ?titleCategoryCat.
		FILTER(lcase(?titleCategoryCat)=lcase(?categoryTitle))

    }}
    
}}order by (?document) limit {limit}";

                    SparqlObject resultadoDeleteAuxImpactIndex = mResourceApi.VirtuosoQueryMultipleGraph(selectDeleteAuxImpactIndex, whereDeleteAuxImpactIndex, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultadoDeleteAuxImpactIndex.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            Guid guid = mResourceApi.GetShortGuid(id);

                            Dictionary<Guid, List<RemoveTriples>> triples = new() { { guid, new List<RemoveTriples>() } };
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoDeleteAuxImpactIndex.results.bindings.Where(x => x["document"].value == id))
                            {
                                string indiceImpactoAEliminar = fila["impactIndexDoc"].value;
                                RemoveTriples t = new();
                                t.Predicate = "http://w3id.org/roh/impactIndex";
                                t.Value = indiceImpactoAEliminar;
                                triples[guid].Add(t);
                            }
                            if (triples[guid].Count > 0)
                            {
                                var resultado = mResourceApi.DeletePropertiesLoadedResources(triples);
                            }
                        });
                    }

                    if (resultadoDeleteAuxImpactIndex.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Completamos http://w3id.org/roh/ImpactIndex con http://w3id.org/roh/publicationPosition
                while (true)
                {
                    int limitPublicationPosition = 500;

                    String selectPublicationPosition = @"select distinct ?document ?impactIndexDoc ?publicationPositionCargar ?publicationPositionCargado  ";
                    String wherePublicationPosition = @$"where{{
    ?document a <http://purl.org/ontology/bibo/Document>. 
	?document <http://w3id.org/roh/impactIndex> ?impactIndexDoc.	
    ?impactIndexDoc <http://w3id.org/roh/impactIndexCategoryEntity> ?impactCategoryAux.
    BIND(uri(?impactCategoryAux) as ?impactCategory)
    {filter}
    OPTIONAL{{
        ?impactCategory <http://w3id.org/roh/publicationPosition> ?publicationPositionCargar.      
    }} 
    OPTIONAL{{
        ?impactIndexDoc <http://w3id.org/roh/publicationPosition> ?publicationPositionCargado.
    }}  
    FILTER(?publicationPositionCargado!= ?publicationPositionCargar OR (!BOUND(?publicationPositionCargado) AND BOUND(?publicationPositionCargar)) OR (!BOUND(?publicationPositionCargar) AND BOUND(?publicationPositionCargado)))
}} limit {limitPublicationPosition}";

                    SparqlObject resultadoPublicationPosition = mResourceApi.VirtuosoQueryMultipleGraph(selectPublicationPosition, wherePublicationPosition, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultadoPublicationPosition.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoPublicationPosition.results.bindings.Where(x => x["document"].value == id))
                            {
                                string document = fila["document"].value;
                                string impactIndexDoc = fila["impactIndexDoc"].value;
                                string publicationPositionCargar = "";
                                if (fila.ContainsKey("publicationPositionCargar"))
                                {
                                    publicationPositionCargar = impactIndexDoc + "|" + fila["publicationPositionCargar"].value;
                                }
                                string publicationPositionCargado = "";
                                if (fila.ContainsKey("publicationPositionCargado"))
                                {
                                    publicationPositionCargado = impactIndexDoc + "|" + fila["publicationPositionCargado"].value;
                                }
                                ActualizadorTriple(document, "http://w3id.org/roh/impactIndex|http://w3id.org/roh/publicationPosition", publicationPositionCargado, publicationPositionCargar);
                            }
                        });
                    }

                    if (resultadoPublicationPosition.results.bindings.Count != limitPublicationPosition)
                    {
                        break;
                    }
                }

                //Completamos http://w3id.org/roh/ImpactIndex con http://w3id.org/roh/journalNumberInCat
                while (true)
                {
                    int limitJournalNumberInCat = 500;

                    String selectJournalNumberInCat = @"select distinct ?document ?impactIndexDoc ?journalNumberInCatCargar ?journalNumberInCatCargado ";
                    String whereJournalNumberInCat = @$"where{{
    ?document a <http://purl.org/ontology/bibo/Document>. 
	?document <http://w3id.org/roh/impactIndex> ?impactIndexDoc.	
    ?impactIndexDoc <http://w3id.org/roh/impactIndexCategoryEntity> ?impactCategoryAux.
    BIND(uri(?impactCategoryAux) as ?impactCategory)
    {filter}
	OPTIONAL{{
		?impactCategory <http://w3id.org/roh/journalNumberInCat> ?journalNumberInCatCargar.      
	}}  
	OPTIONAL{{
		?impactIndexDoc <http://w3id.org/roh/journalNumberInCat> ?journalNumberInCatCargado.
	}}  
    FILTER(?journalNumberInCatCargado!= ?journalNumberInCatCargar OR (!BOUND(?journalNumberInCatCargado) AND BOUND(?journalNumberInCatCargar)) OR (!BOUND(?journalNumberInCatCargar) AND BOUND(?journalNumberInCatCargado)))
}} limit {limitJournalNumberInCat}";

                    SparqlObject resultadoJournalNumberInCat = mResourceApi.VirtuosoQueryMultipleGraph(selectJournalNumberInCat, whereJournalNumberInCat, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultadoJournalNumberInCat.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoJournalNumberInCat.results.bindings.Where(x => x["document"].value == id))
                            {
                                string document = fila["document"].value;
                                string impactIndexDoc = fila["impactIndexDoc"].value;
                                string journalNumberInCatCargar = "";
                                if (fila.ContainsKey("journalNumberInCatCargar"))
                                {
                                    journalNumberInCatCargar = impactIndexDoc + "|" + fila["journalNumberInCatCargar"].value;
                                }
                                string journalNumberInCatCargado = "";
                                if (fila.ContainsKey("journalNumberInCatCargado"))
                                {
                                    journalNumberInCatCargado = impactIndexDoc + "|" + fila["journalNumberInCatCargado"].value;
                                }
                                ActualizadorTriple(document, "http://w3id.org/roh/impactIndex|http://w3id.org/roh/journalNumberInCat", journalNumberInCatCargado, journalNumberInCatCargar);
                            }
                        });
                    }

                    if (resultadoJournalNumberInCat.results.bindings.Count != limitJournalNumberInCat)
                    {
                        break;
                    }
                }

                //Completamos http://w3id.org/roh/ImpactIndex con http://w3id.org/roh/quartile
                while (true)
                {
                    int limitQuartile = 500;

                    String selectQuartile = @"select distinct ?document ?impactIndexDoc ?quartileCargar ?quartileCargado ";
                    String whereQuartile = @$"where{{
    ?document a <http://purl.org/ontology/bibo/Document>. 
	?document <http://w3id.org/roh/impactIndex> ?impactIndexDoc.	
    ?impactIndexDoc <http://w3id.org/roh/impactIndexCategoryEntity> ?impactCategoryAux.
    BIND(uri(?impactCategoryAux) as ?impactCategory)
	OPTIONAL{{
		?impactCategory <http://w3id.org/roh/quartile> ?quartileCargar.      
	}}  
	OPTIONAL{{
		?impactIndexDoc <http://w3id.org/roh/quartile> ?quartileCargado.
	}}  
    FILTER(?quartileCargado!= ?quartileCargar OR (!BOUND(?quartileCargado) AND BOUND(?quartileCargar)) OR (!BOUND(?quartileCargar) AND BOUND(?quartileCargado)))
}} limit {limitQuartile}";

                    SparqlObject resultadoQuartile = mResourceApi.VirtuosoQueryMultipleGraph(selectQuartile, whereQuartile, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultadoQuartile.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuartile.results.bindings.Where(x => x["document"].value == id))
                            {
                                string document = fila["document"].value;
                                string impactIndexDoc = fila["impactIndexDoc"].value;
                                string quartileCargar = "";
                                if (fila.ContainsKey("quartileCargar"))
                                {
                                    quartileCargar = impactIndexDoc + "|" + fila["quartileCargar"].value;
                                }
                                string quartileCargado = "";
                                if (fila.ContainsKey("quartileCargado"))
                                {
                                    quartileCargado = impactIndexDoc + "|" + fila["quartileCargado"].value;
                                }
                                ActualizadorTriple(document, "http://w3id.org/roh/impactIndex|http://w3id.org/roh/quartile", quartileCargado, quartileCargar);
                            }
                        });
                    }

                    if (resultadoQuartile.results.bindings.Count != limitQuartile)
                    {
                        break;
                    }
                }

                //Insertamos las auxiliares http://w3id.org/roh/ImpactIndex sin categorías
                while (true)
                {
                    int limitImpactIndexSinCat = 500;

                    String selectImpactIndexSinCat = @"select distinct ?document ?impactSource ?impactIndexInYear ";
                    String whereImpactIndexSinCat = @$"where{{          
    ?document a <http://purl.org/ontology/bibo/Document>.
    {filter}
    {{
        #Deseables
        ?document <http://purl.org/dc/terms/issued> ?fechaDoc.
        ?document<http://w3id.org/roh/year> ?anio.
        ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista. 
        ?revista <http://w3id.org/roh/impactIndex> ?impactIndex.
        ?impactIndex <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYear.
        ?impactIndex <http://w3id.org/roh/year> ?anio.     
        ?impactIndex  <http://w3id.org/roh/impactSource> ?impactSource.    
    }}MINUS
    {{
        #Actuales
        ?document <http://w3id.org/roh/impactIndex> ?impactIndexDoc.        
        ?impactIndexDoc  <http://w3id.org/roh/impactSource> ?impactSource.
        ?impactIndexDoc <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYear.
    }}
}}order by (?document) limit {limitImpactIndexSinCat}";
                    SparqlObject resultadoImpactIndexSinCat = mResourceApi.VirtuosoQueryMultipleGraph(selectImpactIndexSinCat, whereImpactIndexSinCat, new List<string>() { "document", "maindocument", "taxonomy" });

                    InsercionIndicesImpacto(resultadoImpactIndexSinCat.results.bindings);

                    if (resultadoImpactIndexSinCat.results.bindings.Count != limitImpactIndexSinCat)
                    {
                        break;
                    }
                }

                //Eliminamos las auxiliares http://w3id.org/roh/ImpactIndex sin categorías
                while (true)
                {
                    int limitEliminarImpactIndexSinCat = 500;

                    String selectEliminarImpactIndexSinCat = @"select ?document ?impactIndexDoc ";
                    String whereEliminarImpactIndexSinCat = @$"where{{          
    ?document a <http://purl.org/ontology/bibo/Document>.
    {filter}
    {{
        #Actuales
        ?document <http://w3id.org/roh/impactIndex> ?impactIndexDoc.        
        ?impactIndexDoc  <http://w3id.org/roh/impactSource> ?impactSource.
        ?impactIndexDoc <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYear.
        MINUS{{?impactIndexDoc  <http://w3id.org/roh/impactIndexCategoryEntity> ?impactIndexCategoryEntity}}
    }}MINUS
    {{
        #Deseables
        ?document <http://purl.org/dc/terms/issued> ?fechaDoc.
        ?document<http://w3id.org/roh/year> ?anio.
        ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista. 
        ?revista <http://w3id.org/roh/impactIndex> ?impactIndex.
        ?impactIndex <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYear.
        ?impactIndex <http://w3id.org/roh/year> ?anio.     
        ?impactIndex  <http://w3id.org/roh/impactSource> ?impactSource.   
    }}
}}order by (?document) limit {limitEliminarImpactIndexSinCat}";

                    SparqlObject resultadoEliminarImpactIndexSinCat = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarImpactIndexSinCat, whereEliminarImpactIndexSinCat, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultadoEliminarImpactIndexSinCat.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            Guid guid = mResourceApi.GetShortGuid(id);

                            Dictionary<Guid, List<RemoveTriples>> triples = new() { { guid, new List<RemoveTriples>() } };
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoEliminarImpactIndexSinCat.results.bindings.Where(x => x["document"].value == id))
                            {
                                string indiceImpactoAEliminar = fila["impactIndexDoc"].value;
                                RemoveTriples t = new();
                                t.Predicate = "http://w3id.org/roh/impactIndex";
                                t.Value = indiceImpactoAEliminar;
                                triples[guid].Add(t);
                            }
                            if (triples[guid].Count > 0)
                            {
                                var resultado = mResourceApi.DeletePropertiesLoadedResources(triples);
                            }
                        });
                    }

                    if (resultadoEliminarImpactIndexSinCat.results.bindings.Count != limitEliminarImpactIndexSinCat)
                    {
                        break;
                    }
                }

                //Actualizamos cuartiles del doc
                //Eliminamos los duplicados
                EliminarDuplicados("document", $"{GetUrlPrefix("bibo")}Document", "http://w3id.org/roh/quartile");
                while (true)
                {
                    int limitActualizarQuartil = 500;
                    String selectActualizarQuartil = @"select distinct ?document ?quartileCargado ?quartileCargar ";
                    String whereActualizarQuartil = @$"where{{
                                    ?document a <http://purl.org/ontology/bibo/Document>.
                                    {filter}
                                    OPTIONAL
                                    {{
                                        ?document <http://w3id.org/roh/quartile> ?quartileCargadoAux. 
                                        BIND(xsd:int(?quartileCargadoAux) as ?quartileCargado)
                                    }}
                                    OPTIONAL
                                    {{
                                      select ?document min(xsd:int(?quartile)) as ?quartileCargar
                                      Where{{
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        ?document <http://w3id.org/roh/impactIndex> ?impactIndexDoc.	
                                        ?impactIndexDoc <http://w3id.org/roh/quartile> ?quartile.
                                      }}
                                    }}
                                    FILTER(?quartileCargado!= ?quartileCargar OR (!BOUND(?quartileCargado) AND BOUND(?quartileCargar)) OR (!BOUND(?quartileCargar) AND BOUND(?quartileCargado)))
                                    }} limit {limitActualizarQuartil}";
                    SparqlObject resultadoActualizarQuartil = mResourceApi.VirtuosoQuery(selectActualizarQuartil, whereActualizarQuartil, "document");

                    Parallel.ForEach(resultadoActualizarQuartil.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string quartileCargar = "";
                        if (fila.ContainsKey("quartileCargar"))
                        {
                            quartileCargar = fila["quartileCargar"].value;
                        }
                        string quartileCargado = "";
                        if (fila.ContainsKey("quartileCargado"))
                        {
                            quartileCargado = fila["quartileCargado"].value;
                        }
                        ActualizadorTriple(document, $"{GetUrlPrefix("roh")}quartile", quartileCargado, quartileCargar);
                    });

                    if (resultadoActualizarQuartil.results.bindings.Count != limitActualizarQuartil)
                    {
                        break;
                    }
                }

                //Actualizamos el indice de impacto del doc
                //Eliminamos los duplicados
                EliminarDuplicados("document", $"{GetUrlPrefix("bibo")}Document", $"{GetUrlPrefix("roh")}impactIndexInYear");
                while (true)
                {
                    int limitActualizarImpactDoc = 500;
                    String selectActualizarImpactDoc = @"select distinct ?document ?impactIndexInYearCargado ?impactIndexInYearCargar ";
                    String whereActualizarImpactDoc = @$"where{{
    ?document a <http://purl.org/ontology/bibo/Document>.
    {filter}
    OPTIONAL
    {{
        ?document <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYearCargado. 
    }}
    OPTIONAL
    {{
        select ?document min(?impactIndexInYear) as ?impactIndexInYearCargar
        Where{{
            ?document a <http://purl.org/ontology/bibo/Document>.
            ?document <http://w3id.org/roh/impactIndex> ?impactIndexDoc.	
            ?impactIndexDoc <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYear.
        }}
    }}
    FILTER(?impactIndexInYearCargado!= ?impactIndexInYearCargar OR (!BOUND(?impactIndexInYearCargado) AND BOUND(?impactIndexInYearCargar)) OR (!BOUND(?impactIndexInYearCargar) AND BOUND(?impactIndexInYearCargado)))
}} limit {limitActualizarImpactDoc}";
                    SparqlObject resultadoActualizarImpactDoc = mResourceApi.VirtuosoQuery(selectActualizarImpactDoc, whereActualizarImpactDoc, "document");

                    Parallel.ForEach(resultadoActualizarImpactDoc.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string impactIndexInYearCargar = "";
                        if (fila.ContainsKey("impactIndexInYearCargar"))
                        {
                            impactIndexInYearCargar = fila["impactIndexInYearCargar"].value;
                        }
                        string impactIndexInYearCargado = "";
                        if (fila.ContainsKey("impactIndexInYearCargado"))
                        {
                            impactIndexInYearCargado = fila["impactIndexInYearCargado"].value;
                        }
                        ActualizadorTriple(document, "http://w3id.org/roh/impactIndexInYear", impactIndexInYearCargado, impactIndexInYearCargar);
                    });

                    if (resultadoActualizarImpactDoc.results.bindings.Count != limitActualizarImpactDoc)
                    {
                        break;
                    }
                }

            }
        }

        // <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/linkedCount de los http://purl.org/ontology/bibo/Document
        /// el nº de recursos relacionados públicos de una publicación
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">IDs de documentos</param>
        public void ActualizarNumeroVinculados(List<string> pDocuments = null)
        {
            HashSet<string> filtersNumeroVinculados = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersNumeroVinculados.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersNumeroVinculados.Count == 0)
            {
                filtersNumeroVinculados.Add("");
            }

            //Eliminamos los duplicados
            EliminarDuplicados("document", $"{GetUrlPrefix("bibo")}Document", "http://w3id.org/roh/linkedCount");

            foreach (string filter in filtersNumeroVinculados)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limitNumeroVinculados = 500;
                    String selectNumeroVinculados = @"select ?document ?numLinkedCargados IF (BOUND (?numLinkedACargar), ?numLinkedACargar, 0 ) as ?numLinkedACargar ";
                    String whereNumeroVinculados = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                OPTIONAL
                                {{
                                  ?document <http://w3id.org/roh/linkedCount> ?numLinkedCargadosAux. 
                                  BIND(xsd:int( ?numLinkedCargadosAux) as  ?numLinkedCargados)
                                }}
                                OPTIONAL{{
                                  select ?document count(distinct(?linkedID)) as ?numLinkedACargar
                                  Where{{                                    
                                    ?document a <http://purl.org/ontology/bibo/Document>.
                                    OPTIONAL
                                    {{
                                        {{
                                            ?document ?linked ?linkedID.
                                            Filter (?linked in (<http://w3id.org/roh/linkedDocument>, <http://w3id.org/roh/linkedRO>))
                                            ?linkedID <http://w3id.org/roh/isValidated> 'true'.                              
                                        }}UNION
                                        {{
                                            ?linkedID ?linked ?document. 
                                            Filter (?linked in (<http://w3id.org/roh/linkedDocument>, <http://w3id.org/roh/linkedRO>))
                                            ?linkedID <http://w3id.org/roh/isValidated> 'true'.                     
                                        }}
                                    }}
                                  }}
                                }}
                                FILTER(?numLinkedCargados!= ?numLinkedACargar OR !BOUND(?numLinkedCargados) )
                            }} limit {limitNumeroVinculados}";
                    SparqlObject resultadoNumeroVinculados = mResourceApi.VirtuosoQueryMultipleGraph(selectNumeroVinculados, whereNumeroVinculados, new List<string>() { "document", "researchobject" });

                    Parallel.ForEach(resultadoNumeroVinculados.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string numLinkedACargar = fila["numLinkedACargar"].value;
                        string numLinkedCargados = "";
                        if (fila.ContainsKey("numLinkedCargados"))
                        {
                            numLinkedCargados = fila["numLinkedCargados"].value;
                        }
                        ActualizadorTriple(document, "http://w3id.org/roh/linkedCount", numLinkedCargados, numLinkedACargar);
                    });

                    if (resultadoNumeroVinculados.results.bindings.Count != limitNumeroVinculados)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Método para inserción múltiple de indices de impacto
        /// </summary>
        /// <param name="pFilas">Filas con los datos para insertar</param>
        public void InsercionIndicesImpacto(List<Dictionary<string, SparqlObject.Data>> pFilas)
        {
            List<string> idsInsercionIndicesImpacto = pFilas.Select(x => x["document"].value).Distinct().ToList();
            if (idsInsercionIndicesImpacto.Count > 0)
            {
                Parallel.ForEach(idsInsercionIndicesImpacto, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idInsercionIndicesImpacto =>
                {
                    Guid guid = mResourceApi.GetShortGuid(idInsercionIndicesImpacto);

                    Dictionary<Guid, List<TriplesToInclude>> triples = new() { { guid, new List<TriplesToInclude>() } };
                    foreach (Dictionary<string, SparqlObject.Data> fila in pFilas.Where(x => x["document"].value == idInsercionIndicesImpacto))
                    {
                        string idAux = mResourceApi.GraphsUrl + "items/ImpactIndex_" + guid.ToString().ToLower() + "_" + Guid.NewGuid().ToString().ToLower();
                        string document = fila["document"].value;
                        
                        TriplesToInclude t = new();
                        t.Predicate = $"{GetUrlPrefix("roh")}impactIndex|http://w3id.org/roh/impactIndexInYear";
                        t.NewValue = idAux + "|" + fila["impactIndexInYear"].value;
                        triples[guid].Add(t);
                        
                        if (fila.ContainsKey("impactSource"))
                        {
                            TriplesToInclude timpactSource = new();
                            timpactSource.Predicate = $"{GetUrlPrefix("roh")}impactIndex|http://w3id.org/roh/impactSource";
                            timpactSource.NewValue = idAux + "|" + fila["impactSource"].value;
                            triples[guid].Add(timpactSource);
                        }
                        if (fila.ContainsKey("impactSourceOther"))
                        {
                            TriplesToInclude timpactSourceOther = new();
                            timpactSourceOther.Predicate = $"{GetUrlPrefix("roh")}impactIndex|http://w3id.org/roh/impactSourceOther";
                            timpactSourceOther.NewValue = idAux + "|" + fila["impactSourceOther"].value;
                            triples[guid].Add(timpactSourceOther);
                        }
                        if (fila.ContainsKey("categoryTitle"))
                        {
                            TriplesToInclude tcategoryTitle = new();
                            tcategoryTitle.Predicate = $"{GetUrlPrefix("roh")}impactIndex|http://w3id.org/roh/impactIndexCategory";
                            tcategoryTitle.NewValue = idAux + "|" + fila["categoryTitle"].value;
                            triples[guid].Add(tcategoryTitle);
                        }
                        if (fila.ContainsKey("publicationPosition"))
                        {
                            TriplesToInclude tpublicationPosition = new();
                            tpublicationPosition.Predicate = $"{GetUrlPrefix("roh")}impactIndex|http://w3id.org/roh/publicationPosition";
                            tpublicationPosition.NewValue = idAux + "|" + fila["publicationPosition"].value;
                            triples[guid].Add(tpublicationPosition);
                        }
                        if (fila.ContainsKey("journalNumberInCat"))
                        {
                            TriplesToInclude tjournalNumberInCat = new();
                            tjournalNumberInCat.Predicate = $"{GetUrlPrefix("roh")}impactIndex|http://w3id.org/roh/journalNumberInCat";
                            tjournalNumberInCat.NewValue = idAux + "|" + fila["journalNumberInCat"].value;
                            triples[guid].Add(tjournalNumberInCat);
                        }
                        if (fila.ContainsKey("quartile"))
                        {
                            TriplesToInclude tquartile = new();
                            tquartile.Predicate = $"{GetUrlPrefix("roh")}impactIndex|http://w3id.org/roh/quartile";
                            tquartile.NewValue = idAux + "|" + fila["quartile"].value;
                            triples[guid].Add(tquartile);
                        }
                        if (fila.ContainsKey("impactCategory"))
                        {
                            TriplesToInclude timpactCategory = new();
                            timpactCategory.Predicate = $"{GetUrlPrefix("roh")}impactIndex|http://w3id.org/roh/impactIndexCategoryEntity";
                            timpactCategory.NewValue = idAux + "|" + fila["impactCategory"].value;
                            triples[guid].Add(timpactCategory);
                        }
                    }
                    if (triples[guid].Count > 0)
                    {
                        var resultado = mResourceApi.InsertPropertiesLoadedResources(triples);
                    }
                });
            }
        }
    }
}
