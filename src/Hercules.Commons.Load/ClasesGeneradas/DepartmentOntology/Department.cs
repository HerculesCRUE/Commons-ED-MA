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

namespace DepartmentOntology
{
	[ExcludeFromCodeCoverage]
	public class Department : GnossOCBase
	{

		public Department() : base() { } 

		public Department(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Dc_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/title"));
			this.Dc_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/identifier"));
		}

		public virtual string RdfType { get { return "http://vivoweb.org/ontology/core#Department"; } }
		public virtual string RdfsLabel { get { return "http://vivoweb.org/ontology/core#Department"; } }
		[LABEL(LanguageEnum.es,"http://purl.org/dc/elements/1.1/title")]
		[RDFProperty("http://purl.org/dc/elements/1.1/title")]
		public  string Dc_title { get; set;}

		[LABEL(LanguageEnum.es,"http://purl.org/dc/elements/1.1/identifier")]
		[RDFProperty("http://purl.org/dc/elements/1.1/identifier")]
		public  string Dc_identifier { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("dc:title", this.Dc_title));
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
			SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "http://vivoweb.org/ontology/core#Department", "http://vivoweb.org/ontology/core#Department", prefList, propList,identificador,listSecondaryEntity, null);
			resource.SecondaryOntology = ontology;
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Department_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://vivoweb.org/ontology/core#Department>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Department_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://vivoweb.org/ontology/core#Department\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Department_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Department_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title)}\"", list, " . ");
				}
				if(this.Dc_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Department_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/identifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_identifier)}\"", list, " . ");
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
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_title).ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/DepartmentOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
		}





	}
}
