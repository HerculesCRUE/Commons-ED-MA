using System;
using System.ComponentModel.DataAnnotations;
using Gnoss.ApiWrapper.Model;
using GnossBase;
using System.Diagnostics.CodeAnalysis;
using OfferState = OfferstateOntology.OfferState;

namespace OfferOntology
{
    [ExcludeFromCodeCoverage]
    public class AvailabilityChangeEvent : GnossOCBase
    {

        public AvailabilityChangeEvent() : base() { }

        public virtual string RdfType { get { return "http://w3id.org/roh/AvailabilityChangeEvent"; } }
        public virtual string RdfsLabel { get { return "http://w3id.org/roh/AvailabilityChangeEvent"; } }
        public OntologyEntity Entity { get; set; }

        public string IdRoh_roleOf { get; set; }

        [RDFProperty("http://www.schema.org/availability")]
        [Required]
        public OfferState Schema_availability { get; set; }
        public string IdSchema_availability { get; set; }

        [RDFProperty("http://www.schema.org/validFrom")]
        public DateTime Schema_validFrom { get; set; }


        internal override void GetProperties()
        {
            base.GetProperties();
            propList.Add(new StringOntologyProperty("roh:roleOf", this.IdRoh_roleOf));
            propList.Add(new StringOntologyProperty("schema:availability", this.IdSchema_availability));
            propList.Add(new DateOntologyProperty("schema:validFrom", this.Schema_validFrom));
        }

    }
}
