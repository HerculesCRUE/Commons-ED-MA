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
    public class PersonalData : GnossOCBase
    {
        public PersonalData() : base() { }
        public virtual string RdfType { get { return $"{ActualizadorBase.GetUrlPrefix("roh")}PersonalData"; } }
        public virtual string RdfsLabel { get { return $"{ActualizadorBase.GetUrlPrefix("roh")}PersonalData"; } }
        public OntologyEntity Entity { get; set; }
        public string Foaf_firstName { get; set; }
        public string Foaf_familyName { get; set; }
        public string Roh_secondFamilyName { get; set; }
        public TelephoneType Roh_hasFax { get; set; }
        public Address Roh_birthplace { get; set; }
        public List<Document> Roh_otherIds { get; set; }
        public Address Vcard_address { get; set; }
        public string IdFoaf_gender { get; set; }
        public TelephoneType Vcard_hasTelephone { get; set; }
        public TelephoneType Roh_hasMobilePhone { get; set; }
        public string Roh_nie { get; set; }
        public string Vivo_researcherId { get; set; }
        public string Vivo_scopusId { get; set; }
        public string Foaf_img { get; set; }
        public string Roh_dni { get; set; }
        public string Foaf_homepage { get; set; }
        public string Roh_ORCID { get; set; }
        public string Roh_passport { get; set; }
        public DateTime? Vcard_birth_date { get; set; }
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
