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
using Feature = FeatureOntology.Feature;
using GeographicRegion = GeographicregionOntology.GeographicRegion;
using Organization = OrganizationOntology.Organization;
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using CallType = CalltypeOntology.CallType;
using DedicationRegime = DedicationregimeOntology.DedicationRegime;
using LaboralDurationType = LaboraldurationtypeOntology.LaboralDurationType;
using ParticipationTypeProject = ParticipationtypeprojectOntology.ParticipationTypeProject;
using Person = PersonOntology.Person;

namespace TeachingprojectOntology
{
	[ExcludeFromCodeCoverage]
	public class TeachingProject : GnossOCBase
	{

		public TeachingProject() : base() { } 

		public TeachingProject(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Roh_participates = new List<Organization>();
			SemanticPropertyModel propRoh_participates = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participates");
			if(propRoh_participates != null && propRoh_participates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_participates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_participates = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_participates.Add(roh_participates);
					}
				}
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new GeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_fundedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundedBy");
			if(propRoh_fundedBy != null && propRoh_fundedBy.PropertyValues.Count > 0)
			{
				this.Roh_fundedBy = new Organization(propRoh_fundedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_fundedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundedByType");
			if(propRoh_fundedByType != null && propRoh_fundedByType.PropertyValues.Count > 0)
			{
				this.Roh_fundedByType = new OrganizationType(propRoh_fundedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_callType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/callType");
			if(propRoh_callType != null && propRoh_callType.PropertyValues.Count > 0)
			{
				this.Roh_callType = new CallType(propRoh_callType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_dedication = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dedication");
			if(propRoh_dedication != null && propRoh_dedication.PropertyValues.Count > 0)
			{
				this.Roh_dedication = new DedicationRegime(propRoh_dedication.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_laboralDurationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/laboralDurationType");
			if(propRoh_laboralDurationType != null && propRoh_laboralDurationType.PropertyValues.Count > 0)
			{
				this.Roh_laboralDurationType = new LaboralDurationType(propRoh_laboralDurationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_participationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationType");
			if(propRoh_participationType != null && propRoh_participationType.PropertyValues.Count > 0)
			{
				this.Roh_participationType = new ParticipationTypeProject(propRoh_participationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_principalInvestigatorNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorNick"));
			this.Roh_durationMonths = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Roh_durationDays = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_fundedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundedByTitle"));
			this.Roh_participationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationTypeOther"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_fundedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundedByTypeOther"));
			this.Roh_principalInvestigatorFirstSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorFirstSurname"));
			this.Roh_contribution = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contribution"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_durationYears = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_principalInvestigatorName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorName"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			SemanticPropertyModel propRoh_cvnCode = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode");
			this.Roh_cvnCode = new List<string>();
			if (propRoh_cvnCode != null && propRoh_cvnCode.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_cvnCode.PropertyValues)
				{
					this.Roh_cvnCode.Add(propValue.Value);
				}
			}
			this.Roh_callTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/callTypeOther"));
			this.Roh_principalInvestigatorSecondSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorSecondSurname"));
			this.Roh_monetaryAmount = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/monetaryAmount"));
			this.Roh_participantsNumber = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participantsNumber"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public TeachingProject(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_participates = new List<Organization>();
			SemanticPropertyModel propRoh_participates = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participates");
			if(propRoh_participates != null && propRoh_participates.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_participates.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Organization roh_participates = new Organization(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_participates.Add(roh_participates);
					}
				}
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVivo_geographicFocus = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#geographicFocus");
			if(propVivo_geographicFocus != null && propVivo_geographicFocus.PropertyValues.Count > 0)
			{
				this.Vivo_geographicFocus = new GeographicRegion(propVivo_geographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_fundedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundedBy");
			if(propRoh_fundedBy != null && propRoh_fundedBy.PropertyValues.Count > 0)
			{
				this.Roh_fundedBy = new Organization(propRoh_fundedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_fundedByType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundedByType");
			if(propRoh_fundedByType != null && propRoh_fundedByType.PropertyValues.Count > 0)
			{
				this.Roh_fundedByType = new OrganizationType(propRoh_fundedByType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_callType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/callType");
			if(propRoh_callType != null && propRoh_callType.PropertyValues.Count > 0)
			{
				this.Roh_callType = new CallType(propRoh_callType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_dedication = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dedication");
			if(propRoh_dedication != null && propRoh_dedication.PropertyValues.Count > 0)
			{
				this.Roh_dedication = new DedicationRegime(propRoh_dedication.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_laboralDurationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/laboralDurationType");
			if(propRoh_laboralDurationType != null && propRoh_laboralDurationType.PropertyValues.Count > 0)
			{
				this.Roh_laboralDurationType = new LaboralDurationType(propRoh_laboralDurationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_participationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationType");
			if(propRoh_participationType != null && propRoh_participationType.PropertyValues.Count > 0)
			{
				this.Roh_participationType = new ParticipationTypeProject(propRoh_participationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_principalInvestigatorNick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorNick"));
			this.Roh_durationMonths = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationMonths"));
			this.Roh_geographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/geographicFocusOther"));
			this.Roh_durationDays = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationDays"));
			this.Roh_fundedByTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundedByTitle"));
			this.Roh_participationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationTypeOther"));
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Roh_fundedByTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/fundedByTypeOther"));
			this.Roh_principalInvestigatorFirstSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorFirstSurname"));
			this.Roh_contribution = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contribution"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Roh_durationYears = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/durationYears"));
			this.Roh_principalInvestigatorName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorName"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			SemanticPropertyModel propRoh_cvnCode = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/cvnCode");
			this.Roh_cvnCode = new List<string>();
			if (propRoh_cvnCode != null && propRoh_cvnCode.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_cvnCode.PropertyValues)
				{
					this.Roh_cvnCode.Add(propValue.Value);
				}
			}
			this.Roh_callTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/callTypeOther"));
			this.Roh_principalInvestigatorSecondSurname = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/principalInvestigatorSecondSurname"));
			this.Roh_monetaryAmount = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/monetaryAmount"));
			this.Roh_participantsNumber = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participantsNumber"));
			SemanticPropertyModel propRoh_owner = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/owner");
			if(propRoh_owner != null && propRoh_owner.PropertyValues.Count > 0)
			{
				this.Roh_owner = new Person(propRoh_owner.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/TeachingProject"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/TeachingProject"; } }
		[RDFProperty("http://w3id.org/roh/participates")]
		public  List<Organization> Roh_participates { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("http://vivoweb.org/ontology/core#geographicFocus")]
		public  GeographicRegion Vivo_geographicFocus  { get; set;} 
		public string IdVivo_geographicFocus  { get; set;} 

		[RDFProperty("http://w3id.org/roh/fundedBy")]
		public  Organization Roh_fundedBy  { get; set;} 
		public string IdRoh_fundedBy  { get; set;} 

		[RDFProperty("http://w3id.org/roh/fundedByType")]
		public  OrganizationType Roh_fundedByType  { get; set;} 
		public string IdRoh_fundedByType  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/callType")]
		public  CallType Roh_callType  { get; set;} 
		public string IdRoh_callType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/dedication")]
		public  DedicationRegime Roh_dedication  { get; set;} 
		public string IdRoh_dedication  { get; set;} 

		[RDFProperty("http://w3id.org/roh/laboralDurationType")]
		public  LaboralDurationType Roh_laboralDurationType  { get; set;} 
		public string IdRoh_laboralDurationType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/participationType")]
		public  ParticipationTypeProject Roh_participationType  { get; set;} 
		public string IdRoh_participationType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/principalInvestigatorNick")]
		public  string Roh_principalInvestigatorNick { get; set;}

		[RDFProperty("http://w3id.org/roh/durationMonths")]
		public  float? Roh_durationMonths { get; set;}

		[RDFProperty("http://w3id.org/roh/geographicFocusOther")]
		public  string Roh_geographicFocusOther { get; set;}

		[RDFProperty("http://w3id.org/roh/durationDays")]
		public  float? Roh_durationDays { get; set;}

		[RDFProperty("http://w3id.org/roh/fundedByTitle")]
		public  string Roh_fundedByTitle { get; set;}

		[RDFProperty("http://w3id.org/roh/participationTypeOther")]
		public  string Roh_participationTypeOther { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("http://w3id.org/roh/fundedByTypeOther")]
		public  string Roh_fundedByTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/principalInvestigatorFirstSurname")]
		public  string Roh_principalInvestigatorFirstSurname { get; set;}

		[RDFProperty("http://w3id.org/roh/contribution")]
		public  string Roh_contribution { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/durationYears")]
		public  float? Roh_durationYears { get; set;}

		[RDFProperty("http://w3id.org/roh/principalInvestigatorName")]
		public  string Roh_principalInvestigatorName { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#end")]
		public  DateTime? Vivo_end { get; set;}

		[RDFProperty("http://w3id.org/roh/cvnCode")]
		public  List<string> Roh_cvnCode { get; set;}

		[RDFProperty("http://w3id.org/roh/callTypeOther")]
		public  string Roh_callTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/principalInvestigatorSecondSurname")]
		public  string Roh_principalInvestigatorSecondSurname { get; set;}

		[RDFProperty("http://w3id.org/roh/monetaryAmount")]
		public  float? Roh_monetaryAmount { get; set;}

		[RDFProperty("http://w3id.org/roh/participantsNumber")]
		public  float? Roh_participantsNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/owner")]
		[Required]
		public  Person Roh_owner  { get; set;} 
		public string IdRoh_owner  { get; set;} 

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("vivo:geographicFocus", this.IdVivo_geographicFocus));
			propList.Add(new StringOntologyProperty("roh:fundedBy", this.IdRoh_fundedBy));
			propList.Add(new StringOntologyProperty("roh:fundedByType", this.IdRoh_fundedByType));
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:callType", this.IdRoh_callType));
			propList.Add(new StringOntologyProperty("roh:dedication", this.IdRoh_dedication));
			propList.Add(new StringOntologyProperty("roh:laboralDurationType", this.IdRoh_laboralDurationType));
			propList.Add(new StringOntologyProperty("roh:participationType", this.IdRoh_participationType));
			propList.Add(new StringOntologyProperty("roh:principalInvestigatorNick", this.Roh_principalInvestigatorNick));
			propList.Add(new StringOntologyProperty("roh:durationMonths", this.Roh_durationMonths.ToString()));
			propList.Add(new StringOntologyProperty("roh:geographicFocusOther", this.Roh_geographicFocusOther));
			propList.Add(new StringOntologyProperty("roh:durationDays", this.Roh_durationDays.ToString()));
			propList.Add(new StringOntologyProperty("roh:fundedByTitle", this.Roh_fundedByTitle));
			propList.Add(new StringOntologyProperty("roh:participationTypeOther", this.Roh_participationTypeOther));
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new StringOntologyProperty("roh:fundedByTypeOther", this.Roh_fundedByTypeOther));
			propList.Add(new StringOntologyProperty("roh:principalInvestigatorFirstSurname", this.Roh_principalInvestigatorFirstSurname));
			propList.Add(new StringOntologyProperty("roh:contribution", this.Roh_contribution));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("roh:durationYears", this.Roh_durationYears.ToString()));
			propList.Add(new StringOntologyProperty("roh:principalInvestigatorName", this.Roh_principalInvestigatorName));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			if (this.Vivo_end.HasValue){
				propList.Add(new DateOntologyProperty("vivo:end", this.Vivo_end.Value));
				}
			propList.Add(new ListStringOntologyProperty("roh:cvnCode", this.Roh_cvnCode));
			propList.Add(new StringOntologyProperty("roh:callTypeOther", this.Roh_callTypeOther));
			propList.Add(new StringOntologyProperty("roh:principalInvestigatorSecondSurname", this.Roh_principalInvestigatorSecondSurname));
			propList.Add(new StringOntologyProperty("roh:monetaryAmount", this.Roh_monetaryAmount.ToString()));
			propList.Add(new StringOntologyProperty("roh:participantsNumber", this.Roh_participantsNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_participates!=null){
				foreach(Organization prop in Roh_participates){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityOrganization = new OntologyEntity("http://w3id.org/roh/Organization", "http://w3id.org/roh/Organization", "roh:participates", prop.propList, prop.entList);
				entList.Add(entityOrganization);
				prop.Entity= entityOrganization;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/TeachingProject>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/TeachingProject\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_participates != null)
			{
			foreach(var item0 in this.Roh_participates)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Organization>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Organization\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/participates", $"<{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRoh_organization != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{item0.IdRoh_organization}>", list, " . ");
				}
				if(item0.IdRoh_organizationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{item0.IdRoh_organizationType}>", list, " . ");
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther)}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle)}\"", list, " . ");
				}
			}
			}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdVivo_geographicFocus != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#geographicFocus", $"<{this.IdVivo_geographicFocus}>", list, " . ");
				}
				if(this.IdRoh_fundedBy != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/fundedBy", $"<{this.IdRoh_fundedBy}>", list, " . ");
				}
				if(this.IdRoh_fundedByType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/fundedByType", $"<{this.IdRoh_fundedByType}>", list, " . ");
				}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_callType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/callType", $"<{this.IdRoh_callType}>", list, " . ");
				}
				if(this.IdRoh_dedication != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/dedication", $"<{this.IdRoh_dedication}>", list, " . ");
				}
				if(this.IdRoh_laboralDurationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/laboralDurationType", $"<{this.IdRoh_laboralDurationType}>", list, " . ");
				}
				if(this.IdRoh_participationType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/participationType", $"<{this.IdRoh_participationType}>", list, " . ");
				}
				if(this.Roh_principalInvestigatorNick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/principalInvestigatorNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorNick)}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationMonths", $"{this.Roh_durationMonths.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther)}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationDays", $"{this.Roh_durationDays.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_fundedByTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/fundedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_fundedByTitle)}\"", list, " . ");
				}
				if(this.Roh_participationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/participationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_participationTypeOther)}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#start", $"\"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_fundedByTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/fundedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_fundedByTypeOther)}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorFirstSurname != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/principalInvestigatorFirstSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorFirstSurname)}\"", list, " . ");
				}
				if(this.Roh_contribution != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/contribution", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_contribution)}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/durationYears", $"{this.Roh_durationYears.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_principalInvestigatorName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/principalInvestigatorName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorName)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#end", $"\"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					foreach(var item2 in this.Roh_cvnCode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_callTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/callTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_callTypeOther)}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorSecondSurname != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/principalInvestigatorSecondSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorSecondSurname)}\"", list, " . ");
				}
				if(this.Roh_monetaryAmount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/monetaryAmount", $"{this.Roh_monetaryAmount.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_participantsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/participantsNumber", $"{this.Roh_participantsNumber.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.IdRoh_owner != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/owner", $"<{this.IdRoh_owner}>", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/TeachingProject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"teachingproject\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/TeachingProject\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_participates != null)
			{
			foreach(var item0 in this.Roh_participates)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/participates", $"<{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organization", $"<{itemRegex}>", list, " . ");
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
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationType", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Roh_organizationTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTypeOther).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_organizationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/organization_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/organizationTitle", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_organizationTitle).ToLower()}\"", list, " . ");
				}
			}
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
				if(this.IdRoh_fundedBy != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_fundedBy;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/fundedBy", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_fundedByType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_fundedByType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/fundedByType", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_dedication != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_dedication;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/dedication", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_laboralDurationType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_laboralDurationType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/laboralDurationType", $"<{itemRegex}>", list, " . ");
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
				if(this.Roh_principalInvestigatorNick != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/principalInvestigatorNick", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorNick).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationMonths != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationMonths", $"{this.Roh_durationMonths.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_geographicFocusOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/geographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_geographicFocusOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationDays != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationDays", $"{this.Roh_durationDays.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_fundedByTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/fundedByTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_fundedByTitle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_participationTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/participationTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_participationTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_start != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#start", $"{this.Vivo_start.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_fundedByTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/fundedByTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_fundedByTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorFirstSurname != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/principalInvestigatorFirstSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorFirstSurname).ToLower()}\"", list, " . ");
				}
				if(this.Roh_contribution != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/contribution", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_contribution).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_durationYears != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/durationYears", $"{this.Roh_durationYears.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_principalInvestigatorName != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/principalInvestigatorName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorName).ToLower()}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_end != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#end", $"{this.Vivo_end.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_cvnCode != null)
				{
					foreach(var item2 in this.Roh_cvnCode)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/cvnCode", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_callTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/callTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_callTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_principalInvestigatorSecondSurname != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/principalInvestigatorSecondSurname", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_principalInvestigatorSecondSurname).ToLower()}\"", list, " . ");
				}
				if(this.Roh_monetaryAmount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/monetaryAmount", $"{this.Roh_monetaryAmount.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_participantsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/participantsNumber", $"{this.Roh_participantsNumber.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
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
			return $"{resourceAPI.GraphsUrl}items/TeachingprojectOntology_{ResourceID}_{ArticleID}";
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
