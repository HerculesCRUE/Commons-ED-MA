using ContributiongradeprojectOntology;
using FeatureOntology;
using DedicationregimeOntology;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Hercules.CommonsEDMA.Load.Models.CVN;
using ProjectmodalityOntology;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static GnossBase.GnossOCBase;
using EventinscriptiontypeOntology;
using ContributiongradedocumentOntology;
using ReferencesourceOntology;
using ImpactindexcategoryOntology;
using LanguageOntology;
using PublicationtypeOntology;
using EventtypeOntology;
using GeographicregionOntology;
using OrganizationtypeOntology;
using DocumentformatOntology;
using GenderOntology;
using ParticipationtypeprojectOntology;
using ProjecttypeOntology;
using ParticipationtypedocumentOntology;
using IndustrialpropertytypeOntology;
using ColaborationtypegroupOntology;
using ManagementtypeactivityOntology;
using TargetgroupprofileOntology;
using AccesssystemactivityOntology;
using ParticipationtypeactivityOntology;
using StaygoalOntology;
using GrantaimOntology;
using RelationshiptypeOntology;
using ScientificactivitydocumentOntology;
using DepartmentOntology;
using ScientificexperienceprojectOntology;
using ActivitymodalityOntology;
using TaxonomyOntology;
using Hercules.CommonsEDMA.Load.Models.TaxonomyOntology;
using FramingsectorOntology;
using ResearchobjecttypeOntology;
using ContractmodalityOntology;
using ScopemanagementactivityOntology;
using DoctoralprogramtypeOntology;
using DegreetypeOntology;
using QualificationtypeOntology;
using PrizetypeOntology;
using HindexsourceOntology;
using UniversitydegreetypeOntology;
using FormationtypeOntology;
using PostgradedegreeOntology;
using LanguagelevelOntology;
using FormationactivitytypeOntology;
using TutorshipsprogramtypeOntology;
using ProjectcharactertypeOntology;
using TeachingtypeOntology;
using ModalityteachingtypeOntology;
using ProgramtypeOntology;
using CoursetypeOntology;
using HourscreditsectstypeOntology;
using EvaluationtypeOntology;
using CalltypeOntology;
using SupporttypeOntology;
using EventgeographicregionOntology;
using ResulttypeOntology;
using LaboraldurationtypeOntology;
using SeminarinscriptiontypeOntology;
using SeminareventtypeOntology;
using OfferstateOntology;
using MaturestateOntology;
using System.Data;
using ExcelDataReader;
using CongressTypeOntology;

namespace Hercules.CommonsEDMA.Load
{
    /// <summary>
    /// Clase encargada de cargar los datos de las entidades secundarias de Hércules-MA.
    /// </summary>
    public class CargaEntidades
    {
        //Ruta con el XML de datos a leer.
        private static string RUTA_XML = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Dataset{Path.DirectorySeparatorChar}CVN{Path.DirectorySeparatorChar}ReferenceTables.xml";
        private static string RUTA_XML_THESAURUS = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Dataset{Path.DirectorySeparatorChar}CVN{Path.DirectorySeparatorChar}Thesaurus.xml";

        //Resource API.
        public static ResourceApi mResourceApi = new ResourceApi($@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config");

        //Identificadores de las tablas.
        private static readonly string idPaises = "ISO_3166";
        private static readonly string idRegiones = "CVN_REGION";
        private static readonly string idProvincias = "CVN_PROVINCE";
        private static readonly string idParticipationTypeProject = "CVN_PARTICIPATION_A";
        private static readonly string idContributionGradeProject = "CVN_PARTICIPATION_B";
        private static readonly string idContributionGradeDocument = "CVN_PARTICIPATION_G";
        private static readonly string idProjectModality = "CVN_PROJECT_C";
        private static readonly string idDedicationRegime = "CVN_DEDICATION_A";
        private static readonly string idEventInscriptionType = "CVN_SUPERVISION_A";
        private static readonly string idReferenceSource = "CVN_AGENCY_B";
        private static readonly string idImpactIndexCategory = "CVN_CATEGORY_A";
        private static readonly string idLanguage = "ISO_639";
        private static readonly string idPublicationType = "CVN_PUBLICATION_A";
        private static readonly string idEventType = "CVN_EVENT_B";
        private static readonly string idGeographicRegion = "CVN_SCOPE_A";
        private static readonly string idOrganizationType = "CVN_ENTITY_TYPE";
        private static readonly string idDocumentFormat = "CVN_SUPPORT_B";
        private static readonly string idGender = "CVN_SEX_A";
        private static readonly string idProjectType = "CVN_PARTICIPATION_F";
        private static readonly string idParticipationTypeDocument = "CVN_PARTICIPATION_E";
        private static readonly string idIndustrialPropertyType = "CVN_KNOW_A";
        private static readonly string idColaborationTypeGroup = "CVN_COOPERANTION_A";
        private static readonly string idPublicationIdentifierType = "CVN_SOURCE_B";
        private static readonly string idManagementTypeActivity = "CVN_MANAGEMENT_A";
        private static readonly string idTargetGroupProfile = "CVN_AGENCY_A";
        private static readonly string idAccessSystemActivity = "CVN_ACCESS_A";
        private static readonly string idParticipationTypeActivity = "CVN_PARTICIPATION_C";
        private static readonly string idStayGoal = "CVN_STAY_A";
        private static readonly string idGrantAim = "CVN_SUMMONS_A";
        private static readonly string idRelationshipType = "CVN_COLLABORATION_A";
        private static readonly string idActivityModality = "CVN_ACTIVITY_A";
        private static readonly string idContractModality = "CVN_SITUATION_A";
        private static readonly string idScopeManagementActivity = "CVN_MANAGEMENT_TYPE_A";
        private static readonly string idQualificationType = "CVN_QUALIFICATION_B";
        private static readonly string idUniversityDegreeType = "CVN_TITLE_A";
        private static readonly string idDegreeType = "CVN_TITLE_B";
        private static readonly string idDoctoralProgramType = "CVN_TITLE_C";
        private static readonly string idPrizeType = "CVN_PRIZE_A";
        private static readonly string idFormationType = "CVN_FORMATION_A";
        private static readonly string idPostgradeDegree = "CVN_TITLE_D";
        private static readonly string idLanguageLevel = "CVN_LANGUAGE_B";
        private static readonly string idFormationActivityType = "CVN_ACTIVITY_B";
        private static readonly string idTutorshipsProgramType = "CVN_PROGRAMME_C";
        private static readonly string idProjectCharacterType = "CVN_PROJECT_A";
        private static readonly string idTeachingType = "CVN_TEACHING_A";
        private static readonly string idModalityTeachingType = "CVN_TEACHING_B";
        private static readonly string idProgramType = "CVN_PROGRAMME_A";
        private static readonly string idCourseType = "CVN_SUBJECT_A";
        private static readonly string idHoursCreditsECTSType = "CVN_TIME_A";
        private static readonly string idEvaluationType = "CVN_EVALUATION_A";
        private static readonly string idCallType = "CVN_SUMMONS_B";
        private static readonly string idSupportType = "CVN_SUPPORT_A";
        private static readonly string idEventGeographicRegion = "CVN_SCOPE_B";
        private static readonly string idResultType = "CVN_QUALIFICATION_A";
        private static readonly string idLaboralDurationType = "CVN_DURATION_A";
        private static readonly string idSeminarInscriptionType = "CVN_SUPERVISION_B";
        private static readonly string idSeminarEventType = "CVN_EVENT_C";

        //Número de hilos para el paralelismo.
        public static int NUM_HILOS = 6;

        /// <summary>
        /// Método para cargar las entidades secundarias.
        /// </summary>
        public static void CargarEntidades()
        {
            //Lectura del XML con los datos.
            XmlDocument documento_XML = new XmlDocument();
            documento_XML.Load(RUTA_XML);
            XmlSerializer serializer_XML = new XmlSerializer(typeof(ReferenceTables));
            ReferenceTables tablas = (ReferenceTables)serializer_XML.Deserialize(new StringReader(documento_XML.InnerXml));

            XmlDocument documento_THESAURUS = new XmlDocument();
            documento_THESAURUS.Load(RUTA_XML_THESAURUS);
            XmlSerializer serializer_THESAURUS = new XmlSerializer(typeof(Thesaurus));
            Thesaurus tablas_THESAURUS = (Thesaurus)serializer_THESAURUS.Deserialize(new StringReader(documento_THESAURUS.InnerXml));

            //Carga de entidades
            Console.WriteLine("1/67.- CargarTesauroAreaProcedencia");
            CargarTesauroAreaProcedencia();
            Console.WriteLine("2/67.- CargarTesauroSectorAplicacion");
            CargarTesauroSectorAplicacion();
            Console.WriteLine("3/67.- CargarTesauroResearchArea");
            CargarTesauroResearchArea();
            Console.WriteLine("4/67.- CargarFeatures");
            CargarFeatures(tablas, "feature");
            Console.WriteLine("5/67.- CargarProjectModality");
            CargarProjectModality(tablas, "projectmodality");
            Console.WriteLine("6/67.- CargarContributionGradeProject");
            CargarContributionGradeProject(tablas, "contributiongradeproject");
            Console.WriteLine("7/67.- CargarParticipationTypeProject");
            CargarParticipationTypeProject(tablas, "participationtypeproject");
            Console.WriteLine("8/67.- CargarDedicationRegime ");
            CargarDedicationRegime(tablas, "dedicationregime");
            Console.WriteLine("9/67.- CargarEventInscriptionType ");
            CargarEventInscriptionType(tablas, "eventinscriptiontype");
            Console.WriteLine("10/67.- CargarContributionGradeDocument ");
            CargarContributionGradeDocument(tablas, "contributiongradedocument");
            Console.WriteLine("11/67.- CargarReferenceSource ");
            CargarReferenceSource(tablas, "referencesource");
            Console.WriteLine("12/67.- CargarImpactIndexCategory ");
            CargarImpactIndexCategory(tablas, "impactindexcategory");
            Console.WriteLine("13/67.- CargarLanguage ");
            CargarLanguage(tablas, "language");
            Console.WriteLine("14/67.- CargarPublicationType ");
            CargarPublicationType(tablas, "publicationtype");
            Console.WriteLine("15/67.- CargarEventType ");
            CargarEventType(tablas, "eventtype");
            Console.WriteLine("16/67.- CargarGeographicRegion ");
            CargarGeographicRegion(tablas, "geographicregion");
            Console.WriteLine("17/67.- CargarOrganizationType ");
            CargarOrganizationType(tablas, "organizationtype");
            Console.WriteLine("18/67.- CargarDocumentFormat ");
            CargarDocumentFormat(tablas, "documentformat");
            Console.WriteLine("19/67.- CargarGender ");
            CargarGender(tablas, "gender");
            Console.WriteLine("20/67.- CargarProjectType ");
            CargarProjectType(tablas, "projecttype");
            Console.WriteLine("21/67.- CargarParticipationTypeDocument ");
            CargarParticipationTypeDocument(tablas, "participationtypedocument");
            Console.WriteLine("22/67.- CargarIndustrialPropertyType ");
            CargarIndustrialPropertyType(tablas, "industrialpropertytype");
            Console.WriteLine("23/67.- CargarColaborationTypeGroup ");
            CargarColaborationTypeGroup(tablas, "colaborationtypegroup");
            Console.WriteLine("24/67.- CargarManagementTypeActivity ");
            CargarManagementTypeActivity(tablas, "managementtypeactivity");
            Console.WriteLine("25/67.- CargarTargetGroupProfile ");
            CargarTargetGroupProfile(tablas, "targetgroupprofile");
            Console.WriteLine("26/67.- CargarAccessSystemActivity ");
            CargarAccessSystemActivity(tablas, "accesssystemactivity");
            Console.WriteLine("27/67.- CargarParticipationTypeActivity ");
            CargarParticipationTypeActivity(tablas, "participationtypeactivity");
            Console.WriteLine("28/67.- CargarStayGoal ");
            CargarStayGoal(tablas, "staygoal");
            Console.WriteLine("29/67.- CargarGrantAim ");
            CargarGrantAim(tablas, "grantaim");
            Console.WriteLine("30/67.- CargarRelationshipType ");
            CargarRelationshipType(tablas, "relationshiptype");
            Console.WriteLine("31/67.- CargarScientificActivityDocument ");
            CargarScientificActivityDocument("scientificactivitydocument");
            Console.WriteLine("32/67.- CargarScientificExperienceProject ");
            CargarScientificExperienceProject("scientificexperienceproject");
            Console.WriteLine("33/67.- CargarActivityModality ");
            CargarActivityModality(tablas, "activitymodality");
            Console.WriteLine("34/67.- CargarContractModality ");
            CargarContractModality(tablas, "contractmodality");
            Console.WriteLine("35/67.- CargarScopeManagementActivity ");
            CargarScopeManagementActivity(tablas, "scopemanagementactivity");
            Console.WriteLine("36/67.- CargarTesauroUnesco ");
            CargarTesauroUnesco(tablas, "taxonomy");
            Console.WriteLine("37/67.- CargarTesauroCVN ");
            CargarTesauroCVN(tablas_THESAURUS, "taxonomy");
            Console.WriteLine("38/67.- CargarHIndexSource ");
            CargarHIndexSource("hindexsource");
            Console.WriteLine("39/67.- CargarQualificationType ");
            CargarQualificationType(tablas, "qualificationtype");
            Console.WriteLine("40/67.- CargarUniversityDegreeType ");
            CargarUniversityDegreeType(tablas, "universitydegreetype");
            Console.WriteLine("41/67.- CargarDegreeType ");
            CargarDegreeType(tablas, "degreetype");
            Console.WriteLine("42/67.- CargarDoctoralProgramType ");
            CargarDoctoralProgramType(tablas, "doctoralprogramtype");
            Console.WriteLine("43/67.- CargarPrizeType ");
            CargarPrizeType(tablas, "prizetype");
            Console.WriteLine("44/67.- CargarFormationType ");
            CargarFormationType(tablas, "formationtype");
            Console.WriteLine("45/67.- CargarPostgradeDegree ");
            CargarPostgradeDegree(tablas, "postgradedegree");
            Console.WriteLine("46/67.- CargarFormationActivityType ");
            CargarFormationActivityType(tablas, "formationactivitytype");
            Console.WriteLine("47/67.- CargarLanguageLevel ");
            CargarLanguageLevel(tablas, "languagelevel");
            Console.WriteLine("48/67.- CargarTutorshipsProgramType ");
            CargarTutorshipsProgramType(tablas, "tutorshipsprogramtype");
            Console.WriteLine("49/67.- CargarProjectCharacterType ");
            CargarProjectCharacterType(tablas, "projectcharactertype");
            Console.WriteLine("50/67.- CargarTeachingType ");
            CargarTeachingType(tablas, "teachingtype");
            Console.WriteLine("51/67.- CargarModalityTeachingType ");
            CargarModalityTeachingType(tablas, "modalityteachingtype");
            Console.WriteLine("52/67.- CargarProgramType ");
            CargarProgramType(tablas, "programtype");
            Console.WriteLine("53/67.- CargarCourseType ");
            CargarCourseType(tablas, "coursetype");
            Console.WriteLine("54/67.- CargarHoursCreditsECTSType ");
            CargarHoursCreditsECTSType(tablas, "hourscreditsectstype");
            Console.WriteLine("55/67.- CargarEvaluationType ");
            CargarEvaluationType(tablas, "evaluationtype");
            Console.WriteLine("56/67.- CargarCallType ");
            CargarCallType(tablas, "calltype");
            Console.WriteLine("57/67.- CargarSupportType ");
            CargarSupportType(tablas, "supporttype");
            Console.WriteLine("58/67.- CargarEventGeographicRegion ");
            CargarEventGeographicRegion(tablas, "eventgeographicregion");
            Console.WriteLine("59/67.- CargarResultType ");
            CargarResultType(tablas, "resulttype");
            Console.WriteLine("60/67.- CargarLaboralDurationType ");
            CargarLaboralDurationType(tablas, "laboraldurationtype");
            Console.WriteLine("61/67.- CargarSeminarInscriptionType ");
            CargarSeminarInscriptionType(tablas, "seminarinscriptiontype");
            Console.WriteLine("62/67.- CargarSeminarEventType ");
            CargarSeminarEventType(tablas, "seminareventtype");
            Console.WriteLine("63/67.- CargarOfferState ");
            CargarOfferState("offerstate");
            Console.WriteLine("64/67.- CargarMatureStates ");
            CargarMatureStates("maturestate");
            Console.WriteLine("65/67.- CargarFramingSectors ");
            CargarFramingSectors("framingsector");
            Console.WriteLine("66/67.- CargarResearhObjectType ");
            CargarResearhObjectType();
            Console.WriteLine("67/67.- CargarCongressType ");
            CargarCongressType();
            Console.WriteLine("El proceso ha terminado");

        }

        /// <summary>
        /// Carga la entidad secundaria Feature.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarFeatures(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://www.geonames.org/ontology#Feature", pOntology);

            //Obtención de los objetos a cargar.
            List<FeatureOntology.Feature> features = new List<FeatureOntology.Feature>();
            features = ObtenerDatosFeature(pTablas, idPaises, "PCLD", features, pOntology);
            features = ObtenerDatosFeature(pTablas, idRegiones, "ADM1", features, pOntology);
            features = ObtenerDatosFeature(pTablas, idProvincias, "ADM2", features, pOntology);

            //Carga.
            Parallel.ForEach(features, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, feature =>
            {
                mResourceApi.LoadSecondaryResource(feature.ToGnossApiResource(mResourceApi, pOntology + "_" + feature.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos Feature a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pId">Número que se le agregará al ID creado.</param>
        /// <param name="pListaFeatures">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<FeatureOntology.Feature> ObtenerDatosFeature(ReferenceTables pTablas, string pCodigoTabla, string pId, List<FeatureOntology.Feature> pListaFeatures, string pOntology)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {

                    FeatureOntology.Feature feature = new FeatureOntology.Feature();
                    Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                    string identificador = $@"{pId}_{item.Code}";
                    foreach (TableItemNameDetail pais in item.Name)
                    {
                        LanguageEnum idioma = dicIdiomasMapeados[pais.lang];
                        string nombre = pais.Name;
                        dicIdioma.Add(idioma, nombre);
                    }

                    //Se agrega las propiedades.
                    feature.Dc_identifier = identificador;
                    feature.Gn_name = dicIdioma;
                    feature.Gn_featureCode = pId;

                    //Se comprueba que no haya ningún código padre que sea null y que no sea de país.
                    string codigoPadre = item.AntecesorCode;
                    if (string.IsNullOrEmpty(codigoPadre))
                    {
                        codigoPadre = null;
                        if (pId != "PCLD")
                        {
                            continue;
                        }
                    }

                    switch (pId)
                    {
                        case "ADM1":
                            //Contruyo el ID del padre, debido al error que hay en los datos.
                            if (!string.IsNullOrEmpty(codigoPadre) && Int32.Parse(codigoPadre) < 10)
                            {
                                codigoPadre = "00" + codigoPadre;
                            }
                            else if (!string.IsNullOrEmpty(codigoPadre) && Int32.Parse(codigoPadre) < 100)
                            {
                                codigoPadre = "0" + codigoPadre;
                            }

                            feature.IdGn_parentFeature = $@"{pOntology}_PCLD_{codigoPadre}";
                            feature.Gn_parentFeature = pListaFeatures.First(x => x.Dc_identifier == $@"PCLD_{codigoPadre}");
                            break;
                        case "ADM2":
                            feature.IdGn_parentFeature = $@"{pOntology}_ADM1_{codigoPadre}";
                            feature.Gn_parentFeature = pListaFeatures.First(x => x.Dc_identifier == $@"ADM1_{codigoPadre}");
                            break;
                    }

                    //Se guarda el objeto a la lista.
                    if (pCodigoTabla == idPaises && feature.Dc_identifier.Split('_')[1].Length == 3)
                    {
                        pListaFeatures.Add(feature);
                    }
                    else if (string.IsNullOrEmpty(item.Delegate) && (pCodigoTabla == idRegiones || pCodigoTabla == idProvincias))
                    {
                        pListaFeatures.Add(feature);
                    }
                }
            }

            return pListaFeatures;
        }

        /// <summary>
        /// Carga la entidad secundaria Modality.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarProjectModality(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ProjectModality", pOntology);

            //Obtención de los objetos a cargar.
            List<ProjectModality> modalities = new List<ProjectModality>();
            modalities = ObtenerDatosProjectModality(pTablas, idProjectModality, modalities);

            //Carga.
            Parallel.ForEach(modalities, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, modality =>
            {
                mResourceApi.LoadSecondaryResource(modality.ToGnossApiResource(mResourceApi, pOntology + "_" + modality.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos Modality a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaModality">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ProjectModality> ObtenerDatosProjectModality(ReferenceTables pTablas, string pCodigoTabla, List<ProjectModality> pListaModality)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ProjectModality modality = new ProjectModality();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        modality.Dc_identifier = identificador;
                        modality.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaModality.Add(modality);
                    }
                }
            }

            return pListaModality;
        }

        /// <summary>
        /// Carga la entidad secundaria OfferState.
        /// </summary>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarOfferState(string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/OfferState", pOntology);

            //Obtención de los objetos a cargar.
            List<OfferState> offers = new List<OfferState>();
            string[] titles = new string[] {
                "En Borrador",
                "En Revisión",
                "Validada",
                "Denegada",
                "Archivada",
            };

            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            offers = titles.Select((e, i) => {
                var texts = new Dictionary<LanguageEnum, string>();
                foreach (var lang in dicIdiomasMapeados)
                {
                    texts.Add(lang.Value, e);
                }
                return new OfferState() { Dc_identifier = "00" + (i + 1).ToString(), Dc_title =  texts };
            }).ToList();

            // contributions = ObtenerDatosContributionGradeProject(pTablas, idContributionGradeProject, contributions);

            //Carga.
            Parallel.ForEach(offers, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, offer =>
            {
                mResourceApi.LoadSecondaryResource(offer.ToGnossApiResource(mResourceApi, pOntology + "_" + offer.Dc_identifier));
            });
        }

        /// <summary>
        /// Carga la entidad secundaria MatureState.
        /// </summary>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarMatureStates(string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/MatureState", pOntology);

            //Obtención de los objetos a cargar.
            List<MatureState> states = new List<MatureState>();
            string[] sectors = new string[] {
                "En investigación (TRL 1-2)",
                "Tecnología validada en laboratorio (TRL 3-5)",
                "Tecnología demostrada con prototipo funcional (TRL 6-7)",
                "Sistema completo disponible para cliente-mercado (TRL 8-9)",
            };

            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            states = sectors.Select((e, i) => {
                var texts = new Dictionary<LanguageEnum, string>();
                foreach (var lang in dicIdiomasMapeados)
                {
                    texts.Add( lang.Value, e );
                }
                return new MatureState() { Dc_identifier = "00" + (i + 1).ToString(), Dc_title =  texts };
            }).ToList();

            // contributions = ObtenerDatosContributionGradeProject(pTablas, idContributionGradeProject, contributions);

            //Carga.
            Parallel.ForEach(states, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, state =>
            {
                mResourceApi.LoadSecondaryResource(state.ToGnossApiResource(mResourceApi, pOntology + "_" + state.Dc_identifier));
            });
        }

