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

        private static string RUTA_PREFIJOS = $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Utilidades/prefijos.json";
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

                        // Sleep.
                        Thread.Sleep((time.Value.UtcDateTime - DateTimeOffset.UtcNow));
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

            int i = 0;
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
                    i++;
                    Console.WriteLine($"Procesando fichero de {pSet} {i}/{idsACargar.Count}");

                    switch (pSet)
                    {
                        #region - Organizacion
                        case "Organizacion":
                            Empresa empresa = null;
                            try
                            {
                                empresa = Empresa.GetOrganizacionSGI(harvesterServices, _Config, id, pDicRutas);
                            }
                            catch
                            {
                                // ID no válido.
                                break;
                            }

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
                            Persona persona = null;
                            try
                            {
                                persona = Persona.GetPersonaSGI(harvesterServices, _Config, id, pDicRutas);
                            }
                            catch
                            {
                                // ID no válido.
                                break;
                            }
                            if (persona != null && !string.IsNullOrEmpty(persona.Nombre))
                            {
                                string idGnossPersona = persona.Cargar(harvesterServices, pConfig, mResourceApi, "person", pDicIdentificadores, pDicRutas, pRabbitConf, true);
                                pDicIdentificadores["person"].Add(idGnossPersona);
                            }
                            File.AppendAllText(pDicRutas[pSet][directorioPendientes], id + Environment.NewLine);
                            break;

                        #endregion

                        #region - Proyecto
                        case "Proyecto":
                            Proyecto proyectoSGI = null;
                            try
                            {
                                proyectoSGI = Proyecto.GetProyectoSGI(harvesterServices, _Config, id, pDicRutas);
                            }
                            catch
                            {
                                // ID no válido.
                                break;
                            }
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
                            Autorizacion autorizacionSGI = null;
                            try
                            {
                                autorizacionSGI = Autorizacion.GetAutorizacionSGI(harvesterServices, _Config, id, pDicRutas);
                            }
                            catch
                            {
                                // ID no válido.
                                break;
                            }
                            if (autorizacionSGI != null && !string.IsNullOrEmpty(autorizacionSGI.tituloProyecto) && !string.IsNullOrEmpty(autorizacionSGI.solicitanteRef) && !string.IsNullOrEmpty(autorizacionSGI.entidadRef))
                            {
                                string idGnossAutorizacion = autorizacionSGI.Cargar(harvesterServices, pConfig, mResourceApi, "projectauthorization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["projectauthorization"].Add(idGnossAutorizacion);
                            }
                            break;
                        #endregion

                        #region - Invencion
                        case "Invencion":
                            Invencion invencion = null;
                            try
                            {
                                invencion = Invencion.GetInvencionSGI(harvesterServices, _Config, id, pDicRutas);
                            }
                            catch
                            {
                                // ID no válido.
                                break;
                            }
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
                            Grupo grupo = null;
                            try
                            {
                                grupo = Grupo.GetGrupoSGI(harvesterServices, _Config, id, pDicRutas);
                            }
                            catch
                            {
                                // ID no válido.
                                break;
                            }
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
    }
}