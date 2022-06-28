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
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using Organization = OrganizationOntology.Organization;

namespace TechnologicalresultOntology
{
	[ExcludeFromCodeCoverage]
	public class Organization : GnossOCBase
	{

		public Organization() : base() { } 

		public Organization(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_organizationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organizationType");
			if(propRoh_organizationType != null && propRoh_organizationType.PropertyValues.Count > 0)
			{
				this.Roh_organizationType = new OrganizationType(propRoh_organizationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_organization = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organization");
			if(propRoh_organization != null && propRoh_organization.PropertyValues.Count > 0)
			{
				this.Roh_organization = new Organization(propRoh_organization.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propVcard_locality = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality");
			this.Vcard_locality = new List<string>();
			if (propVcard_locality != null && propVcard_locality.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVcard_locality.PropertyValues)
				{
					this.Vcard_locality.Add(propValue.Value);
				}
			}
			this.Roh_organizationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organizationTypeOther"));
			this.Roh_organizationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organizationTitle"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/Organization"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/Organization"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/organizationType")]
		public  OrganizationType Roh_organizationType  { get; set;} 
		public string IdRoh_organizationType  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/organization")]
		public  Organization Roh_organization  { get; set;} 
		public string IdRoh_organization  { get; set;} 

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  List<string> Vcard_locality { get; set;}

		[RDFProperty("http://w3id.org/roh/organizationTypeOther")]
		public  string Roh_organizationTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/organizationTitle")]
		public  string Roh_organizationTitle { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:organizationType", this.IdRoh_organizationType));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("roh:organization", this.IdRoh_organization));
			propList.Add(new ListStringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("roh:organizationTypeOther", this.Roh_organizationTypeOther));
			propList.Add(new StringOntologyProperty("roh:organizationTitle", this.Roh_organizationTitle));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
