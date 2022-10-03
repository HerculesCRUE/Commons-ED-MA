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
	public class RelatedScientificPublication : GnossOCBase
	{

		public RelatedScientificPublication() : base() { } 

		public RelatedScientificPublication(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_relatedScientificPublicationCV = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relatedScientificPublicationCV");
			if(propRoh_relatedScientificPublicationCV != null && propRoh_relatedScientificPublicationCV.PropertyValues.Count > 0)
			{
				this.Roh_relatedScientificPublicationCV = new RelatedScientificPublicationCV(propRoh_relatedScientificPublicationCV.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isPublic= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isPublic"));
			SemanticPropertyModel propVivo_relatedBy = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#relatedBy");
			if(propVivo_relatedBy != null && propVivo_relatedBy.PropertyValues.Count > 0)
			{
				this.Vivo_relatedBy = new Document(propVivo_relatedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/RelatedScientificPublication"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/RelatedScientificPublication"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/relatedScientificPublicationCV")]
		public  RelatedScientificPublicationCV Roh_relatedScientificPublicationCV { get; set;}

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
			if(Roh_relatedScientificPublicationCV!=null){
				Roh_relatedScientificPublicationCV.GetProperties();
				Roh_relatedScientificPublicationCV.GetEntities();
				OntologyEntity entityRoh_relatedScientificPublicationCV = new OntologyEntity("http://w3id.org/roh/RelatedScientificPublicationCV", "http://w3id.org/roh/RelatedScientificPublicationCV", "roh:relatedScientificPublicationCV", Roh_relatedScientificPublicationCV.propList, Roh_relatedScientificPublicationCV.entList);
				entList.Add(entityRoh_relatedScientificPublicationCV);
			}
		} 











	}
}
