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
using EventType = EventtypeOntology.EventType;
using Feature = FeatureOntology.Feature;
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using TargetGroupProfile = TargetgroupprofileOntology.TargetGroupProfile;
using AccessSystemActivity = AccesssystemactivityOntology.AccessSystemActivity;
using GeographicRegion = GeographicregionOntology.GeographicRegion;
using EventInscriptionType = EventinscriptiontypeOntology.EventInscriptionType;
using ActivityModality = ActivitymodalityOntology.ActivityModality;
using PublicationType = PublicationtypeOntology.PublicationType;
using ManagementTypeActivity = ManagementtypeactivityOntology.ManagementTypeActivity;
using ParticipationTypeActivity = ParticipationtypeactivityOntology.ParticipationTypeActivity;
using Person = PersonOntology.Person;

namespace ActivityOntology
{
	[ExcludeFromCodeCoverage]
	public class Activity : GnossOCBase
	{

		public Activity() : base() { } 

		public Activity(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_promotedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedBy");
			if(propRoh_promotedBy != null && propRoh_promotedBy.PropertyValues.Count > 0)
			{
				this.Roh_promotedBy = new Organization(propRoh_promotedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_eventType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventType");
			if(propRoh_eventType != null && propRoh_eventType.PropertyValues.Count > 0)
			{
				this.Roh_eventType = new EventType(propRoh_eventType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_representedEntityHasRegion = new List<Feature>();
			SemanticPropertyModel propRoh_representedEntityHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityHasRegion");
			if(propRoh_representedEntityHasRegion != null && propRoh_representedEntityHasRegion.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_representedEntityHasRegion.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Feature roh_representedEntityHasRegion = new Feature(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_representedEntityHasRegion.Add(roh_representedEntityHasRegion);
					}
				}
			}
			SemanticPropertyModel propRoh_conductedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByType");
			if(propRoh_conductedByType != null && propRoh_conductedByType.PropertyValues.Count > 0)
			{
				this.Roh_conductedByType = new OrganizationType(propRoh_conductedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_targetGroupProfile = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/targetGroupProfile");
			if(propRoh_targetGroupProfile != null && propRoh_targetGroupProfile.PropertyValues.Count > 0)
			{
				this.Roh_targetGroupProfile = new TargetGroupProfile(propRoh_targetGroupProfile.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_authorList = new List<BFO_0000023>();
			SemanticPropertyModel propBibo_authorList = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/authorList");
			if(propBibo_authorList != null && propBibo_authorList.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_authorList.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						BFO_0000023 bibo_authorList = new BFO_0000023(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_authorList.Add(bibo_authorList);
					}
				}
			}
			SemanticPropertyModel propRoh_promotedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByType");
			if(propRoh_promotedByType != null && propRoh_promotedByType.PropertyValues.Count > 0)
			{
				this.Roh_promotedByType = new OrganizationType(propRoh_promotedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByHasCountryName");
			if(propRoh_promotedByHasCountryName != null && propRoh_promotedByHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_promotedByHasCountryName = new Feature(propRoh_promotedByHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByHasRegion");
			if(propRoh_promotedByHasRegion != null && propRoh_promotedByHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_promotedByHasRegion = new Feature(propRoh_promotedByHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_accessSystemActivity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/accessSystemActivity");
			if(propRoh_accessSystemActivity != null && propRoh_accessSystemActivity.PropertyValues.Count > 0)
			{
				this.Roh_accessSystemActivity = new AccessSystemActivity(propRoh_accessSystemActivity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_publicationHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationHasRegion");
			if(propRoh_publicationHasRegion != null && propRoh_publicationHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_publicationHasRegion = new Feature(propRoh_publicationHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new GeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_eventInscriptionType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventInscriptionType");
			if(propRoh_eventInscriptionType != null && propRoh_eventInscriptionType.PropertyValues.Count > 0)
			{
				this.Roh_eventInscriptionType = new EventInscriptionType(propRoh_eventInscriptionType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_activityModality = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/activityModality");
			if(propRoh_activityModality != null && propRoh_activityModality.PropertyValues.Count > 0)
			{
				this.Roh_activityModality = new ActivityModality(propRoh_activityModality.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_representedEntityType = new List<OrganizationType>();
			SemanticPropertyModel propRoh_representedEntityType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityType");
			if(propRoh_representedEntityType != null && propRoh_representedEntityType.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_representedEntityType.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						OrganizationType roh_representedEntityType = new OrganizationType(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_representedEntityType.Add(roh_representedEntityType);
					}
				}
			}
			SemanticPropertyModel propRoh_publicationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationType");
			if(propRoh_publicationType != null && propRoh_publicationType.PropertyValues.Count > 0)
			{
				this.Roh_publicationType = new PublicationType(propRoh_publicationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_managementType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/managementType");
			if(propRoh_managementType != null && propRoh_managementType.PropertyValues.Count > 0)
			{
				this.Roh_managementType = new ManagementTypeActivity(propRoh_managementType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_identifier = new List<Document>();
			SemanticPropertyModel propBibo_identifier = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/identifier");
			if(propBibo_identifier != null && propBibo_identifier.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_identifier.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document bibo_identifier = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_identifier.Add(bibo_identifier);
					}
				}
			}
			SemanticPropertyModel propRoh_participationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationType");
			if(propRoh_participationType != null && propRoh_participationType.PropertyValues.Count > 0)
			{
				this.Roh_participationType = new ParticipationTypeActivity(propRoh_participationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_conductedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedBy");
			if(propRoh_conductedBy != null && propRoh_conductedBy.PropertyValues.Count > 0)
			{
				this.Roh_conductedBy = new Organization(propRoh_conductedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_representedEntityHasCountryName = new List<Feature>();
			SemanticPropertyModel propRoh_representedEntityHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityHasCountryName");
			if(propRoh_representedEntityHasCountryName != null && propRoh_representedEntityHasCountryName.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_representedEntityHasCountryName.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Feature roh_representedEntityHasCountryName = new Feature(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_representedEntityHasCountryName.Add(roh_representedEntityHasCountryName);
					}
				}
			}
			SemanticPropertyModel propRoh_publicationHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationHasCountryName");
			if(propRoh_publicationHasCountryName != null && propRoh_publicationHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_publicationHasCountryName = new Feature(propRoh_publicationHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_hasKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_hasKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasKnowledgeArea");
			if(propRoh_hasKnowledgeArea != null && propRoh_hasKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_hasKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_hasKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_hasKnowledgeArea.Add(roh_hasKnowledgeArea);
					}
				}
			}
			this.Roh_representedEntity = new List<Organization>();
			SemanticPropertyModel propRoh_representedEntity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntity");
			if(propRoh_representedEntity != null && propRoh_representedEntity.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_representedEntity.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_representedEntity = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_representedEntity.Add(roh_representedEntity);
					}
				}
			}
			this.Roh_promotedByLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByLocality"));
			this.Roh_isbn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isbn"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_frequency = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/frequency"));
			this.Roh_conductedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTitle"));
			this.Roh_concreteFunctions = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/concreteFunctions"));
			this.Roh_eventTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventTypeOther"));
			this.Roh_correspondingAuthor= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/correspondingAuthor"));
			this.Dc_type = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/type"));
			this.Roh_publicationName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationName"));
			this.Roh_professionalCategory = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/professionalCategory"));
			this.Roh_participationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationTypeOther"));
			this.Bibo_issue = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issue"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_representedEntityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityTitle"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_attendants = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/attendants"));
			this.Vcard_url = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#url"));
			this.Roh_activityModalityOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/activityModalityOther"));
			this.Bibo_publisher = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/publisher"));
			this.Roh_legalDeposit = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/legalDeposit"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_accessSystemActivityOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/accessSystemActivityOther"));
			this.Roh_goals = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/goals"));
			this.Roh_publicationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationTitle"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_eventInscriptionTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventInscriptionTypeOther"));
			this.Roh_representedEntityLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityLocality"));
			this.Roh_promotedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTitle"));
			this.Bibo_presentedAt = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/presentedAt"));
			this.Roh_personNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/personNumber"));
			this.Bibo_pmid = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pmid"));
			this.Roh_withExternalAdmissionsCommittee= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/withExternalAdmissionsCommittee"));
			this.Roh_managementTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/managementTypeOther"));
			this.Bibo_handle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/handle"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_conductedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTypeOther"));
			this.Roh_averageAnnualBudget = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/averageAnnualBudget"));
			this.Roh_promotedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTypeOther"));
			this.Bibo_doi = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/doi"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Bibo_volume = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/volume"));
			this.Bibo_issn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issn"));
			this.Bibo_pageStart = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pageStart"));
			this.Roh_representedEntityTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityTypeOther"));
			this.Roh_congressProceedingsPublication= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/congressProceedingsPublication"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Bibo_pageEnd = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pageEnd"));
			this.Roh_functions = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/functions"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_classificationCVN = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/classificationCVN"));
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public Activity(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_promotedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedBy");
			if(propRoh_promotedBy != null && propRoh_promotedBy.PropertyValues.Count > 0)
			{
				this.Roh_promotedBy = new Organization(propRoh_promotedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_eventType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventType");
			if(propRoh_eventType != null && propRoh_eventType.PropertyValues.Count > 0)
			{
				this.Roh_eventType = new EventType(propRoh_eventType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_representedEntityHasRegion = new List<Feature>();
			SemanticPropertyModel propRoh_representedEntityHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityHasRegion");
			if(propRoh_representedEntityHasRegion != null && propRoh_representedEntityHasRegion.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_representedEntityHasRegion.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Feature roh_representedEntityHasRegion = new Feature(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_representedEntityHasRegion.Add(roh_representedEntityHasRegion);
					}
				}
			}
			SemanticPropertyModel propRoh_conductedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByType");
			if(propRoh_conductedByType != null && propRoh_conductedByType.PropertyValues.Count > 0)
			{
				this.Roh_conductedByType = new OrganizationType(propRoh_conductedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_targetGroupProfile = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/targetGroupProfile");
			if(propRoh_targetGroupProfile != null && propRoh_targetGroupProfile.PropertyValues.Count > 0)
			{
				this.Roh_targetGroupProfile = new TargetGroupProfile(propRoh_targetGroupProfile.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_authorList = new List<BFO_0000023>();
			SemanticPropertyModel propBibo_authorList = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/authorList");
			if(propBibo_authorList != null && propBibo_authorList.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_authorList.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						BFO_0000023 bibo_authorList = new BFO_0000023(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_authorList.Add(bibo_authorList);
					}
				}
			}
			SemanticPropertyModel propRoh_promotedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByType");
			if(propRoh_promotedByType != null && propRoh_promotedByType.PropertyValues.Count > 0)
			{
				this.Roh_promotedByType = new OrganizationType(propRoh_promotedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByHasCountryName");
			if(propRoh_promotedByHasCountryName != null && propRoh_promotedByHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_promotedByHasCountryName = new Feature(propRoh_promotedByHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByHasRegion");
			if(propRoh_promotedByHasRegion != null && propRoh_promotedByHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_promotedByHasRegion = new Feature(propRoh_promotedByHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_accessSystemActivity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/accessSystemActivity");
			if(propRoh_accessSystemActivity != null && propRoh_accessSystemActivity.PropertyValues.Count > 0)
			{
				this.Roh_accessSystemActivity = new AccessSystemActivity(propRoh_accessSystemActivity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_publicationHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationHasRegion");
			if(propRoh_publicationHasRegion != null && propRoh_publicationHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_publicationHasRegion = new Feature(propRoh_publicationHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new GeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_eventInscriptionType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventInscriptionType");
			if(propRoh_eventInscriptionType != null && propRoh_eventInscriptionType.PropertyValues.Count > 0)
			{
				this.Roh_eventInscriptionType = new EventInscriptionType(propRoh_eventInscriptionType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_activityModality = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/activityModality");
			if(propRoh_activityModality != null && propRoh_activityModality.PropertyValues.Count > 0)
			{
				this.Roh_activityModality = new ActivityModality(propRoh_activityModality.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_representedEntityType = new List<OrganizationType>();
			SemanticPropertyModel propRoh_representedEntityType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityType");
			if(propRoh_representedEntityType != null && propRoh_representedEntityType.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_representedEntityType.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						OrganizationType roh_representedEntityType = new OrganizationType(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_representedEntityType.Add(roh_representedEntityType);
					}
				}
			}
			SemanticPropertyModel propRoh_publicationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationType");
			if(propRoh_publicationType != null && propRoh_publicationType.PropertyValues.Count > 0)
			{
				this.Roh_publicationType = new PublicationType(propRoh_publicationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_managementType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/managementType");
			if(propRoh_managementType != null && propRoh_managementType.PropertyValues.Count > 0)
			{
				this.Roh_managementType = new ManagementTypeActivity(propRoh_managementType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_identifier = new List<Document>();
			SemanticPropertyModel propBibo_identifier = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/identifier");
			if(propBibo_identifier != null && propBibo_identifier.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_identifier.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document bibo_identifier = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_identifier.Add(bibo_identifier);
					}
				}
			}
			SemanticPropertyModel propRoh_participationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationType");
			if(propRoh_participationType != null && propRoh_participationType.PropertyValues.Count > 0)
			{
				this.Roh_participationType = new ParticipationTypeActivity(propRoh_participationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_conductedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedBy");
			if(propRoh_conductedBy != null && propRoh_conductedBy.PropertyValues.Count > 0)
			{
				this.Roh_conductedBy = new Organization(propRoh_conductedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_representedEntityHasCountryName = new List<Feature>();
			SemanticPropertyModel propRoh_representedEntityHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityHasCountryName");
			if(propRoh_representedEntityHasCountryName != null && propRoh_representedEntityHasCountryName.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_representedEntityHasCountryName.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Feature roh_representedEntityHasCountryName = new Feature(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_representedEntityHasCountryName.Add(roh_representedEntityHasCountryName);
					}
				}
			}
			SemanticPropertyModel propRoh_publicationHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationHasCountryName");
			if(propRoh_publicationHasCountryName != null && propRoh_publicationHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_publicationHasCountryName = new Feature(propRoh_publicationHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_hasKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_hasKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasKnowledgeArea");
			if(propRoh_hasKnowledgeArea != null && propRoh_hasKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_hasKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_hasKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_hasKnowledgeArea.Add(roh_hasKnowledgeArea);
					}
				}
			}
			this.Roh_representedEntity = new List<Organization>();
			SemanticPropertyModel propRoh_representedEntity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntity");
			if(propRoh_representedEntity != null && propRoh_representedEntity.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_representedEntity.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_representedEntity = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_representedEntity.Add(roh_representedEntity);
					}
				}
			}
			this.Roh_promotedByLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByLocality"));
			this.Roh_isbn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isbn"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_frequency = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/frequency"));
			this.Roh_conductedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTitle"));
			this.Roh_concreteFunctions = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/concreteFunctions"));
			this.Roh_eventTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventTypeOther"));
			this.Roh_correspondingAuthor= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/correspondingAuthor"));
			this.Dc_type = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/type"));
			this.Roh_publicationName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationName"));
			this.Roh_professionalCategory = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/professionalCategory"));
			this.Roh_participationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationTypeOther"));
			this.Bibo_issue = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issue"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_representedEntityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityTitle"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Roh_attendants = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/attendants"));
			this.Vcard_url = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#url"));
			this.Roh_activityModalityOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/activityModalityOther"));
			this.Bibo_publisher = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/publisher"));
			this.Roh_legalDeposit = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/legalDeposit"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_accessSystemActivityOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/accessSystemActivityOther"));
			this.Roh_goals = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/goals"));
			this.Roh_publicationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationTitle"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_eventInscriptionTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/eventInscriptionTypeOther"));
			this.Roh_representedEntityLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityLocality"));
			this.Roh_promotedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTitle"));
			this.Bibo_presentedAt = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/presentedAt"));
			this.Roh_personNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/personNumber"));
			this.Bibo_pmid = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pmid"));
			this.Roh_withExternalAdmissionsCommittee= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/withExternalAdmissionsCommittee"));
			this.Roh_managementTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/managementTypeOther"));
			this.Bibo_handle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/handle"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_conductedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTypeOther"));
			this.Roh_averageAnnualBudget = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/averageAnnualBudget"));
			this.Roh_promotedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTypeOther"));
			this.Bibo_doi = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/doi"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Bibo_volume = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/volume"));
			this.Bibo_issn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issn"));
			this.Bibo_pageStart = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pageStart"));
			this.Roh_representedEntityTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/representedEntityTypeOther"));
			this.Roh_congressProceedingsPublication= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/congressProceedingsPublication"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Bibo_pageEnd = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pageEnd"));
			this.Roh_functions = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/functions"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_classificationCVN = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/classificationCVN"));
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/Activity"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/Activity"; } }
		[LABEL(LanguageEnum.es,"http://w3id.org/roh/promotedBy")]
		[RDFProperty("http://w3id.org/roh/promotedBy")]
		public  Organization Roh_promotedBy  { get; set;} 
		public string IdRoh_promotedBy  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/eventType")]
		[RDFProperty("http://w3id.org/roh/eventType")]
		public  EventType Roh_eventType  { get; set;} 
		public string IdRoh_eventType  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/representedEntityHasRegion")]
		[RDFProperty("http://w3id.org/roh/representedEntityHasRegion")]
		public  List<Feature> Roh_representedEntityHasRegion { get; set;}
		public List<string> IdsRoh_representedEntityHasRegion { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/conductedByType")]
		[RDFProperty("http://w3id.org/roh/conductedByType")]
		public  OrganizationType Roh_conductedByType  { get; set;} 
		public string IdRoh_conductedByType  { get; set;} 

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#hasCountryName")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/targetGroupProfile")]
		[RDFProperty("http://w3id.org/roh/targetGroupProfile")]
		public  TargetGroupProfile Roh_targetGroupProfile  { get; set;} 
		public string IdRoh_targetGroupProfile  { get; set;} 

		[RDFProperty("http://purl.org/ontology/bibo/authorList")]
		public  List<BFO_0000023> Bibo_authorList { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/promotedByType")]
		[RDFProperty("http://w3id.org/roh/promotedByType")]
		public  OrganizationType Roh_promotedByType  { get; set;} 
		public string IdRoh_promotedByType  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/promotedByHasCountryName")]
		[RDFProperty("http://w3id.org/roh/promotedByHasCountryName")]
		public  Feature Roh_promotedByHasCountryName  { get; set;} 
		public string IdRoh_promotedByHasCountryName  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/promotedByHasRegion")]
		[RDFProperty("http://w3id.org/roh/promotedByHasRegion")]
		public  Feature Roh_promotedByHasRegion  { get; set;} 
		public string IdRoh_promotedByHasRegion  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/accessSystemActivity")]
		[RDFProperty("http://w3id.org/roh/accessSystemActivity")]
		public  AccessSystemActivity Roh_accessSystemActivity  { get; set;} 
		public string IdRoh_accessSystemActivity  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/publicationHasRegion")]
		[RDFProperty("http://w3id.org/roh/publicationHasRegion")]
		public  Feature Roh_publicationHasRegion  { get; set;} 
		public string IdRoh_publicationHasRegion  { get; set;} 

		[LABEL(LanguageEnum.es,"http://vivoweb.org/ontology/core#geographicFocus")]
		[RDFProperty("http://vivoweb.org/ontology/core#geographicFocus")]
		public  GeographicRegion Vivo_geographicFocus  { get; set;} 
		public string IdVivo_geographicFocus  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/eventInscriptionType")]
		[RDFProperty("http://w3id.org/roh/eventInscriptionType")]
		public  EventInscriptionType Roh_eventInscriptionType  { get; set;} 
		public string IdRoh_eventInscriptionType  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/activityModality")]
		[RDFProperty("http://w3id.org/roh/activityModality")]
		public  ActivityModality Roh_activityModality  { get; set;} 
		public string IdRoh_activityModality  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/representedEntityType")]
		[RDFProperty("http://w3id.org/roh/representedEntityType")]
		public  List<OrganizationType> Roh_representedEntityType { get; set;}
		public List<string> IdsRoh_representedEntityType { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/publicationType")]
		[RDFProperty("http://w3id.org/roh/publicationType")]
		public  PublicationType Roh_publicationType  { get; set;} 
		public string IdRoh_publicationType  { get; set;} 

		[RDFProperty("http://vivoweb.org/ontology/core#freeTextKeyword")]
		public  List<CategoryPath> Vivo_freeTextKeyword { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/managementType")]
		[RDFProperty("http://w3id.org/roh/managementType")]
		public  ManagementTypeActivity Roh_managementType  { get; set;} 
		public string IdRoh_managementType  { get; set;} 

		[RDFProperty("http://purl.org/ontology/bibo/identifier")]
		public  List<Document> Bibo_identifier { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/participationType")]
		[RDFProperty("http://w3id.org/roh/participationType")]
		public  ParticipationTypeActivity Roh_participationType  { get; set;} 
		public string IdRoh_participationType  { get; set;} 

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#hasRegion")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/conductedBy")]
		[RDFProperty("http://w3id.org/roh/conductedBy")]
		public  Organization Roh_conductedBy  { get; set;} 
		public string IdRoh_conductedBy  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/representedEntityHasCountryName")]
		[RDFProperty("http://w3id.org/roh/representedEntityHasCountryName")]
		public  List<Feature> Roh_representedEntityHasCountryName { get; set;}
		public List<string> IdsRoh_representedEntityHasCountryName { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/publicationHasCountryName")]
		[RDFProperty("http://w3id.org/roh/publicationHasCountryName")]
		public  Feature Roh_publicationHasCountryName  { get; set;} 
		public string IdRoh_publicationHasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/hasKnowledgeArea")]
		public  List<CategoryPath> Roh_hasKnowledgeArea { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/representedEntity")]
		[RDFProperty("http://w3id.org/roh/representedEntity")]
		public  List<Organization> Roh_representedEntity { get; set;}
		public List<string> IdsRoh_representedEntity { get; set;}

		[RDFProperty("http://w3id.org/roh/promotedByLocality")]
		public  string Roh_promotedByLocality { get; set;}

		[RDFProperty("http://w3id.org/roh/isbn")]
		public  string Roh_isbn { get; set;}

		[RDFProperty("http://w3id.org/roh/durationDays")]
		public  string Roh_durationDays { get; set;}

		[RDFProperty("http://w3id.org/roh/frequency")]
		public  int? Roh_frequency { get; set;}

		[RDFProperty("http://w3id.org/roh/conductedByTitle")]
		public  string Roh_conductedByTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/concreteFunctions")]
		public  string Roh_concreteFunctions { get; set;}

		[RDFProperty("http://w3id.org/roh/eventTypeOther")]
		public  string Roh_eventTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/correspondingAuthor")]
		public  bool Roh_correspondingAuthor { get; set;}

		[RDFProperty("http://purl.org/dc/elements/1.1/type")]
		public  string Dc_type { get; set;}

		[RDFProperty("http://w3id.org/roh/publicationName")]
		public  string Roh_publicationName { get; set;}

		[RDFProperty("http://w3id.org/roh/professionalCategory")]
		public  string Roh_professionalCategory { get; set;}

		[RDFProperty("http://w3id.org/roh/participationTypeOther")]
		public  string Roh_participationTypeOther { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/issue")]
		public  string Bibo_issue { get; set;}

		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime? Dct_issued { get; set;}

		[RDFProperty("http://w3id.org/roh/representedEntityTitle")]
		public  string Roh_representedEntityTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://w3id.org/roh/attendants")]
		public  int? Roh_attendants { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#url")]
		public  string Vcard_url { get; set;}

		[RDFProperty("http://w3id.org/roh/activityModalityOther")]
		public  string Roh_activityModalityOther { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/publisher")]
		public  string Bibo_publisher { get; set;}

		[RDFProperty("http://w3id.org/roh/legalDeposit")]
		public  string Roh_legalDeposit { get; set;}

		[RDFProperty("http://w3id.org/roh/geographicFocusOther")]
		public  string Roh_geographicFocusOther { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("http://w3id.org/roh/accessSystemActivityOther")]
		public  string Roh_accessSystemActivityOther { get; set;}

		[RDFProperty("http://w3id.org/roh/goals")]
		public  string Roh_goals { get; set;}

		[RDFProperty("http://w3id.org/roh/publicationTitle")]
		public  string Roh_publicationTitle { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/eventInscriptionTypeOther")]
		public  string Roh_eventInscriptionTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/representedEntityLocality")]
		public  string Roh_representedEntityLocality { get; set;}

		[RDFProperty("http://w3id.org/roh/promotedByTitle")]
		public  string Roh_promotedByTitle { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/presentedAt")]
		public  string Bibo_presentedAt { get; set;}

		[RDFProperty("http://w3id.org/roh/personNumber")]
		public  int? Roh_personNumber { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/pmid")]
		public  string Bibo_pmid { get; set;}

		[RDFProperty("http://w3id.org/roh/withExternalAdmissionsCommittee")]
		public  bool Roh_withExternalAdmissionsCommittee { get; set;}

		[RDFProperty("http://w3id.org/roh/managementTypeOther")]
		public  string Roh_managementTypeOther { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/handle")]
		public  string Bibo_handle { get; set;}

		[RDFProperty("http://w3id.org/roh/durationMonths")]
		public  string Roh_durationMonths { get; set;}

		[RDFProperty("http://w3id.org/roh/conductedByTypeOther")]
		public  string Roh_conductedByTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/averageAnnualBudget")]
		public  float? Roh_averageAnnualBudget { get; set;}

		[RDFProperty("http://w3id.org/roh/promotedByTypeOther")]
		public  string Roh_promotedByTypeOther { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/doi")]
		public  string Bibo_doi { get; set;}

		[RDFProperty("http://w3id.org/roh/durationYears")]
		public  string Roh_durationYears { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/volume")]
		public  string Bibo_volume { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/issn")]
		public  string Bibo_issn { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/pageStart")]
		public  string Bibo_pageStart { get; set;}

		[RDFProperty("http://w3id.org/roh/representedEntityTypeOther")]
		public  string Roh_representedEntityTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/congressProceedingsPublication")]
		public  bool Roh_congressProceedingsPublication { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#end")]
		public  DateTime? Vivo_end { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/pageEnd")]
		public  string Bibo_pageEnd { get; set;}

		[RDFProperty("http://w3id.org/roh/functions")]
		public  string Roh_functions { get; set;}

		[RDFProperty("http://w3id.org/roh/owner")]
		[Required]
		public  Person Roh_owner  { get; set;} 
		public string IdRoh_owner  { get; set;} 

		[RDFProperty("http://w3id.org/roh/classificationCVN")]
		public  string Roh_classificationCVN { get; set;}

		[RDFProperty("http://w3id.org/roh/cvnCode")]
		public  string Roh_cvnCode { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:promotedBy", this.IdRoh_promotedBy));
			propList.Add(new StringOntologyProperty("roh:eventType", this.IdRoh_eventType));
			propList.Add(new ListStringOntologyProperty("roh:representedEntityHasRegion", this.IdsRoh_representedEntityHasRegion));
			propList.Add(new StringOntologyProperty("roh:conductedByType", this.IdRoh_conductedByType));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:targetGroupProfile", this.IdRoh_targetGroupProfile));
			propList.Add(new StringOntologyProperty("roh:promotedByType", this.IdRoh_promotedByType));
			propList.Add(new StringOntologyProperty("roh:promotedByHasCountryName", this.IdRoh_promotedByHasCountryName));
			propList.Add(new StringOntologyProperty("roh:promotedByHasRegion", this.IdRoh_promotedByHasRegion));
			propList.Add(new StringOntologyProperty("roh:accessSystemActivity", this.IdRoh_accessSystemActivity));
			propList.Add(new StringOntologyProperty("roh:publicationHasRegion", this.IdRoh_publicationHasRegion));
			propList.Add(new StringOntologyProperty("vivo:geographicFocus", this.IdVivo_geographicFocus));
			propList.Add(new StringOntologyProperty("roh:eventInscriptionType", this.IdRoh_eventInscriptionType));
			propList.Add(new StringOntologyProperty("roh:activityModality", this.IdRoh_activityModality));
			propList.Add(new ListStringOntologyProperty("roh:representedEntityType", this.IdsRoh_representedEntityType));
			propList.Add(new StringOntologyProperty("roh:publicationType", this.IdRoh_publicationType));
			propList.Add(new StringOntologyProperty("roh:managementType", this.IdRoh_managementType));
			propList.Add(new StringOntologyProperty("roh:participationType", this.IdRoh_participationType));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("roh:conductedBy", this.IdRoh_conductedBy));
			propList.Add(new ListStringOntologyProperty("roh:representedEntityHasCountryName", this.IdsRoh_representedEntityHasCountryName));
			propList.Add(new StringOntologyProperty("roh:publicationHasCountryName", this.IdRoh_publicationHasCountryName));
			propList.Add(new ListStringOntologyProperty("roh:representedEntity", this.IdsRoh_representedEntity));
			propList.Add(new StringOntologyProperty("roh:promotedByLocality", this.Roh_promotedByLocality));
			propList.Add(new StringOntologyProperty("roh:isbn", this.Roh_isbn));
			propList.Add(new StringOntologyProperty("roh:durationDays", this.Roh_durationDays));
			propList.Add(new StringOntologyProperty("roh:frequency", this.Roh_frequency.ToString()));
			propList.Add(new StringOntologyProperty("roh:conductedByTitle", this.Roh_conductedByTitle));
			propList.Add(new StringOntologyProperty("roh:concreteFunctions", this.Roh_concreteFunctions));
			propList.Add(new StringOntologyProperty("roh:eventTypeOther", this.Roh_eventTypeOther));
			propList.Add(new BoolOntologyProperty("roh:correspondingAuthor", this.Roh_correspondingAuthor));
			propList.Add(new StringOntologyProperty("dc:type", this.Dc_type));
			propList.Add(new StringOntologyProperty("roh:publicationName", this.Roh_publicationName));
			propList.Add(new StringOntologyProperty("roh:professionalCategory", this.Roh_professionalCategory));
			propList.Add(new StringOntologyProperty("roh:participationTypeOther", this.Roh_participationTypeOther));
			propList.Add(new StringOntologyProperty("bibo:issue", this.Bibo_issue));
			if (this.Dct_issued.HasValue){
				propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued.Value));
				}
			propList.Add(new StringOntologyProperty("roh:representedEntityTitle", this.Roh_representedEntityTitle));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			propList.Add(new StringOntologyProperty("roh:attendants", this.Roh_attendants.ToString()));
			propList.Add(new StringOntologyProperty("vcard:url", this.Vcard_url));
			propList.Add(new StringOntologyProperty("roh:activityModalityOther", this.Roh_activityModalityOther));
			propList.Add(new StringOntologyProperty("bibo:publisher", this.Bibo_publisher));
			propList.Add(new StringOntologyProperty("roh:legalDeposit", this.Roh_legalDeposit));
			propList.Add(new StringOntologyProperty("roh:geographicFocusOther", this.Roh_geographicFocusOther));
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new StringOntologyProperty("roh:accessSystemActivityOther", this.Roh_accessSystemActivityOther));
			propList.Add(new StringOntologyProperty("roh:goals", this.Roh_goals));
			propList.Add(new StringOntologyProperty("roh:publicationTitle", this.Roh_publicationTitle));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("roh:eventInscriptionTypeOther", this.Roh_eventInscriptionTypeOther));
			propList.Add(new StringOntologyProperty("roh:representedEntityLocality", this.Roh_representedEntityLocality));
			propList.Add(new StringOntologyProperty("roh:promotedByTitle", this.Roh_promotedByTitle));
			propList.Add(new StringOntologyProperty("bibo:presentedAt", this.Bibo_presentedAt));
			propList.Add(new StringOntologyProperty("roh:personNumber", this.Roh_personNumber.ToString()));
			propList.Add(new StringOntologyProperty("bibo:pmid", this.Bibo_pmid));
			propList.Add(new BoolOntologyProperty("roh:withExternalAdmissionsCommittee", this.Roh_withExternalAdmissionsCommittee));
			propList.Add(new StringOntologyProperty("roh:managementTypeOther", this.Roh_managementTypeOther));
			propList.Add(new StringOntologyProperty("bibo:handle", this.Bibo_handle));
			propList.Add(new StringOntologyProperty("roh:durationMonths", this.Roh_durationMonths));
			propList.Add(new StringOntologyProperty("roh:conductedByTypeOther", this.Roh_conductedByTypeOther));
			propList.Add(new StringOntologyProperty("roh:averageAnnualBudget", this.Roh_averageAnnualBudget.ToString()));
			propList.Add(new StringOntologyProperty("roh:promotedByTypeOther", this.Roh_promotedByTypeOther));
			propList.Add(new StringOntologyProperty("bibo:doi", this.Bibo_doi));
			propList.Add(new StringOntologyProperty("roh:durationYears", this.Roh_durationYears));
			propList.Add(new StringOntologyProperty("bibo:volume", this.Bibo_volume));
			propList.Add(new StringOntologyProperty("bibo:issn", this.Bibo_issn));
			propList.Add(new StringOntologyProperty("bibo:pageStart", this.Bibo_pageStart));
			propList.Add(new StringOntologyProperty("roh:representedEntityTypeOther", this.Roh_representedEntityTypeOther));
			propList.Add(new BoolOntologyProperty("roh:congressProceedingsPublication", this.Roh_congressProceedingsPublication));
			if (this.Vivo_end.HasValue){
				propList.Add(new DateOntologyProperty("vivo:end", this.Vivo_end.Value));
				}
			propList.Add(new StringOntologyProperty("bibo:pageEnd", this.Bibo_pageEnd));
			propList.Add(new StringOntologyProperty("roh:functions", this.Roh_functions));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:classificationCVN", this.Roh_classificationCVN));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Bibo_authorList!=null){
				foreach(BFO_0000023 prop in Bibo_authorList){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityBFO_0000023 = new OntologyEntity("http://purl.obolibrary.org/obo/BFO_0000023", "http://purl.obolibrary.org/obo/BFO_0000023", "bibo:authorList", prop.propList, prop.entList);
				entList.Add(entityBFO_0000023);
				prop.Entity= entityBFO_0000023;
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
			if(Bibo_identifier!=null){
				foreach(Document prop in Bibo_identifier){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityDocument = new OntologyEntity("http://xmlns.com/foaf/0.1/Document", "http://xmlns.com/foaf/0.1/Document", "bibo:identifier", prop.propList, prop.entList);
				entList.Add(entityDocument);
				prop.Entity= entityDocument;
				}
			}
			if(Roh_hasKnowledgeArea!=null){
				foreach(CategoryPath prop in Roh_hasKnowledgeArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:hasKnowledgeArea", prop.propList, prop.entList);
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Activity>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Activity\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Bibo_authorList != null)
			{
			foreach(var item0 in this.Bibo_authorList)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://purl.obolibrary.org/obo/BFO_0000023>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://purl.obolibrary.org/obo/BFO_0000023\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://purl.org/ontology/bibo/authorList", $"<{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName)}\"", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick)}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName)}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName)}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_hasKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_hasKnowledgeArea)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://w3id.org/roh/hasKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Bibo_identifier != null)
			{
			foreach(var item0 in this.Bibo_identifier)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://xmlns.com/foaf/0.1/Document>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://xmlns.com/foaf/0.1/Document\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://purl.org/ontology/bibo/identifier", $"<{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Dc_title)}\"", list, " . ");
				}
				if(item0.Foaf_topic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/topic", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_topic)}\"", list, " . ");
				}
			}
			}
				if(this.IdRoh_promotedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedBy", $"<{this.IdRoh_promotedBy}>", list, " . ");
				}
				if(this.IdRoh_eventType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/eventType", $"<{this.IdRoh_eventType}>", list, " . ");
				}
				if(this.IdsRoh_representedEntityHasRegion != null)
				{
					foreach(var item2 in this.IdsRoh_representedEntityHasRegion)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://w3id.org/roh/representedEntityHasRegion", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdRoh_conductedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedByType", $"<{this.IdRoh_conductedByType}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_targetGroupProfile != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/targetGroupProfile", $"<{this.IdRoh_targetGroupProfile}>", list, " . ");
				}
				if(this.IdRoh_promotedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByType", $"<{this.IdRoh_promotedByType}>", list, " . ");
				}
				if(this.IdRoh_promotedByHasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByHasCountryName", $"<{this.IdRoh_promotedByHasCountryName}>", list, " . ");
				}
				if(this.IdRoh_promotedByHasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByHasRegion", $"<{this.IdRoh_promotedByHasRegion}>", list, " . ");
				}
				if(this.IdRoh_accessSystemActivity != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/accessSystemActivity", $"<{this.IdRoh_accessSystemActivity}>", list, " . ");
				}
				if(this.IdRoh_publicationHasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicationHasRegion", $"<{this.IdRoh_publicationHasRegion}>", list, " . ");
				}
				if(this.IdVivo_geographicFocus != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#geographicFocus", $"<{this.IdVivo_geographicFocus}>", list, " . ");
				}
				if(this.IdRoh_eventInscriptionType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/eventInscriptionType", $"<{this.IdRoh_eventInscriptionType}>", list, " . ");
				}
				if(this.IdRoh_activityModality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/activityModality", $"<{this.IdRoh_activityModality}>", list, " . ");
				}
				if(this.IdsRoh_representedEntityType != null)
				{
					foreach(var item2 in this.IdsRoh_representedEntityType)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://w3id.org/roh/representedEntityType", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdRoh_publicationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicationType", $"<{this.IdRoh_publicationType}>", list, " . ");
				}
				if(this.IdRoh_managementType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/managementType", $"<{this.IdRoh_managementType}>", list, " . ");
				}
				if(this.IdRoh_participationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/participationType", $"<{this.IdRoh_participationType}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdRoh_conductedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedBy", $"<{this.IdRoh_conductedBy}>", list, " . ");
				}
				if(this.IdsRoh_representedEntityHasCountryName != null)
				{
					foreach(var item2 in this.IdsRoh_representedEntityHasCountryName)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://w3id.org/roh/representedEntityHasCountryName", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdRoh_publicationHasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicationHasCountryName", $"<{this.IdRoh_publicationHasCountryName}>", list, " . ");
				}
				if(this.IdsRoh_representedEntity != null)
				{
					foreach(var item2 in this.IdsRoh_representedEntity)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}", "http://w3id.org/roh/representedEntity", $"<{item2}>", list, " . ");
					}
				}
				if(this.Roh_promotedByLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByLocality)}\"", list, " . ");
				}
				if(this.Roh_isbn != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isbn", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_isbn)}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays)}\"", list, " . ");
				}
				if(this.Roh_frequency != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/frequency", $"{this.Roh_frequency.Value.ToString()}", list, " . ");
				}
				if(this.Roh_conductedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTitle)}\"", list, " . ");
				}
				if(this.Roh_concreteFunctions != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/concreteFunctions", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_concreteFunctions)}\"", list, " . ");
				}
				if(this.Roh_eventTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/eventTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_eventTypeOther)}\"", list, " . ");
				}
				if(this.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/correspondingAuthor", $"\"{this.Roh_correspondingAuthor.ToString()}\"", list, " . ");
				}
				if(this.Dc_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/type", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_type)}\"", list, " . ");
				}
				if(this.Roh_publicationName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicationName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_publicationName)}\"", list, " . ");
				}
				if(this.Roh_professionalCategory != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/professionalCategory", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_professionalCategory)}\"", list, " . ");
				}
				if(this.Roh_participationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/participationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_participationTypeOther)}\"", list, " . ");
				}
				if(this.Bibo_issue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/issue", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issue)}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_representedEntityTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/representedEntityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_representedEntityTitle)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Roh_attendants != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/attendants", $"{this.Roh_attendants.Value.ToString()}", list, " . ");
				}
				if(this.Vcard_url != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#url", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_url)}\"", list, " . ");
				}
				if(this.Roh_activityModalityOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/activityModalityOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_activityModalityOther)}\"", list, " . ");
				}
				if(this.Bibo_publisher != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/publisher", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_publisher)}\"", list, " . ");
				}
				if(this.Roh_legalDeposit != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/legalDeposit", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_legalDeposit)}\"", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther)}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_accessSystemActivityOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/accessSystemActivityOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_accessSystemActivityOther)}\"", list, " . ");
				}
				if(this.Roh_goals != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/goals", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_goals)}\"", list, " . ");
				}
				if(this.Roh_publicationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicationTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_publicationTitle)}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Roh_eventInscriptionTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/eventInscriptionTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_eventInscriptionTypeOther)}\"", list, " . ");
				}
				if(this.Roh_representedEntityLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/representedEntityLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_representedEntityLocality)}\"", list, " . ");
				}
				if(this.Roh_promotedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTitle)}\"", list, " . ");
				}
				if(this.Bibo_presentedAt != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/presentedAt", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_presentedAt)}\"", list, " . ");
				}
				if(this.Roh_personNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/personNumber", $"{this.Roh_personNumber.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_pmid != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/pmid", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pmid)}\"", list, " . ");
				}
				if(this.Roh_withExternalAdmissionsCommittee != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/withExternalAdmissionsCommittee", $"\"{this.Roh_withExternalAdmissionsCommittee.ToString()}\"", list, " . ");
				}
				if(this.Roh_managementTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/managementTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_managementTypeOther)}\"", list, " . ");
				}
				if(this.Bibo_handle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/handle", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_handle)}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths)}\"", list, " . ");
				}
				if(this.Roh_conductedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTypeOther)}\"", list, " . ");
				}
				if(this.Roh_averageAnnualBudget != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/averageAnnualBudget", $"{this.Roh_averageAnnualBudget.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_promotedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTypeOther)}\"", list, " . ");
				}
				if(this.Bibo_doi != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_doi)}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears)}\"", list, " . ");
				}
				if(this.Bibo_volume != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/volume", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_volume)}\"", list, " . ");
				}
				if(this.Bibo_issn != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/issn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issn)}\"", list, " . ");
				}
				if(this.Bibo_pageStart != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/pageStart", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pageStart)}\"", list, " . ");
				}
				if(this.Roh_representedEntityTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/representedEntityTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_representedEntityTypeOther)}\"", list, " . ");
				}
				if(this.Roh_congressProceedingsPublication != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/congressProceedingsPublication", $"\"{this.Roh_congressProceedingsPublication.ToString()}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#end", $"\"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Bibo_pageEnd != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/pageEnd", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pageEnd)}\"", list, " . ");
				}
				if(this.Roh_functions != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/functions", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_functions)}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_classificationCVN != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/classificationCVN", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_classificationCVN)}\"", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Activity_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"activity\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/Activity\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Bibo_authorList != null)
			{
			foreach(var item0 in this.Bibo_authorList)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/ontology/bibo/authorList", $"<{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName).ToLower()}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
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
			if(this.Roh_hasKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_hasKnowledgeArea)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/hasKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
			if(this.Bibo_identifier != null)
			{
			foreach(var item0 in this.Bibo_identifier)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/ontology/bibo/identifier", $"<{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Dc_title).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_topic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/topic", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_topic).ToLower()}\"", list, " . ");
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
				if(this.IdRoh_eventType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_eventType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/eventType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsRoh_representedEntityHasRegion != null)
				{
					foreach(var item2 in this.IdsRoh_representedEntityHasRegion)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/representedEntityHasRegion", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdRoh_conductedByType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_conductedByType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/conductedByType", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_targetGroupProfile != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_targetGroupProfile;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/targetGroupProfile", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_promotedByHasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_promotedByHasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByHasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_promotedByHasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_promotedByHasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByHasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_accessSystemActivity != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_accessSystemActivity;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/accessSystemActivity", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_publicationHasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_publicationHasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicationHasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdVivo_geographicFocus != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVivo_geographicFocus;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#geographicFocus", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_eventInscriptionType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_eventInscriptionType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/eventInscriptionType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_activityModality != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_activityModality;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/activityModality", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsRoh_representedEntityType != null)
				{
					foreach(var item2 in this.IdsRoh_representedEntityType)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/representedEntityType", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdRoh_publicationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_publicationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicationType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_managementType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_managementType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/managementType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_participationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_participationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/participationType", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_conductedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_conductedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/conductedBy", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsRoh_representedEntityHasCountryName != null)
				{
					foreach(var item2 in this.IdsRoh_representedEntityHasCountryName)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/representedEntityHasCountryName", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdRoh_publicationHasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_publicationHasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicationHasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsRoh_representedEntity != null)
				{
					foreach(var item2 in this.IdsRoh_representedEntity)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/representedEntity", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.Roh_promotedByLocality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByLocality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_isbn != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isbn", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_isbn).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays).ToLower()}\"", list, " . ");
				}
				if(this.Roh_frequency != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/frequency", $"{this.Roh_frequency.Value.ToString()}", list, " . ");
				}
				if(this.Roh_conductedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/conductedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_concreteFunctions != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/concreteFunctions", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_concreteFunctions).ToLower()}\"", list, " . ");
				}
				if(this.Roh_eventTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/eventTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_eventTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_correspondingAuthor != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/correspondingAuthor", $"\"{this.Roh_correspondingAuthor.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Dc_type != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/elements/1.1/type", $"\"{GenerarTextoSinSaltoDeLinea(this.Dc_type).ToLower()}\"", list, " . ");
				}
				if(this.Roh_publicationName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicationName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_publicationName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_professionalCategory != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/professionalCategory", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_professionalCategory).ToLower()}\"", list, " . ");
				}
				if(this.Roh_participationTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/participationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_participationTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_issue != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/issue", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issue).ToLower()}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/terms/issued", $"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_representedEntityTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/representedEntityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_representedEntityTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Roh_attendants != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/attendants", $"{this.Roh_attendants.Value.ToString()}", list, " . ");
				}
				if(this.Vcard_url != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#url", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_url).ToLower()}\"", list, " . ");
				}
				if(this.Roh_activityModalityOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/activityModalityOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_activityModalityOther).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_publisher != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/publisher", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_publisher).ToLower()}\"", list, " . ");
				}
				if(this.Roh_legalDeposit != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/legalDeposit", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_legalDeposit).ToLower()}\"", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#start", $"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_accessSystemActivityOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/accessSystemActivityOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_accessSystemActivityOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_goals != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/goals", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_goals).ToLower()}\"", list, " . ");
				}
				if(this.Roh_publicationTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicationTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_publicationTitle).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_eventInscriptionTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/eventInscriptionTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_eventInscriptionTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_representedEntityLocality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/representedEntityLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_representedEntityLocality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_promotedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_presentedAt != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/presentedAt", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_presentedAt).ToLower()}\"", list, " . ");
				}
				if(this.Roh_personNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/personNumber", $"{this.Roh_personNumber.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_pmid != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/pmid", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pmid).ToLower()}\"", list, " . ");
				}
				if(this.Roh_withExternalAdmissionsCommittee != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/withExternalAdmissionsCommittee", $"\"{this.Roh_withExternalAdmissionsCommittee.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_managementTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/managementTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_managementTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_handle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/handle", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_handle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths).ToLower()}\"", list, " . ");
				}
				if(this.Roh_conductedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/conductedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_averageAnnualBudget != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/averageAnnualBudget", $"{this.Roh_averageAnnualBudget.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_promotedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_doi != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_doi).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_volume != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/volume", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_volume).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_issn != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/issn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issn).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_pageStart != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/pageStart", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pageStart).ToLower()}\"", list, " . ");
				}
				if(this.Roh_representedEntityTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/representedEntityTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_representedEntityTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_congressProceedingsPublication != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/congressProceedingsPublication", $"\"{this.Roh_congressProceedingsPublication.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#end", $"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Bibo_pageEnd != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/pageEnd", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pageEnd).ToLower()}\"", list, " . ");
				}
				if(this.Roh_functions != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/functions", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_functions).ToLower()}\"", list, " . ");
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
				if(this.Roh_classificationCVN != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/classificationCVN", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_classificationCVN).ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/ActivityOntology_{ResourceID}_{ArticleID}";
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
