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
	public class NoDuplicateGroup : GnossOCBase
	{

		public NoDuplicateGroup() : base() { } 

		public NoDuplicateGroup(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_noDuplicateId = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/noDuplicateId");
			this.Roh_noDuplicateId = new List<string>();
			if (propRoh_noDuplicateId != null && propRoh_noDuplicateId.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_noDuplicateId.PropertyValues)
				{
					this.Roh_noDuplicateId.Add(propValue.Value);
				}
			}
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/NoDuplicateGroup"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/NoDuplicateGroup"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/noDuplicateId")]
		public  List<string> Roh_noDuplicateId { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("roh:noDuplicateId", this.Roh_noDuplicateId));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
