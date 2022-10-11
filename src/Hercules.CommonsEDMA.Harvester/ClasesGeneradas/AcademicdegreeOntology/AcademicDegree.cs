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
using DegreeType = DegreetypeOntology.DegreeType;
using DoctoralProgramType = DoctoralprogramtypeOntology.DoctoralProgramType;
using QualificationType = QualificationtypeOntology.QualificationType;
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using Feature = FeatureOntology.Feature;
using FormationType = FormationtypeOntology.FormationType;
using PrizeType = PrizetypeOntology.PrizeType;
using StayGoal = StaygoalOntology.StayGoal;
using PostgradeDegree = PostgradedegreeOntology.PostgradeDegree;
using Organization = OrganizationOntology.Organization;
using FormationActivityType = FormationactivitytypeOntology.FormationActivityType;
using UniversityDegreeType = UniversitydegreetypeOntology.UniversityDegreeType;
using Person = PersonOntology.Person;

namespace AcademicdegreeOntology
{
	[ExcludeFromCodeCoverage]
	public class AcademicDegree : GnossOCBase
	{

		public AcademicDegree() : base() { } 

		public AcademicDegree(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_foreignDegreeType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/foreignDegreeType");
			if(propRoh_foreignDegreeType != null && propRoh_foreignDegreeType.PropertyValues.Count > 0)
			{
				this.Roh_foreignDegreeType = new DegreeType(propRoh_foreignDegreeType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_doctoralProgram = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/doctoralProgram");
			if(propRoh_doctoralProgram != null && propRoh_doctoralProgram.PropertyValues.Count > 0)
			{
				this.Roh_doctoralProgram = new DoctoralProgramType(propRoh_doctoralProgram.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_mark = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/mark");
			if(propRoh_mark != null && propRoh_mark.PropertyValues.Count > 0)
			{
				this.Roh_mark = new QualificationType(propRoh_mark.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_formationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/formationType");
			if(propRoh_formationType != null && propRoh_formationType.PropertyValues.Count > 0)
			{
				this.Roh_formationType = new FormationType(propRoh_formationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_degreeType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/degreeType");
			if(propRoh_degreeType != null && propRoh_degreeType.PropertyValues.Count > 0)
			{
				this.Roh_degreeType = new DegreeType(propRoh_degreeType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_prize = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/prize");
			if(propRoh_prize != null && propRoh_prize.PropertyValues.Count > 0)
			{
				this.Roh_prize = new PrizeType(propRoh_prize.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_stayGoal = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/stayGoal");
			if(propRoh_stayGoal != null && propRoh_stayGoal.PropertyValues.Count > 0)
			{
				this.Roh_stayGoal = new StayGoal(propRoh_stayGoal.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_postgradeDegree = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/postgradeDegree");
			if(propRoh_postgradeDegree != null && propRoh_postgradeDegree.PropertyValues.Count > 0)
			{
				this.Roh_postgradeDegree = new PostgradeDegree(propRoh_postgradeDegree.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_conductedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedBy");
			if(propRoh_conductedBy != null && propRoh_conductedBy.PropertyValues.Count > 0)
			{
				this.Roh_conductedBy = new Organization(propRoh_conductedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_formationActivityType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/formationActivityType");
			if(propRoh_formationActivityType != null && propRoh_formationActivityType.PropertyValues.Count > 0)
			{
				this.Roh_formationActivityType = new FormationActivityType(propRoh_formationActivityType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_deaEntity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/deaEntity");
			if(propRoh_deaEntity != null && propRoh_deaEntity.PropertyValues.Count > 0)
			{
				this.Roh_deaEntity = new Organization(propRoh_deaEntity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_universityDegreeType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/universityDegreeType");
			if(propRoh_universityDegreeType != null && propRoh_universityDegreeType.PropertyValues.Count > 0)
			{
				this.Roh_universityDegreeType = new UniversityDegreeType(propRoh_universityDegreeType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_thesisTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/thesisTitle"));
			this.Roh_trainerSecondSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trainerSecondSurname"));
			this.Roh_directorNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/directorNick"));
			this.Roh_doctorExtraordinaryAwardDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/doctorExtraordinaryAwardDate"));
			this.Roh_directorSecondSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/directorSecondSurname"));
			this.Roh_durationDays = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_trainerNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trainerNick"));
			this.Roh_targetProfile = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/targetProfile"));
			this.Roh_formationActivityTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/formationActivityTypeOther"));
			this.Roh_europeanDoctorateDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanDoctorateDate"));
			this.Roh_goals = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/goals"));
			this.Roh_directorName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/directorName"));
			this.Roh_trainerName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trainerName"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_performedTasks = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/performedTasks"));
			this.Roh_prizeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/prizeOther"));
			this.Roh_europeanDoctorate= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanDoctorate"));
			this.Roh_conductedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTitle"));
			this.Roh_approvedDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/approvedDate"));
			this.Roh_trainerFirstSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trainerFirstSurname"));
			this.Roh_qualification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualification"));
			this.Roh_universityDegreeTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/universityDegreeTypeOther"));
			this.Roh_deaDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/deaDate"));
			this.Roh_durationMonths = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_stayGoalOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/stayGoalOther"));
			this.Roh_conductedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTypeOther"));
			this.Roh_fundingProgram = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundingProgram"));
			this.Roh_durationYears = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_doctorExtraordinaryAward= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/doctorExtraordinaryAward"));
			this.Roh_directorFirstSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/directorFirstSurname"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Roh_center = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/center"));
			this.Roh_qualityMention= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualityMention"));
			this.Roh_deaEntityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/deaEntityTitle"));
			this.Roh_durationHours = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationHours"));
			this.Roh_approvedDegree= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/approvedDegree"));
			this.Roh_foreignTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/foreignTitle"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public AcademicDegree(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_foreignDegreeType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/foreignDegreeType");
			if(propRoh_foreignDegreeType != null && propRoh_foreignDegreeType.PropertyValues.Count > 0)
			{
				this.Roh_foreignDegreeType = new DegreeType(propRoh_foreignDegreeType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_doctoralProgram = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/doctoralProgram");
			if(propRoh_doctoralProgram != null && propRoh_doctoralProgram.PropertyValues.Count > 0)
			{
				this.Roh_doctoralProgram = new DoctoralProgramType(propRoh_doctoralProgram.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_mark = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/mark");
			if(propRoh_mark != null && propRoh_mark.PropertyValues.Count > 0)
			{
				this.Roh_mark = new QualificationType(propRoh_mark.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_formationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/formationType");
			if(propRoh_formationType != null && propRoh_formationType.PropertyValues.Count > 0)
			{
				this.Roh_formationType = new FormationType(propRoh_formationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_degreeType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/degreeType");
			if(propRoh_degreeType != null && propRoh_degreeType.PropertyValues.Count > 0)
			{
				this.Roh_degreeType = new DegreeType(propRoh_degreeType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_prize = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/prize");
			if(propRoh_prize != null && propRoh_prize.PropertyValues.Count > 0)
			{
				this.Roh_prize = new PrizeType(propRoh_prize.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_stayGoal = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/stayGoal");
			if(propRoh_stayGoal != null && propRoh_stayGoal.PropertyValues.Count > 0)
			{
				this.Roh_stayGoal = new StayGoal(propRoh_stayGoal.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_postgradeDegree = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/postgradeDegree");
			if(propRoh_postgradeDegree != null && propRoh_postgradeDegree.PropertyValues.Count > 0)
			{
				this.Roh_postgradeDegree = new PostgradeDegree(propRoh_postgradeDegree.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_conductedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedBy");
			if(propRoh_conductedBy != null && propRoh_conductedBy.PropertyValues.Count > 0)
			{
				this.Roh_conductedBy = new Organization(propRoh_conductedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_formationActivityType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/formationActivityType");
			if(propRoh_formationActivityType != null && propRoh_formationActivityType.PropertyValues.Count > 0)
			{
				this.Roh_formationActivityType = new FormationActivityType(propRoh_formationActivityType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_deaEntity = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/deaEntity");
			if(propRoh_deaEntity != null && propRoh_deaEntity.PropertyValues.Count > 0)
			{
				this.Roh_deaEntity = new Organization(propRoh_deaEntity.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_universityDegreeType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/universityDegreeType");
			if(propRoh_universityDegreeType != null && propRoh_universityDegreeType.PropertyValues.Count > 0)
			{
				this.Roh_universityDegreeType = new UniversityDegreeType(propRoh_universityDegreeType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_thesisTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/thesisTitle"));
			this.Roh_trainerSecondSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trainerSecondSurname"));
			this.Roh_directorNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/directorNick"));
			this.Roh_doctorExtraordinaryAwardDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/doctorExtraordinaryAwardDate"));
			this.Roh_directorSecondSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/directorSecondSurname"));
			this.Roh_durationDays = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_trainerNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trainerNick"));
			this.Roh_targetProfile = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/targetProfile"));
			this.Roh_formationActivityTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/formationActivityTypeOther"));
			this.Roh_europeanDoctorateDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanDoctorateDate"));
			this.Roh_goals = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/goals"));
			this.Roh_directorName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/directorName"));
			this.Roh_trainerName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trainerName"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_performedTasks = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/performedTasks"));
			this.Roh_prizeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/prizeOther"));
			this.Roh_europeanDoctorate= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/europeanDoctorate"));
			this.Roh_conductedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTitle"));
			this.Roh_approvedDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/approvedDate"));
			this.Roh_trainerFirstSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/trainerFirstSurname"));
			this.Roh_qualification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualification"));
			this.Roh_universityDegreeTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/universityDegreeTypeOther"));
			this.Roh_deaDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/deaDate"));
			this.Roh_durationMonths = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_stayGoalOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/stayGoalOther"));
			this.Roh_conductedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/conductedByTypeOther"));
			this.Roh_fundingProgram = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundingProgram"));
			this.Roh_durationYears = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_doctorExtraordinaryAward= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/doctorExtraordinaryAward"));
			this.Roh_directorFirstSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/directorFirstSurname"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Roh_center = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/center"));
			this.Roh_qualityMention= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualityMention"));
			this.Roh_deaEntityTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/deaEntityTitle"));
			this.Roh_durationHours = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationHours"));
			this.Roh_approvedDegree= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/approvedDegree"));
			this.Roh_foreignTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/foreignTitle"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://vivoweb.org/ontology/core#AcademicDegree"; } }
		public virtual string RdfsLabel { get { return "http://vivoweb.org/ontology/core#AcademicDegree"; } }
		[RDFProperty("http://w3id.org/roh/foreignDegreeType")]
		public  DegreeType Roh_foreignDegreeType  { get; set;} 
		public string IdRoh_foreignDegreeType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/doctoralProgram")]
		public  DoctoralProgramType Roh_doctoralProgram  { get; set;} 
		public string IdRoh_doctoralProgram  { get; set;} 

		[RDFProperty("http://w3id.org/roh/mark")]
		public  QualificationType Roh_mark  { get; set;} 
		public string IdRoh_mark  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/conductedByType")]
		[RDFProperty("http://w3id.org/roh/conductedByType")]
		public  OrganizationType Roh_conductedByType  { get; set;} 
		public string IdRoh_conductedByType  { get; set;} 

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#hasCountryName")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/formationType")]
		public  FormationType Roh_formationType  { get; set;} 
		public string IdRoh_formationType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/degreeType")]
		public  DegreeType Roh_degreeType  { get; set;} 
		public string IdRoh_degreeType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/prize")]
		public  PrizeType Roh_prize  { get; set;} 
		public string IdRoh_prize  { get; set;} 

		[RDFProperty("http://w3id.org/roh/stayGoal")]
		public  StayGoal Roh_stayGoal  { get; set;} 
		public string IdRoh_stayGoal  { get; set;} 

		[RDFProperty("http://w3id.org/roh/postgradeDegree")]
		public  PostgradeDegree Roh_postgradeDegree  { get; set;} 
		public string IdRoh_postgradeDegree  { get; set;} 

		[RDFProperty("http://w3id.org/roh/codirector")]
		public  List<PersonAux> Roh_codirector { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/conductedBy")]
		[RDFProperty("http://w3id.org/roh/conductedBy")]
		public  Organization Roh_conductedBy  { get; set;} 
		public string IdRoh_conductedBy  { get; set;} 

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#hasRegion")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/formationActivityType")]
		public  FormationActivityType Roh_formationActivityType  { get; set;} 
		public string IdRoh_formationActivityType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/deaEntity")]
		public  Organization Roh_deaEntity  { get; set;} 
		public string IdRoh_deaEntity  { get; set;} 

		[RDFProperty("http://w3id.org/roh/universityDegreeType")]
		public  UniversityDegreeType Roh_universityDegreeType  { get; set;} 
		public string IdRoh_universityDegreeType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/thesisTitle")]
		public  string Roh_thesisTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/trainerSecondSurname")]
		public  string Roh_trainerSecondSurname { get; set;}

		[RDFProperty("http://w3id.org/roh/directorNick")]
		public  string Roh_directorNick { get; set;}

		[RDFProperty("http://w3id.org/roh/doctorExtraordinaryAwardDate")]
		public  DateTime? Roh_doctorExtraordinaryAwardDate { get; set;}

		[RDFProperty("http://w3id.org/roh/directorSecondSurname")]
		public  string Roh_directorSecondSurname { get; set;}

		[RDFProperty("http://w3id.org/roh/durationDays")]
		public  int? Roh_durationDays { get; set;}

		[RDFProperty("http://w3id.org/roh/trainerNick")]
		public  string Roh_trainerNick { get; set;}

		[RDFProperty("http://w3id.org/roh/targetProfile")]
		public  string Roh_targetProfile { get; set;}

		[RDFProperty("http://w3id.org/roh/formationActivityTypeOther")]
		public  string Roh_formationActivityTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/europeanDoctorateDate")]
		public  DateTime? Roh_europeanDoctorateDate { get; set;}

		[RDFProperty("http://w3id.org/roh/goals")]
		public  string Roh_goals { get; set;}

		[RDFProperty("http://w3id.org/roh/directorName")]
		public  string Roh_directorName { get; set;}

		[RDFProperty("http://w3id.org/roh/trainerName")]
		public  string Roh_trainerName { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/performedTasks")]
		public  string Roh_performedTasks { get; set;}

		[RDFProperty("http://w3id.org/roh/prizeOther")]
		public  string Roh_prizeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/europeanDoctorate")]
		public  bool Roh_europeanDoctorate { get; set;}

		[RDFProperty("http://w3id.org/roh/conductedByTitle")]
		public  string Roh_conductedByTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/approvedDate")]
		public  DateTime? Roh_approvedDate { get; set;}

		[RDFProperty("http://w3id.org/roh/trainerFirstSurname")]
		public  string Roh_trainerFirstSurname { get; set;}

		[RDFProperty("http://w3id.org/roh/qualification")]
		public  string Roh_qualification { get; set;}

		[RDFProperty("http://w3id.org/roh/universityDegreeTypeOther")]
		public  string Roh_universityDegreeTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/deaDate")]
		public  DateTime? Roh_deaDate { get; set;}

		[RDFProperty("http://w3id.org/roh/durationMonths")]
		public  int? Roh_durationMonths { get; set;}

		[RDFProperty("http://w3id.org/roh/stayGoalOther")]
		public  string Roh_stayGoalOther { get; set;}

		[RDFProperty("http://w3id.org/roh/conductedByTypeOther")]
		public  string Roh_conductedByTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/fundingProgram")]
		public  string Roh_fundingProgram { get; set;}

		[RDFProperty("http://w3id.org/roh/durationYears")]
		public  int? Roh_durationYears { get; set;}

		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime? Dct_issued { get; set;}

		[RDFProperty("http://w3id.org/roh/doctorExtraordinaryAward")]
		public  bool Roh_doctorExtraordinaryAward { get; set;}

		[RDFProperty("http://w3id.org/roh/directorFirstSurname")]
		public  string Roh_directorFirstSurname { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#end")]
		public  DateTime? Vivo_end { get; set;}

		[RDFProperty("http://w3id.org/roh/center")]
		public  string Roh_center { get; set;}

		[RDFProperty("http://w3id.org/roh/qualityMention")]
		public  bool Roh_qualityMention { get; set;}

		[RDFProperty("http://w3id.org/roh/deaEntityTitle")]
		public  string Roh_deaEntityTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/durationHours")]
		public  int? Roh_durationHours { get; set;}

		[RDFProperty("http://w3id.org/roh/approvedDegree")]
		public  bool Roh_approvedDegree { get; set;}

		[RDFProperty("http://w3id.org/roh/foreignTitle")]
		public  string Roh_foreignTitle { get; set;}

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
			propList.Add(new StringOntologyProperty("roh:foreignDegreeType", this.IdRoh_foreignDegreeType));
			propList.Add(new StringOntologyProperty("roh:doctoralProgram", this.IdRoh_doctoralProgram));
			propList.Add(new StringOntologyProperty("roh:mark", this.IdRoh_mark));
			propList.Add(new StringOntologyProperty("roh:conductedByType", this.IdRoh_conductedByType));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:formationType", this.IdRoh_formationType));
			propList.Add(new StringOntologyProperty("roh:degreeType", this.IdRoh_degreeType));
			propList.Add(new StringOntologyProperty("roh:prize", this.IdRoh_prize));
			propList.Add(new StringOntologyProperty("roh:stayGoal", this.IdRoh_stayGoal));
			propList.Add(new StringOntologyProperty("roh:postgradeDegree", this.IdRoh_postgradeDegree));
			propList.Add(new StringOntologyProperty("roh:conductedBy", this.IdRoh_conductedBy));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("roh:formationActivityType", this.IdRoh_formationActivityType));
			propList.Add(new StringOntologyProperty("roh:deaEntity", this.IdRoh_deaEntity));
			propList.Add(new StringOntologyProperty("roh:universityDegreeType", this.IdRoh_universityDegreeType));
			propList.Add(new StringOntologyProperty("roh:thesisTitle", this.Roh_thesisTitle));
			propList.Add(new StringOntologyProperty("roh:trainerSecondSurname", this.Roh_trainerSecondSurname));
			propList.Add(new StringOntologyProperty("roh:directorNick", this.Roh_directorNick));
			if (this.Roh_doctorExtraordinaryAwardDate.HasValue){
				propList.Add(new DateOntologyProperty("roh:doctorExtraordinaryAwardDate", this.Roh_doctorExtraordinaryAwardDate.Value));
				}
			propList.Add(new StringOntologyProperty("roh:directorSecondSurname", this.Roh_directorSecondSurname));
			propList.Add(new StringOntologyProperty("roh:durationDays", this.Roh_durationDays.ToString()));
			propList.Add(new StringOntologyProperty("roh:trainerNick", this.Roh_trainerNick));
			propList.Add(new StringOntologyProperty("roh:targetProfile", this.Roh_targetProfile));
			propList.Add(new StringOntologyProperty("roh:formationActivityTypeOther", this.Roh_formationActivityTypeOther));
			if (this.Roh_europeanDoctorateDate.HasValue){
				propList.Add(new DateOntologyProperty("roh:europeanDoctorateDate", this.Roh_europeanDoctorateDate.Value));
				}
			propList.Add(new StringOntologyProperty("roh:goals", this.Roh_goals));
			propList.Add(new StringOntologyProperty("roh:directorName", this.Roh_directorName));
			propList.Add(new StringOntologyProperty("roh:trainerName", this.Roh_trainerName));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("roh:performedTasks", this.Roh_performedTasks));
			propList.Add(new StringOntologyProperty("roh:prizeOther", this.Roh_prizeOther));
			propList.Add(new BoolOntologyProperty("roh:europeanDoctorate", this.Roh_europeanDoctorate));
			propList.Add(new StringOntologyProperty("roh:conductedByTitle", this.Roh_conductedByTitle));
			if (this.Roh_approvedDate.HasValue){
				propList.Add(new DateOntologyProperty("roh:approvedDate", this.Roh_approvedDate.Value));
				}
			propList.Add(new StringOntologyProperty("roh:trainerFirstSurname", this.Roh_trainerFirstSurname));
			propList.Add(new StringOntologyProperty("roh:qualification", this.Roh_qualification));
			propList.Add(new StringOntologyProperty("roh:universityDegreeTypeOther", this.Roh_universityDegreeTypeOther));
			if (this.Roh_deaDate.HasValue){
				propList.Add(new DateOntologyProperty("roh:deaDate", this.Roh_deaDate.Value));
				}
			propList.Add(new StringOntologyProperty("roh:durationMonths", this.Roh_durationMonths.ToString()));
			propList.Add(new StringOntologyProperty("roh:stayGoalOther", this.Roh_stayGoalOther));
			propList.Add(new StringOntologyProperty("roh:conductedByTypeOther", this.Roh_conductedByTypeOther));
			propList.Add(new StringOntologyProperty("roh:fundingProgram", this.Roh_fundingProgram));
			propList.Add(new StringOntologyProperty("roh:durationYears", this.Roh_durationYears.ToString()));
			if (this.Dct_issued.HasValue){
				propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued.Value));
				}
			propList.Add(new BoolOntologyProperty("roh:doctorExtraordinaryAward", this.Roh_doctorExtraordinaryAward));
			propList.Add(new StringOntologyProperty("roh:directorFirstSurname", this.Roh_directorFirstSurname));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			if (this.Vivo_end.HasValue){
				propList.Add(new DateOntologyProperty("vivo:end", this.Vivo_end.Value));
				}
			propList.Add(new StringOntologyProperty("roh:center", this.Roh_center));
			propList.Add(new BoolOntologyProperty("roh:qualityMention", this.Roh_qualityMention));
			propList.Add(new StringOntologyProperty("roh:deaEntityTitle", this.Roh_deaEntityTitle));
			propList.Add(new StringOntologyProperty("roh:durationHours", this.Roh_durationHours.ToString()));
			propList.Add(new BoolOntologyProperty("roh:approvedDegree", this.Roh_approvedDegree));
			propList.Add(new StringOntologyProperty("roh:foreignTitle", this.Roh_foreignTitle));
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://vivoweb.org/ontology/core#AcademicDegree>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://vivoweb.org/ontology/core#AcademicDegree\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_codirector != null)
			{
			foreach(var item0 in this.Roh_codirector)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/PersonAux>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/PersonAux\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}", "http://w3id.org/roh/codirector", $"<{resourceAPI.GraphsUrl}items/PersonAux_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
				if(this.IdRoh_foreignDegreeType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/foreignDegreeType", $"<{this.IdRoh_foreignDegreeType}>", list, " . ");
				}
				if(this.IdRoh_doctoralProgram != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/doctoralProgram", $"<{this.IdRoh_doctoralProgram}>", list, " . ");
				}
				if(this.IdRoh_mark != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/mark", $"<{this.IdRoh_mark}>", list, " . ");
				}
				if(this.IdRoh_conductedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedByType", $"<{this.IdRoh_conductedByType}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_formationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/formationType", $"<{this.IdRoh_formationType}>", list, " . ");
				}
				if(this.IdRoh_degreeType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/degreeType", $"<{this.IdRoh_degreeType}>", list, " . ");
				}
				if(this.IdRoh_prize != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/prize", $"<{this.IdRoh_prize}>", list, " . ");
				}
				if(this.IdRoh_stayGoal != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/stayGoal", $"<{this.IdRoh_stayGoal}>", list, " . ");
				}
				if(this.IdRoh_postgradeDegree != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/postgradeDegree", $"<{this.IdRoh_postgradeDegree}>", list, " . ");
				}
				if(this.IdRoh_conductedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedBy", $"<{this.IdRoh_conductedBy}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdRoh_formationActivityType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/formationActivityType", $"<{this.IdRoh_formationActivityType}>", list, " . ");
				}
				if(this.IdRoh_deaEntity != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/deaEntity", $"<{this.IdRoh_deaEntity}>", list, " . ");
				}
				if(this.IdRoh_universityDegreeType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/universityDegreeType", $"<{this.IdRoh_universityDegreeType}>", list, " . ");
				}
				if(this.Roh_thesisTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/thesisTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_thesisTitle)}\"", list, " . ");
				}
				if(this.Roh_trainerSecondSurname != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/trainerSecondSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_trainerSecondSurname)}\"", list, " . ");
				}
				if(this.Roh_directorNick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/directorNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_directorNick)}\"", list, " . ");
				}
				if(this.Roh_doctorExtraordinaryAwardDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/doctorExtraordinaryAwardDate", $"\"{this.Roh_doctorExtraordinaryAwardDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_directorSecondSurname != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/directorSecondSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_directorSecondSurname)}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationDays", $"{this.Roh_durationDays.Value.ToString()}", list, " . ");
				}
				if(this.Roh_trainerNick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/trainerNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_trainerNick)}\"", list, " . ");
				}
				if(this.Roh_targetProfile != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/targetProfile", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_targetProfile)}\"", list, " . ");
				}
				if(this.Roh_formationActivityTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/formationActivityTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_formationActivityTypeOther)}\"", list, " . ");
				}
				if(this.Roh_europeanDoctorateDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/europeanDoctorateDate", $"\"{this.Roh_europeanDoctorateDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_goals != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/goals", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_goals)}\"", list, " . ");
				}
				if(this.Roh_directorName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/directorName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_directorName)}\"", list, " . ");
				}
				if(this.Roh_trainerName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/trainerName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_trainerName)}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Roh_performedTasks != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/performedTasks", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_performedTasks)}\"", list, " . ");
				}
				if(this.Roh_prizeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/prizeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_prizeOther)}\"", list, " . ");
				}
				if(this.Roh_europeanDoctorate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/europeanDoctorate", $"\"{this.Roh_europeanDoctorate.ToString()}\"", list, " . ");
				}
				if(this.Roh_conductedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTitle)}\"", list, " . ");
				}
				if(this.Roh_approvedDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/approvedDate", $"\"{this.Roh_approvedDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_trainerFirstSurname != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/trainerFirstSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_trainerFirstSurname)}\"", list, " . ");
				}
				if(this.Roh_qualification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/qualification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualification)}\"", list, " . ");
				}
				if(this.Roh_universityDegreeTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/universityDegreeTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_universityDegreeTypeOther)}\"", list, " . ");
				}
				if(this.Roh_deaDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/deaDate", $"\"{this.Roh_deaDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationMonths", $"{this.Roh_durationMonths.Value.ToString()}", list, " . ");
				}
				if(this.Roh_stayGoalOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/stayGoalOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_stayGoalOther)}\"", list, " . ");
				}
				if(this.Roh_conductedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/conductedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTypeOther)}\"", list, " . ");
				}
				if(this.Roh_fundingProgram != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/fundingProgram", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_fundingProgram)}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationYears", $"{this.Roh_durationYears.Value.ToString()}", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_doctorExtraordinaryAward != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/doctorExtraordinaryAward", $"\"{this.Roh_doctorExtraordinaryAward.ToString()}\"", list, " . ");
				}
				if(this.Roh_directorFirstSurname != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/directorFirstSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_directorFirstSurname)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#end", $"\"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_center != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/center", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_center)}\"", list, " . ");
				}
				if(this.Roh_qualityMention != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/qualityMention", $"\"{this.Roh_qualityMention.ToString()}\"", list, " . ");
				}
				if(this.Roh_deaEntityTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/deaEntityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_deaEntityTitle)}\"", list, " . ");
				}
				if(this.Roh_durationHours != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationHours", $"{this.Roh_durationHours.Value.ToString()}", list, " . ");
				}
				if(this.Roh_approvedDegree != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/approvedDegree", $"\"{this.Roh_approvedDegree.ToString()}\"", list, " . ");
				}
				if(this.Roh_foreignTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/foreignTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_foreignTitle)}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/AcademicDegree_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"academicdegree\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://vivoweb.org/ontology/core#AcademicDegree\"", list, " . ");
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
				if(this.IdRoh_foreignDegreeType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_foreignDegreeType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/foreignDegreeType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_doctoralProgram != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_doctoralProgram;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/doctoralProgram", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_mark != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_mark;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/mark", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_formationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_formationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/formationType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_degreeType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_degreeType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/degreeType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_prize != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_prize;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/prize", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_stayGoal != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_stayGoal;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/stayGoal", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_postgradeDegree != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_postgradeDegree;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/postgradeDegree", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_formationActivityType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_formationActivityType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/formationActivityType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_deaEntity != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_deaEntity;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/deaEntity", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_universityDegreeType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_universityDegreeType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/universityDegreeType", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_thesisTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/thesisTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_thesisTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_trainerSecondSurname != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/trainerSecondSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_trainerSecondSurname).ToLower()}\"", list, " . ");
				}
				if(this.Roh_directorNick != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/directorNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_directorNick).ToLower()}\"", list, " . ");
				}
				if(this.Roh_doctorExtraordinaryAwardDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/doctorExtraordinaryAwardDate", $"{this.Roh_doctorExtraordinaryAwardDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_directorSecondSurname != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/directorSecondSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_directorSecondSurname).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationDays", $"{this.Roh_durationDays.Value.ToString()}", list, " . ");
				}
				if(this.Roh_trainerNick != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/trainerNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_trainerNick).ToLower()}\"", list, " . ");
				}
				if(this.Roh_targetProfile != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/targetProfile", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_targetProfile).ToLower()}\"", list, " . ");
				}
				if(this.Roh_formationActivityTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/formationActivityTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_formationActivityTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_europeanDoctorateDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/europeanDoctorateDate", $"{this.Roh_europeanDoctorateDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_goals != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/goals", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_goals).ToLower()}\"", list, " . ");
				}
				if(this.Roh_directorName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/directorName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_directorName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_trainerName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/trainerName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_trainerName).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_performedTasks != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/performedTasks", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_performedTasks).ToLower()}\"", list, " . ");
				}
				if(this.Roh_prizeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/prizeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_prizeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_europeanDoctorate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/europeanDoctorate", $"\"{this.Roh_europeanDoctorate.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_conductedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/conductedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_approvedDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/approvedDate", $"{this.Roh_approvedDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_trainerFirstSurname != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/trainerFirstSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_trainerFirstSurname).ToLower()}\"", list, " . ");
				}
				if(this.Roh_qualification != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/qualification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualification).ToLower()}\"", list, " . ");
				}
				if(this.Roh_universityDegreeTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/universityDegreeTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_universityDegreeTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_deaDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/deaDate", $"{this.Roh_deaDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationMonths", $"{this.Roh_durationMonths.Value.ToString()}", list, " . ");
				}
				if(this.Roh_stayGoalOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/stayGoalOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_stayGoalOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_conductedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/conductedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_conductedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_fundingProgram != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/fundingProgram", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_fundingProgram).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationYears", $"{this.Roh_durationYears.Value.ToString()}", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/terms/issued", $"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_doctorExtraordinaryAward != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/doctorExtraordinaryAward", $"\"{this.Roh_doctorExtraordinaryAward.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_directorFirstSurname != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/directorFirstSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_directorFirstSurname).ToLower()}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#end", $"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_center != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/center", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_center).ToLower()}\"", list, " . ");
				}
				if(this.Roh_qualityMention != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/qualityMention", $"\"{this.Roh_qualityMention.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_deaEntityTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/deaEntityTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_deaEntityTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationHours != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationHours", $"{this.Roh_durationHours.Value.ToString()}", list, " . ");
				}
				if(this.Roh_approvedDegree != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/approvedDegree", $"\"{this.Roh_approvedDegree.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_foreignTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/foreignTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_foreignTitle).ToLower()}\"", list, " . ");
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
				if(!string.IsNullOrEmpty(this.Roh_title))
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title).ToLower()}\"", list, " . ");
					search += $"{this.Roh_title} ";
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
			string titulo = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/AcademicdegreeOntology_{ResourceID}_{ArticleID}";
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
