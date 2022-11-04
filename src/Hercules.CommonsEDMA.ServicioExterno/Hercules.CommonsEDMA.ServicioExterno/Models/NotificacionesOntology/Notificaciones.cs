using System;
using System.Collections.Generic;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using System.Diagnostics.CodeAnalysis;

namespace NotificationOntology
{
	[ExcludeFromCodeCoverage]
	public class Notification
	{
		private List<OntologyEntity> entList = new();
		private List<OntologyProperty> propList = new();
		private List<string> prefList = new();

		public Notification()
		{
			prefList.Add("xmlns:roh=\"http://w3id.org/roh/\"");
			prefList.Add("xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"");
			prefList.Add("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema#\"");
			prefList.Add("xmlns:skos=\"http://www.w3.org/2008/05/skos#\"");
			prefList.Add("xmlns:rdfs=\"http://www.w3.org/2000/01/rdf-schema#\"");
			prefList.Add("xmlns:owl=\"http://www.w3.org/2002/07/owl#\"");
			prefList.Add("xmlns:dc=\"http://purl.org/dc/elements/1.1/\"");
			prefList.Add("xmlns:foaf=\"http://xmlns.com/foaf/0.1/\"");
			prefList.Add("xmlns:vivo=\"http://vivoweb.org/ontology/core#\"");
			prefList.Add("xmlns:bibo=\"http://purl.org/ontology/bibo/\"");
			prefList.Add("xmlns:dct=\"http://purl.org/dc/terms/\"");
			prefList.Add("xmlns:obo=\"http://purl.obolibrary.org/obo/\"");
			prefList.Add("xmlns:vcard=\"https://www.w3.org/2006/vcard/ns#\"");
			prefList.Add("xmlns:schema=\"http://www.schema.org/\"");
			prefList.Add("xmlns:gn=\"http://www.geonames.org/ontology#\"");
		}

		public string RdfType { get { return "http://w3id.org/roh/Notification"; } }
		public string RdfsLabel { get { return "http://w3id.org/roh/Notification"; } }


		public string IdRoh_trigger { get; set; }
		public string Roh_idEntityCV { get; set; }
		public string Roh_tabPropertyCV { get; set; }
		public string Roh_entity { get; set; }
		public string IdRoh_owner { get; set; }
		public DateTime Dct_issued { get; set; }
		public string Roh_type { get; set; }
		public string Roh_text { get; set; }
		public string CvnCode { get; set; }


		private void GetProperties()
		{
			propList.Add(new StringOntologyProperty("roh:trigger", this.IdRoh_trigger));
			propList.Add(new StringOntologyProperty("roh:idEntityCV", this.Roh_idEntityCV));
			propList.Add(new StringOntologyProperty("roh:tabPropertyCV", this.Roh_tabPropertyCV));
			propList.Add(new StringOntologyProperty("roh:entity", this.Roh_entity));
			propList.Add(new StringOntologyProperty("roh:owner", this.IdRoh_owner));
			propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued));
			propList.Add(new StringOntologyProperty("roh:type", this.Roh_type));
			propList.Add(new StringOntologyProperty("roh:text", this.Roh_text));
			propList.Add(new StringOntologyProperty("roh:cvnCode", this.CvnCode));
		}

		public ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI)
		{
			return ToGnossApiResource(resourceAPI, Guid.Empty, Guid.Empty);
		}

		public ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new();
			Ontology ontology = null;
			GetProperties();
			if (idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList, idrecurso, idarticulo);
			}
			resource.Ontology = ontology;
			resource.Title = this.IdRoh_owner;
			return resource;
		}
	}
}