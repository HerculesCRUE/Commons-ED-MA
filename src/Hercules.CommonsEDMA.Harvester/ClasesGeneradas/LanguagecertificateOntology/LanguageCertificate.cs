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
using LanguageLevel = LanguagelevelOntology.LanguageLevel;
using Language = LanguageOntology.Language;

namespace LanguagecertificateOntology
{
	[ExcludeFromCodeCoverage]
	public class LanguageCertificate : GnossOCBase
	{

		public LanguageCertificate() : base() { } 

		public LanguageCertificate(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_listeningSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/listeningSkill");
			if(propRoh_listeningSkill != null && propRoh_listeningSkill.PropertyValues.Count > 0)
			{
				this.Roh_listeningSkill = new LanguageLevel(propRoh_listeningSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_writingSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/writingSkill");
			if(propRoh_writingSkill != null && propRoh_writingSkill.PropertyValues.Count > 0)
			{
				this.Roh_writingSkill = new LanguageLevel(propRoh_writingSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_speakingSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/speakingSkill");
			if(propRoh_speakingSkill != null && propRoh_speakingSkill.PropertyValues.Count > 0)
			{
				this.Roh_speakingSkill = new LanguageLevel(propRoh_speakingSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_readingSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/readingSkill");
			if(propRoh_readingSkill != null && propRoh_readingSkill.PropertyValues.Count > 0)
			{
				this.Roh_readingSkill = new LanguageLevel(propRoh_readingSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_spokingInteractionSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/spokingInteractionSkill");
			if(propRoh_spokingInteractionSkill != null && propRoh_spokingInteractionSkill.PropertyValues.Count > 0)
			{
				this.Roh_spokingInteractionSkill = new LanguageLevel(propRoh_spokingInteractionSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			SemanticPropertyModel propRoh_languageOfTheCertificate = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/languageOfTheCertificate");
			if(propRoh_languageOfTheCertificate != null && propRoh_languageOfTheCertificate.PropertyValues.Count > 0)
			{
				this.Roh_languageOfTheCertificate = new Language(propRoh_languageOfTheCertificate.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public LanguageCertificate(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_listeningSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/listeningSkill");
			if(propRoh_listeningSkill != null && propRoh_listeningSkill.PropertyValues.Count > 0)
			{
				this.Roh_listeningSkill = new LanguageLevel(propRoh_listeningSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_writingSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/writingSkill");
			if(propRoh_writingSkill != null && propRoh_writingSkill.PropertyValues.Count > 0)
			{
				this.Roh_writingSkill = new LanguageLevel(propRoh_writingSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_speakingSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/speakingSkill");
			if(propRoh_speakingSkill != null && propRoh_speakingSkill.PropertyValues.Count > 0)
			{
				this.Roh_speakingSkill = new LanguageLevel(propRoh_speakingSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_readingSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/readingSkill");
			if(propRoh_readingSkill != null && propRoh_readingSkill.PropertyValues.Count > 0)
			{
				this.Roh_readingSkill = new LanguageLevel(propRoh_readingSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_spokingInteractionSkill = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/spokingInteractionSkill");
			if(propRoh_spokingInteractionSkill != null && propRoh_spokingInteractionSkill.PropertyValues.Count > 0)
			{
				this.Roh_spokingInteractionSkill = new LanguageLevel(propRoh_spokingInteractionSkill.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			SemanticPropertyModel propRoh_languageOfTheCertificate = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/languageOfTheCertificate");
			if(propRoh_languageOfTheCertificate != null && propRoh_languageOfTheCertificate.PropertyValues.Count > 0)
			{
				this.Roh_languageOfTheCertificate = new Language(propRoh_languageOfTheCertificate.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/LanguageCertificate"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/LanguageCertificate"; } }
		[RDFProperty("http://w3id.org/roh/owner")]
		public  Person Roh_owner  { get; set;} 
		public string IdRoh_owner  { get; set;} 

		[RDFProperty("http://w3id.org/roh/listeningSkill")]
		public  LanguageLevel Roh_listeningSkill  { get; set;} 
		public string IdRoh_listeningSkill  { get; set;} 

		[RDFProperty("http://w3id.org/roh/writingSkill")]
		public  LanguageLevel Roh_writingSkill  { get; set;} 
		public string IdRoh_writingSkill  { get; set;} 

		[RDFProperty("http://w3id.org/roh/speakingSkill")]
		public  LanguageLevel Roh_speakingSkill  { get; set;} 
		public string IdRoh_speakingSkill  { get; set;} 

		[RDFProperty("http://w3id.org/roh/readingSkill")]
		public  LanguageLevel Roh_readingSkill  { get; set;} 
		public string IdRoh_readingSkill  { get; set;} 

		[RDFProperty("http://w3id.org/roh/spokingInteractionSkill")]
		public  LanguageLevel Roh_spokingInteractionSkill  { get; set;} 
		public string IdRoh_spokingInteractionSkill  { get; set;} 

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://w3id.org/roh/cvnCode")]
		public  string Roh_cvnCode { get; set;}

		[RDFProperty("http://w3id.org/roh/languageOfTheCertificate")]
		[Required]
		public  Language Roh_languageOfTheCertificate  { get; set;} 
		public string IdRoh_languageOfTheCertificate  { get; set;} 


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:listeningSkill", this.IdRoh_listeningSkill));
			propList.Add(new StringOntologyProperty("roh:writingSkill", this.IdRoh_writingSkill));
			propList.Add(new StringOntologyProperty("roh:speakingSkill", this.IdRoh_speakingSkill));
			propList.Add(new StringOntologyProperty("roh:readingSkill", this.IdRoh_readingSkill));
			propList.Add(new StringOntologyProperty("roh:spokingInteractionSkill", this.IdRoh_spokingInteractionSkill));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:languageOfTheCertificate", this.IdRoh_languageOfTheCertificate));
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/LanguageCertificate>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/LanguageCertificate\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.IdRoh_listeningSkill != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/listeningSkill", $"<{this.IdRoh_listeningSkill}>", list, " . ");
				}
				if(this.IdRoh_writingSkill != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/writingSkill", $"<{this.IdRoh_writingSkill}>", list, " . ");
				}
				if(this.IdRoh_speakingSkill != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/speakingSkill", $"<{this.IdRoh_speakingSkill}>", list, " . ");
				}
				if(this.IdRoh_readingSkill != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/readingSkill", $"<{this.IdRoh_readingSkill}>", list, " . ");
				}
				if(this.IdRoh_spokingInteractionSkill != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/spokingInteractionSkill", $"<{this.IdRoh_spokingInteractionSkill}>", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.IdRoh_languageOfTheCertificate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/LanguageCertificate_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/languageOfTheCertificate", $"<{this.IdRoh_languageOfTheCertificate}>", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"languagecertificate\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/LanguageCertificate\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
			string search = string.Empty;
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
				if(this.IdRoh_listeningSkill != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_listeningSkill;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/listeningSkill", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_writingSkill != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_writingSkill;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/writingSkill", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_speakingSkill != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_speakingSkill;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/speakingSkill", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_readingSkill != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_readingSkill;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/readingSkill", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_spokingInteractionSkill != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_spokingInteractionSkill;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/spokingInteractionSkill", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode).ToLower()}\"", list, " . ");
				}
				if(this.IdRoh_languageOfTheCertificate != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_languageOfTheCertificate;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/languageOfTheCertificate", $"<{itemRegex}>", list, " . ");
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
			string titulo = $"{this.Roh_crisIdentifier.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Roh_crisIdentifier.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/LanguagecertificateOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Roh_crisIdentifier;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Roh_crisIdentifier;
		}




	}
}
