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
using ContributionGradeDocument = ContributiongradedocumentOntology.ContributionGradeDocument;

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class RelatedScientificPublicationCV : GnossOCBase
	{

		public RelatedScientificPublicationCV() : base() { } 

		public RelatedScientificPublicationCV(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_contributionGrade = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contributionGrade");
			if(propRoh_contributionGrade != null && propRoh_contributionGrade.PropertyValues.Count > 0)
			{
				this.Roh_contributionGrade = new ContributionGradeDocument(propRoh_contributionGrade.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_googleScholarCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/googleScholarCitationCount"));
			this.Roh_correspondingAuthor= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/correspondingAuthor"));
			this.Roh_relevantPublication= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relevantPublication"));
			this.Roh_relevantResults = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/relevantResults"));
			this.Roh_inrecsCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/inrecsCitationCount"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/RelatedScientificPublicationCV"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/RelatedScientificPublicationCV"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/contributionGrade")]
		public  ContributionGradeDocument Roh_contributionGrade  { get; set;} 
		public string IdRoh_contributionGrade  { get; set;} 

		[RDFProperty("http://w3id.org/roh/googleScholarCitationCount")]
		public  int? Roh_googleScholarCitationCount { get; set;}

		[RDFProperty("http://w3id.org/roh/correspondingAuthor")]
		public  bool Roh_correspondingAuthor { get; set;}

		[RDFProperty("http://w3id.org/roh/relevantPublication")]
		public  bool Roh_relevantPublication { get; set;}

		[RDFProperty("http://w3id.org/roh/relevantResults")]
		public  string Roh_relevantResults { get; set;}

		[RDFProperty("http://w3id.org/roh/inrecsCitationCount")]
		public  int? Roh_inrecsCitationCount { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:contributionGrade", this.IdRoh_contributionGrade));
			propList.Add(new StringOntologyProperty("roh:googleScholarCitationCount", this.Roh_googleScholarCitationCount.ToString()));
			propList.Add(new BoolOntologyProperty("roh:correspondingAuthor", this.Roh_correspondingAuthor));
			propList.Add(new BoolOntologyProperty("roh:relevantPublication", this.Roh_relevantPublication));
			propList.Add(new StringOntologyProperty("roh:relevantResults", this.Roh_relevantResults));
			propList.Add(new StringOntologyProperty("roh:inrecsCitationCount", this.Roh_inrecsCitationCount.ToString()));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
