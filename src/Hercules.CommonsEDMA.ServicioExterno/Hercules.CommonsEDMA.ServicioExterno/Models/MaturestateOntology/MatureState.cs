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

namespace MaturestateOntology
{
	[ExcludeFromCodeCoverage]
	public class MatureState : GnossOCBase
	{

		public MatureState() : base() { } 

		
		public virtual string RdfType { get { return "http://w3id.org/roh/MatureState"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/MatureState"; } }
		[RDFProperty("http://purl.org/dc/elements/1.1/title")]
		public  Dictionary<LanguageEnum,string> Dc_title { get; set;}

		[RDFProperty("http://purl.org/dc/elements/1.1/identifier")]
		public  string Dc_identifier { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			if(this.Dc_title != null)
			{
				foreach (LanguageEnum idioma in this.Dc_title.Keys)
				{
					propList.Add(new StringOntologyProperty("dc:title", this.Dc_title[idioma], idioma.ToString()));
				}
			}
			else
			{
				throw new GnossAPIException($"La propiedad dc:title debe tener al menos un valor en el recurso: {resourceID}");
			}
			propList.Add(new StringOntologyProperty("dc:identifier", this.Dc_identifier));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		}
		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>();

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/MaturestateOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
		}





	}
}
