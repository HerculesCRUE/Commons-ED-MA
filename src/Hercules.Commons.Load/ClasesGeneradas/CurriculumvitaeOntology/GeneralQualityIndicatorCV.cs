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
	public class GeneralQualityIndicatorCV : GnossOCBase
	{

		public GeneralQualityIndicatorCV() : base() { } 

		public GeneralQualityIndicatorCV(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_generalQualityIndicator = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/generalQualityIndicator"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/GeneralQualityIndicatorCV"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/GeneralQualityIndicatorCV"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/generalQualityIndicator")]
		public  string Roh_generalQualityIndicator { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:generalQualityIndicator", this.Roh_generalQualityIndicator));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
