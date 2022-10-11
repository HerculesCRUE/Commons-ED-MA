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
using EventInscriptionType = EventinscriptiontypeOntology.EventInscriptionType;

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class RelatedWorkSubmittedSeminarsCV : GnossOCBase
	{

		public RelatedWorkSubmittedSeminarsCV() : base() { } 

		public RelatedWorkSubmittedSeminarsCV(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_inscriptionType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/inscriptionType");
			if(propRoh_inscriptionType != null && propRoh_inscriptionType.PropertyValues.Count > 0)
			{
				this.Roh_inscriptionType = new EventInscriptionType(propRoh_inscriptionType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_correspondingAuthor= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/correspondingAuthor"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/RelatedWorkSubmittedSeminarsCV"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/RelatedWorkSubmittedSeminarsCV"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/inscriptionType")]
		public  EventInscriptionType Roh_inscriptionType  { get; set;} 
		public string IdRoh_inscriptionType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/correspondingAuthor")]
		public  bool Roh_correspondingAuthor { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:inscriptionType", this.IdRoh_inscriptionType));
			propList.Add(new BoolOntologyProperty("roh:correspondingAuthor", this.Roh_correspondingAuthor));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
