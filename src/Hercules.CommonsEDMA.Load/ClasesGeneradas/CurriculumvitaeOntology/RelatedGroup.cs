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
using Group = GroupOntology.Group;

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class RelatedGroup : GnossOCBase
	{

		public RelatedGroup() : base() { } 

		public RelatedGroup(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_relatedGroupCV = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relatedGroupCV");
			if(propRoh_relatedGroupCV != null && propRoh_relatedGroupCV.PropertyValues.Count > 0)
			{
				this.Roh_relatedGroupCV = new RelatedGroupCV(propRoh_relatedGroupCV.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isPublic= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isPublic"));
			SemanticPropertyModel propVivo_relatedBy = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#relatedBy");
			if(propVivo_relatedBy != null && propVivo_relatedBy.PropertyValues.Count > 0)
			{
				this.Vivo_relatedBy = new Group(propVivo_relatedBy.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/RelatedGroup"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/RelatedGroup"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/relatedGroupCV")]
		public  RelatedGroupCV Roh_relatedGroupCV { get; set;}

		[RDFProperty("http://w3id.org/roh/isPublic")]
		public  bool Roh_isPublic { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#relatedBy")]
		[Required]
		public  Group Vivo_relatedBy  { get; set;} 
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
			if(Roh_relatedGroupCV!=null){
				Roh_relatedGroupCV.GetProperties();
				Roh_relatedGroupCV.GetEntities();
				OntologyEntity entityRoh_relatedGroupCV = new OntologyEntity("http://w3id.org/roh/RelatedGroupCV", "http://w3id.org/roh/RelatedGroupCV", "roh:relatedGroupCV", Roh_relatedGroupCV.propList, Roh_relatedGroupCV.entList);
				entList.Add(entityRoh_relatedGroupCV);
			}
		} 











	}
}
