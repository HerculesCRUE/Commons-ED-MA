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
using ResultType = ResulttypeOntology.ResultType;
using Feature = FeatureOntology.Feature;
using Organization = OrganizationOntology.Organization;
using IndustrialPropertyType = IndustrialpropertytypeOntology.IndustrialPropertyType;
using Person = PersonOntology.Person;

namespace PatentOntology
{
	[ExcludeFromCodeCoverage]
	public class Patent : GnossOCBase
	{

		public Patent() : base() { } 

		public Patent(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_results = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/results");
			if(propRoh_results != null && propRoh_results.PropertyValues.Count > 0)
			{
				this.Roh_results = new ResultType(propRoh_results.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_operatingCompanies = new List<Organization>();
			SemanticPropertyModel propRoh_operatingCompanies = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/operatingCompanies");
			if(propRoh_operatingCompanies != null && propRoh_operatingCompanies.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_operatingCompanies.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_operatingCompanies = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_operatingCompanies.Add(roh_operatingCompanies);
					}
				}
			}
			this.Bibo_authorList = new List<PersonAux>();
			SemanticPropertyModel propBibo_authorList = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/authorList");
			if(propBibo_authorList != null && propBibo_authorList.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_authorList.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PersonAux bibo_authorList = new PersonAux(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_authorList.Add(bibo_authorList);
					}
				}
			}
			this.Roh_operatingCountries = new List<Feature>();
			SemanticPropertyModel propRoh_operatingCountries = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/operatingCountries");
			if(propRoh_operatingCountries != null && propRoh_operatingCountries.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_operatingCountries.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Feature roh_operatingCountries = new Feature(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_operatingCountries.Add(roh_operatingCountries);
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
			SemanticPropertyModel propRoh_ownerOrganization = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/ownerOrganization");
			if(propRoh_ownerOrganization != null && propRoh_ownerOrganization.PropertyValues.Count > 0)
			{
				this.Roh_ownerOrganization = new Organization(propRoh_ownerOrganization.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_industrialPropertyType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/industrialPropertyType");
			if(propRoh_industrialPropertyType != null && propRoh_industrialPropertyType.PropertyValues.Count > 0)
			{
				this.Roh_industrialPropertyType = new IndustrialPropertyType(propRoh_industrialPropertyType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_spanishPatent= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/spanishPatent"));
			this.Roh_relevantResults = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relevantResults"));
			this.Roh_applicationNumber = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/applicationNumber"));
			this.Roh_knowHow= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/knowHow"));
			this.Roh_patentNumber = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/patentNumber"));
			this.Roh_industrialPropertyTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/industrialPropertyTypeOther"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
			this.Roh_exclusiveUse= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/exclusiveUse"));
			this.Roh_relatedRights= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relatedRights"));
			SemanticPropertyModel propRoh_products = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/products");
			this.Roh_products = new List<string>();
			if (propRoh_products != null && propRoh_products.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_products.PropertyValues)
				{
					this.Roh_products.Add(propValue.Value);
				}
			}
			this.Roh_referenceCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/referenceCode"));
			this.Roh_europeanPatent= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanPatent"));
			this.Roh_authorsRights= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/authorsRights"));
			this.Roh_tradeSecret= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/tradeSecret"));
			this.Roh_ownerOrganizationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/ownerOrganizationTitle"));
			this.Roh_qualityDescription = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualityDescription"));
			this.Roh_innovativeEnterprise= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/innovativeEnterprise"));
			this.Roh_internationalPatent= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/internationalPatent"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_licenses= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/licenses"));
			this.Roh_dateFiled = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dateFiled"));
			this.Roh_pctPatent= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/pctPatent"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
		}

		public Patent(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_results = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/results");
			if(propRoh_results != null && propRoh_results.PropertyValues.Count > 0)
			{
				this.Roh_results = new ResultType(propRoh_results.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_operatingCompanies = new List<Organization>();
			SemanticPropertyModel propRoh_operatingCompanies = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/operatingCompanies");
			if(propRoh_operatingCompanies != null && propRoh_operatingCompanies.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_operatingCompanies.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_operatingCompanies = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_operatingCompanies.Add(roh_operatingCompanies);
					}
				}
			}
			this.Bibo_authorList = new List<PersonAux>();
			SemanticPropertyModel propBibo_authorList = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/authorList");
			if(propBibo_authorList != null && propBibo_authorList.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_authorList.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PersonAux bibo_authorList = new PersonAux(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_authorList.Add(bibo_authorList);
					}
				}
			}
			this.Roh_operatingCountries = new List<Feature>();
			SemanticPropertyModel propRoh_operatingCountries = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/operatingCountries");
			if(propRoh_operatingCountries != null && propRoh_operatingCountries.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_operatingCountries.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Feature roh_operatingCountries = new Feature(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_operatingCountries.Add(roh_operatingCountries);
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
			SemanticPropertyModel propRoh_ownerOrganization = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/ownerOrganization");
			if(propRoh_ownerOrganization != null && propRoh_ownerOrganization.PropertyValues.Count > 0)
			{
				this.Roh_ownerOrganization = new Organization(propRoh_ownerOrganization.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_industrialPropertyType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/industrialPropertyType");
			if(propRoh_industrialPropertyType != null && propRoh_industrialPropertyType.PropertyValues.Count > 0)
			{
				this.Roh_industrialPropertyType = new IndustrialPropertyType(propRoh_industrialPropertyType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_spanishPatent= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/spanishPatent"));
			this.Roh_relevantResults = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relevantResults"));
			this.Roh_applicationNumber = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/applicationNumber"));
			this.Roh_knowHow= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/knowHow"));
			this.Roh_patentNumber = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/patentNumber"));
			this.Roh_industrialPropertyTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/industrialPropertyTypeOther"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
			this.Roh_exclusiveUse= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/exclusiveUse"));
			this.Roh_relatedRights= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relatedRights"));
			SemanticPropertyModel propRoh_products = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/products");
			this.Roh_products = new List<string>();
			if (propRoh_products != null && propRoh_products.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_products.PropertyValues)
				{
					this.Roh_products.Add(propValue.Value);
				}
			}
			this.Roh_referenceCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/referenceCode"));
			this.Roh_europeanPatent= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanPatent"));
			this.Roh_authorsRights= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/authorsRights"));
			this.Roh_tradeSecret= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/tradeSecret"));
			this.Roh_ownerOrganizationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/ownerOrganizationTitle"));
			this.Roh_qualityDescription = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualityDescription"));
			this.Roh_innovativeEnterprise= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/innovativeEnterprise"));
			this.Roh_internationalPatent= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/internationalPatent"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_licenses= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/licenses"));
			this.Roh_dateFiled = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dateFiled"));
			this.Roh_pctPatent= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/pctPatent"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
		}

		public virtual string RdfType { get { return "http://purl.org/ontology/bibo/Patent"; } }
		public virtual string RdfsLabel { get { return "http://purl.org/ontology/bibo/Patent"; } }
		[RDFProperty("http://w3id.org/roh/results")]
		public  ResultType Roh_results  { get; set;} 
		public string IdRoh_results  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/operatingCompanies")]
		public  List<Organization> Roh_operatingCompanies { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/authorList")]
		public  List<PersonAux> Bibo_authorList { get; set;}

		[RDFProperty("http://w3id.org/roh/operatingCountries")]
		public  List<Feature> Roh_operatingCountries { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#freeTextKeywords")]
		public  List<CategoryPath> Vivo_freeTextKeywords { get; set;}

		[RDFProperty("http://w3id.org/roh/ownerOrganization")]
		public  Organization Roh_ownerOrganization  { get; set;} 
		public string IdRoh_ownerOrganization  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/industrialPropertyType")]
		public  IndustrialPropertyType Roh_industrialPropertyType  { get; set;} 
		public string IdRoh_industrialPropertyType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/spanishPatent")]
		public  bool Roh_spanishPatent { get; set;}

		[RDFProperty("http://w3id.org/roh/relevantResults")]
		public  string Roh_relevantResults { get; set;}

		[RDFProperty("http://w3id.org/roh/applicationNumber")]
		public  string Roh_applicationNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/knowHow")]
		public  bool Roh_knowHow { get; set;}

		[RDFProperty("http://w3id.org/roh/patentNumber")]
		public  string Roh_patentNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/industrialPropertyTypeOther")]
		public  string Roh_industrialPropertyTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}

		[RDFProperty("http://w3id.org/roh/exclusiveUse")]
		public  bool Roh_exclusiveUse { get; set;}

		[RDFProperty("http://w3id.org/roh/relatedRights")]
		public  bool Roh_relatedRights { get; set;}

		[RDFProperty("http://w3id.org/roh/products")]
		public  List<string> Roh_products { get; set;}

		[RDFProperty("http://w3id.org/roh/referenceCode")]
		public  string Roh_referenceCode { get; set;}

		[RDFProperty("http://w3id.org/roh/europeanPatent")]
		public  bool Roh_europeanPatent { get; set;}

		[RDFProperty("http://w3id.org/roh/authorsRights")]
		public  bool Roh_authorsRights { get; set;}

		[RDFProperty("http://w3id.org/roh/tradeSecret")]
		public  bool Roh_tradeSecret { get; set;}

		[RDFProperty("http://w3id.org/roh/ownerOrganizationTitle")]
		public  string Roh_ownerOrganizationTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/qualityDescription")]
		public  string Roh_qualityDescription { get; set;}

		[RDFProperty("http://w3id.org/roh/innovativeEnterprise")]
		public  bool Roh_innovativeEnterprise { get; set;}

		[RDFProperty("http://w3id.org/roh/internationalPatent")]
		public  bool Roh_internationalPatent { get; set;}

		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime? Dct_issued { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://w3id.org/roh/licenses")]
		public  bool Roh_licenses { get; set;}

		[RDFProperty("http://w3id.org/roh/dateFiled")]
		public  DateTime? Roh_dateFiled { get; set;}

		[RDFProperty("http://w3id.org/roh/pctPatent")]
		public  bool Roh_pctPatent { get; set;}

		[RDFProperty("http://w3id.org/roh/owner")]
		[Required]
		public  Person Roh_owner  { get; set;} 
		public string IdRoh_owner  { get; set;} 

		[RDFProperty("http://w3id.org/roh/cvnCode")]
		public  string Roh_cvnCode { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:results", this.IdRoh_results));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:ownerOrganization", this.IdRoh_ownerOrganization));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("roh:industrialPropertyType", this.IdRoh_industrialPropertyType));
			propList.Add(new BoolOntologyProperty("roh:spanishPatent", this.Roh_spanishPatent));
			propList.Add(new StringOntologyProperty("roh:relevantResults", this.Roh_relevantResults));
			propList.Add(new StringOntologyProperty("roh:applicationNumber", this.Roh_applicationNumber));
			propList.Add(new BoolOntologyProperty("roh:knowHow", this.Roh_knowHow));
			propList.Add(new StringOntologyProperty("roh:patentNumber", this.Roh_patentNumber));
			propList.Add(new StringOntologyProperty("roh:industrialPropertyTypeOther", this.Roh_industrialPropertyTypeOther));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
			propList.Add(new BoolOntologyProperty("roh:exclusiveUse", this.Roh_exclusiveUse));
			propList.Add(new BoolOntologyProperty("roh:relatedRights", this.Roh_relatedRights));
			propList.Add(new ListStringOntologyProperty("roh:products", this.Roh_products));
			propList.Add(new StringOntologyProperty("roh:referenceCode", this.Roh_referenceCode));
			propList.Add(new BoolOntologyProperty("roh:europeanPatent", this.Roh_europeanPatent));
			propList.Add(new BoolOntologyProperty("roh:authorsRights", this.Roh_authorsRights));
			propList.Add(new BoolOntologyProperty("roh:tradeSecret", this.Roh_tradeSecret));
			propList.Add(new StringOntologyProperty("roh:ownerOrganizationTitle", this.Roh_ownerOrganizationTitle));
			propList.Add(new StringOntologyProperty("roh:qualityDescription", this.Roh_qualityDescription));
			propList.Add(new BoolOntologyProperty("roh:innovativeEnterprise", this.Roh_innovativeEnterprise));
			propList.Add(new BoolOntologyProperty("roh:internationalPatent", this.Roh_internationalPatent));
			if (this.Dct_issued.HasValue){
				propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued.Value));
				}
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			propList.Add(new BoolOntologyProperty("roh:licenses", this.Roh_licenses));
			if (this.Roh_dateFiled.HasValue){
				propList.Add(new DateOntologyProperty("roh:dateFiled", this.Roh_dateFiled.Value));
				}
			propList.Add(new BoolOntologyProperty("roh:pctPatent", this.Roh_pctPatent));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_operatingCompanies!=null){
				foreach(Organization prop in Roh_operatingCompanies){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityOrganization = new OntologyEntity("http://w3id.org/roh/Organization", "http://w3id.org/roh/Organization", "roh:operatingCompanies", prop.propList, prop.entList);
				entList.Add(entityOrganization);
				prop.Entity= entityOrganization;
				}
			}
			if(Bibo_authorList!=null){
				foreach(PersonAux prop in Bibo_authorList){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityPersonAux = new OntologyEntity("http://w3id.org/roh/PersonAux", "http://w3id.org/roh/PersonAux", "bibo:authorList", prop.propList, prop.entList);
				entList.Add(entityPersonAux);
				prop.Entity= entityPersonAux;
				}
			}
			if(Roh_operatingCountries!=null){
				foreach(Feature prop in Roh_operatingCountries){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityFeature = new OntologyEntity("http://w3id.org/roh/Feature", "http://w3id.org/roh/Feature", "roh:operatingCountries", prop.propList, prop.entList);
				entList.Add(entityFeature);
				prop.Entity= entityFeature;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://purl.org/ontology/bibo/Patent>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://purl.org/ontology/bibo/Patent\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_operatingCompanies != null)
			{
			foreach(var item0 in this.Roh_operatingCompanies)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Organization>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Organization\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}", "http://w3id.org/roh/operatingCompanies", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRoh_organization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{item0.IdRoh_organization}>", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle)}\"", list, " . ");
				}
			}
			}
			if(this.Bibo_authorList != null)
			{
			foreach(var item0 in this.Bibo_authorList)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/PersonAux>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/PersonAux\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}", "http://purl.org/ontology/bibo/authorList", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName)}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName)}\"", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick)}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName)}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
				}
			}
			}
			if(this.Roh_operatingCountries != null)
			{
			foreach(var item0 in this.Roh_operatingCountries)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Feature>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Feature\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}", "http://w3id.org/roh/operatingCountries", $"<{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{item0.ArticleID}", "https://www.w3.org/2006/vcard/ns#hasCountryName",  $"<{item0.IdVcard_hasCountryName}>", list, " . ");
				}
				if(item0.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{item0.IdVcard_hasRegion}>", list, " . ");
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#freeTextKeywords", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
				if(this.IdRoh_results != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/results", $"<{this.IdRoh_results}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_ownerOrganization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/ownerOrganization", $"<{this.IdRoh_ownerOrganization}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdRoh_industrialPropertyType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/industrialPropertyType", $"<{this.IdRoh_industrialPropertyType}>", list, " . ");
				}
				if(this.Roh_spanishPatent != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/spanishPatent", $"\"{this.Roh_spanishPatent.ToString()}\"", list, " . ");
				}
				if(this.Roh_relevantResults != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/relevantResults", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_relevantResults)}\"", list, " . ");
				}
				if(this.Roh_applicationNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/applicationNumber", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_applicationNumber)}\"", list, " . ");
				}
				if(this.Roh_knowHow != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/knowHow", $"\"{this.Roh_knowHow.ToString()}\"", list, " . ");
				}
				if(this.Roh_patentNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/patentNumber", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_patentNumber)}\"", list, " . ");
				}
				if(this.Roh_industrialPropertyTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/industrialPropertyTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_industrialPropertyTypeOther)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
				if(this.Roh_exclusiveUse != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/exclusiveUse", $"\"{this.Roh_exclusiveUse.ToString()}\"", list, " . ");
				}
				if(this.Roh_relatedRights != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/relatedRights", $"\"{this.Roh_relatedRights.ToString()}\"", list, " . ");
				}
				if(this.Roh_products != null)
				{
					foreach(var item2 in this.Roh_products)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}", "http://w3id.org/roh/products", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_referenceCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/referenceCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_referenceCode)}\"", list, " . ");
				}
				if(this.Roh_europeanPatent != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/europeanPatent", $"\"{this.Roh_europeanPatent.ToString()}\"", list, " . ");
				}
				if(this.Roh_authorsRights != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/authorsRights", $"\"{this.Roh_authorsRights.ToString()}\"", list, " . ");
				}
				if(this.Roh_tradeSecret != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/tradeSecret", $"\"{this.Roh_tradeSecret.ToString()}\"", list, " . ");
				}
				if(this.Roh_ownerOrganizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/ownerOrganizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_ownerOrganizationTitle)}\"", list, " . ");
				}
				if(this.Roh_qualityDescription != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/qualityDescription", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualityDescription)}\"", list, " . ");
				}
				if(this.Roh_innovativeEnterprise != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/innovativeEnterprise", $"\"{this.Roh_innovativeEnterprise.ToString()}\"", list, " . ");
				}
				if(this.Roh_internationalPatent != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/internationalPatent", $"\"{this.Roh_internationalPatent.ToString()}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Roh_licenses != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/licenses", $"\"{this.Roh_licenses.ToString()}\"", list, " . ");
				}
				if(this.Roh_dateFiled != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/dateFiled", $"\"{this.Roh_dateFiled.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_pctPatent != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/pctPatent", $"\"{this.Roh_pctPatent.ToString()}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Patent_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"patent\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://purl.org/ontology/bibo/Patent\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_operatingCompanies != null)
			{
			foreach(var item0 in this.Roh_operatingCompanies)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/operatingCompanies", $"<{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Bibo_authorList != null)
			{
			foreach(var item0 in this.Bibo_authorList)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/ontology/bibo/authorList", $"<{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName).ToLower()}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
				}
			}
			}
			if(this.Roh_operatingCountries != null)
			{
			foreach(var item0 in this.Roh_operatingCountries)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/operatingCountries", $"<{resourceAPI.GraphsUrl}items/feature_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/feature_{ResourceID}_{item0.ArticleID}", "https://www.w3.org/2006/vcard/ns#hasCountryName",  $"<{itemRegex}>", list, " . ");
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
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/feature_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_results != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_results;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/results", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_ownerOrganization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_ownerOrganization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/ownerOrganization", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_industrialPropertyType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_industrialPropertyType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/industrialPropertyType", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_spanishPatent != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/spanishPatent", $"\"{this.Roh_spanishPatent.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_relevantResults != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/relevantResults", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_relevantResults).ToLower()}\"", list, " . ");
				}
				if(this.Roh_applicationNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/applicationNumber", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_applicationNumber).ToLower()}\"", list, " . ");
				}
				if(this.Roh_knowHow != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/knowHow", $"\"{this.Roh_knowHow.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_patentNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/patentNumber", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_patentNumber).ToLower()}\"", list, " . ");
				}
				if(this.Roh_industrialPropertyTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/industrialPropertyTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_industrialPropertyTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title).ToLower()}\"", list, " . ");
				}
				if(this.Roh_exclusiveUse != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/exclusiveUse", $"\"{this.Roh_exclusiveUse.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_relatedRights != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/relatedRights", $"\"{this.Roh_relatedRights.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_products != null)
				{
					foreach(var item2 in this.Roh_products)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/products", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_referenceCode != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/referenceCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_referenceCode).ToLower()}\"", list, " . ");
				}
				if(this.Roh_europeanPatent != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/europeanPatent", $"\"{this.Roh_europeanPatent.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_authorsRights != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/authorsRights", $"\"{this.Roh_authorsRights.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_tradeSecret != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/tradeSecret", $"\"{this.Roh_tradeSecret.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_ownerOrganizationTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/ownerOrganizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_ownerOrganizationTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_qualityDescription != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/qualityDescription", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualityDescription).ToLower()}\"", list, " . ");
				}
				if(this.Roh_innovativeEnterprise != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/innovativeEnterprise", $"\"{this.Roh_innovativeEnterprise.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_internationalPatent != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/internationalPatent", $"\"{this.Roh_internationalPatent.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/terms/issued", $"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Roh_licenses != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/licenses", $"\"{this.Roh_licenses.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_dateFiled != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/dateFiled", $"{this.Roh_dateFiled.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_pctPatent != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/pctPatent", $"\"{this.Roh_pctPatent.ToString().ToLower()}\"", list, " . ");
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
			string titulo = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/PatentOntology_{ResourceID}_{ArticleID}";
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
