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
using Person = PersonOntology.Person;

namespace ProjectOntology
{
	[ExcludeFromCodeCoverage]
	public class BFO_0000023 : GnossOCBase
	{

		public BFO_0000023() : base() { } 

		public BFO_0000023(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Vivo_start = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#start"));
			this.Rdf_comment = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://www.w3.org/1999/02/22-rdf-syntax-ns#comment"));
			this.Vivo_end = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#end"));
			this.Foaf_nick = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://xmlns.com/foaf/0.1/nick"));
			SemanticPropertyModel propRoh_roleOf = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/roleOf");
			if(propRoh_roleOf != null && propRoh_roleOf.PropertyValues.Count > 0)
			{
				this.Roh_roleOf = new Person(propRoh_roleOf.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isIP= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isIP"));
		}

		public virtual string RdfType { get { return "http://purl.obolibrary.org/obo/BFO_0000023"; } }
		public virtual string RdfsLabel { get { return "http://purl.obolibrary.org/obo/BFO_0000023"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://vivoweb.org/ontology/core#start")]
		public  DateTime? Vivo_start { get; set;}

		[RDFProperty("http://www.w3.org/1999/02/22-rdf-syntax-ns#comment")]
		public  int? Rdf_comment { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#end")]
		public  DateTime? Vivo_end { get; set;}

		[RDFProperty("http://xmlns.com/foaf/0.1/nick")]
		public  string Foaf_nick { get; set;}

		[RDFProperty("http://w3id.org/roh/roleOf")]
		[Required]
		public  Person Roh_roleOf  { get; set;} 
		public string IdRoh_roleOf  { get; set;} 

		[RDFProperty("http://w3id.org/roh/isIP")]
		public  bool Roh_isIP { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			if (this.Vivo_start.HasValue){
				propList.Add(new DateOntologyProperty("vivo:start", this.Vivo_start.Value));
				}
			propList.Add(new StringOntologyProperty("rdf:comment", this.Rdf_comment.ToString()));
			if (this.Vivo_end.HasValue){
				propList.Add(new DateOntologyProperty("vivo:end", this.Vivo_end.Value));
				}
			propList.Add(new StringOntologyProperty("foaf:nick", this.Foaf_nick));
			propList.Add(new StringOntologyProperty("roh:roleOf", this.IdRoh_roleOf));
			propList.Add(new BoolOntologyProperty("roh:isIP", this.Roh_isIP));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











	}
}
