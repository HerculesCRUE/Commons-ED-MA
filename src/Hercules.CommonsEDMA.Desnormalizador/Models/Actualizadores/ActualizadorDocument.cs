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
    //TODO comentarios completados

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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }
            //Eliminamos los duplicados
            EliminarDuplicados("document", "http://purl.org/ontology/bibo/Document", "http://w3id.org/roh/isValidated");

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?document ?isValidatedCargado ?isValidatedCargar";
                    String where = @$"where{{
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
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "document");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string isValidatedCargar = fila["isValidatedCargar"].value;
                        string isValidatedCargado = "";
                        if (fila.ContainsKey("isValidatedCargado"))
                        {
                            isValidatedCargado = fila["isValidatedCargado"].value;
                        }
                        ActualizadorTriple(document, "http://w3id.org/roh/isValidated", isValidatedCargado, isValidatedCargar);
                    });

                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filters.Add($" FILTER(?grupo in (<{string.Join(">,<", pGroups)}>))");
            }
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?doc in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    //Añadimos a documentos
                    int limit = 500;
                    String select = @"select distinct ?doc ?grupo  ";
                    String where = @$"where{{
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
                                }}order by desc(?doc) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "curriculumvitae", "person", "group" });
                    InsercionMultiple(resultado.results.bindings, "http://w3id.org/roh/isProducedBy", "doc", "grupo");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Eliminamos de documentos
                    int limit = 500;
                    String select = @"select distinct ?doc ?grupo  ";
                    String where = @$"where{{
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
                                }}order by desc(?doc) limit {limit}";
                    var resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "curriculumvitae", "person", "group" });
                    EliminacionMultiple(resultado.results.bindings, "http://w3id.org/roh/isProducedBy", "doc", "grupo");
                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            //Eliminamos los duplicados
            EliminarDuplicados("document", "http://purl.org/ontology/bibo/Document", "http://w3id.org/roh/citationCount");

            foreach (string filter in filters)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?document ?numCitasCargadas IF (BOUND (?numCitasACargar), ?numCitasACargar, 0 ) as ?numCitasACargar";
                    String where = @$"where{{
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
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "document");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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

                    if (resultado.results.bindings.Count != limit)
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

            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                //Eliminamos las categorías duplicadas
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?document ?categoryNode ";
                    String where = @$"where{{
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
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "taxonomy" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string categoryNode = fila["categoryNode"].value;
                        select = @"select ?document ?hasKnowledgeArea ?categoryNode ";
                        where = @$"where{{
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
                        resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "taxonomy" });
                        List<RemoveTriples> triplesRemove = new();
                        foreach (string hasKnowledgeArea in resultado.results.bindings.GetRange(1, resultado.results.bindings.Count - 1).Select(x => x["hasKnowledgeArea"].value).ToList())
                        {
                            triplesRemove.Add(new RemoveTriples()
                            {
                                Predicate = "http://w3id.org/roh/hasKnowledgeArea",
                                Value = hasKnowledgeArea
                            }); ;
                        }
                        if (triplesRemove.Count > 0)
                        {
                            var resultadox = mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.RemoveTriples>>() { { mResourceApi.GetShortGuid(document), triplesRemove } });
                        }
                    });


                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }



                //Cargamos el tesauro
                Dictionary<string, string> dicAreasBroader = new();
                {
                    String select = @"select distinct * ";
                    String where = @$"where{{
                                ?concept a <http://www.w3.org/2008/05/skos#Concept>.
                                ?concept <http://purl.org/dc/elements/1.1/source> 'researcharea'
                                OPTIONAL{{?concept <http://www.w3.org/2008/05/skos#broader> ?broader}}
                            }}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "taxonomy");

                    foreach (Dictionary<string, SparqlObject.Data> fila in resultado.results.bindings)
                    {
                        string concept = fila["concept"].value;
                        string broader = "";
                        if (fila.ContainsKey("broader"))
                        {
                            broader = fila["broader"].value;
                        }
                        dicAreasBroader.Add(concept, broader);
                    }
                }


                while (true)
                {
                    int limit = 500;
                    //INSERTAMOS
                    String select = @"select ?document ?categoryNode ";
                    String where = @$"where{{
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
                            }}order by (?document) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "taxonomy" });
                    InsertarCategorias(resultado, dicAreasBroader, mResourceApi.GraphsUrl, "document", "http://w3id.org/roh/hasKnowledgeArea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limit = 500;
                    //ELIMINAMOS
                    String select = @"select ?document ?hasKnowledgeArea ";
                    String where = @$"where{{
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
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "taxonomy" });
                    EliminarCategorias(resultado, "document", "http://w3id.org/roh/hasKnowledgeArea");
                    if (resultado.results.bindings.Count != limit)
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

            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }
            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    //INSERTAMOS
                    String select = @"select ?document ?tag";
                    String where = @$"where{{
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
                            }}order by (?document) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "document");
                    InsercionMultipleTags(resultado.results.bindings, "document", "tag");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limit = 500;
                    //ELIMINAMOS
                    String select = @"select ?document ?tagAux";
                    String where = @$"where{{
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
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "document");
                    EliminacionMultiple(resultado.results.bindings, "http://vivoweb.org/ontology/core#freeTextKeyword", "document", "tagAux");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Eliminar duplicados
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?document count(?tag) ?tag ";
                    String where = @$"where
                                {{
                                    ?document a <http://purl.org/ontology/bibo/Document>.
                                    {filter}
                                    ?document <http://vivoweb.org/ontology/core#freeTextKeyword> ?freeTextKeyword. 
                                    ?freeTextKeyword <http://w3id.org/roh/title> ?tag. 
                                }}group by (?document) (?tag) HAVING (COUNT(?tag) > 1) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "document");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string document = fila["document"].value;
                        string tag = fila["tag"].value;
                        String select2 = @"select ?document ?data ";
                        String where2 = @$"where
                            {{                            
                                ?document <http://vivoweb.org/ontology/core#freeTextKeyword> ?data. 
                                ?data <http://w3id.org/roh/title> '{tag.Replace("'", "\\'")}'. 
                                FILTER(?document=<{document}>)
                            }}";
                        SparqlObject resultado2 = mResourceApi.VirtuosoQuery(select2, where2, "document");

                        foreach (Dictionary<string, SparqlObject.Data> fila2 in resultado2.results.bindings.GetRange(1, resultado2.results.bindings.Count - 1))
                        {
                            string value = fila2["data"].value;
                            ActualizadorTriple(document, "http://vivoweb.org/ontology/core#freeTextKeyword", value, "");
                        }
                    });
                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            EliminarDuplicados("document", "http://purl.org/ontology/bibo/Document", "http://w3id.org/roh/year");
            foreach (string filter in filters)
            {
                //Inserciones
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct * where{select ?document ?yearCargado ?yearCargar  ";
                    String where = @$"where{{
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

                            }}}} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "document");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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

                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            //Eliminamos los duplicados
            EliminarDuplicados("document", "http://purl.org/ontology/bibo/Document", "http://w3id.org/roh/genderIP");

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?document ?genderIPCargado ?genderIPACargar ";
                    String where = @$"where{{
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
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "person" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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

                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            //Eliminamos los duplicados
            EliminarDuplicados("document", "http://purl.org/ontology/bibo/Document", "http://w3id.org/roh/positionIP");

            foreach (string filter in filters)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?document ?positionIPCargado ?positionIPACargar ";
                    String where = @$"where{{
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
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "person" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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

                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }
            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?document  ";
                    String where = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                {filter}
                                MINUS
                                {{
                                    ?document <http://purl.org/ontology/bibo/authorList> ?autores.
                                    ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                    ?person <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                }}
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "person" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        try
                        {
                            mResourceApi.PersistentDelete(mResourceApi.GetShortGuid(fila["document"].value));
                        }
                        catch (Exception) { }
                    });

                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @$"select *  ";
                    String where = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                OPTIONAL{{?document <http://w3id.org/roh/hasPublicationVenueJournalText> ?nombreDesnormalizadoRevista.}}
                                ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista.
                                ?revista <http://w3id.org/roh/title> ?nombreRevista.
                                FILTER(?nombreDesnormalizadoRevista!=?nombreRevista)
                            }}limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument" });
                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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
                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @$"select * ";
                    String where = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                OPTIONAL{{?document <http://purl.org/ontology/bibo/publisher> ?nombreDesnormalizadoEditorial.}}
                                ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista.
                                ?revista <http://purl.org/ontology/bibo/editor> ?nombreEditorial.
                                FILTER(?nombreDesnormalizadoEditorial!=?nombreEditorial)
                            }}limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument" });
                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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
                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @$"select * ";
                    String where = @$"where{{
                                ?document a <http://purl.org/ontology/bibo/Document>.
                                OPTIONAL{{?document <http://purl.org/ontology/bibo/issn> ?issnDesnormalizado.}}
                                ?document <http://vivoweb.org/ontology/core#hasPublicationVenue> ?revista.
                                ?revista <http://purl.org/ontology/bibo/issn> ?issn.
                                FILTER(?issnDesnormalizado!=?issn)
                            }}limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument" });
                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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
                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                //Insertamos las auxiliares http://w3id.org/roh/ImpactIndex con categorías
                while (true)
                {
                    int limit = 500;

                    String select = @"select distinct ?document ?impactSource ?categoryTitle ?impactCategory ?impactIndexInYear ";
                    String where = @$"where{{          
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
}}order by (?document) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument", "taxonomy" });

                    InsercionIndicesImpacto(resultado.results.bindings);

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Eliminamos las auxiliares http://w3id.org/roh/ImpactIndex con categorías
                while (true)
                {
                    int limit = 500;

                    String select = @"select distinct ?document ?impactIndexDoc ";
                    String where = @$"where{{   
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

                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultado.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            Guid guid = mResourceApi.GetShortGuid(id);

                            Dictionary<Guid, List<RemoveTriples>> triples = new() { { guid, new List<RemoveTriples>() } };
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultado.results.bindings.Where(x => x["document"].value == id))
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

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Completamos http://w3id.org/roh/ImpactIndex con http://w3id.org/roh/publicationPosition
                while (true)
                {
                    int limit = 500;

                    String select = @"select distinct ?document ?impactIndexDoc ?publicationPositionCargar ?publicationPositionCargado  ";
                    String where = @$"where{{
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
}} limit {limit}";

                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultado.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultado.results.bindings.Where(x => x["document"].value == id))
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

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Completamos http://w3id.org/roh/ImpactIndex con http://w3id.org/roh/journalNumberInCat
                while (true)
                {
                    int limit = 500;

                    String select = @"select distinct ?document ?impactIndexDoc ?journalNumberInCatCargar ?journalNumberInCatCargado ";
                    String where = @$"where{{
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
}} limit {limit}";

                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultado.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultado.results.bindings.Where(x => x["document"].value == id))
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

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Completamos http://w3id.org/roh/ImpactIndex con http://w3id.org/roh/quartile
                while (true)
                {
                    int limit = 500;

                    String select = @"select distinct ?document ?impactIndexDoc ?quartileCargar ?quartileCargado ";
                    String where = @$"where{{
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
}} limit {limit}";

                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultado.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultado.results.bindings.Where(x => x["document"].value == id))
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

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Insertamos las auxiliares http://w3id.org/roh/ImpactIndex sin categorías
                while (true)
                {
                    int limit = 500;

                    String select = @"select distinct ?document ?impactSource ?impactIndexInYear ";
                    String where = @$"where{{          
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
}}order by (?document) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument", "taxonomy" });

                    InsercionIndicesImpacto(resultado.results.bindings);

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Eliminamos las auxiliares http://w3id.org/roh/ImpactIndex sin categorías
                while (true)
                {
                    int limit = 500;

                    String select = @"select ?document ?impactIndexDoc ";
                    String where = @$"where{{          
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
}}order by (?document) limit {limit}";

                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "maindocument", "taxonomy" });

                    List<string> ids = resultado.results.bindings.Select(x => x["document"].value).Distinct().ToList();
                    if (ids.Count > 0)
                    {
                        Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                        {
                            Guid guid = mResourceApi.GetShortGuid(id);

                            Dictionary<Guid, List<RemoveTriples>> triples = new() { { guid, new List<RemoveTriples>() } };
                            foreach (Dictionary<string, SparqlObject.Data> fila in resultado.results.bindings.Where(x => x["document"].value == id))
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

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Actualizamos cuartiles del doc
                //Eliminamos los duplicados
                EliminarDuplicados("document", "http://purl.org/ontology/bibo/Document", "http://w3id.org/roh/quartile");
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct ?document ?quartileCargado ?quartileCargar ";
                    String where = @$"where{{
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
                                    }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "document");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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
                        ActualizadorTriple(document, "http://w3id.org/roh/quartile", quartileCargado, quartileCargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Actualizamos el indice de impacto del doc
                //Eliminamos los duplicados
                EliminarDuplicados("document", "http://purl.org/ontology/bibo/Document", "http://w3id.org/roh/impactIndexInYear");
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct ?document ?impactIndexInYearCargado ?impactIndexInYearCargar ";
                    String where = @$"where{{
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
}} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "document");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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

                    if (resultado.results.bindings.Count != limit)
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
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            //Eliminamos los duplicados
            EliminarDuplicados("document", "http://purl.org/ontology/bibo/Document", "http://w3id.org/roh/linkedCount");

            foreach (string filter in filters)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?document ?numLinkedCargados IF (BOUND (?numLinkedACargar), ?numLinkedACargar, 0 ) as ?numLinkedACargar ";
                    String where = @$"where{{
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
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "researchobject" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
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

                    if (resultado.results.bindings.Count != limit)
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
            List<string> ids = pFilas.Select(x => x["document"].value).Distinct().ToList();
            if (ids.Count > 0)
            {
                Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                {
                    Guid guid = mResourceApi.GetShortGuid(id);

                    Dictionary<Guid, List<TriplesToInclude>> triples = new() { { guid, new List<TriplesToInclude>() } };
                    foreach (Dictionary<string, SparqlObject.Data> fila in pFilas.Where(x => x["document"].value == id))
                    {
                        string idAux = mResourceApi.GraphsUrl + "items/ImpactIndex_" + guid.ToString().ToLower() + "_" + Guid.NewGuid().ToString().ToLower();
                        string document = fila["document"].value;
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/impactIndex|http://w3id.org/roh/impactIndexInYear";
                            t.NewValue = idAux + "|" + fila["impactIndexInYear"].value;
                            triples[guid].Add(t);
                        }
                        if (fila.ContainsKey("impactSource"))
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/impactIndex|http://w3id.org/roh/impactSource";
                            t.NewValue = idAux + "|" + fila["impactSource"].value;
                            triples[guid].Add(t);
                        }
                        if (fila.ContainsKey("impactSourceOther"))
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/impactIndex|http://w3id.org/roh/impactSourceOther";
                            t.NewValue = idAux + "|" + fila["impactSourceOther"].value;
                            triples[guid].Add(t);
                        }
                        if (fila.ContainsKey("categoryTitle"))
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/impactIndex|http://w3id.org/roh/impactIndexCategory";
                            t.NewValue = idAux + "|" + fila["categoryTitle"].value;
                            triples[guid].Add(t);
                        }
                        if (fila.ContainsKey("publicationPosition"))
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/impactIndex|http://w3id.org/roh/publicationPosition";
                            t.NewValue = idAux + "|" + fila["publicationPosition"].value;
                            triples[guid].Add(t);
                        }
                        if (fila.ContainsKey("journalNumberInCat"))
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/impactIndex|http://w3id.org/roh/journalNumberInCat";
                            t.NewValue = idAux + "|" + fila["journalNumberInCat"].value;
                            triples[guid].Add(t);
                        }
                        if (fila.ContainsKey("quartile"))
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/impactIndex|http://w3id.org/roh/quartile";
                            t.NewValue = idAux + "|" + fila["quartile"].value;
                            triples[guid].Add(t);
                        }
                        if (fila.ContainsKey("impactCategory"))
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/impactIndex|http://w3id.org/roh/impactIndexCategoryEntity";
                            t.NewValue = idAux + "|" + fila["impactCategory"].value;
                            triples[guid].Add(t);
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
