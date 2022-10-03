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

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class CV : GnossOCBase
	{

		public CV() : base() { } 

		public CV(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Roh_generatedPDFFile = new List<GeneratedPDFFile>();
			SemanticPropertyModel propRoh_generatedPDFFile = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/generatedPDFFile");
			if(propRoh_generatedPDFFile != null && propRoh_generatedPDFFile.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_generatedPDFFile.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						GeneratedPDFFile roh_generatedPDFFile = new GeneratedPDFFile(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_generatedPDFFile.Add(roh_generatedPDFFile);
					}
				}
			}
			this.Roh_multilangProperties = new List<MultilangProperties>();
			SemanticPropertyModel propRoh_multilangProperties = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/multilangProperties");
			if(propRoh_multilangProperties != null && propRoh_multilangProperties.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_multilangProperties.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						MultilangProperties roh_multilangProperties = new MultilangProperties(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_multilangProperties.Add(roh_multilangProperties);
					}
				}
			}
			SemanticPropertyModel propRoh_researchObject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchObject");
			if(propRoh_researchObject != null && propRoh_researchObject.PropertyValues.Count > 0)
			{
				this.Roh_researchObject = new ResearchObjects(propRoh_researchObject.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_professionalSituation = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/professionalSituation");
			if(propRoh_professionalSituation != null && propRoh_professionalSituation.PropertyValues.Count > 0)
			{
				this.Roh_professionalSituation = new ProfessionalSituation(propRoh_professionalSituation.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_cvOf = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvOf");
			if(propRoh_cvOf != null && propRoh_cvOf.PropertyValues.Count > 0)
			{
				this.Roh_cvOf = new Person(propRoh_cvOf.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_scientificExperience = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificExperience");
			if(propRoh_scientificExperience != null && propRoh_scientificExperience.PropertyValues.Count > 0)
			{
				this.Roh_scientificExperience = new ScientificExperience(propRoh_scientificExperience.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_scientificActivity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificActivity");
			if(propRoh_scientificActivity != null && propRoh_scientificActivity.PropertyValues.Count > 0)
			{
				this.Roh_scientificActivity = new ScientificActivity(propRoh_scientificActivity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_freeTextSummary = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/freeTextSummary");
			if(propRoh_freeTextSummary != null && propRoh_freeTextSummary.PropertyValues.Count > 0)
			{
				this.Roh_freeTextSummary = new FreeTextSummary(propRoh_freeTextSummary.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_qualifications = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualifications");
			if(propRoh_qualifications != null && propRoh_qualifications.PropertyValues.Count > 0)
			{
				this.Roh_qualifications = new Qualifications(propRoh_qualifications.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_personalData = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/personalData");
			if(propRoh_personalData != null && propRoh_personalData.PropertyValues.Count > 0)
			{
				this.Roh_personalData = new PersonalData(propRoh_personalData.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_teachingExperience = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingExperience");
			if(propRoh_teachingExperience != null && propRoh_teachingExperience.PropertyValues.Count > 0)
			{
				this.Roh_teachingExperience = new TeachingExperience(propRoh_teachingExperience.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Foaf_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/name"));
		}

		public CV(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_generatedPDFFile = new List<GeneratedPDFFile>();
			SemanticPropertyModel propRoh_generatedPDFFile = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/generatedPDFFile");
			if(propRoh_generatedPDFFile != null && propRoh_generatedPDFFile.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_generatedPDFFile.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						GeneratedPDFFile roh_generatedPDFFile = new GeneratedPDFFile(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_generatedPDFFile.Add(roh_generatedPDFFile);
					}
				}
			}
			this.Roh_multilangProperties = new List<MultilangProperties>();
			SemanticPropertyModel propRoh_multilangProperties = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/multilangProperties");
			if(propRoh_multilangProperties != null && propRoh_multilangProperties.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_multilangProperties.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						MultilangProperties roh_multilangProperties = new MultilangProperties(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_multilangProperties.Add(roh_multilangProperties);
					}
				}
			}
			SemanticPropertyModel propRoh_researchObject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchObject");
			if(propRoh_researchObject != null && propRoh_researchObject.PropertyValues.Count > 0)
			{
				this.Roh_researchObject = new ResearchObjects(propRoh_researchObject.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_professionalSituation = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/professionalSituation");
			if(propRoh_professionalSituation != null && propRoh_professionalSituation.PropertyValues.Count > 0)
			{
				this.Roh_professionalSituation = new ProfessionalSituation(propRoh_professionalSituation.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_cvOf = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvOf");
			if(propRoh_cvOf != null && propRoh_cvOf.PropertyValues.Count > 0)
			{
				this.Roh_cvOf = new Person(propRoh_cvOf.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_scientificExperience = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificExperience");
			if(propRoh_scientificExperience != null && propRoh_scientificExperience.PropertyValues.Count > 0)
			{
				this.Roh_scientificExperience = new ScientificExperience(propRoh_scientificExperience.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_scientificActivity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificActivity");
			if(propRoh_scientificActivity != null && propRoh_scientificActivity.PropertyValues.Count > 0)
			{
				this.Roh_scientificActivity = new ScientificActivity(propRoh_scientificActivity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_freeTextSummary = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/freeTextSummary");
			if(propRoh_freeTextSummary != null && propRoh_freeTextSummary.PropertyValues.Count > 0)
			{
				this.Roh_freeTextSummary = new FreeTextSummary(propRoh_freeTextSummary.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_qualifications = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualifications");
			if(propRoh_qualifications != null && propRoh_qualifications.PropertyValues.Count > 0)
			{
				this.Roh_qualifications = new Qualifications(propRoh_qualifications.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_personalData = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/personalData");
			if(propRoh_personalData != null && propRoh_personalData.PropertyValues.Count > 0)
			{
				this.Roh_personalData = new PersonalData(propRoh_personalData.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_teachingExperience = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingExperience");
			if(propRoh_teachingExperience != null && propRoh_teachingExperience.PropertyValues.Count > 0)
			{
				this.Roh_teachingExperience = new TeachingExperience(propRoh_teachingExperience.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Foaf_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/name"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/CV"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/CV"; } }
		[RDFProperty("http://w3id.org/roh/generatedPDFFile")]
		public  List<GeneratedPDFFile> Roh_generatedPDFFile { get; set;}

		[RDFProperty("http://w3id.org/roh/multilangProperties")]
		public  List<MultilangProperties> Roh_multilangProperties { get; set;}

		[RDFProperty("http://w3id.org/roh/researchObject")]
		public  ResearchObjects Roh_researchObject { get; set;}

		[RDFProperty("http://w3id.org/roh/professionalSituation")]
		public  ProfessionalSituation Roh_professionalSituation { get; set;}

		[RDFProperty("http://w3id.org/roh/cvOf")]
		[Required]
		public  Person Roh_cvOf  { get; set;} 
		public string IdRoh_cvOf  { get; set;} 

		[RDFProperty("http://w3id.org/roh/scientificExperience")]
		public  ScientificExperience Roh_scientificExperience { get; set;}

		[RDFProperty("http://w3id.org/roh/scientificActivity")]
		public  ScientificActivity Roh_scientificActivity { get; set;}

		[RDFProperty("http://w3id.org/roh/freeTextSummary")]
		public  FreeTextSummary Roh_freeTextSummary { get; set;}

		[RDFProperty("http://w3id.org/roh/qualifications")]
		public  Qualifications Roh_qualifications { get; set;}

		[RDFProperty("http://w3id.org/roh/personalData")]
		public  PersonalData Roh_personalData { get; set;}

		[RDFProperty("http://w3id.org/roh/teachingExperience")]
		public  TeachingExperience Roh_teachingExperience { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/name")]
		public  string Foaf_name { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:cvOf", this.IdRoh_cvOf));
			propList.Add(new StringOntologyProperty("foaf:name", this.Foaf_name));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_generatedPDFFile!=null){
				foreach(GeneratedPDFFile prop in Roh_generatedPDFFile){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityGeneratedPDFFile = new OntologyEntity("http://w3id.org/roh/GeneratedPDFFile", "http://w3id.org/roh/GeneratedPDFFile", "roh:generatedPDFFile", prop.propList, prop.entList);
				entList.Add(entityGeneratedPDFFile);
				prop.Entity= entityGeneratedPDFFile;
				}
			}
			if(Roh_multilangProperties!=null){
				foreach(MultilangProperties prop in Roh_multilangProperties){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityMultilangProperties = new OntologyEntity("http://w3id.org/roh/MultilangProperties", "http://w3id.org/roh/MultilangProperties", "roh:multilangProperties", prop.propList, prop.entList);
				entList.Add(entityMultilangProperties);
				prop.Entity= entityMultilangProperties;
				}
			}
			Roh_researchObject.GetProperties();
			Roh_researchObject.GetEntities();
			OntologyEntity entityRoh_researchObject = new OntologyEntity("http://w3id.org/roh/ResearchObjects", "http://w3id.org/roh/ResearchObjects", "roh:researchObject", Roh_researchObject.propList, Roh_researchObject.entList);
			entList.Add(entityRoh_researchObject);
			Roh_professionalSituation.GetProperties();
			Roh_professionalSituation.GetEntities();
			OntologyEntity entityRoh_professionalSituation = new OntologyEntity("http://w3id.org/roh/ProfessionalSituation", "http://w3id.org/roh/ProfessionalSituation", "roh:professionalSituation", Roh_professionalSituation.propList, Roh_professionalSituation.entList);
			entList.Add(entityRoh_professionalSituation);
			Roh_scientificExperience.GetProperties();
			Roh_scientificExperience.GetEntities();
			OntologyEntity entityRoh_scientificExperience = new OntologyEntity("http://w3id.org/roh/ScientificExperience", "http://w3id.org/roh/ScientificExperience", "roh:scientificExperience", Roh_scientificExperience.propList, Roh_scientificExperience.entList);
			entList.Add(entityRoh_scientificExperience);
			Roh_scientificActivity.GetProperties();
			Roh_scientificActivity.GetEntities();
			OntologyEntity entityRoh_scientificActivity = new OntologyEntity("http://w3id.org/roh/ScientificActivity", "http://w3id.org/roh/ScientificActivity", "roh:scientificActivity", Roh_scientificActivity.propList, Roh_scientificActivity.entList);
			entList.Add(entityRoh_scientificActivity);
			Roh_freeTextSummary.GetProperties();
			Roh_freeTextSummary.GetEntities();
			OntologyEntity entityRoh_freeTextSummary = new OntologyEntity("http://w3id.org/roh/FreeTextSummary", "http://w3id.org/roh/FreeTextSummary", "roh:freeTextSummary", Roh_freeTextSummary.propList, Roh_freeTextSummary.entList);
			entList.Add(entityRoh_freeTextSummary);
			Roh_qualifications.GetProperties();
			Roh_qualifications.GetEntities();
			OntologyEntity entityRoh_qualifications = new OntologyEntity("http://w3id.org/roh/Qualifications", "http://w3id.org/roh/Qualifications", "roh:qualifications", Roh_qualifications.propList, Roh_qualifications.entList);
			entList.Add(entityRoh_qualifications);
			Roh_personalData.GetProperties();
			Roh_personalData.GetEntities();
			OntologyEntity entityRoh_personalData = new OntologyEntity("http://w3id.org/roh/PersonalData", "http://w3id.org/roh/PersonalData", "roh:personalData", Roh_personalData.propList, Roh_personalData.entList);
			entList.Add(entityRoh_personalData);
			Roh_teachingExperience.GetProperties();
			Roh_teachingExperience.GetEntities();
			OntologyEntity entityRoh_teachingExperience = new OntologyEntity("http://w3id.org/roh/TeachingExperience", "http://w3id.org/roh/TeachingExperience", "roh:teachingExperience", Roh_teachingExperience.propList, Roh_teachingExperience.entList);
			entList.Add(entityRoh_teachingExperience);
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CV>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CV\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_generatedPDFFile != null)
			{
			foreach(var item0 in this.Roh_generatedPDFFile)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneratedPDFFile_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/GeneratedPDFFile>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneratedPDFFile_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/GeneratedPDFFile\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/GeneratedPDFFile_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/generatedPDFFile", $"<{resourceAPI.GraphsUrl}items/GeneratedPDFFile_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_filePDF != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneratedPDFFile_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/filePDF", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_filePDF)}\"", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneratedPDFFile_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title)}\"", list, " . ");
				}
				if(item0.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneratedPDFFile_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{item0.Dct_issued.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(item0.Roh_status != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneratedPDFFile_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/status", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_status)}\"", list, " . ");
				}
			}
			}
			if(this.Roh_multilangProperties != null)
			{
			foreach(var item0 in this.Roh_multilangProperties)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MultilangProperties_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/MultilangProperties>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MultilangProperties_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/MultilangProperties\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/MultilangProperties_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/multilangProperties", $"<{resourceAPI.GraphsUrl}items/MultilangProperties_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_lang != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MultilangProperties_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/lang", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_lang)}\"", list, " . ");
				}
				if(item0.Roh_value != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MultilangProperties_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/value", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_value)}\"", list, " . ");
				}
				if(item0.Roh_property != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MultilangProperties_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/property", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_property)}\"", list, " . ");
				}
				if(item0.Roh_entity != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/MultilangProperties_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/entity", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_entity)}\"", list, " . ");
				}
			}
			}
			if(this.Roh_researchObject != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObjects_{ResourceID}_{this.Roh_researchObject.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ResearchObjects>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObjects_{ResourceID}_{this.Roh_researchObject.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ResearchObjects\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ResearchObjects_{ResourceID}_{this.Roh_researchObject.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/researchObject", $"<{resourceAPI.GraphsUrl}items/ResearchObjects_{ResourceID}_{this.Roh_researchObject.ArticleID}>", list, " . ");
			if(this.Roh_researchObject.Roh_researchObjects != null)
			{
			foreach(var item1 in this.Roh_researchObject.Roh_researchObjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchObject_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedResearchObject>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchObject_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedResearchObject\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedResearchObject_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObjects_{ResourceID}_{this.Roh_researchObject.ArticleID}", "http://w3id.org/roh/researchObjects", $"<{resourceAPI.GraphsUrl}items/RelatedResearchObject_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchObject_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchObject_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item1.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
				if(this.Roh_researchObject.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObjects_{ResourceID}_{this.Roh_researchObject.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_researchObject.Roh_title)}\"", list, " . ");
				}
			}
			if(this.Roh_professionalSituation != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ProfessionalSituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ProfessionalSituation>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ProfessionalSituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ProfessionalSituation\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ProfessionalSituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/professionalSituation", $"<{resourceAPI.GraphsUrl}items/ProfessionalSituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}>", list, " . ");
			if(this.Roh_professionalSituation.Roh_currentProfessionalSituation != null)
			{
			foreach(var item1 in this.Roh_professionalSituation.Roh_currentProfessionalSituation)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCurrentProfessionalSituation_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedCurrentProfessionalSituation>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCurrentProfessionalSituation_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedCurrentProfessionalSituation\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedCurrentProfessionalSituation_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ProfessionalSituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}", "http://w3id.org/roh/currentProfessionalSituation", $"<{resourceAPI.GraphsUrl}items/RelatedCurrentProfessionalSituation_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCurrentProfessionalSituation_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCurrentProfessionalSituation_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item1.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_professionalSituation.Roh_previousPositions != null)
			{
			foreach(var item2 in this.Roh_professionalSituation.Roh_previousPositions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPreviousPositions_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedPreviousPositions>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPreviousPositions_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedPreviousPositions\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedPreviousPositions_{ResourceID}_{item2.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ProfessionalSituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}", "http://w3id.org/roh/previousPositions", $"<{resourceAPI.GraphsUrl}items/RelatedPreviousPositions_{ResourceID}_{item2.ArticleID}>", list, " . ");
				if(item2.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPreviousPositions_{ResourceID}_{item2.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item2.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item2.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPreviousPositions_{ResourceID}_{item2.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item2.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
				if(this.Roh_professionalSituation.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ProfessionalSituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_professionalSituation.Roh_title)}\"", list, " . ");
				}
			}
			if(this.Roh_scientificExperience != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ScientificExperience>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ScientificExperience\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/scientificExperience", $"<{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}>", list, " . ");
			if(this.Roh_scientificExperience.Roh_supervisedArtisticProjects != null)
			{
			foreach(var item1 in this.Roh_scientificExperience.Roh_supervisedArtisticProjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSupervisedArtisticProject_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedSupervisedArtisticProject>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSupervisedArtisticProject_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedSupervisedArtisticProject\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedSupervisedArtisticProject_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/supervisedArtisticProjects", $"<{resourceAPI.GraphsUrl}items/RelatedSupervisedArtisticProject_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSupervisedArtisticProject_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSupervisedArtisticProject_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item1.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_competitiveProjects != null)
			{
			foreach(var item2 in this.Roh_scientificExperience.Roh_competitiveProjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProject_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedCompetitiveProject>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProject_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedCompetitiveProject\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedCompetitiveProject_{ResourceID}_{item2.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/competitiveProjects", $"<{resourceAPI.GraphsUrl}items/RelatedCompetitiveProject_{ResourceID}_{item2.ArticleID}>", list, " . ");
			if(item2.Roh_relatedCompetitiveProjectCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedCompetitiveProjectCV>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedCompetitiveProjectCV\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProject_{ResourceID}_{item2.ArticleID}", "http://w3id.org/roh/relatedCompetitiveProjectCV", $"<{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}>", list, " . ");
				if(item2.Roh_relatedCompetitiveProjectCV.IdRoh_dedication != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/dedication", $"<{item2.Roh_relatedCompetitiveProjectCV.IdRoh_dedication}>", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.IdRoh_participationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/participationType", $"<{item2.Roh_relatedCompetitiveProjectCV.IdRoh_participationType}>", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.IdRoh_contributionGradeProject != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/contributionGradeProject", $"<{item2.Roh_relatedCompetitiveProjectCV.IdRoh_contributionGradeProject}>", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.Roh_contributionGradeProjectOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/contributionGradeProjectOther", $"\"{GenerarTextoSinSaltoDeLinea(item2.Roh_relatedCompetitiveProjectCV.Roh_contributionGradeProjectOther)}\"", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.Roh_participationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/participationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item2.Roh_relatedCompetitiveProjectCV.Roh_participationTypeOther)}\"", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.Roh_applicantContribution != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProjectCV_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/applicantContribution", $"\"{GenerarTextoSinSaltoDeLinea(item2.Roh_relatedCompetitiveProjectCV.Roh_applicantContribution)}\"", list, " . ");
				}
			}
				if(item2.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProject_{ResourceID}_{item2.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item2.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item2.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCompetitiveProject_{ResourceID}_{item2.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item2.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_groups != null)
			{
			foreach(var item3 in this.Roh_scientificExperience.Roh_groups)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGroup_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedGroup>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGroup_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedGroup\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedGroup_{ResourceID}_{item3.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/groups", $"<{resourceAPI.GraphsUrl}items/RelatedGroup_{ResourceID}_{item3.ArticleID}>", list, " . ");
			if(item3.Roh_relatedGroupCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGroupCV_{ResourceID}_{item3.Roh_relatedGroupCV.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedGroupCV>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGroupCV_{ResourceID}_{item3.Roh_relatedGroupCV.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedGroupCV\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedGroupCV_{ResourceID}_{item3.Roh_relatedGroupCV.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGroup_{ResourceID}_{item3.ArticleID}", "http://w3id.org/roh/relatedGroupCV", $"<{resourceAPI.GraphsUrl}items/RelatedGroupCV_{ResourceID}_{item3.Roh_relatedGroupCV.ArticleID}>", list, " . ");
				if(item3.Roh_relatedGroupCV.IdRoh_collaborationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGroupCV_{ResourceID}_{item3.Roh_relatedGroupCV.ArticleID}",  "http://w3id.org/roh/collaborationType", $"<{item3.Roh_relatedGroupCV.IdRoh_collaborationType}>", list, " . ");
				}
			}
				if(item3.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGroup_{ResourceID}_{item3.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item3.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item3.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGroup_{ResourceID}_{item3.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item3.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_technologicalResults != null)
			{
			foreach(var item4 in this.Roh_scientificExperience.Roh_technologicalResults)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTechnologicalResult_{ResourceID}_{item4.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedTechnologicalResult>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTechnologicalResult_{ResourceID}_{item4.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedTechnologicalResult\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedTechnologicalResult_{ResourceID}_{item4.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/technologicalResults", $"<{resourceAPI.GraphsUrl}items/RelatedTechnologicalResult_{ResourceID}_{item4.ArticleID}>", list, " . ");
				if(item4.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTechnologicalResult_{ResourceID}_{item4.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item4.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item4.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTechnologicalResult_{ResourceID}_{item4.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item4.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_nonCompetitiveProjects != null)
			{
			foreach(var item5 in this.Roh_scientificExperience.Roh_nonCompetitiveProjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProject_{ResourceID}_{item5.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedNonCompetitiveProject>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProject_{ResourceID}_{item5.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedNonCompetitiveProject\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProject_{ResourceID}_{item5.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/nonCompetitiveProjects", $"<{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProject_{ResourceID}_{item5.ArticleID}>", list, " . ");
			if(item5.Roh_relatedNonCompetitiveProjectCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProjectCV_{ResourceID}_{item5.Roh_relatedNonCompetitiveProjectCV.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedNonCompetitiveProjectCV>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProjectCV_{ResourceID}_{item5.Roh_relatedNonCompetitiveProjectCV.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedNonCompetitiveProjectCV\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProjectCV_{ResourceID}_{item5.Roh_relatedNonCompetitiveProjectCV.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProject_{ResourceID}_{item5.ArticleID}", "http://w3id.org/roh/relatedNonCompetitiveProjectCV", $"<{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProjectCV_{ResourceID}_{item5.Roh_relatedNonCompetitiveProjectCV.ArticleID}>", list, " . ");
				if(item5.Roh_relatedNonCompetitiveProjectCV.IdRoh_contributionGradeProject != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProjectCV_{ResourceID}_{item5.Roh_relatedNonCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/contributionGradeProject", $"<{item5.Roh_relatedNonCompetitiveProjectCV.IdRoh_contributionGradeProject}>", list, " . ");
				}
				if(item5.Roh_relatedNonCompetitiveProjectCV.Roh_contributionGradeProjectOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProjectCV_{ResourceID}_{item5.Roh_relatedNonCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/contributionGradeProjectOther", $"\"{GenerarTextoSinSaltoDeLinea(item5.Roh_relatedNonCompetitiveProjectCV.Roh_contributionGradeProjectOther)}\"", list, " . ");
				}
			}
				if(item5.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProject_{ResourceID}_{item5.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item5.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item5.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNonCompetitiveProject_{ResourceID}_{item5.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item5.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_patents != null)
			{
			foreach(var item6 in this.Roh_scientificExperience.Roh_patents)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPatent_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedPatent>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPatent_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedPatent\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedPatent_{ResourceID}_{item6.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/patents", $"<{resourceAPI.GraphsUrl}items/RelatedPatent_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPatent_{ResourceID}_{item6.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item6.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item6.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPatent_{ResourceID}_{item6.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item6.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
				if(this.Roh_scientificExperience.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificExperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_scientificExperience.Roh_title)}\"", list, " . ");
				}
			}
			if(this.Roh_scientificActivity != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ScientificActivity>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ScientificActivity\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/scientificActivity", $"<{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}>", list, " . ");
			if(this.Roh_scientificActivity.Roh_otherDistinctions != null)
			{
			foreach(var item1 in this.Roh_scientificActivity.Roh_otherDistinctions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherDistinction_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedOtherDistinction>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherDistinction_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedOtherDistinction\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedOtherDistinction_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/otherDistinctions", $"<{resourceAPI.GraphsUrl}items/RelatedOtherDistinction_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherDistinction_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherDistinction_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item1.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_generalQualityIndicators != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneralQualityIndicator_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/GeneralQualityIndicator>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneralQualityIndicator_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/GeneralQualityIndicator\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/GeneralQualityIndicator_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/generalQualityIndicators", $"<{resourceAPI.GraphsUrl}items/GeneralQualityIndicator_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.ArticleID}>", list, " . ");
			if(this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneralQualityIndicatorCV_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/GeneralQualityIndicatorCV>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneralQualityIndicatorCV_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/GeneralQualityIndicatorCV\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/GeneralQualityIndicatorCV_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneralQualityIndicator_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.ArticleID}", "http://w3id.org/roh/generalQualityIndicatorCV", $"<{resourceAPI.GraphsUrl}items/GeneralQualityIndicatorCV_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.ArticleID}>", list, " . ");
				if(this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.Roh_generalQualityIndicator != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/GeneralQualityIndicatorCV_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.ArticleID}",  "http://w3id.org/roh/generalQualityIndicator", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.Roh_generalQualityIndicator)}\"", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_worksSubmittedConferences != null)
			{
			foreach(var item3 in this.Roh_scientificActivity.Roh_worksSubmittedConferences)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferences_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedWorkSubmittedConferences>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferences_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedWorkSubmittedConferences\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferences_{ResourceID}_{item3.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/worksSubmittedConferences", $"<{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferences_{ResourceID}_{item3.ArticleID}>", list, " . ");
			if(item3.Roh_relatedWorkSubmittedConferencesCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferencesCV_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedWorkSubmittedConferencesCV>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferencesCV_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedWorkSubmittedConferencesCV\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferencesCV_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferences_{ResourceID}_{item3.ArticleID}", "http://w3id.org/roh/relatedWorkSubmittedConferencesCV", $"<{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferencesCV_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}>", list, " . ");
				if(item3.Roh_relatedWorkSubmittedConferencesCV.IdRoh_participationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferencesCV_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}",  "http://w3id.org/roh/participationType", $"<{item3.Roh_relatedWorkSubmittedConferencesCV.IdRoh_participationType}>", list, " . ");
				}
				if(item3.Roh_relatedWorkSubmittedConferencesCV.IdRoh_inscriptionType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferencesCV_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}",  "http://w3id.org/roh/inscriptionType", $"<{item3.Roh_relatedWorkSubmittedConferencesCV.IdRoh_inscriptionType}>", list, " . ");
				}
				if(item3.Roh_relatedWorkSubmittedConferencesCV.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferencesCV_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}",  "http://w3id.org/roh/correspondingAuthor", $"\"{item3.Roh_relatedWorkSubmittedConferencesCV.Roh_correspondingAuthor.ToString()}\"", list, " . ");
				}
				if(item3.Roh_relatedWorkSubmittedConferencesCV.Roh_inscriptionTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferencesCV_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}",  "http://w3id.org/roh/inscriptionTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item3.Roh_relatedWorkSubmittedConferencesCV.Roh_inscriptionTypeOther)}\"", list, " . ");
				}
			}
				if(item3.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferences_{ResourceID}_{item3.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item3.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item3.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedConferences_{ResourceID}_{item3.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item3.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_societies != null)
			{
			foreach(var item4 in this.Roh_scientificActivity.Roh_societies)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSociety_{ResourceID}_{item4.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedSociety>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSociety_{ResourceID}_{item4.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedSociety\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedSociety_{ResourceID}_{item4.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/societies", $"<{resourceAPI.GraphsUrl}items/RelatedSociety_{ResourceID}_{item4.ArticleID}>", list, " . ");
				if(item4.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSociety_{ResourceID}_{item4.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item4.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item4.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSociety_{ResourceID}_{item4.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item4.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_otherDisseminationActivities != null)
			{
			foreach(var item5 in this.Roh_scientificActivity.Roh_otherDisseminationActivities)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherDisseminationActivity_{ResourceID}_{item5.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedOtherDisseminationActivity>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherDisseminationActivity_{ResourceID}_{item5.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedOtherDisseminationActivity\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedOtherDisseminationActivity_{ResourceID}_{item5.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/otherDisseminationActivities", $"<{resourceAPI.GraphsUrl}items/RelatedOtherDisseminationActivity_{ResourceID}_{item5.ArticleID}>", list, " . ");
				if(item5.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherDisseminationActivity_{ResourceID}_{item5.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item5.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item5.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherDisseminationActivity_{ResourceID}_{item5.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item5.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_councils != null)
			{
			foreach(var item6 in this.Roh_scientificActivity.Roh_councils)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCouncil_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedCouncil>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCouncil_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedCouncil\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedCouncil_{ResourceID}_{item6.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/councils", $"<{resourceAPI.GraphsUrl}items/RelatedCouncil_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCouncil_{ResourceID}_{item6.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item6.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item6.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCouncil_{ResourceID}_{item6.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item6.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_scientificProduction != null)
			{
			foreach(var item7 in this.Roh_scientificActivity.Roh_scientificProduction)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificProduction_{ResourceID}_{item7.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedScientificProduction>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificProduction_{ResourceID}_{item7.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedScientificProduction\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedScientificProduction_{ResourceID}_{item7.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/scientificProduction", $"<{resourceAPI.GraphsUrl}items/RelatedScientificProduction_{ResourceID}_{item7.ArticleID}>", list, " . ");
				if(item7.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificProduction_{ResourceID}_{item7.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item7.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item7.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificProduction_{ResourceID}_{item7.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item7.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_forums != null)
			{
			foreach(var item8 in this.Roh_scientificActivity.Roh_forums)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedForum_{ResourceID}_{item8.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedForum>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedForum_{ResourceID}_{item8.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedForum\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedForum_{ResourceID}_{item8.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/forums", $"<{resourceAPI.GraphsUrl}items/RelatedForum_{ResourceID}_{item8.ArticleID}>", list, " . ");
				if(item8.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedForum_{ResourceID}_{item8.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item8.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item8.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedForum_{ResourceID}_{item8.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item8.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_researchActivityPeriods != null)
			{
			foreach(var item9 in this.Roh_scientificActivity.Roh_researchActivityPeriods)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchActivityPeriod_{ResourceID}_{item9.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedResearchActivityPeriod>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchActivityPeriod_{ResourceID}_{item9.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedResearchActivityPeriod\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedResearchActivityPeriod_{ResourceID}_{item9.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/researchActivityPeriods", $"<{resourceAPI.GraphsUrl}items/RelatedResearchActivityPeriod_{ResourceID}_{item9.ArticleID}>", list, " . ");
				if(item9.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchActivityPeriod_{ResourceID}_{item9.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item9.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item9.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchActivityPeriod_{ResourceID}_{item9.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item9.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_scientificPublications != null)
			{
			foreach(var item10 in this.Roh_scientificActivity.Roh_scientificPublications)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublication_{ResourceID}_{item10.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedScientificPublication>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublication_{ResourceID}_{item10.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedScientificPublication\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedScientificPublication_{ResourceID}_{item10.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/scientificPublications", $"<{resourceAPI.GraphsUrl}items/RelatedScientificPublication_{ResourceID}_{item10.ArticleID}>", list, " . ");
			if(item10.Roh_relatedScientificPublicationCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublicationCV_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedScientificPublicationCV>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublicationCV_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedScientificPublicationCV\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedScientificPublicationCV_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublication_{ResourceID}_{item10.ArticleID}", "http://w3id.org/roh/relatedScientificPublicationCV", $"<{resourceAPI.GraphsUrl}items/RelatedScientificPublicationCV_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}>", list, " . ");
				if(item10.Roh_relatedScientificPublicationCV.IdRoh_contributionGrade != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublicationCV_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}",  "http://w3id.org/roh/contributionGrade", $"<{item10.Roh_relatedScientificPublicationCV.IdRoh_contributionGrade}>", list, " . ");
				}
				if(item10.Roh_relatedScientificPublicationCV.Roh_relevantResults != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublicationCV_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}",  "http://w3id.org/roh/relevantResults", $"\"{GenerarTextoSinSaltoDeLinea(item10.Roh_relatedScientificPublicationCV.Roh_relevantResults)}\"", list, " . ");
				}
				if(item10.Roh_relatedScientificPublicationCV.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublicationCV_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}",  "http://w3id.org/roh/correspondingAuthor", $"\"{item10.Roh_relatedScientificPublicationCV.Roh_correspondingAuthor.ToString()}\"", list, " . ");
				}
				if(item10.Roh_relatedScientificPublicationCV.Roh_relevantPublication != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublicationCV_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}",  "http://w3id.org/roh/relevantPublication", $"\"{item10.Roh_relatedScientificPublicationCV.Roh_relevantPublication.ToString()}\"", list, " . ");
				}
			}
				if(item10.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublication_{ResourceID}_{item10.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item10.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item10.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedScientificPublication_{ResourceID}_{item10.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item10.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_otherAchievements != null)
			{
			foreach(var item11 in this.Roh_scientificActivity.Roh_otherAchievements)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherAchievement_{ResourceID}_{item11.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedOtherAchievement>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherAchievement_{ResourceID}_{item11.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedOtherAchievement\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedOtherAchievement_{ResourceID}_{item11.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/otherAchievements", $"<{resourceAPI.GraphsUrl}items/RelatedOtherAchievement_{ResourceID}_{item11.ArticleID}>", list, " . ");
				if(item11.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherAchievement_{ResourceID}_{item11.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item11.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item11.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherAchievement_{ResourceID}_{item11.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item11.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_researchEvaluations != null)
			{
			foreach(var item12 in this.Roh_scientificActivity.Roh_researchEvaluations)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchEvaluation_{ResourceID}_{item12.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedResearchEvaluation>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchEvaluation_{ResourceID}_{item12.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedResearchEvaluation\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedResearchEvaluation_{ResourceID}_{item12.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/researchEvaluations", $"<{resourceAPI.GraphsUrl}items/RelatedResearchEvaluation_{ResourceID}_{item12.ArticleID}>", list, " . ");
				if(item12.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchEvaluation_{ResourceID}_{item12.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item12.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item12.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedResearchEvaluation_{ResourceID}_{item12.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item12.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_activitiesOrganization != null)
			{
			foreach(var item13 in this.Roh_scientificActivity.Roh_activitiesOrganization)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedActivityOrganization_{ResourceID}_{item13.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedActivityOrganization>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedActivityOrganization_{ResourceID}_{item13.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedActivityOrganization\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedActivityOrganization_{ResourceID}_{item13.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/activitiesOrganization", $"<{resourceAPI.GraphsUrl}items/RelatedActivityOrganization_{ResourceID}_{item13.ArticleID}>", list, " . ");
				if(item13.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedActivityOrganization_{ResourceID}_{item13.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item13.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item13.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedActivityOrganization_{ResourceID}_{item13.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item13.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_otherCollaborations != null)
			{
			foreach(var item14 in this.Roh_scientificActivity.Roh_otherCollaborations)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherCollaboration_{ResourceID}_{item14.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedOtherCollaboration>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherCollaboration_{ResourceID}_{item14.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedOtherCollaboration\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedOtherCollaboration_{ResourceID}_{item14.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/otherCollaborations", $"<{resourceAPI.GraphsUrl}items/RelatedOtherCollaboration_{ResourceID}_{item14.ArticleID}>", list, " . ");
				if(item14.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherCollaboration_{ResourceID}_{item14.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item14.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item14.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherCollaboration_{ResourceID}_{item14.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item14.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_networks != null)
			{
			foreach(var item15 in this.Roh_scientificActivity.Roh_networks)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNetwork_{ResourceID}_{item15.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedNetwork>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNetwork_{ResourceID}_{item15.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedNetwork\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedNetwork_{ResourceID}_{item15.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/networks", $"<{resourceAPI.GraphsUrl}items/RelatedNetwork_{ResourceID}_{item15.ArticleID}>", list, " . ");
				if(item15.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNetwork_{ResourceID}_{item15.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item15.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item15.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedNetwork_{ResourceID}_{item15.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item15.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_grants != null)
			{
			foreach(var item16 in this.Roh_scientificActivity.Roh_grants)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGrant_{ResourceID}_{item16.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedGrant>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGrant_{ResourceID}_{item16.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedGrant\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedGrant_{ResourceID}_{item16.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/grants", $"<{resourceAPI.GraphsUrl}items/RelatedGrant_{ResourceID}_{item16.ArticleID}>", list, " . ");
				if(item16.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGrant_{ResourceID}_{item16.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item16.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item16.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedGrant_{ResourceID}_{item16.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item16.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_activitiesManagement != null)
			{
			foreach(var item17 in this.Roh_scientificActivity.Roh_activitiesManagement)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedActivityManagement_{ResourceID}_{item17.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedActivityManagement>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedActivityManagement_{ResourceID}_{item17.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedActivityManagement\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedActivityManagement_{ResourceID}_{item17.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/activitiesManagement", $"<{resourceAPI.GraphsUrl}items/RelatedActivityManagement_{ResourceID}_{item17.ArticleID}>", list, " . ");
				if(item17.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedActivityManagement_{ResourceID}_{item17.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item17.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item17.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedActivityManagement_{ResourceID}_{item17.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item17.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_prizes != null)
			{
			foreach(var item18 in this.Roh_scientificActivity.Roh_prizes)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPrize_{ResourceID}_{item18.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedPrize>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPrize_{ResourceID}_{item18.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedPrize\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedPrize_{ResourceID}_{item18.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/prizes", $"<{resourceAPI.GraphsUrl}items/RelatedPrize_{ResourceID}_{item18.ArticleID}>", list, " . ");
				if(item18.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPrize_{ResourceID}_{item18.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item18.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item18.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPrize_{ResourceID}_{item18.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item18.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_committees != null)
			{
			foreach(var item19 in this.Roh_scientificActivity.Roh_committees)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCommittee_{ResourceID}_{item19.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedCommittee>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCommittee_{ResourceID}_{item19.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedCommittee\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedCommittee_{ResourceID}_{item19.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/committees", $"<{resourceAPI.GraphsUrl}items/RelatedCommittee_{ResourceID}_{item19.ArticleID}>", list, " . ");
				if(item19.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCommittee_{ResourceID}_{item19.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item19.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item19.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCommittee_{ResourceID}_{item19.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item19.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_stays != null)
			{
			foreach(var item20 in this.Roh_scientificActivity.Roh_stays)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedStay_{ResourceID}_{item20.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedStay>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedStay_{ResourceID}_{item20.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedStay\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedStay_{ResourceID}_{item20.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/stays", $"<{resourceAPI.GraphsUrl}items/RelatedStay_{ResourceID}_{item20.ArticleID}>", list, " . ");
				if(item20.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedStay_{ResourceID}_{item20.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item20.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item20.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedStay_{ResourceID}_{item20.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item20.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_obtainedRecognitions != null)
			{
			foreach(var item21 in this.Roh_scientificActivity.Roh_obtainedRecognitions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedObtainedRecognition_{ResourceID}_{item21.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedObtainedRecognition>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedObtainedRecognition_{ResourceID}_{item21.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedObtainedRecognition\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedObtainedRecognition_{ResourceID}_{item21.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/obtainedRecognitions", $"<{resourceAPI.GraphsUrl}items/RelatedObtainedRecognition_{ResourceID}_{item21.ArticleID}>", list, " . ");
				if(item21.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedObtainedRecognition_{ResourceID}_{item21.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item21.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item21.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedObtainedRecognition_{ResourceID}_{item21.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item21.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_worksSubmittedSeminars != null)
			{
			foreach(var item22 in this.Roh_scientificActivity.Roh_worksSubmittedSeminars)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminars_{ResourceID}_{item22.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedWorkSubmittedSeminars>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminars_{ResourceID}_{item22.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedWorkSubmittedSeminars\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminars_{ResourceID}_{item22.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/worksSubmittedSeminars", $"<{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminars_{ResourceID}_{item22.ArticleID}>", list, " . ");
			if(item22.Roh_relatedWorkSubmittedSeminarsCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminarsCV_{ResourceID}_{item22.Roh_relatedWorkSubmittedSeminarsCV.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedWorkSubmittedSeminarsCV>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminarsCV_{ResourceID}_{item22.Roh_relatedWorkSubmittedSeminarsCV.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedWorkSubmittedSeminarsCV\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminarsCV_{ResourceID}_{item22.Roh_relatedWorkSubmittedSeminarsCV.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminars_{ResourceID}_{item22.ArticleID}", "http://w3id.org/roh/relatedWorkSubmittedSeminarsCV", $"<{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminarsCV_{ResourceID}_{item22.Roh_relatedWorkSubmittedSeminarsCV.ArticleID}>", list, " . ");
				if(item22.Roh_relatedWorkSubmittedSeminarsCV.IdRoh_inscriptionType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminarsCV_{ResourceID}_{item22.Roh_relatedWorkSubmittedSeminarsCV.ArticleID}",  "http://w3id.org/roh/inscriptionType", $"<{item22.Roh_relatedWorkSubmittedSeminarsCV.IdRoh_inscriptionType}>", list, " . ");
				}
				if(item22.Roh_relatedWorkSubmittedSeminarsCV.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminarsCV_{ResourceID}_{item22.Roh_relatedWorkSubmittedSeminarsCV.ArticleID}",  "http://w3id.org/roh/correspondingAuthor", $"\"{item22.Roh_relatedWorkSubmittedSeminarsCV.Roh_correspondingAuthor.ToString()}\"", list, " . ");
				}
			}
				if(item22.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminars_{ResourceID}_{item22.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item22.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item22.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedWorkSubmittedSeminars_{ResourceID}_{item22.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item22.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
				if(this.Roh_scientificActivity.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ScientificActivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_scientificActivity.Roh_title)}\"", list, " . ");
				}
			}
			if(this.Roh_freeTextSummary != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummary_{ResourceID}_{this.Roh_freeTextSummary.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/FreeTextSummary>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummary_{ResourceID}_{this.Roh_freeTextSummary.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/FreeTextSummary\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/FreeTextSummary_{ResourceID}_{this.Roh_freeTextSummary.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/freeTextSummary", $"<{resourceAPI.GraphsUrl}items/FreeTextSummary_{ResourceID}_{this.Roh_freeTextSummary.ArticleID}>", list, " . ");
			if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummaryValues_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/FreeTextSummaryValues>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummaryValues_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/FreeTextSummaryValues\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/FreeTextSummaryValues_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummary_{ResourceID}_{this.Roh_freeTextSummary.ArticleID}", "http://w3id.org/roh/freeTextSummaryValues", $"<{resourceAPI.GraphsUrl}items/FreeTextSummaryValues_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.ArticleID}>", list, " . ");
			if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummaryValuesCV_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/FreeTextSummaryValuesCV>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummaryValuesCV_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/FreeTextSummaryValuesCV\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/FreeTextSummaryValuesCV_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummaryValues_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.ArticleID}", "http://w3id.org/roh/freeTextSummaryValuesCV", $"<{resourceAPI.GraphsUrl}items/FreeTextSummaryValuesCV_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}>", list, " . ");
				if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summaryTFG != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummaryValuesCV_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}",  "http://w3id.org/roh/summaryTFG", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summaryTFG)}\"", list, " . ");
				}
				if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summaryTFM != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummaryValuesCV_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}",  "http://w3id.org/roh/summaryTFM", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summaryTFM)}\"", list, " . ");
				}
				if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summary != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummaryValuesCV_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}",  "http://w3id.org/roh/summary", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summary)}\"", list, " . ");
				}
			}
			}
				if(this.Roh_freeTextSummary.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/FreeTextSummary_{ResourceID}_{this.Roh_freeTextSummary.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_freeTextSummary.Roh_title)}\"", list, " . ");
				}
			}
			if(this.Roh_qualifications != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Qualifications>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Qualifications\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/qualifications", $"<{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}>", list, " . ");
			if(this.Roh_qualifications.Roh_languageSkills != null)
			{
			foreach(var item1 in this.Roh_qualifications.Roh_languageSkills)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedLanguageSkills_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedLanguageSkills>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedLanguageSkills_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedLanguageSkills\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedLanguageSkills_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/languageSkills", $"<{resourceAPI.GraphsUrl}items/RelatedLanguageSkills_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedLanguageSkills_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedLanguageSkills_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item1.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_firstSecondCycles != null)
			{
			foreach(var item2 in this.Roh_qualifications.Roh_firstSecondCycles)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedFirstSecondCycles_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedFirstSecondCycles>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedFirstSecondCycles_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedFirstSecondCycles\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedFirstSecondCycles_{ResourceID}_{item2.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/firstSecondCycles", $"<{resourceAPI.GraphsUrl}items/RelatedFirstSecondCycles_{ResourceID}_{item2.ArticleID}>", list, " . ");
				if(item2.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedFirstSecondCycles_{ResourceID}_{item2.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item2.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item2.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedFirstSecondCycles_{ResourceID}_{item2.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item2.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_postgraduates != null)
			{
			foreach(var item3 in this.Roh_qualifications.Roh_postgraduates)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPostGraduates_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedPostGraduates>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPostGraduates_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedPostGraduates\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedPostGraduates_{ResourceID}_{item3.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/postgraduates", $"<{resourceAPI.GraphsUrl}items/RelatedPostGraduates_{ResourceID}_{item3.ArticleID}>", list, " . ");
				if(item3.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPostGraduates_{ResourceID}_{item3.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item3.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item3.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedPostGraduates_{ResourceID}_{item3.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item3.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_doctorates != null)
			{
			foreach(var item4 in this.Roh_qualifications.Roh_doctorates)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedDoctorates_{ResourceID}_{item4.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedDoctorates>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedDoctorates_{ResourceID}_{item4.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedDoctorates\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedDoctorates_{ResourceID}_{item4.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/doctorates", $"<{resourceAPI.GraphsUrl}items/RelatedDoctorates_{ResourceID}_{item4.ArticleID}>", list, " . ");
				if(item4.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedDoctorates_{ResourceID}_{item4.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item4.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item4.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedDoctorates_{ResourceID}_{item4.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item4.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_specialisedTraining != null)
			{
			foreach(var item5 in this.Roh_qualifications.Roh_specialisedTraining)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSpecialisedTrainings_{ResourceID}_{item5.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedSpecialisedTrainings>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSpecialisedTrainings_{ResourceID}_{item5.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedSpecialisedTrainings\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedSpecialisedTrainings_{ResourceID}_{item5.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/specialisedTraining", $"<{resourceAPI.GraphsUrl}items/RelatedSpecialisedTrainings_{ResourceID}_{item5.ArticleID}>", list, " . ");
				if(item5.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSpecialisedTrainings_{ResourceID}_{item5.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item5.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item5.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedSpecialisedTrainings_{ResourceID}_{item5.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item5.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_coursesAndSeminars != null)
			{
			foreach(var item6 in this.Roh_qualifications.Roh_coursesAndSeminars)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCoursesAndSeminars_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedCoursesAndSeminars>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCoursesAndSeminars_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedCoursesAndSeminars\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedCoursesAndSeminars_{ResourceID}_{item6.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/coursesAndSeminars", $"<{resourceAPI.GraphsUrl}items/RelatedCoursesAndSeminars_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCoursesAndSeminars_{ResourceID}_{item6.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item6.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item6.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedCoursesAndSeminars_{ResourceID}_{item6.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item6.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
				if(this.Roh_qualifications.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualifications.Roh_title)}\"", list, " . ");
				}
			}
			if(this.Roh_personalData != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/PersonalData>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/PersonalData\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/personalData", $"<{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}>", list, " . ");
			if(this.Roh_personalData.Roh_hasFax != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://www.w3.org/2006/vcard/ns#TelephoneType>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://www.w3.org/2006/vcard/ns#TelephoneType\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://w3id.org/roh/hasFax", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Roh_hasFax.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasFax.Roh_hasExtension)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_hasFax.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasFax.Roh_hasInternationalCode)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_hasFax.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasFax.Vcard_hasValue)}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Vcard_hasTelephone != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://www.w3.org/2006/vcard/ns#TelephoneType>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://www.w3.org/2006/vcard/ns#TelephoneType\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}", "https://www.w3.org/2006/vcard/ns#hasTelephone", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Vcard_hasTelephone.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_hasTelephone.Roh_hasExtension)}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_hasTelephone.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_hasTelephone.Roh_hasInternationalCode)}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_hasTelephone.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_hasTelephone.Vcard_hasValue)}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Roh_hasMobilePhone != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://www.w3.org/2006/vcard/ns#TelephoneType>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://www.w3.org/2006/vcard/ns#TelephoneType\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://w3id.org/roh/hasMobilePhone", $"<{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Roh_hasMobilePhone.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasMobilePhone.Roh_hasExtension)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_hasMobilePhone.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasMobilePhone.Roh_hasInternationalCode)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_hasMobilePhone.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TelephoneType_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasMobilePhone.Vcard_hasValue)}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Roh_birthplace != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://www.w3.org/2006/vcard/ns#Address>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://www.w3.org/2006/vcard/ns#Address\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://w3id.org/roh/birthplace", $"<{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Roh_birthplace.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.Roh_personalData.Roh_birthplace.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.IdRoh_hasProvince != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "http://w3id.org/roh/hasProvince", $"<{this.Roh_personalData.Roh_birthplace.IdRoh_hasProvince}>", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.Roh_personalData.Roh_birthplace.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.Vcard_postal_code != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#postal-code", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_birthplace.Vcard_postal_code)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.Vcard_extended_address != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#extended-address", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_birthplace.Vcard_extended_address)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.Vcard_street_address != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#street-address", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_birthplace.Vcard_street_address)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_birthplace.Vcard_locality)}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Vcard_address != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<https://www.w3.org/2006/vcard/ns#Address>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"https://www.w3.org/2006/vcard/ns#Address\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}", "https://www.w3.org/2006/vcard/ns#address", $"<{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Vcard_address.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.Roh_personalData.Vcard_address.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.IdRoh_hasProvince != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "http://w3id.org/roh/hasProvince", $"<{this.Roh_personalData.Vcard_address.IdRoh_hasProvince}>", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.Roh_personalData.Vcard_address.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.Vcard_postal_code != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#postal-code", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_address.Vcard_postal_code)}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.Vcard_extended_address != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#extended-address", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_address.Vcard_extended_address)}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.Vcard_street_address != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#street-address", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_address.Vcard_street_address)}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_address.Vcard_locality)}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Roh_otherIds != null)
			{
			foreach(var item6 in this.Roh_personalData.Roh_otherIds)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://xmlns.com/foaf/0.1/Document>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://xmlns.com/foaf/0.1/Document\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item6.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://w3id.org/roh/otherIds", $"<{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Foaf_topic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item6.ArticleID}",  "http://xmlns.com/foaf/0.1/topic", $"\"{GenerarTextoSinSaltoDeLinea(item6.Foaf_topic)}\"", list, " . ");
				}
				if(item6.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item6.ArticleID}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(item6.Dc_title)}\"", list, " . ");
				}
			}
			}
				if(this.Roh_personalData.IdFoaf_gender != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/gender", $"<{this.Roh_personalData.IdFoaf_gender}>", list, " . ");
				}
				if(this.Roh_personalData.IdSchema_nationality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://www.schema.org/nationality", $"<{this.Roh_personalData.IdSchema_nationality}>", list, " . ");
				}
				if(this.Roh_personalData.Roh_nie != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/nie", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_nie)}\"", list, " . ");
				}
				if(this.Roh_personalData.Vivo_researcherId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://vivoweb.org/ontology/core#researcherId", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vivo_researcherId)}\"", list, " . ");
				}
				if(this.Roh_personalData.Vivo_scopusId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://vivoweb.org/ontology/core#scopusId", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vivo_scopusId)}\"", list, " . ");
				}
				if(this.Roh_personalData.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Foaf_familyName)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_secondFamilyName)}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_email != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "https://www.w3.org/2006/vcard/ns#email", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_email)}\"", list, " . ");
				}
				if(this.Roh_personalData.Foaf_img != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/img", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Foaf_img)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_dni != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/dni", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_dni)}\"", list, " . ");
				}
				if(this.Roh_personalData.Foaf_homepage != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/homepage", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Foaf_homepage)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_ORCID != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/ORCID", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_ORCID)}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_passport != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/passport", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_passport)}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_birth_date != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "https://www.w3.org/2006/vcard/ns#birth-date", $"\"{this.Roh_personalData.Vcard_birth_date.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_personalData.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonalData_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Foaf_firstName)}\"", list, " . ");
				}
			}
			if(this.Roh_teachingExperience != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/TeachingExperience>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/TeachingExperience\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}", "http://w3id.org/roh/teachingExperience", $"<{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}>", list, " . ");
			if(this.Roh_teachingExperience.Roh_thesisSupervisions != null)
			{
			foreach(var item1 in this.Roh_teachingExperience.Roh_thesisSupervisions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedThesisSupervisions_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedThesisSupervisions>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedThesisSupervisions_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedThesisSupervisions\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedThesisSupervisions_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/thesisSupervisions", $"<{resourceAPI.GraphsUrl}items/RelatedThesisSupervisions_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedThesisSupervisions_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedThesisSupervisions_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item1.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_otherActivities != null)
			{
			foreach(var item2 in this.Roh_teachingExperience.Roh_otherActivities)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherActivities_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedOtherActivities>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherActivities_{ResourceID}_{item2.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedOtherActivities\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedOtherActivities_{ResourceID}_{item2.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/otherActivities", $"<{resourceAPI.GraphsUrl}items/RelatedOtherActivities_{ResourceID}_{item2.ArticleID}>", list, " . ");
				if(item2.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherActivities_{ResourceID}_{item2.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item2.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item2.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedOtherActivities_{ResourceID}_{item2.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item2.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_teachingCongress != null)
			{
			foreach(var item3 in this.Roh_teachingExperience.Roh_teachingCongress)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingCongress_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedTeachingCongress>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingCongress_{ResourceID}_{item3.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedTeachingCongress\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedTeachingCongress_{ResourceID}_{item3.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/teachingCongress", $"<{resourceAPI.GraphsUrl}items/RelatedTeachingCongress_{ResourceID}_{item3.ArticleID}>", list, " . ");
				if(item3.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingCongress_{ResourceID}_{item3.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item3.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item3.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingCongress_{ResourceID}_{item3.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item3.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_mostRelevantContributions != null)
			{
			foreach(var item4 in this.Roh_teachingExperience.Roh_mostRelevantContributions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedMostRelevantContributions_{ResourceID}_{item4.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedMostRelevantContributions>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedMostRelevantContributions_{ResourceID}_{item4.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedMostRelevantContributions\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedMostRelevantContributions_{ResourceID}_{item4.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/mostRelevantContributions", $"<{resourceAPI.GraphsUrl}items/RelatedMostRelevantContributions_{ResourceID}_{item4.ArticleID}>", list, " . ");
				if(item4.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedMostRelevantContributions_{ResourceID}_{item4.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item4.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item4.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedMostRelevantContributions_{ResourceID}_{item4.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item4.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_teachingProjects != null)
			{
			foreach(var item5 in this.Roh_teachingExperience.Roh_teachingProjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingProjects_{ResourceID}_{item5.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedTeachingProjects>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingProjects_{ResourceID}_{item5.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedTeachingProjects\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedTeachingProjects_{ResourceID}_{item5.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/teachingProjects", $"<{resourceAPI.GraphsUrl}items/RelatedTeachingProjects_{ResourceID}_{item5.ArticleID}>", list, " . ");
				if(item5.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingProjects_{ResourceID}_{item5.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item5.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item5.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingProjects_{ResourceID}_{item5.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item5.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_academicTutorials != null)
			{
			foreach(var item6 in this.Roh_teachingExperience.Roh_academicTutorials)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedAcademicTutorials_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedAcademicTutorials>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedAcademicTutorials_{ResourceID}_{item6.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedAcademicTutorials\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedAcademicTutorials_{ResourceID}_{item6.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/academicTutorials", $"<{resourceAPI.GraphsUrl}items/RelatedAcademicTutorials_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedAcademicTutorials_{ResourceID}_{item6.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item6.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item6.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedAcademicTutorials_{ResourceID}_{item6.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item6.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_impartedCoursesSeminars != null)
			{
			foreach(var item7 in this.Roh_teachingExperience.Roh_impartedCoursesSeminars)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedImpartedCoursesSeminars_{ResourceID}_{item7.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedImpartedCoursesSeminars>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedImpartedCoursesSeminars_{ResourceID}_{item7.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedImpartedCoursesSeminars\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedImpartedCoursesSeminars_{ResourceID}_{item7.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/impartedCoursesSeminars", $"<{resourceAPI.GraphsUrl}items/RelatedImpartedCoursesSeminars_{ResourceID}_{item7.ArticleID}>", list, " . ");
				if(item7.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedImpartedCoursesSeminars_{ResourceID}_{item7.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item7.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item7.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedImpartedCoursesSeminars_{ResourceID}_{item7.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item7.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_teachingInnovationAwardsReceived != null)
			{
			foreach(var item8 in this.Roh_teachingExperience.Roh_teachingInnovationAwardsReceived)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingInnovationAwardsReceived_{ResourceID}_{item8.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedTeachingInnovationAwardsReceived>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingInnovationAwardsReceived_{ResourceID}_{item8.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedTeachingInnovationAwardsReceived\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedTeachingInnovationAwardsReceived_{ResourceID}_{item8.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/teachingInnovationAwardsReceived", $"<{resourceAPI.GraphsUrl}items/RelatedTeachingInnovationAwardsReceived_{ResourceID}_{item8.ArticleID}>", list, " . ");
				if(item8.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingInnovationAwardsReceived_{ResourceID}_{item8.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item8.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item8.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingInnovationAwardsReceived_{ResourceID}_{item8.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item8.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_teachingPublications != null)
			{
			foreach(var item9 in this.Roh_teachingExperience.Roh_teachingPublications)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingPublications_{ResourceID}_{item9.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedTeachingPublications>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingPublications_{ResourceID}_{item9.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedTeachingPublications\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedTeachingPublications_{ResourceID}_{item9.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/teachingPublications", $"<{resourceAPI.GraphsUrl}items/RelatedTeachingPublications_{ResourceID}_{item9.ArticleID}>", list, " . ");
				if(item9.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingPublications_{ResourceID}_{item9.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item9.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item9.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedTeachingPublications_{ResourceID}_{item9.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item9.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_impartedAcademicTrainings != null)
			{
			foreach(var item10 in this.Roh_teachingExperience.Roh_impartedAcademicTrainings)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedImpartedAcademicTrainings_{ResourceID}_{item10.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/RelatedImpartedAcademicTrainings>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedImpartedAcademicTrainings_{ResourceID}_{item10.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/RelatedImpartedAcademicTrainings\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/RelatedImpartedAcademicTrainings_{ResourceID}_{item10.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/impartedAcademicTrainings", $"<{resourceAPI.GraphsUrl}items/RelatedImpartedAcademicTrainings_{ResourceID}_{item10.ArticleID}>", list, " . ");
				if(item10.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedImpartedAcademicTrainings_{ResourceID}_{item10.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item10.Roh_isPublic.ToString()}\"", list, " . ");
				}
				if(item10.IdVivo_relatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/RelatedImpartedAcademicTrainings_{ResourceID}_{item10.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{item10.IdVivo_relatedBy}>", list, " . ");
				}
			}
			}
				if(this.Roh_teachingExperience.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingExperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_teachingExperience.Roh_title)}\"", list, " . ");
				}
			}
				if(this.IdRoh_cvOf != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvOf", $"<{this.IdRoh_cvOf}>", list, " . ");
				}
				if(this.Foaf_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CV_{ResourceID}_{ArticleID}",  "http://xmlns.com/foaf/0.1/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"curriculumvitae\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/CV\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_generatedPDFFile != null)
			{
			foreach(var item0 in this.Roh_generatedPDFFile)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/generatedPDFFile", $"<{resourceAPI.GraphsUrl}items/generatedpdffile_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_filePDF != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/generatedpdffile_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/filePDF", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_filePDF).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/generatedpdffile_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title).ToLower()}\"", list, " . ");
				}
				if(item0.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/generatedpdffile_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/terms/issued", $"{item0.Dct_issued.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(item0.Roh_status != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/generatedpdffile_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/status", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_status).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Roh_multilangProperties != null)
			{
			foreach(var item0 in this.Roh_multilangProperties)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/multilangProperties", $"<{resourceAPI.GraphsUrl}items/multilangproperties_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_lang != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/multilangproperties_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/lang", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_lang).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_value != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/multilangproperties_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/value", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_value).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_property != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/multilangproperties_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/property", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_property).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_entity != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/multilangproperties_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/entity", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_entity).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Roh_researchObject != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/researchObject", $"<{resourceAPI.GraphsUrl}items/researchobjects_{ResourceID}_{this.Roh_researchObject.ArticleID}>", list, " . ");
			if(this.Roh_researchObject.Roh_researchObjects != null)
			{
			foreach(var item1 in this.Roh_researchObject.Roh_researchObjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/researchobjects_{ResourceID}_{this.Roh_researchObject.ArticleID}", "http://w3id.org/roh/researchObjects", $"<{resourceAPI.GraphsUrl}items/relatedresearchobject_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedresearchobject_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedresearchobject_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
				if(this.Roh_researchObject.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/researchobjects_{ResourceID}_{this.Roh_researchObject.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_researchObject.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_professionalSituation != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/professionalSituation", $"<{resourceAPI.GraphsUrl}items/professionalsituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}>", list, " . ");
			if(this.Roh_professionalSituation.Roh_currentProfessionalSituation != null)
			{
			foreach(var item1 in this.Roh_professionalSituation.Roh_currentProfessionalSituation)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/professionalsituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}", "http://w3id.org/roh/currentProfessionalSituation", $"<{resourceAPI.GraphsUrl}items/relatedcurrentprofessionalsituation_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcurrentprofessionalsituation_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcurrentprofessionalsituation_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_professionalSituation.Roh_previousPositions != null)
			{
			foreach(var item2 in this.Roh_professionalSituation.Roh_previousPositions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/professionalsituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}", "http://w3id.org/roh/previousPositions", $"<{resourceAPI.GraphsUrl}items/relatedpreviouspositions_{ResourceID}_{item2.ArticleID}>", list, " . ");
				if(item2.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedpreviouspositions_{ResourceID}_{item2.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item2.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item2.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedpreviouspositions_{ResourceID}_{item2.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
				if(this.Roh_professionalSituation.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/professionalsituation_{ResourceID}_{this.Roh_professionalSituation.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_professionalSituation.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_scientificExperience != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/scientificExperience", $"<{resourceAPI.GraphsUrl}items/scientificexperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}>", list, " . ");
			if(this.Roh_scientificExperience.Roh_supervisedArtisticProjects != null)
			{
			foreach(var item1 in this.Roh_scientificExperience.Roh_supervisedArtisticProjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificexperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/supervisedArtisticProjects", $"<{resourceAPI.GraphsUrl}items/relatedsupervisedartisticproject_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedsupervisedartisticproject_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedsupervisedartisticproject_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_competitiveProjects != null)
			{
			foreach(var item2 in this.Roh_scientificExperience.Roh_competitiveProjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificexperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/competitiveProjects", $"<{resourceAPI.GraphsUrl}items/relatedcompetitiveproject_{ResourceID}_{item2.ArticleID}>", list, " . ");
			if(item2.Roh_relatedCompetitiveProjectCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcompetitiveproject_{ResourceID}_{item2.ArticleID}", "http://w3id.org/roh/relatedCompetitiveProjectCV", $"<{resourceAPI.GraphsUrl}items/relatedcompetitiveprojectcv_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}>", list, " . ");
				if(item2.Roh_relatedCompetitiveProjectCV.IdRoh_dedication != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.Roh_relatedCompetitiveProjectCV.IdRoh_dedication;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcompetitiveprojectcv_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/dedication", $"<{itemRegex}>", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.IdRoh_participationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.Roh_relatedCompetitiveProjectCV.IdRoh_participationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcompetitiveprojectcv_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/participationType", $"<{itemRegex}>", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.IdRoh_contributionGradeProject != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.Roh_relatedCompetitiveProjectCV.IdRoh_contributionGradeProject;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcompetitiveprojectcv_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/contributionGradeProject", $"<{itemRegex}>", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.Roh_contributionGradeProjectOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcompetitiveprojectcv_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/contributionGradeProjectOther", $"\"{GenerarTextoSinSaltoDeLinea(item2.Roh_relatedCompetitiveProjectCV.Roh_contributionGradeProjectOther).ToLower()}\"", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.Roh_participationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcompetitiveprojectcv_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/participationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item2.Roh_relatedCompetitiveProjectCV.Roh_participationTypeOther).ToLower()}\"", list, " . ");
				}
				if(item2.Roh_relatedCompetitiveProjectCV.Roh_applicantContribution != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcompetitiveprojectcv_{ResourceID}_{item2.Roh_relatedCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/applicantContribution", $"\"{GenerarTextoSinSaltoDeLinea(item2.Roh_relatedCompetitiveProjectCV.Roh_applicantContribution).ToLower()}\"", list, " . ");
				}
			}
				if(item2.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcompetitiveproject_{ResourceID}_{item2.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item2.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item2.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcompetitiveproject_{ResourceID}_{item2.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_groups != null)
			{
			foreach(var item3 in this.Roh_scientificExperience.Roh_groups)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificexperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/groups", $"<{resourceAPI.GraphsUrl}items/relatedgroup_{ResourceID}_{item3.ArticleID}>", list, " . ");
			if(item3.Roh_relatedGroupCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedgroup_{ResourceID}_{item3.ArticleID}", "http://w3id.org/roh/relatedGroupCV", $"<{resourceAPI.GraphsUrl}items/relatedgroupcv_{ResourceID}_{item3.Roh_relatedGroupCV.ArticleID}>", list, " . ");
				if(item3.Roh_relatedGroupCV.IdRoh_collaborationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.Roh_relatedGroupCV.IdRoh_collaborationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedgroupcv_{ResourceID}_{item3.Roh_relatedGroupCV.ArticleID}",  "http://w3id.org/roh/collaborationType", $"<{itemRegex}>", list, " . ");
				}
			}
				if(item3.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedgroup_{ResourceID}_{item3.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item3.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item3.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedgroup_{ResourceID}_{item3.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_technologicalResults != null)
			{
			foreach(var item4 in this.Roh_scientificExperience.Roh_technologicalResults)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificexperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/technologicalResults", $"<{resourceAPI.GraphsUrl}items/relatedtechnologicalresult_{ResourceID}_{item4.ArticleID}>", list, " . ");
				if(item4.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedtechnologicalresult_{ResourceID}_{item4.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item4.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item4.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item4.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedtechnologicalresult_{ResourceID}_{item4.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_nonCompetitiveProjects != null)
			{
			foreach(var item5 in this.Roh_scientificExperience.Roh_nonCompetitiveProjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificexperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/nonCompetitiveProjects", $"<{resourceAPI.GraphsUrl}items/relatednoncompetitiveproject_{ResourceID}_{item5.ArticleID}>", list, " . ");
			if(item5.Roh_relatedNonCompetitiveProjectCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatednoncompetitiveproject_{ResourceID}_{item5.ArticleID}", "http://w3id.org/roh/relatedNonCompetitiveProjectCV", $"<{resourceAPI.GraphsUrl}items/relatednoncompetitiveprojectcv_{ResourceID}_{item5.Roh_relatedNonCompetitiveProjectCV.ArticleID}>", list, " . ");
				if(item5.Roh_relatedNonCompetitiveProjectCV.IdRoh_contributionGradeProject != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item5.Roh_relatedNonCompetitiveProjectCV.IdRoh_contributionGradeProject;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatednoncompetitiveprojectcv_{ResourceID}_{item5.Roh_relatedNonCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/contributionGradeProject", $"<{itemRegex}>", list, " . ");
				}
				if(item5.Roh_relatedNonCompetitiveProjectCV.Roh_contributionGradeProjectOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatednoncompetitiveprojectcv_{ResourceID}_{item5.Roh_relatedNonCompetitiveProjectCV.ArticleID}",  "http://w3id.org/roh/contributionGradeProjectOther", $"\"{GenerarTextoSinSaltoDeLinea(item5.Roh_relatedNonCompetitiveProjectCV.Roh_contributionGradeProjectOther).ToLower()}\"", list, " . ");
				}
			}
				if(item5.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatednoncompetitiveproject_{ResourceID}_{item5.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item5.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item5.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item5.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatednoncompetitiveproject_{ResourceID}_{item5.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificExperience.Roh_patents != null)
			{
			foreach(var item6 in this.Roh_scientificExperience.Roh_patents)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificexperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}", "http://w3id.org/roh/patents", $"<{resourceAPI.GraphsUrl}items/relatedpatent_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedpatent_{ResourceID}_{item6.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item6.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item6.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item6.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedpatent_{ResourceID}_{item6.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
				if(this.Roh_scientificExperience.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificexperience_{ResourceID}_{this.Roh_scientificExperience.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_scientificExperience.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_scientificActivity != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/scientificActivity", $"<{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}>", list, " . ");
			if(this.Roh_scientificActivity.Roh_otherDistinctions != null)
			{
			foreach(var item1 in this.Roh_scientificActivity.Roh_otherDistinctions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/otherDistinctions", $"<{resourceAPI.GraphsUrl}items/relatedotherdistinction_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedotherdistinction_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedotherdistinction_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_generalQualityIndicators != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/generalQualityIndicators", $"<{resourceAPI.GraphsUrl}items/generalqualityindicator_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.ArticleID}>", list, " . ");
			if(this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/generalqualityindicator_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.ArticleID}", "http://w3id.org/roh/generalQualityIndicatorCV", $"<{resourceAPI.GraphsUrl}items/generalqualityindicatorcv_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.ArticleID}>", list, " . ");
				if(this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.Roh_generalQualityIndicator != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/generalqualityindicatorcv_{ResourceID}_{this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.ArticleID}",  "http://w3id.org/roh/generalQualityIndicator", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_scientificActivity.Roh_generalQualityIndicators.Roh_generalQualityIndicatorCV.Roh_generalQualityIndicator).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_worksSubmittedConferences != null)
			{
			foreach(var item3 in this.Roh_scientificActivity.Roh_worksSubmittedConferences)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/worksSubmittedConferences", $"<{resourceAPI.GraphsUrl}items/relatedworksubmittedconferences_{ResourceID}_{item3.ArticleID}>", list, " . ");
			if(item3.Roh_relatedWorkSubmittedConferencesCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedconferences_{ResourceID}_{item3.ArticleID}", "http://w3id.org/roh/relatedWorkSubmittedConferencesCV", $"<{resourceAPI.GraphsUrl}items/relatedworksubmittedconferencescv_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}>", list, " . ");
				if(item3.Roh_relatedWorkSubmittedConferencesCV.IdRoh_participationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.Roh_relatedWorkSubmittedConferencesCV.IdRoh_participationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedconferencescv_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}",  "http://w3id.org/roh/participationType", $"<{itemRegex}>", list, " . ");
				}
				if(item3.Roh_relatedWorkSubmittedConferencesCV.IdRoh_inscriptionType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.Roh_relatedWorkSubmittedConferencesCV.IdRoh_inscriptionType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedconferencescv_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}",  "http://w3id.org/roh/inscriptionType", $"<{itemRegex}>", list, " . ");
				}
				if(item3.Roh_relatedWorkSubmittedConferencesCV.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedconferencescv_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}",  "http://w3id.org/roh/correspondingAuthor", $"\"{item3.Roh_relatedWorkSubmittedConferencesCV.Roh_correspondingAuthor.ToString().ToLower()}\"", list, " . ");
				}
				if(item3.Roh_relatedWorkSubmittedConferencesCV.Roh_inscriptionTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedconferencescv_{ResourceID}_{item3.Roh_relatedWorkSubmittedConferencesCV.ArticleID}",  "http://w3id.org/roh/inscriptionTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item3.Roh_relatedWorkSubmittedConferencesCV.Roh_inscriptionTypeOther).ToLower()}\"", list, " . ");
				}
			}
				if(item3.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedconferences_{ResourceID}_{item3.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item3.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item3.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedconferences_{ResourceID}_{item3.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_societies != null)
			{
			foreach(var item4 in this.Roh_scientificActivity.Roh_societies)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/societies", $"<{resourceAPI.GraphsUrl}items/relatedsociety_{ResourceID}_{item4.ArticleID}>", list, " . ");
				if(item4.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedsociety_{ResourceID}_{item4.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item4.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item4.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item4.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedsociety_{ResourceID}_{item4.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_otherDisseminationActivities != null)
			{
			foreach(var item5 in this.Roh_scientificActivity.Roh_otherDisseminationActivities)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/otherDisseminationActivities", $"<{resourceAPI.GraphsUrl}items/relatedotherdisseminationactivity_{ResourceID}_{item5.ArticleID}>", list, " . ");
				if(item5.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedotherdisseminationactivity_{ResourceID}_{item5.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item5.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item5.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item5.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedotherdisseminationactivity_{ResourceID}_{item5.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_councils != null)
			{
			foreach(var item6 in this.Roh_scientificActivity.Roh_councils)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/councils", $"<{resourceAPI.GraphsUrl}items/relatedcouncil_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcouncil_{ResourceID}_{item6.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item6.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item6.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item6.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcouncil_{ResourceID}_{item6.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_scientificProduction != null)
			{
			foreach(var item7 in this.Roh_scientificActivity.Roh_scientificProduction)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/scientificProduction", $"<{resourceAPI.GraphsUrl}items/relatedscientificproduction_{ResourceID}_{item7.ArticleID}>", list, " . ");
				if(item7.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedscientificproduction_{ResourceID}_{item7.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item7.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item7.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item7.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedscientificproduction_{ResourceID}_{item7.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_forums != null)
			{
			foreach(var item8 in this.Roh_scientificActivity.Roh_forums)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/forums", $"<{resourceAPI.GraphsUrl}items/relatedforum_{ResourceID}_{item8.ArticleID}>", list, " . ");
				if(item8.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedforum_{ResourceID}_{item8.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item8.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item8.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item8.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedforum_{ResourceID}_{item8.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_researchActivityPeriods != null)
			{
			foreach(var item9 in this.Roh_scientificActivity.Roh_researchActivityPeriods)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/researchActivityPeriods", $"<{resourceAPI.GraphsUrl}items/relatedresearchactivityperiod_{ResourceID}_{item9.ArticleID}>", list, " . ");
				if(item9.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedresearchactivityperiod_{ResourceID}_{item9.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item9.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item9.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item9.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedresearchactivityperiod_{ResourceID}_{item9.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_scientificPublications != null)
			{
			foreach(var item10 in this.Roh_scientificActivity.Roh_scientificPublications)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/scientificPublications", $"<{resourceAPI.GraphsUrl}items/relatedscientificpublication_{ResourceID}_{item10.ArticleID}>", list, " . ");
			if(item10.Roh_relatedScientificPublicationCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedscientificpublication_{ResourceID}_{item10.ArticleID}", "http://w3id.org/roh/relatedScientificPublicationCV", $"<{resourceAPI.GraphsUrl}items/relatedscientificpublicationcv_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}>", list, " . ");
				if(item10.Roh_relatedScientificPublicationCV.IdRoh_contributionGrade != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item10.Roh_relatedScientificPublicationCV.IdRoh_contributionGrade;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedscientificpublicationcv_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}",  "http://w3id.org/roh/contributionGrade", $"<{itemRegex}>", list, " . ");
				}
				if(item10.Roh_relatedScientificPublicationCV.Roh_relevantResults != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedscientificpublicationcv_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}",  "http://w3id.org/roh/relevantResults", $"\"{GenerarTextoSinSaltoDeLinea(item10.Roh_relatedScientificPublicationCV.Roh_relevantResults).ToLower()}\"", list, " . ");
				}
				if(item10.Roh_relatedScientificPublicationCV.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedscientificpublicationcv_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}",  "http://w3id.org/roh/correspondingAuthor", $"\"{item10.Roh_relatedScientificPublicationCV.Roh_correspondingAuthor.ToString().ToLower()}\"", list, " . ");
				}
				if(item10.Roh_relatedScientificPublicationCV.Roh_relevantPublication != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedscientificpublicationcv_{ResourceID}_{item10.Roh_relatedScientificPublicationCV.ArticleID}",  "http://w3id.org/roh/relevantPublication", $"\"{item10.Roh_relatedScientificPublicationCV.Roh_relevantPublication.ToString().ToLower()}\"", list, " . ");
				}
			}
				if(item10.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedscientificpublication_{ResourceID}_{item10.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item10.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item10.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item10.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedscientificpublication_{ResourceID}_{item10.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_otherAchievements != null)
			{
			foreach(var item11 in this.Roh_scientificActivity.Roh_otherAchievements)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/otherAchievements", $"<{resourceAPI.GraphsUrl}items/relatedotherachievement_{ResourceID}_{item11.ArticleID}>", list, " . ");
				if(item11.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedotherachievement_{ResourceID}_{item11.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item11.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item11.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item11.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedotherachievement_{ResourceID}_{item11.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_researchEvaluations != null)
			{
			foreach(var item12 in this.Roh_scientificActivity.Roh_researchEvaluations)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/researchEvaluations", $"<{resourceAPI.GraphsUrl}items/relatedresearchevaluation_{ResourceID}_{item12.ArticleID}>", list, " . ");
				if(item12.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedresearchevaluation_{ResourceID}_{item12.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item12.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item12.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item12.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedresearchevaluation_{ResourceID}_{item12.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_activitiesOrganization != null)
			{
			foreach(var item13 in this.Roh_scientificActivity.Roh_activitiesOrganization)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/activitiesOrganization", $"<{resourceAPI.GraphsUrl}items/relatedactivityorganization_{ResourceID}_{item13.ArticleID}>", list, " . ");
				if(item13.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedactivityorganization_{ResourceID}_{item13.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item13.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item13.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item13.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedactivityorganization_{ResourceID}_{item13.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_otherCollaborations != null)
			{
			foreach(var item14 in this.Roh_scientificActivity.Roh_otherCollaborations)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/otherCollaborations", $"<{resourceAPI.GraphsUrl}items/relatedothercollaboration_{ResourceID}_{item14.ArticleID}>", list, " . ");
				if(item14.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedothercollaboration_{ResourceID}_{item14.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item14.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item14.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item14.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedothercollaboration_{ResourceID}_{item14.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_networks != null)
			{
			foreach(var item15 in this.Roh_scientificActivity.Roh_networks)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/networks", $"<{resourceAPI.GraphsUrl}items/relatednetwork_{ResourceID}_{item15.ArticleID}>", list, " . ");
				if(item15.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatednetwork_{ResourceID}_{item15.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item15.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item15.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item15.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatednetwork_{ResourceID}_{item15.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_grants != null)
			{
			foreach(var item16 in this.Roh_scientificActivity.Roh_grants)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/grants", $"<{resourceAPI.GraphsUrl}items/relatedgrant_{ResourceID}_{item16.ArticleID}>", list, " . ");
				if(item16.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedgrant_{ResourceID}_{item16.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item16.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item16.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item16.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedgrant_{ResourceID}_{item16.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_activitiesManagement != null)
			{
			foreach(var item17 in this.Roh_scientificActivity.Roh_activitiesManagement)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/activitiesManagement", $"<{resourceAPI.GraphsUrl}items/relatedactivitymanagement_{ResourceID}_{item17.ArticleID}>", list, " . ");
				if(item17.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedactivitymanagement_{ResourceID}_{item17.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item17.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item17.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item17.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedactivitymanagement_{ResourceID}_{item17.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_prizes != null)
			{
			foreach(var item18 in this.Roh_scientificActivity.Roh_prizes)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/prizes", $"<{resourceAPI.GraphsUrl}items/relatedprize_{ResourceID}_{item18.ArticleID}>", list, " . ");
				if(item18.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedprize_{ResourceID}_{item18.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item18.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item18.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item18.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedprize_{ResourceID}_{item18.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_committees != null)
			{
			foreach(var item19 in this.Roh_scientificActivity.Roh_committees)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/committees", $"<{resourceAPI.GraphsUrl}items/relatedcommittee_{ResourceID}_{item19.ArticleID}>", list, " . ");
				if(item19.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcommittee_{ResourceID}_{item19.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item19.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item19.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item19.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcommittee_{ResourceID}_{item19.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_stays != null)
			{
			foreach(var item20 in this.Roh_scientificActivity.Roh_stays)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/stays", $"<{resourceAPI.GraphsUrl}items/relatedstay_{ResourceID}_{item20.ArticleID}>", list, " . ");
				if(item20.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedstay_{ResourceID}_{item20.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item20.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item20.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item20.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedstay_{ResourceID}_{item20.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_obtainedRecognitions != null)
			{
			foreach(var item21 in this.Roh_scientificActivity.Roh_obtainedRecognitions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/obtainedRecognitions", $"<{resourceAPI.GraphsUrl}items/relatedobtainedrecognition_{ResourceID}_{item21.ArticleID}>", list, " . ");
				if(item21.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedobtainedrecognition_{ResourceID}_{item21.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item21.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item21.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item21.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedobtainedrecognition_{ResourceID}_{item21.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_scientificActivity.Roh_worksSubmittedSeminars != null)
			{
			foreach(var item22 in this.Roh_scientificActivity.Roh_worksSubmittedSeminars)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}", "http://w3id.org/roh/worksSubmittedSeminars", $"<{resourceAPI.GraphsUrl}items/relatedworksubmittedseminars_{ResourceID}_{item22.ArticleID}>", list, " . ");
			if(item22.Roh_relatedWorkSubmittedSeminarsCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedseminars_{ResourceID}_{item22.ArticleID}", "http://w3id.org/roh/relatedWorkSubmittedSeminarsCV", $"<{resourceAPI.GraphsUrl}items/relatedworksubmittedseminarscv_{ResourceID}_{item22.Roh_relatedWorkSubmittedSeminarsCV.ArticleID}>", list, " . ");
				if(item22.Roh_relatedWorkSubmittedSeminarsCV.IdRoh_inscriptionType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item22.Roh_relatedWorkSubmittedSeminarsCV.IdRoh_inscriptionType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedseminarscv_{ResourceID}_{item22.Roh_relatedWorkSubmittedSeminarsCV.ArticleID}",  "http://w3id.org/roh/inscriptionType", $"<{itemRegex}>", list, " . ");
				}
				if(item22.Roh_relatedWorkSubmittedSeminarsCV.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedseminarscv_{ResourceID}_{item22.Roh_relatedWorkSubmittedSeminarsCV.ArticleID}",  "http://w3id.org/roh/correspondingAuthor", $"\"{item22.Roh_relatedWorkSubmittedSeminarsCV.Roh_correspondingAuthor.ToString().ToLower()}\"", list, " . ");
				}
			}
				if(item22.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedseminars_{ResourceID}_{item22.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item22.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item22.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item22.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedworksubmittedseminars_{ResourceID}_{item22.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
				if(this.Roh_scientificActivity.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/scientificactivity_{ResourceID}_{this.Roh_scientificActivity.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_scientificActivity.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_freeTextSummary != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/freeTextSummary", $"<{resourceAPI.GraphsUrl}items/freetextsummary_{ResourceID}_{this.Roh_freeTextSummary.ArticleID}>", list, " . ");
			if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/freetextsummary_{ResourceID}_{this.Roh_freeTextSummary.ArticleID}", "http://w3id.org/roh/freeTextSummaryValues", $"<{resourceAPI.GraphsUrl}items/freetextsummaryvalues_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.ArticleID}>", list, " . ");
			if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/freetextsummaryvalues_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.ArticleID}", "http://w3id.org/roh/freeTextSummaryValuesCV", $"<{resourceAPI.GraphsUrl}items/freetextsummaryvaluescv_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}>", list, " . ");
				if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summaryTFG != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/freetextsummaryvaluescv_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}",  "http://w3id.org/roh/summaryTFG", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summaryTFG).ToLower()}\"", list, " . ");
				}
				if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summaryTFM != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/freetextsummaryvaluescv_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}",  "http://w3id.org/roh/summaryTFM", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summaryTFM).ToLower()}\"", list, " . ");
				}
				if(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summary != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/freetextsummaryvaluescv_{ResourceID}_{this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.ArticleID}",  "http://w3id.org/roh/summary", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_freeTextSummary.Roh_freeTextSummaryValues.Roh_freeTextSummaryValuesCV.Roh_summary).ToLower()}\"", list, " . ");
				}
			}
			}
				if(this.Roh_freeTextSummary.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/freetextsummary_{ResourceID}_{this.Roh_freeTextSummary.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_freeTextSummary.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_qualifications != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/qualifications", $"<{resourceAPI.GraphsUrl}items/qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}>", list, " . ");
			if(this.Roh_qualifications.Roh_languageSkills != null)
			{
			foreach(var item1 in this.Roh_qualifications.Roh_languageSkills)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/languageSkills", $"<{resourceAPI.GraphsUrl}items/relatedlanguageskills_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedlanguageskills_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedlanguageskills_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_firstSecondCycles != null)
			{
			foreach(var item2 in this.Roh_qualifications.Roh_firstSecondCycles)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/firstSecondCycles", $"<{resourceAPI.GraphsUrl}items/relatedfirstsecondcycles_{ResourceID}_{item2.ArticleID}>", list, " . ");
				if(item2.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedfirstsecondcycles_{ResourceID}_{item2.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item2.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item2.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedfirstsecondcycles_{ResourceID}_{item2.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_postgraduates != null)
			{
			foreach(var item3 in this.Roh_qualifications.Roh_postgraduates)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/postgraduates", $"<{resourceAPI.GraphsUrl}items/relatedpostgraduates_{ResourceID}_{item3.ArticleID}>", list, " . ");
				if(item3.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedpostgraduates_{ResourceID}_{item3.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item3.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item3.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedpostgraduates_{ResourceID}_{item3.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_doctorates != null)
			{
			foreach(var item4 in this.Roh_qualifications.Roh_doctorates)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/doctorates", $"<{resourceAPI.GraphsUrl}items/relateddoctorates_{ResourceID}_{item4.ArticleID}>", list, " . ");
				if(item4.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relateddoctorates_{ResourceID}_{item4.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item4.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item4.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item4.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relateddoctorates_{ResourceID}_{item4.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_specialisedTraining != null)
			{
			foreach(var item5 in this.Roh_qualifications.Roh_specialisedTraining)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/specialisedTraining", $"<{resourceAPI.GraphsUrl}items/relatedspecialisedtrainings_{ResourceID}_{item5.ArticleID}>", list, " . ");
				if(item5.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedspecialisedtrainings_{ResourceID}_{item5.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item5.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item5.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item5.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedspecialisedtrainings_{ResourceID}_{item5.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_qualifications.Roh_coursesAndSeminars != null)
			{
			foreach(var item6 in this.Roh_qualifications.Roh_coursesAndSeminars)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}", "http://w3id.org/roh/coursesAndSeminars", $"<{resourceAPI.GraphsUrl}items/relatedcoursesandseminars_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcoursesandseminars_{ResourceID}_{item6.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item6.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item6.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item6.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedcoursesandseminars_{ResourceID}_{item6.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
				if(this.Roh_qualifications.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/qualifications_{ResourceID}_{this.Roh_qualifications.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualifications.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_personalData != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/personalData", $"<{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}>", list, " . ");
			if(this.Roh_personalData.Roh_hasFax != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://w3id.org/roh/hasFax", $"<{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Roh_hasFax.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasFax.Roh_hasExtension).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_hasFax.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasFax.Roh_hasInternationalCode).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_hasFax.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Roh_hasFax.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasFax.Vcard_hasValue).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Vcard_hasTelephone != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}", "https://www.w3.org/2006/vcard/ns#hasTelephone", $"<{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Vcard_hasTelephone.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_hasTelephone.Roh_hasExtension).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_hasTelephone.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_hasTelephone.Roh_hasInternationalCode).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_hasTelephone.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Vcard_hasTelephone.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_hasTelephone.Vcard_hasValue).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Roh_hasMobilePhone != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://w3id.org/roh/hasMobilePhone", $"<{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Roh_hasMobilePhone.Roh_hasExtension != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}",  "http://w3id.org/roh/hasExtension", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasMobilePhone.Roh_hasExtension).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_hasMobilePhone.Roh_hasInternationalCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}",  "http://w3id.org/roh/hasInternationalCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasMobilePhone.Roh_hasInternationalCode).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_hasMobilePhone.Vcard_hasValue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/telephonetype_{ResourceID}_{this.Roh_personalData.Roh_hasMobilePhone.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasValue", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_hasMobilePhone.Vcard_hasValue).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Roh_birthplace != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://w3id.org/roh/birthplace", $"<{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Roh_birthplace.IdVcard_hasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Roh_personalData.Roh_birthplace.IdVcard_hasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.IdRoh_hasProvince != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Roh_personalData.Roh_birthplace.IdRoh_hasProvince;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "http://w3id.org/roh/hasProvince", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.IdVcard_hasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Roh_personalData.Roh_birthplace.IdVcard_hasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.Vcard_postal_code != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#postal-code", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_birthplace.Vcard_postal_code).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.Vcard_extended_address != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#extended-address", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_birthplace.Vcard_extended_address).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.Vcard_street_address != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#street-address", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_birthplace.Vcard_street_address).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_birthplace.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Roh_birthplace.ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_birthplace.Vcard_locality).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Vcard_address != null)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}", "https://www.w3.org/2006/vcard/ns#address", $"<{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}>", list, " . ");
				if(this.Roh_personalData.Vcard_address.IdVcard_hasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Roh_personalData.Vcard_address.IdVcard_hasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.IdRoh_hasProvince != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Roh_personalData.Vcard_address.IdRoh_hasProvince;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "http://w3id.org/roh/hasProvince", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.IdVcard_hasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Roh_personalData.Vcard_address.IdVcard_hasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.Vcard_postal_code != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#postal-code", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_address.Vcard_postal_code).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.Vcard_extended_address != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#extended-address", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_address.Vcard_extended_address).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.Vcard_street_address != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#street-address", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_address.Vcard_street_address).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_address.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/address_{ResourceID}_{this.Roh_personalData.Vcard_address.ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_address.Vcard_locality).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_personalData.Roh_otherIds != null)
			{
			foreach(var item6 in this.Roh_personalData.Roh_otherIds)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}", "http://w3id.org/roh/otherIds", $"<{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Foaf_topic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item6.ArticleID}",  "http://xmlns.com/foaf/0.1/topic", $"\"{GenerarTextoSinSaltoDeLinea(item6.Foaf_topic).ToLower()}\"", list, " . ");
				}
				if(item6.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item6.ArticleID}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(item6.Dc_title).ToLower()}\"", list, " . ");
				}
			}
			}
				if(this.Roh_personalData.IdFoaf_gender != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Roh_personalData.IdFoaf_gender;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/gender", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_personalData.IdSchema_nationality != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.Roh_personalData.IdSchema_nationality;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://www.schema.org/nationality", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_personalData.Roh_nie != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/nie", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_nie).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Vivo_researcherId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://vivoweb.org/ontology/core#researcherId", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vivo_researcherId).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Vivo_scopusId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://vivoweb.org/ontology/core#scopusId", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vivo_scopusId).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Foaf_familyName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_secondFamilyName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_email != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "https://www.w3.org/2006/vcard/ns#email", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Vcard_email).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Foaf_img != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/img", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Foaf_img).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_dni != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/dni", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_dni).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Foaf_homepage != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/homepage", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Foaf_homepage).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_ORCID != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/ORCID", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_ORCID).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Roh_passport != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://w3id.org/roh/passport", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Roh_passport).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personalData.Vcard_birth_date != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "https://www.w3.org/2006/vcard/ns#birth-date", $"{this.Roh_personalData.Vcard_birth_date.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_personalData.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaldata_{ResourceID}_{this.Roh_personalData.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_personalData.Foaf_firstName).ToLower()}\"", list, " . ");
				}
			}
			if(this.Roh_teachingExperience != null)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/teachingExperience", $"<{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}>", list, " . ");
			if(this.Roh_teachingExperience.Roh_thesisSupervisions != null)
			{
			foreach(var item1 in this.Roh_teachingExperience.Roh_thesisSupervisions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/thesisSupervisions", $"<{resourceAPI.GraphsUrl}items/relatedthesissupervisions_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedthesissupervisions_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item1.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item1.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item1.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedthesissupervisions_{ResourceID}_{item1.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_otherActivities != null)
			{
			foreach(var item2 in this.Roh_teachingExperience.Roh_otherActivities)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/otherActivities", $"<{resourceAPI.GraphsUrl}items/relatedotheractivities_{ResourceID}_{item2.ArticleID}>", list, " . ");
				if(item2.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedotheractivities_{ResourceID}_{item2.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item2.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item2.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedotheractivities_{ResourceID}_{item2.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_teachingCongress != null)
			{
			foreach(var item3 in this.Roh_teachingExperience.Roh_teachingCongress)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/teachingCongress", $"<{resourceAPI.GraphsUrl}items/relatedteachingcongress_{ResourceID}_{item3.ArticleID}>", list, " . ");
				if(item3.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedteachingcongress_{ResourceID}_{item3.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item3.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item3.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item3.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedteachingcongress_{ResourceID}_{item3.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_mostRelevantContributions != null)
			{
			foreach(var item4 in this.Roh_teachingExperience.Roh_mostRelevantContributions)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/mostRelevantContributions", $"<{resourceAPI.GraphsUrl}items/relatedmostrelevantcontributions_{ResourceID}_{item4.ArticleID}>", list, " . ");
				if(item4.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedmostrelevantcontributions_{ResourceID}_{item4.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item4.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item4.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item4.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedmostrelevantcontributions_{ResourceID}_{item4.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_teachingProjects != null)
			{
			foreach(var item5 in this.Roh_teachingExperience.Roh_teachingProjects)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/teachingProjects", $"<{resourceAPI.GraphsUrl}items/relatedteachingprojects_{ResourceID}_{item5.ArticleID}>", list, " . ");
				if(item5.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedteachingprojects_{ResourceID}_{item5.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item5.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item5.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item5.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedteachingprojects_{ResourceID}_{item5.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_academicTutorials != null)
			{
			foreach(var item6 in this.Roh_teachingExperience.Roh_academicTutorials)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/academicTutorials", $"<{resourceAPI.GraphsUrl}items/relatedacademictutorials_{ResourceID}_{item6.ArticleID}>", list, " . ");
				if(item6.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedacademictutorials_{ResourceID}_{item6.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item6.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item6.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item6.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedacademictutorials_{ResourceID}_{item6.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_impartedCoursesSeminars != null)
			{
			foreach(var item7 in this.Roh_teachingExperience.Roh_impartedCoursesSeminars)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/impartedCoursesSeminars", $"<{resourceAPI.GraphsUrl}items/relatedimpartedcoursesseminars_{ResourceID}_{item7.ArticleID}>", list, " . ");
				if(item7.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedimpartedcoursesseminars_{ResourceID}_{item7.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item7.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item7.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item7.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedimpartedcoursesseminars_{ResourceID}_{item7.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_teachingInnovationAwardsReceived != null)
			{
			foreach(var item8 in this.Roh_teachingExperience.Roh_teachingInnovationAwardsReceived)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/teachingInnovationAwardsReceived", $"<{resourceAPI.GraphsUrl}items/relatedteachinginnovationawardsreceived_{ResourceID}_{item8.ArticleID}>", list, " . ");
				if(item8.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedteachinginnovationawardsreceived_{ResourceID}_{item8.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item8.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item8.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item8.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedteachinginnovationawardsreceived_{ResourceID}_{item8.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_teachingPublications != null)
			{
			foreach(var item9 in this.Roh_teachingExperience.Roh_teachingPublications)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/teachingPublications", $"<{resourceAPI.GraphsUrl}items/relatedteachingpublications_{ResourceID}_{item9.ArticleID}>", list, " . ");
				if(item9.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedteachingpublications_{ResourceID}_{item9.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item9.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item9.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item9.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedteachingpublications_{ResourceID}_{item9.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_teachingExperience.Roh_impartedAcademicTrainings != null)
			{
			foreach(var item10 in this.Roh_teachingExperience.Roh_impartedAcademicTrainings)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}", "http://w3id.org/roh/impartedAcademicTrainings", $"<{resourceAPI.GraphsUrl}items/relatedimpartedacademictrainings_{ResourceID}_{item10.ArticleID}>", list, " . ");
				if(item10.Roh_isPublic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedimpartedacademictrainings_{ResourceID}_{item10.ArticleID}",  "http://w3id.org/roh/isPublic", $"\"{item10.Roh_isPublic.ToString().ToLower()}\"", list, " . ");
				}
				if(item10.IdVivo_relatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item10.IdVivo_relatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/relatedimpartedacademictrainings_{ResourceID}_{item10.ArticleID}",  "http://vivoweb.org/ontology/core#relatedBy", $"<{itemRegex}>", list, " . ");
				}
			}
			}
				if(this.Roh_teachingExperience.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/teachingexperience_{ResourceID}_{this.Roh_teachingExperience.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_teachingExperience.Roh_title).ToLower()}\"", list, " . ");
				}
			}
				if(this.IdRoh_cvOf != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_cvOf;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/cvOf", $"<{itemRegex}>", list, " . ");
				}
				if(this.Foaf_name != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://xmlns.com/foaf/0.1/name", $"\"{GenerarTextoSinSaltoDeLinea(this.Foaf_name).ToLower()}\"", list, " . ");
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
			string titulo = $"{this.Foaf_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Foaf_name.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/CurriculumvitaeOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Foaf_name;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Foaf_name;
		}




	}
}
