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
using ImpactIndexCategory = ImpactindexcategoryOntology.ImpactIndexCategory;

namespace MaindocumentOntology
{
	[ExcludeFromCodeCoverage]
	public class ImpactCategory : GnossOCBase
	{

		public ImpactCategory() : base() { } 

		public ImpactCategory(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_journalNumberInCat = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/journalNumberInCat"));
			this.Roh_publicationPosition = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationPosition"));
			this.Roh_quartile = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/quartile")).Value;
			SemanticPropertyModel propRoh_impactIndexCategory = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndexCategory");
			if(propRoh_impactIndexCategory != null && propRoh_impactIndexCategory.PropertyValues.Count > 0)
			{
				this.Roh_impactIndexCategory = new ImpactIndexCategory(propRoh_impactIndexCategory.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ImpactCategory"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ImpactCategory"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/journalNumberInCat")]
		public  int? Roh_journalNumberInCat { get; set;}

		[RDFProperty("http://w3id.org/roh/publicationPosition")]
		public  int? Roh_publicationPosition { get; set;}

		[RDFProperty("http://w3id.org/roh/quartile")]
		public  int Roh_quartile { get; set;}

		[LABEL(LanguageEnum.es,"Categoría del índice de impacto")]
		[RDFProperty("http://w3id.org/roh/impactIndexCategory")]
		[Required]
		public  ImpactIndexCategory Roh_impactIndexCategory  { get; set;} 
		public string IdRoh_impactIndexCategory  { get; set;} 


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:journalNumberInCat", this.Roh_journalNumberInCat.ToString()));
			propList.Add(new StringOntologyProperty("roh:publicationPosition", this.Roh_publicationPosition.ToString()));
			propList.Add(new StringOntologyProperty("roh:quartile", this.Roh_quartile.ToString()));
			propList.Add(new StringOntologyProperty("roh:impactIndexCategory", this.IdRoh_impactIndexCategory));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
