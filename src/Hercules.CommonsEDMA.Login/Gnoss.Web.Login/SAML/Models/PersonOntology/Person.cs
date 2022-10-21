extern alias ApiWrapper;

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Es.Riam.Gnoss.Web.MVC.Models;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using ApiWrapper::Gnoss.ApiWrapper.Model;
using ApiWrapper::Gnoss.ApiWrapper;

namespace Gnoss.Web.Login.SAML.Models.PersonOntology
{
	[ExcludeFromCodeCoverage]
	public class Person : GnossOCBase
	{

		public Person() : base() { } 

		public virtual string RdfType { get { return "http://xmlns.com/foaf/0.1/Person"; } }
		public virtual string RdfsLabel { get { return "http://xmlns.com/foaf/0.1/Person"; } }		

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#email")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#email")]
		public  List<string> Vcard_email { get; set;}

		[LABEL(LanguageEnum.es,"http://xmlns.com/foaf/0.1/firstName")]
		[RDFProperty("http://xmlns.com/foaf/0.1/firstName")]
		public  string Foaf_firstName { get; set;}

		[LABEL(LanguageEnum.es,"http://xmlns.com/foaf/0.1/lastName")]
		[RDFProperty("http://xmlns.com/foaf/0.1/lastName")]
		public  string Foaf_lastName { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("vcard:email", this.Vcard_email));
			propList.Add(new StringOntologyProperty("foaf:firstName", this.Foaf_firstName));
			propList.Add(new StringOntologyProperty("foaf:lastName", this.Foaf_lastName));
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology=null;
			GetEntities();
			GetProperties();
			if(idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList,idrecurso,idarticulo);
			}
			resource.Id = GNOSSID;
			resource.Ontology = ontology;
			resource.TextCategories=listaDeCategorias;
			AddResourceTitle(resource);
			AddResourceDescription(resource);
			return resource;
		}

		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Foaf_firstName;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Foaf_firstName;
		}

	}
}
