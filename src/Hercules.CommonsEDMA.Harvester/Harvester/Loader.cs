using DepartmentOntology;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Harvester.Models.ModelsBBDD;
using Harvester.Models.RabbitMQ;
using Harvester.Models.SGI.Autorizaciones;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using Newtonsoft.Json;
using OAI_PMH.Models.SGI.GruposInvestigacion;
using OAI_PMH.Models.SGI.Organization;
using OAI_PMH.Models.SGI.PersonalData;
using OAI_PMH.Models.SGI.Project;
using OAI_PMH.Models.SGI.ProteccionIndustrialIntelectual;
using ProjectauthorizationOntology;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Xml.Serialization;
using Utilidades;

namespace Harvester
{
    public class Loader
    {
        private static Harvester harvester;
        private static IHarvesterServices harvesterServices;
        private static ReadConfig _Config;

        private static string RUTA_PREFIJOS = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Utilidades/prefijos.json";
        private static string mPrefijos = string.Join(" ", JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(RUTA_PREFIJOS)));

        //Resource API
        public static ResourceApi mResourceApi { get; set; }

        /// <summary>
        /// Main.
        /// </summary>
        /// <param name="pResourceApi">ResourceAPI.</param>
        public Loader(ResourceApi pResourceApi)
        {
            harvesterServices = new IHarvesterServices();
            harvester = new Harvester(harvesterServices);
            _Config = new ReadConfig();
            mResourceApi = pResourceApi;
        }

