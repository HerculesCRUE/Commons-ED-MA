using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gnoss.ApiWrapper.ApiModel.SparqlObject;

namespace Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores
{
    //TODO comentarios completados

    /// <summary>
    /// Clase base para los actualizadores
    /// </summary>
    public class ActualizadorBase
    {
        //TODO configurable
        public static int numParallel = 10;
        public static int MAX_INTENTOS = 10;

        /// <summary>
        /// API Wrapper de GNOSS
        /// </summary>
        protected ResourceApi mResourceApi;

        /// <summary>
        /// Lista con los prefijos
        /// </summary>
        private readonly static Dictionary<string, string> dicPrefix = new()
        {
            { "rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#" },
            { "rdfs", "http://www.w3.org/2000/01/rdf-schema#" },
            { "foaf", "http://xmlns.com/foaf/0.1/" },
            { "vivo", "http://vivoweb.org/ontology/core#" },
            { "owl", "http://www.w3.org/2002/07/owl#" },
            { "bibo", "http://purl.org/ontology/bibo/" },
            { "roh", "http://w3id.org/roh/" },
            { "dct", "http://purl.org/dc/terms/" },
            { "xsd", "http://www.w3.org/2001/XMLSchema#" },
            { "obo", "http://purl.obolibrary.org/obo/" },
            { "vcard", "https://www.w3.org/2006/vcard/ns#" },
            { "dc", "http://purl.org/dc/elements/1.1/" },
            { "gn", "http://www.geonames.org/ontology#" },
            { "skos", "http://www.w3.org/2008/05/skos#" }
        };

        public string GetUrlPrefix(string pPrefix)
        {
            return dicPrefix[pPrefix];
        }


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pResourceApi"></param>
        public ActualizadorBase(ResourceApi pResourceApi)
        {
            mResourceApi = pResourceApi;
        }

        /// <summary>
        /// Método para inserción múltiple de triples
        /// </summary>
        /// <param name="pFilas">Filas con los datos para insertar</param>
        /// <param name="pPredicado">Predicado para insertar</param>
        /// <param name="pPropSubject">Propiedad de las filas utilizada para el sujeto</param>
        /// <param name="pPropObject">Propiedad de las filas utilizada para el objeto</param>
        public void InsercionMultiple(List<Dictionary<string, Data>> pFilas, string pPredicado, string pPropSubject, string pPropObject)
        {
            List<string> ids = pFilas.Select(x => x[pPropSubject].value).Distinct().ToList();
            Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
            {
                Guid guid = mResourceApi.GetShortGuid(id);
                Dictionary<Guid, List<TriplesToInclude>> triples = new() { { guid, new List<TriplesToInclude>() } };
                foreach (string value in pFilas.Where(x => x[pPropSubject].value == id).Select(x => x[pPropObject].value))
                {
                    TriplesToInclude t = new();
                    t.Predicate = pPredicado;
                    t.NewValue = value;
                    triples[guid].Add(t);
                }
                var resultado = mResourceApi.InsertPropertiesLoadedResources(triples);
            });
        }

