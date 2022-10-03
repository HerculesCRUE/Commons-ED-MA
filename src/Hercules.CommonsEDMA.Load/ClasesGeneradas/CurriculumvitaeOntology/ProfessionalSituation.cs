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
	public class ProfessionalSituation : GnossOCBase
	{

		public ProfessionalSituation() : base() { } 

		public ProfessionalSituation(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_currentProfessionalSituation = new List<RelatedCurrentProfessionalSituation>();
			SemanticPropertyModel propRoh_currentProfessionalSituation = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/currentProfessionalSituation");
			if(propRoh_currentProfessionalSituation != null && propRoh_currentProfessionalSituation.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_currentProfessionalSituation.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedCurrentProfessionalSituation roh_currentProfessionalSituation = new RelatedCurrentProfessionalSituation(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_currentProfessionalSituation.Add(roh_currentProfessionalSituation);
					}
				}
			}
			this.Roh_previousPositions = new List<RelatedPreviousPositions>();
			SemanticPropertyModel propRoh_previousPositions = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/previousPositions");
			if(propRoh_previousPositions != null && propRoh_previousPositions.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_previousPositions.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						RelatedPreviousPositions roh_previousPositions = new RelatedPreviousPositions(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_previousPositions.Add(roh_previousPositions);
					}
				}
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ProfessionalSituation"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ProfessionalSituation"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/currentProfessionalSituation")]
		public  List<RelatedCurrentProfessionalSituation> Roh_currentProfessionalSituation { get; set;}

		[RDFProperty("http://w3id.org/roh/previousPositions")]
		public  List<RelatedPreviousPositions> Roh_previousPositions { get; set;}

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
			if(Roh_currentProfessionalSituation!=null){
				foreach(RelatedCurrentProfessionalSituation prop in Roh_currentProfessionalSituation){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedCurrentProfessionalSituation = new OntologyEntity("http://w3id.org/roh/RelatedCurrentProfessionalSituation", "http://w3id.org/roh/RelatedCurrentProfessionalSituation", "roh:currentProfessionalSituation", prop.propList, prop.entList);
				entList.Add(entityRelatedCurrentProfessionalSituation);
				prop.Entity= entityRelatedCurrentProfessionalSituation;
				}
			}
			if(Roh_previousPositions!=null){
				foreach(RelatedPreviousPositions prop in Roh_previousPositions){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityRelatedPreviousPositions = new OntologyEntity("http://w3id.org/roh/RelatedPreviousPositions", "http://w3id.org/roh/RelatedPreviousPositions", "roh:previousPositions", prop.propList, prop.entList);
				entList.Add(entityRelatedPreviousPositions);
				prop.Entity= entityRelatedPreviousPositions;
				}
			}
		} 











	}
}
