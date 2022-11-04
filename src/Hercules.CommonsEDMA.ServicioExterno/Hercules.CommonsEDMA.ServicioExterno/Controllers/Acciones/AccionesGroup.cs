using Gnoss.ApiWrapper.ApiModel;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Hercules.CommonsEDMA.ServicioExterno.Models;
using Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataItemRelacion;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesGroup : GnossGetMainResourceApiDataBase
    {

        public List<DataItemRelacion> DatosGraficaMiembrosGrupo(string pIdGroup, string pParametros)
        {
            HashSet<string> miembros = new();
            HashSet<string> ip = new();
            string grupo = "http://gnoss/" + pIdGroup;

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



            //Grupo
            dicNodos = GetGrupos(pIdGroup);

            #endregion

            if (miembros.Union(ip).Any())
            {
                #region Relaciones con el grupo
                {
                    //Proyectos
                    dicRelaciones = GetRelacionesGrupoProyectos(pIdGroup, miembros, ip, dicRelaciones);

                    //Documentos
                    dicRelaciones = GetRelacionesGrupoDocumentos(pIdGroup, miembros, ip, dicRelaciones);
                }
                #endregion

                #region Relaciones entre miembros
                //Proyectos
                Dictionary<string, List<string>> personaProy = GetRelacionesMiembrosProyectos(miembros, ip);
                UtilidadesAPI.ProcessRelations("Proyectos", personaProy, ref dicRelaciones);

                //Documentos
                Dictionary<string, List<string>> personaDoc = GetRelacionesMiembrosDocumentos(miembros, ip);
                UtilidadesAPI.ProcessRelations("Documentos", personaDoc, ref dicRelaciones);
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

        public Dictionary<string, List<DataQueryRelaciones>> GetRelacionesGrupoProyectos(string pIdGroup, HashSet<string> miembros,
            HashSet<string> ip, Dictionary<string, List<DataQueryRelaciones>> dicRelaciones)
        {
            string group = "http://gnoss/" + pIdGroup.ToUpper();
            string relacionProy = "Proyectos";

            string selectRGP = "SELECT ?person COUNT(distinct ?project) AS ?numRelacionesProyectos";
            string whereRGP = $@"
                    WHERE {{ 
                            ?project a 'project'.
                            ?project <http://w3id.org/roh/isProducedBy> <http://gnoss/{pIdGroup}>.
                            ?project ?propRol ?rolProy.
                            FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
                            ?rolProy <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", miembros.Union(ip))}>))
                        }}order by desc(?numRelacionesProyectos)";

            SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(selectRGP, whereRGP, idComunidad);
            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
            {
                string person = fila["person"].value;
                int numRelaciones = int.Parse(fila["numRelacionesDocumentos"].value);

                if (!dicRelaciones.ContainsKey(group))
                {
                    dicRelaciones.Add(group, new List<DataQueryRelaciones>());
                }

                DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[group].FirstOrDefault(x => x.nombreRelacion == relacionProy));
                if (dataQueryRelaciones == null)
                {
                    dataQueryRelaciones = new DataQueryRelaciones(relacionProy, new List<Datos>() { { new Datos(person, numRelaciones) } });
                    dicRelaciones[group].Add(dataQueryRelaciones);
                }
            }
            return dicRelaciones;
        }
        public Dictionary<string, List<string>> GetRelacionesMiembrosProyectos(HashSet<string> miembros, HashSet<string> ip)
        {
            Dictionary<string, List<string>> personaProy = new();

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
            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
            {
                string projects = fila["projects"].value;
                string person = fila["person"].value;
                personaProy.Add(person, new List<string>(projects.Split(',')));
            }
            return personaProy;
        }
        public Dictionary<string, List<DataQueryRelaciones>> GetRelacionesGrupoDocumentos(string pIdGroup, HashSet<string> miembros,
            HashSet<string> ip, Dictionary<string, List<DataQueryRelaciones>> dicRelaciones)
        {
            string group = "http://gnoss/" + pIdGroup.ToUpper();
            string relacionDoc = "Documentos";

            string selectRGD = "SELECT ?person COUNT(distinct ?document) AS ?numRelacionesDocumentos";
            string whereRGD = $@"
                    WHERE {{ 
                            ?document a 'document'.
                            ?document <http://w3id.org/roh/isProducedBy> <http://gnoss/{pIdGroup}>.
                            ?document <http://purl.org/ontology/bibo/authorList> ?lista. 
                            ?lista <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", miembros.Union(ip))}>))
                        }}order by desc(?numRelacionesDocumentos)";
            SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(selectRGD, whereRGD, idComunidad);
            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
            {
                string person = fila["person"].value;
                int numRelaciones = int.Parse(fila["numRelacionesDocumentos"].value);

                if (!dicRelaciones.ContainsKey(group))
                {
                    dicRelaciones.Add(group, new List<DataQueryRelaciones>());
                }

                DataQueryRelaciones dataQueryRelaciones = (dicRelaciones[group].FirstOrDefault(x => x.nombreRelacion == relacionDoc));
                if (dataQueryRelaciones == null)
                {
                    dataQueryRelaciones = new DataQueryRelaciones(relacionDoc, new List<Datos>() { new Datos(person, numRelaciones) });
                    dicRelaciones[group].Add(dataQueryRelaciones);
                }
            }
            return dicRelaciones;
        }
        public Dictionary<string, List<string>> GetRelacionesMiembrosDocumentos(HashSet<string> miembros, HashSet<string> ip)
        {
            Dictionary<string, List<string>> personaDoc = new();
            string select = "SELECT ?person group_concat(distinct ?document;separator=\",\") as ?documents";
            string where = $@"
                    WHERE {{ 
                            ?document a 'document'.
                            ?document <http://purl.org/ontology/bibo/authorList> ?authorList.
                            ?authorList <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                            FILTER(?person in (<{string.Join(">,<", miembros.Union(ip))}>))
                        }}";
            SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, idComunidad);
            foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
            {
                string documents = fila["documents"].value;
                string person = fila["person"].value;
                personaDoc.Add(person, new List<string>(documents.Split(',')));
            }
            return personaDoc;
        }

        public Dictionary<string, string> GetGrupos(string pIdGroup)
        {
            Dictionary<string, string> dicNodos = new();
            string select = $@"{mPrefijos}
                                select distinct ?nombre";
            string where = $@"
                WHERE {{ 
                        <http://gnoss/{pIdGroup}> roh:title ?nombre.                        
                }}";
            string nombreGrupo = resourceApi.VirtuosoQuery(select, where, idComunidad).results.bindings.First()["nombre"].value;
            if (nombreGrupo.Length > 20)
            {
                nombreGrupo = nombreGrupo.Substring(0, 20) + "...";
            }
            dicNodos.Add("http://gnoss/" + pIdGroup, nombreGrupo);
            return dicNodos;
        }

        public List<DataItemRelacion> DatosGraficaColaboradoresGrupo(string pIdGroup, string pParametros, int pMax)
        {
            HashSet<string> colaboradores = new();
            string grupo = "http://gnoss/" + pIdGroup;
            Dictionary<string, int> numRelacionesColaboradorGrupo = new();
            Dictionary<string, int> numRelacionesColaboradorDocumentoGrupo = new();
            Dictionary<string, int> numRelacionesColaboradorProyectoGrupo = new();

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
            //Miembros
            CargaNodosColaboradores(filtrosPersonas, colaboradores, dicNodos);

            //Grupo
            GetGruposColaboradores(pIdGroup, dicNodos);

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
                        string nombreRelacion = "Documentos";
                        if (!dicRelaciones.ContainsKey(group))
                        {
                            dicRelaciones.Add(group, new List<DataQueryRelaciones>());
                        }

                        DataQueryRelaciones dataQueryRelaciones = dicRelaciones[group].FirstOrDefault(x => x.nombreRelacion == nombreRelacion);
                        if (dataQueryRelaciones == null)
                        {
                            dataQueryRelaciones = new DataQueryRelaciones(nombreRelacion, new List<Datos>() { new Datos(colaborador, numRelacionesColaboradorDocumentoGrupo[colaborador]) });
                            dicRelaciones[group].Add(dataQueryRelaciones);
                        }
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
                            ?document <http://w3id.org/roh/isProducedBy> <http://gnoss/{pIdGroup}>.
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
                        else if (grupo == nodo.Key)
                        {
                            type = Models.Graficas.DataItemRelacion.Data.Type.icon_project;
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

        public void CargaNodosColaboradores(string filtrosPersonas, HashSet<string> colaboradores, Dictionary<string, string> dicNodos)
        {
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

        public void GetGruposColaboradores(string pIdGroup, Dictionary<string, string> dicNodos)
        {
            string idGroup = "http://gnoss/" + pIdGroup;
            string select = $@"{mPrefijos}
                                select distinct ?nombre ?firstName";
            string where = $@"
                WHERE {{ 
                      OPTIONAL{{<http://gnoss/{pIdGroup}> foaf:firstName ?firstName.}}
                      OPTIONAL{{<http://gnoss/{pIdGroup}> roh:title ?nombre.}}
                }}";

            try
            {
                var bindingRes = resourceApi.VirtuosoQuery(select, where, idComunidad).results.bindings;
                if (bindingRes.First().ContainsKey("nombre") && bindingRes.First()["nombre"].value != "")
                {
                    dicNodos.Add(idGroup, bindingRes.First()["nombre"].value);
                }
                else if (bindingRes.First().ContainsKey("firstName"))
                {
                    dicNodos.Add(idGroup, bindingRes.First()["firstName"].value);
                }
            }
            catch (Exception ex)
            {
                resourceApi.Log.Error("Excepcion: " + ex.Message);
            }
        }




    }
}
