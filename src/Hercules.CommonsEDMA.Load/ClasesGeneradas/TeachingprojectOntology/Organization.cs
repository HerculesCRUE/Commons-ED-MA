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
using OrganizationType = OrganizationtypeOntology.OrganizationType;

namespace TeachingprojectOntology
{
	[ExcludeFromCodeCoverage]
	public class Organization : GnossOCBase
	{

		public Organization() : base() { } 

		public Organization(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_organization = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organization");
			if(propRoh_organization != null && propRoh_organization.PropertyValues.Count > 0)
			{
				this.Roh_organization = new Organization(propRoh_organization.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_organizationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organizationType");
			if(propRoh_organizationType != null && propRoh_organizationType.PropertyValues.Count > 0)
			{
				this.Roh_organizationType = new OrganizationType(propRoh_organizationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_organizationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organizationTypeOther"));
			this.Roh_organizationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/organizationTitle"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/Organization"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/Organization"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/organization")]
		public  Organization Roh_organization  { get; set;} 
		public string IdRoh_organization  { get; set;} 

		[RDFProperty("http://w3id.org/roh/organizationType")]
		public  OrganizationType Roh_organizationType  { get; set;} 
		public string IdRoh_organizationType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/organizationTypeOther")]
		public  string Roh_organizationTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/organizationTitle")]
		public  string Roh_organizationTitle { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:organization", this.IdRoh_organization));
			propList.Add(new StringOntologyProperty("roh:organizationType", this.IdRoh_organizationType));
			propList.Add(new StringOntologyProperty("roh:organizationTypeOther", this.Roh_organizationTypeOther));
			propList.Add(new StringOntologyProperty("roh:organizationTitle", this.Roh_organizationTitle));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
