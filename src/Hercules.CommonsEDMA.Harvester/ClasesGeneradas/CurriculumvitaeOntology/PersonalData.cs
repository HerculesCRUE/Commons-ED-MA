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
using Gender = GenderOntology.Gender;
using Feature = FeatureOntology.Feature;

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class PersonalData : GnossOCBase
	{

		public PersonalData() : base() { } 

		public PersonalData(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_hasFax = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasFax");
			if(propRoh_hasFax != null && propRoh_hasFax.PropertyValues.Count > 0)
			{
				this.Roh_hasFax = new TelephoneType(propRoh_hasFax.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_birthplace = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/birthplace");
			if(propRoh_birthplace != null && propRoh_birthplace.PropertyValues.Count > 0)
			{
				this.Roh_birthplace = new Address(propRoh_birthplace.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_otherIds = new List<Document>();
			SemanticPropertyModel propRoh_otherIds = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/otherIds");
			if(propRoh_otherIds != null && propRoh_otherIds.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_otherIds.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document roh_otherIds = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_otherIds.Add(roh_otherIds);
					}
				}
			}
			SemanticPropertyModel propVcard_address = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#address");
			if(propVcard_address != null && propVcard_address.PropertyValues.Count > 0)
			{
				this.Vcard_address = new Address(propVcard_address.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propFoaf_gender = pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/gender");
			if(propFoaf_gender != null && propFoaf_gender.PropertyValues.Count > 0)
			{
				this.Foaf_gender = new Gender(propFoaf_gender.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propSchema_nationality = pSemCmsModel.GetPropertyByPath("http://www.schema.org/nationality");
			if(propSchema_nationality != null && propSchema_nationality.PropertyValues.Count > 0)
			{
				this.Schema_nationality = new Feature(propSchema_nationality.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasTelephone = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasTelephone");
			if(propVcard_hasTelephone != null && propVcard_hasTelephone.PropertyValues.Count > 0)
			{
				this.Vcard_hasTelephone = new TelephoneType(propVcard_hasTelephone.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_hasMobilePhone = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasMobilePhone");
			if(propRoh_hasMobilePhone != null && propRoh_hasMobilePhone.PropertyValues.Count > 0)
			{
				this.Roh_hasMobilePhone = new TelephoneType(propRoh_hasMobilePhone.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Vivo_scopusId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#scopusId"));
			this.Foaf_familyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/familyName"));
			this.Vcard_email = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#email"));
			this.Roh_dni = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dni"));
			this.Roh_passport = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/passport"));
			this.Roh_ORCID = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/ORCID"));
			this.Roh_nie = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/nie"));
			this.Vivo_researcherId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#researcherId"));
			this.Roh_secondFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/secondFamilyName"));
			this.Foaf_img = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/img"));
			this.Foaf_homepage = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/homepage"));
			this.Vcard_birth_date = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#birth-date"));
			this.Foaf_firstName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/firstName"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/PersonalData"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/PersonalData"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/hasFax")]
		public  TelephoneType Roh_hasFax { get; set;}

		[RDFProperty("http://w3id.org/roh/birthplace")]
		public  Address Roh_birthplace { get; set;}

		[RDFProperty("http://w3id.org/roh/otherIds")]
		public  List<Document> Roh_otherIds { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#address")]
		public  Address Vcard_address { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/gender")]
		public  Gender Foaf_gender  { get; set;} 
		public string IdFoaf_gender  { get; set;} 

		[RDFProperty("http://www.schema.org/nationality")]
		public  Feature Schema_nationality  { get; set;} 
		public string IdSchema_nationality  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasTelephone")]
		public  TelephoneType Vcard_hasTelephone { get; set;}

		[RDFProperty("http://w3id.org/roh/hasMobilePhone")]
		public  TelephoneType Roh_hasMobilePhone { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#scopusId")]
		public  string Vivo_scopusId { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/familyName")]
		public  string Foaf_familyName { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#email")]
		public  string Vcard_email { get; set;}

		[RDFProperty("http://w3id.org/roh/dni")]
		public  string Roh_dni { get; set;}

		[RDFProperty("http://w3id.org/roh/passport")]
		public  string Roh_passport { get; set;}

		[RDFProperty("http://w3id.org/roh/ORCID")]
		public  string Roh_ORCID { get; set;}

		[RDFProperty("http://w3id.org/roh/nie")]
		public  string Roh_nie { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#researcherId")]
		public  string Vivo_researcherId { get; set;}

		[RDFProperty("http://w3id.org/roh/secondFamilyName")]
		public  string Roh_secondFamilyName { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/img")]
		public  string Foaf_img { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/homepage")]
		public  string Foaf_homepage { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#birth-date")]
		public  DateTime? Vcard_birth_date { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/firstName")]
		public  string Foaf_firstName { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("foaf:gender", this.IdFoaf_gender));
			propList.Add(new StringOntologyProperty("schema:nationality", this.IdSchema_nationality));
			propList.Add(new StringOntologyProperty("vivo:scopusId", this.Vivo_scopusId));
			propList.Add(new StringOntologyProperty("foaf:familyName", this.Foaf_familyName));
			propList.Add(new StringOntologyProperty("vcard:email", this.Vcard_email));
			propList.Add(new StringOntologyProperty("roh:dni", this.Roh_dni));
			propList.Add(new StringOntologyProperty("roh:passport", this.Roh_passport));
			propList.Add(new StringOntologyProperty("roh:ORCID", this.Roh_ORCID));
			propList.Add(new StringOntologyProperty("roh:nie", this.Roh_nie));
			propList.Add(new StringOntologyProperty("vivo:researcherId", this.Vivo_researcherId));
			propList.Add(new StringOntologyProperty("roh:secondFamilyName", this.Roh_secondFamilyName));
			propList.Add(new StringOntologyProperty("foaf:img", this.Foaf_img));
			propList.Add(new StringOntologyProperty("foaf:homepage", this.Foaf_homepage));
			if (this.Vcard_birth_date.HasValue){
				propList.Add(new DateOntologyProperty("vcard:birth-date", this.Vcard_birth_date.Value));
				}
			propList.Add(new StringOntologyProperty("foaf:firstName", this.Foaf_firstName));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_hasFax!=null){
				Roh_hasFax.GetProperties();
				Roh_hasFax.GetEntities();
				OntologyEntity entityRoh_hasFax = new OntologyEntity("https://www.w3.org/2006/vcard/ns#TelephoneType", "https://www.w3.org/2006/vcard/ns#TelephoneType", "roh:hasFax", Roh_hasFax.propList, Roh_hasFax.entList);
				entList.Add(entityRoh_hasFax);
			}
			if(Roh_birthplace!=null){
				Roh_birthplace.GetProperties();
				Roh_birthplace.GetEntities();
				OntologyEntity entityRoh_birthplace = new OntologyEntity("https://www.w3.org/2006/vcard/ns#Address", "https://www.w3.org/2006/vcard/ns#Address", "roh:birthplace", Roh_birthplace.propList, Roh_birthplace.entList);
				entList.Add(entityRoh_birthplace);
			}
			if(Roh_otherIds!=null){
				foreach(Document prop in Roh_otherIds){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityDocument = new OntologyEntity("http://xmlns.com/foaf/0.1/Document", "http://xmlns.com/foaf/0.1/Document", "roh:otherIds", prop.propList, prop.entList);
				entList.Add(entityDocument);
				prop.Entity= entityDocument;
				}
			}
			if(Vcard_address!=null){
				Vcard_address.GetProperties();
				Vcard_address.GetEntities();
				OntologyEntity entityVcard_address = new OntologyEntity("https://www.w3.org/2006/vcard/ns#Address", "https://www.w3.org/2006/vcard/ns#Address", "vcard:address", Vcard_address.propList, Vcard_address.entList);
				entList.Add(entityVcard_address);
			}
			if(Vcard_hasTelephone!=null){
				Vcard_hasTelephone.GetProperties();
				Vcard_hasTelephone.GetEntities();
				OntologyEntity entityVcard_hasTelephone = new OntologyEntity("https://www.w3.org/2006/vcard/ns#TelephoneType", "https://www.w3.org/2006/vcard/ns#TelephoneType", "vcard:hasTelephone", Vcard_hasTelephone.propList, Vcard_hasTelephone.entList);
				entList.Add(entityVcard_hasTelephone);
			}
			if(Roh_hasMobilePhone!=null){
				Roh_hasMobilePhone.GetProperties();
				Roh_hasMobilePhone.GetEntities();
				OntologyEntity entityRoh_hasMobilePhone = new OntologyEntity("https://www.w3.org/2006/vcard/ns#TelephoneType", "https://www.w3.org/2006/vcard/ns#TelephoneType", "roh:hasMobilePhone", Roh_hasMobilePhone.propList, Roh_hasMobilePhone.entList);
				entList.Add(entityRoh_hasMobilePhone);
			}
		} 











	}
}
