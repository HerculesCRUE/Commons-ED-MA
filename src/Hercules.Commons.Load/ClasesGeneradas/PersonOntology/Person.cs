using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using Gnoss.ApiWrapper.Helpers;
using GnossBase;
using Es.Riam.Gnoss.Web.MVC.Models;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using Gnoss.ApiWrapper.Exceptions;
using System.Diagnostics.CodeAnalysis;
using Organization = OrganizationOntology.Organization;
using Department = DepartmentOntology.Department;
using Group = GroupOntology.Group;
using Gender = GenderOntology.Gender;

namespace PersonOntology
{
	[ExcludeFromCodeCoverage]
	public class Person : GnossOCBase
	{

		public Person() : base() { } 

		public Person(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_hasRole = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasRole");
			if(propRoh_hasRole != null && propRoh_hasRole.PropertyValues.Count > 0)
			{
				this.Roh_hasRole = new Organization(propRoh_hasRole.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_metricPage = new List<MetricPage>();
			SemanticPropertyModel propRoh_metricPage = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/metricPage");
			if(propRoh_metricPage != null && propRoh_metricPage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_metricPage.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						MetricPage roh_metricPage = new MetricPage(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_metricPage.Add(roh_metricPage);
					}
				}
			}
			SemanticPropertyModel propVivo_departmentOrSchool = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#departmentOrSchool");
			if(propVivo_departmentOrSchool != null && propVivo_departmentOrSchool.PropertyValues.Count > 0)
			{
				this.Vivo_departmentOrSchool = new Department(propVivo_departmentOrSchool.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Vivo_hasResearchArea = new List<CategoryPath>();
			SemanticPropertyModel propVivo_hasResearchArea = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#hasResearchArea");
			if(propVivo_hasResearchArea != null && propVivo_hasResearchArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_hasResearchArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath vivo_hasResearchArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_hasResearchArea.Add(vivo_hasResearchArea);
					}
				}
			}
			this.Vivo_relates = new List<Group>();
			SemanticPropertyModel propVivo_relates = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#relates");
			if(propVivo_relates != null && propVivo_relates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_relates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Group vivo_relates = new Group(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_relates.Add(vivo_relates);
					}
				}
			}
			SemanticPropertyModel propFoaf_gender = pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/gender");
			if(propFoaf_gender != null && propFoaf_gender.PropertyValues.Count > 0)
			{
				this.Foaf_gender = new Gender(propFoaf_gender.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_lastUpdatedDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/lastUpdatedDate"));
			SemanticPropertyModel propRoh_lineResearch = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/lineResearch");
			this.Roh_lineResearch = new List<string>();
			if (propRoh_lineResearch != null && propRoh_lineResearch.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_lineResearch.PropertyValues)
				{
					this.Roh_lineResearch.Add(propValue.Value);
				}
			}
			this.Roh_isOtriManager= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isOtriManager"));
			this.Roh_publicProjectsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicProjectsNumber"));
			this.Roh_publicationsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationsNumber"));
			this.Vivo_researcherId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#researcherId"));
			this.Roh_ipNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/ipNumber"));
			this.Roh_tokenFigShare = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/tokenFigShare"));
			this.Roh_tokenGitHub = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/tokenGitHub"));
			this.Roh_publicCollaboratorsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicCollaboratorsNumber"));
			this.Vivo_scopusId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#scopusId"));
			this.Vcard_address = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#address"));
			SemanticPropertyModel propVcard_email = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#email");
			this.Vcard_email = new List<string>();
			if (propVcard_email != null && propVcard_email.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVcard_email.PropertyValues)
				{
					this.Vcard_email.Add(propValue.Value);
				}
			}
			this.Vivo_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#description"));
			this.Roh_themedAreasNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/themedAreasNumber"));
			this.Roh_usuarioGitHub = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/usuarioGitHub"));
			this.Roh_publicPublicationsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicPublicationsNumber"));
			this.Roh_projectsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectsNumber"));
			this.Roh_hasPosition = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasPosition"));
			SemanticPropertyModel propFoaf_homepage = pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/homepage");
			this.Foaf_homepage = new List<string>();
			if (propFoaf_homepage != null && propFoaf_homepage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propFoaf_homepage.PropertyValues)
				{
					this.Foaf_homepage.Add(propValue.Value);
				}
			}
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_semanticScholarId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/semanticScholarId"));
			this.Roh_usuarioFigShare = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/usuarioFigShare"));
			this.Roh_ORCID = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/ORCID"));
			this.Roh_h_index = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/h-index"));
			SemanticPropertyModel propVcard_hasTelephone = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasTelephone");
			this.Vcard_hasTelephone = new List<string>();
			if (propVcard_hasTelephone != null && propVcard_hasTelephone.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVcard_hasTelephone.PropertyValues)
				{
					this.Vcard_hasTelephone.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propFoaf_nick = pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/nick");
			this.Foaf_nick = new List<string>();
			if (propFoaf_nick != null && propFoaf_nick.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propFoaf_nick.PropertyValues)
				{
					this.Foaf_nick.Add(propValue.Value);
				}
			}
			this.Roh_isIPGroupActually= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isIPGroupActually"));
			this.Foaf_firstName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/firstName"));
			this.Roh_isIPProjectHistorically= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isIPProjectHistorically"));
			this.Roh_isIPGroupHistorically= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isIPGroupHistorically"));
			this.Roh_isSynchronized= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isSynchronized"));
			this.Roh_isActive= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isActive"));
			this.Roh_isIPProjectActually= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isIPProjectActually"));
			this.Foaf_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/name"));
			this.Foaf_lastName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/lastName"));
		}

