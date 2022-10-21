using DepartmentOntology;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Harvester;
using Harvester.Models.ModelsBBDD;
using Harvester.Models.RabbitMQ;
using Newtonsoft.Json;
using OAI_PMH.Models.SGI.ActividadDocente;
using OAI_PMH.Models.SGI.FormacionAcademica;
using OAI_PMH.Models.SGI.OrganicStructure;
using OAI_PMH.Models.SGI.Organization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Utilidades;

namespace OAI_PMH.Models.SGI.PersonalData
{
    /// <summary>
    /// Persona
    /// </summary>
    public class Persona : SGI_Base
    {
        // Prefijos para las consultas SPARQL.
        private static string RUTA_PREFIJOS = $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Utilidades/prefijos.json";
        private static string mPrefijos = string.Join(" ", JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(RUTA_PREFIJOS)));

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
            PersonOntology.Person persona = CrearPersonOntology(pRabbitConf, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);

            if (pFusionarPersona && !string.IsNullOrEmpty(pIdPersona))
            {
                PersonOntology.Person personaAux = DatosPersonaNoBorrar(pIdPersona, pResourceApi);
                FusionarPersonas(persona, personaAux);
            }

            pResourceApi.ChangeOntoly("person");
            return persona.ToGnossApiResource(pResourceApi, null);
        }

        /// <summary>
        /// Devuelve el ID de la persona en BBDD.
        /// </summary>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public override string ObtenerIDBBDD(ResourceApi pResourceApi)
        {
            Dictionary<string, string> respuesta = ObtenerPersonasBBDD(new HashSet<string>() { Id.ToString() }, pResourceApi);
            if (respuesta.ContainsKey(Id.ToString()) && !string.IsNullOrEmpty(respuesta[Id.ToString()]))
            {
                return respuesta[Id.ToString()];
            }
            return null;
        }

        /// <summary>
        /// Permite cargar las entidades adicionales de la persona.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <param name="pRabbitConf"></param>
        /// <param name="pIdGnoss"></param>
        public override void ToRecursoAdicional(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, string pIdGnoss)
        {
            #region --- TESIS TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("thesissupervision");
            List<string> crisIdentifiersTesisBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "thesissupervision", "030.040.000.000");
            List<string> crisIdentifiersTesisSGI = new List<string>();
            List<TesisBBDD> listaTesisSGI = ObtenerTesisSGI(pRabbitConf, this.Tesis, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (TesisBBDD tesisAux in listaTesisSGI)
            {
                crisIdentifiersTesisSGI.Add(tesisAux.crisIdentifier);
            }

            // Carga.
            try
            {
                List<string> listaTesisCargarCrisIdentifiers = crisIdentifiersTesisSGI.Except(crisIdentifiersTesisBBDD).ToList();
                List<TesisBBDD> listaTesisCargar = listaTesisSGI.Where(x => listaTesisCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
                List<ComplexOntologyResource> listaTesisOntology = GetThesisSupervision(listaTesisCargar, pResourceApi, pIdGnoss);
                CargarDatos(listaTesisOntology, pResourceApi);
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de CARGA de 'Tésis': " + e);
            }

            // Eliminación.
            try
            {
                List<string> listaTesisBorrarCrisIdentifiers = crisIdentifiersTesisBBDD.Except(crisIdentifiersTesisSGI).ToList();
                List<string> listaIdsTesisBorrar = ObtenerDataByCrisIdentifiers(listaTesisBorrarCrisIdentifiers, pResourceApi, "thesissupervision", "030.040.000.000");
                BorrarRecursos(listaIdsTesisBorrar, pResourceApi, "thesissupervision");
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de BORRADO de 'Tésis': " + e);
            }
            #endregion

            #region --- IMPARTED ACADEMIC TRAINING TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("impartedacademictraining");

            List<string> crisIdentifiersImpartedAcademicBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "impartedacademictraining", "030.010.000.000");
            List<string> crisIdentifiersImpartedAcademicSGI = new List<string>();
            List<ImpartedAcademicTrainingBBDD> listaImpartedAcademicCargarSGI = ObtenerImpartedAcademicSGI(pRabbitConf, this.FormacionAcademicaImpartida, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (ImpartedAcademicTrainingBBDD tesisAux in listaImpartedAcademicCargarSGI)
            {
                crisIdentifiersImpartedAcademicSGI.Add(tesisAux.crisIdentifier);
            }

            // Carga.
            try
            {
                List<string> listaImpartedAcademicCargarCrisIdentifiers = crisIdentifiersImpartedAcademicSGI.Except(crisIdentifiersImpartedAcademicBBDD).ToList();
                List<ImpartedAcademicTrainingBBDD> listaImpartedAcademicCargar = listaImpartedAcademicCargarSGI.Where(x => listaImpartedAcademicCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
                List<ComplexOntologyResource> listaImpartedAcademicOntology = GetImpartedAcademic(listaImpartedAcademicCargar, pResourceApi, pIdGnoss);
                CargarDatos(listaImpartedAcademicOntology, pResourceApi);
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de CARGA de 'Formación Académica Impartida': " + e);
            }

            // Eliminación.
            try
            {
                List<string> listaImpartedAcademicBorrarCrisIdentifiers = crisIdentifiersImpartedAcademicBBDD.Except(crisIdentifiersImpartedAcademicSGI).ToList();
                List<string> listaIdsImpartedAcademicBorrar = ObtenerDataByCrisIdentifiers(listaImpartedAcademicBorrarCrisIdentifiers, pResourceApi, "impartedacademictraining", "030.010.000.000");
                BorrarRecursos(listaIdsImpartedAcademicBorrar, pResourceApi, "impartedacademictraining");
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de BORRADO de 'Formación Académica Impartida': " + e);
            }
            #endregion

            #region --- CURSES AND SEMINARS TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("impartedcoursesseminars");
            List<string> crisIdentifiersCursosBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "impartedcoursesseminars", "030.060.000.000");
            List<ImpartedCoursesSeminarsBBDD> listaCursosSGI = ObtenerCursosSGI(pRabbitConf, this.SeminariosCursos, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);

            List<string> crisIdentifiersCursosSGI = new List<string>();
            foreach (ImpartedCoursesSeminarsBBDD cursoAux in listaCursosSGI)
            {
                crisIdentifiersCursosSGI.Add(cursoAux.crisIdentifiers);
            }

            // Carga.
            try
            {
                List<string> listaCursosCargarCrisIdentifiers = crisIdentifiersCursosSGI.Except(crisIdentifiersCursosBBDD).ToList();
                List<ImpartedCoursesSeminarsBBDD> listaCursosCargar = listaCursosSGI.Where(x => listaCursosCargarCrisIdentifiers.Contains(x.crisIdentifiers)).ToList();
                List<ComplexOntologyResource> listaCursosOntology = GetCursosSupervision(listaCursosCargar, pResourceApi, pIdGnoss);
                CargarDatos(listaCursosOntology, pResourceApi);
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de CARGA de 'Cursos y Seminarios': " + e);
            }

            // Eliminación.
            try
            {
                List<string> listaCursosBorrarCrisIdentifiers = crisIdentifiersCursosBBDD.Except(crisIdentifiersCursosSGI).ToList();
                List<string> listaIdsCursosBorrar = ObtenerDataByCrisIdentifiers(listaCursosBorrarCrisIdentifiers, pResourceApi, "impartedcoursesseminars", "030.060.000.000");
                BorrarRecursos(listaIdsCursosBorrar, pResourceApi, "impartedcoursesseminars");
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de BORRADO de 'Cursos y Seminarios': " + e);
            }
            #endregion

            #region --- CICLOS TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("academicdegree");
            List<string> crisIdentifiersCyclesBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "academicdegree", "020.010.010.000");
            List<string> crisIdentifiersCyclesSGI = new List<string>();
            List<CiclosBBDD> listaCiclosSGI = ObtenerCiclosSGI(pRabbitConf, this.Ciclos, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (CiclosBBDD ciclosAux in listaCiclosSGI)
            {
                crisIdentifiersCyclesSGI.Add(ciclosAux.crisIdentifier);
            }

            // Carga.
            try
            {
                List<string> listaCiclosCargarCrisIdentifiers = crisIdentifiersCyclesSGI.Except(crisIdentifiersCyclesBBDD).ToList();
                List<CiclosBBDD> listaCiclosCargar = listaCiclosSGI.Where(x => listaCiclosCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
                List<ComplexOntologyResource> listaCiclosOntology = GetCycles(listaCiclosCargar, pResourceApi, pIdGnoss);
                CargarDatos(listaCiclosOntology, pResourceApi);
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de CARGA de 'Ciclos': " + e);
            }

            // Eliminación.
            try
            {
                List<string> listaCiclosBorrarCrisIdentifiers = crisIdentifiersCyclesBBDD.Except(crisIdentifiersCyclesSGI).ToList();
                List<string> listaIdsCiclosBorrar = ObtenerDataByCrisIdentifiers(listaCiclosBorrarCrisIdentifiers, pResourceApi, "academicdegree", "020.010.010.000");
                BorrarRecursos(listaIdsCiclosBorrar, pResourceApi, "academicdegree");
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de BORRADO de 'Ciclos': " + e);
            }
            #endregion

            #region --- DOCTORADOS TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("academicdegree");
            List<string> crisIdentifiersDoctoradosBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "academicdegree", "020.010.020.000");
            List<string> crisIdentifiersDoctoradosSGI = new List<string>();
            List<DoctoradosBBDD> listaDoctoradosSGI = ObtenerDoctoradosSGI(pRabbitConf, this.Doctorados, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (DoctoradosBBDD doctoradosAux in listaDoctoradosSGI)
            {
                crisIdentifiersCyclesSGI.Add(doctoradosAux.crisIdentifier);
            }

            // Carga.
            try
            {
                List<string> listaDoctoradosCargarCrisIdentifiers = crisIdentifiersDoctoradosSGI.Except(crisIdentifiersDoctoradosBBDD).ToList();
                List<DoctoradosBBDD> listaDoctoradosCargar = listaDoctoradosSGI.Where(x => listaDoctoradosCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
                List<ComplexOntologyResource> listaDoctoradosOntology = GetDoctorates(listaDoctoradosCargar, pResourceApi, pIdGnoss);
                CargarDatos(listaDoctoradosOntology, pResourceApi);
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de CARGA de 'Doctorados': " + e);
            }

            // Eliminación.
            try
            {
                List<string> listaDoctoradosBorrarCrisIdentifiers = crisIdentifiersDoctoradosBBDD.Except(crisIdentifiersDoctoradosSGI).ToList();
                List<string> listaIdsDoctoradosBorrar = ObtenerDataByCrisIdentifiers(listaDoctoradosBorrarCrisIdentifiers, pResourceApi, "academicdegree", "020.010.020.000");
                BorrarRecursos(listaIdsDoctoradosBorrar, pResourceApi, "academicdegree");
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de BORRADO de 'Doctorados': " + e);
            }
            #endregion

            #region --- POSGRADO TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("academicdegree");
            List<string> crisIdentifiersPosgradoBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "academicdegree", "020.010.030.000");
            List<string> crisIdentifiersPosgradoSGI = new List<string>();
            List<PosgradoBBDD> listaPosgradosSGI = ObtenerPosgradosSGI(pRabbitConf, this.Posgrado, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (PosgradoBBDD posgradosAux in listaPosgradosSGI)
            {
                crisIdentifiersPosgradoSGI.Add(posgradosAux.crisIdentifier);
            }

            // Carga.
            try
            {
                List<string> listaPosgradosCargarCrisIdentifiers = crisIdentifiersPosgradoSGI.Except(crisIdentifiersPosgradoBBDD).ToList();
                List<PosgradoBBDD> listaPosgradosCargar = listaPosgradosSGI.Where(x => listaPosgradosCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
                List<ComplexOntologyResource> listaPosgradosOntology = GetPosgrados(listaPosgradosCargar, pResourceApi, pIdGnoss);
                CargarDatos(listaPosgradosOntology, pResourceApi);
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de CARGA de 'Posgrados': " + e);
            }

            // Eliminación.
            try
            {
                List<string> listaPosgradosBorrarCrisIdentifiers = crisIdentifiersPosgradoBBDD.Except(crisIdentifiersPosgradoSGI).ToList();
                List<string> listaIdsPosgradosBorrar = ObtenerDataByCrisIdentifiers(listaPosgradosBorrarCrisIdentifiers, pResourceApi, "academicdegree", "020.010.030.000");
                BorrarRecursos(listaIdsPosgradosBorrar, pResourceApi, "academicdegree");
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de BORRADO de 'Posgrados': " + e);
            }
            #endregion

            #region --- FORMACIÓN ESPECIALIZADA TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("academicdegree");
            List<string> crisIdentifiersEspecializadaBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "academicdegree", "020.020.000.000");
            List<string> crisIdentifiersEspecializadaSGI = new List<string>();
            List<FormacionEspecializadaBBDD> listaEspecializadaSGI = ObtenerFormacionEspecializadaSGI(pRabbitConf, this.FormacionEspecializada, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (FormacionEspecializadaBBDD formacionEspecializadaAux in listaEspecializadaSGI)
            {
                crisIdentifiersEspecializadaSGI.Add(formacionEspecializadaAux.crisIdentifier);
            }

            // Carga.
            try
            {
                List<string> listaEspecializadaCargarCrisIdentifiers = crisIdentifiersEspecializadaSGI.Except(crisIdentifiersEspecializadaBBDD).ToList();
                List<FormacionEspecializadaBBDD> listaEspecializadaCargar = listaEspecializadaSGI.Where(x => listaEspecializadaCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
                List<ComplexOntologyResource> listaEspecializadaOntology = GetFormacionEspecializada(listaEspecializadaCargar, pResourceApi, pIdGnoss);
                CargarDatos(listaEspecializadaOntology, pResourceApi);
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de CARGA de 'Formación Especializada': " + e);
            }

            // Eliminación.
            try
            {
                List<string> listaEspecializadaBorrarCrisIdentifiers = crisIdentifiersEspecializadaBBDD.Except(crisIdentifiersEspecializadaSGI).ToList();
                List<string> listaIdsEspecializadaBorrar = ObtenerDataByCrisIdentifiers(listaEspecializadaBorrarCrisIdentifiers, pResourceApi, "academicdegree", "020.020.000.000");
                BorrarRecursos(listaIdsEspecializadaBorrar, pResourceApi, "academicdegree");
            }
            catch (Exception e)
            {
                Console.WriteLine("[" + this.Id + "] ERROR en el proceso de BORRADO de 'Formación Especializada': " + e);
            }
            #endregion
        }

        /// <summary>
        /// Permite borrar recursos.
        /// </summary>
        /// <param name="pListaGnossId"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pOntology"></param>
        public static void BorrarRecursos(List<string> pListaGnossId, ResourceApi pResourceApi, string pOntology)
        {
            pResourceApi.ChangeOntoly(pOntology);

            foreach (string id in pListaGnossId)
            {
                Guid guid = pResourceApi.GetShortGuid(id);
                pResourceApi.PersistentDelete(guid);
            }
        }

        /// <summary>
        /// Obtiene el ID del recurso mediante un crisidentifier.
        /// </summary>
        /// <param name="pListaCrisIdentifiers"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pOntology"></param>
        /// <param name="pCvnCode"></param>
        /// <returns></returns>
        public static List<string> ObtenerDataByCrisIdentifiers(List<string> pListaCrisIdentifiers, ResourceApi pResourceApi, string pOntology, string pCvnCode)
        {
            List<string> listaTesis = new List<string>();

            string select = string.Empty;
            string where = string.Empty;

            select = $@"SELECT * ";
            where = $@"WHERE {{                        
                        ?s <http://w3id.org/roh/crisIdentifier> ?crisIdentifier. 
                        ?s <http://w3id.org/roh/cvnCode> '{pCvnCode}'.
                        FILTER(?crisIdentifier in ('{string.Join("', '", pListaCrisIdentifiers.Select(x => x))}'))
                    }}";

            SparqlObject resultadoQueryPerson = pResourceApi.VirtuosoQuery(select, where, pOntology);
            if (resultadoQueryPerson != null && resultadoQueryPerson.results != null && resultadoQueryPerson.results.bindings != null && resultadoQueryPerson.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPerson.results.bindings)
                {
                    listaTesis.Add(fila["s"].value);
                }
            }

            return listaTesis;
        }

        /// <summary>
        /// Obtiene los crisidentifiers de las personas mediante el ID del recurso.
        /// </summary>
        /// <param name="pListaIds"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        public static Dictionary<string, string> ObtenerPersonasBBDD(HashSet<string> pListaIds, ResourceApi pResourceApi)
        {
            List<List<string>> listasPersonas = SplitList(pListaIds.ToList(), 1000).ToList();
            Dictionary<string, string> dicPersonasBBDD = new Dictionary<string, string>();
            foreach (string persona in pListaIds)
            {
                dicPersonasBBDD[persona] = "";
            }
            foreach (List<string> listaItem in listasPersonas)
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
                SparqlObject resultadoQueryPerson = pResourceApi.VirtuosoQuery(selectPerson, wherePerson, "person");
                if (resultadoQueryPerson != null && resultadoQueryPerson.results != null && resultadoQueryPerson.results.bindings != null && resultadoQueryPerson.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryPerson.results.bindings)
                    {
                        dicPersonasBBDD[fila["crisIdentifier"].value] = fila["s"].value;
                    }
                }
            }

            return dicPersonasBBDD;
        }

        /// <summary>
        /// Permite consultar los datos de las personas en el SGI.
        /// </summary>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pId"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public static Persona GetPersonaSGI(IHarvesterServices pHarvesterServices, ReadConfig pConfig, string pId, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            // Obtención de datos en bruto.
            Persona persona = new Persona();
            string xmlResult = pHarvesterServices.GetRecord(pId, pConfig);

            if (string.IsNullOrEmpty(xmlResult))
            {
                return null;
            }

            XmlSerializer xmlSerializer = new(typeof(Persona));
            using (StringReader sr = new(xmlResult))
            {
                persona = (Persona)xmlSerializer.Deserialize(sr);
            }

            return persona;
        }

        /// <summary>
        /// Crea el objeto Person.
        /// </summary>
        /// <param name="pRabbitConf"></param>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public PersonOntology.Person CrearPersonOntology(RabbitServiceWriterDenormalizer pRabbitConf, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {

            #region --- Obtenemos el ID de la Organización
            HashSet<string> listaIdsOrganizaciones = new HashSet<string>();
            if (!string.IsNullOrEmpty(this.EntidadPropiaRef))
            {
                listaIdsOrganizaciones.Add(this.EntidadPropiaRef);
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


            #region Construimos el objeto Person
            PersonOntology.Person persona = new PersonOntology.Person();

            // Crisidentifier (Se corresponde al DNI sin letra)
            persona.Roh_crisIdentifier = this.Id;

            // Sincronización.
            persona.Roh_isSynchronized = true;

            // Nombre.
            if (!string.IsNullOrEmpty(this.Nombre))
            {
                persona.Foaf_firstName = this.Nombre;
            }

            // Apellidos.
            if (!string.IsNullOrEmpty(this.Apellidos))
            {
                persona.Foaf_lastName = this.Apellidos;
            }

            //organizacion
            if (!string.IsNullOrEmpty(this.EntidadPropiaRef))
            {
                persona.IdRoh_hasRole = dicOrganizacionesCargadas[this.EntidadPropiaRef];
            }

            // Sexo.
            if (this.Sexo != null && !string.IsNullOrEmpty(this.Sexo.Id))
            {
                if (this.Sexo.Id == "V")
                {
                    persona.IdFoaf_gender = $@"{pResourceApi.GraphsUrl}items/gender_000";
                }
                else
                {
                    persona.IdFoaf_gender = $@"{pResourceApi.GraphsUrl}items/gender_010";
                }
            }

            // Nombre completo.
            if (!string.IsNullOrEmpty(this.Nombre) && !string.IsNullOrEmpty(this.Apellidos))
            {
                persona.Foaf_name = this.Nombre + " " + this.Apellidos;
            }

            // Correos.
            if (this.Emails != null && this.Emails.Any())
            {
                persona.Vcard_email = new List<string>();
                foreach (Email item in this.Emails)
                {
                    persona.Vcard_email.Add(item.email);
                }
            }
            else
            {
                // No tiene correo la persona.
            }

            // Dirección de contacto.
            if (!string.IsNullOrEmpty(this.DatosContacto?.PaisContacto?.Nombre) || !string.IsNullOrEmpty(this.DatosContacto?.ComAutonomaContacto?.Nombre)
                || !string.IsNullOrEmpty(this.DatosContacto?.CiudadContacto) || !string.IsNullOrEmpty(this.DatosContacto?.CodigoPostalContacto)
                || !string.IsNullOrEmpty(this.DatosContacto?.DireccionContacto))
            {
                string direccionContacto = string.IsNullOrEmpty(this.DatosContacto?.DireccionContacto) ? "" : this.DatosContacto.DireccionContacto;
                direccionContacto += string.IsNullOrEmpty(this.DatosContacto?.CodigoPostalContacto) ? "" : ", " + this.DatosContacto.CodigoPostalContacto;
                direccionContacto += string.IsNullOrEmpty(this.DatosContacto?.CiudadContacto) ? "" : ", " + this.DatosContacto.CiudadContacto;
                direccionContacto += string.IsNullOrEmpty(this.DatosContacto?.ProvinciaContacto?.Nombre) ? "" : ", " + this.DatosContacto.ProvinciaContacto.Nombre;
                direccionContacto += string.IsNullOrEmpty(this.DatosContacto?.PaisContacto?.Nombre) ? "" : ", " + this.DatosContacto.PaisContacto.Nombre;

                persona.Vcard_address = direccionContacto;
            }

            // Teléfonos.
            HashSet<string> telefonos = new HashSet<string>();
            if (this.DatosContacto?.Telefonos != null && this.DatosContacto.Telefonos.Any())
            {
                foreach (string item in this.DatosContacto.Telefonos)
                {
                    telefonos.Add(item);
                }
            }
            if (this.DatosContacto?.Moviles != null && this.DatosContacto.Moviles.Any())
            {
                foreach (string item in this.DatosContacto.Telefonos)
                {
                    telefonos.Add(item);
                }
            }
            persona.Vcard_hasTelephone = telefonos.ToList();

            // Activo.
            if (this.Activo.HasValue)
            {
                persona.Roh_isActive = this.Activo.Value;
            }

            // Departamentos.
            if (!string.IsNullOrEmpty(this.Vinculacion?.Departamento?.Id) && !string.IsNullOrEmpty(this.Vinculacion?.Departamento?.Nombre))
            {
                bool deptEncontrado = ComprobarDepartamentoBBDD(this.Vinculacion?.Departamento?.Id, pResourceApi);

                if (!deptEncontrado)
                {
                    // Si no existe, se carga el departamento como entidad secundaria.
                    CargarDepartment(this.Vinculacion.Departamento.Id, this.Vinculacion.Departamento.Nombre, pResourceApi);
                }

                persona.IdVivo_departmentOrSchool = $@"{pResourceApi.GraphsUrl}items/department_{this.Vinculacion.Departamento.Id}";
            }

            // Cargo en la universidad.
            if (!string.IsNullOrEmpty(this.Vinculacion?.VinculacionCategoriaProfesional?.categoriaProfesional?.nombre))
            {
                persona.Roh_hasPosition = this.Vinculacion?.VinculacionCategoriaProfesional?.categoriaProfesional?.nombre;
            }

            // Fecha de actualización.
            persona.Roh_lastUpdatedDate = DateTime.UtcNow;
            #endregion
            return persona;
        }

        /// <summary>
        /// Permite cargar los recursos.
        /// </summary>
        /// <param name="pListaRecursosCargar">Lista de recursos a cargar.</param>
        private static List<string> CargarDatos(List<ComplexOntologyResource> pListaRecursosCargar, ResourceApi pResourceApi)
        {
            ConcurrentBag<string> idsItems = new ConcurrentBag<string>();

            // Carga.
            Parallel.ForEach(pListaRecursosCargar, new ParallelOptions { MaxDegreeOfParallelism = 6 }, recursoCargar =>
            {
                int numIntentos = 0;
                while (!recursoCargar.Uploaded)
                {
                    numIntentos++;

                    if (numIntentos > 6)
                    {
                        break;
                    }
                    string id = "";
                    if (pListaRecursosCargar.Last() == recursoCargar)
                    {
                        id = pResourceApi.LoadComplexSemanticResource(recursoCargar, false, true);
                    }
                    else
                    {
                        id = pResourceApi.LoadComplexSemanticResource(recursoCargar);
                    }
                    if (recursoCargar.Uploaded)
                    {
                        idsItems.Add(id);
                    }
                }
            });
            return idsItems.Distinct().ToList();
        }

        /// <summary>
        /// Construye el objeto de carga de las Tesis.
        /// </summary>
        /// <param name="pTesisList"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pIdGnoss"></param>
        /// <returns></returns>
        public List<ComplexOntologyResource> GetThesisSupervision(List<TesisBBDD> pTesisList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaTesisDevolver = new List<ComplexOntologyResource>();

            foreach (TesisBBDD tesis in pTesisList)
            {
                ThesissupervisionOntology.ThesisSupervision tesisDevolver = new ThesissupervisionOntology.ThesisSupervision();

                // CrisIdentifier
                tesisDevolver.Roh_crisIdentifier = tesis.crisIdentifier;

                tesisDevolver.IdRoh_owner = pIdGnoss;
                tesisDevolver.Roh_cvnCode = "030.040.000.000";
                tesisDevolver.IdRoh_projectCharacterType = tesis.projectCharacterType;
                tesisDevolver.Roh_projectCharacterTypeOther = tesis.projectCharacterTypeOther;
                tesisDevolver.Roh_title = tesis.title;
                tesisDevolver.IdVcard_hasCountryName = tesis.hasCountryName;
                tesisDevolver.IdVcard_hasRegion = tesis.hasRegion;
                tesisDevolver.Vcard_locality = tesis.locality;
                tesisDevolver.Dct_issued = tesis.issued;
                tesisDevolver.Roh_qualification = tesis.qualification;
                tesisDevolver.Roh_europeanDoctorateDate = tesis.europeanDoctorateDate;
                tesisDevolver.Roh_qualityMention = tesis.qualityMention.Value;
                tesisDevolver.Roh_europeanDoctorate = tesis.europeanDoctorate.Value;
                tesisDevolver.Roh_qualityMentionDate = tesis.qualityMentionDate;
                tesisDevolver.Roh_studentName = tesis.studentName;
                tesisDevolver.Roh_studentFirstSurname = tesis.studentFirstSurname;
                tesisDevolver.Roh_studentNick = tesis.studentNick;
                tesisDevolver.Roh_promotedByTitle = tesis.promotedByTitle;
                tesisDevolver.IdRoh_promotedBy = tesis.promotedBy;

                // En los datos únicamente viene un solo codirector.
                if (tesis.codirector != null && tesis.codirector.Any())
                {
                    ThesissupervisionOntology.PersonAux personaAux = new ThesissupervisionOntology.PersonAux();
                    personaAux.Rdf_comment = tesis.codirector[0].comment;
                    personaAux.Foaf_firstName = tesis.codirector[0].firstName;
                    personaAux.Roh_secondFamilyName = tesis.codirector[0].secondFamilyName;
                    personaAux.Foaf_nick = tesis.codirector[0].nick;
                    tesisDevolver.Roh_codirector = new List<ThesissupervisionOntology.PersonAux>() { personaAux };
                }


                listaTesisDevolver.Add(tesisDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaTesisDevolver;
        }

        /// <summary>
        /// Construye el objeto de carga de los Ciclos
        /// </summary>
        /// <param name="pCyclesList"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pIdGnoss"></param>
        /// <returns></returns>
        public List<ComplexOntologyResource> GetCycles(List<CiclosBBDD> pCyclesList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaCiclosDevolver = new List<ComplexOntologyResource>();

            foreach (CiclosBBDD ciclo in pCyclesList)
            {
                AcademicdegreeOntology.AcademicDegree ciclosDevolver = new AcademicdegreeOntology.AcademicDegree();

                ciclosDevolver.IdRoh_owner = pIdGnoss;
                ciclosDevolver.Roh_cvnCode = "020.010.010.000";
                ciclosDevolver.Roh_title = ciclo.nombreTitulo;
                ciclosDevolver.Roh_conductedByTitle = ciclo.entidadTitulacionTitulo;
                ciclosDevolver.IdRoh_conductedBy = ciclo.entidadTitulacion;
                ciclosDevolver.Dct_issued = ciclo.fechaTitulacion;
                ciclosDevolver.IdRoh_universityDegreeType = ciclo.titulacionUni;
                ciclosDevolver.Roh_universityDegreeTypeOther = ciclo.titulacionUniOtros;
                ciclosDevolver.Roh_foreignTitle = ciclo.tituloExtranjero;
                ciclosDevolver.Roh_approvedDegree = ciclo.tituloHomologado;
                ciclosDevolver.Roh_approvedDate = ciclo.fechaHomologacion;
                ciclosDevolver.Vcard_locality = ciclo.ciudadEntidadTitulacion;
                ciclosDevolver.IdVcard_hasRegion = ciclo.cAutonEntidadTitulacion;
                ciclosDevolver.IdVcard_hasCountryName = ciclo.paisEntidadTitulacion;
                // NOTA MEDIA TODO
                ciclosDevolver.IdRoh_prize = ciclo.premio;
                ciclosDevolver.Roh_prizeOther = ciclo.premioOther;

                // CrisIdentifier
                ciclosDevolver.Roh_crisIdentifier = ciclo.crisIdentifier;

                listaCiclosDevolver.Add(ciclosDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaCiclosDevolver;
        }

        /// <summary>
        /// Obtiene el objeto de carga de los Doctorados.
        /// </summary>
        /// <param name="pDoctoratesList"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pIdGnoss"></param>
        /// <returns></returns>
        public List<ComplexOntologyResource> GetDoctorates(List<DoctoradosBBDD> pDoctoratesList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaDoctoradosDevolver = new List<ComplexOntologyResource>();

            foreach (DoctoradosBBDD doctorado in pDoctoratesList)
            {
                AcademicdegreeOntology.AcademicDegree doctoradosDevolver = new AcademicdegreeOntology.AcademicDegree();

                // CrisIdentifier
                doctoradosDevolver.Roh_crisIdentifier = doctorado.crisIdentifier;

                doctoradosDevolver.IdRoh_owner = pIdGnoss;
                doctoradosDevolver.Roh_cvnCode = "020.010.020.000";
                doctoradosDevolver.Roh_title = doctorado.nombreTitulo;
                doctoradosDevolver.Roh_conductedByTitle = doctorado.entidadTitulacionTitulo;
                doctoradosDevolver.IdRoh_conductedBy = doctorado.entidadTitulacion;
                doctoradosDevolver.Dct_issued = doctorado.fechaTitulacion;
                doctoradosDevolver.IdRoh_deaEntity = doctorado.entidadTitDEA;
                doctoradosDevolver.Roh_deaEntityTitle = doctorado.entidadTitDEATitulo;
                doctoradosDevolver.Roh_deaDate = doctorado.obtencionDEA;
                doctoradosDevolver.Roh_thesisTitle = doctorado.tituloTesis;
                doctoradosDevolver.Roh_qualification = doctorado.calificacionObtenida;
                doctoradosDevolver.Roh_directorNick = doctorado.firmaDirector;
                doctoradosDevolver.Roh_directorName = doctorado.nombreDirector;
                doctoradosDevolver.Roh_directorFirstSurname = doctorado.primApeDirector;
                doctoradosDevolver.Roh_directorSecondSurname = doctorado.segunApeDirector;
                // TODO: CoDirectores
                doctoradosDevolver.Roh_europeanDoctorate = doctorado.doctoradoEuropeo;
                doctoradosDevolver.Roh_europeanDoctorateDate = doctorado.fechaDoctorado;
                doctoradosDevolver.Roh_qualityMention = doctorado.mencionCalidad;
                doctoradosDevolver.Roh_doctorExtraordinaryAward = doctorado.premioExtraordinarioDoctor;
                doctoradosDevolver.Roh_doctorExtraordinaryAwardDate = doctorado.fechaPremioDoctor;
                doctoradosDevolver.Roh_approvedDegree = doctorado.tituloHomologado;
                doctoradosDevolver.Roh_approvedDate = doctorado.fechaHomologado;
                doctoradosDevolver.Vcard_locality = doctorado.ciudadEntidadTitulacion;
                doctoradosDevolver.IdVcard_hasRegion = doctorado.cAutonEntidadTitulacion;
                doctoradosDevolver.IdVcard_hasCountryName = doctorado.paisEntidadTitulacion;

                listaDoctoradosDevolver.Add(doctoradosDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaDoctoradosDevolver;
        }

        /// <summary>
        /// Obtiene el objeto de carga de los Posgrados.
        /// </summary>
        /// <param name="pPosgradosList"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pIdGnoss"></param>
        /// <returns></returns>
        public List<ComplexOntologyResource> GetPosgrados(List<PosgradoBBDD> pPosgradosList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaPosgradosDevolver = new List<ComplexOntologyResource>();

            foreach (PosgradoBBDD posgrado in pPosgradosList)
            {
                AcademicdegreeOntology.AcademicDegree posgradosDevolver = new AcademicdegreeOntology.AcademicDegree();

                // CrisIdentifier
                posgradosDevolver.Roh_crisIdentifier = posgrado.crisIdentifier;

                posgradosDevolver.IdRoh_owner = pIdGnoss;
                posgradosDevolver.Roh_cvnCode = "020.010.030.000";
                posgradosDevolver.Roh_title = posgrado.nombreTitulo;
                posgradosDevolver.Roh_conductedByTitle = posgrado.entidadTitulacionTitulo;
                posgradosDevolver.IdRoh_conductedBy = posgrado.entidadTitulacion;
                posgradosDevolver.Dct_issued = posgrado.fechaTitulacion;
                posgradosDevolver.IdRoh_formationType = posgrado.tipoFormacion;
                posgradosDevolver.Roh_qualification = posgrado.calificacionObtenida;
                posgradosDevolver.Roh_approvedDegree = posgrado.tituloHomologado;
                posgradosDevolver.Roh_approvedDate = posgrado.fechaHomologacion;
                posgradosDevolver.Vcard_locality = posgrado.ciudadEntidadTitulacion;
                posgradosDevolver.IdVcard_hasRegion = posgrado.cAutonEntidadTitulacion;
                posgradosDevolver.IdVcard_hasCountryName = posgrado.paisEntidadTitulacion;

                listaPosgradosDevolver.Add(posgradosDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaPosgradosDevolver;
        }

        /// <summary>
        /// Obtiene el objeto de carga de la Formación Especializada.
        /// </summary>
        /// <param name="pEspecializadaList"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pIdGnoss"></param>
        /// <returns></returns>
        public List<ComplexOntologyResource> GetFormacionEspecializada(List<FormacionEspecializadaBBDD> pEspecializadaList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaPosgradosDevolver = new List<ComplexOntologyResource>();

            foreach (FormacionEspecializadaBBDD formacionEspecializada in pEspecializadaList)
            {
                AcademicdegreeOntology.AcademicDegree formEspDevolver = new AcademicdegreeOntology.AcademicDegree();

                // CrisIdentifier
                formEspDevolver.Roh_crisIdentifier = formacionEspecializada.crisIdentifier;

                formEspDevolver.IdRoh_owner = pIdGnoss;
                formEspDevolver.Roh_cvnCode = "020.020.000.000";
                formEspDevolver.Roh_title = formacionEspecializada.nombreTitulo;
                formEspDevolver.Roh_conductedByTitle = formacionEspecializada.entidadTitulacionTitulo;
                formEspDevolver.IdRoh_conductedBy = formacionEspecializada.entidadTitulacion;
                formEspDevolver.Dct_issued = formacionEspecializada.fechaFinalizacion;
                formEspDevolver.Roh_durationHours = (int)(formacionEspecializada.duracionHoras);
                formEspDevolver.IdRoh_formationType = formacionEspecializada.tipoFormacion;
                formEspDevolver.Roh_goals = formacionEspecializada.objetivosEntidad;
                formEspDevolver.Roh_trainerNick = formacionEspecializada.firma;
                formEspDevolver.Roh_trainerName = formacionEspecializada.nombre;
                formEspDevolver.Roh_trainerFirstSurname = formacionEspecializada.primApe;
                formEspDevolver.Roh_trainerSecondSurname = formacionEspecializada.segunApe;
                formEspDevolver.Vcard_locality = formacionEspecializada.ciudadEntidadTitulacion;
                formEspDevolver.IdVcard_hasRegion = formacionEspecializada.cAutonEntidadTitulacion;
                formEspDevolver.IdVcard_hasCountryName = formacionEspecializada.paisEntidadTitulacion;

                listaPosgradosDevolver.Add(formEspDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaPosgradosDevolver;
        }

        /// <summary>
        /// Obtiene el objeto de carga de la Formación Impartida.
        /// </summary>
        /// <param name="pImpartedAcademicList"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pIdGnoss"></param>
        /// <returns></returns>
        public List<ComplexOntologyResource> GetImpartedAcademic(List<ImpartedAcademicTrainingBBDD> pImpartedAcademicList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaImpartedDevolver = new List<ComplexOntologyResource>();

            foreach (ImpartedAcademicTrainingBBDD impartedAcademic in pImpartedAcademicList)
            {
                ImpartedacademictrainingOntology.ImpartedAcademicTraining academicDevolver = new ImpartedacademictrainingOntology.ImpartedAcademicTraining();

                // CrisIdentifier
                academicDevolver.Roh_crisIdentifier = impartedAcademic.crisIdentifier;

                academicDevolver.IdRoh_owner = pIdGnoss;

                academicDevolver.Roh_cvnCode = "030.010.000.000";
                academicDevolver.Roh_title = impartedAcademic.title;
                academicDevolver.IdRoh_degreeType = impartedAcademic.degreeType;
                academicDevolver.Roh_teaches = impartedAcademic.teaches;
                academicDevolver.Vivo_start = impartedAcademic.start;
                academicDevolver.Vivo_end = impartedAcademic.end;
                academicDevolver.Roh_promotedByTitle = impartedAcademic.promotedByTitle;
                academicDevolver.IdRoh_promotedBy = impartedAcademic.promotedBy;
                academicDevolver.IdRoh_promotedByType = impartedAcademic.promotedByType;
                academicDevolver.Roh_promotedByTypeOther = impartedAcademic.promotedByTypeOther;
                academicDevolver.Roh_center = impartedAcademic.center;
                academicDevolver.Vcard_locality = impartedAcademic.locality;
                academicDevolver.IdVcard_hasRegion = impartedAcademic.hasRegion;
                academicDevolver.IdVcard_hasCountryName = impartedAcademic.hasCountryName;
                academicDevolver.IdRoh_teachingType = impartedAcademic.teachingType;
                academicDevolver.Roh_numberECTSHours = (float)impartedAcademic.numberECTSHours;
                academicDevolver.Roh_frequency = (float)impartedAcademic.frequency;
                academicDevolver.IdRoh_programType = impartedAcademic.programType;
                academicDevolver.Roh_programTypeOther = impartedAcademic.programTypeOther;
                academicDevolver.Roh_department = impartedAcademic.department;
                academicDevolver.IdRoh_courseType = impartedAcademic.courseType;
                academicDevolver.Roh_courseTypeOther = impartedAcademic.courseTypeOther;
                academicDevolver.Roh_course = impartedAcademic.course;
                academicDevolver.IdRoh_hoursCreditsECTSType = impartedAcademic.hoursCreditsECTSType;
                academicDevolver.IdVcard_hasLanguage = impartedAcademic.hasLanguage;
                academicDevolver.Roh_competencies = impartedAcademic.competencies;
                academicDevolver.Roh_professionalCategory = impartedAcademic.professionalCategory;
                academicDevolver.Roh_qualification = impartedAcademic.qualification;
                academicDevolver.Roh_maxQualification = impartedAcademic.maxQualification;
                academicDevolver.IdRoh_evaluatedBy = impartedAcademic.evaluatedBy;
                academicDevolver.IdRoh_evaluatedByType = impartedAcademic.evaluatedByType;
                academicDevolver.Roh_evaluatedByTypeOther = impartedAcademic.evaluatedByTypeOther;
                academicDevolver.Roh_evaluatedByLocality = impartedAcademic.evaluatedByLocality;
                academicDevolver.IdRoh_evaluatedByHasCountryName = impartedAcademic.evaluatedByHasCountryName;
                academicDevolver.IdRoh_evaluatedByHasRegion = impartedAcademic.hasRegion;
                academicDevolver.Roh_financedByTitle = impartedAcademic.financedByTitle;
                academicDevolver.IdRoh_financedBy = impartedAcademic.financedBy;
                academicDevolver.IdRoh_financedByType = impartedAcademic.financedByType;
                academicDevolver.Roh_financedByTypeOther = impartedAcademic.financedByTypeOther;
                academicDevolver.IdRoh_financedByHasCountryName = impartedAcademic.financedByHasCountryName;
                academicDevolver.IdRoh_financedByHasRegion = impartedAcademic.financedByHasRegion;
                academicDevolver.IdRoh_callType = impartedAcademic.callType;
                academicDevolver.Roh_callTypeOther = impartedAcademic.callTypeOther;
                academicDevolver.IdVivo_geographicFocus = impartedAcademic.geographicFocus;
                academicDevolver.Roh_geographicFocusOther = impartedAcademic.geographicFocusOther;

                listaImpartedDevolver.Add(academicDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaImpartedDevolver;
        }

        /// <summary>
        /// Obtiene el objeto de carga de Cursos y Seminarios.
        /// </summary>
        /// <param name="pTesisList"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pIdGnoss"></param>
        /// <returns></returns>
        public List<ComplexOntologyResource> GetCursosSupervision(List<ImpartedCoursesSeminarsBBDD> pTesisList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listacursoDevolver = new List<ComplexOntologyResource>();

            foreach (ImpartedCoursesSeminarsBBDD curso in pTesisList)
            {
                ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars courseDevolver = new ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars();

                string crisIdentifier = string.Empty;

                courseDevolver.IdRoh_owner = pIdGnoss;
                courseDevolver.Roh_cvnCode = "030.060.000.000";
                courseDevolver.Roh_title = curso.title;
                courseDevolver.IdRoh_eventType = curso.eventType;
                courseDevolver.Roh_eventTypeOther = curso.eventTypeOther;
                courseDevolver.Roh_promotedByTitle = curso.promotedByTitle;
                courseDevolver.IdRoh_promotedBy = curso.promotedBy;
                courseDevolver.IdRoh_promotedByType = curso.promotedByType;
                courseDevolver.Roh_promotedByTypeOther = curso.promotedByTypeOther;
                courseDevolver.Vivo_start = curso.start;
                courseDevolver.Roh_durationHours = curso.durationHours;
                courseDevolver.IdVcard_hasCountryName = curso.hasCountryName;
                courseDevolver.IdVcard_hasRegion = curso.hasRegion;
                courseDevolver.Vcard_locality = curso.locality;
                courseDevolver.Roh_goals = curso.goals;
                courseDevolver.IdVcard_hasLanguage = curso.hasLanguage;
                courseDevolver.Roh_isbn = curso.isbn;
                courseDevolver.Bibo_issn = curso.issn;
                courseDevolver.Roh_correspondingAuthor = curso.correspondingAuthor;
                courseDevolver.Bibo_doi = curso.doi;
                courseDevolver.Bibo_handle = curso.handle;
                courseDevolver.Bibo_pmid = curso.pmid;
                // TODO Identifier
                courseDevolver.Roh_targetProfile = curso.targetProfile;
                courseDevolver.IdRoh_participationType = curso.participationType;
                courseDevolver.Roh_participationTypeOther = curso.participationTypeOther;

                // CrisIdentifier
                courseDevolver.Roh_crisIdentifier = crisIdentifier;

                listacursoDevolver.Add(courseDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listacursoDevolver;
        }

        /// <summary>
        /// Obtiene los datos que no queremos borrar de la persona.
        /// </summary>
        /// <param name="pIdRecurso"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static PersonOntology.Person DatosPersonaNoBorrar(string pIdRecurso, ResourceApi pResourceApi)
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

            SparqlObject resultadoQueryPerson = pResourceApi.VirtuosoQuery(selectPerson, wherePerson, "person");

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

            SparqlObject resultadoQueryIgnorePublications = pResourceApi.VirtuosoQuery(selectIgnorePublication, whereIgnorePublication, "person");

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

            SparqlObject resultadoQueryMetricPage = pResourceApi.VirtuosoQuery(selecMetricPage, whereMetricPage, "person");

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

                SparqlObject resultadoQueryMetricGraphic = pResourceApi.VirtuosoQuery(selectMetricGraphic, whereMetricGraphic, "person");

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
        /// Permite cargar un departamento.
        /// </summary>
        /// <param name="pCodigoDept"></param>
        /// <param name="pNombreDept"></param>
        /// <param name="pResourceApi"></param>
        private static void CargarDepartment(string pCodigoDept, string pNombreDept, ResourceApi pResourceApi)
        {
            string ontology = "department";

            // Cambio de ontología.
            pResourceApi.ChangeOntoly(ontology);

            // Creación del objeto a cargar.
            Department dept = new Department();
            dept.Dc_identifier = pCodigoDept;
            dept.Dc_title = pNombreDept;

            // Carga.
            var cargado = pResourceApi.LoadSecondaryResource(dept.ToGnossApiResource(pResourceApi, ontology + "_" + dept.Dc_identifier));
        }

        /// <summary>
        /// Comprueba si existe un departamento en BBDD.
        /// </summary>
        /// <param name="pIdentificadorDept"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static bool ComprobarDepartamentoBBDD(string pIdentificadorDept, ResourceApi pResourceApi)
        {
            string idSecundaria = $@"http://gnoss.com/items/department_{pIdentificadorDept}";

            SparqlObject resultadoQuery = null;

            // Consulta sparql.
            string select = "SELECT * ";
            string where = $@"WHERE {{ 
                                <{idSecundaria}> ?p ?o. 
                            }}";

            resultadoQuery = pResourceApi.VirtuosoQuery(select, where, "department");

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Obtiene información de la persona mediante el crisidentifier.
        /// </summary>
        /// <param name="pResourceApi"></param>
        /// <param name="pCrisIdentfierPerson"></param>
        /// <param name="pOntology"></param>
        /// <param name="pCvnCode"></param>
        /// <returns></returns>
        public List<string> ObtenerDataCrisIdentifier(ResourceApi pResourceApi, string pCrisIdentfierPerson, string pOntology, string pCvnCode)
        {
            List<string> listaDevolver = new List<string>();

            string select = string.Empty;
            string where = string.Empty;
            SparqlObject resultadoQuery = null;

            select = "SELECT ?crisIdentifier ";
            where = $@"WHERE {{ 
                                ?s <http://w3id.org/roh/owner> ?persona.
                                ?persona <http://w3id.org/roh/crisIdentifier> '{pCrisIdentfierPerson}'.
                                ?s <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                ?s <http://w3id.org/roh/cvnCode> '{pCvnCode}'.
                            }}";

            resultadoQuery = pResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", pOntology });
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    if (fila.ContainsKey("crisIdentifier") && !string.IsNullOrEmpty(fila["crisIdentifier"].value))
                    {
                        listaDevolver.Add(fila["crisIdentifier"].value);
                    }
                }
            }

            return listaDevolver;
        }

        /// <summary>
        /// Construye el objeto con los datos obtenidos de BBDD.
        /// </summary>
        /// <param name="pRabbitConf"></param>
        /// <param name="pListaImpartedAcademicSGI"></param>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public List<ImpartedAcademicTrainingBBDD> ObtenerImpartedAcademicSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<FormacionAcademicaImpartida> pListaImpartedAcademicSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<ImpartedAcademicTrainingBBDD> listaImpartedAcademic = new List<ImpartedAcademicTrainingBBDD>();

            foreach (FormacionAcademicaImpartida item in pListaImpartedAcademicSGI)
            {
                ImpartedAcademicTrainingBBDD impartedAcademic = new ImpartedAcademicTrainingBBDD();

                string crisIdentifier = "030.010.000.000___";

                impartedAcademic.title = item.TitulacionUniversitaria;
                crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.title)}___";

                impartedAcademic.teaches = item.NombreAsignaturaCurso;
                crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.teaches)}___";

                //impartedAcademic.start = item.FechaInicio;
                //impartedAcademic.end = item.FechaFinalizacion;                

                if (item.EntidadRealizacion != null && !string.IsNullOrEmpty(item.EntidadRealizacion.EntidadRef))
                {
                    Dictionary<string, string> dicOrganizaciones = Empresa.ObtenerOrganizacionesBBDD(new HashSet<string>() { item.EntidadRealizacion.EntidadRef }, pResourceApi);
                    Dictionary<string, string> dicOrganizacionesCargadas = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> organizacion in dicOrganizaciones)
                    {
                        Empresa organizacionAux = Empresa.GetOrganizacionSGI(pHarvesterServices, pConfig, "Organizacion_" + organizacion.Key, pDicRutas);
                        if (organizacionAux != null)
                        {
                            impartedAcademic.promotedByTitle = organizacionAux.Nombre;
                            crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.promotedByTitle)}___";

                            if (string.IsNullOrEmpty(organizacion.Value))
                            {
                                string idGnoss = organizacionAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["organization"].Add(idGnoss);
                                dicOrganizacionesCargadas[organizacion.Key] = idGnoss;
                            }
                            else
                            {
                                dicOrganizacionesCargadas[organizacion.Key] = organizacion.Value;
                            }

                            impartedAcademic.promotedBy = dicOrganizacionesCargadas[item.EntidadRealizacion.EntidadRef];
                        }
                    }
                }

                if (item.TipoDocente != null && !string.IsNullOrEmpty(item.TipoDocente.Nombre))
                {
                    impartedAcademic.teachingType = TeachingType(item.TipoDocente.Nombre, pResourceApi);
                }

                // Código de países.
                if (item.PaisEntidadRealizacion != null && !string.IsNullOrEmpty(item.PaisEntidadRealizacion.Id))
                {
                    impartedAcademic.hasCountryName = IdentificadorPais(item.PaisEntidadRealizacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.PaisEntidadRealizacion.Id}___";
                }

                if (item.CcaaRegionEntidadRealizacion != null && !string.IsNullOrEmpty(item.CcaaRegionEntidadRealizacion.Id))
                {
                    impartedAcademic.hasRegion = IdentificadorRegion(item.CcaaRegionEntidadRealizacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.CcaaRegionEntidadRealizacion.Id}___";
                }

                if (!string.IsNullOrEmpty(item.CiudadEntidadRealizacion))
                {
                    impartedAcademic.locality = item.CiudadEntidadRealizacion;
                    crisIdentifier += $@"{item.CiudadEntidadRealizacion}___";
                }

                impartedAcademic.numberECTSHours = item.NumHorasCreditos.Value;
                crisIdentifier += $@"{impartedAcademic.numberECTSHours}___";

                if (item.FrecuenciaActividad.HasValue)
                {
                    impartedAcademic.frequency = item.FrecuenciaActividad.Value;
                    crisIdentifier += $@"{impartedAcademic.frequency}___";
                }

                if (item.TipoPrograma != null && !string.IsNullOrEmpty(item.TipoPrograma.Nombre))
                {
                    impartedAcademic.programType = ProgramType(item.TipoPrograma.Nombre, pResourceApi);
                    if (impartedAcademic.programType.Contains("OTHERS"))
                    {
                        impartedAcademic.programTypeOther = item.TipoPrograma.Nombre;
                        crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.programTypeOther)}___";
                    }
                }

                if (item.TipoDocencia != null && !string.IsNullOrEmpty(item.TipoDocencia.Nombre))
                {
                    impartedAcademic.modalityTeachingType = ModalityTeachingType(item.TipoDocencia.Nombre, pResourceApi);
                    if (impartedAcademic.modalityTeachingType.Contains("OTHERS"))
                    {
                        impartedAcademic.modalityTeachingTypeOther = item.TipoDocencia.Nombre;
                        crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.modalityTeachingTypeOther)}___";
                    }
                }

                impartedAcademic.department = item.Departamento;
                crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.department)}___";

                if (item.TipoAsignatura != null && !string.IsNullOrEmpty(item.TipoAsignatura.Nombre))
                {
                    impartedAcademic.courseType = CourseType(item.TipoAsignatura.Nombre, pResourceApi);
                    if (impartedAcademic.courseType.Contains("OTHERS"))
                    {
                        impartedAcademic.courseTypeOther = item.TipoAsignatura.Nombre;
                        crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.courseTypeOther)}___";
                    }
                }

                impartedAcademic.course = item.Curso;
                crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.course)}___";

                if (item.TipoHorasCreditos != null && !string.IsNullOrEmpty(item.TipoHorasCreditos.Nombre))
                {
                    impartedAcademic.hoursCreditsECTSType = HoursCreditsECTSType(item.TipoHorasCreditos.Nombre, pResourceApi);
                }

                // TODO: Idioma

                impartedAcademic.competencies = item.Competencias;
                crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.competencies)}___";
                impartedAcademic.professionalCategory = item.CategoriaProfesional;
                crisIdentifier += $@"{RemoveDiacritics(impartedAcademic.professionalCategory)}___";

                // CrisIdentifier
                impartedAcademic.crisIdentifier = crisIdentifier;

                listaImpartedAcademic.Add(impartedAcademic);
            }

            return listaImpartedAcademic;
        }

        /// <summary>
        /// Construye el objeto con los datos obtenidos de BBDD.
        /// </summary>
        /// <param name="pRabbitConf"></param>
        /// <param name="pListaTesisSGI"></param>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public List<TesisBBDD> ObtenerTesisSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<Tesis> pListaTesisSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<TesisBBDD> listaTesis = new List<TesisBBDD>();

            foreach (Tesis item in pListaTesisSGI)
            {
                TesisBBDD tesis = new TesisBBDD();
                string crisIdentifier = "030.040.000.000___";

                if (item.TipoProyecto != null && !string.IsNullOrEmpty(item.TipoProyecto.Nombre))
                {
                    tesis.projectCharacterType = ProjectCharacterType(pResourceApi, item.TipoProyecto.Nombre);
                    if (tesis.projectCharacterType.Contains("OTHERS"))
                    {
                        tesis.projectCharacterTypeOther = item.TipoProyecto.Nombre;
                        crisIdentifier += $@"{RemoveDiacritics(tesis.projectCharacterTypeOther)}___";
                    }
                }

                tesis.title = item.TituloTrabajo;
                crisIdentifier += $@"{RemoveDiacritics(tesis.title)}___";

                // Código de países.
                if (item.PaisEntidadRealizacion != null && !string.IsNullOrEmpty(item.PaisEntidadRealizacion.Id))
                {
                    tesis.hasCountryName = IdentificadorPais(item.PaisEntidadRealizacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.PaisEntidadRealizacion.Id}___";
                }

                if (item.CcaaRegionEntidadRealizacion != null && !string.IsNullOrEmpty(item.CcaaRegionEntidadRealizacion.Id))
                {
                    tesis.hasRegion = IdentificadorRegion(item.CcaaRegionEntidadRealizacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.CcaaRegionEntidadRealizacion.Id}___";
                }

                if (!string.IsNullOrEmpty(item.CiudadEntidadRealizacion))
                {
                    tesis.locality = item.CiudadEntidadRealizacion;
                    crisIdentifier += $@"{item.CiudadEntidadRealizacion}___";
                }

                tesis.issued = item.FechaDefensa;
                tesis.qualification = item.CalificacionObtenida;
                crisIdentifier += $@"{RemoveDiacritics(tesis.qualification)}___";

                tesis.europeanDoctorateDate = item.FechaMencionDoctoradoEuropeo;
                tesis.qualityMention = item.MencionCalidad != null ? (bool)item.MencionCalidad : false;
                crisIdentifier += $@"{tesis.qualityMention}___";

                tesis.europeanDoctorate = item.DoctoradoEuropeo != null ? (bool)item.DoctoradoEuropeo : false;
                crisIdentifier += $@"{tesis.europeanDoctorate}___";

                tesis.qualityMentionDate = item.FechaMencionCalidad;

                Persona alumno = GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + item.Alumno, pDicRutas);
                if (alumno != null)
                {
                    tesis.studentName = alumno.Nombre;
                    tesis.studentFirstSurname = alumno.Apellidos;
                    tesis.studentNick = (alumno.Nombre + " " + alumno.Apellidos).Trim();
                    crisIdentifier += $@"{RemoveDiacritics(tesis.studentNick)}___";
                }

                // ORGANIZACION
                if (item.EntidadRealizacion != null && !string.IsNullOrEmpty(item.EntidadRealizacion.EntidadRef))
                {
                    Dictionary<string, string> dicOrganizaciones = Empresa.ObtenerOrganizacionesBBDD(new HashSet<string>() { item.EntidadRealizacion.EntidadRef }, pResourceApi);
                    Dictionary<string, string> dicOrganizacionesCargadas = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> organizacion in dicOrganizaciones)
                    {
                        Empresa organizacionAux = Empresa.GetOrganizacionSGI(pHarvesterServices, pConfig, "Organizacion_" + organizacion.Key, pDicRutas);
                        if (organizacionAux != null)
                        {
                            tesis.promotedByTitle = organizacionAux.Nombre;
                            crisIdentifier += $@"{RemoveDiacritics(tesis.promotedByTitle)}___";

                            if (string.IsNullOrEmpty(organizacion.Value))
                            {
                                string idGnoss = organizacionAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["organization"].Add(idGnoss);
                                dicOrganizacionesCargadas[organizacion.Key] = idGnoss;
                            }
                            else
                            {
                                dicOrganizacionesCargadas[organizacion.Key] = organizacion.Value;
                            }

                            tesis.promotedBy = dicOrganizacionesCargadas[item.EntidadRealizacion.EntidadRef];
                        }
                    }
                }

                if (item.CoDirectorTesis != null && item.CoDirectorTesis.Count > 0)
                {
                    int orden = 1;
                    tesis.codirector = new List<Codirector>();
                    foreach (Director director in item.CoDirectorTesis)
                    {
                        if (!string.IsNullOrEmpty(director.PersonaRef))
                        {
                            Persona codirectorSGI = GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + director.PersonaRef, pDicRutas);
                            if (codirectorSGI != null)
                            {
                                Codirector codirector = new Codirector();
                                codirector.firstName = codirectorSGI.Nombre;
                                codirector.secondFamilyName = codirectorSGI.Apellidos;
                                codirector.nick = (codirectorSGI.Nombre + " " + codirectorSGI.Apellidos).Trim();
                                codirector.comment = orden;
                                tesis.codirector.Add(codirector);
                                orden++;
                            }
                        }
                    }
                }

                // CrisIdentifier
                tesis.crisIdentifier = crisIdentifier;

                listaTesis.Add(tesis);
            }

            return listaTesis;
        }

        /// <summary>
        /// Construye el objeto con los datos obtenidos de BBDD.
        /// </summary>
        /// <param name="pRabbitConf"></param>
        /// <param name="pListaCursosSGI"></param>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public List<ImpartedCoursesSeminarsBBDD> ObtenerCursosSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<SeminariosCursos> pListaCursosSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<ImpartedCoursesSeminarsBBDD> listaCursos = new List<ImpartedCoursesSeminarsBBDD>();

            foreach (SeminariosCursos item in pListaCursosSGI)
            {
                ImpartedCoursesSeminarsBBDD curso = new ImpartedCoursesSeminarsBBDD();

                string crisIdentifier = "030.060.000.000___";

                curso.title = item.NombreEvento;
                crisIdentifier += $@"{RemoveDiacritics(item.NombreEvento)}___";

                curso.goals = item.ObjetivosCurso;
                crisIdentifier += $@"{RemoveDiacritics(item.ObjetivosCurso)}___";

                curso.targetProfile = item.PerfilDestinatarios;
                crisIdentifier += $@"{RemoveDiacritics(item.PerfilDestinatarios)}___";

                curso.hasLanguage = item.Idioma;
                crisIdentifier += $@"{RemoveDiacritics(item.Idioma)}___";

                curso.start = item.FechaTitulacion;

                if (item.TipoParticipacion != null && !string.IsNullOrEmpty(item.TipoParticipacion.Nombre))
                {
                    curso.participationType = item.TipoParticipacion.Nombre;
                    crisIdentifier += $@"{RemoveDiacritics(item.TipoParticipacion.Nombre)}___";
                }

                curso.correspondingAuthor = item.AutorCorrespondencia != null ? (bool)item.AutorCorrespondencia : false;
                crisIdentifier += $@"{curso.correspondingAuthor}___";

                // ENTIDAD ORGANIZADORA
                if (item.EntidadOrganizacionEvento != null && !string.IsNullOrEmpty(item.EntidadOrganizacionEvento.EntidadRef))
                {
                    Dictionary<string, string> dicOrganizaciones = Empresa.ObtenerOrganizacionesBBDD(new HashSet<string>() { item.EntidadOrganizacionEvento.EntidadRef }, pResourceApi);
                    Dictionary<string, string> dicOrganizacionesCargadas = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> organizacion in dicOrganizaciones)
                    {
                        Empresa organizacionAux = Empresa.GetOrganizacionSGI(pHarvesterServices, pConfig, "Organizacion_" + organizacion.Key, pDicRutas);
                        if (organizacionAux != null)
                        {
                            curso.promotedByTitle = organizacionAux.Nombre;
                            crisIdentifier += $@"{RemoveDiacritics(organizacionAux.Nombre)}___";

                            if (string.IsNullOrEmpty(organizacion.Value))
                            {
                                string idGnoss = organizacionAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["organization"].Add(idGnoss);
                                dicOrganizacionesCargadas[organizacion.Key] = idGnoss;
                            }
                            else
                            {
                                dicOrganizacionesCargadas[organizacion.Key] = organizacion.Value;
                            }

                            curso.promotedBy = dicOrganizacionesCargadas[item.EntidadOrganizacionEvento.EntidadRef];
                        }
                    }
                }

                // Código de países.
                if (item.PaisEntidadOrganizacionEvento != null && !string.IsNullOrEmpty(item.PaisEntidadOrganizacionEvento.Id))
                {
                    curso.hasCountryName = IdentificadorPais(item.PaisEntidadOrganizacionEvento.Id, pResourceApi);
                    crisIdentifier += $@"{item.PaisEntidadOrganizacionEvento.Id}___";
                }

                if (item.CcaaRegionEntidadOrganizacionEvento != null && !string.IsNullOrEmpty(item.CcaaRegionEntidadOrganizacionEvento.Id))
                {
                    curso.hasRegion = IdentificadorRegion(item.CcaaRegionEntidadOrganizacionEvento.Id, pResourceApi);
                    crisIdentifier += $@"{item.CcaaRegionEntidadOrganizacionEvento.Id}___";
                }

                // Código de países.
                if (!string.IsNullOrEmpty(item.CiudadEntidadOrganizacionEvento))
                {
                    curso.locality = item.CiudadEntidadOrganizacionEvento;
                    crisIdentifier += $@"{item.CiudadEntidadOrganizacionEvento}___";
                }

                curso.isbn = item.ISBN;
                crisIdentifier += $@"{RemoveDiacritics(item.ISBN)}___";

                curso.issn = item.ISSN;
                crisIdentifier += $@"{RemoveDiacritics(item.ISSN)}___";

                // CrisIdentifier.
                curso.crisIdentifiers = crisIdentifier;

                listaCursos.Add(curso);
            }

            return listaCursos;
        }

        /// <summary>
        /// Construye el objeto con los datos obtenidos de BBDD.
        /// </summary>
        /// <param name="pRabbitConf"></param>
        /// <param name="pListaCiclosSGI"></param>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public List<CiclosBBDD> ObtenerCiclosSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<Ciclos> pListaCiclosSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<CiclosBBDD> listaCiclos = new List<CiclosBBDD>();

            foreach (Ciclos item in pListaCiclosSGI)
            {
                CiclosBBDD ciclo = new CiclosBBDD();

                string crisIdentifier = "020.010.010.000___";

                ciclo.nombreTitulo = item.NombreTitulo;
                crisIdentifier += $@"{RemoveDiacritics(item.NombreTitulo)}___";

                if (item.EntidadTitulacion != null && !string.IsNullOrEmpty(item.EntidadTitulacion.EntidadRef))
                {
                    Dictionary<string, string> dicOrganizaciones = Empresa.ObtenerOrganizacionesBBDD(new HashSet<string>() { item.EntidadTitulacion.EntidadRef }, pResourceApi);
                    Dictionary<string, string> dicOrganizacionesCargadas = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> organizacion in dicOrganizaciones)
                    {
                        Empresa organizacionAux = Empresa.GetOrganizacionSGI(pHarvesterServices, pConfig, "Organizacion_" + organizacion.Key, pDicRutas);
                        if (organizacionAux != null)
                        {
                            ciclo.entidadTitulacionTitulo = organizacionAux.Nombre;
                            crisIdentifier += $@"{RemoveDiacritics(ciclo.entidadTitulacionTitulo)}___";

                            if (string.IsNullOrEmpty(organizacion.Value))
                            {
                                string idGnoss = organizacionAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["organization"].Add(idGnoss);
                                dicOrganizacionesCargadas[organizacion.Key] = idGnoss;
                            }
                            else
                            {
                                dicOrganizacionesCargadas[organizacion.Key] = organizacion.Value;
                            }

                            ciclo.entidadTitulacion = dicOrganizacionesCargadas[item.EntidadTitulacion.EntidadRef];
                        }
                    }
                }

                // Código de países.
                if (item.PaisEntidadTitulacion != null && !string.IsNullOrEmpty(item.PaisEntidadTitulacion.Id))
                {
                    ciclo.cAutonEntidadTitulacion = IdentificadorPais(item.PaisEntidadTitulacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.PaisEntidadTitulacion.Id}___";
                }

                if (item.CcaaRegionEntidadTitulacion != null && !string.IsNullOrEmpty(item.CcaaRegionEntidadTitulacion.Id))
                {
                    ciclo.paisEntidadTitulacion = IdentificadorRegion(item.CcaaRegionEntidadTitulacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.CcaaRegionEntidadTitulacion.Id}___";
                }

                if (!string.IsNullOrEmpty(item.CiudadEntidadTitulacion))
                {
                    ciclo.ciudadEntidadTitulacion = item.CiudadEntidadTitulacion;
                    crisIdentifier += $@"{item.CiudadEntidadTitulacion}___";
                }


                ciclo.fechaTitulacion = item.FechaTitulacion;

                // TODO: NOTA MEDIA

                if (item.Premio != null && !string.IsNullOrEmpty(item.Premio.Nombre))
                {
                    ciclo.premio = PrizeType(pResourceApi, item.Premio.Nombre);
                    crisIdentifier += $@"{RemoveDiacritics(item.Premio.Nombre)}___";

                    if (ciclo.premio.Contains("OTHERS"))
                    {
                        ciclo.premioOther = item.Premio.Nombre;
                        crisIdentifier += $@"{RemoveDiacritics(ciclo.premioOther)}___";
                    }
                }

                if (item.TitulacionUniversitaria != null && !string.IsNullOrEmpty(item.TitulacionUniversitaria.Nombre))
                {
                    ciclo.titulacionUni = UniversityDegreeType(pResourceApi, item.TitulacionUniversitaria.Nombre);
                    crisIdentifier += $@"{RemoveDiacritics(item.TitulacionUniversitaria.Nombre)}___";

                    if (ciclo.titulacionUni.Contains("OTHERS"))
                    {
                        ciclo.titulacionUniOtros = item.TitulacionUniversitaria.Nombre;
                        crisIdentifier += $@"{RemoveDiacritics(ciclo.titulacionUniOtros)}___";
                    }
                }

                ciclo.tituloExtranjero = item.TituloExtranjero;
                crisIdentifier += $@"{RemoveDiacritics(item.TituloExtranjero)}___";

                ciclo.tituloHomologado = item.TituloHomologado != null ? (bool)item.TituloHomologado : false;
                ciclo.fechaHomologacion = item.FechaHomologacion;

                ciclo.crisIdentifier = crisIdentifier;

                listaCiclos.Add(ciclo);
            }

            return listaCiclos;
        }

        /// <summary>
        /// Construye el objeto con los datos obtenidos de BBDD.
        /// </summary>
        /// <param name="pRabbitConf"></param>
        /// <param name="pListaDoctoradosSGI"></param>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public List<DoctoradosBBDD> ObtenerDoctoradosSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<Doctorados> pListaDoctoradosSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<DoctoradosBBDD> listaDoctorados = new List<DoctoradosBBDD>();

            foreach (Doctorados item in pListaDoctoradosSGI)
            {
                DoctoradosBBDD doctorado = new DoctoradosBBDD();

                string crisIdentifier = $@"020.010.020.000___";

                doctorado.nombreTitulo = item.ProgramaDoctorado;
                crisIdentifier += $@"{RemoveDiacritics(doctorado.nombreTitulo)}___";

                if (item.EntidadTitulacion != null && !string.IsNullOrEmpty(item.EntidadTitulacion.EntidadRef))
                {
                    Dictionary<string, string> dicOrganizaciones = Empresa.ObtenerOrganizacionesBBDD(new HashSet<string>() { item.EntidadTitulacion.EntidadRef }, pResourceApi);
                    Dictionary<string, string> dicOrganizacionesCargadas = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> organizacion in dicOrganizaciones)
                    {
                        Empresa organizacionAux = Empresa.GetOrganizacionSGI(pHarvesterServices, pConfig, "Organizacion_" + organizacion.Key, pDicRutas);
                        if (organizacionAux != null)
                        {
                            doctorado.entidadTitulacionTitulo = organizacionAux.Nombre;
                            crisIdentifier += $@"{RemoveDiacritics(doctorado.entidadTitulacionTitulo)}___";

                            if (string.IsNullOrEmpty(organizacion.Value))
                            {
                                string idGnoss = organizacionAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["organization"].Add(idGnoss);
                                dicOrganizacionesCargadas[organizacion.Key] = idGnoss;
                            }
                            else
                            {
                                dicOrganizacionesCargadas[organizacion.Key] = organizacion.Value;
                            }

                            doctorado.entidadTitulacion = dicOrganizacionesCargadas[item.EntidadTitulacion.EntidadRef];
                        }
                    }
                }

                doctorado.fechaTitulacion = item.FechaTitulacion;

                if (item.PaisEntidadTitulacion != null && !string.IsNullOrEmpty(item.PaisEntidadTitulacion.Id))
                {
                    doctorado.paisEntidadTitulacion = IdentificadorPais(item.PaisEntidadTitulacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.PaisEntidadTitulacion.Id}___";
                }

                if (item.CcaaRegionEntidadTitulacion != null && !string.IsNullOrEmpty(item.CcaaRegionEntidadTitulacion.Id))
                {
                    doctorado.cAutonEntidadTitulacion = IdentificadorRegion(item.CcaaRegionEntidadTitulacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.CcaaRegionEntidadTitulacion.Id}___";
                }

                if (!string.IsNullOrEmpty(item.CiudadEntidadTitulacion))
                {
                    doctorado.ciudadEntidadTitulacion = item.CiudadEntidadTitulacion;
                    crisIdentifier += $@"{item.CiudadEntidadTitulacion}___";
                }

                if (item.EntidadTitulacionDEA != null && !string.IsNullOrEmpty(item.EntidadTitulacionDEA.EntidadRef))
                {
                    Dictionary<string, string> dicOrganizaciones = Empresa.ObtenerOrganizacionesBBDD(new HashSet<string>() { item.EntidadTitulacionDEA.EntidadRef }, pResourceApi);
                    Dictionary<string, string> dicOrganizacionesCargadas = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> organizacion in dicOrganizaciones)
                    {
                        Empresa organizacionAux = Empresa.GetOrganizacionSGI(pHarvesterServices, pConfig, "Organizacion_" + organizacion.Key, pDicRutas);
                        if (organizacionAux != null)
                        {
                            doctorado.entidadTitDEATitulo = organizacionAux.Nombre;
                            crisIdentifier += $@"{RemoveDiacritics(doctorado.entidadTitDEATitulo)}___";

                            if (string.IsNullOrEmpty(organizacion.Value))
                            {
                                string idGnoss = organizacionAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["organization"].Add(idGnoss);
                                dicOrganizacionesCargadas[organizacion.Key] = idGnoss;
                            }
                            else
                            {
                                dicOrganizacionesCargadas[organizacion.Key] = organizacion.Value;
                            }

                            doctorado.entidadTitDEA = dicOrganizacionesCargadas[item.EntidadTitulacionDEA.EntidadRef];
                        }
                    }
                }

                doctorado.obtencionDEA = item.FechaTitulacionDEA;
                doctorado.tituloTesis = item.TituloTesis;
                crisIdentifier += $@"{RemoveDiacritics(doctorado.tituloTesis)}___";
                doctorado.calificacionObtenida = item.CalificacionObtenida;
                crisIdentifier += $@"{RemoveDiacritics(doctorado.calificacionObtenida)}___";

                Persona director = GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + item.DirectorTesis, pDicRutas);
                if (director != null)
                {
                    doctorado.nombreDirector = director.Nombre;
                    doctorado.primApeDirector = director.Apellidos;
                    doctorado.firmaDirector = (director.Nombre + " " + director.Apellidos).Trim();
                    crisIdentifier += $@"{RemoveDiacritics(doctorado.firmaDirector)}___";
                }

                if (item.CoDirectorTesis != null && item.CoDirectorTesis.Count > 0)
                {
                    int orden = 1;
                    doctorado.codirectorTesis = new List<DoctoradosBBDD.Codirector>();
                    foreach (Director codirectortesis in item.CoDirectorTesis)
                    {
                        if (!string.IsNullOrEmpty(codirectortesis.PersonaRef))
                        {
                            Persona codirectorSGI = GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + codirectortesis.PersonaRef, pDicRutas);
                            if (codirectorSGI != null)
                            {
                                DoctoradosBBDD.Codirector codirector = new DoctoradosBBDD.Codirector();
                                codirector.firstName = codirectorSGI.Nombre;
                                codirector.secondFamilyName = codirectorSGI.Apellidos;
                                codirector.nick = (codirectorSGI.Nombre + " " + codirectorSGI.Apellidos).Trim();
                                codirector.comment = orden;
                                doctorado.codirectorTesis.Add(codirector);
                                orden++;
                            }
                        }
                    }
                }

                doctorado.doctoradoEuropeo = item.DoctoradoEuropeo != null ? (bool)item.DoctoradoEuropeo : false;
                crisIdentifier += $@"{doctorado.doctoradoEuropeo}___";
                doctorado.fechaDoctorado = item.FechaMencionDoctoradoEuropeo;
                doctorado.mencionCalidad = item.MencionCalidad != null ? (bool)item.MencionCalidad : false;
                crisIdentifier += $@"{doctorado.mencionCalidad}___";
                doctorado.premioExtraordinarioDoctor = item.PremioExtraordinarioDoctor != null ? (bool)item.PremioExtraordinarioDoctor : false;
                crisIdentifier += $@"{doctorado.premioExtraordinarioDoctor}___";
                doctorado.fechaPremioDoctor = item.FechaPremioExtraordinarioDoctor;
                doctorado.tituloHomologado = item.TituloHomologado != null ? (bool)item.TituloHomologado : false;
                crisIdentifier += $@"{doctorado.tituloHomologado}___";
                doctorado.fechaHomologado = item.FechaHomologacion;

                // CrisIdentifier
                doctorado.crisIdentifier = crisIdentifier;

                listaDoctorados.Add(doctorado);
            }

            return listaDoctorados;
        }

        /// <summary>
        /// Construye el objeto con los datos obtenidos de BBDD.
        /// </summary>
        /// <param name="pRabbitConf"></param>
        /// <param name="pListaPosgradosSGI"></param>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public List<PosgradoBBDD> ObtenerPosgradosSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<Posgrado> pListaPosgradosSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<PosgradoBBDD> listaPosgrados = new List<PosgradoBBDD>();

            foreach (Posgrado item in pListaPosgradosSGI)
            {
                PosgradoBBDD posgrado = new PosgradoBBDD();

                string crisIdentifier = string.Empty;

                posgrado.nombreTitulo = item.NombreTituloPosgrado;
                crisIdentifier += $@"{RemoveDiacritics(posgrado.nombreTitulo)}___";

                if (item.EntidadTitulacion != null && !string.IsNullOrEmpty(item.EntidadTitulacion.EntidadRef))
                {
                    Dictionary<string, string> dicOrganizaciones = Empresa.ObtenerOrganizacionesBBDD(new HashSet<string>() { item.EntidadTitulacion.EntidadRef }, pResourceApi);
                    Dictionary<string, string> dicOrganizacionesCargadas = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> organizacion in dicOrganizaciones)
                    {
                        Empresa organizacionAux = Empresa.GetOrganizacionSGI(pHarvesterServices, pConfig, "Organizacion_" + organizacion.Key, pDicRutas);
                        if (organizacionAux != null)
                        {
                            posgrado.entidadTitulacionTitulo = organizacionAux.Nombre;
                            crisIdentifier += $@"{RemoveDiacritics(posgrado.entidadTitulacionTitulo)}___";

                            if (string.IsNullOrEmpty(organizacion.Value))
                            {
                                string idGnoss = organizacionAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["organization"].Add(idGnoss);
                                dicOrganizacionesCargadas[organizacion.Key] = idGnoss;
                            }
                            else
                            {
                                dicOrganizacionesCargadas[organizacion.Key] = organizacion.Value;
                            }

                            posgrado.entidadTitulacion = dicOrganizacionesCargadas[item.EntidadTitulacion.EntidadRef];
                        }
                    }
                }

                posgrado.fechaTitulacion = item.FechaTitulacion;

                if (item.PaisEntidadTitulacion != null && !string.IsNullOrEmpty(item.PaisEntidadTitulacion.Id))
                {
                    posgrado.paisEntidadTitulacion = IdentificadorPais(item.PaisEntidadTitulacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.PaisEntidadTitulacion.Id}___";
                }

                if (item.CcaaRegionEntidadTitulacion != null && !string.IsNullOrEmpty(item.CcaaRegionEntidadTitulacion.Id))
                {
                    posgrado.cAutonEntidadTitulacion = IdentificadorRegion(item.CcaaRegionEntidadTitulacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.CcaaRegionEntidadTitulacion.Id}___";
                }

                if (!string.IsNullOrEmpty(item.CiudadEntidadTitulacion))
                {
                    posgrado.ciudadEntidadTitulacion = item.CiudadEntidadTitulacion;
                    crisIdentifier += $@"{item.CiudadEntidadTitulacion}___";
                }

                if (item.TipoFormacionHomologada != null && !string.IsNullOrEmpty(item.TipoFormacionHomologada.Nombre))
                {
                    posgrado.tipoFormacion = FormationType(item.TipoFormacionHomologada.Nombre, pResourceApi);
                }

                posgrado.calificacionObtenida = item.CalificacionObtenida;
                crisIdentifier += $@"{posgrado.calificacionObtenida}___";
                posgrado.tituloHomologado = item.TituloHomologado != null ? (bool)item.TituloHomologado : false;
                crisIdentifier += $@"{posgrado.tituloHomologado}___";
                posgrado.fechaHomologacion = item.FechaHomologacion;

                // CrisIdentifier
                posgrado.crisIdentifier = crisIdentifier;

                listaPosgrados.Add(posgrado);
            }

            return listaPosgrados;
        }

        /// <summary>
        /// Construye el objeto con los datos obtenidos de BBDD.
        /// </summary>
        /// <param name="pRabbitConf"></param>
        /// <param name="pListaEspecializadaSGI"></param>
        /// <param name="pHarvesterServices"></param>
        /// <param name="pConfig"></param>
        /// <param name="pResourceApi"></param>
        /// <param name="pDicIdentificadores"></param>
        /// <param name="pDicRutas"></param>
        /// <returns></returns>
        public List<FormacionEspecializadaBBDD> ObtenerFormacionEspecializadaSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<FormacionEspecializada> pListaEspecializadaSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<FormacionEspecializadaBBDD> listaFormEspecializada = new List<FormacionEspecializadaBBDD>();

            foreach (FormacionEspecializada item in pListaEspecializadaSGI)
            {
                FormacionEspecializadaBBDD formEspecializada = new FormacionEspecializadaBBDD();

                string crisIdentifier = "020.020.000.000___";

                formEspecializada.nombreTitulo = item.NombreTitulo;
                crisIdentifier += $@"{RemoveDiacritics(formEspecializada.nombreTitulo)}___";

                if (item.EntidadTitulacion != null && !string.IsNullOrEmpty(item.EntidadTitulacion.EntidadRef))
                {
                    Dictionary<string, string> dicOrganizaciones = Empresa.ObtenerOrganizacionesBBDD(new HashSet<string>() { item.EntidadTitulacion.EntidadRef }, pResourceApi);
                    Dictionary<string, string> dicOrganizacionesCargadas = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> organizacion in dicOrganizaciones)
                    {
                        Empresa organizacionAux = Empresa.GetOrganizacionSGI(pHarvesterServices, pConfig, "Organizacion_" + organizacion.Key, pDicRutas);
                        if (organizacionAux != null)
                        {
                            formEspecializada.entidadTitulacionTitulo = organizacionAux.Nombre;
                            crisIdentifier += $@"{RemoveDiacritics(formEspecializada.entidadTitulacionTitulo)}___";

                            if (string.IsNullOrEmpty(organizacion.Value))
                            {
                                string idGnoss = organizacionAux.Cargar(pHarvesterServices, pConfig, pResourceApi, "organization", pDicIdentificadores, pDicRutas, pRabbitConf);
                                pDicIdentificadores["organization"].Add(idGnoss);
                                dicOrganizacionesCargadas[organizacion.Key] = idGnoss;
                            }
                            else
                            {
                                dicOrganizacionesCargadas[organizacion.Key] = organizacion.Value;
                            }

                            formEspecializada.entidadTitulacion = dicOrganizacionesCargadas[item.EntidadTitulacion.EntidadRef];
                        }
                    }
                }

                formEspecializada.fechaFinalizacion = item.FechaTitulacion;

                if (item.PaisEntidadTitulacion != null && !string.IsNullOrEmpty(item.PaisEntidadTitulacion.Id))
                {
                    formEspecializada.paisEntidadTitulacion = IdentificadorPais(item.PaisEntidadTitulacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.PaisEntidadTitulacion.Id}___";
                }

                if (item.CcaaRegionEntidadTitulacion != null && !string.IsNullOrEmpty(item.CcaaRegionEntidadTitulacion.Id))
                {
                    formEspecializada.cAutonEntidadTitulacion = IdentificadorRegion(item.CcaaRegionEntidadTitulacion.Id, pResourceApi);
                    crisIdentifier += $@"{item.CcaaRegionEntidadTitulacion.Id}___";
                }

                if (!string.IsNullOrEmpty(item.CiudadEntidadTitulacion))
                {
                    formEspecializada.ciudadEntidadTitulacion = item.CiudadEntidadTitulacion;
                    crisIdentifier += $@"{item.CiudadEntidadTitulacion}___";
                }

                if (item.DuracionTitulacion.HasValue)
                {
                    formEspecializada.duracionHoras = item.DuracionTitulacion.Value;
                    crisIdentifier += $@"{formEspecializada.duracionHoras}___";
                }

                if (item.TipoFormacion != null && !string.IsNullOrEmpty(item.TipoFormacion.Nombre))
                {
                    formEspecializada.tipoFormacion = FormationType(item.TipoFormacion.Nombre, pResourceApi);
                }

                formEspecializada.objetivosEntidad = item.Objetivos;
                crisIdentifier += $@"{RemoveDiacritics(formEspecializada.objetivosEntidad)}___";

                if (item.ResponsableFormacion != null)
                {
                    formEspecializada.firma = item.ResponsableFormacion.NombreCompleto;
                    crisIdentifier += $@"{RemoveDiacritics(formEspecializada.firma)}___";
                    formEspecializada.nombre = item.ResponsableFormacion.Nombre;
                    formEspecializada.primApe = item.ResponsableFormacion.PrimerApellido;
                    formEspecializada.segunApe = item.ResponsableFormacion.SegundoApellido;
                }

                // CrisIdentifier
                formEspecializada.crisIdentifier = crisIdentifier;

                listaFormEspecializada.Add(formEspecializada);
            }

            return listaFormEspecializada;
        }

        /// <summary>
        /// Limpia un texto.
        /// </summary>
        /// <param name="pText"></param>
        /// <returns></returns>
        static string RemoveDiacritics(string pText)
        {
            if (string.IsNullOrEmpty(pText))
            {
                return pText;
            }

            string text = pText.Trim().ToLower();

            const string removeChars = "?&^$#@!()+-,:;<>’\'\"._*";
            StringBuilder sb = new StringBuilder(text.Length);
            foreach (char x in text.Where(c => !removeChars.Contains(c)))
            {
                sb.Append(x);
            }

            text = sb.ToString();
            text = text.Replace(" ", "-");

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="tipoFormacion"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static string FormationType(string tipoFormacion, ResourceApi pResourceApi)
        {
            string id = pResourceApi.GraphsUrl + "items/formationtype_";
            switch (tipoFormacion.ToLower())
            {
                case "máster":
                    id += "034";
                    break;
                case "master":
                    id += "034";
                    break;
                case "postgrado":
                    id += "050";
                    break;
                case "posgrado":
                    id += "050";
                    break;
                case "extensión universitaria":
                    id += "178";
                    break;
                case "especialidad":
                    id += "179";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="hoursCreditsECTSType"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static string HoursCreditsECTSType(string hoursCreditsECTSType, ResourceApi pResourceApi)
        {
            string id = pResourceApi.GraphsUrl + "items/hourscreditsectstype_";
            switch (hoursCreditsECTSType)
            {
                case "Creditos":
                    id += "000";
                    break;
                case "Horas":
                    id += "010";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="courseType"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static string CourseType(string courseType, ResourceApi pResourceApi)
        {
            string id = pResourceApi.GraphsUrl + "items/coursetype_";
            switch (courseType.ToLower())
            {
                case "troncal":
                    id += "000";
                    break;
                case "optativa":
                    id += "020";
                    break;
                case "obligatoria":
                    id += "010";
                    break;
                case "libre configuración":
                    id += "030";
                    break;
                case "doctorado/a":
                    id += "050";
                    break;
                default:
                    id += "OTHERS";
                    break;
            }
            return id;
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="modalityTeachingType"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static string ModalityTeachingType(string modalityTeachingType, ResourceApi pResourceApi)
        {
            string id = pResourceApi.GraphsUrl + "items/modalityteachingtype_";
            switch (modalityTeachingType.ToLower())
            {
                case "clínico":
                    id += "060";
                    break;
                case "prácticas de laboratorio":
                    id += "700";
                    break;
                case "práctica (aula-problemas)":
                    id += "705";
                    break;
                case "teórica presencial":
                    id += "840";
                    break;
                case "virtual":
                    id += "860";
                    break;
                default:
                    id += "OTHERS";
                    break;
            }
            return id;
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="programType"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static string ProgramType(string programType, ResourceApi pResourceApi)
        {
            string id = pResourceApi.GraphsUrl + "items/programtype_";
            switch (programType.ToLower())
            {
                case "arquitectura":
                    id += "020";
                    break;
                case "arquitectura técnica":
                    id += "030";
                    break;
                case "diplomatura":
                    id += "240";
                    break;
                case "doctorado/a":
                    id += "250";
                    break;
                case "ingeniería":
                    id += "420";
                    break;
                case "ingeniería técnica":
                    id += "430";
                    break;
                case "licenciatura":
                    id += "470";
                    break;
                case "máster oficial":
                    id += "480";
                    break;
                default:
                    id += "OTHERS";
                    break;
            }
            return id;
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="teachingtype"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static string TeachingType(string teachingtype, ResourceApi pResourceApi)
        {
            string id = pResourceApi.GraphsUrl + "items/teachingtype_";
            switch (teachingtype.ToLower())
            {
                case "docencia internacional":
                    id += "014";
                    break;
                case "docencia oficial":
                    id += "015";
                    break;
                case "docencia no oficial":
                    id += "016";
                    break;
                default:
                    return null;
            }
            return id;
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="pResourceApi"></param>
        /// <param name="premio"></param>
        /// <returns></returns>
        private static string PrizeType(ResourceApi pResourceApi, string premio)
        {
            string id = pResourceApi.GraphsUrl + "items/prizetype_";
            switch (premio.ToLower())
            {
                case "premio extraordinario de licenciatura":
                    id += "000";
                    break;
                case "premio fin de carrera":
                    id += "010";
                    break;
                default:
                    id += "OTHERS";
                    break;
            }
            return id;
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="pResourceApi"></param>
        /// <param name="tipoTitulacion"></param>
        /// <returns></returns>
        private static string UniversityDegreeType(ResourceApi pResourceApi, string tipoTitulacion)
        {
            string id = pResourceApi.GraphsUrl + "items/universitydegreetype_";
            switch (tipoTitulacion.ToLower())
            {
                case "doctor":
                    id += "940";
                    break;
                case "titulado medio":
                    id += "950";
                    break;
                case "titulado superior":
                    id += "960";
                    break;
                default:
                    id += "OTHERS";
                    break;
            }
            return id;
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="pais"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static string IdentificadorPais(string pais, ResourceApi pResourceApi)
        {
            string codigo = UtilidadesGeneral.DicPaisesContienePais(pais);

            if (string.IsNullOrEmpty(codigo))
            {
                return null;
            }

            return pResourceApi.GraphsUrl + "items/feature_PCLD_" + codigo;
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="pResourceApi"></param>
        /// <returns></returns>
        private static string IdentificadorRegion(string region, ResourceApi pResourceApi)
        {
            if (!UtilidadesGeneral.DicRegionesContieneRegion(region))
            {
                return null;
            }
            return pResourceApi.GraphsUrl + "items/feature_ADM1_" + UtilidadesGeneral.dicRegiones[region];
        }

        /// <summary>
        /// Devuelve el ID de la secundaria.
        /// </summary>
        /// <param name="pResourceApi"></param>
        /// <param name="projectCharacterType"></param>
        /// <returns></returns>
        private static string ProjectCharacterType(ResourceApi pResourceApi, string projectCharacterType)
        {
            string id = pResourceApi.GraphsUrl + "items/projectcharactertype_";
            switch (projectCharacterType.ToLower())
            {
                case "proyecto de fin de carrera":
                    id += "055";
                    break;
                case "proyecto final de carrera":
                    id += "055";
                    break;
                case "tesina":
                    id += "066";
                    break;
                case "tesis doctoral":
                    id += "067";
                    break;
                case "trabajo conducente a la obtención de dea":
                    id += "071";
                    break;
                default:
                    id += "OTHERS";
                    break;
            }
            return id;
        }

        /// <summary>
        /// Identificador de la persona.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Nombre de la persona.
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Apellidos de la persona.
        /// </summary>
        public string Apellidos { get; set; }
        /// <summary>
        /// Se devuelve la entidad Sexo con todos sus campos.
        /// </summary>
        public Sexo Sexo { get; set; }
        /// <summary>
        /// Número de documento de identificación personal.
        /// </summary>
        public string NumeroDocumento { get; set; }
        /// <summary>
        /// Se devuelve la entidad TipoDocumento con todos sus campos.
        /// </summary>
        public TipoDocumento TipoDocumento { get; set; }
        /// <summary>
        /// Se devuelve el identificador/referencia de la entidad Empresa.
        /// </summary>
        public string EmpresaRef { get; set; }
        /// <summary>
        /// Indica si es personal de la Universidad o no (a día de hoy).
        /// </summary>
        public bool? PersonalPropio { get; set; }
        /// <summary>
        /// Se devuelve el identificador/referencia de la entidad que representa a la UM
        /// </summary>
        public string EntidadPropiaRef { get; set; }
        /// <summary>
        /// Lista con los emails de la persona 
        /// </summary>
        public List<Email> Emails { get; set; }
        /// <summary>
        /// Indica si la persona esta activa o no
        /// </summary>
        public bool? Activo { get; set; }
        /// <summary>
        /// Datos personales
        /// </summary>
        public DatosPersonales DatosPersonales { get; set; }
        /// <summary>
        /// Datos de contacto
        /// </summary>
        public DatosContacto DatosContacto { get; set; }
        /// <summary>
        /// Vinculación
        /// </summary>
        public Vinculacion Vinculacion { get; set; }
        /// <summary>
        /// Datos academicos
        /// </summary>
        public DatosAcademicos DatosAcademicos { get; set; }
        /// <summary>
        /// Colectivo
        /// </summary>
        public Colectivo Colectivo { get; set; }
        /// <summary>
        /// Fotografía
        /// </summary>
        public Fotografia Fotografia { get; set; }
        /// <summary>
        /// Sexenios
        /// </summary>
        public Sexenio Sexenios { get; set; }
        /// <summary>
        /// Posgrado
        /// </summary>
        public List<Posgrado> Posgrado { get; set; }
        /// <summary>
        /// Ciclos
        /// </summary>
        public List<Ciclos> Ciclos { get; set; }
        /// <summary>
        /// Doctorados
        /// </summary>
        public List<Doctorados> Doctorados { get; set; }
        /// <summary>
        /// Formación especializada
        /// </summary>
        public List<FormacionEspecializada> FormacionEspecializada { get; set; }
        /// <summary>
        /// Tesis
        /// </summary>
        public List<Tesis> Tesis { get; set; }
        /// <summary>
        /// Seminarios/Cursos
        /// </summary>
        public List<SeminariosCursos> SeminariosCursos { get; set; }
        /// <summary>
        /// Formación academica impartida
        /// </summary>
        public List<FormacionAcademicaImpartida> FormacionAcademicaImpartida { get; set; }
    }
}
