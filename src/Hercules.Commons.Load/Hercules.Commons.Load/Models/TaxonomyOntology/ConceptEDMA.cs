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
using System.Text.RegularExpressions;
using System.Globalization;
using System.Collections;
using Gnoss.ApiWrapper.Exceptions;
using TaxonomyOntology;
using GnossBase;

namespace Hercules.Commons.Load.Models.TaxonomyOntology
{
    public class ConceptEDMA : Concept
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



        public ConceptEDMA() : base() { }

        public virtual SecondaryResource ToGnossApiResource(ResourceApi resourceAPI, string identificador)
        {
            SecondaryResource resource = new SecondaryResource();
            List<SecondaryEntity> listSecondaryEntity = null;
            GetProperties();
            SecondaryOntology ontology = new SecondaryOntology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, "http://www.w3.org/2008/05/skos#Concept", "http://www.w3.org/2008/05/skos#Concept", prefList, propList, this.Dc_source + "_" + identificador, listSecondaryEntity, entList);
            resource.SecondaryOntology = ontology;
            return resource;
        }


        [LABEL(LanguageEnum.es, "Etiqueta preferente")]
        [RDFProperty("http://www.w3.org/2008/05/skos#prefLabel")]
        public Dictionary<LanguageEnum, string> Skos_prefLabelMulti { get; set; }

        private void GetProperties()
        {
            propList.Add(new StringOntologyProperty("skos:prefLabel", this.Skos_prefLabel));
            if (this.Skos_prefLabelMulti != null)
            {
                foreach (LanguageEnum idioma in this.Skos_prefLabelMulti.Keys)
                {
                    propList.Add(new StringOntologyProperty("skos:prefLabel", this.Skos_prefLabelMulti[idioma], idioma.ToString()));
                }
            }
            if (this.Skos_prefLabel != null)
            {
                propList.Add(new StringOntologyProperty("skos:prefLabel", this.Skos_prefLabel));
            }

            propList.Add(new StringOntologyProperty("skos:symbol", this.Skos_symbol));
            propList.Add(new StringOntologyProperty("dc:identifier", this.Dc_identifier));
            propList.Add(new StringOntologyProperty("dc:source", this.Dc_source));
            if (this.Skos_narrower != null && this.Skos_narrower.Count > 0)
            {
                propList.Add(new ListStringOntologyProperty("skos:narrower", this.Skos_narrower.Select(x => Dc_source + "_" + x.Dc_identifier).ToList()));
            }
            if (this.Skos_broader != null && this.Skos_broader.Count > 0)
            {
                propList.Add(new ListStringOntologyProperty("skos:broader", this.Skos_broader.Select(x => Dc_source + "_" + x.Dc_identifier).ToList()));
            }
        }




    }
}
