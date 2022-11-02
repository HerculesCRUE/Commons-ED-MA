using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Hercules.CommonsEDMA.ServicioExterno.Models;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaProyectos;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataItemRelacion;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.GraficaBarras;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesProyecto : GnossGetMainResourceApiDataBase
    {        
        private const string COLOR_GRAFICAS_HORIZONTAL = "#6cafe3";

        /// <summary>
        /// Obtiene los datos para crear la gráfico de los proyectos por año.
        /// </summary>
        /// <param name="pParametros">Filtros aplicados en las facetas.</param>
        public GraficasProyectos GetDatosGraficaProyectos(string pParametros)
        {
            GraficasProyectos graficasProyectos = new GraficasProyectos();


            #region Gráfico de barras
            {
                Dictionary<string, DataFechas> dicResultados = new();
                SparqlObject resultadoQuery = null;
                // Consultas sparql.

                #region --- Obtención de datos del año de inicio de los proyectos
                {
                    string select = $@"{mPrefijos}
                                    SELECT COUNT(DISTINCT(?proyecto)) AS ?numProyectos ?idTipo ?anyoInicio ";
                    int aux = 0;
                    string where = $@"WHERE {{ 
                                    ?proyecto vivo:start ?fecha
                                    BIND( xsd:int(?fecha/10000000000) AS ?anyoInicio)
                                    OPTIONAL{{
                                        ?proyecto roh:scientificExperienceProject ?tipo.
                                        ?tipo dc:identifier ?idTipo
                                    }}
                                    {UtilidadesAPI.CrearFiltros(UtilidadesAPI.ObtenerParametros(pParametros), "?proyecto", ref aux)}
                                }}ORDER BY ?anyoInicio";
                    resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

                    if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                    {
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string anyo = UtilidadesAPI.GetValorFilaSparqlObject(fila, "anyoInicio");
                            int numProyectos = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numProyectos"));
                            string tipo = UtilidadesAPI.GetValorFilaSparqlObject(fila, "idTipo");
                            if (tipo == null)
                            {
                                tipo = "";
                            }
                            if (!dicResultados.ContainsKey(anyo))
                            {
                                // Si no contiene el año, creo el objeto.
                                DataFechas data = new();
                                data.numProyectosInicio = new Dictionary<string, int>() { { tipo, numProyectos } };
                                data.numProyectosFin = 0;
                                dicResultados.Add(anyo, data);
                            }
                            else
                            {
                                // Si lo contiene, se lo agrego.
                                dicResultados[anyo].numProyectosInicio[tipo] = numProyectos;
                            }
                        }
                    }
                }
                #endregion

                #region --- Obtención de datos del año de fin de los proyectos
                {
                    string select = $@"{mPrefijos}
                                    SELECT COUNT(DISTINCT(?proyecto)) AS ?numProyectos ?anyoFin ";
                    int aux = 0;
                    string where = $@"WHERE {{ 
                                    ?proyecto vivo:end ?fecha
                                    BIND( xsd:int(?fecha/10000000000) AS ?anyoFin)
                                    {UtilidadesAPI.CrearFiltros(UtilidadesAPI.ObtenerParametros(pParametros), "?proyecto", ref aux)}
                                }}ORDER BY ?anyoFin";
                    resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

                    if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                    {
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string anyo = UtilidadesAPI.GetValorFilaSparqlObject(fila, "anyoFin");
                            int numProyectos = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numProyectos"));

                            if (!dicResultados.ContainsKey(anyo))
                            {
                                // Si no contiene el año, creo el objeto.
                                DataFechas data = new();
                                data.numProyectosInicio = new Dictionary<string, int>();
                                data.numProyectosFin = numProyectos;
                                dicResultados.Add(anyo, data);
                            }
                            else
                            {
                                // Si lo contiene, se lo agrego.
                                dicResultados[anyo].numProyectosFin = numProyectos;
                            }
                        }
                    }
                }
                #endregion

                // Rellenar años intermedios y ordenarlos.
                if (dicResultados.Count > 0)
                {
                    int inicio = dicResultados.Keys.Select(x => int.Parse(x)).Min();
                    int fin = dicResultados.Keys.Select(x => int.Parse(x)).Max();
                    for (int i = inicio; i < fin; i++)
                    {
                        if (!dicResultados.ContainsKey(i.ToString()))
                        {
                            dicResultados.Add(i.ToString(), new DataFechas() { numProyectosInicio = new Dictionary<string, int>(), numProyectosFin = 0 });
                        }
                    }
                }
                dicResultados = dicResultados.OrderBy(item => item.Key).ToDictionary((keyItem) => keyItem.Key, (valueItem) => valueItem.Value);

                // Se coge los datos del número de proyectos.
                List<int> listaIniciosCompetitivos = new();
                List<int> listaIniciosNoCompetitivos = new();
                List<int> listaIniciosOtros = new();

                List<int> listaFines = new();
                foreach (KeyValuePair<string, DataFechas> item in dicResultados)
                {
                    if (item.Value.numProyectosInicio.Any())
                    {
                        if (item.Value.numProyectosInicio.ContainsKey("SEP1"))
                        {
                            listaIniciosCompetitivos.Add(item.Value.numProyectosInicio["SEP1"]);
                            listaIniciosNoCompetitivos.Add(0);
                            listaIniciosOtros.Add(0);
                        }
                        if (item.Value.numProyectosInicio.ContainsKey("SEP2"))
                        {
                            listaIniciosCompetitivos.Add(0);
                            listaIniciosNoCompetitivos.Add(item.Value.numProyectosInicio["SEP2"]);
                            listaIniciosOtros.Add(0);
                        }
                        if (item.Value.numProyectosInicio.ContainsKey(""))
                        {
                            listaIniciosCompetitivos.Add(0);
                            listaIniciosNoCompetitivos.Add(0);
                            listaIniciosOtros.Add(item.Value.numProyectosInicio[""]);
                        }
                    }
                    else
                    {
                        listaIniciosCompetitivos.Add(0);
                        listaIniciosNoCompetitivos.Add(0);
                        listaIniciosOtros.Add(0);
                    }

                    listaFines.Add(item.Value.numProyectosFin);
                }

                // Se construye el objeto con los datos.
                List<DatosBarra> listaDatos = new List<DatosBarra>();
                listaDatos.Add(new DatosBarra("Inicio competitivos", "#6cafd3", listaIniciosCompetitivos, 1, "inicio"));
                listaDatos.Add(new DatosBarra("Inicio no competitivos", "#7cbfe3", listaIniciosNoCompetitivos, 1, "inicio"));
                if (listaIniciosOtros.Exists(x => x > 0))
                {
                    listaDatos.Add(new DatosBarra("Inicio otros", "#8ccff3", listaIniciosOtros, 1, "inicio"));
                }
                listaDatos.Add(new DatosBarra("Fin", "#BF4858", listaFines, 0.2f, null));
                // Se crea el objeto de la gráfica.
                graficasProyectos.graficaBarrasAnios = new GraficaBarras(new DataGraficaBarras(dicResultados.Keys.ToList(), listaDatos));
            }
            #endregion

            #region Gráfico de sectores
            {
                Dictionary<string, KeyValuePair<string, int>> dicAmbitos = new Dictionary<string, KeyValuePair<string, int>>();

                SparqlObject resultadoQuery = null;
                // Lista a ordenar: Internacional, Unión Europea, Nacional, Autonómica/Regional, Otros/Propio
                ArrayList ambitosOrdenados = new ArrayList() { "Internacional", "Unión Europea", "Nacional", "Regional", "Autonómica", "Autonómica/Regional", "Otros/Propio", "Otros", "Propio" };

                // Consultas sparql.

                #region --- Obtención de ámbitos
                {
                    string select = $@"{mPrefijos}
                                    SELECT COUNT(DISTINCT(?proyecto)) AS ?numProyectos ?ambitoID ?ambitoNombre";
                    int aux = 0;
                    string where = $@"WHERE {{ 
                                    ?proyecto a 'project'.
                                    ?proyecto vivo:geographicFocus ?ambito.
                                    ?ambito dc:identifier ?ambitoID.
                                    ?ambito dc:title ?ambitoNombre.
                                    FILTER(lang(?ambitoNombre)='es')
                                    {UtilidadesAPI.CrearFiltros(UtilidadesAPI.ObtenerParametros(pParametros), "?proyecto", ref aux)}
                                }}ORDER BY desc(?numProyectos)";
                    resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

                    if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                    {
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            dicAmbitos[UtilidadesAPI.GetValorFilaSparqlObject(fila, "ambitoID")] = new KeyValuePair<string, int>(UtilidadesAPI.GetValorFilaSparqlObject(fila, "ambitoNombre"), int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numProyectos")));
                        }
                    }
                }
                #endregion

                // Se ordenan los datos
                List<int> datosBarraItem3 = new List<int>();
                List<string> datosBarraItem3Keys = new List<string>();

                foreach (string ambito in ambitosOrdenados)
                {
                    foreach (var itemAmbito in dicAmbitos)
                    {
                        if (itemAmbito.Value.Key == ambito)
                        {
                            datosBarraItem3.Add(itemAmbito.Value.Value);
                            datosBarraItem3Keys.Add(itemAmbito.Value.Key);
                        }
                    }
                }

                // Se construye el objeto con los datos.
                List<DatosBarra> listaDatos = new List<DatosBarra>();
                listaDatos.Add(new DatosBarra("Proyectos", "#6cafd3", datosBarraItem3, 1, null));
                // Se crea el objeto de la gráfica.
                graficasProyectos.graficaSectoresAmbito = new GraficaBarras(new DataGraficaBarras(datosBarraItem3Keys, listaDatos));

            }
            #endregion

            #region Gráfico de barras miembros
            {
                Dictionary<string, int> dicNumMiembrosProyecto = new Dictionary<string, int>();
                SparqlObject resultadoQuery = null;
                // Consultas sparql.

                #region --- Obtención nº de miembros
                {
                    string select = $@"{mPrefijos}
                                    SELECT ?proyecto COUNT(DISTINCT(?person)) AS ?numMiembros";
                    int aux = 0;
                    string where = $@"WHERE {{ 
                                    ?proyecto a 'project'.
                                    OPTIONAL{{
                                        ?proyecto ?propRol ?rol.
                                        FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                                        ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                    }}
                                    {UtilidadesAPI.CrearFiltros(UtilidadesAPI.ObtenerParametros(pParametros), "?proyecto", ref aux)}
                                }}ORDER BY desc(?numMiembros)";
                    resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

                    if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                    {
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            dicNumMiembrosProyecto[UtilidadesAPI.GetValorFilaSparqlObject(fila, "proyecto")] = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numMiembros"));
                        }
                    }
                }
                #endregion

                Dictionary<string, int> numMiembros = new Dictionary<string, int>();
                numMiembros.Add("1-3", 0);
                numMiembros.Add("4-10", 0);
                numMiembros.Add("11-30", 0);
                numMiembros.Add("30+", 0);

                foreach (KeyValuePair<string, int> item in dicNumMiembrosProyecto)
                {
                    if (item.Value > 0 && item.Value < 4)
                    {
                        numMiembros["1-3"]++;
                    }
                    else if (item.Value >= 4 && item.Value < 11)
                    {
                        numMiembros["4-10"]++;
                    }
                    else if (item.Value >= 11 && item.Value < 31)
                    {
                        numMiembros["11-30"]++;
                    }
                    else if (item.Value >= 31)
                    {
                        numMiembros["30+"]++;
                    }
                }

                // Se construye el objeto con los datos.
                List<DatosBarra> listaDatos = new List<DatosBarra>();
                listaDatos.Add(new DatosBarra("Proyectos", "#BF4858", numMiembros.Values.ToList(), 1, null));
                // Se crea el objeto de la gráfica.
                graficasProyectos.graficaBarrasMiembros = new GraficaBarras(new DataGraficaBarras(numMiembros.Keys.ToList(), listaDatos));

            }
            #endregion

            return graficasProyectos;
        }


        /// <summary>
        /// Obtienes los datos de las pestañas de cada sección de la ficha.
        /// </summary>
        /// <param name="pProyecto">ID del recurso del proyecto.</param>
        /// <returns>Objeto con todos los datos necesarios para crear la gráfica en el JS.</returns>
        public Dictionary<string, int> GetDatosCabeceraProyecto(string pProyecto)
        {
            string idGrafoBusqueda = UtilidadesAPI.ObtenerIdBusqueda(resourceApi, pProyecto);
            Dictionary<string, int> dicResultados = new Dictionary<string, int>();
            

            // Consulta sparql.
            string select = mPrefijos;
            select += "SELECT COUNT(DISTINCT ?persona) AS ?NumParticipantes COUNT(DISTINCT ?documento) AS ?NumPublicaciones COUNT(DISTINCT ?nombreCategoria) AS ?NumCategorias ";
            string where = $@"WHERE {{ 
                                <{idGrafoBusqueda}> vivo:relates ?relacion. 
                                ?relacion <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?persona.
                            }} UNION {{ 
                                ?documento roh:project <{idGrafoBusqueda}>.
                            }} UNION {{ 
                                ?documento roh:project <{idGrafoBusqueda}>. 
                                ?documento roh:hasKnowledgeArea ?area.
                                ?area roh:categoryNode ?categoria.
                                ?categoria skos:prefLabel ?nombreCategoria.
                            }}";

            SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    int numParticipantes = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "NumParticipantes"));
                    int numPublicaciones = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "NumPublicaciones"));
                    int numCategorias = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "NumCategorias"));
                    dicResultados.Add("Participantes", numParticipantes);
                    dicResultados.Add("Publicaciones", numPublicaciones);
                    dicResultados.Add("Categorias", numCategorias);
                }
            }

            return dicResultados;
        }

        /// <summary>
        /// Obtiene los datos para crear el grafo de miembros.
        /// </summary>
        /// <param name="pIdProyecto">ID del recurso del proyecto.</param>
        /// <param name="pParametros">Filtros aplicados en las facetas.</param>
        /// <returns>Objeto con todos los datos necesarios para crear la gráfica en el JS.</returns>
        public List<DataItemRelacion> DatosGraficaRedMiembros(string pIdProyecto, string pParametros)
        {

            HashSet<string> miembros = new HashSet<string>();
            HashSet<string> ip = new HashSet<string>();
            string proyecto = "";

            //Nodos            
            Dictionary<string, string> dicNodos = new Dictionary<string, string>();
            //Relaciones
            Dictionary<string, List<DataQueryRelaciones>> dicRelaciones = new Dictionary<string, List<DataQueryRelaciones>>();
            //Respuesta
            List<DataItemRelacion> items = new List<DataItemRelacion>();

            int aux = 0;
            Dictionary<string, List<string>> dicParametros = UtilidadesAPI.ObtenerParametros(pParametros);
            string filtrosPersonas = UtilidadesAPI.CrearFiltros(dicParametros, "?person", ref aux);


            #region Cargamos nodos
            {

                //Miembros
                string select = $@"{mPrefijos}
                                select distinct ?person ?nombre ?ip";
                string where = $@"
                WHERE {{ 
                        {filtrosPersonas}
                        ?person a 'person'.
                        ?person foaf:name ?nombre.
                        {{
                            <http://gnoss/{pIdProyecto}> <http://w3id.org/roh/researchers> ?main.
                            ?main <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            BIND(false as ?ip)
                        }}UNION
                        {{
                            <http://gnoss/{pIdProyecto}> <http://w3id.org/roh/mainResearchers> ?member.
                            ?member <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            BIND(true as ?ip)
                        }}
                        
                }}";

                SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        if (!dicNodos.ContainsKey(fila["person"].value))
                        {
                            dicNodos.Add(fila["person"].value, fila["nombre"].value);
                        }
                        if (fila.ContainsKey("ip") && (fila["ip"].value == "1" || fila["ip"].value == "true"))
                        {
                            ip.Add(fila["person"].value);
                        }
                        else
                        {
                            miembros.Add(fila["person"].value);
                        }
                    }
                }
                miembros.ExceptWith(ip);
            }
            {
                //Proyecto
                string select = $@"{mPrefijos}
                                select distinct ?nombre";
                string where = $@"
                WHERE {{ 
                        <http://gnoss/{pIdProyecto}> roh:title ?nombre.                        
                }}";

                string nombreProyecto = resourceApi.VirtuosoQuery(select, where, idComunidad).results.bindings.First()["nombre"].value;
                if (nombreProyecto.Length > 20)
                {
                    nombreProyecto = nombreProyecto.Substring(0, 20) + "...";
                }
                proyecto = "http://gnoss/" + pIdProyecto;
                dicNodos.Add("http://gnoss/" + pIdProyecto, nombreProyecto);
            }
            #endregion

            if (miembros.Union(ip).Any())
            {
                #region Relaciones con el proyecto
                {

                    string nombreRelacion = "Proyectos";
                    foreach (var fila in dicNodos)
                    {
                        string person = fila.Key;
                        string project = "http://gnoss/" + pIdProyecto.ToUpper();
                        int numRelaciones = 1;
                        if (!dicRelaciones.ContainsKey(project))
                        {
                            dicRelaciones.Add(project, new List<DataQueryRelaciones>());
                        }

                        DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[project].FirstOrDefault(x => x.nombreRelacion == nombreRelacion));
                        if (dataQueryRelaciones == null)
                        {
                            dataQueryRelaciones = new DataQueryRelaciones()
                            {
                                nombreRelacion = nombreRelacion,
                                idRelacionados = new List<Datos>()
                            };
                            dicRelaciones[project].Add(dataQueryRelaciones);
                        }
                        dataQueryRelaciones.idRelacionados.Add(new Datos()
                        {
                            idRelacionado = person,
                            numVeces = numRelaciones
                        });
                    }
                }
                #endregion

                #region Relaciones entre miembros
                {
                    //Proyectos
                    {
                        string select = "SELECT ?person group_concat(distinct ?project;separator=\",\") as ?projects";
                        string where = $@"
                    WHERE {{ 
                            ?project a 'project'.
                            ?project ?propRol ?rol
                            FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", miembros.Union(ip))}>))
                        }}";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        Dictionary<string, List<string>> personaProy = new Dictionary<string, List<string>>();
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string projects = fila["projects"].value;
                            string person = fila["person"].value;
                            personaProy.Add(person, new List<string>(projects.Split(',')));
                        }
                        UtilidadesAPI.ProcessRelations("Proyectos", personaProy, ref dicRelaciones);
                    }
                    //DOCUMENTOS
                    {
                        string select = "SELECT ?person group_concat(distinct ?document;separator=\",\") as ?documents";
                        string where = $@"
                        WHERE {{ 
                            ?document a 'document'.
                            ?document <http://purl.org/ontology/bibo/authorList> ?authorList.
                            ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", miembros.Union(ip))}>))
                        }}";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        Dictionary<string, List<string>> personaDoc = new Dictionary<string, List<string>>();
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string documents = fila["documents"].value;
                            string person = fila["person"].value;
                            personaDoc.Add(person, new List<string>(documents.Split(',')));
                        }
                        UtilidadesAPI.ProcessRelations("Documentos", personaDoc, ref dicRelaciones);
                    }
                }
                #endregion

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

                // Nodos. 
                if (dicNodos != null && dicNodos.Count > 0)
                {
                    foreach (KeyValuePair<string, string> nodo in dicNodos)
                    {
                        string clave = nodo.Key;
                        Models.Graficas.DataItemRelacion.Data.Type type = Models.Graficas.DataItemRelacion.Data.Type.none;
                        if (ip.Contains(nodo.Key))
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_ip;
                        }
                        else if (miembros.Contains(nodo.Key))
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_member;
                        }
                        else if (proyecto == nodo.Key)
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_project;
                        }
                        Models.Graficas.DataItemRelacion.Data data = new Models.Graficas.DataItemRelacion.Data(clave, nodo.Value, null, null, null, "nodes", type);
                        DataItemRelacion dataColabo = new DataItemRelacion(data, true, true);
                        items.Add(dataColabo);
                    }
                }

                // Relaciones.
                if (dicRelaciones != null && dicRelaciones.Count > 0)
                {
                    foreach (KeyValuePair<string, List<DataQueryRelaciones>> sujeto in dicRelaciones)
                    {
                        foreach (DataQueryRelaciones relaciones in sujeto.Value)
                        {
                            foreach (Datos relaciones2 in relaciones.idRelacionados)
                            {
                                string id = $@"{sujeto.Key}~{relaciones.nombreRelacion}~{relaciones2.idRelacionado}~{relaciones2.numVeces}";
                                Models.Graficas.DataItemRelacion.Data.Type type = Models.Graficas.DataItemRelacion.Data.Type.none;
                                if (relaciones.nombreRelacion == "Proyectos")
                                {
                                    type = Models.Graficas.DataItemRelacion.Data.Type.relation_project;
                                }
                                else if (relaciones.nombreRelacion == "Documentos")
                                {
                                    type = Models.Graficas.DataItemRelacion.Data.Type.relation_document;
                                }
                                Models.Graficas.DataItemRelacion.Data data = new Models.Graficas.DataItemRelacion.Data(id, relaciones.nombreRelacion, sujeto.Key, relaciones2.idRelacionado, UtilidadesAPI.CalcularGrosor(maximasRelaciones, relaciones2.numVeces), "edges", type);
                                DataItemRelacion dataColabo = new DataItemRelacion(data, null, null);
                                items.Add(dataColabo);
                            }
                        }
                    }
                }
            }
            return items;

        }



        /// <summary>
        /// Obtiene los datos para crear el grafo de relaciones con otros investigadores.
        /// </summary>
        /// <param name="pIdProyecto">ID del recurso del proyecto.</param>
        /// <param name="pParametros">Filtros aplicados en las facetas.</param>
        /// <param name="pMax"></param>
        /// <returns>Objeto con todos los datos necesarios para crear la gráfica en el JS.</returns>
        public List<DataItemRelacion> GetDatosGraficaRedColaboradores(string pIdProyecto, string pParametros, int pMax)
        {
            HashSet<string> colaboradores = new HashSet<string>();
            Dictionary<string, int> numRelacionesColaboradorProjecto = new Dictionary<string, int>();
            Dictionary<string, int> numRelacionesColaboradorDocumentoProjecto = new Dictionary<string, int>();
            Dictionary<string, int> numRelacionesColaboradorProyectoProjecto = new Dictionary<string, int>();


            string project = "http://gnoss/" + pIdProyecto.ToUpper();

            //Nodos            
            Dictionary<string, string> dicNodos = new Dictionary<string, string>();
            //Relaciones
            Dictionary<string, List<DataQueryRelaciones>> dicRelaciones = new Dictionary<string, List<DataQueryRelaciones>>();
            //Respuesta
            List<DataItemRelacion> items = new List<DataItemRelacion>();

            int aux = 0;
            Dictionary<string, List<string>> dicParametros = UtilidadesAPI.ObtenerParametros(pParametros);
            string filtrosPersonas = UtilidadesAPI.CrearFiltros(dicParametros, "?person", ref aux);


            #region Cargamos nodos
            {
                //Miembros
                string select = $@"{mPrefijos}
                                select distinct ?person ?nombre";
                string where = $@"
                WHERE {{ 
                        {filtrosPersonas}
                        ?person a 'person'.
                        ?person foaf:name ?nombre.
                }}";

                SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        if (!dicNodos.ContainsKey(fila["person"].value))
                        {
                            dicNodos.Add(fila["person"].value, fila["nombre"].value);
                        }
                        colaboradores.Add(fila["person"].value);
                    }
                }
            }
            {
                //Projecto
                string select = $@"{mPrefijos}
                                select distinct ?nombre";
                string where = $@"
                WHERE {{ 
                        <http://gnoss/{pIdProyecto}> roh:title ?nombre.                        
                }}";

                string nombreProyecto = resourceApi.VirtuosoQuery(select, where, idComunidad).results.bindings.First()["nombre"].value;
                if (nombreProyecto.Length > 20)
                {
                    nombreProyecto = nombreProyecto.Substring(0, 20) + "...";
                }
                dicNodos.Add("http://gnoss/" + pIdProyecto, nombreProyecto);
            }
            #endregion
            if (colaboradores.Count > 0)
            {
                #region Relaciones con el projecto
                {
                    //Proyectos
                    {
                        string select = "SELECT ?person COUNT(distinct ?project) AS ?numRelacionesProyectos";
                        string where = $@"
                        WHERE {{ 
                            ?project a 'project'.
                            ?project ?propRol ?rolProy.
                            FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?rolProy <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}order by desc(?numRelacionesProyectos)";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string person = fila["person"].value;
                            int numRelaciones = int.Parse(fila["numRelacionesProyectos"].value);
                            if (!numRelacionesColaboradorProjecto.ContainsKey(person))
                            {
                                numRelacionesColaboradorProjecto[person] = 0;
                            }
                            if (!numRelacionesColaboradorProyectoProjecto.ContainsKey(person))
                            {
                                numRelacionesColaboradorProyectoProjecto[person] = 0;
                            }
                            numRelacionesColaboradorProjecto[person] += numRelaciones;
                            numRelacionesColaboradorProyectoProjecto[person] += numRelaciones;
                        }
                    }
                    //DOCUMENTOS
                    {
                        string select = "SELECT ?person COUNT(distinct ?document) AS ?numRelacionesDocumentos";
                        string where = $@"
                        WHERE {{ 
                            ?document a 'document'.
                            ?document <http://purl.org/ontology/bibo/authorList> ?authorList.
                            ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}order by desc(?numRelacionesDocumentos)";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string person = fila["person"].value;
                            int numRelaciones = int.Parse(fila["numRelacionesDocumentos"].value);
                            if (!numRelacionesColaboradorProjecto.ContainsKey(person))
                            {
                                numRelacionesColaboradorProjecto[person] = 0;
                            }
                            if (!numRelacionesColaboradorDocumentoProjecto.ContainsKey(person))
                            {
                                numRelacionesColaboradorDocumentoProjecto[person] = 0;
                            }
                            numRelacionesColaboradorProjecto[person] += numRelaciones;
                            numRelacionesColaboradorDocumentoProjecto[person] += numRelaciones;
                        }
                    }
                }
                #endregion

                //Seleccionamos los pMax colaboradores mas relacionados con el Projecto
                numRelacionesColaboradorProjecto = numRelacionesColaboradorProjecto.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                if (numRelacionesColaboradorProjecto.Count > pMax)
                {
                    colaboradores = new HashSet<string>(numRelacionesColaboradorProjecto.Keys.ToList().GetRange(0, pMax));
                    //Eliminamos los nodos que no son necesarios
                    foreach (string idNodo in dicNodos.Keys.ToList())
                    {
                        if (!colaboradores.Contains(idNodo) && idNodo != ("http://gnoss/" + pIdProyecto))
                        {
                            dicNodos.Remove(idNodo);
                        }
                    }
                }
                //Creamos las relaciones entre el Projecto y los colaboradores
                foreach (string colaborador in numRelacionesColaboradorProyectoProjecto.Keys)
                {
                    if (colaboradores.Contains(colaborador))
                    {
                        string nombreRelacion = "Proyectos";
                        if (!dicRelaciones.ContainsKey(project))
                        {
                            dicRelaciones.Add(project, new List<DataQueryRelaciones>());
                        }

                        DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[project].FirstOrDefault(x => x.nombreRelacion == nombreRelacion));
                        if (dataQueryRelaciones == null)
                        {
                            dataQueryRelaciones = new DataQueryRelaciones()
                            {
                                nombreRelacion = nombreRelacion,
                                idRelacionados = new List<Datos>()
                            };
                            dicRelaciones[project].Add(dataQueryRelaciones);
                        }
                        dataQueryRelaciones.idRelacionados.Add(new Datos()
                        {
                            idRelacionado = colaborador,
                            numVeces = numRelacionesColaboradorProyectoProjecto[colaborador]
                        });
                    }
                }
                foreach (string colaborador in numRelacionesColaboradorDocumentoProjecto.Keys)
                {
                    if (colaboradores.Contains(colaborador))
                    {
                        string nombreRelacion = "Documentos";
                        if (!dicRelaciones.ContainsKey(project))
                        {
                            dicRelaciones.Add(project, new List<DataQueryRelaciones>());
                        }

                        DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[project].FirstOrDefault(x => x.nombreRelacion == nombreRelacion));
                        if (dataQueryRelaciones == null)
                        {
                            dataQueryRelaciones = new DataQueryRelaciones()
                            {
                                nombreRelacion = nombreRelacion,
                                idRelacionados = new List<Datos>()
                            };
                            dicRelaciones[project].Add(dataQueryRelaciones);
                        }
                        dataQueryRelaciones.idRelacionados.Add(new Datos()
                        {
                            idRelacionado = colaborador,
                            numVeces = numRelacionesColaboradorDocumentoProjecto[colaborador]
                        });
                    }
                }

                #region Relaciones entre miembros DENTRO DEl PROJECTO
                {
                    //Proyectos
                    {
                        string select = "SELECT ?person group_concat(distinct ?project;separator=\",\") as ?projects";
                        string where = $@"
                        WHERE {{ 
                            ?project a 'project'.
                            ?project ?propRol ?rol.
                            FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        Dictionary<string, List<string>> personaProy = new Dictionary<string, List<string>>();
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string projects = fila["projects"].value;
                            string person = fila["person"].value;
                            personaProy.Add(person, new List<string>(projects.Split(',')));
                        }
                        UtilidadesAPI.ProcessRelations("Proyectos", personaProy, ref dicRelaciones);
                    }
                    //DOCUMENTOS
                    {
                        string select = "SELECT ?person group_concat(?document;separator=\",\") as ?documents";
                        string where = $@"
                        WHERE {{ 
                            ?document a 'document'.
                            ?document <http://purl.org/ontology/bibo/authorList> ?authorList.
                            ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        Dictionary<string, List<string>> personaDoc = new Dictionary<string, List<string>>();
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string documents = fila["documents"].value;
                            string person = fila["person"].value;
                            personaDoc.Add(person, new List<string>(documents.Split(',')));
                        }
                        UtilidadesAPI.ProcessRelations("Documentos", personaDoc, ref dicRelaciones);
                    }
                }
                #endregion



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

                // Nodos. 
                if (dicNodos != null && dicNodos.Count > 0)
                {
                    foreach (KeyValuePair<string, string> nodo in dicNodos)
                    {
                        string clave = nodo.Key;
                        Models.Graficas.DataItemRelacion.Data.Type type = Models.Graficas.DataItemRelacion.Data.Type.none;
                        if (colaboradores.Contains(nodo.Key))
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_member;
                        }
                        else if (project == nodo.Key)
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_project;
                        }
                        Models.Graficas.DataItemRelacion.Data data = new Models.Graficas.DataItemRelacion.Data(clave, nodo.Value, null, null, null, "nodes", type);
                        DataItemRelacion dataColabo = new DataItemRelacion(data, true, true);
                        items.Add(dataColabo);
                    }
                }

                // Relaciones.
                if (dicRelaciones != null && dicRelaciones.Count > 0)
                {
                    foreach (KeyValuePair<string, List<DataQueryRelaciones>> sujeto in dicRelaciones)
                    {
                        foreach (DataQueryRelaciones relaciones in sujeto.Value)
                        {
                            foreach (Datos relaciones2 in relaciones.idRelacionados)
                            {
                                string id = $@"{sujeto.Key}~{relaciones.nombreRelacion}~{relaciones2.idRelacionado}~{relaciones2.numVeces}";
                                Models.Graficas.DataItemRelacion.Data.Type type = Models.Graficas.DataItemRelacion.Data.Type.none;
                                if (relaciones.nombreRelacion == "Proyectos")
                                {
                                    type = Models.Graficas.DataItemRelacion.Data.Type.relation_project;
                                }
                                else if (relaciones.nombreRelacion == "Documentos")
                                {
                                    type = Models.Graficas.DataItemRelacion.Data.Type.relation_document;
                                }
                                Models.Graficas.DataItemRelacion.Data data = new Models.Graficas.DataItemRelacion.Data(id, relaciones.nombreRelacion, sujeto.Key, relaciones2.idRelacionado, UtilidadesAPI.CalcularGrosor(maximasRelaciones, relaciones2.numVeces), "edges", type);
                                DataItemRelacion dataColabo = new DataItemRelacion(data, null, null);
                                items.Add(dataColabo);
                            }
                        }
                    }
                }
            }
            return items;
        }


        /// <summary>
        /// Obtiene los datos para crear la gráfica de las publicaciones (horizontal).
        /// </summary>
        /// <param name="pIdProyecto">ID del recurso del proyecto.</param>
        /// <param name="pParametros">Filtros aplicados en las facetas.</param>
        /// <returns>Objeto con todos los datos necesarios para crear la gráfica en el JS.</returns>
        public DataGraficaAreasTags GetDatosGraficaPublicacionesHorizontal(string pIdProyecto, string pParametros)
        {
            string idGrafoBusqueda = UtilidadesAPI.ObtenerIdBusqueda(resourceApi, pIdProyecto);
            Dictionary<string, int> dicResultados = new Dictionary<string, int>();
            StringBuilder select = new StringBuilder(), where = new StringBuilder();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT ?nombreCategoria COUNT(?nombreCategoria) AS ?numCategorias ");
            where.Append("WHERE { ");
            where.Append($@"?documento roh:project <{idGrafoBusqueda}>. ");
            where.Append("?documento dct:issued ?fecha. ");
            where.Append("?documento roh:hasKnowledgeArea ?area. ");
            where.Append("?area roh:categoryNode ?categoria. ");
            where.Append("?categoria skos:prefLabel ?nombreCategoria. ");
            if (!string.IsNullOrEmpty(pParametros))
            {
                // Creación de los filtros obtenidos por parámetros.
                int aux = 0;
                Dictionary<string, List<string>> dicParametros = UtilidadesAPI.ObtenerParametros(pParametros);
                string filtros = UtilidadesAPI.CrearFiltros(dicParametros, "?documento", ref aux);
                where.Append(filtros);
            }
            where.Append("} ");
            where.Append("GROUP BY (?nombreCategoria) ORDER BY DESC (?numCategorias) ");

            SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    string nombreCategoria = UtilidadesAPI.GetValorFilaSparqlObject(fila, "nombreCategoria");
                    int numCategoria = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numCategorias"));
                    dicResultados.Add(nombreCategoria, numCategoria);
                }
            }

            // Calculo del porcentaje.
            int numTotalCategorias = 0;
            foreach (KeyValuePair<string, int> item in dicResultados)
            {
                numTotalCategorias += item.Value;
            }
            Dictionary<string, double> dicResultadosPorcentaje = new Dictionary<string, double>();
            foreach (KeyValuePair<string, int> item in dicResultados)
            {
                double porcentaje = Math.Round((double)(100 * item.Value) / numTotalCategorias, 2);
                dicResultadosPorcentaje.Add(item.Key, porcentaje);
            }

            // Contruir el objeto de la gráfica.
            List<string> listaColores = UtilidadesAPI.CrearListaColores(dicResultados.Count, COLOR_GRAFICAS_HORIZONTAL);
            Datasets datasets = new Datasets(dicResultadosPorcentaje.Values.ToList(), listaColores);
            Models.Graficas.DataGraficaAreasTags.Data data = new Models.Graficas.DataGraficaAreasTags.Data(dicResultadosPorcentaje.Keys.ToList(), new List<Datasets> { datasets });

            // Máximo.
            x xAxes = new x(new Ticks(0, 100), new ScaleLabel(true, "Percentage"));

            Options options = new Options("y", new Plugins(null, new Legend(false)), new Scales(xAxes));
            DataGraficaAreasTags dataGrafica = new DataGraficaAreasTags("bar", data, options);

            return dataGrafica;
        }
    }
}