        /// <summary>
        /// Carga la entidad secundaria FramingSector.
        /// </summary>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarFramingSectors(string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/FramingSector", pOntology);

            //Obtención de los objetos a cargar.
            List<FramingSector> framings = new List<FramingSector>();
            string[] titles = new string[] {
                "Acuicultura y Economía Azul",
                "Agroalimentación",
                "Arte y Humanidades",
                "Biomedicina",
                "Biotecnología",
                "Ciencias Sociales y Jurídicas",
                "Deporte",
                "Energía y Medio Ambiente",
                "Medicina y Salud",
                "Nanotecnología",
                "Química",
                "Salud y Bienestar Animal",
                "TICS",
            };

            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();


            framings = titles.Select((e, i) => {
                var texts = new Dictionary<LanguageEnum, string>();
                foreach (var lang in dicIdiomasMapeados)
                {
                    texts.Add(lang.Value, e);
                }
                return new FramingSector() { Dc_identifier = "00" + (i + 1).ToString(), Dc_title =  texts };
            }).ToList();

            // framings = titles.Select((e, i) => new FramingSector() { Dc_identifier = i.ToString(), Dc_title = e }).ToList();

            // contributions = ObtenerDatosContributionGradeProject(pTablas, idContributionGradeProject, contributions);

            //Carga.
            Parallel.ForEach(framings, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, framing =>
            {
                mResourceApi.LoadSecondaryResource(framing.ToGnossApiResource(mResourceApi, pOntology + "_" + framing.Dc_identifier));
            });
        }

        /// <summary>
        /// Carga la entidad secundaria ContributionGradeProject.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarContributionGradeProject(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ContributionGradeProject", pOntology);

            //Obtención de los objetos a cargar.
            List<ContributionGradeProject> contributions = new List<ContributionGradeProject>();
            contributions = ObtenerDatosContributionGradeProject(pTablas, idContributionGradeProject, contributions);

            //Carga.
            Parallel.ForEach(contributions, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, contribution =>
            {
                mResourceApi.LoadSecondaryResource(contribution.ToGnossApiResource(mResourceApi, pOntology + "_" + contribution.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ContributionGradeProject a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaContributionGrade">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ContributionGradeProject> ObtenerDatosContributionGradeProject(ReferenceTables pTablas, string pCodigoTabla, List<ContributionGradeProject> pListaContributionGrade)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ContributionGradeProject contribution = new ContributionGradeProject();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail contribucion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[contribucion.lang];
                            string nombre = contribucion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        contribution.Dc_identifier = identificador;
                        contribution.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaContributionGrade.Add(contribution);
                    }
                }
            }

            return pListaContributionGrade;
        }

        /// <summary>
        /// Carga la entidad secundaria ParticipationType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarParticipationTypeProject(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ParticipationTypeProject", pOntology);

            //Obtención de los objetos a cargar.
            List<ParticipationTypeProject> participations = new List<ParticipationTypeProject>();
            participations = ObtenerDatosParticipationTypeProject(pTablas, idParticipationTypeProject, participations);

