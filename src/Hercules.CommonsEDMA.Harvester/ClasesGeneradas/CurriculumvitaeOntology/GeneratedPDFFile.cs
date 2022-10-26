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
	public class GeneratedPDFFile : GnossOCBase
	{

		public GeneratedPDFFile() : base() { } 

		public GeneratedPDFFile(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_filePDF = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/filePDF"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
var item0 = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
if(item0.HasValue){
			this.Dct_issued = item0.Value;
}
			this.Roh_status = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/status"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/GeneratedPDFFile"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/GeneratedPDFFile"; } }
		public OntologyEntity Entity { get; set; }

		[RDFProperty("http://w3id.org/roh/filePDF")]
		public  string Roh_filePDF { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}

		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime Dct_issued { get; set;}

		[RDFProperty("http://w3id.org/roh/status")]
		public  string Roh_status { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("roh:filePDF", this.Roh_filePDF));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
			propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued));
			propList.Add(new StringOntologyProperty("roh:status", this.Roh_status));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
		} 











		internal override void AddFiles(ComplexOntologyResource pResource)
		{
			base.AddFiles(pResource);
			FileStream fileRoh_filePDF = System.IO.File.Create(Roh_filePDF);
			long lengthRoh_filePDF = fileRoh_filePDF.Length;
			byte[] dataRoh_filePDF = new byte[lengthRoh_filePDF];
			fileRoh_filePDF.Read(dataRoh_filePDF, 0, Convert.ToInt32(lengthRoh_filePDF));
			string nameRoh_filePDF = Roh_filePDF;
			pResource.AttachFile(dataRoh_filePDF, "http://w3id.org/roh/filePDF", nameRoh_filePDF);
			fileRoh_filePDF.Dispose();
		}
	}
}
