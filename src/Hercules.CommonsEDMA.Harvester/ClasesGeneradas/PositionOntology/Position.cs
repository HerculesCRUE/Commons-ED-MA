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
using Feature = FeatureOntology.Feature;
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using DedicationRegime = DedicationregimeOntology.DedicationRegime;
using Organization = OrganizationOntology.Organization;
using Person = PersonOntology.Person;
using ScopeManagementActivity = ScopemanagementactivityOntology.ScopeManagementActivity;
using ContractModality = ContractmodalityOntology.ContractModality;

namespace PositionOntology
{
	[ExcludeFromCodeCoverage]
	public class Position : GnossOCBase
	{

		public Position() : base() { } 

		public Position(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
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
			SemanticPropertyModel propRoh_employerOrganizationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/employerOrganizationType");
			if(propRoh_employerOrganizationType != null && propRoh_employerOrganizationType.PropertyValues.Count > 0)
			{
				this.Roh_employerOrganizationType = new OrganizationType(propRoh_employerOrganizationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_dedication = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dedication");
			if(propRoh_dedication != null && propRoh_dedication.PropertyValues.Count > 0)
			{
				this.Roh_dedication = new DedicationRegime(propRoh_dedication.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_hasFax = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasFax");
			if(propRoh_hasFax != null && propRoh_hasFax.PropertyValues.Count > 0)
			{
				this.Roh_hasFax = new TelephoneType(propRoh_hasFax.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_employerOrganization = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/employerOrganization");
			if(propRoh_employerOrganization != null && propRoh_employerOrganization.PropertyValues.Count > 0)
			{
				this.Roh_employerOrganization = new Organization(propRoh_employerOrganization.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_scopeManagementActivity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scopeManagementActivity");
			if(propRoh_scopeManagementActivity != null && propRoh_scopeManagementActivity.PropertyValues.Count > 0)
			{
				this.Roh_scopeManagementActivity = new ScopeManagementActivity(propRoh_scopeManagementActivity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_contractModality = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contractModality");
			if(propRoh_contractModality != null && propRoh_contractModality.PropertyValues.Count > 0)
			{
				this.Roh_contractModality = new ContractModality(propRoh_contractModality.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasTelephone = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasTelephone");
			if(propVcard_hasTelephone != null && propVcard_hasTelephone.PropertyValues.Count > 0)
			{
				this.Vcard_hasTelephone = new TelephoneType(propVcard_hasTelephone.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Vivo_freeTextKeyword = new List<CategoryPath>();
			SemanticPropertyModel propVivo_freeTextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeyword");
			if(propVivo_freeTextKeyword != null && propVivo_freeTextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeyword.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath vivo_freeTextKeyword = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_freeTextKeyword.Add(vivo_freeTextKeyword);
					}
				}
			}
			this.Roh_professionalCategory = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/professionalCategory"));
			this.Roh_contractModalityOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contractModalityOther"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_teachingManagement= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingManagement"));
			this.Roh_employerOrganizationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/employerOrganizationTitle"));
			this.Roh_employerOrganizationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/employerOrganizationTypeOther"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_scopeManagementActivityOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scopeManagementActivityOther"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_concreteFunctions = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/concreteFunctions"));
			SemanticPropertyModel propVcard_email = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#email");
			this.Vcard_email = new List<string>();
			if (propVcard_email != null && propVcard_email.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVcard_email.PropertyValues)
				{
					this.Vcard_email.Add(propValue.Value);
				}
			}
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			SemanticPropertyModel propRoh_center = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/center");
			this.Roh_center = new List<string>();
			if (propRoh_center != null && propRoh_center.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_center.PropertyValues)
				{
					this.Roh_center.Add(propValue.Value);
				}
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Bibo_abstract = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/abstract"));
			SemanticPropertyModel propRoh_department = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/department");
			this.Roh_department = new List<string>();
			if (propRoh_department != null && propRoh_department.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_department.PropertyValues)
				{
					this.Roh_department.Add(propValue.Value);
				}
			}
		}

		public Position(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
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
			SemanticPropertyModel propRoh_employerOrganizationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/employerOrganizationType");
			if(propRoh_employerOrganizationType != null && propRoh_employerOrganizationType.PropertyValues.Count > 0)
			{
				this.Roh_employerOrganizationType = new OrganizationType(propRoh_employerOrganizationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_dedication = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dedication");
			if(propRoh_dedication != null && propRoh_dedication.PropertyValues.Count > 0)
			{
				this.Roh_dedication = new DedicationRegime(propRoh_dedication.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_hasFax = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasFax");
			if(propRoh_hasFax != null && propRoh_hasFax.PropertyValues.Count > 0)
			{
				this.Roh_hasFax = new TelephoneType(propRoh_hasFax.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_employerOrganization = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/employerOrganization");
			if(propRoh_employerOrganization != null && propRoh_employerOrganization.PropertyValues.Count > 0)
			{
				this.Roh_employerOrganization = new Organization(propRoh_employerOrganization.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_scopeManagementActivity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scopeManagementActivity");
			if(propRoh_scopeManagementActivity != null && propRoh_scopeManagementActivity.PropertyValues.Count > 0)
			{
				this.Roh_scopeManagementActivity = new ScopeManagementActivity(propRoh_scopeManagementActivity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_contractModality = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contractModality");
			if(propRoh_contractModality != null && propRoh_contractModality.PropertyValues.Count > 0)
			{
				this.Roh_contractModality = new ContractModality(propRoh_contractModality.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasTelephone = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasTelephone");
			if(propVcard_hasTelephone != null && propVcard_hasTelephone.PropertyValues.Count > 0)
			{
				this.Vcard_hasTelephone = new TelephoneType(propVcard_hasTelephone.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Vivo_freeTextKeyword = new List<CategoryPath>();
			SemanticPropertyModel propVivo_freeTextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeyword");
			if(propVivo_freeTextKeyword != null && propVivo_freeTextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeyword.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath vivo_freeTextKeyword = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_freeTextKeyword.Add(vivo_freeTextKeyword);
					}
				}
			}
			this.Roh_professionalCategory = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/professionalCategory"));
			this.Roh_contractModalityOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contractModalityOther"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_teachingManagement= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingManagement"));
			this.Roh_employerOrganizationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/employerOrganizationTitle"));
			this.Roh_employerOrganizationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/employerOrganizationTypeOther"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_scopeManagementActivityOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scopeManagementActivityOther"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_concreteFunctions = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/concreteFunctions"));
			SemanticPropertyModel propVcard_email = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#email");
			this.Vcard_email = new List<string>();
			if (propVcard_email != null && propVcard_email.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVcard_email.PropertyValues)
				{
					this.Vcard_email.Add(propValue.Value);
				}
			}
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			SemanticPropertyModel propRoh_center = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/center");
			this.Roh_center = new List<string>();
			if (propRoh_center != null && propRoh_center.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_center.PropertyValues)
				{
					this.Roh_center.Add(propValue.Value);
				}
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Bibo_abstract = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/abstract"));
			SemanticPropertyModel propRoh_department = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/department");
			this.Roh_department = new List<string>();
			if (propRoh_department != null && propRoh_department.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_department.PropertyValues)
				{
					this.Roh_department.Add(propValue.Value);
				}
			}
		}

		public virtual string RdfType { get { return "http://vivoweb.org/ontology/core#Position"; } }
		public virtual string RdfsLabel { get { return "http://vivoweb.org/ontology/core#Position"; } }
		[LABEL(LanguageEnum.es,"hasCountryName")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/unescoPrimary")]
		public  List<CategoryPath> Roh_unescoPrimary { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/employerOrganizationType")]
		[RDFProperty("http://w3id.org/roh/employerOrganizationType")]
		public  OrganizationType Roh_employerOrganizationType  { get; set;} 
		public string IdRoh_employerOrganizationType  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/dedicationRegime")]
		[RDFProperty("http://w3id.org/roh/dedication")]
		public  DedicationRegime Roh_dedication  { get; set;} 
		public string IdRoh_dedication  { get; set;} 

		[RDFProperty("http://w3id.org/roh/hasFax")]
		public  TelephoneType Roh_hasFax { get; set;}

		[LABEL(LanguageEnum.es,"employerOrganization")]
		[RDFProperty("http://w3id.org/roh/employerOrganization")]
		public  Organization Roh_employerOrganization  { get; set;} 
		public string IdRoh_employerOrganization  { get; set;} 

		[RDFProperty("http://w3id.org/roh/unescoTertiary")]
		public  List<CategoryPath> Roh_unescoTertiary { get; set;}

		[RDFProperty("http://w3id.org/roh/unescoSecondary")]
		public  List<CategoryPath> Roh_unescoSecondary { get; set;}

		[RDFProperty("http://w3id.org/roh/owner")]
		public  Person Roh_owner  { get; set;} 
		public string IdRoh_owner  { get; set;} 

		[LABEL(LanguageEnum.es,"hasRegion")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/scopeManagementActivity")]
		[RDFProperty("http://w3id.org/roh/scopeManagementActivity")]
		public  ScopeManagementActivity Roh_scopeManagementActivity  { get; set;} 
		public string IdRoh_scopeManagementActivity  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/contractModality")]
		[RDFProperty("http://w3id.org/roh/contractModality")]
		public  ContractModality Roh_contractModality  { get; set;} 
		public string IdRoh_contractModality  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasTelephone")]
		public  TelephoneType Vcard_hasTelephone { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#freeTextKeyword")]
		public  List<CategoryPath> Vivo_freeTextKeyword { get; set;}

		[RDFProperty("http://w3id.org/roh/professionalCategory")]
		public  string Roh_professionalCategory { get; set;}

		[RDFProperty("http://w3id.org/roh/contractModalityOther")]
		public  string Roh_contractModalityOther { get; set;}

		[RDFProperty("http://w3id.org/roh/durationMonths")]
		public  string Roh_durationMonths { get; set;}

		[RDFProperty("http://w3id.org/roh/teachingManagement")]
		public  bool Roh_teachingManagement { get; set;}

		[RDFProperty("http://w3id.org/roh/employerOrganizationTitle")]
		public  string Roh_employerOrganizationTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/employerOrganizationTypeOther")]
		public  string Roh_employerOrganizationTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/durationDays")]
		public  string Roh_durationDays { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("http://w3id.org/roh/scopeManagementActivityOther")]
		public  string Roh_scopeManagementActivityOther { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/durationYears")]
		public  string Roh_durationYears { get; set;}

		[RDFProperty("http://w3id.org/roh/concreteFunctions")]
		public  string Roh_concreteFunctions { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#email")]
		public  List<string> Vcard_email { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#end")]
		public  DateTime? Vivo_end { get; set;}

		[RDFProperty("http://w3id.org/roh/center")]
		public  List<string> Roh_center { get; set;}

		[RDFProperty("http://w3id.org/roh/cvnCode")]
		public  string Roh_cvnCode { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/abstract")]
		public  string Bibo_abstract { get; set;}

		[RDFProperty("http://w3id.org/roh/department")]
		public  List<string> Roh_department { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:employerOrganizationType", this.IdRoh_employerOrganizationType));
			propList.Add(new StringOntologyProperty("roh:dedication", this.IdRoh_dedication));
			propList.Add(new StringOntologyProperty("roh:employerOrganization", this.IdRoh_employerOrganization));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("roh:scopeManagementActivity", this.IdRoh_scopeManagementActivity));
			propList.Add(new StringOntologyProperty("roh:contractModality", this.IdRoh_contractModality));
			propList.Add(new StringOntologyProperty("roh:professionalCategory", this.Roh_professionalCategory));
			propList.Add(new StringOntologyProperty("roh:contractModalityOther", this.Roh_contractModalityOther));
			propList.Add(new StringOntologyProperty("roh:durationMonths", this.Roh_durationMonths));
			propList.Add(new BoolOntologyProperty("roh:teachingManagement", this.Roh_teachingManagement));
			propList.Add(new StringOntologyProperty("roh:employerOrganizationTitle", this.Roh_employerOrganizationTitle));
			propList.Add(new StringOntologyProperty("roh:employerOrganizationTypeOther", this.Roh_employerOrganizationTypeOther));
			propList.Add(new StringOntologyProperty("roh:durationDays", this.Roh_durationDays));
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new StringOntologyProperty("roh:scopeManagementActivityOther", this.Roh_scopeManagementActivityOther));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("roh:durationYears", this.Roh_durationYears));
			propList.Add(new StringOntologyProperty("roh:concreteFunctions", this.Roh_concreteFunctions));
			propList.Add(new ListStringOntologyProperty("vcard:email", this.Vcard_email));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			if (this.Vivo_end.HasValue){
				propList.Add(new DateOntologyProperty("vivo:end", this.Vivo_end.Value));
				}
			propList.Add(new ListStringOntologyProperty("roh:center", this.Roh_center));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("bibo:abstract", this.Bibo_abstract));
			propList.Add(new ListStringOntologyProperty("roh:department", this.Roh_department));
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
			if(Roh_hasFax!=null){
				Roh_hasFax.GetProperties();
				Roh_hasFax.GetEntities();
				OntologyEntity entityRoh_hasFax = new OntologyEntity("https://www.w3.org/2006/vcard/ns#TelephoneType", "https://www.w3.org/2006/vcard/ns#TelephoneType", "roh:hasFax", Roh_hasFax.propList, Roh_hasFax.entList);
				entList.Add(entityRoh_hasFax);
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
			if(Vcard_hasTelephone!=null){
				Vcard_hasTelephone.GetProperties();
				Vcard_hasTelephone.GetEntities();
				OntologyEntity entityVcard_hasTelephone = new OntologyEntity("https://www.w3.org/2006/vcard/ns#TelephoneType", "https://www.w3.org/2006/vcard/ns#TelephoneType", "vcard:hasTelephone", Vcard_hasTelephone.propList, Vcard_hasTelephone.entList);
				entList.Add(entityVcard_hasTelephone);
			}
			if(Vivo_freeTextKeyword!=null){
				foreach(CategoryPath prop in Vivo_freeTextKeyword){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "vivo:freeTextKeyword", prop.propList, prop.entList);
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://vivoweb.org/ontology/core#Position>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://vivoweb.org/ontology/core#Position\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_unescoPrimary != null)
			{
			foreach(var item0 in this.Roh_unescoPrimary)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "http://w3id.org/roh/unescoPrimary", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "http://w3id.org/roh/unescoTertiary", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "http://w3id.org/roh/unescoSecondary", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Vivo_freeTextKeyword != null)
			{
			foreach(var item0 in this.Vivo_freeTextKeyword)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_hasFax != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_hasFax.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://www.w3.org/2006/vcard/ns#TelephoneType>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_hasFax.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://www.w3.org/2006/vcard/ns#TelephoneType\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_hasFax.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "http://w3id.org/roh/hasFax", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_hasFax.ArticleID}>", list, " . ");
				if(this.Roh_hasFax.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_hasFax.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasFax.Roh_hasExtension)}\"", list, " . ");
				}
				if(this.Roh_hasFax.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_hasFax.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasFax.Roh_hasInternationalCode)}\"", list, " . ");
				}
				if(this.Roh_hasFax.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_hasFax.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasFax.Vcard_hasValue)}\"", list, " . ");
				}
			}
			if(this.Vcard_hasTelephone != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://www.w3.org/2006/vcard/ns#TelephoneType>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://www.w3.org/2006/vcard/ns#TelephoneType\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "https://www.w3.org/2006/vcard/ns#hasTelephone", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}>", list, " . ");
				if(this.Vcard_hasTelephone.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_hasTelephone.Roh_hasExtension)}\"", list, " . ");
				}
				if(this.Vcard_hasTelephone.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_hasTelephone.Roh_hasInternationalCode)}\"", list, " . ");
				}
				if(this.Vcard_hasTelephone.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_hasTelephone.Vcard_hasValue)}\"", list, " . ");
				}
			}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_employerOrganizationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/employerOrganizationType", $"<{this.IdRoh_employerOrganizationType}>", list, " . ");
				}
				if(this.IdRoh_dedication != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/dedication", $"<{this.IdRoh_dedication}>", list, " . ");
				}
				if(this.IdRoh_employerOrganization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/employerOrganization", $"<{this.IdRoh_employerOrganization}>", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdRoh_scopeManagementActivity != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/scopeManagementActivity", $"<{this.IdRoh_scopeManagementActivity}>", list, " . ");
				}
				if(this.IdRoh_contractModality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/contractModality", $"<{this.IdRoh_contractModality}>", list, " . ");
				}
				if(this.Roh_professionalCategory != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/professionalCategory", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_professionalCategory)}\"", list, " . ");
				}
				if(this.Roh_contractModalityOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/contractModalityOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_contractModalityOther)}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths)}\"", list, " . ");
				}
				if(this.Roh_teachingManagement != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/teachingManagement", $"\"{this.Roh_teachingManagement.ToString()}\"", list, " . ");
				}
				if(this.Roh_employerOrganizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/employerOrganizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_employerOrganizationTitle)}\"", list, " . ");
				}
				if(this.Roh_employerOrganizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/employerOrganizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_employerOrganizationTypeOther)}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays)}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_scopeManagementActivityOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/scopeManagementActivityOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_scopeManagementActivityOther)}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears)}\"", list, " . ");
				}
				if(this.Roh_concreteFunctions != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/concreteFunctions", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_concreteFunctions)}\"", list, " . ");
				}
				if(this.Vcard_email != null)
				{
					foreach(var item2 in this.Vcard_email)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "https://www.w3.org/2006/vcard/ns#email", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#end", $"\"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_center != null)
				{
					foreach(var item2 in this.Roh_center)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "http://w3id.org/roh/center", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.Bibo_abstract != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/abstract", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_abstract)}\"", list, " . ");
				}
				if(this.Roh_department != null)
				{
					foreach(var item2 in this.Roh_department)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Position_{ResourceID}_{ArticleID}", "http://w3id.org/roh/department", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"position\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://vivoweb.org/ontology/core#Position\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_employerOrganizationTitle)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_employerOrganizationTitle)}\"", list, " . ");
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
			if(this.Vivo_freeTextKeyword != null)
			{
			foreach(var item0 in this.Vivo_freeTextKeyword)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
			if(this.Roh_hasFax != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/hasFax", $"<{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_hasFax.ArticleID}>", list, " . ");
				if(this.Roh_hasFax.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_hasFax.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasFax.Roh_hasExtension).ToLower()}\"", list, " . ");
				}
				if(this.Roh_hasFax.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_hasFax.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasFax.Roh_hasInternationalCode).ToLower()}\"", list, " . ");
				}
				if(this.Roh_hasFax.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_hasFax.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasFax.Vcard_hasValue).ToLower()}\"", list, " . ");
				}
			}
			if(this.Vcard_hasTelephone != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://www.w3.org/2006/vcard/ns#hasTelephone", $"<{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}>", list, " . ");
				if(this.Vcard_hasTelephone.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_hasTelephone.Roh_hasExtension).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_hasTelephone.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_hasTelephone.Roh_hasInternationalCode).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_hasTelephone.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Vcard_hasTelephone.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_hasTelephone.Vcard_hasValue).ToLower()}\"", list, " . ");
				}
			}
				if(this.IdVcard_hasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVcard_hasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_employerOrganizationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_employerOrganizationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/employerOrganizationType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_dedication != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_dedication;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/dedication", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_employerOrganization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_employerOrganization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/employerOrganization", $"<{itemRegex}>", list, " . ");
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
				if(this.IdVcard_hasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVcard_hasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_scopeManagementActivity != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_scopeManagementActivity;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/scopeManagementActivity", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_contractModality != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_contractModality;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/contractModality", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_professionalCategory != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/professionalCategory", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_professionalCategory).ToLower()}\"", list, " . ");
				}
				if(this.Roh_contractModalityOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/contractModalityOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_contractModalityOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths).ToLower()}\"", list, " . ");
				}
				if(this.Roh_teachingManagement != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/teachingManagement", $"\"{this.Roh_teachingManagement.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_employerOrganizationTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/employerOrganizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_employerOrganizationTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_employerOrganizationTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/employerOrganizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_employerOrganizationTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#start", $"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_scopeManagementActivityOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/scopeManagementActivityOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_scopeManagementActivityOther).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears).ToLower()}\"", list, " . ");
				}
				if(this.Roh_concreteFunctions != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/concreteFunctions", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_concreteFunctions).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_email != null)
				{
					foreach(var item2 in this.Vcard_email)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://www.w3.org/2006/vcard/ns#email", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#end", $"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_center != null)
				{
					foreach(var item2 in this.Roh_center)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/center", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_abstract != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/abstract", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_abstract).ToLower()}\"", list, " . ");
				}
				if(this.Roh_department != null)
				{
					foreach(var item2 in this.Roh_department)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/department", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
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
			string titulo = $"{this.Roh_employerOrganizationTitle.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Roh_employerOrganizationTitle.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/PositionOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Roh_employerOrganizationTitle;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Roh_employerOrganizationTitle;
		}




	}
}