            //Carga.
            Parallel.ForEach(participations, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, participation =>
            {
                mResourceApi.LoadSecondaryResource(participation.ToGnossApiResource(mResourceApi, pOntology + "_" + participation.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ParticipationType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaParticipationType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ParticipationTypeProject> ObtenerDatosParticipationTypeProject(ReferenceTables pTablas, string pCodigoTabla, List<ParticipationTypeProject> pListaParticipationType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ParticipationTypeProject participation = new ParticipationTypeProject();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail participacion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[participacion.lang];
                            string nombre = participacion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        participation.Dc_identifier = identificador;
                        participation.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaParticipationType.Add(participation);
                    }
                }
            }

            return pListaParticipationType;
        }

        /// <summary>
        /// Carga la entidad secundaria DedicationRegime.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarDedicationRegime(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/DedicationRegime", pOntology);

            //Obtención de los objetos a cargar.
            List<DedicationRegime> dedications = new List<DedicationRegime>();
            dedications = ObtenerDatosDedicationRegime(pTablas, idDedicationRegime, dedications);

            //Carga.
            Parallel.ForEach(dedications, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, dedication =>
            {
                mResourceApi.LoadSecondaryResource(dedication.ToGnossApiResource(mResourceApi, pOntology + "_" + dedication.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos DedicationRegime a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDedicationRegime">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<DedicationRegime> ObtenerDatosDedicationRegime(ReferenceTables pTablas, string pCodigoTabla, List<DedicationRegime> pListaDedicationRegime)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        DedicationRegime dedication = new DedicationRegime();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail dedicacion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[dedicacion.lang];
                            string nombre = dedicacion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        dedication.Dc_identifier = identificador;
                        dedication.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDedicationRegime.Add(dedication);
                    }
                }
            }

            return pListaDedicationRegime;
        }

        /// <summary>
        /// Carga la entidad secundaria Motivation.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarEventInscriptionType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/EventInscriptionType", pOntology);

            //Obtención de los objetos a cargar.
            List<EventInscriptionType> motivations = new List<EventInscriptionType>();
            motivations = ObtenerDatosEventInscriptionType(pTablas, idEventInscriptionType, motivations);

            //Carga.
            Parallel.ForEach(motivations, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, motivation =>
            {
                mResourceApi.LoadSecondaryResource(motivation.ToGnossApiResource(mResourceApi, pOntology + "_" + motivation.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos Motivation a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaMotivation">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<EventInscriptionType> ObtenerDatosEventInscriptionType(ReferenceTables pTablas, string pCodigoTabla, List<EventInscriptionType> pListaMotivation)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        EventInscriptionType motivation = new EventInscriptionType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail motivacion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[motivacion.lang];
                            string nombre = motivacion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        motivation.Dc_identifier = identificador;
                        motivation.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaMotivation.Add(motivation);
                    }
                }
            }

            return pListaMotivation;
        }

        /// <summary>
        /// Carga la entidad secundaria ContributionGradeDocument.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarContributionGradeDocument(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ContributionGradeDocument", pOntology);

            //Obtención de los objetos a cargar.
            List<ContributionGradeDocument> contributions = new List<ContributionGradeDocument>();
            contributions = ObtenerDatosContributionGradeDocument(pTablas, idContributionGradeDocument, contributions);

            //Carga.
            Parallel.ForEach(contributions, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, contribution =>
            {
                mResourceApi.LoadSecondaryResource(contribution.ToGnossApiResource(mResourceApi, pOntology + "_" + contribution.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ContributionGradeDocument a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaContributionGrade">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ContributionGradeDocument> ObtenerDatosContributionGradeDocument(ReferenceTables pTablas, string pCodigoTabla, List<ContributionGradeDocument> pListaContributionGrade)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ContributionGradeDocument contribution = new ContributionGradeDocument();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail contribucion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[contribucion.lang];
                            string nombre = contribucion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        contribution.Dc_identifier = identificador;
                        contribution.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaContributionGrade.Add(contribution);
                    }
                }
            }

            return pListaContributionGrade;
        }

        /// <summary>
        /// Carga la entidad secundaria ReferenceSource.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarReferenceSource(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://purl.org/ontology/bibo/ReferenceSource", pOntology);

            //Obtención de los objetos a cargar.
            List<ReferenceSource> references = new List<ReferenceSource>();
            references = ObtenerDatosReferenceSource(pTablas, idReferenceSource, references);

            //Carga.
            Parallel.ForEach(references, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, reference =>
            {
                mResourceApi.LoadSecondaryResource(reference.ToGnossApiResource(mResourceApi, pOntology + "_" + reference.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ReferenceSource a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaReferenceSource">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ReferenceSource> ObtenerDatosReferenceSource(ReferenceTables pTablas, string pCodigoTabla, List<ReferenceSource> pListaReferenceSource)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ReferenceSource reference = new ReferenceSource();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail referencia in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[referencia.lang];
                            string nombre = referencia.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        reference.Dc_identifier = identificador;
                        reference.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaReferenceSource.Add(reference);
                    }
                }
            }

            return pListaReferenceSource;
        }

        /// <summary>
        /// Carga la entidad secundaria ImpactIndexCategory.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarImpactIndexCategory(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ImpactIndexCategory", pOntology);

            //Obtención de los objetos a cargar.
            List<ImpactIndexCategory> categorias = new List<ImpactIndexCategory>();
            categorias = ObtenerDatosImpactIndexCategory(pTablas, idImpactIndexCategory, categorias);

            //Carga.
            Parallel.ForEach(categorias, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, category =>
            {
                mResourceApi.LoadSecondaryResource(category.ToGnossApiResource(mResourceApi, pOntology + "_" + category.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ImpactIndexCategory a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaImpactIndexCategory">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ImpactIndexCategory> ObtenerDatosImpactIndexCategory(ReferenceTables pTablas, string pCodigoTabla, List<ImpactIndexCategory> pListaImpactIndexCategory)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ImpactIndexCategory categories = new ImpactIndexCategory();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail categoria in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[categoria.lang];
                            string nombre = categoria.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        categories.Dc_identifier = identificador;
                        categories.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaImpactIndexCategory.Add(categories);
                    }
                }
            }

            return pListaImpactIndexCategory;
        }

        /// <summary>
        /// Carga la entidad secundaria Language.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarLanguage(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/Language", pOntology);

            //Obtención de los objetos a cargar.
            List<Language> lenguajes = new List<Language>();
            lenguajes = ObtenerDatosLanguage(pTablas, idLanguage, lenguajes);

            //Carga.
            Parallel.ForEach(lenguajes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, language =>
            {
                mResourceApi.LoadSecondaryResource(language.ToGnossApiResource(mResourceApi, pOntology + "_" + language.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos Language a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaLanguage">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<Language> ObtenerDatosLanguage(ReferenceTables pTablas, string pCodigoTabla, List<Language> pListaLanguage)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        Language language = new Language();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail lenguaje in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[lenguaje.lang];
                            string nombre = lenguaje.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        language.Dc_identifier = identificador;
                        language.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaLanguage.Add(language);
                    }
                }
            }

            return pListaLanguage;
        }

        /// <summary>
        /// Carga la entidad secundaria PublicationType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarPublicationType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/PublicationType", pOntology);

            //Obtención de los objetos a cargar.
            List<PublicationType> publicaciones = new List<PublicationType>();
            publicaciones = ObtenerDatosPublicationType(pTablas, idPublicationType, publicaciones);

            //Carga.
            Parallel.ForEach(publicaciones, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, publication =>
            {
                mResourceApi.LoadSecondaryResource(publication.ToGnossApiResource(mResourceApi, pOntology + "_" + publication.Dc_identifier));
            });
        }

