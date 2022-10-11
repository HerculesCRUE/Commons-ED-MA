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
//using Document = DocumentOntology.Document;
//using ResearchObject = ResearchobjectOntology.ResearchObject;
//using Person = PersonOntology.Person;

namespace AnnotationOntology
{
	[ExcludeFromCodeCoverage]
	public class Annotation : GnossOCBase
	{

		public Annotation() : base() { }

		public virtual string RdfType { get { return "http://w3id.org/roh/Annotation"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/Annotation"; } }
		[RDFProperty("http://w3id.org/roh/document")]
		//public List<Document> Roh_document { get; set; }
		public List<string> IdsRoh_document { get; set; }

		[RDFProperty("http://w3id.org/roh/researchobject")]
		//public List<ResearchObject> Roh_researchobject { get; set; }
		public List<string> IdsRoh_researchobject { get; set; }

		[RDFProperty("http://w3id.org/roh/dateIssued")]
		public DateTime? Roh_dateIssued { get; set; }

		[RDFProperty("http://w3id.org/roh/title")]
		public string Roh_title { get; set; }


		[RDFProperty("http://w3id.org/roh/text")]
		public string Roh_text { get; set; }

		[RDFProperty("http://w3id.org/roh/owner")]
		[Required]
		//public Person Roh_owner { get; set; }
		public string IdRoh_owner { get; set; }


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("roh:document", this.IdsRoh_document));
			propList.Add(new ListStringOntologyProperty("roh:researchobject", this.IdsRoh_researchobject));
			if (this.Roh_dateIssued.HasValue)
			{
				propList.Add(new DateOntologyProperty("roh:dateIssued", this.Roh_dateIssued.Value));
			}
			propList.Add(new StringOntologyProperty("roh:text", this.Roh_text));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		}
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology = null;
			GetEntities();
			GetProperties();
			if (idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList, idrecurso, idarticulo);
			}
			resource.Id = GNOSSID;
			resource.Ontology = ontology;
			resource.TextCategories=listaDeCategorias;
			AddResourceTitle(resource);
			return resource;
		}

		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{

			//Insert en la tabla Documento
			string tags = "";
			foreach (string tag in tagList)
			{
				tags += $"{tag}, ";
			}
			if (!string.IsNullOrEmpty(tags))
			{
				tags = tags.Substring(0, tags.LastIndexOf(','));
			}
			string titulo = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/AnnotationOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Roh_title;
		}
	}
}
