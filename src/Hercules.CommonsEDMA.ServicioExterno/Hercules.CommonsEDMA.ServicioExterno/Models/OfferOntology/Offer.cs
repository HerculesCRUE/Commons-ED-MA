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
using OfferState = OfferstateOntology.OfferState;
using FramingSector = FramingsectorOntology.FramingSector;
using MatureState = MaturestateOntology.MatureState;

namespace OfferOntology
{
	[ExcludeFromCodeCoverage]
	public class Offer : GnossOCBase
	{

		public Offer() : base() { } 

		public virtual string RdfType { get { return "http://www.schema.org/Offer"; } }
		public virtual string RdfsLabel { get { return "http://www.schema.org/Offer"; } }
		public List<string> IdsRoh_researchers { get; set;}
		public List<string> IdsRoh_document { get; set;}

		[RDFProperty("http://w3id.org/roh/groups")]
		public List<Group> Roh_groups { get; set; }
		public List<string> IdsRoh_groups { get; set; }
		public List<string> IdsRoh_project { get; set;}

		[RDFProperty("http://w3id.org/roh/availabilityChangeEvent")]
		[MinLength(1)]
		public  List<AvailabilityChangeEvent> Roh_availabilityChangeEvent { get; set;}
		public List<string> IdsRoh_patents { get; set;}

		[RDFProperty("http://w3id.org/roh/search")]
		public string Roh_search { get; set; }

		[RDFProperty("http://vocab.data.gov/def/drm#origin")]
		public  string Drm_origin { get; set;}

		[RDFProperty("http://w3id.org/roh/lineResearch")]
		public  List<string> Roh_lineResearch { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#freetextKeyword")]
		public  List<string> Vivo_freetextKeyword { get; set;}

		[RDFProperty("http://w3id.org/roh/innovation")]
		public  string Roh_innovation { get; set; }

		[RDFProperty("http://w3id.org/roh/areaprocedencia")]
		public List<CategoryPath> Roh_areaprocedencia { get; set; }

		[RDFProperty("http://w3id.org/roh/sectoraplicacion")]
		public List<CategoryPath> Roh_sectoraplicacion { get; set; }

		[RDFProperty("http://w3id.org/roh/advantagesBenefits")]
		public string Roh_advantagesBenefits { get; set; }

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
			propList.Add(new ListStringOntologyProperty("roh:groups", this.IdsRoh_groups));
			propList.Add(new ListStringOntologyProperty("roh:project", this.IdsRoh_project));
			propList.Add(new ListStringOntologyProperty("roh:patents", this.IdsRoh_patents));
			propList.Add(new StringOntologyProperty("roh:search", this.Roh_search));
			propList.Add(new StringOntologyProperty("drm:origin", this.Drm_origin));
			propList.Add(new ListStringOntologyProperty("roh:lineResearch", this.Roh_lineResearch));
			propList.Add(new ListStringOntologyProperty("vivo:freetextKeyword", this.Vivo_freetextKeyword));
			propList.Add(new StringOntologyProperty("roh:innovation", this.Roh_innovation));
			propList.Add(new StringOntologyProperty("roh:collaborationSought", this.Roh_collaborationSought));
			propList.Add(new StringOntologyProperty("roh:partnerType", this.Roh_partnerType));
			propList.Add(new StringOntologyProperty("roh:advantagesBenefits", this.Roh_advantagesBenefits));
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
			if (Roh_sectoraplicacion!=null)
			{
				foreach (CategoryPath prop in Roh_sectoraplicacion)
				{
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:sectoraplicacion", prop.propList, prop.entList);
					entList.Add(entityCategoryPath);
					prop.Entity= entityCategoryPath;
				}
			}
			if (Roh_availabilityChangeEvent!=null)
			{
				foreach (AvailabilityChangeEvent prop in Roh_availabilityChangeEvent)
				{
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityAvailabilityChangeEvent = new("http://w3id.org/roh/AvailabilityChangeEvent", "http://w3id.org/roh/AvailabilityChangeEvent", "roh:availabilityChangeEvent", prop.propList, prop.entList);
					entList.Add(entityAvailabilityChangeEvent);
					prop.Entity= entityAvailabilityChangeEvent;
				}
			}
			if (Roh_areaprocedencia!=null)
			{
				foreach (CategoryPath prop in Roh_areaprocedencia)
				{
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:areaprocedencia", prop.propList, prop.entList);
					entList.Add(entityCategoryPath);
					prop.Entity= entityCategoryPath;
				}
			}
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
			KeyValuePair<Guid, string> valor = new(ResourceID, tablaDoc);

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



		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new();
			Ontology ontology = null;
			GetEntities();
			GetProperties();
			if (idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList, idrecurso, idarticulo);
			}
			resource.Id = GNOSSID;
			resource.Ontology = ontology;
			resource.TextCategories=listaDeCategorias;
			AddResourceTitle(resource);
			AddResourceDescription(resource);
			return resource;
		}


	}
}
