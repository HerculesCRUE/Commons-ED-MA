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
using Organization = OrganizationOntology.Organization;
using Feature = FeatureOntology.Feature;
using ProjectCharacterType = ProjectcharactertypeOntology.ProjectCharacterType;
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using Person = PersonOntology.Person;

namespace ThesissupervisionOntology
{
	[ExcludeFromCodeCoverage]
	public class ThesisSupervision : GnossOCBase
	{

		public ThesisSupervision() : base() { } 

		public ThesisSupervision(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_promotedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedBy");
			if(propRoh_promotedBy != null && propRoh_promotedBy.PropertyValues.Count > 0)
			{
				this.Roh_promotedBy = new Organization(propRoh_promotedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_projectCharacterType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectCharacterType");
			if(propRoh_projectCharacterType != null && propRoh_projectCharacterType.PropertyValues.Count > 0)
			{
				this.Roh_projectCharacterType = new ProjectCharacterType(propRoh_projectCharacterType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByType");
			if(propRoh_promotedByType != null && propRoh_promotedByType.PropertyValues.Count > 0)
			{
				this.Roh_promotedByType = new OrganizationType(propRoh_promotedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_codirector = new List<PersonAux>();
			SemanticPropertyModel propRoh_codirector = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/codirector");
			if(propRoh_codirector != null && propRoh_codirector.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_codirector.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PersonAux roh_codirector = new PersonAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_codirector.Add(roh_codirector);
					}
				}
			}
			this.Vivo_freeTextKeyword = new List<CategoryPath>();
			SemanticPropertyModel propVivo_freeTextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeyword");
			if(propVivo_freeTextKeyword != null && propVivo_freeTextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeyword.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath vivo_freeTextKeyword = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_freeTextKeyword.Add(vivo_freeTextKeyword);
					}
				}
			}
			this.Roh_studentSecondSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/studentSecondSurname"));
			this.Roh_qualification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualification"));
			this.Roh_projectCharacterTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectCharacterTypeOther"));
			this.Roh_studentFirstSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/studentFirstSurname"));
			this.Roh_promotedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTypeOther"));
			this.Roh_europeanDoctorateDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanDoctorateDate"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_europeanDoctorate= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanDoctorate"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_qualityMentionDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualityMentionDate"));
			this.Roh_promotedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTitle"));
			this.Roh_studentNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/studentNick"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_studentName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/studentName"));
			this.Roh_qualityMention= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualityMention"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public ThesisSupervision(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_promotedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedBy");
			if(propRoh_promotedBy != null && propRoh_promotedBy.PropertyValues.Count > 0)
			{
				this.Roh_promotedBy = new Organization(propRoh_promotedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_projectCharacterType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectCharacterType");
			if(propRoh_projectCharacterType != null && propRoh_projectCharacterType.PropertyValues.Count > 0)
			{
				this.Roh_projectCharacterType = new ProjectCharacterType(propRoh_projectCharacterType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByType");
			if(propRoh_promotedByType != null && propRoh_promotedByType.PropertyValues.Count > 0)
			{
				this.Roh_promotedByType = new OrganizationType(propRoh_promotedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_codirector = new List<PersonAux>();
			SemanticPropertyModel propRoh_codirector = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/codirector");
			if(propRoh_codirector != null && propRoh_codirector.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_codirector.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PersonAux roh_codirector = new PersonAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_codirector.Add(roh_codirector);
					}
				}
			}
			this.Vivo_freeTextKeyword = new List<CategoryPath>();
			SemanticPropertyModel propVivo_freeTextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeyword");
			if(propVivo_freeTextKeyword != null && propVivo_freeTextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeyword.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath vivo_freeTextKeyword = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_freeTextKeyword.Add(vivo_freeTextKeyword);
					}
				}
			}
			this.Roh_studentSecondSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/studentSecondSurname"));
			this.Roh_qualification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualification"));
			this.Roh_projectCharacterTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectCharacterTypeOther"));
			this.Roh_studentFirstSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/studentFirstSurname"));
			this.Roh_promotedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTypeOther"));
			this.Roh_europeanDoctorateDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanDoctorateDate"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_europeanDoctorate= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanDoctorate"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_qualityMentionDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualityMentionDate"));
			this.Roh_promotedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTitle"));
			this.Roh_studentNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/studentNick"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_studentName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/studentName"));
			this.Roh_qualityMention= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualityMention"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ThesisSupervision"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ThesisSupervision"; } }
		[RDFProperty("http://w3id.org/roh/promotedBy")]
		public  Organization Roh_promotedBy  { get; set;} 
		public string IdRoh_promotedBy  { get; set;} 

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#hasRegion")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#hasCountryName")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/projectCharacterType")]
		public  ProjectCharacterType Roh_projectCharacterType  { get; set;} 
		public string IdRoh_projectCharacterType  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/promotedByType")]
		[RDFProperty("http://w3id.org/roh/promotedByType")]
		public  OrganizationType Roh_promotedByType  { get; set;} 
		public string IdRoh_promotedByType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/codirector")]
		public  List<PersonAux> Roh_codirector { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#freeTextKeyword")]
		public  List<CategoryPath> Vivo_freeTextKeyword { get; set;}

		[RDFProperty("http://w3id.org/roh/studentSecondSurname")]
		public  string Roh_studentSecondSurname { get; set;}

		[RDFProperty("http://w3id.org/roh/qualification")]
		public  string Roh_qualification { get; set;}

		[RDFProperty("http://w3id.org/roh/projectCharacterTypeOther")]
		public  string Roh_projectCharacterTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/studentFirstSurname")]
		public  string Roh_studentFirstSurname { get; set;}

		[RDFProperty("http://w3id.org/roh/promotedByTypeOther")]
		public  string Roh_promotedByTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/europeanDoctorateDate")]
		public  DateTime? Roh_europeanDoctorateDate { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/europeanDoctorate")]
		public  bool Roh_europeanDoctorate { get; set;}

		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime? Dct_issued { get; set;}

		[RDFProperty("http://w3id.org/roh/qualityMentionDate")]
		public  DateTime? Roh_qualityMentionDate { get; set;}

		[RDFProperty("http://w3id.org/roh/promotedByTitle")]
		public  string Roh_promotedByTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/studentNick")]
		public  string Roh_studentNick { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://w3id.org/roh/studentName")]
		public  string Roh_studentName { get; set;}

		[RDFProperty("http://w3id.org/roh/qualityMention")]
		public  bool Roh_qualityMention { get; set;}

		[RDFProperty("http://w3id.org/roh/owner")]
		[Required]
		public  Person Roh_owner  { get; set;} 
		public string IdRoh_owner  { get; set;} 

		[RDFProperty("http://w3id.org/roh/cvnCode")]
		public  string Roh_cvnCode { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:promotedBy", this.IdRoh_promotedBy));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:projectCharacterType", this.IdRoh_projectCharacterType));
			propList.Add(new StringOntologyProperty("roh:promotedByType", this.IdRoh_promotedByType));
			propList.Add(new StringOntologyProperty("roh:studentSecondSurname", this.Roh_studentSecondSurname));
			propList.Add(new StringOntologyProperty("roh:qualification", this.Roh_qualification));
			propList.Add(new StringOntologyProperty("roh:projectCharacterTypeOther", this.Roh_projectCharacterTypeOther));
			propList.Add(new StringOntologyProperty("roh:studentFirstSurname", this.Roh_studentFirstSurname));
			propList.Add(new StringOntologyProperty("roh:promotedByTypeOther", this.Roh_promotedByTypeOther));
			if (this.Roh_europeanDoctorateDate.HasValue){
				propList.Add(new DateOntologyProperty("roh:europeanDoctorateDate", this.Roh_europeanDoctorateDate.Value));
				}
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new BoolOntologyProperty("roh:europeanDoctorate", this.Roh_europeanDoctorate));
			if (this.Dct_issued.HasValue){
				propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued.Value));
				}
			if (this.Roh_qualityMentionDate.HasValue){
				propList.Add(new DateOntologyProperty("roh:qualityMentionDate", this.Roh_qualityMentionDate.Value));
				}
			propList.Add(new StringOntologyProperty("roh:promotedByTitle", this.Roh_promotedByTitle));
			propList.Add(new StringOntologyProperty("roh:studentNick", this.Roh_studentNick));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			propList.Add(new StringOntologyProperty("roh:studentName", this.Roh_studentName));
			propList.Add(new BoolOntologyProperty("roh:qualityMention", this.Roh_qualityMention));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_codirector!=null){
				foreach(PersonAux prop in Roh_codirector){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityPersonAux = new OntologyEntity("http://w3id.org/roh/PersonAux", "http://w3id.org/roh/PersonAux", "roh:codirector", prop.propList, prop.entList);
				entList.Add(entityPersonAux);
				prop.Entity= entityPersonAux;
				}
			}
			if(Vivo_freeTextKeyword!=null){
				foreach(CategoryPath prop in Vivo_freeTextKeyword){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "vivo:freeTextKeyword", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ThesisSupervision>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ThesisSupervision\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_codirector != null)
			{
			foreach(var item0 in this.Roh_codirector)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/PersonAux>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/PersonAux\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}", "http://w3id.org/roh/codirector", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName)}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName)}\"", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick)}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName)}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
				}
			}
			}
			if(this.Vivo_freeTextKeyword != null)
			{
			foreach(var item0 in this.Vivo_freeTextKeyword)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
				if(this.IdRoh_promotedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedBy", $"<{this.IdRoh_promotedBy}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_projectCharacterType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/projectCharacterType", $"<{this.IdRoh_projectCharacterType}>", list, " . ");
				}
				if(this.IdRoh_promotedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByType", $"<{this.IdRoh_promotedByType}>", list, " . ");
				}
				if(this.Roh_studentSecondSurname != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/studentSecondSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_studentSecondSurname)}\"", list, " . ");
				}
				if(this.Roh_qualification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/qualification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualification)}\"", list, " . ");
				}
				if(this.Roh_projectCharacterTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/projectCharacterTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_projectCharacterTypeOther)}\"", list, " . ");
				}
				if(this.Roh_studentFirstSurname != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/studentFirstSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_studentFirstSurname)}\"", list, " . ");
				}
				if(this.Roh_promotedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTypeOther)}\"", list, " . ");
				}
				if(this.Roh_europeanDoctorateDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/europeanDoctorateDate", $"\"{this.Roh_europeanDoctorateDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Roh_europeanDoctorate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/europeanDoctorate", $"\"{this.Roh_europeanDoctorate.ToString()}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_qualityMentionDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/qualityMentionDate", $"\"{this.Roh_qualityMentionDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_promotedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTitle)}\"", list, " . ");
				}
				if(this.Roh_studentNick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/studentNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_studentNick)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Roh_studentName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/studentName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_studentName)}\"", list, " . ");
				}
				if(this.Roh_qualityMention != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/qualityMention", $"\"{this.Roh_qualityMention.ToString()}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ThesisSupervision_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"thesissupervision\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/ThesisSupervision\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_codirector != null)
			{
			foreach(var item0 in this.Roh_codirector)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/codirector", $"<{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName).ToLower()}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
				}
			}
			}
			if(this.Vivo_freeTextKeyword != null)
			{
			foreach(var item0 in this.Vivo_freeTextKeyword)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
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
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
				if(this.IdRoh_promotedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_promotedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedBy", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVcard_hasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVcard_hasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_projectCharacterType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_projectCharacterType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/projectCharacterType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_promotedByType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_promotedByType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByType", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_studentSecondSurname != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/studentSecondSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_studentSecondSurname).ToLower()}\"", list, " . ");
				}
				if(this.Roh_qualification != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/qualification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualification).ToLower()}\"", list, " . ");
				}
				if(this.Roh_projectCharacterTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/projectCharacterTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_projectCharacterTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_studentFirstSurname != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/studentFirstSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_studentFirstSurname).ToLower()}\"", list, " . ");
				}
				if(this.Roh_promotedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_europeanDoctorateDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/europeanDoctorateDate", $"{this.Roh_europeanDoctorateDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_europeanDoctorate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/europeanDoctorate", $"\"{this.Roh_europeanDoctorate.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/terms/issued", $"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_qualityMentionDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/qualityMentionDate", $"{this.Roh_qualityMentionDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_promotedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_studentNick != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/studentNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_studentNick).ToLower()}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Roh_studentName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/studentName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_studentName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_qualityMention != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/qualityMention", $"\"{this.Roh_qualityMention.ToString().ToLower()}\"", list, " . ");
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
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode).ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/ThesissupervisionOntology_{ResourceID}_{ArticleID}";
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
