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
	public class FreeTextSummary : GnossOCBase
	{

		public FreeTextSummary() : base() { } 

		public FreeTextSummary(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_freeTextSummaryValues = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/freeTextSummaryValues");
			if(propRoh_freeTextSummaryValues != null && propRoh_freeTextSummaryValues.PropertyValues.Count > 0)
			{
				this.Roh_freeTextSummaryValues = new FreeTextSummaryValues(propRoh_freeTextSummaryValues.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/FreeTextSummary"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/FreeTextSummary"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/freeTextSummaryValues")]
		public  FreeTextSummaryValues Roh_freeTextSummaryValues { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_freeTextSummaryValues!=null){
				Roh_freeTextSummaryValues.GetProperties();
				Roh_freeTextSummaryValues.GetEntities();
				OntologyEntity entityRoh_freeTextSummaryValues = new OntologyEntity("http://w3id.org/roh/FreeTextSummaryValues", "http://w3id.org/roh/FreeTextSummaryValues", "roh:freeTextSummaryValues", Roh_freeTextSummaryValues.propList, Roh_freeTextSummaryValues.entList);
				entList.Add(entityRoh_freeTextSummaryValues);
			}
		} 











	}
}
