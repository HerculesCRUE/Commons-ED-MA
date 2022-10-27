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
	public class ProfessionalSituation : GnossOCBase
	{
		public ProfessionalSituation() : base() { }

		public virtual string RdfType { get { return $"{ActualizadorBase.GetUrlPrefix("roh")}ProfessionalSituation"; } }
		public virtual string RdfsLabel { get { return $"{ActualizadorBase.GetUrlPrefix("roh")}ProfessionalSituation"; } }
		public OntologyEntity Entity { get; set; }
		public  string Roh_title { get; set;}

		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}
	}
}
