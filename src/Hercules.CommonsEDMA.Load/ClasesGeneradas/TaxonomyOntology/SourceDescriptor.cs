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

namespace TaxonomyOntology
{
	[ExcludeFromCodeCoverage]
	public class SourceDescriptor : GnossOCBase
	{

		public SourceDescriptor() : base() { } 

		public SourceDescriptor(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_impactSourceCategory = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactSourceCategory"));
			SemanticPropertyModel propRoh_impactSource = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactSource");
			if(propRoh_impactSource != null && propRoh_impactSource.PropertyValues.Count > 0)
			{
				this.Roh_impactSource = new ReferenceSource(propRoh_impactSource.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/SourceDescriptor"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/SourceDescriptor"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/impactSourceCategory")]
		public  string Roh_impactSourceCategory { get; set;}

		[LABEL(LanguageEnum.es,"Fuente de impacto")]
		[RDFProperty("http://w3id.org/roh/impactSource")]
		[Required]
		public  ReferenceSource Roh_impactSource  { get; set;} 
		public string IdRoh_impactSource  { get; set;} 


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:impactSourceCategory", this.Roh_impactSourceCategory));
			propList.Add(new StringOntologyProperty("roh:impactSource", this.IdRoh_impactSource));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 



		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>();

			return valor;
		}








	}
}
