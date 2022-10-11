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
using EventType = EventtypeOntology.EventType;
using Feature = FeatureOntology.Feature;
using Language = LanguageOntology.Language;
using ParticipationTypeDocument = ParticipationtypedocumentOntology.ParticipationTypeDocument;
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using Person = PersonOntology.Person;

namespace ImpartedcoursesseminarsOntology
{
	[ExcludeFromCodeCoverage]
	public class ImpartedCoursesSeminars : GnossOCBase
	{

		public ImpartedCoursesSeminars() : base() { } 

		public ImpartedCoursesSeminars(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_promotedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedBy");
			if(propRoh_promotedBy != null && propRoh_promotedBy.PropertyValues.Count > 0)
			{
				this.Roh_promotedBy = new Organization(propRoh_promotedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_eventType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventType");
			if(propRoh_eventType != null && propRoh_eventType.PropertyValues.Count > 0)
			{
				this.Roh_eventType = new EventType(propRoh_eventType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_identifier = new List<Document>();
			SemanticPropertyModel propBibo_identifier = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/identifier");
			if(propBibo_identifier != null && propBibo_identifier.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_identifier.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document bibo_identifier = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_identifier.Add(bibo_identifier);
					}
				}
			}
			SemanticPropertyModel propVcard_hasLanguage = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasLanguage");
			if(propVcard_hasLanguage != null && propVcard_hasLanguage.PropertyValues.Count > 0)
			{
				this.Vcard_hasLanguage = new Language(propVcard_hasLanguage.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_participationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationType");
			if(propRoh_participationType != null && propRoh_participationType.PropertyValues.Count > 0)
			{
				this.Roh_participationType = new ParticipationTypeDocument(propRoh_participationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByType");
			if(propRoh_promotedByType != null && propRoh_promotedByType.PropertyValues.Count > 0)
			{
				this.Roh_promotedByType = new OrganizationType(propRoh_promotedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isbn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isbn"));
			this.Bibo_handle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/handle"));
			this.Roh_participationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationTypeOther"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_targetProfile = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/targetProfile"));
			this.Roh_promotedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTypeOther"));
			this.Roh_goals = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/goals"));
			this.Bibo_doi = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/doi"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Bibo_issn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issn"));
			this.Roh_eventTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventTypeOther"));
			this.Roh_promotedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTitle"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_correspondingAuthor= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/correspondingAuthor"));
			this.Roh_durationHours = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationHours"));
			this.Bibo_pmid = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pmid"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public ImpartedCoursesSeminars(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_promotedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedBy");
			if(propRoh_promotedBy != null && propRoh_promotedBy.PropertyValues.Count > 0)
			{
				this.Roh_promotedBy = new Organization(propRoh_promotedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_eventType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventType");
			if(propRoh_eventType != null && propRoh_eventType.PropertyValues.Count > 0)
			{
				this.Roh_eventType = new EventType(propRoh_eventType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_identifier = new List<Document>();
			SemanticPropertyModel propBibo_identifier = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/identifier");
			if(propBibo_identifier != null && propBibo_identifier.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_identifier.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document bibo_identifier = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_identifier.Add(bibo_identifier);
					}
				}
			}
			SemanticPropertyModel propVcard_hasLanguage = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasLanguage");
			if(propVcard_hasLanguage != null && propVcard_hasLanguage.PropertyValues.Count > 0)
			{
				this.Vcard_hasLanguage = new Language(propVcard_hasLanguage.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_participationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationType");
			if(propRoh_participationType != null && propRoh_participationType.PropertyValues.Count > 0)
			{
				this.Roh_participationType = new ParticipationTypeDocument(propRoh_participationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByType");
			if(propRoh_promotedByType != null && propRoh_promotedByType.PropertyValues.Count > 0)
			{
				this.Roh_promotedByType = new OrganizationType(propRoh_promotedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isbn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isbn"));
			this.Bibo_handle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/handle"));
			this.Roh_participationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationTypeOther"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_targetProfile = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/targetProfile"));
			this.Roh_promotedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTypeOther"));
			this.Roh_goals = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/goals"));
			this.Bibo_doi = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/doi"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Bibo_issn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issn"));
			this.Roh_eventTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventTypeOther"));
			this.Roh_promotedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTitle"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_correspondingAuthor= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/correspondingAuthor"));
			this.Roh_durationHours = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationHours"));
			this.Bibo_pmid = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pmid"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ImpartedCoursesSeminars"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ImpartedCoursesSeminars"; } }
		[RDFProperty("http://w3id.org/roh/promotedBy")]
		public  Organization Roh_promotedBy  { get; set;} 
		public string IdRoh_promotedBy  { get; set;} 

		[RDFProperty("http://w3id.org/roh/eventType")]
		public  EventType Roh_eventType  { get; set;} 
		public string IdRoh_eventType  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://purl.org/ontology/bibo/identifier")]
		public  List<Document> Bibo_identifier { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasLanguage")]
		public  Language Vcard_hasLanguage  { get; set;} 
		public string IdVcard_hasLanguage  { get; set;} 

		[RDFProperty("http://w3id.org/roh/participationType")]
		public  ParticipationTypeDocument Roh_participationType  { get; set;} 
		public string IdRoh_participationType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/promotedByType")]
		public  OrganizationType Roh_promotedByType  { get; set;} 
		public string IdRoh_promotedByType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/isbn")]
		public  string Roh_isbn { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/handle")]
		public  string Bibo_handle { get; set;}

		[RDFProperty("http://w3id.org/roh/participationTypeOther")]
		public  string Roh_participationTypeOther { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("http://w3id.org/roh/targetProfile")]
		public  string Roh_targetProfile { get; set;}

		[RDFProperty("http://w3id.org/roh/promotedByTypeOther")]
		public  string Roh_promotedByTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/goals")]
		public  string Roh_goals { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/doi")]
		public  string Bibo_doi { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/issn")]
		public  string Bibo_issn { get; set;}

		[RDFProperty("http://w3id.org/roh/eventTypeOther")]
		public  string Roh_eventTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/promotedByTitle")]
		public  string Roh_promotedByTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://w3id.org/roh/correspondingAuthor")]
		public  bool Roh_correspondingAuthor { get; set;}

		[RDFProperty("http://w3id.org/roh/durationHours")]
		public  float? Roh_durationHours { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/pmid")]
		public  string Bibo_pmid { get; set;}

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
			propList.Add(new StringOntologyProperty("roh:promotedBy", this.IdRoh_promotedBy));
			propList.Add(new StringOntologyProperty("roh:eventType", this.IdRoh_eventType));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("vcard:hasLanguage", this.IdVcard_hasLanguage));
			propList.Add(new StringOntologyProperty("roh:participationType", this.IdRoh_participationType));
			propList.Add(new StringOntologyProperty("roh:promotedByType", this.IdRoh_promotedByType));
			propList.Add(new StringOntologyProperty("roh:isbn", this.Roh_isbn));
			propList.Add(new StringOntologyProperty("bibo:handle", this.Bibo_handle));
			propList.Add(new StringOntologyProperty("roh:participationTypeOther", this.Roh_participationTypeOther));
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new StringOntologyProperty("roh:targetProfile", this.Roh_targetProfile));
			propList.Add(new StringOntologyProperty("roh:promotedByTypeOther", this.Roh_promotedByTypeOther));
			propList.Add(new StringOntologyProperty("roh:goals", this.Roh_goals));
			propList.Add(new StringOntologyProperty("bibo:doi", this.Bibo_doi));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("bibo:issn", this.Bibo_issn));
			propList.Add(new StringOntologyProperty("roh:eventTypeOther", this.Roh_eventTypeOther));
			propList.Add(new StringOntologyProperty("roh:promotedByTitle", this.Roh_promotedByTitle));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			propList.Add(new BoolOntologyProperty("roh:correspondingAuthor", this.Roh_correspondingAuthor));
			propList.Add(new StringOntologyProperty("roh:durationHours", this.Roh_durationHours.ToString()));
			propList.Add(new StringOntologyProperty("bibo:pmid", this.Bibo_pmid));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Bibo_identifier!=null){
				foreach(Document prop in Bibo_identifier){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityDocument = new OntologyEntity("http://xmlns.com/foaf/0.1/Document", "http://xmlns.com/foaf/0.1/Document", "bibo:identifier", prop.propList, prop.entList);
				entList.Add(entityDocument);
				prop.Entity= entityDocument;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ImpartedCoursesSeminars>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ImpartedCoursesSeminars\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Bibo_identifier != null)
			{
			foreach(var item0 in this.Bibo_identifier)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://xmlns.com/foaf/0.1/Document>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://xmlns.com/foaf/0.1/Document\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}", "http://purl.org/ontology/bibo/identifier", $"<{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Dc_title)}\"", list, " . ");
				}
				if(item0.Foaf_topic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/topic", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_topic)}\"", list, " . ");
				}
			}
			}
				if(this.IdRoh_promotedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedBy", $"<{this.IdRoh_promotedBy}>", list, " . ");
				}
				if(this.IdRoh_eventType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/eventType", $"<{this.IdRoh_eventType}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdVcard_hasLanguage != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasLanguage", $"<{this.IdVcard_hasLanguage}>", list, " . ");
				}
				if(this.IdRoh_participationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/participationType", $"<{this.IdRoh_participationType}>", list, " . ");
				}
				if(this.IdRoh_promotedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByType", $"<{this.IdRoh_promotedByType}>", list, " . ");
				}
				if(this.Roh_isbn != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isbn", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_isbn)}\"", list, " . ");
				}
				if(this.Bibo_handle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/handle", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_handle)}\"", list, " . ");
				}
				if(this.Roh_participationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/participationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_participationTypeOther)}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_targetProfile != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/targetProfile", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_targetProfile)}\"", list, " . ");
				}
				if(this.Roh_promotedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTypeOther)}\"", list, " . ");
				}
				if(this.Roh_goals != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/goals", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_goals)}\"", list, " . ");
				}
				if(this.Bibo_doi != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_doi)}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Bibo_issn != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/issn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issn)}\"", list, " . ");
				}
				if(this.Roh_eventTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/eventTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_eventTypeOther)}\"", list, " . ");
				}
				if(this.Roh_promotedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTitle)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/correspondingAuthor", $"\"{this.Roh_correspondingAuthor.ToString()}\"", list, " . ");
				}
				if(this.Roh_durationHours != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationHours", $"{this.Roh_durationHours.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Bibo_pmid != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/pmid", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pmid)}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedCoursesSeminars_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"impartedcoursesseminars\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/ImpartedCoursesSeminars\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Bibo_identifier != null)
			{
			foreach(var item0 in this.Bibo_identifier)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/ontology/bibo/identifier", $"<{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Dc_title).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_topic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/topic", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_topic).ToLower()}\"", list, " . ");
				}
			}
			}
				if(this.IdRoh_promotedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_promotedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedBy", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_eventType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_eventType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/eventType", $"<{itemRegex}>", list, " . ");
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
				if(this.IdVcard_hasLanguage != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVcard_hasLanguage;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#hasLanguage", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_participationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_participationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/participationType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_promotedByType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_promotedByType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByType", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_isbn != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isbn", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_isbn).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_handle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/handle", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_handle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_participationTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/participationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_participationTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#start", $"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_targetProfile != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/targetProfile", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_targetProfile).ToLower()}\"", list, " . ");
				}
				if(this.Roh_promotedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_goals != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/goals", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_goals).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_doi != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_doi).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_issn != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/issn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issn).ToLower()}\"", list, " . ");
				}
				if(this.Roh_eventTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/eventTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_eventTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_promotedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/correspondingAuthor", $"\"{this.Roh_correspondingAuthor.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationHours != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationHours", $"{this.Roh_durationHours.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Bibo_pmid != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/pmid", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pmid).ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/ImpartedcoursesseminarsOntology_{ResourceID}_{ArticleID}";
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
