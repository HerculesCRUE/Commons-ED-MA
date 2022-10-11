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
using ResearchObject = ResearchobjectOntology.ResearchObject;
using Document = DocumentOntology.Document;
using Person = PersonOntology.Person;

namespace AnnotationOntology
{
	[ExcludeFromCodeCoverage]
	public class Annotation : GnossOCBase
	{

		public Annotation() : base() { } 

		public Annotation(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Roh_researchobject = new List<ResearchObject>();
			SemanticPropertyModel propRoh_researchobject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchobject");
			if(propRoh_researchobject != null && propRoh_researchobject.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_researchobject.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						ResearchObject roh_researchobject = new ResearchObject(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_researchobject.Add(roh_researchobject);
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
			this.Roh_dateIssued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dateIssued"));
			this.Roh_text = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/text"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public Annotation(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_researchobject = new List<ResearchObject>();
			SemanticPropertyModel propRoh_researchobject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchobject");
			if(propRoh_researchobject != null && propRoh_researchobject.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_researchobject.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						ResearchObject roh_researchobject = new ResearchObject(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_researchobject.Add(roh_researchobject);
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
			this.Roh_dateIssued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dateIssued"));
			this.Roh_text = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/text"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/Annotation"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/Annotation"; } }
		[RDFProperty("http://w3id.org/roh/researchobject")]
		public  List<ResearchObject> Roh_researchobject { get; set;}
		public List<string> IdsRoh_researchobject { get; set;}

		[RDFProperty("http://w3id.org/roh/document")]
		public  List<Document> Roh_document { get; set;}
		public List<string> IdsRoh_document { get; set;}

		[RDFProperty("http://w3id.org/roh/dateIssued")]
		public  DateTime? Roh_dateIssued { get; set;}

		[RDFProperty("http://w3id.org/roh/text")]
		public  string Roh_text { get; set;}

		[RDFProperty("http://w3id.org/roh/owner")]
		[Required]
		public  Person Roh_owner  { get; set;} 
		public string IdRoh_owner  { get; set;} 

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("roh:researchobject", this.IdsRoh_researchobject));
			propList.Add(new ListStringOntologyProperty("roh:document", this.IdsRoh_document));
			if (this.Roh_dateIssued.HasValue){
				propList.Add(new DateOntologyProperty("roh:dateIssued", this.Roh_dateIssued.Value));
				}
			propList.Add(new StringOntologyProperty("roh:text", this.Roh_text));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology=null;
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
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Annotation>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Annotation\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdsRoh_researchobject != null)
				{
					foreach(var item2 in this.IdsRoh_researchobject)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{ArticleID}", "http://w3id.org/roh/researchobject", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsRoh_document != null)
				{
					foreach(var item2 in this.IdsRoh_document)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{ArticleID}", "http://w3id.org/roh/document", $"<{item2}>", list, " . ");
					}
				}
				if(this.Roh_dateIssued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/dateIssued", $"\"{this.Roh_dateIssued.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_text != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/text", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_text)}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"annotation\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/Annotation\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
				if(this.IdsRoh_researchobject != null)
				{
					foreach(var item2 in this.IdsRoh_researchobject)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/researchobject", $"<{itemRegex}>", list, " . ");
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
				if(this.Roh_dateIssued != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/dateIssued", $"{this.Roh_dateIssued.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_text != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/text", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_text).ToLower()}\"", list, " . ");
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
			string tablaDoc = $"'{titulo}', '', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/AnnotationOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Roh_title;
		}





	}
}
