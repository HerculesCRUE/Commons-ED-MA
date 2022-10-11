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
	public class Qualifications : GnossOCBase
	{

		public Qualifications() : base() { } 

		public Qualifications(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_languageSkills = new List<RelatedLanguageSkills>();
			SemanticPropertyModel propRoh_languageSkills = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/languageSkills");
			if(propRoh_languageSkills != null && propRoh_languageSkills.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_languageSkills.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedLanguageSkills roh_languageSkills = new RelatedLanguageSkills(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_languageSkills.Add(roh_languageSkills);
					}
				}
			}
			this.Roh_firstSecondCycles = new List<RelatedFirstSecondCycles>();
			SemanticPropertyModel propRoh_firstSecondCycles = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/firstSecondCycles");
			if(propRoh_firstSecondCycles != null && propRoh_firstSecondCycles.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_firstSecondCycles.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedFirstSecondCycles roh_firstSecondCycles = new RelatedFirstSecondCycles(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_firstSecondCycles.Add(roh_firstSecondCycles);
					}
				}
			}
			this.Roh_postgraduates = new List<RelatedPostGraduates>();
			SemanticPropertyModel propRoh_postgraduates = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/postgraduates");
			if(propRoh_postgraduates != null && propRoh_postgraduates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_postgraduates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedPostGraduates roh_postgraduates = new RelatedPostGraduates(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_postgraduates.Add(roh_postgraduates);
					}
				}
			}
			this.Roh_doctorates = new List<RelatedDoctorates>();
			SemanticPropertyModel propRoh_doctorates = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/doctorates");
			if(propRoh_doctorates != null && propRoh_doctorates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_doctorates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedDoctorates roh_doctorates = new RelatedDoctorates(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_doctorates.Add(roh_doctorates);
					}
				}
			}
			this.Roh_specialisedTraining = new List<RelatedSpecialisedTrainings>();
			SemanticPropertyModel propRoh_specialisedTraining = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/specialisedTraining");
			if(propRoh_specialisedTraining != null && propRoh_specialisedTraining.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_specialisedTraining.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedSpecialisedTrainings roh_specialisedTraining = new RelatedSpecialisedTrainings(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_specialisedTraining.Add(roh_specialisedTraining);
					}
				}
			}
			this.Roh_coursesAndSeminars = new List<RelatedCoursesAndSeminars>();
			SemanticPropertyModel propRoh_coursesAndSeminars = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/coursesAndSeminars");
			if(propRoh_coursesAndSeminars != null && propRoh_coursesAndSeminars.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_coursesAndSeminars.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedCoursesAndSeminars roh_coursesAndSeminars = new RelatedCoursesAndSeminars(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_coursesAndSeminars.Add(roh_coursesAndSeminars);
					}
				}
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/Qualifications"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/Qualifications"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/languageSkills")]
		public  List<RelatedLanguageSkills> Roh_languageSkills { get; set;}

		[RDFProperty("http://w3id.org/roh/firstSecondCycles")]
		public  List<RelatedFirstSecondCycles> Roh_firstSecondCycles { get; set;}

		[RDFProperty("http://w3id.org/roh/postgraduates")]
		public  List<RelatedPostGraduates> Roh_postgraduates { get; set;}

		[RDFProperty("http://w3id.org/roh/doctorates")]
		public  List<RelatedDoctorates> Roh_doctorates { get; set;}

		[RDFProperty("http://w3id.org/roh/specialisedTraining")]
		public  List<RelatedSpecialisedTrainings> Roh_specialisedTraining { get; set;}

		[RDFProperty("http://w3id.org/roh/coursesAndSeminars")]
		public  List<RelatedCoursesAndSeminars> Roh_coursesAndSeminars { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_languageSkills!=null){
				foreach(RelatedLanguageSkills prop in Roh_languageSkills){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedLanguageSkills = new OntologyEntity("http://w3id.org/roh/RelatedLanguageSkills", "http://w3id.org/roh/RelatedLanguageSkills", "roh:languageSkills", prop.propList, prop.entList);
				entList.Add(entityRelatedLanguageSkills);
				prop.Entity= entityRelatedLanguageSkills;
				}
			}
			if(Roh_firstSecondCycles!=null){
				foreach(RelatedFirstSecondCycles prop in Roh_firstSecondCycles){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedFirstSecondCycles = new OntologyEntity("http://w3id.org/roh/RelatedFirstSecondCycles", "http://w3id.org/roh/RelatedFirstSecondCycles", "roh:firstSecondCycles", prop.propList, prop.entList);
				entList.Add(entityRelatedFirstSecondCycles);
				prop.Entity= entityRelatedFirstSecondCycles;
				}
			}
			if(Roh_postgraduates!=null){
				foreach(RelatedPostGraduates prop in Roh_postgraduates){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedPostGraduates = new OntologyEntity("http://w3id.org/roh/RelatedPostGraduates", "http://w3id.org/roh/RelatedPostGraduates", "roh:postgraduates", prop.propList, prop.entList);
				entList.Add(entityRelatedPostGraduates);
				prop.Entity= entityRelatedPostGraduates;
				}
			}
			if(Roh_doctorates!=null){
				foreach(RelatedDoctorates prop in Roh_doctorates){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedDoctorates = new OntologyEntity("http://w3id.org/roh/RelatedDoctorates", "http://w3id.org/roh/RelatedDoctorates", "roh:doctorates", prop.propList, prop.entList);
				entList.Add(entityRelatedDoctorates);
				prop.Entity= entityRelatedDoctorates;
				}
			}
			if(Roh_specialisedTraining!=null){
				foreach(RelatedSpecialisedTrainings prop in Roh_specialisedTraining){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedSpecialisedTrainings = new OntologyEntity("http://w3id.org/roh/RelatedSpecialisedTrainings", "http://w3id.org/roh/RelatedSpecialisedTrainings", "roh:specialisedTraining", prop.propList, prop.entList);
				entList.Add(entityRelatedSpecialisedTrainings);
				prop.Entity= entityRelatedSpecialisedTrainings;
				}
			}
			if(Roh_coursesAndSeminars!=null){
				foreach(RelatedCoursesAndSeminars prop in Roh_coursesAndSeminars){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedCoursesAndSeminars = new OntologyEntity("http://w3id.org/roh/RelatedCoursesAndSeminars", "http://w3id.org/roh/RelatedCoursesAndSeminars", "roh:coursesAndSeminars", prop.propList, prop.entList);
				entList.Add(entityRelatedCoursesAndSeminars);
				prop.Entity= entityRelatedCoursesAndSeminars;
				}
			}
		} 











	}
}
