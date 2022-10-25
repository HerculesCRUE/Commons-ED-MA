using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores
{
    /// <summary>
    /// Clase para actualizar propiedades de documentos
    /// </summary>
    class ActualizadorRO : ActualizadorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pResourceApi">API Wrapper de GNOSS</param>
        public ActualizadorRO(ResourceApi pResourceApi) : base(pResourceApi)
        {
        }


        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/hasKnowledgeArea de los http://w3id.org/roh/ResearchObject
        /// las áreas del documento (obtenido de varias propiedades en las que están las áreas en función de su origen)
        /// No tiene dependencias
        /// </summary>
        /// <param name="pROs">IDs de researchObjects</param>
        public void ActualizarAreasRO(List<string> pROs = null)
        {
            //Categorías
            //unificada-->http://w3id.org/roh/hasKnowledgeArea
            //usuario-->http://w3id.org/roh/userKnowledgeArea
            //external-->http://w3id.org/roh/externalKnowledgeArea
            //enriched-->http://w3id.org/roh/enrichedKnowledgeArea
            HashSet<string> filtersActualizarAreasRO = new HashSet<string>();
            if (pROs != null && pROs.Count > 0)
            {
                filtersActualizarAreasRO.Add($" FILTER(?ro in (<{string.Join(">,<", pROs)}>))");
            }
            if (filtersActualizarAreasRO.Count == 0)
            {
                filtersActualizarAreasRO.Add("");
            }

            foreach (string filter in filtersActualizarAreasRO)
            {
                //Eliminamos las categorías duplicadas
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?ro ?categoryNode ";
                    String where = @$"where{{
                                select distinct ?ro ?hasKnowledgeAreaAux  ?categoryNode
                                where{{
                                    {filter}
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    ?ro  <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaAux.
                                    ?hasKnowledgeAreaAux <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                               }}
                            }}group by ?ro ?categoryNode HAVING (COUNT(*) > 1) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "researchobject", "taxonomy" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string ro = fila["ro"].value;
                        string categoryNode = fila["categoryNode"].value;
                        select = @"select ?ro ?hasKnowledgeArea   ?categoryNode ";
                        where = @$"where{{
                                    FILTER(?ro=<{ro}>)
                                    FILTER(?categoryNode =<{categoryNode}>)
                                    {{ 
                                        select distinct ?ro ?hasKnowledgeArea  ?categoryNode
                                        where{{
                                            ?ro a <http://w3id.org/roh/ResearchObject>.
                                            ?ro  <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                                            ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?categoryNode.
                                            MINUS{{
                                                ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                            }}
                                        }}
                                    }}
                                }}";
                        resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "researchobject", "taxonomy" });
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
                            var resultadox = mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.RemoveTriples>>() { { mResourceApi.GetShortGuid(ro), triplesRemove } });
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
                    String select = @"select distinct * where{select ?ro ?categoryNode ";
                    String where = @$"where{{
                            ?ro a <http://w3id.org/roh/ResearchObject>.
                            {filter}
                            {{
                                select  distinct ?ro ?hasKnowledgeAreaDocument ?categoryNode where{{
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    ?ro ?props ?hasKnowledgeAreaDocument.
                                    FILTER(?props in (<http://w3id.org/roh/userKnowledgeArea>,<http://w3id.org/roh/externalKnowledgeArea>,<http://w3id.org/roh/enrichedKnowledgeArea>))
                                    ?hasKnowledgeAreaDocument <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}
                            }}
                            MINUS{{
                                select distinct ?ro ?hasKnowledgeAreaDocumentAux ?categoryNode 
                                where{{
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    ?ro <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaDocumentAux.
                                    ?hasKnowledgeAreaDocumentAux <http://w3id.org/roh/categoryNode> ?categoryNode
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}
                            }}
                            }}}}order by (?ro) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "researchobject", "taxonomy" });
                    InsertarCategorias(resultado, dicAreasBroader, mResourceApi.GraphsUrl, "ro", "http://w3id.org/roh/hasKnowledgeArea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limit = 500;
                    //ELIMINAMOS
                    String select = @"select distinct * where{select ?ro ?hasKnowledgeArea ";
                    String where = @$"where{{
                            ?ro a <http://w3id.org/roh/ResearchObject>.
                            {filter}
                            {{
                                select distinct ?ro ?hasKnowledgeArea ?categoryNode 
                                where{{
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    ?ro <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                                    ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?categoryNode
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}                               
                            }}
                            MINUS{{
                                select  distinct ?ro ?hasKnowledgeAreaDocument ?categoryNode where{{
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    ?ro ?props ?hasKnowledgeAreaDocument.
                                    FILTER(?props in (<http://w3id.org/roh/userKnowledgeArea>,<http://w3id.org/roh/externalKnowledgeArea>,<http://w3id.org/roh/enrichedKnowledgeArea>))
                                    ?hasKnowledgeAreaDocument <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}
                                 
                            }}
                            }}}} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "researchobject", "taxonomy" });
                    EliminarCategorias(resultado, "ro", "http://w3id.org/roh/hasKnowledgeArea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos en la propiedad http://vivoweb.org/ontology/core#freeTextKeyword de los http://w3id.org/roh/ResearchObject
        /// los tagso (obtenido de varias propiedades en las que están los tags en función de su origen)
        /// No tiene dependencias
        /// </summary>
        /// <param name="pROs">ID de ROs</param>
        public void ActualizarTagsRO(List<string> pROs = null)
        {
            //Etiquetas
            //unificada-->http://vivoweb.org/ontology/core#freeTextKeyword
            //usuario-->http://w3id.org/roh/userKeywords
            //external-->http://w3id.org/roh/externalKeywords
            //enriched-->http://w3id.org/roh/enrichedKeywords


            HashSet<string> filtersActualizarTagsRO = new HashSet<string>();
            if (pROs != null && pROs.Count > 0)
            {
                filtersActualizarTagsRO.Add($" FILTER(?ro in (<{string.Join(">,<", pROs)}>))");
            }
            if (filtersActualizarTagsRO.Count == 0)
            {
                filtersActualizarTagsRO.Add("");
            }

            foreach (string filter in filtersActualizarTagsRO)
            {
                while (true)
                {
                    int limit = 500;
                    //INSERTAMOS
                    String select = @"select distinct * where{select ?ro ?tag";
                    String where = @$"where{{
                            ?ro a <http://w3id.org/roh/ResearchObject>.
                            {filter}
                            {{
                                select  distinct ?ro ?tag where{{
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    ?ro ?props ?tag.
                                    FILTER(?props in (<http://w3id.org/roh/userKeywords>,<http://w3id.org/roh/externalKeywords>,<http://w3id.org/roh/enrichedKeywords>))
                                }}
                            }}
                            MINUS{{
                                select distinct ?document ?tag
                                where{{
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    ?ro <http://vivoweb.org/ontology/core#freeTextKeyword> ?tag.
                                }}
                            }}
                            }}}}order by (?ro) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "researchobject");
                    InsercionMultiple(resultado.results.bindings, "http://vivoweb.org/ontology/core#freeTextKeyword", "ro", "tag");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limit = 500;
                    //ELIMINAMOS
                    String select = @"select distinct * where{select ?ro ?tag";
                    String where = @$"where{{
                            ?ro a <http://w3id.org/roh/ResearchObject>.
                            {filter}
                            {{
                                select distinct ?ro ?tag
                                where{{
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    ?ro <http://vivoweb.org/ontology/core#freeTextKeyword> ?tag.
                                }}                               
                            }}
                            MINUS{{
                                select  distinct ?ro ?tag where{{
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    ?ro ?props ?tag.
                                    FILTER(?props in (<http://w3id.org/roh/userKeywords>,<http://w3id.org/roh/externalKeywords>,<http://w3id.org/roh/enrichedKeywords>))
                                }}
                                 
                            }}
                            }}}} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "researchobject");
                    EliminacionMultiple(resultado.results.bindings, "http://vivoweb.org/ontology/core#freeTextKeyword", "ro", "tag");
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
        /// <param name="pROs">ID de ROs</param>
        public void EliminarROsSinAutoresActivos(List<string> pROs = null)
        {
            HashSet<string> filtersEliminarROsSinAutoresActivos = new HashSet<string>();
            if (pROs != null && pROs.Count > 0)
            {
                filtersEliminarROsSinAutoresActivos.Add($" FILTER(?ro in (<{string.Join(">,<", pROs)}>))");
            }
            if (filtersEliminarROsSinAutoresActivos.Count == 0)
            {
                filtersEliminarROsSinAutoresActivos.Add("");
            }
            foreach (string filter in filtersEliminarROsSinAutoresActivos)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?ro ";
                    String where = @$"where{{
                                ?ro a <http://w3id.org/roh/ResearchObject>.
                                {filter}
                                MINUS
                                {{
                                    ?ro <http://purl.org/ontology/bibo/authorList> ?autores.
                                    ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                    ?person <http://w3id.org/roh/isActive> 'true'.
                                }}
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "researchobject", "person" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        try
                        {
                            mResourceApi.PersistentDelete(mResourceApi.GetShortGuid(fila["ro"].value));
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

        // <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/linkedCount de los http://w3id.org/roh/ResearchObject
        /// el nº de recursos relacionados públicos de un researchObject
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">IDs de documentos</param>
        public void ActualizarNumeroVinculados(List<string> pROs = null)
        {
            HashSet<string> filtersActualizarNumeroVinculados = new HashSet<string>();
            if (pROs != null && pROs.Count > 0)
            {
                filtersActualizarNumeroVinculados.Add($" FILTER(?ro in (<{string.Join(">,<", pROs)}>))");
            }
            if (filtersActualizarNumeroVinculados.Count == 0)
            {
                filtersActualizarNumeroVinculados.Add("");
            }

            //Eliminamos los duplicados
            EliminarDuplicados("researchobject", "http://w3id.org/roh/ResearchObject", "http://w3id.org/roh/linkedCount");

            foreach (string filter in filtersActualizarNumeroVinculados)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?ro ?numLinkedCargados IF (BOUND (?numLinkedACargar), ?numLinkedACargar, 0 ) as ?numLinkedACargar ";
                    String where = @$"where{{
                                ?ro a <http://w3id.org/roh/ResearchObject>.
                                {filter}
                                OPTIONAL
                                {{
                                  ?ro <http://w3id.org/roh/linkedCount> ?numLinkedCargadosAux. 
                                  BIND(xsd:int( ?numLinkedCargadosAux) as  ?numLinkedCargados)
                                }}
                                OPTIONAL{{
                                  select ?ro count(distinct(?linkedID)) as ?numLinkedACargar
                                  Where{{                                    
                                    ?ro a <http://w3id.org/roh/ResearchObject>.
                                    OPTIONAL
                                    {{
                                        {{
                                            ?ro ?linked ?linkedID.
                                            Filter (?linked in (<http://w3id.org/roh/linkedDocument>, <http://w3id.org/roh/linkedRO>))
                                            ?linkedID <http://w3id.org/roh/isValidated> 'true'.                              
                                        }}UNION
                                        {{
                                            ?linkedID ?linked ?ro. 
                                            Filter (?linked in (<http://w3id.org/roh/linkedDocument>, <http://w3id.org/roh/linkedRO>))
                                            ?linkedID <http://w3id.org/roh/isValidated> 'true'.                     
                                        }}
                                    }}
                                  }}
                                }}
                                FILTER(?numLinkedCargados!= ?numLinkedACargar OR !BOUND(?numLinkedCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "researchobject", "document" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string ro = fila["ro"].value;
                        string numLinkedACargar = fila["numLinkedACargar"].value;
                        string numLinkedCargados = "";
                        if (fila.ContainsKey("numLinkedCargados"))
                        {
                            numLinkedCargados = fila["numLinkedCargados"].value;
                        }
                        ActualizadorTriple(ro, "http://w3id.org/roh/linkedCount", numLinkedCargados, numLinkedACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

    }
}
