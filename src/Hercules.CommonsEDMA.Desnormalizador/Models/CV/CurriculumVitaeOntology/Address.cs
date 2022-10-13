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
	public class Address : GnossOCBase
	{
		public Address() : base() { } 
		public virtual string RdfType { get { return "https://www.w3.org/2006/vcard/ns#Address"; } }
		public virtual string RdfsLabel { get { return "https://www.w3.org/2006/vcard/ns#Address"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#postal-code")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#postal-code")]
		public  string Vcard_postal_code { get; set;}

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#extended-address")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#extended-address")]
		public  string Vcard_extended_address { get; set;}

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#street-address")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#street-address")]
		public  string Vcard_street_address { get; set;}

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#locality")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("vcard:postal-code", this.Vcard_postal_code));
			propList.Add(new StringOntologyProperty("vcard:extended-address", this.Vcard_extended_address));
			propList.Add(new StringOntologyProperty("vcard:street-address", this.Vcard_street_address));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 
	}
}
