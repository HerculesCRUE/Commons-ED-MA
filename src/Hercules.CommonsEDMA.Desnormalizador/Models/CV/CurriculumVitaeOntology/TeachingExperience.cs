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
	public class TeachingExperience : GnossOCBase
	{
		public TeachingExperience() : base() { }

		public virtual string RdfType { get { return "http://w3id.org/roh/TeachingExperience"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/TeachingExperience"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/title")]
		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}

		internal override void GetProperties()
		{
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}
	}
}
