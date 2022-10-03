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
	public class GeneralQualityIndicator : GnossOCBase
	{

		public GeneralQualityIndicator() : base() { } 

		public GeneralQualityIndicator(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_generalQualityIndicatorCV = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/generalQualityIndicatorCV");
			if(propRoh_generalQualityIndicatorCV != null && propRoh_generalQualityIndicatorCV.PropertyValues.Count > 0)
			{
				this.Roh_generalQualityIndicatorCV = new GeneralQualityIndicatorCV(propRoh_generalQualityIndicatorCV.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/GeneralQualityIndicator"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/GeneralQualityIndicator"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/generalQualityIndicatorCV")]
		public  GeneralQualityIndicatorCV Roh_generalQualityIndicatorCV { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			Roh_generalQualityIndicatorCV.GetProperties();
			Roh_generalQualityIndicatorCV.GetEntities();
			OntologyEntity entityRoh_generalQualityIndicatorCV = new OntologyEntity("http://w3id.org/roh/GeneralQualityIndicatorCV", "http://w3id.org/roh/GeneralQualityIndicatorCV", "roh:generalQualityIndicatorCV", Roh_generalQualityIndicatorCV.propList, Roh_generalQualityIndicatorCV.entList);
			entList.Add(entityRoh_generalQualityIndicatorCV);
		} 











	}
}
