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
	public class ResearchObjects : GnossOCBase
	{

		public ResearchObjects() : base() { } 

		public ResearchObjects(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_researchObjects = new List<RelatedResearchObject>();
			SemanticPropertyModel propRoh_researchObjects = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/researchObjects");
			if(propRoh_researchObjects != null && propRoh_researchObjects.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_researchObjects.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedResearchObject roh_researchObjects = new RelatedResearchObject(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_researchObjects.Add(roh_researchObjects);
					}
				}
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ResearchObjects"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ResearchObjects"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/researchObjects")]
		public  List<RelatedResearchObject> Roh_researchObjects { get; set;}

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
			if(Roh_researchObjects!=null){
				foreach(RelatedResearchObject prop in Roh_researchObjects){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedResearchObject = new OntologyEntity("http://w3id.org/roh/RelatedResearchObject", "http://w3id.org/roh/RelatedResearchObject", "roh:researchObjects", prop.propList, prop.entList);
				entList.Add(entityRelatedResearchObject);
				prop.Entity= entityRelatedResearchObject;
				}
			}
		} 











	}
}
