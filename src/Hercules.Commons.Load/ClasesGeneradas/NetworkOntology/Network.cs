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
using Organization = OrganizationOntology.Organization;
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using Person = PersonOntology.Person;

namespace NetworkOntology
{
	[ExcludeFromCodeCoverage]
	public class Network : GnossOCBase
	{

		public Network() : base() { } 

		public Network(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_selectionEntityHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityHasRegion");
			if(propRoh_selectionEntityHasRegion != null && propRoh_selectionEntityHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_selectionEntityHasRegion = new Feature(propRoh_selectionEntityHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_selectionEntityType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityType");
			if(propRoh_selectionEntityType != null && propRoh_selectionEntityType.PropertyValues.Count > 0)
			{
				this.Roh_selectionEntityType = new OrganizationType(propRoh_selectionEntityType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_selectionEntity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntity");
			if(propRoh_selectionEntity != null && propRoh_selectionEntity.PropertyValues.Count > 0)
			{
				this.Roh_selectionEntity = new Organization(propRoh_selectionEntity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_selectionEntityHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityHasCountryName");
			if(propRoh_selectionEntityHasCountryName != null && propRoh_selectionEntityHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_selectionEntityHasCountryName = new Feature(propRoh_selectionEntityHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_selectionEntityLocality = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityLocality");
			this.Roh_selectionEntityLocality = new List<string>();
			if (propRoh_selectionEntityLocality != null && propRoh_selectionEntityLocality.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_selectionEntityLocality.PropertyValues)
				{
					this.Roh_selectionEntityLocality.Add(propValue.Value);
				}
			}
			this.Roh_selectionEntityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityTitle"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_organizationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organizationTypeOther"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_performedTasks = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/performedTasks"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_identification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/identification"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_selectionEntityTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityTypeOther"));
			this.Roh_members = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/members"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public Network(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_selectionEntityHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityHasRegion");
			if(propRoh_selectionEntityHasRegion != null && propRoh_selectionEntityHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_selectionEntityHasRegion = new Feature(propRoh_selectionEntityHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_selectionEntityType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityType");
			if(propRoh_selectionEntityType != null && propRoh_selectionEntityType.PropertyValues.Count > 0)
			{
				this.Roh_selectionEntityType = new OrganizationType(propRoh_selectionEntityType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_selectionEntity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntity");
			if(propRoh_selectionEntity != null && propRoh_selectionEntity.PropertyValues.Count > 0)
			{
				this.Roh_selectionEntity = new Organization(propRoh_selectionEntity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_selectionEntityHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityHasCountryName");
			if(propRoh_selectionEntityHasCountryName != null && propRoh_selectionEntityHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_selectionEntityHasCountryName = new Feature(propRoh_selectionEntityHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_selectionEntityLocality = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityLocality");
			this.Roh_selectionEntityLocality = new List<string>();
			if (propRoh_selectionEntityLocality != null && propRoh_selectionEntityLocality.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_selectionEntityLocality.PropertyValues)
				{
					this.Roh_selectionEntityLocality.Add(propValue.Value);
				}
			}
			this.Roh_selectionEntityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityTitle"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_organizationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organizationTypeOther"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_performedTasks = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/performedTasks"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_identification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/identification"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_selectionEntityTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/selectionEntityTypeOther"));
			this.Roh_members = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/members"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/Network"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/Network"; } }
		[RDFProperty("http://w3id.org/roh/selectionEntityHasRegion")]
		public  Feature Roh_selectionEntityHasRegion  { get; set;} 
		public string IdRoh_selectionEntityHasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/participates")]
		public  List<Organization> Roh_participates { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/selectionEntityType")]
		public  OrganizationType Roh_selectionEntityType  { get; set;} 
		public string IdRoh_selectionEntityType  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/selectionEntity")]
		public  Organization Roh_selectionEntity  { get; set;} 
		public string IdRoh_selectionEntity  { get; set;} 

		[RDFProperty("http://w3id.org/roh/selectionEntityHasCountryName")]
		public  Feature Roh_selectionEntityHasCountryName  { get; set;} 
		public string IdRoh_selectionEntityHasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/selectionEntityLocality")]
		public  List<string> Roh_selectionEntityLocality { get; set;}

		[RDFProperty("http://w3id.org/roh/selectionEntityTitle")]
		public  string Roh_selectionEntityTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/durationMonths")]
		public  string Roh_durationMonths { get; set;}

		[RDFProperty("http://w3id.org/roh/durationDays")]
		public  string Roh_durationDays { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("http://w3id.org/roh/organizationTypeOther")]
		public  string Roh_organizationTypeOther { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/performedTasks")]
		public  string Roh_performedTasks { get; set;}

		[RDFProperty("http://w3id.org/roh/durationYears")]
		public  string Roh_durationYears { get; set;}

		[RDFProperty("http://w3id.org/roh/identification")]
		public  string Roh_identification { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://w3id.org/roh/selectionEntityTypeOther")]
		public  string Roh_selectionEntityTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/members")]
		public  int? Roh_members { get; set;}

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
			propList.Add(new StringOntologyProperty("roh:selectionEntityHasRegion", this.IdRoh_selectionEntityHasRegion));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("roh:selectionEntityType", this.IdRoh_selectionEntityType));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:selectionEntity", this.IdRoh_selectionEntity));
			propList.Add(new StringOntologyProperty("roh:selectionEntityHasCountryName", this.IdRoh_selectionEntityHasCountryName));
			propList.Add(new ListStringOntologyProperty("roh:selectionEntityLocality", this.Roh_selectionEntityLocality));
			propList.Add(new StringOntologyProperty("roh:selectionEntityTitle", this.Roh_selectionEntityTitle));
			propList.Add(new StringOntologyProperty("roh:durationMonths", this.Roh_durationMonths));
			propList.Add(new StringOntologyProperty("roh:durationDays", this.Roh_durationDays));
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new StringOntologyProperty("roh:organizationTypeOther", this.Roh_organizationTypeOther));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("roh:performedTasks", this.Roh_performedTasks));
			propList.Add(new StringOntologyProperty("roh:durationYears", this.Roh_durationYears));
			propList.Add(new StringOntologyProperty("roh:identification", this.Roh_identification));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			propList.Add(new StringOntologyProperty("roh:selectionEntityTypeOther", this.Roh_selectionEntityTypeOther));
			propList.Add(new StringOntologyProperty("roh:members", this.Roh_members.ToString()));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Network>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Network\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_participates != null)
			{
			foreach(var item0 in this.Roh_participates)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Organization>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Organization\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}", "http://w3id.org/roh/participates", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRoh_organization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{item0.IdRoh_organization}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{item0.IdRoh_organizationType}>", list, " . ");
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					foreach(var item2 in item0.Roh_organizationTypeOther)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle)}\"", list, " . ");
				}
			}
			}
				if(this.IdRoh_selectionEntityHasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/selectionEntityHasRegion", $"<{this.IdRoh_selectionEntityHasRegion}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdRoh_selectionEntityType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/selectionEntityType", $"<{this.IdRoh_selectionEntityType}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_selectionEntity != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/selectionEntity", $"<{this.IdRoh_selectionEntity}>", list, " . ");
				}
				if(this.IdRoh_selectionEntityHasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/selectionEntityHasCountryName", $"<{this.IdRoh_selectionEntityHasCountryName}>", list, " . ");
				}
				if(this.Roh_selectionEntityLocality != null)
				{
					foreach(var item2 in this.Roh_selectionEntityLocality)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}", "http://w3id.org/roh/selectionEntityLocality", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_selectionEntityTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/selectionEntityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_selectionEntityTitle)}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths)}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays)}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_organizationTypeOther)}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Roh_performedTasks != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/performedTasks", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_performedTasks)}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears)}\"", list, " . ");
				}
				if(this.Roh_identification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/identification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_identification)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Roh_selectionEntityTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/selectionEntityTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_selectionEntityTypeOther)}\"", list, " . ");
				}
				if(this.Roh_members != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/members", $"{this.Roh_members.Value.ToString()}", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Network_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"network\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/Network\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_participates != null)
			{
			foreach(var item0 in this.Roh_participates)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/participates", $"<{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
				if(item0.Roh_organizationTypeOther != null)
				{
					foreach(var item2 in item0.Roh_organizationTypeOther)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle).ToLower()}\"", list, " . ");
				}
			}
			}
				if(this.IdRoh_selectionEntityHasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_selectionEntityHasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/selectionEntityHasRegion", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_selectionEntityType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_selectionEntityType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/selectionEntityType", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_selectionEntity != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_selectionEntity;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/selectionEntity", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_selectionEntityHasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_selectionEntityHasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/selectionEntityHasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_selectionEntityLocality != null)
				{
					foreach(var item2 in this.Roh_selectionEntityLocality)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/selectionEntityLocality", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_selectionEntityTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/selectionEntityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_selectionEntityTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#start", $"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_organizationTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_performedTasks != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/performedTasks", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_performedTasks).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears).ToLower()}\"", list, " . ");
				}
				if(this.Roh_identification != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/identification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_identification).ToLower()}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Roh_selectionEntityTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/selectionEntityTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_selectionEntityTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_members != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/members", $"{this.Roh_members.Value.ToString()}", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/NetworkOntology_{ResourceID}_{ArticleID}";
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
