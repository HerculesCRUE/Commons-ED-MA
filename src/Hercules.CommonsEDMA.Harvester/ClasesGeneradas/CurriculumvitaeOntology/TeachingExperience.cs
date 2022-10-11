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
	public class TeachingExperience : GnossOCBase
	{

		public TeachingExperience() : base() { } 

		public TeachingExperience(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_thesisSupervisions = new List<RelatedThesisSupervisions>();
			SemanticPropertyModel propRoh_thesisSupervisions = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/thesisSupervisions");
			if(propRoh_thesisSupervisions != null && propRoh_thesisSupervisions.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_thesisSupervisions.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedThesisSupervisions roh_thesisSupervisions = new RelatedThesisSupervisions(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_thesisSupervisions.Add(roh_thesisSupervisions);
					}
				}
			}
			this.Roh_teachingCongress = new List<RelatedTeachingCongress>();
			SemanticPropertyModel propRoh_teachingCongress = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingCongress");
			if(propRoh_teachingCongress != null && propRoh_teachingCongress.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_teachingCongress.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedTeachingCongress roh_teachingCongress = new RelatedTeachingCongress(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_teachingCongress.Add(roh_teachingCongress);
					}
				}
			}
			this.Roh_otherActivities = new List<RelatedOtherActivities>();
			SemanticPropertyModel propRoh_otherActivities = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/otherActivities");
			if(propRoh_otherActivities != null && propRoh_otherActivities.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_otherActivities.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedOtherActivities roh_otherActivities = new RelatedOtherActivities(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_otherActivities.Add(roh_otherActivities);
					}
				}
			}
			this.Roh_mostRelevantContributions = new List<RelatedMostRelevantContributions>();
			SemanticPropertyModel propRoh_mostRelevantContributions = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/mostRelevantContributions");
			if(propRoh_mostRelevantContributions != null && propRoh_mostRelevantContributions.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_mostRelevantContributions.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedMostRelevantContributions roh_mostRelevantContributions = new RelatedMostRelevantContributions(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_mostRelevantContributions.Add(roh_mostRelevantContributions);
					}
				}
			}
			this.Roh_teachingProjects = new List<RelatedTeachingProjects>();
			SemanticPropertyModel propRoh_teachingProjects = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingProjects");
			if(propRoh_teachingProjects != null && propRoh_teachingProjects.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_teachingProjects.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedTeachingProjects roh_teachingProjects = new RelatedTeachingProjects(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_teachingProjects.Add(roh_teachingProjects);
					}
				}
			}
			this.Roh_academicTutorials = new List<RelatedAcademicTutorials>();
			SemanticPropertyModel propRoh_academicTutorials = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/academicTutorials");
			if(propRoh_academicTutorials != null && propRoh_academicTutorials.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_academicTutorials.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedAcademicTutorials roh_academicTutorials = new RelatedAcademicTutorials(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_academicTutorials.Add(roh_academicTutorials);
					}
				}
			}
			this.Roh_impartedCoursesSeminars = new List<RelatedImpartedCoursesSeminars>();
			SemanticPropertyModel propRoh_impartedCoursesSeminars = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impartedCoursesSeminars");
			if(propRoh_impartedCoursesSeminars != null && propRoh_impartedCoursesSeminars.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_impartedCoursesSeminars.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedImpartedCoursesSeminars roh_impartedCoursesSeminars = new RelatedImpartedCoursesSeminars(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_impartedCoursesSeminars.Add(roh_impartedCoursesSeminars);
					}
				}
			}
			this.Roh_teachingInnovationAwardsReceived = new List<RelatedTeachingInnovationAwardsReceived>();
			SemanticPropertyModel propRoh_teachingInnovationAwardsReceived = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingInnovationAwardsReceived");
			if(propRoh_teachingInnovationAwardsReceived != null && propRoh_teachingInnovationAwardsReceived.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_teachingInnovationAwardsReceived.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedTeachingInnovationAwardsReceived roh_teachingInnovationAwardsReceived = new RelatedTeachingInnovationAwardsReceived(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_teachingInnovationAwardsReceived.Add(roh_teachingInnovationAwardsReceived);
					}
				}
			}
			this.Roh_teachingPublications = new List<RelatedTeachingPublications>();
			SemanticPropertyModel propRoh_teachingPublications = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingPublications");
			if(propRoh_teachingPublications != null && propRoh_teachingPublications.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_teachingPublications.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedTeachingPublications roh_teachingPublications = new RelatedTeachingPublications(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_teachingPublications.Add(roh_teachingPublications);
					}
				}
			}
			this.Roh_impartedAcademicTrainings = new List<RelatedImpartedAcademicTrainings>();
			SemanticPropertyModel propRoh_impartedAcademicTrainings = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impartedAcademicTrainings");
			if(propRoh_impartedAcademicTrainings != null && propRoh_impartedAcademicTrainings.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_impartedAcademicTrainings.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedImpartedAcademicTrainings roh_impartedAcademicTrainings = new RelatedImpartedAcademicTrainings(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_impartedAcademicTrainings.Add(roh_impartedAcademicTrainings);
					}
				}
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/TeachingExperience"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/TeachingExperience"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/thesisSupervisions")]
		public  List<RelatedThesisSupervisions> Roh_thesisSupervisions { get; set;}

		[RDFProperty("http://w3id.org/roh/teachingCongress")]
		public  List<RelatedTeachingCongress> Roh_teachingCongress { get; set;}

		[RDFProperty("http://w3id.org/roh/otherActivities")]
		public  List<RelatedOtherActivities> Roh_otherActivities { get; set;}

		[RDFProperty("http://w3id.org/roh/mostRelevantContributions")]
		public  List<RelatedMostRelevantContributions> Roh_mostRelevantContributions { get; set;}

		[RDFProperty("http://w3id.org/roh/teachingProjects")]
		public  List<RelatedTeachingProjects> Roh_teachingProjects { get; set;}

		[RDFProperty("http://w3id.org/roh/academicTutorials")]
		public  List<RelatedAcademicTutorials> Roh_academicTutorials { get; set;}

		[RDFProperty("http://w3id.org/roh/impartedCoursesSeminars")]
		public  List<RelatedImpartedCoursesSeminars> Roh_impartedCoursesSeminars { get; set;}

		[RDFProperty("http://w3id.org/roh/teachingInnovationAwardsReceived")]
		public  List<RelatedTeachingInnovationAwardsReceived> Roh_teachingInnovationAwardsReceived { get; set;}

		[RDFProperty("http://w3id.org/roh/teachingPublications")]
		public  List<RelatedTeachingPublications> Roh_teachingPublications { get; set;}

		[RDFProperty("http://w3id.org/roh/impartedAcademicTrainings")]
		public  List<RelatedImpartedAcademicTrainings> Roh_impartedAcademicTrainings { get; set;}

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
			if(Roh_thesisSupervisions!=null){
				foreach(RelatedThesisSupervisions prop in Roh_thesisSupervisions){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedThesisSupervisions = new OntologyEntity("http://w3id.org/roh/RelatedThesisSupervisions", "http://w3id.org/roh/RelatedThesisSupervisions", "roh:thesisSupervisions", prop.propList, prop.entList);
				entList.Add(entityRelatedThesisSupervisions);
				prop.Entity= entityRelatedThesisSupervisions;
				}
			}
			if(Roh_teachingCongress!=null){
				foreach(RelatedTeachingCongress prop in Roh_teachingCongress){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedTeachingCongress = new OntologyEntity("http://w3id.org/roh/RelatedTeachingCongress", "http://w3id.org/roh/RelatedTeachingCongress", "roh:teachingCongress", prop.propList, prop.entList);
				entList.Add(entityRelatedTeachingCongress);
				prop.Entity= entityRelatedTeachingCongress;
				}
			}
			if(Roh_otherActivities!=null){
				foreach(RelatedOtherActivities prop in Roh_otherActivities){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedOtherActivities = new OntologyEntity("http://w3id.org/roh/RelatedOtherActivities", "http://w3id.org/roh/RelatedOtherActivities", "roh:otherActivities", prop.propList, prop.entList);
				entList.Add(entityRelatedOtherActivities);
				prop.Entity= entityRelatedOtherActivities;
				}
			}
			if(Roh_mostRelevantContributions!=null){
				foreach(RelatedMostRelevantContributions prop in Roh_mostRelevantContributions){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedMostRelevantContributions = new OntologyEntity("http://w3id.org/roh/RelatedMostRelevantContributions", "http://w3id.org/roh/RelatedMostRelevantContributions", "roh:mostRelevantContributions", prop.propList, prop.entList);
				entList.Add(entityRelatedMostRelevantContributions);
				prop.Entity= entityRelatedMostRelevantContributions;
				}
			}
			if(Roh_teachingProjects!=null){
				foreach(RelatedTeachingProjects prop in Roh_teachingProjects){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedTeachingProjects = new OntologyEntity("http://w3id.org/roh/RelatedTeachingProjects", "http://w3id.org/roh/RelatedTeachingProjects", "roh:teachingProjects", prop.propList, prop.entList);
				entList.Add(entityRelatedTeachingProjects);
				prop.Entity= entityRelatedTeachingProjects;
				}
			}
			if(Roh_academicTutorials!=null){
				foreach(RelatedAcademicTutorials prop in Roh_academicTutorials){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedAcademicTutorials = new OntologyEntity("http://w3id.org/roh/RelatedAcademicTutorials", "http://w3id.org/roh/RelatedAcademicTutorials", "roh:academicTutorials", prop.propList, prop.entList);
				entList.Add(entityRelatedAcademicTutorials);
				prop.Entity= entityRelatedAcademicTutorials;
				}
			}
			if(Roh_impartedCoursesSeminars!=null){
				foreach(RelatedImpartedCoursesSeminars prop in Roh_impartedCoursesSeminars){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedImpartedCoursesSeminars = new OntologyEntity("http://w3id.org/roh/RelatedImpartedCoursesSeminars", "http://w3id.org/roh/RelatedImpartedCoursesSeminars", "roh:impartedCoursesSeminars", prop.propList, prop.entList);
				entList.Add(entityRelatedImpartedCoursesSeminars);
				prop.Entity= entityRelatedImpartedCoursesSeminars;
				}
			}
			if(Roh_teachingInnovationAwardsReceived!=null){
				foreach(RelatedTeachingInnovationAwardsReceived prop in Roh_teachingInnovationAwardsReceived){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedTeachingInnovationAwardsReceived = new OntologyEntity("http://w3id.org/roh/RelatedTeachingInnovationAwardsReceived", "http://w3id.org/roh/RelatedTeachingInnovationAwardsReceived", "roh:teachingInnovationAwardsReceived", prop.propList, prop.entList);
				entList.Add(entityRelatedTeachingInnovationAwardsReceived);
				prop.Entity= entityRelatedTeachingInnovationAwardsReceived;
				}
			}
			if(Roh_teachingPublications!=null){
				foreach(RelatedTeachingPublications prop in Roh_teachingPublications){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedTeachingPublications = new OntologyEntity("http://w3id.org/roh/RelatedTeachingPublications", "http://w3id.org/roh/RelatedTeachingPublications", "roh:teachingPublications", prop.propList, prop.entList);
				entList.Add(entityRelatedTeachingPublications);
				prop.Entity= entityRelatedTeachingPublications;
				}
			}
			if(Roh_impartedAcademicTrainings!=null){
				foreach(RelatedImpartedAcademicTrainings prop in Roh_impartedAcademicTrainings){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedImpartedAcademicTrainings = new OntologyEntity("http://w3id.org/roh/RelatedImpartedAcademicTrainings", "http://w3id.org/roh/RelatedImpartedAcademicTrainings", "roh:impartedAcademicTrainings", prop.propList, prop.entList);
				entList.Add(entityRelatedImpartedAcademicTrainings);
				prop.Entity= entityRelatedImpartedAcademicTrainings;
				}
			}
		} 











	}
}
