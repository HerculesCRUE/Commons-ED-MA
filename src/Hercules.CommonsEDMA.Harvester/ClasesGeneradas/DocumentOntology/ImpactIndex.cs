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
using ReferenceSource = ReferencesourceOntology.ReferenceSource;

namespace DocumentOntology
{
	[ExcludeFromCodeCoverage]
	public class ImpactIndex : GnossOCBase
	{

		public ImpactIndex() : base() { } 

		public ImpactIndex(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_impactSource = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactSource");
			if(propRoh_impactSource != null && propRoh_impactSource.PropertyValues.Count > 0)
			{
				this.Roh_impactSource = new ReferenceSource(propRoh_impactSource.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_impactIndexInYear = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndexInYear"));
			this.Roh_impactSourceOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactSourceOther"));
			this.Roh_impactIndexCategoryEntity = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndexCategoryEntity"));
			this.Roh_journalNumberInCat = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/journalNumberInCat"));
			this.Roh_impactIndexCategory = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndexCategory"));
			this.Roh_publicationPosition = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationPosition"));
			this.Roh_quartile = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/quartile"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ImpactIndex"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ImpactIndex"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"Fuente de impacto")]
		[RDFProperty("http://w3id.org/roh/impactSource")]
		public  ReferenceSource Roh_impactSource  { get; set;} 
		public string IdRoh_impactSource  { get; set;} 

		[RDFProperty("http://w3id.org/roh/impactIndexInYear")]
		public  float? Roh_impactIndexInYear { get; set;}

		[RDFProperty("http://w3id.org/roh/impactSourceOther")]
		public  string Roh_impactSourceOther { get; set;}

		[RDFProperty("http://w3id.org/roh/impactIndexCategoryEntity")]
		public  string Roh_impactIndexCategoryEntity { get; set;}

		[RDFProperty("http://w3id.org/roh/journalNumberInCat")]
		public  int? Roh_journalNumberInCat { get; set;}

		[RDFProperty("http://w3id.org/roh/impactIndexCategory")]
		public  string Roh_impactIndexCategory { get; set;}

		[RDFProperty("http://w3id.org/roh/publicationPosition")]
		public  int? Roh_publicationPosition { get; set;}

		[RDFProperty("http://w3id.org/roh/quartile")]
		public  int? Roh_quartile { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:impactSource", this.IdRoh_impactSource));
			propList.Add(new StringOntologyProperty("roh:impactIndexInYear", this.Roh_impactIndexInYear.ToString()));
			propList.Add(new StringOntologyProperty("roh:impactSourceOther", this.Roh_impactSourceOther));
			propList.Add(new StringOntologyProperty("roh:impactIndexCategoryEntity", this.Roh_impactIndexCategoryEntity));
			propList.Add(new StringOntologyProperty("roh:journalNumberInCat", this.Roh_journalNumberInCat.ToString()));
			propList.Add(new StringOntologyProperty("roh:impactIndexCategory", this.Roh_impactIndexCategory));
			propList.Add(new StringOntologyProperty("roh:publicationPosition", this.Roh_publicationPosition.ToString()));
			propList.Add(new StringOntologyProperty("roh:quartile", this.Roh_quartile.ToString()));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
