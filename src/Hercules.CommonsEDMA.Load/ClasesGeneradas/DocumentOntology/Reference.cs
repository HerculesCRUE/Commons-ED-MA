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

namespace DocumentOntology
{
	[ExcludeFromCodeCoverage]
	public class Reference : GnossOCBase
	{

		public Reference() : base() { } 

		public Reference(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_authorList = new List<ReferenceAuthor>();
			SemanticPropertyModel propRoh_authorList = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/authorList");
			if(propRoh_authorList != null && propRoh_authorList.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_authorList.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						ReferenceAuthor roh_authorList = new ReferenceAuthor(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_authorList.Add(roh_authorList);
					}
				}
			}
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Vcard_url = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#url"));
			this.Roh_hasPublicationVenueText = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasPublicationVenueText"));
			this.Bibo_doi = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/doi"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/Reference"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/Reference"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/authorList")]
		public  List<ReferenceAuthor> Roh_authorList { get; set;}

		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime? Dct_issued { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#url")]
		public  string Vcard_url { get; set;}

		[RDFProperty("http://w3id.org/roh/hasPublicationVenueText")]
		public  string Roh_hasPublicationVenueText { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/doi")]
		public  string Bibo_doi { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			if (this.Dct_issued.HasValue){
				propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued.Value));
				}
			propList.Add(new StringOntologyProperty("vcard:url", this.Vcard_url));
			propList.Add(new StringOntologyProperty("roh:hasPublicationVenueText", this.Roh_hasPublicationVenueText));
			propList.Add(new StringOntologyProperty("bibo:doi", this.Bibo_doi));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_authorList!=null){
				foreach(ReferenceAuthor prop in Roh_authorList){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityReferenceAuthor = new OntologyEntity("http://w3id.org/roh/ReferenceAuthor", "http://w3id.org/roh/ReferenceAuthor", "roh:authorList", prop.propList, prop.entList);
				entList.Add(entityReferenceAuthor);
				prop.Entity= entityReferenceAuthor;
				}
			}
		} 











	}
}
