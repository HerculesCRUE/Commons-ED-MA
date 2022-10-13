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
using ModalityTeachingType = ModalityteachingtypeOntology.ModalityTeachingType;
using Language = LanguageOntology.Language;
using DegreeType = DegreetypeOntology.DegreeType;
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using EventGeographicRegion = EventgeographicregionOntology.EventGeographicRegion;
using CourseType = CoursetypeOntology.CourseType;
using CallType = CalltypeOntology.CallType;
using HoursCreditsECTSType = HourscreditsectstypeOntology.HoursCreditsECTSType;
using ProgramType = ProgramtypeOntology.ProgramType;
using TeachingType = TeachingtypeOntology.TeachingType;
using EvaluationType = EvaluationtypeOntology.EvaluationType;
using Person = PersonOntology.Person;

namespace ImpartedacademictrainingOntology
{
	[ExcludeFromCodeCoverage]
	public class ImpartedAcademicTraining : GnossOCBase
	{

		public ImpartedAcademicTraining() : base() { } 

		public ImpartedAcademicTraining(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			SemanticPropertyModel propRoh_promotedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedBy");
			if(propRoh_promotedBy != null && propRoh_promotedBy.PropertyValues.Count > 0)
			{
				this.Roh_promotedBy = new Organization(propRoh_promotedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_financedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedBy");
			if(propRoh_financedBy != null && propRoh_financedBy.PropertyValues.Count > 0)
			{
				this.Roh_financedBy = new Organization(propRoh_financedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_financedByHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByHasRegion");
			if(propRoh_financedByHasRegion != null && propRoh_financedByHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_financedByHasRegion = new Feature(propRoh_financedByHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_financedByHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByHasCountryName");
			if(propRoh_financedByHasCountryName != null && propRoh_financedByHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_financedByHasCountryName = new Feature(propRoh_financedByHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_modalityTeachingType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/modalityTeachingType");
			if(propRoh_modalityTeachingType != null && propRoh_modalityTeachingType.PropertyValues.Count > 0)
			{
				this.Roh_modalityTeachingType = new ModalityTeachingType(propRoh_modalityTeachingType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasLanguage = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasLanguage");
			if(propVcard_hasLanguage != null && propVcard_hasLanguage.PropertyValues.Count > 0)
			{
				this.Vcard_hasLanguage = new Language(propVcard_hasLanguage.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_degreeType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/degreeType");
			if(propRoh_degreeType != null && propRoh_degreeType.PropertyValues.Count > 0)
			{
				this.Roh_degreeType = new DegreeType(propRoh_degreeType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluatedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedBy");
			if(propRoh_evaluatedBy != null && propRoh_evaluatedBy.PropertyValues.Count > 0)
			{
				this.Roh_evaluatedBy = new Organization(propRoh_evaluatedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByType");
			if(propRoh_promotedByType != null && propRoh_promotedByType.PropertyValues.Count > 0)
			{
				this.Roh_promotedByType = new OrganizationType(propRoh_promotedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluatedByHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByHasCountryName");
			if(propRoh_evaluatedByHasCountryName != null && propRoh_evaluatedByHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_evaluatedByHasCountryName = new Feature(propRoh_evaluatedByHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluatedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByType");
			if(propRoh_evaluatedByType != null && propRoh_evaluatedByType.PropertyValues.Count > 0)
			{
				this.Roh_evaluatedByType = new OrganizationType(propRoh_evaluatedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluatedByHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByHasRegion");
			if(propRoh_evaluatedByHasRegion != null && propRoh_evaluatedByHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_evaluatedByHasRegion = new Feature(propRoh_evaluatedByHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new EventGeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_courseType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/courseType");
			if(propRoh_courseType != null && propRoh_courseType.PropertyValues.Count > 0)
			{
				this.Roh_courseType = new CourseType(propRoh_courseType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_callType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/callType");
			if(propRoh_callType != null && propRoh_callType.PropertyValues.Count > 0)
			{
				this.Roh_callType = new CallType(propRoh_callType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_hoursCreditsECTSType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hoursCreditsECTSType");
			if(propRoh_hoursCreditsECTSType != null && propRoh_hoursCreditsECTSType.PropertyValues.Count > 0)
			{
				this.Roh_hoursCreditsECTSType = new HoursCreditsECTSType(propRoh_hoursCreditsECTSType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_programType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/programType");
			if(propRoh_programType != null && propRoh_programType.PropertyValues.Count > 0)
			{
				this.Roh_programType = new ProgramType(propRoh_programType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_financedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByType");
			if(propRoh_financedByType != null && propRoh_financedByType.PropertyValues.Count > 0)
			{
				this.Roh_financedByType = new OrganizationType(propRoh_financedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_teachingType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingType");
			if(propRoh_teachingType != null && propRoh_teachingType.PropertyValues.Count > 0)
			{
				this.Roh_teachingType = new TeachingType(propRoh_teachingType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluationType");
			if(propRoh_evaluationType != null && propRoh_evaluationType.PropertyValues.Count > 0)
			{
				this.Roh_evaluationType = new EvaluationType(propRoh_evaluationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_maxQualification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/maxQualification"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_frequency = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/frequency"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_competencies = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/competencies"));
			this.Roh_financedByLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByLocality"));
			this.Roh_promotedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTitle"));
			this.Roh_callTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/callTypeOther"));
			this.Roh_modalityTeachingTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/modalityTeachingTypeOther"));
			this.Roh_evaluatedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByTypeOther"));
			this.Roh_financedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByTitle"));
			this.Roh_teaches = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teaches"));
			this.Roh_qualification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualification"));
			this.Roh_numberECTSHours = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/numberECTSHours"));
			this.Roh_professionalCategory = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/professionalCategory"));
			this.Roh_courseTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/courseTypeOther"));
			this.Roh_evaluationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluationTypeOther"));
			this.Roh_evaluatedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByTitle"));
			this.Roh_promotedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTypeOther"));
			this.Roh_evaluatedByLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByLocality"));
			this.Roh_financedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByTypeOther"));
			this.Roh_course = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/course"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Roh_center = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/center"));
			this.Roh_programTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/programTypeOther"));
			this.Roh_department = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/department"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public ImpartedAcademicTraining(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_promotedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedBy");
			if(propRoh_promotedBy != null && propRoh_promotedBy.PropertyValues.Count > 0)
			{
				this.Roh_promotedBy = new Organization(propRoh_promotedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_financedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedBy");
			if(propRoh_financedBy != null && propRoh_financedBy.PropertyValues.Count > 0)
			{
				this.Roh_financedBy = new Organization(propRoh_financedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_financedByHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByHasRegion");
			if(propRoh_financedByHasRegion != null && propRoh_financedByHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_financedByHasRegion = new Feature(propRoh_financedByHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_financedByHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByHasCountryName");
			if(propRoh_financedByHasCountryName != null && propRoh_financedByHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_financedByHasCountryName = new Feature(propRoh_financedByHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_modalityTeachingType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/modalityTeachingType");
			if(propRoh_modalityTeachingType != null && propRoh_modalityTeachingType.PropertyValues.Count > 0)
			{
				this.Roh_modalityTeachingType = new ModalityTeachingType(propRoh_modalityTeachingType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasLanguage = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasLanguage");
			if(propVcard_hasLanguage != null && propVcard_hasLanguage.PropertyValues.Count > 0)
			{
				this.Vcard_hasLanguage = new Language(propVcard_hasLanguage.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_degreeType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/degreeType");
			if(propRoh_degreeType != null && propRoh_degreeType.PropertyValues.Count > 0)
			{
				this.Roh_degreeType = new DegreeType(propRoh_degreeType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluatedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedBy");
			if(propRoh_evaluatedBy != null && propRoh_evaluatedBy.PropertyValues.Count > 0)
			{
				this.Roh_evaluatedBy = new Organization(propRoh_evaluatedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_promotedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByType");
			if(propRoh_promotedByType != null && propRoh_promotedByType.PropertyValues.Count > 0)
			{
				this.Roh_promotedByType = new OrganizationType(propRoh_promotedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluatedByHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByHasCountryName");
			if(propRoh_evaluatedByHasCountryName != null && propRoh_evaluatedByHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_evaluatedByHasCountryName = new Feature(propRoh_evaluatedByHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluatedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByType");
			if(propRoh_evaluatedByType != null && propRoh_evaluatedByType.PropertyValues.Count > 0)
			{
				this.Roh_evaluatedByType = new OrganizationType(propRoh_evaluatedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluatedByHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByHasRegion");
			if(propRoh_evaluatedByHasRegion != null && propRoh_evaluatedByHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_evaluatedByHasRegion = new Feature(propRoh_evaluatedByHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new EventGeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_courseType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/courseType");
			if(propRoh_courseType != null && propRoh_courseType.PropertyValues.Count > 0)
			{
				this.Roh_courseType = new CourseType(propRoh_courseType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_callType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/callType");
			if(propRoh_callType != null && propRoh_callType.PropertyValues.Count > 0)
			{
				this.Roh_callType = new CallType(propRoh_callType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_hoursCreditsECTSType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hoursCreditsECTSType");
			if(propRoh_hoursCreditsECTSType != null && propRoh_hoursCreditsECTSType.PropertyValues.Count > 0)
			{
				this.Roh_hoursCreditsECTSType = new HoursCreditsECTSType(propRoh_hoursCreditsECTSType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_programType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/programType");
			if(propRoh_programType != null && propRoh_programType.PropertyValues.Count > 0)
			{
				this.Roh_programType = new ProgramType(propRoh_programType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_financedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByType");
			if(propRoh_financedByType != null && propRoh_financedByType.PropertyValues.Count > 0)
			{
				this.Roh_financedByType = new OrganizationType(propRoh_financedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_teachingType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teachingType");
			if(propRoh_teachingType != null && propRoh_teachingType.PropertyValues.Count > 0)
			{
				this.Roh_teachingType = new TeachingType(propRoh_teachingType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_evaluationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluationType");
			if(propRoh_evaluationType != null && propRoh_evaluationType.PropertyValues.Count > 0)
			{
				this.Roh_evaluationType = new EvaluationType(propRoh_evaluationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_maxQualification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/maxQualification"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_frequency = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/frequency"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_competencies = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/competencies"));
			this.Roh_financedByLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByLocality"));
			this.Roh_promotedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTitle"));
			this.Roh_callTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/callTypeOther"));
			this.Roh_modalityTeachingTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/modalityTeachingTypeOther"));
			this.Roh_evaluatedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByTypeOther"));
			this.Roh_financedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByTitle"));
			this.Roh_teaches = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/teaches"));
			this.Roh_qualification = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/qualification"));
			this.Roh_numberECTSHours = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/numberECTSHours"));
			this.Roh_professionalCategory = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/professionalCategory"));
			this.Roh_courseTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/courseTypeOther"));
			this.Roh_evaluationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluationTypeOther"));
			this.Roh_evaluatedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByTitle"));
			this.Roh_promotedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/promotedByTypeOther"));
			this.Roh_evaluatedByLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/evaluatedByLocality"));
			this.Roh_financedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/financedByTypeOther"));
			this.Roh_course = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/course"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Roh_center = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/center"));
			this.Roh_programTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/programTypeOther"));
			this.Roh_department = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/department"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_cvnCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ImpartedAcademicTraining"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ImpartedAcademicTraining"; } }
		[RDFProperty("http://w3id.org/roh/promotedBy")]
		public  Organization Roh_promotedBy  { get; set;} 
		public string IdRoh_promotedBy  { get; set;} 

		[RDFProperty("http://w3id.org/roh/financedBy")]
		public  Organization Roh_financedBy  { get; set;} 
		public string IdRoh_financedBy  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/financedByHasRegion")]
		public  Feature Roh_financedByHasRegion  { get; set;} 
		public string IdRoh_financedByHasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/financedByHasCountryName")]
		public  Feature Roh_financedByHasCountryName  { get; set;} 
		public string IdRoh_financedByHasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/modalityTeachingType")]
		public  ModalityTeachingType Roh_modalityTeachingType  { get; set;} 
		public string IdRoh_modalityTeachingType  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasLanguage")]
		public  Language Vcard_hasLanguage  { get; set;} 
		public string IdVcard_hasLanguage  { get; set;} 

		[RDFProperty("http://w3id.org/roh/degreeType")]
		public  DegreeType Roh_degreeType  { get; set;} 
		public string IdRoh_degreeType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/evaluatedBy")]
		public  Organization Roh_evaluatedBy  { get; set;} 
		public string IdRoh_evaluatedBy  { get; set;} 

		[RDFProperty("http://w3id.org/roh/promotedByType")]
		public  OrganizationType Roh_promotedByType  { get; set;} 
		public string IdRoh_promotedByType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/evaluatedByHasCountryName")]
		public  Feature Roh_evaluatedByHasCountryName  { get; set;} 
		public string IdRoh_evaluatedByHasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/evaluatedByType")]
		public  OrganizationType Roh_evaluatedByType  { get; set;} 
		public string IdRoh_evaluatedByType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/evaluatedByHasRegion")]
		public  Feature Roh_evaluatedByHasRegion  { get; set;} 
		public string IdRoh_evaluatedByHasRegion  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("http://vivoweb.org/ontology/core#geographicFocus")]
		public  EventGeographicRegion Vivo_geographicFocus  { get; set;} 
		public string IdVivo_geographicFocus  { get; set;} 

		[RDFProperty("http://w3id.org/roh/courseType")]
		public  CourseType Roh_courseType  { get; set;} 
		public string IdRoh_courseType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/callType")]
		public  CallType Roh_callType  { get; set;} 
		public string IdRoh_callType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/hoursCreditsECTSType")]
		public  HoursCreditsECTSType Roh_hoursCreditsECTSType  { get; set;} 
		public string IdRoh_hoursCreditsECTSType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/programType")]
		public  ProgramType Roh_programType  { get; set;} 
		public string IdRoh_programType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/financedByType")]
		public  OrganizationType Roh_financedByType  { get; set;} 
		public string IdRoh_financedByType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/teachingType")]
		public  TeachingType Roh_teachingType  { get; set;} 
		public string IdRoh_teachingType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/evaluationType")]
		public  EvaluationType Roh_evaluationType  { get; set;} 
		public string IdRoh_evaluationType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/maxQualification")]
		public  string Roh_maxQualification { get; set;}

		[RDFProperty("http://w3id.org/roh/geographicFocusOther")]
		public  string Roh_geographicFocusOther { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("http://w3id.org/roh/frequency")]
		public  float? Roh_frequency { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/competencies")]
		public  string Roh_competencies { get; set;}

		[RDFProperty("http://w3id.org/roh/financedByLocality")]
		public  string Roh_financedByLocality { get; set;}

		[RDFProperty("http://w3id.org/roh/promotedByTitle")]
		public  string Roh_promotedByTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/callTypeOther")]
		public  string Roh_callTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/modalityTeachingTypeOther")]
		public  string Roh_modalityTeachingTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/evaluatedByTypeOther")]
		public  string Roh_evaluatedByTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/financedByTitle")]
		public  string Roh_financedByTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/teaches")]
		public  string Roh_teaches { get; set;}

		[RDFProperty("http://w3id.org/roh/qualification")]
		public  string Roh_qualification { get; set;}

		[RDFProperty("http://w3id.org/roh/numberECTSHours")]
		public  float? Roh_numberECTSHours { get; set;}

		[RDFProperty("http://w3id.org/roh/professionalCategory")]
		public  string Roh_professionalCategory { get; set;}

		[RDFProperty("http://w3id.org/roh/courseTypeOther")]
		public  string Roh_courseTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/evaluationTypeOther")]
		public  string Roh_evaluationTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/evaluatedByTitle")]
		public  string Roh_evaluatedByTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/promotedByTypeOther")]
		public  string Roh_promotedByTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/evaluatedByLocality")]
		public  string Roh_evaluatedByLocality { get; set;}

		[RDFProperty("http://w3id.org/roh/financedByTypeOther")]
		public  string Roh_financedByTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/course")]
		public  string Roh_course { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#end")]
		public  DateTime? Vivo_end { get; set;}

		[RDFProperty("http://w3id.org/roh/center")]
		public  string Roh_center { get; set;}

		[RDFProperty("http://w3id.org/roh/programTypeOther")]
		public  string Roh_programTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/department")]
		public  string Roh_department { get; set;}

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
			propList.Add(new StringOntologyProperty("roh:financedBy", this.IdRoh_financedBy));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:financedByHasRegion", this.IdRoh_financedByHasRegion));
			propList.Add(new StringOntologyProperty("roh:financedByHasCountryName", this.IdRoh_financedByHasCountryName));
			propList.Add(new StringOntologyProperty("roh:modalityTeachingType", this.IdRoh_modalityTeachingType));
			propList.Add(new StringOntologyProperty("vcard:hasLanguage", this.IdVcard_hasLanguage));
			propList.Add(new StringOntologyProperty("roh:degreeType", this.IdRoh_degreeType));
			propList.Add(new StringOntologyProperty("roh:evaluatedBy", this.IdRoh_evaluatedBy));
			propList.Add(new StringOntologyProperty("roh:promotedByType", this.IdRoh_promotedByType));
			propList.Add(new StringOntologyProperty("roh:evaluatedByHasCountryName", this.IdRoh_evaluatedByHasCountryName));
			propList.Add(new StringOntologyProperty("roh:evaluatedByType", this.IdRoh_evaluatedByType));
			propList.Add(new StringOntologyProperty("roh:evaluatedByHasRegion", this.IdRoh_evaluatedByHasRegion));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("vivo:geographicFocus", this.IdVivo_geographicFocus));
			propList.Add(new StringOntologyProperty("roh:courseType", this.IdRoh_courseType));
			propList.Add(new StringOntologyProperty("roh:callType", this.IdRoh_callType));
			propList.Add(new StringOntologyProperty("roh:hoursCreditsECTSType", this.IdRoh_hoursCreditsECTSType));
			propList.Add(new StringOntologyProperty("roh:programType", this.IdRoh_programType));
			propList.Add(new StringOntologyProperty("roh:financedByType", this.IdRoh_financedByType));
			propList.Add(new StringOntologyProperty("roh:teachingType", this.IdRoh_teachingType));
			propList.Add(new StringOntologyProperty("roh:evaluationType", this.IdRoh_evaluationType));
			propList.Add(new StringOntologyProperty("roh:maxQualification", this.Roh_maxQualification));
			propList.Add(new StringOntologyProperty("roh:geographicFocusOther", this.Roh_geographicFocusOther));
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new StringOntologyProperty("roh:frequency", this.Roh_frequency.ToString()));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("roh:competencies", this.Roh_competencies));
			propList.Add(new StringOntologyProperty("roh:financedByLocality", this.Roh_financedByLocality));
			propList.Add(new StringOntologyProperty("roh:promotedByTitle", this.Roh_promotedByTitle));
			propList.Add(new StringOntologyProperty("roh:callTypeOther", this.Roh_callTypeOther));
			propList.Add(new StringOntologyProperty("roh:modalityTeachingTypeOther", this.Roh_modalityTeachingTypeOther));
			propList.Add(new StringOntologyProperty("roh:evaluatedByTypeOther", this.Roh_evaluatedByTypeOther));
			propList.Add(new StringOntologyProperty("roh:financedByTitle", this.Roh_financedByTitle));
			propList.Add(new StringOntologyProperty("roh:teaches", this.Roh_teaches));
			propList.Add(new StringOntologyProperty("roh:qualification", this.Roh_qualification));
			propList.Add(new StringOntologyProperty("roh:numberECTSHours", this.Roh_numberECTSHours.ToString()));
			propList.Add(new StringOntologyProperty("roh:professionalCategory", this.Roh_professionalCategory));
			propList.Add(new StringOntologyProperty("roh:courseTypeOther", this.Roh_courseTypeOther));
			propList.Add(new StringOntologyProperty("roh:evaluationTypeOther", this.Roh_evaluationTypeOther));
			propList.Add(new StringOntologyProperty("roh:evaluatedByTitle", this.Roh_evaluatedByTitle));
			propList.Add(new StringOntologyProperty("roh:promotedByTypeOther", this.Roh_promotedByTypeOther));
			propList.Add(new StringOntologyProperty("roh:evaluatedByLocality", this.Roh_evaluatedByLocality));
			propList.Add(new StringOntologyProperty("roh:financedByTypeOther", this.Roh_financedByTypeOther));
			propList.Add(new StringOntologyProperty("roh:course", this.Roh_course));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			if (this.Vivo_end.HasValue){
				propList.Add(new DateOntologyProperty("vivo:end", this.Vivo_end.Value));
				}
			propList.Add(new StringOntologyProperty("roh:center", this.Roh_center));
			propList.Add(new StringOntologyProperty("roh:programTypeOther", this.Roh_programTypeOther));
			propList.Add(new StringOntologyProperty("roh:department", this.Roh_department));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology=null;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ImpartedAcademicTraining>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ImpartedAcademicTraining\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}>", list, " . ");
				if(this.IdRoh_promotedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedBy", $"<{this.IdRoh_promotedBy}>", list, " . ");
				}
				if(this.IdRoh_financedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/financedBy", $"<{this.IdRoh_financedBy}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_financedByHasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/financedByHasRegion", $"<{this.IdRoh_financedByHasRegion}>", list, " . ");
				}
				if(this.IdRoh_financedByHasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/financedByHasCountryName", $"<{this.IdRoh_financedByHasCountryName}>", list, " . ");
				}
				if(this.IdRoh_modalityTeachingType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/modalityTeachingType", $"<{this.IdRoh_modalityTeachingType}>", list, " . ");
				}
				if(this.IdVcard_hasLanguage != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasLanguage", $"<{this.IdVcard_hasLanguage}>", list, " . ");
				}
				if(this.IdRoh_degreeType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/degreeType", $"<{this.IdRoh_degreeType}>", list, " . ");
				}
				if(this.IdRoh_evaluatedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/evaluatedBy", $"<{this.IdRoh_evaluatedBy}>", list, " . ");
				}
				if(this.IdRoh_promotedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByType", $"<{this.IdRoh_promotedByType}>", list, " . ");
				}
				if(this.IdRoh_evaluatedByHasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/evaluatedByHasCountryName", $"<{this.IdRoh_evaluatedByHasCountryName}>", list, " . ");
				}
				if(this.IdRoh_evaluatedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/evaluatedByType", $"<{this.IdRoh_evaluatedByType}>", list, " . ");
				}
				if(this.IdRoh_evaluatedByHasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/evaluatedByHasRegion", $"<{this.IdRoh_evaluatedByHasRegion}>", list, " . ");
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdVivo_geographicFocus != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#geographicFocus", $"<{this.IdVivo_geographicFocus}>", list, " . ");
				}
				if(this.IdRoh_courseType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/courseType", $"<{this.IdRoh_courseType}>", list, " . ");
				}
				if(this.IdRoh_callType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/callType", $"<{this.IdRoh_callType}>", list, " . ");
				}
				if(this.IdRoh_hoursCreditsECTSType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/hoursCreditsECTSType", $"<{this.IdRoh_hoursCreditsECTSType}>", list, " . ");
				}
				if(this.IdRoh_programType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/programType", $"<{this.IdRoh_programType}>", list, " . ");
				}
				if(this.IdRoh_financedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/financedByType", $"<{this.IdRoh_financedByType}>", list, " . ");
				}
				if(this.IdRoh_teachingType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/teachingType", $"<{this.IdRoh_teachingType}>", list, " . ");
				}
				if(this.IdRoh_evaluationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/evaluationType", $"<{this.IdRoh_evaluationType}>", list, " . ");
				}
				if(this.Roh_maxQualification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/maxQualification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_maxQualification)}\"", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther)}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_frequency != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/frequency", $"{this.Roh_frequency.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Roh_competencies != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/competencies", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_competencies)}\"", list, " . ");
				}
				if(this.Roh_financedByLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/financedByLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_financedByLocality)}\"", list, " . ");
				}
				if(this.Roh_promotedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTitle)}\"", list, " . ");
				}
				if(this.Roh_callTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/callTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_callTypeOther)}\"", list, " . ");
				}
				if(this.Roh_modalityTeachingTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/modalityTeachingTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_modalityTeachingTypeOther)}\"", list, " . ");
				}
				if(this.Roh_evaluatedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/evaluatedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_evaluatedByTypeOther)}\"", list, " . ");
				}
				if(this.Roh_financedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/financedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_financedByTitle)}\"", list, " . ");
				}
				if(this.Roh_teaches != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/teaches", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_teaches)}\"", list, " . ");
				}
				if(this.Roh_qualification != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/qualification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualification)}\"", list, " . ");
				}
				if(this.Roh_numberECTSHours != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/numberECTSHours", $"{this.Roh_numberECTSHours.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_professionalCategory != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/professionalCategory", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_professionalCategory)}\"", list, " . ");
				}
				if(this.Roh_courseTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/courseTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_courseTypeOther)}\"", list, " . ");
				}
				if(this.Roh_evaluationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/evaluationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_evaluationTypeOther)}\"", list, " . ");
				}
				if(this.Roh_evaluatedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/evaluatedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_evaluatedByTitle)}\"", list, " . ");
				}
				if(this.Roh_promotedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/promotedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTypeOther)}\"", list, " . ");
				}
				if(this.Roh_evaluatedByLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/evaluatedByLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_evaluatedByLocality)}\"", list, " . ");
				}
				if(this.Roh_financedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/financedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_financedByTypeOther)}\"", list, " . ");
				}
				if(this.Roh_course != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/course", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_course)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#end", $"\"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_center != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/center", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_center)}\"", list, " . ");
				}
				if(this.Roh_programTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/programTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_programTypeOther)}\"", list, " . ");
				}
				if(this.Roh_department != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/department", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_department)}\"", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_cvnCode)}\"", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpartedAcademicTraining_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"impartedacademictraining\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/ImpartedAcademicTraining\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
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
				if(this.IdRoh_financedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_financedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/financedBy", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_financedByHasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_financedByHasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/financedByHasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_financedByHasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_financedByHasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/financedByHasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_modalityTeachingType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_modalityTeachingType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/modalityTeachingType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdVcard_hasLanguage != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVcard_hasLanguage;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#hasLanguage", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_evaluatedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_evaluatedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/evaluatedBy", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_evaluatedByHasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_evaluatedByHasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/evaluatedByHasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_evaluatedByType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_evaluatedByType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/evaluatedByType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_evaluatedByHasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_evaluatedByHasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/evaluatedByHasRegion", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_courseType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_courseType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/courseType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_callType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_callType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/callType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_hoursCreditsECTSType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_hoursCreditsECTSType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/hoursCreditsECTSType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_programType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_programType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/programType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_financedByType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_financedByType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/financedByType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_teachingType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_teachingType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/teachingType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_evaluationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_evaluationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/evaluationType", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_maxQualification != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/maxQualification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_maxQualification).ToLower()}\"", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#start", $"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_frequency != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/frequency", $"{this.Roh_frequency.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_competencies != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/competencies", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_competencies).ToLower()}\"", list, " . ");
				}
				if(this.Roh_financedByLocality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/financedByLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_financedByLocality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_promotedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_callTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/callTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_callTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_modalityTeachingTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/modalityTeachingTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_modalityTeachingTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_evaluatedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/evaluatedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_evaluatedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_financedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/financedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_financedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_teaches != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/teaches", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_teaches).ToLower()}\"", list, " . ");
				}
				if(this.Roh_qualification != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/qualification", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_qualification).ToLower()}\"", list, " . ");
				}
				if(this.Roh_numberECTSHours != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/numberECTSHours", $"{this.Roh_numberECTSHours.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_professionalCategory != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/professionalCategory", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_professionalCategory).ToLower()}\"", list, " . ");
				}
				if(this.Roh_courseTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/courseTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_courseTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_evaluationTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/evaluationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_evaluationTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_evaluatedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/evaluatedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_evaluatedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_promotedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/promotedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_promotedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_evaluatedByLocality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/evaluatedByLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_evaluatedByLocality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_financedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/financedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_financedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_course != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/course", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_course).ToLower()}\"", list, " . ");
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
				if(this.Roh_programTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/programTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_programTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_department != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/department", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_department).ToLower()}\"", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/ImpartedacademictrainingOntology_{ResourceID}_{ArticleID}";
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
