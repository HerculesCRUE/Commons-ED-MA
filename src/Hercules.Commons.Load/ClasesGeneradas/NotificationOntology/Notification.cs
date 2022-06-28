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

namespace NotificationOntology
{
	[ExcludeFromCodeCoverage]
	public class Notification : GnossOCBase
	{

		public Notification() : base() { } 

		public Notification(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_entity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/entity");
			if(propRoh_entity != null && propRoh_entity.PropertyValues.Count > 0)
			{
				this.Roh_entity = new Person(propRoh_entity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_trigger = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trigger");
			if(propRoh_trigger != null && propRoh_trigger.PropertyValues.Count > 0)
			{
				this.Roh_trigger = new Person(propRoh_trigger.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_idEntityCV = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/idEntityCV"));
			this.Roh_tabPropertyCV = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/tabPropertyCV"));
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
var item0 = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
if(item0.HasValue){
			this.Dct_issued = item0.Value;
}
			this.Roh_type = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/type"));
		}

		public Notification(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_entity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/entity");
			if(propRoh_entity != null && propRoh_entity.PropertyValues.Count > 0)
			{
				this.Roh_entity = new Person(propRoh_entity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_trigger = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trigger");
			if(propRoh_trigger != null && propRoh_trigger.PropertyValues.Count > 0)
			{
				this.Roh_trigger = new Person(propRoh_trigger.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_idEntityCV = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/idEntityCV"));
			this.Roh_tabPropertyCV = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/tabPropertyCV"));
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
var item1 = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
if(item1.HasValue){
			this.Dct_issued = item1.Value;
}
			this.Roh_type = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/type"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/Notification"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/Notification"; } }
		[RDFProperty("http://w3id.org/roh/entity")]
		public  Person Roh_entity  { get; set;} 
		public string IdRoh_entity  { get; set;} 

		[RDFProperty("http://w3id.org/roh/trigger")]
		public  Person Roh_trigger  { get; set;} 
		public string IdRoh_trigger  { get; set;} 

		[RDFProperty("http://w3id.org/roh/idEntityCV")]
		public  string Roh_idEntityCV { get; set;}

		[RDFProperty("http://w3id.org/roh/tabPropertyCV")]
		public  string Roh_tabPropertyCV { get; set;}

		[RDFProperty("http://w3id.org/roh/cvnCode")]
		public  string Roh_cvnCode { get; set;}

		[RDFProperty("http://w3id.org/roh/owner")]
		[Required]
		public  Person Roh_owner  { get; set;} 
		public string IdRoh_owner  { get; set;} 

		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime Dct_issued { get; set;}

		[RDFProperty("http://w3id.org/roh/type")]
		public  string Roh_type { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:entity", this.IdRoh_entity));
			propList.Add(new StringOntologyProperty("roh:trigger", this.IdRoh_trigger));
			propList.Add(new StringOntologyProperty("roh:idEntityCV", this.Roh_idEntityCV));
			propList.Add(new StringOntologyProperty("roh:tabPropertyCV", this.Roh_tabPropertyCV));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued));
			propList.Add(new StringOntologyProperty("roh:type", this.Roh_type));
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
			AddResourceDescription(resource);
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Notification>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Notification\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdRoh_entity != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/entity", $"<{this.IdRoh_entity}>", list, " . ");
				}
				if(this.IdRoh_trigger != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/trigger", $"<{this.IdRoh_trigger}>", list, " . ");
				}
				if(this.Roh_idEntityCV != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/idEntityCV", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_idEntityCV)}\"", list, " . ");
				}
				if(this.Roh_tabPropertyCV != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/tabPropertyCV", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_tabPropertyCV)}\"", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Dct_issued != null && this.Dct_issued != DateTime.MinValue)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{this.Dct_issued.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Notification_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/type", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_type)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"notification\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/Notification\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_type)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_type)}\"", list, " . ");
			string search = string.Empty;
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
				if(this.IdRoh_trigger != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_trigger;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/trigger", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_idEntityCV != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/idEntityCV", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_idEntityCV).ToLower()}\"", list, " . ");
				}
				if(this.Roh_tabPropertyCV != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/tabPropertyCV", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_tabPropertyCV).ToLower()}\"", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode).ToLower()}\"", list, " . ");
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
				if(this.Dct_issued != null && this.Dct_issued != DateTime.MinValue)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/terms/issued", $"{this.Dct_issued.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_type != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/type", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_type).ToLower()}\"", list, " . ");
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
			string titulo = $"{this.Roh_type.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Roh_type.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/NotificationOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Roh_type;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Roh_type;
		}




	}
}
