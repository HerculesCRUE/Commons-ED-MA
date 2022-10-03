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

namespace PositionOntology
{
	[ExcludeFromCodeCoverage]
	public class TelephoneType : GnossOCBase
	{

		public TelephoneType() : base() { } 

		public TelephoneType(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_hasExtension = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasExtension"));
			this.Roh_hasInternationalCode = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasInternationalCode"));
			this.Vcard_hasValue = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasValue"));
		}

		public virtual string RdfType { get { return "https://www.w3.org/2006/vcard/ns#TelephoneType"; } }
		public virtual string RdfsLabel { get { return "https://www.w3.org/2006/vcard/ns#TelephoneType"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/hasExtension")]
		public  string Roh_hasExtension { get; set;}

		[RDFProperty("http://w3id.org/roh/hasInternationalCode")]
		public  string Roh_hasInternationalCode { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasValue")]
		public  string Vcard_hasValue { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:hasExtension", this.Roh_hasExtension));
			propList.Add(new StringOntologyProperty("roh:hasInternationalCode", this.Roh_hasInternationalCode));
			propList.Add(new StringOntologyProperty("vcard:hasValue", this.Vcard_hasValue));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
