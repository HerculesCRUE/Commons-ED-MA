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

namespace CurriculumvitaeOntology
{
    [ExcludeFromCodeCoverage]
    public class PersonalData : GnossOCBase
    {
        public PersonalData() : base() { }
        public virtual string RdfType { get { return "http://w3id.org/roh/PersonalData"; } }
        public virtual string RdfsLabel { get { return "http://w3id.org/roh/PersonalData"; } }
        public OntologyEntity Entity { get; set; }

        [LABEL(LanguageEnum.es, "http://xmlns.com/foaf/0.1/firstName")]
        [RDFProperty("http://xmlns.com/foaf/0.1/firstName")]
        public string Foaf_firstName { get; set; }

        [LABEL(LanguageEnum.es, "http://xmlns.com/foaf/0.1/familyName")]
        [RDFProperty("http://xmlns.com/foaf/0.1/familyName")]
        public string Foaf_familyName { get; set; }

        [LABEL(LanguageEnum.es, "http://w3id.org/roh/secondFamilyName")]
        [RDFProperty("http://w3id.org/roh/secondFamilyName")]
        public string Roh_secondFamilyName { get; set; }


        [LABEL(LanguageEnum.es, "http://w3id.org/roh/hasFax")]
        [RDFProperty("http://w3id.org/roh/hasFax")]
        public TelephoneType Roh_hasFax { get; set; }

        [LABEL(LanguageEnum.es, "http://w3id.org/roh/birthplace")]
        [RDFProperty("http://w3id.org/roh/birthplace")]
        public Address Roh_birthplace { get; set; }

        [LABEL(LanguageEnum.es, "http://w3id.org/roh/otherIds")]
        [RDFProperty("http://w3id.org/roh/otherIds")]
        public List<Document> Roh_otherIds { get; set; }

        [LABEL(LanguageEnum.es, "https://www.w3.org/2006/vcard/ns#address")]
        [RDFProperty("https://www.w3.org/2006/vcard/ns#address")]
        public Address Vcard_address { get; set; }

        public string IdFoaf_gender { get; set; }

        [LABEL(LanguageEnum.es, "https://www.w3.org/2006/vcard/ns#hasTelephone")]
        [RDFProperty("https://www.w3.org/2006/vcard/ns#hasTelephone")]
        public TelephoneType Vcard_hasTelephone { get; set; }

        [LABEL(LanguageEnum.es, "http://w3id.org/roh/hasMobilePhone")]
        [RDFProperty("http://w3id.org/roh/hasMobilePhone")]
        public TelephoneType Roh_hasMobilePhone { get; set; }

        [LABEL(LanguageEnum.es, "http://w3id.org/roh/nie")]
        [RDFProperty("http://w3id.org/roh/nie")]
        public string Roh_nie { get; set; }

        [LABEL(LanguageEnum.es, "http://vivoweb.org/ontology/core#researcherId")]
        [RDFProperty("http://vivoweb.org/ontology/core#researcherId")]
        public string Vivo_researcherId { get; set; }

        [LABEL(LanguageEnum.es, "http://vivoweb.org/ontology/core#scopusId")]
        [RDFProperty("http://vivoweb.org/ontology/core#scopusId")]
        public string Vivo_scopusId { get; set; }

        [LABEL(LanguageEnum.es, "Imagen")]
        [RDFProperty("http://xmlns.com/foaf/0.1/img")]
        public string Foaf_img { get; set; }

        [LABEL(LanguageEnum.es, "http://w3id.org/roh/dni")]
        [RDFProperty("http://w3id.org/roh/dni")]
        public string Roh_dni { get; set; }

        [LABEL(LanguageEnum.es, "http://xmlns.com/foaf/0.1/homepage")]
        [RDFProperty("http://xmlns.com/foaf/0.1/homepage")]
        public string Foaf_homepage { get; set; }

        [LABEL(LanguageEnum.es, "http://w3id.org/roh/ORCID")]
        [RDFProperty("http://w3id.org/roh/ORCID")]
        public string Roh_ORCID { get; set; }

        [LABEL(LanguageEnum.es, "http://w3id.org/roh/passport")]
        [RDFProperty("http://w3id.org/roh/passport")]
        public string Roh_passport { get; set; }

        [LABEL(LanguageEnum.es, "https://www.w3.org/2006/vcard/ns#birth-date")]
        [RDFProperty("https://www.w3.org/2006/vcard/ns#birth-date")]
        public DateTime? Vcard_birth_date { get; set; }

