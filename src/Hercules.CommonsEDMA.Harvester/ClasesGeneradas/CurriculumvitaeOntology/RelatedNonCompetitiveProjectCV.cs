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
using ContributionGradeProject = ContributiongradeprojectOntology.ContributionGradeProject;

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class RelatedNonCompetitiveProjectCV : GnossOCBase
	{

		public RelatedNonCompetitiveProjectCV() : base() { } 

		public RelatedNonCompetitiveProjectCV(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_contributionGradeProject = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contributionGradeProject");
			if(propRoh_contributionGradeProject != null && propRoh_contributionGradeProject.PropertyValues.Count > 0)
			{
				this.Roh_contributionGradeProject = new ContributionGradeProject(propRoh_contributionGradeProject.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_contributionGradeProjectOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/contributionGradeProjectOther"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/RelatedNonCompetitiveProjectCV"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/RelatedNonCompetitiveProjectCV"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/contributionGradeProject")]
		public  ContributionGradeProject Roh_contributionGradeProject  { get; set;} 
		public string IdRoh_contributionGradeProject  { get; set;} 

		[RDFProperty("http://w3id.org/roh/contributionGradeProjectOther")]
		public  string Roh_contributionGradeProjectOther { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:contributionGradeProject", this.IdRoh_contributionGradeProject));
			propList.Add(new StringOntologyProperty("roh:contributionGradeProjectOther", this.Roh_contributionGradeProjectOther));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
