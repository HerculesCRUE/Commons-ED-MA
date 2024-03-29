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

namespace PersonOntology
{
	[ExcludeFromCodeCoverage]
	public class IgnorePublication : GnossOCBase
	{

		public IgnorePublication() : base() { } 

		public IgnorePublication(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
			this.Foaf_topic = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/topic"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/IgnorePublication"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/IgnorePublication"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/topic")]
		public  string Foaf_topic { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
			propList.Add(new StringOntologyProperty("foaf:topic", this.Foaf_topic));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
