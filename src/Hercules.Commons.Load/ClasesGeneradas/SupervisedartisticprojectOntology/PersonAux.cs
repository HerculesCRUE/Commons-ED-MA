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

namespace SupervisedartisticprojectOntology
{
	[ExcludeFromCodeCoverage]
	public class PersonAux : GnossOCBase
	{

		public PersonAux() : base() { } 

		public PersonAux(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Foaf_familyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/familyName"));
			this.Roh_secondFamilyName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/secondFamilyName"));
			this.Foaf_nick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/nick"));
			this.Foaf_firstName = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/firstName"));
			this.Rdf_comment = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.w3.org/1999/02/22-rdf-syntax-ns#comment")).Value;
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/PersonAux"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/PersonAux"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://xmlns.com/foaf/0.1/familyName")]
		public  string Foaf_familyName { get; set;}

		[RDFProperty("http://w3id.org/roh/secondFamilyName")]
		public  string Roh_secondFamilyName { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/nick")]
		public  string Foaf_nick { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/firstName")]
		public  string Foaf_firstName { get; set;}

		[RDFProperty("http://www.w3.org/1999/02/22-rdf-syntax-ns#comment")]
		public  int Rdf_comment { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("foaf:familyName", this.Foaf_familyName));
			propList.Add(new StringOntologyProperty("roh:secondFamilyName", this.Roh_secondFamilyName));
			propList.Add(new StringOntologyProperty("foaf:nick", this.Foaf_nick));
			propList.Add(new StringOntologyProperty("foaf:firstName", this.Foaf_firstName));
			propList.Add(new StringOntologyProperty("rdf:comment", this.Rdf_comment.ToString()));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
