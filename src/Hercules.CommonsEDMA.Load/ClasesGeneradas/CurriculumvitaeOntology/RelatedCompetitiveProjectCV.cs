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
using DedicationRegime = DedicationregimeOntology.DedicationRegime;
using ParticipationTypeProject = ParticipationtypeprojectOntology.ParticipationTypeProject;
using ContributionGradeProject = ContributiongradeprojectOntology.ContributionGradeProject;

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class RelatedCompetitiveProjectCV : GnossOCBase
	{

		public RelatedCompetitiveProjectCV() : base() { } 

		public RelatedCompetitiveProjectCV(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_dedication = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/dedication");
			if(propRoh_dedication != null && propRoh_dedication.PropertyValues.Count > 0)
			{
				this.Roh_dedication = new DedicationRegime(propRoh_dedication.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_participationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationType");
			if(propRoh_participationType != null && propRoh_participationType.PropertyValues.Count > 0)
			{
				this.Roh_participationType = new ParticipationTypeProject(propRoh_participationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_contributionGradeProject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contributionGradeProject");
			if(propRoh_contributionGradeProject != null && propRoh_contributionGradeProject.PropertyValues.Count > 0)
			{
				this.Roh_contributionGradeProject = new ContributionGradeProject(propRoh_contributionGradeProject.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_contributionGradeProjectOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contributionGradeProjectOther"));
			this.Roh_participationTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/participationTypeOther"));
			this.Roh_applicantContribution = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/applicantContribution"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/RelatedCompetitiveProjectCV"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/RelatedCompetitiveProjectCV"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/dedication")]
		public  DedicationRegime Roh_dedication  { get; set;} 
		public string IdRoh_dedication  { get; set;} 

		[RDFProperty("http://w3id.org/roh/participationType")]
		public  ParticipationTypeProject Roh_participationType  { get; set;} 
		public string IdRoh_participationType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/contributionGradeProject")]
		public  ContributionGradeProject Roh_contributionGradeProject  { get; set;} 
		public string IdRoh_contributionGradeProject  { get; set;} 

		[RDFProperty("http://w3id.org/roh/contributionGradeProjectOther")]
		public  string Roh_contributionGradeProjectOther { get; set;}

		[RDFProperty("http://w3id.org/roh/participationTypeOther")]
		public  string Roh_participationTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/applicantContribution")]
		public  string Roh_applicantContribution { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:dedication", this.IdRoh_dedication));
			propList.Add(new StringOntologyProperty("roh:participationType", this.IdRoh_participationType));
			propList.Add(new StringOntologyProperty("roh:contributionGradeProject", this.IdRoh_contributionGradeProject));
			propList.Add(new StringOntologyProperty("roh:contributionGradeProjectOther", this.Roh_contributionGradeProjectOther));
			propList.Add(new StringOntologyProperty("roh:participationTypeOther", this.Roh_participationTypeOther));
			propList.Add(new StringOntologyProperty("roh:applicantContribution", this.Roh_applicantContribution));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
