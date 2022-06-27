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
using Person = PersonOntology.Person;

namespace ClusterOntology
{
	[ExcludeFromCodeCoverage]
	public class ClusterPerfil : GnossOCBase
	{

		public ClusterPerfil() : base() { } 

		public ClusterPerfil(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Rdf_member = new List<Person>();
			SemanticPropertyModel propRdf_member = pSemCmsModel.GetPropertyByPath("http://www.w3.org/1999/02/22-rdf-syntax-ns#member");
			if(propRdf_member != null && propRdf_member.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRdf_member.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Person rdf_member = new Person(propValue.RelatedEntity,idiomaUsuario);
						this.Rdf_member.Add(rdf_member);
					}
				}
			}
			this.Roh_hasKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_hasKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasKnowledgeArea");
			if(propRoh_hasKnowledgeArea != null && propRoh_hasKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_hasKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_hasKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_hasKnowledgeArea.Add(roh_hasKnowledgeArea);
					}
				}
			}
			SemanticPropertyModel propVivo_freeTextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeyword");
			this.Vivo_freeTextKeyword = new List<string>();
			if (propVivo_freeTextKeyword != null && propVivo_freeTextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeyword.PropertyValues)
				{
					this.Vivo_freeTextKeyword.Add(propValue.Value);
				}
			}
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ClusterPerfil"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ClusterPerfil"; } }
		public OntologyEntity Entity { get; set; }

		[LABEL(LanguageEnum.es,"http://www.w3.org/1999/02/22-rdf-syntax-ns#member")]
		[RDFProperty("http://www.w3.org/1999/02/22-rdf-syntax-ns#member")]
		public  List<Person> Rdf_member { get; set;}
		public List<string> IdsRdf_member { get; set;}

		[RDFProperty("http://w3id.org/roh/hasKnowledgeArea")]
		public  List<CategoryPath> Roh_hasKnowledgeArea { get; set;}

		[RDFProperty("http://vivoweb.org/ontology/core#freeTextKeyword")]
		public  List<string> Vivo_freeTextKeyword { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("rdf:member", this.IdsRdf_member));
			propList.Add(new ListStringOntologyProperty("vivo:freeTextKeyword", this.Vivo_freeTextKeyword));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_hasKnowledgeArea!=null){
				foreach(CategoryPath prop in Roh_hasKnowledgeArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:hasKnowledgeArea", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
		} 











	}
}
