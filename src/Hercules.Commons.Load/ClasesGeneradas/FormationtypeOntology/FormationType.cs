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

namespace FormationtypeOntology
{
	[ExcludeFromCodeCoverage]
	public class FormationType : GnossOCBase
	{

		public FormationType() : base() { } 

		public FormationType(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Dc_title = new Dictionary<LanguageEnum,string>();
			this.Dc_title.Add(idiomaUsuario , GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/title")));
			
			this.Dc_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/identifier"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/FormationType"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/FormationType"; } }
		[RDFProperty("http://purl.org/dc/elements/1.1/title")]
		public  Dictionary<LanguageEnum,string> Dc_title { get; set;}

		[RDFProperty("http://purl.org/dc/elements/1.1/identifier")]
		public  string Dc_identifier { get; set;}


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
			SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "http://w3id.org/roh/FormationType", "http://w3id.org/roh/FormationType", prefList, propList,identificador,listSecondaryEntity, null);
			resource.SecondaryOntology = ontology;
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FormationType_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/FormationType>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FormationType_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/FormationType\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/FormationType_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.Dc_title != null)
				{
							foreach (LanguageEnum idioma in this.Dc_title.Keys)
							{
								AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FormationType_{ResourceID}_{ArticleID}", "http://purl.org/dc/elements/1.1/title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title[idioma])}\"", list,  $"{idioma} . ");
							}
				}
				if(this.Dc_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FormationType_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/identifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_identifier)}\"", list, " . ");
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
								AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/dc/elements/1.1/title",  $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title[idioma]).ToLower()}\"", list,  $"{idioma} . ");
							}
				}
				if(this.Dc_identifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/elements/1.1/identifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_identifier).ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/FormationtypeOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
		}





	}
}