        /// <summary>
        /// Carga la entidad secundaria ResearchObjcetType.
        /// </summary>
        private static void CargarResearhObjectType()
        {
            string ontology = "researchobjecttype";
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(ontology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ResearchObjectType", ontology);

            //Obtención de los objetos a cargar.
            List<ResearchObjectType> researchObjects = new List<ResearchObjectType>();
            researchObjects.Add(
                new ResearchObjectType()
                {
                    Dc_identifier = "1",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Dataset" } },
                }
            );
            researchObjects.Add(
                new ResearchObjectType()
                {
                    Dc_identifier = "2",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Presentación" } },
                }
            );
            researchObjects.Add(
                new ResearchObjectType()
                {
                    Dc_identifier = "3",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Gráfico" } },
                }
            );
            researchObjects.Add(
                new ResearchObjectType()
                {
                    Dc_identifier = "4",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Documento" } },
                }
            );
            researchObjects.Add(
                new ResearchObjectType()
                {
                    Dc_identifier = "5",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Enlace" } },
                }
            );
            researchObjects.Add(
                new ResearchObjectType()
                {
                    Dc_identifier = "6",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Video" } },
                }
            );
            researchObjects.Add(
                new ResearchObjectType()
                {
                    Dc_identifier = "7",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Poster" } },
                }
            );
            researchObjects.Add(
                new ResearchObjectType()
                {
                    Dc_identifier = "8",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Lección" } },
                }
            );
            researchObjects.Add(
                new ResearchObjectType()
                {
                    Dc_identifier = "9",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Código" } },
                }
            );

            //Carga.
            Parallel.ForEach(researchObjects, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, ro =>
            {
                mResourceApi.LoadSecondaryResource(ro.ToGnossApiResource(mResourceApi, ontology + "_" + ro.Dc_identifier));
            });
        }

        /// <summary>
        /// Carga la entidad secundaria CongressType.
        /// </summary>
        private static void CargarCongressType()
        {
            string ontology = "congresstype";
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(ontology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/CongressType", ontology);

            //Obtención de los objetos a cargar.
            List<CongressType> congressTypes = new List<CongressType>();
            congressTypes.Add(
                new CongressType()
                {
                    Dc_identifier = "1",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Clase 1" } },
                }
            );
            congressTypes.Add(
                new CongressType()
                {
                    Dc_identifier = "2",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Clase 2" } },
                }
            );
            congressTypes.Add(
                new CongressType()
                {
                    Dc_identifier = "3",
                    Dc_title = new Dictionary<LanguageEnum, string>() { { LanguageEnum.es, "Clase 3" } },
                }
            );
            //Carga.
            Parallel.ForEach(congressTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, ro =>
            {
                mResourceApi.LoadSecondaryResource(ro.ToGnossApiResource(mResourceApi, ontology + "_" + ro.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos Language a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaPublicationType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<PublicationType> ObtenerDatosPublicationType(ReferenceTables pTablas, string pCodigoTabla, List<PublicationType> pListaPublicationType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        PublicationType publication = new PublicationType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail publicacion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[publicacion.lang];
                            string nombre = publicacion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        publication.Dc_identifier = identificador;
                        publication.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaPublicationType.Add(publication);
                    }
                }
            }

            return pListaPublicationType;
        }

        /// <summary>
        /// Carga la entidad secundaria EventType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarEventType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/EventType", pOntology);

            //Obtención de los objetos a cargar.
            List<EventType> eventos = new List<EventType>();
            eventos = ObtenerDatosEventType(pTablas, idEventType, eventos);

            //Carga.
            Parallel.ForEach(eventos, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, eventType =>
            {
                mResourceApi.LoadSecondaryResource(eventType.ToGnossApiResource(mResourceApi, pOntology + "_" + eventType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos EventType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaEventType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<EventType> ObtenerDatosEventType(ReferenceTables pTablas, string pCodigoTabla, List<EventType> pListaEventType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        EventType eventType = new EventType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail evento in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[evento.lang];
                            string nombre = evento.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        eventType.Dc_identifier = identificador;
                        eventType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaEventType.Add(eventType);
                    }
                }
            }

            return pListaEventType;
        }

        /// <summary>
        /// Carga la entidad secundaria GeographicRegion.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarGeographicRegion(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://vivoweb.org/ontology/core#GeographicRegion", pOntology);

            //Obtención de los objetos a cargar.
            List<GeographicRegion> regiones = new List<GeographicRegion>();
            regiones = ObtenerDatosGeographicRegion(pTablas, idGeographicRegion, regiones);

            //Carga.
            Parallel.ForEach(regiones, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, region =>
            {
                mResourceApi.LoadSecondaryResource(region.ToGnossApiResource(mResourceApi, pOntology + "_" + region.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos GeographicRegion a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaGeographicRegion">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<GeographicRegion> ObtenerDatosGeographicRegion(ReferenceTables pTablas, string pCodigoTabla, List<GeographicRegion> pListaGeographicRegion)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        GeographicRegion geographicRegion = new GeographicRegion();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail region in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[region.lang];
                            string nombre = region.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        geographicRegion.Dc_identifier = identificador;
                        geographicRegion.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaGeographicRegion.Add(geographicRegion);
                    }
                }
            }

            return pListaGeographicRegion;
        }

        /// <summary>
        /// Carga la entidad secundaria OrganizationType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarOrganizationType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/OrganizationType", pOntology);

            //Obtención de los objetos a cargar.
            List<OrganizationType> organizaciones = new List<OrganizationType>();
            organizaciones = ObtenerDatosOrganizationType(pTablas, idOrganizationType, organizaciones);

            //Carga.
            Parallel.ForEach(organizaciones, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, organization =>
            {
                mResourceApi.LoadSecondaryResource(organization.ToGnossApiResource(mResourceApi, pOntology + "_" + organization.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos OrganizationType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaOrganizationType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<OrganizationType> ObtenerDatosOrganizationType(ReferenceTables pTablas, string pCodigoTabla, List<OrganizationType> pListaOrganizationType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        OrganizationType organization = new OrganizationType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail organizacion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[organizacion.lang];
                            string nombre = organizacion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        organization.Dc_identifier = identificador;
                        organization.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaOrganizationType.Add(organization);
                    }
                }
            }

            return pListaOrganizationType;
        }

        /// <summary>
        /// Carga la entidad secundaria DocumentFormat.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarDocumentFormat(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/DocumentFormat", pOntology);

            //Obtención de los objetos a cargar.
            List<DocumentFormat> publicaciones = new List<DocumentFormat>();
            publicaciones = ObtenerDatosDocumentFormat(pTablas, idDocumentFormat, publicaciones);

            //Carga.
            Parallel.ForEach(publicaciones, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, publication =>
            {
                mResourceApi.LoadSecondaryResource(publication.ToGnossApiResource(mResourceApi, pOntology + "_" + publication.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos Language a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDocumentFormat">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<DocumentFormat> ObtenerDatosDocumentFormat(ReferenceTables pTablas, string pCodigoTabla, List<DocumentFormat> pListaDocumentFormat)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        DocumentFormat publication = new DocumentFormat();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail publicacion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[publicacion.lang];
                            string nombre = publicacion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        publication.Dc_identifier = identificador;
                        publication.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDocumentFormat.Add(publication);
                    }
                }
            }

            return pListaDocumentFormat;
        }

        /// <summary>
        /// Carga la entidad secundaria Gender.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarGender(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/Gender", pOntology);

            //Obtención de los objetos a cargar.
            List<Gender> generos = new List<Gender>();
            generos = ObtenerDatosGender(pTablas, idGender, generos);

            //Carga.
            Parallel.ForEach(generos, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, genre =>
            {
                mResourceApi.LoadSecondaryResource(genre.ToGnossApiResource(mResourceApi, pOntology + "_" + genre.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos Gender a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaGender">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<Gender> ObtenerDatosGender(ReferenceTables pTablas, string pCodigoTabla, List<Gender> pListaGender)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        Gender genre = new Gender();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail genero in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[genero.lang];
                            string nombre = genero.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        genre.Dc_identifier = identificador;
                        genre.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaGender.Add(genre);
                    }
                }
            }

            return pListaGender;
        }

        /// <summary>
        /// Carga la entidad secundaria ProjectType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarProjectType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ProjectType", pOntology);

            //Obtención de los objetos a cargar.
            List<ProjectType> tipoProyectos = new List<ProjectType>();
            tipoProyectos = ObtenerDatosProjectType(pTablas, idProjectType, tipoProyectos);

            //Carga.
            Parallel.ForEach(tipoProyectos, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, project =>
            {
                mResourceApi.LoadSecondaryResource(project.ToGnossApiResource(mResourceApi, pOntology + "_" + project.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ProjectType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaProjectType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ProjectType> ObtenerDatosProjectType(ReferenceTables pTablas, string pCodigoTabla, List<ProjectType> pListaProjectType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ProjectType projectType = new ProjectType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail tipoProyecto in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[tipoProyecto.lang];
                            string nombre = tipoProyecto.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        projectType.Dc_identifier = identificador;
                        projectType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaProjectType.Add(projectType);
                    }
                }
            }

            return pListaProjectType;
        }

        /// <summary>
        /// Carga la entidad secundaria ProjectType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarParticipationTypeDocument(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ParticipationTypeDocument", pOntology);

            //Obtención de los objetos a cargar.
            List<ParticipationTypeDocument> tipoParticipacion = new List<ParticipationTypeDocument>();
            tipoParticipacion = ObtenerDatosParticipationTypeDocument(pTablas, idParticipationTypeDocument, tipoParticipacion);

            //Carga.
            Parallel.ForEach(tipoParticipacion, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, participacion =>
            {
                mResourceApi.LoadSecondaryResource(participacion.ToGnossApiResource(mResourceApi, pOntology + "_" + participacion.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ProjectType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaProjectType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ParticipationTypeDocument> ObtenerDatosParticipationTypeDocument(ReferenceTables pTablas, string pCodigoTabla, List<ParticipationTypeDocument> pListaDatosParticipationTypeDocument)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ParticipationTypeDocument participationType = new ParticipationTypeDocument();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail tipoParticipacion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[tipoParticipacion.lang];
                            string nombre = tipoParticipacion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        participationType.Dc_identifier = identificador;
                        participationType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosParticipationTypeDocument.Add(participationType);
                    }
                }
            }

            return pListaDatosParticipationTypeDocument;
        }

        /// <summary>
        /// Carga la entidad secundaria IndustrialPropertyType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarIndustrialPropertyType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/IndustrialPropertyType", pOntology);

            //Obtención de los objetos a cargar.
            List<IndustrialPropertyType> propiedadIndustrial = new List<IndustrialPropertyType>();
            propiedadIndustrial = ObtenerDatosIndustrialPropertyType(pTablas, idIndustrialPropertyType, propiedadIndustrial);

            //Carga.
            Parallel.ForEach(propiedadIndustrial, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, propiedad =>
            {
                mResourceApi.LoadSecondaryResource(propiedad.ToGnossApiResource(mResourceApi, pOntology + "_" + propiedad.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos IndustrialPropertyType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosIndustrialPropertyType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<IndustrialPropertyType> ObtenerDatosIndustrialPropertyType(ReferenceTables pTablas, string pCodigoTabla, List<IndustrialPropertyType> pListaDatosIndustrialPropertyType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        IndustrialPropertyType idustrialProperty = new IndustrialPropertyType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail propiedadIndustrial in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[propiedadIndustrial.lang];
                            string nombre = propiedadIndustrial.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        idustrialProperty.Dc_identifier = identificador;
                        idustrialProperty.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosIndustrialPropertyType.Add(idustrialProperty);
                    }
                }
            }

            return pListaDatosIndustrialPropertyType;
        }

        /// <summary>
        /// Carga la entidad secundaria ColaborationTypeGroup.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarColaborationTypeGroup(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ColaborationTypeGroup", pOntology);

            //Obtención de los objetos a cargar.
            List<ColaborationTypeGroup> grupoColaborativo = new List<ColaborationTypeGroup>();
            grupoColaborativo = ObtenerDatosColaborationTypeGroup(pTablas, idColaborationTypeGroup, grupoColaborativo);

            //Carga.
            Parallel.ForEach(grupoColaborativo, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, grupo =>
            {
                mResourceApi.LoadSecondaryResource(grupo.ToGnossApiResource(mResourceApi, pOntology + "_" + grupo.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ColaborationTypeGroup a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosColaborationTypeGroup">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ColaborationTypeGroup> ObtenerDatosColaborationTypeGroup(ReferenceTables pTablas, string pCodigoTabla, List<ColaborationTypeGroup> pListaDatosColaborationTypeGroup)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ColaborationTypeGroup colaborationGroup = new ColaborationTypeGroup();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail grupoColaboracion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[grupoColaboracion.lang];
                            string nombre = grupoColaboracion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        colaborationGroup.Dc_identifier = identificador;
                        colaborationGroup.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosColaborationTypeGroup.Add(colaborationGroup);
                    }
                }
            }

            return pListaDatosColaborationTypeGroup;
        }

        /// <summary>
        /// Carga la entidad secundaria ManagementTypeActivity.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarManagementTypeActivity(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ManagementTypeActivity", pOntology);

            //Obtención de los objetos a cargar.
            List<ManagementTypeActivity> tipoActividad = new List<ManagementTypeActivity>();
            tipoActividad = ObtenerDatosManagementTypeActivity(pTablas, idManagementTypeActivity, tipoActividad);

            //Carga.
            Parallel.ForEach(tipoActividad, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, typeActivity =>
            {
                mResourceApi.LoadSecondaryResource(typeActivity.ToGnossApiResource(mResourceApi, pOntology + "_" + typeActivity.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ManagementTypeActivity a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosManagementTypeActivity">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ManagementTypeActivity> ObtenerDatosManagementTypeActivity(ReferenceTables pTablas, string pCodigoTabla, List<ManagementTypeActivity> pListaDatosManagementTypeActivity)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ManagementTypeActivity typeActivity = new ManagementTypeActivity();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail tipoActividad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[tipoActividad.lang];
                            string nombre = tipoActividad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        typeActivity.Dc_identifier = identificador;
                        typeActivity.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosManagementTypeActivity.Add(typeActivity);
                    }
                }
            }

            return pListaDatosManagementTypeActivity;
        }

        /// <summary>
        /// Carga la entidad secundaria TargetGroupProfile.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarTargetGroupProfile(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/TargetGroupProfile", pOntology);

            //Obtención de los objetos a cargar.
            List<TargetGroupProfile> perfilGrupo = new List<TargetGroupProfile>();
            perfilGrupo = ObtenerDatosTargetGroupProfile(pTablas, idTargetGroupProfile, perfilGrupo);

            //Carga.
            Parallel.ForEach(perfilGrupo, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, groupProfile =>
            {
                mResourceApi.LoadSecondaryResource(groupProfile.ToGnossApiResource(mResourceApi, pOntology + "_" + groupProfile.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos TargetGroupProfile a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosTargetGroupProfile">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<TargetGroupProfile> ObtenerDatosTargetGroupProfile(ReferenceTables pTablas, string pCodigoTabla, List<TargetGroupProfile> pListaDatosTargetGroupProfile)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        TargetGroupProfile groupProfile = new TargetGroupProfile();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail perfilGrupo in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[perfilGrupo.lang];
                            string nombre = perfilGrupo.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        groupProfile.Dc_identifier = identificador;
                        groupProfile.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosTargetGroupProfile.Add(groupProfile);
                    }
                }
            }

            return pListaDatosTargetGroupProfile;
        }

        /// <summary>
        /// Carga la entidad secundaria AccessSystemActivity.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarAccessSystemActivity(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/AccessSystemActivity", pOntology);

            //Obtención de los objetos a cargar.
            List<AccessSystemActivity> sistemaAcceso = new List<AccessSystemActivity>();
            sistemaAcceso = ObtenerDatosAccessSystemActivity(pTablas, idAccessSystemActivity, sistemaAcceso);

            //Carga.
            Parallel.ForEach(sistemaAcceso, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, accessSistem =>
            {
                mResourceApi.LoadSecondaryResource(accessSistem.ToGnossApiResource(mResourceApi, pOntology + "_" + accessSistem.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos AccessSystemActivity a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosAccessSystemActivity">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<AccessSystemActivity> ObtenerDatosAccessSystemActivity(ReferenceTables pTablas, string pCodigoTabla, List<AccessSystemActivity> pListaDatosAccessSystemActivity)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        AccessSystemActivity accessSystem = new AccessSystemActivity();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail sistemaAcceso in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[sistemaAcceso.lang];
                            string nombre = sistemaAcceso.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        accessSystem.Dc_identifier = identificador;
                        accessSystem.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosAccessSystemActivity.Add(accessSystem);
                    }
                }
            }

            return pListaDatosAccessSystemActivity;
        }

        /// <summary>
        /// Carga la entidad secundaria ParticipationTypeActivity.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarParticipationTypeActivity(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ParticipationTypeActivity", pOntology);

            //Obtención de los objetos a cargar.
            List<ParticipationTypeActivity> tipoActividad = new List<ParticipationTypeActivity>();
            tipoActividad = ObtenerDatosParticipationTypeActivity(pTablas, idParticipationTypeActivity, tipoActividad);

            //Carga.
            Parallel.ForEach(tipoActividad, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, activityType =>
            {
                mResourceApi.LoadSecondaryResource(activityType.ToGnossApiResource(mResourceApi, pOntology + "_" + activityType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ParticipationTypeActivity a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosParticipationTypeActivity">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ParticipationTypeActivity> ObtenerDatosParticipationTypeActivity(ReferenceTables pTablas, string pCodigoTabla, List<ParticipationTypeActivity> pListaDatosParticipationTypeActivity)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ParticipationTypeActivity participationType = new ParticipationTypeActivity();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail tipoParticipacion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[tipoParticipacion.lang];
                            string nombre = tipoParticipacion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        participationType.Dc_identifier = identificador;
                        participationType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosParticipationTypeActivity.Add(participationType);
                    }
                }
            }

            return pListaDatosParticipationTypeActivity;
        }

        /// <summary>
        /// Carga la entidad secundaria StayGoal.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarStayGoal(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/StayGoal", pOntology);

            //Obtención de los objetos a cargar.
            List<StayGoal> meta = new List<StayGoal>();
            meta = ObtenerDatosStayGoal(pTablas, idStayGoal, meta);

            //Carga.
            Parallel.ForEach(meta, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, stayGoal =>
            {
                mResourceApi.LoadSecondaryResource(stayGoal.ToGnossApiResource(mResourceApi, pOntology + "_" + stayGoal.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos StayGoal a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosStayGoal">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<StayGoal> ObtenerDatosStayGoal(ReferenceTables pTablas, string pCodigoTabla, List<StayGoal> pListaDatosStayGoal)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        StayGoal stayGoal = new StayGoal();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail meta in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[meta.lang];
                            string nombre = meta.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        stayGoal.Dc_identifier = identificador;
                        stayGoal.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosStayGoal.Add(stayGoal);
                    }
                }
            }

            return pListaDatosStayGoal;
        }

        /// <summary>
        /// Carga la entidad secundaria GrantAim.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarGrantAim(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/GrantAim", pOntology);

            //Obtención de los objetos a cargar.
            List<GrantAim> objetivo = new List<GrantAim>();
            objetivo = ObtenerDatosGrantAim(pTablas, idGrantAim, objetivo);

            //Carga.
            Parallel.ForEach(objetivo, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, grantAim =>
            {
                mResourceApi.LoadSecondaryResource(grantAim.ToGnossApiResource(mResourceApi, pOntology + "_" + grantAim.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos GrantAim a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosGrantAim">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<GrantAim> ObtenerDatosGrantAim(ReferenceTables pTablas, string pCodigoTabla, List<GrantAim> pListaDatosGrantAim)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        GrantAim grantAim = new GrantAim();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail objetivo in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[objetivo.lang];
                            string nombre = objetivo.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        grantAim.Dc_identifier = identificador;
                        grantAim.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosGrantAim.Add(grantAim);
                    }
                }
            }

            return pListaDatosGrantAim;
        }

        /// <summary>
        /// Carga la entidad secundaria RelationshipType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarRelationshipType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/RelationshipType", pOntology);

            //Obtención de los objetos a cargar.
            List<RelationshipType> tipoRelacion = new List<RelationshipType>();
            tipoRelacion = ObtenerDatosRelationshipType(pTablas, idRelationshipType, tipoRelacion);

            //Carga.
            Parallel.ForEach(tipoRelacion, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, relationType =>
            {
                mResourceApi.LoadSecondaryResource(relationType.ToGnossApiResource(mResourceApi, pOntology + "_" + relationType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos RelationshipType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosRelationshipType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<RelationshipType> ObtenerDatosRelationshipType(ReferenceTables pTablas, string pCodigoTabla, List<RelationshipType> pListaDatosRelationshipType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        RelationshipType relationType = new RelationshipType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail tipoRelacion in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[tipoRelacion.lang];
                            string nombre = tipoRelacion.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        relationType.Dc_identifier = identificador;
                        relationType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosRelationshipType.Add(relationType);
                    }
                }
            }

            return pListaDatosRelationshipType;
        }

        /// <summary>
        /// Carga la entidad secundaria ActivityModality.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarActivityModality(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ActivityModality", pOntology);

            //Obtención de los objetos a cargar.
            List<ActivityModality> modalidad = new List<ActivityModality>();
            modalidad = ObtenerDatosActivityModality(pTablas, idActivityModality, modalidad);

            //Carga.
            Parallel.ForEach(modalidad, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, modality =>
            {
                mResourceApi.LoadSecondaryResource(modality.ToGnossApiResource(mResourceApi, pOntology + "_" + modality.Dc_identifier));
            });
        }

        /// <summary>
        /// Carga la entidad secundaria ContractModality.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarContractModality(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ContractModality", pOntology);

            //Obtención de los objetos a cargar.
            List<ContractModality> modalidad = new List<ContractModality>();
            modalidad = ObtenerDatosContractModality(pTablas, idContractModality, modalidad);

            //Carga.
            Parallel.ForEach(modalidad, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, modality =>
            {
                mResourceApi.LoadSecondaryResource(modality.ToGnossApiResource(mResourceApi, pOntology + "_" + modality.Dc_identifier));
            });
        }

        /// <summary>
        /// Carga la entidad secundaria ScopeManagementActivity.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarScopeManagementActivity(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ScopeManagementActivity", pOntology);

            //Obtención de los objetos a cargar.
            List<ScopeManagementActivity> modalidad = new List<ScopeManagementActivity>();
            modalidad = ObtenerDatosScopeManagementActivity(pTablas, idScopeManagementActivity, modalidad);

            //Carga.
            Parallel.ForEach(modalidad, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, modality =>
            {
                mResourceApi.LoadSecondaryResource(modality.ToGnossApiResource(mResourceApi, pOntology + "_" + modality.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos RelationshipType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosActivityModality">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ActivityModality> ObtenerDatosActivityModality(ReferenceTables pTablas, string pCodigoTabla, List<ActivityModality> pListaDatosActivityModality)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ActivityModality modality = new ActivityModality();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        modality.Dc_identifier = identificador;
                        modality.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosActivityModality.Add(modality);
                    }
                }
            }

            return pListaDatosActivityModality;
        }

        /// <summary>
        /// Obtiene los objetos RelationshipType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosActivityModality">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ContractModality> ObtenerDatosContractModality(ReferenceTables pTablas, string pCodigoTabla, List<ContractModality> pListaDatosConstractModality)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ContractModality modality = new ContractModality();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        modality.Dc_identifier = identificador;
                        modality.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosConstractModality.Add(modality);
                    }
                }
            }

            return pListaDatosConstractModality;
        }

        /// <summary>
        /// Obtiene los objetos RelationshipType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosActivityModality">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ScopeManagementActivity> ObtenerDatosScopeManagementActivity(ReferenceTables pTablas, string pCodigoTabla, List<ScopeManagementActivity> pListaDatosScopeManagementActivity)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ScopeManagementActivity modality = new ScopeManagementActivity();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        modality.Dc_identifier = identificador;
                        modality.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosScopeManagementActivity.Add(modality);
                    }
                }
            }