        [LABEL(LanguageEnum.es, "https://www.w3.org/2006/vcard/ns#email")]
        [RDFProperty("https://www.w3.org/2006/vcard/ns#email")]
        public string Vcard_email { get; set; }

        internal override void GetProperties()
        {
            propList.Add(new StringOntologyProperty("foaf:firstName", this.Foaf_firstName));
            propList.Add(new StringOntologyProperty("foaf:familyName", this.Foaf_familyName));
            propList.Add(new StringOntologyProperty("roh:secondFamilyName", this.Roh_secondFamilyName));
            propList.Add(new StringOntologyProperty("foaf:gender", this.IdFoaf_gender));
            propList.Add(new StringOntologyProperty("roh:nie", this.Roh_nie));
            propList.Add(new StringOntologyProperty("vivo:researcherId", this.Vivo_researcherId));
            propList.Add(new StringOntologyProperty("vivo:scopusId", this.Vivo_scopusId));
            propList.Add(new StringOntologyProperty("foaf:img", this.Foaf_img));
            propList.Add(new StringOntologyProperty("roh:dni", this.Roh_dni));
            propList.Add(new StringOntologyProperty("foaf:homepage", this.Foaf_homepage));
            propList.Add(new StringOntologyProperty("roh:ORCID", this.Roh_ORCID));
            propList.Add(new StringOntologyProperty("roh:passport", this.Roh_passport));
            if (this.Vcard_birth_date.HasValue)
            {
                propList.Add(new DateOntologyProperty("vcard:birth-date", this.Vcard_birth_date.Value));
            }
            propList.Add(new StringOntologyProperty("vcard:email", this.Vcard_email));
        }

        internal override void GetEntities()
        {
            if (Roh_hasFax != null)
            {
                Roh_hasFax.GetProperties();
                Roh_hasFax.GetEntities();
                OntologyEntity entityRoh_hasFax = new("https://www.w3.org/2006/vcard/ns#TelephoneType", "https://www.w3.org/2006/vcard/ns#TelephoneType", "roh:hasFax", Roh_hasFax.propList, Roh_hasFax.entList);
                entList.Add(entityRoh_hasFax);
            }
            if (Roh_birthplace != null)
            {
                Roh_birthplace.GetProperties();
                Roh_birthplace.GetEntities();
                OntologyEntity entityRoh_birthplace = new ("https://www.w3.org/2006/vcard/ns#Address", "https://www.w3.org/2006/vcard/ns#Address", "roh:birthplace", Roh_birthplace.propList, Roh_birthplace.entList);
                entList.Add(entityRoh_birthplace);
            }
            if (Roh_otherIds != null)
            {
                foreach (Document prop in Roh_otherIds)
                {
                    prop.GetProperties();
                    prop.GetEntities();
                    OntologyEntity entityDocument = new ("http://xmlns.com/foaf/0.1/Document", "http://xmlns.com/foaf/0.1/Document", "roh:otherIds", prop.propList, prop.entList);
                    entList.Add(entityDocument);
                    prop.Entity = entityDocument;
                }
            }
            if (Vcard_address != null)
            {
                Vcard_address.GetProperties();
                Vcard_address.GetEntities();
                OntologyEntity entityVcard_address = new ("https://www.w3.org/2006/vcard/ns#Address", "https://www.w3.org/2006/vcard/ns#Address", "vcard:address", Vcard_address.propList, Vcard_address.entList);
                entList.Add(entityVcard_address);
            }
            if (Vcard_hasTelephone != null)
            {

                Vcard_hasTelephone.GetProperties();
                Vcard_hasTelephone.GetEntities();
                OntologyEntity entityVcard_hasTelephone = new ("https://www.w3.org/2006/vcard/ns#TelephoneType", "https://www.w3.org/2006/vcard/ns#TelephoneType", "vcard:hasTelephone", Vcard_hasTelephone.propList, Vcard_hasTelephone.entList);
                entList.Add(entityVcard_hasTelephone);

            }
            if (Roh_hasMobilePhone != null)
            {
                Roh_hasMobilePhone.GetProperties();
                Roh_hasMobilePhone.GetEntities();
                OntologyEntity entityRoh_hasMobilePhone = new ("https://www.w3.org/2006/vcard/ns#TelephoneType", "https://www.w3.org/2006/vcard/ns#TelephoneType", "roh:hasMobilePhone", Roh_hasMobilePhone.propList, Roh_hasMobilePhone.entList);
                entList.Add(entityRoh_hasMobilePhone);
            }
        }
    }
}
