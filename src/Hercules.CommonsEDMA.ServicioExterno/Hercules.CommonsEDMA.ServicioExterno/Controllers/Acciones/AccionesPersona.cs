using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Hercules.CommonsEDMA.ServicioExterno.Models;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataItemRelacion;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesPersona : GnossGetMainResourceApiDataBase
    {
        #region --- Constantes  
        private static string COLOR_GRAFICAS_HORIZONTAL = "#6cafe3";
        #endregion


        /// <summary>
        /// Obtienes los datos de las pestañas de cada sección de la ficha.
        /// </summary>
        /// <param name="pPersona">Nombre de la persona.</param>
        /// <returns>Objeto con todos los datos necesarios para crear la gráfica en el JS.</returns>
        public Dictionary<string, int> GetDatosCabeceraPersona(string pPersona)
        {
            string idGrafoBusqueda = UtilidadesAPI.ObtenerIdBusqueda(resourceApi, pPersona);
            Dictionary<string, int> dicResultados = new();
            SparqlObject resultadoQuery = null;
            StringBuilder select = new(), where = new();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT COUNT(DISTINCT ?proyecto) AS ?NumProyectos COUNT(DISTINCT ?documento) AS ?NumPublicaciones COUNT(DISTINCT ?categoria) AS ?NumCategorias ");
            where.Append("WHERE {{ "); // Total Proyectos.
            where.Append("?proyecto vivo:relates ?relacion. ");
            where.Append("?proyecto gnoss:hasprivacidadCom 'publico'. ");
            where.Append("?relacion <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?persona. ");
            where.Append($@"FILTER(?persona = <{idGrafoBusqueda}>) ");
            where.Append("} UNION { "); // Total Documentos.
            where.Append("?documento bibo:authorList ?listaAutores. ");
            where.Append("?listaAutores rdf:member ?persona. ");
            where.Append($@"FILTER(?persona = <{idGrafoBusqueda}>) ");
            where.Append("} UNION { "); // Total Categorías.
            where.Append("?s ?p ?documentoC. ");
            where.Append("?documentoC bibo:authorList ?listaAutoresC. ");
            where.Append("?listaAutoresC rdf:member ?persona. ");
            where.Append("?documentoC roh:hasKnowledgeArea ?area. ");
            where.Append("?area roh:categoryNode ?categoria. ");
            where.Append($@"FILTER(?persona = <{idGrafoBusqueda}>) ");
            where.Append("}} ");

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    int numProyectos = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "NumProyectos"));
                    int numDocumentos = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "NumPublicaciones"));
                    int numCategorias = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "NumCategorias"));
                    dicResultados.Add("Proyectos", numProyectos);
                    dicResultados.Add("Publicaciones", numDocumentos);
                    dicResultados.Add("Categorias", numCategorias);
                }
            }

            // Consulta sparql.
            select = new StringBuilder();
            where = new StringBuilder();
            select.Append(mPrefijos);
            select.Append("SELECT  COUNT(DISTINCT ?id) AS ?NumColaboradores ");
            where.Append("WHERE {{ ");
            where.Append("SELECT * WHERE { ");
            where.Append("?proyecto vivo:relates ?relacion. ");
            where.Append("?relacion <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?persona. ");
            where.Append($@"FILTER(?persona = <{idGrafoBusqueda}>) ");
            where.Append("?proyecto vivo:relates ?relacion2. ");
            where.Append("?relacion2 <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?id. ");
            where.Append($@"FILTER(?id != <{idGrafoBusqueda}>)}} ");
            where.Append("} UNION { ");
            where.Append("SELECT * WHERE { ");
            where.Append("?documento bibo:authorList ?listaAutores. ");
            where.Append("?listaAutores rdf:member ?persona2. ");
            where.Append($@"FILTER(?persona2 = <{idGrafoBusqueda}>) ");
            where.Append("?documento bibo:authorList ?listaAutores2. ");
            where.Append("?listaAutores2 rdf:member ?id. ");
            where.Append($@"FILTER(?id != <{idGrafoBusqueda}>) ");
            where.Append("}}} ");

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    int numColaboradores = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "NumColaboradores"));
                    dicResultados.Add("Colaboradores", numColaboradores);
                }
            }

            return dicResultados;
        }


        /// <summary>
        /// Obtiene el dato del grupo de investigación el cual pertenece la persona.
        /// </summary>
        /// <param name="pIdPersona">ID del recurso de la persona.</param>
        /// <returns>Lista con los grupos de investigación pertenecientes.</returns>
        public List<string> GetGrupoInvestigacion(string pIdPersona)
        {
            string idGrafoBusqueda = UtilidadesAPI.ObtenerIdBusqueda(resourceApi, pIdPersona);
            List<string> grupos = new();
            SparqlObject resultadoQuery = null;
            StringBuilder select = new(), where = new();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT DISTINCT(?tituloGrupo) ");
            where.Append("WHERE { ");
            where.Append("?s ?p ?grupo. ");
            where.Append("?s foaf:member ?miembros. ");
            where.Append("?s roh:mainResearcher ?investigadorPrincipal. ");
            where.Append("?s roh:title ?tituloGrupo. ");
            where.Append("?miembros roh:roleOf ?persona1. ");
            where.Append("?investigadorPrincipal  roh:roleOf ?persona2. ");
            where.Append("FILTER(?grupo = 'group') ");
            where.Append($@"FILTER(?persona1 LIKE <{idGrafoBusqueda}> || ?persona2 LIKE <{idGrafoBusqueda}>) ");
            where.Append("} ");

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    grupos.Add(UtilidadesAPI.GetValorFilaSparqlObject(fila, "tituloGrupo"));
                }
            }

            return grupos;
        }

        /// <summary>
        /// Obtiene los topics de las publicaciones de la persona.
        /// </summary>
        /// <param name="pIdPersona">ID del recurso de la persona.</param>
        /// <returns>Listado de topics de dicha persona.</returns>
        public List<Dictionary<string, string>> GetTopicsPersona(string pIdPersona)
        {
            string idGrafoBusqueda = UtilidadesAPI.ObtenerIdBusqueda(resourceApi, pIdPersona);
            List<Dictionary<string, string>> categorias = new();
            SparqlObject resultadoQuery = null;
            StringBuilder select = new(), where = new();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("SELECT DISTINCT ?nombreCategoria ?source ?identifier ");
            where.Append("WHERE { ");
            where.Append("?s ?p ?documento. ");
            where.Append("?documento bibo:authorList ?listaAutores. ");
            where.Append("?listaAutores rdf:member ?persona. ");
            where.Append("?documento roh:hasKnowledgeArea ?area. ");
            where.Append("?area roh:categoryNode ?categoria. ");
            where.Append("?categoria skos:prefLabel ?nombreCategoria. ");
            where.Append("?categoria <http://purl.org/dc/elements/1.1/source> ?source. ");
            where.Append("?categoria <http://purl.org/dc/elements/1.1/identifier> ?identifier. ");

            where.Append($@"FILTER(?persona = <{idGrafoBusqueda}>)");
            where.Append("} ");

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    var dictRes = new Dictionary<string, string>();
                    dictRes.Add("nombreCategoria", UtilidadesAPI.GetValorFilaSparqlObject(fila, "nombreCategoria"));
                    dictRes.Add("source", UtilidadesAPI.GetValorFilaSparqlObject(fila, "source"));
                    dictRes.Add("identifier", UtilidadesAPI.GetValorFilaSparqlObject(fila, "identifier"));
                    categorias.Add(dictRes);
                }
            }

            return categorias;
        }

        /// <summary>
        /// Controlador para obtener los datos de la gráfica de nodos de personas.
        /// </summary>
        /// <param name="pIdPersona">ID de la persona en cuestión.</param>
        /// <param name="pParametros">Filtros de la búsqueda.</param>
        /// <param name="pMax">Nº máximo para pintar</param>
        /// <returns>Listado de objetos de la gráfica.</returns>
        public List<DataItemRelacion> GetDatosGraficaRedColaboradoresPersonas(string pIdPersona, string pParametros, int pMax)
        {
            HashSet<string> colaboradores = new();
            Dictionary<string, int> numRelacionesColaboradorPersona = new();
            Dictionary<string, int> numRelacionesColaboradorDocumentoPersona = new();
            Dictionary<string, int> numRelacionesColaboradorProyectoPersona = new();

            //Nodos            
            Dictionary<string, string> dicNodos = new();
            //Relaciones
            Dictionary<string, List<DataQueryRelaciones>> dicRelaciones = new();
            //Respuesta
            List<DataItemRelacion> items = new();

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
                //Persona
                string select = $@"{mPrefijos}
                                select distinct ?nombre ?firstName";
                string where = $@"
                WHERE {{ 
                      OPTIONAL{{<http://gnoss/{pIdPersona}> foaf:firstName ?firstName.}}
                      OPTIONAL{{<http://gnoss/{pIdPersona}> foaf:name ?nombre.}}
                }}";

                string nombreGrupo = "";
                try
                {
                    var bindingRes = resourceApi.VirtuosoQuery(select, where, idComunidad).results.bindings;
                    if (bindingRes.First().ContainsKey("nombre") && bindingRes.First()["nombre"].value != "")
                    {
                        nombreGrupo = bindingRes.First()["nombre"].value;
                    }
                    else if (bindingRes.First().ContainsKey("firstName"))
                    {
                        nombreGrupo = bindingRes.First()["firstName"].value;
                    }
                }
                catch (Exception ex)
                {
                    resourceApi.Log.Error("Excepcion: " + ex.Message);
                }
                dicNodos.Add("http://gnoss/" + pIdPersona, nombreGrupo);
            }
            #endregion

            if (colaboradores.Count > 0)
            {
                #region Relaciones con la persona
                {
                    //Proyectos
                    {
                        string select = "SELECT ?person COUNT(distinct ?project) AS ?numRelacionesProyectos";
                        string where = $@"
                    WHERE {{ 
                            ?project a 'project'.
					        ?project ?propRolA ?roleA.
                            FILTER(?propRolA in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?roleA <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> <http://gnoss/{pIdPersona}>.
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
                            if (!numRelacionesColaboradorPersona.ContainsKey(person))
                            {
                                numRelacionesColaboradorPersona[person] = 0;
                            }
                            if (!numRelacionesColaboradorProyectoPersona.ContainsKey(person))
                            {
                                numRelacionesColaboradorProyectoPersona[person] = 0;
                            }
                            numRelacionesColaboradorPersona[person] += numRelaciones;
                            numRelacionesColaboradorProyectoPersona[person] += numRelaciones;
                        }
                    }
                    //DOCUMENTOS
                    {
                        string select = "SELECT ?person COUNT(distinct ?documento) AS ?numRelacionesDocumentos";
                        string where = $@"
                    WHERE {{ 
                            ?documento a 'document'.
                            ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutoresA.
					        ?listaAutoresA <http://www.w3.org/1999/02/22-rdf-syntax-ns#member><http://gnoss/{pIdPersona}>.
                            ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutoresB.
					        ?listaAutoresB <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}order by desc(?numRelacionesDocumentos)";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string person = fila["person"].value;
                            int numRelaciones = int.Parse(fila["numRelacionesDocumentos"].value);
                            if (!numRelacionesColaboradorPersona.ContainsKey(person))
                            {
                                numRelacionesColaboradorPersona[person] = 0;
                            }
                            if (!numRelacionesColaboradorDocumentoPersona.ContainsKey(person))
                            {
                                numRelacionesColaboradorDocumentoPersona[person] = 0;
                            }
                            numRelacionesColaboradorPersona[person] += numRelaciones;
                            numRelacionesColaboradorDocumentoPersona[person] += numRelaciones;
                        }
                    }
                }
                #endregion

                //Seleccionamos los pMax colaboradores mas relacionados con el grupo
                numRelacionesColaboradorPersona = numRelacionesColaboradorPersona.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                if (numRelacionesColaboradorPersona.Count > pMax)
                {
                    colaboradores = new HashSet<string>(numRelacionesColaboradorPersona.Keys.ToList().GetRange(0, pMax));
                    //Eliminamos los nodos que no son necesarios
                    foreach (string idNodo in dicNodos.Keys.ToList())
                    {
                        if (!colaboradores.Contains(idNodo) && idNodo != ("http://gnoss/" + pIdPersona))
                        {
                            dicNodos.Remove(idNodo);
                        }
                    }
                }
                //Creamos las relaciones entre la persona y los colaboradores
                foreach (string colaborador in numRelacionesColaboradorProyectoPersona.Keys)
                {
                    if (colaboradores.Contains(colaborador))
                    {
                        string person = "http://gnoss/" + pIdPersona.ToUpper();
                        string nombreRelacion = "Proyectos";
                        if (!dicRelaciones.ContainsKey(person))
                        {
                            dicRelaciones.Add(person, new List<DataQueryRelaciones>());
                        }

                        DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[person].FirstOrDefault(x => x.nombreRelacion == nombreRelacion));
                        if (dataQueryRelaciones == null)
                        {
                            dataQueryRelaciones = new DataQueryRelaciones()
                            {
                                nombreRelacion = nombreRelacion,
                                idRelacionados = new List<Datos>()
                            };
                            dicRelaciones[person].Add(dataQueryRelaciones);
                        }
                        dataQueryRelaciones.idRelacionados.Add(new Datos()
                        {
                            idRelacionado = colaborador,
                            numVeces = numRelacionesColaboradorProyectoPersona[colaborador]
                        });
                    }
                }
                foreach (string colaborador in numRelacionesColaboradorDocumentoPersona.Keys)
                {
                    if (colaboradores.Contains(colaborador))
                    {
                        string person = "http://gnoss/" + pIdPersona.ToUpper();
                        string nombreRelacion = "Documentos";
                        if (!dicRelaciones.ContainsKey(person))
                        {
                            dicRelaciones.Add(person, new List<DataQueryRelaciones>());
                        }

                        DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[person].FirstOrDefault(x => x.nombreRelacion == nombreRelacion));
                        if (dataQueryRelaciones == null)
                        {
                            dataQueryRelaciones = new DataQueryRelaciones()
                            {
                                nombreRelacion = nombreRelacion,
                                idRelacionados = new List<Datos>()
                            };
                            dicRelaciones[person].Add(dataQueryRelaciones);
                        }
                        dataQueryRelaciones.idRelacionados.Add(new Datos()
                        {
                            idRelacionado = colaborador,
                            numVeces = numRelacionesColaboradorDocumentoPersona[colaborador]
                        });
                    }
                }

                #region Relaciones entre miembros DENTRO DE LAS COLABORACIONES
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
                            ?project ?propRolB ?rolProyB.
                            FILTER(?propRolB in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?rolProyB <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> <http://gnoss/{pIdPersona}>.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        Dictionary<string, List<string>> personaProy = new();
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
                            ?document <http://purl.org/ontology/bibo/authorList> ?authorListB.
                            ?authorListB <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> <http://gnoss/{pIdPersona}>.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        Dictionary<string, List<string>> personaDoc = new();
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
                        else
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_ip;
                        }
                        Models.Graficas.DataItemRelacion.Data data = new(clave, nodo.Value, null, null, null, "nodes", type);
                        DataItemRelacion dataColabo = new(data, true, true);
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
                                Models.Graficas.DataItemRelacion.Data data = new(id, relaciones.nombreRelacion, sujeto.Key, relaciones2.idRelacionado, UtilidadesAPI.CalcularGrosor(maximasRelaciones, relaciones2.numVeces), "edges", type);
                                DataItemRelacion dataColabo = new(data, null, null);
                                items.Add(dataColabo);
                            }
                        }
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Obtiene el objeto de la gráfica horizontal de topics.
        /// </summary>
        /// <param name="pIdPersona">ID del recurso de la persona.</param>
        /// <param name="pParametros">Filtros de las facetas.</param>
        /// <returns>Objeto que se trata en JS para contruir la gráfica.</returns>
        public DataGraficaAreasTags GetDatosGraficaProyectosPersonaHorizontal(string pIdPersona, string pParametros)
        {
            string idGrafoBusqueda = UtilidadesAPI.ObtenerIdBusqueda(resourceApi, pIdPersona);
            Dictionary<string, int> dicResultados = new();
            SparqlObject resultadoQuery = null;
            StringBuilder select = new(), where = new();

            // Consulta sparql.
            select.Append(mPrefijos);
            select.Append("Select ?nombreCategoria count(*) as ?numCategorias ");
            where.Append("where");
            where.Append("{ ");
            where.Append("?s ?p ?documento. ");
            where.Append("?documento bibo:authorList ?listaAutores.");
            where.Append("?listaAutores rdf:member ?persona.");
            where.Append("?documento roh:hasKnowledgeArea ?area. ");
            where.Append("?area roh:categoryNode ?categoria. ");
            where.Append("?categoria skos:prefLabel ?nombreCategoria. ");
            where.Append($@"FILTER(?persona = <{idGrafoBusqueda}>)");
            where.Append("} ");
            where.Append("Group by(?nombreCategoria)");

            if (!string.IsNullOrEmpty(pParametros))
            {
                // Creación de los filtros obtenidos por parámetros.
                int aux = 0;
                Dictionary<string, List<string>> dicParametros = UtilidadesAPI.ObtenerParametros(pParametros);
                string filtros = UtilidadesAPI.CrearFiltros(dicParametros, "?documento", ref aux);
                where.Append(filtros);
            }

            resultadoQuery = resourceApi.VirtuosoQuery(select.ToString(), where.ToString(), idComunidad);

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    string nombreCategoria = UtilidadesAPI.GetValorFilaSparqlObject(fila, "nombreCategoria");
                    int numCategoria = int.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "numCategorias"));
                    dicResultados.Add(nombreCategoria, numCategoria);
                }
            }

            //Ordenar diccionario
            var dicionarioOrdenado = dicResultados.OrderByDescending(x => x.Value);

            // Calculo del porcentaje.
            int numTotalCategorias = 0;
            foreach (KeyValuePair<string, int> item in dicionarioOrdenado)
            {
                numTotalCategorias += item.Value;
            }
            Dictionary<string, double> dicResultadosPorcentaje = new();
            foreach (KeyValuePair<string, int> item in dicionarioOrdenado)
            {
                double porcentaje = Math.Round((double)(100 * item.Value) / numTotalCategorias, 2);
                dicResultadosPorcentaje.Add(item.Key, porcentaje);
            }

            // Contruir el objeto de la gráfica.
            List<string> listaColores = UtilidadesAPI.CrearListaColores(dicionarioOrdenado.Count(), COLOR_GRAFICAS_HORIZONTAL);
            Datasets datasets = new(dicResultadosPorcentaje.Values.ToList(), listaColores);
            Models.Graficas.DataGraficaAreasTags.Data data = new(dicResultadosPorcentaje.Keys.ToList(), new List<Datasets> { datasets });

            // Máximo.
            x xAxes = new(new Ticks(0, 100), new ScaleLabel(true, "Percentage"));

            Options options = new("y", new Plugins(null, new Legend(false)), new Scales(xAxes));
            DataGraficaAreasTags dataGrafica = new("bar", data, options);

            return dataGrafica;
        }



        /// <summary>
        /// Obtiene los topics de las publicaciones de la persona.
        /// </summary>
        /// <param name="pIdGnossUser">ID del recurso de la persona.</param>
        /// <returns>Listado de topics de dicha persona.</returns>
        public Dictionary<string, string> GetInfoHomeEdUser(string pIdGnossUser)
        {
            string idGrafoBusqueda = UtilidadesAPI.ObtenerIdBusqueda(resourceApi, pIdGnossUser);
            Dictionary<string, string> result = new();
            SparqlObject resultadoQuery = null;
            string select = "", where = "";

            select = $@"SELECT distinct ?s COUNT(distinct ?ro) as ?numOtrosR";
            where = $@"WHERE {{
                    ?s a <http://xmlns.com/foaf/0.1/Person>.
	                ?s <http://w3id.org/roh/gnossUser> <http://gnoss/{pIdGnossUser.ToUpper()}>.

                    ?ro a <http://w3id.org/roh/ResearchObject>.
                    ?ro <http://purl.org/ontology/bibo/authorList> ?authorListr.
                    ?authorListr <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?s.
                }}";
            resultadoQuery = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "researchobject" ,"person" });

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    result.Add("numOtrosR", UtilidadesAPI.GetValorFilaSparqlObject(fila, "numOtrosR"));
                }
            }

            select = $@"SELECT distinct ?s COUNT(distinct ?doc2) as ?numPV";
            where = $@"WHERE {{
                    ?s a <http://xmlns.com/foaf/0.1/Person>.

                    ?doc2 a <http://purl.org/ontology/bibo/Document>.
                    ?doc2 <http://purl.org/ontology/bibo/authorList> ?authorList2.
                    ?authorList2 <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?s.
                    ?doc2 <http://w3id.org/roh/isValidated> 'false'.

	                ?s <http://w3id.org/roh/gnossUser> <http://gnoss/{pIdGnossUser.ToUpper()}>
                }}";
            resultadoQuery = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "document" ,"person"});

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    result.Add("numPV", UtilidadesAPI.GetValorFilaSparqlObject(fila, "numPV"));
                }
            }


            select = $@"SELECT distinct ?s COUNT(distinct ?doc) as ?numPublicaciones";
            where = $@"WHERE {{
                    ?s a <http://xmlns.com/foaf/0.1/Person>.

                    ?doc a <http://purl.org/ontology/bibo/Document>.
                    ?doc <http://purl.org/ontology/bibo/authorList> ?authorList.
                    ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?s.

	                ?s <http://w3id.org/roh/gnossUser> <http://gnoss/{pIdGnossUser.ToUpper()}>
                }}";
            resultadoQuery = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "document", "person" });

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    result.Add("numPublicaciones", UtilidadesAPI.GetValorFilaSparqlObject(fila, "numPublicaciones"));
                }
            }

            return result;
        }
    }
}
