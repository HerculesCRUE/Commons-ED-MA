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
using KeyWordConcept = KeywordconceptOntology.KeyWordConcept;

namespace DocumentOntology
{
	[ExcludeFromCodeCoverage]
	public class KeyWord : GnossOCBase
	{

		public KeyWord() : base() { } 

		public KeyWord(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			SemanticPropertyModel propRoh_keyWordConcept = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/keyWordConcept");
			if(propRoh_keyWordConcept != null && propRoh_keyWordConcept.PropertyValues.Count > 0)
			{
				this.Roh_keyWordConcept = new KeyWordConcept(propRoh_keyWordConcept.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/KeyWord"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/KeyWord"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/keyWordConcept")]
		public  KeyWordConcept Roh_keyWordConcept  { get; set;} 
		public string IdRoh_keyWordConcept  { get; set;} 

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:keyWordConcept", this.IdRoh_keyWordConcept));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
