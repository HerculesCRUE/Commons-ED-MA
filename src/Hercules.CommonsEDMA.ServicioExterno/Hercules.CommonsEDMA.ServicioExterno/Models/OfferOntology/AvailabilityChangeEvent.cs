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
using OfferState = OfferstateOntology.OfferState;

namespace OfferOntology
{
	[ExcludeFromCodeCoverage]
	public class AvailabilityChangeEvent : GnossOCBase
	{

		public AvailabilityChangeEvent() : base() { } 

		public virtual string RdfType { get { return "http://w3id.org/roh/AvailabilityChangeEvent"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/AvailabilityChangeEvent"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/roleOf")]
		[Required]
		//public  Person Roh_roleOf  { get; set;} 
		public string IdRoh_roleOf  { get; set;} 

		[RDFProperty("http://www.schema.org/availability")]
		[Required]
		public  OfferState Schema_availability  { get; set;} 
		public string IdSchema_availability  { get; set;} 

		[RDFProperty("http://www.schema.org/validFrom")]
		public  DateTime Schema_validFrom { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:roleOf", this.IdRoh_roleOf));
			propList.Add(new StringOntologyProperty("schema:availability", this.IdSchema_availability));
			propList.Add(new DateOntologyProperty("schema:validFrom", this.Schema_validFrom));
		}

	}
}
