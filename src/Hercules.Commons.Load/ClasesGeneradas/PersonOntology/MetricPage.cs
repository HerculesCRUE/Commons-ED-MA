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
	public class MetricPage : GnossOCBase
	{

		public MetricPage() : base() { } 

		public MetricPage(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_metricGraphic = new List<MetricGraphic>();
			SemanticPropertyModel propRoh_metricGraphic = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/metricGraphic");
			if(propRoh_metricGraphic != null && propRoh_metricGraphic.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_metricGraphic.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						MetricGraphic roh_metricGraphic = new MetricGraphic(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_metricGraphic.Add(roh_metricGraphic);
					}
				}
			}
			this.Roh_order = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/order")).Value;
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/MetricPage"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/MetricPage"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/metricGraphic")]
		public  List<MetricGraphic> Roh_metricGraphic { get; set;}

		[RDFProperty("http://w3id.org/roh/order")]
		public  int Roh_order { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:order", this.Roh_order.ToString()));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_metricGraphic!=null){
				foreach(MetricGraphic prop in Roh_metricGraphic){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityMetricGraphic = new OntologyEntity("http://w3id.org/roh/MetricGraphic", "http://w3id.org/roh/MetricGraphic", "roh:metricGraphic", prop.propList, prop.entList);
				entList.Add(entityMetricGraphic);
				prop.Entity= entityMetricGraphic;
				}
			}
		} 











	}
}
