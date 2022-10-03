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
using ColaborationTypeGroup = ColaborationtypegroupOntology.ColaborationTypeGroup;

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class RelatedGroupCV : GnossOCBase
	{

		public RelatedGroupCV() : base() { } 

		public RelatedGroupCV(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_collaborationType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collaborationType");
			if(propRoh_collaborationType != null && propRoh_collaborationType.PropertyValues.Count > 0)
			{
				this.Roh_collaborationType = new ColaborationTypeGroup(propRoh_collaborationType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/RelatedGroupCV"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/RelatedGroupCV"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/collaborationType")]
		public  ColaborationTypeGroup Roh_collaborationType  { get; set;} 
		public string IdRoh_collaborationType  { get; set;} 


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:collaborationType", this.IdRoh_collaborationType));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
