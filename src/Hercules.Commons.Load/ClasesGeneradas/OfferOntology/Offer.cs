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
using Person = PersonOntology.Person;
using Document = DocumentOntology.Document;
using Project = ProjectOntology.Project;
using Patent = PatentOntology.Patent;
using OfferState = OfferstateOntology.OfferState;
using FramingSector = FramingsectorOntology.FramingSector;
using MatureState = MaturestateOntology.MatureState;

namespace OfferOntology
{
	[ExcludeFromCodeCoverage]
	public class Offer : GnossOCBase
	{

		public Offer() : base() { } 

		public Offer(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Roh_researchers = new List<Person>();
			SemanticPropertyModel propRoh_researchers = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchers");
			if(propRoh_researchers != null && propRoh_researchers.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_researchers.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person roh_researchers = new Person(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_researchers.Add(roh_researchers);
					}
				}
			}
			this.Roh_document = new List<Document>();
			SemanticPropertyModel propRoh_document = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/document");
			if(propRoh_document != null && propRoh_document.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_document.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document roh_document = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_document.Add(roh_document);
					}
				}
			}
			this.Roh_project = new List<Project>();
			SemanticPropertyModel propRoh_project = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/project");
			if(propRoh_project != null && propRoh_project.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_project.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Project roh_project = new Project(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_project.Add(roh_project);
					}
				}
			}
			this.Roh_availabilityChangeEvent = new List<AvailabilityChangeEvent>();
			SemanticPropertyModel propRoh_availabilityChangeEvent = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/availabilityChangeEvent");
			if(propRoh_availabilityChangeEvent != null && propRoh_availabilityChangeEvent.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_availabilityChangeEvent.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						AvailabilityChangeEvent roh_availabilityChangeEvent = new AvailabilityChangeEvent(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_availabilityChangeEvent.Add(roh_availabilityChangeEvent);
					}
				}
			}
			this.Roh_patents = new List<Patent>();
			SemanticPropertyModel propRoh_patents = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/patents");
			if(propRoh_patents != null && propRoh_patents.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_patents.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Patent roh_patents = new Patent(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_patents.Add(roh_patents);
					}
				}
			}
			this.Drm_origin = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vocab.data.gov/def/drm#origin"));
			SemanticPropertyModel propRoh_lineResearch = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/lineResearch");
			this.Roh_lineResearch = new List<string>();
			if (propRoh_lineResearch != null && propRoh_lineResearch.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_lineResearch.PropertyValues)
				{
					this.Roh_lineResearch.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propVivo_freetextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freetextKeyword");
			this.Vivo_freetextKeyword = new List<string>();
			if (propVivo_freetextKeyword != null && propVivo_freetextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freetextKeyword.PropertyValues)
				{
					this.Vivo_freetextKeyword.Add(propValue.Value);
				}
			}
			this.Roh_innovation = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/innovation"));
			this.Roh_collaborationSought = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collaborationSought"));
			this.Roh_partnerType = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/partnerType"));
			this.Qb_observation = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/linked-data/cube#observation"));
			this.Bibo_recipient = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/recipient"));