        /// <summary>
        /// Método para eliminación múltiple de triples
        /// </summary>
        /// <param name="pFilas">Filas con los datos para eliminar</param>
        /// <param name="pPredicado">Predicado para eliminar</param>
        /// <param name="pPropSubject">Propiedad de las filas utilizada para el sujeto</param>
        /// <param name="pPropObject">Propiedad de las filas utilizada para el objeto</param>
        public void EliminacionMultiple(List<Dictionary<string, Data>> pFilas, string pPredicado, string pPropSubject, string pPropObject)
        {
            List<string> ids = pFilas.Select(x => x[pPropSubject].value).Distinct().ToList();
            Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
            {
                Guid guid = mResourceApi.GetShortGuid(id);
                Dictionary<Guid, List<Gnoss.ApiWrapper.Model.RemoveTriples>> triples = new() { { guid, new List<RemoveTriples>() } };
                foreach (string value in pFilas.Where(x => x[pPropSubject].value == id).Select(x => x[pPropObject].value))
                {
                    RemoveTriples t = new();
                    t.Predicate = pPredicado;
                    t.Value = value;
                    triples[guid].Add(t);
                }
                var resultado = mResourceApi.DeletePropertiesLoadedResources(triples);
            });
        }

        /// <summary>
        /// Método para inserción múltiple de triples
        /// </summary>
        /// <param name="pFilas">Filas con los datos para insertar</param>
        /// <param name="pPropSubject">Propiedad de las filas utilizada para el sujeto</param>
        /// <param name="pPropObject">Propiedad de las filas utilizada para el objeto</param>
        public void InsercionMultipleTags(List<Dictionary<string, Data>> pFilas, string pPropSubject, string pPropObject)
        {
            List<string> ids = pFilas.Select(x => x[pPropSubject].value).Distinct().ToList();
            if (ids.Count > 0)
            {
                Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                {
                    Guid guid = mResourceApi.GetShortGuid(id);
                    Dictionary<Guid, List<TriplesToInclude>> triples = new() { { guid, new List<TriplesToInclude>() } };
                    foreach (string value in pFilas.Where(x => x[pPropSubject].value == id).Select(x => x[pPropObject].value))
                    {
                        string idAux = mResourceApi.GraphsUrl + "items/KeyWord_" + guid.ToString().ToLower() + "_" + Guid.NewGuid().ToString().ToLower();
                        TriplesToInclude t = new();
                        t.Predicate = "http://vivoweb.org/ontology/core#freeTextKeyword|http://w3id.org/roh/title";
                        t.NewValue = idAux + "|" + value;
                        triples[guid].Add(t);
                    }
                    var resultado = mResourceApi.InsertPropertiesLoadedResources(triples);
                });


                String selectInsercionMultipleTags = @"select ?id ?getKeyWords ";
                String whereInsercionMultipleTags = @$"where
                                {{
                                    ?id <http://w3id.org/roh/getKeyWords> ?getKeyWords. 
                                    FILTER(?id in (<{string.Join(">,<", ids)}>))
                                }}";
                SparqlObject resultadoInsercionMultipleTags = mResourceApi.VirtuosoQuery(selectInsercionMultipleTags, whereInsercionMultipleTags, "document");
                Dictionary<string, string> documentGetKeywords = new Dictionary<string, string>();
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoInsercionMultipleTags.results.bindings)
                {
                    documentGetKeywords[fila["id"].value] = fila["getKeyWords"].value;
                }



                Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                {
                    string valorAnterior = "";
                    if(documentGetKeywords.ContainsKey(id))
                    {
                        valorAnterior = documentGetKeywords[id];
                    }
                    ActualizadorTriple(id, "http://w3id.org/roh/getKeyWords", valorAnterior, "true");
                });

            }


        }

        /// <summary>
        /// Método para cargar/actualizar/eliminar triples
        /// </summary>
        /// <param name="pSujeto">Sujeto</param>
        /// <param name="pPredicado">Predicado</param>
        /// <param name="pValorAntiguo">Valor antiguo (si es vacío se inserta el valor nuevo)</param>
        /// <param name="pValorNuevo">Valor nuevo (si es vacío se eliminar el valor antiguo)</param>
        public void ActualizadorTriple(string pSujeto, string pPredicado, string pValorAntiguo, string pValorNuevo)
        {
            Guid guid = mResourceApi.GetShortGuid(pSujeto);
            if (!string.IsNullOrEmpty(pValorAntiguo) && !string.IsNullOrEmpty(pValorNuevo) && pValorAntiguo!=pValorNuevo)
            {
                //Si el valor nuevo y el viejo no son nulos -->modificamos
                TriplesToModify t = new();
                t.NewValue = pValorNuevo;
                t.OldValue = pValorAntiguo;
                t.Predicate = pPredicado;
                var x=mResourceApi.ModifyPropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.TriplesToModify>>() { { guid, new List<Gnoss.ApiWrapper.Model.TriplesToModify>() { t } } });
            }
            else if (string.IsNullOrEmpty(pValorAntiguo) && !string.IsNullOrEmpty(pValorNuevo))
            {
                //Si el valor nuevo no es nulo y viejo si es nulo -->insertamos
                TriplesToInclude t = new();
                t.Predicate = pPredicado;
                t.NewValue = pValorNuevo;
                var x = mResourceApi.InsertPropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.TriplesToInclude>>() { { guid, new List<Gnoss.ApiWrapper.Model.TriplesToInclude>() { t } } });
            }
            else if (!string.IsNullOrEmpty(pValorAntiguo) && string.IsNullOrEmpty(pValorNuevo))
            {
                //Si el valor nuevo es nulo y viejo si no es nulo -->eliminamos
                RemoveTriples t = new();
                t.Predicate = pPredicado;
                t.Value = pValorAntiguo;
                var x = mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.RemoveTriples>>() { { guid, new List<Gnoss.ApiWrapper.Model.RemoveTriples>() { t } } });
            }
        }

        /// <summary>
        /// Método para eliminar propiedades duplicadas que sólo deberían tener un valor
        /// </summary>
        /// <param name="pGraph">Grafo donde realizar la consulta</param>
        /// <param name="pRdfType">Rdftype del sujeto en el que comprobar la propiedad</param>
        /// <param name="pProperty">Propiedad a comprobar</param>
        public void EliminarDuplicados(string pGraph, string pRdfType, string pProperty)
        {
            while (true)
            {
                int limit = 500;
                String selectEliminarDuplicados = @"select ?id count(?data) ";
                String whereEliminarDuplicados = @$"where
                                {{
                                    ?id a <{pRdfType}>.
                                    ?id <{pProperty}> ?data. 
                                }}group by (?id) HAVING (COUNT(?data) > 1) limit {limit}";
                SparqlObject resultadoEliminarDuplicados = mResourceApi.VirtuosoQuery(selectEliminarDuplicados, whereEliminarDuplicados, pGraph);

                Parallel.ForEach(resultadoEliminarDuplicados.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                {
                    string id = fila["id"].value;
                    String selectEliminarDuplicadosIn = @"select ?data ";
                    String whereEliminarDuplicadosIn = @$"where
                            {{
                                <{id}> <{pProperty}> ?data. 
                            }}";
                    SparqlObject resultadoEliminarDuplicadosIn = mResourceApi.VirtuosoQuery(selectEliminarDuplicadosIn, whereEliminarDuplicadosIn, pGraph);
                    foreach (Dictionary<string, SparqlObject.Data> filaEliminarDuplicadosIn in resultadoEliminarDuplicadosIn.results.bindings.GetRange(1, resultadoEliminarDuplicadosIn.results.bindings.Count - 1))
                    {
                        string value = filaEliminarDuplicadosIn["data"].value;
                        ActualizadorTriple(id, pProperty, value, "");
                    }
                });
                if (resultadoEliminarDuplicados.results.bindings.Count != limit)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Método para insertar Categorías
        /// </summary>
        /// <param name="pResultado">Resultado de la query de la que obtener los datos</param>
        /// <param name="pDicAreasBroader">Diccionario con las áreas y sus padres</param>
        /// <param name="pGraphsUrl">Url interna de los grafos</param>
        /// <param name="pVarItem">Ítem en el que insertar las categorias</param>
        /// <param name="pPropCategoria">Propiedad en la que insertar las categorías</param>
        public void InsertarCategorias(SparqlObject pResultado, Dictionary<string, string> pDicAreasBroader, string pGraphsUrl, string pVarItem, string pPropCategoria)
        {
            Dictionary<Guid, List<TriplesToInclude>> triplesToInclude = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pResultado.results.bindings)
            {
                string item = fila[pVarItem].value;
                string categoryNode = fila["categoryNode"].value;

                string idNewAux = pGraphsUrl + "items/CategoryPath_" + mResourceApi.GetShortGuid(item).ToString().ToLower() + "_" + Guid.NewGuid();
                List<TriplesToInclude> listaTriples = new();
                string idEntityAux = idNewAux;

                string categoryNodeAux = categoryNode;
                while (!string.IsNullOrEmpty(categoryNodeAux))
                {
                    string predicadoCategoria = pPropCategoria + "|http://w3id.org/roh/categoryNode";
                    TriplesToInclude tr2 = new(idEntityAux + "|" + categoryNodeAux, predicadoCategoria);
                    listaTriples.Add(tr2);
                    categoryNodeAux = pDicAreasBroader[categoryNodeAux];
                }

                Guid idItem = mResourceApi.GetShortGuid(item);
                if (triplesToInclude.ContainsKey(idItem))
                {
                    triplesToInclude[idItem].AddRange(listaTriples);
                }
                else
                {
                    triplesToInclude.Add(idItem, listaTriples);
                }
            }
            Parallel.ForEach(triplesToInclude.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idItem =>
            {
                mResourceApi.InsertPropertiesLoadedResources(new() { { idItem, triplesToInclude[idItem] } });
            });
        }

        /// <summary>
        /// Método para eliminar Categorías
        /// </summary>
        /// <param name="pResultado">Resultado de la query de la que obtener los datos</param>
        /// <param name="pVarItem">Ítem en el que insertar las categorias</param>
        /// <param name="pPropCategoria">Propiedad en la que insertar las categorías</param>
        public void EliminarCategorias(SparqlObject pResultado, string pVarItem, string pPropCategoria)
        {
            Dictionary<Guid, List<RemoveTriples>> triplesToRemove = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pResultado.results.bindings)
            {
                string item = fila[pVarItem].value;
                string hasKnowledgeArea = fila["hasKnowledgeArea"].value;

                RemoveTriples removeTriple = new();
                removeTriple.Predicate = pPropCategoria;
                removeTriple.Value = hasKnowledgeArea;

                Guid idItem = mResourceApi.GetShortGuid(item);
                if (triplesToRemove.ContainsKey(idItem))
                {
                    triplesToRemove[idItem].Add(removeTriple);
                }
                else
                {
                    triplesToRemove.Add(idItem, new List<RemoveTriples>() { removeTriple });
                }
            }
            Parallel.ForEach(triplesToRemove.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idItem =>
            {
                mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<RemoveTriples>>() { { idItem, triplesToRemove[idItem] } });
            });
        }

        /// <summary>
        /// Cambia la propiedad añadiendole elprefijo
        /// </summary>
        /// <param name="pProperty">Propiedad con la URL completa</param>
        /// <returns>Url con prefijo</returns>
        public static string AniadirPrefijo(string pProperty)
        {
            KeyValuePair<string, string> prefix = dicPrefix.First(x => pProperty.StartsWith(x.Value));
            return pProperty.Replace(prefix.Value, prefix.Key + ":");
        }

        /// <summary>
        /// Trocea una lista en listas de N tamaño
        /// </summary>
        /// <typeparam name="T">Clased de los elementos de la lista</typeparam>
        /// <param name="pList">Lista</param>
        /// <param name="nSize">Tamaño de la lista</param>
        /// <returns></returns>
        public static IEnumerable<List<T>> SplitList<T>(List<T> pList, int pSize)
        {
            for (int i = 0; i < pList.Count; i += pSize)
            {
                yield return pList.GetRange(i, Math.Min(pSize, pList.Count - i));
            }
        }


        //TODO comenrtario
        protected void InsercionMiembrosProyectoGrupo(List<Dictionary<string, Data>> pFilas, string pTipo)
        {
            List<string> ids = pFilas.Select(x => x[pTipo].value).Distinct().ToList();
            Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
            {
                Guid guid = mResourceApi.GetShortGuid(id);
                Dictionary<Guid, List<TriplesToInclude>> triples = new() { { guid, new List<TriplesToInclude>() } };
                foreach (Dictionary<string, Data> fila in pFilas.Where(x => x[pTipo].value == id))
                {
                    string idAux = mResourceApi.GraphsUrl + "items/PersonAux_" + guid.ToString().ToLower() + "_" + Guid.NewGuid().ToString().ToLower();
                    string propMiembro = "";
                    if (fila["ip"].value == "true")
                    {
                        propMiembro = "http://w3id.org/roh/mainResearchers";
                    }
                    else
                    {
                        propMiembro = "http://w3id.org/roh/researchers";
                    }
                    {
                        TriplesToInclude t = new();
                        t.Predicate = propMiembro + "|http://www.w3.org/1999/02/22-rdf-syntax-ns#member";
                        t.NewValue = idAux + "|" + fila["person"].value;
                        triples[guid].Add(t);
                    }
                    if (fila.ContainsKey("nick"))
                    {
                        TriplesToInclude t = new();
                        t.Predicate = propMiembro + "|http://xmlns.com/foaf/0.1/nick";
                        t.NewValue = idAux + "|" + fila["nick"].value;
                        triples[guid].Add(t);
                    }else if (fila.ContainsKey("nombre"))
                    {
                        TriplesToInclude t = new();
                        t.Predicate = propMiembro + "|http://xmlns.com/foaf/0.1/nick";
                        t.NewValue = idAux + "|" + fila["nombre"].value;
                        triples[guid].Add(t);
                    }
                }
                var resultado = mResourceApi.InsertPropertiesLoadedResources(triples);
            });
        }
        //TODO comenrtario
        protected void EliminacionMiembrosProyectoGrupo(List<Dictionary<string, Data>> pFilas, string pTipo)
        {
            List<string> ids = pFilas.Select(x => x[pTipo].value).Distinct().ToList();
            Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
            {
                Guid guid = mResourceApi.GetShortGuid(id);
                Dictionary<Guid, List<RemoveTriples>> triples = new() { { guid, new List<RemoveTriples>() } };
                foreach (Dictionary<string, Data> fila in pFilas.Where(x => x[pTipo].value == id))
                {
                    RemoveTriples t = new();
                    t.Predicate = fila["propPersonAux"].value;
                    t.Value = fila["personAux"].value;
                    triples[guid].Add(t);
                }
                var resultado = mResourceApi.DeletePropertiesLoadedResources(triples);
            });
        }

        protected void ActualizarPropiedadMiembrosProyectoGrupoPatente(List<Dictionary<string, Data>> pFilas, string pTipo)
        {
            List<string> ids = pFilas.Select(x => x[pTipo].value).Distinct().ToList();
            Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
            {
                Guid guid = mResourceApi.GetShortGuid(id);
                Dictionary<Guid, List<TriplesToModify>> triplesModify = new() { { guid, new List<TriplesToModify>() } };
                Dictionary<Guid, List<TriplesToInclude>> triplesInsertar = new() { { guid, new List<TriplesToInclude>() } };
                Dictionary<Guid, List<RemoveTriples>> triplesEliminar = new() { { guid, new List<RemoveTriples>() } };
                foreach (Dictionary<string, Data> fila in pFilas.Where(x => x[pTipo].value == id))
                {
                    string valorAntiguo = "";
                    string valorNuevo = "";
                    if (fila.ContainsKey("propertyLoad"))
                    {
                        valorAntiguo = fila["propertyLoad"].value;
                    }
                    if (fila.ContainsKey("propertyToLoad"))
                    {
                        valorNuevo = fila["propertyToLoad"].value;
                    }

                    if (!string.IsNullOrEmpty(valorNuevo) && string.IsNullOrEmpty(valorAntiguo))
                    {
                        TriplesToInclude t = new();
                        t.Predicate = fila["propPersonAux"].value + "|" + fila["property"].value;
                        t.NewValue = fila["personAux"].value + "|" + fila["propertyToLoad"].value;
                        triplesInsertar[guid].Add(t);
                    }
                    else if (string.IsNullOrEmpty(valorNuevo) && !string.IsNullOrEmpty(valorAntiguo))
                    {
                        RemoveTriples t = new();
                        t.Predicate = fila["propPersonAux"].value + "|" + fila["property"].value;
                        t.Value = fila["personAux"].value + "|" + fila["propertyLoad"].value;
                        triplesEliminar[guid].Add(t);
                    }
                    else if (!string.IsNullOrEmpty(valorNuevo) && !string.IsNullOrEmpty(valorAntiguo) && valorAntiguo != valorNuevo)
                    {
                        TriplesToModify t = new();
                        t.Predicate = fila["propPersonAux"].value + "|" + fila["property"].value;
                        t.NewValue = fila["personAux"].value + "|" + fila["propertyToLoad"].value;
                        t.OldValue = fila["personAux"].value + "|" + fila["propertyLoad"].value;
                        triplesModify[guid].Add(t);
                    }
                }
                if (triplesInsertar[guid].Count > 0)
                {
                    var resultadoInsetar = mResourceApi.InsertPropertiesLoadedResources(triplesInsertar);
                }
                if (triplesEliminar[guid].Count > 0)
                {
                    var resultadoEliminar = mResourceApi.DeletePropertiesLoadedResources(triplesEliminar);
                }
                if (triplesModify[guid].Count > 0)
                {
                    var resultadoModificar = mResourceApi.ModifyPropertiesLoadedResources(triplesModify);
                }
            });
        }

        protected Dictionary<string, string> ObtenerAreasBroader()
        {
            Dictionary<string, string> dicAreasBroader = new();
            String selectCargarTesauro = @"select distinct * ";
            String whereCargarTesauro = @$"where{{
                                ?concept a <http://www.w3.org/2008/05/skos#Concept>.
                                ?concept <http://purl.org/dc/elements/1.1/source> 'researcharea'
                                OPTIONAL{{?concept <http://www.w3.org/2008/05/skos#broader> ?broader}}
                            }}";
            SparqlObject resultadoCargarTesauro = mResourceApi.VirtuosoQuery(selectCargarTesauro, whereCargarTesauro, "taxonomy");

            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoCargarTesauro.results.bindings)
            {
                string concept = fila["concept"].value;
                string broader = "";
                if (fila.ContainsKey("broader"))
                {
                    broader = fila["broader"].value;
                }
                dicAreasBroader.Add(concept, broader);
            }
            return dicAreasBroader;
        }

    }
}
