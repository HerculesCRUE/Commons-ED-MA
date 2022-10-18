using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Harvester;
using Harvester.Models.ModelsBBDD;
using Harvester.Models.RabbitMQ;
using OAI_PMH.Models.SGI.Organization;
using OAI_PMH.Models.SGI.PersonalData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace OAI_PMH.Models.SGI.Project
{
    /// <summary>
    /// Proyecto
    /// </summary>
    public class Proyecto : SGI_Base
    {
        /// <summary>
        /// Crea el objeto ComplexOntologyResource para ser cargado.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <param name="pRabbitConf"></param>
        /// <param name="pFusionarPersona"></param>
        /// <param name="pIdPersona"></param>
        /// <returns></returns>
        public override ComplexOntologyResource ToRecurso(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, bool pFusionarPersona = false, string pIdPersona = null)
        {
            ProjectOntology.Project proyecto = CrearProjectOntology(pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas, pRabbitConf);
            pResourceApi.ChangeOntoly("project");
            return proyecto.ToGnossApiResource(pResourceApi, null);
        }

        /// <summary>
        /// Obtiene los IDs de BBDD mediante el crisidentifier.
        /// </summary>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public override string ObtenerIDBBDD(ResourceApi pResourceApi)
        {
            Dictionary<string, string> respuesta = ObtenerProyectoBBDD(new HashSet<string>() { Id.ToString() }, pResourceApi);
            if (respuesta.ContainsKey(Id.ToString()) && !string.IsNullOrEmpty(respuesta[Id.ToString()]))
            {
                return respuesta[Id.ToString()];
            }
            return null;
        }

        public override void ToRecursoAdicional(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, string pIdGnoss)
        {
            // No es necesario en esta clase.
        }

        /// <summary>
        /// Crea el objeto proyecto para ser cargado.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <param name="pRabbitConf"></param>
        /// <returns></returns>
        public ProjectOntology.Project CrearProjectOntology(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf)
        {
            #region --- Obtenemos los IDs de las personas del proyecto.
            HashSet<string> listaIdsPersonas = new HashSet<string>();
            if (this.Equipo != null && this.Equipo.Any())
            {
                foreach (ProyectoEquipo item in this.Equipo)
                {
                    // Obtención de personas de BBDD con los IDs obtenidos por el SGI.
                    listaIdsPersonas.Add(item.PersonaRef);
                }
            }
            #endregion

            #region --- Obtenemos los IDs de las personas en BBDD.
            Dictionary<string, string> dicPersonasBBDD = Persona.ObtenerPersonasBBDD(listaIdsPersonas, pResourceApi);
            #endregion

            #region --- Obtención de personas
            Dictionary<string, string> dicPersonasCargadas = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in dicPersonasBBDD)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    Persona personaAux = Persona.GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + item.Key, pDicRutas);
                    if (personaAux != null)
                    {
                        string idGnoss = personaAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "person", pDicIdentificadores, pDicRutas, pRabbitConf, true);
                        pDicIdentificadores["person"].Add(idGnoss);
                        dicPersonasCargadas[item.Key] = idGnoss;
                    }
                }
                else
                {
                    dicPersonasCargadas[item.Key] = item.Value;
                }
            }
            #endregion

            #region --- Obtenemos los IDs de las organizaciones del proyecto.
            HashSet<string> listaIdsOrganizaciones = new HashSet<string>();
            if (this.EntidadesFinanciadoras != null && this.EntidadesFinanciadoras.Any())
            {
                foreach (ProyectoEntidadFinanciadora entidadFinanciadora in this.EntidadesFinanciadoras)
                {
                    // Obtención de personas de BBDD con los IDs obtenidos por el SGI.
                    listaIdsOrganizaciones.Add(entidadFinanciadora.EntidadRef);
                }
            }
            if (this.EntidadesGestoras != null && this.EntidadesGestoras.Any())
            {
                foreach (ProyectoEntidadGestora entidadGestora in this.EntidadesGestoras)
                {
                    // Obtención de personas de BBDD con los IDs obtenidos por el SGI.
                    listaIdsOrganizaciones.Add(entidadGestora.EntidadRef);
                }
            }
            if (this.EntidadesConvocantes != null && this.EntidadesConvocantes.Any())
            {
                foreach (ProyectoEntidadConvocante entidadConvocante in this.EntidadesConvocantes)
                {
                    // Obtención de personas de BBDD con los IDs obtenidos por el SGI.
                    listaIdsOrganizaciones.Add(entidadConvocante.EntidadRef);
                }
            }
            #endregion

            #region --- Obtenemos los IDs de las organizaciones en BBDD.
            Dictionary<string, string> dicOrganizaciones = Empresa.ObtenerOrganizacionesBBDD(listaIdsOrganizaciones, pResourceApi);
            #endregion

            #region --- Obtención de organizaciones
            Dictionary<string, string> dicOrganizacionesCargadas = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> item in dicOrganizaciones)
            {
                if (string.IsNullOrEmpty(item.Value))
                {
                    Empresa organizacionAux = Empresa.GetOrganizacionSGI(pHarvesterServices, pConfig, "Organizacion_" + item.Key, pDicRutas);
                    string idGnoss = organizacionAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                    pDicIdentificadores["organization"].Add(idGnoss);
                    dicOrganizacionesCargadas[item.Key] = idGnoss;
                }
                else
                {
                    dicOrganizacionesCargadas[item.Key] = item.Value;
                }
            }
            #endregion

            #region --- Construimos el objeto de 'Project'
            ProjectOntology.Project project = new ProjectOntology.Project();

            // Crisidentifier
            project.Roh_crisIdentifier = this.Id.ToString();

            // IsValidated
            project.Roh_isValidated = true;

            // ValidationStatusProject
            project.Roh_validationStatusProject = "validado";

            // Tipo de proyecto (COMPETITIVOS o NO_COMPETITIVOS)
            if (string.IsNullOrEmpty(project.IdRoh_scientificExperienceProject))
            {
                switch (this.ClasificacionCVN)
                {
                    case "COMPETITIVOS":
                        project.IdRoh_scientificExperienceProject = pResourceApi.GraphsUrl + "items/scientificexperienceproject_SEP1";
                        break;
                    case "NO_COMPETITIVOS":
                        project.IdRoh_scientificExperienceProject = pResourceApi.GraphsUrl + "items/scientificexperienceproject_SEP2";
                        break;
                    default:
                        break;
                }
            }

            // Se añade el tipo de proyecto en caso de ser no competitivo. (875 - Coordinación, 876 - Cooperación)
            if (project.IdRoh_scientificExperienceProject != null && project.IdRoh_scientificExperienceProject.Equals(pResourceApi.GraphsUrl + "items/scientificexperienceproject_SEP2")
                && this.CoordinadorExterno != null)
            {
                string projectType = pResourceApi.GraphsUrl + "items/projecttype_";
                project.IdRoh_projectType = (bool)this.CoordinadorExterno ? projectType + "875" : projectType + "876";
            }

            // Título
            if (!string.IsNullOrEmpty(this.Titulo))
            {
                project.Roh_title = this.Titulo;
            }

            // Observaciones
            if (!string.IsNullOrEmpty(this.Observaciones))
            {
                project.Vivo_description = this.Observaciones;
            }

            // Equipos
            if (this.Equipo != null && this.Equipo.Any())
            {
                project.Vivo_relates = new List<ProjectOntology.BFO_0000023>();
                int orden = 1;
                foreach (ProyectoEquipo item in this.Equipo)
                {
                    if (dicPersonasBBDD.ContainsKey(item.PersonaRef))
                    {
                        ProjectOntology.BFO_0000023 BFO = new ProjectOntology.BFO_0000023();
                        BFO.Rdf_comment = orden;
                        BFO.IdRoh_roleOf = dicPersonasBBDD[item.PersonaRef];

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
            }

            // Añado las entidades financiadoras que no existan en BBDD
            if (this.EntidadesFinanciadoras != null && this.EntidadesFinanciadoras.Any())
            {
                project.Roh_grantedBy = new List<ProjectOntology.OrganizationAux>();

                foreach (ProyectoEntidadFinanciadora entidadFinanciadora in this.EntidadesFinanciadoras)
                {
                    project.Roh_grantedBy.Add(CrearEntidadOrganizationAux(dicOrganizacionesCargadas[entidadFinanciadora.EntidadRef], pResourceApi));
                }
            }

            // Añado las entidades gestoras que no existan en BBDD. Se coge la primera porque en la ontología es 0..1
            if (this.EntidadesGestoras != null && this.EntidadesGestoras.Any())
            {
                project.Roh_participates = new List<ProjectOntology.OrganizationAux>();

                foreach (ProyectoEntidadGestora entidadGestora in this.EntidadesGestoras)
                {
                    project.Roh_participates.Add(CrearEntidadOrganizationAux(dicOrganizacionesCargadas[entidadGestora.EntidadRef], pResourceApi));
                }
            }

            // Se añade las entidades participación que no existan en BBDD.
            if (this.EntidadesConvocantes != null && this.EntidadesConvocantes.Any())
            {
                ProyectoEntidadConvocante entidadConvocante = this.EntidadesConvocantes[0];
                OrganizacionBBDD empresa = Empresa.GetOrganizacionBBDD(pHarvesterServices, pConfig, pResourceApi, dicOrganizaciones[entidadConvocante.EntidadRef]);
                if (empresa != null)
                {
                    project.Roh_conductedByTitle = empresa.title;
                    project.IdRoh_conductedBy = dicOrganizaciones[entidadConvocante.EntidadRef];
                }
            }

            // Número de investigadores. TODO: ¿Actuales?
            project.Roh_researchersNumber = this.Equipo.Select(x => x.PersonaRef).GroupBy(x => x).Count();

            // Porcentajes
            double porcentajeSubvencion = 0;
            double porcentajeCredito = 0;
            double porcentajeMixto = 0;

            foreach (ProyectoEntidadFinanciadora entidadFinanciadora in this.EntidadesFinanciadoras)
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
            double cuantiaTotal = this.TotalImporteConcedido != null ? (double)this.TotalImporteConcedido : 0;

            if (cuantiaTotal == 0)
            {
                foreach (ProyectoAnualidadResumen anualidadResumen in this.ResumenAnualidades)
                {
                    if (anualidadResumen.Presupuestar != null && (bool)anualidadResumen.Presupuestar)
                    {
                        cuantiaTotal += anualidadResumen.TotalGastosConcedido;
                    }
                }
                project.Roh_monetaryAmount = (float)cuantiaTotal;
            }

            // Fecha Inicio.
            project.Vivo_start = this.FechaInicio != null ? Convert.ToDateTime(this.FechaInicio) : null;

            // Fecha Fin. (Nos quuedamos con la fecha de fin definitiva si existe)
            project.Vivo_end = this.FechaFin != null ? Convert.ToDateTime(this.FechaFin) : null;
            project.Vivo_end = this.FechaFinDefinitiva != null ? Convert.ToDateTime(this.FechaFinDefinitiva) : Convert.ToDateTime(this.FechaFin);

            if (this.FechaFinDefinitiva != null)
            {
                if (this.FechaInicio != null)
                {
                    Tuple<string, string, string> duration = RestarFechas(Convert.ToDateTime(this.FechaInicio), Convert.ToDateTime(this.FechaFinDefinitiva));
                    project.Roh_durationYears = duration.Item1;
                    project.Roh_durationMonths = duration.Item2;
                    project.Roh_durationDays = duration.Item3;
                }
            }
            else
            {
                if (this.FechaInicio != null)
                {
                    Tuple<string, string, string> duration = RestarFechas(Convert.ToDateTime(this.FechaInicio), Convert.ToDateTime(this.FechaFin));
                    project.Roh_durationYears = duration.Item1;
                    project.Roh_durationMonths = duration.Item2;
                    project.Roh_durationDays = duration.Item3;
                }
            }

            project.Roh_relevantResults = this.Contexto?.ResultadosPrevistos;
            project.Roh_projectCode = this.CodigoExterno;

            // Ámbito geográfico
            if (string.IsNullOrEmpty(project.IdRoh_scientificExperienceProject) && this.AmbitoGeografico != null && !string.IsNullOrEmpty(this.AmbitoGeografico.Nombre))
            {
                switch (this.AmbitoGeografico.Nombre.ToLower())
                {
                    case "autonómica":
                        project.IdVivo_geographicFocus = pResourceApi.GraphsUrl + "items/geographicregion_000";
                        break;
                    case "autonómico":
                        project.IdVivo_geographicFocus = pResourceApi.GraphsUrl + "items/geographicregion_000";
                        break;
                    case "nacional":
                        project.IdVivo_geographicFocus = pResourceApi.GraphsUrl + "items/geographicregion_010";
                        break;
                    case "unión europea":
                        project.IdVivo_geographicFocus = pResourceApi.GraphsUrl + "items/geographicregion_020";
                        break;
                    case "europeo":
                        project.IdVivo_geographicFocus = pResourceApi.GraphsUrl + "items/geographicregion_020";
                        break;
                    case "internacional no ue":
                        project.IdVivo_geographicFocus = pResourceApi.GraphsUrl + "items/geographicregion_030";
                        break;
                    case "internacional no europeo":
                        project.IdVivo_geographicFocus = pResourceApi.GraphsUrl + "items/geographicregion_030";
                        break;
                    default:
                        project.IdVivo_geographicFocus = pResourceApi.GraphsUrl + "items/geographicregion_OTHERS";
                        project.Roh_geographicFocusOther = this.AmbitoGeografico.Nombre;
                        break;
                }
            }
            #endregion

            return project;
        }

        /// <summary>
        /// Obtiene los dato de los proyectos de SGI.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pId"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public static Proyecto GetProyectoSGI(IHarvesterServices pHarvesterServices, ReadConfig pConfig, string pId, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            // Obtención de datos en bruto.
            Proyecto proyecto = new Proyecto();
            string xmlResult = pHarvesterServices.GetRecord(pId, pConfig);

            if (string.IsNullOrEmpty(xmlResult))
            {
                return null;
            }

            XmlSerializer xmlSerializer = new(typeof(Proyecto));
            using (StringReader sr = new(xmlResult))
            {
                proyecto = (Proyecto)xmlSerializer.Deserialize(sr);
            }

            return proyecto;
        }

        /// <summary>
        /// Obtiene los proyectos de BBDD mediante el crisidentifiers.
        /// </summary>
        /// <param name="pListaIds"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ObtenerProyectoBBDD(HashSet<string> pListaIds, ResourceApi pResourceApi)
        {
            List<List<string>> listaProyecto = SplitList(pListaIds.ToList(), 1000).ToList();
            Dictionary<string, string> dicProyectosBBDD = new Dictionary<string, string>();
            foreach (string organizacion in pListaIds)
            {
                if (organizacion.Contains("_"))
                {
                    dicProyectosBBDD[organizacion.Split("_")[1]] = "";
                }
                else
                {
                    dicProyectosBBDD[organizacion] = "";
                }
            }
            foreach (List<string> listaItem in listaProyecto)
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
                string selectPerson = $@"SELECT DISTINCT ?s ?crisIdentifier ";
                string wherePerson = $@"WHERE {{ 
                            ?s <http://w3id.org/roh/crisIdentifier> ?crisIdentifier. 
                            FILTER(?crisIdentifier in ('{string.Join("', '", listaAux.Select(x => x))}')) }}";
                SparqlObject resultadoQueryPerson = pResourceApi.VirtuosoQuery(selectPerson, wherePerson, "project");
                if (resultadoQueryPerson != null && resultadoQueryPerson.results != null && resultadoQueryPerson.results.bindings != null && resultadoQueryPerson.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPerson.results.bindings)
                    {
                        dicProyectosBBDD[fila["crisIdentifier"].value] = fila["s"].value;
                    }
                }
            }

            return dicProyectosBBDD;
        }

        /// <summary>
        /// Crea el objeto OrganizationAux para ser cargado.
        /// </summary>
        /// <param name="pGnossId"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static ProjectOntology.OrganizationAux CrearEntidadOrganizationAux(string pGnossId, ResourceApi pResourceApi)
        {
            OrganizacionBBDD organizacionBBDD = GetOrganizacionBBDD(pGnossId, pResourceApi);

            // Asignación.
            ProjectOntology.OrganizationAux organizationAux = new ProjectOntology.OrganizationAux();
            organizationAux.IdRoh_organization = pGnossId;
            organizationAux.Roh_organizationTitle = organizacionBBDD.title;
            organizationAux.Vcard_locality = organizacionBBDD.locality;

            return organizationAux;
        }

        /// <summary>
        /// Obtiene datos adicionales de las organizaciones.
        /// </summary>
        /// <param name="pIdGnoss"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public static OrganizacionBBDD GetOrganizacionBBDD(string pIdGnoss, ResourceApi pResourceApi)
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

            resultadoQuery = pResourceApi.VirtuosoQuery(select.ToString(), where.ToString(), "organization");

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

        /// <summary>
        /// Identificador del proyecto.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Nombre del usuario que ha creado la entidad.
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Fecha de la creación de la entidad.
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// Nombre del usuario que ha modificado por última vez la entidad.
        /// </summary>
        public string LastModifiedBy { get; set; }
        /// <summary>
        /// Fecha de la última modificación de la entidad.
        /// </summary>
        public string LastModifiedDate { get; set; }
        /// <summary>
        /// Identificador de la convocatoria del proyecto en caso de que la convocatoria este registrada en el SGI.
        /// </summary>
        public string ConvocatoriaId { get; set; }
        /// <summary>
        /// Identificador de la solicitud de convocatoria que dió lugar al proyecto.
        /// </summary>
        public string SolicitudId { get; set; }
        /// <summary>
        /// Estado del proyecto.
        /// </summary>
        public EstadoProyecto Estado { get; set; }
        /// <summary>
        /// Título del proyecto.
        /// </summary>
        public string Titulo { get; set; }
        /// <summary>
        /// Identificador corto del proyecto.
        /// </summary>
        public string Acronimo { get; set; }
        /// <summary>
        /// Código o referencia con el que se identifica el proyecto en la entidad convocante externa
        /// </summary>
        public string CodigoExterno { get; set; }
        /// <summary>
        /// Fecha de inicio del proyecto.
        /// </summary>
        public string FechaInicio { get; set; }
        /// <summary>
        /// Fecha de fin del proyecto.
        /// </summary>
        public string FechaFin { get; set; }
        /// <summary>
        /// Fecha de fin definitiva del proyecto.
        /// </summary>
        public string FechaFinDefinitiva { get; set; }
        /// <summary>
        /// Identificador de la Unidad de gestión
        /// </summary>
        public string UnidadGestionRef { get; set; }
        /// <summary>
        /// Entidad que representa un modelo de ejecución.
        /// </summary>
        public ModeloEjecucion ModeloEjecucion { get; set; }
        /// <summary>
        /// Entidad que representa la finalidad del proyecto.
        /// </summary>
        public TipoFinalidad Finalidad { get; set; }
        /// <summary>
        /// Permite mostrar o recoger la identificación externa de la convocatoria, dependiendo si el proyecto se asocia o no a una convocatoria registrada en el SGI.
        /// </summary>
        public string ConvocatoriaExterna { get; set; }
        /// <summary>
        /// Entidad que representa el ámbito geográfico.
        /// </summary>
        public TipoAmbitoGeografico AmbitoGeografico { get; set; }
        /// <summary>
        /// Indica si el proyecto es confidencial.
        /// </summary>
        public bool? Confidencial { get; set; }
        /// <summary>
        /// Indica el apartado del CVN al que correspondería el proyecto. 
        /// </summary>
        public string ClasificacionCVN { get; set; }
        /// <summary>
        /// Indica si el proyecto se realizará de forma coordinada con otros socios.
        /// </summary>
        public bool? Coordinado { get; set; }
        /// <summary>
        /// Indica si un proyecto coordinado es además colaborativo.
        /// </summary>
        public bool? Colaborativo { get; set; }
        /// <summary>
        /// Indica quién actúa como coordinador del proyecto.
        /// Un valor "false" indica que es la propia universidad quien actúa en calidad de coordinador del proyecto. 
        /// </summary>
        public bool? CoordinadorExterno { get; set; }
        /// <summary>
        /// Indica si el proyecto requiere gestión de Timesheet.
        /// </summary>
        public bool? Timesheet { get; set; }
        /// <summary>
        /// Indica si el proyecto requiere gestión de paquetes de trabajo en los Timesheet.
        /// </summary>
        public bool? PermitePaquetesTrabajo { get; set; }
        /// <summary>
        /// Indica si el proyecto requerirá realizar el cálculo de coste de hora de personal.
        /// </summary>
        public bool? CosteHora { get; set; }
        /// <summary>
        /// Indica el criterio de las horas anuales para el cálculo del coste/hora.
        /// </summary>
        public string TipoHorasAnuales { get; set; }
        /// <summary>
        /// IVA del proyecto.
        /// </summary>
        public ProyectoIVA Iva { get; set; }
        /// <summary>
        /// Indica la causa de exención de IVA.
        /// </summary>
        public string CausaExencion { get; set; }
        /// <summary>
        /// Observaciones de carácter interno del proyecto.
        /// </summary>
        public string Observaciones { get; set; }
        /// <summary>
        /// Indica si en el presupuesto se va a introducir por anualidades o no.
        /// </summary>
        public bool? Anualidades { get; set; }
        /// <summary>
        /// Importe presupuesto correspondiente al proyecto a desarrollar por la Universidad
        /// </summary>
        public double? ImportePresupuesto { get; set; }
        /// <summary>
        /// Importe concedido correspondiente al proyecto a desarrollar por la Universidad
        /// </summary>
        public double? ImporteConcedido { get; set; }
        /// <summary>
        /// Importe total presupuestado por todos los socios (adicionales a la Universidad) que participan en el proyecto
        /// </summary>
        public double? ImportePresupuestoSocios { get; set; }
        /// <summary>
        /// Importe total concedido por todos los socios (adicionales a la Universidad) que participan en el proyecto
        /// </summary>
        public double? ImporteConcedidoSocios { get; set; }
        /// <summary>
        /// Importe total presupuestado del proyecto (Universidad y socios)
        /// </summary>
        public double? TotalImportePresupuesto { get; set; }
        /// <summary>
        /// Importe total concedido del proyecto (Universidad y socios)
        /// </summary>
        public double? TotalImporteConcedido { get; set; }
        /// <summary>
        /// Indica si esta activa o no. 
        /// </summary>
        public bool? Activo { get; set; }
        /// <summary>
        /// Contexto
        /// </summary>
        public ContextoProyecto Contexto { get; set; }
        /// <summary>
        /// Equipo
        /// </summary>
        public List<ProyectoEquipo> Equipo { get; set; }
        /// <summary>
        /// Entidades gestoras
        /// </summary>
        public List<ProyectoEntidadGestora> EntidadesGestoras { get; set; }
        /// <summary>
        /// Entidades convocantes
        /// </summary>
        public List<ProyectoEntidadConvocante> EntidadesConvocantes { get; set; }
        /// <summary>
        /// Entidades financiadoras
        /// </summary>
        public List<ProyectoEntidadFinanciadora> EntidadesFinanciadoras { get; set; }
        /// <summary>
        /// Resumen anualidades
        /// </summary>
        public List<ProyectoAnualidadResumen> ResumenAnualidades { get; set; }
        /// <summary>
        /// Presupuestos totales
        /// </summary>
        public ProyectoPresupuestoTotales PresupuestosTotales { get; set; }
        /// <summary>
        /// Presupuestos clasificación
        /// </summary>
        public List<ProyectoClasificacion> ProyectoClasificacion { get; set; }
        /// <summary>
        /// Notificación de proyecto externo CVN
        /// </summary>
        public List<NotificacionProyectoExternoCVN> NotificacionProyectoExternoCVN { get; set; }
        /// <summary>
        /// Areas de conocimiento
        /// </summary>
        public List<ProyectoAreaConocimiento> AreasConocimiento { get; set; }
        /// <summary>
        /// Palabras clave
        /// </summary>
        public List<PalabraClave> PalabrasClaves { get; set; }
        /// <summary>
        /// Histórico de estados.
        /// </summary>
        public List<EstadoProyecto> historicoProyectos { get; set; }
    }
}
