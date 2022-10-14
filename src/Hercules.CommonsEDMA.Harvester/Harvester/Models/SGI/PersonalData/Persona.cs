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
using OAI_PMH.Models.SGI.Organization;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
        private static string RUTA_PREFIJOS = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Utilidades/prefijos.json";
        private static string mPrefijos = string.Join(" ", JsonConvert.DeserializeObject<List<string>>(File.ReadAllText(RUTA_PREFIJOS)));

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

        public override string ObtenerIDBBDD(ResourceApi pResourceApi)
        {
            Dictionary<string, string> respuesta = ObtenerPersonasBBDD(new HashSet<string>() { Id.ToString() }, pResourceApi);
            if (respuesta.ContainsKey(Id.ToString()) && !string.IsNullOrEmpty(respuesta[Id.ToString()]))
            {
                return respuesta[Id.ToString()];
            }
            return null;
        }

        public override void ToRecursoAdicional(IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas, RabbitServiceWriterDenormalizer pRabbitConf, string pIdGnoss)
        {
            #region --- TESIS
            pResourceApi.ChangeOntoly("thesissupervision");
            List<string> crisIdentifiersTesisBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "thesissupervision");
            List<string> crisIdentifiersTesisSGI = new List<string>();
            List<TesisBBDD> listaTesisSGI = ObtenerTesisSGI(pRabbitConf, this.Tesis, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (TesisBBDD tesisAux in listaTesisSGI)
            {
                crisIdentifiersTesisSGI.Add(tesisAux.crisIdentifier);
            }

            // Carga.
            List<string> listaTesisCargarCrisIdentifiers = crisIdentifiersTesisSGI.Except(crisIdentifiersTesisBBDD).ToList();
            List<TesisBBDD> listaTesisCargar = listaTesisSGI.Where(x => listaTesisCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
            List<ComplexOntologyResource> listaTesisOntology = GetThesisSupervision(listaTesisCargar, pResourceApi, pIdGnoss);
            CargarDatos(listaTesisOntology, pResourceApi);

            // Eliminación.
            List<string> listaTesisBorrarCrisIdentifiers = crisIdentifiersTesisBBDD.Except(crisIdentifiersTesisSGI).ToList();
            List<string> listaIdsTesisBorrar = ObtenerDataByCrisIdentifiers(listaTesisBorrarCrisIdentifiers, pResourceApi, "thesissupervision");
            BorrarRecursos(listaIdsTesisBorrar, pResourceApi, "thesissupervision");
            #endregion

            #region --- IMPARTED ACADEMIC TRAINING TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("impartedacademictraining");

            List<string> crisIdentifiersImpartedAcademicBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "impartedacademictraining");
            List<string> crisIdentifiersImpartedAcademicSGI = new List<string>();
            List<ImpartedAcademicTrainingBBDD> listaImpartedAcademicCargarSGI = ObtenerImpartedAcademicSGI(pRabbitConf, this.FormacionAcademicaImpartida, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (ImpartedAcademicTrainingBBDD tesisAux in listaImpartedAcademicCargarSGI)
            {
                crisIdentifiersImpartedAcademicSGI.Add(tesisAux.crisIdentifier);
            }

            // Carga.
            List<string> listaImpartedAcademicCargarCrisIdentifiers = crisIdentifiersImpartedAcademicSGI.Except(crisIdentifiersImpartedAcademicBBDD).ToList();
            List<ImpartedAcademicTrainingBBDD> listaImpartedAcademicCargar = listaImpartedAcademicCargarSGI.Where(x => listaImpartedAcademicCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
            List<ComplexOntologyResource> listaImpartedAcademicOntology = GetImpartedAcademic(listaImpartedAcademicCargar, pResourceApi, pIdGnoss);
            CargarDatos(listaImpartedAcademicOntology, pResourceApi);

            // Eliminación.
            List<string> listaImpartedAcademicBorrarCrisIdentifiers = crisIdentifiersImpartedAcademicBBDD.Except(crisIdentifiersImpartedAcademicSGI).ToList();
            List<string> listaIdsImpartedAcademicBorrar = ObtenerDataByCrisIdentifiers(listaImpartedAcademicBorrarCrisIdentifiers, pResourceApi, "impartedacademictraining");
            BorrarRecursos(listaIdsImpartedAcademicBorrar, pResourceApi, "impartedacademictraining");
            #endregion

            #region --- CURSES AND SEMINARS TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("impartedcoursesseminars");
            List<string> crisIdentifiersCursosBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "impartedcoursesseminars");
            List<ImpartedCoursesSeminarsBBDD> listaCursosSGI = ObtenerCursosSGI(pRabbitConf, this.SeminariosCursos, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);

            List<string> crisIdentifiersCursosSGI = new List<string>();
            foreach (ImpartedCoursesSeminarsBBDD cursoAux in listaCursosSGI)
            {
                crisIdentifiersCursosSGI.Add(cursoAux.crisIdentifiers);
            }

            // Carga.
            List<string> listaCursosCargarCrisIdentifiers = crisIdentifiersCursosSGI.Except(crisIdentifiersCursosBBDD).ToList();
            List<ImpartedCoursesSeminarsBBDD> listaCursosCargar = listaCursosSGI.Where(x => listaCursosCargarCrisIdentifiers.Contains(x.crisIdentifiers)).ToList();
            List<ComplexOntologyResource> listaCursosOntology = GetCursosSupervision(listaCursosCargar, pResourceApi, pIdGnoss);
            CargarDatos(listaTesisOntology, pResourceApi);

            // Eliminación.
            List<string> listaCursosBorrarCrisIdentifiers = crisIdentifiersCursosBBDD.Except(crisIdentifiersCursosSGI).ToList();
            List<string> listaIdsCursosBorrar = ObtenerDataByCrisIdentifiers(listaCursosBorrarCrisIdentifiers, pResourceApi, "impartedcoursesseminars");
            BorrarRecursos(listaIdsCursosBorrar, pResourceApi, "impartedcoursesseminars");
            #endregion

            #region --- CICLOS TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("academicdegree");
            List<string> crisIdentifiersCyclesBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "academicdegree");
            List<string> crisIdentifiersCyclesSGI = new List<string>();
            List<CiclosBBDD> listaCiclosSGI = ObtenerCiclosSGI(pRabbitConf, this.Ciclos, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (CiclosBBDD ciclosAux in listaCiclosSGI)
            {
                crisIdentifiersCyclesSGI.Add(ciclosAux.crisIdentifier);
            }

            // Carga.
            List<string> listaCiclosCargarCrisIdentifiers = crisIdentifiersCyclesSGI.Except(crisIdentifiersCyclesBBDD).ToList();
            List<CiclosBBDD> listaCiclosCargar = listaCiclosSGI.Where(x => listaCiclosCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
            List<ComplexOntologyResource> listaCiclosOntology = GetCycles(listaCiclosCargar, pResourceApi, pIdGnoss);
            CargarDatos(listaCiclosOntology, pResourceApi);

            // Eliminación.
            List<string> listaCiclosBorrarCrisIdentifiers = crisIdentifiersCyclesBBDD.Except(crisIdentifiersCyclesSGI).ToList();
            List<string> listaIdsCiclosBorrar = ObtenerDataByCrisIdentifiers(listaCiclosBorrarCrisIdentifiers, pResourceApi, "academicdegree");
            BorrarRecursos(listaIdsCiclosBorrar, pResourceApi, "academicdegree");
            #endregion

            #region --- DOCTORADOS TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("academicdegree");
            List<string> crisIdentifiersDoctoradosBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "academicdegree");
            List<string> crisIdentifiersDoctoradosSGI = new List<string>();
            List<DoctoradosBBDD> listaDoctoradosSGI = ObtenerDoctoradosSGI(pRabbitConf, this.Doctorados, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (DoctoradosBBDD doctoradosAux in listaDoctoradosSGI)
            {
                crisIdentifiersCyclesSGI.Add(doctoradosAux.crisIdentifier);
            }

            // Carga.
            List<string> listaDoctoradosCargarCrisIdentifiers = crisIdentifiersDoctoradosSGI.Except(crisIdentifiersDoctoradosBBDD).ToList();
            List<DoctoradosBBDD> listaDoctoradosCargar = listaDoctoradosSGI.Where(x => listaDoctoradosCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
            List<ComplexOntologyResource> listaDoctoradosOntology = GetDoctorates(listaDoctoradosCargar, pResourceApi, pIdGnoss);
            CargarDatos(listaDoctoradosOntology, pResourceApi);

            // Eliminación.
            List<string> listaDoctoradosBorrarCrisIdentifiers = crisIdentifiersDoctoradosBBDD.Except(crisIdentifiersDoctoradosSGI).ToList();
            List<string> listaIdsDoctoradosBorrar = ObtenerDataByCrisIdentifiers(listaDoctoradosBorrarCrisIdentifiers, pResourceApi, "academicdegree");
            BorrarRecursos(listaIdsDoctoradosBorrar, pResourceApi, "academicdegree");
            #endregion

            #region --- POSGRADO TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("academicdegree");
            List<string> crisIdentifiersPosgradoBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "academicdegree");
            List<string> crisIdentifiersPosgradoSGI = new List<string>();
            List<PosgradoBBDD> listaPosgradosSGI = ObtenerPosgradosSGI(pRabbitConf, this.Posgrado, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (PosgradoBBDD posgradosAux in listaPosgradosSGI)
            {
                crisIdentifiersPosgradoSGI.Add(posgradosAux.crisIdentifier);
            }

            // Carga.
            List<string> listaPosgradosCargarCrisIdentifiers = crisIdentifiersPosgradoSGI.Except(crisIdentifiersPosgradoBBDD).ToList();
            List<PosgradoBBDD> listaPosgradosCargar = listaPosgradosSGI.Where(x => listaPosgradosCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
            List<ComplexOntologyResource> listaPosgradosOntology = GetPosgrados(listaPosgradosCargar, pResourceApi, pIdGnoss);
            CargarDatos(listaPosgradosOntology, pResourceApi);

            // Eliminación.
            List<string> listaPosgradosBorrarCrisIdentifiers = crisIdentifiersPosgradoBBDD.Except(crisIdentifiersPosgradoSGI).ToList();
            List<string> listaIdsPosgradosBorrar = ObtenerDataByCrisIdentifiers(listaPosgradosBorrarCrisIdentifiers, pResourceApi, "academicdegree");
            BorrarRecursos(listaIdsPosgradosBorrar, pResourceApi, "academicdegree");
            #endregion

            #region --- FORMACIÓN ESPECIALIZADA TODO: REVISAR PROPIEDADES
            pResourceApi.ChangeOntoly("academicdegree");
            List<string> crisIdentifiersEspecializadaBBDD = ObtenerDataCrisIdentifier(pResourceApi, this.Id, "academicdegree");
            List<string> crisIdentifiersEspecializadaSGI = new List<string>();
            List<FormacionEspecializadaBBDD> listaEspecializadaSGI = ObtenerFormacionEspecializadaSGI(pRabbitConf, this.FormacionEspecializada, pHarvesterServices, pConfig, pResourceApi, pDicIdentificadores, pDicRutas);
            foreach (FormacionEspecializadaBBDD formacionEspecializadaAux in listaEspecializadaSGI)
            {
                crisIdentifiersEspecializadaSGI.Add(formacionEspecializadaAux.crisIdentifier);
            }

            // Carga.
            List<string> listaEspecializadaCargarCrisIdentifiers = crisIdentifiersEspecializadaSGI.Except(crisIdentifiersEspecializadaBBDD).ToList();
            List<FormacionEspecializadaBBDD> listaEspecializadaCargar = listaEspecializadaSGI.Where(x => listaEspecializadaCargarCrisIdentifiers.Contains(x.crisIdentifier)).ToList();
            List<ComplexOntologyResource> listaEspecializadaOntology = GetFormacionEspecializada(listaEspecializadaCargar, pResourceApi, pIdGnoss);
            CargarDatos(listaPosgradosOntology, pResourceApi);

            // Eliminación.
            List<string> listaEspecializadaBorrarCrisIdentifiers = crisIdentifiersEspecializadaBBDD.Except(crisIdentifiersEspecializadaSGI).ToList();
            List<string> listaIdsEspecializadaBorrar = ObtenerDataByCrisIdentifiers(listaEspecializadaBorrarCrisIdentifiers, pResourceApi, "academicdegree");
            BorrarRecursos(listaIdsEspecializadaBorrar, pResourceApi, "academicdegree");
            #endregion
        }

        public static void BorrarRecursos(List<string> pListaGnossId, ResourceApi pResourceApi, string pOntology)
        {
            pResourceApi.ChangeOntoly(pOntology);

            foreach (string id in pListaGnossId)
            {
                Guid guid = pResourceApi.GetShortGuid(id);
                pResourceApi.PersistentDelete(guid);
            }
        }

        public static List<string> ObtenerDataByCrisIdentifiers(List<string> pListaCrisIdentifiers, ResourceApi pResourceApi, string pOntology)
        {
            List<string> listaTesis = new List<string>();

            string select = string.Empty;
            string where = string.Empty;

            select = $@"SELECT * ";
            where = $@"WHERE {{                        
                        ?s <http://w3id.org/roh/crisIdentifier> ?crisIdentifier. 
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

        public PersonOntology.Person CrearPersonOntology(RabbitServiceWriterDenormalizer pRabbitConf, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
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

        public List<ComplexOntologyResource> GetThesisSupervision(List<TesisBBDD> pTesisList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaTesisDevolver = new List<ComplexOntologyResource>();

            foreach (TesisBBDD tesis in pTesisList)
            {
                ThesissupervisionOntology.ThesisSupervision tesisDevolver = new ThesissupervisionOntology.ThesisSupervision();

                tesisDevolver.IdRoh_owner = pIdGnoss;
                tesisDevolver.Roh_cvnCode = "030.040.000.000";
                tesisDevolver.Roh_crisIdentifier = tesis.crisIdentifier;
                tesisDevolver.IdRoh_projectCharacterType = tesis.projectCharacterType;
                tesisDevolver.Roh_projectCharacterTypeOther = tesis.projectCharacterTypeOther;
                tesisDevolver.Roh_title = tesis.title;
                //tesisDevolver.IdVcard_hasCountryName = tesis.hasCountryName;
                //tesisDevolver.IdVcard_hasRegion = tesis.hasRegion;
                //tesisDevolver.Vcard_locality = tesis.locality;
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

        public List<ComplexOntologyResource> GetCycles(List<CiclosBBDD> pCyclesList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaCiclosDevolver = new List<ComplexOntologyResource>();

            foreach (CiclosBBDD ciclo in pCyclesList)
            {
                AcademicdegreeOntology.AcademicDegree ciclosDevolver = new AcademicdegreeOntology.AcademicDegree();

                ciclosDevolver.IdRoh_owner = pIdGnoss;
                ciclosDevolver.Roh_title = ciclo.nombreTitulo;
                ciclosDevolver.Roh_crisIdentifier = ciclo.crisIdentifier;
                ciclosDevolver.Roh_conductedByTitle = ciclo.entidadTitulacionTitulo;
                ciclosDevolver.IdRoh_conductedBy = ciclo.entidadTitulacion;
                ciclosDevolver.Dct_issued = ciclo.fechaTitulacion;
                ciclosDevolver.IdRoh_universityDegreeType = ciclo.titulacionUni;
                ciclosDevolver.Roh_universityDegreeTypeOther = ciclo.titulacionUniOtros;
                ciclosDevolver.Roh_foreignTitle = ciclo.tituloExtranjero;
                ciclosDevolver.Roh_approvedDegree = ciclo.tituloHomologado;
                ciclosDevolver.Roh_approvedDate = ciclo.fechaHomologacion;
                // NOTA MEDIA TODO
                ciclosDevolver.IdRoh_prize = ciclo.premio;
                ciclosDevolver.Roh_prizeOther = ciclo.premioOther;

                listaCiclosDevolver.Add(ciclosDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaCiclosDevolver;
        }

        public List<ComplexOntologyResource> GetDoctorates(List<DoctoradosBBDD> pDoctoratesList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaDoctoradosDevolver = new List<ComplexOntologyResource>();

            foreach (DoctoradosBBDD doctorado in pDoctoratesList)
            {
                AcademicdegreeOntology.AcademicDegree doctoradosDevolver = new AcademicdegreeOntology.AcademicDegree();

                doctoradosDevolver.IdRoh_owner = pIdGnoss;
                doctoradosDevolver.Roh_title = doctorado.nombreTitulo;
                doctoradosDevolver.Roh_crisIdentifier = doctorado.crisIdentifier;
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

                listaDoctoradosDevolver.Add(doctoradosDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaDoctoradosDevolver;
        }

        public List<ComplexOntologyResource> GetPosgrados(List<PosgradoBBDD> pPosgradosList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaPosgradosDevolver = new List<ComplexOntologyResource>();

            foreach (PosgradoBBDD posgrado in pPosgradosList)
            {
                AcademicdegreeOntology.AcademicDegree posgradosDevolver = new AcademicdegreeOntology.AcademicDegree();

                posgradosDevolver.IdRoh_owner = pIdGnoss;
                posgradosDevolver.Roh_title = posgrado.nombreTitulo;
                posgradosDevolver.Roh_crisIdentifier = posgrado.crisIdentifier;
                posgradosDevolver.Roh_conductedByTitle = posgrado.entidadTitulacionTitulo;
                posgradosDevolver.IdRoh_conductedBy = posgrado.entidadTitulacion;
                posgradosDevolver.Dct_issued = posgrado.fechaTitulacion;

                posgradosDevolver.IdRoh_formationType = posgrado.tipoFormacion;
                posgradosDevolver.Roh_qualification = posgrado.calificacionObtenida;
                posgradosDevolver.Roh_approvedDegree = posgrado.tituloHomologado;
                posgradosDevolver.Roh_approvedDate = posgrado.fechaHomologacion;

                listaPosgradosDevolver.Add(posgradosDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaPosgradosDevolver;
        }

        public List<ComplexOntologyResource> GetFormacionEspecializada(List<FormacionEspecializadaBBDD> pEspecializadaList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaPosgradosDevolver = new List<ComplexOntologyResource>();

            foreach (FormacionEspecializadaBBDD formacionEspecializada in pEspecializadaList)
            {
                AcademicdegreeOntology.AcademicDegree formEspDevolver = new AcademicdegreeOntology.AcademicDegree();

                formEspDevolver.IdRoh_owner = pIdGnoss;
                formEspDevolver.Roh_title = formacionEspecializada.nombreTitulo;
                formEspDevolver.Roh_crisIdentifier = formacionEspecializada.crisIdentifier;
                formEspDevolver.Roh_conductedByTitle = formacionEspecializada.entidadTitulacionTitulo;
                formEspDevolver.IdRoh_conductedBy = formacionEspecializada.entidadTitulacion;
                formEspDevolver.Dct_issued = formacionEspecializada.fechaFinalizacion;

                formEspDevolver.Roh_durationHours = formacionEspecializada.duracionHoras;
                formEspDevolver.IdRoh_formationType = formacionEspecializada.tipoFormacion;
                formEspDevolver.Roh_goals = formacionEspecializada.objetivosEntidad;
                formEspDevolver.Roh_trainerNick = formacionEspecializada.firma;
                formEspDevolver.Roh_trainerName = formacionEspecializada.nombre;
                formEspDevolver.Roh_trainerFirstSurname = formacionEspecializada.primApe;
                formEspDevolver.Roh_trainerSecondSurname = formacionEspecializada.segunApe;

                listaPosgradosDevolver.Add(formEspDevolver.ToGnossApiResource(pResourceApi, null));
            }

            return listaPosgradosDevolver;
        }


        public List<ComplexOntologyResource> GetImpartedAcademic(List<ImpartedAcademicTrainingBBDD> pImpartedAcademicList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listaImpartedDevolver = new List<ComplexOntologyResource>();

            foreach (ImpartedAcademicTrainingBBDD impartedAcademic in pImpartedAcademicList)
            {
                ImpartedacademictrainingOntology.ImpartedAcademicTraining academicDevolver = new ImpartedacademictrainingOntology.ImpartedAcademicTraining();

                academicDevolver.IdRoh_owner = pIdGnoss;
                academicDevolver.Roh_crisIdentifier = impartedAcademic.crisIdentifier;
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

        public List<ComplexOntologyResource> GetCursosSupervision(List<ImpartedCoursesSeminarsBBDD> pTesisList, ResourceApi pResourceApi, string pIdGnoss)
        {
            List<ComplexOntologyResource> listacursoDevolver = new List<ComplexOntologyResource>();

            foreach (ImpartedCoursesSeminarsBBDD curso in pTesisList)
            {
                ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars courseDevolver = new ImpartedcoursesseminarsOntology.ImpartedCoursesSeminars();

                courseDevolver.IdRoh_owner = pIdGnoss;
                courseDevolver.Roh_crisIdentifier = curso.crisIdentifiers;
                courseDevolver.Roh_title = curso.title;
                courseDevolver.IdRoh_eventType = curso.eventType;
                courseDevolver.Roh_eventTypeOther = curso.eventTypeOther;
                courseDevolver.Roh_promotedByTitle = curso.promotedByTitle;
                courseDevolver.IdRoh_promotedBy = curso.promotedBy;
                courseDevolver.IdRoh_promotedByType = curso.promotedByType;
                courseDevolver.Roh_promotedByTypeOther = curso.promotedByTypeOther;
                courseDevolver.Vivo_start = curso.start;
                courseDevolver.Roh_durationHours = curso.durationHours;
                // TODO
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
        public List<string> ObtenerCodigosAcademicDegree(ResourceApi pResourceApi, string pCrisIdentifierPerson, string pTipo)
        {
            List<string> listaAcademicDegree = new List<string>();

            string select = "SELECT ?crisIdentifier";
            string where = $@"
            WHERE {{
                ?cv <http://w3id.org/roh/cvOf> ?persona.
                ?persona <http://w3id.org/roh/crisIdentifier> '{pCrisIdentifierPerson}'.
                ?cv <http://w3id.org/roh/qualifications> ?qualifications.
                ?qualifications <{pTipo}> ?aux.
                ?aux <http://vivoweb.org/ontology/core#relatedBy> ?s.
                ?s <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                
            }}";

            SparqlObject resultadoQuery = pResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "curriculumvitae", "academicdegree", "person" });
            if (!(resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0))
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    if (fila.ContainsKey("crisIdentifier") && !string.IsNullOrEmpty(fila["crisIdentifier"].value))
                    {
                        listaAcademicDegree.Add(fila["crisIdentifier"].value);
                    }
                }
            }
            return listaAcademicDegree;
        }
        public List<AcademicDegreeBBDD> ObtenerAcademicDegree(ResourceApi pResourceApi, string pCrisIdentifierPerson, string pTipo)
        {
            List<AcademicDegreeBBDD> listaAcademicDegree = new List<AcademicDegreeBBDD>();
            Dictionary<string, List<string>> dicCodirectores = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> dicCategoryPaths = new Dictionary<string, List<string>>();

            string select = "SELECT *";
            string where = $@"
            WHERE {{
                ?cv <http://w3id.org/roh/cvOf> ?persona.
                ?cv <http://w3id.org/roh/qualifications> ?qualifications.
                ?qualifications <{pTipo}> ?aux.
                ?aux <http://vivoweb.org/ontology/core#relatedBy> ?s.
                OPTIONAL {{ ?s <http://w3id.org/roh/title> ?title. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/degreeType> ?degreeType. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/postgradeDegree> ?postgradeDegree. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/doctoralProgram> ?doctoralProgram. }}
                OPTIONAL {{ ?s <http://purl.org/dc/terms/issued> ?issued. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/universityDegreeType> ?universityDegreeType. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/universityDegreeTypeOther> ?universityDegreeTypeOther. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/conductedByTitle> ?conductedByTitle. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/conductedBy> ?conductedBy. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/conductedByType> ?conductedByType. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/conductedByTypeOther> ?conductedByTypeOther. }}
                OPTIONAL {{ ?s <http://www.w3.org/2006/vcard/ns#locality> ?locality. }}
                OPTIONAL {{ ?s <http://www.w3.org/2006/vcard/ns#hasCountryName> ?hasCountryName. }}
                OPTIONAL {{ ?s <http://www.w3.org/2006/vcard/ns#hasRegion> ?hasRegion. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/foreignTitle> ?foreignTitle. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/foreignDegreeType> ?foreignDegreeType. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/approvedDegree> ?approvedDegree. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/approvedDate> ?approvedDate. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/mark> ?mark. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/prize> ?prize. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/prizeOther> ?prizeOther. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/formationType> ?formationType. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/qualification> ?qualification. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/center> ?center. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/durationHours> ?durationHours. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/goals> ?goals. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/trainerNick> ?trainerNick. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/trainerName> ?trainerName. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/trainerFistSurname> ?trainerFistSurname. }}
                OPTIONAL {{ ?s <http://vivoweb.org/ontology/core#end> ?end. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/formationActivityType> ?formationActivityType. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/formationActivityTypeOther> ?formationActivityTypeOther. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/stayGoal> ?stayGoal. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/stayGoalOther> ?stayGoalOther. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/fundingProgram> ?fundingProgram. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/targetProfile> ?targetProfile. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/performedTasks> ?performedTasks. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/durationYears> ?durationYears. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/durationMonths> ?durationMonths. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/durationDays> ?durationDays. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/deaEntityTitle> ?deaEntityTitle. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/deaEntity> ?deaEntity. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/deaDate> ?deaDate. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/thesiTitle> ?thesiTitle. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/directorNick> ?directorNick. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/directorName> ?directorName. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/directorFirstSurname> ?directorFirstSurname. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/directorSecondSurname> ?directorSecondSurname. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/codirector> ?codirector. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/europeanDoctorate> ?europeanDoctorate. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/europeanDoctorateDate> ?europeanDoctorateDate. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/qualityMention> ?qualityMention. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/doctorExtraordinaryAward> ?doctorExtraordinaryAward. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/doctorExtraordinaryAwardDate> ?doctorExtraordinaryAwardDate. }}
                ?persona <http://w3id.org/roh/crisIdentifier> '{pCrisIdentifierPerson}'.
            }}";

            SparqlObject resultadoQuery = pResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "thesissupervision" });
            if (!(resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0))
            {
                return listaAcademicDegree;
            }

            if (pTipo.Equals("http://w3id.org/roh/relatedFirstSecondCycles"))
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    listaAcademicDegree.Add(ObtenerCiclosBBDD(fila));
                }
            }
            else if (pTipo.Equals("http://w3id.org/roh/relatedDoctorates"))
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    listaAcademicDegree.Add(ObtenerDoctoradosBBDD(fila));
                }
            }
            else if (pTipo.Equals("http://w3id.org/roh/relatedPostGraduates"))
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    listaAcademicDegree.Add(ObtenerPosgradoBBDD(fila));
                }
            }
            else if (pTipo.Equals("http://w3id.org/roh/relatedSpecialisedTrainings"))
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    listaAcademicDegree.Add(ObtenerFormacionEspecializadaBBDD(fila));
                }
            }
            else if (pTipo.Equals("http://w3id.org/roh/relatedCoursesAndSeminars"))
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    listaAcademicDegree.Add(ObtenerSeminariosCursosBBDD(fila));
                }
            }
            return listaAcademicDegree;
        }
        public CiclosBBDD ObtenerCiclosBBDD(Dictionary<string, SparqlObject.Data> fila)
        {
            CiclosBBDD ciclo = new CiclosBBDD();
            ciclo.crisIdentifier = fila["s"].value;
            ciclo.nombreTitulo = fila["title"].value;

            return ciclo;
        }
        public DoctoradosBBDD ObtenerDoctoradosBBDD(Dictionary<string, SparqlObject.Data> fila)
        {
            DoctoradosBBDD doctorados = new DoctoradosBBDD();
            doctorados.tituloTesis = fila["thesiTitle"].value;
            return doctorados;
        }
        public PosgradoBBDD ObtenerPosgradoBBDD(Dictionary<string, SparqlObject.Data> fila)
        {
            PosgradoBBDD posgrado = new PosgradoBBDD();

            return posgrado;
        }
        public FormacionEspecializadaBBDD ObtenerFormacionEspecializadaBBDD(Dictionary<string, SparqlObject.Data> fila)
        {
            FormacionEspecializadaBBDD formacionEspecializada = new FormacionEspecializadaBBDD();

            return formacionEspecializada;
        }
        public SeminariosCursosBBDD ObtenerSeminariosCursosBBDD(Dictionary<string, SparqlObject.Data> fila)
        {
            SeminariosCursosBBDD seminariosCursos = new SeminariosCursosBBDD();

            return seminariosCursos;
        }

        public List<TesisBBDD> ObtenerTesisBBDD(ResourceApi pResourceApi, string pCrisIdentifierPerson)
        {
            List<TesisBBDD> listaTesis = new List<TesisBBDD>();
            Dictionary<string, List<string>> dicCodirectores = new Dictionary<string, List<string>>();
            Dictionary<string, List<string>> dicCategoryPaths = new Dictionary<string, List<string>>();

            string select = string.Empty;
            string where = string.Empty;
            SparqlObject resultadoQuery = null;

            #region --- Obtención datos básicos Tesis
            select = "SELECT * ";
            where = $@"WHERE {{ 
                                ?s <http://w3id.org/roh/owner> ?persona.
                                ?s <http://w3id.org/roh/crisIdentifier> ?crisIdentifier.
                                ?s <http://w3id.org/roh/title> ?title.
                                OPTIONAL {{ ?s <http://purl.org/dc/terms/issued> ?issued. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/studentNick> ?studentNick. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/studentName> ?studentName. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/studentFirstSurname> ?studentFirstSurname. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/studentSecondSurname> ?studentSecondSurname. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/promotedByTitle> ?promotedByTitle. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/promotedBy> ?promotedBy. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/promotedByType> ?promotedByType. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/promotedByTypeOther> ?promotedByTypeOther. }}
                                OPTIONAL {{ ?s <http://www.w3.org/2006/vcard/ns#locality> ?locality. }}
                                OPTIONAL {{ ?s <http://www.w3.org/2006/vcard/ns#hasCountryName> ?hasCountryName. }}
                                OPTIONAL {{ ?s <http://www.w3.org/2006/vcard/ns#hasRegion> ?hasRegion. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/projectCharacterType> ?projectCharacterType. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/projectCharacterTypeOther> ?projectCharacterTypeOther. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/codirector> ?codirector. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/qualification> ?qualification. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/qualityMention> ?qualityMention. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/qualityMentionDate> ?qualityMentionDate. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/europeanDoctorate> ?europeanDoctorate. }}
                                OPTIONAL {{ ?s <http://w3id.org/roh/europeanDoctorateDate> ?europeanDoctorateDate. }}
                                OPTIONAL {{ ?s <http://vivoweb.org/ontology/core#freeTextKeyword> ?freeTextKeyword. }}
                                ?persona <http://w3id.org/roh/crisIdentifier> '{pCrisIdentifierPerson}'.
                            }}";

            resultadoQuery = pResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { "person", "thesissupervision" });
            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                {
                    TesisBBDD tesis = new TesisBBDD();
                    tesis.idGnoss = fila["s"].value;
                    tesis.crisIdentifier = fila["s"].value;
                    tesis.title = fila["title"].value;
                    if (fila.ContainsKey("issued") && !string.IsNullOrEmpty(fila["issued"].value))
                    {
                        int anio = int.Parse(fila["issued"].value.Substring(0, 4));
                        int mes = int.Parse(fila["issued"].value.Substring(4, 2));
                        int dia = int.Parse(fila["issued"].value.Substring(6, 2));
                        tesis.issued = new DateTime(anio, mes, dia, 0, 0, 0, DateTimeKind.Utc);
                    }
                    if (fila.ContainsKey("studentNick") && !string.IsNullOrEmpty(fila["studentNick"].value))
                    {
                        tesis.studentNick = fila["studentNick"].value;
                    }
                    if (fila.ContainsKey("studentName") && !string.IsNullOrEmpty(fila["studentName"].value))
                    {
                        tesis.studentName = fila["studentName"].value;
                    }
                    if (fila.ContainsKey("studentFirstSurname") && !string.IsNullOrEmpty(fila["studentFirstSurname"].value))
                    {
                        tesis.studentFirstSurname = fila["studentFirstSurname"].value;
                    }
                    if (fila.ContainsKey("studentSecondSurname") && !string.IsNullOrEmpty(fila["studentSecondSurname"].value))
                    {
                        tesis.studentSecondSurname = fila["studentSecondSurname"].value;
                    }
                    if (fila.ContainsKey("promotedByTitle") && !string.IsNullOrEmpty(fila["promotedByTitle"].value))
                    {
                        tesis.promotedByTitle = fila["promotedByTitle"].value;
                    }
                    if (fila.ContainsKey("promotedBy") && !string.IsNullOrEmpty(fila["promotedBy"].value))
                    {
                        tesis.promotedBy = fila["promotedBy"].value;
                    }
                    if (fila.ContainsKey("promotedByType") && !string.IsNullOrEmpty(fila["promotedByType"].value))
                    {
                        tesis.promotedByType = fila["promotedByType"].value;
                    }
                    if (fila.ContainsKey("promotedByTypeOther") && !string.IsNullOrEmpty(fila["promotedByTypeOther"].value))
                    {
                        tesis.promotedByTypeOther = fila["promotedByTypeOther"].value;
                    }
                    if (fila.ContainsKey("codirector") && !string.IsNullOrEmpty(fila["codirector"].value))
                    {
                        if (dicCodirectores.ContainsKey(fila["s"].value))
                        {
                            dicCodirectores[fila["s"].value].Add(fila["codirector"].value);
                        }
                        else
                        {
                            tesis.codirector = new List<Codirector>();
                            dicCodirectores[fila["s"].value] = new List<string>() { fila["codirector"].value };
                        }
                    }
                    if (fila.ContainsKey("locality") && !string.IsNullOrEmpty(fila["locality"].value))
                    {
                        tesis.locality = fila["locality"].value;
                    }
                    if (fila.ContainsKey("hasCountryName") && !string.IsNullOrEmpty(fila["hasCountryName"].value))
                    {
                        tesis.hasCountryName = fila["hasCountryName"].value;
                    }
                    if (fila.ContainsKey("hasRegion") && !string.IsNullOrEmpty(fila["hasRegion"].value))
                    {
                        tesis.hasRegion = fila["hasRegion"].value;
                    }
                    if (fila.ContainsKey("projectCharacterType") && !string.IsNullOrEmpty(fila["projectCharacterType"].value))
                    {
                        tesis.projectCharacterType = fila["projectCharacterType"].value;
                    }
                    if (fila.ContainsKey("projectCharacterTypeOther") && !string.IsNullOrEmpty(fila["projectCharacterTypeOther"].value))
                    {
                        tesis.projectCharacterTypeOther = fila["projectCharacterTypeOther"].value;
                    }
                    if (fila.ContainsKey("qualityMention") && !string.IsNullOrEmpty(fila["qualityMention"].value))
                    {
                        tesis.qualityMention = bool.Parse(fila["qualityMention"].value);
                    }
                    if (fila.ContainsKey("qualityMentionDate") && !string.IsNullOrEmpty(fila["qualityMentionDate"].value))
                    {
                        int anio = int.Parse(fila["qualityMentionDate"].value.Substring(0, 4));
                        int mes = int.Parse(fila["qualityMentionDate"].value.Substring(4, 2));
                        int dia = int.Parse(fila["qualityMentionDate"].value.Substring(6, 2));
                        tesis.qualityMentionDate = new DateTime(anio, mes, dia, 0, 0, 0, DateTimeKind.Utc);
                    }
                    if (fila.ContainsKey("europeanDoctorate") && !string.IsNullOrEmpty(fila["europeanDoctorate"].value))
                    {
                        tesis.europeanDoctorate = bool.Parse(fila["europeanDoctorate"].value);
                    }
                    if (fila.ContainsKey("europeanDoctorateDate") && !string.IsNullOrEmpty(fila["europeanDoctorateDate"].value))
                    {
                        int anio = int.Parse(fila["europeanDoctorateDate"].value.Substring(0, 4));
                        int mes = int.Parse(fila["europeanDoctorateDate"].value.Substring(4, 2));
                        int dia = int.Parse(fila["europeanDoctorateDate"].value.Substring(6, 2));
                        tesis.europeanDoctorateDate = new DateTime(anio, mes, dia, 0, 0, 0, DateTimeKind.Utc);
                    }
                    if (fila.ContainsKey("freeTextKeyword") && !string.IsNullOrEmpty(fila["freeTextKeyword"].value))
                    {
                        if (dicCategoryPaths.ContainsKey(fila["s"].value))
                        {
                            dicCategoryPaths[fila["s"].value].Add(fila["freeTextKeyword"].value);
                        }
                        else
                        {
                            tesis.freeTextKeyword = new List<string>();
                            dicCategoryPaths[fila["s"].value] = new List<string>() { fila["freeTextKeyword"].value };
                        }
                    }
                    listaTesis.Add(tesis);
                }
            }
            #endregion

            #region --- Obtención de los datos de codirectores
            foreach (KeyValuePair<string, List<string>> item in dicCodirectores)
            {
                select = "SELECT * ";
                where = $@"WHERE {{
                        ?s <http://www.w3.org/1999/02/22-rdf-syntax-ns#comment> ?comment.
                        ?s <http://xmlns.com/foaf/0.1/nick> ?nick.
                        ?s <http://xmlns.com/foaf/0.1/firtName> ?firstName.
                        ?s <http://xmlns.com/foaf/0.1/familyName> ?familyName.
                        ?s <http://w3id.org/roh/secondFamilyName> ?secondFamilyName.
                        FILTER(?s in (<{string.Join(">, <", item.Value.Select(x => x))}>))}}
                    }}";

                resultadoQuery = pResourceApi.VirtuosoQuery(select, where, "thesissupervision");
                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        Codirector codirector = new Codirector();
                        codirector.comment = Int32.Parse(fila["comment"].value);
                        codirector.firstName = fila["firstName"].value;

                        if (fila.ContainsKey("nick") && !string.IsNullOrEmpty(fila["nick"].value))
                        {
                            codirector.nick = fila["nick"].value;
                        }
                        if (fila.ContainsKey("familyName") && !string.IsNullOrEmpty(fila["familyName"].value))
                        {
                            codirector.familyName = fila["familyName"].value;
                        }
                        if (fila.ContainsKey("secondFamilyName") && !string.IsNullOrEmpty(fila["secondFamilyName"].value))
                        {
                            codirector.secondFamilyName = fila["secondFamilyName"].value;
                        }

                        listaTesis.First(x => x.idGnoss == item.Key).codirector.Add(codirector);
                    }
                }
            }
            #endregion

            return listaTesis;
        }

        public List<string> ObtenerDataCrisIdentifier(ResourceApi pResourceApi, string pCrisIdentfierPerson, string pOntology)
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
        public List<ImpartedAcademicTrainingBBDD> ObtenerImpartedAcademicSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<FormacionAcademicaImpartida> pListaImpartedAcademicSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<ImpartedAcademicTrainingBBDD> listaImpartedAcademic = new List<ImpartedAcademicTrainingBBDD>();

            foreach (FormacionAcademicaImpartida item in pListaImpartedAcademicSGI)
            {
                ImpartedAcademicTrainingBBDD impartedAcademic = new ImpartedAcademicTrainingBBDD();

                string tituloTrabajo = RemoveDiacritics(item.TitulacionUniversitaria);
                tituloTrabajo = tituloTrabajo.Replace(" ", "-");
                impartedAcademic.crisIdentifier = $@"{item.Id}___{tituloTrabajo}";
                impartedAcademic.title = item.TitulacionUniversitaria;
                impartedAcademic.teaches = item.NombreAsignaturaCurso;

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

                impartedAcademic.numberECTSHours = item.NumHorasCreditos.Value;
                impartedAcademic.frequency = item.FrecuenciaActividad.Value;

                if (item.TipoPrograma != null && !string.IsNullOrEmpty(item.TipoPrograma.Nombre))
                {
                    impartedAcademic.programType = ProgramType(item.TipoPrograma.Nombre, pResourceApi);
                    if (impartedAcademic.programType.Contains("OTHERS"))
                    {
                        impartedAcademic.programTypeOther = item.TipoPrograma.Nombre;
                    }
                }

                if (item.TipoDocencia != null && !string.IsNullOrEmpty(item.TipoDocencia.Nombre))
                {
                    impartedAcademic.modalityTeachingType = ModalityTeachingType(item.TipoDocencia.Nombre, pResourceApi);
                    if (impartedAcademic.modalityTeachingType.Contains("OTHERS"))
                    {
                        impartedAcademic.modalityTeachingTypeOther = item.TipoDocencia.Nombre;
                    }
                }

                impartedAcademic.department = item.Departamento;

                if (item.TipoAsignatura != null && !string.IsNullOrEmpty(item.TipoAsignatura.Nombre))
                {
                    impartedAcademic.courseType = CourseType(item.TipoAsignatura.Nombre, pResourceApi);
                    if (impartedAcademic.courseType.Contains("OTHERS"))
                    {
                        impartedAcademic.courseTypeOther = item.TipoAsignatura.Nombre;
                    }
                }

                impartedAcademic.course = item.Curso;

                if (item.TipoHorasCreditos != null && !string.IsNullOrEmpty(item.TipoHorasCreditos.Nombre))
                {
                    impartedAcademic.hoursCreditsECTSType = HoursCreditsECTSType(item.TipoHorasCreditos.Nombre, pResourceApi);
                }

                // TODO: Idioma

                impartedAcademic.competencies = item.Competencias;
                impartedAcademic.professionalCategory = item.CategoriaProfesional;

                listaImpartedAcademic.Add(impartedAcademic);
            }

            return listaImpartedAcademic;
        }

        public List<TesisBBDD> ObtenerTesisSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<Tesis> pListaTesisSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<TesisBBDD> listaTesis = new List<TesisBBDD>();

            foreach (Tesis item in pListaTesisSGI)
            {
                TesisBBDD tesis = new TesisBBDD();

                string tituloTrabajo = RemoveDiacritics(item.TituloTrabajo.ToLower());
                tituloTrabajo = tituloTrabajo.Replace(" ", "-");
                tesis.crisIdentifier = $@"{item.Id}___{item.Alumno}___{tituloTrabajo}";

                if (item.TipoProyecto != null && !string.IsNullOrEmpty(item.TipoProyecto.Nombre))
                {
                    tesis.projectCharacterType = ProjectCharacterType(pResourceApi, item.TipoProyecto.Nombre);
                    if (tesis.projectCharacterType.Contains("OTHERS"))
                    {
                        tesis.projectCharacterTypeOther = item.TipoProyecto.Nombre;
                    }
                }

                tesis.title = item.TituloTrabajo;
                // TODO: Código del pais no mapeable.
                //tesis.hasCountryName = IdentificadorPais(item.PaisEntidadRealizacion.Id, pResourceApi);
                //tesis.hasRegion = IdentificadorRegion(item.CcaaRegionEntidadRealizacion.Id, pResourceApi);
                //tesis.locality = item.CiudadEntidadRealizacion;
                tesis.issued = item.FechaDefensa;
                tesis.qualification = item.CalificacionObtenida;
                tesis.europeanDoctorateDate = item.FechaMencionDoctoradoEuropeo;
                tesis.qualityMention = item.MencionCalidad != null ? (bool)item.MencionCalidad : false;
                tesis.europeanDoctorate = item.DoctoradoEuropeo != null ? (bool)item.DoctoradoEuropeo : false;
                tesis.qualityMentionDate = item.FechaMencionCalidad;

                Persona alumno = GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + item.Alumno, pDicRutas);
                if (alumno != null)
                {
                    tesis.studentName = alumno.Nombre;
                    tesis.studentFirstSurname = alumno.Apellidos;
                    tesis.studentNick = alumno.Nombre + " " + alumno.Apellidos;
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

                if (item.CoDirectorTesis != null && !string.IsNullOrEmpty(item.CoDirectorTesis.PersonaRef))
                {
                    Persona codirectorSGI = GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + item.CoDirectorTesis.PersonaRef, pDicRutas);
                    if (codirectorSGI != null)
                    {
                        Codirector codirector = new Codirector();
                        codirector.firstName = codirectorSGI.Nombre;
                        codirector.secondFamilyName = codirectorSGI.Apellidos;
                        codirector.nick = codirectorSGI.Nombre + " " + codirectorSGI.Apellidos;
                        codirector.comment = 1;
                        tesis.codirector = new List<Codirector>() { codirector };
                    }
                }

                listaTesis.Add(tesis);
            }

            return listaTesis;
        }

        public List<ImpartedCoursesSeminarsBBDD> ObtenerCursosSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<SeminariosCursos> pListaCursosSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<ImpartedCoursesSeminarsBBDD> listaCursos = new List<ImpartedCoursesSeminarsBBDD>();

            foreach (SeminariosCursos item in pListaCursosSGI)
            {
                ImpartedCoursesSeminarsBBDD curso = new ImpartedCoursesSeminarsBBDD();

                string tituloTrabajo = RemoveDiacritics(item.NombreEvento.ToLower());
                tituloTrabajo = tituloTrabajo.Replace(" ", "-");
                curso.crisIdentifiers = $@"{item.Id}___{tituloTrabajo}";
                curso.goals = item.ObjetivosCurso;
                curso.targetProfile = item.PerfilDestinatarios;
                curso.hasLanguage = item.Idioma;
                curso.start = item.FechaTitulacion;

                if (item.TipoParticipacion != null && !string.IsNullOrEmpty(item.TipoParticipacion.Nombre))
                {
                    curso.participationType = item.TipoParticipacion.Nombre;
                }

                curso.correspondingAuthor = item.AutorCorrespondencia != null ? (bool)item.AutorCorrespondencia : false;

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

                curso.isbn = item.ISBN;
                curso.issn = item.ISSN;

                listaCursos.Add(curso);
            }

            return listaCursos;
        }

        public List<CiclosBBDD> ObtenerCiclosSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<Ciclos> pListaCiclosSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<CiclosBBDD> listaCiclos = new List<CiclosBBDD>();

            foreach (Ciclos item in pListaCiclosSGI)
            {
                CiclosBBDD ciclo = new CiclosBBDD();

                string tituloTrabajo = RemoveDiacritics(item.NombreTitulo.ToLower());
                tituloTrabajo = tituloTrabajo.Replace(" ", "-");
                ciclo.crisIdentifier = $@"{item.Id}___{tituloTrabajo}";

                ciclo.nombreTitulo = item.NombreTitulo;

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


                ciclo.fechaTitulacion = item.FechaTitulacion;

                // TODO: NOTA MEDIA

                if (item.Premio != null && !string.IsNullOrEmpty(item.Premio.Nombre))
                {
                    ciclo.premio = PrizeType(pResourceApi, item.Premio.Nombre);
                    if (ciclo.premio.Contains("OTHERS"))
                    {
                        ciclo.premioOther = item.Premio.Nombre;
                    }
                }

                if (item.TitulacionUniversitaria != null && !string.IsNullOrEmpty(item.TitulacionUniversitaria.Nombre))
                {
                    ciclo.titulacionUni = UniversityDegreeType(pResourceApi, item.TitulacionUniversitaria.Nombre);
                    if (ciclo.titulacionUni.Contains("OTHERS"))
                    {
                        ciclo.titulacionUniOtros = item.TitulacionUniversitaria.Nombre;
                    }
                }

                ciclo.tituloExtranjero = item.TituloExtranjero;

                ciclo.tituloHomologado = item.TituloHomologado != null ? (bool)item.TituloHomologado : false;
                ciclo.fechaHomologacion = item.FechaHomologacion;

                listaCiclos.Add(ciclo);
            }

            return listaCiclos;
        }

        public List<DoctoradosBBDD> ObtenerDoctoradosSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<Doctorados> pListaDoctoradosSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<DoctoradosBBDD> listaDoctorados = new List<DoctoradosBBDD>();

            foreach (Doctorados item in pListaDoctoradosSGI)
            {
                DoctoradosBBDD doctorado = new DoctoradosBBDD();

                string tituloTrabajo = RemoveDiacritics(item.ProgramaDoctorado.ToLower());
                tituloTrabajo = tituloTrabajo.Replace(" ", "-");
                doctorado.crisIdentifier = $@"{item.Id}___{tituloTrabajo}";

                doctorado.nombreTitulo = item.ProgramaDoctorado;

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

                // TODO: Ciudad, Pais, CCAA

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
                doctorado.calificacionObtenida = item.CalificacionObtenida;

                Persona director = GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + item.DirectorTesis, pDicRutas);
                if (director != null)
                {
                    doctorado.nombreDirector = director.Nombre;
                    doctorado.primApeDirector = director.Apellidos;
                    doctorado.firmaDirector = director.Nombre + " " + director.Apellidos;
                }

                if (item.CoDirectorTesis != null && !string.IsNullOrEmpty(item.CoDirectorTesis.PersonaRef))
                {
                    Persona codirectorSGI = GetPersonaSGI(pHarvesterServices, pConfig, "Persona_" + item.CoDirectorTesis.PersonaRef, pDicRutas);
                    if (codirectorSGI != null)
                    {
                        DoctoradosBBDD.Codirector codirector = new DoctoradosBBDD.Codirector();
                        codirector.firstName = codirectorSGI.Nombre;
                        codirector.secondFamilyName = codirectorSGI.Apellidos;
                        codirector.nick = codirectorSGI.Nombre + " " + codirectorSGI.Apellidos;
                        codirector.comment = 1;
                        doctorado.codirectorTesis = new List<DoctoradosBBDD.Codirector>() { codirector };
                    }
                }

                doctorado.doctoradoEuropeo = item.DoctoradoEuropeo != null ? (bool)item.DoctoradoEuropeo : false;
                doctorado.fechaDoctorado = item.FechaMencionDoctoradoEuropeo;
                doctorado.mencionCalidad = item.MencionCalidad != null ? (bool)item.MencionCalidad : false;
                doctorado.premioExtraordinarioDoctor = item.PremioExtraordinarioDoctor != null ? (bool)item.PremioExtraordinarioDoctor : false;
                doctorado.fechaPremioDoctor = item.FechaPremioExtraordinarioDoctor;
                doctorado.tituloHomologado = item.TituloHomologado != null ? (bool)item.TituloHomologado : false;
                doctorado.fechaHomologado = item.FechaHomologacion;

                listaDoctorados.Add(doctorado);
            }

            return listaDoctorados;
        }

        public List<PosgradoBBDD> ObtenerPosgradosSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<Posgrado> pListaPosgradosSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<PosgradoBBDD> listaPosgrados = new List<PosgradoBBDD>();

            foreach (Posgrado item in pListaPosgradosSGI)
            {
                PosgradoBBDD posgrado = new PosgradoBBDD();

                string tituloTrabajo = RemoveDiacritics(item.NombreTituloPosgrado.ToLower());
                tituloTrabajo = tituloTrabajo.Replace(" ", "-");
                posgrado.crisIdentifier = $@"{item.Id}___{tituloTrabajo}";

                posgrado.nombreTitulo = item.NombreTituloPosgrado;

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

                // TODO: Ciudad, Pais, CCAA

                if (item.TipoFormacionHomologada != null && !string.IsNullOrEmpty(item.TipoFormacionHomologada.Nombre))
                {
                    posgrado.tipoFormacion = FormationType(item.TipoFormacionHomologada.Nombre, pResourceApi);
                }

                posgrado.calificacionObtenida = item.CalificacionObtenida;
                posgrado.tituloHomologado = item.TituloHomologado != null ? (bool)item.TituloHomologado : false;
                posgrado.fechaHomologacion = item.FechaHomologacion;

                listaPosgrados.Add(posgrado);
            }

            return listaPosgrados;
        }

        public List<FormacionEspecializadaBBDD> ObtenerFormacionEspecializadaSGI(RabbitServiceWriterDenormalizer pRabbitConf, List<FormacionEspecializada> pListaEspecializadaSGI, IHarvesterServices pHarvesterServices, ReadConfig pConfig, ResourceApi pResourceApi, Dictionary<string, HashSet<string>> pDicIdentificadores, Dictionary<string, Dictionary<string, string>> pDicRutas)
        {
            List<FormacionEspecializadaBBDD> listaFormEspecializada = new List<FormacionEspecializadaBBDD>();

            foreach (FormacionEspecializada item in pListaEspecializadaSGI)
            {
                FormacionEspecializadaBBDD formEspecializada = new FormacionEspecializadaBBDD();

                string tituloTrabajo = RemoveDiacritics(item.NombreTitulo.ToLower());
                tituloTrabajo = tituloTrabajo.Replace(" ", "-");
                formEspecializada.crisIdentifier = $@"{item.Id}___{tituloTrabajo}";

                formEspecializada.nombreTitulo = item.NombreTitulo;

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

                // TODO: Ciudad, Pais, CCAA

                formEspecializada.duracionHoras = item.DuracionTitulacion.Value;

                if (item.TipoFormacion != null && !string.IsNullOrEmpty(item.TipoFormacion.Nombre))
                {
                    formEspecializada.tipoFormacion = FormationType(item.TipoFormacion.Nombre, pResourceApi);
                }

                formEspecializada.objetivosEntidad = item.Objetivos;

                if (!string.IsNullOrEmpty(item.ResponsableFormacion))
                {
                    formEspecializada.firma = item.ResponsableFormacion;
                    if (item.ResponsableFormacion.Contains(" "))
                    {
                        formEspecializada.nombre = item.ResponsableFormacion.Split(" ")[0];
                        formEspecializada.primApe = item.ResponsableFormacion.Split(" ")[1];
                    }
                }

                listaFormEspecializada.Add(formEspecializada);
            }

            return listaFormEspecializada;
        }


        static string RemoveDiacritics(string text)
        {
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

        private static string IdentificadorPais(string pais, ResourceApi pResourceApi)
        {
            if (!UtilidadesGeneral.DicPaisesContienePais(pais))
            {
                return null;
            }
            return pResourceApi.GraphsUrl + "items/feature_PCLD_" + UtilidadesGeneral.dicPaises[pais];
        }

        private static string IdentificadorRegion(string region, ResourceApi pResourceApi)
        {
            if (!UtilidadesGeneral.DicRegionesContieneRegion(region))
            {
                return null;
            }
            return pResourceApi.GraphsUrl + "items/feature_ADM1_" + UtilidadesGeneral.dicRegiones[region];
        }

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