		public Person(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_hasRole = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasRole");
			if(propRoh_hasRole != null && propRoh_hasRole.PropertyValues.Count > 0)
			{
				this.Roh_hasRole = new Organization(propRoh_hasRole.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_metricPage = new List<MetricPage>();
			SemanticPropertyModel propRoh_metricPage = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/metricPage");
			if(propRoh_metricPage != null && propRoh_metricPage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_metricPage.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						MetricPage roh_metricPage = new MetricPage(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_metricPage.Add(roh_metricPage);
					}
				}
			}
			SemanticPropertyModel propVivo_departmentOrSchool = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#departmentOrSchool");
			if(propVivo_departmentOrSchool != null && propVivo_departmentOrSchool.PropertyValues.Count > 0)
			{
				this.Vivo_departmentOrSchool = new Department(propVivo_departmentOrSchool.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Vivo_hasResearchArea = new List<CategoryPath>();
			SemanticPropertyModel propVivo_hasResearchArea = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#hasResearchArea");
			if(propVivo_hasResearchArea != null && propVivo_hasResearchArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_hasResearchArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath vivo_hasResearchArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_hasResearchArea.Add(vivo_hasResearchArea);
					}
				}
			}
			this.Vivo_relates = new List<Group>();
			SemanticPropertyModel propVivo_relates = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#relates");
			if(propVivo_relates != null && propVivo_relates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_relates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Group vivo_relates = new Group(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_relates.Add(vivo_relates);
					}
				}
			}
			SemanticPropertyModel propFoaf_gender = pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/gender");
			if(propFoaf_gender != null && propFoaf_gender.PropertyValues.Count > 0)
			{
				this.Foaf_gender = new Gender(propFoaf_gender.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_lastUpdatedDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/lastUpdatedDate"));
			SemanticPropertyModel propRoh_lineResearch = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/lineResearch");
			this.Roh_lineResearch = new List<string>();
			if (propRoh_lineResearch != null && propRoh_lineResearch.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_lineResearch.PropertyValues)
				{
					this.Roh_lineResearch.Add(propValue.Value);
				}
			}
			this.Roh_isOtriManager= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isOtriManager"));
			this.Roh_publicProjectsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicProjectsNumber"));
			this.Roh_publicationsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationsNumber"));
			this.Vivo_researcherId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#researcherId"));
			this.Roh_ipNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/ipNumber"));
			this.Roh_tokenFigShare = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/tokenFigShare"));
			this.Roh_tokenGitHub = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/tokenGitHub"));
			this.Roh_publicCollaboratorsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicCollaboratorsNumber"));
			this.Vivo_scopusId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#scopusId"));
			this.Vcard_address = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#address"));
			SemanticPropertyModel propVcard_email = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#email");
			this.Vcard_email = new List<string>();
			if (propVcard_email != null && propVcard_email.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVcard_email.PropertyValues)
				{
					this.Vcard_email.Add(propValue.Value);
				}
			}
			this.Vivo_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#description"));
			this.Roh_themedAreasNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/themedAreasNumber"));
			this.Roh_usuarioGitHub = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/usuarioGitHub"));
			this.Roh_publicPublicationsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicPublicationsNumber"));
			this.Roh_projectsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectsNumber"));
			this.Roh_hasPosition = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasPosition"));
			SemanticPropertyModel propFoaf_homepage = pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/homepage");
			this.Foaf_homepage = new List<string>();
			if (propFoaf_homepage != null && propFoaf_homepage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propFoaf_homepage.PropertyValues)
				{
					this.Foaf_homepage.Add(propValue.Value);
				}
			}
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_semanticScholarId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/semanticScholarId"));
			this.Roh_usuarioFigShare = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/usuarioFigShare"));
			this.Roh_ORCID = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/ORCID"));
			this.Roh_h_index = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/h-index"));
			SemanticPropertyModel propVcard_hasTelephone = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasTelephone");
			this.Vcard_hasTelephone = new List<string>();
			if (propVcard_hasTelephone != null && propVcard_hasTelephone.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVcard_hasTelephone.PropertyValues)
				{
					this.Vcard_hasTelephone.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propFoaf_nick = pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/nick");
			this.Foaf_nick = new List<string>();
			if (propFoaf_nick != null && propFoaf_nick.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propFoaf_nick.PropertyValues)
				{
					this.Foaf_nick.Add(propValue.Value);
				}
			}
			this.Roh_isIPGroupActually= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isIPGroupActually"));
			this.Foaf_firstName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/firstName"));
			this.Roh_isIPProjectHistorically= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isIPProjectHistorically"));
			this.Roh_isIPGroupHistorically= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isIPGroupHistorically"));
			this.Roh_isSynchronized= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isSynchronized"));
			this.Roh_isActive= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isActive"));
			this.Roh_isIPProjectActually= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isIPProjectActually"));
			this.Foaf_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/name"));
			this.Foaf_lastName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/lastName"));
		}

		public virtual string RdfType { get { return "http://xmlns.com/foaf/0.1/Person"; } }
		public virtual string RdfsLabel { get { return "http://xmlns.com/foaf/0.1/Person"; } }
		[LABEL(LanguageEnum.es,"http://w3id.org/roh/hasRole")]
		[RDFProperty("http://w3id.org/roh/hasRole")]
		public  Organization Roh_hasRole  { get; set;} 
		public string IdRoh_hasRole  { get; set;} 

		[RDFProperty("http://w3id.org/roh/metricPage")]
		public  List<MetricPage> Roh_metricPage { get; set;}

		[LABEL(LanguageEnum.es,"http://vivoweb.org/ontology/core#departmentOrSchool")]
		[RDFProperty("http://vivoweb.org/ontology/core#departmentOrSchool")]
		public  Department Vivo_departmentOrSchool  { get; set;} 
		public string IdVivo_departmentOrSchool  { get; set;} 

		[LABEL(LanguageEnum.es,"Usuario Gnoss")]
		[RDFProperty("http://w3id.org/roh/gnossUser")]
		public  object Roh_gnossUser  { get; set;} 
		public string IdRoh_gnossUser  { get; set;} 

		[LABEL(LanguageEnum.es,"http://vivoweb.org/ontology/core#hasResearchArea")]
		[RDFProperty("http://vivoweb.org/ontology/core#hasResearchArea")]
		public  List<CategoryPath> Vivo_hasResearchArea { get; set;}

		[LABEL(LanguageEnum.es,"http://vivoweb.org/ontology/core#relates")]
		[RDFProperty("http://vivoweb.org/ontology/core#relates")]
		public  List<Group> Vivo_relates { get; set;}
		public List<string> IdsVivo_relates { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/gender")]
		public  Gender Foaf_gender  { get; set;} 
		public string IdFoaf_gender  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/lastUpdatedDate")]
		[RDFProperty("http://w3id.org/roh/lastUpdatedDate")]
		public  DateTime? Roh_lastUpdatedDate { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/lineResearch")]
		[RDFProperty("http://w3id.org/roh/lineResearch")]
		public  List<string> Roh_lineResearch { get; set;}

		[RDFProperty("http://w3id.org/roh/isOtriManager")]
		public  bool Roh_isOtriManager { get; set;}

		[RDFProperty("http://w3id.org/roh/publicProjectsNumber")]
		public  int? Roh_publicProjectsNumber { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/publicationsNumber")]
		[RDFProperty("http://w3id.org/roh/publicationsNumber")]
		public  int? Roh_publicationsNumber { get; set;}

		[LABEL(LanguageEnum.es,"http://vivoweb.org/ontology/core#researcherId")]
		[RDFProperty("http://vivoweb.org/ontology/core#researcherId")]
		public  string Vivo_researcherId { get; set;}

		[RDFProperty("http://w3id.org/roh/ipNumber")]
		public  int? Roh_ipNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/tokenFigShare")]
		public  string Roh_tokenFigShare { get; set;}

		[RDFProperty("http://w3id.org/roh/tokenGitHub")]
		public  string Roh_tokenGitHub { get; set;}

		[RDFProperty("http://w3id.org/roh/publicCollaboratorsNumber")]
		public  int? Roh_publicCollaboratorsNumber { get; set;}

		[LABEL(LanguageEnum.es,"http://vivoweb.org/ontology/core#scopusId")]
		[RDFProperty("http://vivoweb.org/ontology/core#scopusId")]
		public  string Vivo_scopusId { get; set;}

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#address")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#address")]
		public  string Vcard_address { get; set;}

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#email")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#email")]
		public  List<string> Vcard_email { get; set;}

		[LABEL(LanguageEnum.es,"http://vivoweb.org/ontology/core#description")]
		[RDFProperty("http://vivoweb.org/ontology/core#description")]
		public  string Vivo_description { get; set;}

		[RDFProperty("http://w3id.org/roh/themedAreasNumber")]
		public  int? Roh_themedAreasNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/usuarioGitHub")]
		public  string Roh_usuarioGitHub { get; set;}

		[RDFProperty("http://w3id.org/roh/publicPublicationsNumber")]
		public  int? Roh_publicPublicationsNumber { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/projectsNumber")]
		[RDFProperty("http://w3id.org/roh/projectsNumber")]
		public  int? Roh_projectsNumber { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/hasPosition")]
		[RDFProperty("http://w3id.org/roh/hasPosition")]
		public  string Roh_hasPosition { get; set;}

		[LABEL(LanguageEnum.es,"http://xmlns.com/foaf/0.1/homepage")]
		[RDFProperty("http://xmlns.com/foaf/0.1/homepage")]
		public  List<string> Foaf_homepage { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/crisIdentifier")]
		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/semanticScholarId")]
		[RDFProperty("http://w3id.org/roh/semanticScholarId")]
		public  string Roh_semanticScholarId { get; set;}

		[RDFProperty("http://w3id.org/roh/usuarioFigShare")]
		public  string Roh_usuarioFigShare { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/ORCID")]
		[RDFProperty("http://w3id.org/roh/ORCID")]
		public  string Roh_ORCID { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/h-index")]
		[RDFProperty("http://w3id.org/roh/h-index")]
		public  float? Roh_h_index { get; set;}

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#hasTelephone")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasTelephone")]
		public  List<string> Vcard_hasTelephone { get; set;}

		[LABEL(LanguageEnum.es,"http://xmlns.com/foaf/0.1/nick")]
		[RDFProperty("http://xmlns.com/foaf/0.1/nick")]
		public  List<string> Foaf_nick { get; set;}

		[RDFProperty("http://w3id.org/roh/isIPGroupActually")]
		public  bool Roh_isIPGroupActually { get; set;}

		[LABEL(LanguageEnum.es,"http://xmlns.com/foaf/0.1/firstName")]
		[RDFProperty("http://xmlns.com/foaf/0.1/firstName")]
		public  string Foaf_firstName { get; set;}

		[RDFProperty("http://w3id.org/roh/isIPProjectHistorically")]
		public  bool Roh_isIPProjectHistorically { get; set;}

		[RDFProperty("http://w3id.org/roh/isIPGroupHistorically")]
		public  bool Roh_isIPGroupHistorically { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/isSynchronized")]
		[RDFProperty("http://w3id.org/roh/isSynchronized")]
		public  bool Roh_isSynchronized { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/isActive")]
		[RDFProperty("http://w3id.org/roh/isActive")]
		public  bool Roh_isActive { get; set;}

		[RDFProperty("http://w3id.org/roh/isIPProjectActually")]
		public  bool Roh_isIPProjectActually { get; set;}

		[LABEL(LanguageEnum.es,"http://xmlns.com/foaf/0.1/name")]
		[RDFProperty("http://xmlns.com/foaf/0.1/name")]
		public  string Foaf_name { get; set;}

		[LABEL(LanguageEnum.es,"http://xmlns.com/foaf/0.1/lastName")]
		[RDFProperty("http://xmlns.com/foaf/0.1/lastName")]
		public  string Foaf_lastName { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:hasRole", this.IdRoh_hasRole));
			propList.Add(new StringOntologyProperty("vivo:departmentOrSchool", this.IdVivo_departmentOrSchool));
			//propList.Add(new StringOntologyProperty("roh:gnossUser", this.Roh_gnossUser));
			propList.Add(new ListStringOntologyProperty("vivo:relates", this.IdsVivo_relates));
			propList.Add(new StringOntologyProperty("foaf:gender", this.IdFoaf_gender));
			if (this.Roh_lastUpdatedDate.HasValue){
				propList.Add(new DateOntologyProperty("roh:lastUpdatedDate", this.Roh_lastUpdatedDate.Value));
				}
			propList.Add(new ListStringOntologyProperty("roh:lineResearch", this.Roh_lineResearch));
			propList.Add(new BoolOntologyProperty("roh:isOtriManager", this.Roh_isOtriManager));
			propList.Add(new StringOntologyProperty("roh:publicProjectsNumber", this.Roh_publicProjectsNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:publicationsNumber", this.Roh_publicationsNumber.ToString()));
			propList.Add(new StringOntologyProperty("vivo:researcherId", this.Vivo_researcherId));
			propList.Add(new StringOntologyProperty("roh:ipNumber", this.Roh_ipNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:tokenFigShare", this.Roh_tokenFigShare));
			propList.Add(new StringOntologyProperty("roh:tokenGitHub", this.Roh_tokenGitHub));
			propList.Add(new StringOntologyProperty("roh:publicCollaboratorsNumber", this.Roh_publicCollaboratorsNumber.ToString()));
			propList.Add(new StringOntologyProperty("vivo:scopusId", this.Vivo_scopusId));
			propList.Add(new StringOntologyProperty("vcard:address", this.Vcard_address));
			propList.Add(new ListStringOntologyProperty("vcard:email", this.Vcard_email));
			propList.Add(new StringOntologyProperty("vivo:description", this.Vivo_description));
			propList.Add(new StringOntologyProperty("roh:themedAreasNumber", this.Roh_themedAreasNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:usuarioGitHub", this.Roh_usuarioGitHub));
			propList.Add(new StringOntologyProperty("roh:publicPublicationsNumber", this.Roh_publicPublicationsNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:projectsNumber", this.Roh_projectsNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:hasPosition", this.Roh_hasPosition));
			propList.Add(new ListStringOntologyProperty("foaf:homepage", this.Foaf_homepage));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			propList.Add(new StringOntologyProperty("roh:semanticScholarId", this.Roh_semanticScholarId));
			propList.Add(new StringOntologyProperty("roh:usuarioFigShare", this.Roh_usuarioFigShare));
			propList.Add(new StringOntologyProperty("roh:ORCID", this.Roh_ORCID));
			propList.Add(new StringOntologyProperty("roh:h-index", this.Roh_h_index.ToString()));
			propList.Add(new ListStringOntologyProperty("vcard:hasTelephone", this.Vcard_hasTelephone));
			propList.Add(new ListStringOntologyProperty("foaf:nick", this.Foaf_nick));
			propList.Add(new BoolOntologyProperty("roh:isIPGroupActually", this.Roh_isIPGroupActually));
			propList.Add(new StringOntologyProperty("foaf:firstName", this.Foaf_firstName));
			propList.Add(new BoolOntologyProperty("roh:isIPProjectHistorically", this.Roh_isIPProjectHistorically));
			propList.Add(new BoolOntologyProperty("roh:isIPGroupHistorically", this.Roh_isIPGroupHistorically));
			propList.Add(new BoolOntologyProperty("roh:isSynchronized", this.Roh_isSynchronized));
			propList.Add(new BoolOntologyProperty("roh:isActive", this.Roh_isActive));
			propList.Add(new BoolOntologyProperty("roh:isIPProjectActually", this.Roh_isIPProjectActually));
			propList.Add(new StringOntologyProperty("foaf:name", this.Foaf_name));
			propList.Add(new StringOntologyProperty("foaf:lastName", this.Foaf_lastName));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_metricPage!=null){
				foreach(MetricPage prop in Roh_metricPage){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityMetricPage = new OntologyEntity("http://w3id.org/roh/MetricPage", "http://w3id.org/roh/MetricPage", "roh:metricPage", prop.propList, prop.entList);
				entList.Add(entityMetricPage);
				prop.Entity= entityMetricPage;
				}
			}
			if(Vivo_hasResearchArea!=null){
				foreach(CategoryPath prop in Vivo_hasResearchArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "vivo:hasResearchArea", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
		} 
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology=null;
			GetEntities();
			GetProperties();
			if(idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList,idrecurso,idarticulo);
			}
			resource.Id = GNOSSID;
			resource.Ontology = ontology;
			resource.TextCategories=listaDeCategorias;
			AddResourceTitle(resource);
			AddResourceDescription(resource);
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://xmlns.com/foaf/0.1/Person>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://xmlns.com/foaf/0.1/Person\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_metricPage != null)
			{
			foreach(var item0 in this.Roh_metricPage)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricPage_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/MetricPage>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricPage_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/MetricPage\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/MetricPage_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://w3id.org/roh/metricPage", $"<{resourceAPI.GraphsUrl}items/MetricPage_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Roh_metricGraphic != null)
			{
			foreach(var item1 in item0.Roh_metricGraphic)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/MetricGraphic>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/MetricGraphic\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricPage_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/metricGraphic", $"<{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_filters != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/filters", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_filters)}\"", list, " . ");
				}
				if(item1.Roh_width != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/width", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_width)}\"", list, " . ");
				}
				if(item1.Roh_pageId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/pageId", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_pageId)}\"", list, " . ");
				}
				if(item1.Roh_order != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/order", $"{item1.Roh_order.ToString()}", list, " . ");
				}
				if(item1.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_title)}\"", list, " . ");
				}
				if(item1.Roh_graphicId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricGraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/graphicId", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_graphicId)}\"", list, " . ");
				}
			}
			}
				if(item0.Roh_order != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricPage_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/order", $"{item0.Roh_order.ToString()}", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MetricPage_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title)}\"", list, " . ");
				}
			}
			}
			if(this.Vivo_hasResearchArea != null)
			{
			foreach(var item0 in this.Vivo_hasResearchArea)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#hasResearchArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{item2}>", list, " . ");
					}
				}
			}
			}
				if(this.IdRoh_hasRole != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/hasRole", $"<{this.IdRoh_hasRole}>", list, " . ");
				}
				if(this.IdVivo_departmentOrSchool != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#departmentOrSchool", $"<{this.IdVivo_departmentOrSchool}>", list, " . ");
				}
				if(this.IdRoh_gnossUser != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/gnossUser", $"<{this.IdRoh_gnossUser}>", list, " . ");
				}
				if(this.IdsVivo_relates != null)
				{
					foreach(var item2 in this.IdsVivo_relates)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#relates", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdFoaf_gender != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://xmlns.com/foaf/0.1/gender", $"<{this.IdFoaf_gender}>", list, " . ");
				}
				if(this.Roh_lastUpdatedDate != null && this.Roh_lastUpdatedDate != DateTime.MinValue)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/lastUpdatedDate", $"\"{this.Roh_lastUpdatedDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_lineResearch != null)
				{
					foreach(var item2 in this.Roh_lineResearch)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://w3id.org/roh/lineResearch", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_isOtriManager != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isOtriManager", $"\"{this.Roh_isOtriManager.ToString()}\"", list, " . ");
				}
				if(this.Roh_publicProjectsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicProjectsNumber", $"{this.Roh_publicProjectsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_publicationsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicationsNumber", $"{this.Roh_publicationsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Vivo_researcherId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#researcherId", $"\"{GenerarTextoSinSaltoDeLinea(this.Vivo_researcherId)}\"", list, " . ");
				}
				if(this.Roh_ipNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/ipNumber", $"{this.Roh_ipNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_tokenFigShare != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/tokenFigShare", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_tokenFigShare)}\"", list, " . ");
				}
				if(this.Roh_tokenGitHub != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/tokenGitHub", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_tokenGitHub)}\"", list, " . ");
				}
				if(this.Roh_publicCollaboratorsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicCollaboratorsNumber", $"{this.Roh_publicCollaboratorsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Vivo_scopusId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#scopusId", $"\"{GenerarTextoSinSaltoDeLinea(this.Vivo_scopusId)}\"", list, " . ");
				}
				if(this.Vcard_address != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#address", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_address)}\"", list, " . ");
				}
				if(this.Vcard_email != null)
				{
					foreach(var item2 in this.Vcard_email)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://www.w3.org/2006/vcard/ns#email", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Vivo_description != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#description", $"\"{GenerarTextoSinSaltoDeLinea(this.Vivo_description)}\"", list, " . ");
				}
				if(this.Roh_themedAreasNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/themedAreasNumber", $"{this.Roh_themedAreasNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_usuarioGitHub != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/usuarioGitHub", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_usuarioGitHub)}\"", list, " . ");
				}
				if(this.Roh_publicPublicationsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicPublicationsNumber", $"{this.Roh_publicPublicationsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_projectsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/projectsNumber", $"{this.Roh_projectsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_hasPosition != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/hasPosition", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasPosition)}\"", list, " . ");
				}
				if(this.Foaf_homepage != null)
				{
					foreach(var item2 in this.Foaf_homepage)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://xmlns.com/foaf/0.1/homepage", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Roh_semanticScholarId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/semanticScholarId", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_semanticScholarId)}\"", list, " . ");
				}
				if(this.Roh_usuarioFigShare != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/usuarioFigShare", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_usuarioFigShare)}\"", list, " . ");
				}
				if(this.Roh_ORCID != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/ORCID", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_ORCID)}\"", list, " . ");
				}
				if(this.Roh_h_index != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/h-index", $"{this.Roh_h_index.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Vcard_hasTelephone != null)
				{
					foreach(var item2 in this.Vcard_hasTelephone)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "https://www.w3.org/2006/vcard/ns#hasTelephone", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Foaf_nick != null)
				{
					foreach(var item2 in this.Foaf_nick)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}", "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_isIPGroupActually != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isIPGroupActually", $"\"{this.Roh_isIPGroupActually.ToString()}\"", list, " . ");
				}
				if(this.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_firstName)}\"", list, " . ");
				}
				if(this.Roh_isIPProjectHistorically != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isIPProjectHistorically", $"\"{this.Roh_isIPProjectHistorically.ToString()}\"", list, " . ");
				}
				if(this.Roh_isIPGroupHistorically != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isIPGroupHistorically", $"\"{this.Roh_isIPGroupHistorically.ToString()}\"", list, " . ");
				}
				if(this.Roh_isSynchronized != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isSynchronized", $"\"{this.Roh_isSynchronized.ToString()}\"", list, " . ");
				}
				if(this.Roh_isActive != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isActive", $"\"{this.Roh_isActive.ToString()}\"", list, " . ");
				}
				if(this.Roh_isIPProjectActually != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isIPProjectActually", $"\"{this.Roh_isIPProjectActually.ToString()}\"", list, " . ");
				}
				if(this.Foaf_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://xmlns.com/foaf/0.1/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
				}
				if(this.Foaf_lastName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Person_{ResourceID}_{ArticleID}",  "http://xmlns.com/foaf/0.1/lastName", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_lastName)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"person\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://xmlns.com/foaf/0.1/Person\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_metricPage != null)
			{
			foreach(var item0 in this.Roh_metricPage)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/metricPage", $"<{resourceAPI.GraphsUrl}items/metricpage_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Roh_metricGraphic != null)
			{
			foreach(var item1 in item0.Roh_metricGraphic)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/metricpage_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/metricGraphic", $"<{resourceAPI.GraphsUrl}items/metricgraphic_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_filters != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/metricgraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/filters", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_filters).ToLower()}\"", list, " . ");
				}
				if(item1.Roh_width != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/metricgraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/width", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_width).ToLower()}\"", list, " . ");
				}
				if(item1.Roh_pageId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/metricgraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/pageId", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_pageId).ToLower()}\"", list, " . ");
				}
				if(item1.Roh_order != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/metricgraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/order", $"{item1.Roh_order.ToString()}", list, " . ");
				}
				if(item1.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/metricgraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_title).ToLower()}\"", list, " . ");
				}
				if(item1.Roh_graphicId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/metricgraphic_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/graphicId", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_graphicId).ToLower()}\"", list, " . ");
				}
			}
			}
				if(item0.Roh_order != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/metricpage_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/order", $"{item0.Roh_order.ToString()}", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/metricpage_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Vivo_hasResearchArea != null)
			{
			foreach(var item0 in this.Vivo_hasResearchArea)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://vivoweb.org/ontology/core#hasResearchArea", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
				if(this.IdRoh_hasRole != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_hasRole;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/hasRole", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdVivo_departmentOrSchool != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVivo_departmentOrSchool;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#departmentOrSchool", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_gnossUser != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_gnossUser;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/gnossUser", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsVivo_relates != null)
				{
					foreach(var item2 in this.IdsVivo_relates)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://vivoweb.org/ontology/core#relates", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdFoaf_gender != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdFoaf_gender;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://xmlns.com/foaf/0.1/gender", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_lastUpdatedDate != null && this.Roh_lastUpdatedDate != DateTime.MinValue)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/lastUpdatedDate", $"{this.Roh_lastUpdatedDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_lineResearch != null)
				{
					foreach(var item2 in this.Roh_lineResearch)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/lineResearch", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_isOtriManager != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isOtriManager", $"\"{this.Roh_isOtriManager.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_publicProjectsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicProjectsNumber", $"{this.Roh_publicProjectsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_publicationsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicationsNumber", $"{this.Roh_publicationsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Vivo_researcherId != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#researcherId", $"\"{GenerarTextoSinSaltoDeLinea(this.Vivo_researcherId).ToLower()}\"", list, " . ");
				}
				if(this.Roh_ipNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/ipNumber", $"{this.Roh_ipNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_tokenFigShare != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/tokenFigShare", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_tokenFigShare).ToLower()}\"", list, " . ");
				}
				if(this.Roh_tokenGitHub != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/tokenGitHub", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_tokenGitHub).ToLower()}\"", list, " . ");
				}
				if(this.Roh_publicCollaboratorsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicCollaboratorsNumber", $"{this.Roh_publicCollaboratorsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Vivo_scopusId != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#scopusId", $"\"{GenerarTextoSinSaltoDeLinea(this.Vivo_scopusId).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_address != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#address", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_address).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_email != null)
				{
					foreach(var item2 in this.Vcard_email)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://www.w3.org/2006/vcard/ns#email", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Vivo_description != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#description", $"\"{GenerarTextoSinSaltoDeLinea(this.Vivo_description).ToLower()}\"", list, " . ");
				}
				if(this.Roh_themedAreasNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/themedAreasNumber", $"{this.Roh_themedAreasNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_usuarioGitHub != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/usuarioGitHub", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_usuarioGitHub).ToLower()}\"", list, " . ");
				}
				if(this.Roh_publicPublicationsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicPublicationsNumber", $"{this.Roh_publicPublicationsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_projectsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/projectsNumber", $"{this.Roh_projectsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_hasPosition != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/hasPosition", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasPosition).ToLower()}\"", list, " . ");
				}
				if(this.Foaf_homepage != null)
				{
					foreach(var item2 in this.Foaf_homepage)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/homepage", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Roh_semanticScholarId != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/semanticScholarId", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_semanticScholarId).ToLower()}\"", list, " . ");
				}
				if(this.Roh_usuarioFigShare != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/usuarioFigShare", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_usuarioFigShare).ToLower()}\"", list, " . ");
				}
				if(this.Roh_ORCID != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/ORCID", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_ORCID).ToLower()}\"", list, " . ");
				}
				if(this.Roh_h_index != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/h-index", $"{this.Roh_h_index.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Vcard_hasTelephone != null)
				{
					foreach(var item2 in this.Vcard_hasTelephone)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://www.w3.org/2006/vcard/ns#hasTelephone", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Foaf_nick != null)
				{
					foreach(var item2 in this.Foaf_nick)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_isIPGroupActually != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isIPGroupActually", $"\"{this.Roh_isIPGroupActually.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Foaf_firstName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_firstName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_isIPProjectHistorically != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isIPProjectHistorically", $"\"{this.Roh_isIPProjectHistorically.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_isIPGroupHistorically != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isIPGroupHistorically", $"\"{this.Roh_isIPGroupHistorically.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_isSynchronized != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isSynchronized", $"\"{this.Roh_isSynchronized.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_isActive != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isActive", $"\"{this.Roh_isActive.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_isIPProjectActually != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isIPProjectActually", $"\"{this.Roh_isIPProjectActually.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Foaf_name != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://xmlns.com/foaf/0.1/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name).ToLower()}\"", list, " . ");
				}
				if(this.Foaf_lastName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://xmlns.com/foaf/0.1/lastName", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_lastName).ToLower()}\"", list, " . ");
				}
			if (listaSearch != null && listaSearch.Count > 0)
			{
				foreach(string valorSearch in listaSearch)
				{
					search += $"{valorSearch} ";
				}
			}
			if(!string.IsNullOrEmpty(search))
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/search", $"\"{GenerarTextoSinSaltoDeLinea(search.ToLower())}\"", list, " . ");
			}
			return list;
		}

		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{

			//Insert en la tabla Documento
			string tags = "";
			foreach(string tag in tagList)
			{
				tags += $"{tag}, ";
			}
			if (!string.IsNullOrEmpty(tags))
			{
				tags = tags.Substring(0, tags.LastIndexOf(','));
			}
			string titulo = $"{this.Foaf_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Foaf_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/PersonOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Foaf_name;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Foaf_name;
		}




	}
}
