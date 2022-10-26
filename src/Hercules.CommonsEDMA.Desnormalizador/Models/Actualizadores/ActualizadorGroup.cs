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
    /// Clase para actualizar propiedades de grupos
    /// </summary>
    public class ActualizadorGroup : ActualizadorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pResourceApi">API Wrapper de GNOSS</param>
        public ActualizadorGroup(ResourceApi pResourceApi) : base(pResourceApi)
        {
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/mainResearchers y http://w3id.org/roh/researchers de los http://xmlns.com/foaf/0.1/Group los miembros de los grupos,
        /// Además desnormalizamos los nombres dentro de la entidad auxiliar http://w3id.org/roh/PersonAux a la que apuntan estas propiedades
        /// No tiene dependencias
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        public void ActualizarMiembros(List<string> pGroups = null)
        {
            string fechaActual = DateTime.UtcNow.ToString("yyyyMMdd000000");

            HashSet<string> filtersActualizarMiembros = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarMiembros.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarMiembros.Count == 0)
            {
                filtersActualizarMiembros.Add("");
            }

            foreach (string filter in filtersActualizarMiembros)
            {

                //Creamos los miembros
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct ?group ?person ?nick ?ip  ";
                    String where = @$"where{{
                                    {filter}
                                    {{
                                        select distinct ?group ?person ?nick ?ip
                                        Where{{
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                                            ?group <http://vivoweb.org/ontology/core#relates> ?member.
                                            ?member <http://w3id.org/roh/roleOf> ?person.
                                            ?member <http://w3id.org/roh/isIP> ?ip.
                                            OPTIONAL{{?member <http://xmlns.com/foaf/0.1/nick> ?nick.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
                                            BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                                            BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                                            FILTER(?fechaPersonaInitAux<={fechaActual} AND ?fechaPersonaEndAux>={fechaActual})
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        {{
                                            ?group <http://w3id.org/roh/mainResearchers> ?mainResearcher.
                                            ?mainResearcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person
                                            BIND('true' as ?ip)
                                        }}
                                        UNION
                                        {{
                                            ?group <http://w3id.org/roh/researchers> ?researcher.
                                            ?researcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person
                                            BIND('false' as ?ip)
                                        }}
                                    }}
                                }}order by desc(?group) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "group");
                    InsercionMiembrosProyectoGrupo(resultado.results.bindings, "group");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Eliminamos los miembros
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct ?group ?propPersonAux ?personAux  ";
                    String where = @$"where{{
                                    {filter}
                                    {{
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        {{
                                            ?group <http://w3id.org/roh/mainResearchers> ?mainResearcher.
                                            ?mainResearcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person
                                            BIND('true' as ?ip)
                                            BIND(?mainResearcher as ?personAux)
                                            BIND(<http://w3id.org/roh/mainResearchers> as ?propPersonAux)
                                        }}
                                        UNION
                                        {{
                                            ?group <http://w3id.org/roh/researchers> ?researcher.
                                            ?researcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person
                                            BIND('false' as ?ip)
                                            BIND(?researcher as ?personAux)
                                            BIND(<http://w3id.org/roh/researchers> as ?propPersonAux)
                                        }}
                                    }}MINUS
                                    {{
                                        select distinct ?group ?person ?nick ?ip
                                        Where{{
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                                            ?group <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                            ?group <http://vivoweb.org/ontology/core#relates> ?member.
                                            ?member <http://w3id.org/roh/roleOf> ?person.
                                            ?member <http://w3id.org/roh/isIP> ?ip.
                                            OPTIONAL{{?member <http://xmlns.com/foaf/0.1/nick> ?nick.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
                                            BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                                            BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                                            FILTER(?fechaPersonaInitAux<={fechaActual} AND ?fechaPersonaEndAux>={fechaActual})
                                        }}
                                    }}                                    
                                }}order by desc(?group) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "group");
                    EliminacionMiembrosProyectoGrupo(resultado.results.bindings, "group");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Asignamos foaf:firstName
                Dictionary<string, string> propiedadesPersonGroup = new Dictionary<string, string>();
                propiedadesPersonGroup["http://xmlns.com/foaf/0.1/firstName"] = "http://xmlns.com/foaf/0.1/firstName";
                propiedadesPersonGroup["http://xmlns.com/foaf/0.1/lastName"] = "http://xmlns.com/foaf/0.1/familyName";
                propiedadesPersonGroup["--"] = "http://w3id.org/roh/secondFamilyName";
                foreach (string propPerson in propiedadesPersonGroup.Keys)
                {
                    string propGroup = propiedadesPersonGroup[propPerson];
                    while (true)
                    {
                        int limit = 500;
                        String select = @"select distinct ?group ?personAux ?propPersonAux ?property ?propertyLoad ?propertyToLoad ";
                        String where = @$"where{{
                                    {filter}
                                    {{
                                        select distinct ?group ?personAux ?propPersonAux ?property ?propertyLoad ?propertyToLoad
                                        Where{{
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                                            ?group <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                            {{
                                                ?group <http://w3id.org/roh/mainResearchers> ?mainResearcher.
                                                ?mainResearcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                OPTIONAL{{?mainResearcher <{propGroup}> ?propertyLoad.}}
                                                BIND(?mainResearcher as ?personAux)
                                                BIND(<http://w3id.org/roh/mainResearchers> as ?propPersonAux)
                                            }}
                                            UNION
                                            {{
                                                ?group <http://w3id.org/roh/researchers> ?researcher.
                                                ?researcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                 OPTIONAL{{?researcher <{propGroup}> ?propertyLoad.}}
                                                BIND(?researcher as ?personAux)
                                                BIND(<http://w3id.org/roh/researchers> as ?propPersonAux)
                                            }}
                                            ?person <{propPerson}> ?propertyToLoad.
                                            BIND(<{propGroup}> as ?property).
                                            FILTER(
                                                    (!BOUND(?propertyLoad) AND BOUND(?propertyToLoad)) 
                                                    OR 
                                                    (BOUND(?propertyLoad) AND !BOUND(?propertyToLoad)) 
                                                    OR 
                                                    (str(?propertyLoad) != str(?propertyToLoad))
                                            )
                                        }}
                                    }}
                                }}order by desc(?group) limit {limit}";
                        SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "group", "person" });
                        ActualizarPropiedadMiembrosProyectoGrupoPatente(resultado.results.bindings, "group");
                        if (resultado.results.bindings.Count != limit)
                        {
                            break;
                        }
                    }
                }
            }

        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/membersGroup de los http://xmlns.com/foaf/0.1/Group
        /// todos los miembros del grupo 
        /// Depende de ActualizadorGroup.ActualizarMiembros
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        public void ActualizarMiembrosUnificados(List<string> pGroups = null)
        {
            HashSet<string> filtersActualizarMiembrosUnificados = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarMiembrosUnificados.Add($" FILTER(?group in(<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarMiembrosUnificados.Count == 0)
            {
                filtersActualizarMiembrosUnificados.Add("");
            }
            foreach (string filter in filtersActualizarMiembrosUnificados)
            {
                //Creamos los miembros
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct ?group ?person ";
                    String where = @$"where{{
                                    {filter}
                                    {{                                        
                                        select distinct ?group ?person
                                        Where{{                               
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.                                                 
                                            ?group ?propRol ?rol.
                                            FILTER(?propRol in ( <http://w3id.org/roh/mainResearchers>, <http://w3id.org/roh/researchers>))
                                            ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person    
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        ?group <http://w3id.org/roh/membersGroup> ?person.
                                    }}
                                }}order by desc(?group) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "group");
                    InsercionMultiple(resultado.results.bindings, "http://w3id.org/roh/membersGroup", "group", "person");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Eliminamos los miembros
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct ?group ?person ";
                    String where = @$"where{{
                                    {filter}
                                    {{         
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        ?group <http://w3id.org/roh/membersGroup> ?person.                                                      
                                    }}
                                    MINUS
                                    {{
                                        select distinct ?group ?person
                                        Where{{                               
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                                            ?group ?propRol ?rol.
                                            FILTER(?propRol in ( <http://w3id.org/roh/mainResearchers>, <http://w3id.org/roh/researchers>))
                                            ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person      
                                        }}
                                    }}
                                }}order by desc(?group) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "group");
                    EliminacionMultiple(resultado.results.bindings, "http://w3id.org/roh/membersGroup", "group", "person");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/isValidated de los http://xmlns.com/foaf/0.1/Group
        /// los grupos validados (son los proyectos oficiales, es decir, los que tienen http://w3id.org/roh/crisIdentifier)
        /// Esta propiedad se utilizará como filtro en el bucador general de grupos
        /// No tiene dependencias
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        public void ActualizarGruposValidados(List<string> pGroups = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("group", "http://xmlns.com/foaf/0.1/Group", "http://w3id.org/roh/isValidated");

            HashSet<string> filtersActualizarGruposValidados = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarGruposValidados.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarGruposValidados.Count == 0)
            {
                filtersActualizarGruposValidados.Add("");
            }

            foreach (string filter in filtersActualizarGruposValidados)
            {

                while (true)
                {
                    int limit = 500;
                    String select = @"select ?group ?isValidatedCargado ?isValidatedCargar ";
                    String where = @$"where{{
                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                            {filter}
                            OPTIONAL
                            {{
                              ?group <http://w3id.org/roh/isValidated> ?isValidatedCargado.
                            }}
                            {{
                              select ?group IF(BOUND(?crisIdentifier),'true','false')  as ?isValidatedCargar
                              Where{{                               
                                ?group a <http://xmlns.com/foaf/0.1/Group>.
                                OPTIONAL
                                {{
                                    ?group <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                }}                                
                              }}
                            }}
                            FILTER(?isValidatedCargado!= ?isValidatedCargar OR !BOUND(?isValidatedCargado) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "group");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string group = fila["group"].value;
                        string isValidatedCargar = fila["isValidatedCargar"].value;
                        string isValidatedCargado = "";
                        if (fila.ContainsKey("isValidatedCargado"))
                        {
                            isValidatedCargado = fila["isValidatedCargado"].value;
                        }
                        ActualizadorTriple(group, "http://w3id.org/roh/isValidated", isValidatedCargado, isValidatedCargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/membersNumber de los http://xmlns.com/foaf/0.1/Group 
        /// el nº de miembros 
        /// Depende de ActualizadorGrupos.ActualizarMiembrosUnificados
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        public void ActualizarNumeroMiembros(List<string> pGroups = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("group", "http://xmlns.com/foaf/0.1/Group", "http://w3id.org/roh/membersNumber");

            HashSet<string> filtersActualizarNumeroMiembros = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarNumeroMiembros.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarNumeroMiembros.Count == 0)
            {
                filtersActualizarNumeroMiembros.Add("");
            }

            foreach (string filter in filtersActualizarNumeroMiembros)
            {

                while (true)
                {
                    int limit = 500;
                    String select = @"select ?group ?numMiembrosCargados ?numMiembrosACargar  ";
                    String where = @$"where{{
                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                            {filter}
                            OPTIONAL
                            {{
                              ?group <http://w3id.org/roh/membersNumber> ?numMiembrosCargadosAux. 
                              BIND(xsd:int( ?numMiembrosCargadosAux) as  ?numMiembrosCargados)
                            }}
                            {{
                              select ?group count(distinct ?person) as ?numMiembrosACargar
                              Where{{                               
                                ?group a <http://xmlns.com/foaf/0.1/Group>.
                                ?group <http://w3id.org/roh/membersGroup> ?person.
                              }}Group by ?group 
                            }}
                            FILTER(?numMiembrosCargados!= ?numMiembrosACargar OR !BOUND(?numMiembrosCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "group");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string group = fila["group"].value;
                        string numMiembrosACargar = fila["numMiembrosACargar"].value;
                        string numMiembrosCargados = "";
                        if (fila.ContainsKey("numMiembrosCargados"))
                        {
                            numMiembrosCargados = fila["numMiembrosCargados"].value;
                        }
                        ActualizadorTriple(group, "http://w3id.org/roh/membersNumber", numMiembrosCargados, numMiembrosACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/collaboratorsNumber de los http://xmlns.com/foaf/0.1/Group 
        /// el nº de colaboradores  (personas con autorías en documentos del grupo validados o miembros de proyectos validados del grupo que no son miembros del grupo)
        /// Depende de ActualizadorProject.ActualizarPertenenciaGrupos, ActualizadorDocument.ActualizarPertenenciaGrupos y ActualizadorDocument.ActualizarGruposValidados
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarNumeroColaboradores(List<string> pGroups = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("group", "http://xmlns.com/foaf/0.1/Group", "http://w3id.org/roh/collaboratorsNumber");

            HashSet<string> filtersActualizarNumeroColaboradores = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarNumeroColaboradores.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarNumeroColaboradores.Count == 0)
            {
                filtersActualizarNumeroColaboradores.Add("");
            }

            foreach (string filter in filtersActualizarNumeroColaboradores)
            {

                while (true)
                {
                    int limit = 500;

                    String select = @"select ?group ?numColaboradoresCargados ?numColaboradoresACargar ";
                    String where = @$"where{{
                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                            {filter}
                            OPTIONAL
                            {{
                              ?group <http://w3id.org/roh/collaboratorsNumber> ?numColaboradoresCargadosAux. 
                              BIND(xsd:int( ?numColaboradoresCargadosAux) as  ?numColaboradoresCargados)
                            }}
                            {{
                              select ?group count(distinct ?person) as ?numColaboradoresACargar
                              Where{{                               
                                ?group a <http://xmlns.com/foaf/0.1/Group>.
                                OPTIONAL
                                {{
                                    {{
	                                    SELECT DISTINCT ?person ?group
	                                    WHERE 
	                                    {{	
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                                            ?group  <http://w3id.org/roh/isValidated> 'true'.
		                                    {{
			                                    {{
				                                    #Documentos
				                                    SELECT *
				                                    WHERE {{
					                                    ?documento <http://w3id.org/roh/isProducedBy> ?group.
					                                    ?documento a <http://purl.org/ontology/bibo/Document>.
                                                        ?documento  <http://w3id.org/roh/isValidated> 'true'.
					                                    ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutores.
					                                    ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
				                                    }}
			                                    }} 
			                                    UNION 
			                                    {{
				                                    #Proyectos
				                                    SELECT *
				                                    WHERE {{
					                                    ?proy <http://w3id.org/roh/isProducedBy> ?group.
					                                    ?proy a <http://vivoweb.org/ontology/core#Project>.
                                                        ?proy <http://w3id.org/roh/isValidated> 'true'.
					                                    ?proy ?propRol ?role.
					                                    FILTER(?propRol in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
					                                    ?role <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
				                                    }}
			                                    }}
		                                    }}		
		                                    MINUS
		                                    {{
                                                ?group ?propRolGroup ?roleGroup.
					                            FILTER(?propRolGroup in (<http://w3id.org/roh/researchers>,<http://w3id.org/roh/mainResearchers>))
					                            ?roleGroup <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
		                                    }}
	                                    }}
                                    }}
                                }}                                
                              }}Group by ?group 
                            }}
                            FILTER(?numColaboradoresCargados!= ?numColaboradoresACargar OR !BOUND(?numColaboradoresCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "group", "person", "project", "document" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string group = fila["group"].value;
                        string numColaboradoresACargar = fila["numColaboradoresACargar"].value;
                        string numColaboradoresCargados = "";
                        if (fila.ContainsKey("numColaboradoresCargados"))
                        {
                            numColaboradoresCargados = fila["numColaboradoresCargados"].value;
                        }
                        ActualizadorTriple(group, "http://w3id.org/roh/collaboratorsNumber", numColaboradoresCargados, numColaboradoresACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/projectsNumber de los http://xmlns.com/foaf/0.1/Group 
        /// el nº de proyectos (proyectos que apuntan al grupo con la propiedad 'http://w3id.org/roh/isProducedBy')
        /// Depende de ActualizadorProject.ActualizarPertenenciaGrupos y ActualizadorDocument.ActualizarGruposValidados
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        /// <param name="pProjects">IDs de proyectos</param>
        public void ActualizarNumeroProyectos(List<string> pGroups = null, List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("group", "http://xmlns.com/foaf/0.1/Group", "http://w3id.org/roh/projectsNumber");

            HashSet<string> filtersActualizarNumeroProyectos = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarNumeroProyectos.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarNumeroProyectos.Add($" ?project <http://w3id.org/roh/isProducedBy> ?group.  FILTER(?project in (<{string.Join(">,<", pProjects)}>))");
            }
            if (filtersActualizarNumeroProyectos.Count == 0)
            {
                filtersActualizarNumeroProyectos.Add("");
            }

            foreach (string filter in filtersActualizarNumeroProyectos)
            {

                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?group  ?numProyectosCargados ?numProyectosACargar ";
                    String where = @$"where{{
                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                            {filter}
                            OPTIONAL
                            {{
                              ?group <http://w3id.org/roh/projectsNumber> ?numProyectosCargadosAux. 
                              BIND(xsd:int( ?numProyectosCargadosAux) as  ?numProyectosCargados)
                            }}
                            {{
                              select ?group count(distinct ?proyecto) as ?numProyectosACargar
                              Where{{
                                ?group a <http://xmlns.com/foaf/0.1/Group>.
                                OPTIONAL{{                                 
                                    ?group <http://w3id.org/roh/isValidated> 'true'.
                                    ?proyecto a <http://vivoweb.org/ontology/core#Project>.
                                    ?proyecto <http://w3id.org/roh/isValidated> 'true'.
                                    ?proyecto <http://w3id.org/roh/isProducedBy> ?group.
                                }}                                
                              }}Group by ?group 
                            }}
                            FILTER(?numProyectosCargados!= ?numProyectosACargar OR !BOUND(?numProyectosCargados) )
                        }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "group", "person", "project", });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string grupo = fila["group"].value;
                        string numProyectosACargar = fila["numProyectosACargar"].value;
                        string numProyectosCargados = "";
                        if (fila.ContainsKey("numProyectosCargados"))
                        {
                            numProyectosCargados = fila["numProyectosCargados"].value;
                        }
                        ActualizadorTriple(grupo, "http://w3id.org/roh/projectsNumber", numProyectosCargados, numProyectosACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/publicationsNumber de los http://xmlns.com/foaf/0.1/Group  
        /// el número de publicaciones validadas del grupo (publicaciones que apuntan al grupo con la propiedad 'http://w3id.org/roh/isProducedBy')
        /// Depende de ActualizadorDocument.ActualizarPertenenciaGrupos
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarNumeroPublicaciones(List<string> pGroups = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("group", "http://xmlns.com/foaf/0.1/Group", "http://w3id.org/roh/publicationsNumber");

            HashSet<string> filtersActualizarNumeroPublicaciones = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarNumeroPublicaciones.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarNumeroPublicaciones.Count == 0)
            {
                filtersActualizarNumeroPublicaciones.Add("");
            }

            foreach (string filter in filtersActualizarNumeroPublicaciones)
            {

                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?group  ?numDocumentosCargados ?numDocumentosACargar ";
                    String where = @$"where{{
                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                            {filter}
                            OPTIONAL
                            {{
                              ?group <http://w3id.org/roh/publicationsNumber> ?numDocumentosCargadosAux. 
                              BIND(xsd:int( ?numDocumentosCargadosAux) as  ?numDocumentosCargados)
                            }}
                            {{
                              select ?group count(distinct ?doc) as ?numDocumentosACargar
                              Where{{
                                ?group a <http://xmlns.com/foaf/0.1/Group>.
                                OPTIONAL{{
                                    ?doc a <http://purl.org/ontology/bibo/Document>.
                                    ?doc <http://w3id.org/roh/isProducedBy> ?group.
                                }}
                              }}Group by ?group 
                            }}
                            FILTER(?numDocumentosCargados!= ?numDocumentosACargar OR !BOUND(?numDocumentosCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "group", "document" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string group = fila["group"].value;
                        string numDocumentosACargar = fila["numDocumentosACargar"].value;
                        string numDocumentosCargados = "";
                        if (fila.ContainsKey("numDocumentosCargados"))
                        {
                            numDocumentosCargados = fila["numDocumentosCargados"].value;
                        }
                        ActualizadorTriple(group, "http://w3id.org/roh/publicationsNumber", numDocumentosCargados, numDocumentosACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/themedAreasNumber de los http://xmlns.com/foaf/0.1/Group  
        /// el número de áreas temáticas de las publicaciones validads del grupo, este dato es inferido de las publicaciones validadas que pertenecen al grupo (publicaciones que apuntan al grupo con la propiedad 'http://w3id.org/roh/isProducedBy')
        /// Depende de ActualizadorDocument.ActualizarPertenenciaGrupos, ActualizadorDocument.ActualizarDocumentosValidados y ActualizadorDocument.ActualizarAreasDocumentos
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        public void ActualizarNumeroAreasTematicas(List<string> pGroups = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("group", "http://xmlns.com/foaf/0.1/Group", "http://w3id.org/roh/themedAreasNumber");

            HashSet<string> filtersActualizarNumeroAreasTematicas = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarNumeroAreasTematicas.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarNumeroAreasTematicas.Count == 0)
            {
                filtersActualizarNumeroAreasTematicas.Add("");
            }

            foreach (string filter in filtersActualizarNumeroAreasTematicas)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?group  ?numAreasTematicasCargadas ?numAreasTematicasACargar   ";
                    String where = @$"where{{
                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                            {filter}
                            OPTIONAL
                            {{
                              ?group <http://w3id.org/roh/themedAreasNumber> ?numAreasTematicasCargadasAux. 
                              BIND(xsd:int( ?numAreasTematicasCargadasAux) as  ?numAreasTematicasCargadas)
                            }}
                            {{
                              select ?group count(distinct ?categoria) as ?numAreasTematicasACargar
                              Where{{
                                ?group a <http://xmlns.com/foaf/0.1/Group>.
                                OPTIONAL{{
                                    ?documento a <http://purl.org/ontology/bibo/Document>. 
                                    ?documento <http://w3id.org/roh/isValidated> 'true'.
                                    ?documento <http://w3id.org/roh/isProducedBy> ?group.
                                    ?documento <http://w3id.org/roh/hasKnowledgeArea> ?area.
                                    ?area <http://w3id.org/roh/categoryNode> ?categoria.
                                    ?categoria <http://www.w3.org/2008/05/skos#prefLabel> ?nombreCategoria.
                                    MINUS
                                    {{
                                        ?categoria <http://www.w3.org/2008/05/skos#narrower> ?hijos
                                    }}
                                }}
                              }}Group by ?group 
                            }}
                            FILTER(?numAreasTematicasCargadas!= ?numAreasTematicasACargar OR !BOUND(?numAreasTematicasCargadas) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "group", "document", "taxonomy" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string group = fila["group"].value;
                        string numAreasTematicasACargar = fila["numAreasTematicasACargar"].value;
                        string numAreasTematicasCargadas = "";
                        if (fila.ContainsKey("numAreasTematicasCargadas"))
                        {
                            numAreasTematicasCargadas = fila["numAreasTematicasCargadas"].value;
                        }
                        ActualizadorTriple(group, "http://w3id.org/roh/themedAreasNumber", numAreasTematicasCargadas, numAreasTematicasACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/lineResearch de los http://xmlns.com/foaf/0.1/Group 
        /// la líneas de investigación de sus miembros activos en el momento actual
        /// No tiene dependencias
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        public void ActualizarPertenenciaLineas(List<string> pGroups = null)
        {
            string fechaActual = DateTime.UtcNow.ToString("yyyyMMdd000000");

            HashSet<string> filtersActualizarPertenenciaLineas = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarPertenenciaLineas.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarPertenenciaLineas.Count == 0)
            {
                filtersActualizarPertenenciaLineas.Add("");
            }

            foreach (string filter in filtersActualizarPertenenciaLineas)
            {

                while (true)
                {
                    //Añadimos líneas
                    int limit = 500;
                    String select = @"select distinct ?group  ?linea ";
                    String where = @$"where{{
                                    {filter}
                                    {{
                                        select distinct ?group ?linea
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.  
                                            {{
                                                ?group <http://vivoweb.org/ontology/core#relates> ?rol.
                                                ?rol <http://w3id.org/roh/roleOf> ?person.                                                
                                                OPTIONAL{{?rol <http://vivoweb.org/ontology/core#start> ?startAux.}}
                                                OPTIONAL{{?rol <http://vivoweb.org/ontology/core#end> ?endAux.}}
                                                BIND(IF(BOUND(?endAux),xsd:integer(?endAux) ,30000000000000)  as ?end)
                                                BIND(IF(BOUND(?startAux),xsd:integer(?startAux),10000000000000)  as ?start)
                                                ?rol <http://vivoweb.org/ontology/core#hasResearchArea> ?lineaAux.
                                                ?lineaAux <http://w3id.org/roh/title> ?linea.        
                                                OPTIONAL{{?linea <http://vivoweb.org/ontology/core#start> ?startLineAux.}}
                                                OPTIONAL{{?linea <http://vivoweb.org/ontology/core#end> ?endLineAux.}}
                                                BIND(IF(BOUND(?endLineAux),xsd:integer(?endLineAux) ,30000000000000)  as ?endLine)
                                                BIND(IF(BOUND(?startLineAux),xsd:integer(?startLineAux),10000000000000)  as ?startLine)
                                            }}
                                            FILTER(?start<={fechaActual} AND ?end>={fechaActual} )
                                            FILTER(?startLine<={fechaActual} AND ?endLine>={fechaActual} )
                                            MINUS
                                            {{
                                                ?group a <http://xmlns.com/foaf/0.1/Group>.
                                                ?group <http://w3id.org/roh/lineResearch> ?linea. 
                                            }}
                                        }}
                                    }}                                    
                                }}order by desc(?group) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where,new List<string>() { "group" ,"person"});
                    InsercionMultiple(resultado.results.bindings, $"{GetUrlPrefix("roh")}lineResearch", "group", "linea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Eliminamos líneas
                    int limit = 500;
                    String select = @"select distinct ?group  ?linea ";
                    String where = @$"where{{
                                    {filter}
                                    {{
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        ?group <http://w3id.org/roh/lineResearch> ?linea.                                        
                                    }}
                                    MINUS
                                    {{
                                        select distinct ?person ?linea
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.              
                                            {{
                                                ?group <http://vivoweb.org/ontology/core#relates> ?rol.
                                                ?rol <http://w3id.org/roh/roleOf> ?person.                                                
                                                OPTIONAL{{?rol <http://vivoweb.org/ontology/core#start> ?startAux.}}
                                                OPTIONAL{{?rol <http://vivoweb.org/ontology/core#end> ?endAux.}}
                                                BIND(IF(BOUND(?endAux),xsd:integer(?endAux) ,30000000000000)  as ?end)
                                                BIND(IF(BOUND(?startAux),xsd:integer(?startAux),10000000000000)  as ?start)
                                                ?rol <http://vivoweb.org/ontology/core#hasResearchArea> ?lineaAux.
                                                ?lineaAux <http://w3id.org/roh/title> ?linea.        
                                                OPTIONAL{{?linea <http://vivoweb.org/ontology/core#start> ?startLineAux.}}
                                                OPTIONAL{{?linea <http://vivoweb.org/ontology/core#end> ?endLineAux.}}
                                                BIND(IF(BOUND(?endLineAux),xsd:integer(?endLineAux) ,30000000000000)  as ?endLine)
                                                BIND(IF(BOUND(?startLineAux),xsd:integer(?startLineAux),10000000000000)  as ?startLine)
                                            }}
                                            FILTER(?start<={fechaActual} AND ?end>={fechaActual} )
                                            FILTER(?startLine<={fechaActual} AND ?endLine>={fechaActual} )
                                        }}
                                    }}
                                }}order by desc(?group) limit {limit}";
                    var resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "group", "person" });
                    EliminacionMultiple(resultado.results.bindings, "http://w3id.org/roh/lineResearch", "group", "linea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/hasKnowledgeArea de los http://xmlns.com/foaf/0.1/Group 
        /// las áreas de los miembros actuales del grupo
        /// Depende de ActualizadorGrupos.ActualizarMiembrosUnificados y ActualizadorPerson.ActualizarAreasPersonas
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        public void ActualizarAreasGrupos(List<string> pGroups = null)
        {
            HashSet<string> filtersActualizarAreasGrupos = new HashSet<string>();
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarAreasGrupos.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarAreasGrupos.Count == 0)
            {
                filtersActualizarAreasGrupos.Add("");
            }

            foreach (string filter in filtersActualizarAreasGrupos)
            {

                //Eliminamos las categorías duplicadas
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?group ?categoryNode  ";
                    String where = @$"where{{
                                select distinct ?group ?hasKnowledgeAreaAux  ?categoryNode
                                where{{
                                    {filter}
                                    ?group a <http://xmlns.com/foaf/0.1/Group>.
                                    ?group <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaAux .
                                    ?hasKnowledgeAreaAux <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                               }}
                            }}group by ?group ?categoryNode HAVING (COUNT(*) > 1) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where,new List<string>() { "group","taxonomy" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string group = fila["group"].value;
                        string categoryNode = fila["categoryNode"].value;
                        select = @"select ?group ?hasKnowledgeArea  ?categoryNode ";
                        where = @$"where{{
                                    FILTER(?group=<{group}>)
                                    FILTER(?categoryNode =<{categoryNode}>)
                                    {{ 
                                        select distinct ?group ?hasKnowledgeArea  ?categoryNode
                                        where{{
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                                            ?group <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                                            ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?categoryNode.
                                            MINUS{{
                                                ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                            }}
                                        }}
                                    }}
                                }}";
                        resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "group" ,"taxonomy"});
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
                            var resultadox = mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.RemoveTriples>>() { { mResourceApi.GetShortGuid(group), triplesRemove } });
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
                    String select = @"select distinct * where{select ?group ?categoryNode ";
                    String where = @$"where{{
                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                            {filter}
                            {{
                                select  distinct ?group ?hasKnowledgeAreaPerson ?categoryNode where{{
                                    ?group a <http://xmlns.com/foaf/0.1/Group>.
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    ?group <http://w3id.org/roh/membersGroup> ?person.                            
                                    ?person  <http://vivoweb.org/ontology/core#hasResearchArea> ?hasKnowledgeAreaPerson.
                                    ?hasKnowledgeAreaPerson <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}
                            }}
                            MINUS{{
                                select distinct ?group ?hasKnowledgeArea ?categoryNode 
                                where{{
                                    ?group a <http://xmlns.com/foaf/0.1/Group>.
                                    ?group <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                                    ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?categoryNode
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}
                            }}
                            }}}}order by (?group) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where,new List<string>() { "group" , "person", "taxonomy" });
                    InsertarCategorias(resultado, dicAreasBroader, mResourceApi.GraphsUrl, "group", "http://w3id.org/roh/hasKnowledgeArea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limit = 500;
                    //ELIMINAMOS                    
                    String select = @"select distinct * where{select ?group ?hasKnowledgeArea ";
                    String where = @$"where{{
                            ?group a <http://xmlns.com/foaf/0.1/Group>.
                            {filter}
                            {{
                                select distinct ?group ?hasKnowledgeArea ?categoryNode 
                                where{{
                                    ?group a <http://xmlns.com/foaf/0.1/Group>.
                                    ?group <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeArea.
                                    ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?categoryNode
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}                               
                            }}
                            MINUS{{
                                select  distinct ?group ?hasKnowledgeAreaPerson ?categoryNode where{{
                                    ?group a <http://xmlns.com/foaf/0.1/Group>.
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    ?group <http://w3id.org/roh/membersGroup> ?person.
                                    ?person  <http://vivoweb.org/ontology/core#hasResearchArea> ?hasKnowledgeAreaPerson.
                                    ?hasKnowledgeAreaPerson <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}
                                 
                            }}
                            }}}}order by (?group) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where,new List<string>() { "group", "person", "document", "taxonomy" });
                    EliminarCategorias(resultado, "group", "http://w3id.org/roh/hasKnowledgeArea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }
    }
}
