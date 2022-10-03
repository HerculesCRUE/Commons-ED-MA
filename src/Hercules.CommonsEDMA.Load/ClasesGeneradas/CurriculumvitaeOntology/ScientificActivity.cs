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
	public class ScientificActivity : GnossOCBase
	{

		public ScientificActivity() : base() { } 

		public ScientificActivity(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_otherDistinctions = new List<RelatedOtherDistinction>();
			SemanticPropertyModel propRoh_otherDistinctions = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/otherDistinctions");
			if(propRoh_otherDistinctions != null && propRoh_otherDistinctions.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_otherDistinctions.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedOtherDistinction roh_otherDistinctions = new RelatedOtherDistinction(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_otherDistinctions.Add(roh_otherDistinctions);
					}
				}
			}
			SemanticPropertyModel propRoh_generalQualityIndicators = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/generalQualityIndicators");
			if(propRoh_generalQualityIndicators != null && propRoh_generalQualityIndicators.PropertyValues.Count > 0)
			{
				this.Roh_generalQualityIndicators = new GeneralQualityIndicator(propRoh_generalQualityIndicators.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_worksSubmittedConferences = new List<RelatedWorkSubmittedConferences>();
			SemanticPropertyModel propRoh_worksSubmittedConferences = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/worksSubmittedConferences");
			if(propRoh_worksSubmittedConferences != null && propRoh_worksSubmittedConferences.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_worksSubmittedConferences.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedWorkSubmittedConferences roh_worksSubmittedConferences = new RelatedWorkSubmittedConferences(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_worksSubmittedConferences.Add(roh_worksSubmittedConferences);
					}
				}
			}
			this.Roh_societies = new List<RelatedSociety>();
			SemanticPropertyModel propRoh_societies = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/societies");
			if(propRoh_societies != null && propRoh_societies.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_societies.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedSociety roh_societies = new RelatedSociety(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_societies.Add(roh_societies);
					}
				}
			}
			this.Roh_otherDisseminationActivities = new List<RelatedOtherDisseminationActivity>();
			SemanticPropertyModel propRoh_otherDisseminationActivities = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/otherDisseminationActivities");
			if(propRoh_otherDisseminationActivities != null && propRoh_otherDisseminationActivities.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_otherDisseminationActivities.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedOtherDisseminationActivity roh_otherDisseminationActivities = new RelatedOtherDisseminationActivity(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_otherDisseminationActivities.Add(roh_otherDisseminationActivities);
					}
				}
			}
			this.Roh_councils = new List<RelatedCouncil>();
			SemanticPropertyModel propRoh_councils = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/councils");
			if(propRoh_councils != null && propRoh_councils.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_councils.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedCouncil roh_councils = new RelatedCouncil(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_councils.Add(roh_councils);
					}
				}
			}
			this.Roh_scientificProduction = new List<RelatedScientificProduction>();
			SemanticPropertyModel propRoh_scientificProduction = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificProduction");
			if(propRoh_scientificProduction != null && propRoh_scientificProduction.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_scientificProduction.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedScientificProduction roh_scientificProduction = new RelatedScientificProduction(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_scientificProduction.Add(roh_scientificProduction);
					}
				}
			}
			this.Roh_forums = new List<RelatedForum>();
			SemanticPropertyModel propRoh_forums = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/forums");
			if(propRoh_forums != null && propRoh_forums.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_forums.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedForum roh_forums = new RelatedForum(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_forums.Add(roh_forums);
					}
				}
			}
			this.Roh_researchActivityPeriods = new List<RelatedResearchActivityPeriod>();
			SemanticPropertyModel propRoh_researchActivityPeriods = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchActivityPeriods");
			if(propRoh_researchActivityPeriods != null && propRoh_researchActivityPeriods.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_researchActivityPeriods.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedResearchActivityPeriod roh_researchActivityPeriods = new RelatedResearchActivityPeriod(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_researchActivityPeriods.Add(roh_researchActivityPeriods);
					}
				}
			}
			this.Roh_scientificPublications = new List<RelatedScientificPublication>();
			SemanticPropertyModel propRoh_scientificPublications = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificPublications");
			if(propRoh_scientificPublications != null && propRoh_scientificPublications.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_scientificPublications.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedScientificPublication roh_scientificPublications = new RelatedScientificPublication(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_scientificPublications.Add(roh_scientificPublications);
					}
				}
			}
			this.Roh_otherAchievements = new List<RelatedOtherAchievement>();
			SemanticPropertyModel propRoh_otherAchievements = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/otherAchievements");
			if(propRoh_otherAchievements != null && propRoh_otherAchievements.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_otherAchievements.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedOtherAchievement roh_otherAchievements = new RelatedOtherAchievement(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_otherAchievements.Add(roh_otherAchievements);
					}
				}
			}
			this.Roh_researchEvaluations = new List<RelatedResearchEvaluation>();
			SemanticPropertyModel propRoh_researchEvaluations = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchEvaluations");
			if(propRoh_researchEvaluations != null && propRoh_researchEvaluations.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_researchEvaluations.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedResearchEvaluation roh_researchEvaluations = new RelatedResearchEvaluation(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_researchEvaluations.Add(roh_researchEvaluations);
					}
				}
			}
			this.Roh_activitiesOrganization = new List<RelatedActivityOrganization>();
			SemanticPropertyModel propRoh_activitiesOrganization = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/activitiesOrganization");
			if(propRoh_activitiesOrganization != null && propRoh_activitiesOrganization.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_activitiesOrganization.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedActivityOrganization roh_activitiesOrganization = new RelatedActivityOrganization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_activitiesOrganization.Add(roh_activitiesOrganization);
					}
				}
			}
			this.Roh_otherCollaborations = new List<RelatedOtherCollaboration>();
			SemanticPropertyModel propRoh_otherCollaborations = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/otherCollaborations");
			if(propRoh_otherCollaborations != null && propRoh_otherCollaborations.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_otherCollaborations.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedOtherCollaboration roh_otherCollaborations = new RelatedOtherCollaboration(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_otherCollaborations.Add(roh_otherCollaborations);
					}
				}
			}
			this.Roh_networks = new List<RelatedNetwork>();
			SemanticPropertyModel propRoh_networks = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/networks");
			if(propRoh_networks != null && propRoh_networks.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_networks.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedNetwork roh_networks = new RelatedNetwork(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_networks.Add(roh_networks);
					}
				}
			}
			this.Roh_grants = new List<RelatedGrant>();
			SemanticPropertyModel propRoh_grants = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/grants");
			if(propRoh_grants != null && propRoh_grants.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_grants.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedGrant roh_grants = new RelatedGrant(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_grants.Add(roh_grants);
					}
				}
			}
			this.Roh_activitiesManagement = new List<RelatedActivityManagement>();
			SemanticPropertyModel propRoh_activitiesManagement = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/activitiesManagement");
			if(propRoh_activitiesManagement != null && propRoh_activitiesManagement.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_activitiesManagement.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedActivityManagement roh_activitiesManagement = new RelatedActivityManagement(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_activitiesManagement.Add(roh_activitiesManagement);
					}
				}
			}
			this.Roh_prizes = new List<RelatedPrize>();
			SemanticPropertyModel propRoh_prizes = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/prizes");
			if(propRoh_prizes != null && propRoh_prizes.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_prizes.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedPrize roh_prizes = new RelatedPrize(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_prizes.Add(roh_prizes);
					}
				}
			}
			this.Roh_committees = new List<RelatedCommittee>();
			SemanticPropertyModel propRoh_committees = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/committees");
			if(propRoh_committees != null && propRoh_committees.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_committees.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedCommittee roh_committees = new RelatedCommittee(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_committees.Add(roh_committees);
					}
				}
			}
			this.Roh_stays = new List<RelatedStay>();
			SemanticPropertyModel propRoh_stays = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/stays");
			if(propRoh_stays != null && propRoh_stays.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_stays.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedStay roh_stays = new RelatedStay(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_stays.Add(roh_stays);
					}
				}
			}
			this.Roh_obtainedRecognitions = new List<RelatedObtainedRecognition>();
			SemanticPropertyModel propRoh_obtainedRecognitions = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/obtainedRecognitions");
			if(propRoh_obtainedRecognitions != null && propRoh_obtainedRecognitions.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_obtainedRecognitions.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedObtainedRecognition roh_obtainedRecognitions = new RelatedObtainedRecognition(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_obtainedRecognitions.Add(roh_obtainedRecognitions);
					}
				}
			}
			this.Roh_worksSubmittedSeminars = new List<RelatedWorkSubmittedSeminars>();
			SemanticPropertyModel propRoh_worksSubmittedSeminars = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/worksSubmittedSeminars");
			if(propRoh_worksSubmittedSeminars != null && propRoh_worksSubmittedSeminars.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_worksSubmittedSeminars.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedWorkSubmittedSeminars roh_worksSubmittedSeminars = new RelatedWorkSubmittedSeminars(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_worksSubmittedSeminars.Add(roh_worksSubmittedSeminars);
					}
				}
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ScientificActivity"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ScientificActivity"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/otherDistinctions")]
		public  List<RelatedOtherDistinction> Roh_otherDistinctions { get; set;}

		[RDFProperty("http://w3id.org/roh/generalQualityIndicators")]
		public  GeneralQualityIndicator Roh_generalQualityIndicators { get; set;}

		[RDFProperty("http://w3id.org/roh/worksSubmittedConferences")]
		public  List<RelatedWorkSubmittedConferences> Roh_worksSubmittedConferences { get; set;}

		[RDFProperty("http://w3id.org/roh/societies")]
		public  List<RelatedSociety> Roh_societies { get; set;}

		[RDFProperty("http://w3id.org/roh/otherDisseminationActivities")]
		public  List<RelatedOtherDisseminationActivity> Roh_otherDisseminationActivities { get; set;}

		[RDFProperty("http://w3id.org/roh/councils")]
		public  List<RelatedCouncil> Roh_councils { get; set;}

		[RDFProperty("http://w3id.org/roh/scientificProduction")]
		public  List<RelatedScientificProduction> Roh_scientificProduction { get; set;}

		[RDFProperty("http://w3id.org/roh/forums")]
		public  List<RelatedForum> Roh_forums { get; set;}

		[RDFProperty("http://w3id.org/roh/researchActivityPeriods")]
		public  List<RelatedResearchActivityPeriod> Roh_researchActivityPeriods { get; set;}

		[RDFProperty("http://w3id.org/roh/scientificPublications")]
		public  List<RelatedScientificPublication> Roh_scientificPublications { get; set;}

		[RDFProperty("http://w3id.org/roh/otherAchievements")]
		public  List<RelatedOtherAchievement> Roh_otherAchievements { get; set;}

		[RDFProperty("http://w3id.org/roh/researchEvaluations")]
		public  List<RelatedResearchEvaluation> Roh_researchEvaluations { get; set;}

		[RDFProperty("http://w3id.org/roh/activitiesOrganization")]
		public  List<RelatedActivityOrganization> Roh_activitiesOrganization { get; set;}

		[RDFProperty("http://w3id.org/roh/otherCollaborations")]
		public  List<RelatedOtherCollaboration> Roh_otherCollaborations { get; set;}

		[RDFProperty("http://w3id.org/roh/networks")]
		public  List<RelatedNetwork> Roh_networks { get; set;}

		[RDFProperty("http://w3id.org/roh/grants")]
		public  List<RelatedGrant> Roh_grants { get; set;}

		[RDFProperty("http://w3id.org/roh/activitiesManagement")]
		public  List<RelatedActivityManagement> Roh_activitiesManagement { get; set;}

		[RDFProperty("http://w3id.org/roh/prizes")]
		public  List<RelatedPrize> Roh_prizes { get; set;}

		[RDFProperty("http://w3id.org/roh/committees")]
		public  List<RelatedCommittee> Roh_committees { get; set;}

		[RDFProperty("http://w3id.org/roh/stays")]
		public  List<RelatedStay> Roh_stays { get; set;}

		[RDFProperty("http://w3id.org/roh/obtainedRecognitions")]
		public  List<RelatedObtainedRecognition> Roh_obtainedRecognitions { get; set;}

		[RDFProperty("http://w3id.org/roh/worksSubmittedSeminars")]
		public  List<RelatedWorkSubmittedSeminars> Roh_worksSubmittedSeminars { get; set;}

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
			if(Roh_otherDistinctions!=null){
				foreach(RelatedOtherDistinction prop in Roh_otherDistinctions){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedOtherDistinction = new OntologyEntity("http://w3id.org/roh/RelatedOtherDistinction", "http://w3id.org/roh/RelatedOtherDistinction", "roh:otherDistinctions", prop.propList, prop.entList);
				entList.Add(entityRelatedOtherDistinction);
				prop.Entity= entityRelatedOtherDistinction;
				}
			}
			if(Roh_generalQualityIndicators!=null){
				Roh_generalQualityIndicators.GetProperties();
				Roh_generalQualityIndicators.GetEntities();
				OntologyEntity entityRoh_generalQualityIndicators = new OntologyEntity("http://w3id.org/roh/GeneralQualityIndicator", "http://w3id.org/roh/GeneralQualityIndicator", "roh:generalQualityIndicators", Roh_generalQualityIndicators.propList, Roh_generalQualityIndicators.entList);
				entList.Add(entityRoh_generalQualityIndicators);
			}
			if(Roh_worksSubmittedConferences!=null){
				foreach(RelatedWorkSubmittedConferences prop in Roh_worksSubmittedConferences){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedWorkSubmittedConferences = new OntologyEntity("http://w3id.org/roh/RelatedWorkSubmittedConferences", "http://w3id.org/roh/RelatedWorkSubmittedConferences", "roh:worksSubmittedConferences", prop.propList, prop.entList);
				entList.Add(entityRelatedWorkSubmittedConferences);
				prop.Entity= entityRelatedWorkSubmittedConferences;
				}
			}
			if(Roh_societies!=null){
				foreach(RelatedSociety prop in Roh_societies){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedSociety = new OntologyEntity("http://w3id.org/roh/RelatedSociety", "http://w3id.org/roh/RelatedSociety", "roh:societies", prop.propList, prop.entList);
				entList.Add(entityRelatedSociety);
				prop.Entity= entityRelatedSociety;
				}
			}
			if(Roh_otherDisseminationActivities!=null){
				foreach(RelatedOtherDisseminationActivity prop in Roh_otherDisseminationActivities){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedOtherDisseminationActivity = new OntologyEntity("http://w3id.org/roh/RelatedOtherDisseminationActivity", "http://w3id.org/roh/RelatedOtherDisseminationActivity", "roh:otherDisseminationActivities", prop.propList, prop.entList);
				entList.Add(entityRelatedOtherDisseminationActivity);
				prop.Entity= entityRelatedOtherDisseminationActivity;
				}
			}
			if(Roh_councils!=null){
				foreach(RelatedCouncil prop in Roh_councils){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedCouncil = new OntologyEntity("http://w3id.org/roh/RelatedCouncil", "http://w3id.org/roh/RelatedCouncil", "roh:councils", prop.propList, prop.entList);
				entList.Add(entityRelatedCouncil);
				prop.Entity= entityRelatedCouncil;
				}
			}
			if(Roh_scientificProduction!=null){
				foreach(RelatedScientificProduction prop in Roh_scientificProduction){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedScientificProduction = new OntologyEntity("http://w3id.org/roh/RelatedScientificProduction", "http://w3id.org/roh/RelatedScientificProduction", "roh:scientificProduction", prop.propList, prop.entList);
				entList.Add(entityRelatedScientificProduction);
				prop.Entity= entityRelatedScientificProduction;
				}
			}
			if(Roh_forums!=null){
				foreach(RelatedForum prop in Roh_forums){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedForum = new OntologyEntity("http://w3id.org/roh/RelatedForum", "http://w3id.org/roh/RelatedForum", "roh:forums", prop.propList, prop.entList);
				entList.Add(entityRelatedForum);
				prop.Entity= entityRelatedForum;
				}
			}
			if(Roh_researchActivityPeriods!=null){
				foreach(RelatedResearchActivityPeriod prop in Roh_researchActivityPeriods){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedResearchActivityPeriod = new OntologyEntity("http://w3id.org/roh/RelatedResearchActivityPeriod", "http://w3id.org/roh/RelatedResearchActivityPeriod", "roh:researchActivityPeriods", prop.propList, prop.entList);
				entList.Add(entityRelatedResearchActivityPeriod);
				prop.Entity= entityRelatedResearchActivityPeriod;
				}
			}
			if(Roh_scientificPublications!=null){
				foreach(RelatedScientificPublication prop in Roh_scientificPublications){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedScientificPublication = new OntologyEntity("http://w3id.org/roh/RelatedScientificPublication", "http://w3id.org/roh/RelatedScientificPublication", "roh:scientificPublications", prop.propList, prop.entList);
				entList.Add(entityRelatedScientificPublication);
				prop.Entity= entityRelatedScientificPublication;
				}
			}
			if(Roh_otherAchievements!=null){
				foreach(RelatedOtherAchievement prop in Roh_otherAchievements){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedOtherAchievement = new OntologyEntity("http://w3id.org/roh/RelatedOtherAchievement", "http://w3id.org/roh/RelatedOtherAchievement", "roh:otherAchievements", prop.propList, prop.entList);
				entList.Add(entityRelatedOtherAchievement);
				prop.Entity= entityRelatedOtherAchievement;
				}
			}
			if(Roh_researchEvaluations!=null){
				foreach(RelatedResearchEvaluation prop in Roh_researchEvaluations){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedResearchEvaluation = new OntologyEntity("http://w3id.org/roh/RelatedResearchEvaluation", "http://w3id.org/roh/RelatedResearchEvaluation", "roh:researchEvaluations", prop.propList, prop.entList);
				entList.Add(entityRelatedResearchEvaluation);
				prop.Entity= entityRelatedResearchEvaluation;
				}
			}
			if(Roh_activitiesOrganization!=null){
				foreach(RelatedActivityOrganization prop in Roh_activitiesOrganization){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedActivityOrganization = new OntologyEntity("http://w3id.org/roh/RelatedActivityOrganization", "http://w3id.org/roh/RelatedActivityOrganization", "roh:activitiesOrganization", prop.propList, prop.entList);
				entList.Add(entityRelatedActivityOrganization);
				prop.Entity= entityRelatedActivityOrganization;
				}
			}
			if(Roh_otherCollaborations!=null){
				foreach(RelatedOtherCollaboration prop in Roh_otherCollaborations){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedOtherCollaboration = new OntologyEntity("http://w3id.org/roh/RelatedOtherCollaboration", "http://w3id.org/roh/RelatedOtherCollaboration", "roh:otherCollaborations", prop.propList, prop.entList);
				entList.Add(entityRelatedOtherCollaboration);
				prop.Entity= entityRelatedOtherCollaboration;
				}
			}
			if(Roh_networks!=null){
				foreach(RelatedNetwork prop in Roh_networks){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedNetwork = new OntologyEntity("http://w3id.org/roh/RelatedNetwork", "http://w3id.org/roh/RelatedNetwork", "roh:networks", prop.propList, prop.entList);
				entList.Add(entityRelatedNetwork);
				prop.Entity= entityRelatedNetwork;
				}
			}
			if(Roh_grants!=null){
				foreach(RelatedGrant prop in Roh_grants){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedGrant = new OntologyEntity("http://w3id.org/roh/RelatedGrant", "http://w3id.org/roh/RelatedGrant", "roh:grants", prop.propList, prop.entList);
				entList.Add(entityRelatedGrant);
				prop.Entity= entityRelatedGrant;
				}
			}
			if(Roh_activitiesManagement!=null){
				foreach(RelatedActivityManagement prop in Roh_activitiesManagement){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedActivityManagement = new OntologyEntity("http://w3id.org/roh/RelatedActivityManagement", "http://w3id.org/roh/RelatedActivityManagement", "roh:activitiesManagement", prop.propList, prop.entList);
				entList.Add(entityRelatedActivityManagement);
				prop.Entity= entityRelatedActivityManagement;
				}
			}
			if(Roh_prizes!=null){
				foreach(RelatedPrize prop in Roh_prizes){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedPrize = new OntologyEntity("http://w3id.org/roh/RelatedPrize", "http://w3id.org/roh/RelatedPrize", "roh:prizes", prop.propList, prop.entList);
				entList.Add(entityRelatedPrize);
				prop.Entity= entityRelatedPrize;
				}
			}
			if(Roh_committees!=null){
				foreach(RelatedCommittee prop in Roh_committees){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedCommittee = new OntologyEntity("http://w3id.org/roh/RelatedCommittee", "http://w3id.org/roh/RelatedCommittee", "roh:committees", prop.propList, prop.entList);
				entList.Add(entityRelatedCommittee);
				prop.Entity= entityRelatedCommittee;
				}
			}
			if(Roh_stays!=null){
				foreach(RelatedStay prop in Roh_stays){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedStay = new OntologyEntity("http://w3id.org/roh/RelatedStay", "http://w3id.org/roh/RelatedStay", "roh:stays", prop.propList, prop.entList);
				entList.Add(entityRelatedStay);
				prop.Entity= entityRelatedStay;
				}
			}
			if(Roh_obtainedRecognitions!=null){
				foreach(RelatedObtainedRecognition prop in Roh_obtainedRecognitions){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedObtainedRecognition = new OntologyEntity("http://w3id.org/roh/RelatedObtainedRecognition", "http://w3id.org/roh/RelatedObtainedRecognition", "roh:obtainedRecognitions", prop.propList, prop.entList);
				entList.Add(entityRelatedObtainedRecognition);
				prop.Entity= entityRelatedObtainedRecognition;
				}
			}
			if(Roh_worksSubmittedSeminars!=null){
				foreach(RelatedWorkSubmittedSeminars prop in Roh_worksSubmittedSeminars){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedWorkSubmittedSeminars = new OntologyEntity("http://w3id.org/roh/RelatedWorkSubmittedSeminars", "http://w3id.org/roh/RelatedWorkSubmittedSeminars", "roh:worksSubmittedSeminars", prop.propList, prop.entList);
				entList.Add(entityRelatedWorkSubmittedSeminars);
				prop.Entity= entityRelatedWorkSubmittedSeminars;
				}
			}
		} 











	}
}
