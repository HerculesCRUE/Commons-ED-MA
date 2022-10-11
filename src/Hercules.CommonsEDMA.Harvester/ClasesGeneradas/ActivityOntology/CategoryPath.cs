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
using Concept = TaxonomyOntology.Concept;

namespace ActivityOntology
{
	[ExcludeFromCodeCoverage]
	public class CategoryPath : GnossOCBase
	{

		public CategoryPath() : base() { } 

		public CategoryPath(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_categoryNode = new List<Concept>();
			SemanticPropertyModel propRoh_categoryNode = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/categoryNode");
			if(propRoh_categoryNode != null && propRoh_categoryNode.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_categoryNode.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Concept roh_categoryNode = new Concept(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_categoryNode.Add(roh_categoryNode);
					}
				}
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/CategoryPath"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/CategoryPath"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/categoryNode")]
		[RDFProperty("http://w3id.org/roh/categoryNode")]
		[MinLength(1)]
		public  List<Concept> Roh_categoryNode { get; set;}
		public List<string> IdsRoh_categoryNode { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("roh:categoryNode", this.IdsRoh_categoryNode));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
