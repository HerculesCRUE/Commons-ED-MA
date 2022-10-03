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
	public class FreeTextSummaryValues : GnossOCBase
	{

		public FreeTextSummaryValues() : base() { } 

		public FreeTextSummaryValues(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_freeTextSummaryValuesCV = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/freeTextSummaryValuesCV");
			if(propRoh_freeTextSummaryValuesCV != null && propRoh_freeTextSummaryValuesCV.PropertyValues.Count > 0)
			{
				this.Roh_freeTextSummaryValuesCV = new FreeTextSummaryValuesCV(propRoh_freeTextSummaryValuesCV.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/FreeTextSummaryValues"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/FreeTextSummaryValues"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/freeTextSummaryValuesCV")]
		public  FreeTextSummaryValuesCV Roh_freeTextSummaryValuesCV { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			Roh_freeTextSummaryValuesCV.GetProperties();
			Roh_freeTextSummaryValuesCV.GetEntities();
			OntologyEntity entityRoh_freeTextSummaryValuesCV = new OntologyEntity("http://w3id.org/roh/FreeTextSummaryValuesCV", "http://w3id.org/roh/FreeTextSummaryValuesCV", "roh:freeTextSummaryValuesCV", Roh_freeTextSummaryValuesCV.propList, Roh_freeTextSummaryValuesCV.entList);
			entList.Add(entityRoh_freeTextSummaryValuesCV);
		} 











	}
}
