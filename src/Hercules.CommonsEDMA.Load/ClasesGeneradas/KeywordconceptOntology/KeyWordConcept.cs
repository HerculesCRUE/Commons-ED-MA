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
using KeyWordConcept = KeywordconceptOntology.KeyWordConcept;

namespace KeywordconceptOntology
{
	[ExcludeFromCodeCoverage]
	public class KeyWordConcept : GnossOCBase
	{

		public KeyWordConcept() : base() { } 

		public KeyWordConcept(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Roh_broaders = new List<KeyWordConcept>();
			SemanticPropertyModel propRoh_broaders = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/broaders");
			if(propRoh_broaders != null && propRoh_broaders.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_broaders.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept roh_broaders = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_broaders.Add(roh_broaders);
					}
				}
			}
			this.Skos_closeMatch = new List<KeyWordConcept>();
			SemanticPropertyModel propSkos_closeMatch = pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#closeMatch");
			if(propSkos_closeMatch != null && propSkos_closeMatch.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSkos_closeMatch.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept skos_closeMatch = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Skos_closeMatch.Add(skos_closeMatch);
					}
				}
			}
			this.Skos_exactMatch = new List<KeyWordConcept>();
			SemanticPropertyModel propSkos_exactMatch = pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#exactMatch");
			if(propSkos_exactMatch != null && propSkos_exactMatch.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSkos_exactMatch.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept skos_exactMatch = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Skos_exactMatch.Add(skos_exactMatch);
					}
				}
			}
			this.Roh_relatedTo = new List<KeyWordConcept>();
			SemanticPropertyModel propRoh_relatedTo = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relatedTo");
			if(propRoh_relatedTo != null && propRoh_relatedTo.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_relatedTo.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept roh_relatedTo = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_relatedTo.Add(roh_relatedTo);
					}
				}
			}
			this.Roh_match = new List<KeyWordConcept>();
			SemanticPropertyModel propRoh_match = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/match");
			if(propRoh_match != null && propRoh_match.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_match.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept roh_match = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_match.Add(roh_match);
					}
				}
			}
			this.Roh_qualifiers = new List<KeyWordConcept>();
			SemanticPropertyModel propRoh_qualifiers = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualifiers");
			if(propRoh_qualifiers != null && propRoh_qualifiers.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_qualifiers.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept roh_qualifiers = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_qualifiers.Add(roh_qualifiers);
					}
				}
			}
			this.Roh_url = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/url"));
			this.Roh_type = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/type"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public KeyWordConcept(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_broaders = new List<KeyWordConcept>();
			SemanticPropertyModel propRoh_broaders = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/broaders");
			if(propRoh_broaders != null && propRoh_broaders.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_broaders.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept roh_broaders = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_broaders.Add(roh_broaders);
					}
				}
			}
			this.Skos_closeMatch = new List<KeyWordConcept>();
			SemanticPropertyModel propSkos_closeMatch = pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#closeMatch");
			if(propSkos_closeMatch != null && propSkos_closeMatch.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSkos_closeMatch.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept skos_closeMatch = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Skos_closeMatch.Add(skos_closeMatch);
					}
				}
			}
			this.Skos_exactMatch = new List<KeyWordConcept>();
			SemanticPropertyModel propSkos_exactMatch = pSemCmsModel.GetPropertyByPath("http://www.w3.org/2008/05/skos#exactMatch");
			if(propSkos_exactMatch != null && propSkos_exactMatch.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propSkos_exactMatch.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept skos_exactMatch = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Skos_exactMatch.Add(skos_exactMatch);
					}
				}
			}
			this.Roh_relatedTo = new List<KeyWordConcept>();
			SemanticPropertyModel propRoh_relatedTo = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relatedTo");
			if(propRoh_relatedTo != null && propRoh_relatedTo.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_relatedTo.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept roh_relatedTo = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_relatedTo.Add(roh_relatedTo);
					}
				}
			}
			this.Roh_match = new List<KeyWordConcept>();
			SemanticPropertyModel propRoh_match = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/match");
			if(propRoh_match != null && propRoh_match.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_match.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept roh_match = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_match.Add(roh_match);
					}
				}
			}
			this.Roh_qualifiers = new List<KeyWordConcept>();
			SemanticPropertyModel propRoh_qualifiers = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualifiers");
			if(propRoh_qualifiers != null && propRoh_qualifiers.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_qualifiers.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWordConcept roh_qualifiers = new KeyWordConcept(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_qualifiers.Add(roh_qualifiers);
					}
				}
			}
			this.Roh_url = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/url"));
			this.Roh_type = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/type"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/KeyWordConcept"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/KeyWordConcept"; } }
		[RDFProperty("http://w3id.org/roh/broaders")]
		public  List<KeyWordConcept> Roh_broaders { get; set;}
		public List<string> IdsRoh_broaders { get; set;}

		[RDFProperty("http://www.w3.org/2008/05/skos#closeMatch")]
		public  List<KeyWordConcept> Skos_closeMatch { get; set;}
		public List<string> IdsSkos_closeMatch { get; set;}

		[RDFProperty("http://www.w3.org/2008/05/skos#exactMatch")]
		public  List<KeyWordConcept> Skos_exactMatch { get; set;}
		public List<string> IdsSkos_exactMatch { get; set;}

		[RDFProperty("http://w3id.org/roh/relatedTo")]
		public  List<KeyWordConcept> Roh_relatedTo { get; set;}
		public List<string> IdsRoh_relatedTo { get; set;}

		[RDFProperty("http://w3id.org/roh/match")]
		public  List<KeyWordConcept> Roh_match { get; set;}
		public List<string> IdsRoh_match { get; set;}

		[RDFProperty("http://w3id.org/roh/qualifiers")]
		public  List<KeyWordConcept> Roh_qualifiers { get; set;}
		public List<string> IdsRoh_qualifiers { get; set;}

		[RDFProperty("http://w3id.org/roh/url")]
		public  string Roh_url { get; set;}

		[RDFProperty("http://w3id.org/roh/type")]
		public  string Roh_type { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("roh:broaders", this.IdsRoh_broaders));
			propList.Add(new ListStringOntologyProperty("skos:closeMatch", this.IdsSkos_closeMatch));
			propList.Add(new ListStringOntologyProperty("skos:exactMatch", this.IdsSkos_exactMatch));
			propList.Add(new ListStringOntologyProperty("roh:relatedTo", this.IdsRoh_relatedTo));
			propList.Add(new ListStringOntologyProperty("roh:match", this.IdsRoh_match));
			propList.Add(new ListStringOntologyProperty("roh:qualifiers", this.IdsRoh_qualifiers));
			propList.Add(new StringOntologyProperty("roh:url", this.Roh_url));
			propList.Add(new StringOntologyProperty("roh:type", this.Roh_type));
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
			AddResourceDescription(resource);
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/KeyWordConcept>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/KeyWordConcept\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdsRoh_broaders != null)
				{
					foreach(var item2 in this.IdsRoh_broaders)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}", "http://w3id.org/roh/broaders", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSkos_closeMatch != null)
				{
					foreach(var item2 in this.IdsSkos_closeMatch)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}", "http://www.w3.org/2008/05/skos#closeMatch", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsSkos_exactMatch != null)
				{
					foreach(var item2 in this.IdsSkos_exactMatch)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}", "http://www.w3.org/2008/05/skos#exactMatch", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsRoh_relatedTo != null)
				{
					foreach(var item2 in this.IdsRoh_relatedTo)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}", "http://w3id.org/roh/relatedTo", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsRoh_match != null)
				{
					foreach(var item2 in this.IdsRoh_match)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}", "http://w3id.org/roh/match", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsRoh_qualifiers != null)
				{
					foreach(var item2 in this.IdsRoh_qualifiers)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}", "http://w3id.org/roh/qualifiers", $"<{item2}>", list, " . ");
					}
				}
				if(this.Roh_url != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/url", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_url)}\"", list, " . ");
				}
				if(this.Roh_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/type", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_type)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWordConcept_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"keywordconcept\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/KeyWordConcept\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
				if(this.IdsRoh_broaders != null)
				{
					foreach(var item2 in this.IdsRoh_broaders)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/broaders", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSkos_closeMatch != null)
				{
					foreach(var item2 in this.IdsSkos_closeMatch)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/2008/05/skos#closeMatch", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsSkos_exactMatch != null)
				{
					foreach(var item2 in this.IdsSkos_exactMatch)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/2008/05/skos#exactMatch", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsRoh_relatedTo != null)
				{
					foreach(var item2 in this.IdsRoh_relatedTo)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/relatedTo", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsRoh_match != null)
				{
					foreach(var item2 in this.IdsRoh_match)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/match", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsRoh_qualifiers != null)
				{
					foreach(var item2 in this.IdsRoh_qualifiers)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/qualifiers", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Roh_url != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/url", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_url).ToLower()}\"", list, " . ");
				}
				if(this.Roh_type != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/type", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_type).ToLower()}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title).ToLower()}\"", list, " . ");
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
			string titulo = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/KeywordconceptOntology_{ResourceID}_{ArticleID}";
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
