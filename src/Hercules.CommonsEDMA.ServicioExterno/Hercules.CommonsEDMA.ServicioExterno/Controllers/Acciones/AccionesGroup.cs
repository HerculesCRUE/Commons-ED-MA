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
using System.Threading;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesGroup
    {
        #region --- Constantes   
        private static string RUTA_OAUTH = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config";
        private static ResourceApi mResourceAPI = null;
        private static CommunityApi mCommunityAPI = null;
        private static Guid? mIDComunidad = null;
        private static string RUTA_PREFIJOS = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Models{Path.DirectorySeparatorChar}JSON{Path.DirectorySeparatorChar}prefijos.json";
        private static string mPrefijos = string.Join(" ", JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(RUTA_PREFIJOS)));
        #endregion

        private static ResourceApi resourceApi
        {
            get
            {
                while (mResourceAPI == null)
                {
                    try
                    {
                        mResourceAPI = new ResourceApi(RUTA_OAUTH);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido iniciar ResourceApi");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mResourceAPI;
            }
        }

        private static CommunityApi communityApi
        {
            get
            {
                while (mCommunityAPI == null)
                {
                    try
                    {
                        mCommunityAPI = new CommunityApi(RUTA_OAUTH);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido iniciar CommunityApi");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mCommunityAPI;
            }
        }

        private static Guid idComunidad
        {
            get
            {
                while (!mIDComunidad.HasValue)
                {
                    try
                    {
                        mIDComunidad = communityApi.GetCommunityId();
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido obtener el ID de la comnunidad");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mIDComunidad.Value;
            }
        }


        public List<DataItemRelacion> DatosGraficaMiembrosGrupo(string pIdGroup, string pParametros)
        {
            HashSet<string> miembros = new HashSet<string>();
            HashSet<string> ip = new HashSet<string>();
            string grupo = "";

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
                            <http://gnoss/{pIdGroup}> <http://w3id.org/roh/mainResearchers> ?main.
                            ?main <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            OPTIONAL{{?main <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                            OPTIONAL{{?main <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
                            BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                            BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                            BIND(true as ?ip)
                        }}UNION
                        {{
                            <http://gnoss/{pIdGroup}> <http://w3id.org/roh/researchers> ?member.
                            ?member <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            OPTIONAL{{?person <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                            OPTIONAL{{?person <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
                            BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                            BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                            BIND(false as ?ip)
                        }}
                        FILTER(?fechaPersonaInitAux<={DateTime.Now.ToString("yyyyMMddHHmmss")} AND ?fechaPersonaEndAux>={DateTime.Now.ToString("yyyyMMddHHmmss")} )
                        
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
                //Grupo
                string select = $@"{mPrefijos}
                                select distinct ?nombre";
                string where = $@"
                WHERE {{ 
                        <http://gnoss/{pIdGroup}> roh:title ?nombre.                        
                }}";
                string nombreGrupo = resourceApi.VirtuosoQuery(select, where, idComunidad).results.bindings.First()["nombre"].value.Substring(0, 20) + "...";
                grupo = "http://gnoss/" + pIdGroup;
                dicNodos.Add("http://gnoss/" + pIdGroup, nombreGrupo);
            }
            #endregion

            if (miembros.Union(ip).Count() > 0)
            {
                #region Relaciones con el grupo
                {
                    //Proyectos
                    {
                        string select = "SELECT ?person COUNT(distinct ?project) AS ?numRelacionesProyectos";
                        string where = $@"
                    WHERE {{ 
                            ?project a 'project'.
                            ?project <http://w3id.org/roh/isProducedBy> <http://gnoss/{pIdGroup}>.
                            ?project ?propRol ?rolProy.
                            FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?rolProy <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", miembros.Union(ip))}>))
                        }}order by desc(?numRelacionesProyectos)";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string person = fila["person"].value;
                            string group = "http://gnoss/" + pIdGroup.ToUpper();
                            int numRelaciones = int.Parse(fila["numRelacionesProyectos"].value);
                            string nombreRelacion = "Proyectos";
                            if (!dicRelaciones.ContainsKey(group))
                            {
                                dicRelaciones.Add(group, new List<DataQueryRelaciones>());
                            }

                            DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[group].FirstOrDefault(x => x.nombreRelacion == nombreRelacion));
                            if (dataQueryRelaciones == null)
                            {
                                dataQueryRelaciones = new DataQueryRelaciones()
                                {
                                    nombreRelacion = nombreRelacion,
                                    idRelacionados = new List<Datos>()
                                };
                                dicRelaciones[group].Add(dataQueryRelaciones);
                            }
                            dataQueryRelaciones.idRelacionados.Add(new Datos()
                            {
                                idRelacionado = person,
                                numVeces = numRelaciones
                            });
                        }
                    }
                    //DOCUMENTOS
                    {
                        string select = "SELECT ?person COUNT(distinct ?document) AS ?numRelacionesDocumentos";
                        string where = $@"
                    WHERE {{ 
                            ?document a 'document'.
                            ?document <http://w3id.org/roh/isProducedBy> <http://gnoss/{pIdGroup}>.
                            ?document <http://purl.org/ontology/bibo/authorList> ?lista. 
                            ?lista <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", miembros.Union(ip))}>))
                        }}order by desc(?numRelacionesDocumentos)";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string person = fila["person"].value;
                            string group = "http://gnoss/" + pIdGroup.ToUpper();
                            int numRelaciones = int.Parse(fila["numRelacionesDocumentos"].value);
                            string nombreRelacion = "Documentos";
                            if (!dicRelaciones.ContainsKey(group))
                            {
                                dicRelaciones.Add(group, new List<DataQueryRelaciones>());
                            }

                            DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[group].FirstOrDefault(x => x.nombreRelacion == nombreRelacion));
                            if (dataQueryRelaciones == null)
                            {
                                dataQueryRelaciones = new DataQueryRelaciones()
                                {
                                    nombreRelacion = nombreRelacion,
                                    idRelacionados = new List<Datos>()
                                };
                                dicRelaciones[group].Add(dataQueryRelaciones);
                            }
                            dataQueryRelaciones.idRelacionados.Add(new Datos()
                            {
                                idRelacionado = person,
                                numVeces = numRelaciones
                            });
                        }
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
                        else if (grupo == nodo.Key)
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


        public List<DataItemRelacion> DatosGraficaColaboradoresGrupo(string pIdGroup, string pParametros, int pMax)
        {
            HashSet<string> colaboradores = new HashSet<string>();
            string grupo = "";
            Dictionary<string, int> numRelacionesColaboradorGrupo = new Dictionary<string, int>();
            Dictionary<string, int> numRelacionesColaboradorDocumentoGrupo = new Dictionary<string, int>();
            Dictionary<string, int> numRelacionesColaboradorProyectoGrupo = new Dictionary<string, int>();

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
                //Grupo
                string select = $@"{mPrefijos}
                                select distinct ?nombre ?firstName";
                string where = $@"
                WHERE {{ 
                      OPTIONAL{{<http://gnoss/{pIdGroup}> foaf:firstName ?firstName.}}
                      OPTIONAL{{<http://gnoss/{pIdGroup}> roh:title ?nombre.}}
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
                dicNodos.Add("http://gnoss/" + pIdGroup, nombreGrupo);
            }
            #endregion
            if (colaboradores.Count > 0)
            {
                #region Relaciones con el grupo
                {
                    //Proyectos
                    {
                        string select = "SELECT ?person COUNT(distinct ?project) AS ?numRelacionesProyectos";
                        string where = $@"
                    WHERE {{ 
                            ?project a 'project'.
                            ?project <http://w3id.org/roh/isProducedBy> <http://gnoss/{pIdGroup}>.
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
                            if (!numRelacionesColaboradorGrupo.ContainsKey(person))
                            {
                                numRelacionesColaboradorGrupo[person] = 0;
                            }
                            if (!numRelacionesColaboradorProyectoGrupo.ContainsKey(person))
                            {
                                numRelacionesColaboradorProyectoGrupo[person] = 0;
                            }
                            numRelacionesColaboradorGrupo[person] += numRelaciones;
                            numRelacionesColaboradorProyectoGrupo[person] += numRelaciones;
                        }
                    }
                    //DOCUMENTOS
                    {
                        string select = "SELECT ?person COUNT(distinct ?document) AS ?numRelacionesDocumentos";
                        string where = $@"
                    WHERE {{ 
                            ?document a 'document'.
                            ?document <http://w3id.org/roh/isProducedBy> <http://gnoss/{pIdGroup}>.
                            ?document <http://purl.org/ontology/bibo/authorList> ?lista.
                            ?lista <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", colaboradores)}>))
                        }}order by desc(?numRelacionesDocumentos)";
                        SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            string person = fila["person"].value;
                            int numRelaciones = int.Parse(fila["numRelacionesDocumentos"].value);
                            if (!numRelacionesColaboradorGrupo.ContainsKey(person))
                            {
                                numRelacionesColaboradorGrupo[person] = 0;
                            }
                            if (!numRelacionesColaboradorDocumentoGrupo.ContainsKey(person))
                            {
                                numRelacionesColaboradorDocumentoGrupo[person] = 0;
                            }
                            numRelacionesColaboradorGrupo[person] += numRelaciones;
                            numRelacionesColaboradorDocumentoGrupo[person] += numRelaciones;
                        }
                    }
                }
                #endregion

                //Seleccionamos los pMax colaboradores mas relacionados con el grupo
                numRelacionesColaboradorGrupo = numRelacionesColaboradorGrupo.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                if (numRelacionesColaboradorGrupo.Count > pMax)
                {
                    colaboradores = new HashSet<string>(numRelacionesColaboradorGrupo.Keys.ToList().GetRange(0, pMax));
                    //Eliminamos los nodos que no son necesarios
                    foreach (string idNodo in dicNodos.Keys.ToList())
                    {
                        if (!colaboradores.Contains(idNodo) && idNodo != ("http://gnoss/" + pIdGroup))
                        {
                            dicNodos.Remove(idNodo);
                        }
                    }
                }
                //Creamos las relaciones entre el grupo y los colaboradores
                foreach (string colaborador in numRelacionesColaboradorProyectoGrupo.Keys)
                {
                    if (colaboradores.Contains(colaborador))
                    {
                        string group = "http://gnoss/" + pIdGroup.ToUpper();
                        string nombreRelacion = "Proyectos";
                        if (!dicRelaciones.ContainsKey(group))
                        {
                            dicRelaciones.Add(group, new List<DataQueryRelaciones>());
                        }

                        DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[group].FirstOrDefault(x => x.nombreRelacion == nombreRelacion));
                        if (dataQueryRelaciones == null)
                        {
                            dataQueryRelaciones = new DataQueryRelaciones()
                            {
                                nombreRelacion = nombreRelacion,
                                idRelacionados = new List<Datos>()
                            };
                            dicRelaciones[group].Add(dataQueryRelaciones);
                        }
                        dataQueryRelaciones.idRelacionados.Add(new Datos()
                        {
                            idRelacionado = colaborador,
                            numVeces = numRelacionesColaboradorProyectoGrupo[colaborador]
                        });
                    }
                }
                foreach (string colaborador in numRelacionesColaboradorDocumentoGrupo.Keys)
                {
                    if (colaboradores.Contains(colaborador))
                    {
                        string group = "http://gnoss/" + pIdGroup.ToUpper();
                        grupo = "http://gnoss/" + pIdGroup;
                        string nombreRelacion = "Documentos";
                        if (!dicRelaciones.ContainsKey(group))
                        {
                            dicRelaciones.Add(group, new List<DataQueryRelaciones>());
                        }

                        DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[group].FirstOrDefault(x => x.nombreRelacion == nombreRelacion));
                        if (dataQueryRelaciones == null)
                        {
                            dataQueryRelaciones = new DataQueryRelaciones()
                            {
                                nombreRelacion = nombreRelacion,
                                idRelacionados = new List<Datos>()
                            };
                            dicRelaciones[group].Add(dataQueryRelaciones);
                        }
                        dataQueryRelaciones.idRelacionados.Add(new Datos()
                        {
                            idRelacionado = colaborador,
                            numVeces = numRelacionesColaboradorDocumentoGrupo[colaborador]
                        });
                    }
                }

                #region Relaciones entre miembros DENTRO DEl GRUPO
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
                            ?project <http://w3id.org/roh/isProducedBy> <http://gnoss/{pIdGroup}>.
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
                            ?document <http://w3id.org/roh/isProducedBy> <http://gnoss/{pIdGroup}>.
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
                        else if (grupo == nodo.Key)
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_project;
                        }
                        else
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_ip;
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

    }
}
