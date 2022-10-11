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
using Document = DocumentOntology.Document;

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class RelatedWorkSubmittedSeminars : GnossOCBase
	{

		public RelatedWorkSubmittedSeminars() : base() { } 

		public RelatedWorkSubmittedSeminars(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_relatedWorkSubmittedSeminarsCV = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relatedWorkSubmittedSeminarsCV");
			if(propRoh_relatedWorkSubmittedSeminarsCV != null && propRoh_relatedWorkSubmittedSeminarsCV.PropertyValues.Count > 0)
			{
				this.Roh_relatedWorkSubmittedSeminarsCV = new RelatedWorkSubmittedSeminarsCV(propRoh_relatedWorkSubmittedSeminarsCV.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isPublic= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isPublic"));
			SemanticPropertyModel propVivo_relatedBy = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#relatedBy");
			if(propVivo_relatedBy != null && propVivo_relatedBy.PropertyValues.Count > 0)
			{
				this.Vivo_relatedBy = new Document(propVivo_relatedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/RelatedWorkSubmittedSeminars"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/RelatedWorkSubmittedSeminars"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/relatedWorkSubmittedSeminarsCV")]
		public  RelatedWorkSubmittedSeminarsCV Roh_relatedWorkSubmittedSeminarsCV { get; set;}

		[RDFProperty("http://w3id.org/roh/isPublic")]
		public  bool Roh_isPublic { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#relatedBy")]
		[Required]
		public  Document Vivo_relatedBy  { get; set;} 
		public string IdVivo_relatedBy  { get; set;} 


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new BoolOntologyProperty("roh:isPublic", this.Roh_isPublic));
			propList.Add(new StringOntologyProperty("vivo:relatedBy", this.IdVivo_relatedBy));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_relatedWorkSubmittedSeminarsCV!=null){
				Roh_relatedWorkSubmittedSeminarsCV.GetProperties();
				Roh_relatedWorkSubmittedSeminarsCV.GetEntities();
				OntologyEntity entityRoh_relatedWorkSubmittedSeminarsCV = new OntologyEntity("http://w3id.org/roh/RelatedWorkSubmittedSeminarsCV", "http://w3id.org/roh/RelatedWorkSubmittedSeminarsCV", "roh:relatedWorkSubmittedSeminarsCV", Roh_relatedWorkSubmittedSeminarsCV.propList, Roh_relatedWorkSubmittedSeminarsCV.entList);
				entList.Add(entityRoh_relatedWorkSubmittedSeminarsCV);
			}
		} 











	}
}
