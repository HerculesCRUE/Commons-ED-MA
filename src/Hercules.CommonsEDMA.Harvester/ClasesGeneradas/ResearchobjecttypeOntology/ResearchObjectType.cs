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

namespace ResearchobjecttypeOntology
{
	[ExcludeFromCodeCoverage]
	public class ResearchObjectType : GnossOCBase
	{

		public ResearchObjectType() : base() { } 

		public ResearchObjectType(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Dc_title = new Dictionary<LanguageEnum,string>();
			this.Dc_title.Add(idiomaUsuario , GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/title")));
			
			this.Dc_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/identifier"));
			this.Dc_type = new Dictionary<LanguageEnum,string>();
			this.Dc_type.Add(idiomaUsuario , GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/type")));
			
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ResearchObjectType"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ResearchObjectType"; } }
		[LABEL(LanguageEnum.es,"Tipo de publicación")]
		[RDFProperty("http://purl.org/dc/elements/1.1/title")]
		public  Dictionary<LanguageEnum,string> Dc_title { get; set;}

		[LABEL(LanguageEnum.es,"Identificador del tipo de publicación")]
		[RDFProperty("http://purl.org/dc/elements/1.1/identifier")]
		public  string Dc_identifier { get; set;}

		[RDFProperty("http://purl.org/dc/elements/1.1/type")]
		public  Dictionary<LanguageEnum,string> Dc_type { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			if(this.Dc_title != null)
			{
				foreach (LanguageEnum idioma in this.Dc_title.Keys)
				{
					propList.Add(new StringOntologyProperty("dc:title", this.Dc_title[idioma], idioma.ToString()));
				}
			}
			else
			{
				throw new GnossAPIException($"La propiedad dc:title debe tener al menos un valor en el recurso: {resourceID}");
			}
			propList.Add(new StringOntologyProperty("dc:identifier", this.Dc_identifier));
			if(this.Dc_type != null)
			{
				foreach (LanguageEnum idioma in this.Dc_type.Keys)
				{
					propList.Add(new StringOntologyProperty("dc:type", this.Dc_type[idioma], idioma.ToString()));
				}
			}
			else
			{
				throw new GnossAPIException($"La propiedad dc:type debe tener al menos un valor en el recurso: {resourceID}");
			}
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
		public virtual SecondaryResource ToGnossApiResource(ResourceApi resourceAPI,string identificador)
		{
			SecondaryResource resource = new SecondaryResource();
			List<SecondaryEntity> listSecondaryEntity = null;
			GetProperties();
			SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "http://w3id.org/roh/ResearchObjectType", "http://w3id.org/roh/ResearchObjectType", prefList, propList,identificador,listSecondaryEntity, null);
			resource.SecondaryOntology = ontology;
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObjectType_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ResearchObjectType>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObjectType_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ResearchObjectType\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ResearchObjectType_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.Dc_title != null)
				{
							foreach (LanguageEnum idioma in this.Dc_title.Keys)
							{
								AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObjectType_{ResourceID}_{ArticleID}", "http://purl.org/dc/elements/1.1/title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title[idioma])}\"", list,  $"@{idioma} . ");
							}
				}
				if(this.Dc_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObjectType_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/identifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_identifier)}\"", list, " . ");
				}
				if(this.Dc_type != null)
				{
							foreach (LanguageEnum idioma in this.Dc_type.Keys)
							{
								AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObjectType_{ResourceID}_{ArticleID}", "http://purl.org/dc/elements/1.1/type",  $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_type[idioma])}\"", list,  $"@{idioma} . ");
							}
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			string search = string.Empty;
				if(this.Dc_title != null)
				{
							foreach (LanguageEnum idioma in this.Dc_title.Keys)
							{
								AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/dc/elements/1.1/title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title[idioma]).ToLower()}\"", list,  $"@{idioma} . ");
							}
				}
				if(this.Dc_identifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/elements/1.1/identifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_identifier).ToLower()}\"", list, " . ");
				}
				if(this.Dc_type != null)
				{
							foreach (LanguageEnum idioma in this.Dc_type.Keys)
							{
								AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/dc/elements/1.1/type",  $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_type[idioma]).ToLower()}\"", list,  $"@{idioma} . ");
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
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>();

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/ResearchobjecttypeOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
		}





	}
}