var item0 = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
if(item0.HasValue){
			this.Dct_issued = item0.Value;
}
			SemanticPropertyModel propSchema_offeredBy = pSemCmsModel.GetPropertyByPath("http://www.schema.org/offeredBy");
			if(propSchema_offeredBy != null && propSchema_offeredBy.PropertyValues.Count > 0)
			{
				this.Schema_offeredBy = new Person(propSchema_offeredBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_application = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/application"));
			SemanticPropertyModel propSchema_availability = pSemCmsModel.GetPropertyByPath("http://www.schema.org/availability");
			if(propSchema_availability != null && propSchema_availability.PropertyValues.Count > 0)
			{
				this.Schema_availability = new OfferState(propSchema_availability.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.schema.org/description"));
			SemanticPropertyModel propRoh_framingSector = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/framingSector");
			if(propRoh_framingSector != null && propRoh_framingSector.PropertyValues.Count > 0)
			{
				this.Roh_framingSector = new FramingSector(propRoh_framingSector.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propBibo_status = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/status");
			if(propBibo_status != null && propBibo_status.PropertyValues.Count > 0)
			{
				this.Bibo_status = new MatureState(propBibo_status.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.schema.org/name"));
		}

		public Offer(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_researchers = new List<Person>();
			SemanticPropertyModel propRoh_researchers = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchers");
			if(propRoh_researchers != null && propRoh_researchers.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_researchers.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person roh_researchers = new Person(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_researchers.Add(roh_researchers);
					}
				}
			}
			this.Roh_document = new List<Document>();
			SemanticPropertyModel propRoh_document = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/document");
			if(propRoh_document != null && propRoh_document.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_document.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document roh_document = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_document.Add(roh_document);
					}
				}
			}
			this.Roh_project = new List<Project>();
			SemanticPropertyModel propRoh_project = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/project");
			if(propRoh_project != null && propRoh_project.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_project.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Project roh_project = new Project(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_project.Add(roh_project);
					}
				}
			}
			this.Roh_availabilityChangeEvent = new List<AvailabilityChangeEvent>();
			SemanticPropertyModel propRoh_availabilityChangeEvent = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/availabilityChangeEvent");
			if(propRoh_availabilityChangeEvent != null && propRoh_availabilityChangeEvent.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_availabilityChangeEvent.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						AvailabilityChangeEvent roh_availabilityChangeEvent = new AvailabilityChangeEvent(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_availabilityChangeEvent.Add(roh_availabilityChangeEvent);
					}
				}
			}
			this.Roh_patents = new List<Patent>();
			SemanticPropertyModel propRoh_patents = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/patents");
			if(propRoh_patents != null && propRoh_patents.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_patents.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Patent roh_patents = new Patent(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_patents.Add(roh_patents);
					}
				}
			}
			this.Drm_origin = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vocab.data.gov/def/drm#origin"));
			SemanticPropertyModel propRoh_lineResearch = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/lineResearch");
			this.Roh_lineResearch = new List<string>();
			if (propRoh_lineResearch != null && propRoh_lineResearch.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_lineResearch.PropertyValues)
				{
					this.Roh_lineResearch.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propVivo_freetextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freetextKeyword");
			this.Vivo_freetextKeyword = new List<string>();
			if (propVivo_freetextKeyword != null && propVivo_freetextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freetextKeyword.PropertyValues)
				{
					this.Vivo_freetextKeyword.Add(propValue.Value);
				}
			}
			this.Roh_innovation = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/innovation"));
			this.Roh_collaborationSought = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collaborationSought"));
			this.Roh_partnerType = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/partnerType"));
			this.Qb_observation = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/linked-data/cube#observation"));
			this.Bibo_recipient = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/recipient"));
var item1 = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
if(item1.HasValue){
			this.Dct_issued = item1.Value;
}
			SemanticPropertyModel propSchema_offeredBy = pSemCmsModel.GetPropertyByPath("http://www.schema.org/offeredBy");
			if(propSchema_offeredBy != null && propSchema_offeredBy.PropertyValues.Count > 0)
			{
				this.Schema_offeredBy = new Person(propSchema_offeredBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_application = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/application"));
			SemanticPropertyModel propSchema_availability = pSemCmsModel.GetPropertyByPath("http://www.schema.org/availability");
			if(propSchema_availability != null && propSchema_availability.PropertyValues.Count > 0)
			{
				this.Schema_availability = new OfferState(propSchema_availability.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Schema_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.schema.org/description"));
			SemanticPropertyModel propRoh_framingSector = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/framingSector");
			if(propRoh_framingSector != null && propRoh_framingSector.PropertyValues.Count > 0)
			{
				this.Roh_framingSector = new FramingSector(propRoh_framingSector.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propBibo_status = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/status");
			if(propBibo_status != null && propBibo_status.PropertyValues.Count > 0)
			{
				this.Bibo_status = new MatureState(propBibo_status.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Schema_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.schema.org/name"));
		}

		public virtual string RdfType { get { return "http://www.schema.org/Offer"; } }
		public virtual string RdfsLabel { get { return "http://www.schema.org/Offer"; } }
		[RDFProperty("http://w3id.org/roh/researchers")]
		public  List<Person> Roh_researchers { get; set;}
		public List<string> IdsRoh_researchers { get; set;}

		[RDFProperty("http://w3id.org/roh/document")]
		public  List<Document> Roh_document { get; set;}
		public List<string> IdsRoh_document { get; set;}

		[RDFProperty("http://w3id.org/roh/project")]
		public  List<Project> Roh_project { get; set;}
		public List<string> IdsRoh_project { get; set;}

		[RDFProperty("http://w3id.org/roh/availabilityChangeEvent")]
		[MinLength(1)]
		public  List<AvailabilityChangeEvent> Roh_availabilityChangeEvent { get; set;}

		[RDFProperty("http://w3id.org/roh/patents")]
		public  List<Patent> Roh_patents { get; set;}
		public List<string> IdsRoh_patents { get; set;}

		[RDFProperty("http://vocab.data.gov/def/drm#origin")]
		public  string Drm_origin { get; set;}

		[RDFProperty("http://w3id.org/roh/lineResearch")]
		public  List<string> Roh_lineResearch { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#freetextKeyword")]
		public  List<string> Vivo_freetextKeyword { get; set;}

		[RDFProperty("http://w3id.org/roh/innovation")]
		public  string Roh_innovation { get; set;}

		[RDFProperty("http://w3id.org/roh/collaborationSought")]
		public  string Roh_collaborationSought { get; set;}

		[RDFProperty("http://w3id.org/roh/partnerType")]
		public  string Roh_partnerType { get; set;}

		[RDFProperty("http://purl.org/linked-data/cube#observation")]
		public  string Qb_observation { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/recipient")]
		public  string Bibo_recipient { get; set;}

		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime Dct_issued { get; set;}

		[RDFProperty("http://www.schema.org/offeredBy")]
		[Required]
		public  Person Schema_offeredBy  { get; set;} 
		public string IdSchema_offeredBy  { get; set;} 

		[RDFProperty("http://w3id.org/roh/application")]
		public  string Roh_application { get; set;}

		[RDFProperty("http://www.schema.org/availability")]
		[Required]
		public  OfferState Schema_availability  { get; set;} 
		public string IdSchema_availability  { get; set;} 

		[RDFProperty("http://www.schema.org/description")]
		public  string Schema_description { get; set;}

		[RDFProperty("http://w3id.org/roh/framingSector")]
		[Required]
		public  FramingSector Roh_framingSector  { get; set;} 
		public string IdRoh_framingSector  { get; set;} 

		[RDFProperty("http://purl.org/ontology/bibo/status")]
		[Required]
		public  MatureState Bibo_status  { get; set;} 
		public string IdBibo_status  { get; set;} 

		[RDFProperty("http://www.schema.org/name")]
		public  string Schema_name { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("roh:researchers", this.IdsRoh_researchers));
			propList.Add(new ListStringOntologyProperty("roh:document", this.IdsRoh_document));
			propList.Add(new ListStringOntologyProperty("roh:project", this.IdsRoh_project));
			propList.Add(new ListStringOntologyProperty("roh:patents", this.IdsRoh_patents));
			propList.Add(new StringOntologyProperty("drm:origin", this.Drm_origin));
			propList.Add(new ListStringOntologyProperty("roh:lineResearch", this.Roh_lineResearch));
			propList.Add(new ListStringOntologyProperty("vivo:freetextKeyword", this.Vivo_freetextKeyword));
			propList.Add(new StringOntologyProperty("roh:innovation", this.Roh_innovation));
			propList.Add(new StringOntologyProperty("roh:collaborationSought", this.Roh_collaborationSought));
			propList.Add(new StringOntologyProperty("roh:partnerType", this.Roh_partnerType));
			propList.Add(new StringOntologyProperty("qb:observation", this.Qb_observation));
			propList.Add(new StringOntologyProperty("bibo:recipient", this.Bibo_recipient));
			propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued));
			propList.Add(new StringOntologyProperty("schema:offeredBy", this.IdSchema_offeredBy));
			propList.Add(new StringOntologyProperty("roh:application", this.Roh_application));
			propList.Add(new StringOntologyProperty("schema:availability", this.IdSchema_availability));
			propList.Add(new StringOntologyProperty("schema:description", this.Schema_description));
			propList.Add(new StringOntologyProperty("roh:framingSector", this.IdRoh_framingSector));
			propList.Add(new StringOntologyProperty("bibo:status", this.IdBibo_status));
			propList.Add(new StringOntologyProperty("schema:name", this.Schema_name));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_availabilityChangeEvent!=null){
				foreach(AvailabilityChangeEvent prop in Roh_availabilityChangeEvent){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityAvailabilityChangeEvent = new OntologyEntity("http://w3id.org/roh/AvailabilityChangeEvent", "http://w3id.org/roh/AvailabilityChangeEvent", "roh:availabilityChangeEvent", prop.propList, prop.entList);
				entList.Add(entityAvailabilityChangeEvent);
				prop.Entity= entityAvailabilityChangeEvent;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://www.schema.org/Offer>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://www.schema.org/Offer\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_availabilityChangeEvent != null)
			{
			foreach(var item0 in this.Roh_availabilityChangeEvent)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AvailabilityChangeEvent_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/AvailabilityChangeEvent>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AvailabilityChangeEvent_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/AvailabilityChangeEvent\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/AvailabilityChangeEvent_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}", "http://w3id.org/roh/availabilityChangeEvent", $"<{resourceAPI.GraphsUrl}items/AvailabilityChangeEvent_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRoh_roleOf != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AvailabilityChangeEvent_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/roleOf", $"<{item0.IdRoh_roleOf}>", list, " . ");
				}
				if(item0.IdSchema_availability != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AvailabilityChangeEvent_{ResourceID}_{item0.ArticleID}",  "http://www.schema.org/availability", $"<{item0.IdSchema_availability}>", list, " . ");
				}
				if(item0.Schema_validFrom != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AvailabilityChangeEvent_{ResourceID}_{item0.ArticleID}",  "http://www.schema.org/validFrom", $"\"{item0.Schema_validFrom.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
			}
			}
				if(this.IdsRoh_researchers != null)
				{
					foreach(var item2 in this.IdsRoh_researchers)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}", "http://w3id.org/roh/researchers", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsRoh_document != null)
				{
					foreach(var item2 in this.IdsRoh_document)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}", "http://w3id.org/roh/document", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsRoh_project != null)
				{
					foreach(var item2 in this.IdsRoh_project)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}", "http://w3id.org/roh/project", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsRoh_patents != null)
				{
					foreach(var item2 in this.IdsRoh_patents)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}", "http://w3id.org/roh/patents", $"<{item2}>", list, " . ");
					}
				}
				if(this.Drm_origin != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://vocab.data.gov/def/drm#origin", $"\"{GenerarTextoSinSaltoDeLinea(this.Drm_origin)}\"", list, " . ");
				}
				if(this.Roh_lineResearch != null)
				{
					foreach(var item2 in this.Roh_lineResearch)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}", "http://w3id.org/roh/lineResearch", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Vivo_freetextKeyword != null)
				{
					foreach(var item2 in this.Vivo_freetextKeyword)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#freetextKeyword", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_innovation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/innovation", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_innovation)}\"", list, " . ");
				}
				if(this.Roh_collaborationSought != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/collaborationSought", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_collaborationSought)}\"", list, " . ");
				}
				if(this.Roh_partnerType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/partnerType", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_partnerType)}\"", list, " . ");
				}
				if(this.Qb_observation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://purl.org/linked-data/cube#observation", $"\"{GenerarTextoSinSaltoDeLinea(this.Qb_observation)}\"", list, " . ");
				}
				if(this.Bibo_recipient != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/recipient", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_recipient)}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{this.Dct_issued.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.IdSchema_offeredBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://www.schema.org/offeredBy", $"<{this.IdSchema_offeredBy}>", list, " . ");
				}
				if(this.Roh_application != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/application", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_application)}\"", list, " . ");
				}
				if(this.IdSchema_availability != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://www.schema.org/availability", $"<{this.IdSchema_availability}>", list, " . ");
				}
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://www.schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description)}\"", list, " . ");
				}
				if(this.IdRoh_framingSector != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/framingSector", $"<{this.IdRoh_framingSector}>", list, " . ");
				}
				if(this.IdBibo_status != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/status", $"<{this.IdBibo_status}>", list, " . ");
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Offer_{ResourceID}_{ArticleID}",  "http://www.schema.org/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"offer\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://www.schema.org/Offer\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_availabilityChangeEvent != null)
			{
			foreach(var item0 in this.Roh_availabilityChangeEvent)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/availabilityChangeEvent", $"<{resourceAPI.GraphsUrl}items/availabilitychangeevent_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRoh_roleOf != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_roleOf;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/availabilitychangeevent_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/roleOf", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdSchema_availability != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdSchema_availability;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/availabilitychangeevent_{ResourceID}_{item0.ArticleID}",  "http://www.schema.org/availability", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Schema_validFrom != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/availabilitychangeevent_{ResourceID}_{item0.ArticleID}",  "http://www.schema.org/validFrom", $"{item0.Schema_validFrom.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
			}
			}
				if(this.IdsRoh_researchers != null)
				{
					foreach(var item2 in this.IdsRoh_researchers)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/researchers", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsRoh_document != null)
				{
					foreach(var item2 in this.IdsRoh_document)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/document", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsRoh_project != null)
				{
					foreach(var item2 in this.IdsRoh_project)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/project", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsRoh_patents != null)
				{
					foreach(var item2 in this.IdsRoh_patents)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/patents", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Drm_origin != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vocab.data.gov/def/drm#origin", $"\"{GenerarTextoSinSaltoDeLinea(this.Drm_origin).ToLower()}\"", list, " . ");
				}
				if(this.Roh_lineResearch != null)
				{
					foreach(var item2 in this.Roh_lineResearch)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/lineResearch", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Vivo_freetextKeyword != null)
				{
					foreach(var item2 in this.Vivo_freetextKeyword)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://vivoweb.org/ontology/core#freetextKeyword", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_innovation != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/innovation", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_innovation).ToLower()}\"", list, " . ");
				}
				if(this.Roh_collaborationSought != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/collaborationSought", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_collaborationSought).ToLower()}\"", list, " . ");
				}
				if(this.Roh_partnerType != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/partnerType", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_partnerType).ToLower()}\"", list, " . ");
				}
				if(this.Qb_observation != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/linked-data/cube#observation", $"\"{GenerarTextoSinSaltoDeLinea(this.Qb_observation).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_recipient != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/recipient", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_recipient).ToLower()}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/terms/issued", $"{this.Dct_issued.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.IdSchema_offeredBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdSchema_offeredBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.schema.org/offeredBy", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_application != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/application", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_application).ToLower()}\"", list, " . ");
				}
				if(this.IdSchema_availability != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdSchema_availability;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.schema.org/availability", $"<{itemRegex}>", list, " . ");
				}
				if(this.Schema_description != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.schema.org/description", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_description).ToLower()}\"", list, " . ");
				}
				if(this.IdRoh_framingSector != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_framingSector;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/framingSector", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdBibo_status != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdBibo_status;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/status", $"<{itemRegex}>", list, " . ");
				}
				if(this.Schema_name != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.schema.org/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Schema_name).ToLower()}\"", list, " . ");
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
			string titulo = $"{this.Schema_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Schema_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/OfferOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Schema_name;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Schema_name;
		}




	}
}
