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

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class CV : GnossOCBase
	{
        public CV() : base() { } 
		public virtual string RdfType { get { return "http://w3id.org/roh/CV"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/CV"; } }
		[LABEL(LanguageEnum.es,"Usuario Gnoss")]
		[RDFProperty("http://w3id.org/roh/gnossUser")]
		public  object Roh_gnossUser  { get; set;} 
		public string IdRoh_gnossUser  { get; set;}

		[LABEL(LanguageEnum.es, "http://w3id.org/roh/professionalSituation")]
		[RDFProperty("http://w3id.org/roh/professionalSituation")]
		public ProfessionalSituation Roh_professionalSituation { get; set; }

		[LABEL(LanguageEnum.es, "http://w3id.org/roh/qualifications")]
		[RDFProperty("http://w3id.org/roh/qualifications")]
		public Qualifications Roh_qualifications { get; set; }

		[LABEL(LanguageEnum.es, "http://w3id.org/roh/teachingExperience")]
		[RDFProperty("http://w3id.org/roh/teachingExperience")]
		public TeachingExperience Roh_teachingExperience { get; set; }

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/scientificExperience")]
		[RDFProperty("http://w3id.org/roh/scientificExperience")]
		public  ScientificExperience Roh_scientificExperience { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/scientificActivity")]
		[RDFProperty("http://w3id.org/roh/scientificActivity")]
		public  ScientificActivity Roh_scientificActivity { get; set;}

		[LABEL(LanguageEnum.es, "http://w3id.org/roh/freeTextSummary")]
		[RDFProperty("http://w3id.org/roh/freeTextSummary")]
		public FreeTextSummary Roh_freeTextSummary { get; set; }

		[LABEL(LanguageEnum.es, "http://w3id.org/roh/researchObject")]
		[RDFProperty("http://w3id.org/roh/researchObject")]
		public ResearchObjects Roh_researchObject { get; set; }

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/personalData")]
		[RDFProperty("http://w3id.org/roh/personalData")]
		public  PersonalData Roh_personalData { get; set;}

		public string IdRoh_cvOf  { get; set;} 

		[LABEL(LanguageEnum.es,"http://xmlns.com/foaf/0.1/name")]
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

			Roh_personalData.GetProperties();
			Roh_personalData.GetEntities();
			OntologyEntity entityRoh_personalData = new("http://w3id.org/roh/PersonalData", "http://w3id.org/roh/PersonalData", "roh:personalData", Roh_personalData.propList, Roh_personalData.entList);
			entList.Add(entityRoh_personalData);

			Roh_professionalSituation.GetProperties();
			Roh_professionalSituation.GetEntities();
			OntologyEntity entityRoh_professionalSituation = new("http://w3id.org/roh/ProfessionalSituation", "http://w3id.org/roh/ProfessionalSituation", "roh:professionalSituation", Roh_professionalSituation.propList, Roh_professionalSituation.entList);
			entList.Add(entityRoh_professionalSituation);

			Roh_qualifications.GetProperties();
			Roh_qualifications.GetEntities();
			OntologyEntity entityRoh_qualifications = new("http://w3id.org/roh/Qualifications", "http://w3id.org/roh/Qualifications", "roh:qualifications", Roh_qualifications.propList, Roh_qualifications.entList);
			entList.Add(entityRoh_qualifications);

			Roh_teachingExperience.GetProperties();
			Roh_teachingExperience.GetEntities();
			OntologyEntity entityRoh_teachingExperience = new("http://w3id.org/roh/TeachingExperience", "http://w3id.org/roh/TeachingExperience", "roh:teachingExperience", Roh_teachingExperience.propList, Roh_teachingExperience.entList);
			entList.Add(entityRoh_teachingExperience);

			Roh_scientificExperience.GetProperties();
			Roh_scientificExperience.GetEntities();
			OntologyEntity entityRoh_scientificExperience = new ("http://w3id.org/roh/ScientificExperience", "http://w3id.org/roh/ScientificExperience", "roh:scientificExperience", Roh_scientificExperience.propList, Roh_scientificExperience.entList);
			entList.Add(entityRoh_scientificExperience);

			Roh_scientificActivity.GetProperties();
			Roh_scientificActivity.GetEntities();
			OntologyEntity entityRoh_scientificActivity = new ("http://w3id.org/roh/ScientificActivity", "http://w3id.org/roh/ScientificActivity", "roh:scientificActivity", Roh_scientificActivity.propList, Roh_scientificActivity.entList);
			entList.Add(entityRoh_scientificActivity);

			Roh_freeTextSummary.GetProperties();
			Roh_freeTextSummary.GetEntities();
			OntologyEntity entityRoh_freeTextSummary = new("http://w3id.org/roh/FreeTextSummary", "http://w3id.org/roh/FreeTextSummary", "roh:freeTextSummary", Roh_freeTextSummary.propList, Roh_freeTextSummary.entList);
			entList.Add(entityRoh_freeTextSummary);

			Roh_researchObject.GetProperties();
			Roh_researchObject.GetEntities();
			OntologyEntity entityRoh_researchObject = new("http://w3id.org/roh/ResearchObjects", "http://w3id.org/roh/ResearchObjects", "roh:researchObject", Roh_researchObject.propList, Roh_researchObject.entList);
			entList.Add(entityRoh_researchObject);
		} 

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new();
			Ontology ontology;
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
			return resource;
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
