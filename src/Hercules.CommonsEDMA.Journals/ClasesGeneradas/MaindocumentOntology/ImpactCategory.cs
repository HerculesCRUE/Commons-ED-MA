using System.Linq;
using Gnoss.ApiWrapper.Model;
using GnossBase;
using Es.Riam.Gnoss.Web.MVC.Models;
using System.Diagnostics.CodeAnalysis;

namespace MaindocumentOntology
{
    [ExcludeFromCodeCoverage]
    public class ImpactCategory : GnossOCBase
    {

        public ImpactCategory() : base() { }

        public ImpactCategory(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
        {
            this.mGNOSSID = pSemCmsModel.Entity.Uri;
            this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
            this.Roh_journalNumberInCat = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/journalNumberInCat"));
            this.Roh_publicationPosition = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationPosition"));
            this.Roh_quartile = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/quartile")).Value;
            this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
        }

        public virtual string RdfType { get { return "http://w3id.org/roh/ImpactCategory"; } }
        public virtual string RdfsLabel { get { return "http://w3id.org/roh/ImpactCategory"; } }
        public OntologyEntity Entity { get; set; }

        [RDFProperty("http://w3id.org/roh/journalNumberInCat")]
        public int? Roh_journalNumberInCat { get; set; }

        [RDFProperty("http://w3id.org/roh/publicationPosition")]
        public int? Roh_publicationPosition { get; set; }

        [RDFProperty("http://w3id.org/roh/quartile")]
        public int Roh_quartile { get; set; }

        [RDFProperty("http://w3id.org/roh/title")]
        public string Roh_title { get; set; }


        internal override void GetProperties()
        {
            base.GetProperties();
            propList.Add(new StringOntologyProperty("roh:journalNumberInCat", this.Roh_journalNumberInCat.ToString()));
            propList.Add(new StringOntologyProperty("roh:publicationPosition", this.Roh_publicationPosition.ToString()));
            propList.Add(new StringOntologyProperty("roh:quartile", this.Roh_quartile.ToString()));
            propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
        }

        internal override void GetEntities()
        {
            base.GetEntities();
        }











    }
}
