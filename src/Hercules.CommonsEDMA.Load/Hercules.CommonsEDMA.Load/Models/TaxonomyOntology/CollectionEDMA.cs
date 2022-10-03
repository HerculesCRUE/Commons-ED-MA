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
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using Gnoss.ApiWrapper.Exceptions;
using System.Collections.ObjectModel;
using TaxonomyOntology;

namespace Hercules.CommonsEDMA.Load.Models.TaxonomyOntology
{
	public class CollectionEDMA : Collection
	{
		private List<OntologyEntity> entList = new List<OntologyEntity>();
		private List<OntologyProperty> propList = new List<OntologyProperty>();
		private List<string> prefList = new List<string>() {
			"xmlns:foaf=\"http://xmlns.com/foaf/0.1/\"",
			"xmlns:vivo=\"http://vivoweb.org/ontology/core#\"",
			"xmlns:rdfs=\"http://www.w3.org/2000/01/rdf-schema#\"",
			"xmlns:owl=\"http://www.w3.org/2002/07/owl#\"",
			"xmlns:bibo=\"http://purl.org/ontology/bibo/\"",
			"xmlns:roh=\"http://w3id.org/roh/\"",
			"xmlns:dct=\"http://purl.org/dc/terms/\"",
			"xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"",
			"xmlns:xsd=\"http://www.w3.org/2001/XMLSchema#\"",
			"xmlns:obo=\"http://purl.obolibrary.org/obo/\"",
			"xmlns:vcard=\"https://www.w3.org/2006/vcard/ns#\"",
			"xmlns:dc=\"http://purl.org/dc/elements/1.1/\"",
			"xmlns:skos=\"http://www.w3.org/2008/05/skos#\"",
			};

		public CollectionEDMA() : base() { } 

		private void GetProperties()
		{
			propList.Add(new StringOntologyProperty("skos:scopeNote", this.Skos_scopeNote));
			propList.Add(new StringOntologyProperty("dc:source", this.Dc_source));
			if (this.Skos_member != null && this.Skos_member.Count > 0)
			{
				propList.Add(new ListStringOntologyProperty("skos:member", this.Skos_member.Select(x =>Dc_source+"_"+ x.Dc_identifier).ToList()));
			}
		}
		public virtual SecondaryResource ToGnossApiResource(ResourceApi resourceAPI,string identificador)
		{
			SecondaryResource resource = new SecondaryResource();
			List<SecondaryEntity> listSecondaryEntity = null;
			GetProperties();
			SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "http://www.w3.org/2008/05/skos#Collection", "http://www.w3.org/2008/05/skos#Collection", prefList, propList, this.Dc_source + "_" + identificador,listSecondaryEntity, entList);
			resource.SecondaryOntology = ontology;
			return resource;
		}
	}
}
