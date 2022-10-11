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
using ContributionGradeProject = ContributiongradeprojectOntology.ContributionGradeProject;
using GeographicRegion = GeographicregionOntology.GeographicRegion;
using Person = PersonOntology.Person;

namespace TechnologicalresultOntology
{
	[ExcludeFromCodeCoverage]
	public class TechnologicalResult : GnossOCBase
	{

		public TechnologicalResult() : base() { } 

		public TechnologicalResult(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Roh_unescoPrimary = new List<CategoryPath>();
			SemanticPropertyModel propRoh_unescoPrimary = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/unescoPrimary");
			if(propRoh_unescoPrimary != null && propRoh_unescoPrimary.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_unescoPrimary.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_unescoPrimary = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_unescoPrimary.Add(roh_unescoPrimary);
					}
				}
			}
			this.Roh_targetOrganizations = new List<Organization>();
			SemanticPropertyModel propRoh_targetOrganizations = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/targetOrganizations");
			if(propRoh_targetOrganizations != null && propRoh_targetOrganizations.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_targetOrganizations.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_targetOrganizations = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_targetOrganizations.Add(roh_targetOrganizations);
					}
				}
			}
			this.Roh_unescoTertiary = new List<CategoryPath>();
			SemanticPropertyModel propRoh_unescoTertiary = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/unescoTertiary");
			if(propRoh_unescoTertiary != null && propRoh_unescoTertiary.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_unescoTertiary.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_unescoTertiary = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_unescoTertiary.Add(roh_unescoTertiary);
					}
				}
			}
			SemanticPropertyModel propRoh_contributionGrade = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contributionGrade");
			if(propRoh_contributionGrade != null && propRoh_contributionGrade.PropertyValues.Count > 0)
			{
				this.Roh_contributionGrade = new ContributionGradeProject(propRoh_contributionGrade.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_unescoSecondary = new List<CategoryPath>();
			SemanticPropertyModel propRoh_unescoSecondary = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/unescoSecondary");
			if(propRoh_unescoSecondary != null && propRoh_unescoSecondary.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_unescoSecondary.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_unescoSecondary = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_unescoSecondary.Add(roh_unescoSecondary);
					}
				}
			}
			this.Vivo_freeTextKeywords = new List<CategoryPath>();
			SemanticPropertyModel propVivo_freeTextKeywords = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeywords");
			if(propVivo_freeTextKeywords != null && propVivo_freeTextKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeywords.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath vivo_freeTextKeywords = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_freeTextKeywords.Add(vivo_freeTextKeywords);
					}
				}
			}
			this.Roh_participates = new List<Organization>();
			SemanticPropertyModel propRoh_participates = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participates");
			if(propRoh_participates != null && propRoh_participates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_participates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_participates = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_participates.Add(roh_participates);
					}
				}
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new GeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_relevantResults = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relevantResults"));
			this.Roh_principalInvestigatorNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorNick"));
			this.Roh_spinoffCompanies= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/spinoffCompanies"));
			this.Roh_activityResults= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/activityResults"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Roh_durationDays = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_principalInvestigatorSecondFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorSecondFamilyName"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_technologicalExpert= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/technologicalExpert"));
			this.Roh_coprincipalInvestigatorSecondFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/coprincipalInvestigatorSecondFamilyName"));
			this.Roh_principalInvestigatorFirstName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorFirstName"));
			this.Roh_coprincipalInvestigatorNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/coprincipalInvestigatorNick"));
			this.Roh_newTechniques= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/newTechniques"));
			this.Roh_principalInvestigatorFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorFamilyName"));
			this.Roh_durationMonths = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_collaborationAgreements= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collaborationAgreements"));
			this.Roh_durationYears = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_coprincipalInvestigatorFirstName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/coprincipalInvestigatorFirstName"));
			this.Roh_coprincipalInvestigatorFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/coprincipalInvestigatorFamilyName"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_homologation= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/homologation"));
			this.Roh_contributionGradeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contributionGradeOther"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public TechnologicalResult(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_unescoPrimary = new List<CategoryPath>();
			SemanticPropertyModel propRoh_unescoPrimary = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/unescoPrimary");
			if(propRoh_unescoPrimary != null && propRoh_unescoPrimary.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_unescoPrimary.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_unescoPrimary = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_unescoPrimary.Add(roh_unescoPrimary);
					}
				}
			}
			this.Roh_targetOrganizations = new List<Organization>();
			SemanticPropertyModel propRoh_targetOrganizations = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/targetOrganizations");
			if(propRoh_targetOrganizations != null && propRoh_targetOrganizations.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_targetOrganizations.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_targetOrganizations = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_targetOrganizations.Add(roh_targetOrganizations);
					}
				}
			}
			this.Roh_unescoTertiary = new List<CategoryPath>();
			SemanticPropertyModel propRoh_unescoTertiary = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/unescoTertiary");
			if(propRoh_unescoTertiary != null && propRoh_unescoTertiary.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_unescoTertiary.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_unescoTertiary = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_unescoTertiary.Add(roh_unescoTertiary);
					}
				}
			}
			SemanticPropertyModel propRoh_contributionGrade = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contributionGrade");
			if(propRoh_contributionGrade != null && propRoh_contributionGrade.PropertyValues.Count > 0)
			{
				this.Roh_contributionGrade = new ContributionGradeProject(propRoh_contributionGrade.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_unescoSecondary = new List<CategoryPath>();
			SemanticPropertyModel propRoh_unescoSecondary = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/unescoSecondary");
			if(propRoh_unescoSecondary != null && propRoh_unescoSecondary.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_unescoSecondary.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_unescoSecondary = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_unescoSecondary.Add(roh_unescoSecondary);
					}
				}
			}
			this.Vivo_freeTextKeywords = new List<CategoryPath>();
			SemanticPropertyModel propVivo_freeTextKeywords = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeywords");
			if(propVivo_freeTextKeywords != null && propVivo_freeTextKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeywords.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath vivo_freeTextKeywords = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_freeTextKeywords.Add(vivo_freeTextKeywords);
					}
				}
			}
			this.Roh_participates = new List<Organization>();
			SemanticPropertyModel propRoh_participates = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participates");
			if(propRoh_participates != null && propRoh_participates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_participates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_participates = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_participates.Add(roh_participates);
					}
				}
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new GeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_relevantResults = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relevantResults"));
			this.Roh_principalInvestigatorNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorNick"));
			this.Roh_spinoffCompanies= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/spinoffCompanies"));
			this.Roh_activityResults= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/activityResults"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Roh_durationDays = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_principalInvestigatorSecondFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorSecondFamilyName"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_technologicalExpert= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/technologicalExpert"));
			this.Roh_coprincipalInvestigatorSecondFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/coprincipalInvestigatorSecondFamilyName"));
			this.Roh_principalInvestigatorFirstName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorFirstName"));
			this.Roh_coprincipalInvestigatorNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/coprincipalInvestigatorNick"));
			this.Roh_newTechniques= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/newTechniques"));
			this.Roh_principalInvestigatorFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorFamilyName"));
			this.Roh_durationMonths = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_collaborationAgreements= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collaborationAgreements"));
			this.Roh_durationYears = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_coprincipalInvestigatorFirstName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/coprincipalInvestigatorFirstName"));
			this.Roh_coprincipalInvestigatorFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/coprincipalInvestigatorFamilyName"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_homologation= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/homologation"));
			this.Roh_contributionGradeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contributionGradeOther"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/TechnologicalResult"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/TechnologicalResult"; } }
		[RDFProperty("http://w3id.org/roh/unescoPrimary")]
		public  List<CategoryPath> Roh_unescoPrimary { get; set;}

		[RDFProperty("http://w3id.org/roh/targetOrganizations")]
		public  List<Organization> Roh_targetOrganizations { get; set;}

		[RDFProperty("http://w3id.org/roh/unescoTertiary")]
		public  List<CategoryPath> Roh_unescoTertiary { get; set;}

		[RDFProperty("http://w3id.org/roh/contributionGrade")]
		public  ContributionGradeProject Roh_contributionGrade  { get; set;} 
		public string IdRoh_contributionGrade  { get; set;} 

		[RDFProperty("http://w3id.org/roh/unescoSecondary")]
		public  List<CategoryPath> Roh_unescoSecondary { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#freeTextKeywords")]
		public  List<CategoryPath> Vivo_freeTextKeywords { get; set;}

		[RDFProperty("http://w3id.org/roh/participates")]
		public  List<Organization> Roh_participates { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#geographicFocus")]
		public  GeographicRegion Vivo_geographicFocus  { get; set;} 
		public string IdVivo_geographicFocus  { get; set;} 

		[RDFProperty("http://w3id.org/roh/relevantResults")]
		public  string Roh_relevantResults { get; set;}

		[RDFProperty("http://w3id.org/roh/principalInvestigatorNick")]
		public  string Roh_principalInvestigatorNick { get; set;}

		[RDFProperty("http://w3id.org/roh/spinoffCompanies")]
		public  bool Roh_spinoffCompanies { get; set;}

		[RDFProperty("http://w3id.org/roh/activityResults")]
		public  bool Roh_activityResults { get; set;}

		[RDFProperty("http://w3id.org/roh/geographicFocusOther")]
		public  string Roh_geographicFocusOther { get; set;}

		[RDFProperty("http://w3id.org/roh/durationDays")]
		public  float? Roh_durationDays { get; set;}

		[RDFProperty("http://w3id.org/roh/principalInvestigatorSecondFamilyName")]
		public  string Roh_principalInvestigatorSecondFamilyName { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("http://w3id.org/roh/technologicalExpert")]
		public  bool Roh_technologicalExpert { get; set;}

		[RDFProperty("http://w3id.org/roh/coprincipalInvestigatorSecondFamilyName")]
		public  string Roh_coprincipalInvestigatorSecondFamilyName { get; set;}

		[RDFProperty("http://w3id.org/roh/principalInvestigatorFirstName")]
		public  string Roh_principalInvestigatorFirstName { get; set;}

		[RDFProperty("http://w3id.org/roh/coprincipalInvestigatorNick")]
		public  string Roh_coprincipalInvestigatorNick { get; set;}

		[RDFProperty("http://w3id.org/roh/newTechniques")]
		public  bool Roh_newTechniques { get; set;}

		[RDFProperty("http://w3id.org/roh/principalInvestigatorFamilyName")]
		public  string Roh_principalInvestigatorFamilyName { get; set;}

		[RDFProperty("http://w3id.org/roh/durationMonths")]
		public  float? Roh_durationMonths { get; set;}

		[RDFProperty("http://w3id.org/roh/collaborationAgreements")]
		public  bool Roh_collaborationAgreements { get; set;}

		[RDFProperty("http://w3id.org/roh/durationYears")]
		public  float? Roh_durationYears { get; set;}

		[RDFProperty("http://w3id.org/roh/coprincipalInvestigatorFirstName")]
		public  string Roh_coprincipalInvestigatorFirstName { get; set;}

		[RDFProperty("http://w3id.org/roh/coprincipalInvestigatorFamilyName")]
		public  string Roh_coprincipalInvestigatorFamilyName { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://w3id.org/roh/homologation")]
		public  bool Roh_homologation { get; set;}

		[RDFProperty("http://w3id.org/roh/contributionGradeOther")]
		public  string Roh_contributionGradeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/owner")]
		[Required]
		public  Person Roh_owner  { get; set;} 
		public string IdRoh_owner  { get; set;} 

		[RDFProperty("http://w3id.org/roh/cvnCode")]
		public  string Roh_cvnCode { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:contributionGrade", this.IdRoh_contributionGrade));
			propList.Add(new StringOntologyProperty("vivo:geographicFocus", this.IdVivo_geographicFocus));
			propList.Add(new StringOntologyProperty("roh:relevantResults", this.Roh_relevantResults));
			propList.Add(new StringOntologyProperty("roh:principalInvestigatorNick", this.Roh_principalInvestigatorNick));
			propList.Add(new BoolOntologyProperty("roh:spinoffCompanies", this.Roh_spinoffCompanies));
			propList.Add(new BoolOntologyProperty("roh:activityResults", this.Roh_activityResults));
			propList.Add(new StringOntologyProperty("roh:geographicFocusOther", this.Roh_geographicFocusOther));
			propList.Add(new StringOntologyProperty("roh:durationDays", this.Roh_durationDays.ToString()));
			propList.Add(new StringOntologyProperty("roh:principalInvestigatorSecondFamilyName", this.Roh_principalInvestigatorSecondFamilyName));
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new BoolOntologyProperty("roh:technologicalExpert", this.Roh_technologicalExpert));
			propList.Add(new StringOntologyProperty("roh:coprincipalInvestigatorSecondFamilyName", this.Roh_coprincipalInvestigatorSecondFamilyName));
			propList.Add(new StringOntologyProperty("roh:principalInvestigatorFirstName", this.Roh_principalInvestigatorFirstName));
			propList.Add(new StringOntologyProperty("roh:coprincipalInvestigatorNick", this.Roh_coprincipalInvestigatorNick));
			propList.Add(new BoolOntologyProperty("roh:newTechniques", this.Roh_newTechniques));
			propList.Add(new StringOntologyProperty("roh:principalInvestigatorFamilyName", this.Roh_principalInvestigatorFamilyName));
			propList.Add(new StringOntologyProperty("roh:durationMonths", this.Roh_durationMonths.ToString()));
			propList.Add(new BoolOntologyProperty("roh:collaborationAgreements", this.Roh_collaborationAgreements));
			propList.Add(new StringOntologyProperty("roh:durationYears", this.Roh_durationYears.ToString()));
			propList.Add(new StringOntologyProperty("roh:coprincipalInvestigatorFirstName", this.Roh_coprincipalInvestigatorFirstName));
			propList.Add(new StringOntologyProperty("roh:coprincipalInvestigatorFamilyName", this.Roh_coprincipalInvestigatorFamilyName));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			propList.Add(new BoolOntologyProperty("roh:homologation", this.Roh_homologation));
			propList.Add(new StringOntologyProperty("roh:contributionGradeOther", this.Roh_contributionGradeOther));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_unescoPrimary!=null){
				foreach(CategoryPath prop in Roh_unescoPrimary){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:unescoPrimary", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
			if(Roh_targetOrganizations!=null){
				foreach(Organization prop in Roh_targetOrganizations){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityOrganization = new OntologyEntity("http://w3id.org/roh/Organization", "http://w3id.org/roh/Organization", "roh:targetOrganizations", prop.propList, prop.entList);
				entList.Add(entityOrganization);
				prop.Entity= entityOrganization;
				}
			}
			if(Roh_unescoTertiary!=null){
				foreach(CategoryPath prop in Roh_unescoTertiary){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:unescoTertiary", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
			if(Roh_unescoSecondary!=null){
				foreach(CategoryPath prop in Roh_unescoSecondary){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:unescoSecondary", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
			if(Vivo_freeTextKeywords!=null){
				foreach(CategoryPath prop in Vivo_freeTextKeywords){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "vivo:freeTextKeywords", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
			if(Roh_participates!=null){
				foreach(Organization prop in Roh_participates){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityOrganization = new OntologyEntity("http://w3id.org/roh/Organization", "http://w3id.org/roh/Organization", "roh:participates", prop.propList, prop.entList);
				entList.Add(entityOrganization);
				prop.Entity= entityOrganization;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/TechnologicalResult>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/TechnologicalResult\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_unescoPrimary != null)
			{
			foreach(var item0 in this.Roh_unescoPrimary)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}", "http://w3id.org/roh/unescoPrimary", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_unescoTertiary != null)
			{
			foreach(var item0 in this.Roh_unescoTertiary)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}", "http://w3id.org/roh/unescoTertiary", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_unescoSecondary != null)
			{
			foreach(var item0 in this.Roh_unescoSecondary)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}", "http://w3id.org/roh/unescoSecondary", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Vivo_freeTextKeywords != null)
			{
			foreach(var item0 in this.Vivo_freeTextKeywords)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#freeTextKeywords", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_targetOrganizations != null)
			{
			foreach(var item0 in this.Roh_targetOrganizations)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Organization>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Organization\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}", "http://w3id.org/roh/targetOrganizations", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{item0.IdVcard_hasCountryName}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{item0.IdRoh_organizationType}>", list, " . ");
				}
				if(item0.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{item0.IdVcard_hasRegion}>", list, " . ");
				}
				if(item0.IdRoh_organization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{item0.IdRoh_organization}>", list, " . ");
				}
				if(item0.Vcard_locality != null)
				{
					foreach(var item2 in item0.Vcard_locality)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther)}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle)}\"", list, " . ");
				}
			}
			}
			if(this.Roh_participates != null)
			{
			foreach(var item0 in this.Roh_participates)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Organization>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Organization\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}", "http://w3id.org/roh/participates", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{item0.IdVcard_hasCountryName}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{item0.IdRoh_organizationType}>", list, " . ");
				}
				if(item0.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{item0.IdVcard_hasRegion}>", list, " . ");
				}
				if(item0.IdRoh_organization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{item0.IdRoh_organization}>", list, " . ");
				}
				if(item0.Vcard_locality != null)
				{
					foreach(var item2 in item0.Vcard_locality)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther)}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle)}\"", list, " . ");
				}
			}
			}
				if(this.IdRoh_contributionGrade != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/contributionGrade", $"<{this.IdRoh_contributionGrade}>", list, " . ");
				}
				if(this.IdVivo_geographicFocus != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#geographicFocus", $"<{this.IdVivo_geographicFocus}>", list, " . ");
				}
				if(this.Roh_relevantResults != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/relevantResults", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_relevantResults)}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorNick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/principalInvestigatorNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorNick)}\"", list, " . ");
				}
				if(this.Roh_spinoffCompanies != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/spinoffCompanies", $"\"{this.Roh_spinoffCompanies.ToString()}\"", list, " . ");
				}
				if(this.Roh_activityResults != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/activityResults", $"\"{this.Roh_activityResults.ToString()}\"", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther)}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationDays", $"{this.Roh_durationDays.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_principalInvestigatorSecondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/principalInvestigatorSecondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorSecondFamilyName)}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_technologicalExpert != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/technologicalExpert", $"\"{this.Roh_technologicalExpert.ToString()}\"", list, " . ");
				}
				if(this.Roh_coprincipalInvestigatorSecondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/coprincipalInvestigatorSecondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_coprincipalInvestigatorSecondFamilyName)}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorFirstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/principalInvestigatorFirstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorFirstName)}\"", list, " . ");
				}
				if(this.Roh_coprincipalInvestigatorNick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/coprincipalInvestigatorNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_coprincipalInvestigatorNick)}\"", list, " . ");
				}
				if(this.Roh_newTechniques != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/newTechniques", $"\"{this.Roh_newTechniques.ToString()}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/principalInvestigatorFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorFamilyName)}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationMonths", $"{this.Roh_durationMonths.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_collaborationAgreements != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/collaborationAgreements", $"\"{this.Roh_collaborationAgreements.ToString()}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationYears", $"{this.Roh_durationYears.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_coprincipalInvestigatorFirstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/coprincipalInvestigatorFirstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_coprincipalInvestigatorFirstName)}\"", list, " . ");
				}
				if(this.Roh_coprincipalInvestigatorFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/coprincipalInvestigatorFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_coprincipalInvestigatorFamilyName)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Roh_homologation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/homologation", $"\"{this.Roh_homologation.ToString()}\"", list, " . ");
				}
				if(this.Roh_contributionGradeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/contributionGradeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_contributionGradeOther)}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TechnologicalResult_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"technologicalresult\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/TechnologicalResult\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_unescoPrimary != null)
			{
			foreach(var item0 in this.Roh_unescoPrimary)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/unescoPrimary", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_unescoTertiary != null)
			{
			foreach(var item0 in this.Roh_unescoTertiary)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/unescoTertiary", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_unescoSecondary != null)
			{
			foreach(var item0 in this.Roh_unescoSecondary)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/unescoSecondary", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Vivo_freeTextKeywords != null)
			{
			foreach(var item0 in this.Vivo_freeTextKeywords)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://vivoweb.org/ontology/core#freeTextKeywords", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_targetOrganizations != null)
			{
			foreach(var item0 in this.Roh_targetOrganizations)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/targetOrganizations", $"<{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdVcard_hasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdVcard_hasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_organizationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdVcard_hasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdVcard_hasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdRoh_organization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_organization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Vcard_locality != null)
				{
					foreach(var item2 in item0.Vcard_locality)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}", "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Roh_participates != null)
			{
			foreach(var item0 in this.Roh_participates)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/participates", $"<{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdVcard_hasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdVcard_hasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_organizationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdVcard_hasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdVcard_hasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdRoh_organization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_organization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Vcard_locality != null)
				{
					foreach(var item2 in item0.Vcard_locality)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}", "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle).ToLower()}\"", list, " . ");
				}
			}
			}
				if(this.IdRoh_contributionGrade != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_contributionGrade;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/contributionGrade", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdVivo_geographicFocus != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVivo_geographicFocus;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#geographicFocus", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_relevantResults != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/relevantResults", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_relevantResults).ToLower()}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorNick != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/principalInvestigatorNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorNick).ToLower()}\"", list, " . ");
				}
				if(this.Roh_spinoffCompanies != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/spinoffCompanies", $"\"{this.Roh_spinoffCompanies.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_activityResults != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/activityResults", $"\"{this.Roh_activityResults.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationDays", $"{this.Roh_durationDays.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_principalInvestigatorSecondFamilyName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/principalInvestigatorSecondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorSecondFamilyName).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#start", $"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_technologicalExpert != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/technologicalExpert", $"\"{this.Roh_technologicalExpert.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_coprincipalInvestigatorSecondFamilyName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/coprincipalInvestigatorSecondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_coprincipalInvestigatorSecondFamilyName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorFirstName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/principalInvestigatorFirstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorFirstName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_coprincipalInvestigatorNick != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/coprincipalInvestigatorNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_coprincipalInvestigatorNick).ToLower()}\"", list, " . ");
				}
				if(this.Roh_newTechniques != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/newTechniques", $"\"{this.Roh_newTechniques.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorFamilyName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/principalInvestigatorFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorFamilyName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationMonths", $"{this.Roh_durationMonths.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_collaborationAgreements != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/collaborationAgreements", $"\"{this.Roh_collaborationAgreements.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationYears", $"{this.Roh_durationYears.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_coprincipalInvestigatorFirstName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/coprincipalInvestigatorFirstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_coprincipalInvestigatorFirstName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_coprincipalInvestigatorFamilyName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/coprincipalInvestigatorFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_coprincipalInvestigatorFamilyName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Roh_homologation != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/homologation", $"\"{this.Roh_homologation.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_contributionGradeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/contributionGradeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_contributionGradeOther).ToLower()}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_owner;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/owner", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode).ToLower()}\"", list, " . ");
				}
				if(!string.IsNullOrEmpty(this.Roh_title))
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title).ToLower()}\"", list, " . ");
					search += $"{this.Roh_title} ";
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
			string titulo = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/TechnologicalresultOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Roh_title;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Roh_title;
		}




	}
}
