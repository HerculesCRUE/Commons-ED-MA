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
using GrantAim = GrantaimOntology.GrantAim;
using Organization = OrganizationOntology.Organization;
using Person = PersonOntology.Person;

namespace GrantOntology
{
	[ExcludeFromCodeCoverage]
	public class Grant : GnossOCBase
	{

		public Grant() : base() { } 

		public Grant(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_awardingEntityType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/awardingEntityType");
			if(propRoh_awardingEntityType != null && propRoh_awardingEntityType.PropertyValues.Count > 0)
			{
				this.Roh_awardingEntityType = new OrganizationType(propRoh_awardingEntityType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_aims = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/aims");
			if(propRoh_aims != null && propRoh_aims.PropertyValues.Count > 0)
			{
				this.Roh_aims = new GrantAim(propRoh_aims.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_entity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/entity");
			if(propRoh_entity != null && propRoh_entity.PropertyValues.Count > 0)
			{
				this.Roh_entity = new Organization(propRoh_entity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_awardingEntity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/awardingEntity");
			if(propRoh_awardingEntity != null && propRoh_awardingEntity.PropertyValues.Count > 0)
			{
				this.Roh_awardingEntity = new Organization(propRoh_awardingEntity.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			this.Roh_entityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/entityTitle"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_conferralDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conferralDate"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_aimsOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/aimsOther"));
			this.Roh_awardingEntityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/awardingEntityTitle"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_awardingEntityTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/awardingEntityTypeOther"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Roh_center = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/center"));
			this.Roh_monetaryAmount = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/monetaryAmount"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public Grant(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_awardingEntityType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/awardingEntityType");
			if(propRoh_awardingEntityType != null && propRoh_awardingEntityType.PropertyValues.Count > 0)
			{
				this.Roh_awardingEntityType = new OrganizationType(propRoh_awardingEntityType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_aims = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/aims");
			if(propRoh_aims != null && propRoh_aims.PropertyValues.Count > 0)
			{
				this.Roh_aims = new GrantAim(propRoh_aims.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_entity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/entity");
			if(propRoh_entity != null && propRoh_entity.PropertyValues.Count > 0)
			{
				this.Roh_entity = new Organization(propRoh_entity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_awardingEntity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/awardingEntity");
			if(propRoh_awardingEntity != null && propRoh_awardingEntity.PropertyValues.Count > 0)
			{
				this.Roh_awardingEntity = new Organization(propRoh_awardingEntity.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			this.Roh_entityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/entityTitle"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_conferralDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conferralDate"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_aimsOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/aimsOther"));
			this.Roh_awardingEntityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/awardingEntityTitle"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_awardingEntityTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/awardingEntityTypeOther"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Roh_center = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/center"));
			this.Roh_monetaryAmount = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/monetaryAmount"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://vivoweb.org/ontology/core#Grant"; } }
		public virtual string RdfsLabel { get { return "http://vivoweb.org/ontology/core#Grant"; } }
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/awardingEntityType")]
		public  OrganizationType Roh_awardingEntityType  { get; set;} 
		public string IdRoh_awardingEntityType  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/aims")]
		public  GrantAim Roh_aims  { get; set;} 
		public string IdRoh_aims  { get; set;} 

		[RDFProperty("http://w3id.org/roh/entity")]
		public  Organization Roh_entity  { get; set;} 
		public string IdRoh_entity  { get; set;} 

		[RDFProperty("http://w3id.org/roh/awardingEntity")]
		public  Organization Roh_awardingEntity  { get; set;} 
		public string IdRoh_awardingEntity  { get; set;} 

		[RDFProperty("http://vivoweb.org/ontology/core#freeTextKeyword")]
		public  List<CategoryPath> Vivo_freeTextKeyword { get; set;}

		[RDFProperty("http://w3id.org/roh/entityTitle")]
		public  string Roh_entityTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/durationMonths")]
		public  string Roh_durationMonths { get; set;}

		[RDFProperty("http://w3id.org/roh/durationDays")]
		public  string Roh_durationDays { get; set;}

		[RDFProperty("http://w3id.org/roh/conferralDate")]
		public  DateTime? Roh_conferralDate { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("http://w3id.org/roh/aimsOther")]
		public  string Roh_aimsOther { get; set;}

		[RDFProperty("http://w3id.org/roh/awardingEntityTitle")]
		public  string Roh_awardingEntityTitle { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/durationYears")]
		public  string Roh_durationYears { get; set;}

		[RDFProperty("http://w3id.org/roh/awardingEntityTypeOther")]
		public  string Roh_awardingEntityTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#end")]
		public  DateTime? Vivo_end { get; set;}

		[RDFProperty("http://w3id.org/roh/center")]
		public  string Roh_center { get; set;}

		[RDFProperty("http://w3id.org/roh/monetaryAmount")]
		public  float? Roh_monetaryAmount { get; set;}

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
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("roh:awardingEntityType", this.IdRoh_awardingEntityType));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:aims", this.IdRoh_aims));
			propList.Add(new StringOntologyProperty("roh:entity", this.IdRoh_entity));
			propList.Add(new StringOntologyProperty("roh:awardingEntity", this.IdRoh_awardingEntity));
			propList.Add(new StringOntologyProperty("roh:entityTitle", this.Roh_entityTitle));
			propList.Add(new StringOntologyProperty("roh:durationMonths", this.Roh_durationMonths));
			propList.Add(new StringOntologyProperty("roh:durationDays", this.Roh_durationDays));
			if (this.Roh_conferralDate.HasValue){
				propList.Add(new DateOntologyProperty("roh:conferralDate", this.Roh_conferralDate.Value));
				}
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new StringOntologyProperty("roh:aimsOther", this.Roh_aimsOther));
			propList.Add(new StringOntologyProperty("roh:awardingEntityTitle", this.Roh_awardingEntityTitle));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("roh:durationYears", this.Roh_durationYears));
			propList.Add(new StringOntologyProperty("roh:awardingEntityTypeOther", this.Roh_awardingEntityTypeOther));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			if (this.Vivo_end.HasValue){
				propList.Add(new DateOntologyProperty("vivo:end", this.Vivo_end.Value));
				}
			propList.Add(new StringOntologyProperty("roh:center", this.Roh_center));
			propList.Add(new StringOntologyProperty("roh:monetaryAmount", this.Roh_monetaryAmount.ToString()));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://vivoweb.org/ontology/core#Grant>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://vivoweb.org/ontology/core#Grant\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Vivo_freeTextKeyword != null)
			{
			foreach(var item0 in this.Vivo_freeTextKeyword)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdRoh_awardingEntityType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/awardingEntityType", $"<{this.IdRoh_awardingEntityType}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_aims != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/aims", $"<{this.IdRoh_aims}>", list, " . ");
				}
				if(this.IdRoh_entity != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/entity", $"<{this.IdRoh_entity}>", list, " . ");
				}
				if(this.IdRoh_awardingEntity != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/awardingEntity", $"<{this.IdRoh_awardingEntity}>", list, " . ");
				}
				if(this.Roh_entityTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/entityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_entityTitle)}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths)}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays)}\"", list, " . ");
				}
				if(this.Roh_conferralDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conferralDate", $"\"{this.Roh_conferralDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_aimsOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/aimsOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_aimsOther)}\"", list, " . ");
				}
				if(this.Roh_awardingEntityTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/awardingEntityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_awardingEntityTitle)}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears)}\"", list, " . ");
				}
				if(this.Roh_awardingEntityTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/awardingEntityTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_awardingEntityTypeOther)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#end", $"\"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_center != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/center", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_center)}\"", list, " . ");
				}
				if(this.Roh_monetaryAmount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/monetaryAmount", $"{this.Roh_monetaryAmount.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Grant_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"grant\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://vivoweb.org/ontology/core#Grant\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
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
				if(this.IdRoh_awardingEntityType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_awardingEntityType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/awardingEntityType", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_aims != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_aims;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/aims", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_entity != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_entity;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/entity", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_awardingEntity != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_awardingEntity;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/awardingEntity", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_entityTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/entityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_entityTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays).ToLower()}\"", list, " . ");
				}
				if(this.Roh_conferralDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/conferralDate", $"{this.Roh_conferralDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#start", $"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_aimsOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/aimsOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_aimsOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_awardingEntityTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/awardingEntityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_awardingEntityTitle).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears).ToLower()}\"", list, " . ");
				}
				if(this.Roh_awardingEntityTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/awardingEntityTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_awardingEntityTypeOther).ToLower()}\"", list, " . ");
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
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/center", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_center).ToLower()}\"", list, " . ");
				}
				if(this.Roh_monetaryAmount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/monetaryAmount", $"{this.Roh_monetaryAmount.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
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
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title).ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/GrantOntology_{ResourceID}_{ArticleID}";
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
