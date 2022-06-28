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
	public class ScientificExperience : GnossOCBase
	{

		public ScientificExperience() : base() { } 

		public ScientificExperience(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_supervisedArtisticProjects = new List<RelatedSupervisedArtisticProject>();
			SemanticPropertyModel propRoh_supervisedArtisticProjects = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/supervisedArtisticProjects");
			if(propRoh_supervisedArtisticProjects != null && propRoh_supervisedArtisticProjects.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_supervisedArtisticProjects.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedSupervisedArtisticProject roh_supervisedArtisticProjects = new RelatedSupervisedArtisticProject(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_supervisedArtisticProjects.Add(roh_supervisedArtisticProjects);
					}
				}
			}
			this.Roh_competitiveProjects = new List<RelatedCompetitiveProject>();
			SemanticPropertyModel propRoh_competitiveProjects = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/competitiveProjects");
			if(propRoh_competitiveProjects != null && propRoh_competitiveProjects.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_competitiveProjects.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedCompetitiveProject roh_competitiveProjects = new RelatedCompetitiveProject(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_competitiveProjects.Add(roh_competitiveProjects);
					}
				}
			}
			this.Roh_groups = new List<RelatedGroup>();
			SemanticPropertyModel propRoh_groups = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/groups");
			if(propRoh_groups != null && propRoh_groups.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_groups.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedGroup roh_groups = new RelatedGroup(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_groups.Add(roh_groups);
					}
				}
			}
			this.Roh_technologicalResults = new List<RelatedTechnologicalResult>();
			SemanticPropertyModel propRoh_technologicalResults = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/technologicalResults");
			if(propRoh_technologicalResults != null && propRoh_technologicalResults.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_technologicalResults.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedTechnologicalResult roh_technologicalResults = new RelatedTechnologicalResult(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_technologicalResults.Add(roh_technologicalResults);
					}
				}
			}
			this.Roh_nonCompetitiveProjects = new List<RelatedNonCompetitiveProject>();
			SemanticPropertyModel propRoh_nonCompetitiveProjects = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/nonCompetitiveProjects");
			if(propRoh_nonCompetitiveProjects != null && propRoh_nonCompetitiveProjects.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_nonCompetitiveProjects.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedNonCompetitiveProject roh_nonCompetitiveProjects = new RelatedNonCompetitiveProject(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_nonCompetitiveProjects.Add(roh_nonCompetitiveProjects);
					}
				}
			}
			this.Roh_patents = new List<RelatedPatent>();
			SemanticPropertyModel propRoh_patents = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/patents");
			if(propRoh_patents != null && propRoh_patents.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_patents.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedPatent roh_patents = new RelatedPatent(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_patents.Add(roh_patents);
					}
				}
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ScientificExperience"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ScientificExperience"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/supervisedArtisticProjects")]
		public  List<RelatedSupervisedArtisticProject> Roh_supervisedArtisticProjects { get; set;}

		[RDFProperty("http://w3id.org/roh/competitiveProjects")]
		public  List<RelatedCompetitiveProject> Roh_competitiveProjects { get; set;}

		[RDFProperty("http://w3id.org/roh/groups")]
		public  List<RelatedGroup> Roh_groups { get; set;}

		[RDFProperty("http://w3id.org/roh/technologicalResults")]
		public  List<RelatedTechnologicalResult> Roh_technologicalResults { get; set;}

		[RDFProperty("http://w3id.org/roh/nonCompetitiveProjects")]
		public  List<RelatedNonCompetitiveProject> Roh_nonCompetitiveProjects { get; set;}

		[RDFProperty("http://w3id.org/roh/patents")]
		public  List<RelatedPatent> Roh_patents { get; set;}

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
			if(Roh_supervisedArtisticProjects!=null){
				foreach(RelatedSupervisedArtisticProject prop in Roh_supervisedArtisticProjects){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedSupervisedArtisticProject = new OntologyEntity("http://w3id.org/roh/RelatedSupervisedArtisticProject", "http://w3id.org/roh/RelatedSupervisedArtisticProject", "roh:supervisedArtisticProjects", prop.propList, prop.entList);
				entList.Add(entityRelatedSupervisedArtisticProject);
				prop.Entity= entityRelatedSupervisedArtisticProject;
				}
			}
			if(Roh_competitiveProjects!=null){
				foreach(RelatedCompetitiveProject prop in Roh_competitiveProjects){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedCompetitiveProject = new OntologyEntity("http://w3id.org/roh/RelatedCompetitiveProject", "http://w3id.org/roh/RelatedCompetitiveProject", "roh:competitiveProjects", prop.propList, prop.entList);
				entList.Add(entityRelatedCompetitiveProject);
				prop.Entity= entityRelatedCompetitiveProject;
				}
			}
			if(Roh_groups!=null){
				foreach(RelatedGroup prop in Roh_groups){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedGroup = new OntologyEntity("http://w3id.org/roh/RelatedGroup", "http://w3id.org/roh/RelatedGroup", "roh:groups", prop.propList, prop.entList);
				entList.Add(entityRelatedGroup);
				prop.Entity= entityRelatedGroup;
				}
			}
			if(Roh_technologicalResults!=null){
				foreach(RelatedTechnologicalResult prop in Roh_technologicalResults){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedTechnologicalResult = new OntologyEntity("http://w3id.org/roh/RelatedTechnologicalResult", "http://w3id.org/roh/RelatedTechnologicalResult", "roh:technologicalResults", prop.propList, prop.entList);
				entList.Add(entityRelatedTechnologicalResult);
				prop.Entity= entityRelatedTechnologicalResult;
				}
			}
			if(Roh_nonCompetitiveProjects!=null){
				foreach(RelatedNonCompetitiveProject prop in Roh_nonCompetitiveProjects){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedNonCompetitiveProject = new OntologyEntity("http://w3id.org/roh/RelatedNonCompetitiveProject", "http://w3id.org/roh/RelatedNonCompetitiveProject", "roh:nonCompetitiveProjects", prop.propList, prop.entList);
				entList.Add(entityRelatedNonCompetitiveProject);
				prop.Entity= entityRelatedNonCompetitiveProject;
				}
			}
			if(Roh_patents!=null){
				foreach(RelatedPatent prop in Roh_patents){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedPatent = new OntologyEntity("http://w3id.org/roh/RelatedPatent", "http://w3id.org/roh/RelatedPatent", "roh:patents", prop.propList, prop.entList);
				entList.Add(entityRelatedPatent);
				prop.Entity= entityRelatedPatent;
				}
			}
		} 











	}
}
