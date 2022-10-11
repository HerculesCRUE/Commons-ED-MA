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
using DocumentFormat = DocumentformatOntology.DocumentFormat;

namespace MaindocumentOntology
{
	[ExcludeFromCodeCoverage]
	public class MainDocument : GnossOCBase
	{

		public MainDocument() : base() { } 

		public MainDocument(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Roh_impactIndex = new List<ImpactIndex>();
			SemanticPropertyModel propRoh_impactIndex = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndex");
			if(propRoh_impactIndex != null && propRoh_impactIndex.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_impactIndex.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						ImpactIndex roh_impactIndex = new ImpactIndex(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_impactIndex.Add(roh_impactIndex);
					}
				}
			}
			SemanticPropertyModel propRoh_format = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/format");
			if(propRoh_format != null && propRoh_format.PropertyValues.Count > 0)
			{
				this.Roh_format = new DocumentFormat(propRoh_format.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_eissn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/eissn"));
			this.Bibo_editor = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/editor"));
			this.Bibo_issn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issn"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public MainDocument(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_impactIndex = new List<ImpactIndex>();
			SemanticPropertyModel propRoh_impactIndex = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndex");
			if(propRoh_impactIndex != null && propRoh_impactIndex.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_impactIndex.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						ImpactIndex roh_impactIndex = new ImpactIndex(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_impactIndex.Add(roh_impactIndex);
					}
				}
			}
			SemanticPropertyModel propRoh_format = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/format");
			if(propRoh_format != null && propRoh_format.PropertyValues.Count > 0)
			{
				this.Roh_format = new DocumentFormat(propRoh_format.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_eissn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/eissn"));
			this.Bibo_editor = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/editor"));
			this.Bibo_issn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issn"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/MainDocument"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/MainDocument"; } }
		[RDFProperty("http://w3id.org/roh/impactIndex")]
		public  List<ImpactIndex> Roh_impactIndex { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/DocumentFormat")]
		[RDFProperty("http://w3id.org/roh/format")]
		public  DocumentFormat Roh_format  { get; set;} 
		public string IdRoh_format  { get; set;} 

		[RDFProperty("http://purl.org/ontology/bibo/eissn")]
		public  string Bibo_eissn { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/editor")]
		public  string Bibo_editor { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/issn")]
		public  string Bibo_issn { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:format", this.IdRoh_format));
			propList.Add(new StringOntologyProperty("bibo:eissn", this.Bibo_eissn));
			propList.Add(new StringOntologyProperty("bibo:editor", this.Bibo_editor));
			propList.Add(new StringOntologyProperty("bibo:issn", this.Bibo_issn));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_impactIndex!=null){
				foreach(ImpactIndex prop in Roh_impactIndex){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityImpactIndex = new OntologyEntity("http://w3id.org/roh/ImpactIndex", "http://w3id.org/roh/ImpactIndex", "roh:impactIndex", prop.propList, prop.entList);
				entList.Add(entityImpactIndex);
				prop.Entity= entityImpactIndex;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MainDocument_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/MainDocument>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MainDocument_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/MainDocument\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/MainDocument_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_impactIndex != null)
			{
			foreach(var item0 in this.Roh_impactIndex)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ImpactIndex>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ImpactIndex\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MainDocument_{ResourceID}_{ArticleID}", "http://w3id.org/roh/impactIndex", $"<{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Roh_impactCategory != null)
			{
			foreach(var item1 in item0.Roh_impactCategory)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactCategory_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ImpactCategory>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactCategory_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ImpactCategory\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ImpactCategory_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/impactCategory", $"<{resourceAPI.GraphsUrl}items/ImpactCategory_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_journalNumberInCat != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactCategory_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/journalNumberInCat", $"{item1.Roh_journalNumberInCat.Value.ToString()}", list, " . ");
				}
				if(item1.Roh_publicationPosition != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactCategory_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/publicationPosition", $"{item1.Roh_publicationPosition.Value.ToString()}", list, " . ");
				}
				if(item1.Roh_quartile != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactCategory_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/quartile", $"{item1.Roh_quartile.ToString()}", list, " . ");
				}
				if(item1.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactCategory_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_title)}\"", list, " . ");
				}
			}
			}
				if(item0.Roh_impactSourceOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactSourceOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_impactSourceOther)}\"", list, " . ");
				}
				if(item0.Roh_impactIndexInYear != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactIndexInYear", $"{item0.Roh_impactIndexInYear.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(item0.Roh_year != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/year", $"{item0.Roh_year.ToString()}", list, " . ");
				}
				if(item0.IdRoh_impactSource != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactSource", $"<{item0.IdRoh_impactSource}>", list, " . ");
				}
			}
			}
				if(this.IdRoh_format != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MainDocument_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/format", $"<{this.IdRoh_format}>", list, " . ");
				}
				if(this.Bibo_eissn != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MainDocument_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/eissn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_eissn)}\"", list, " . ");
				}
				if(this.Bibo_editor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MainDocument_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/editor", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_editor)}\"", list, " . ");
				}
				if(this.Bibo_issn != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MainDocument_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/issn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issn)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MainDocument_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"maindocument\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/MainDocument\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_impactIndex != null)
			{
			foreach(var item0 in this.Roh_impactIndex)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/impactIndex", $"<{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Roh_impactCategory != null)
			{
			foreach(var item1 in item0.Roh_impactCategory)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/impactCategory", $"<{resourceAPI.GraphsUrl}items/impactcategory_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_journalNumberInCat != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactcategory_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/journalNumberInCat", $"{item1.Roh_journalNumberInCat.Value.ToString()}", list, " . ");
				}
				if(item1.Roh_publicationPosition != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactcategory_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/publicationPosition", $"{item1.Roh_publicationPosition.Value.ToString()}", list, " . ");
				}
				if(item1.Roh_quartile != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactcategory_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/quartile", $"{item1.Roh_quartile.ToString()}", list, " . ");
				}
				if(item1.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactcategory_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			}
				if(item0.Roh_impactSourceOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactSourceOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_impactSourceOther).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_impactIndexInYear != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactIndexInYear", $"{item0.Roh_impactIndexInYear.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(item0.Roh_year != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/year", $"{item0.Roh_year.ToString()}", list, " . ");
				}
				if(item0.IdRoh_impactSource != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_impactSource;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactSource", $"<{itemRegex}>", list, " . ");
				}
			}
			}
				if(this.IdRoh_format != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_format;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/format", $"<{itemRegex}>", list, " . ");
				}
				if(this.Bibo_eissn != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/eissn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_eissn).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_editor != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/editor", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_editor).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_issn != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/issn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issn).ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/MaindocumentOntology_{ResourceID}_{ArticleID}";
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
