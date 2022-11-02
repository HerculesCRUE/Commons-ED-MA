using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Hercules.CommonsEDMA.ServicioExterno.Models;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataItemRelacion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesAreasTematicas : GnossGetMainResourceApiDataBase
    {
        /// <summary>
        /// Obtiene el objeto para crear la gráfica de áreas temáticas
        /// </summary>
        /// <param name="pId">ID del elemento en cuestión.</param>
        /// <param name="pType">Tipo del elemento.</param>
        /// <param name="pAnioInicio">Año de inicio</param>
        /// <param name="pAnioFin">Año de fin</param>
        /// <returns>Objeto que se trata en JS para contruir la gráfica.</returns>
        public DataGraficaAreasTags DatosGraficaAreasTematicas(string pId, string pType, string pAnioInicio, string pAnioFin)
        {
            string idGrafoBusqueda = UtilidadesAPI.ObtenerIdBusqueda(resourceApi, pId);
            string filtroElemento = "";
            switch (pType)
            {
                case "group":
                    filtroElemento = $@"?documento roh:isProducedBy <{idGrafoBusqueda}>.";
                    break;
                case "person":
                    filtroElemento = $@"?documento bibo:authorList ?lista. ";
                    filtroElemento += $@"?lista rdf:member <{idGrafoBusqueda}>.";
                    break;
                case "project":
                    filtroElemento = $@"?documento roh:project <{idGrafoBusqueda}>.";
                    break;
                default:
                    throw new ArgumentException("No hay configuración para el tipo '" + pType + "'");
            }
            if (!string.IsNullOrEmpty(pAnioInicio))
            {
                filtroElemento += $@"?documento dct:issued ?fecha.";
                filtroElemento += $@"FILTER(?fecha>={pAnioInicio}0000000000)";
                if (!string.IsNullOrEmpty(pAnioFin))
                {
                    filtroElemento += $@"FILTER(?fecha<={int.Parse(pAnioFin) + 1}0000000000)";
                }
            }

            Dictionary<string, int> dicResultados = new Dictionary<string, int>();
            int numDocumentos = 0;
            {
                //Nº de documentos por categoría
                SparqlObject resultadoQuery = null;
                string select = $"{mPrefijos} Select ?nombreCategoria count(distinct ?documento) as ?numCategorias";
                string where = $@"  where
                                {{
                                    ?documento a 'document'. 
                                    {filtroElemento}
                                    ?documento roh:hasKnowledgeArea ?area.
                                    ?area roh:categoryNode ?categoria.
                                    ?categoria skos:prefLabel ?nombreCategoria.
                                    MINUS
                                    {{
                                        ?categoria skos:narrower ?hijos
                                    }}
                                }}
                                Group by(?nombreCategoria)";

                resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        string nombreCategoria = UtilidadesAPI.GetValorFilaSparqlObject(fila, "nombreCategoria");
                        int numCategoria = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numCategorias"));
                        dicResultados.Add(nombreCategoria + " (" + numCategoria +")", numCategoria);
                    }
                }
            }
            {
                //Nº total de documentos
                SparqlObject resultadoQuery = null;
                string select = $"{mPrefijos} Select count(distinct ?documento) as ?numDocumentos";
                string where = $@"  where
                                {{
                                    ?documento a 'document'. 
                                    {filtroElemento}
                                }}";

                resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        numDocumentos = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numDocumentos"));
                    }
                }
            }

            //Ordenar diccionario
            var dicionarioOrdenado = dicResultados.OrderByDescending(x => x.Value);

            Dictionary<string, double> dicResultadosPorcentaje = new Dictionary<string, double>();
            foreach (KeyValuePair<string, int> item in dicionarioOrdenado)
            {
                double porcentaje = Math.Round((double)(100 * item.Value) / numDocumentos, 2);
                dicResultadosPorcentaje.Add(item.Key, porcentaje);
            }

            // Contruir el objeto de la gráfica.
            List<string> listaColores = UtilidadesAPI.CrearListaColores(dicionarioOrdenado.Count(), "#6cafe3");
            Datasets datasets = new Datasets(dicResultadosPorcentaje.Values.ToList(), listaColores);
            Models.Graficas.DataGraficaAreasTags.Data data = new Models.Graficas.DataGraficaAreasTags.Data(dicResultadosPorcentaje.Keys.ToList(), new List<Datasets> { datasets });

            // Máximo.
            x xAxes = new x(new Ticks(0, 100), new ScaleLabel(true, "Percentage"));

            Options options = new Options("y", new Plugins(null, new Legend(false)), new Scales(xAxes));
            DataGraficaAreasTags dataGrafica = new DataGraficaAreasTags("bar", data, options);

            return dataGrafica;
        }

        /// <summary>
        /// Obtiene el objeto para crear la gráfica de áreas temáticas
        /// </summary>
        /// <param name="pId">ID del elemento en cuestión.</param>
        /// <param name="pType">Tipo del elemento.</param>
        /// <param name="pAnioInicio">Año de inicio</param>
        /// <param name="pAnioFin">Año de fin</param>
        /// <param name="pNumAreas">Número de areas</param>
        /// <returns>Objeto que se trata en JS para contruir la gráfica.</returns>
        public List<DataItemRelacion> DatosGraficaAreasTematicasArania(string pId, string pType, string pAnioInicio, string pAnioFin, int pNumAreas)
        {
            string idGrafoBusqueda = UtilidadesAPI.ObtenerIdBusqueda(resourceApi, pId);
            string filtroElemento = "";
            switch (pType)
            {
                case "group":
                    filtroElemento = $@"?documento roh:isProducedBy <{idGrafoBusqueda}>.";
                    break;
                case "person":
                    filtroElemento = $@"?documento bibo:authorList ?lista. ";
                    filtroElemento += $@"?lista rdf:member <{idGrafoBusqueda}>.";
                    break;
                case "project":
                    filtroElemento = $@"?documento roh:project <{idGrafoBusqueda}>.";
                    break;
                default:
                    throw new ArgumentException("No hay configuración para el tipo '" + pType + "'");
            }
            if (!string.IsNullOrEmpty(pAnioInicio))
            {
                filtroElemento += $@"?documento dct:issued ?fecha.";
                filtroElemento += $@"FILTER(?fecha>={pAnioInicio}0000000000)";
                if (!string.IsNullOrEmpty(pAnioFin))
                {
                    filtroElemento += $@"FILTER(?fecha<={int.Parse(pAnioFin) + 1}0000000000)";
                }
            }

            //Nodos            
            Dictionary<string, string> dicNodos = new Dictionary<string, string>();
            //Relaciones
            Dictionary<string, List<DataQueryRelaciones>> dicRelaciones = new Dictionary<string, List<DataQueryRelaciones>>();
            //Respuesta
            List<DataItemRelacion> items = new List<DataItemRelacion>();

            Dictionary<string, List<string>> dicResultadosAreaRelacionAreas = new Dictionary<string, List<string>>();
            Dictionary<string, int> scoreNodes = new Dictionary<string, int>();
            {
                string select = $"{mPrefijos} Select ?documento group_concat(?categoria;separator=\",\") as ?idCategorias";
                string where = $@"  where
                                {{
                                    ?documento a 'document'. 
                                    {filtroElemento}
                                    ?documento roh:hasKnowledgeArea ?area.
                                    ?area roh:categoryNode ?categoria.
                                    MINUS
                                    {{
                                        ?categoria skos:narrower ?hijos
                                    }}
                                }}";

                SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        string idCategorias = UtilidadesAPI.GetValorFilaSparqlObject(fila, "idCategorias");
                        HashSet<string> categorias = new HashSet<string>(idCategorias.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));

                        foreach (string categoria in categorias)
                        {
                            if (!scoreNodes.ContainsKey(categoria))
                            {
                                scoreNodes.Add(categoria, 0);
                            }
                            scoreNodes[categoria]++;
                            if (!dicResultadosAreaRelacionAreas.ContainsKey(categoria))
                            {
                                dicResultadosAreaRelacionAreas.Add(categoria, new List<string>());
                            }
                            dicResultadosAreaRelacionAreas[categoria].AddRange(categorias.Except(new List<string>() { categoria }));
                        }
                    }
                }
            }
            UtilidadesAPI.ProcessRelations("Category", dicResultadosAreaRelacionAreas, ref dicRelaciones);


            int maximasRelaciones = 0;
            foreach (KeyValuePair<string, List<DataQueryRelaciones>> sujeto in dicRelaciones)
            {
                foreach (DataQueryRelaciones relaciones in sujeto.Value)
                {
                    foreach (Datos relaciones2 in relaciones.idRelacionados)
                    {
                        maximasRelaciones = Math.Max(maximasRelaciones, relaciones2.numVeces);
                    }
                }
            }

            //Creamos los nodos y las relaciones en función de pNumAreas
            Dictionary<string, int> numRelaciones = new Dictionary<string, int>();
            foreach (KeyValuePair<string, List<DataQueryRelaciones>> sujeto in dicRelaciones)
            {
                if(!numRelaciones.ContainsKey(sujeto.Key))
                {
                    numRelaciones.Add(sujeto.Key, 0);
                }
                foreach (DataQueryRelaciones relaciones in sujeto.Value)
                {
                    foreach (Datos relaciones2 in relaciones.idRelacionados)
                    {
                        if (!numRelaciones.ContainsKey(relaciones2.idRelacionado))
                        {
                            numRelaciones.Add(relaciones2.idRelacionado, 0);
                        }
                        numRelaciones[sujeto.Key]+= relaciones2.numVeces;
                        numRelaciones[relaciones2.idRelacionado] += relaciones2.numVeces;
                    }
                }
            }
            List<string> itemsSeleccionados = numRelaciones.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Keys.Distinct().ToList();
            if(itemsSeleccionados.Count()> pNumAreas)
            {
                itemsSeleccionados = itemsSeleccionados.GetRange(0, pNumAreas);
            }

            if (itemsSeleccionados.Count > 0)
            {
                //Recuperamos los nombres de caregorías y creamos los nodos
                {
                    string select = $"{mPrefijos} Select ?categoria ?nombreCategoria";
                    string where = $@"  where
                                {{
                                    ?categoria skos:prefLabel ?nombreCategoria.
                                    FILTER(?categoria in(<{string.Join(">,<", itemsSeleccionados)}>))
                                }}";

                    SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                    if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                    {
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            if (!dicNodos.ContainsKey(fila["categoria"].value))
                            {
                                dicNodos.Add(fila["categoria"].value, fila["nombreCategoria"].value);
                            }
                        }
                    }
                }


                // Nodos. 
                if (dicNodos != null && dicNodos.Count > 0)
                {
                    foreach (KeyValuePair<string, string> nodo in dicNodos)
                    {
                        string clave = nodo.Key;
                        Models.Graficas.DataItemRelacion.Data data = new Models.Graficas.DataItemRelacion.Data(clave, nodo.Value, null, null, null, "nodes", Models.Graficas.DataItemRelacion.Data.Type.icon_area);
                        if (scoreNodes.ContainsKey(clave))
                        {
                            data.score = scoreNodes[clave];
                            data.name = data.name + " (" + data.score + ")";
                        }
                        DataItemRelacion dataColabo = new DataItemRelacion(data, true, true);
                        items.Add(dataColabo);
                    }
                }

                // Relaciones.
                if (dicRelaciones != null && dicRelaciones.Count > 0)
                {
                    foreach (KeyValuePair<string, List<DataQueryRelaciones>> sujeto in dicRelaciones)
                    {
                        if (itemsSeleccionados.Contains(sujeto.Key))
                        {
                            foreach (DataQueryRelaciones relaciones in sujeto.Value)
                            {
                                foreach (Datos relaciones2 in relaciones.idRelacionados)
                                {
                                    if (itemsSeleccionados.Contains(relaciones2.idRelacionado))
                                    {
                                        string id = $@"{sujeto.Key}~{relaciones.nombreRelacion}~{relaciones2.idRelacionado}~{relaciones2.numVeces}";
                                        Models.Graficas.DataItemRelacion.Data data = new Models.Graficas.DataItemRelacion.Data(id, relaciones.nombreRelacion, sujeto.Key, relaciones2.idRelacionado, UtilidadesAPI.CalcularGrosor(maximasRelaciones, relaciones2.numVeces), "edges", Models.Graficas.DataItemRelacion.Data.Type.relation_document);
                                        DataItemRelacion dataColabo = new DataItemRelacion(data, null, null);
                                        items.Add(dataColabo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return items;
        }
    }
}
