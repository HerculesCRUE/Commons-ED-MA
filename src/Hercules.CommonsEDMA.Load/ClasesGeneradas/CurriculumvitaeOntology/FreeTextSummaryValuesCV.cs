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
	public class FreeTextSummaryValuesCV : GnossOCBase
	{

		public FreeTextSummaryValuesCV() : base() { } 

		public FreeTextSummaryValuesCV(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_summaryTFG = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/summaryTFG"));
			this.Roh_summaryTFM = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/summaryTFM"));
			this.Roh_summary = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/summary"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/FreeTextSummaryValuesCV"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/FreeTextSummaryValuesCV"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/summaryTFG")]
		public  string Roh_summaryTFG { get; set;}

		[RDFProperty("http://w3id.org/roh/summaryTFM")]
		public  string Roh_summaryTFM { get; set;}

		[RDFProperty("http://w3id.org/roh/summary")]
		public  string Roh_summary { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:summaryTFG", this.Roh_summaryTFG));
			propList.Add(new StringOntologyProperty("roh:summaryTFM", this.Roh_summaryTFM));
			propList.Add(new StringOntologyProperty("roh:summary", this.Roh_summary));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
