﻿using Gnoss.ApiWrapper;
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
                    int limitEliminarDuplicadas = 500;
                    String selectEliminarDuplicadas = @"select ?ro ?categoryNode ";
                    String whereEliminarDuplicadas = @$"where{{
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
                            }}group by ?ro ?categoryNode HAVING (COUNT(*) > 1) limit {limitEliminarDuplicadas}";
                    SparqlObject resultadoEliminarDuplicadas = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarDuplicadas, whereEliminarDuplicadas, new List<string>() { "researchobject", "taxonomy" });

                    Parallel.ForEach(resultadoEliminarDuplicadas.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string ro = fila["ro"].value;
                        string categoryNode = fila["categoryNode"].value;
                        string selectEliminarDuplicadasIn = @"select ?ro ?hasKnowledgeArea   ?categoryNode ";
                        string whereEliminarDuplicadasIn = @$"where{{
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
                        SparqlObject resultadoEliminarDuplicadasIn = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarDuplicadasIn, whereEliminarDuplicadasIn, new List<string>() { "researchobject", "taxonomy" });
                        List<RemoveTriples> triplesRemove = new();
                        foreach (string hasKnowledgeArea in resultadoEliminarDuplicadasIn.results.bindings.GetRange(1, resultadoEliminarDuplicadasIn.results.bindings.Count - 1).Select(x => x["hasKnowledgeArea"].value).ToList())
                        {
                            triplesRemove.Add(new RemoveTriples()
                            {
                                Predicate = $"{GetUrlPrefix("roh")}hasKnowledgeArea",
                                Value = hasKnowledgeArea
                            }); ;
                        }
                        if (triplesRemove.Count > 0)
                        {
                            var resultadox = mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.RemoveTriples>>() { { mResourceApi.GetShortGuid(ro), triplesRemove } });
                        }
                    });


                    if (resultadoEliminarDuplicadas.results.bindings.Count != limitEliminarDuplicadas)
                    {
                        break;
                    }
                }

                //Cargamos el tesauro
                Dictionary<string, string> dicAreasBroader = ObtenerAreasBroader();

                while (true)
                {
                    int limitInsertamos = 500;
                    //INSERTAMOS
                    String selectInsertamos = @"select distinct * where{select ?ro ?categoryNode ";
                    String whereInsertamos = @$"where{{
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
                            }}}}order by (?ro) limit {limitInsertamos}";
                    SparqlObject resultadoInsertamos = mResourceApi.VirtuosoQueryMultipleGraph(selectInsertamos, whereInsertamos, new List<string>() { "researchobject", "taxonomy" });
                    InsertarCategorias(resultadoInsertamos, dicAreasBroader, mResourceApi.GraphsUrl, "ro", $"{GetUrlPrefix("roh")}hasKnowledgeArea");
                    if (resultadoInsertamos.results.bindings.Count != limitInsertamos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limitEliminamos = 500;
                    //ELIMINAMOS
                    String selectEliminamos = @"select distinct * where{select ?ro ?hasKnowledgeArea ";
                    String whereEliminamos = @$"where{{
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
                            }}}} limit {limitEliminamos}";
                    SparqlObject resultadoEliminamos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminamos, whereEliminamos, new List<string>() { "researchobject", "taxonomy" });
                    EliminarCategorias(resultadoEliminamos, "ro", $"{GetUrlPrefix("roh")}hasKnowledgeArea");
                    if (resultadoEliminamos.results.bindings.Count != limitEliminamos)
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
                    int limitInsertamos = 500;
                    //INSERTAMOS
                    String selectInsertamos = @"select distinct * where{select ?ro ?tag";
                    String whereInsertamos = @$"where{{
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
                            }}}}order by (?ro) limit {limitInsertamos}";
                    SparqlObject resultadoInsertamos = mResourceApi.VirtuosoQuery(selectInsertamos, whereInsertamos, "researchobject");
                    InsercionMultiple(resultadoInsertamos.results.bindings, $"{GetUrlPrefix("vivo")}freeTextKeyword", "ro", "tag");
                    if (resultadoInsertamos.results.bindings.Count != limitInsertamos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limitEliminamos = 500;
                    //ELIMINAMOS
                    String selectEliminamos = @"select distinct * where{select ?ro ?tag";
                    String whereEliminamos = @$"where{{
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
                            }}}} limit {limitEliminamos}";
                    SparqlObject resultadoEliminamos = mResourceApi.VirtuosoQuery(selectEliminamos, whereEliminamos, "researchobject");
                    EliminacionMultiple(resultadoEliminamos.results.bindings, $"{GetUrlPrefix("vivo")}freeTextKeyword", "ro", "tag");
                    if (resultadoEliminamos.results.bindings.Count != limitEliminamos)
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
                    int limitEliminarROsSinAutoresActivos = 500;
                    String selectEliminarROsSinAutoresActivos = @"select ?ro ";
                    String whereEliminarROsSinAutoresActivos = @$"where{{
                                ?ro a <http://w3id.org/roh/ResearchObject>.
                                {filter}
                                MINUS
                                {{
                                    ?ro <http://purl.org/ontology/bibo/authorList> ?autores.
                                    ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                    ?person <http://w3id.org/roh/isActive> 'true'.
                                }}
                            }} limit {limitEliminarROsSinAutoresActivos}";
                    SparqlObject resultadoEliminarROsSinAutoresActivos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarROsSinAutoresActivos, whereEliminarROsSinAutoresActivos, new List<string>() { "researchobject", "person" });

                    Parallel.ForEach(resultadoEliminarROsSinAutoresActivos.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        try
                        {
                            mResourceApi.PersistentDelete(mResourceApi.GetShortGuid(fila["ro"].value));
                        }
                        catch (Exception) { }
                    });

                    if (resultadoEliminarROsSinAutoresActivos.results.bindings.Count != limitEliminarROsSinAutoresActivos)
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
            EliminarDuplicados("researchobject", $"{GetUrlPrefix("roh")}ResearchObject", $"{GetUrlPrefix("roh")}linkedCount");

            foreach (string filter in filtersActualizarNumeroVinculados)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limitActualizarNumeroVinculados = 500;
                    String selectActualizarNumeroVinculados = @"select ?ro ?numLinkedCargados IF (BOUND (?numLinkedACargar), ?numLinkedACargar, 0 ) as ?numLinkedACargar ";
                    String whereActualizarNumeroVinculados = @$"where{{
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
                            }} limit {limitActualizarNumeroVinculados}";
                    SparqlObject resultadoActualizarNumeroVinculados = mResourceApi.VirtuosoQueryMultipleGraph(selectActualizarNumeroVinculados, whereActualizarNumeroVinculados, new List<string>() { "researchobject", "document" });

                    Parallel.ForEach(resultadoActualizarNumeroVinculados.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string ro = fila["ro"].value;
                        string numLinkedACargar = fila["numLinkedACargar"].value;
                        string numLinkedCargados = "";
                        if (fila.ContainsKey("numLinkedCargados"))
                        {
                            numLinkedCargados = fila["numLinkedCargados"].value;
                        }
                        ActualizadorTriple(ro, $"{GetUrlPrefix("roh")}linkedCount", numLinkedCargados, numLinkedACargar);
                    });

                    if (resultadoActualizarNumeroVinculados.results.bindings.Count != limitActualizarNumeroVinculados)
                    {
                        break;
                    }
                }
            }
        }

    }
}
