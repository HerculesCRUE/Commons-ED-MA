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
	public class PublicationMetric : GnossOCBase
	{

		public PublicationMetric() : base() { } 

		public PublicationMetric(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_metricName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/metricName"));
			this.Roh_citationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/citationCount")).Value;
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/PublicationMetric"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/PublicationMetric"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/metricName")]
		public  string Roh_metricName { get; set;}

		[RDFProperty("http://w3id.org/roh/citationCount")]
		public  int Roh_citationCount { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:metricName", this.Roh_metricName));
			propList.Add(new StringOntologyProperty("roh:citationCount", this.Roh_citationCount.ToString()));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
