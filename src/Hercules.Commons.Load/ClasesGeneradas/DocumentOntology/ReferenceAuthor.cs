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

namespace DocumentOntology
{
	[ExcludeFromCodeCoverage]
	public class ReferenceAuthor : GnossOCBase
	{

		public ReferenceAuthor() : base() { } 

		public ReferenceAuthor(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_semanticScholarId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/semanticScholarId"));
			this.Foaf_name = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/name"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ReferenceAuthor"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ReferenceAuthor"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/semanticScholarId")]
		public  string Roh_semanticScholarId { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/name")]
		public  string Foaf_name { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:semanticScholarId", this.Roh_semanticScholarId));
			propList.Add(new StringOntologyProperty("foaf:name", this.Foaf_name));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
