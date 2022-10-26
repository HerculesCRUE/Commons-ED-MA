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
    /// <summary>
    /// Clase para actualizar propiedades de proyectos
    /// </summary>
    class ActualizadorProject : ActualizadorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pResourceApi">API Wrapper de GNOSS</param>
        public ActualizadorProject(ResourceApi pResourceApi) : base(pResourceApi)
        {
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/isValidated de los http://vivoweb.org/ontology/core#Project 
        /// los proyectos validados (son los proyectos oficiales, es decir, los que tienen http://w3id.org/roh/crisIdentifier)
        /// Esta propiedad se utilizará como filtro en el bucador general de proyectos
        /// No tiene dependencias
        /// </summary>
        /// <param name="pProjects">IDs de los proyectos</param>
        public void ActualizarProyectosValidados(List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("project", $"{GetUrlPrefix("vivo")}Project", $"{GetUrlPrefix("roh")}isValidated");

            HashSet<string> filtersActualizarProyectosValidados = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarProyectosValidados.Add($" FILTER(?project in(<{string.Join(">,<", pProjects)}>))");
            }
            if (filtersActualizarProyectosValidados.Count == 0)
            {
                filtersActualizarProyectosValidados.Add("");
            }
            foreach (string filter in filtersActualizarProyectosValidados)
            {
                while (true)
                {
                    int limitActualizarProyectosValidados = 500;
                    String selectActualizarProyectosValidados = @"select ?project ?isValidatedCargado ?isValidatedCargar ";
                    String whereActualizarProyectosValidados = @$"where{{
                            ?project a <http://vivoweb.org/ontology/core#Project>.
                            {filter}
                            OPTIONAL
                            {{
                              ?project <http://w3id.org/roh/isValidated> ?isValidatedCargado.
                            }}
                            {{
                              select ?project IF(BOUND(?crisIdentifier),'true','false')  as ?isValidatedCargar
                              Where{{                               
                                ?project a <http://vivoweb.org/ontology/core#Project>.
                                OPTIONAL
                                {{
                                    ?project <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                }}                                
                              }}
                            }}
                            FILTER(?isValidatedCargado!= ?isValidatedCargar OR !BOUND(?isValidatedCargado) )
                            }} limit {limitActualizarProyectosValidados}";
                    SparqlObject resultadoActualizarProyectosValidados = mResourceApi.VirtuosoQuery(selectActualizarProyectosValidados, whereActualizarProyectosValidados, "project");

                    Parallel.ForEach(resultadoActualizarProyectosValidados.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string project = fila["project"].value;
                        string isValidatedCargar = fila["isValidatedCargar"].value;
                        string isValidatedCargado = "";
                        if (fila.ContainsKey("isValidatedCargado"))
                        {
                            isValidatedCargado = fila["isValidatedCargado"].value;
                        }
                        ActualizadorTriple(project, $"{GetUrlPrefix("roh")}isValidated", isValidatedCargado, isValidatedCargar);
                    });

                    if (resultadoActualizarProyectosValidados.results.bindings.Count != limitActualizarProyectosValidados)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/mainResearchers y http://w3id.org/roh/researchers de los http://vivoweb.org/ontology/core#Project los miembros de los proyectos,
        /// Además desnormalizamos los nombres dentro de la entidad auxiliar http://w3id.org/roh/PersonAux a la que apuntan estas propiedades
        /// No tiene dependencias
        /// </summary>
        /// <param name="pProjects">IDs de los proyectos</param>
        public void ActualizarMiembros(List<string> pProjects = null)
        {
            string fechaActual = DateTime.UtcNow.ToString("yyyyMMdd000000");

            //Si el proyecto a terminado, han de aparecer todos los participantes que haya tenido,
            //pero si sigue activo solamente los participantes actuales

            HashSet<string> filtersActualizarMiembros = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarMiembros.Add($" FILTER(?project in(<{string.Join(">,<", pProjects)}>))");
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
                    int limitCrearMiembros = 500;
                    String selectCrearMiembros = @"select distinct ?project ?person ?comment ?nick ?nombre ?ip ";
                    String whereCrearMiembros = @$"where{{
                                    {filter}
                                    {{
                                        select distinct ?project ?person ?comment ?nick ?nombre ?ip
                                        Where{{
                                            ?project a <http://vivoweb.org/ontology/core#Project>.
                                            ?project <http://vivoweb.org/ontology/core#relates> ?member.    
                                            ?member <http://w3id.org/roh/roleOf> ?person.                                            
                                            ?person <http://xmlns.com/foaf/0.1/name> ?nombre.                                            
                                            ?member <http://w3id.org/roh/isIP> ?ip.
                                            OPTIONAL{{?member <http://www.w3.org/1999/02/22-rdf-syntax-ns#comment> ?comment.}}
                                            OPTIONAL{{?member <http://xmlns.com/foaf/0.1/nick> ?nick.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}                                            
                                            {{
                                                BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                                                BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                                                FILTER(?fechaPersonaInitAux<={fechaActual} AND ?fechaPersonaEndAux>={fechaActual})
                                            }}UNION
                                            {{
                                                ?project <http://vivoweb.org/ontology/core#end> ?fin.
                                                BIND(xsd:integer(?fin) as ?finAux).
                                                FILTER(?finAux<={fechaActual})
                                            }}
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        ?project a <http://vivoweb.org/ontology/core#Project>.
                                        {{
                                            ?project <http://w3id.org/roh/mainResearchers> ?mainResearcher.
                                            ?mainResearcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person
                                            BIND('true' as ?ip)
                                        }}
                                        UNION
                                        {{
                                            ?project <http://w3id.org/roh/researchers> ?researcher.
                                            ?researcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person
                                            BIND('false' as ?ip)
                                        }}
                                    }}
                                }}order by desc(?project) limit {limitCrearMiembros}";
                    SparqlObject resultadoCrearMiembros = mResourceApi.VirtuosoQueryMultipleGraph(selectCrearMiembros, whereCrearMiembros, new List<string>() { "project", "person" });
                    InsercionMiembrosProyectoGrupo(resultadoCrearMiembros.results.bindings, "project");
                    if (resultadoCrearMiembros.results.bindings.Count != limitCrearMiembros)
                    {
                        break;
                    }
                }

                //Eliminamos los miembros
                while (true)
                {
                    int limitEliminarMiembros = 500;
                    String selectEliminarMiembros = @"select distinct ?project ?propPersonAux ?personAux ";
                    String whereEliminarMiembros = @$"where{{
                                    {filter}
                                    {{
                                        ?project a <http://vivoweb.org/ontology/core#Project>.
                                        {{
                                            ?project <http://w3id.org/roh/mainResearchers> ?mainResearcher.
                                            ?mainResearcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person
                                            BIND('true' as ?ip)
                                            BIND(?mainResearcher as ?personAux)
                                            BIND(<http://w3id.org/roh/mainResearchers> as ?propPersonAux)
                                        }}
                                        UNION
                                        {{
                                            ?project <http://w3id.org/roh/researchers> ?researcher.
                                            ?researcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person
                                            BIND('false' as ?ip)
                                            BIND(?researcher as ?personAux)
                                            BIND(<http://w3id.org/roh/researchers> as ?propPersonAux)
                                        }}
                                    }}MINUS
                                    {{
                                        select distinct ?project ?person ?nick ?ip
                                        Where{{
                                            ?project a <http://vivoweb.org/ontology/core#Project>.
                                            ?project <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                            ?project <http://vivoweb.org/ontology/core#relates> ?member.
                                            ?member <http://w3id.org/roh/roleOf> ?person.
                                            ?member <http://w3id.org/roh/isIP> ?ip.
                                            OPTIONAL{{?member <http://xmlns.com/foaf/0.1/nick> ?nick.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}                                            
                                            {{
                                                BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                                                BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                                                FILTER(?fechaPersonaInitAux<={fechaActual} AND ?fechaPersonaEndAux>={fechaActual})
                                            }}UNION
                                            {{
                                                ?project <http://vivoweb.org/ontology/core#end> ?fin.
                                                BIND(xsd:integer(?fin) as ?finAux).
                                                FILTER(?finAux<={fechaActual})
                                            }}
                                        }}
                                    }}                                    
                                }}order by desc(?project) limit {limitEliminarMiembros}";
                    SparqlObject resultadoEliminarMiembros = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarMiembros, whereEliminarMiembros, new List<string>() { "project", "person" });
                    EliminacionMiembrosProyectoGrupo(resultadoEliminarMiembros.results.bindings, "project");
                    if (resultadoEliminarMiembros.results.bindings.Count != limitEliminarMiembros)
                    {
                        break;
                    }
                }

                //Asignamos foaf:firstName
                Dictionary<string, string> propiedadesPersonProject = new Dictionary<string, string>();
                propiedadesPersonProject[$"{GetUrlPrefix("foaf")}firstName"] = $"{GetUrlPrefix("foaf")}firstName";
                propiedadesPersonProject[$"{GetUrlPrefix("foaf")}lastName"] = $"{GetUrlPrefix("foaf")}familyName";
                propiedadesPersonProject["--"] = $"{GetUrlPrefix("roh")}secondFamilyName";
                foreach (string propPerson in propiedadesPersonProject.Keys)
                {
                    string propProject = propiedadesPersonProject[propPerson];
                    while (true)
                    {
                        int limitAsignarName = 500;
                        String selectAsignarName = @"select distinct ?project ?personAux ?propPersonAux ?property ?propertyLoad ?propertyToLoad ";
                        String whereAsignarName = @$"where{{
                                    {filter}
                                    {{
                                        select distinct ?project ?personAux ?propPersonAux ?property ?propertyLoad ?propertyToLoad
                                        Where{{
                                            ?project a <http://vivoweb.org/ontology/core#Project>.
                                            ?project <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                            {{
                                                ?project <http://w3id.org/roh/mainResearchers> ?mainResearcher.
                                                ?mainResearcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                OPTIONAL{{?mainResearcher <{propProject}> ?propertyLoad.}}
                                                BIND(?mainResearcher as ?personAux)
                                                BIND(<http://w3id.org/roh/mainResearchers> as ?propPersonAux)
                                            }}
                                            UNION
                                            {{
                                                ?project <http://w3id.org/roh/researchers> ?researcher.
                                                ?researcher <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                                 OPTIONAL{{?researcher <{propProject}> ?propertyLoad.}}
                                                BIND(?researcher as ?personAux)
                                                BIND(<http://w3id.org/roh/researchers> as ?propPersonAux)
                                            }}
                                            ?person <{propPerson}> ?propertyToLoad.
                                            BIND(<{propProject}> as ?property).
                                            FILTER(
                                                    (!BOUND(?propertyLoad) AND BOUND(?propertyToLoad)) 
                                                    OR 
                                                    (BOUND(?propertyLoad) AND !BOUND(?propertyToLoad)) 
                                                    OR 
                                                    (str(?propertyLoad) != str(?propertyToLoad))
                                            )
                                        }}
                                    }}
                                }}order by desc(?project) limit {limitAsignarName}";
                        SparqlObject resultadoAsignarName = mResourceApi.VirtuosoQueryMultipleGraph(selectAsignarName, whereAsignarName, new List<string>() { "project", "person" });
                        ActualizarPropiedadMiembrosProyectoGrupoPatente(resultadoAsignarName.results.bindings, "project");
                        if (resultadoAsignarName.results.bindings.Count != limitAsignarName)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/membersProject de los http://vivoweb.org/ontology/core#Project
        /// todos los miembros del proyecto 
        /// Depende de ActualizadorProject.ActualizarMiembros
        /// </summary>
        /// <param name="pProjects">IDs de los proyectos</param>
        public void ActualizarMiembrosUnificados(List<string> pProjects = null)
        {
            HashSet<string> filtersActualizarMiembrosUnificados = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarMiembrosUnificados.Add($" FILTER(?project in(<{string.Join(">,<", pProjects)}>))");
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
                    int limitCrearMiembros = 500;
                    String selectCrearMiembros = @"select distinct ?project ?person ";
                    String whereCrearMiembros = @$"where{{
                                    {filter}
                                    {{                                        
                                        select distinct ?project ?person
                                        Where{{                               
                                            ?project a <http://vivoweb.org/ontology/core#Project>.                                                 
                                            ?project ?propRol ?rol.
                                            FILTER(?propRol in ( <http://w3id.org/roh/mainResearchers>, <http://w3id.org/roh/researchers>))
                                            ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person    
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        ?project a <http://vivoweb.org/ontology/core#Project>.
                                        ?project <http://w3id.org/roh/membersProject> ?person.
                                    }}
                                }}order by desc(?project) limit {limitCrearMiembros}";
                    SparqlObject resultadoCrearMiembros = mResourceApi.VirtuosoQuery(selectCrearMiembros, whereCrearMiembros, "project");
                    InsercionMultiple(resultadoCrearMiembros.results.bindings, $"{GetUrlPrefix("roh")}membersProject", "project", "person");
                    if (resultadoCrearMiembros.results.bindings.Count != limitCrearMiembros)
                    {
                        break;
                    }
                }

                //Eliminamos los miembros
                while (true)
                {
                    int limitEliminarMiembros = 500;
                    String selectEliminarMiembros = @"select distinct ?project ?person ";
                    String whereEliminarMiembros = @$"where{{
                                    {filter}
                                    {{         
                                        ?project a <http://vivoweb.org/ontology/core#Project>.
                                        ?project <http://w3id.org/roh/membersProject> ?person.                                                      
                                    }}
                                    MINUS
                                    {{
                                        select distinct ?project ?person
                                        Where{{                               
                                            ?project a <http://vivoweb.org/ontology/core#Project>.
                                            ?project ?propRol ?rol.
                                            FILTER(?propRol in ( <http://w3id.org/roh/mainResearchers>, <http://w3id.org/roh/researchers>))
                                            ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person      
                                        }}
                                    }}
                                }}order by desc(?project) limit {limitEliminarMiembros}";
                    SparqlObject resultadoEliminarMiembros = mResourceApi.VirtuosoQuery(selectEliminarMiembros, whereEliminarMiembros, "project");
                    EliminacionMultiple(resultadoEliminarMiembros.results.bindings, $"{GetUrlPrefix("roh")}membersProject", "project", "person");
                    if (resultadoEliminarMiembros.results.bindings.Count != limitEliminarMiembros)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/isProducedBy de los http://vivoweb.org/ontology/core#Project validados
        /// los grupos validados a los que pertenecían los miembros  del grupo cuando coinciden en el tiempo su pertenencia en el proyecto y en el grupo
        /// No tiene dependencias
        /// </summary>
        /// <param name="pGroups">IDs de los grupos</param>
        /// <param name="pProjects">IDs de los proyectos</param>
        public void ActualizarPertenenciaGrupos(List<string> pGroups = null, List<string> pProjects = null)
        {
            HashSet<string> filtersActualizarPertenenciaGrupos = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarPertenenciaGrupos.Add($" FILTER(?proyecto in (<{string.Join(">,<", pProjects)}>))");
            }
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersActualizarPertenenciaGrupos.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filtersActualizarPertenenciaGrupos.Count == 0)
            {
                filtersActualizarPertenenciaGrupos.Add("");
            }
            foreach (string filter in filtersActualizarPertenenciaGrupos)
            {
                while (true)
                {
                    //Añadimos a grupos
                    int limitAniadirGrupos = 500;
                    String selectAniadirGrupos = @"select distinct ?proyecto  ?group  ";
                    String whereAniadirGrupos = @$"where{{
                                    {filter}
                                    {{
                                        select ?proyecto ?group
                                        Where{{
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.   
                                            ?group <http://w3id.org/roh/isValidated> 'true'.   
                                            ?proyecto a <http://vivoweb.org/ontology/core#Project>.
                                            ?proyecto <http://w3id.org/roh/isValidated> 'true'.   
                                            ?proyecto <http://vivoweb.org/ontology/core#relates> ?rol.                              
                                            ?rol <http://w3id.org/roh/roleOf> ?person. 
                                            #Fechas proyectos
                                            OPTIONAL{{?proyecto <http://vivoweb.org/ontology/core#start> ?fechaProjInit.}}
                                            OPTIONAL{{?proyecto <http://vivoweb.org/ontology/core#end> ?fechaProjEnd.}} 
                                            BIND(IF(bound(?fechaProjEnd), xsd:integer(?fechaProjEnd), 30000000000000) as ?fechaProjEndAux)
                                            BIND(IF(bound(?fechaProjInit), xsd:integer(?fechaProjInit), 10000000000000) as ?fechaProjInitAux)
                                            #Fechas pertenencias a grupos
                                            ?group <http://vivoweb.org/ontology/core#relates> ?member.
                                            ?member <http://w3id.org/roh/roleOf> ?person. 
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#start> ?fechaGroupInit.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#end> ?fechaGroupEnd.}}
                                            BIND(IF(bound(?fechaGroupEnd), xsd:integer(?fechaGroupEnd), 30000000000000) as ?fechaGroupEndAux)
                                            BIND(IF(bound(?fechaGroupInit), xsd:integer(?fechaGroupInit), 10000000000000) as ?fechaGroupInitAux)
                                            FILTER(?fechaGroupEndAux >= ?fechaProjInitAux AND ?fechaGroupInitAux <= ?fechaProjEndAux)                                                                            
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        ?proyecto a <http://vivoweb.org/ontology/core#Project>.
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        ?proyecto <http://w3id.org/roh/isProducedBy> ?group.
                                    }}
                                }}order by desc(?proyecto) limit {limitAniadirGrupos}";
                    SparqlObject resultadoAniadirGrupos = mResourceApi.VirtuosoQueryMultipleGraph(selectAniadirGrupos, whereAniadirGrupos, new List<string>() { "project", "person", "group" });
                    InsercionMultiple(resultadoAniadirGrupos.results.bindings, $"{GetUrlPrefix("roh")}isProducedBy", "proyecto", "group");
                    if (resultadoAniadirGrupos.results.bindings.Count != limitAniadirGrupos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Eliminamos de grupos
                    int limitEliminarGrupos = 500;
                    String selectEliminarGrupos = @"select distinct ?proyecto  ?group ";
                    String whereEliminarGrupos = @$"where{{
                                    {filter}
                                    {{
                                        ?proyecto a <http://vivoweb.org/ontology/core#Project>.
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        ?proyecto <http://w3id.org/roh/isProducedBy> ?group.                                  
                                    }}
                                    MINUS
                                    {{
                                        select ?proyecto ?group
                                        Where{{
                                            ?group a <http://xmlns.com/foaf/0.1/Group>.   
                                            ?group <http://w3id.org/roh/isValidated> 'true'.   
                                            ?proyecto a <http://vivoweb.org/ontology/core#Project>.
                                            ?proyecto <http://w3id.org/roh/isValidated> 'true'.   
                                            ?proyecto <http://vivoweb.org/ontology/core#relates> ?rol.                    
                                            ?rol <http://w3id.org/roh/roleOf> ?person. 
                                            #Fechas proyectos
                                            OPTIONAL{{?proyecto <http://vivoweb.org/ontology/core#start> ?fechaProjInit.}}
                                            OPTIONAL{{?proyecto <http://vivoweb.org/ontology/core#end> ?fechaProjEnd.}} 
                                            BIND(IF(bound(?fechaProjEnd), xsd:integer(?fechaProjEnd), 30000000000000) as ?fechaProjEndAux)
                                            BIND(IF(bound(?fechaProjInit), xsd:integer(?fechaProjInit), 10000000000000) as ?fechaProjInitAux)
                                            #Fechas pertenencias a grupos
                                            ?group <http://vivoweb.org/ontology/core#relates> ?member.                    
                                            ?member <http://w3id.org/roh/roleOf> ?person.
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#start> ?fechaGroupInit.}}
                                            OPTIONAL{{?member <http://vivoweb.org/ontology/core#end> ?fechaGroupEnd.}}
                                            BIND(IF(bound(?fechaGroupEnd), xsd:integer(?fechaGroupEnd), 30000000000000) as ?fechaGroupEndAux)
                                            BIND(IF(bound(?fechaGroupInit), xsd:integer(?fechaGroupInit), 10000000000000) as ?fechaGroupInitAux)
                                            FILTER(?fechaGroupEndAux >= ?fechaProjInitAux AND ?fechaGroupInitAux <= ?fechaProjEndAux)                                                                            
                                        }}
                                    }}
                                }}order by desc(?proyecto) limit {limitEliminarGrupos}";
                    SparqlObject resultadoEliminarGrupos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarGrupos, whereEliminarGrupos, new List<string>() { "project", "person", "group" });
                    EliminacionMultiple(resultadoEliminarGrupos.results.bindings, $"{GetUrlPrefix("roh")}isProducedBy", "proyecto", "group");
                    if (resultadoEliminarGrupos.results.bindings.Count != limitEliminarGrupos)
                    {
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/themedAreasNumber de los http://vivoweb.org/ontology/core#Project 
        /// el número de áreas temáticas de las publicaciones oficiales del proyecto
        /// Depende de ActualizadorDocument.ActualizarDocumentosValidados y ActualizarDocument.ActualizarAreasDocumentos
        /// </summary>
        /// <param name="pProjects">IDs de los proyectos</param>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarNumeroAreasTematicas(List<string> pProjects = null, List<string> pDocuments = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("project", $"{GetUrlPrefix("vivo")}Project", $"{GetUrlPrefix("roh")}themedAreasNumber");

            HashSet<string> filtersActualizarNumeroAreasTematicas = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarNumeroAreasTematicas.Add($" FILTER(?project in(<{string.Join(">,<", pProjects)}>))");
            }
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersActualizarNumeroAreasTematicas.Add($" ?docAux <http://w3id.org/roh/project> ?project.  FILTER(?docAux in (<{string.Join(">,<", pDocuments)}>))");
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
                    int limitActualizarNumeroAreasTematicas = 500;
                    String selectActualizarNumeroAreasTematicas = @"select ?project  ?numAreasTematicasCargadas ?numAreasTematicasACargar  ";
                    String whereActualizarNumeroAreasTematicas = @$"  where
                                        {{
                                            ?project a <http://vivoweb.org/ontology/core#Project>.
                                            {filter}
                                            OPTIONAL
                                            {{
                                              ?project <http://w3id.org/roh/themedAreasNumber> ?numAreasTematicasCargadasAux. 
                                              BIND(xsd:int( ?numAreasTematicasCargadasAux) as  ?numAreasTematicasCargadas)
                                            }}
                                            {{
                                              select ?project count(distinct ?categoria) as ?numAreasTematicasACargar
                                              Where{{
                                                ?project a <http://vivoweb.org/ontology/core#Project>.
                                                OPTIONAL{{
                                                    ?documento a <http://purl.org/ontology/bibo/Document>. 
                                                    ?documento <http://w3id.org/roh/isValidated> 'true'.
                                                    ?documento <http://w3id.org/roh/project> ?project.
                                                    ?documento <http://w3id.org/roh/hasKnowledgeArea> ?area.
                                                    ?area <http://w3id.org/roh/categoryNode> ?categoria.
                                                    ?categoria <http://www.w3.org/2008/05/skos#prefLabel> ?nombreCategoria.
                                                    MINUS
                                                    {{
                                                        ?categoria <http://www.w3.org/2008/05/skos#narrower> ?hijos
                                                    }}
                                                }}
                                              }}Group by ?project 
                                            }}
                                            FILTER(?numAreasTematicasCargadas!= ?numAreasTematicasACargar OR !BOUND(?numAreasTematicasCargadas) )
                                        }} limit {limitActualizarNumeroAreasTematicas}";
                    SparqlObject resultadoActualizarNumeroAreasTematicas = mResourceApi.VirtuosoQueryMultipleGraph(selectActualizarNumeroAreasTematicas, whereActualizarNumeroAreasTematicas, new List<string>() { "project", "document", "taxonomy" });

                    Parallel.ForEach(resultadoActualizarNumeroAreasTematicas.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string project = fila["project"].value;
                        string numAreasTematicasACargar = fila["numAreasTematicasACargar"].value;
                        string numAreasTematicasCargadas = "";
                        if (fila.ContainsKey("numAreasTematicasCargadas"))
                        {
                            numAreasTematicasCargadas = fila["numAreasTematicasCargadas"].value;
                        }
                        ActualizadorTriple(project, $"{GetUrlPrefix("roh")}themedAreasNumber", numAreasTematicasCargadas, numAreasTematicasACargar);
                    });

                    if (resultadoActualizarNumeroAreasTematicas.results.bindings.Count != limitActualizarNumeroAreasTematicas)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/publicationsNumber de los http://vivoweb.org/ontology/core#Project  
        /// el número de publicaciones validadas del proyecto
        /// Depende de ActualizadorDocument.ActualizarDocumentosValidados
        /// </summary>
        /// <param name="pProjects">IDs de los proyectos</param>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarNumeroPublicaciones(List<string> pProjects = null, List<string> pDocuments = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("project", $"{GetUrlPrefix("vivo")}Project", $"{GetUrlPrefix("roh")}publicationsNumber");

            HashSet<string> filtersActualizarNumeroPublicaciones = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarNumeroPublicaciones.Add($" FILTER(?project in(<{string.Join(">,<", pProjects)}>))");
            }
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersActualizarNumeroPublicaciones.Add($" ?docAux <http://w3id.org/roh/project> ?project.  FILTER(?docAux in (<{string.Join(">,<", pDocuments)}>))");
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
                    int limitActualizarNumeroPublicaciones = 500;
                    String selectActualizarNumeroPublicaciones = @"select ?project  ?numDocumentosCargados ?numDocumentosACargar   ";
                    String whereActualizarNumeroPublicaciones = @$"where{{
                            ?project a <http://vivoweb.org/ontology/core#Project>.
                            {filter}
                            OPTIONAL
                            {{
                              ?project <http://w3id.org/roh/publicationsNumber> ?numDocumentosCargadosAux. 
                              BIND(xsd:int( ?numDocumentosCargadosAux) as  ?numDocumentosCargados)
                            }}
                            {{
                              select ?project count(distinct ?doc) as ?numDocumentosACargar
                              Where{{
                                ?project a <http://vivoweb.org/ontology/core#Project>.
                                OPTIONAL{{
                                    ?doc a <http://purl.org/ontology/bibo/Document>. 
                                    ?doc <http://w3id.org/roh/isValidated> 'true'.
                                    ?doc <http://w3id.org/roh/project> ?project.
                                }}
                              }}Group by ?project 
                            }}
                            FILTER(?numDocumentosCargados!= ?numDocumentosACargar OR !BOUND(?numDocumentosCargados) )
                            }} limit {limitActualizarNumeroPublicaciones}";
                    SparqlObject resultadoActualizarNumeroPublicaciones = mResourceApi.VirtuosoQueryMultipleGraph(selectActualizarNumeroPublicaciones, whereActualizarNumeroPublicaciones, new List<string>() { "project", "document", "person" });

                    Parallel.ForEach(resultadoActualizarNumeroPublicaciones.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string project = fila["project"].value;
                        string numDocumentosACargar = fila["numDocumentosACargar"].value;
                        string numDocumentosCargados = "";
                        if (fila.ContainsKey("numDocumentosCargados"))
                        {
                            numDocumentosCargados = fila["numDocumentosCargados"].value;
                        }
                        ActualizadorTriple(project, $"{GetUrlPrefix("roh")}publicationsNumber", numDocumentosCargados, numDocumentosACargar);
                    });

                    if (resultadoActualizarNumeroPublicaciones.results.bindings.Count != limitActualizarNumeroPublicaciones)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/collaboratorsNumber de los http://vivoweb.org/ontology/core#Project  validados
        /// el nº de colaboradores (personas con autorías en documentos validados del proyecto que no son miembros del proyecto)
        /// Depende de ActualizadorProject.ActualizarMiembrosUnificados y ActualizadorDocument.ActualizarDocumentosValidados
        /// </summary>
        /// <param name="pProjects">IDs de los proyectos</param>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarNumeroColaboradores(List<string> pProjects = null, List<string> pDocuments = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("project", $"{GetUrlPrefix("vivo")}Project", $"{GetUrlPrefix("roh")}collaboratorsNumber");

            HashSet<string> filtersActualizarNumeroColaboradores = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarNumeroColaboradores.Add($" FILTER(?project in(<{string.Join(">,<", pProjects)}>))");
            }
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersActualizarNumeroColaboradores.Add($" ?docAux <http://w3id.org/roh/project> ?project.  FILTER(?docAux in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (filtersActualizarNumeroColaboradores.Count == 0)
            {
                filtersActualizarNumeroColaboradores.Add("");
            }
            foreach (string filter in filtersActualizarNumeroColaboradores)
            {

                while (true)
                {
                    int limitActualizarNumeroColaboradores = 500;
                    String selectActualizarNumeroColaboradores = @"select ?project ?numColaboradoresCargados ?numColaboradoresACargar ";
                    String whereActualizarNumeroColaboradores = @$"where{{
                            ?project a <http://vivoweb.org/ontology/core#Project>.
                            ?project <http://w3id.org/roh/isValidated> 'true'.
                            {filter}
                            OPTIONAL
                            {{
                              ?project <http://w3id.org/roh/collaboratorsNumber> ?numColaboradoresCargadosAux. 
                              BIND(xsd:int( ?numColaboradoresCargadosAux) as  ?numColaboradoresCargados)
                            }}
                            {{
                              select ?project count(distinct ?person) as ?numColaboradoresACargar
                              Where{{                               
                                ?project a <http://vivoweb.org/ontology/core#Project>.
                                OPTIONAL
                                {{
                                    {{
	                                    SELECT DISTINCT ?person ?project
	                                    WHERE 
	                                    {{	
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.
		                                    {{
			                                    {{
				                                    #Documentos
				                                    SELECT *
				                                    WHERE {{
					                                    ?documento <http://w3id.org/roh/project> ?project.
                                                        ?documento <http://w3id.org/roh/isValidated> 'true'.
					                                    ?documento a <http://purl.org/ontology/bibo/Document>.
					                                    ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutores.
					                                    ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
				                                    }}
			                                    }} 
		                                    }}		
		                                    MINUS
		                                    {{
			                                    ?project <http://w3id.org/roh/membersProject> ?person.
		                                    }}
	                                    }}
                                    }}
                                }}                                
                              }}Group by ?project 
                            }}
                            FILTER(?numColaboradoresCargados!= ?numColaboradoresACargar OR !BOUND(?numColaboradoresCargados) )
                            }} limit {limitActualizarNumeroColaboradores}";
                    SparqlObject resultadoActualizarNumeroColaboradores = mResourceApi.VirtuosoQueryMultipleGraph(selectActualizarNumeroColaboradores, whereActualizarNumeroColaboradores, new List<string>() { "project", "document", "person" });

                    Parallel.ForEach(resultadoActualizarNumeroColaboradores.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string project = fila["project"].value;
                        string numColaboradoresACargar = fila["numColaboradoresACargar"].value;
                        string numColaboradoresCargados = "";
                        if (fila.ContainsKey("numColaboradoresCargados"))
                        {
                            numColaboradoresCargados = fila["numColaboradoresCargados"].value;
                        }
                        ActualizadorTriple(project, $"{GetUrlPrefix("roh")}collaboratorsNumber", numColaboradoresCargados, numColaboradoresACargar);
                    });

                    if (resultadoActualizarNumeroColaboradores.results.bindings.Count != limitActualizarNumeroColaboradores)
                    {
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/researchersNumber de los http://vivoweb.org/ontology/core#Project
        /// el nº de miembros  (sólo para los validados, los no validados no tienen 'miembros' reales)
        /// Depende de ActualizadorProject.ActualizarMiembrosUnificados
        /// </summary>
        /// <param name="pProjects">IDs de los proyectos</param>
        public void ActualizarNumeroMiembros(List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("project", $"{GetUrlPrefix("vivo")}Project", $"{GetUrlPrefix("roh")}researchersNumber");

            HashSet<string> filtersActualizarNumeroMiembros = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarNumeroMiembros.Add($" FILTER(?project in(<{string.Join(">,<", pProjects)}>))");
            }
            if (filtersActualizarNumeroMiembros.Count == 0)
            {
                filtersActualizarNumeroMiembros.Add("");
            }
            foreach (string filter in filtersActualizarNumeroMiembros)
            {
                while (true)
                {
                    int limitActualizarNumeroMiembros = 500;
                    String selectActualizarNumeroMiembros = @"select ?project ?numMiembrosCargados ?numMiembrosACargar ";
                    String whereActualizarNumeroMiembros = @$"where{{
                            ?project a <http://vivoweb.org/ontology/core#Project>.
                            ?project <http://w3id.org/roh/isValidated> 'true'.
                            {filter}
                            OPTIONAL
                            {{
                              ?project <http://w3id.org/roh/researchersNumber> ?numMiembrosCargadosAux. 
                              BIND(xsd:int( ?numMiembrosCargadosAux) as  ?numMiembrosCargados)
                            }}
                            {{
                              select ?project count(distinct ?person) as ?numMiembrosACargar
                              Where{{                               
                                ?project a <http://vivoweb.org/ontology/core#Project>.
                                OPTIONAL
                                {{      
                                    ?project <http://w3id.org/roh/membersProject> ?person.
                                }}                                
                              }}Group by ?project 
                            }}
                            FILTER(?numMiembrosCargados!= ?numMiembrosACargar OR !BOUND(?numMiembrosCargados) )
                            }} limit {limitActualizarNumeroMiembros}";
                    SparqlObject resultadoActualizarNumeroMiembros = mResourceApi.VirtuosoQueryMultipleGraph(selectActualizarNumeroMiembros, whereActualizarNumeroMiembros, new List<string>() { "project", "document", "person" });

                    Parallel.ForEach(resultadoActualizarNumeroMiembros.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string project = fila["project"].value;
                        string numMiembrosACargar = fila["numMiembrosACargar"].value;
                        string numMiembrosCargados = "";
                        if (fila.ContainsKey("numMiembrosCargados"))
                        {
                            numMiembrosCargados = fila["numMiembrosCargados"].value;
                        }
                        ActualizadorTriple(project, $"{GetUrlPrefix("roh")}researchersNumber", numMiembrosCargados, numMiembrosACargar);
                    });

                    if (resultadoActualizarNumeroMiembros.results.bindings.Count != limitActualizarNumeroMiembros)
                    {
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/yearStart de los http://vivoweb.org/ontology/core#Project
        /// el año de inicio
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarAniosInicio(List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("project", $"{GetUrlPrefix("vivo")}Project", $"{GetUrlPrefix("roh")}yearStart");

            HashSet<string> filtersActualizarAniosInicio = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarAniosInicio.Add($" FILTER(?project in(<{string.Join(">,<", pProjects)}>))");
            }
            if (filtersActualizarAniosInicio.Count == 0)
            {
                filtersActualizarAniosInicio.Add("");
            }

            foreach (string filter in filtersActualizarAniosInicio)
            {
                //Inserciones
                while (true)
                {
                    int limitActualizarAniosInicio = 500;

                    String selectActualizarAniosInicio = @"select distinct * where{select ?project ?yearCargado ?yearCargar  ";
                    String whereActualizarAniosInicio = @$"where{{
                                ?project a <http://vivoweb.org/ontology/core#Project>.
                                {filter}
                                OPTIONAL{{
	                                ?project <http://vivoweb.org/ontology/core#start> ?fecha.
	                                BIND(substr(str(?fecha),0,4) as ?yearCargar).
                                }}
                                OPTIONAL{{
                                    ?project <http://w3id.org/roh/yearStart> ?yearCargado.      
                                }}
                                
                                FILTER(?yearCargado!= ?yearCargar)

                            }}}} limit {limitActualizarAniosInicio}";
                    SparqlObject resultadoActualizarAniosInicio = mResourceApi.VirtuosoQuery(selectActualizarAniosInicio, whereActualizarAniosInicio, "project");

                    Parallel.ForEach(resultadoActualizarAniosInicio.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string project = fila["project"].value;
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
                        ActualizadorTriple(project, $"{GetUrlPrefix("roh")}yearStart", yearCargado, yearCargar);
                    });

                    if (resultadoActualizarAniosInicio.results.bindings.Count != limitActualizarAniosInicio)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos en la propiedad http://w3id.org/roh/yearEnd de los http://vivoweb.org/ontology/core#Project
        /// el año de inicio
        /// No tiene dependencias
        /// </summary>
        /// <param name="pDocuments">ID de documentos</param>
        public void ActualizarAniosFin(List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("project", $"{GetUrlPrefix("vivo")}Project", $"{GetUrlPrefix("roh")}yearEnd");

            HashSet<string> filtersActualizarAniosFin = new HashSet<string>();
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersActualizarAniosFin.Add($" FILTER(?project in(<{string.Join(">,<", pProjects)}>))");
            }
            if (filtersActualizarAniosFin.Count == 0)
            {
                filtersActualizarAniosFin.Add("");
            }

            foreach (string filter in filtersActualizarAniosFin)
            {
                //Inserciones
                while (true)
                {
                    int limitActualizarAniosFin = 500;

                    String selectActualizarAniosFin = @"select distinct * where{select ?project ?yearCargado ?yearCargar  ";
                    String whereActualizarAniosFin = @$"where{{
                                ?project a <http://vivoweb.org/ontology/core#Project>.
                                {filter}
                                OPTIONAL{{
	                                ?project <http://vivoweb.org/ontology/core#end> ?fecha.
	                                BIND(substr(str(?fecha),0,4) as ?yearCargar).
                                }}
                                OPTIONAL{{
                                    ?project <http://w3id.org/roh/yearEnd> ?yearCargado.      
                                }}
                                
                                FILTER(?yearCargado!= ?yearCargar)

                            }}}} limit {limitActualizarAniosFin}";
                    SparqlObject resultadoActualizarAniosFin = mResourceApi.VirtuosoQuery(selectActualizarAniosFin, whereActualizarAniosFin, "project");

                    Parallel.ForEach(resultadoActualizarAniosFin.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string project = fila["project"].value;
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
                        ActualizadorTriple(project, $"{GetUrlPrefix("roh")}yearEnd", yearCargado, yearCargar);
                    });

                    if (resultadoActualizarAniosFin.results.bindings.Count != limitActualizarAniosFin)
                    {
                        break;
                    }
                }
            }
        }
    }
}