        /// <summary>
        /// Carga las entidades principales.
        /// </summary>
        public void LoadMainEntities()
        {
            RabbitServiceWriterDenormalizer rabbitServiceWriterDenormalizer = new RabbitServiceWriterDenormalizer(_Config);
            string expresionCron = _Config.GetCronExpression();

            UtilidadesGeneral.IniciadorDiccionarioPaises();
            UtilidadesGeneral.IniciadorDiccionarioRegion();

            while (true)
            {
                try
                {
                    var expression = new CronExpression(expresionCron);
                    DateTimeOffset? time = expression.GetTimeAfter(DateTimeOffset.UtcNow);

                    if (time.HasValue)
                    {
                        Thread.Sleep((time.Value.UtcDateTime - DateTimeOffset.UtcNow));

                        // Carga de datos.
                        CargarDatosSGI(rabbitServiceWriterDenormalizer);

                        // Fecha de la última actualización.
                        //string fecha = "1500-01-01T00:00:00Z";
                        string fecha = LeerFicheroFecha(_Config);

                        // Genero los ficheros con los datos a procesar desde la fecha.
                        GuardarIdentificadores(_Config, "Organizacion", fecha);
                        GuardarIdentificadores(_Config, "Persona", fecha);
                        GuardarIdentificadores(_Config, "Proyecto", fecha);
                        GuardarIdentificadores(_Config, "PRC", fecha, true);
                        GuardarIdentificadores(_Config, "AutorizacionProyecto", fecha);
                        GuardarIdentificadores(_Config, "Grupo", fecha);
                        GuardarIdentificadores(_Config, "Invencion", fecha);

                        // Actualizo la última fecha de carga.
                        UpdateLastDate(_Config, DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'"));

                        // Carga de datos.
                        CargarDatosSGI(rabbitServiceWriterDenormalizer);
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(60000);
                }
            }
        }

        /// <summary>
        /// Obtiene los datos del SGI y los carga.
        /// </summary>
        /// <param name="pRabbitConf"></param>
        public void CargarDatosSGI(RabbitServiceWriterDenormalizer pRabbitConf)
        {
            Dictionary<string, HashSet<string>> dicIdentificadores = new Dictionary<string, HashSet<string>>();
            dicIdentificadores.Add("organization", new HashSet<string>());
            dicIdentificadores.Add("person", new HashSet<string>());
            dicIdentificadores.Add("project", new HashSet<string>());
            dicIdentificadores.Add("group", new HashSet<string>());
            dicIdentificadores.Add("patent", new HashSet<string>());
            dicIdentificadores.Add("projectauthorization", new HashSet<string>());
            dicIdentificadores.Add("document", new HashSet<string>());

            Dictionary<string, Dictionary<string, string>> dicRutas = new Dictionary<string, Dictionary<string, string>>();
            dicRutas.Add("Organizacion", new Dictionary<string, string>() { { $@"{_Config.GetLogCargas()}\Organizacion\pending", $@"{_Config.GetLogCargas()}\Organizacion\processed" } });
            dicRutas.Add("Persona", new Dictionary<string, string>() { { $@"{_Config.GetLogCargas()}\Persona\pending", $@"{_Config.GetLogCargas()}\Persona\processed" } });
            dicRutas.Add("Proyecto", new Dictionary<string, string>() { { $@"{_Config.GetLogCargas()}\Proyecto\pending", $@"{_Config.GetLogCargas()}\Proyecto\processed" } });
            dicRutas.Add("Grupo", new Dictionary<string, string>() { { $@"{_Config.GetLogCargas()}\Grupo\pending", $@"{_Config.GetLogCargas()}\Grupo\processed" } });
            dicRutas.Add("Invencion", new Dictionary<string, string>() { { $@"{_Config.GetLogCargas()}\Invencion\pending", $@"{_Config.GetLogCargas()}\Invencion\processed" } });
            dicRutas.Add("AutorizacionProyecto", new Dictionary<string, string>() { { $@"{_Config.GetLogCargas()}\AutorizacionProyecto\pending", $@"{_Config.GetLogCargas()}\AutorizacionProyecto\processed" } });
            dicRutas.Add("PRC", new Dictionary<string, string>() { { $@"{_Config.GetLogCargas()}\PRC\pending", $@"{_Config.GetLogCargas()}\PRC\processed" } });

            // Organizaciones.
            mResourceApi.ChangeOntoly("organization");
            ProcesarFichero(_Config, "Organizacion", dicIdentificadores, dicRutas, pRabbitConf);

            // Personas. 
            mResourceApi.ChangeOntoly("person");
            ProcesarFichero(_Config, "Persona", dicIdentificadores, dicRutas, pRabbitConf);

            // Proyectos.
            mResourceApi.ChangeOntoly("project");
            ProcesarFichero(_Config, "Proyecto", dicIdentificadores, dicRutas, pRabbitConf);

            // Document.
            mResourceApi.ChangeOntoly("document");
            ProcesarFichero(_Config, "PRC", dicIdentificadores, dicRutas, pRabbitConf);

            // Autorizaciones.
            mResourceApi.ChangeOntoly("projectauthorization");
            ProcesarFichero(_Config, "AutorizacionProyecto", dicIdentificadores, dicRutas, pRabbitConf);

            // Grupos.
            mResourceApi.ChangeOntoly("group");
            ProcesarFichero(_Config, "Grupo", dicIdentificadores, dicRutas, pRabbitConf);

            // Patentes.
            mResourceApi.ChangeOntoly("patent");
            ProcesarFichero(_Config, "Invencion", dicIdentificadores, dicRutas, pRabbitConf);            
        }

        /// <summary>
        /// Obtiene los identificadores de los datos modificados.
        /// </summary>
        /// <param name="pConfig"></param>
        /// <param name="pSet"></param>
        /// <param name="pFecha"></param>
        public void GuardarIdentificadores(ReadConfig pConfig, string pSet, string pFecha, bool pPRC = false)
        {
            if (pPRC == false)
            {
                harvester.Harvest(pConfig, pSet, pFecha);
            }
            else
            {
                harvester.HarvestPRC(pConfig, pSet, pFecha);
            }
        }

        /// <summary>
        /// Inserción a la cola.
        /// </summary>
        /// <param name="pRabbit"></param>
        /// <param name="pListaIds"></param>
        public void InsertToQueue(RabbitServiceWriterDenormalizer pRabbit, HashSet<string> pListaIds, string pTipo)
        {
            if (pListaIds.Count > 0)
            {
                switch (pTipo)
                {
                    case "person":
                        pRabbit.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.person, pListaIds));
                        break;
                    case "project":
                        pRabbit.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.project, pListaIds));
                        break;
                    case "group":
                        pRabbit.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.group, pListaIds));
                        break;
                    case "patent":
                        pRabbit.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.patent, pListaIds));
                        break;
                    case "organization":
                        pRabbit.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.organization, pListaIds));
                        break;
                    case "projectauthorization":
                        pRabbit.PublishMessage(new DenormalizerItemQueue(DenormalizerItemQueue.ItemType.projectauthorization, pListaIds));
                        break;
                }
            }
        }

        /// <summary>
        /// Obtiene los datos de los ficheros y los carga.
        /// </summary>
        /// <param name="pConfig"></param>
        /// <param name="pSet"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <param name="dicProyectos"></param>
        /// <param name="dicPersonas"></param>
        public void ProcesarFichero(ReadConfig pConfig, string pSet, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf)
        {
            string directorioPendientes = pDicRutas[pSet].First().Key;
            string directorioProcesados = pDicRutas[pSet].First().Value;

            if (!Directory.Exists(directorioPendientes))
            {
                Directory.CreateDirectory(directorioPendientes);
            }
            if (!Directory.Exists(directorioProcesados))
            {
                Directory.CreateDirectory(directorioProcesados);
            }

            foreach (string fichero in Directory.EnumerateFiles(directorioPendientes))
            {
                pDicRutas[pSet][directorioPendientes] += fichero.Substring(fichero.LastIndexOf("\\"));
                List<string> idsACargar = File.ReadAllLines(fichero).Distinct().ToList();

                if (File.Exists(pDicRutas[pSet][directorioPendientes]))
                {
                    List<string> listaIdsCargados = File.ReadAllLines(pDicRutas[pSet][directorioPendientes]).Distinct().ToList();
                    idsACargar = idsACargar.Except(listaIdsCargados).Distinct().ToList();
                }
                else
                {
                    FileStream ficheroAux = File.Create(pDicRutas[pSet][directorioPendientes]);
                    ficheroAux.Close();
                }

                idsACargar.Sort();

                foreach (string id in idsACargar)
                {
                    switch (pSet)
                    {
                        #region - Organizacion
                        case "Organizacion":

                            Empresa empresa = Empresa.GetOrganizacionSGI(harvesterServices, _Config, id, pDicRutas);
                            if (empresa != null && !string.IsNullOrEmpty(empresa.Nombre))
                            {
                                string idGnossOrg = empresa.Cargar(harvesterServices, pConfig, mResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["organization"].Add(idGnossOrg);
                            }
                            File.AppendAllText(pDicRutas[pSet][directorioPendientes], id + Environment.NewLine);
                            break;

                        #endregion

                        #region - Persona
                        case "Persona":

                            Persona persona = Persona.GetPersonaSGI(harvesterServices, _Config, id, pDicRutas);                            
                            if (persona != null && !string.IsNullOrEmpty(persona.Nombre))
                            {
                                if (persona.SeminariosCursos != null && persona.SeminariosCursos.Any())
                                {

                                }
                                if (persona.Ciclos != null && persona.Ciclos.Any())
                                {

                                }

                                string idGnossPersona = persona.Cargar(harvesterServices, pConfig, mResourceApi, "person", pDicIdentificadores, pDicRutas, pRabbitConf, true);
                                pDicIdentificadores["person"].Add(idGnossPersona);
                            }
                            File.AppendAllText(pDicRutas[pSet][directorioPendientes], id + Environment.NewLine);
                            break;
                                                    
                        #endregion

                        #region - Proyecto
                        case "Proyecto":

                            Proyecto proyectoSGI = Proyecto.GetProyectoSGI(harvesterServices, _Config, id, pDicRutas);
                            if (proyectoSGI != null && !string.IsNullOrEmpty(proyectoSGI.Titulo))
                            {
                                string idGnossProy = proyectoSGI.Cargar(harvesterServices, pConfig, mResourceApi, "project", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["project"].Add(idGnossProy);
                            }
                            File.AppendAllText(pDicRutas[pSet][directorioPendientes], id + Environment.NewLine);
                            break;
                        
                        #endregion

                        #region - PRC
                        case "PRC":
                            bool eliminar = false;
                            string idRecurso = id.Split("||")[0];
                            if (id.StartsWith("Eliminar_"))
                            {
                                eliminar = true;
                                idRecurso = idRecurso.Split("Eliminar_")[1];
                            }
                            if (!idRecurso.Contains('/'))
                            {
                                idRecurso = "http://gnoss.com/items/" + idRecurso;
                            }
                            string estado = id.Split("||")[1];

                            Guid guid = mResourceApi.GetShortGuid(idRecurso);
                            Dictionary<string, string> data = GetValues(idRecurso);

                            foreach (KeyValuePair<string, string> item in data)
                            {
                                if (!string.IsNullOrEmpty(item.Value))
                                {
                                    if (item.Key == "projectAux")
                                    {
                                        Borrado(guid, "http://w3id.org/roh/projectAux", item.Value);
                                    }
                                    else if (item.Key == "validationStatusPRC" && item.Value != "validado")
                                    {
                                        switch (estado)
                                        {
                                            case "VALIDADO":
                                                Modificacion(guid, "http://w3id.org/roh/validationStatusPRC", "validado", item.Value);
                                                break;
                                            default:
                                                Modificacion(guid, "http://w3id.org/roh/validationStatusPRC", "rechazado", item.Value);
                                                break;
                                        }
                                    }
                                    else if (item.Key == "validationDeleteStatusPRC" && eliminar)
                                    {
                                        if (estado.Equals("VALIDADO"))
                                        {
                                            BorrarPublicacion(idRecurso);
                                        }
                                        else
                                        {
                                            Modificacion(guid, "http://w3id.org/roh/validationDeleteStatusPRC", "rechazado", item.Value);
                                        }
                                    }
                                    else
                                    {
                                        switch (estado)
                                        {
                                            case "VALIDADO":
                                                Modificacion(guid, "http://w3id.org/roh/isValidated", "true", item.Value);
                                                break;
                                            default:
                                                Modificacion(guid, "http://w3id.org/roh/isValidated", "false", item.Value);
                                                break;
                                        }
                                    }
                                }
                            }

                            // Guardamos el ID cargado.
                            File.AppendAllText(pDicRutas[pSet][directorioPendientes], id + Environment.NewLine);
                            break;
                        #endregion

                        #region - Autorizacion proyecto
                        case "AutorizacionProyecto":

                            Autorizacion autorizacionSGI = Autorizacion.GetAutorizacionSGI(harvesterServices, _Config, id, pDicRutas);
                            if (autorizacionSGI != null && !string.IsNullOrEmpty(autorizacionSGI.tituloProyecto) && !string.IsNullOrEmpty(autorizacionSGI.solicitanteRef) && !string.IsNullOrEmpty(autorizacionSGI.entidadRef))
                            {
                                string idGnossAutorizacion = autorizacionSGI.Cargar(harvesterServices, pConfig, mResourceApi, "projectauthorization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["projectauthorization"].Add(idGnossAutorizacion);
                            }
                            break;
                            
                        #endregion

                        #region - Invencion
                        case "Invencion":

                            Invencion invencion = Invencion.GetInvencionSGI(harvesterServices, _Config, id, pDicRutas);
                            if (invencion != null && !string.IsNullOrEmpty(invencion.titulo))
                            {
                                string idGnossInv = invencion.Cargar(harvesterServices, pConfig, mResourceApi, "patent", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["patent"].Add(idGnossInv);
                            }
                            File.AppendAllText(pDicRutas[pSet][directorioPendientes], id + Environment.NewLine);
                            break;

                        #endregion

                        #region - Grupo
                        case "Grupo":

                            Grupo grupo = Grupo.GetGrupoSGI(harvesterServices, _Config, id, pDicRutas);
                            if (grupo != null && !string.IsNullOrEmpty(grupo.nombre))
                            {
                                string idGnossGrupo = grupo.Cargar(harvesterServices, pConfig, mResourceApi, "group", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["group"].Add(idGnossGrupo);
                            }
                            File.AppendAllText(pDicRutas[pSet][directorioPendientes], id + Environment.NewLine);
                            break;

                       #endregion
                    }
                }

                // Borra el fichero.
                File.Delete(fichero);
            }
        }

        /// <summary>
        /// Permite leer el fichero de la última fecha de modificación.
        /// </summary>
        /// <param name="pConfig"></param>
        /// <returns></returns>
        public string LeerFicheroFecha(ReadConfig pConfig)
        {
            string ficheroFecha = pConfig.GetLastUpdateDate();

            if (!File.Exists(ficheroFecha))
            {
                string fecha = DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
                FileStream fichero = File.Create(ficheroFecha);
                fichero.Close();
                File.WriteAllText(pConfig.GetLastUpdateDate(), fecha);
                return fecha;
            }
            else
            {
                return File.ReadAllText(ficheroFecha);
            }
        }

        /// <summary>
        /// Obtiene el listado de recursos mediante el crisidentifier.
        /// </summary>
        /// <param name="pListaIds">IDs a consultar.</param>
        /// <param name="pOntologia">Ontología.</param>
        /// <returns></returns>
        private static Dictionary<string, string> GetDataBBDD(List<string> pListaIds, string pOntologia)
        {
            List<List<string>> listas = SplitList(pListaIds, 1000).ToList();

            Dictionary<string, string> dicDevolver = new Dictionary<string, string>();

            foreach (List<string> listaItem in listas)
            {
                List<string> listaAux = new List<string>();
                foreach (string item in listaItem)
                {
                    if (item.Contains("_"))
                    {
                        listaAux.Add(item.Split("_")[1]);
                    }
                    else
                    {
                        listaAux.Add(item);
                    }
                }

                string selectPerson = $@"{mPrefijos} SELECT DISTINCT ?s ?crisIdentifier ";
                string wherePerson = $@"WHERE {{ 
                            ?s roh:crisIdentifier ?crisIdentifier. 
                            FILTER(?crisIdentifier in ('{string.Join("', '", listaAux.Select(x => x))}')) }}";

                SparqlObject resultadoQueryPerson = mResourceApi.VirtuosoQuery(selectPerson, wherePerson, pOntologia);

                if (resultadoQueryPerson != null && resultadoQueryPerson.results != null && resultadoQueryPerson.results.bindings != null && resultadoQueryPerson.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPerson.results.bindings)
                    {
                        dicDevolver.Add(fila["crisIdentifier"].value, fila["s"].value);
                    }
                }
            }

            return dicDevolver;
        }

        /// <summary>
        /// Divide una lista en listas pequeñas.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pItems"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> SplitList<T>(List<T> pItems, int pSize)
        {
            for (int i = 0; i < pItems.Count; i += pSize)
            {
                yield return pItems.GetRange(i, Math.Min(pSize, pItems.Count - i));
            }
        }

        /// <summary>
        /// Obtiene los datos de la persona que no deben de borrarse.
        /// </summary>
        /// <param name="pIdRecurso">ID del recurso de la persona a obtener datos.</param>
        private PersonOntology.Person DatosPersonaNoBorrar(string pIdRecurso)
        {
            // Objeto Persona Final
            PersonOntology.Person persona = new PersonOntology.Person();
            persona.Roh_metricPage = new List<PersonOntology.MetricPage>();
            persona.Roh_ignorePublication = new List<PersonOntology.IgnorePublication>();

            HashSet<string> listaMetricPage = new HashSet<string>();
            HashSet<string> listaIgnorePublications = new HashSet<string>();

            List<MetricPageBBDD> listaMetricPagesBBDD = new List<MetricPageBBDD>();

            #region --- Datos de la Persona
            string selectPerson = $@"{mPrefijos} SELECT DISTINCT ?isOtriManager ?isGraphicManager ?ORCID ?scopusId ?researcherId ?semanticScholarId ?gnossUser ?usuarioFigShare ?tokenFigShare ?usuarioGitHub ?tokenGitHub ?metricPage ?useMatching ?ignorePublication ";
            string wherePerson = $@"WHERE {{ 
                            ?s a foaf:Person. 
                            OPTIONAL {{?s roh:isOtriManager ?isOtriManager. }}
                            OPTIONAL {{?s roh:isGraphicManager ?isGraphicManager. }} 
                            OPTIONAL {{?s roh:ORCID ?ORCID. }}
                            OPTIONAL {{?s vivo:scopusId ?scopusId. }}
                            OPTIONAL {{?s vivo:researcherId ?researcherId. }}
                            OPTIONAL {{?s roh:semanticScholarId ?semanticScholarId. }}
                            OPTIONAL {{?s roh:gnossUser ?gnossUser. }}
                            OPTIONAL {{?s roh:usuarioFigShare ?usuarioFigShare. }}
                            OPTIONAL {{?s roh:tokenFigShare ?tokenFigShare. }}
                            OPTIONAL {{?s roh:usuarioGitHub ?usuarioGitHub. }}
                            OPTIONAL {{?s roh:tokenGitHub ?tokenGitHub. }}
                            OPTIONAL {{?s roh:metricPage ?metricPage. }}
                            OPTIONAL {{?s roh:useMatching ?useMatching. }}
                            OPTIONAL {{?s roh:ignorePublication ?ignorePublication. }}
                            FILTER(?s = <{pIdRecurso}>)}} ";

            SparqlObject resultadoQueryPerson = mResourceApi.VirtuosoQuery(selectPerson, wherePerson, "person");

            if (resultadoQueryPerson != null && resultadoQueryPerson.results != null && resultadoQueryPerson.results.bindings != null && resultadoQueryPerson.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPerson.results.bindings)
                {
                    if (fila.ContainsKey("isOtriManager") && !string.IsNullOrEmpty(fila["isOtriManager"].value))
                    {
                        persona.Roh_isOtriManager = bool.Parse(fila["isOtriManager"].value);
                    }
                    if (fila.ContainsKey("isGraphicManager") && !string.IsNullOrEmpty(fila["isGraphicManager"].value))
                    {
                        persona.Roh_isGraphicManager = bool.Parse(fila["isGraphicManager"].value);
                    }
                    if (fila.ContainsKey("ORCID") && !string.IsNullOrEmpty(fila["ORCID"].value))
                    {
                        persona.Roh_ORCID = fila["ORCID"].value;
                    }
                    if (fila.ContainsKey("scopusId") && !string.IsNullOrEmpty(fila["scopusId"].value))
                    {
                        persona.Vivo_scopusId = fila["scopusId"].value;
                    }
                    if (fila.ContainsKey("researcherId") && !string.IsNullOrEmpty(fila["researcherId"].value))
                    {
                        persona.Vivo_researcherId = fila["researcherId"].value;
                    }
                    if (fila.ContainsKey("semanticScholarId") && !string.IsNullOrEmpty(fila["semanticScholarId"].value))
                    {
                        persona.Roh_semanticScholarId = fila["semanticScholarId"].value;
                    }
                    if (fila.ContainsKey("gnossUser") && !string.IsNullOrEmpty(fila["gnossUser"].value))
                    {
                        persona.IdRoh_gnossUser = fila["gnossUser"].value;
                    }
                    if (fila.ContainsKey("usuarioFigShare") && !string.IsNullOrEmpty(fila["usuarioFigShare"].value))
                    {
                        persona.Roh_usuarioFigShare = fila["usuarioFigShare"].value;
                    }
                    if (fila.ContainsKey("tokenFigShare") && !string.IsNullOrEmpty(fila["tokenFigShare"].value))
                    {
                        persona.Roh_tokenFigShare = fila["tokenFigShare"].value;
                    }
                    if (fila.ContainsKey("usuarioGitHub") && !string.IsNullOrEmpty(fila["usuarioGitHub"].value))
                    {
                        persona.Roh_usuarioGitHub = fila["usuarioGitHub"].value;
                    }
                    if (fila.ContainsKey("tokenGitHub") && !string.IsNullOrEmpty(fila["tokenGitHub"].value))
                    {
                        persona.Roh_tokenGitHub = fila["tokenGitHub"].value;
                    }
                    if (fila.ContainsKey("metricPage") && !string.IsNullOrEmpty(fila["metricPage"].value))
                    {
                        listaMetricPage.Add(fila["metricPage"].value);
                    }
                    if (fila.ContainsKey("useMatching") && !string.IsNullOrEmpty(fila["useMatching"].value))
                    {
                        persona.Roh_useMatching = bool.Parse(fila["useMatching"].value);
                    }
                    if (fila.ContainsKey("ignorePublication") && !string.IsNullOrEmpty(fila["ignorePublication"].value))
                    {
                        listaIgnorePublications.Add(fila["ignorePublication"].value);
                    }
                }
            }
            #endregion

            #region --- Datos del IgnorePublication
            string selectIgnorePublication = $@"{mPrefijos} SELECT DISTINCT ?id ?tipo ";
            string whereIgnorePublication = $@"WHERE {{ 
                            ?s a roh:IgnorePublication. 
                            ?s roh:title ?id. 
                            ?s foaf:topic ?tipo. 
                            FILTER(?s in (<{string.Join(">, <", listaIgnorePublications.Select(x => x))}>))}} ";

            SparqlObject resultadoQueryIgnorePublications = mResourceApi.VirtuosoQuery(selectIgnorePublication, whereIgnorePublication, "person");

            if (resultadoQueryIgnorePublications != null && resultadoQueryIgnorePublications.results != null && resultadoQueryIgnorePublications.results.bindings != null && resultadoQueryIgnorePublications.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryIgnorePublications.results.bindings)
                {
                    PersonOntology.IgnorePublication ignorePub = new PersonOntology.IgnorePublication();
                    ignorePub.Foaf_topic = fila["tipo"].value;
                    ignorePub.Roh_title = fila["id"].value;
                    persona.Roh_ignorePublication.Add(ignorePub);
                }
            }
            #endregion

            #region --- Datos del MetricPage
            string selecMetricPage = $@"{mPrefijos} SELECT DISTINCT ?order ?title ?metricGraphic ";
            string whereMetricPage = $@"WHERE {{ 
                            ?s a roh:MetricPage. 
                            ?s roh:order ?order. 
                            ?s roh:title ?title. 
                            OPTIONAL {{?s roh:metricGraphic ?metricGraphic. }}
                            FILTER(?s in (<{string.Join(">, <", listaMetricPage.Select(x => x))}>))}} ";

            SparqlObject resultadoQueryMetricPage = mResourceApi.VirtuosoQuery(selecMetricPage, whereMetricPage, "person");

            if (resultadoQueryMetricPage != null && resultadoQueryMetricPage.results != null && resultadoQueryMetricPage.results.bindings != null && resultadoQueryMetricPage.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryMetricPage.results.bindings)
                {
                    MetricPageBBDD metricPage = new MetricPageBBDD();
                    metricPage.listaGraficas = new List<PersonOntology.MetricGraphic>();
                    metricPage.order = Int32.Parse(fila["order"].value);
                    metricPage.title = fila["title"].value;

                    if (fila.ContainsKey("metricGraphic") && !string.IsNullOrEmpty(fila["metricGraphic"].value))
                    {
                        if (listaMetricPagesBBDD.Any(x => x.order == Int32.Parse(fila["order"].value) && x.title == fila["title"].value))
                        {
                            listaMetricPagesBBDD.First(x => x.order == Int32.Parse(fila["order"].value) && x.title == fila["title"].value).idsMetricGraphic.Add(fila["metricGraphic"].value);
                        }
                        else
                        {
                            metricPage.idsMetricGraphic = new List<string>() { fila["metricGraphic"].value };
                            listaMetricPagesBBDD.Add(metricPage);
                        }
                    }
                    else
                    {
                        listaMetricPagesBBDD.Add(metricPage);
                    }
                }
            }
            #endregion

            #region --- MetricGraphic
            foreach (MetricPageBBDD item in listaMetricPagesBBDD)
            {
                string selectMetricGraphic = $@"{mPrefijos} SELECT DISTINCT ?title ?order ?pageId ?graphicId ?filters ?width ?scales ";
                string whereMetricGraphic = $@"WHERE {{ 
                            ?s a roh:MetricGraphic.
                            ?s roh:title ?title.
                            ?s roh:order ?order.
                            ?s roh:pageId ?pageId.
                            ?s roh:graphicId ?graphicId.
                            OPTIONAL{{ ?s roh:filters ?filters. }}
                            ?s roh:width ?width.
                            OPTIONAL{{ ?s roh:scales ?scales. }}
                            FILTER(?s in (<{string.Join(">, <", item.idsMetricGraphic.Select(x => x))}>))}} ";

                SparqlObject resultadoQueryMetricGraphic = mResourceApi.VirtuosoQuery(selectMetricGraphic, whereMetricGraphic, "person");

                if (resultadoQueryMetricGraphic != null && resultadoQueryMetricGraphic.results != null && resultadoQueryMetricGraphic.results.bindings != null && resultadoQueryMetricGraphic.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryMetricGraphic.results.bindings)
                    {
                        PersonOntology.MetricGraphic metricGraphic = new PersonOntology.MetricGraphic();
                        metricGraphic.Roh_title = fila["title"].value;
                        metricGraphic.Roh_order = Int32.Parse(fila["order"].value);
                        metricGraphic.Roh_pageId = fila["pageId"].value;
                        metricGraphic.Roh_graphicId = fila["graphicId"].value;
                        metricGraphic.Roh_width = fila["width"].value;

                        if (fila.ContainsKey("filters") && !string.IsNullOrEmpty(fila["filters"].value))
                        {
                            metricGraphic.Roh_filters = fila["filters"].value;
                        }

                        if (fila.ContainsKey("scales") && !string.IsNullOrEmpty(fila["scales"].value))
                        {
                            metricGraphic.Roh_scales = fila["scales"].value;
                        }

                        item.listaGraficas.Add(metricGraphic);
                    }
                }
            }
            #endregion

            // Construcción de objetos finales.
            foreach (MetricPageBBDD item in listaMetricPagesBBDD)
            {
                PersonOntology.MetricPage metricPage = new PersonOntology.MetricPage();
                metricPage.Roh_order = item.order;
                metricPage.Roh_title = item.title;
                metricPage.Roh_metricGraphic = item.listaGraficas;
                persona.Roh_metricPage.Add(metricPage);
            }

            return persona;
        }

        /// <summary>
        /// Junta la información de BBDD con la persona obtenida del SGI.
        /// </summary>
        /// <param name="pPersonaSGI">Persona del SGI.</param>
        /// <param name="pPersonaBBDD">Persona de BBDD.</param>
        public void FusionarPersonas(PersonOntology.Person pPersonaSGI, PersonOntology.Person pPersonaBBDD)
        {
            pPersonaSGI.Roh_isOtriManager = pPersonaBBDD.Roh_isOtriManager;
            pPersonaSGI.Roh_isGraphicManager = pPersonaBBDD.Roh_isGraphicManager;
            pPersonaSGI.Roh_ORCID = pPersonaBBDD.Roh_ORCID;
            pPersonaSGI.Vivo_scopusId = pPersonaBBDD.Vivo_scopusId;
            pPersonaSGI.Vivo_researcherId = pPersonaBBDD.Vivo_researcherId;
            pPersonaSGI.Roh_semanticScholarId = pPersonaBBDD.Roh_semanticScholarId;
            pPersonaSGI.IdRoh_gnossUser = pPersonaBBDD.IdRoh_gnossUser;
            pPersonaSGI.Roh_usuarioFigShare = pPersonaBBDD.Roh_usuarioFigShare;
            pPersonaSGI.Roh_tokenFigShare = pPersonaBBDD.Roh_tokenFigShare;
            pPersonaSGI.Roh_usuarioGitHub = pPersonaBBDD.Roh_usuarioGitHub;
            pPersonaSGI.Roh_tokenGitHub = pPersonaBBDD.Roh_tokenGitHub;
            pPersonaSGI.Roh_metricPage = pPersonaBBDD.Roh_metricPage;
            pPersonaSGI.Roh_useMatching = pPersonaBBDD.Roh_useMatching;
            pPersonaSGI.Roh_ignorePublication = pPersonaBBDD.Roh_ignorePublication;
        }

        /// <summary>
        /// Inicia los diccionarios y los puebla con los datos leidos de BBDD
        /// </summary>
        /// <param name="dicProyectos"></param>
        /// <param name="dicPersonas"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <param name="dicAutorizaciones"></param>
        /// <param name="dicGrupos"></param>
        private void IniciacionDiccionarios(ref Dictionary<string, Tuple<string, string>> dicProyectos,
           ref Dictionary<string, Tuple<string, string>> dicOrganizaciones,
           ref Dictionary<string, Tuple<string, string>> dicAutorizaciones, ref Dictionary<string, Tuple<string, string>> dicGrupos,
           ref Dictionary<string, Tuple<string, string>> dicInvenciones)
        {
            //Proyectos
            dicProyectos = new Dictionary<string, Tuple<string, string>>();
            Dictionary<string, string> dicProyectosAux = UtilidadesLoader.GetEntityBBDD("http://vivoweb.org/ontology/core#Project", "project", mResourceApi);
            foreach (KeyValuePair<string, string> keyValue in dicProyectosAux)
            {
                dicProyectos.Add(keyValue.Key, new Tuple<string, string>(keyValue.Value, ""));
            }

            //Personas
            //dicPersonas = new Dictionary<string, Tuple<string, string>>();
            //Dictionary<string, string> dicPersonasAux = UtilidadesLoader.GetEntityBBDD("http://xmlns.com/foaf/0.1/Person", "person", mResourceApi);
            //foreach (KeyValuePair<string, string> keyValue in dicPersonasAux)
            //{
            //    dicPersonas.Add(keyValue.Key, new Tuple<string, string>(keyValue.Value, ""));
            //}

            //Organizaciones
            dicOrganizaciones = new Dictionary<string, Tuple<string, string>>();
            Dictionary<string, string> dicOrganizacionesAux = UtilidadesLoader.GetEntityBBDD("http://xmlns.com/foaf/0.1/Organization", "organization", mResourceApi);
            foreach (KeyValuePair<string, string> keyValue in dicOrganizacionesAux)
            {
                dicOrganizaciones.Add(keyValue.Key, new Tuple<string, string>(keyValue.Value, ""));
            }

            //Autorizaciones
            dicAutorizaciones = new Dictionary<string, Tuple<string, string>>();
            Dictionary<string, string> dicAutorizacionesAux = UtilidadesLoader.GetEntityBBDD("http://w3id.org/roh/ProjectAuthorization", "projectauthorization", mResourceApi);
            foreach (KeyValuePair<string, string> keyValue in dicAutorizacionesAux)
            {
                dicAutorizaciones.Add(keyValue.Key, new Tuple<string, string>(keyValue.Value, ""));
            }

            //Grupos
            dicGrupos = new Dictionary<string, Tuple<string, string>>();
            Dictionary<string, string> dicGruposAux = UtilidadesLoader.GetEntityBBDD("http://xmlns.com/foaf/0.1/Group", "group", mResourceApi);
            foreach (KeyValuePair<string, string> keyValue in dicGruposAux)
            {
                dicGrupos.Add(keyValue.Key, new Tuple<string, string>(keyValue.Value, ""));
            }

            //Invenciones
            dicInvenciones = new Dictionary<string, Tuple<string, string>>();
            Dictionary<string, string> dicInvencionesAux = UtilidadesLoader.GetEntityBBDD("http://purl.org/ontology/bibo/Patent", "patent", mResourceApi);
            foreach (KeyValuePair<string, string> keyValue in dicInvencionesAux)
            {
                dicInvenciones.Add(keyValue.Key, new Tuple<string, string>(keyValue.Value, ""));
            }
        }


        /// <summary>
        /// Devuelve un diccionario con los valores de identificador del documento, si esta validado y estado de validacion en PRC
        /// </summary>
        /// <param name="pIdRecurso">Identificador del recurso</param>
        /// <returns></returns>
        private Dictionary<string, string> GetValues(string pIdRecurso)
        {
            Dictionary<string, string> dicResultados = new Dictionary<string, string>();
            dicResultados.Add("projectAux", "");
            dicResultados.Add("isValidated", "");
            dicResultados.Add("validationStatusPRC", "");
            dicResultados.Add("validationDeleteStatusPRC", "");

            string valorEnviado = string.Empty;
            StringBuilder select = new StringBuilder();
            StringBuilder where = new StringBuilder();

            select.Append(mPrefijos);
            select.Append("SELECT DISTINCT ?projectAux ?isValidated ?validationStatusPRC ?validationDeleteStatusPRC ");
            where.Append("WHERE { ");
            where.Append("?s a bibo:Document. ");
            where.Append("OPTIONAL{?s roh:projectAux ?projectAux. } ");
            where.Append("OPTIONAL{?s roh:isValidated ?isValidated. } ");
            where.Append("OPTIONAL{?s roh:validationStatusPRC ?validationStatusPRC. } ");
            where.Append("OPTIONAL{?s roh:validationDeleteStatusPRC ?validationDeleteStatusPRC. } ");
            where.Append($@"FILTER(?s = <{pIdRecurso}>) ");
            where.Append("} ");

            SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select.ToString(), where.ToString(), "document");

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    if (fila.ContainsKey("projectAux"))
                    {
                        dicResultados["projectAux"] = UtilidadesAPI.GetValorFilaSparqlObject(fila, "projectAux");
                    }
                    if (fila.ContainsKey("isValidated"))
                    {
                        dicResultados["isValidated"] = UtilidadesAPI.GetValorFilaSparqlObject(fila, "isValidated");
                    }
                    if (fila.ContainsKey("validationStatusPRC"))
                    {
                        dicResultados["validationStatusPRC"] = UtilidadesAPI.GetValorFilaSparqlObject(fila, "validationStatusPRC");
                    }
                    if (fila.ContainsKey("validationDeleteStatusPRC"))
                    {
                        dicResultados["validationDeleteStatusPRC"] = UtilidadesAPI.GetValorFilaSparqlObject(fila, "validationDeleteStatusPRC");
                    }
                }
            }

            return dicResultados;
        }

        /// <summary>
        /// Inserta un triple.
        /// </summary>
        /// <param name="pGuid"></param>
        /// <param name="pPropiedad"></param>
        /// <param name="pValorNuevo"></param>
        private void Insercion(Guid pGuid, string pPropiedad, string pValorNuevo)
        {
            Dictionary<Guid, List<TriplesToInclude>> dicInsercion = new Dictionary<Guid, List<TriplesToInclude>>();
            List<TriplesToInclude> listaTriplesInsercion = new List<TriplesToInclude>();
            TriplesToInclude triple = new TriplesToInclude();
            triple.Predicate = pPropiedad;
            triple.NewValue = pValorNuevo;
            listaTriplesInsercion.Add(triple);
            dicInsercion.Add(pGuid, listaTriplesInsercion);
            mResourceApi.InsertPropertiesLoadedResources(dicInsercion);
        }

        /// <summary>
        /// Modifica un triple.
        /// </summary>
        /// <param name="pGuid"></param>
        /// <param name="pPropiedad"></param>
        /// <param name="pValorNuevo"></param>
        /// <param name="pValorAntiguo"></param>
        private void Modificacion(Guid pGuid, string pPropiedad, string pValorNuevo, string pValorAntiguo)
        {
            Dictionary<Guid, List<TriplesToModify>> dicModificacion = new Dictionary<Guid, List<TriplesToModify>>();
            List<TriplesToModify> listaTriplesModificacion = new List<TriplesToModify>();
            TriplesToModify triple = new TriplesToModify();
            triple.Predicate = pPropiedad;
            triple.NewValue = pValorNuevo;
            triple.OldValue = pValorAntiguo;
            listaTriplesModificacion.Add(triple);
            dicModificacion.Add(pGuid, listaTriplesModificacion);
            mResourceApi.ModifyPropertiesLoadedResources(dicModificacion);
        }

        /// <summary>
        /// Borra un triple.
        /// </summary>
        /// <param name="pGuid"></param>
        /// <param name="pPropiedad"></param>
        /// <param name="pValorAntiguo"></param>
        private void Borrado(Guid pGuid, string pPropiedad, string pValorAntiguo)
        {
            Dictionary<Guid, List<RemoveTriples>> dicBorrado = new Dictionary<Guid, List<RemoveTriples>>();
            List<RemoveTriples> listaTriplesBorrado = new List<RemoveTriples>();
            RemoveTriples triple = new RemoveTriples();
            triple.Predicate = pPropiedad;
            triple.Value = pValorAntiguo;
            triple.Title = false;
            triple.Description = false;
            listaTriplesBorrado.Add(triple);
            dicBorrado.Add(pGuid, listaTriplesBorrado);
            mResourceApi.DeletePropertiesLoadedResources(dicBorrado);
        }

        /// <summary>
        /// Elimina una publicación y sus referencias
        /// </summary>
        /// <param name="pEntity">Entidad a eliminar</param>
        /// <returns></returns>
        public void BorrarPublicacion(string pEntity)
        {
            //Eliminar las referencias en los CVs
            String select = @$"select ?cv ?p1 ?o1 ?p2 ?o2 ?item ";
            String where = @$"  where{{
                                            ?cv a <http://w3id.org/roh/CV>.
                                            ?cv ?p1 ?o1.
                                            ?o1 ?p2 ?o2.
                                            ?o2 <http://vivoweb.org/ontology/core#relatedBy> ?item.
                                            FILTER(?item=<{pEntity}>)
                                        }}";
            SparqlObject resultado = mResourceApi.VirtuosoQuery(select, where, "curriculumvitae");

            Dictionary<Guid, List<RemoveTriples>> triplesEliminar = new Dictionary<Guid, List<RemoveTriples>>();
            foreach (var fila in resultado.results.bindings)
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
            mResourceApi.DeletePropertiesLoadedResources(triplesEliminar);

            //Eliminar la publicación
            try
            {
                mResourceApi.PersistentDelete(mResourceApi.GetShortGuid(pEntity), true);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Modifica el fichero con la última fecha.
        /// </summary>
        /// <param name="pConfig"></param>
        public void UpdateLastDate(ReadConfig pConfig, string pFecha)
        {
            File.WriteAllText(pConfig.GetLastUpdateDate(), pFecha);
        }

        /// <summary>
        /// Devuelve una PersonOntology.Person con los datos pasados en <paramref name="pDatos"/>, la cual se ha almacenado en BBDD
        /// </summary>
        /// <param name="pDatos"></param>
        /// <returns></returns>
        public static PersonOntology.Person CrearPersona(Persona pDatos)
        {
            PersonOntology.Person persona = new PersonOntology.Person();

            // Crisidentifier (Se corresponde al DNI sin letra)
            persona.Roh_crisIdentifier = pDatos.Id;

            // Sincronización.
            persona.Roh_isSynchronized = true;

            // Nombre.
            if (!string.IsNullOrEmpty(pDatos.Nombre))
            {
                persona.Foaf_firstName = pDatos.Nombre;
            }

            // Apellidos.
            if (!string.IsNullOrEmpty(pDatos.Apellidos))
            {
                persona.Foaf_lastName = pDatos.Apellidos;
            }

            // Sexo.
            if (pDatos.Sexo != null && !string.IsNullOrEmpty(pDatos.Sexo.Id))
            {
                if (pDatos.Sexo.Id == "V")
                {
                    persona.IdFoaf_gender = $@"{mResourceApi.GraphsUrl}items/gender_000";
                }
                else
                {
                    persona.IdFoaf_gender = $@"{mResourceApi.GraphsUrl}items/gender_010";
                }
            }

            // Nombre completo.
            if (!string.IsNullOrEmpty(pDatos.Nombre) && !string.IsNullOrEmpty(pDatos.Apellidos))
            {
                persona.Foaf_name = pDatos.Nombre + " " + pDatos.Apellidos;
            }

            // Correos.
            if (pDatos.Emails != null && pDatos.Emails.Any())
            {
                persona.Vcard_email = new List<string>();
                foreach (Email item in pDatos.Emails)
                {
                    persona.Vcard_email.Add(item.email);
                }
            }

            // Dirección de contacto.
            if (!string.IsNullOrEmpty(pDatos.DatosContacto?.PaisContacto?.Nombre) || !string.IsNullOrEmpty(pDatos.DatosContacto?.ComAutonomaContacto?.Nombre)
                || !string.IsNullOrEmpty(pDatos.DatosContacto?.CiudadContacto) || !string.IsNullOrEmpty(pDatos.DatosContacto?.CodigoPostalContacto)
                || !string.IsNullOrEmpty(pDatos.DatosContacto?.DireccionContacto))
            {
                string direccionContacto = string.IsNullOrEmpty(pDatos.DatosContacto?.DireccionContacto) ? "" : pDatos.DatosContacto.DireccionContacto;
                direccionContacto += string.IsNullOrEmpty(pDatos.DatosContacto?.CodigoPostalContacto) ? "" : ", " + pDatos.DatosContacto.CodigoPostalContacto;
                direccionContacto += string.IsNullOrEmpty(pDatos.DatosContacto?.CiudadContacto) ? "" : ", " + pDatos.DatosContacto.CiudadContacto;
                direccionContacto += string.IsNullOrEmpty(pDatos.DatosContacto?.ProvinciaContacto?.Nombre) ? "" : ", " + pDatos.DatosContacto.ProvinciaContacto.Nombre;
                direccionContacto += string.IsNullOrEmpty(pDatos.DatosContacto?.PaisContacto?.Nombre) ? "" : ", " + pDatos.DatosContacto.PaisContacto.Nombre;

                persona.Vcard_address = direccionContacto;
            }

            // Teléfonos.
            HashSet<string> telefonos = new HashSet<string>();
            if (pDatos.DatosContacto?.Telefonos != null && pDatos.DatosContacto.Telefonos.Any())
            {
                foreach (string item in pDatos.DatosContacto.Telefonos)
                {
                    telefonos.Add(item);
                }
            }
            if (pDatos.DatosContacto?.Moviles != null && pDatos.DatosContacto.Moviles.Any())
            {
                foreach (string item in pDatos.DatosContacto.Telefonos)
                {
                    telefonos.Add(item);
                }
            }
            persona.Vcard_hasTelephone = telefonos.ToList();

            // Activo.
            if (pDatos.Activo.HasValue)
            {
                persona.Roh_isActive = pDatos.Activo.Value;
            }

            // Departamentos.
            if (!string.IsNullOrEmpty(pDatos.Vinculacion?.Departamento?.Id) && !string.IsNullOrEmpty(pDatos.Vinculacion?.Departamento?.Nombre))
            {
                bool deptEncontrado = ComprobarDepartamentoBBDD(pDatos.Vinculacion?.Departamento?.Id);

                if (!deptEncontrado)
                {
                    // Si no existe, se carga el departamento como entidad secundaria.
                    CargarDepartment(pDatos.Vinculacion.Departamento.Id, pDatos.Vinculacion.Departamento.Nombre);
                }

                persona.IdVivo_departmentOrSchool = $@"{mResourceApi.GraphsUrl}items/department_{pDatos.Vinculacion.Departamento.Id}";
            }

            // Cargo en la universidad.
            if (!string.IsNullOrEmpty(pDatos.Vinculacion?.VinculacionCategoriaProfesional?.categoriaProfesional?.nombre))
            {
                persona.Roh_hasPosition = pDatos.Vinculacion?.VinculacionCategoriaProfesional?.categoriaProfesional?.nombre;
            }

            // Fecha de actualización.
            persona.Roh_lastUpdatedDate = DateTime.UtcNow;

            return persona;
        }

        /// <summary>
        /// Carga la entidad secundaria Department.
        /// </summary>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarDepartment(string pCodigoDept, string pNombreDept)
        {
            string ontology = "department";

            // Cambio de ontología.
            mResourceApi.ChangeOntoly(ontology);

            // Creación del objeto a cargar.
            Department dept = new Department();
            dept.Dc_identifier = pCodigoDept;
            dept.Dc_title = pNombreDept;

            // Carga.
            var cargado = mResourceApi.LoadSecondaryResource(dept.ToGnossApiResource(mResourceApi, ontology + "_" + dept.Dc_identifier));
        }

        /// <summary>
        /// Comprueba si existe el departamento en la BBDD.
        /// </summary>
        /// <param name="pIdentificadorDept">ID del departamento.</param>
        /// <returns></returns>
        private static bool ComprobarDepartamentoBBDD(string pIdentificadorDept)
        {
            string idSecundaria = $@"http://gnoss.com/items/department_{pIdentificadorDept}";

            SparqlObject resultadoQuery = null;

            // Consulta sparql.
            string select = "SELECT * ";
            string where = $@"WHERE {{ 
                                <{idSecundaria}> ?p ?o. 
                            }}";

            resultadoQuery = mResourceApi.VirtuosoQuery(select, where, "department");

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Devuelve el identificador del pais con el formato:
        /// mResourceApi.GraphsUrl + "items/feature_PCLD_" + ID_Pais
        /// </summary>
        /// <param name="pais"></param>
        /// <returns></returns>
        private static string IdentificadorPais(string pais)
        {
            if (!UtilidadesGeneral.DicPaisesContienePais(pais))
            {
                return null;
            }
            return mResourceApi.GraphsUrl + "items/feature_PCLD_" + UtilidadesGeneral.dicPaises[pais];
        }

        /// <summary>
        /// Devuelve el identificador de la región con el formato:
        /// mResourceApi.GraphsUrl + "items/feature_ADM1_" + ID_Region
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        private static string IdentificadorRegion(string region)
        {
            if (!UtilidadesGeneral.DicRegionesContieneRegion(region))
            {
                return null;
            }
            return mResourceApi.GraphsUrl + "items/feature_ADM1_" + UtilidadesGeneral.dicRegiones[region];
        }

        /// <summary>
        /// Devuelve el nombre de la empresa
        /// </summary>
        /// <param name="id">Identificador de la empresa</param>
        /// <returns></returns>
        private static string NombreEmpresa(string id)
        {
            // Obtención de datos en bruto.
            Empresa organization = new Empresa();
            string xmlResult = harvesterServices.GetRecord(id, _Config);

            if (string.IsNullOrEmpty(xmlResult))
            {
                return "";
            }

            XmlSerializer xmlSerializer = new(typeof(Empresa));
            using (StringReader sr = new(xmlResult))
            {
                organization = (Empresa)xmlSerializer.Deserialize(sr);
            }
            return organization.Nombre;
        }

        /// <summary>
        /// Crea un listado de AcademicdegreeOntology.AcademicDegree, perteneciente a recursos de posgrado
        /// </summary>
        /// <param name="pDatos"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <returns></returns>
        private static List<AcademicdegreeOntology.AcademicDegree> CrearPosgrado(Persona pDatos, Dictionary<string, Tuple<string, string>> dicOrganizaciones)
        {
            List<AcademicdegreeOntology.AcademicDegree> listaPosgrado = new List<AcademicdegreeOntology.AcademicDegree>();
            AcademicdegreeOntology.AcademicDegree academicDegree = new AcademicdegreeOntology.AcademicDegree();
            foreach (OAI_PMH.Models.SGI.FormacionAcademica.Posgrado posgrado in pDatos.Posgrado)
            {
                academicDegree.IdRoh_formationActivityType = FormationType(posgrado.TipoFormacionHomologada.Nombre);
                academicDegree.IdVcard_hasCountryName = IdentificadorPais(posgrado.PaisEntidadTitulacion.Id);
                academicDegree.IdVcard_hasRegion = IdentificadorRegion(posgrado.CcaaRegionEntidadTitulacion.Id);
                academicDegree.Vcard_locality = posgrado.CiudadEntidadTitulacion;
                academicDegree.Dct_issued = posgrado.FechaTitulacion;
                academicDegree.Roh_qualification = posgrado.CalificacionObtenida;
                academicDegree.Roh_approvedDegree = posgrado.TituloHomologado != null ? (bool)posgrado.TituloHomologado : false;
                academicDegree.Roh_approvedDate = posgrado.FechaHomologacion;
                //Titulacion posgrado
                academicDegree.Roh_title = posgrado.NombreTituloPosgrado;

                //Entidad Titulacion
                academicDegree.Roh_conductedByTitle = NombreEmpresa(posgrado.EntidadTitulacion.EntidadRef);
                academicDegree.IdRoh_conductedBy = dicOrganizaciones[posgrado.EntidadTitulacion.EntidadRef].Item1;
            }

            return listaPosgrado;
        }

        /// <summary>
        /// Tipo de formación
        /// </summary>
        /// <param name="tipoFormacion"></param>
        /// <returns></returns>
        private static string FormationType(string tipoFormacion)
        {
            string id = mResourceApi.GraphsUrl + "items/formationtype_";
            switch (tipoFormacion)
            {
                case "Master":
                    id += "034";
                    break;
                case "Posgrado":
                    id += "050";
                    break;
                case "Extensión universitaria":
                    id += "178";
                    break;
                case "Especialidad":
                    id += "179";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Premio
        /// </summary>
        /// <param name="premio"></param>
        /// <returns></returns>
        private static string PrizeType(string premio)
        {
            string id = mResourceApi.GraphsUrl + "items/prizetype_";
            switch (premio)
            {
                case "Premio extraordinario de licenciatura":
                    id += "000";
                    break;
                case "Premio fin de carrera":
                    id += "010";
                    break;
                case "Texto de otros":
                    id += "OTHERS";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Tipo de titulación
        /// </summary>
        /// <param name="tipoTitulacion"></param>
        /// <returns></returns>
        private static string UniversityDegreeType(string tipoTitulacion)
        {
            string id = mResourceApi.GraphsUrl + "items/universitydegreetype_";
            switch (tipoTitulacion)
            {
                case "Doctor":
                    id += "940";
                    break;
                case "Titulado medio":
                    id += "950";
                    break;
                case "Titulado superior":
                    id += "960";
                    break;
                case "Texto de otros":
                    id += "OTHERS";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Tipo nota media expediente
        /// </summary>
        /// <param name="qualificationType"></param>
        /// <returns></returns>
        private static string QualificationType(string qualificationType)
        {
            string id = mResourceApi.GraphsUrl + "items/qualificationtype_";
            switch (qualificationType)
            {
                case "Aprobado":
                    id += "000";
                    break;
                case "Notable":
                    id += "010";
                    break;
                case "Sobresaliente":
                    id += "020";
                    break;
                case "Matrícula de Honor":
                    id += "030";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Crea un listado de AcademicdegreeOntology.AcademicDegree, perteneciente a recursos de ciclos
        /// </summary>
        /// <param name="pDatos"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <returns></returns>
        private static List<AcademicdegreeOntology.AcademicDegree> CrearCiclos(Persona pDatos, Dictionary<string, Tuple<string, string>> dicOrganizaciones)
        {
            List<AcademicdegreeOntology.AcademicDegree> listaCiclos = new List<AcademicdegreeOntology.AcademicDegree>();
            AcademicdegreeOntology.AcademicDegree academicDegree = new AcademicdegreeOntology.AcademicDegree();
            foreach (OAI_PMH.Models.SGI.FormacionAcademica.Ciclos ciclo in pDatos.Ciclos)
            {
                academicDegree.IdVcard_hasCountryName = IdentificadorPais(ciclo.PaisEntidadTitulacion.Id);
                academicDegree.IdVcard_hasRegion = IdentificadorRegion(ciclo.CcaaRegionEntidadTitulacion.Id);
                academicDegree.Vcard_locality = ciclo.CiudadEntidadTitulacion;
                academicDegree.Dct_issued = ciclo.FechaTitulacion;
                //TODO academicDegree.IdRoh_mark = QualificationType(ciclo.NotaMediaExpediente);
                academicDegree.Roh_approvedDate = ciclo.FechaHomologacion;
                academicDegree.Roh_approvedDegree = ciclo.TituloHomologado != null ? (bool)ciclo.TituloHomologado : false;

                //Tipo titulacion
                //academicDegree.IdRoh_universityDegreeType = UniversityDegreeType(ciclo.tit);

                //Premio
                academicDegree.IdRoh_prize = PrizeType(ciclo.Premio.Id);

                //Titulacion
                academicDegree.Roh_title = ciclo.NombreTitulo;

                //Titulo Extranjero
                academicDegree.Roh_foreignTitle = ciclo.TituloExtranjero;

                //Entidad titulacion
                academicDegree.Roh_conductedByTitle = NombreEmpresa(ciclo.EntidadTitulacion.EntidadRef);
                academicDegree.IdRoh_conductedBy = dicOrganizaciones[ciclo.EntidadTitulacion.EntidadRef].Item1;

                listaCiclos.Add(academicDegree);
            }
            return listaCiclos;
        }

        /// <summary>
        /// Crea un listado de AcademicdegreeOntology.AcademicDegree, perteneciente a recursos de doctorados
        /// </summary>
        /// <param name="pDatos"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <param name="dicPersonas"></param>
        /// <returns></returns>
        private static List<AcademicdegreeOntology.AcademicDegree> CrearDoctorados(Persona pDatos, Dictionary<string, Tuple<string, string>> dicOrganizaciones,
            Dictionary<string, Tuple<string, string>> dicPersonas)
        {
            List<AcademicdegreeOntology.AcademicDegree> listadoDoctorados = new List<AcademicdegreeOntology.AcademicDegree>();
            AcademicdegreeOntology.AcademicDegree academicDegree = new AcademicdegreeOntology.AcademicDegree();
            foreach (OAI_PMH.Models.SGI.FormacionAcademica.Doctorados doctorado in pDatos.Doctorados)
            {
                academicDegree.Roh_deaDate = doctorado.FechaTitulacionDEA;
                academicDegree.IdVcard_hasCountryName = IdentificadorPais(doctorado.PaisEntidadTitulacion.Id);
                academicDegree.IdVcard_hasRegion = IdentificadorRegion(doctorado.CcaaRegionEntidadTitulacion.Id);
                academicDegree.Vcard_locality = doctorado.CiudadEntidadTitulacion;
                academicDegree.Dct_issued = doctorado.FechaTitulacion;
                academicDegree.Roh_thesisTitle = doctorado.TituloTesis;
                academicDegree.Roh_qualification = doctorado.CalificacionObtenida;
                academicDegree.Roh_qualityMention = doctorado.MencionCalidad != null ? (bool)doctorado.MencionCalidad : false;

                //Programa doctorado
                academicDegree.Roh_title = doctorado.ProgramaDoctorado;

                //Entidad titulacion
                academicDegree.Roh_conductedByTitle = NombreEmpresa(doctorado.EntidadTitulacion.EntidadRef);
                academicDegree.IdRoh_conductedBy = dicOrganizaciones[doctorado.EntidadTitulacion.EntidadRef].Item1;

                //Entidad titulacion DEA
                academicDegree.Roh_deaEntityTitle = NombreEmpresa(doctorado.EntidadTitulacionDEA.EntidadRef);
                academicDegree.IdRoh_deaEntity = dicOrganizaciones[doctorado.EntidadTitulacionDEA.EntidadRef].Item1;

                //Director tesis
                //TODO metodo cargar datos persona
                //academicDegree.Roh_directorName = dicPersonas[doctorado.DirectorTesis].Item1;

                //Codirectores
                //academicDegree.Roh_codirector.Add = dicPersonas[doctorado.CoDirectorTesis].Item1;

                //Doctorado UE
                academicDegree.Roh_europeanDoctorate = doctorado.DoctoradoEuropeo != null ? (bool)doctorado.DoctoradoEuropeo : false;
                academicDegree.Roh_europeanDoctorateDate = doctorado.FechaMencionDoctoradoEuropeo;

                //Premio extraordinario
                academicDegree.Roh_doctorExtraordinaryAward = doctorado.PremioExtraordinarioDoctor != null ? (bool)doctorado.PremioExtraordinarioDoctor : false;
                academicDegree.Roh_doctorExtraordinaryAwardDate = doctorado.FechaPremioExtraordinarioDoctor;

                //Titulo homologado
                academicDegree.Roh_approvedDegree = doctorado.TituloHomologado != null ? (bool)doctorado.TituloHomologado : false;
                academicDegree.Roh_approvedDate = doctorado.FechaHomologacion;

                listadoDoctorados.Add(academicDegree);
            }

            return listadoDoctorados;
        }

        /// <summary>
        /// Tipo de formación
        /// </summary>
        /// <param name="formationActivityType"></param>
        /// <returns></returns>
        private static string FormationActivityType(string formationActivityType)
        {
            string id = mResourceApi.GraphsUrl + "items/formationactivitytype_";
            switch (formationActivityType)
            {
                case "Curso":
                    id += "011";
                    break;
                case "Prácticas":
                    id += "051";
                    break;
                case "Estancias":
                    id += "184";
                    break;
                case "Texto de otros":
                    id += "OTHERS";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Crea un listado de AcademicdegreeOntology.AcademicDegree, perteneciente a recursos de formación especializada
        /// </summary>
        /// <param name="pDatos"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <param name="dicPersonas"></param>
        /// <returns></returns>
        private static List<AcademicdegreeOntology.AcademicDegree> CrearFormacionEspecializada(Persona pDatos, Dictionary<string, Tuple<string, string>> dicOrganizaciones,
           Dictionary<string, Tuple<string, string>> dicPersonas)
        {
            List<AcademicdegreeOntology.AcademicDegree> listaFormacionEspecializada = new List<AcademicdegreeOntology.AcademicDegree>();
            AcademicdegreeOntology.AcademicDegree academicDegree = new AcademicdegreeOntology.AcademicDegree();
            foreach (OAI_PMH.Models.SGI.FormacionAcademica.FormacionEspecializada formacionEspecializada in pDatos.FormacionEspecializada)
            {
                academicDegree.IdRoh_formationActivityType = FormationActivityType(formacionEspecializada.TipoFormacion.Nombre);
                academicDegree.Roh_title = formacionEspecializada.NombreTitulo;
                academicDegree.IdVcard_hasCountryName = IdentificadorPais(formacionEspecializada.PaisEntidadTitulacion.Id);
                academicDegree.IdVcard_hasRegion = IdentificadorRegion(formacionEspecializada.CcaaRegionEntidadTitulacion.Id);
                academicDegree.Vcard_locality = formacionEspecializada.CiudadEntidadTitulacion;
                academicDegree.Roh_goals = formacionEspecializada.Objetivos;
                academicDegree.Roh_durationHours = formacionEspecializada.DuracionTitulacion;
                academicDegree.Vivo_end = formacionEspecializada.FechaTitulacion;

                //Entidad titulacion
                academicDegree.Roh_conductedByTitle = NombreEmpresa(formacionEspecializada.EntidadTitulacion.EntidadRef);
                academicDegree.IdRoh_conductedBy = dicOrganizaciones[formacionEspecializada.EntidadTitulacion.EntidadRef].Item1;

                //Responsable
                //academicDegree.Roh_trainerNick = formacionEspecializada.ResponsableFormacion;

                listaFormacionEspecializada.Add(academicDegree);
            }
            return listaFormacionEspecializada;
        }

        /// <summary>
        /// Tipo de trabajo dirigido
        /// </summary>
        /// <param name="projectCharacterType"></param>
        /// <returns></returns>
        private static string ProjectCharacterType(string projectCharacterType)
        {
            string id = mResourceApi.GraphsUrl + "items/projectcharactertype_";
            switch (projectCharacterType)
            {
                case "Proyecto de fin de carrera":
                    id += "055";
                    break;
                case "Tesina":
                    id += "066";
                    break;
                case "Tesis Doctoral":
                    id += "067";
                    break;
                case "Trabajo conducente a la obtención de DEA":
                    id += "071";
                    break;
                case "Texto de otros":
                    id += "OTHERS";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Crea un listado de ThesissupervisionOntology.ThesisSupervision, perteneciente a recursos de tesis
        /// </summary>
        /// <param name="pDatos"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <param name="dicPersonas"></param>
        /// <returns></returns>
        private static List<ThesissupervisionOntology.ThesisSupervision> CrearTesis(Persona pDatos, Dictionary<string, Tuple<string, string>> dicOrganizaciones,
           Dictionary<string, Tuple<string, string>> dicPersonas)
        {
            List<ThesissupervisionOntology.ThesisSupervision> listaTesis = new List<ThesissupervisionOntology.ThesisSupervision>();
            ThesissupervisionOntology.ThesisSupervision thesisSupervision = new ThesissupervisionOntology.ThesisSupervision();
            foreach (OAI_PMH.Models.SGI.ActividadDocente.Tesis tesis in pDatos.Tesis)
            {
                thesisSupervision.IdRoh_projectCharacterType = ProjectCharacterType(tesis.TipoProyecto.Nombre);
                thesisSupervision.Roh_title = tesis.TituloTrabajo;
                thesisSupervision.IdVcard_hasCountryName = IdentificadorPais(tesis.PaisEntidadRealizacion.Id);
                thesisSupervision.IdVcard_hasRegion = IdentificadorRegion(tesis.CcaaRegionEntidadRealizacion.Id);
                thesisSupervision.Vcard_locality = tesis.CiudadEntidadRealizacion;
                thesisSupervision.Dct_issued = tesis.FechaDefensa;
                thesisSupervision.Roh_qualification = tesis.CalificacionObtenida;
                thesisSupervision.Roh_europeanDoctorateDate = tesis.FechaMencionDoctoradoEuropeo;
                thesisSupervision.Roh_qualityMention = tesis.MencionCalidad != null ? (bool)tesis.MencionCalidad : false;
                thesisSupervision.Roh_europeanDoctorate = tesis.DoctoradoEuropeo != null ? (bool)tesis.DoctoradoEuropeo : false;
                thesisSupervision.Roh_qualityMentionDate = tesis.FechaMencionCalidad;

                //Palabras clave
                //TODO ?

                //Entidad realizacion
                thesisSupervision.IdRoh_promotedBy = dicOrganizaciones[tesis.EntidadRealizacion.EntidadRef].Item1;
                thesisSupervision.Roh_promotedByTitle = NombreEmpresa(tesis.EntidadRealizacion.EntidadRef);

                //Alumno TODO - check
                Persona alumno = ObtenerPersona(tesis.Alumno);
                thesisSupervision.Roh_studentName = alumno.Nombre;
                thesisSupervision.Roh_studentFirstSurname = alumno.Apellidos;
                thesisSupervision.Roh_studentNick = alumno.Nombre + " " + alumno.Apellidos;

                //Codirectores
                //thesisSupervision.cod = dicPersonas[tesis.CoDirectorTesis.PersonaRef].Item1;

                listaTesis.Add(thesisSupervision);
            }
            return listaTesis;
        }

        /// <summary>
        /// Tipo de labor docente
        /// </summary>
        /// <param name="teachingtype"></param>
        /// <returns></returns>
        private static string TeachingType(string teachingtype)
        {
            string id = mResourceApi.GraphsUrl + "items/teachingtype_";
            switch (teachingtype)
            {
                case "Docencia internacional":
                    id += "014";
                    break;
                case "Docencia oficial":
                    id += "015";
                    break;
                case "Docencia no oficial":
                    id += "016";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Tipo de trabajo de programa.
        /// </summary>
        /// <param name="programType"></param>
        /// <returns></returns>
        private static string ProgramType(string programType)
        {
            string id = mResourceApi.GraphsUrl + "items/programtype_";
            switch (programType)
            {
                case "Arquitectura":
                    id += "020";
                    break;
                case "Arquitectura técnica":
                    id += "030";
                    break;
                case "Diplomatura":
                    id += "240";
                    break;
                case "Doctorado":
                    id += "250";
                    break;
                case "Ingeniería":
                    id += "420";
                    break;
                case "Ingeniería Técnica":
                    id += "430";
                    break;
                case "Licenciatura":
                    id += "470";
                    break;
                case "Máster oficial":
                    id += "480";
                    break;
                case "Texto de otros":
                    id += "OTHERS";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Tipo de docencia
        /// </summary>
        /// <param name="modalityTeachingType"></param>
        /// <returns></returns>
        private static string ModalityTeachingType(string modalityTeachingType)
        {
            string id = mResourceApi.GraphsUrl + "items/modalityteachingtype_";
            switch (modalityTeachingType)
            {
                case "Clínico":
                    id += "060";
                    break;
                case "Prácticas de Laboratorio":
                    id += "700";
                    break;
                case "Práctica (Aula-Problemas)":
                    id += "705";
                    break;
                case "Teórica presencial":
                    id += "840";
                    break;
                case "Virtual":
                    id += "860";
                    break;
                case "Texto de otros":
                    id += "OTHERS";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Tipo de asignatura
        /// </summary>
        /// <param name="courseType"></param>
        /// <returns></returns>
        private static string CourseType(string courseType)
        {
            string id = mResourceApi.GraphsUrl + "items/coursetype_";
            switch (courseType)
            {
                case "Troncal":
                    id += "000";
                    break;
                case "Optativa":
                    id += "020";
                    break;
                case "Obligatoria":
                    id += "010";
                    break;
                case "Libre configuración":
                    id += "030";
                    break;
                case "Doctorado/a":
                    id += "050";
                    break;
                case "Texto de otros":
                    id += "OTHERS";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// hourscreditsectstype_" + hoursCreditsECTSType
        /// </summary>
        /// <param name="hoursCreditsECTSType"></param>
        /// <returns></returns>
        private static string HoursCreditsECTSType(string hoursCreditsECTSType)
        {
            string id = mResourceApi.GraphsUrl + "items/hourscreditsectstype_";
            switch (hoursCreditsECTSType)
            {
                case "Creditos":
                    id += "000";
                    break;
                case "No competitivo":
                    id += "010";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// language_" + language
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private static string Language(string language)
        {
            return mResourceApi.GraphsUrl + "items/language_" + language;
        }

        /// <summary>
        /// Tipo de convocatoria
        /// </summary>
        /// <param name="courseType"></param>
        /// <returns></returns>
        private static string CallType(string callType)
        {
            string id = mResourceApi.GraphsUrl + "items/calltype_";
            switch (callType)
            {
                case "Competitivo":
                    id += "1040";
                    break;
                case "No competitivo":
                    id += "1050";
                    break;
                case "Texto de otros":
                    id += "OTHERS";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Tipo del ámbito geográfico
        /// </summary>
        /// <param name="geographicRegion"></param>
        /// <returns></returns>
        private static string GeographicRegion(string geographicRegion)
        {
            string id = mResourceApi.GraphsUrl + "items/geographicregion_";
            switch (geographicRegion)
            {
                case "Autonómica":
                    id += "000";
                    break;
                case "Internacional no UE":
                    id += "030";
                    break;
                case "Nacional":
                    id += "010";
                    break;
                case "Unión Europea":
                    id += "020";
                    break;
                case "Texto de otros":
                    id += "OTHERS";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Crea un listado de ImpartedacademictrainingOntology.ImpartedAcademicTraining, perteneciente a recursos de formacion academica impartida
        /// </summary>
        /// <param name="pDatos"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <param name="dicPersonas"></param>
        /// <returns></returns>
        private static List<ImpartedacademictrainingOntology.ImpartedAcademicTraining> CrearFormacionAcademicaImpartida(Persona pDatos, Dictionary<string, Tuple<string, string>> dicOrganizaciones,
           Dictionary<string, Tuple<string, string>> dicPersonas)
        {
            List<ImpartedacademictrainingOntology.ImpartedAcademicTraining> listaFormacionAcademicaImpartida = new List<ImpartedacademictrainingOntology.ImpartedAcademicTraining>();
            ImpartedacademictrainingOntology.ImpartedAcademicTraining academicTraining = new ImpartedacademictrainingOntology.ImpartedAcademicTraining();
            foreach (OAI_PMH.Models.SGI.ActividadDocente.FormacionAcademicaImpartida formacionAcademica in pDatos.FormacionAcademicaImpartida)
            {
                academicTraining.IdRoh_teachingType = TeachingType(formacionAcademica.TipoDocente.Nombre);
                academicTraining.IdVcard_hasCountryName = IdentificadorPais(formacionAcademica.PaisEntidadRealizacion.Id);
                academicTraining.IdVcard_hasRegion = IdentificadorRegion(formacionAcademica.CcaaRegionEntidadRealizacion.Id);
                academicTraining.Vcard_locality = formacionAcademica.CiudadEntidadRealizacion;
                academicTraining.Roh_department = formacionAcademica.Departamento;
                academicTraining.IdRoh_programType = ProgramType(formacionAcademica.TipoPrograma.Nombre);
                academicTraining.Roh_teaches = formacionAcademica.NombreAsignaturaCurso;
                academicTraining.IdRoh_modalityTeachingType = ModalityTeachingType(formacionAcademica.TipoDocencia.Nombre);
                academicTraining.IdRoh_courseType = CourseType(formacionAcademica.TipoAsignatura.Nombre);
                academicTraining.Roh_course = formacionAcademica.Curso;
                academicTraining.IdRoh_hoursCreditsECTSType = HoursCreditsECTSType(formacionAcademica.TipoHorasCreditos.Nombre);
                academicTraining.Roh_numberECTSHours = formacionAcademica.NumHorasCreditos != null ? (float?)formacionAcademica.NumHorasCreditos : null;
                academicTraining.IdVcard_hasLanguage = Language(formacionAcademica.Idioma);
                academicTraining.Roh_frequency = (float?)formacionAcademica.FrecuenciaActividad;
                academicTraining.Roh_competencies = formacionAcademica.Competencias;
                academicTraining.Roh_professionalCategory = formacionAcademica.CategoriaProfesional;
                //academicTraining.Roh_qualification = formacionAcademica.calificacion;
                //academicTraining.Roh_maxQualification = formacionAcademica.maxcalificacion;
                //academicTraining.IdRoh_evaluatedByHasCountryName = IdentificadorPais(formacionAcademica.EntidadEvaluacion.pais.id);
                //academicTraining.evaluatedregion = IdentificadorRegion(formacionAcademica.EntidadEvaluacion.region.id);
                //academicTraining.Roh_evaluatedByLocality = formacionAcademica.EntidadEvaluacion.ciudad;
                //academicTraining.IdRoh_evaluationType = EvaluationType(formacionAcademica.TipoEvaluacion) ; 
                //academicTraining.IdRoh_financedByHasCountryName = IdentificadorPais(formacionAcademica.EntidadFinanciadora);
                //academicTraining.IdRoh_financedByHasRegion = IdentificadorRegion(formacionAcademica.EntidadFinanciadora);
                //academicTraining.Roh_financedByLocality = formacionAcademica.EntidadFinanciadora.ciudad;
                //academicTraining.IdRoh_callType = CallType(formacionAcademica.TipoConvocatoria);
                //academicTraining.IdVivo_geographicFocus = GeographicRegion(formacionAcademica.AmbitoGeografico);
                //academicTraining.Roh_center = formacionAcademica.facultad;
                //academicTraining.Vivo_start = formacionAcademica.FechaInicio;
                //academicTraining.Vivo_end = formacionAcademica.FechaFinalizacion;

                //Titulacion universitaria
                academicTraining.Roh_title = formacionAcademica.TitulacionUniversitaria;

                //Entidad realizacion
                academicTraining.IdRoh_promotedBy = dicOrganizaciones[formacionAcademica.EntidadRealizacion.EntidadRef].Item1;
                academicTraining.Roh_promotedByTitle = NombreEmpresa(formacionAcademica.EntidadRealizacion.EntidadRef);

                //Entidad financiadora
                //academicTraining.IdRoh_financedBy = dicOrganizaciones[formacionAcademica.EntidadFinanciadora.EntidadRef].Item1;
                //academicTraining.Roh_financedByTitle =  NombreEmpresa(formacionAcademica.EntidadFinanciadora);

                //Entidad evaluacion
                //academicTraining.IdRoh_evaluatedBy = dicOrganizaciones[formacionAcademica.EntidadEvaluacion.EntidadRef].Item1;
                //academicTraining.Roh_evaluatedByTitle =  NombreEmpresa(formacionAcademica.EntidadEvaluacion.EntidadRef);

                listaFormacionAcademicaImpartida.Add(academicTraining);
            }
            return listaFormacionAcademicaImpartida;
        }

        /// <summary>
        /// Crea un listado de ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars, perteneciente a recursos de seminarios, cursos
        /// </summary>
        /// <param name="pDatos"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <param name="dicPersonas"></param>
        /// <returns></returns>
        private static List<ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars> CrearSeminariosCursos(Persona pDatos, Dictionary<string, Tuple<string, string>> dicOrganizaciones,
           Dictionary<string, Tuple<string, string>> dicPersonas)
        {
            List<ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars> listaSeminariosCursos = new List<ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars>();
            ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars impartedCourses = new ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars();
            foreach (OAI_PMH.Models.SGI.ActividadDocente.SeminariosCursos seminariosCursos in pDatos.SeminariosCursos)
            {
                //TODO impartedCourses.Roh_eventType = EventType(semianrioscursos.tipoevento);
                impartedCourses.Roh_title = seminariosCursos.NombreEvento;
                //impartedCourses.IdVcard_hasCountryName = IdentificadorPais(seminariosCursos.paisentidadorganizadora);
                //impartedCourses.IdVcard_hasRegion = IdentificadorRegion(seminariosCursos.region);
                impartedCourses.Vcard_locality = seminariosCursos.CiudadEntidadOrganizacionEvento;
                impartedCourses.Roh_goals = seminariosCursos.ObjetivosCurso;
                impartedCourses.Roh_targetProfile = seminariosCursos.PerfilDestinatarios;
                impartedCourses.IdVcard_hasLanguage = seminariosCursos.Idioma;
                impartedCourses.Vivo_start = seminariosCursos.FechaTitulacion;
                //impartedCourses.Roh_durationHours = seminariosCursos.horasimpartidas;
                impartedCourses.IdRoh_participationType = seminariosCursos.TipoParticipacion.Nombre;
                impartedCourses.Roh_correspondingAuthor = seminariosCursos.AutorCorrespondencia != null ? (bool)seminariosCursos.AutorCorrespondencia : false;

                //Entidad orgnaizadora
                impartedCourses.Roh_promotedByTitle = NombreEmpresa(seminariosCursos.EntidadOrganizacionEvento.EntidadRef);
                impartedCourses.IdRoh_promotedBy = dicOrganizaciones[seminariosCursos.EntidadOrganizacionEvento.EntidadRef].Item1;

                //ISBN
                impartedCourses.Roh_isbn = seminariosCursos.ISBN;

                //ISSN
                impartedCourses.Bibo_issn = seminariosCursos.ISSN;

                //IDPublicacion
                //impartedCourses.Bibo_handle = seminariosCursos.IdentificadoresPublicacion.
                //impartedCourses.Bibo_doi = ;
                //impartedCourses.Bibo_pmid = ;


                listaSeminariosCursos.Add(impartedCourses);
            }
            return listaSeminariosCursos;
        }

        /// <summary>
        /// Crea un listado de AccreditationOntology.Accreditation, perteneciente a recursos de sexenios
        /// </summary>
        /// <param name="pDatos"></param>
        /// <returns></returns>
        private static List<AccreditationOntology.Accreditation> CrearSexenios(Persona pDatos)
        {
            List<AccreditationOntology.Accreditation> listaSexenios = new List<AccreditationOntology.Accreditation>();
            if (pDatos.Sexenios != null)
            {
                AccreditationOntology.Accreditation sexenio = new AccreditationOntology.Accreditation();
                sexenio.Roh_recognizedPeriods = Convert.ToInt32(pDatos.Sexenios.Numero);
                sexenio.IdVcard_hasCountryName = IdentificadorPais(pDatos.Sexenios.PaisRef);

                listaSexenios.Add(sexenio);
            }

            return listaSexenios;
        }

        /// <summary>
        /// Crea un OrganizationOntology.Organization a partir de los datos de <paramref name="pDatos"/>
        /// </summary>
        /// <param name="pDatos"></param>
        /// <returns></returns>
        public static OrganizationOntology.Organization CrearOrganizacionOntology(Empresa pDatos)
        {
            OrganizationOntology.Organization organization = new OrganizationOntology.Organization();
            organization.Roh_crisIdentifier = pDatos.Id;
            organization.Roh_title = pDatos.Nombre;
            organization.Vcard_locality = pDatos.DatosContacto?.Direccion;
            return organization;
        }

        //TODO
        private static OrganizationOntology.Organization CrearEntidadGestora(string entidadGestoraID)
        {
            OrganizationOntology.Organization organization = new OrganizationOntology.Organization();
            Empresa empresa = new Empresa();
            string emp = harvesterServices.GetRecord("Organizacion_" + entidadGestoraID, _Config);
            XmlSerializer xmlSerializer = new(typeof(Empresa));
            using (StringReader sr = new(emp))
            {
                empresa = (Empresa)xmlSerializer.Deserialize(sr);
            }
            organization.Roh_crisIdentifier = entidadGestoraID;
            organization.Roh_title = empresa.Nombre;
            organization.Vcard_locality = empresa.DatosContacto?.Direccion;

            //TODO insertar

            return organization;
        }

        /// <summary>
        /// Obtiene la OrganizacionAux.
        /// </summary>
        /// <param name="entidadConvocanteID"></param>
        /// <param name="pDicOrganizaciones"></param>
        /// <returns></returns>
        private static ProjectOntology.OrganizationAux CrearEntidadOrganizationAux(string entidadConvocanteID)
        {
            string idGnossOrganizacion = string.Empty;
            string nombreOrganizacion = string.Empty;
            string localidadOrganizacion = string.Empty;

            Dictionary<string, string> dicDatosBBDD = GetDataBBDD(new List<string>() { entidadConvocanteID }, "organization");

            // Se comprueba en el diccionario de organizaciones que no la tengamos previamente cargada.            
            if (dicDatosBBDD.ContainsKey(entidadConvocanteID))
            {
                OrganizacionBBDD organizacionBBDD = GetOrganizacionBBDD(dicDatosBBDD[entidadConvocanteID]);

                // Asignación.
                idGnossOrganizacion = dicDatosBBDD[entidadConvocanteID];
                nombreOrganizacion = organizacionBBDD.title;
                localidadOrganizacion = organizacionBBDD.locality;
            }
            else
            {
                // Petición al SGI.
                string emp = harvesterServices.GetRecord("Organizacion_" + entidadConvocanteID, _Config);

                if (!string.IsNullOrEmpty(emp))
                {
                    OrganizationOntology.Organization organization = new OrganizationOntology.Organization();
                    Empresa empresa = new Empresa();
                    XmlSerializer xmlSerializer = new(typeof(Empresa));
                    using (StringReader sr = new(emp))
                    {
                        empresa = (Empresa)xmlSerializer.Deserialize(sr);
                    }
                    organization.Roh_crisIdentifier = entidadConvocanteID;
                    organization.Roh_title = empresa.Nombre;
                    organization.Vcard_locality = empresa.DatosContacto?.Direccion;

                    // Carga.
                    mResourceApi.ChangeOntoly("organization");
                    ComplexOntologyResource resource = organization.ToGnossApiResource(mResourceApi, null);
                    mResourceApi.LoadComplexSemanticResource(resource, false, false);

                    // Asignación.
                    idGnossOrganizacion = resource.GnossId;
                    nombreOrganizacion = empresa.Nombre;
                    localidadOrganizacion = empresa.DatosContacto?.Direccion;
                }
            }

            ProjectOntology.OrganizationAux organizationAux = new ProjectOntology.OrganizationAux();
            organizationAux.IdRoh_organization = idGnossOrganizacion;
            organizationAux.Roh_organizationTitle = nombreOrganizacion;
            organizationAux.Vcard_locality = localidadOrganizacion;

            return organizationAux;
        }

        /// <summary>
        /// Obtiene la Organizacion.
        /// </summary>
        /// <param name="entidadConvocanteID"></param>
        /// <param name="pDicOrganizaciones"></param>
        /// <returns></returns>
        private static Dictionary<string, OrganizacionBBDD> CrearEntidadOrganization(string entidadConvocanteID)
        {
            Dictionary<string, OrganizacionBBDD> dicResultado = new Dictionary<string, OrganizacionBBDD>();

            Dictionary<string, string> dicDatosBBDD = GetDataBBDD(new List<string>() { entidadConvocanteID }, "organization");

            // Se comprueba en el diccionario de organizaciones que no la tengamos previamente cargada.            
            if (dicDatosBBDD.ContainsKey(entidadConvocanteID))
            {
                OrganizacionBBDD organizacionBBDD = GetOrganizacionBBDD(dicDatosBBDD[entidadConvocanteID]);
                dicResultado.Add(dicDatosBBDD[entidadConvocanteID], organizacionBBDD);
            }
            else
            {
                // Petición al SGI.
                string emp = harvesterServices.GetRecord("Organizacion_" + entidadConvocanteID, _Config);

                if (!string.IsNullOrEmpty(emp))
                {
                    OrganizationOntology.Organization organization = new OrganizationOntology.Organization();
                    Empresa empresa = new Empresa();
                    XmlSerializer xmlSerializer = new(typeof(Empresa));
                    using (StringReader sr = new(emp))
                    {
                        empresa = (Empresa)xmlSerializer.Deserialize(sr);
                    }
                    organization.Roh_crisIdentifier = entidadConvocanteID;
                    organization.Roh_title = empresa.Nombre;
                    organization.Vcard_locality = empresa.DatosContacto?.Direccion;

                    // Carga.
                    mResourceApi.ChangeOntoly("organization");
                    ComplexOntologyResource resource = organization.ToGnossApiResource(mResourceApi, null);
                    mResourceApi.LoadComplexSemanticResource(resource, false, false);

                    // Asignación.
                    OrganizacionBBDD organizacionBBDD = new OrganizacionBBDD();
                    organizacionBBDD.crisIdentifier = entidadConvocanteID;
                    organizacionBBDD.title = empresa.Nombre;
                    organizacionBBDD.locality = empresa.DatosContacto?.Direccion;

                    dicResultado.Add(resource.GnossId, organizacionBBDD);
                }
            }

            return dicResultado;
        }

        //TODO
        private static string CrearEntidadFinanciadora(string entidadFinanciadoraID)
        {
            OrganizationOntology.Organization organization = new OrganizationOntology.Organization();
            Empresa empresa = new Empresa();
            string emp = harvesterServices.GetRecord("Organizacion_" + entidadFinanciadoraID, _Config);
            XmlSerializer xmlSerializer = new(typeof(Empresa));
            using (StringReader sr = new(emp))
            {
                empresa = (Empresa)xmlSerializer.Deserialize(sr);
            }
            organization.Roh_crisIdentifier = entidadFinanciadoraID;
            organization.Roh_title = empresa.Nombre;
            organization.Vcard_locality = empresa.DatosContacto?.Direccion;

            //TODO insertar

            ProjectOntology.OrganizationAux organizationAux = new ProjectOntology.OrganizationAux();
            organizationAux.Roh_organization = organization;
            //comprobar o cambiar por identificador al añadir
            //organizationAux.IdRoh_organization = organization.GNOSSID;
            organizationAux.Roh_organizationTitle = empresa.Nombre;
            organizationAux.Vcard_locality = empresa.DatosContacto?.Direccion;
            //TODO insertar

            return organizationAux.GNOSSID;//TODO asignar si no se autoasigna
        }

        private static PatentOntology.Patent CrearInvencionesOntology(Invencion pInvencion, HashSet<string> pListaPersonas)
        {
            PatentOntology.Patent patente = new PatentOntology.Patent();

            // CrisIdentifier
            patente.Roh_crisIdentifier = pInvencion.id.ToString();

            // Título
            patente.Roh_title = pInvencion.titulo;

            // Fecha
            patente.Dct_issued = pInvencion.fechaComunicacion;

            // Descripción
            patente.Roh_qualityDescription = pInvencion.descripcion;

            // Autores
            List<PatentOntology.PersonAux> listaPersonas = new List<PatentOntology.PersonAux>();
            foreach (Inventor inventor in pInvencion.inventores)
            {
                // Obtención de personas de BBDD con los IDs obtenidos por el SGI.
                Dictionary<string, string> dicPersonasBBDD = GetDataBBDD(new List<string>() { inventor.inventorRef }, "person");

                PatentOntology.PersonAux persona = new PatentOntology.PersonAux();
                if (dicPersonasBBDD.ContainsKey(inventor.inventorRef))
                {
                    persona.IdRdf_member = dicPersonasBBDD[inventor.inventorRef];

                    // Agregar a la lista de IDs
                    pListaPersonas.Add(dicPersonasBBDD[inventor.inventorRef]);
                }
                else
                {
                    PersonOntology.Person personOntology = new PersonOntology.Person();

                    // Se piden los datos de la persona.
                    Persona person = ObtenerPersona(inventor.inventorRef);

                    // TODO: Desambiguación de Personas

                    // Si la persona no tiene nombre, no se inserta.
                    if (!string.IsNullOrEmpty(person.Nombre))
                    {
                        personOntology = CrearPersona(person);
                        mResourceApi.ChangeOntoly("person");
                        ComplexOntologyResource resource = personOntology.ToGnossApiResource(mResourceApi, null);
                        mResourceApi.LoadComplexSemanticResource(resource, false, false);
                        dicPersonasBBDD[personOntology.Roh_crisIdentifier] = resource.GnossId;
                        persona.IdRdf_member = dicPersonasBBDD[inventor.inventorRef];

                        // Agregar a la lista de IDs
                        pListaPersonas.Add(resource.GnossId);
                    }
                }

                listaPersonas.Add(persona);
            }

            patente.Bibo_authorList = listaPersonas;

            return patente;
        }

        /// <summary>
        /// Crea un ProjectAuthorization a partir de los datos de <paramref name="pAutorizacionProyecto"/>
        /// </summary>
        /// <param name="pAutorizacionProyecto"></param>
        /// <returns></returns>
        public static ProjectAuthorization CrearAutorizacion(Autorizacion pAutorizacionProyecto)
        {
            ProjectAuthorization autorizacion = new ProjectAuthorization();
            autorizacion.Roh_crisIdentifier = pAutorizacionProyecto.entidadRef;
            autorizacion.Roh_title = pAutorizacionProyecto.tituloProyecto;
            autorizacion.IdRoh_owner = GetPersonGnossId(pAutorizacionProyecto.solicitanteRef);

            // TODO: En teoría el OWNER nunca debería de ser vacío. En el caso de que lo fuera...
            // ¿Hay que cargar la persona?

            if (!string.IsNullOrEmpty(autorizacion.Roh_crisIdentifier) && !string.IsNullOrEmpty(autorizacion.Roh_title)
                && !string.IsNullOrEmpty(autorizacion.IdRoh_owner))
            {
                return autorizacion;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Crea un GroupOntology.Group a partir de los datos de <paramref name="grupo"/>
        /// </summary>
        /// <param name="grupo"></param>
        /// <returns></returns>
        public static GroupOntology.Group CrearGrupo(Grupo grupo, HashSet<string> pListaPersonas)
        {
            GroupOntology.Group groupOntology = new GroupOntology.Group();

            // Crisidentifier.
            groupOntology.Roh_crisIdentifier = grupo.id.ToString();

            // Nombre del grupo.
            groupOntology.Roh_title = grupo.nombre;

            // Código interno del grupo.
            groupOntology.Roh_normalizedCode = grupo.codigo;

            // Fecha de inicio del grupo.
            groupOntology.Roh_foundationDate = grupo.fechaInicio;

            // Duración.
            if (grupo.fechaInicio != null && grupo.fechaFin != null)
            {
                Tuple<string, string, string> duracion = RestarFechas((DateTime)grupo.fechaInicio, (DateTime)grupo.fechaFin);
                groupOntology.Roh_durationYears = duracion.Item1;
                groupOntology.Roh_durationMonths = duracion.Item2;
                groupOntology.Roh_durationDays = duracion.Item3;
            }

            List<GroupOntology.BFO_0000023> listaPersonas = new List<GroupOntology.BFO_0000023>();
            foreach (GrupoEquipo grupoEquipo in grupo.equipo)
            {
                // Obtención de personas de BBDD con los IDs obtenidos por el SGI.
                Dictionary<string, string> dicPersonasBBDD = GetDataBBDD(new List<string>() { grupoEquipo.personaRef }, "person");

                GroupOntology.BFO_0000023 persona = new GroupOntology.BFO_0000023();
                if (dicPersonasBBDD.ContainsKey(grupoEquipo.personaRef))
                {
                    // TODO: Mario revisar.

                    persona.IdRoh_roleOf = dicPersonasBBDD[grupoEquipo.personaRef];
                    List<GroupOntology.ResearchArea> hasResearchArea = new List<GroupOntology.ResearchArea>();
                    string select = "SELECT ?end ?start ?hasResearchArea ?title ?startRA ?endRA ?isIP";
                    string where = $@"WHERE {{
                                    ?s <http://w3id.org/roh/roleOf> <{dicPersonasBBDD[grupoEquipo.personaRef]}>.
                                    OPTIONAL {{ ?s <http://vivoweb.org/ontology/core#end> ?end. }}
                                    OPTIONAL {{ ?s <http://vivoweb.org/ontology/core#start> ?start. }}
                                    OPTIONAL {{ ?s <http://vivoweb.org/ontology/core#hasResearchArea> ?hasResearchArea. }}
                                    OPTIONAL {{ ?hasResearchArea <http://w3id.org/roh/title> ?title. }}
                                    OPTIONAL {{ ?hasResearchArea <http://vivoweb.org/ontology/core#start> ?startRA. }}
                                    OPTIONAL {{ ?hasResearchArea <http://vivoweb.org/ontology/core#end> ?endRA. }}
                                    OPTIONAL {{ ?s <http://w3id.org/roh/isIP> ?isIP. }}
                                   }}";
                    SparqlObject resultadoQuery = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "person", "project" });
                    if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                    {
                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                        {
                            if (fila.ContainsKey("end"))
                            {
                                persona.Vivo_end = DateTime.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "end"));
                            }
                            if (fila.ContainsKey("start"))
                            {
                                persona.Vivo_start = DateTime.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "start"));
                            }
                            if (fila.ContainsKey("hasResearchArea"))
                            {
                                GroupOntology.ResearchArea researchArea = new GroupOntology.ResearchArea();
                                if (fila.ContainsKey("title"))
                                {
                                    researchArea.Roh_title = fila["title"].value;
                                }
                                if (fila.ContainsKey("startRA"))
                                {
                                    researchArea.Vivo_start = DateTime.Parse(fila["startRA"].value);
                                }
                                if (fila.ContainsKey("endRA"))
                                {
                                    researchArea.Vivo_end = DateTime.Parse(fila["endRA"].value);
                                }
                                hasResearchArea.Add(researchArea);
                            }
                            if (fila.ContainsKey("isIP"))
                            {
                                persona.Roh_isIP = bool.Parse(UtilidadesAPI.GetValorFilaSparqlObject(fila, "isIP"));
                            }
                        }
                    }
                    persona.Vivo_hasResearchArea = hasResearchArea;

                    // Agregar a la lista de IDs
                    pListaPersonas.Add(dicPersonasBBDD[grupoEquipo.personaRef]);
                }
                else
                {
                    // Se piden los datos de la persona.
                    Persona person = ObtenerPersona(grupoEquipo.personaRef);

                    // TODO: Desambiguación de Personas

                    // Si la persona no tiene nombre, no se inserta.
                    if (!string.IsNullOrEmpty(person.Nombre))
                    {
                        PersonOntology.Person personOntology = new PersonOntology.Person();
                        personOntology = CrearPersona(person);

                        // Carga de la persona.
                        mResourceApi.ChangeOntoly("person");
                        ComplexOntologyResource resource = personOntology.ToGnossApiResource(mResourceApi, null);
                        mResourceApi.LoadComplexSemanticResource(resource, false, false);

                        dicPersonasBBDD[personOntology.Roh_crisIdentifier] = resource.GnossId;
                        persona.IdRoh_roleOf = dicPersonasBBDD[grupoEquipo.personaRef];

                        // Agregar a la lista de IDs
                        pListaPersonas.Add(resource.GnossId);
                    }
                }

                // IP principal
                persona.Roh_isIP = grupoEquipo.rol.abreviatura == "IP";

                // Fecha de inicio de la persona en el grupo.
                if (!string.IsNullOrEmpty(grupoEquipo.fechaInicio))
                {
                    persona.Vivo_start = Convert.ToDateTime(grupoEquipo.fechaInicio);
                }

                // Fecha de fin de la persona en el grupo.
                if (!string.IsNullOrEmpty(grupoEquipo.fechaFin))
                {
                    persona.Vivo_end = Convert.ToDateTime(grupoEquipo.fechaFin);
                }

                listaPersonas.Add(persona);
            }

            // Relates.
            groupOntology.Vivo_relates = listaPersonas;

            return groupOntology;
        }

        /// <summary>
        /// Devuelve el gnossId pasado el crisIdentifier de una persona.
        /// </summary>
        /// <param name="pCrisIdentifier"></param>
        /// <returns></returns>
        public static string GetPersonGnossId(string pCrisIdentifier)
        {
            SparqlObject resultadoQuery = null;
            StringBuilder select = new StringBuilder(), where = new StringBuilder();

            // Consulta sparql.
            select.Append("SELECT ?s ");
            where.Append("WHERE { ");
            where.Append($@"?s <http://w3id.org/roh/crisIdentifier> '{pCrisIdentifier}'. ");
            where.Append("} ");

            resultadoQuery = mResourceApi.VirtuosoQuery(select.ToString(), where.ToString(), "person");

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    return fila["s"].value;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Obtiene los datos de la persona del SGI 
        /// </summary>
        /// <param name="personaRef"></param>
        /// <returns></returns>
        private static Persona ObtenerPersona(string personaRef)
        {
            Persona persona = new Persona();
            string person = harvesterServices.GetRecord("Persona_" + personaRef, _Config);
            XmlSerializer xmlSerializer = new(typeof(Persona));
            using (StringReader sr = new(person))
            {
                persona = (Persona)xmlSerializer.Deserialize(sr);
            }
            return persona;
        }

        /// <summary>
        /// Devuelve el tipo de proyecto de <paramref name="project"/>
        /// </summary>
        /// <param name="project"></param>
        /// <param name="pDatos"></param>
        private static void TipoProyecto(ProjectOntology.Project project, Proyecto pDatos)
        {
            if (string.IsNullOrEmpty(project.IdRoh_scientificExperienceProject))
            {
                switch (pDatos.ClasificacionCVN)
                {
                    case "COMPETITIVOS":
                        project.IdRoh_scientificExperienceProject = mResourceApi.GraphsUrl + "items/scientificexperienceproject_SEP1";
                        break;
                    case "NO_COMPETITIVOS":
                        project.IdRoh_scientificExperienceProject = mResourceApi.GraphsUrl + "items/scientificexperienceproject_SEP2";
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Devuelve el ámbito del proyecto de <paramref name="project"/>
        /// </summary>
        /// <param name="project"></param>
        /// <param name="pDatos"></param>
        private static void AmbitoGeograficoProyecto(ProjectOntology.Project project, Proyecto pDatos)
        {
            if (string.IsNullOrEmpty(project.IdRoh_scientificExperienceProject) && pDatos.AmbitoGeografico != null && !string.IsNullOrEmpty(pDatos.AmbitoGeografico.Nombre))
            {
                switch (pDatos.AmbitoGeografico.Nombre.ToLower())
                {
                    case "autonómica":
                        project.IdVivo_geographicFocus = mResourceApi.GraphsUrl + "items/geographicregion_000";
                        break;
                    case "autonómico":
                        project.IdVivo_geographicFocus = mResourceApi.GraphsUrl + "items/geographicregion_000";
                        break;
                    case "nacional":
                        project.IdVivo_geographicFocus = mResourceApi.GraphsUrl + "items/geographicregion_010";
                        break;
                    case "unión europea":
                        project.IdVivo_geographicFocus = mResourceApi.GraphsUrl + "items/geographicregion_020";
                        break;
                    case "europeo":
                        project.IdVivo_geographicFocus = mResourceApi.GraphsUrl + "items/geographicregion_020";
                        break;
                    case "internacional no ue":
                        project.IdVivo_geographicFocus = mResourceApi.GraphsUrl + "items/geographicregion_030";
                        break;
                    case "internacional no europeo":
                        project.IdVivo_geographicFocus = mResourceApi.GraphsUrl + "items/geographicregion_030";
                        break;
                    default:
                        project.IdVivo_geographicFocus = mResourceApi.GraphsUrl + "items/geographicregion_OTHERS";
                        project.Roh_geographicFocusOther = pDatos.AmbitoGeografico.Nombre;
                        break;
                }
            }
        }

        /// <summary>
        /// Obtiene los años, meses y dias entre la fecha de inicio y de fin
        /// </summary>
        /// <param name="fechaInicio"></param>
        /// <param name="fechaFin"></param>
        /// <returns></returns>
        public static Tuple<string, string, string> RestarFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            int total = (fechaFin - fechaInicio).Days;
            int anios = total / 365;
            int meses = (total - (365 * anios)) / 30;
            int dias = total - (365 * anios) - (30 * meses);
            return new Tuple<string, string, string>(anios.ToString(), meses.ToString(), dias.ToString());
        }

        private static string ObtenerProyecto(string crisIdentifier)
        {
            string select = "select ?project ";
            string where = $@"where{{
                                ?project <http://w3id.org/roh/crisIdentifier> '{crisIdentifier}' .
                            }}";
            SparqlObject sparqlObject = mResourceApi.VirtuosoQuery(select, where, "project");
            var data = sparqlObject.results.bindings;

            foreach (Dictionary<string, SparqlObject.Data> keyValue in data)
            {
                if (keyValue.ContainsKey("project"))
                {
                    return keyValue["project"].value;
                }
            }


            return "";
        }

        /// <summary>
        /// Crea un ProjectOntology.Project a partir de los datos de <paramref name="pDatos"/>, <paramref name="dicPersonas"/>, <paramref name="dicOrganizaciones"/>.
        /// </summary>
        /// <param name="pDatos"></param>
        /// <param name="dicPersonas"></param>
        /// <param name="dicOrganizaciones"></param>
        /// <returns></returns>
        public static ProjectOntology.Project CrearProyecto(Proyecto pDatos, HashSet<string> pListaPersonas)
        {
            ProjectOntology.Project project = new ProjectOntology.Project();

            // Crisidentifier
            project.Roh_crisIdentifier = pDatos.Id.ToString();

            // IsValidated
            project.Roh_isValidated = true;

            // ValidationStatusProject
            project.Roh_validationStatusProject = "validado";

            // Tipo de proyecto (COMPETITIVOS, NO_COMPETITIVOS, AYUDAS)
            TipoProyecto(project, pDatos);

            // Se añade el tipo de proyecto en caso de ser no competitivo.
            // 875 - Coordinación, 876 - Cooperación
            if (project.IdRoh_scientificExperienceProject != null && project.IdRoh_scientificExperienceProject.Equals(mResourceApi.GraphsUrl + "items/scientificexperienceproject_SEP2")
                && pDatos.CoordinadorExterno != null)
            {
                string projectType = mResourceApi.GraphsUrl + "items/projecttype_";
                project.IdRoh_projectType = (bool)pDatos.CoordinadorExterno ? projectType + "875" : projectType + "876";
            }

            // Título
            if (!string.IsNullOrEmpty(pDatos.Titulo))
            {
                project.Roh_title = pDatos.Titulo;
            }

            // Observaciones
            if (!string.IsNullOrEmpty(pDatos.Observaciones))
            {
                project.Vivo_description = pDatos.Observaciones;
            }

            // Equipos
            if (pDatos.Equipo != null && pDatos.Equipo.Any())
            {
                project.Vivo_relates = new List<ProjectOntology.BFO_0000023>();
                int orden = 1;
                foreach (ProyectoEquipo item in pDatos.Equipo)
                {
                    // Obtención de personas de BBDD con los IDs obtenidos por el SGI.
                    Dictionary<string, string> dicPersonasBBDD = GetDataBBDD(new List<string>() { item.PersonaRef }, "person");

                    ProjectOntology.BFO_0000023 BFO = new ProjectOntology.BFO_0000023();
                    BFO.Rdf_comment = orden;

                    if (dicPersonasBBDD.ContainsKey(item.PersonaRef))
                    {
                        BFO.IdRoh_roleOf = dicPersonasBBDD[item.PersonaRef];

                        // Agregar a la lista de IDs
                        pListaPersonas.Add(dicPersonasBBDD[item.PersonaRef]);
                    }
                    else
                    {
                        // Se piden los datos de la persona.
                        Persona persona = ObtenerPersona(item.PersonaRef);

                        // TODO: Desambiguación de Personas

                        // Si la persona no tiene nombre, no se inserta.
                        if (!string.IsNullOrEmpty(persona.Nombre))
                        {
                            PersonOntology.Person personOntology = CrearPersona(persona);

                            mResourceApi.ChangeOntoly("person");
                            ComplexOntologyResource resource = personOntology.ToGnossApiResource(mResourceApi, null);
                            mResourceApi.LoadComplexSemanticResource(resource, false, false);

                            BFO.IdRoh_roleOf = resource.GnossId;

                            // Agregar a la lista de IDs
                            pListaPersonas.Add(resource.GnossId);
                        }
                    }

                    // IP principal
                    BFO.Roh_isIP = item.RolProyecto.RolPrincipal.HasValue;
                    if (!string.IsNullOrEmpty(item.FechaInicio))
                    {
                        BFO.Vivo_start = Convert.ToDateTime(item.FechaInicio);
                    }
                    if (!string.IsNullOrEmpty(item.FechaFin))
                    {
                        BFO.Vivo_end = Convert.ToDateTime(item.FechaFin);
                    }
                    project.Vivo_relates.Add(BFO);
                    orden++;
                }
            }

            // Añado las entidades financiadoras que no existan en BBDD
            if (pDatos.EntidadesFinanciadoras != null && pDatos.EntidadesFinanciadoras.Any())
            {
                project.Roh_grantedBy = new List<ProjectOntology.OrganizationAux>();

                foreach (ProyectoEntidadFinanciadora entidadFinanciadora in pDatos.EntidadesFinanciadoras)
                {
                    project.Roh_grantedBy.Add(CrearEntidadOrganizationAux(entidadFinanciadora.EntidadRef));
                }
            }

            // Añado las entidades gestoras que no existan en BBDD. Se coge la primera porque en la ontología es 0..1
            if (pDatos.EntidadesGestoras != null && pDatos.EntidadesGestoras.Any())
            {
                project.Roh_participates = new List<ProjectOntology.OrganizationAux>();

                foreach (ProyectoEntidadGestora entidadGestora in pDatos.EntidadesGestoras)
                {
                    project.Roh_participates.Add(CrearEntidadOrganizationAux(entidadGestora.EntidadRef));
                }
            }

            // Se añade las entidades participación que no existan en BBDD.
            if (pDatos.EntidadesConvocantes != null && pDatos.EntidadesConvocantes.Any())
            {
                ProyectoEntidadConvocante entidadConvocante = pDatos.EntidadesConvocantes[0];

                Dictionary<string, OrganizacionBBDD> organizacion = CrearEntidadOrganization(entidadConvocante.EntidadRef);

                // Solamente contrendrá un elemento.
                foreach (KeyValuePair<string, OrganizacionBBDD> item in organizacion)
                {
                    project.Roh_conductedByTitle = item.Value.title;
                    project.IdRoh_conductedBy = item.Key;
                }
            }

            // Número de investigadores. TODO: ¿Actuales?
            project.Roh_researchersNumber = pDatos.Equipo.Select(x => x.PersonaRef).GroupBy(x => x).Count();

            // Porcentajes
            double porcentajeSubvencion = 0;
            double porcentajeCredito = 0;
            double porcentajeMixto = 0;

            foreach (ProyectoEntidadFinanciadora entidadFinanciadora in pDatos.EntidadesFinanciadoras)
            {
                if (entidadFinanciadora.TipoFinanciacion != null && entidadFinanciadora.PorcentajeFinanciacion != null)
                {
                    if (entidadFinanciadora.TipoFinanciacion.Nombre.Equals("Subvención"))
                    {
                        porcentajeSubvencion += (double)entidadFinanciadora.PorcentajeFinanciacion;
                    }
                    else if (entidadFinanciadora.TipoFinanciacion.Nombre.Equals("Préstamo"))
                    {
                        porcentajeCredito += (double)entidadFinanciadora.PorcentajeFinanciacion;
                    }
                    else if (entidadFinanciadora.TipoFinanciacion.Nombre.Equals("Mixto"))
                    {
                        porcentajeMixto += (double)entidadFinanciadora.PorcentajeFinanciacion;
                    }
                }
            }

            project.Roh_grantsPercentage = (float)porcentajeSubvencion;
            project.Roh_creditPercentage = (float)porcentajeCredito;
            project.Roh_mixedPercentage = (float)porcentajeMixto;

            // Cuantía Total
            double cuantiaTotal = pDatos.TotalImporteConcedido != null ? (double)pDatos.TotalImporteConcedido : 0;

            if (cuantiaTotal == 0)
            {
                foreach (ProyectoAnualidadResumen anualidadResumen in pDatos.ResumenAnualidades)
                {
                    if (anualidadResumen.Presupuestar != null && (bool)anualidadResumen.Presupuestar)
                    {
                        cuantiaTotal += anualidadResumen.TotalGastosConcedido;
                    }
                }
                project.Roh_monetaryAmount = (float)cuantiaTotal;
            }

            // Fecha Inicio.
            project.Vivo_start = pDatos.FechaInicio != null ? Convert.ToDateTime(pDatos.FechaInicio) : null;

            // Fecha Fin. (Nos quuedamos con la fecha de fin definitiva si existe)
            project.Vivo_end = pDatos.FechaFin != null ? Convert.ToDateTime(pDatos.FechaFin) : null;
            project.Vivo_end = pDatos.FechaFinDefinitiva != null ? Convert.ToDateTime(pDatos.FechaFinDefinitiva) : Convert.ToDateTime(pDatos.FechaFin);

            if (pDatos.FechaFinDefinitiva != null)
            {
                if (pDatos.FechaInicio != null)
                {
                    Tuple<string, string, string> duration = RestarFechas(Convert.ToDateTime(pDatos.FechaInicio), Convert.ToDateTime(pDatos.FechaFinDefinitiva));
                    project.Roh_durationYears = duration.Item1;
                    project.Roh_durationMonths = duration.Item2;
                    project.Roh_durationDays = duration.Item3;
                }
            }
            else
            {
                if (pDatos.FechaInicio != null)
                {
                    Tuple<string, string, string> duration = RestarFechas(Convert.ToDateTime(pDatos.FechaInicio), Convert.ToDateTime(pDatos.FechaFin));
                    project.Roh_durationYears = duration.Item1;
                    project.Roh_durationMonths = duration.Item2;
                    project.Roh_durationDays = duration.Item3;
                }
            }

            project.Roh_relevantResults = pDatos.Contexto?.ResultadosPrevistos;
            project.Roh_projectCode = pDatos.CodigoExterno;

            // Ámbito geográfico
            AmbitoGeograficoProyecto(project, pDatos);

            return project;
        }

        public bool ComprobarID(string pIdMalFormado)
        {
            // Si el ID es distinto a 8 dígitos, está mal formado.
            if (pIdMalFormado.Count() != 8)
            {
                return true;
            }

            // Si contiene '@' o '|' está mal formado.
            if (pIdMalFormado.Contains("@") || pIdMalFormado.Contains("|"))
            {
                return true;
            }

            // Si el NIE está mal formado, es inválido.
            if (!Int32.TryParse(pIdMalFormado.Substring(1), out int result))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Obtiene los datos de la organización a raíz del ID GNOSS.
        /// </summary>
        /// <param name="pIdGnoss"></param>
        /// <returns></returns>
        public static OrganizacionBBDD GetOrganizacionBBDD(string pIdGnoss)
        {
            SparqlObject resultadoQuery = null;
            StringBuilder select = new StringBuilder(), where = new StringBuilder();

            // Consulta sparql.
            select.Append("SELECT ?crisIdentifier ?titulo ?localidad ");
            where.Append("WHERE { ");
            where.Append($@"<{pIdGnoss}> <http://w3id.org/roh/crisIdentifier> ?crisIdentifier. ");
            where.Append($@"<{pIdGnoss}> <http://w3id.org/roh/title> ?titulo. ");
            where.Append($@"OPTIONAL{{<{pIdGnoss}> <https://www.w3.org/2006/vcard/ns#locality> ?localidad. }}");
            where.Append("} ");

            resultadoQuery = mResourceApi.VirtuosoQuery(select.ToString(), where.ToString(), "organization");

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    OrganizacionBBDD organizacion = new OrganizacionBBDD();
                    if (!string.IsNullOrEmpty(fila["crisIdentifier"].value))
                    {
                        organizacion.crisIdentifier = fila["crisIdentifier"].value;
                    }
                    if (!string.IsNullOrEmpty(fila["titulo"].value))
                    {
                        organizacion.title = fila["titulo"].value;
                    }
                    if (fila.ContainsKey("localidad") && !string.IsNullOrEmpty(fila["localidad"].value))
                    {
                        organizacion.locality = fila["localidad"].value;
                    }
                    return organizacion;
                }
            }

            return null;
        }
    }
}