using Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores;
using Hercules.CommonsEDMA.Desnormalizador.Models.Services;
using Hercules.CommonsEDMA.Desnormalizador.Models.Similarity;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hercules.CommonsEDMA.Desnormalizador.Models
{
    public static class ActualizadorEDMA
    {
        private readonly static string rutaOauth = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config";
        private static ResourceApi mResourceApi = null;
        private static CommunityApi mCommunityApi = null;
        private static Guid? mCommunityID = null;


        private static ResourceApi resourceApi
        {
            get
            {
                while (mResourceApi == null)
                {
                    try
                    {
                        mResourceApi = new ResourceApi(rutaOauth);
                    }
                    catch (Exception) { }
                }
                return mResourceApi;
            }
        }

        private static CommunityApi communityApi
        {
            get
            {
                while (mCommunityApi == null)
                {
                    try
                    {
                        mCommunityApi = new CommunityApi(rutaOauth);
                    }
                    catch (Exception) { }
                }
                return mCommunityApi;
            }
        }

        private static Guid communityID
        {
            get
            {
                while (!mCommunityID.HasValue)
                {
                    try
                    {
                        mCommunityID = communityApi.GetCommunityId();
                    }
                    catch (Exception) { }
                }
                return mCommunityID.Value;
            }
        }

        /// <summary>
        /// Actualiza todos los elementos desnormalizados
        /// </summary>
        public static void DesnormalizarTodo(ConfigService pConfigService)
        {
            ActualizadorCV actualizadorCV = new(resourceApi);
            ActualizadorPerson actualizadorPersonas = new(resourceApi);
            ActualizadorGroup actualizadorGrupos = new(resourceApi);
            ActualizadorDocument actualizadorDocument = new(resourceApi);
            ActualizadorProject actualizadorProject = new(resourceApi);
            ActualizadorRO actualizadorRO = new(resourceApi);
            ActualizadorPatent actualizadorPatent = new(resourceApi);
            ActualizadorNotification actualizadorNotification = new(resourceApi);

            //Ejecuciones ordenadas en función de sus dependencias

            //Sin dependencias
            //CV sin dependencias 
            actualizadorCV.CrearCVs();

            //Persona sin dependencias                
            actualizadorPersonas.ActualizarPertenenciaLineas();
            actualizadorPersonas.ActualizarNumeroIPProyectos();
            actualizadorPersonas.ActualizarIPGruposActuales();
            actualizadorPersonas.ActualizarIPGruposHistoricos();
            actualizadorPersonas.ActualizarIPProyectosActuales();
            actualizadorPersonas.ActualizarIPProyectosHistoricos();

            //Grupo sin dependencias                
            actualizadorGrupos.ActualizarMiembros();
            actualizadorGrupos.ActualizarGruposValidados();
            actualizadorGrupos.ActualizarPertenenciaLineas();

            //Proyectos sin dependencias
            actualizadorProject.ActualizarProyectosValidados();
            actualizadorProject.ActualizarMiembros();
            actualizadorProject.ActualizarPertenenciaGrupos();
            actualizadorProject.ActualizarAniosInicio();
            actualizadorProject.ActualizarAniosFin();

            //Documentos sin dependencias
            actualizadorDocument.ActualizarDocumentosValidados();
            actualizadorDocument.ActualizarPertenenciaGrupos();
            actualizadorDocument.ActualizarNumeroCitasMaximas();
            actualizadorDocument.ActualizarAreasDocumentos();
            actualizadorDocument.ActualizarTagsDocumentos();
            actualizadorDocument.ActualizarAnios();
            actualizadorDocument.ActualizarIndicesImpacto();
            actualizadorDocument.ActualizarGenderAutorPrincipal();
            actualizadorDocument.ActualizarPositionAutorPrincipal();
            actualizadorDocument.EliminarDocumentosSinAutoresSGI();
            actualizadorDocument.ModificarNombreRevistaDesnormalizado();
            actualizadorDocument.ModificarEditorialRevistaDesnormalizado();
            actualizadorDocument.ModificarISSNRevistaDesnormalizado();
            actualizadorDocument.ActualizarNumeroVinculados();

            //ROs sin dependencias
            actualizadorRO.ActualizarAreasRO();
            actualizadorRO.ActualizarTagsRO();
            actualizadorRO.EliminarROsSinAutoresActivos();
            actualizadorRO.ActualizarNumeroVinculados();

            //Patentes sin dependencias
            actualizadorPatent.ActualizarPatentesValidadas();
            actualizadorPatent.ActualizarMiembros();


            //CV con dependencias 
            actualizadorCV.ModificarDocumentos();
            //actualizadorCV.CambiarPrivacidadDocumentos();
            actualizadorCV.ModificarResearchObjects();
            actualizadorCV.CambiarPrivacidadResearchObjects();
            actualizadorCV.ModificarProyectos();
            actualizadorCV.ModificarGrupos();
            actualizadorCV.ModificarPatentes();
            actualizadorCV.ModificarElementosCV();
            actualizadorCV.ModificarOrganizacionesCV();
            actualizadorCV.EliminarDuplicados();
            actualizadorCV.EliminarItemsEliminados();

            //Proyectos con dependencias
            actualizadorProject.ActualizarNumeroAreasTematicas();
            actualizadorProject.ActualizarMiembrosUnificados();
            actualizadorProject.ActualizarNumeroMiembros();
            actualizadorProject.ActualizarNumeroColaboradores();
            actualizadorProject.ActualizarNumeroPublicaciones();

            //Persona con dependencias  
            actualizadorPersonas.ActualizarNumeroPublicacionesValidadas();
            actualizadorPersonas.ActualizarAreasPersonas();
            actualizadorPersonas.ActualizarNumeroColaboradoresPublicos();
            actualizadorPersonas.ActualizarNumeroPublicacionesPublicas();
            actualizadorPersonas.ActualizarNumeroProyectosValidados();
            actualizadorPersonas.ActualizarNumeroProyectosPublicos();
            actualizadorPersonas.ActualizarNumeroResearchObjectsPublicos();
            actualizadorPersonas.ActualizarNumeroAreasTematicas();
            actualizadorPersonas.ActualizarHIndex();

            //Grupo con dependencias
            actualizadorGrupos.ActualizarMiembrosUnificados();
            actualizadorGrupos.ActualizarNumeroMiembros();
            actualizadorGrupos.ActualizarNumeroPublicaciones();
            actualizadorGrupos.ActualizarNumeroColaboradores();
            actualizadorGrupos.ActualizarNumeroAreasTematicas();
            actualizadorGrupos.ActualizarAreasGrupos();
            actualizadorGrupos.ActualizarNumeroProyectos();

            //Notificaciones
            actualizadorNotification.ActualizarNotificaciones();

            if (!string.IsNullOrEmpty(pConfigService.GetUrlSimilarity()))
            {
                UtilsSimilarity utilsSimilarityDocument = new UtilsSimilarity(pConfigService.GetUrlSimilarity(), resourceApi, "research_paper");
                utilsSimilarityDocument.SincroComplete();

                UtilsSimilarity utilsSimilarityRos = new UtilsSimilarity(pConfigService.GetUrlSimilarity(), resourceApi, "code_project");
                utilsSimilarityRos.SincroComplete();
            }

            //TODO eliminar personas externas sin publicaciones
        }

        /// <summary>
        /// Inserta/elimina de los CV todos los datos
        /// </summary>
        public static void DesnormalizarDatosCV()
        {
            ActualizadorCV actualizadorCV = new(resourceApi);
            actualizadorCV.CrearCVs();
            actualizadorCV.ModificarDocumentos();
            //actualizadorCV.CambiarPrivacidadDocumentos();
            actualizadorCV.ModificarResearchObjects();
            actualizadorCV.CambiarPrivacidadResearchObjects();
            actualizadorCV.ModificarProyectos();
            actualizadorCV.ModificarGrupos();
            actualizadorCV.ModificarPatentes();
            actualizadorCV.ModificarElementosCV();
            actualizadorCV.ModificarOrganizacionesCV();
            actualizadorCV.EliminarDuplicados();
            actualizadorCV.EliminarItemsEliminados();
        }


        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican personas
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        public static void DesnormalizarDatosPersonas(List<string> pPersons = null)
        {
            ActualizadorPerson actualizadorPersonas = new(resourceApi);
            ActualizadorDocument actualizadorDocumentos = new(resourceApi);

            //Ejecuciones ordenadas en función de sus dependencias

            //Persona sin dependencias                
            actualizadorPersonas.ActualizarPertenenciaLineas(pPersons: pPersons);
            actualizadorPersonas.ActualizarNumeroIPProyectos(pPersons: pPersons);
            actualizadorPersonas.ActualizarIPGruposActuales(pPersons: pPersons);
            actualizadorPersonas.ActualizarIPGruposHistoricos(pPersons: pPersons);
            actualizadorPersonas.ActualizarIPProyectosActuales(pPersons: pPersons);
            actualizadorPersonas.ActualizarIPProyectosHistoricos(pPersons: pPersons);

            //Persona con dependencias  
            actualizadorPersonas.ActualizarNumeroPublicacionesValidadas(pPersons: pPersons);
            actualizadorPersonas.ActualizarAreasPersonas(pPersons: pPersons);
            actualizadorPersonas.ActualizarNumeroColaboradoresPublicos(pPersons: pPersons);
            actualizadorPersonas.ActualizarNumeroPublicacionesPublicas(pPersons: pPersons);
            actualizadorPersonas.ActualizarNumeroProyectosValidados(pPersons: pPersons);
            actualizadorPersonas.ActualizarNumeroProyectosPublicos(pPersons: pPersons);
            actualizadorPersonas.ActualizarNumeroResearchObjectsPublicos(pPersons: pPersons);
            actualizadorPersonas.ActualizarNumeroAreasTematicas(pPersons: pPersons);


            //Documentos sin dependencias  
            List<string> documents = resourceApi.VirtuosoQuery("select ?document",
                $@" where
                    {{ 
                        ?document <http://purl.org/ontology/bibo/authorList> ?autores. 
                        ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.  
                        FILTER(?person in (<{string.Join(">,<", pPersons)}>))
                    }}", "document").results.bindings.Select(x => x["document"].value).Distinct().ToList();
            List<List<string>> listDocuments = ActualizadorBase.SplitList(documents, 500).ToList();
            foreach (List<string> listDocumentIn in listDocuments)
            {
                actualizadorDocumentos.ActualizarPositionAutorPrincipal(pDocuments: listDocumentIn);
                actualizadorDocumentos.ActualizarGenderAutorPrincipal(pDocuments: listDocumentIn);
            }
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican proyectos
        /// </summary>
        /// <param name="pProyectso">ID de proyectos</param>
        public static void DesnormalizarDatosProyectos(List<string> pProjects = null)
        {
            ActualizadorPerson actualizadorPersonas = new(resourceApi);
            ActualizadorGroup actualizadorGrupos = new(resourceApi);
            ActualizadorProject actualizadorProject = new(resourceApi);

            //Ejecuciones ordenadas en función de sus dependencias

            //Persona sin dependencias                
            actualizadorPersonas.ActualizarNumeroIPProyectos(pProjects: pProjects);
            actualizadorPersonas.ActualizarIPProyectosActuales(pProjects: pProjects);
            actualizadorPersonas.ActualizarIPProyectosHistoricos(pProjects: pProjects);

            //Proyectos sin dependencias
            actualizadorProject.ActualizarProyectosValidados(pProjects: pProjects);
            actualizadorProject.ActualizarMiembros(pProjects: pProjects);
            actualizadorProject.ActualizarPertenenciaGrupos(pProjects: pProjects);
            actualizadorProject.ActualizarAniosInicio(pProjects: pProjects);
            actualizadorProject.ActualizarAniosFin(pProjects: pProjects);

            //Proyectos con dependencias
            actualizadorProject.ActualizarNumeroAreasTematicas(pProjects: pProjects);
            actualizadorProject.ActualizarMiembrosUnificados(pProjects: pProjects);
            actualizadorProject.ActualizarNumeroMiembros(pProjects: pProjects);
            actualizadorProject.ActualizarNumeroColaboradores(pProjects: pProjects);
            actualizadorProject.ActualizarNumeroPublicaciones(pProjects: pProjects);

            //Persona con dependencias  
            actualizadorPersonas.ActualizarNumeroColaboradoresPublicos(pProjects: pProjects);
            actualizadorPersonas.ActualizarNumeroProyectosValidados(pProjects: pProjects);
            actualizadorPersonas.ActualizarNumeroProyectosPublicos(pProjects: pProjects);

            //Grupo con dependencias
            actualizadorGrupos.ActualizarNumeroProyectos(pProjects: pProjects);
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican grupos
        /// </summary>
        /// <param name="pGroups">ID de grupos</param>
        public static void DesnormalizarDatosGrupos(List<string> pGroups = null)
        {
            ActualizadorPerson actualizadorPersonas = new(resourceApi);
            ActualizadorGroup actualizadorGrupos = new(resourceApi);
            ActualizadorDocument actualizadorDocument = new(resourceApi);
            ActualizadorProject actualizadorProject = new(resourceApi);

            //Ejecuciones ordenadas en función de sus dependencias

            //Persona sin dependencias                
            actualizadorPersonas.ActualizarPertenenciaLineas(pGroups: pGroups);
            actualizadorPersonas.ActualizarIPGruposActuales(pGroups: pGroups);
            actualizadorPersonas.ActualizarIPGruposHistoricos(pGroups: pGroups);

            //Grupo sin dependencias                
            actualizadorGrupos.ActualizarMiembros(pGroups: pGroups);
            actualizadorGrupos.ActualizarGruposValidados(pGroups: pGroups);
            actualizadorGrupos.ActualizarPertenenciaLineas(pGroups: pGroups);

            //Proyectos sin dependencias
            actualizadorProject.ActualizarPertenenciaGrupos(pGroups: pGroups);

            //Documentos sin dependencias
            actualizadorDocument.ActualizarPertenenciaGrupos(pGroups: pGroups);

            //Grupo con dependencias
            actualizadorGrupos.ActualizarMiembrosUnificados(pGroups: pGroups);
            actualizadorGrupos.ActualizarNumeroMiembros(pGroups: pGroups);
            actualizadorGrupos.ActualizarNumeroPublicaciones(pGroups: pGroups);
            actualizadorGrupos.ActualizarNumeroColaboradores(pGroups: pGroups);
            actualizadorGrupos.ActualizarNumeroAreasTematicas(pGroups: pGroups);
            actualizadorGrupos.ActualizarAreasGrupos(pGroups: pGroups);
            actualizadorGrupos.ActualizarNumeroProyectos(pGroups: pGroups);
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican documentos
        /// </summary>
        /// <param name="pDocuments">ID de los documentos</param>
        public static void DesnormalizarDatosDocumento(List<string> pDocuments)
        {
            ActualizadorPerson actualizadorPersonas = new(resourceApi);
            ActualizadorGroup actualizadorGrupos = new(resourceApi);
            ActualizadorDocument actualizadorDocument = new(resourceApi);
            ActualizadorProject actualizadorProject = new(resourceApi);

            //Ejecuciones ordenadas en función de sus dependencias

            //Documentos sin dependencias
            actualizadorDocument.ActualizarDocumentosValidados(pDocuments: pDocuments);
            actualizadorDocument.ActualizarPertenenciaGrupos(pDocuments: pDocuments);
            actualizadorDocument.ActualizarNumeroCitasMaximas(pDocuments: pDocuments);
            actualizadorDocument.ActualizarAreasDocumentos(pDocuments: pDocuments);
            actualizadorDocument.ActualizarTagsDocumentos(pDocuments: pDocuments);
            actualizadorDocument.ActualizarAnios(pDocuments: pDocuments);
            actualizadorDocument.ActualizarIndicesImpacto(pDocuments: pDocuments);
            actualizadorDocument.ActualizarPositionAutorPrincipal(pDocuments: pDocuments);
            actualizadorDocument.ActualizarGenderAutorPrincipal(pDocuments: pDocuments);
            actualizadorDocument.ModificarNombreRevistaDesnormalizado(pDocuments: pDocuments);
            actualizadorDocument.ModificarEditorialRevistaDesnormalizado(pDocuments: pDocuments);
            actualizadorDocument.ModificarISSNRevistaDesnormalizado(pDocuments: pDocuments);
            actualizadorDocument.ActualizarNumeroVinculados(pDocuments: pDocuments);

            //Proyectos con dependencias
            actualizadorProject.ActualizarNumeroAreasTematicas(pDocuments: pDocuments);
            actualizadorProject.ActualizarNumeroColaboradores(pDocuments: pDocuments);
            actualizadorProject.ActualizarNumeroPublicaciones(pDocuments: pDocuments);

            //Persona con dependencias  
            List<string> persons = resourceApi.VirtuosoQuery("select ?person",
                $@" where
                    {{ 
                        ?document <http://purl.org/ontology/bibo/authorList> ?autores. 
                        ?autores <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.  
                        FILTER(?document in (<{string.Join(">,<", pDocuments)}>))
                    }}", "document").results.bindings.Select(x => x["person"].value).Distinct().ToList();
            List<List<string>> listPersons = ActualizadorBase.SplitList(persons, 500).ToList();
            foreach (List<string> listPersonIn in listPersons)
            {
                actualizadorPersonas.ActualizarNumeroPublicacionesValidadas(pPersons: listPersonIn);
                actualizadorPersonas.ActualizarAreasPersonas(pPersons: listPersonIn);
                actualizadorPersonas.ActualizarNumeroColaboradoresPublicos(pPersons: listPersonIn);
                actualizadorPersonas.ActualizarNumeroPublicacionesPublicas(pPersons: listPersonIn);
                actualizadorPersonas.ActualizarNumeroAreasTematicas(pPersons: listPersonIn);
                actualizadorPersonas.ActualizarHIndex(pPersons: listPersonIn);
            }

            //Grupo con dependencias
            List<string> groups = resourceApi.VirtuosoQuery("select ?group",
                $@" where
                    {{   
                        ?document <http://w3id.org/roh/isProducedBy> ?group.  FILTER(?document in (<{string.Join(">,<", pDocuments)}>))
                    }}", "document").results.bindings.Select(x => x["group"].value).Distinct().ToList();
            List<List<string>> listGroups = ActualizadorBase.SplitList(groups, 500).ToList();
            foreach (List<string> listGroupsIn in listGroups)
            {
                actualizadorGrupos.ActualizarNumeroPublicaciones(pGroups: listGroupsIn);
                actualizadorGrupos.ActualizarNumeroColaboradores(pGroups: listGroupsIn);
                actualizadorGrupos.ActualizarNumeroAreasTematicas(pGroups: listGroupsIn);
                actualizadorGrupos.ActualizarAreasGrupos(pGroups: listGroupsIn);
            }
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican research objects
        /// </summary>
        /// <param name="pROs">ID de los research objects</param>
        public static void DesnormalizarDatosResearchObject(List<string> pROs)
        {
            ActualizadorRO actualizadorRO = new(resourceApi);

            //Ejecuciones ordenadas en función de sus dependencias

            //ROs sin dependencias
            actualizadorRO.ActualizarAreasRO(pROs: pROs);
            actualizadorRO.ActualizarTagsRO(pROs: pROs);
            actualizadorRO.ActualizarNumeroVinculados(pROs: pROs);
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican patentes
        /// </summary>
        /// <param name="pPatents">ID de las patentes</param>
        public static void DesnormalizarDatosPatentes(List<string> pPatents)
        {
            ActualizadorPatent actualizadorPatent = new(resourceApi);

            //Ejecuciones ordenadas en función de sus dependencias

            //Patentes sin dependencias
            actualizadorPatent.ActualizarPatentesValidadas(pPatents: pPatents);
            actualizadorPatent.ActualizarMiembros(pPatents: pPatents);
        }


        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican personas relacionados con el CV
        /// </summary>
        /// <param name="pPersons">ID de las personas</param>
        public static void DesnormalizarDatosCVPersonas(List<string> pPersons = null)
        {
            ActualizadorCV actualizadorCV = new(resourceApi);
            actualizadorCV.CrearCVs(pPersons: pPersons);
            actualizadorCV.ModificarDocumentos(pPersons: pPersons);
            //actualizadorCV.CambiarPrivacidadDocumentos(pPersons: pPersons);
            actualizadorCV.ModificarResearchObjects(pPersons: pPersons);
            actualizadorCV.CambiarPrivacidadResearchObjects(pPersons: pPersons);
            actualizadorCV.ModificarProyectos(pPersons: pPersons);
            actualizadorCV.ModificarGrupos(pPersons: pPersons);
            actualizadorCV.ModificarPatentes(pPersons: pPersons);
            actualizadorCV.ModificarElementosCV(pPersons: pPersons);
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican proyectos relacionados con el CV
        /// </summary>
        /// <param name="pProyectso">ID de proyectos</param>
        public static void DesnormalizarDatosCVProyectos(List<string> pProjects = null)
        {
            ActualizadorCV actualizadorCV = new(resourceApi);
            actualizadorCV.CrearCVs(pProjects: pProjects);
            actualizadorCV.ModificarProyectos(pProjects: pProjects);
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican grupos relacionados con el CV
        /// </summary>
        /// <param name="pGroups">ID de grupos</param>
        public static void DesnormalizarDatosCVGrupos(List<string> pGroups = null)
        {
            ActualizadorCV actualizadorCV = new(resourceApi);
            actualizadorCV.CrearCVs(pGroups: pGroups);
            actualizadorCV.ModificarGrupos(pGroups: pGroups);
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican documentos relacionados con el CV
        /// </summary>
        /// <param name="pDocuments">ID de los documentos</param>
        public static void DesnormalizarDatosCVDocumento(List<string> pDocuments)
        {
            ActualizadorCV actualizadorCV = new(resourceApi);
            actualizadorCV.CrearCVs(pDocuments: pDocuments);
            actualizadorCV.ModificarDocumentos(pDocuments: pDocuments);
            //actualizadorCV.CambiarPrivacidadDocumentos(pDocuments: pDocuments);
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican research objects relacionados con el CV
        /// </summary>
        /// <param name="pROs">ID de los research objects</param>
        public static void DesnormalizarDatosCVResearchObject(List<string> pROs)
        {
            ActualizadorCV actualizadorCV = new(resourceApi);
            actualizadorCV.CrearCVs(pROs: pROs);
            actualizadorCV.ModificarResearchObjects(pROs: pROs);
            actualizadorCV.CambiarPrivacidadResearchObjects(pROs: pROs);
        }

        /// <summary>
        /// Actualiza elementos desnormalizados cuando se crean/modifican patentes relacionados con el CV
        /// </summary>
        /// <param name="pPatents">ID de las patentes</param>
        public static void DesnormalizarDatosCVPatentes(List<string> pPatents)
        {
            ActualizadorCV actualizadorCV = new(resourceApi);
            actualizadorCV.CrearCVs(pPatents: pPatents);
            actualizadorCV.ModificarPatentes(pPatents: pPatents);
        }



        /// <summary>
        /// Realiza la fusión de 2 entidades
        /// <param name="pIdMalo">Identificador a eliminar</param>
        /// <param name="pIdBueno">Identificador donde meter la fusión</param>
        /// </summary>
        public static void Fusion(string pIdMalo, string pIdBueno)
        {
            while (true)
            {
                string select = "SELECT DISTINCT ?id ?type ?prop ";
                string where = $@"WHERE {{                                
                                ?id ?prop <http://gnoss/{resourceApi.GetShortGuid(pIdMalo).ToString().ToUpper()}>.
                                OPTIONAL{{?id <http://www.w3.org/1999/02/22-rdf-syntax-ns#type> ?type.}}
                                
                            }} ";
                SparqlObject resultadoQuery = resourceApi.VirtuosoQuery(select, where, communityID);
                if (resultadoQuery.results.bindings.Count == 0)
                {
                    break;
                }
                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        string id = fila["id"].value;
                        string prop = fila["prop"].value;
                        //Si el ID es un GUID modificamos directamente
                        if (Guid.TryParse(id.Replace("http://gnoss/", ""), out Guid x))
                        {
                            TriplesToModify t = new();
                            t.NewValue = pIdBueno;
                            t.OldValue = pIdMalo;
                            t.Predicate = prop;
                            resourceApi.ModifyPropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.TriplesToModify>>() { { resourceApi.GetShortGuid(id), new List<Gnoss.ApiWrapper.Model.TriplesToModify>() { t } } });
                        }
                        else
                        {
                            Guid main = resourceApi.GetShortGuid(id);
                            string rdftype = resourceApi.VirtuosoQuery("select ?type", "where{<http://gnoss/" + main.ToString().ToUpper() + "> a ?type}", new Guid("b836078b-78a0-4939-b809-3f2ccf4e5c01")).results.bindings[0]["type"].value;
                            List<string> props = new List<string>() { prop };
                            List<string> entities = new List<string>() { id };

                            string oAux = id;
                            while (true)
                            {
                                SparqlObject respuesta = resourceApi.VirtuosoQuery("select ?s ?p", "where{?s ?p <" + oAux + ">. FILTER(?p!=<http://gnoss/hasEntidad> )}", rdftype);
                                if (respuesta.results.bindings.Count == 0)
                                {
                                    entities.Remove(entities.Last());
                                    break;
                                }
                                else
                                {
                                    Dictionary<string, SparqlObject.Data> fila2 = respuesta.results.bindings[0];
                                    if (resourceApi.GetShortGuid(id) != resourceApi.GetShortGuid(fila2["s"].value))
                                    {
                                        entities.Remove(entities.Last());
                                        break;
                                    }
                                    else
                                    {
                                        entities.Add(fila2["s"].value);
                                        props.Add(fila2["p"].value);
                                        oAux = fila2["s"].value;
                                    }
                                }
                            }
                            props.Reverse();
                            entities.Reverse();
                            TriplesToModify t = new();
                            t.NewValue = string.Join("|", entities) + "|" + pIdBueno;
                            t.OldValue = string.Join("|", entities) + "|" + pIdMalo;
                            t.Predicate = string.Join("|", props);
                            resourceApi.ModifyPropertiesLoadedResources(new Dictionary<Guid, List<Gnoss.ApiWrapper.Model.TriplesToModify>>() { { resourceApi.GetShortGuid(id), new List<Gnoss.ApiWrapper.Model.TriplesToModify>() { t } } });
                        }
                    }
                }
            }
            resourceApi.PersistentDelete(resourceApi.GetShortGuid(pIdMalo));
        }


    }
}
