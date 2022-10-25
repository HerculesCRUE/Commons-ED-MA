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
	public class Document : GnossOCBase
	{
		public Document() : base() { } 

		public virtual string RdfType { get { return "http://xmlns.com/foaf/0.1/Document"; } }
		public virtual string RdfsLabel { get { return "http://xmlns.com/foaf/0.1/Document"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"http://purl.org/dc/elements/1.1/title")]
		[RDFProperty("http://purl.org/dc/elements/1.1/title")]
		public  string Dc_title { get; set;}

		[LABEL(LanguageEnum.es,"http://xmlns.com/foaf/0.1/topic")]
		[RDFProperty("http://xmlns.com/foaf/0.1/topic")]
		public  string Foaf_topic { get; set;}


		internal override void GetProperties()
		{
			propList.Add(new StringOntologyProperty("dc:title", this.Dc_title));
			propList.Add(new StringOntologyProperty("foaf:topic", this.Foaf_topic));
		}
	}
}
