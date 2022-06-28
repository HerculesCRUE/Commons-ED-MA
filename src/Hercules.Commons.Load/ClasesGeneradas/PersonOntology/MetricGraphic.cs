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

namespace PersonOntology
{
	[ExcludeFromCodeCoverage]
	public class MetricGraphic : GnossOCBase
	{

		public MetricGraphic() : base() { } 

		public MetricGraphic(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_filters = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/filters"));
			this.Roh_width = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/width"));
			this.Roh_pageId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/pageId"));
			this.Roh_order = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/order")).Value;
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
			this.Roh_graphicId = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/graphicId"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/MetricGraphic"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/MetricGraphic"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/filters")]
		public  string Roh_filters { get; set;}

		[RDFProperty("http://w3id.org/roh/width")]
		public  string Roh_width { get; set;}

		[RDFProperty("http://w3id.org/roh/pageId")]
		public  string Roh_pageId { get; set;}

		[RDFProperty("http://w3id.org/roh/order")]
		public  int Roh_order { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}

		[RDFProperty("http://w3id.org/roh/graphicId")]
		public  string Roh_graphicId { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:filters", this.Roh_filters));
			propList.Add(new StringOntologyProperty("roh:width", this.Roh_width));
			propList.Add(new StringOntologyProperty("roh:pageId", this.Roh_pageId));
			propList.Add(new StringOntologyProperty("roh:order", this.Roh_order.ToString()));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
			propList.Add(new StringOntologyProperty("roh:graphicId", this.Roh_graphicId));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
