using System;
using System.Collections.Generic;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using GnossBase;
using Gnoss.ApiWrapper.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace FramingsectorOntology
{
    [ExcludeFromCodeCoverage]
    public class FramingSector : GnossOCBase
    {

        public FramingSector() : base() { }

        public virtual string RdfType { get { return "http://w3id.org/roh/FramingSector"; } }
        public virtual string RdfsLabel { get { return "http://w3id.org/roh/FramingSector"; } }
        [RDFProperty("http://purl.org/dc/elements/1.1/title")]
        public Dictionary<LanguageEnum, string> Dc_title { get; set; }

        [RDFProperty("http://purl.org/dc/elements/1.1/identifier")]
        public string Dc_identifier { get; set; }


        internal override void GetProperties()
        {
            base.GetProperties();
            if (this.Dc_title != null)
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

        public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
        {
            KeyValuePair<Guid, string> valor = new();

            return valor;
        }

        public override string GetURI(ResourceApi resourceAPI)
        {
            return $"{resourceAPI.GraphsUrl}items/FramingsectorOntology_{ResourceID}_{ArticleID}";
        }

    }
}
