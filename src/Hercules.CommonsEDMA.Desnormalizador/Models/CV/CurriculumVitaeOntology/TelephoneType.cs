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
using Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores;

namespace CurriculumvitaeOntology
{
	[ExcludeFromCodeCoverage]
	public class TelephoneType : GnossOCBase
	{
		public TelephoneType() : base() { } 

		public virtual string RdfType { get { return $"{ActualizadorBase.GetUrlPrefix("vcard")}TelephoneType"; } }
		public virtual string RdfsLabel { get { return $"{ActualizadorBase.GetUrlPrefix("vcard")}TelephoneType"; } }
		public OntologyEntity Entity { get; set; }
		public  string Roh_hasExtension { get; set;}

		public  string Roh_hasInternationalCode { get; set;}
		public  string Vcard_hasValue { get; set;}

		internal override void GetProperties()
		{
			propList.Add(new StringOntologyProperty("roh:hasExtension", this.Roh_hasExtension));
			propList.Add(new StringOntologyProperty("roh:hasInternationalCode", this.Roh_hasInternationalCode));
			propList.Add(new StringOntologyProperty("vcard:hasValue", this.Vcard_hasValue));
		}
	}
}