            return pListaDatosScopeManagementActivity;
        }

        /// <summary>
        /// Carga el tesauro de unesco
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarTesauroUnesco(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Collection", "taxonomy", "unesco");
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Concept", "taxonomy", "unesco");

            //Obtención de los objetos a cargar.
            List<SecondaryResource> categorias = ObtenerDatosUnesco(pTablas, "unesco");

            //Carga.
            Parallel.ForEach(categorias, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, categoria =>
            {
                mResourceApi.LoadSecondaryResource(categoria);
            });
        }

        /// <summary>
        /// Obtiene las categorías del tesauro de la UNESCO.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pSource">Origen del tesauro</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static List<SecondaryResource> ObtenerDatosUnesco(ReferenceTables pTablas, string pSource)
        {
            List<SecondaryResource> secondaryResources = new List<SecondaryResource>();

            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            List<Concept> listConcepts = new List<Concept>();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == "UNESCO_CODES"))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (item.Code.Length != 6)
                    {
                        throw new Exception();
                    }
                    int level = 3;
                    if (item.Code.EndsWith("00"))
                    {
                        level = 2;
                    }
                    if (item.Code.EndsWith("0000"))
                    {
                        level = 1;
                    }

                    ConceptEDMA concept = new ConceptEDMA();
                    concept.Dc_identifier = item.Code;
                    concept.Dc_source = pSource;
                    concept.Skos_prefLabelMulti = new Dictionary<LanguageEnum, string>();
                    foreach (TableItemNameDetail name in item.Name)
                    {
                        concept.Skos_prefLabelMulti.Add(dicIdiomasMapeados[name.lang], name.Name);
                    }
                    concept.Skos_symbol = level.ToString();
                    listConcepts.Add(concept);
                }
            }

            foreach (Concept concept in listConcepts)
            {
                concept.Skos_narrower = new List<Concept>();
                concept.Skos_broader = new List<Concept>();
                if (concept.Dc_identifier.EndsWith("0000"))
                {
                    concept.Skos_narrower = listConcepts.Where(x => x.Dc_identifier.StartsWith(concept.Dc_identifier.Substring(0, 2)) && x.Dc_identifier.EndsWith("00") && x.Dc_identifier != concept.Dc_identifier).ToList();
                }
                else if (concept.Dc_identifier.EndsWith("00"))
                {
                    concept.Skos_narrower = listConcepts.Where(x => x.Dc_identifier.StartsWith(concept.Dc_identifier.Substring(0, 4)) && x.Dc_identifier != concept.Dc_identifier).ToList();
                    concept.Skos_broader = listConcepts.Where(x => x.Dc_identifier.EndsWith("0000") && x.Dc_identifier.StartsWith(concept.Dc_identifier.Substring(0, 2))).ToList();
                    if (concept.Skos_broader.Count != 1)
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    concept.Skos_broader = listConcepts.Where(x => x.Dc_identifier.StartsWith(concept.Dc_identifier.Substring(0, 4)) && x.Dc_identifier.EndsWith("00") && x.Dc_identifier != concept.Dc_identifier).ToList();
                    if (concept.Skos_broader.Count != 1)
                    {
                        throw new Exception();
                    }
                }
                secondaryResources.Add(((ConceptEDMA)concept).ToGnossApiResource(mResourceApi, concept.Dc_identifier));
            }


            CollectionEDMA collection = new CollectionEDMA();
            collection.Dc_source = pSource;
            collection.Skos_member = listConcepts.Where(x => x.Dc_identifier.EndsWith("0000")).ToList();
            collection.Skos_scopeNote = "Tesauro UNESCO";
            secondaryResources.Add(collection.ToGnossApiResource(mResourceApi, "0"));

            return secondaryResources;
        }

        /// <summary>
        /// Carga el tesauro de CVN
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarTesauroCVN(Thesaurus pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Collection", "taxonomy", "tesauro_cvn");
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Concept", "taxonomy", "tesauro_cvn");

            //Obtención de los objetos a cargar.
            List<SecondaryResource> categorias = ObtenerDatosTesauroCVN(pTablas, "tesauro_cvn");

            //Carga.
            Parallel.ForEach(categorias, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, categoria =>
            {
                mResourceApi.LoadSecondaryResource(categoria);
            });
        }
        
        /// <summary>
        /// Obtiene las categorías del tesauro de CVN
        /// </summary>
        /// <param name="pTablas"></param>
        /// <param name="pSource"></param>
        /// <returns></returns>
        private static List<SecondaryResource> ObtenerDatosTesauroCVN(Thesaurus pTablas, string pSource)
        {
            List<SecondaryResource> secondaryResources = new List<SecondaryResource>();

            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            List<Concept> listConcepts = new List<Concept>();

            foreach (item item in pTablas.item)
            {

                ConceptEDMA concept = new ConceptEDMA();
                concept.Dc_identifier = item.itemId;
                concept.Dc_source = pSource;
                concept.Skos_prefLabelMulti = new Dictionary<LanguageEnum, string>();
                concept.Skos_broader = new List<Concept>();
                concept.Skos_narrower = new List<Concept>();
                foreach (string idioma in dicIdiomasMapeados.Keys)
                {
                    switch (idioma)
                    {
                        case "spa":
                            concept.Skos_prefLabelMulti.Add(dicIdiomasMapeados[idioma], item.itemDescription.First(x => x.NameDetail.lang == "spa").NameDetail.Name);
                            break;
                        case "eng":
                            concept.Skos_prefLabelMulti.Add(dicIdiomasMapeados[idioma], item.itemDescription.First(x => x.NameDetail.lang == "eng").NameDetail.Name);
                            break;
                        default:
                            concept.Skos_prefLabelMulti.Add(dicIdiomasMapeados[idioma], item.itemDescription.First(x => x.NameDetail.lang == "spa").NameDetail.Name);
                            break;
                    }
                }
                listConcepts.Add(concept);
            }

            //Broaders
            foreach (Concept concept in listConcepts)
            {
                item item = pTablas.item.First(x => x.itemId == concept.Dc_identifier);
                Concept conceptBroader = listConcepts.FirstOrDefault(x => x.Dc_identifier == item.itemAncestorId);
                if (conceptBroader != null)
                {
                    concept.Skos_broader.Add(conceptBroader);
                }
            }

            //Narrowers
            foreach (Concept concept in listConcepts)
            {
                List<Concept> conceptsNarrower = listConcepts.Where(x => x.Skos_broader.FirstOrDefault()?.Dc_identifier == concept.Dc_identifier).ToList();
                if (conceptsNarrower != null)
                {
                    concept.Skos_narrower = conceptsNarrower;
                }
            }

            //Levels
            foreach (Concept concept in listConcepts)
            {
                int level = 0;
                Concept conceptBroader = concept;
                while (conceptBroader != null)
                {
                    conceptBroader = listConcepts.FirstOrDefault(x => x.Dc_identifier == conceptBroader.Skos_broader.FirstOrDefault()?.Dc_identifier);
                    level++;
                }
                concept.Skos_symbol = level.ToString();
            }

            foreach (Concept concept in listConcepts)
            {
                secondaryResources.Add(((ConceptEDMA)concept).ToGnossApiResource(mResourceApi, concept.Dc_identifier));
            }



            CollectionEDMA collection = new CollectionEDMA();
            collection.Dc_source = pSource;
            collection.Skos_member = listConcepts.Where(x => x.Skos_broader.Count == 0).ToList();
            collection.Skos_scopeNote = "Tesauro CVN";
            secondaryResources.Add(collection.ToGnossApiResource(mResourceApi, "0"));

            return secondaryResources;
        }

        /// <summary>
        /// Carga la entidad secundaria ScientificActivityDocument.
        /// </summary>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarScientificActivityDocument(string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ScientificActivityDocument", pOntology);

            //Obtención de los objetos a cargar.
            List<ScientificActivityDocument> documentoCientifico = new List<ScientificActivityDocument>();
            documentoCientifico = ObtenerDatosScientificActivityDocument(documentoCientifico);

            //Carga.
            Parallel.ForEach(documentoCientifico, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, scientificDocument =>
            {
                mResourceApi.LoadSecondaryResource(scientificDocument.ToGnossApiResource(mResourceApi, pOntology + "_" + scientificDocument.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ScientificActivityDocument a cargar.
        /// </summary>
        /// <param name="pListaDatosScientificActivityDocument">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ScientificActivityDocument> ObtenerDatosScientificActivityDocument(List<ScientificActivityDocument> pListaDatosScientificActivityDocument)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            //Diccionario con datos.
            Dictionary<string, string> dicDatos = new Dictionary<string, string>();
            dicDatos.Add("SAD1~Publicaciones", "Publicaciones, documentos científicos y técnicos");
            dicDatos.Add("SAD2~En congresos", "Trabajos presentados en congresos nacionales o internacionales");
            dicDatos.Add("SAD3~En jornadas, seminarios…", "Trabajos presentados en jornadas, seminarios, talleres de trabajo y/o cursos nacionales o internacionales");

            foreach (KeyValuePair<string, string> itemData in dicDatos)
            {
                ScientificActivityDocument scientificDocument = new ScientificActivityDocument();
                Dictionary<LanguageEnum, string> dicIdiomaTitulos = new Dictionary<LanguageEnum, string>();
                Dictionary<LanguageEnum, string> dicIdiomaDescripciones = new Dictionary<LanguageEnum, string>();

                //Titulo.
                foreach (KeyValuePair<string, LanguageEnum> item in dicIdiomasMapeados)
                {
                    LanguageEnum idioma = dicIdiomasMapeados[item.Key];
                    dicIdiomaTitulos.Add(idioma, itemData.Key.Split("~")[1]);
                }

                //Descripción.
                foreach (KeyValuePair<string, LanguageEnum> item in dicIdiomasMapeados)
                {
                    LanguageEnum idioma = dicIdiomasMapeados[item.Key];
                    dicIdiomaDescripciones.Add(idioma, itemData.Value);
                }

                //Se agrega las propiedades.
                scientificDocument.Dc_identifier = itemData.Key.Split("~")[0];
                scientificDocument.Dc_title = dicIdiomaTitulos;
                scientificDocument.Bibo_abstract = dicIdiomaDescripciones;

                //Se guarda el objeto a la lista.
                pListaDatosScientificActivityDocument.Add(scientificDocument);
            }

            return pListaDatosScientificActivityDocument;
        }

        /// <summary>
        /// Carga la entidad secundaria ScientificExperienceProject.
        /// </summary>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarScientificExperienceProject(string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ScientificExperienceProject", pOntology);

            //Obtención de los objetos a cargar.
            List<ScientificExperienceProject> proyectoCientifico = new List<ScientificExperienceProject>();
            proyectoCientifico = ObtenerDatosScientificExperienceProject(proyectoCientifico);

            //Carga.
            Parallel.ForEach(proyectoCientifico, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, scientificProject =>
            {
                mResourceApi.LoadSecondaryResource(scientificProject.ToGnossApiResource(mResourceApi, pOntology + "_" + scientificProject.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ScientificExperienceProject a cargar.
        /// </summary>
        /// <param name="pListaDatosScientificActivityDocument">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ScientificExperienceProject> ObtenerDatosScientificExperienceProject(List<ScientificExperienceProject> pListaDatosScientificExperienceProject)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            //Diccionario con datos.
            Dictionary<string, string> dicDatos = new Dictionary<string, string>();
            dicDatos.Add("SEP1~Competitivos", "Proyectos de I+D+i financiados en convocatorias competitivas de Administraciones o entidades públicas y privadas");
            dicDatos.Add("SEP2~No competitivos", "Contratos, convenios o proyectos de I+D+i no competitivos con Administraciones o entidades públicas o privadas");

            foreach (KeyValuePair<string, string> itemData in dicDatos)
            {
                ScientificExperienceProject scientificProject = new ScientificExperienceProject();
                Dictionary<LanguageEnum, string> dicIdiomaTitulos = new Dictionary<LanguageEnum, string>();
                Dictionary<LanguageEnum, string> dicIdiomaDescripciones = new Dictionary<LanguageEnum, string>();

                //Titulo.
                foreach (KeyValuePair<string, LanguageEnum> item in dicIdiomasMapeados)
                {
                    LanguageEnum idioma = dicIdiomasMapeados[item.Key];
                    dicIdiomaTitulos.Add(idioma, itemData.Key.Split("~")[1]);
                }

                //Descripción.
                foreach (KeyValuePair<string, LanguageEnum> item in dicIdiomasMapeados)
                {
                    LanguageEnum idioma = dicIdiomasMapeados[item.Key];
                    dicIdiomaDescripciones.Add(idioma, itemData.Value);
                }

                //Se agrega las propiedades.
                scientificProject.Dc_identifier = itemData.Key.Split("~")[0];
                scientificProject.Dc_title = dicIdiomaTitulos;
                scientificProject.Bibo_abstract = dicIdiomaDescripciones;

                //Se guarda el objeto a la lista.
                pListaDatosScientificExperienceProject.Add(scientificProject);
            }

            return pListaDatosScientificExperienceProject;
        }

        /// <summary>
        /// Carga la entidad secundaria HIndexSource.
        /// </summary>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarHIndexSource(string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/HIndexSource", pOntology);

            //Obtención de los objetos a cargar.
            List<HIndexSource> hindexsources = ObtenerDatosHIndexSource();

            //Carga.
            Parallel.ForEach(hindexsources, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, hindexsource =>
            {
                var x = mResourceApi.LoadSecondaryResource(hindexsource.ToGnossApiResource(mResourceApi, pOntology + "_" + hindexsource.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos HIndexSource a cargar.
        /// </summary>
        /// <param name="pListaDatosHIndexSource">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<HIndexSource> ObtenerDatosHIndexSource()
        {
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            //Lista con datos.
            List<HIndexSource> hIndexSources = new List<HIndexSource>();

            //Otros
            {
                HIndexSource hindexsource = new HIndexSource()
                {
                    Dc_identifier = "OTHERS",
                    Dc_title = new Dictionary<LanguageEnum, string>()
                };
                foreach (LanguageEnum lang in dicIdiomasMapeados.Values)
                {
                    string texto = "";
                    switch (lang)
                    {
                        case LanguageEnum.ca:
                            texto = "Altres";
                            break;
                        case LanguageEnum.en:
                            texto = "Others";
                            break;
                        case LanguageEnum.es:
                            texto = "Otros";
                            break;
                        case LanguageEnum.eu:
                            texto = "Beste batzuk";
                            break;
                        case LanguageEnum.fr:
                            texto = "Autres";
                            break;
                        case LanguageEnum.gl:
                            texto = "Outros";
                            break;
                        default:
                            texto = "Otros";
                            break;

                    }
                    hindexsource.Dc_title[lang] = texto;
                }
                hIndexSources.Add(hindexsource);
            }

            //GOOGLE SCHOLAR
            {
                HIndexSource hindexsource = new HIndexSource()
                {
                    Dc_identifier = "001",
                    Dc_title = new Dictionary<LanguageEnum, string>()
                };
                foreach (LanguageEnum lang in dicIdiomasMapeados.Values)
                {
                    string texto = "GOOGLE SCHOLAR";
                    hindexsource.Dc_title[lang] = texto;
                }
                hIndexSources.Add(hindexsource);
            }

            //SCOPUS
            {
                HIndexSource hindexsource = new HIndexSource()
                {
                    Dc_identifier = "010",
                    Dc_title = new Dictionary<LanguageEnum, string>()
                };
                foreach (LanguageEnum lang in dicIdiomasMapeados.Values)
                {
                    string texto = "SCOPUS";
                    hindexsource.Dc_title[lang] = texto;
                }
                hIndexSources.Add(hindexsource);
            }

            //WOS
            {
                HIndexSource hindexsource = new HIndexSource()
                {
                    Dc_identifier = "000",
                    Dc_title = new Dictionary<LanguageEnum, string>()
                };
                foreach (LanguageEnum lang in dicIdiomasMapeados.Values)
                {
                    string texto = "WOS";
                    hindexsource.Dc_title[lang] = texto;
                }
                hIndexSources.Add(hindexsource);
            }
            return hIndexSources;
        }

        /// <summary>
        /// Carga la entidad secundaria QualificationType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarQualificationType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/QualificationType", pOntology);

            //Obtención de los objetos a cargar.
            List<QualificationType> qualificationTypes = new List<QualificationType>();
            qualificationTypes = ObtenerDatosQualificationType(pTablas, idQualificationType, qualificationTypes);

            //Carga.
            Parallel.ForEach(qualificationTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, qualificationType =>
            {
                mResourceApi.LoadSecondaryResource(qualificationType.ToGnossApiResource(mResourceApi, pOntology + "_" + qualificationType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos QualificationType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosQualificationType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<QualificationType> ObtenerDatosQualificationType(ReferenceTables pTablas, string pCodigoTabla, List<QualificationType> pListaDatosQualificationType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        QualificationType qualificationType = new QualificationType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        qualificationType.Dc_identifier = identificador;
                        qualificationType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosQualificationType.Add(qualificationType);
                    }
                }
            }

            return pListaDatosQualificationType;
        }

        /// <summary>
        /// Carga la entidad secundaria CargarUniversityDegreeType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarUniversityDegreeType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/UniversityDegreeType", pOntology);

            //Obtención de los objetos a cargar.
            List<UniversityDegreeType> universityDegreeTypes = new List<UniversityDegreeType>();
            universityDegreeTypes = ObtenerDatosUniversityDegreeType(pTablas, idUniversityDegreeType, universityDegreeTypes);

            //Carga.
            Parallel.ForEach(universityDegreeTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, universityDegreeType =>
            {
                mResourceApi.LoadSecondaryResource(universityDegreeType.ToGnossApiResource(mResourceApi, pOntology + "_" + universityDegreeType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos UniversityDegreeType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosUniversityDegreeType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<UniversityDegreeType> ObtenerDatosUniversityDegreeType(ReferenceTables pTablas, string pCodigoTabla, List<UniversityDegreeType> pListaDatosUniversityDegreeType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        UniversityDegreeType universityDegreeType = new UniversityDegreeType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        universityDegreeType.Dc_identifier = identificador;
                        universityDegreeType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosUniversityDegreeType.Add(universityDegreeType);
                    }
                }
            }

            return pListaDatosUniversityDegreeType;
        }
 
        /// <summary>
        /// Carga la entidad secundaria CargarDegreeType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarDegreeType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/DegreeType", pOntology);

            //Obtención de los objetos a cargar.
            List<DegreeType> degreeTypes = new List<DegreeType>();
            degreeTypes = ObtenerDatosDegreeType(pTablas, idDegreeType, degreeTypes);

            //Carga.
            Parallel.ForEach(degreeTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, degreeType =>
            {
                mResourceApi.LoadSecondaryResource(degreeType.ToGnossApiResource(mResourceApi, pOntology + "_" + degreeType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos UniversityDegreeType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosDegreeType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<DegreeType> ObtenerDatosDegreeType(ReferenceTables pTablas, string pCodigoTabla, List<DegreeType> pListaDatosDegreeType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        DegreeType degreeType = new DegreeType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        degreeType.Dc_identifier = identificador;
                        degreeType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosDegreeType.Add(degreeType);
                    }
                }
            }

            return pListaDatosDegreeType;
        }

        /// <summary>
        /// Carga la entidad secundaria CargarFormationType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarFormationType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/FormationType", pOntology);

            //Obtención de los objetos a cargar.
            List<FormationType> formationTypes = new List<FormationType>();
            formationTypes = ObtenerDatosFormationType(pTablas, idFormationType, formationTypes);

            //Carga.
            Parallel.ForEach(formationTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, formationType =>
            {
                mResourceApi.LoadSecondaryResource(formationType.ToGnossApiResource(mResourceApi, pOntology + "_" + formationType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos UniversityFormationType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosDegreeType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<FormationType> ObtenerDatosFormationType(ReferenceTables pTablas, string pCodigoTabla, List<FormationType> pListaDatosDegreeType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        FormationType degreeType = new FormationType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        degreeType.Dc_identifier = identificador;
                        degreeType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosDegreeType.Add(degreeType);
                    }
                }
            }

            return pListaDatosDegreeType;
        }

        /// <summary>
        /// Carga la entidad secundaria CargarPostgradeDegree.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarPostgradeDegree(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/PostgradeDegree", pOntology);

            //Obtención de los objetos a cargar.
            List<PostgradeDegree> postgradeDegrees = new List<PostgradeDegree>();
            postgradeDegrees = ObtenerDatosPostgradeDegree(pTablas, idPostgradeDegree, postgradeDegrees);

            //Carga.
            Parallel.ForEach(postgradeDegrees, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, postgradeDegree =>
            {
                mResourceApi.LoadSecondaryResource(postgradeDegree.ToGnossApiResource(mResourceApi, pOntology + "_" + postgradeDegree.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos UniversityFormationType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosDegreeType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<PostgradeDegree> ObtenerDatosPostgradeDegree(ReferenceTables pTablas, string pCodigoTabla, List<PostgradeDegree> pListaDatosDegreeType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        PostgradeDegree degreeType = new PostgradeDegree();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        degreeType.Dc_identifier = identificador;
                        degreeType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosDegreeType.Add(degreeType);
                    }
                }
            }

            return pListaDatosDegreeType;
        }

        /// <summary>
        /// Carga la entidad secundaria CargarFormationActivityType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarFormationActivityType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/FormationActivityType", pOntology);

            //Obtención de los objetos a cargar.
            List<FormationActivityType> formations = new List<FormationActivityType>();
            formations = ObtenerFormationActivityType(pTablas, idFormationActivityType, formations);

            //Carga.
            Parallel.ForEach(formations, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, formation =>
            {
                mResourceApi.LoadSecondaryResource(formation.ToGnossApiResource(mResourceApi, pOntology + "_" + formation.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos FormationActivityType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosDegreeType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<FormationActivityType> ObtenerFormationActivityType(ReferenceTables pTablas, string pCodigoTabla, List<FormationActivityType> pListaDatosDegreeType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        FormationActivityType degreeType = new FormationActivityType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        degreeType.Dc_identifier = identificador;
                        degreeType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosDegreeType.Add(degreeType);
                    }
                }
            }

            return pListaDatosDegreeType;
        }

        /// <summary>
        /// Carga la entidad secundaria LanguageLevel.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarLanguageLevel(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/LanguageLevel", pOntology);

            //Obtención de los objetos a cargar.
            List<LanguageLevel> languages = new List<LanguageLevel>();
            languages = ObtenerLanguageLevel(pTablas, idLanguageLevel, languages);

            //Carga.
            Parallel.ForEach(languages, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, language =>
            {
                mResourceApi.LoadSecondaryResource(language.ToGnossApiResource(mResourceApi, pOntology + "_" + language.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos LanguageLevel a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosLanguageLevel">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<LanguageLevel> ObtenerLanguageLevel(ReferenceTables pTablas, string pCodigoTabla, List<LanguageLevel> pListaDatosLanguageLevel)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        LanguageLevel languageLevel = new LanguageLevel();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        languageLevel.Dc_identifier = identificador;
                        languageLevel.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosLanguageLevel.Add(languageLevel);
                    }
                }
            }

            return pListaDatosLanguageLevel;
        }

        /// <summary>
        /// Carga la entidad secundaria TutorshipsProgramType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarTutorshipsProgramType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/TutorshipsProgramType", pOntology);

            //Obtención de los objetos a cargar.
            List<TutorshipsProgramType> tutorshipsProgramTypes = new List<TutorshipsProgramType>();
            tutorshipsProgramTypes = ObtenerTutorshipsProgramType(pTablas, idTutorshipsProgramType, tutorshipsProgramTypes);

            //Carga.
            Parallel.ForEach(tutorshipsProgramTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, tutorshipsProgramType =>
            {
                mResourceApi.LoadSecondaryResource(tutorshipsProgramType.ToGnossApiResource(mResourceApi, pOntology + "_" + tutorshipsProgramType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos TutorshipsProgramType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosTutorshipsProgramType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<TutorshipsProgramType> ObtenerTutorshipsProgramType(ReferenceTables pTablas, string pCodigoTabla, List<TutorshipsProgramType> pListaDatosTutorshipsProgramType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        TutorshipsProgramType tutorshipsProgramType = new TutorshipsProgramType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        tutorshipsProgramType.Dc_identifier = identificador;
                        tutorshipsProgramType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosTutorshipsProgramType.Add(tutorshipsProgramType);
                    }
                }
            }

            return pListaDatosTutorshipsProgramType;
        }

        /// <summary>
        /// Carga la entidad secundaria ProjectCharacterType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarProjectCharacterType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ProjectCharacterType", pOntology);

            //Obtención de los objetos a cargar.
            List<ProjectCharacterType> projectCharacterTypes = new List<ProjectCharacterType>();
            projectCharacterTypes = ObtenerProjectCharacterType(pTablas, idProjectCharacterType, projectCharacterTypes);

            //Carga.
            Parallel.ForEach(projectCharacterTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, projectCharacterType =>
            {
                mResourceApi.LoadSecondaryResource(projectCharacterType.ToGnossApiResource(mResourceApi, pOntology + "_" + projectCharacterType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ProjectCharacterType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosProjectCharacterType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ProjectCharacterType> ObtenerProjectCharacterType(ReferenceTables pTablas, string pCodigoTabla, List<ProjectCharacterType> pListaDatosProjectCharacterType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ProjectCharacterType projectCharacterType = new ProjectCharacterType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        projectCharacterType.Dc_identifier = identificador;
                        projectCharacterType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosProjectCharacterType.Add(projectCharacterType);
                    }
                }
            }

            return pListaDatosProjectCharacterType;
        }

        /// <summary>
        /// Carga la entidad secundaria TeachingType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarTeachingType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/TeachingType", pOntology);

            //Obtención de los objetos a cargar.
            List<TeachingType> teachingTypes = new List<TeachingType>();
            teachingTypes = ObtenerTeachingType(pTablas, idTeachingType, teachingTypes);

            //Carga.
            Parallel.ForEach(teachingTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, teachingType =>
            {
                mResourceApi.LoadSecondaryResource(teachingType.ToGnossApiResource(mResourceApi, pOntology + "_" + teachingType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos TeachingType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosTeachingType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<TeachingType> ObtenerTeachingType(ReferenceTables pTablas, string pCodigoTabla, List<TeachingType> pListaDatosTeachingType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        TeachingType teachingType = new TeachingType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        teachingType.Dc_identifier = identificador;
                        teachingType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosTeachingType.Add(teachingType);
                    }
                }
            }

            return pListaDatosTeachingType;
        }

        /// <summary>
        /// Carga la entidad secundaria ModalityTeachingType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarModalityTeachingType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ModalityTeachingType", pOntology);

            //Obtención de los objetos a cargar.
            List<ModalityTeachingType> modalityteachingTypes = new List<ModalityTeachingType>();
            modalityteachingTypes = ObtenerModalityTeachingType(pTablas, idModalityTeachingType, modalityteachingTypes);

            //Carga.
            Parallel.ForEach(modalityteachingTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, modalityTeachingType =>
            {
                mResourceApi.LoadSecondaryResource(modalityTeachingType.ToGnossApiResource(mResourceApi, pOntology + "_" + modalityTeachingType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ModalityTeachingType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosModalityTeachingType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ModalityTeachingType> ObtenerModalityTeachingType(ReferenceTables pTablas, string pCodigoTabla, List<ModalityTeachingType> pListaDatosModalityTeachingType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ModalityTeachingType modalityTeachingType = new ModalityTeachingType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        modalityTeachingType.Dc_identifier = identificador;
                        modalityTeachingType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosModalityTeachingType.Add(modalityTeachingType);
                    }
                }
            }

            return pListaDatosModalityTeachingType;
        }

        /// <summary>
        /// Carga la entidad secundaria ProgramType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarProgramType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ProgramType", pOntology);

            //Obtención de los objetos a cargar.
            List<ProgramType> ProgramTypes = new List<ProgramType>();
            ProgramTypes = ObtenerProgramType(pTablas, idProgramType, ProgramTypes);

            //Carga.
            Parallel.ForEach(ProgramTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, ProgramType =>
            {
                mResourceApi.LoadSecondaryResource(ProgramType.ToGnossApiResource(mResourceApi, pOntology + "_" + ProgramType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ProgramType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosProgramType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ProgramType> ObtenerProgramType(ReferenceTables pTablas, string pCodigoTabla, List<ProgramType> pListaDatosProgramType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ProgramType ProgramType = new ProgramType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        ProgramType.Dc_identifier = identificador;
                        ProgramType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosProgramType.Add(ProgramType);
                    }
                }
            }

            return pListaDatosProgramType;
        }

        /// <summary>
        /// Carga la entidad secundaria CourseType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarCourseType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/CourseType", pOntology);

            //Obtención de los objetos a cargar.
            List<CourseType> CourseTypes = new List<CourseType>();
            CourseTypes = ObtenerCourseType(pTablas, idCourseType, CourseTypes);

            //Carga.
            Parallel.ForEach(CourseTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, CourseType =>
            {
                mResourceApi.LoadSecondaryResource(CourseType.ToGnossApiResource(mResourceApi, pOntology + "_" + CourseType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos CourseType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosCourseType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<CourseType> ObtenerCourseType(ReferenceTables pTablas, string pCodigoTabla, List<CourseType> pListaDatosCourseType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        CourseType CourseType = new CourseType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        CourseType.Dc_identifier = identificador;
                        CourseType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosCourseType.Add(CourseType);
                    }
                }
            }

            return pListaDatosCourseType;
        }

        /// <summary>
        /// Carga la entidad secundaria HoursCreditsECTSType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarHoursCreditsECTSType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/HoursCreditsECTSType", pOntology);

            //Obtención de los objetos a cargar.
            List<HoursCreditsECTSType> HoursCreditsECTSTypes = new List<HoursCreditsECTSType>();
            HoursCreditsECTSTypes = ObtenerHoursCreditsECTSType(pTablas, idHoursCreditsECTSType, HoursCreditsECTSTypes);

            //Carga.
            Parallel.ForEach(HoursCreditsECTSTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, HoursCreditsECTSType =>
            {
                mResourceApi.LoadSecondaryResource(HoursCreditsECTSType.ToGnossApiResource(mResourceApi, pOntology + "_" + HoursCreditsECTSType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos HoursCreditsECTSType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosHoursCreditsECTSType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<HoursCreditsECTSType> ObtenerHoursCreditsECTSType(ReferenceTables pTablas, string pCodigoTabla, List<HoursCreditsECTSType> pListaDatosHoursCreditsECTSType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        HoursCreditsECTSType HoursCreditsECTSType = new HoursCreditsECTSType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        HoursCreditsECTSType.Dc_identifier = identificador;
                        HoursCreditsECTSType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosHoursCreditsECTSType.Add(HoursCreditsECTSType);
                    }
                }
            }

            return pListaDatosHoursCreditsECTSType;
        }

        /// <summary>
        /// Carga la entidad secundaria EvaluationType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarEvaluationType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/EvaluationType", pOntology);

            //Obtención de los objetos a cargar.
            List<EvaluationType> EvaluationTypes = new List<EvaluationType>();
            EvaluationTypes = ObtenerEvaluationType(pTablas, idEvaluationType, EvaluationTypes);

            //Carga.
            Parallel.ForEach(EvaluationTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, EvaluationType =>
            {
                mResourceApi.LoadSecondaryResource(EvaluationType.ToGnossApiResource(mResourceApi, pOntology + "_" + EvaluationType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos EvaluationType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosEvaluationType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<EvaluationType> ObtenerEvaluationType(ReferenceTables pTablas, string pCodigoTabla, List<EvaluationType> pListaDatosEvaluationType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        EvaluationType EvaluationType = new EvaluationType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        EvaluationType.Dc_identifier = identificador;
                        EvaluationType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosEvaluationType.Add(EvaluationType);
                    }
                }
            }

            return pListaDatosEvaluationType;
        }

        /// <summary>
        /// Carga la entidad secundaria CallType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarCallType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/CallType", pOntology);

            //Obtención de los objetos a cargar.
            List<CallType> CallTypes = new List<CallType>();
            CallTypes = ObtenerCallType(pTablas, idCallType, CallTypes);

            //Carga.
            Parallel.ForEach(CallTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, CallType =>
            {
                mResourceApi.LoadSecondaryResource(CallType.ToGnossApiResource(mResourceApi, pOntology + "_" + CallType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos CallType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosCallType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<CallType> ObtenerCallType(ReferenceTables pTablas, string pCodigoTabla, List<CallType> pListaDatosCallType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        CallType CallType = new CallType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        CallType.Dc_identifier = identificador;
                        CallType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosCallType.Add(CallType);
                    }
                }
            }

            return pListaDatosCallType;
        }

        /// <summary>
        /// Carga la entidad secundaria SupportType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarSupportType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/SupportType", pOntology);

            //Obtención de los objetos a cargar.
            List<SupportType> SupportTypes = new List<SupportType>();
            SupportTypes = ObtenerSupportType(pTablas, idSupportType, SupportTypes);

            //Carga.
            Parallel.ForEach(SupportTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, SupportType =>
            {
                mResourceApi.LoadSecondaryResource(SupportType.ToGnossApiResource(mResourceApi, pOntology + "_" + SupportType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos SupportType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosSupportType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<SupportType> ObtenerSupportType(ReferenceTables pTablas, string pCodigoTabla, List<SupportType> pListaDatosSupportType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        SupportType SupportType = new SupportType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        SupportType.Dc_identifier = identificador;
                        SupportType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosSupportType.Add(SupportType);
                    }
                }
            }

            return pListaDatosSupportType;
        }

        /// <summary>
        /// Carga la entidad secundaria EventGeographicRegion.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarEventGeographicRegion(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/EventGeographicRegion", pOntology);

            //Obtención de los objetos a cargar.
            List<EventGeographicRegion> EventGeographicRegions = new List<EventGeographicRegion>();
            EventGeographicRegions = ObtenerEventGeographicRegion(pTablas, idEventGeographicRegion, EventGeographicRegions);

            //Carga.
            Parallel.ForEach(EventGeographicRegions, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, EventGeographicRegion =>
            {
                mResourceApi.LoadSecondaryResource(EventGeographicRegion.ToGnossApiResource(mResourceApi, pOntology + "_" + EventGeographicRegion.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos EventGeographicRegion a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosEventGeographicRegion">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<EventGeographicRegion> ObtenerEventGeographicRegion(ReferenceTables pTablas, string pCodigoTabla, List<EventGeographicRegion> pListaDatosEventGeographicRegion)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        EventGeographicRegion EventGeographicRegion = new EventGeographicRegion();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        EventGeographicRegion.Dc_identifier = identificador;
                        EventGeographicRegion.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosEventGeographicRegion.Add(EventGeographicRegion);
                    }
                }
            }

            return pListaDatosEventGeographicRegion;
        }

        /// <summary>
        /// Carga la entidad secundaria ResultType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarResultType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/ResultType", pOntology);

            //Obtención de los objetos a cargar.
            List<ResultType> ResultTypes = new List<ResultType>();
            ResultTypes = ObtenerResultType(pTablas, idResultType, ResultTypes);

            //Carga.
            Parallel.ForEach(ResultTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, ResultType =>
            {
                mResourceApi.LoadSecondaryResource(ResultType.ToGnossApiResource(mResourceApi, pOntology + "_" + ResultType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos ResultType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosResultType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<ResultType> ObtenerResultType(ReferenceTables pTablas, string pCodigoTabla, List<ResultType> pListaDatosResultType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        ResultType ResultType = new ResultType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        ResultType.Dc_identifier = identificador;
                        ResultType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosResultType.Add(ResultType);
                    }
                }
            }

            return pListaDatosResultType;
        }

        /// <summary>
        /// Carga la entidad secundaria LaboralDurationType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarLaboralDurationType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/LaboralDurationType", pOntology);

            //Obtención de los objetos a cargar.
            List<LaboralDurationType> LaboralDurationTypes = new List<LaboralDurationType>();
            LaboralDurationTypes = ObtenerLaboralDurationType(pTablas, idLaboralDurationType, LaboralDurationTypes);

            //Carga.
            Parallel.ForEach(LaboralDurationTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, LaboralDurationType =>
            {
                mResourceApi.LoadSecondaryResource(LaboralDurationType.ToGnossApiResource(mResourceApi, pOntology + "_" + LaboralDurationType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos LaboralDurationType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosLaboralDurationType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<LaboralDurationType> ObtenerLaboralDurationType(ReferenceTables pTablas, string pCodigoTabla, List<LaboralDurationType> pListaDatosLaboralDurationType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        LaboralDurationType LaboralDurationType = new LaboralDurationType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        LaboralDurationType.Dc_identifier = identificador;
                        LaboralDurationType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosLaboralDurationType.Add(LaboralDurationType);
                    }
                }
            }

            return pListaDatosLaboralDurationType;
        }

        /// <summary>
        /// Carga la entidad secundaria SeminarInscriptionType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarSeminarInscriptionType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/SeminarInscriptionType", pOntology);

            //Obtención de los objetos a cargar.
            List<SeminarInscriptionType> SeminarInscriptionTypes = new List<SeminarInscriptionType>();
            SeminarInscriptionTypes = ObtenerSeminarInscriptionType(pTablas, idSeminarInscriptionType, SeminarInscriptionTypes);

            //Carga.
            Parallel.ForEach(SeminarInscriptionTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, SeminarInscriptionType =>
            {
                mResourceApi.LoadSecondaryResource(SeminarInscriptionType.ToGnossApiResource(mResourceApi, pOntology + "_" + SeminarInscriptionType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos SeminarInscriptionType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosSeminarInscriptionType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<SeminarInscriptionType> ObtenerSeminarInscriptionType(ReferenceTables pTablas, string pCodigoTabla, List<SeminarInscriptionType> pListaDatosSeminarInscriptionType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        SeminarInscriptionType SeminarInscriptionType = new SeminarInscriptionType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        SeminarInscriptionType.Dc_identifier = identificador;
                        SeminarInscriptionType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosSeminarInscriptionType.Add(SeminarInscriptionType);
                    }
                }
            }

            return pListaDatosSeminarInscriptionType;
        }

        /// <summary>
        /// Carga la entidad secundaria SeminarEventType.
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarSeminarEventType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/SeminarEventType", pOntology);

            //Obtención de los objetos a cargar.
            List<SeminarEventType> SeminarEventTypes = new List<SeminarEventType>();
            SeminarEventTypes = ObtenerSeminarEventType(pTablas, idSeminarEventType, SeminarEventTypes);

            //Carga.
            Parallel.ForEach(SeminarEventTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, SeminarEventType =>
            {
                mResourceApi.LoadSecondaryResource(SeminarEventType.ToGnossApiResource(mResourceApi, pOntology + "_" + SeminarEventType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos SeminarEventType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosSeminarEventType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<SeminarEventType> ObtenerSeminarEventType(ReferenceTables pTablas, string pCodigoTabla, List<SeminarEventType> pListaDatosSeminarEventType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        SeminarEventType SeminarEventType = new SeminarEventType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        SeminarEventType.Dc_identifier = identificador;
                        SeminarEventType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosSeminarEventType.Add(SeminarEventType);
                    }
                }
            }

            return pListaDatosSeminarEventType;
        }

        /// <summary>
        /// Carga la entidad secundaria CargarDoctoralProgramType
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarDoctoralProgramType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/DoctoralProgramType", pOntology);

            //Obtención de los objetos a cargar.
            List<DoctoralProgramType> doctoralProgramTypes = new List<DoctoralProgramType>();
            doctoralProgramTypes = ObtenerDatosDoctoralProgramType(pTablas, idDoctoralProgramType, doctoralProgramTypes);

            //Carga.
            Parallel.ForEach(doctoralProgramTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, doctoralProgramType =>
            {
                mResourceApi.LoadSecondaryResource(doctoralProgramType.ToGnossApiResource(mResourceApi, pOntology + "_" + doctoralProgramType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos DoctoralProgramType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosDoctoralProgramType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<DoctoralProgramType> ObtenerDatosDoctoralProgramType(ReferenceTables pTablas, string pCodigoTabla, List<DoctoralProgramType> pListaDatosDoctoralProgramType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        DoctoralProgramType doctoralProgramType = new DoctoralProgramType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        doctoralProgramType.Dc_identifier = identificador;
                        doctoralProgramType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosDoctoralProgramType.Add(doctoralProgramType);
                    }
                }
            }

            return pListaDatosDoctoralProgramType;
        }

        /// <summary>
        /// Carga la entidad secundaria PrizeType
        /// </summary>
        /// <param name="pTablas">Tablas con los datos a obtener.</param>
        /// <param name="pOntology">Ontología.</param>
        private static void CargarPrizeType(ReferenceTables pTablas, string pOntology)
        {
            //Cambio de ontología.
            mResourceApi.ChangeOntoly(pOntology);

            //Elimina los datos cargados antes de volverlos a cargar.
            EliminarDatosCargados("http://w3id.org/roh/PrizeType", pOntology);

            //Obtención de los objetos a cargar.
            List<PrizeType> prizeTypes = new List<PrizeType>();
            prizeTypes = ObtenerDatosPrizeType(pTablas, idPrizeType, prizeTypes);

            //Carga.
            Parallel.ForEach(prizeTypes, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, prizeType =>
            {
                mResourceApi.LoadSecondaryResource(prizeType.ToGnossApiResource(mResourceApi, pOntology + "_" + prizeType.Dc_identifier));
            });
        }

        /// <summary>
        /// Obtiene los objetos DoctoralProgramType a cargar.
        /// </summary>
        /// <param name="pTablas">Objetos con los datos a obtener.</param>
        /// <param name="pCodigoTabla">ID de la tabla a consultar.</param>
        /// <param name="pListaDatosPrizeType">Lista dónde guardar los objetos.</param>
        /// <returns>Lista con los objetos creados.</returns>
        private static List<PrizeType> ObtenerDatosPrizeType(ReferenceTables pTablas, string pCodigoTabla, List<PrizeType> pListaDatosPrizeType)
        {
            //Mapea los idiomas.
            Dictionary<string, LanguageEnum> dicIdiomasMapeados = MapearLenguajes();

            foreach (Table tabla in pTablas.Table.Where(x => x.name == pCodigoTabla))
            {
                foreach (TableItem item in tabla.Item)
                {
                    if (string.IsNullOrEmpty(item.Delegate))
                    {
                        PrizeType prizeType = new PrizeType();
                        Dictionary<LanguageEnum, string> dicIdioma = new Dictionary<LanguageEnum, string>();
                        string identificador = item.Code;
                        foreach (TableItemNameDetail modalidad in item.Name)
                        {
                            LanguageEnum idioma = dicIdiomasMapeados[modalidad.lang];
                            string nombre = modalidad.Name;
                            dicIdioma.Add(idioma, nombre);
                        }

                        //Se agrega las propiedades.
                        prizeType.Dc_identifier = identificador;
                        prizeType.Dc_title = dicIdioma;

                        //Se guarda el objeto a la lista.
                        pListaDatosPrizeType.Add(prizeType);
                    }
                }
            }

            return pListaDatosPrizeType;
        }

        /// <summary>
        /// Carga el tesauro del area de procedencia
        /// </summary>
        private static void CargarTesauroAreaProcedencia()
        {
            string ontology = "taxonomy";

            //Cambio de ontología.
            mResourceApi.ChangeOntoly(ontology);
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Collection", "taxonomy", "areaprocedencia");
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Concept", "taxonomy", "areaprocedencia");

            //Obtención de los objetos a cargar.
            List<SecondaryResource> categorias = new List<SecondaryResource>();
            ObtenerTesauroExcelAreaProcedencia(ref categorias, "areaprocedencia");

            //Carga.
            Parallel.ForEach(categorias, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, categoria =>
            {
                mResourceApi.LoadSecondaryResource(categoria);
            });
        }

        /// <summary>
        /// Proceso para contruir el tesáuro del área de procedencia
        /// </summary>
        /// <param name="pListaRecursosCargar">Lista de recursos a cargar.</param>
        /// <param name="pSource">Nombre del tesáuro.</param>
        private static void ObtenerTesauroExcelAreaProcedencia(ref List<SecondaryResource> pListaRecursosCargar, string pSource)
        {
            DataSet ds = LeerDatosExcel($"Dataset{Path.DirectorySeparatorChar}AreaProcedencia.xlsx");

            //Recorremos las filas
            List<Concept> listConcepts = new List<Concept>();
            foreach (DataRow fila in ds.Tables["Hoja1"].Rows)
            {
                string id = fila["ID"].ToString();
                string nombre = fila["Nombre"].ToString();

                ConceptEDMA concept = new ConceptEDMA();
                concept.Skos_prefLabel = nombre;
                concept.Dc_identifier = id;
                concept.Dc_source = pSource;
                listConcepts.Add(concept);
            }

            foreach (Concept concept in listConcepts)
            {
                string[] idSplit = concept.Dc_identifier.Split('.');
                concept.Skos_narrower = new List<Concept>();
                concept.Skos_broader = new List<Concept>();
                if (concept.Dc_identifier.EndsWith(".0"))
                {
                    concept.Skos_narrower = listConcepts.Where(x => x.Dc_identifier.StartsWith(idSplit[0] + ".") && x.Dc_identifier != concept.Dc_identifier).ToList();
                    concept.Skos_symbol = "1";
                }
                else
                {
                    concept.Skos_broader = listConcepts.Where(x => x.Dc_identifier.StartsWith(idSplit[0] + '.') && x.Dc_identifier.EndsWith(".0") && x.Dc_identifier != concept.Dc_identifier).ToList();
                    concept.Skos_symbol = "2";
                }
                pListaRecursosCargar.Add(((ConceptEDMA)concept).ToGnossApiResource(mResourceApi, concept.Dc_identifier));
            }

            CollectionEDMA collection = new CollectionEDMA();
            collection.Dc_source = pSource;
            collection.Skos_member = listConcepts.Where(x => x.Dc_identifier.EndsWith(".0")).ToList();
            collection.Skos_scopeNote = "Area procedencia";
            pListaRecursosCargar.Add(collection.ToGnossApiResource(mResourceApi, "0"));
        }

        /// <summary>
        /// Carga el tesauro de sector de aplicacion
        /// </summary>
        private static void CargarTesauroSectorAplicacion()
        {
            string ontology = "taxonomy";

            //Cambio de ontología.
            mResourceApi.ChangeOntoly(ontology);
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Collection", "taxonomy", "sectoraplicacion");
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Concept", "taxonomy", "sectoraplicacion");

            //Obtención de los objetos a cargar.
            List<SecondaryResource> categorias = new List<SecondaryResource>();
            ObtenerTesauroExcelSectorAplicacion(ref categorias, "sectoraplicacion");

            //Carga.
            Parallel.ForEach(categorias, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, categoria =>
            {
                mResourceApi.LoadSecondaryResource(categoria);
            });
        }

        /// <summary>
        /// Proceso para contruir el tesáuro del sector aplicación
        /// </summary>
        /// <param name="pListaRecursosCargar">Lista de recursos a cargar.</param>
        /// <param name="pSource">Nombre del tesáuro.</param>
        private static void ObtenerTesauroExcelSectorAplicacion(ref List<SecondaryResource> pListaRecursosCargar, string pSource)
        {
            DataSet ds = LeerDatosExcel($"Dataset{Path.DirectorySeparatorChar}SectorAplicacion.xlsx");

            //Recorremos las filas
            List<Concept> listConcepts = new List<Concept>();
            foreach (DataRow fila in ds.Tables["Hoja1"].Rows)
            {
                string id = fila["ID"].ToString();
                string nombre = fila["Nombre"].ToString();
                string traduccion = fila["Traducido"].ToString();

                ConceptEDMA concept = new ConceptEDMA();
                Dictionary<GnossBase.GnossOCBase.LanguageEnum, string> dicNombresTraduccion = new Dictionary<GnossBase.GnossOCBase.LanguageEnum, string>();
                dicNombresTraduccion.Add(GnossBase.GnossOCBase.LanguageEnum.es, nombre);
                dicNombresTraduccion.Add(GnossBase.GnossOCBase.LanguageEnum.en, traduccion);
                concept.Skos_prefLabelMulti = dicNombresTraduccion;
                concept.Dc_identifier = id;
                concept.Dc_source = pSource;
                listConcepts.Add(concept);
            }

            foreach (Concept concept in listConcepts)
            {
                string[] idSplit = concept.Dc_identifier.Split('.');
                concept.Skos_narrower = new List<Concept>();
                concept.Skos_broader = new List<Concept>();
                if (concept.Dc_identifier.EndsWith(".0.0"))
                {
                    concept.Skos_narrower = listConcepts.Where(x => x.Dc_identifier.StartsWith(idSplit[0] + ".") && x.Dc_identifier.EndsWith(".0") && x.Dc_identifier != concept.Dc_identifier).ToList();
                    concept.Skos_symbol = "1";
                }
                else if (concept.Dc_identifier.EndsWith(".0"))
                {
                    concept.Skos_broader = listConcepts.Where(x => x.Dc_identifier.EndsWith(".0.0") && x.Dc_identifier.StartsWith(idSplit[0] + ".")).ToList();
                    concept.Skos_narrower = listConcepts.Where(x => x.Dc_identifier.StartsWith(idSplit[0] + "." + idSplit[1] + ".") && x.Dc_identifier != concept.Dc_identifier).ToList();
                    concept.Skos_symbol = "2";
                }
                else
                {
                    concept.Skos_broader = listConcepts.Where(x => x.Dc_identifier.StartsWith(idSplit[0] + "." + idSplit[1] + ".") && x.Dc_identifier.EndsWith(".0") && x.Dc_identifier != concept.Dc_identifier).ToList();
                    concept.Skos_symbol = "3";
                }
                pListaRecursosCargar.Add(((ConceptEDMA)concept).ToGnossApiResource(mResourceApi, concept.Dc_identifier));
            }

            CollectionEDMA collection = new CollectionEDMA();
            collection.Dc_source = pSource;
            collection.Skos_member = listConcepts.Where(x => x.Dc_identifier.EndsWith(".0.0")).ToList();
            collection.Skos_scopeNote = "Sector aplicacion";
            pListaRecursosCargar.Add(collection.ToGnossApiResource(mResourceApi, "0"));
        }

        /// <summary>
        /// Carga el tesauro de research area
        /// </summary>
        private static void CargarTesauroResearchArea()
        {
            string ontology = "taxonomy";

            //Cambio de ontología.
            mResourceApi.ChangeOntoly(ontology);
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Collection", "taxonomy", "researcharea");
            EliminarDatosCargados("http://www.w3.org/2008/05/skos#Concept", "taxonomy", "researcharea");

            //Obtención de los objetos a cargar.
            List<SecondaryResource> categorias = new List<SecondaryResource>();
            ObtenerTesauroResearchArea(ref categorias, "researcharea");

            //Carga.
            Parallel.ForEach(categorias, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, categoria =>
            {
                mResourceApi.LoadSecondaryResource(categoria);
            });
        }

        /// <summary>
        /// Proceso para contruir el tesáuro con la taxonomía unificada.
        /// </summary>
        /// <param name="pListaRecursosCargar">Lista de recursos a cargar.</param>
        /// <param name="pSource">Nombre del tesáuro.</param>
        private static void ObtenerTesauroResearchArea(ref List<SecondaryResource> pListaRecursosCargar, string pSource)
        {
            DataSet ds = LeerDatosExcel($"Dataset{Path.DirectorySeparatorChar}Hércules-CommonsEDMA_Taxonomías_v1.36.xlsx");

            //Recorremos las filas
            List<Concept> listConcepts = new List<Concept>();
            foreach (DataRow fila in ds.Tables["HérculesKA-taxonomy (clean)"].Rows)
            {
                string level0 = fila["Level 0"].ToString();
                string level1 = fila["Level 1"].ToString();
                string level2 = fila["Level 2"].ToString();
                string level3 = fila["Level 3"].ToString();
                string scopusDescriptor = fila["ASJC-Scopus descriptor"].ToString();
                string wosDescriptor = fila["WoS-JCR descriptor"].ToString();
                string id = fila["ID"].ToString();
                string nombre = level0;
                string symbol = "1";
                if (string.IsNullOrEmpty(nombre))
                {
                    nombre = level1;
                    symbol = "2";
                }
                if (string.IsNullOrEmpty(nombre))
                {
                    nombre = level2;
                    symbol = "3";
                }
                if (string.IsNullOrEmpty(nombre))
                {
                    nombre = level3;
                    symbol = "4";
                }
                if (string.IsNullOrEmpty(nombre))
                {
                    throw new Exception("No tiene nombre la categoría");
                }

                ConceptEDMA concept = new ConceptEDMA();
                concept.Dc_identifier = id;
                concept.Dc_source = pSource;
                concept.Skos_prefLabel = nombre;
                concept.Skos_symbol = symbol;
                concept.Roh_sourceDescriptor = new List<SourceDescriptor>();
                if (!string.IsNullOrEmpty(wosDescriptor))
                {
                    SourceDescriptor sourceDescriptor = new SourceDescriptor();
                    sourceDescriptor.IdRoh_impactSource = mResourceApi.GraphsUrl + "items/referencesource_000";
                    sourceDescriptor.Roh_impactSourceCategory = wosDescriptor;
                    concept.Roh_sourceDescriptor.Add(sourceDescriptor);
                }
                if (!string.IsNullOrEmpty(scopusDescriptor))
                {
                    SourceDescriptor sourceDescriptor = new SourceDescriptor();
                    sourceDescriptor.IdRoh_impactSource = mResourceApi.GraphsUrl + "items/referencesource_010";
                    sourceDescriptor.Roh_impactSourceCategory = scopusDescriptor;
                    concept.Roh_sourceDescriptor.Add(sourceDescriptor);
                }
                listConcepts.Add(concept);
            }

            foreach (Concept concept in listConcepts)
            {
                string[] idSplit = concept.Dc_identifier.Split('.');
                concept.Skos_narrower = new List<Concept>();
                concept.Skos_broader = new List<Concept>();
                if (concept.Dc_identifier.EndsWith(".0.0.0"))
                {
                    concept.Skos_narrower = listConcepts.Where(x => x.Dc_identifier.StartsWith(idSplit[0] + ".") && x.Dc_identifier.EndsWith(".0.0") && x.Dc_identifier != concept.Dc_identifier).ToList();
                }
                else if (concept.Dc_identifier.EndsWith(".0.0"))
                {
                    concept.Skos_broader = listConcepts.Where(x => x.Dc_identifier.EndsWith(".0.0.0") && x.Dc_identifier.StartsWith(idSplit[0] + ".")).ToList();
                    concept.Skos_narrower = listConcepts.Where(x => x.Dc_identifier.StartsWith(idSplit[0] + "." + idSplit[1] + ".") && x.Dc_identifier.EndsWith(".0") && x.Dc_identifier != concept.Dc_identifier).ToList();
                }
                else if (concept.Dc_identifier.EndsWith(".0"))
                {
                    concept.Skos_broader = listConcepts.Where(x => x.Dc_identifier.EndsWith(".0.0") && x.Dc_identifier.StartsWith(idSplit[0] + "." + idSplit[1] + ".")).ToList();
                    concept.Skos_narrower = listConcepts.Where(x => x.Dc_identifier.StartsWith(idSplit[0] + "." + idSplit[1] + "." + idSplit[2] + ".") && x.Dc_identifier != concept.Dc_identifier).ToList();
                }
                else
                {
                    concept.Skos_broader = listConcepts.Where(x => x.Dc_identifier.StartsWith(idSplit[0] + "." + idSplit[1] + "." + idSplit[2] + ".") && x.Dc_identifier.EndsWith(".0") && x.Dc_identifier != concept.Dc_identifier).ToList();
                }
                pListaRecursosCargar.Add(((ConceptEDMA)concept).ToGnossApiResource(mResourceApi, concept.Dc_identifier));
            }

            CollectionEDMA collection = new CollectionEDMA();
            collection.Dc_source = pSource;
            collection.Skos_member = listConcepts.Where(x => x.Dc_identifier.EndsWith(".0.0.0")).ToList();
            collection.Skos_scopeNote = "Research areas";
            pListaRecursosCargar.Add(collection.ToGnossApiResource(mResourceApi, "0"));
        }



        /// <summary>
        /// Elimina los datos del grafo.
        /// </summary>
        /// <param name="pRdfType">RdfType del recurso a borrar.</param>
        /// <param name="pOntology">Ontología a consultar.</param>
        /// <param name="pSource">Source para los tesauros semánticos</param>
        public static void EliminarDatosCargados(string pRdfType, string pOntology, string pSource = "")
        {
            //Consulta.
            string select = string.Empty, where = string.Empty;
            select += $@"SELECT ?s ";
            where += $@"WHERE {{ ";
            where += $@"?s a <{pRdfType}>. ";
            if (!string.IsNullOrEmpty(pSource))
            {
                where += "?s <http://purl.org/dc/elements/1.1/source> '" + pSource + "'";
            }
            where += $@"}} ";

            //Obtiene las URLs de los recursos a borrar.
            List<string> listaUrlSecundarias = new List<string>();
            SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, pOntology);

            if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
            {
                listaUrlSecundarias = resultadoQuery.results.bindings.Select(x => GetValorFilaSparqlObject(x, "s")).ToList();
            }
            Parallel.ForEach(listaUrlSecundarias, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, url =>
            {
                List<string> listaUrlSecundariasAux = new List<string>() { url };
                mResourceApi.DeleteSecondaryEntitiesList(ref listaUrlSecundariasAux);
            });
        }

        /// <summary>
        /// Obtiene el valor de de las filas de la consulta.
        /// </summary>
        /// <param name="pFila">Fila con el resultado.</param>
        /// <param name="pParametro">Parametro a obtener.</param>
        /// <returns>Dato guardado.</returns>
        public static string GetValorFilaSparqlObject(Dictionary<string, SparqlObject.Data> pFila, string pParametro)
        {
            if (pFila.ContainsKey(pParametro) && !string.IsNullOrEmpty(pFila[pParametro].value))
            {
                return pFila[pParametro].value;
            }
            return null;
        }

        /// <summary>
        /// Permite mapear el idioma obtenido del XML al idioma de las clases generadas.
        /// </summary>
        /// <returns>Diccionario con la conversión de los idiomas.</returns>
        private static Dictionary<string, LanguageEnum> MapearLenguajes()
        {
            Dictionary<string, LanguageEnum> dicIdiomas = new Dictionary<string, LanguageEnum>();
            dicIdiomas.Add("spa", LanguageEnum.es);
            dicIdiomas.Add("eng", LanguageEnum.en);
            dicIdiomas.Add("cat", LanguageEnum.ca);
            dicIdiomas.Add("eus", LanguageEnum.eu);
            dicIdiomas.Add("glg", LanguageEnum.gl);
            dicIdiomas.Add("fra", LanguageEnum.fr);
            return dicIdiomas;
        }

        /// <summary>
        /// Permite leer los datos de un excel.
        /// </summary>
        /// <param name="pRuta">Ruta del excel.</param>
        /// <returns>Objeto con los datos leidos.</returns>
        private static DataSet LeerDatosExcel(string pRuta)
        {
            // Lectura del Excel.
            DataSet ds = new DataSet();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(pRuta, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true,
                        }
                    });
                }
            }

            return ds;
        }
    }
}
