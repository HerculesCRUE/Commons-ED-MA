using CurriculumvitaeOntology;
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
    /// Clase para actualizar propiedades de CVs
    /// </summary>
    class ActualizadorCV : ActualizadorBase
    {
        /// <summary>
        /// Objeto para 'ModificarElementosCV'
        /// </summary>
        public class CVSection
        {
            /// <summary>
            /// Código de CVN
            /// </summary>
            public string cvnCode { get; set; }
            /// <summary>
            /// Grafo de la entidad
            /// </summary>
            public string graph { get; set; }
            /// <summary>
            /// rdf:type de la entidad
            /// </summary>
            public string rdfType { get; set; }
            /// <summary>
            /// rdf:type de la entidad auxiliar
            /// </summary>
            public string rdfTypeAux { get; set; }
            /// <summary>
            /// Propiedad que apunta a la sección
            /// </summary>
            public string sectionProperty { get; set; }
            /// <summary>
            /// Propiedad que apunta de la sección a la entidad auxiliar
            /// </summary>
            public string itemProperty { get; set; }
            public CVSection(string pCvnCode, string pGraph, string pRdfType, string pSectionProperty, string pItemProperty, string pRdfTypeAux)
            {
                cvnCode = pCvnCode;
                graph = pGraph;
                rdfType = pRdfType;
                sectionProperty = pSectionProperty;
                itemProperty = pItemProperty;
                rdfTypeAux = pRdfTypeAux;
            }
        }

        /// <summary>
        /// Objeto para 'ModificarOrganizacionesCV'
        /// </summary>
        public class OrgTitleCVTitleOrg
        {
            /// <summary>
            /// PropiedadAuxiliar
            /// </summary>
            public string propAux { get; set; }
            /// <summary>
            /// Grafo
            /// </summary>
            public string graph { get; set; }
            /// <summary>
            /// rdf:type de la entidad
            /// </summary>
            public string rdfType { get; set; }
            /// <summary>
            /// Propiedad con el título desnormalizado
            /// </summary>
            public string propTituloDesnormalizado { get; set; }
            /// <summary>
            /// Propiedad con la orgnización
            /// </summary>
            public string propOrganizacion { get; set; }
            public OrgTitleCVTitleOrg(string pGraph, string pRdfType, string pPropAux, string pPropTituloDesnormalizado, string pPropOrganizacion)
            {
                propAux = pPropAux;
                graph = pGraph;
                rdfType = pRdfType;
                propTituloDesnormalizado = pPropTituloDesnormalizado;
                propOrganizacion = pPropOrganizacion;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pResourceApi">API Wrapper de GNOSS</param>
        public ActualizadorCV(ResourceApi pResourceApi) : base(pResourceApi)
        {
        }

        #region Métodos públicos

        /// <summary>
        /// Crea un currículum para los investigadores activos (http://w3id.org/roh/isActive 'true')
        /// No tiene dependencias
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        /// <param name="pProjects">ID de proyectos</param>
        /// <param name="pGroups">ID de grupos</param>
        /// <param name="pDocuments">ID de documentos</param>
        /// <param name="pROs">ID de research objects</param>
        /// <param name="pPatents">ID de patentes</param>
        public void CrearCVs(List<string> pPersons = null, List<string> pProjects = null, List<string> pGroups = null, List<string> pDocuments = null, List<string> pROs = null, List<string> pPatents = null)
        {
            HashSet<string> filtersCrearCVs = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filtersCrearCVs.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersCrearCVs.Add($" ?projectAux <http://vivoweb.org/ontology/core#relates> ?relatesAux. ?relatesAux <http://w3id.org/roh/roleOf> ?person.  FILTER(?projectAux in (<{string.Join(">,<", pProjects)}>))");
            }
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersCrearCVs.Add($" ?groupAux <http://vivoweb.org/ontology/core#relates> ?relatesAux. ?relatesAux <http://w3id.org/roh/roleOf> ?person.  FILTER(?groupAux in (<{string.Join(">,<", pGroups)}>))");
            }
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersCrearCVs.Add($" ?docAux <http://purl.org/ontology/bibo/authorList> ?autoresAux. ?autoresAux <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.  FILTER(?docAux in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (pROs != null && pROs.Count > 0)
            {
                filtersCrearCVs.Add($" ?roAux <http://purl.org/ontology/bibo/authorList> ?autoresAux. ?autoresAux <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.  FILTER(?roAux in (<{string.Join(">,<", pROs)}>))");
            }
            if (pPatents != null && pPatents.Count > 0)
            {
                filtersCrearCVs.Add($" ?patentAux <http://purl.org/ontology/bibo/authorList> ?autoresAux. ?autoresAux <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.  FILTER(?patentAux in (<{string.Join(">,<", pPatents)}>))");
            }
            if (filtersCrearCVs.Count == 0)
            {
                filtersCrearCVs.Add("");
            }
            foreach (string filter in filtersCrearCVs)
            {
                while (true)
                {
                    //Creamos CVs
                    int limitCrearCVs = 50;
                    String selectCrearCVs = @"SELECT distinct ?person  ";
                    String whereCrearCVs = @$"  where{{
                                            {filter}
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.
                                            ?person <http://w3id.org/roh/isActive> 'true'.
                                            MINUS{{ ?cv  <http://w3id.org/roh/cvOf> ?person}}
                                        }} limit {limitCrearCVs}";

                    SparqlObject resultadoCrearCVs = mResourceApi.VirtuosoQueryMultipleGraph(selectCrearCVs, whereCrearCVs, new List<string>() { "person", "curriculumvitae", "project", "group", "document", "researchobject" });

                    // Personas que no poseen actualmente un CV y deberían tenerlo
                    List<string> persons = new();
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoCrearCVs.results.bindings)
                    {
                        persons.Add(fila["person"].value);
                    }

                    // Obtenemos los CV a cargar
                    mResourceApi.ChangeOntoly("curriculumvitae");
                    List<CV> listaCVCargar = GenerateCVFromPersons(persons);
                    Parallel.ForEach(listaCVCargar, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, cv =>
                    {
                        ComplexOntologyResource resource = cv.ToGnossApiResource(mResourceApi, null);
                        int numIntentos = 0;
                        while (!resource.Uploaded)
                        {
                            numIntentos++;
                            if (numIntentos > MAX_INTENTOS)
                            {
                                break;
                            }
                            if (listaCVCargar.Last() == cv)
                            {
                                mResourceApi.LoadComplexSemanticResource(resource, false, true);
                            }
                            else
                            {
                                mResourceApi.LoadComplexSemanticResource(resource);
                            }
                        }
                    });

                    if (resultadoCrearCVs.results.bindings.Count != limitCrearCVs)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos/eliminamos en los CV las publicaciones de las que el dueño del CV es autor con la privacidad correspondiente
        /// Depende de ActualizadorCV.CrearCVs
        /// </summary>        
        /// <param name="pPersons">IDs de la persona</param>
        /// <param name="pDocuments">IDs del documento</param>
        /// <param name="pCVs">IDs del CV</param>
        public void ModificarDocumentos(List<string> pPersons = null, List<string> pDocuments = null, List<string> pCVs = null)
        {
            HashSet<string> filtersModificarDocumentos = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filtersModificarDocumentos.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pDocuments != null && pDocuments.Count > 0)
            {
                filtersModificarDocumentos.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
            }
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersModificarDocumentos.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
            }
            if (filtersModificarDocumentos.Count == 0)
            {
                filtersModificarDocumentos.Add("");
            }

            foreach (string filter in filtersModificarDocumentos)
            {
                while (true)
                {
                    //Añadimos documentos
                    int limitAniadirDocumentos = 500;
                    String selectAniadirDocumentos = @"select distinct ?cv ?scientificActivity ?document ?isValidated ?typeDocument ";
                    String whereAniadirDocumentos = @$"where{{
                                    {filter}
                                    {{
                                        #DESEABLES
                                        select distinct ?person ?cv ?scientificActivity ?document ?isValidated ?typeDocument
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                            ?document a <http://purl.org/ontology/bibo/Document>.
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv <http://w3id.org/roh/cvOf> ?person.
                                            ?cv <http://w3id.org/roh/scientificActivity> ?scientificActivity.
                                            ?document <http://purl.org/ontology/bibo/authorList> ?autor.
                                            ?autor <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                            ?document <http://w3id.org/roh/scientificActivityDocument> ?scientificActivityDocument.
                                            OPTIONAL{{?document <http://w3id.org/roh/isValidated> ?isValidated.}}
                                            ?scientificActivityDocument <http://purl.org/dc/elements/1.1/identifier> ?typeDocument.
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificActivity> ?scientificActivity.
                                        {{
                                                ?scientificActivity <http://w3id.org/roh/scientificPublications> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?document.
                                                BIND(""SAD1"" as ?typeDocument)
                                        }}
                                        UNION
                                        {{
                                                ?scientificActivity <http://w3id.org/roh/worksSubmittedConferences> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?document.
                                                BIND(""SAD2"" as ?typeDocument)
                                        }}
                                        UNION
                                        {{
                                                ?scientificActivity <http://w3id.org/roh/worksSubmittedSeminars> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?document.
                                                BIND(""SAD3"" as ?typeDocument)
                                        }}
                                    }}
                                }}order by desc(?cv) limit {limitAniadirDocumentos}";
                    SparqlObject resultadoAniadirDocumentos = mResourceApi.VirtuosoQueryMultipleGraph(selectAniadirDocumentos, whereAniadirDocumentos, new List<string>() { "curriculumvitae", "document", "person", "scientificactivitydocument" });
                    InsertarDocumentosCV(resultadoAniadirDocumentos);
                    if (resultadoAniadirDocumentos.results.bindings.Count != limitAniadirDocumentos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Elminamos documentos
                    int limitEliminarDocumentos = 500;
                    String selectEliminarDocumentos = @"select distinct ?cv ?scientificActivity ?item ?typeDocument ";
                    String whereEliminarDocumentos = @$"where{{
                                    {filter}                                    
                                    {{
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?document a <http://purl.org/ontology/bibo/Document>.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificActivity> ?scientificActivity.
                                        {{
                                                ?scientificActivity <http://w3id.org/roh/scientificPublications> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?document.
                                                BIND(""SAD1"" as ?typeDocument)
                                        }}
                                        UNION
                                        {{
                                                ?scientificActivity <http://w3id.org/roh/worksSubmittedConferences> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?document.
                                                BIND(""SAD2"" as ?typeDocument)
                                        }}
                                        UNION
                                        {{
                                                ?scientificActivity <http://w3id.org/roh/worksSubmittedSeminars> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?document.
                                                BIND(""SAD3"" as ?typeDocument)
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        #DESEABLES
                                        select distinct ?person ?cv ?scientificActivity ?document ?typeDocument
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                            ?document a <http://purl.org/ontology/bibo/Document>.
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv <http://w3id.org/roh/cvOf> ?person.
                                            ?cv <http://w3id.org/roh/scientificActivity> ?scientificActivity.
                                            ?document <http://purl.org/ontology/bibo/authorList> ?autor.
                                            ?autor <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                            ?document <http://w3id.org/roh/scientificActivityDocument> ?scientificActivityDocument.
                                            ?scientificActivityDocument  <http://purl.org/dc/elements/1.1/identifier> ?typeDocument.
                                        }}                                        
                                    }}
                                }}order by desc(?cv) limit {limitEliminarDocumentos}";
                    SparqlObject resultadoEliminarDocumentos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarDocumentos, whereEliminarDocumentos, new List<string>() { "curriculumvitae", "document", "person", "scientificactivitydocument" });
                    EliminarDocumentosCV(resultadoEliminarDocumentos);
                    if (resultadoEliminarDocumentos.results.bindings.Count != limitEliminarDocumentos)
                    {
                        break;
                    }
                }

                EliminarDuplicados(pCVs);
                EliminarItemsEliminados(pCVs);
            }
        }

        ///// <summary>
        ///// Modifica la privacidad de las publicaciones de los CV en caso de que haya que hacerlo
        ///// (Solo convierte en públicos aquellos documentos que sean privados pero deberían ser públicos)
        ///// Depende de ActualizadorCV.CrearCVs
        ///// </summary>
        ///// <param name="pPersons">IDs de la persona</param>
        ///// <param name="pDocuments">IDs del documento</param>
        ///// <param name="pCVs">IDs del CV</param>
        //public void CambiarPrivacidadDocumentos(List<string> pPersons = null, List<string> pDocuments = null, List<string> pCVs = null)
        //{
        //    HashSet<string> filtersCambiarPrivacidadDocumentos = new HashSet<string>();
        //    if (pPersons != null && pPersons.Count > 0)
        //    {
        //        filtersCambiarPrivacidadDocumentos.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
        //    }
        //    if (pDocuments != null && pDocuments.Count > 0)
        //    {
        //        filtersCambiarPrivacidadDocumentos.Add($" FILTER(?document in (<{string.Join(">,<", pDocuments)}>))");
        //    }
        //    if (pCVs != null && pCVs.Count > 0)
        //    {
        //        filtersCambiarPrivacidadDocumentos.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
        //    }
        //    if (filters.Count == 0)
        //    {
        //        filtersCambiarPrivacidadDocumentos.Add("");
        //    }

        //    foreach (string filter in filtersCambiarPrivacidadDocumentos)
        //    {

        //        while (true)
        //        {
        //            //Publicamos los documentos
        //            int limit = 500;
        //            String select = @"select distinct ?cv ?scientificActivity ?propItem ?item ";
        //            String where = @$"where{{
        //                        {filter}
        //                        {{
        //                            ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
        //                            ?document a <http://purl.org/ontology/bibo/Document>.
        //                            ?document <http://w3id.org/roh/isValidated> 'true'.
        //                            ?cv a <http://w3id.org/roh/CV>.
        //                            ?cv <http://w3id.org/roh/cvOf> ?person.
        //                            ?cv <http://w3id.org/roh/scientificActivity> ?scientificActivity.                                        
        //                            ?scientificActivity ?propItem ?item.
        //                            ?item <http://vivoweb.org/ontology/core#relatedBy> ?document.
        //                            ?item <http://w3id.org/roh/isPublic> 'false'.
        //                        }}
        //                    }}order by desc(?cv) limit {limit}";
        //            SparqlObject resultado = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "curriculumvitae", "document","person"});
        //            PublicarDocumentosCV(resultado);
        //            if (resultado.results.bindings.Count != limit)
        //            {
        //                break;
        //            }
        //        }
        //    }
        //}


        /// <summary>       
        /// Insertamos/eliminamos en los CV los researchobjects de las que el dueño del CV es autor con la privacidad correspondiente
        /// Depende de ActualizadorCV.CrearCVs
        /// </summary>        
        /// <param name="pPersons">IDs de la persona</param>
        /// <param name="pROs">IDs del research object</param>
        /// <param name="pCVs">IDs del CV</param>
        public void ModificarResearchObjects(List<string> pPersons = null, List<string> pROs = null, List<string> pCVs = null)
        {
            HashSet<string> filtersModificarResearchObjects = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filtersModificarResearchObjects.Add($" FILTER(?person in ( <{string.Join(">,<", pPersons)}>))");
            }
            if (pROs != null && pROs.Count > 0)
            {
                filtersModificarResearchObjects.Add($" FILTER(?ro in ( <{string.Join(">,<", pROs)}>))");
            }
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersModificarResearchObjects.Add($" FILTER(?cv in ( <{string.Join(">,<", pCVs)}>))");
            }
            if (filtersModificarResearchObjects.Count == 0)
            {
                filtersModificarResearchObjects.Add("");
            }

            foreach (string filter in filtersModificarResearchObjects)
            {
                while (true)
                {
                    //Añadimos documentos
                    int limitAniadirDocumentos = 500;
                    String selectAniadirDocumentos = @"select distinct ?cv ?researchObject ?ro ";
                    String whereAniadirDocumentos = @$"where{{
                                    {filter}
                                    {{
                                        #DESEABLES
                                        select distinct ?person ?cv ?researchObject ?ro 
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                            ?ro a <http://w3id.org/roh/ResearchObject>.
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv <http://w3id.org/roh/cvOf> ?person.
                                            ?cv <http://w3id.org/roh/researchObject> ?researchObject.
                                            ?ro <http://purl.org/ontology/bibo/authorList> ?autor.
                                            ?autor <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?ro a <http://w3id.org/roh/ResearchObject>.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/researchObject> ?researchObject.
                                        ?researchObject <http://w3id.org/roh/researchObjects> ?item.
                                        ?item <http://vivoweb.org/ontology/core#relatedBy> ?ro.
                                    }}
                                }}order by desc(?cv) limit {limitAniadirDocumentos}";
                    SparqlObject resultadoAniadirDocumentos = mResourceApi.VirtuosoQueryMultipleGraph(selectAniadirDocumentos, whereAniadirDocumentos, new List<string>() { "curriculumvitae", "researchobject", "person" });
                    InsertarResearchObjectsCV(resultadoAniadirDocumentos);
                    if (resultadoAniadirDocumentos.results.bindings.Count != limitAniadirDocumentos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Elminamos documentos
                    int limitEliminarDocumentos = 500;
                    String selectEliminarDocumentos = @"select distinct ?cv ?researchObject ?item ";
                    String whereEliminarDocumentos = @$"where{{
                                    {filter}                                    
                                    {{
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?ro a <http://w3id.org/roh/ResearchObject>.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/researchObject> ?researchObject.
                                        ?researchObject <http://w3id.org/roh/researchObjects> ?item.
                                        ?item <http://vivoweb.org/ontology/core#relatedBy> ?ro.
                                    }}
                                    MINUS
                                    {{
                                        #DESEABLES
                                        select distinct ?person ?cv ?researchObject ?ro 
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                            ?ro a <http://w3id.org/roh/ResearchObject>.
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv <http://w3id.org/roh/cvOf> ?person.
                                            ?cv <http://w3id.org/roh/researchObject> ?researchObject.
                                            ?ro <http://purl.org/ontology/bibo/authorList> ?autor.
                                            ?autor <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                        }}                                      
                                    }}
                                }}order by desc(?cv) limit {limitEliminarDocumentos}";
                    SparqlObject resultadoEliminarDocumentos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarDocumentos, whereEliminarDocumentos, new List<string>() { "curriculumvitae", "researchobject", "person" });
                    EliminarResearchObjectsCV(resultadoEliminarDocumentos);
                    if (resultadoEliminarDocumentos.results.bindings.Count != limitEliminarDocumentos)
                    {
                        break;
                    }
                }

                EliminarDuplicados(pCVs);
                EliminarItemsEliminados(pCVs);
            }
        }

        /// <summary>
        /// Modifica la privacidad de los researchobjects de los CV en caso de que haya que hacerlo
        /// (Solo convierte en públicos aquellos researchobjects que sean privados pero deberían ser públicos)
        /// Depende de ActualizadorCV.CrearCVs
        /// </summary>
        /// <param name="pPersons">IDs de la persona</param>
        /// <param name="pROs">IDs del research object</param>
        /// <param name="pCVs">IDs del CV</param>
        public void CambiarPrivacidadResearchObjects(List<string> pPersons = null, List<string> pROs = null, List<string> pCVs = null)
        {
            HashSet<string> filtersCambiarPrivacidadResearchObjects = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filtersCambiarPrivacidadResearchObjects.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pROs != null && pROs.Count > 0)
            {
                filtersCambiarPrivacidadResearchObjects.Add($" FILTER(?ro in (<{string.Join(">,<", pROs)}>))");
            }
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersCambiarPrivacidadResearchObjects.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
            }
            if (filtersCambiarPrivacidadResearchObjects.Count == 0)
            {
                filtersCambiarPrivacidadResearchObjects.Add("");
            }

            foreach (string filter in filtersCambiarPrivacidadResearchObjects)
            {
                while (true)
                {
                    //Publicamos los documentos
                    int limitCambiarPrivacidadResearchObjects = 500;
                    String selectCambiarPrivacidadResearchObjects = @"select distinct ?cv ?researchObject ?propItem ?item ";
                    String whereCambiarPrivacidadResearchObjects = @$"where{{
                                    {filter}
                                    {{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?ro a <http://w3id.org/roh/ResearchObject>.
                                        ?ro <http://w3id.org/roh/isValidated> 'true'.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/researchObject> ?researchObject.
                                        ?researchObject ?propItem ?item.
                                        ?item <http://vivoweb.org/ontology/core#relatedBy> ?ro.
                                        ?item <http://w3id.org/roh/isPublic> 'false'.
                                    }}
                                }}order by desc(?cv) limit {limitCambiarPrivacidadResearchObjects}";
                    SparqlObject resultadoCambiarPrivacidadResearchObjects = mResourceApi.VirtuosoQueryMultipleGraph(selectCambiarPrivacidadResearchObjects, whereCambiarPrivacidadResearchObjects, new List<string>() { "curriculumvitae", "researchobject", "person" });
                    PublicarResearchObjectsCV(resultadoCambiarPrivacidadResearchObjects);
                    if (resultadoCambiarPrivacidadResearchObjects.results.bindings.Count != limitCambiarPrivacidadResearchObjects)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Insertamos/eliminamos en los CV los proyectos oficiales (con http://w3id.org/roh/isValidated='true' ) de los que el dueño del CV ha sido miembro y les ponemos privacidad pública
        /// Depende de ActualizadorCV.CrearCVs
        /// </summary>        
        /// <param name="pPersons">IDs de la persona</param>
        /// <param name="pProjects">IDs del documento</param>
        /// <param name="pCVs">IDs del CV</param>
        public void ModificarProyectos(List<string> pPersons = null, List<string> pProjects = null, List<string> pCVs = null)
        {
            HashSet<string> filtersModificarProyectos = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filtersModificarProyectos.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pProjects != null && pProjects.Count > 0)
            {
                filtersModificarProyectos.Add($" FILTER(?project in (<{string.Join(">,<", pProjects)}>))");
            }
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersModificarProyectos.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
            }
            if (filtersModificarProyectos.Count == 0)
            {
                filtersModificarProyectos.Add("");
            }

            foreach (string filter in filtersModificarProyectos)
            {
                while (true)
                {
                    //Añadimos proyectos
                    int limitAniadirProyectos = 500;
                    String selectAniadirProyectos = @"select distinct ?cv ?scientificExperience ?project ?typeProject ";
                    String whereAniadirProyectos = @$"where{{
                                    {filter}
                                    {{
                                        #DESEABLES
                                        select distinct ?cv ?scientificExperience ?project ?typeProject 
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                            ?project a <http://vivoweb.org/ontology/core#Project>.
                                            ?project <http://w3id.org/roh/isValidated> 'true'.
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv <http://w3id.org/roh/cvOf> ?person.
                                            ?cv <http://w3id.org/roh/scientificExperience> ?scientificExperience.
                                            ?project <http://vivoweb.org/ontology/core#relates> ?rol.
                                            ?rol <http://w3id.org/roh/roleOf> ?person.
                                            ?project <http://w3id.org/roh/scientificExperienceProject> ?scientificExperienceProject.
                                            ?scientificExperienceProject <http://purl.org/dc/elements/1.1/identifier> ?typeProject.
                                        }}
                                    }}
                                    MINUS
                                    {{
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?project a <http://vivoweb.org/ontology/core#Project>.
                                        ?project <http://w3id.org/roh/isValidated> 'true'.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?scientificExperience.
                                        {{
                                                ?scientificExperience <http://w3id.org/roh/competitiveProjects> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?project.
                                                BIND(""SEP1"" as ?typeProject)
                                        }}
                                        UNION
                                        {{
                                                ?scientificExperience <http://w3id.org/roh/nonCompetitiveProjects> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?project.
                                                BIND(""SEP2"" as ?typeProject)
                                        }}
                                    }}
                                }}order by desc(?cv) limit {limitAniadirProyectos}";
                    SparqlObject resultadoAniadirProyectos = mResourceApi.VirtuosoQueryMultipleGraph(selectAniadirProyectos, whereAniadirProyectos, new List<string>() { "curriculumvitae", "project", "person", "scientificexperienceproject" });
                    InsertarProyectosCV(resultadoAniadirProyectos);
                    if (resultadoAniadirProyectos.results.bindings.Count != limitAniadirProyectos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Elminamos proyectos
                    int limitEliminarProyectos = 500;
                    String selectEliminarProyectos = @"select distinct ?cv ?scientificExperience ?project ?item ?typeProject ";
                    String whereEliminarProyectos = @$"where{{
                                    {filter}
                                    
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?project a <http://vivoweb.org/ontology/core#Project>.
                                        ?project <http://w3id.org/roh/isValidated> 'true'.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?scientificExperience.
                                        {{
                                                ?scientificExperience <http://w3id.org/roh/competitiveProjects> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?project.
                                                BIND(""SEP1"" as ?typeProject)
                                        }}
                                        UNION
                                        {{
                                                ?scientificExperience <http://w3id.org/roh/nonCompetitiveProjects> ?item.
                                                ?item <http://vivoweb.org/ontology/core#relatedBy> ?project.
                                                BIND(""SEP2"" as ?typeProject)
                                        }}                                       
                                    
                                    MINUS
                                    {{
                                        #DESEABLES
                                        select distinct ?cv ?scientificExperience ?project ?typeProject 
                                        Where
                                        {{
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                            ?project a <http://vivoweb.org/ontology/core#Project>.
                                            ?project <http://w3id.org/roh/isValidated> 'true'.
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv <http://w3id.org/roh/cvOf> ?person.
                                            ?cv <http://w3id.org/roh/scientificExperience> ?scientificExperience.
                                            ?project <http://vivoweb.org/ontology/core#relates> ?rol.
                                            ?rol <http://w3id.org/roh/roleOf> ?person.
                                            ?project <http://w3id.org/roh/scientificExperienceProject> ?scientificExperienceProject.
                                            ?scientificExperienceProject <http://purl.org/dc/elements/1.1/identifier> ?typeProject.
                                        }}
                                    }}
                                }}order by desc(?cv) limit {limitEliminarProyectos}";
                    SparqlObject resultadoEliminarProyectos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarProyectos, whereEliminarProyectos, new List<string>() { "curriculumvitae", "project", "person", "scientificexperienceproject" });
                    EliminarProyectosCV(resultadoEliminarProyectos);
                    if (resultadoEliminarProyectos.results.bindings.Count != limitEliminarProyectos)
                    {
                        break;
                    }
                }

                EliminarDuplicados(pCVs);
                EliminarItemsEliminados(pCVs);
            }
        }

        /// <summary>
        /// Insertamos/eliminamos en los CV los grupos oficiales (con http://w3id.org/roh/isValidated='true' ) de los que el dueño del CV ha sido miembro y les ponemos privacidad pública
        /// Depende de ActualizadorCV.CrearCVs
        /// </summary>        
        /// <param name="pPersons">IDs de las personas</param>
        /// <param name="pGroups">IDs de los grupos</param>
        /// <param name="pCVs">IDs de los CVs</param>
        public void ModificarGrupos(List<string> pPersons = null, List<string> pGroups = null, List<string> pCVs = null)
        {
            HashSet<string> filtersModificarGrupos = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filtersModificarGrupos.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pGroups != null && pGroups.Count > 0)
            {
                filtersModificarGrupos.Add($" FILTER(?group in (<{string.Join(">,<", pGroups)}>))");
            }
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersModificarGrupos.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
            }
            if (filtersModificarGrupos.Count == 0)
            {
                filtersModificarGrupos.Add("");
            }

            foreach (string filter in filtersModificarGrupos)
            {
                while (true)
                {
                    //Añadimos grupos
                    int limitAniadirGrupos = 500;
                    String selectAniadirGrupos = @"select distinct ?cv ?idSection ?item 
                                        'http://w3id.org/roh/RelatedGroup' as ?rdfTypeAux 
                                        'http://w3id.org/roh/scientificExperience' as ?sectionProperty
                                        'http://w3id.org/roh/groups' as ?auxProperty  ";
                    String whereAniadirGrupos = @$"where{{
                                    {filter} 
                                    {{
                                        #DESEABLES                                        
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?item a <http://xmlns.com/foaf/0.1/Group>.
                                        ?item <http://w3id.org/roh/isValidated> 'true'.
                                        ?item <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?idSection.       
                                        ?item <http://vivoweb.org/ontology/core#relates> ?rol.
                                        ?rol <http://w3id.org/roh/roleOf> ?person.
                                    }}
                                    MINUS
                                    {{
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?item a <http://xmlns.com/foaf/0.1/Group>.
                                        ?item <http://w3id.org/roh/isValidated> 'true'.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?idSection.
                                        ?idSection <http://w3id.org/roh/groups> ?auxSection.
                                        ?auxSection <http://vivoweb.org/ontology/core#relatedBy> ?item.
                                    }}
                                }}order by desc(?cv) limit {limitAniadirGrupos}";
                    SparqlObject resultadoAniadirGrupos = mResourceApi.VirtuosoQueryMultipleGraph(selectAniadirGrupos, whereAniadirGrupos, new List<string>() { "curriculumvitae", "group", "person" });
                    InsertarItemsCV(resultadoAniadirGrupos);

                    if (resultadoAniadirGrupos.results.bindings.Count != limitAniadirGrupos)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Elminamos grupos
                    int limitEliminarGrupos = 500;
                    String selectEliminarGrupos = @"select distinct ?cv ?idSection ?auxEntity 
                                        'http://w3id.org/roh/scientificExperience' as ?sectionProperty
                                        'http://w3id.org/roh/groups' as ?auxProperty ";
                    String whereEliminarGrupos = @$"where{{
                                    {filter}                                    
                                    {{
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        ?group <http://w3id.org/roh/isValidated> 'true'.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?idSection.
                                        ?idSection <http://w3id.org/roh/groups> ?auxEntity.
                                        ?auxEntity <http://vivoweb.org/ontology/core#relatedBy> ?group.                                
                                    }}
                                    MINUS
                                    {{
                                        #DESEABLES                                        
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        ?group <http://w3id.org/roh/isValidated> 'true'.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?idSection.       
                                        ?group <http://vivoweb.org/ontology/core#relates> ?rol.
                                        ?rol <http://w3id.org/roh/roleOf> ?person.
                                    }}
                                }}order by desc(?cv) limit {limitEliminarGrupos}";
                    SparqlObject resultadoEliminarGrupos = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarGrupos, whereEliminarGrupos, new List<string>() { "curriculumvitae", "group", "person" });
                    EliminarItemsCV(resultadoEliminarGrupos);
                    if (resultadoEliminarGrupos.results.bindings.Count != limitEliminarGrupos)
                    {
                        break;
                    }
                }

                EliminarDuplicados(pCVs);
                EliminarItemsEliminados(pCVs);
            }
        }

        /// <summary>
        /// Insertamos/eliminamos en los CV las patentes oficiales (con http://w3id.org/roh/crisIdentifier ) de los que el dueño del CV es autor y les ponemos privacidad pública
        /// Depende de ActualizadorCV.CrearCVs
        /// </summary>        
        /// <param name="pPersons">IDs de la persona</param>
        /// <param name="pPatents">IDs de patentes</param>
        /// <param name="pCVs">IDs del CV</param>
        public void ModificarPatentes(List<string> pPersons = null, List<string> pPatents = null, List<string> pCVs = null)
        {
            HashSet<string> filtersModificarPatentes = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filtersModificarPatentes.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pPatents != null && pPatents.Count > 0)
            {
                filtersModificarPatentes.Add($" FILTER(?patent in (<{string.Join(">,<", pPatents)}>))");
            }
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersModificarPatentes.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
            }
            if (filtersModificarPatentes.Count == 0)
            {
                filtersModificarPatentes.Add("");
            }

            foreach (string filter in filtersModificarPatentes)
            {
                while (true)
                {
                    //Añadimos patentes
                    int limitAniadirPatentes = 500;
                    String selectAniadirPatentes = @"select distinct ?cv ?idSection ?item 'http://w3id.org/roh/RelatedPatent' as ?rdfTypeAux 
                                        'http://w3id.org/roh/scientificExperience' as ?sectionProperty
                                        'http://w3id.org/roh/patents' as ?auxProperty ";
                    String whereAniadirPatentes = @$"where{{
                                    {filter}
                                    {{
                                        #DESEABLES                                        
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?item a <http://purl.org/ontology/bibo/Patent>.
                                        ?item <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                        ?item <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?idSection.   
                                        ?item <http://purl.org/ontology/bibo/authorList> ?rol.
                                        ?rol <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
                                    }}
                                    MINUS
                                    {{
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?item a <http://purl.org/ontology/bibo/Patent>.
                                        ?item <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?idSection.
                                        ?idSection <http://w3id.org/roh/patents> ?auxSection.
                                        ?auxSection <http://vivoweb.org/ontology/core#relatedBy> ?item.
                                    }}
                                }}order by desc(?cv) limit {limitAniadirPatentes}";
                    SparqlObject resultadoAniadirPatentes = mResourceApi.VirtuosoQueryMultipleGraph(selectAniadirPatentes, whereAniadirPatentes, new List<string>() { "curriculumvitae", "patent", "person" });
                    InsertarItemsCV(resultadoAniadirPatentes);
                    if (resultadoAniadirPatentes.results.bindings.Count != limitAniadirPatentes)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Elminamos patentes
                    int limitEliminarPatentes = 500;
                    String selectEliminarPatentes = @"select distinct ?cv ?idSection ?auxEntity 
                                        'http://w3id.org/roh/scientificExperience' as ?sectionProperty
                                        'http://w3id.org/roh/patents' as ?auxProperty      
                                        ?group ";
                    String whereEliminarPatentes = @$"where{{
                                    {filter}                                    
                                    {{
                                        #ACTUALES
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        ?group <http://w3id.org/roh/isValidated> 'true'.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?idSection.
                                        ?idSection <http://w3id.org/roh/groups> ?auxEntity.
                                        ?auxEntity <http://vivoweb.org/ontology/core#relatedBy> ?group.                                
                                    }}
                                    MINUS
                                    {{
                                        #DESEABLES                                        
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                        ?group a <http://xmlns.com/foaf/0.1/Group>.
                                        ?group <http://w3id.org/roh/isValidated> 'true'.
                                        ?cv a <http://w3id.org/roh/CV>.
                                        ?cv <http://w3id.org/roh/cvOf> ?person.
                                        ?cv <http://w3id.org/roh/scientificExperience> ?idSection.       
                                        ?group <http://vivoweb.org/ontology/core#relates> ?rol.
                                        ?rol <http://w3id.org/roh/roleOf> ?person.
                                    }}
                                }}order by desc(?cv) limit {limitEliminarPatentes}";
                    SparqlObject resultadoEliminarPatentes = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarPatentes, whereEliminarPatentes, new List<string>() { "curriculumvitae", "patent", "person" });
                    EliminarItemsCV(resultadoEliminarPatentes);
                    if (resultadoEliminarPatentes.results.bindings.Count != limitEliminarPatentes)
                    {
                        break;
                    }
                }

                EliminarDuplicados(pCVs);
                EliminarItemsEliminados(pCVs);
            }
        }


        /// <summary>
        /// Modifica los elementos del CV agrgandolos/eliminandolos del CV
        /// Depende de ActualizadorCV.CrearCVs
        /// </summary>
        /// <param name="pPersons">>IDs de las personas</param>
        /// <param name="pCVs">IDs de los CVs</param>
        public void ModificarElementosCV(List<string> pPersons = null, List<string> pCVs = null)
        {
            List<CVSection> listaSecciones = new List<CVSection>();

            //Situación profesional actual
            listaSecciones.Add(new CVSection("010.010.000.000", "position", $"{GetUrlPrefix("vivo")}Position", $"{GetUrlPrefix("roh")}professionalSituation", $"{GetUrlPrefix("roh")}currentProfessionalSituation", $"{GetUrlPrefix("roh")}RelatedCurrentProfessionalSituation"));
            //Cargos y actividades desempeñados con anterioridad
            listaSecciones.Add(new CVSection("010.020.000.000", "position", $"{GetUrlPrefix("vivo")}Position", $"{GetUrlPrefix("roh")}professionalSituation", $"{GetUrlPrefix("roh")}previousPositions", $"{GetUrlPrefix("roh")}RelatedPreviousPositions"));

            //Estudios de 1º y 2º ciclo, y antiguos ciclos
            listaSecciones.Add(new CVSection("020.010.010.000", "academicdegree", $"{GetUrlPrefix("vivo")}AcademicDegree", $"{GetUrlPrefix("roh")}qualifications", $"{GetUrlPrefix("roh")}firstSecondCycles", $"{GetUrlPrefix("roh")}RelatedFirstSecondCycles"));
            //Doctorados
            listaSecciones.Add(new CVSection("020.010.020.000", "academicdegree", $"{GetUrlPrefix("vivo")}AcademicDegree", $"{GetUrlPrefix("roh")}qualifications", $"{GetUrlPrefix("roh")}doctorates", $"{GetUrlPrefix("roh")}RelatedDoctorates"));
            //Conocimiento de idiomas
            listaSecciones.Add(new CVSection("020.060.000.000", "languagecertificate", $"{GetUrlPrefix("roh")}LanguageCertificate", $"{GetUrlPrefix("roh")}qualifications", $"{GetUrlPrefix("roh")}languageSkills", $"{GetUrlPrefix("roh")}RelatedLanguageSkills"));
            //Otra formación universitaria de posgrado
            listaSecciones.Add(new CVSection("020.010.030.000", "academicdegree", $"{GetUrlPrefix("vivo")}AcademicDegree", $"{GetUrlPrefix("roh")}qualifications", $"{GetUrlPrefix("roh")}postgraduates", $"{GetUrlPrefix("roh")}RelatedPostGraduates"));
            //Formación especializada
            listaSecciones.Add(new CVSection("020.020.000.000", "academicdegree", $"{GetUrlPrefix("vivo")}AcademicDegree", $"{GetUrlPrefix("roh")}qualifications", $"{GetUrlPrefix("roh")}specialisedTraining", $"{GetUrlPrefix("roh")}RelatedSpecialisedTrainings"));
            //Cursos y semin. mejora docente
            listaSecciones.Add(new CVSection("020.050.000.000", "academicdegree", $"{GetUrlPrefix("vivo")}AcademicDegree", $"{GetUrlPrefix("roh")}qualifications", $"{GetUrlPrefix("roh")}coursesAndSeminars", $"{GetUrlPrefix("roh")}RelatedCoursesAndSeminars"));

            //Dirección tesis y/o proyectos
            listaSecciones.Add(new CVSection("030.040.000.000", "thesissupervision", $"{GetUrlPrefix("roh")}ThesisSupervision", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}thesisSupervisions", $"{GetUrlPrefix("roh")}RelatedThesisSupervisions"));
            //Formación académica impartida
            listaSecciones.Add(new CVSection("030.010.000.000", "impartedacademictraining", $"{GetUrlPrefix("roh")}ImpartedAcademicTraining", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}impartedAcademicTrainings", $"{GetUrlPrefix("roh")}RelatedImpartedAcademicTrainings"));
            //Tutorías académicas
            listaSecciones.Add(new CVSection("030.050.000.000", "tutorship", $"{GetUrlPrefix("roh")}Tutorship", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}academicTutorials", $"{GetUrlPrefix("roh")}RelatedAcademicTutorials"));
            //Cursos y semin. impartidos
            listaSecciones.Add(new CVSection("030.060.000.000", "impartedcoursesseminars", $"{GetUrlPrefix("roh")}ImpartedCoursesSeminars", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}impartedCoursesSeminars", $"{GetUrlPrefix("roh")}RelatedImpartedCoursesSeminars"));
            //Publicaciones docentes
            listaSecciones.Add(new CVSection("030.070.000.000", "teachingpublication", $"{GetUrlPrefix("roh")}TeachingPublication", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}teachingPublications", $"{GetUrlPrefix("roh")}RelatedTeachingPublications"));
            //Participac. proyectos innov. docente
            listaSecciones.Add(new CVSection("030.080.000.000", "teachingproject", $"{GetUrlPrefix("roh")}TeachingProject", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}teachingProjects", $"{GetUrlPrefix("roh")}RelatedTeachingProjects"));
            //Participac. congresos formac. docente
            listaSecciones.Add(new CVSection("030.090.000.000", "teachingcongress", $"{GetUrlPrefix("roh")}TeachingCongress", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}teachingCongress", $"{GetUrlPrefix("roh")}RelatedTeachingCongress"));
            //Premios innov. docente
            listaSecciones.Add(new CVSection("060.030.080.000", "accreditation", $"{GetUrlPrefix("roh")}Accreditation", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}teachingInnovationAwardsReceived", $"{GetUrlPrefix("roh")}RelatedTeachingInnovationAwardsReceived"));
            //Otras actividades
            listaSecciones.Add(new CVSection("030.100.000.000", "activity", $"{GetUrlPrefix("roh")}Activity", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}otherActivities", $"{GetUrlPrefix("roh")}RelatedOtherActivities"));
            //Aportaciones relevantes
            listaSecciones.Add(new CVSection("030.110.000.000", "activity", $"{GetUrlPrefix("roh")}Activity", $"{GetUrlPrefix("roh")}teachingExperience", $"{GetUrlPrefix("roh")}mostRelevantContributions", $"{GetUrlPrefix("roh")}RelatedMostRelevantContributions"));

            //Obras artísticas dirigidas
            listaSecciones.Add(new CVSection("050.020.030.000", "supervisedartisticproject", $"{GetUrlPrefix("roh")}SupervisedArtisticProject", $"{GetUrlPrefix("roh")}scientificExperience", $"{GetUrlPrefix("roh")}supervisedArtisticProjects", $"{GetUrlPrefix("roh")}RelatedSupervisedArtisticProject"));
            //Resultados tecnológicos
            listaSecciones.Add(new CVSection("050.030.020.000", "technologicalresult", $"{GetUrlPrefix("roh")}TechnologicalResult", $"{GetUrlPrefix("roh")}scientificExperience", $"{GetUrlPrefix("roh")}technologicalResults", $"{GetUrlPrefix("roh")}RelatedTechnologicalResult"));

            //Comités científicos, técnicos y/o asesores
            listaSecciones.Add(new CVSection("060.020.010.000", "committee", $"{GetUrlPrefix("roh")}Committee", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}committees", $"{GetUrlPrefix("roh")}RelatedCommittee"));
            //Organiz. activ. I+D+i
            listaSecciones.Add(new CVSection("060.020.030.000", "activity", $"{GetUrlPrefix("roh")}Activity", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}activitiesOrganization", $"{GetUrlPrefix("roh")}RelatedActivityOrganization"));
            //Gestión I+D+i
            listaSecciones.Add(new CVSection("060.020.040.000", "activity", $"{GetUrlPrefix("roh")}Activity", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}activitiesManagement", $"{GetUrlPrefix("roh")}RelatedActivityManagement"));
            //Producción científica
            listaSecciones.Add(new CVSection("060.010.000.000", "scientificproduction", $"{GetUrlPrefix("roh")}ScientificProduction", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}scientificProduction", $"{GetUrlPrefix("roh")}RelatedScientificProduction"));
            //Otras actividades divulgación
            listaSecciones.Add(new CVSection("060.010.040.000", "activity", $"{GetUrlPrefix("roh")}Activity", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}otherDisseminationActivities", $"{GetUrlPrefix("roh")}RelatedOtherDisseminationActivity"));
            //Foros y comités
            listaSecciones.Add(new CVSection("060.020.050.000", "activity", $"{GetUrlPrefix("roh")}Activity", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}forums", $"{GetUrlPrefix("roh")}RelatedForum"));
            //Evaluación y revisión de proyectos y artículos de I+D+i
            listaSecciones.Add(new CVSection("060.020.060.000", "activity", $"{GetUrlPrefix("roh")}Activity", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}researchEvaluations", $"{GetUrlPrefix("roh")}RelatedResearchEvaluation"));
            //Estancias en centros I+D+i
            listaSecciones.Add(new CVSection("060.010.050.000", "stay", $"{GetUrlPrefix("roh")}Stay", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}stays", $"{GetUrlPrefix("roh")}RelatedStay"));
            //Ayudas y becas obtenidas
            listaSecciones.Add(new CVSection("060.030.010.000", "grant", $"{GetUrlPrefix("vivo")}Grant", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}grants", $"{GetUrlPrefix("roh")}RelatedGrant"));
            //Otros modos de colaboración
            listaSecciones.Add(new CVSection("060.020.020.000", "collaboration", $"{GetUrlPrefix("roh")}Collaboration", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}otherCollaborations", $"{GetUrlPrefix("roh")}RelatedOtherCollaboration"));
            //Sdades. Científicas y Asoc. Profesionales
            listaSecciones.Add(new CVSection("060.030.020.000", "society", $"{GetUrlPrefix("roh")}Society", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}societies", $"{GetUrlPrefix("roh")}RelatedSociety"));
            //Consejos editoriales
            listaSecciones.Add(new CVSection("060.030.030.000", "council", $"{GetUrlPrefix("roh")}Council", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}councils", $"{GetUrlPrefix("roh")}RelatedCouncil"));
            //Redes de cooperación
            listaSecciones.Add(new CVSection("060.030.040.000", "network", $"{GetUrlPrefix("roh")}Network", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}networks", $"{GetUrlPrefix("roh")}RelatedNetwork"));
            //Premios, menciones y distinc.
            listaSecciones.Add(new CVSection("060.030.050.000", "accreditation", $"{GetUrlPrefix("roh")}Accreditation", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}prizes", $"{GetUrlPrefix("roh")}RelatedPrize"));
            //Otras distinc. carrera profes./empr.
            listaSecciones.Add(new CVSection("060.030.060.000", "accreditation", $"{GetUrlPrefix("roh")}Accreditation", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}otherDistinctions", $"{GetUrlPrefix("roh")}RelatedOtherDistinction"));
            //Períodos activ. investigadora
            listaSecciones.Add(new CVSection("060.030.070.000", "accreditation", $"{GetUrlPrefix("roh")}Accreditation", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}researchActivityPeriods", $"{GetUrlPrefix("roh")}RelatedResearchActivityPeriod"));
            //Acreditaciones/reconocimientos
            listaSecciones.Add(new CVSection("060.030.090.000", "accreditation", $"{GetUrlPrefix("roh")}Accreditation", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}obtainedRecognitions", $"{GetUrlPrefix("roh")}RelatedObtainedRecognition"));
            //Resumen de otros méritos
            listaSecciones.Add(new CVSection("060.030.100.000", "accreditation", $"{GetUrlPrefix("roh")}Accreditation", $"{GetUrlPrefix("roh")}scientificActivity", $"{GetUrlPrefix("roh")}otherAchievements", $"{GetUrlPrefix("roh")}RelatedOtherAchievement"));


            HashSet<string> filtersModificarElementosCV = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filtersModificarElementosCV.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersModificarElementosCV.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
            }
            if (filtersModificarElementosCV.Count == 0)
            {
                filtersModificarElementosCV.Add("");
            }

            List<string> querySectionsAniadir = new List<string>();
            List<string> querySectionsEliminar = new List<string>();
            foreach (CVSection section in listaSecciones)
            {
                string querySectionAniadir = $@"
                                    {{
                                        {{
                                            #DESEABLES
                                            select distinct ?person ?cv ?idSection 
                                            '{section.rdfTypeAux}' as ?rdfTypeAux 
                                            ?item 
                                            '{section.sectionProperty}' as ?sectionProperty   
                                            '{section.itemProperty}' as ?auxProperty   
                                            ?crisIdentifier
                                            Where
                                            {{
                                                ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                                ?item a <{section.rdfType}>.
                                                ?item <http://w3id.org/roh/cvnCode> ""{section.cvnCode}"".
                                                ?cv a <http://w3id.org/roh/CV>.
                                                ?cv <http://w3id.org/roh/cvOf> ?person.
                                                ?cv <{section.sectionProperty}> ?idSection.
                                                ?item <http://w3id.org/roh/owner> ?person.
                                                OPTIONAL{{?item <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.}}
                                            }}
                                        }}
                                        MINUS
                                        {{
                                            #ACTUALES
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.      
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv <http://w3id.org/roh/cvOf> ?person.
                                            ?cv <{section.sectionProperty}> ?idSection.
                                            ?idSection <{section.itemProperty}> ?auxEntity.
                                            ?auxEntity <http://vivoweb.org/ontology/core#relatedBy> ?item.        
                                        }}
                                    }}";
                querySectionsAniadir.Add(querySectionAniadir);

                string querySectionEliminar = $@"
                                    {{
                                        {{
                                            #ACTUALES                                            
                                            select distinct ?person ?cv ?idSection ?auxEntity '{section.sectionProperty}' as ?sectionProperty  '{section.itemProperty}' as ?auxProperty  ?item
                                            Where
                                            {{
                                                ?person a <http://xmlns.com/foaf/0.1/Person>.         
                                                ?cv a <http://w3id.org/roh/CV>.
                                                ?cv <http://w3id.org/roh/cvOf> ?person.
                                                ?cv <{section.sectionProperty}> ?idSection.
                                                ?idSection <{section.itemProperty}> ?auxEntity.
                                                ?auxEntity <http://vivoweb.org/ontology/core#relatedBy> ?item.  
                                            }}
                                        }}
                                        MINUS
                                        {{
                                            #DESEABLES
                                            ?person a <http://xmlns.com/foaf/0.1/Person>.                                            
                                            ?item a <{section.rdfType}>.
                                            ?item <http://w3id.org/roh/cvnCode> ""{section.cvnCode}"".
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv <http://w3id.org/roh/cvOf> ?person.
                                            ?cv <{section.sectionProperty}> ?idSection.
                                            ?item <http://w3id.org/roh/owner> ?person.
                                        }}
                                    }}";
                querySectionsEliminar.Add(querySectionEliminar);
            }

            foreach (string filter in filtersModificarElementosCV)
            {
                List<string> listaFroms = new List<string>() { "curriculumvitae", "person" };
                listaFroms.AddRange(listaSecciones.Select(x => x.graph));
                while (true)
                {

                    //Añadimos items
                    int limitAniadirItems = 500;
                    String selectAniadirItems = @$"select distinct ?cv ?idSection ?rdfTypeAux ?item ?sectionProperty ?auxProperty ?crisIdentifier ";
                    String whereAniadirItems = @$"where{{
                                    {filter}
                                    {string.Join("UNION", querySectionsAniadir)}
                                }}order by desc(?cv) limit {limitAniadirItems}";

                    SparqlObject resultadoAniadirItems = mResourceApi.VirtuosoQueryMultipleGraph(selectAniadirItems, whereAniadirItems, listaFroms);
                    InsertarItemsCV(resultadoAniadirItems);
                    if (resultadoAniadirItems.results.bindings.Count != limitAniadirItems)
                    {
                        break;
                    }
                }

                while (true)
                {
                    //Eliminamos items
                    int limitEliminarItems = 500;
                    String selectEliminarItems = @$"select distinct ?cv ?idSection ?auxEntity ?sectionProperty ?auxProperty ";
                    String whereEliminarItems = @$"where{{
                                    {filter}
                                    {string.Join("UNION", querySectionsEliminar)}
                                }}order by desc(?cv) limit {limitEliminarItems}";
                    SparqlObject resultadoEliminarItems = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarItems, whereEliminarItems, listaFroms);
                    EliminarItemsCV(resultadoEliminarItems);
                    if (resultadoEliminarItems.results.bindings.Count != limitEliminarItems)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Modifica los nombres de las organizaciones en el CV en función de como se llamen
        /// Depende de ActualizadorCV.CrearCVs
        /// </summary>
        /// <param name="pPersons">IDs de las personas</param>
        /// <param name="pCVs">IDs de los CVs</param>
        public void ModificarOrganizacionesCV(List<string> pPersons = null, List<string> pCVs = null)
        {
            List<OrgTitleCVTitleOrg> listaOrgs = new List<OrgTitleCVTitleOrg>();

            listaOrgs.Add(new OrgTitleCVTitleOrg("academicdegree", $"{GetUrlPrefix("vivo")}AcademicDegree", "", $"{GetUrlPrefix("roh")}conductedByTitle", $"{GetUrlPrefix("roh")}conductedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("academicdegree", $"{GetUrlPrefix("vivo")}AcademicDegree", "", $"{GetUrlPrefix("roh")}deaEntityTitle", $"{GetUrlPrefix("roh")}deaEntity"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("accreditation", $"{GetUrlPrefix("roh")}Accreditation", "", $"{GetUrlPrefix("roh")}accreditationIssuedByTitle", $"{GetUrlPrefix("roh")}accreditationIssuedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("activity", $"{GetUrlPrefix("roh")}Activity", "", $"{GetUrlPrefix("roh")}conductedByTitle", $"{GetUrlPrefix("roh")}conductedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("activity", $"{GetUrlPrefix("roh")}Activity", "", $"{GetUrlPrefix("roh")}promotedByTitle", $"{GetUrlPrefix("roh")}promotedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("activity", $"{GetUrlPrefix("roh")}Activity", "", $"{GetUrlPrefix("roh")}representedEntityTitle", $"{GetUrlPrefix("roh")}representedEntity"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("collaboration", $"{GetUrlPrefix("roh")}Collaboration", $"{GetUrlPrefix("roh")}participates", $"{GetUrlPrefix("roh")}organizationTitle", $"{GetUrlPrefix("roh")}organization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("committee", $"{GetUrlPrefix("roh")}Committee", "", $"{GetUrlPrefix("roh")}affiliatedOrganizationTitle", $"{GetUrlPrefix("vivo")}affiliatedOrganization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("council", $"{GetUrlPrefix("roh")}Council", "", $"{GetUrlPrefix("roh")}affiliatedOrganizationTitle", $"{GetUrlPrefix("vivo")}affiliatedOrganization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("document", $"{GetUrlPrefix("bibo")}Document", "", $"{GetUrlPrefix("roh")}presentedAtOrganizerTitle", $"{GetUrlPrefix("roh")}presentedAtOrganizer"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("grant", $"{GetUrlPrefix("vivo")}Grant", "", $"{GetUrlPrefix("roh")}awardingEntityTitle", $"{GetUrlPrefix("roh")}awardingEntity"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("grant", $"{GetUrlPrefix("vivo")}Grant", "", $"{GetUrlPrefix("roh")}entityTitle", $"{GetUrlPrefix("roh")}entity"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("group", $"{GetUrlPrefix("foaf")}Group", "", $"{GetUrlPrefix("roh")}affiliatedOrganizationTitle", $"{GetUrlPrefix("vivo")}affiliatedOrganization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("impartedacademictraining", $"{GetUrlPrefix("roh")}ImpartedAcademicTraining", "", $"{GetUrlPrefix("roh")}evaluatedByTitle", $"{GetUrlPrefix("roh")}evaluatedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("impartedacademictraining", $"{GetUrlPrefix("roh")}ImpartedAcademicTraining", "", $"{GetUrlPrefix("roh")}financedByTitle", $"{GetUrlPrefix("roh")}financedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("impartedacademictraining", $"{GetUrlPrefix("roh")}ImpartedAcademicTraining", "", $"{GetUrlPrefix("roh")}promotedByTitle", $"{GetUrlPrefix("roh")}promotedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("impartedcoursesseminars", $"{GetUrlPrefix("roh")}ImpartedCoursesSeminars", "", $"{GetUrlPrefix("roh")}promotedByTitle", $"{GetUrlPrefix("roh")}promotedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("network", $"{GetUrlPrefix("roh")}Network", $"{GetUrlPrefix("roh")}participates", $"{GetUrlPrefix("roh")}organizationTitle", $"{GetUrlPrefix("roh")}organization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("network", $"{GetUrlPrefix("roh")}Network", "", $"{GetUrlPrefix("roh")}selectionEntityTitle", $"{GetUrlPrefix("roh")}selectionEntity"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("patent", $"{GetUrlPrefix("bibo")}Patent", $"{GetUrlPrefix("roh")}operatingCompanies", $"{GetUrlPrefix("roh")}organizationTitle", $"{GetUrlPrefix("roh")}organization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("patent", $"{GetUrlPrefix("bibo")}Patent", "", $"{GetUrlPrefix("roh")}ownerOrganizationTitle", $"{GetUrlPrefix("roh")}ownerOrganization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("position", $"{GetUrlPrefix("vivo")}Position", "", $"{GetUrlPrefix("roh")}employerOrganizationTitle", $"{GetUrlPrefix("roh")}employerOrganization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("project", $"{GetUrlPrefix("vivo")}Project", $"{GetUrlPrefix("roh")}grantedBy", $"{GetUrlPrefix("roh")}organizationTitle", $"{GetUrlPrefix("roh")}organization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("project", $"{GetUrlPrefix("vivo")}Project", $"{GetUrlPrefix("roh")}participates", $"{GetUrlPrefix("roh")}organizationTitle", $"{GetUrlPrefix("roh")}organization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("project", $"{GetUrlPrefix("vivo")}Project", "", $"{GetUrlPrefix("roh")}conductedByTitle", $"{GetUrlPrefix("roh")}conductedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("society", $"{GetUrlPrefix("roh")}Society", "", $"{GetUrlPrefix("roh")}affiliatedOrganizationTitle", $"{GetUrlPrefix("vivo")}affiliatedOrganization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("stay", $"{GetUrlPrefix("roh")}Stay", "", $"{GetUrlPrefix("roh")}entityTitle", $"{GetUrlPrefix("roh")}entity"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("stay", $"{GetUrlPrefix("roh")}Stay", "", $"{GetUrlPrefix("roh")}fundedByTitle", $"{GetUrlPrefix("roh")}fundedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("teachingcongress", $"{GetUrlPrefix("roh")}TeachingCongress", "", $"{GetUrlPrefix("roh")}conductedByTitle", $"{GetUrlPrefix("roh")}conductedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("teachingproject", $"{GetUrlPrefix("roh")}TeachingProject", "", $"{GetUrlPrefix("roh")}fundedByTitle", $"{GetUrlPrefix("roh")}fundedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("teachingproject", $"{GetUrlPrefix("roh")}TeachingProject", $"{GetUrlPrefix("roh")}participates", $"{GetUrlPrefix("roh")}organizationTitle", $"{GetUrlPrefix("roh")}organization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("technologicalresult", $"{GetUrlPrefix("roh")}TechnologicalResult", $"{GetUrlPrefix("roh")}participates", $"{GetUrlPrefix("roh")}organizationTitle", $"{GetUrlPrefix("roh")}organization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("technologicalresult", $"{GetUrlPrefix("roh")}TechnologicalResult", $"{GetUrlPrefix("roh")}targetOrganizations", $"{GetUrlPrefix("roh")}organizationTitle", $"{GetUrlPrefix("roh")}organization"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("thesissupervision", $"{GetUrlPrefix("roh")}ThesisSupervision", "", $"{GetUrlPrefix("roh")}promotedByTitle", $"{GetUrlPrefix("roh")}promotedBy"));
            listaOrgs.Add(new OrgTitleCVTitleOrg("tutorship", $"{GetUrlPrefix("roh")}Tutorship", "", $"{GetUrlPrefix("roh")}conductedByTitle", $"{GetUrlPrefix("roh")}conductedBy"));


            HashSet<string> filtersModificarOrganizacionesCV = new HashSet<string>();
            if (pPersons != null && pPersons.Count > 0)
            {
                filtersModificarOrganizacionesCV.Add($" FILTER(?person in (<{string.Join(">,<", pPersons)}>))");
            }
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersModificarOrganizacionesCV.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
            }
            if (filtersModificarOrganizacionesCV.Count == 0)
            {
                filtersModificarOrganizacionesCV.Add("");
            }

            foreach (OrgTitleCVTitleOrg section in listaOrgs)
            {
                foreach (string filter in filtersModificarOrganizacionesCV)
                {
                    while (true)
                    {
                        string[] propsAux = section.propAux.Split("|", StringSplitOptions.RemoveEmptyEntries);
                        string filterPropAux = "";
                        int i = 0;
                        foreach (string propAux in propsAux)
                        {
                            filterPropAux += $"?s{i} <{propAux}> ?s{i + 1}.\n";
                            i++;
                        }

                        int limitModificarOrganizacionesCV = 500;
                        String selectModificarOrganizacionesCV = @$"select *  ";
                        String whereModificarOrganizacionesCV = @$"where{{
                                    ?s0 a <{section.rdfType}>.
                                    {filterPropAux}
                                    OPTIONAL{{?s{i} <{section.propTituloDesnormalizado}> ?orgTituloDesnormalizado.}}
                                    ?s{i} <{section.propOrganizacion}> ?org.
                                    ?org <http://w3id.org/roh/title> ?titleOrg.
                                    FILTER(?orgTituloDesnormalizado!=?titleOrg)
                                }}limit {limitModificarOrganizacionesCV}";
                        SparqlObject resultadoModificarOrganizacionesCV = mResourceApi.VirtuosoQueryMultipleGraph(selectModificarOrganizacionesCV, whereModificarOrganizacionesCV, new List<string>() { section.graph, "organization" });
                        Parallel.ForEach(resultadoModificarOrganizacionesCV.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                        {
                            //Entidad principal
                            string mainEntity = fila["s0"].value;
                            //Predicado
                            string predicado = section.propTituloDesnormalizado;
                            if (!string.IsNullOrEmpty(section.propAux))
                            {
                                predicado = section.propAux + "|" + predicado;
                            }
                            //Valor antiguo
                            string valorAntiguo = "";
                            if (fila.ContainsKey("orgTituloDesnormalizado"))
                            {
                                valorAntiguo = fila["orgTituloDesnormalizado"].value;
                                List<string> auxEntities = new List<string>();
                                foreach (string key in fila.Keys)
                                {
                                    if (key.StartsWith("s") && key != "s0")
                                    {
                                        auxEntities.Add(fila[key].value);
                                    }
                                }
                                if (auxEntities.Count > 0)
                                {
                                    valorAntiguo = string.Join("|", auxEntities) + "|" + valorAntiguo;
                                }
                            }
                            //Valor nuevo
                            string valorNuevo = fila["titleOrg"].value;
                            {
                                List<string> auxEntities = new List<string>();
                                foreach (string key in fila.Keys)
                                {
                                    if (key.StartsWith("s") && key != "s0")
                                    {
                                        auxEntities.Add(fila[key].value);
                                    }
                                }
                                if (auxEntities.Count > 0)
                                {
                                    valorNuevo = string.Join("|", auxEntities) + "|" + valorNuevo;
                                }
                            }
                            ActualizadorTriple(mainEntity, predicado, valorAntiguo, valorNuevo);
                        });
                        if (resultadoModificarOrganizacionesCV.results.bindings.Count != limitModificarOrganizacionesCV)
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Elimina elementos duplicados de los CVs
        /// </summary>
        /// <param name="pCVs">IDs de cvs</param>
        public void EliminarDuplicados(List<string> pCVs = null)
        {
            HashSet<string> filtersEliminarDuplicados = new HashSet<string>();
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersEliminarDuplicados.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
            }
            if (filtersEliminarDuplicados.Count == 0)
            {
                filtersEliminarDuplicados.Add("");
            }

            foreach (string filter in filtersEliminarDuplicados)
            {
                while (true)
                {
                    int limitEliminarDuplicados = 500;
                    String selectEliminarDuplicados = @$"select * where{{ select ?cv ?item count(?o2) as ?numItems  ";
                    String whereEliminarDuplicados = @$"  where{{
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv ?p1 ?o1.
                                            ?o1 ?p2 ?o2.
                                            ?o2 <http://vivoweb.org/ontology/core#relatedBy> ?item.
                                            {filter}
                                        }}
                                    }}group by ?cv ?item HAVING (?numItems > 1)  order by desc(?cv) limit {limitEliminarDuplicados}";
                    SparqlObject resultadoEliminarDuplicados = mResourceApi.VirtuosoQuery(selectEliminarDuplicados, whereEliminarDuplicados, "curriculumvitae");
                    Parallel.ForEach(resultadoEliminarDuplicados.results.bindings, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, fila =>
                    {
                        string cv = fila["cv"].value;
                        string item = fila["item"].value;

                        String selectIn = @$"select * ";
                        String whereIn = @$"  where{{
                                            <{cv}> ?p1 ?o1.
                                            ?o1 ?p2 ?o2.
                                            ?o2 <http://vivoweb.org/ontology/core#relatedBy> <{item}>.
                                        }}limit 10000 offset 1";
                        SparqlObject resultadoIn = mResourceApi.VirtuosoQuery(selectIn, whereIn, "curriculumvitae");
                        foreach (Dictionary<string, SparqlObject.Data> filain in resultadoIn.results.bindings)
                        {
                            string predicado = filain["p1"].value + "|" + filain["p2"].value;
                            string valorAntiguo = filain["o1"].value + "|" + filain["o2"].value;
                            ActualizadorTriple(cv, predicado, valorAntiguo, "");
                        }

                    });
                    if (resultadoEliminarDuplicados.results.bindings.Count != limitEliminarDuplicados)
                    {
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Elimina items eliminados de los CVs
        /// </summary>
        /// <param name="pCVs">IDs de cvs</param>
        public void EliminarItemsEliminados(List<string> pCVs = null)
        {
            HashSet<string> filtersEliminarItemsEliminados = new HashSet<string>();
            if (pCVs != null && pCVs.Count > 0)
            {
                filtersEliminarItemsEliminados.Add($" FILTER(?cv in (<{string.Join(">,<", pCVs)}>))");
            }
            if (filtersEliminarItemsEliminados.Count == 0)
            {
                filtersEliminarItemsEliminados.Add("");
            }

            List<string> graphs = new List<string>();
            graphs.Add("document");
            graphs.Add("project");
            graphs.Add("tutorship");
            graphs.Add("thesissupervision");
            graphs.Add("teachingpublication");
            graphs.Add("teachingproject");
            graphs.Add("stay");
            graphs.Add("society");
            graphs.Add("position");
            graphs.Add("languagecertificate");
            graphs.Add("group");
            graphs.Add("grant");
            graphs.Add("activity");
            graphs.Add("impartedacademictraining");
            graphs.Add("accreditation");
            graphs.Add("academicdegree");
            graphs.Add("impartedcoursesseminars");
            graphs.Add("network");
            graphs.Add("council");
            graphs.Add("collaboration");
            graphs.Add("committee");
            graphs.Add("technologicalresult");
            graphs.Add("teachingcongress");
            graphs.Add("supervisedartisticproject");
            graphs.Add("scientificproduction");
            graphs.Add("patent");
            graphs.Add("researchobject");
            graphs.Add("curriculumvitae");

            foreach (string filter in filtersEliminarItemsEliminados)
            {
                while (true)
                {
                    int limitEliminarItemsEliminados = 500;
                    String selectEliminarItemsEliminados = @$"select ?cv ?p1 ?o1 ?p2 ?o2 ?item ";
                    String whereEliminarItemsEliminados = @$"  where{{
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv ?p1 ?o1.
                                            ?o1 ?p2 ?o2.
                                            ?o2 <http://vivoweb.org/ontology/core#relatedBy> ?item.
                                            MINUS{{?item a ?rdfType}}
                                            {filter}
                                        }}limit {limitEliminarItemsEliminados}";
                    SparqlObject resultadoEliminarItemsEliminados = mResourceApi.VirtuosoQueryMultipleGraph(selectEliminarItemsEliminados, whereEliminarItemsEliminados, graphs);

                    Dictionary<Guid, List<RemoveTriples>> triplesEliminar = new Dictionary<Guid, List<RemoveTriples>>();
                    foreach (var fila in resultadoEliminarItemsEliminados.results.bindings)
                    {
                        Guid cv = mResourceApi.GetShortGuid(fila["cv"].value);
                        string p1 = fila["p1"].value;
                        string o1 = fila["o1"].value;
                        string p2 = fila["p2"].value;
                        string o2 = fila["o2"].value;
                        string predicado = p1 + "|" + p2;
                        string objeto = o1 + "|" + o2;

                        if (!triplesEliminar.ContainsKey(cv))
                        {
                            triplesEliminar[cv] = new List<RemoveTriples>();
                        }
                        RemoveTriples t = new();
                        t.Predicate = predicado;
                        t.Value = objeto;
                        triplesEliminar[cv].Add(t);
                    }

                    Parallel.ForEach(triplesEliminar, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, item =>
                    {
                        mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<RemoveTriples>>() { { item.Key, item.Value } });
                    });
                    if (resultadoEliminarItemsEliminados.results.bindings.Count != limitEliminarItemsEliminados)
                    {
                        break;
                    }
                }
            }
        }

        #endregion

        #region Métodos privados
        /// <summary>
        /// Genera objetos CV de las personas pasadas por parámetro
        /// </summary>
        /// <param name="personsIDs">Identificadores de las personas</param>
        /// <returns></returns>
        private List<CV> GenerateCVFromPersons(List<string> personsIDs)
        {
            Dictionary<string, CV> listaCV = new();
            if (personsIDs.Count > 0)
            {
                var personasIDsStr = string.Join(',', personsIDs.Select(item => "<" + item + ">"));

                //Nombre
                {
                    String selectNombre = @"SELECT DISTINCT ?person ?name ?firstName ?lastName";
                    String whereNombre = @$"where{{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        ?person <http://xmlns.com/foaf/0.1/name> ?name.
                                        OPTIONAL{{?person <http://xmlns.com/foaf/0.1/firstName> ?firstName}}
                                        OPTIONAL{{?person <http://xmlns.com/foaf/0.1/lastName> ?lastName}}
                                        FILTER( ?person IN ( {personasIDsStr} )).
                        }}";
                    SparqlObject resultadoNombre = mResourceApi.VirtuosoQuery(selectNombre, whereNombre, "person");

                    // Personas que no poseen actualmente un CV
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoNombre.results.bindings)
                    {
                        string person = fila["person"].value;
                        string name = fila["name"].value;
                        string firstName = "";
                        string lastName = "";
                        if (fila.ContainsKey("firstName"))
                        {
                            firstName = fila["firstName"].value;
                        }
                        if (fila.ContainsKey("lastName"))
                        {
                            lastName = fila["lastName"].value;
                        }
                        string[] nameSplit = name.Split(' ');
                        if (string.IsNullOrEmpty(firstName))
                        {
                            firstName = nameSplit[0];
                        }
                        if (string.IsNullOrEmpty(lastName))
                        {
                            if (nameSplit.Count() > 1)
                            {
                                lastName = nameSplit[1];
                            }
                            else
                            {
                                lastName = nameSplit[0];
                            }

                        }
                        CV cv = new();
                        if (listaCV.ContainsKey(person))
                        {
                            cv = listaCV[person];
                        }
                        else
                        {
                            listaCV.Add(person, cv);
                        }
                        cv.Foaf_name = name;
                        cv.IdRoh_cvOf = person;
                        cv.Roh_professionalSituation = new ProfessionalSituation() { Roh_title = "-" };
                        cv.Roh_qualifications = new Qualifications() { Roh_title = "-" };
                        cv.Roh_teachingExperience = new TeachingExperience() { Roh_title = "-" };
                        cv.Roh_scientificExperience = new ScientificExperience() { Roh_title = "-" };
                        cv.Roh_scientificActivity = new ScientificActivity() { Roh_title = "-" };
                        cv.Roh_researchObject = new ResearchObjects() { Roh_title = "-" };
                        cv.Roh_freeTextSummary = new FreeTextSummary() { Roh_title = "-" };
                        cv.Roh_personalData = new PersonalData() { Foaf_firstName = firstName, Foaf_familyName = lastName };
                    }
                }

                //Email
                {
                    String selectEmail = @"SELECT DISTINCT ?person ?email";
                    String whereEmail = @$"where{{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        ?person <https://www.w3.org/2006/vcard/ns#email> ?email.
                                        FILTER( ?person IN ( {personasIDsStr} )).
                        }}";
                    SparqlObject resultadoEmail = mResourceApi.VirtuosoQuery(selectEmail, whereEmail, "person");

                    // Personas que no poseen actualmente un CV
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoEmail.results.bindings)
                    {
                        string person = fila["person"].value;
                        string email = fila["email"].value;
                        if (listaCV.ContainsKey(person))
                        {
                            CV cv = listaCV[person];
                            cv.Roh_personalData.Vcard_email = email;
                        }
                    }
                }

                //Teléfono
                {
                    String selectTelefono = @"SELECT DISTINCT ?person ?telephone";
                    String whereTelefono = @$"where{{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        ?person <https://www.w3.org/2006/vcard/ns#hasTelephone> ?telephone.
                                        FILTER( ?person IN ( {personasIDsStr} )).
                        }}";
                    SparqlObject resultadoTelefono = mResourceApi.VirtuosoQuery(selectTelefono, whereTelefono, "person");

                    // Personas que no poseen actualmente un CV
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoTelefono.results.bindings)
                    {
                        string person = fila["person"].value;
                        string telephone = fila["telephone"].value;
                        if (listaCV.ContainsKey(person))
                        {
                            CV cv = listaCV[person];
                            cv.Roh_personalData.Vcard_hasTelephone = new TelephoneType() { Vcard_hasValue = telephone };
                        }
                    }
                }

                //Página
                {
                    String selectPagina = @"SELECT DISTINCT ?person ?homepage";
                    String wherePagina = @$"where{{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        ?person <http://xmlns.com/foaf/0.1/homepage> ?homepage.
                                        FILTER( ?person IN ( {personasIDsStr} )).
                        }}";
                    SparqlObject resultadoPagina = mResourceApi.VirtuosoQuery(selectPagina, wherePagina, "person");

                    // Personas que no poseen actualmente un CV
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoPagina.results.bindings)
                    {
                        string person = fila["person"].value;
                        string homepage = fila["homepage"].value;
                        if (listaCV.ContainsKey(person))
                        {
                            CV cv = listaCV[person];
                            cv.Roh_personalData.Foaf_homepage = homepage;
                        }
                    }
                }

                //ORCID
                //SCOPUS
                //ResearcherId
                {
                    String selectIDs = @"SELECT DISTINCT ?person ?orcid ?scopusId ?researcherId";
                    String whereIDs = @$"where{{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        OPTIONAL{{?person <http://w3id.org/roh/ORCID> ?orcid.}}
                                        OPTIONAL{{?person <http://vivoweb.org/ontology/core#scopusId> ?scopusId.}}
                                        OPTIONAL{{?person <http://vivoweb.org/ontology/core#researcherId> ?researcherId.}}
                                        FILTER( ?person IN ( {personasIDsStr} )).
                        }}";
                    SparqlObject resultadoIDs = mResourceApi.VirtuosoQuery(selectIDs, whereIDs, "person");

                    // Personas que no poseen actualmente un CV
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoIDs.results.bindings)
                    {
                        string person = fila["person"].value;
                        string orcid = "";
                        string scopusId = "";
                        string researcherId = "";
                        if (fila.ContainsKey("orcid"))
                        {
                            orcid = fila["orcid"].value;
                        }
                        if (fila.ContainsKey("scopusId"))
                        {
                            scopusId = fila["scopusId"].value;
                        }
                        if (fila.ContainsKey("researcherId"))
                        {
                            researcherId = fila["researcherId"].value;
                        }
                        if (listaCV.ContainsKey(person))
                        {
                            CV cv = listaCV[person];
                            cv.Roh_personalData.Roh_ORCID = orcid;
                            cv.Roh_personalData.Vivo_scopusId = scopusId;
                            cv.Roh_personalData.Vivo_researcherId = researcherId;
                        }
                    }
                }

                //Otros IDs
                {
                    String selectIDsOther = @"SELECT DISTINCT ?person ?semanticScholarId";
                    String whereIDsOther = @$"where{{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        ?person <http://w3id.org/roh/semanticScholarId> ?semanticScholarId.
                                        FILTER( ?person IN ( {personasIDsStr} )).
                        }}";
                    SparqlObject resultadoIDsOther = mResourceApi.VirtuosoQuery(selectIDsOther, whereIDsOther, "person");

                    // Personas que no poseen actualmente un CV
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoIDsOther.results.bindings)
                    {
                        string person = fila["person"].value;
                        string semanticScholarId = fila["semanticScholarId"].value;
                        if (listaCV.ContainsKey(person))
                        {
                            CV cv = listaCV[person];
                            if (cv.Roh_personalData.Roh_otherIds == null)
                            {
                                cv.Roh_personalData.Roh_otherIds = new List<Document>();
                            }
                            cv.Roh_personalData.Roh_otherIds.Add(new Document() { Foaf_topic = "SemanticScholar", Dc_title = semanticScholarId });
                        }
                    }
                }

                //Direccion
                {
                    String selectDireccion = @"SELECT DISTINCT ?person ?address";
                    String whereDireccion = @$"where{{
                                        ?person a <http://xmlns.com/foaf/0.1/Person>.
                                        ?person <https://www.w3.org/2006/vcard/ns#address> ?address.
                                        FILTER( ?person IN ( {personasIDsStr} )).
                        }}";
                    SparqlObject resultadoDireccion = mResourceApi.VirtuosoQuery(selectDireccion, whereDireccion, "person");

                    // Personas que no poseen actualmente un CV
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoDireccion.results.bindings)
                    {
                        string person = fila["person"].value;
                        string address = fila["address"].value;
                        if (listaCV.ContainsKey(person))
                        {
                            CV cv = listaCV[person];
                            if (cv.Roh_personalData.Vcard_address == null)
                            {
                                cv.Roh_personalData.Vcard_address = new Address();
                            }
                            cv.Roh_personalData.Vcard_address.Vcard_locality = address;
                        }
                    }
                }
            }
            return listaCV.Values.ToList();
        }

        /// <summary>
        /// Inserta documentos en un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void InsertarDocumentosCV(SparqlObject pDatosCargar)
        {
            Dictionary<Guid, List<TriplesToInclude>> triplesToIncludeDocuments = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string scientificActivity = fila["scientificActivity"].value;
                string document = fila["document"].value;
                string typeDocument = fila["typeDocument"].value;
                string isValidated = "false";
                if (fila.ContainsKey("isValidated"))
                {
                    isValidated = fila["isValidated"].value;
                }

                string rdftype = "";
                string property = "";
                switch (typeDocument)
                {
                    case "SAD1":
                        rdftype = $"{GetUrlPrefix("roh")}RelatedScientificPublication";
                        property = $"{GetUrlPrefix("roh")}scientificPublications";
                        break;
                    case "SAD2":
                        rdftype = $"{GetUrlPrefix("roh")}RelatedWorkSubmittedConferences";
                        property = $"{GetUrlPrefix("roh")}worksSubmittedConferences";
                        break;
                    case "SAD3":
                        rdftype = $"{GetUrlPrefix("roh")}RelatedWorkSubmittedSeminars";
                        property = $"{GetUrlPrefix("roh")}worksSubmittedSeminars";
                        break;
                }

                //Obtenemos la auxiliar en la que cargar la entidad  
                string rdfTypePrefix = AniadirPrefijo(rdftype);
                rdfTypePrefix = rdfTypePrefix.Substring(rdfTypePrefix.IndexOf(":") + 1);
                string idNewAux = mResourceApi.GraphsUrl + "items/" + rdfTypePrefix + "_" + mResourceApi.GetShortGuid(cv) + "_" + Guid.NewGuid();
                List<TriplesToInclude> listaTriples = new();
                string idEntityAux = scientificActivity + "|" + idNewAux;

                //Privacidad            
                string predicadoPrivacidad = $"{GetUrlPrefix("roh")}scientificActivity|" + property + "|http://w3id.org/roh/isPublic";
                TriplesToInclude tr2 = new(idEntityAux + "|" + isValidated, predicadoPrivacidad);
                listaTriples.Add(tr2);

                //Entidad
                string predicadoEntidad = $"{GetUrlPrefix("roh")}scientificActivity|" + property + "|http://vivoweb.org/ontology/core#relatedBy";
                TriplesToInclude tr1 = new(idEntityAux + "|" + document, predicadoEntidad);
                listaTriples.Add(tr1);

                Guid idCVDocument = mResourceApi.GetShortGuid(cv);
                if (triplesToIncludeDocuments.ContainsKey(idCVDocument))
                {
                    triplesToIncludeDocuments[idCVDocument].AddRange(listaTriples);
                }
                else
                {
                    triplesToIncludeDocuments.Add(mResourceApi.GetShortGuid(cv), listaTriples);
                }
            }

            Parallel.ForEach(triplesToIncludeDocuments.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                List<List<TriplesToInclude>> listasDeListas = SplitList(triplesToIncludeDocuments[idCV], 50).ToList();
                foreach (List<TriplesToInclude> triples in listasDeListas)
                {
                    mResourceApi.InsertPropertiesLoadedResources(new() { { idCV, triples } });
                }
            });
        }

        /// <summary>
        /// Cambia a públicos documentos de un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void PublicarDocumentosCV(SparqlObject pDatosCargar)
        {
            Dictionary<Guid, List<TriplesToModify>> triplesToModifyDocuments = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string scientificActivity = fila["scientificActivity"].value;
                string propItem = fila["propItem"].value;
                string item = fila["item"].value;


                TriplesToModify triple = new()
                {
                    OldValue = scientificActivity + "|" + item + "|false",
                    NewValue = scientificActivity + "|" + item + "|true",
                    Predicate = $"{GetUrlPrefix("roh")}scientificActivity|" + propItem + "|http://w3id.org/roh/isPublic"
                };

                Guid idCVDocumentsModify = mResourceApi.GetShortGuid(cv);
                if (triplesToModifyDocuments.ContainsKey(idCVDocumentsModify))
                {
                    triplesToModifyDocuments[idCVDocumentsModify].Add(triple);
                }
                else
                {
                    triplesToModifyDocuments.Add(mResourceApi.GetShortGuid(cv), new List<TriplesToModify>() { triple });
                }
            }

            Parallel.ForEach(triplesToModifyDocuments.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                List<List<TriplesToModify>> listasDeListas = SplitList(triplesToModifyDocuments[idCV], 50).ToList();
                foreach (List<TriplesToModify> triples in listasDeListas)
                {
                    mResourceApi.ModifyPropertiesLoadedResources(new() { { idCV, triples } });
                }
            });
        }

        /// <summary>
        /// Elimina documentos en un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void EliminarDocumentosCV(SparqlObject pDatosCargar)
        {
            Dictionary<Guid, List<RemoveTriples>> triplesToDeleteDocument = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string scientificActivity = fila["scientificActivity"].value;
                string item = fila["item"].value;
                string typeDocument = fila["typeDocument"].value;

                string property = "";
                switch (typeDocument)
                {
                    case "SAD1":
                        property = $"{GetUrlPrefix("roh")}scientificPublications";
                        break;
                    case "SAD2":
                        property = $"{GetUrlPrefix("roh")}worksSubmittedConferences";
                        break;
                    case "SAD3":
                        property = $"{GetUrlPrefix("roh")}worksSubmittedSeminars";
                        break;
                }

                RemoveTriples removeTriple = new();
                removeTriple.Predicate = $"{GetUrlPrefix("roh")}scientificActivity|" + property;
                removeTriple.Value = scientificActivity + "|" + item;
                Guid idCVDocumentsDelete = mResourceApi.GetShortGuid(cv);
                if (triplesToDeleteDocument.ContainsKey(idCVDocumentsDelete))
                {
                    triplesToDeleteDocument[idCVDocumentsDelete].Add(removeTriple);
                }
                else
                {
                    triplesToDeleteDocument.Add(idCVDocumentsDelete, new() { removeTriple });
                }
            }

            Parallel.ForEach(triplesToDeleteDocument.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                List<List<RemoveTriples>> listasDeListas = SplitList(triplesToDeleteDocument[idCV], 50).ToList();
                foreach (List<RemoveTriples> triples in listasDeListas)
                {
                    mResourceApi.DeletePropertiesLoadedResources(new() { { idCV, triples } });
                }
            });
        }

        /// <summary>
        /// Inserta ResearchObjects en un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void InsertarResearchObjectsCV(SparqlObject pDatosCargar)
        {
            Dictionary<Guid, List<TriplesToInclude>> triplesToIncludeRO = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string researchObject = fila["researchObject"].value;
                string ro = fila["ro"].value;

                string rdftype = $"{GetUrlPrefix("roh")}RelatedResearchObject";
                string property = $"{GetUrlPrefix("roh")}researchObjects";

                //Obtenemos la auxiliar en la que cargar la entidad  
                string rdfTypePrefix = AniadirPrefijo(rdftype);
                rdfTypePrefix = rdfTypePrefix.Substring(rdfTypePrefix.IndexOf(":") + 1);
                string idNewAux = mResourceApi.GraphsUrl + "items/" + rdfTypePrefix + "_" + mResourceApi.GetShortGuid(cv) + "_" + Guid.NewGuid();
                List<TriplesToInclude> listaTriples = new();
                string idEntityAux = researchObject + "|" + idNewAux;

                //Privacidad            
                string predicadoPrivacidad = $"{GetUrlPrefix("roh")}researchObject|{property}|{GetUrlPrefix("roh")}isPublic";
                TriplesToInclude tr2 = new(idEntityAux + "|true", predicadoPrivacidad);
                listaTriples.Add(tr2);

                //Entidad
                string predicadoEntidad = $"{GetUrlPrefix("roh")}researchObject|{property}|{GetUrlPrefix("vivo")}relatedBy";
                TriplesToInclude tr1 = new(idEntityAux + "|" + ro, predicadoEntidad);
                listaTriples.Add(tr1);

                Guid idCVIncludeRO = mResourceApi.GetShortGuid(cv);
                if (triplesToIncludeRO.ContainsKey(idCVIncludeRO))
                {
                    triplesToIncludeRO[idCVIncludeRO].AddRange(listaTriples);
                }
                else
                {
                    triplesToIncludeRO.Add(mResourceApi.GetShortGuid(cv), listaTriples);
                }
            }

            Parallel.ForEach(triplesToIncludeRO.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                List<List<TriplesToInclude>> listasDeListas = SplitList(triplesToIncludeRO[idCV], 50).ToList();
                foreach (List<TriplesToInclude> triples in listasDeListas)
                {
                    mResourceApi.InsertPropertiesLoadedResources(new() { { idCV, triples } });
                }
            });
        }

        /// <summary>
        /// Cambia a públicos ResearchObjects de un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void PublicarResearchObjectsCV(SparqlObject pDatosCargar)
        {
            Dictionary<Guid, List<TriplesToModify>> triplesToModifyRO = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string researchObject = fila["researchObject"].value;
                string propItem = fila["propItem"].value;
                string item = fila["item"].value;


                TriplesToModify triple = new()
                {
                    OldValue = researchObject + "|" + item + "|false",
                    NewValue = researchObject + "|" + item + "|true",
                    Predicate = $"{GetUrlPrefix("roh")}researchObject|" + propItem + "|http://w3id.org/roh/isPublic"
                };

                Guid idCVModifyRO = mResourceApi.GetShortGuid(cv);
                if (triplesToModifyRO.ContainsKey(idCVModifyRO))
                {
                    triplesToModifyRO[idCVModifyRO].Add(triple);
                }
                else
                {
                    triplesToModifyRO.Add(mResourceApi.GetShortGuid(cv), new List<TriplesToModify>() { triple });
                }
            }

            Parallel.ForEach(triplesToModifyRO.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                List<List<TriplesToModify>> listasDeListas = SplitList(triplesToModifyRO[idCV], 50).ToList();
                foreach (List<TriplesToModify> triples in listasDeListas)
                {
                    mResourceApi.ModifyPropertiesLoadedResources(new() { { idCV, triples } });
                }
            });
        }

        /// <summary>
        /// Elimina ResearchObjects en un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void EliminarResearchObjectsCV(SparqlObject pDatosCargar)
        {
            Dictionary<Guid, List<RemoveTriples>> triplesToDeleteRO = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string researchObject = fila["researchObject"].value;
                string item = fila["item"].value;

                string property = $"{GetUrlPrefix("roh")}researchObjects";
                RemoveTriples removeTriple = new();
                removeTriple.Predicate = $"{GetUrlPrefix("roh")}researchObject|{property}";
                removeTriple.Value = researchObject + "|" + item;
                Guid idCVDeleteRO = mResourceApi.GetShortGuid(cv);
                if (triplesToDeleteRO.ContainsKey(idCVDeleteRO))
                {
                    triplesToDeleteRO[idCVDeleteRO].Add(removeTriple);
                }
                else
                {
                    triplesToDeleteRO.Add(idCVDeleteRO, new() { removeTriple });
                }
            }

            Parallel.ForEach(triplesToDeleteRO.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                List<List<RemoveTriples>> listasDeListas = SplitList(triplesToDeleteRO[idCV], 50).ToList();
                foreach (List<RemoveTriples> triples in listasDeListas)
                {
                    mResourceApi.DeletePropertiesLoadedResources(new() { { idCV, triples } });
                }
            });
        }

        /// <summary>
        /// Inserta proyectos en un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void InsertarProyectosCV(SparqlObject pDatosCargar)
        {
            //http://gnoss.com/items/scientificexperienceproject_SEP1
            Dictionary<Guid, List<TriplesToInclude>> triplesToIncludeProjects = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string scientificExperience = fila["scientificExperience"].value;
                string project = fila["project"].value;
                string typeProject = fila["typeProject"].value;

                string rdftype = "";
                string property = "";
                switch (typeProject)
                {
                    case "SEP1":
                        rdftype = $"{GetUrlPrefix("roh")}RelatedCompetitiveProject";
                        property = $"{GetUrlPrefix("roh")}competitiveProjects";
                        break;
                    case "SEP2":
                        rdftype = $"{GetUrlPrefix("roh")}RelatedNonCompetitiveProject";
                        property = $"{GetUrlPrefix("roh")}nonCompetitiveProjects";
                        break;
                }

                //Obtenemos la auxiliar en la que cargar la entidad     
                string rdfTypePrefix = AniadirPrefijo(rdftype);
                rdfTypePrefix = rdfTypePrefix.Substring(rdfTypePrefix.IndexOf(":") + 1);
                string idNewAux = mResourceApi.GraphsUrl + "items/" + rdfTypePrefix + "_" + mResourceApi.GetShortGuid(cv) + "_" + Guid.NewGuid();
                List<TriplesToInclude> listaTriples = new();
                string idEntityAux = scientificExperience + "|" + idNewAux;

                //Privacidad, true (son proyectos oficiales)
                string predicadoPrivacidad = $"{GetUrlPrefix("roh")}scientificExperience|{property}|{GetUrlPrefix("roh")}isPublic";
                TriplesToInclude tr2 = new(idEntityAux + "|true", predicadoPrivacidad);
                listaTriples.Add(tr2);

                //Entidad
                string predicadoEntidad = $"{GetUrlPrefix("roh")}scientificExperience|{property}|{GetUrlPrefix("vivo")}relatedBy";
                TriplesToInclude tr1 = new(idEntityAux + "|" + project, predicadoEntidad);
                listaTriples.Add(tr1);

                Guid idCVIncludeProjects = mResourceApi.GetShortGuid(cv);
                if (triplesToIncludeProjects.ContainsKey(idCVIncludeProjects))
                {
                    triplesToIncludeProjects[idCVIncludeProjects].AddRange(listaTriples);
                }
                else
                {
                    triplesToIncludeProjects.Add(mResourceApi.GetShortGuid(cv), listaTriples);
                }
            }

            Parallel.ForEach(triplesToIncludeProjects.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                mResourceApi.InsertPropertiesLoadedResources(new Dictionary<Guid, List<TriplesToInclude>>() { { idCV, triplesToIncludeProjects[idCV] } });
            });
        }

        /// <summary>
        /// Elimina proyectos en un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void EliminarProyectosCV(SparqlObject pDatosCargar)
        {
            Dictionary<Guid, List<RemoveTriples>> triplesToDeleteProject = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string scientificExperience = fila["scientificExperience"].value;
                string item = fila["item"].value;
                string typeProject = fila["typeProject"].value;

                string property = "";
                switch (typeProject)
                {
                    case "SEP1":
                        property = $"{GetUrlPrefix("roh")}competitiveProjects";
                        break;
                    case "SEP2":
                        property = $"{GetUrlPrefix("roh")}nonCompetitiveProjects";
                        break;
                }

                RemoveTriples removeTriple = new();
                removeTriple.Predicate = $"{GetUrlPrefix("roh")}scientificExperience|" + property;
                removeTriple.Value = scientificExperience + "|" + item;
                Guid idCVDeleteProject = mResourceApi.GetShortGuid(cv);
                if (triplesToDeleteProject.ContainsKey(idCVDeleteProject))
                {
                    triplesToDeleteProject[idCVDeleteProject].Add(removeTriple);
                }
                else
                {
                    triplesToDeleteProject.Add(idCVDeleteProject, new List<RemoveTriples>() { removeTriple });
                }
            }

            Parallel.ForEach(triplesToDeleteProject.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<RemoveTriples>>() { { idCV, triplesToDeleteProject[idCV] } });
            });
        }

        /// <summary>
        /// Inserta items en un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void InsertarItemsCV(SparqlObject pDatosCargar)
        {
            //cv-->Identificador del CV
            //idSection-->Identificador de la sección del CV
            //rdfTypeAux-->RdfType de la auxiliar
            //item-->Entidad a añadir
            //sectionProperty-->Propiedad que apunta a la sección
            //auxProperty-->Propiedad que apunta de la sección a la auxiliar
            //crisIdentifier-->Identificador (opcional) si tiene valor es público

            Dictionary<Guid, List<TriplesToInclude>> triplesToInclude = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string section = fila["idSection"].value;
                string entity = fila["item"].value;

                //Obtenemos la auxiliar en la que cargar la entidad     
                string rdfTypePrefix = AniadirPrefijo(fila["rdfTypeAux"].value);
                rdfTypePrefix = rdfTypePrefix.Substring(rdfTypePrefix.IndexOf(":") + 1);
                string idNewAux = mResourceApi.GraphsUrl + "items/" + rdfTypePrefix + "_" + mResourceApi.GetShortGuid(cv) + "_" + Guid.NewGuid();
                List<TriplesToInclude> listaTriples = new();
                string idEntityAux = section + "|" + idNewAux;

                //Privacidad                  
                string predicadoPrivacidad = fila["sectionProperty"].value + "|" + fila["auxProperty"].value + "|http://w3id.org/roh/isPublic";
                if (fila.ContainsKey("crisIdentifier") && !string.IsNullOrEmpty(fila["crisIdentifier"].value))
                {
                    TriplesToInclude tr2 = new(idEntityAux + "|true", predicadoPrivacidad);
                    listaTriples.Add(tr2);
                }
                else
                {
                    TriplesToInclude tr2 = new(idEntityAux + "|false", predicadoPrivacidad);
                    listaTriples.Add(tr2);
                }

                //Entidad
                string predicadoEntidad = fila["sectionProperty"].value + "|" + fila["auxProperty"].value + "|http://vivoweb.org/ontology/core#relatedBy";
                TriplesToInclude tr1 = new(idEntityAux + "|" + entity, predicadoEntidad);
                listaTriples.Add(tr1);

                Guid idCV = mResourceApi.GetShortGuid(cv);
                if (triplesToInclude.ContainsKey(idCV))
                {
                    triplesToInclude[idCV].AddRange(listaTriples);
                }
                else
                {
                    triplesToInclude.Add(mResourceApi.GetShortGuid(cv), listaTriples);
                }
            }

            Parallel.ForEach(triplesToInclude.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                mResourceApi.InsertPropertiesLoadedResources(new Dictionary<Guid, List<TriplesToInclude>>() { { idCV, triplesToInclude[idCV] } });
            });
        }

        /// <summary>
        /// Elimina items de un CV
        /// </summary>
        /// <param name="pDatosCargar">Datos</param>
        private void EliminarItemsCV(SparqlObject pDatosCargar)
        {
            //cv-->Identificador del CV
            //idSection-->Identificador de la sección del CV
            //auxEntity-->Entidad auxiliar a eliminar
            //sectionProperty-->Propiedad que apunta a la sección
            //auxProperty-->Propiedad que apunta de la sección a la auxiliar

            Dictionary<Guid, List<RemoveTriples>> triplesToDelete = new();
            foreach (Dictionary<string, SparqlObject.Data> fila in pDatosCargar.results.bindings)
            {
                string cv = fila["cv"].value;
                string section = fila["idSection"].value;
                string item = fila["auxEntity"].value;

                RemoveTriples removeTriple = new();
                removeTriple.Predicate = fila["sectionProperty"].value + "|" + fila["auxProperty"].value;
                removeTriple.Value = section + "|" + item;
                Guid idCV = mResourceApi.GetShortGuid(cv);
                if (triplesToDelete.ContainsKey(idCV))
                {
                    triplesToDelete[idCV].Add(removeTriple);
                }
                else
                {
                    triplesToDelete.Add(idCV, new List<RemoveTriples>() { removeTriple });
                }
            }

            Parallel.ForEach(triplesToDelete.Keys, new ParallelOptions { MaxDegreeOfParallelism = ActualizadorBase.numParallel }, idCV =>
            {
                mResourceApi.DeletePropertiesLoadedResources(new Dictionary<Guid, List<RemoveTriples>>() { { idCV, triplesToDelete[idCV] } });
            });
        }

        #endregion
    }
}
