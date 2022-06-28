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
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using Feature = FeatureOntology.Feature;
using Person = PersonOntology.Person;
using Group = GroupOntology.Group;
using GeographicRegion = GeographicregionOntology.GeographicRegion;
using Organization = OrganizationOntology.Organization;
using ProjectAuthorization = ProjectauthorizationOntology.ProjectAuthorization;
using ProjectType = ProjecttypeOntology.ProjectType;
using ProjectModality = ProjectmodalityOntology.ProjectModality;
using ScientificExperienceProject = ScientificexperienceprojectOntology.ScientificExperienceProject;

namespace ProjectOntology
{
	[ExcludeFromCodeCoverage]
	public class Project : GnossOCBase
	{

		public Project() : base() { } 

		public Project(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
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
			this.Roh_grantedBy = new List<OrganizationAux>();
			SemanticPropertyModel propRoh_grantedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/grantedBy");
			if(propRoh_grantedBy != null && propRoh_grantedBy.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_grantedBy.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						OrganizationAux roh_grantedBy = new OrganizationAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_grantedBy.Add(roh_grantedBy);
					}
				}
			}
			this.Roh_mainResearchers = new List<PersonAux>();
			SemanticPropertyModel propRoh_mainResearchers = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/mainResearchers");
			if(propRoh_mainResearchers != null && propRoh_mainResearchers.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_mainResearchers.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PersonAux roh_mainResearchers = new PersonAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_mainResearchers.Add(roh_mainResearchers);
					}
				}
			}
			this.Roh_membersProject = new List<Person>();
			SemanticPropertyModel propRoh_membersProject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/membersProject");
			if(propRoh_membersProject != null && propRoh_membersProject.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_membersProject.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person roh_membersProject = new Person(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_membersProject.Add(roh_membersProject);
					}
				}
			}
			this.Roh_isProducedBy = new List<Group>();
			SemanticPropertyModel propRoh_isProducedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isProducedBy");
			if(propRoh_isProducedBy != null && propRoh_isProducedBy.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_isProducedBy.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Group roh_isProducedBy = new Group(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_isProducedBy.Add(roh_isProducedBy);
					}
				}
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new GeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_researchers = new List<PersonAux>();
			SemanticPropertyModel propRoh_researchers = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchers");
			if(propRoh_researchers != null && propRoh_researchers.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_researchers.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PersonAux roh_researchers = new PersonAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_researchers.Add(roh_researchers);
					}
				}
			}
			this.Roh_participates = new List<OrganizationAux>();
			SemanticPropertyModel propRoh_participates = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participates");
			if(propRoh_participates != null && propRoh_participates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_participates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						OrganizationAux roh_participates = new OrganizationAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_participates.Add(roh_participates);
					}
				}
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
			SemanticPropertyModel propRoh_projectAuthorization = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectAuthorization");
			if(propRoh_projectAuthorization != null && propRoh_projectAuthorization.PropertyValues.Count > 0)
			{
				this.Roh_projectAuthorization = new ProjectAuthorization(propRoh_projectAuthorization.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_projectType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectType");
			if(propRoh_projectType != null && propRoh_projectType.PropertyValues.Count > 0)
			{
				this.Roh_projectType = new ProjectType(propRoh_projectType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Vivo_relates = new List<BFO_0000023>();
			SemanticPropertyModel propVivo_relates = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#relates");
			if(propVivo_relates != null && propVivo_relates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_relates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						BFO_0000023 vivo_relates = new BFO_0000023(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_relates.Add(vivo_relates);
					}
				}
			}
			SemanticPropertyModel propRoh_modality = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/modality");
			if(propRoh_modality != null && propRoh_modality.PropertyValues.Count > 0)
			{
				this.Roh_modality = new ProjectModality(propRoh_modality.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_mixedPercentage = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/mixedPercentage"));
			this.Roh_isSupportedBy = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isSupportedBy"));
			this.Roh_relevantResults = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relevantResults"));
			this.Roh_creditPercentage = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/creditPercentage"));
			this.Roh_peopleYearNumber = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/peopleYearNumber"));
			this.Roh_validationStatusProject = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/validationStatusProject"));
			this.Roh_yearEnd = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/yearEnd"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Roh_publicationsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationsNumber"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_conductedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTitle"));
			this.Roh_grantsPercentage = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/grantsPercentage"));
			this.Vivo_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#description"));
			this.Roh_subProjectMonetaryAmount = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/subProjectMonetaryAmount"));
			this.Roh_themedAreasNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/themedAreasNumber"));
			this.Roh_yearStart = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/yearStart"));
			this.Roh_projectCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectCode"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_researchersNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchersNumber"));
			this.Roh_conductedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTypeOther"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Roh_collaborative= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collaborative"));
			this.Roh_monetaryAmount = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/monetaryAmount"));
			this.Roh_collaboratorsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collaboratorsNumber"));
			SemanticPropertyModel propRoh_scientificExperienceProject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificExperienceProject");
			if(propRoh_scientificExperienceProject != null && propRoh_scientificExperienceProject.PropertyValues.Count > 0)
			{
				this.Roh_scientificExperienceProject = new ScientificExperienceProject(propRoh_scientificExperienceProject.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isValidated= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isValidated"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public Project(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
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
			this.Roh_grantedBy = new List<OrganizationAux>();
			SemanticPropertyModel propRoh_grantedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/grantedBy");
			if(propRoh_grantedBy != null && propRoh_grantedBy.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_grantedBy.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						OrganizationAux roh_grantedBy = new OrganizationAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_grantedBy.Add(roh_grantedBy);
					}
				}
			}
			this.Roh_mainResearchers = new List<PersonAux>();
			SemanticPropertyModel propRoh_mainResearchers = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/mainResearchers");
			if(propRoh_mainResearchers != null && propRoh_mainResearchers.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_mainResearchers.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PersonAux roh_mainResearchers = new PersonAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_mainResearchers.Add(roh_mainResearchers);
					}
				}
			}
			this.Roh_membersProject = new List<Person>();
			SemanticPropertyModel propRoh_membersProject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/membersProject");
			if(propRoh_membersProject != null && propRoh_membersProject.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_membersProject.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person roh_membersProject = new Person(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_membersProject.Add(roh_membersProject);
					}
				}
			}
			this.Roh_isProducedBy = new List<Group>();
			SemanticPropertyModel propRoh_isProducedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isProducedBy");
			if(propRoh_isProducedBy != null && propRoh_isProducedBy.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_isProducedBy.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Group roh_isProducedBy = new Group(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_isProducedBy.Add(roh_isProducedBy);
					}
				}
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new GeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_researchers = new List<PersonAux>();
			SemanticPropertyModel propRoh_researchers = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchers");
			if(propRoh_researchers != null && propRoh_researchers.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_researchers.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PersonAux roh_researchers = new PersonAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_researchers.Add(roh_researchers);
					}
				}
			}
			this.Roh_participates = new List<OrganizationAux>();
			SemanticPropertyModel propRoh_participates = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participates");
			if(propRoh_participates != null && propRoh_participates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_participates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						OrganizationAux roh_participates = new OrganizationAux(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_participates.Add(roh_participates);
					}
				}
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
			SemanticPropertyModel propRoh_projectAuthorization = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectAuthorization");
			if(propRoh_projectAuthorization != null && propRoh_projectAuthorization.PropertyValues.Count > 0)
			{
				this.Roh_projectAuthorization = new ProjectAuthorization(propRoh_projectAuthorization.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_projectType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectType");
			if(propRoh_projectType != null && propRoh_projectType.PropertyValues.Count > 0)
			{
				this.Roh_projectType = new ProjectType(propRoh_projectType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Vivo_relates = new List<BFO_0000023>();
			SemanticPropertyModel propVivo_relates = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#relates");
			if(propVivo_relates != null && propVivo_relates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_relates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						BFO_0000023 vivo_relates = new BFO_0000023(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_relates.Add(vivo_relates);
					}
				}
			}
			SemanticPropertyModel propRoh_modality = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/modality");
			if(propRoh_modality != null && propRoh_modality.PropertyValues.Count > 0)
			{
				this.Roh_modality = new ProjectModality(propRoh_modality.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_mixedPercentage = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/mixedPercentage"));
			this.Roh_isSupportedBy = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isSupportedBy"));
			this.Roh_relevantResults = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relevantResults"));
			this.Roh_creditPercentage = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/creditPercentage"));
			this.Roh_peopleYearNumber = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/peopleYearNumber"));
			this.Roh_validationStatusProject = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/validationStatusProject"));
			this.Roh_yearEnd = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/yearEnd"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Roh_publicationsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationsNumber"));
			this.Roh_durationDays = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_conductedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTitle"));
			this.Roh_grantsPercentage = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/grantsPercentage"));
			this.Vivo_description = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#description"));
			this.Roh_subProjectMonetaryAmount = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/subProjectMonetaryAmount"));
			this.Roh_themedAreasNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/themedAreasNumber"));
			this.Roh_yearStart = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/yearStart"));
			this.Roh_projectCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectCode"));
			this.Roh_durationMonths = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_researchersNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchersNumber"));
			this.Roh_conductedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTypeOther"));
			this.Roh_durationYears = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Roh_collaborative= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collaborative"));
			this.Roh_monetaryAmount = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/monetaryAmount"));
			this.Roh_collaboratorsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collaboratorsNumber"));
			SemanticPropertyModel propRoh_scientificExperienceProject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificExperienceProject");
			if(propRoh_scientificExperienceProject != null && propRoh_scientificExperienceProject.PropertyValues.Count > 0)
			{
				this.Roh_scientificExperienceProject = new ScientificExperienceProject(propRoh_scientificExperienceProject.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isValidated= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isValidated"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://vivoweb.org/ontology/core#Project"; } }
		public virtual string RdfsLabel { get { return "http://vivoweb.org/ontology/core#Project"; } }
		[RDFProperty("http://w3id.org/roh/conductedByType")]
		public  OrganizationType Roh_conductedByType  { get; set;} 
		public string IdRoh_conductedByType  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/grantedBy")]
		public  List<OrganizationAux> Roh_grantedBy { get; set;}

		[RDFProperty("http://w3id.org/roh/mainResearchers")]
		public  List<PersonAux> Roh_mainResearchers { get; set;}

		[LABEL(LanguageEnum.es,"Persona")]
		[RDFProperty("http://w3id.org/roh/membersProject")]
		public  List<Person> Roh_membersProject { get; set;}
		public List<string> IdsRoh_membersProject { get; set;}

		[RDFProperty("http://w3id.org/roh/isProducedBy")]
		public  List<Group> Roh_isProducedBy { get; set;}
		public List<string> IdsRoh_isProducedBy { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#geographicFocus")]
		public  GeographicRegion Vivo_geographicFocus  { get; set;} 
		public string IdVivo_geographicFocus  { get; set;} 

		[RDFProperty("http://w3id.org/roh/researchers")]
		public  List<PersonAux> Roh_researchers { get; set;}

		[RDFProperty("http://w3id.org/roh/participates")]
		public  List<OrganizationAux> Roh_participates { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/conductedBy")]
		public  Organization Roh_conductedBy  { get; set;} 
		public string IdRoh_conductedBy  { get; set;} 

		[RDFProperty("http://w3id.org/roh/projectAuthorization")]
		public  ProjectAuthorization Roh_projectAuthorization  { get; set;} 
		public string IdRoh_projectAuthorization  { get; set;} 

		[RDFProperty("http://w3id.org/roh/hasKnowledgeArea")]
		public  List<CategoryPath> Roh_hasKnowledgeArea { get; set;}

		[RDFProperty("http://w3id.org/roh/projectType")]
		public  ProjectType Roh_projectType  { get; set;} 
		public string IdRoh_projectType  { get; set;} 

		[RDFProperty("http://vivoweb.org/ontology/core#relates")]
		public  List<BFO_0000023> Vivo_relates { get; set;}

		[RDFProperty("http://w3id.org/roh/modality")]
		public  ProjectModality Roh_modality  { get; set;} 
		public string IdRoh_modality  { get; set;} 

		[RDFProperty("http://w3id.org/roh/mixedPercentage")]
		public  float? Roh_mixedPercentage { get; set;}

		[RDFProperty("http://w3id.org/roh/isSupportedBy")]
		public  string Roh_isSupportedBy { get; set;}

		[RDFProperty("http://w3id.org/roh/relevantResults")]
		public  string Roh_relevantResults { get; set;}

		[RDFProperty("http://w3id.org/roh/creditPercentage")]
		public  float? Roh_creditPercentage { get; set;}

		[RDFProperty("http://w3id.org/roh/peopleYearNumber")]
		public  float? Roh_peopleYearNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/validationStatusProject")]
		public  string Roh_validationStatusProject { get; set;}

		[RDFProperty("http://w3id.org/roh/yearEnd")]
		public  int? Roh_yearEnd { get; set;}

		[RDFProperty("http://w3id.org/roh/geographicFocusOther")]
		public  string Roh_geographicFocusOther { get; set;}

		[RDFProperty("http://w3id.org/roh/publicationsNumber")]
		public  int? Roh_publicationsNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/durationDays")]
		public  string Roh_durationDays { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/conductedByTitle")]
		public  string Roh_conductedByTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/grantsPercentage")]
		public  float? Roh_grantsPercentage { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#description")]
		public  string Vivo_description { get; set;}

		[RDFProperty("http://w3id.org/roh/subProjectMonetaryAmount")]
		public  float? Roh_subProjectMonetaryAmount { get; set;}

		[RDFProperty("http://w3id.org/roh/themedAreasNumber")]
		public  int? Roh_themedAreasNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/yearStart")]
		public  int? Roh_yearStart { get; set;}

		[RDFProperty("http://w3id.org/roh/projectCode")]
		public  string Roh_projectCode { get; set;}

		[RDFProperty("http://w3id.org/roh/durationMonths")]
		public  string Roh_durationMonths { get; set;}

		[RDFProperty("http://w3id.org/roh/researchersNumber")]
		public  int? Roh_researchersNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/conductedByTypeOther")]
		public  string Roh_conductedByTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/durationYears")]
		public  string Roh_durationYears { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#end")]
		public  DateTime? Vivo_end { get; set;}

		[RDFProperty("http://w3id.org/roh/collaborative")]
		public  bool Roh_collaborative { get; set;}

		[RDFProperty("http://w3id.org/roh/monetaryAmount")]
		public  float? Roh_monetaryAmount { get; set;}

		[RDFProperty("http://w3id.org/roh/collaboratorsNumber")]
		public  int? Roh_collaboratorsNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/scientificExperienceProject")]
		[Required]
		public  ScientificExperienceProject Roh_scientificExperienceProject  { get; set;} 
		public string IdRoh_scientificExperienceProject  { get; set;} 

		[RDFProperty("http://w3id.org/roh/isValidated")]
		public  bool Roh_isValidated { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:conductedByType", this.IdRoh_conductedByType));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new ListStringOntologyProperty("roh:membersProject", this.IdsRoh_membersProject));
			propList.Add(new ListStringOntologyProperty("roh:isProducedBy", this.IdsRoh_isProducedBy));
			propList.Add(new StringOntologyProperty("vivo:geographicFocus", this.IdVivo_geographicFocus));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("roh:conductedBy", this.IdRoh_conductedBy));
			propList.Add(new StringOntologyProperty("roh:projectAuthorization", this.IdRoh_projectAuthorization));
			propList.Add(new StringOntologyProperty("roh:projectType", this.IdRoh_projectType));
			propList.Add(new StringOntologyProperty("roh:modality", this.IdRoh_modality));
			propList.Add(new StringOntologyProperty("roh:mixedPercentage", this.Roh_mixedPercentage.ToString()));
			propList.Add(new StringOntologyProperty("roh:isSupportedBy", this.Roh_isSupportedBy));
			propList.Add(new StringOntologyProperty("roh:relevantResults", this.Roh_relevantResults));
			propList.Add(new StringOntologyProperty("roh:creditPercentage", this.Roh_creditPercentage.ToString()));
			propList.Add(new StringOntologyProperty("roh:peopleYearNumber", this.Roh_peopleYearNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:validationStatusProject", this.Roh_validationStatusProject));
			propList.Add(new StringOntologyProperty("roh:yearEnd", this.Roh_yearEnd.ToString()));
			propList.Add(new StringOntologyProperty("roh:geographicFocusOther", this.Roh_geographicFocusOther));
			propList.Add(new StringOntologyProperty("roh:publicationsNumber", this.Roh_publicationsNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:durationDays", this.Roh_durationDays));
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("roh:conductedByTitle", this.Roh_conductedByTitle));
			propList.Add(new StringOntologyProperty("roh:grantsPercentage", this.Roh_grantsPercentage.ToString()));
			propList.Add(new StringOntologyProperty("vivo:description", this.Vivo_description));
			propList.Add(new StringOntologyProperty("roh:subProjectMonetaryAmount", this.Roh_subProjectMonetaryAmount.ToString()));
			propList.Add(new StringOntologyProperty("roh:themedAreasNumber", this.Roh_themedAreasNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:yearStart", this.Roh_yearStart.ToString()));
			propList.Add(new StringOntologyProperty("roh:projectCode", this.Roh_projectCode));
			propList.Add(new StringOntologyProperty("roh:durationMonths", this.Roh_durationMonths));
			propList.Add(new StringOntologyProperty("roh:researchersNumber", this.Roh_researchersNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:conductedByTypeOther", this.Roh_conductedByTypeOther));
			propList.Add(new StringOntologyProperty("roh:durationYears", this.Roh_durationYears));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			if (this.Vivo_end.HasValue){
				propList.Add(new DateOntologyProperty("vivo:end", this.Vivo_end.Value));
				}
			propList.Add(new BoolOntologyProperty("roh:collaborative", this.Roh_collaborative));
			propList.Add(new StringOntologyProperty("roh:monetaryAmount", this.Roh_monetaryAmount.ToString()));
			propList.Add(new StringOntologyProperty("roh:collaboratorsNumber", this.Roh_collaboratorsNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:scientificExperienceProject", this.IdRoh_scientificExperienceProject));
			propList.Add(new BoolOntologyProperty("roh:isValidated", this.Roh_isValidated));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_grantedBy!=null){
				foreach(OrganizationAux prop in Roh_grantedBy){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityOrganizationAux = new OntologyEntity("http://w3id.org/roh/OrganizationAux", "http://w3id.org/roh/OrganizationAux", "roh:grantedBy", prop.propList, prop.entList);
				entList.Add(entityOrganizationAux);
				prop.Entity= entityOrganizationAux;
				}
			}
			if(Roh_mainResearchers!=null){
				foreach(PersonAux prop in Roh_mainResearchers){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityPersonAux = new OntologyEntity("http://w3id.org/roh/PersonAux", "http://w3id.org/roh/PersonAux", "roh:mainResearchers", prop.propList, prop.entList);
				entList.Add(entityPersonAux);
				prop.Entity= entityPersonAux;
				}
			}
			if(Roh_researchers!=null){
				foreach(PersonAux prop in Roh_researchers){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityPersonAux = new OntologyEntity("http://w3id.org/roh/PersonAux", "http://w3id.org/roh/PersonAux", "roh:researchers", prop.propList, prop.entList);
				entList.Add(entityPersonAux);
				prop.Entity= entityPersonAux;
				}
			}
			if(Roh_participates!=null){
				foreach(OrganizationAux prop in Roh_participates){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityOrganizationAux = new OntologyEntity("http://w3id.org/roh/OrganizationAux", "http://w3id.org/roh/OrganizationAux", "roh:participates", prop.propList, prop.entList);
				entList.Add(entityOrganizationAux);
				prop.Entity= entityOrganizationAux;
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
			if(Vivo_relates!=null){
				foreach(BFO_0000023 prop in Vivo_relates){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityBFO_0000023 = new OntologyEntity("http://purl.obolibrary.org/obo/BFO_0000023", "http://purl.obolibrary.org/obo/BFO_0000023", "vivo:relates", prop.propList, prop.entList);
				entList.Add(entityBFO_0000023);
				prop.Entity= entityBFO_0000023;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://vivoweb.org/ontology/core#Project>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://vivoweb.org/ontology/core#Project\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_grantedBy != null)
			{
			foreach(var item0 in this.Roh_grantedBy)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/OrganizationAux>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/OrganizationAux\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://w3id.org/roh/grantedBy", $"<{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{item0.IdVcard_hasCountryName}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{item0.IdRoh_organizationType}>", list, " . ");
				}
				if(item0.IdRoh_organization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{item0.IdRoh_organization}>", list, " . ");
				}
				if(item0.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{item0.IdVcard_hasRegion}>", list, " . ");
				}
				if(item0.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(item0.Vcard_locality)}\"", list, " . ");
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther)}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle)}\"", list, " . ");
				}
			}
			}
			if(this.Roh_participates != null)
			{
			foreach(var item0 in this.Roh_participates)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/OrganizationAux>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/OrganizationAux\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://w3id.org/roh/participates", $"<{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{item0.IdVcard_hasCountryName}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{item0.IdRoh_organizationType}>", list, " . ");
				}
				if(item0.IdRoh_organization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{item0.IdRoh_organization}>", list, " . ");
				}
				if(item0.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{item0.IdVcard_hasRegion}>", list, " . ");
				}
				if(item0.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(item0.Vcard_locality)}\"", list, " . ");
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther)}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/OrganizationAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle)}\"", list, " . ");
				}
			}
			}
			if(this.Roh_mainResearchers != null)
			{
			foreach(var item0 in this.Roh_mainResearchers)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/PersonAux>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/PersonAux\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://w3id.org/roh/mainResearchers", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRdf_member != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#member", $"<{item0.IdRdf_member}>", list, " . ");
				}
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName)}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName)}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName)}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.Value.ToString()}", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick)}\"", list, " . ");
				}
			}
			}
			if(this.Roh_researchers != null)
			{
			foreach(var item0 in this.Roh_researchers)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/PersonAux>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/PersonAux\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://w3id.org/roh/researchers", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRdf_member != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#member", $"<{item0.IdRdf_member}>", list, " . ");
				}
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName)}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName)}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName)}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.Value.ToString()}", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick)}\"", list, " . ");
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://w3id.org/roh/hasKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode",  $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Vivo_relates != null)
			{
			foreach(var item0 in this.Vivo_relates)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://purl.obolibrary.org/obo/BFO_0000023>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://purl.obolibrary.org/obo/BFO_0000023\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#relates", $"<{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{item0.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.Value.ToString()}", list, " . ");
				}
				if(item0.Vivo_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://vivoweb.org/ontology/core#end", $"\"{item0.Vivo_end.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick)}\"", list, " . ");
				}
				if(item0.IdRoh_roleOf != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/roleOf", $"<{item0.IdRoh_roleOf}>", list, " . ");
				}
				if(item0.Roh_isIP != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/isIP", $"\"{item0.Roh_isIP.ToString()}\"", list, " . ");
				}
			}
			}
				if(this.IdRoh_conductedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedByType", $"<{this.IdRoh_conductedByType}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdsRoh_membersProject != null)
				{
					foreach(var item2 in this.IdsRoh_membersProject)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://w3id.org/roh/membersProject", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdsRoh_isProducedBy != null)
				{
					foreach(var item2 in this.IdsRoh_isProducedBy)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}", "http://w3id.org/roh/isProducedBy", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdVivo_geographicFocus != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#geographicFocus", $"<{this.IdVivo_geographicFocus}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdRoh_conductedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedBy", $"<{this.IdRoh_conductedBy}>", list, " . ");
				}
				if(this.IdRoh_projectAuthorization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/projectAuthorization", $"<{this.IdRoh_projectAuthorization}>", list, " . ");
				}
				if(this.IdRoh_projectType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/projectType", $"<{this.IdRoh_projectType}>", list, " . ");
				}
				if(this.IdRoh_modality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/modality", $"<{this.IdRoh_modality}>", list, " . ");
				}
				if(this.Roh_mixedPercentage != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/mixedPercentage", $"{this.Roh_mixedPercentage.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_isSupportedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isSupportedBy", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_isSupportedBy)}\"", list, " . ");
				}
				if(this.Roh_relevantResults != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/relevantResults", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_relevantResults)}\"", list, " . ");
				}
				if(this.Roh_creditPercentage != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/creditPercentage", $"{this.Roh_creditPercentage.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_peopleYearNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/peopleYearNumber", $"{this.Roh_peopleYearNumber.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_validationStatusProject != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/validationStatusProject", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_validationStatusProject)}\"", list, " . ");
				}
				if(this.Roh_yearEnd != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/yearEnd", $"{this.Roh_yearEnd.Value.ToString()}", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther)}\"", list, " . ");
				}
				if(this.Roh_publicationsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicationsNumber", $"{this.Roh_publicationsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays)}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Roh_conductedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTitle)}\"", list, " . ");
				}
				if(this.Roh_grantsPercentage != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/grantsPercentage", $"{this.Roh_grantsPercentage.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Vivo_description != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#description", $"\"{GenerarTextoSinSaltoDeLinea(this.Vivo_description)}\"", list, " . ");
				}
				if(this.Roh_subProjectMonetaryAmount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/subProjectMonetaryAmount", $"{this.Roh_subProjectMonetaryAmount.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_themedAreasNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/themedAreasNumber", $"{this.Roh_themedAreasNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_yearStart != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/yearStart", $"{this.Roh_yearStart.Value.ToString()}", list, " . ");
				}
				if(this.Roh_projectCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/projectCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_projectCode)}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths)}\"", list, " . ");
				}
				if(this.Roh_researchersNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/researchersNumber", $"{this.Roh_researchersNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_conductedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTypeOther)}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#end", $"\"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_collaborative != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/collaborative", $"\"{this.Roh_collaborative.ToString()}\"", list, " . ");
				}
				if(this.Roh_monetaryAmount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/monetaryAmount", $"{this.Roh_monetaryAmount.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_collaboratorsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/collaboratorsNumber", $"{this.Roh_collaboratorsNumber.Value.ToString()}", list, " . ");
				}
				if(this.IdRoh_scientificExperienceProject != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/scientificExperienceProject", $"<{this.IdRoh_scientificExperienceProject}>", list, " . ");
				}
				if(this.Roh_isValidated != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isValidated", $"\"{this.Roh_isValidated.ToString()}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Project_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"project\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://vivoweb.org/ontology/core#Project\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_grantedBy != null)
			{
			foreach(var item0 in this.Roh_grantedBy)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/grantedBy", $"<{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdVcard_hasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdVcard_hasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_organizationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdRoh_organization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_organization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdVcard_hasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdVcard_hasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(item0.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Roh_participates != null)
			{
			foreach(var item0 in this.Roh_participates)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/participates", $"<{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdVcard_hasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdVcard_hasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_organizationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdRoh_organization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_organization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdVcard_hasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdVcard_hasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(item0.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organizationaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Roh_mainResearchers != null)
			{
			foreach(var item0 in this.Roh_mainResearchers)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/mainResearchers", $"<{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRdf_member != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRdf_member;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#member", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName).ToLower()}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.Value.ToString()}", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Roh_researchers != null)
			{
			foreach(var item0 in this.Roh_researchers)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/researchers", $"<{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRdf_member != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRdf_member;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#member", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Foaf_familyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/familyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_familyName).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_secondFamilyName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/secondFamilyName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_secondFamilyName).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_firstName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_firstName).ToLower()}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.Value.ToString()}", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/personaux_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick).ToLower()}\"", list, " . ");
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
			if(this.Vivo_relates != null)
			{
			foreach(var item0 in this.Vivo_relates)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://vivoweb.org/ontology/core#relates", $"<{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://vivoweb.org/ontology/core#start", $"{item0.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.Value.ToString()}", list, " . ");
				}
				if(item0.Vivo_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://vivoweb.org/ontology/core#end", $"{item0.Vivo_end.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick).ToLower()}\"", list, " . ");
				}
				if(item0.IdRoh_roleOf != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_roleOf;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/roleOf", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Roh_isIP != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/isIP", $"\"{item0.Roh_isIP.ToString().ToLower()}\"", list, " . ");
				}
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
				if(this.IdsRoh_membersProject != null)
				{
					foreach(var item2 in this.IdsRoh_membersProject)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/membersProject", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdsRoh_isProducedBy != null)
				{
					foreach(var item2 in this.IdsRoh_isProducedBy)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/isProducedBy", $"<{itemRegex}>", list, " . ");
					}
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
				if(this.IdRoh_projectAuthorization != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_projectAuthorization;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/projectAuthorization", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_projectType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_projectType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/projectType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_modality != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_modality;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/modality", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_mixedPercentage != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/mixedPercentage", $"{this.Roh_mixedPercentage.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_isSupportedBy != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isSupportedBy", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_isSupportedBy).ToLower()}\"", list, " . ");
				}
				if(this.Roh_relevantResults != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/relevantResults", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_relevantResults).ToLower()}\"", list, " . ");
				}
				if(this.Roh_creditPercentage != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/creditPercentage", $"{this.Roh_creditPercentage.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_peopleYearNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/peopleYearNumber", $"{this.Roh_peopleYearNumber.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_validationStatusProject != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/validationStatusProject", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_validationStatusProject).ToLower()}\"", list, " . ");
				}
				if(this.Roh_yearEnd != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/yearEnd", $"{this.Roh_yearEnd.Value.ToString()}", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_publicationsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicationsNumber", $"{this.Roh_publicationsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationDays", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationDays).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#start", $"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_conductedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/conductedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_grantsPercentage != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/grantsPercentage", $"{this.Roh_grantsPercentage.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Vivo_description != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#description", $"\"{GenerarTextoSinSaltoDeLinea(this.Vivo_description).ToLower()}\"", list, " . ");
				}
				if(this.Roh_subProjectMonetaryAmount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/subProjectMonetaryAmount", $"{this.Roh_subProjectMonetaryAmount.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_themedAreasNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/themedAreasNumber", $"{this.Roh_themedAreasNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_yearStart != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/yearStart", $"{this.Roh_yearStart.Value.ToString()}", list, " . ");
				}
				if(this.Roh_projectCode != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/projectCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_projectCode).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationMonths", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationMonths).ToLower()}\"", list, " . ");
				}
				if(this.Roh_researchersNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/researchersNumber", $"{this.Roh_researchersNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_conductedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/conductedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationYears", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_durationYears).ToLower()}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#end", $"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_collaborative != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/collaborative", $"\"{this.Roh_collaborative.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_monetaryAmount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/monetaryAmount", $"{this.Roh_monetaryAmount.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_collaboratorsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/collaboratorsNumber", $"{this.Roh_collaboratorsNumber.Value.ToString()}", list, " . ");
				}
				if(this.IdRoh_scientificExperienceProject != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_scientificExperienceProject;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/scientificExperienceProject", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_isValidated != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isValidated", $"\"{this.Roh_isValidated.ToString().ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/ProjectOntology_{ResourceID}_{ArticleID}";
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
