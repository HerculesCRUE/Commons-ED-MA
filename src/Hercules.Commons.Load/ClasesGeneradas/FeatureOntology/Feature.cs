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

namespace FeatureOntology
{
	[ExcludeFromCodeCoverage]
	public class Feature : GnossOCBase
	{

		public Feature() : base() { } 

		public Feature(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propGn_parentFeature = pSemCmsModel.GetPropertyByPath("http://www.geonames.org/ontology#parentFeature");
			if(propGn_parentFeature != null && propGn_parentFeature.PropertyValues.Count > 0)
			{
				this.Gn_parentFeature = new Feature(propGn_parentFeature.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Dc_identifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/identifier"));
			this.Gn_name = new Dictionary<LanguageEnum,string>();
			this.Gn_name.Add(idiomaUsuario , GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.geonames.org/ontology#name")));
			
			this.Gn_featureCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.geonames.org/ontology#featureCode"));
		}

		public virtual string RdfType { get { return "http://www.geonames.org/ontology#Feature"; } }
		public virtual string RdfsLabel { get { return "http://www.geonames.org/ontology#Feature"; } }
		[LABEL(LanguageEnum.es,"Rasgo padre")]
		[RDFProperty("http://www.geonames.org/ontology#parentFeature")]
		public  Feature Gn_parentFeature  { get; set;} 
		public string IdGn_parentFeature  { get; set;} 

		[LABEL(LanguageEnum.es,"Identificador")]
		[RDFProperty("http://purl.org/dc/elements/1.1/identifier")]
		public  string Dc_identifier { get; set;}

		[LABEL(LanguageEnum.es,"Nombre")]
		[RDFProperty("http://www.geonames.org/ontology#name")]
		public  Dictionary<LanguageEnum,string> Gn_name { get; set;}

		[LABEL(LanguageEnum.es,"Código de rasgo")]
		[RDFProperty("http://www.geonames.org/ontology#featureCode")]
		public  string Gn_featureCode { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("gn:parentFeature", this.IdGn_parentFeature));
			propList.Add(new StringOntologyProperty("dc:identifier", this.Dc_identifier));
			if(this.Gn_name != null)
			{
				foreach (LanguageEnum idioma in this.Gn_name.Keys)
				{
					propList.Add(new StringOntologyProperty("gn:name", this.Gn_name[idioma], idioma.ToString()));
				}
			}
			else
			{
				throw new GnossAPIException($"La propiedad gn:name debe tener al menos un valor en el recurso: {resourceID}");
			}
			propList.Add(new StringOntologyProperty("gn:featureCode", this.Gn_featureCode));
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
			SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "http://www.geonames.org/ontology#Feature", "http://www.geonames.org/ontology#Feature", prefList, propList,identificador,listSecondaryEntity, null);
			resource.SecondaryOntology = ontology;
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://www.geonames.org/ontology#Feature>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://www.geonames.org/ontology#Feature\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdGn_parentFeature != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{ArticleID}",  "http://www.geonames.org/ontology#parentFeature", $"<{this.IdGn_parentFeature}>", list, " . ");
				}
				if(this.Dc_identifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/identifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_identifier)}\"", list, " . ");
				}
				if(this.Gn_name != null)
				{
							foreach (LanguageEnum idioma in this.Gn_name.Keys)
							{
								AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{ArticleID}", "http://www.geonames.org/ontology#name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Gn_name[idioma])}\"", list,  $"{idioma} . ");
							}
				}
				if(this.Gn_featureCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Feature_{ResourceID}_{ArticleID}",  "http://www.geonames.org/ontology#featureCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Gn_featureCode)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			string search = string.Empty;
				if(this.IdGn_parentFeature != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdGn_parentFeature;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.geonames.org/ontology#parentFeature", $"<{itemRegex}>", list, " . ");
				}
				if(this.Dc_identifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/elements/1.1/identifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_identifier).ToLower()}\"", list, " . ");
				}
				if(this.Gn_name != null)
				{
							foreach (LanguageEnum idioma in this.Gn_name.Keys)
							{
								AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.geonames.org/ontology#name",  $"\"{GenerarTextoSinSaltoDeLinea(this.Gn_name[idioma]).ToLower()}\"", list,  $"{idioma} . ");
							}
				}
				if(this.Gn_featureCode != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://www.geonames.org/ontology#featureCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Gn_featureCode).ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/FeatureOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
		}





	}
}
