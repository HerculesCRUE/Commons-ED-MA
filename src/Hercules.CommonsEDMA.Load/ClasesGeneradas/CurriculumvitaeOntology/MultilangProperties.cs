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
	public class MultilangProperties : GnossOCBase
	{

		public MultilangProperties() : base() { } 

		public MultilangProperties(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_lang = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/lang"));
			this.Roh_value = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/value"));
			this.Roh_property = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/property"));
			this.Roh_entity = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/entity"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/MultilangProperties"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/MultilangProperties"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/lang")]
		public  string Roh_lang { get; set;}

		[RDFProperty("http://w3id.org/roh/value")]
		public  string Roh_value { get; set;}

		[RDFProperty("http://w3id.org/roh/property")]
		public  string Roh_property { get; set;}

		[RDFProperty("http://w3id.org/roh/entity")]
		public  string Roh_entity { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:lang", this.Roh_lang));
			propList.Add(new StringOntologyProperty("roh:value", this.Roh_value));
			propList.Add(new StringOntologyProperty("roh:property", this.Roh_property));
			propList.Add(new StringOntologyProperty("roh:entity", this.Roh_entity));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
