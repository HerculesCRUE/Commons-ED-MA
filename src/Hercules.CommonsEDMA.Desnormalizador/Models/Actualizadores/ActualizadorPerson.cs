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
    /// Clase para actualizar propiedades de personas
    /// </summary>
    public class ActualizadorPerson : ActualizadorBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pResourceApi">API Wrapper de GNOSS</param>
        public ActualizadorPerson(ResourceApi pResourceApi) : base(pResourceApi)
        {
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/lineResearch de las http://xmlns.com/foaf/0.1/Person  
        /// las líneas de investigación activas
        /// No tiene dependencias
        /// </summary>
        /// <param name="pPersons">IDs de las personas</param>
        /// <param name="pGroups">ID de grupos</param>
        public void ActualizarPertenenciaLineas(List<string> pPersons = null, List<string> pGroups = null)
        {
            string fechaActual = DateTime.UtcNow.ToString("yyyyMMdd000000");
            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pGroups != null && pGroups.Count > 0)
            {
                filters.Add($" ?groupAux <http://vivoweb.org/ontology/core#relates> ?relatesAux. ?relatesAux <http://w3id.org/roh/roleOf> ?person.  FILTER(?groupAux in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    //Añadimos líneas
                    int limit = 500;
                    String select = @"select distinct ?person  ?linea  ";
                    String where = @$"where{{
                                    {filter}
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
                                            MINUS
                                            {{
                                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                                ?person <http://w3id.org/roh/lineResearch> ?linea. 
                                            }}
                                        }}
                                    }}                                    
                                }}order by desc(?person) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "group", "person" });
                    InsercionMultiple(resultado.results.bindings, "http://w3id.org/roh/lineResearch", "person", "linea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Eliminamos líneas
                    int limit = 500;
                    String select = @"select distinct ?person  ?linea  ";
                    String where = @$"where{{
                                    {filter}
                                    {{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        ?person <http://w3id.org/roh/lineResearch> ?linea.                                        
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
                                }}order by desc(?person) limit {limit}";
                    var resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "group", "person" });
                    EliminacionMultiple(resultado.results.bindings, "http://w3id.org/roh/lineResearch", "person", "linea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/publicationsNumber de las http://xmlns.com/foaf/0.1/Person el nº de publicaciones validadas
        /// Depende de ActualizadorDocument.ActualizarDocumentosValidados
        /// </summary>
        /// <param name="pPersons">IDs de las personas</param>
        public void ActualizarNumeroPublicacionesValidadas(List<string> pPersons = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/publicationsNumber");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person  ?numDocumentosCargados ?numDocumentosACargar ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/publicationsNumber> ?numDocumentosCargadosAux. 
                              BIND(xsd:int( ?numDocumentosCargadosAux) as  ?numDocumentosCargados)
                            }}
                            {{
                              select ?person count(distinct ?doc) as ?numDocumentosACargar
                              Where{{
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL{{
                                    ?doc a <http://purl.org/ontology/bibo/Document>.
                                    ?doc <http://w3id.org/roh/isValidated> 'true'.
                                    ?doc <http://purl.org/ontology/bibo/authorList> ?listaAutores.
					                ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                }}
                              }}Group by ?person 
                            }}
                            FILTER(?numDocumentosCargados!= ?numDocumentosACargar OR !BOUND(?numDocumentosCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "document", "curriculumvitae" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string numDocumentosACargar = fila["numDocumentosACargar"].value;
                        string numDocumentosCargados = "";
                        if (fila.ContainsKey("numDocumentosCargados"))
                        {
                            numDocumentosCargados = fila["numDocumentosCargados"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/publicationsNumber", numDocumentosCargados, numDocumentosACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/publicPublicationsNumber de las http://xmlns.com/foaf/0.1/Person el nº de publicaciones publicas (validadas + publicas en su cv)
        /// Depende de ActualizadorCV.ModificarDocumentos y actualizadorCV.CambiarPrivacidadDocumentos
        /// </summary>
        /// <param name="pPersons">IDs de las personas</param>
        public void ActualizarNumeroPublicacionesPublicas(List<string> pPersons = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/publicPublicationsNumber");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {

                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person  ?numDocumentosCargados ?numDocumentosACargar ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/publicPublicationsNumber> ?numDocumentosCargadosAux. 
                              BIND(xsd:int( ?numDocumentosCargadosAux) as  ?numDocumentosCargados)
                            }}
                            {{
                              select ?person count(distinct ?doc) as ?numDocumentosACargar
                              Where{{
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL
                                {{
                                    {{
			                            ?doc a <http://purl.org/ontology/bibo/Document>.
			                            ?cv <http://w3id.org/roh/cvOf> ?person.
			                            ?cv  <http://w3id.org/roh/scientificActivity> ?scientificActivity.
			                            ?scientificActivity ?pAux ?oAux.
			                            ?oAux <http://w3id.org/roh/isPublic> 'true'.
			                            ?oAux <http://vivoweb.org/ontology/core#relatedBy> ?doc
		                            }}
                                    #UNION
		                            #{{
			                        #    ?doc a <http://purl.org/ontology/bibo/Document>.
			                        #    ?doc <http://w3id.org/roh/isValidated> 'true'.
			                        #    ?doc <http://purl.org/ontology/bibo/authorList> ?listaAutores.
			                        #    ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
		                            #}}
                                }}
                              }}Group by ?person 
                            }}
                            FILTER(?numDocumentosCargados!= ?numDocumentosACargar OR !BOUND(?numDocumentosCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "document", "curriculumvitae" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string numDocumentosACargar = fila["numDocumentosACargar"].value;
                        string numDocumentosCargados = "";
                        if (fila.ContainsKey("numDocumentosCargados"))
                        {
                            numDocumentosCargados = fila["numDocumentosCargados"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/publicPublicationsNumber", numDocumentosCargados, numDocumentosACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/ipNumber de las http://xmlns.com/foaf/0.1/Person el nº de proyectos en los que ha sido IP
        /// No tiene dependencias
        /// </summary>
        /// <param name="pPersons">IDs de las personas</param>
        /// <param name="pProjects">ID de proyectos</param>
        public void ActualizarNumeroIPProyectos(List<string> pPersons = null, List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/ipNumber");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pProjects != null && pProjects.Count > 0)
            {
                filters.Add($" ?project <http://vivoweb.org/ontology/core#relates> ?relatesAux. ?relatesAux <http://w3id.org/roh/roleOf> ?person.  FILTER(?project in (<{string.Join(">,<", pProjects)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {

                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person  ?numIPCargados ?numIPACargar ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/ipNumber> ?numIPCargadosAux. 
                              BIND(xsd:int( ?numIPCargadosAux) as  ?numIPCargados)
                            }}
                            {{
                              select ?person count(distinct ?proy) as ?numIPACargar
                              Where{{
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL{{
			                        ?proy a <http://vivoweb.org/ontology/core#Project>.
			                        ?proy <http://w3id.org/roh/isValidated> 'true'.
                                    ?proy <http://vivoweb.org/ontology/core#relates> ?listprojauth.
                                    ?listprojauth <http://w3id.org/roh/roleOf> ?person.
                                    ?listprojauth <http://w3id.org/roh/isIP> 'true'.
		                        }}
                              }}Group by ?person 
                            }}
                            FILTER(?numIPCargados!= ?numIPACargar OR !BOUND(?numIPCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "project" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string numIPACargar = fila["numIPACargar"].value;
                        string numIPCargados = "";
                        if (fila.ContainsKey("numIPCargados"))
                        {
                            numIPCargados = fila["numIPCargados"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/ipNumber", numIPCargados, numIPACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/projectsNumber de las http://xmlns.com/foaf/0.1/Person el nº de proyectos validados
        /// Depende de ActualizadorProject.ActualizarMiembrosUnificados
        /// </summary>
        /// <param name="pPersons">IDs de las personas</param>
        /// <param name="pProjects">IDs de proyectos</param>
        public void ActualizarNumeroProyectosValidados(List<string> pPersons = null, List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/projectsNumber");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pProjects != null && pProjects.Count > 0)
            {
                filters.Add($" ?project <http://vivoweb.org/ontology/core#relates> ?relatesAux. ?relatesAux <http://w3id.org/roh/roleOf> ?person.  FILTER(?project in (<{string.Join(">,<", pProjects)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {

                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person  ?numProyectosCargados ?numProyectosACargar ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/projectsNumber> ?numProyectosCargadosAux. 
                              BIND(xsd:int( ?numProyectosCargadosAux) as  ?numProyectosCargados)
                            }}
                            {{
                              select ?person count(distinct ?proyecto) as ?numProyectosACargar
                              Where{{
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL{{
                                    ?proyecto a <http://vivoweb.org/ontology/core#Project>.
                                    ?proyecto <http://w3id.org/roh/isValidated> 'true'.                                    
                                    ?proyecto <http://w3id.org/roh/membersProject> ?person.
                                }}
                              }}Group by ?person 
                            }}
                            FILTER(?numProyectosCargados!= ?numProyectosACargar OR !BOUND(?numProyectosCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "project" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string numProyectosACargar = fila["numProyectosACargar"].value;
                        string numProyectosCargados = "";
                        if (fila.ContainsKey("numProyectosCargados"))
                        {
                            numProyectosCargados = fila["numProyectosCargados"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/projectsNumber", numProyectosCargados, numProyectosACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/projectsNumber de las http://xmlns.com/foaf/0.1/Person el nº de proyectos públicos
        /// Depende de ActualizadorProject.ActualizarMiembrosUnificados y ActualizadorCV.ModificarProyectos();
        /// </summary>
        /// <param name="pPersons">IDs de las personas</param>
        /// <param name="pProjects">IDs de proyectos</param>
        public void ActualizarNumeroProyectosPublicos(List<string> pPersons = null, List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/publicProjectsNumber");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pProjects != null && pProjects.Count > 0)
            {
                filters.Add($" ?project <http://vivoweb.org/ontology/core#relates> ?relatesAux. ?relatesAux <http://w3id.org/roh/roleOf> ?person.  FILTER(?project in (<{string.Join(">,<", pProjects)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {

                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person  ?numProyectosCargados ?numProyectosACargar ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/publicProjectsNumber> ?numProyectosCargadosAux. 
                              BIND(xsd:int( ?numProyectosCargadosAux) as  ?numProyectosCargados)
                            }}
                            {{
                              select ?person count(distinct ?proyecto) as ?numProyectosACargar
                              Where{{
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL{{
                                    ?proyecto a <http://vivoweb.org/ontology/core#Project>.
                                    {{
			                            ?cv <http://w3id.org/roh/cvOf> ?person.
			                            ?cv  <http://w3id.org/roh/scientificExperience> ?scientificExperience.
			                            ?scientificExperience ?pAux ?oAux.
			                            ?oAux <http://w3id.org/roh/isPublic> 'true'.
			                            ?oAux <http://vivoweb.org/ontology/core#relatedBy> ?proyecto
		                            }}
                                    #UNION
                                    #{{
                                    #    ?proyecto <http://w3id.org/roh/isValidated> 'true'.
                                    #    ?proyecto <http://w3id.org/roh/membersProject> ?person.
                                    #}}
                                }}
                              }}Group by ?person 
                            }}
                            FILTER(?numProyectosCargados!= ?numProyectosACargar OR !BOUND(?numProyectosCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "project", "curriculumvitae" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string numProyectosACargar = fila["numProyectosACargar"].value;
                        string numProyectosCargados = "";
                        if (fila.ContainsKey("numProyectosCargados"))
                        {
                            numProyectosCargados = fila["numProyectosCargados"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/publicProjectsNumber", numProyectosCargados, numProyectosACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        public void ActualizarNumeroResearchObjectsPublicos(List<string> pPersons = null, List<string> pResearchObjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/publicResearchObjectsNumber");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pResearchObjects != null && pResearchObjects.Count > 0)
            {
                filters.Add($" ?project <http://vivoweb.org/ontology/core#relates> ?relatesAux. ?relatesAux <http://w3id.org/roh/roleOf> ?person.  FILTER(?project in (<{string.Join(">,<", pResearchObjects)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person ?numResearchObjectsCargados ?numResearchObjectsACargar ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/publicResearchObjectsNumber> ?numResearchObjectsCargadosAux. 
                              BIND(xsd:int( ?numResearchObjectsCargadosAux) as  ?numResearchObjectsCargados)
                            }}
                            {{
                              select ?person count(distinct ?researchObject) as ?numResearchObjectsACargar
                              Where{{
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL{{
			                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                    ?cv  <http://w3id.org/roh/researchObject> ?researchObjects.
                                    ?researchObjects ?pAux ?oAux.
                                    ?oAux <http://w3id.org/roh/isPublic> 'true'.
                                    ?oAux <http://vivoweb.org/ontology/core#relatedBy> ?researchObject .		                            
                                }}
                              }}Group by ?person 
                            }}
                        FILTER(?numResearchObjectsCargados!= ?numResearchObjectsACargar OR !BOUND(?numResearchObjectsCargados) )
                        }}
                        limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "curriculumvitae" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string numResearchObjectsACargar = fila["numResearchObjectsACargar"].value;
                        string numResearchObjectsCargados = "";
                        if (fila.ContainsKey("numResearchObjectsCargados"))
                        {
                            numResearchObjectsCargados = fila["numResearchObjectsCargados"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/publicResearchObjectsNumber", numResearchObjectsCargados, numResearchObjectsACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// Actualizamos en la propiedad http://vivoweb.org/ontology/core#hasResearchArea de las http://xmlns.com/foaf/0.1/Person 
        /// las áreas en función de sus publicaciones validadas 
        /// Depende de ActualiadorDocument.ActualizarAreasDocumentos
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        public void ActualizarAreasPersonas(List<string> pPersons = null)
        {
            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                //Eliminamos las categorías duplicadas
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person ?categoryNode ";
                    String where = @$"where{{
                                select distinct ?person ?hasKnowledgeAreaAux  ?categoryNode
                                where{{
                                    {filter}
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    ?person <http://vivoweb.org/ontology/core#hasResearchArea> ?hasKnowledgeAreaAux .
                                    ?hasKnowledgeAreaAux <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                               }}
                            }}group by ?person ?categoryNode HAVING (COUNT(*) > 1) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "taxonomy" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string categoryNode = fila["categoryNode"].value;
                        select = @"select ?person ?hasKnowledgeArea ?categoryNode ";
                        where = @$"where{{
                                    FILTER(?person=<{person}>)
                                    FILTER(?categoryNode =<{categoryNode}>)
                                    {{ 
                                        select distinct ?person ?hasKnowledgeArea  ?categoryNode
                                        where{{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                                            ?person <http://vivoweb.org/ontology/core#hasResearchArea> ?hasKnowledgeArea.
                                            ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?categoryNode.
                                            MINUS{{
                                                ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                            }}
                                        }}
                                    }}
                                }}";
                        resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "taxonomy" });
                        List<RemoveTriples> triplesRemove = new();
                        foreach (string hasKnowledgeArea in resultado.results.bindings.GetRange(1, resultado.results.bindings.Count - 1).Select(x => x["hasKnowledgeArea"].value).ToList())
                        {
                            triplesRemove.Add(new RemoveTriples()
                            {
                                Predicate = "http://vivoweb.org/ontology/core#hasResearchArea",
                                Value = hasKnowledgeArea
                            }); ;
                        }
                        if (triplesRemove.Count > 0)
                        {
                            var resultadox = mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<RemoveTriples>>() { { mResourceApi.GetShortGuid(person), triplesRemove } });
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
                    String select = @"select distinct ?person ?categoryNode ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.                            
                            {{                                
                                select  distinct ?person ?hasKnowledgeAreaDocument ?categoryNode 
                                where{{
                                    {filter}
                                    ?document a <http://purl.org/ontology/bibo/Document>.
                                    ?document <http://w3id.org/roh/isValidated> 'true'.
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    ?document <http://purl.org/ontology/bibo/authorList> ?autores.
                                    ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                    ?document  <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaDocument.
                                    ?hasKnowledgeAreaDocument <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}
                            }}
                            MINUS{{
                                select distinct ?person ?hasKnowledgeAreaPerson ?categoryNode 
                                where{{
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    ?person <http://vivoweb.org/ontology/core#hasResearchArea> ?hasKnowledgeAreaPerson.
                                    ?hasKnowledgeAreaPerson <http://w3id.org/roh/categoryNode> ?categoryNode
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}
                            }}
                            }}order by (?person) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "taxonomy", "document" });
                    InsertarCategorias(resultado, dicAreasBroader, mResourceApi.GraphsUrl, "person", "http://vivoweb.org/ontology/core#hasResearchArea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    int limit = 500;
                    //ELIMINAMOS
                    String select = @"select ?person ?hasKnowledgeArea ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.                            
                            {{
                                select distinct ?person ?hasKnowledgeArea ?categoryNode 
                                where{{
                                    {filter}
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    ?person <http://vivoweb.org/ontology/core#hasResearchArea> ?hasKnowledgeArea.
                                    ?hasKnowledgeArea <http://w3id.org/roh/categoryNode> ?categoryNode
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}                                
                            }}
                            MINUS{{
                                select  distinct ?person ?hasKnowledgeAreaDocument ?categoryNode where{{
                                    ?document a <http://purl.org/ontology/bibo/Document>.
                                    ?document <http://w3id.org/roh/isValidated> 'true'.
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    ?document <http://purl.org/ontology/bibo/authorList> ?autores.
                                    ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                    ?document  <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaDocument.
                                    ?hasKnowledgeAreaDocument <http://w3id.org/roh/categoryNode> ?categoryNode.
                                    MINUS{{
                                        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
                                    }}
                                }}
                                 
                            }}
                            }}order by (?person) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "taxonomy", "document" });
                    EliminarCategorias(resultado, "person", "http://vivoweb.org/ontology/core#hasResearchArea");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/themedAreasNumber de las http://xmlns.com/foaf/0.1/Person 
        /// el número de áreas temáticas de sus publicaciones públicas
        /// Depende de ActualizadorCV.ModificarDocumentos, ActualizadorCV.CambiarPrivacidadDocumentos y AcualizadorDocument.ActualizarAreasDocumentos
        /// </summary>
        /// <param name="pPersons">IDs de la personas</param>
        public void ActualizarNumeroAreasTematicas(List<string> pPersons = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/themedAreasNumber");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {

                //Actualizamos los datos
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person  ?numAreasTematicasCargadas ?numAreasTematicasACargar ";
                    String where = @$"where{{                            
                            {filter}
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/themedAreasNumber> ?numAreasTematicasCargadasAux. 
                              BIND(xsd:int( ?numAreasTematicasCargadasAux) as  ?numAreasTematicasCargadas)
                            }}
                            OPTIONAL{{
                              select ?person count(distinct ?categoria) as ?numAreasTematicasACargar
                              Where{{
                                ?person a <http://xmlns.com/foaf/0.1/Person>.   
                                OPTIONAL{{
                                    ?documento a <http://purl.org/ontology/bibo/Document>. 
                                    {{
                                        ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutores.
					                    ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                        ?documento <http://w3id.org/roh/isValidated> 'true'.
                                    }}UNION
                                    {{
			                            ?cv <http://w3id.org/roh/cvOf> ?person.
			                            ?cv  <http://w3id.org/roh/scientificActivity> ?scientificActivity.
			                            ?scientificActivity ?pAux ?oAux.
			                            ?oAux <http://w3id.org/roh/isPublic> 'true'.
			                            ?oAux <http://vivoweb.org/ontology/core#relatedBy> ?documento
		                            }}
                                    ?documento <http://w3id.org/roh/hasKnowledgeArea> ?area.
                                    ?area <http://w3id.org/roh/categoryNode> ?categoria.
                                    ?categoria <http://www.w3.org/2008/05/skos#prefLabel> ?nombreCategoria.
                                    MINUS
                                    {{
                                        ?categoria <http://www.w3.org/2008/05/skos#narrower> ?hijos
                                    }}
                                }}
                              }}Group by ?person 
                            }}
                            FILTER(?numAreasTematicasCargadas!= ?numAreasTematicasACargar  OR !BOUND(?numAreasTematicasCargadas) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "document", "curriculumvitae", "taxonomy" });

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string numAreasTematicasACargar = fila["numAreasTematicasACargar"].value;
                        string numAreasTematicasCargadas = "";
                        if (fila.ContainsKey("numAreasTematicasCargadas"))
                        {
                            numAreasTematicasCargadas = fila["numAreasTematicasCargadas"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/themedAreasNumber", numAreasTematicasCargadas, numAreasTematicasACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/collaboratorsNumber de las http://xmlns.com/foaf/0.1/Person 
        /// el nº de colaboradores (personas con coautorías en publicaciones públicos y personas con coautorias en proyectos públicos)
        /// Depende de ActualizadorProject.ActualizarMiembrosUnificados
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        /// <param name="pProjects">IDs de proyectos</param>
        public void ActualizarNumeroColaboradoresPublicos(List<string> pPersons = null, List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/publicCollaboratorsNumber");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pProjects != null && pProjects.Count > 0)
            {
                filters.Add($" ?projectAux <http://vivoweb.org/ontology/core#relates> ?relatesAux. ?relatesAux <http://w3id.org/roh/roleOf> ?person.  FILTER(?projectAux in (<{string.Join(">,<", pProjects)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct ?person ?numColaboradoresCargados ?numColaboradoresACargar ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/publicCollaboratorsNumber> ?numColaboradoresCargadosAux. 
                              BIND(xsd:int( ?numColaboradoresCargadosAux) as  ?numColaboradoresCargados)
                            }}
                            {{
                              select ?person count(distinct ?collaborator) as ?numColaboradoresACargar
                              Where{{                               
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL
                                {{
                                    {{
	                                    SELECT DISTINCT ?collaborator ?person
	                                    WHERE 
	                                    {{	
                                            ?collaborator a <http://xmlns.com/foaf/0.1/Person>
		                                    {{
			                                    {{
				                                    #Documentos
				                                    SELECT *
				                                    WHERE {{                                                        
			                                            ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutoresActual.
			                                            ?listaAutoresActual <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
					                                    ?documento <http://purl.org/ontology/bibo/authorList> ?listaAutores.
					                                    ?listaAutores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?collaborator.
				                                    }}
			                                    }} 
			                                    UNION 
			                                    {{
				                                    #Proyectos
				                                    SELECT *
				                                    WHERE {{
                                                        ?proy a <http://vivoweb.org/ontology/core#Project>.
                                                        ?proy <http://w3id.org/roh/membersProject> ?person.    
					                                    ?proy <http://w3id.org/roh/membersProject> ?collaborator.
				                                    }}
			                                    }}
		                                    }}		
		                                    FILTER(?collaborator!=?person)
	                                    }}
                                    }}
                                }}                                
                              }}Group by ?person 
                            }}
                            FILTER(?numColaboradoresCargados!= ?numColaboradoresACargar OR !BOUND(?numColaboradoresCargados) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "project", "document" });
                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string numColaboradoresACargar = fila["numColaboradoresACargar"].value;
                        string numColaboradoresCargados = "";
                        if (fila.ContainsKey("numColaboradoresCargados"))
                        {
                            numColaboradoresCargados = fila["numColaboradoresCargados"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/publicCollaboratorsNumber", numColaboradoresCargados, numColaboradoresACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/isIPGroupActually de las http://xmlns.com/foaf/0.1/Person 
        /// si la persona es actualmente IP de algún grupo
        /// No tiene dependencias
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        /// <param name="pGroups">ID de los grupos</param>
        public void ActualizarIPGruposActuales(List<string> pPersons = null, List<string> pGroups = null)
        {
            string fechaActual = DateTime.UtcNow.ToString("yyyyMMdd000000");

            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/isIPGroupActually");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pGroups != null && pGroups.Count > 0)
            {
                filters.Add($" ?groupAux <http://vivoweb.org/ontology/core#relates> ?member. ?member <http://w3id.org/roh/roleOf> ?person. FILTER(?groupAux in (<{string.Join(">,<", pGroups)}>))");
            }

            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person ?datoActual ?datoCargar  ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/isIPGroupActually> ?datoActual. 
                            }}
                            {{
                              select ?person IF(BOUND(?group),'true','false') as ?datoCargar
                              Where{{                               
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL
                                {{
                                    ?group a <http://xmlns.com/foaf/0.1/Group>.
                                    ?group <http://vivoweb.org/ontology/core#relates> ?member.
                                    ?member <http://w3id.org/roh/roleOf> ?person.
                                    ?member <http://w3id.org/roh/isIP> 'true'.
                                    OPTIONAL{{?member <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                                    OPTIONAL{{?member <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
                                    BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                                    BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                                    FILTER(?fechaPersonaInitAux<={fechaActual} AND ?fechaPersonaEndAux>={fechaActual})
                                }}                                
                              }}
                            }}
                            FILTER(?datoActual!= ?datoCargar OR !BOUND(?datoActual) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "group" });
                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string datoCargar = fila["datoCargar"].value;
                        string datoActual = "";
                        if (fila.ContainsKey("datoActual"))
                        {
                            datoActual = fila["datoActual"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/isIPGroupActually", datoActual, datoCargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/isIPGroupHistorically de las http://xmlns.com/foaf/0.1/Person 
        /// si la persona ha sido IP de algún grupo
        /// No tiene dependencias
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        /// <param name="pGroups">ID de los grupos</param>
        public void ActualizarIPGruposHistoricos(List<string> pPersons = null, List<string> pGroups = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/isIPGroupHistorically");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pGroups != null && pGroups.Count > 0)
            {
                filters.Add($" ?groupAux <http://vivoweb.org/ontology/core#relates> ?member. ?member <http://w3id.org/roh/roleOf> ?person. FILTER(?groupAux in (<{string.Join(">,<", pGroups)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person ?datoActual ?datoCargar  ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/isIPGroupHistorically> ?datoActual. 
                            }}
                            {{
                              select ?person IF(BOUND(?group),'true','false') as ?datoCargar
                              Where{{                               
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL
                                {{
                                    ?group a <http://xmlns.com/foaf/0.1/Group>.
                                    ?group <http://vivoweb.org/ontology/core#relates> ?member.
                                    ?member <http://w3id.org/roh/roleOf> ?person.
                                    ?member <http://w3id.org/roh/isIP> 'true'.
                                }}                                
                              }}
                            }}
                            FILTER(?datoActual!= ?datoCargar OR !BOUND(?datoActual) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "group" });
                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string datoCargar = fila["datoCargar"].value;
                        string datoActual = "";
                        if (fila.ContainsKey("datoActual"))
                        {
                            datoActual = fila["datoActual"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/isIPGroupHistorically", datoActual, datoCargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/isIPProjectActually de las http://xmlns.com/foaf/0.1/Person 
        /// si la persona es actualmente IP de algún proyecto
        /// No tiene dependencias
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        /// <param name="pProjects">ID de los proyectos</param>
        public void ActualizarIPProyectosActuales(List<string> pPersons = null, List<string> pProjects = null)
        {
            string fechaActual = DateTime.UtcNow.ToString("yyyyMMdd000000");

            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/isIPProjectActually");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pProjects != null && pProjects.Count > 0)
            {
                filters.Add($" ?projectAux <http://vivoweb.org/ontology/core#relates> ?member. ?member <http://w3id.org/roh/roleOf> ?person. FILTER(?projectAux in (<{string.Join(">,<", pProjects)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct ?person ?datoActual ?datoCargar  ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/isIPProjectActually> ?datoActual. 
                            }}
                            {{
                              select ?person IF(BOUND(?project),'true','false') as ?datoCargar
                              Where{{                               
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL
                                {{
                                    ?project a <http://vivoweb.org/ontology/core#Project>.
                                    ?project <http://vivoweb.org/ontology/core#relates> ?member.
                                    ?member <http://w3id.org/roh/roleOf> ?person.
                                    ?member <http://w3id.org/roh/isIP> 'true'.
                                    OPTIONAL{{?member <http://vivoweb.org/ontology/core#start> ?fechaPersonaInit.}}
                                    OPTIONAL{{?member <http://vivoweb.org/ontology/core#end> ?fechaPersonaEnd.}}
                                    BIND(IF(bound(?fechaPersonaEnd), xsd:integer(?fechaPersonaEnd), 30000000000000) as ?fechaPersonaEndAux)
                                    BIND(IF(bound(?fechaPersonaInit), xsd:integer(?fechaPersonaInit), 10000000000000) as ?fechaPersonaInitAux)
                                    FILTER(?fechaPersonaInitAux<={fechaActual} AND ?fechaPersonaEndAux>={fechaActual})
                                }}                                
                              }}
                            }}
                            FILTER(?datoActual!= ?datoCargar OR !BOUND(?datoActual) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "project" });
                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string datoCargar = fila["datoCargar"].value;
                        string datoActual = "";
                        if (fila.ContainsKey("datoActual"))
                        {
                            datoActual = fila["datoActual"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/isIPProjectActually", datoActual, datoCargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/isIPProjectHistorically de las http://xmlns.com/foaf/0.1/Person 
        /// si la persona ha sido IP de algún grupo
        /// No tiene dependencias
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        /// <param name="pProjects">ID de los proyectos</param>
        public void ActualizarIPProyectosHistoricos(List<string> pPersons = null, List<string> pProjects = null)
        {
            //Eliminamos los duplicados
            EliminarDuplicados("person", "http://xmlns.com/foaf/0.1/Person", "http://w3id.org/roh/isIPProjectHistorically");

            HashSet<string> filters = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pProjects != null && pProjects.Count > 0)
            {
                filters.Add($" ?projectAux <http://vivoweb.org/ontology/core#relates> ?member. ?member <http://w3id.org/roh/roleOf> ?person. FILTER(?projectAux in (<{string.Join(">,<", pProjects)}>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            foreach (string filter in filters)
            {
                while (true)
                {
                    int limit = 500;
                    String select = @"select distinct ?person ?datoActual ?datoCargar   ";
                    String where = @$"where{{
                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                            {filter}
                            OPTIONAL
                            {{
                              ?person <http://w3id.org/roh/isIPProjectHistorically> ?datoActual. 
                            }}
                            {{
                              select ?person IF(BOUND(?project),'true','false') as ?datoCargar
                              Where{{                               
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                OPTIONAL
                                {{
                                    ?project a <http://vivoweb.org/ontology/core#Project>.
                                    ?project <http://vivoweb.org/ontology/core#relates> ?member.
                                    ?member <http://w3id.org/roh/roleOf> ?person.
                                    ?member <http://w3id.org/roh/isIP> 'true'.
                                }}                                
                              }}
                            }}
                            FILTER(?datoActual!= ?datoCargar OR !BOUND(?datoActual) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "project" });
                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string datoCargar = fila["datoCargar"].value;
                        string datoActual = "";
                        if (fila.ContainsKey("datoActual"))
                        {
                            datoActual = fila["datoActual"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/isIPProjectHistorically", datoActual, datoCargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Actualizamos en la propiedad http://w3id.org/roh/hIndex de las http://xmlns.com/foaf/0.1/Person 
        /// los hIndex de las diferentes fuentes
        /// Depende de ActualizadorDocument.ActualizarNumeroCitasMaximas
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        /// <param name="pProjects">ID de los proyectos</param>
        public void ActualizarHIndex(List<string> pDocuments = null, List<string> pPersons = null)
        {
            HashSet<string> filters = new HashSet<string>();
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filters.Add($@" ?document a <http://purl.org/ontology/bibo/Document>.                               
	                            ?document <http://purl.org/ontology/bibo/authorList> ?autores.
	                            ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                FILTER(?document in (<{string.Join(">,<", pDocuments)}>,<>))");
            }
            if (pPersons != null && pPersons.Count > 0)
            {
                filters.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>,<>))");
            }
            if (filters.Count == 0)
            {
                filters.Add("");
            }

            Dictionary<string, string> listSources = new Dictionary<string, string>();
            listSources.Add("SCOPUS", "http://w3id.org/roh/scopusCitationCount");
            listSources.Add("WOS", "http://w3id.org/roh/wosCitationCount");
            listSources.Add("Hércules", "http://w3id.org/roh/citationCount");
            listSources.Add("Semantic Scholar", "http://w3id.org/roh/semanticScholarcitationCount");

            foreach (string filter in filters)
            {
                //Eliminamos HIndex
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person ?hIndexEntity  ";
                    String where = @$"where{{
                                    {filter}
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    ?person <http://w3id.org/roh/hIndex> ?hIndexEntity.
                                    ?hIndexEntity <http://w3id.org/roh/citationSource> ?citationSource.
                                    FILTER(?citationSource not in ('{string.Join("','", listSources.Keys)}'))
                                }}order by desc(?person) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "person" });
                    EliminacionMultiple(resultado.results.bindings, "http://w3id.org/roh/hIndex", "person", "hIndexEntity");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                //Eliminamos HIndexCitationCount
                while (true)
                {
                    int limit = 500;
                    String select = @"select ?person ?hIndexCitationCount  ";
                    String where = @$"where{{
                                    {filter}
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    ?person <http://w3id.org/roh/hIndexCitationCount> ?hIndexCitationCount.
                                    ?hIndexCitationCount <http://w3id.org/roh/citationSource> ?citationSource.
                                    FILTER(?citationSource not in ('{string.Join("','", listSources.Keys)}'))
                                }}order by desc(?person) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "person" });
                    EliminacionMultiple(resultado.results.bindings, "http://w3id.org/roh/hIndexCitationCount", "person", "hIndexCitationCount");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                foreach (string source in listSources.Keys)
                {
                    while (true)
                    {
                        //Añadimos nº citas
                        int limit = 500;
                        String select = @"select *  ";
                        String where = @$"where{{
                                    {filter}
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    {{
                                        #Los que debe de haber
                                        select distinct ?person count(distinct(?document)) as ?publicationNumber ?citationCount ?citationSource
                                        Where{{
                                            ?person <http://w3id.org/roh/crisIdentifier> ?crisIdentifier. 
	                                        ?document a <http://purl.org/ontology/bibo/Document>.                               
	                                        ?document <http://purl.org/ontology/bibo/authorList> ?autores.
	                                        ?document <{listSources[source]}> ?citationCount.
	                                        ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
	                                        FILTER(?citationCount!=""0"")
                                            BIND('{source}' as ?citationSource)
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        #Los que hay
                                        ?person <http://w3id.org/roh/hIndexCitationCount> ?hIndexCitationCount.
                                        ?hIndexCitationCount <http://w3id.org/roh/citationCount> ?citationCount.
                                        ?hIndexCitationCount <http://w3id.org/roh/publicationNumber> ?publicationNumberAux.
                                        BIND(xsd:int(?publicationNumberAux) as ?publicationNumber)
                                        ?hIndexCitationCount <http://w3id.org/roh/citationSource> ?citationSource.
                                        FILTER(?citationSource='{source}')
                                    }}
                                }}order by desc(?person) limit {limit}";
                        SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "person" });
                        InsercionHIndexCitationCount(resultado.results.bindings);
                        if (resultado.results.bindings.Count != limit)
                        {
                            break;
                        }
                    }

                    while (true)
                    {
                        //Eliminamos nº citas
                        int limit = 500;
                        String select = @"select *  ";
                        String where = @$"where{{
                                    {filter}
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.
                                    {{
                                        #Los que hay
                                        ?person <http://w3id.org/roh/hIndexCitationCount> ?hIndexCitationCount.
                                        ?hIndexCitationCount <http://w3id.org/roh/citationCount> ?citationCount.
                                        ?hIndexCitationCount <http://w3id.org/roh/publicationNumber> ?publicationNumberAux.
                                        BIND(xsd:int(?publicationNumberAux) as ?publicationNumber)
                                        ?hIndexCitationCount <http://w3id.org/roh/citationSource> ?citationSource.    
                                        FILTER(?citationSource='{source}')
                                    }}
                                    MINUS
                                    {{
                                        #Los que debe de haber
                                        select distinct ?person count(distinct(?document)) as ?publicationNumber ?citationCount ?citationSource
                                        Where{{
                                            ?person <http://w3id.org/roh/crisIdentifier> ?crisIdentifier. 
	                                        ?document a <http://purl.org/ontology/bibo/Document>.                               
	                                        ?document <http://purl.org/ontology/bibo/authorList> ?autores.
	                                        ?document <{listSources[source]}> ?citationCount.
	                                        ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
	                                        FILTER(?citationCount!=""0"")
                                            BIND('{source}' as ?citationSource)
                                        }}
                                    }}
                                }}order by desc(?person) limit {limit}";
                        SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "document", "person" });

                        EliminacionMultiple(resultado.results.bindings, "http://w3id.org/roh/hIndexCitationCount", "person", "hIndexCitationCount");

                        if (resultado.results.bindings.Count != limit)
                        {
                            break;
                        }
                    }
                }

                

                while (true)
                {
                    //Actualizamos AcumulatedPublicationNumber
                    int limit = 500;
                    String select = @"select ?person ?hIndexCitationCount ?numAcumuladoCargadas ?numAcumuladoACargar";
                    String where = @$"where{{
                                {filter}
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                ?person <http://w3id.org/roh/hIndexCitationCount> ?hIndexCitationCount.
                                OPTIONAL{{
                                    
                                    ?hIndexCitationCount <http://w3id.org/roh/acumulatedPublicationNumber> ?numAcumuladoCargadasAux.
                                    BIND(xsd:int(?numAcumuladoCargadasAux) as ?numAcumuladoCargadas)
                                }}
                                {{                                  
                                    select ?person ?hIndexCitationCount SUM(?publicationNumber) as ?numAcumuladoACargar
                                    where
                                    {{
	                                    {{
		                                    select distinct ?person  ?hIndexCitationCount xsd:long(?citas) as ?citas ?citationSource
		                                    where 
		                                    {{					
			                                    ?person <http://w3id.org/roh/hIndexCitationCount> ?hIndexCitationCount.
			                                    ?hIndexCitationCount <http://w3id.org/roh/citationSource> ?citationSource.                                                    
			                                    ?hIndexCitationCount <http://w3id.org/roh/citationCount> ?citas.
		                                    }}
	                                    }}
	                                    {{
		                                    select ?person ?publicationNumber xsd:long(?citas2) as ?acumulatedCitationCount ?citationSource
		                                    where 
		                                    {{
			                                    ?person <http://w3id.org/roh/hIndexCitationCount> ?hIndexCitationCount.
			                                    ?hIndexCitationCount <http://w3id.org/roh/citationSource> ?citationSource.
			                                    ?hIndexCitationCount <http://w3id.org/roh/citationCount> ?citas2.
			                                    ?hIndexCitationCount <http://w3id.org/roh/publicationNumber> ?publicationNumberAux.
			                                    BIND(xsd:int(?publicationNumberAux) as ?publicationNumber)
		                                    }}
	                                    }}
	                                    FILTER(?acumulatedCitationCount >=?citas)	       
                                    }}
                                }}
                                FILTER(?numAcumuladoCargadas!= ?numAcumuladoACargar OR !BOUND(?numAcumuladoCargadas))
                            }}order by desc(?person) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "person");
                    ActualizarHIndexCitationCount(resultado.results.bindings);

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Actualizamos hIndexMax
                    int limit = 500;
                    String select = @"select ?person ?hIndexCitationCount ?numMaxCargado ?numMaxACargar";
                    String where = @$"where{{
                                {filter}
                                ?person a <http://xmlns.com/foaf/0.1/Person>.
                                ?person <http://w3id.org/roh/hIndexCitationCount> ?hIndexCitationCount.
                                OPTIONAL
                                {{                                    
                                    ?hIndexCitationCount <http://w3id.org/roh/hIndexMax> ?numMaxCargadoAux.
                                    BIND(xsd:int(?numMaxCargadoAux) as ?numMaxCargado)
                                }}                                
                                    
                                ?hIndexCitationCount <http://w3id.org/roh/acumulatedPublicationNumber> ?numAcumuladoCargadasAux.
                                BIND(xsd:int(?numAcumuladoCargadasAux) as ?numAcumuladoCargadas)                                
                                    
                                ?hIndexCitationCount <http://w3id.org/roh/citationCount> ?citasAux.
                                BIND(xsd:int(?citasAux) as ?citas)
                                
                                BIND(IF(?numAcumuladoCargadas>=?citas,?citas,?numAcumuladoCargadas) as ?numMaxACargar)
                                FILTER(?numMaxCargado!= ?numMaxACargar OR !BOUND(?numMaxCargado) )
                            }}order by desc(?person) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "person");
                    ActualizarHIndexCitationCount(resultado.results.bindings);

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Añadimos auxiliar H-Index
                    int limit = 500;
                    String select = @"select ?person ?source ?hIndexCalculado ";
                    String where = @$"where{{
                                    {filter}                                    
                                    {{
                                        select ?person ?source MAX(?hIndexMax) as ?hIndexCalculado
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.     
					                        ?person <http://w3id.org/roh/hIndexCitationCount> ?hIndexCitationCount.
                                            ?hIndexCitationCount <http://w3id.org/roh/citationSource> ?source.    
                                            ?hIndexCitationCount <http://w3id.org/roh/hIndexMax> ?hIndexMaxAux.                                    
                                            BIND(xsd:int(?hIndexMaxAux) as ?hIndexMax).
                                        }}                                        
                                    }}
                                    MINUS{{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.     
                                        ?person <http://w3id.org/roh/hIndex> ?hIndexEntity.
                                        ?hIndexEntity <http://w3id.org/roh/h-index> ?hIndexAux.
                                        ?hIndexEntity <http://w3id.org/roh/citationSource> ?source.    
                                        BIND(xsd:int(?hIndexAux) as ?hIndex)
                                    }}
                                }}order by desc(?person) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "person");
                    InsercionHIndex(resultado.results.bindings);
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }               

                while (true)
                {
                    //Eliminamos auxiliar H-Index
                    int limit = 500;
                    String select = @"select distinct ?person ?hIndexEntity ";
                    String where = @$"where{{
                                    {filter}                                    
                                    ?person a <http://xmlns.com/foaf/0.1/Person>.     
                                    {{                                        
                                        ?person <http://w3id.org/roh/hIndex> ?hIndexEntity.
                                        ?hIndexEntity <http://w3id.org/roh/h-index> ?hIndexAux.
                                        ?hIndexEntity <http://w3id.org/roh/citationSource> ?source.    
                                        BIND(xsd:int(?hIndexAux) as ?hIndex)                                        
                                    }}
                                    MINUS{{
                                        select ?person ?source MAX(?hIndexMax) as ?hIndexCalculado
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.     
					                        ?person <http://w3id.org/roh/hIndexCitationCount> ?hIndexCitationCount.
                                            ?hIndexCitationCount <http://w3id.org/roh/citationSource> ?source.    
                                            ?hIndexCitationCount <http://w3id.org/roh/hIndexMax> ?hIndexMaxAux.                                    
                                            BIND(xsd:int(?hIndexMaxAux) as ?hIndexMax).
                                        }}                                        
                                    }}
                                }}order by desc(?person) limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "person");
                    EliminacionMultiple(resultado.results.bindings, "http://w3id.org/roh/hIndex", "person", "hIndexEntity");
                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Actualizamos H-Index
                    int limit = 500;
                    String select = @"select ?person ?hIndexCargado ?hIndexACargar";
                    String where = @$"where{{
                                ?person a <http://xmlns.com/foaf/0.1/Person>.     
                                {filter}
                                OPTIONAL
                                {{
                                    ?person <http://w3id.org/roh/h-index> ?hIndexCargado.
                                }}
                                OPTIONAL{{
                                    ?person <http://w3id.org/roh/hIndex> ?hIndexEntity.
                                    ?hIndexEntity <http://w3id.org/roh/h-index> ?hIndexACargar.
                                    ?hIndexEntity <http://w3id.org/roh/citationSource> 'Hércules'.    
                                }}
                                FILTER(?hIndexCargado!= ?hIndexACargar OR (!BOUND(?hIndexCargado) AND BOUND(?hIndexACargar)) OR (BOUND(?hIndexCargado) AND !BOUND(?hIndexACargar)) )
                            }} limit {limit}";
                    SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "person");

                    Parallel.ForEach(resultado.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string person = fila["person"].value;
                        string hIndexACargar = fila["hIndexACargar"].value;
                        string hIndexCargado = "";
                        if (fila.ContainsKey("hIndexCargado"))
                        {
                            hIndexCargado = fila["hIndexCargado"].value;
                        }
                        ActualizadorTriple(person, "http://w3id.org/roh/h-index", hIndexCargado, hIndexACargar);
                    });

                    if (resultado.results.bindings.Count != limit)
                    {
                        break;
                    }
                }

            }
        }

        private void InsercionHIndexCitationCount(List<Dictionary<string, SparqlObject.Data>> pFilas)
        {
            List<string> ids = pFilas.Select(x => x["person"].value).Distinct().ToList();
            if (ids.Count > 0)
            {
                Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                {
                    Guid guid = mResourceApi.GetShortGuid(id);

                    Dictionary<Guid, List<TriplesToInclude>> triples = new() { { guid, new List<TriplesToInclude>() } };
                    foreach (Dictionary<string, SparqlObject.Data> fila in pFilas.Where(x => x["person"].value == id))
                    {
                        string idAux = mResourceApi.GraphsUrl + "items/HIndexCitationCount_" + guid.ToString().ToLower() + "_" + Guid.NewGuid().ToString().ToLower();
                        string person = fila["person"].value;
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/hIndexCitationCount|http://w3id.org/roh/citationCount";
                            t.NewValue = idAux + "|" + fila["citationCount"].value;
                            triples[guid].Add(t);
                        }
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/hIndexCitationCount|http://w3id.org/roh/publicationNumber";
                            t.NewValue = idAux + "|" + fila["publicationNumber"].value;
                            triples[guid].Add(t);
                        }
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/hIndexCitationCount|http://w3id.org/roh/citationSource";
                            t.NewValue = idAux + "|" + fila["citationSource"].value;
                            triples[guid].Add(t);
                        }
                    }
                    if (triples[guid].Count > 0)
                    {
                        var resultado = mResourceApi.InsertPropertiesLoadedResources(triples);
                    }
                });
            }
        }

        private void ActualizarHIndexCitationCount(List<Dictionary<string, SparqlObject.Data>> pFilas)
        {
            List<string> ids = pFilas.Select(x => x["person"].value).Distinct().ToList();
            if (ids.Count > 0)
            {
                Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                {
                    Guid guid = mResourceApi.GetShortGuid(id);

                    Dictionary<Guid, List<TriplesToInclude>> triplesInsert = new() { { guid, new List<TriplesToInclude>() } };
                    Dictionary<Guid, List<TriplesToModify>> triplesModify = new() { { guid, new List<TriplesToModify>() } };


                    foreach (Dictionary<string, SparqlObject.Data> fila in pFilas.Where(x => x["person"].value == id))
                    {
                        string idAux = fila["hIndexCitationCount"].value;
                        string person = fila["person"].value;
                        if (fila.ContainsKey("numAcumuladoCargadas"))
                        {
                            TriplesToModify t = new();
                            t.Predicate = "http://w3id.org/roh/hIndexCitationCount|http://w3id.org/roh/acumulatedPublicationNumber";
                            t.NewValue = idAux + "|" + fila["numAcumuladoACargar"].value;
                            t.OldValue = idAux + "|" + fila["numAcumuladoCargadas"].value;
                            triplesModify[guid].Add(t);
                        }
                        else if (fila.ContainsKey("numAcumuladoACargar"))
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/hIndexCitationCount|http://w3id.org/roh/acumulatedPublicationNumber";
                            t.NewValue = idAux + "|" + fila["numAcumuladoACargar"].value;
                            triplesInsert[guid].Add(t);
                        }

                        if (fila.ContainsKey("numMaxCargado"))
                        {
                            TriplesToModify t = new();
                            t.Predicate = "http://w3id.org/roh/hIndexCitationCount|http://w3id.org/roh/hIndexMax";
                            t.NewValue = idAux + "|" + fila["numMaxACargar"].value;
                            t.OldValue = idAux + "|" + fila["numMaxCargado"].value;
                            triplesModify[guid].Add(t);
                        }
                        else if (fila.ContainsKey("numMaxACargar"))
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/hIndexCitationCount|http://w3id.org/roh/hIndexMax";
                            t.NewValue = idAux + "|" + fila["numMaxACargar"].value;
                            triplesInsert[guid].Add(t);
                        }
                    }

                    if (triplesInsert[guid].Count > 0)
                    {
                        var resultado = mResourceApi.InsertPropertiesLoadedResources(triplesInsert);
                    }
                    if (triplesModify[guid].Count > 0)
                    {
                        var resultado = mResourceApi.ModifyPropertiesLoadedResources(triplesModify);
                    }
                });
            }
        }

        private void InsercionHIndex(List<Dictionary<string, SparqlObject.Data>> pFilas)
        {
            List<string> ids = pFilas.Select(x => x["person"].value).Distinct().ToList();
            if (ids.Count > 0)
            {
                Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, id =>
                {
                    Guid guid = mResourceApi.GetShortGuid(id);

                    Dictionary<Guid, List<TriplesToInclude>> triples = new() { { guid, new List<TriplesToInclude>() } };
                    foreach (Dictionary<string, SparqlObject.Data> fila in pFilas.Where(x => x["person"].value == id))
                    {
                        string idAux = mResourceApi.GraphsUrl + "items/HIndex_" + guid.ToString().ToLower() + "_" + Guid.NewGuid().ToString().ToLower();
                        string person = fila["person"].value;
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/hIndex|http://w3id.org/roh/h-index";
                            t.NewValue = idAux + "|" + fila["hIndexCalculado"].value;
                            triples[guid].Add(t);
                        }
                        {
                            TriplesToInclude t = new();
                            t.Predicate = "http://w3id.org/roh/hIndex|http://w3id.org/roh/citationSource";
                            t.NewValue = idAux + "|" + fila["source"].value;
                            triples[guid].Add(t);
                        }
                    }
                    if (triples[guid].Count > 0)
                    {
                        var resultado = mResourceApi.InsertPropertiesLoadedResources(triples);
                    }
                });
            }
        }

    }
}
