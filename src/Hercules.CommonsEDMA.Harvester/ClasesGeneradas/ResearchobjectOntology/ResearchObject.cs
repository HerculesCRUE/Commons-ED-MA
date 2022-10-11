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
using ResearchObject = ResearchobjectOntology.ResearchObject;
using Project = ProjectOntology.Project;
using Document = DocumentOntology.Document;
using ResearchObjectType = ResearchobjecttypeOntology.ResearchObjectType;

namespace ResearchobjectOntology
{
	[ExcludeFromCodeCoverage]
	public class ResearchObject : GnossOCBase
	{

		public ResearchObject() : base() { } 

		public ResearchObject(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
			this.Roh_annotations = new List<Annotation>();
			SemanticPropertyModel propRoh_annotations = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/annotations");
			if(propRoh_annotations != null && propRoh_annotations.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_annotations.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Annotation roh_annotations = new Annotation(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_annotations.Add(roh_annotations);
					}
				}
			}
			this.Roh_suggestedKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_suggestedKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/suggestedKnowledgeArea");
			if(propRoh_suggestedKnowledgeArea != null && propRoh_suggestedKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_suggestedKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_suggestedKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_suggestedKnowledgeArea.Add(roh_suggestedKnowledgeArea);
					}
				}
			}
			this.Roh_linkedRO = new List<ResearchObject>();
			SemanticPropertyModel propRoh_linkedRO = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/linkedRO");
			if(propRoh_linkedRO != null && propRoh_linkedRO.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_linkedRO.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						ResearchObject roh_linkedRO = new ResearchObject(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_linkedRO.Add(roh_linkedRO);
					}
				}
			}
			this.Vcard_hasLanguage = new List<Language>();
			SemanticPropertyModel propVcard_hasLanguage = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasLanguage");
			if(propVcard_hasLanguage != null && propVcard_hasLanguage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVcard_hasLanguage.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Language vcard_hasLanguage = new Language(propValue.RelatedEntity,idiomaUsuario);
						this.Vcard_hasLanguage.Add(vcard_hasLanguage);
					}
				}
			}
			this.Bibo_authorList = new List<BFO_0000023>();
			SemanticPropertyModel propBibo_authorList = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/authorList");
			if(propBibo_authorList != null && propBibo_authorList.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_authorList.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						BFO_0000023 bibo_authorList = new BFO_0000023(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_authorList.Add(bibo_authorList);
					}
				}
			}
			SemanticPropertyModel propRoh_project = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/project");
			if(propRoh_project != null && propRoh_project.PropertyValues.Count > 0)
			{
				this.Roh_project = new Project(propRoh_project.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_linkedDocument = new List<Document>();
			SemanticPropertyModel propRoh_linkedDocument = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/linkedDocument");
			if(propRoh_linkedDocument != null && propRoh_linkedDocument.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_linkedDocument.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document roh_linkedDocument = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_linkedDocument.Add(roh_linkedDocument);
					}
				}
			}
			this.Roh_userKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_userKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/userKnowledgeArea");
			if(propRoh_userKnowledgeArea != null && propRoh_userKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_userKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_userKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_userKnowledgeArea.Add(roh_userKnowledgeArea);
					}
				}
			}
			this.Roh_externalKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_externalKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/externalKnowledgeArea");
			if(propRoh_externalKnowledgeArea != null && propRoh_externalKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_externalKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_externalKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_externalKnowledgeArea.Add(roh_externalKnowledgeArea);
					}
				}
			}
			SemanticPropertyModel propDc_type = pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/type");
			if(propDc_type != null && propDc_type.PropertyValues.Count > 0)
			{
				this.Dc_type = new ResearchObjectType(propDc_type.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			this.Roh_enrichedKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_enrichedKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/enrichedKnowledgeArea");
			if(propRoh_enrichedKnowledgeArea != null && propRoh_enrichedKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_enrichedKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_enrichedKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_enrichedKnowledgeArea.Add(roh_enrichedKnowledgeArea);
					}
				}
			}
			this.Roh_releasesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/releasesNumber"));
			this.Bibo_abstract = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/abstract"));
			this.Roh_branchesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/branchesNumber"));
			this.Roh_updatedDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/updatedDate"));
			this.Roh_packagesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/packagesNumber"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_forksNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/forksNumber"));
			this.Vcard_url = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#url"));
			this.Roh_idZenodo = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/idZenodo"));
			SemanticPropertyModel propRoh_userKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/userKeywords");
			this.Roh_userKeywords = new List<string>();
			if (propRoh_userKeywords != null && propRoh_userKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_userKeywords.PropertyValues)
				{
					this.Roh_userKeywords.Add(propValue.Value);
				}
			}
			this.Roh_idGit = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/idGit"));
			SemanticPropertyModel propRoh_externalKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/externalKeywords");
			this.Roh_externalKeywords = new List<string>();
			if (propRoh_externalKeywords != null && propRoh_externalKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_externalKeywords.PropertyValues)
				{
					this.Roh_externalKeywords.Add(propValue.Value);
				}
			}
			this.Roh_linkedCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/linkedCount"));
			this.Dct_license = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/license"));
			SemanticPropertyModel propVivo_freeTextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeyword");
			this.Vivo_freeTextKeyword = new List<string>();
			if (propVivo_freeTextKeyword != null && propVivo_freeTextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeyword.PropertyValues)
				{
					this.Vivo_freeTextKeyword.Add(propValue.Value);
				}
			}
			this.Roh_resolvedIssuesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/resolvedIssuesNumber"));
			this.Bibo_doi = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/doi"));
			this.Roh_issuesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/issuesNumber"));
			this.Roh_idFigShare = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/idFigShare"));
			SemanticPropertyModel propRoh_suggestedKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/suggestedKeywords");
			this.Roh_suggestedKeywords = new List<string>();
			if (propRoh_suggestedKeywords != null && propRoh_suggestedKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_suggestedKeywords.PropertyValues)
				{
					this.Roh_suggestedKeywords.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propRoh_enrichedKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/enrichedKeywords");
			this.Roh_enrichedKeywords = new List<string>();
			if (propRoh_enrichedKeywords != null && propRoh_enrichedKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_enrichedKeywords.PropertyValues)
				{
					this.Roh_enrichedKeywords.Add(propValue.Value);
				}
			}
			this.Roh_viewsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/viewsNumber"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
			this.Roh_isValidated= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isValidated"));
			this.Roh_citationLoadedCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/citationLoadedCount")).Value;
		}

		public ResearchObject(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
			this.Roh_annotations = new List<Annotation>();
			SemanticPropertyModel propRoh_annotations = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/annotations");
			if(propRoh_annotations != null && propRoh_annotations.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_annotations.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Annotation roh_annotations = new Annotation(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_annotations.Add(roh_annotations);
					}
				}
			}
			this.Roh_suggestedKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_suggestedKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/suggestedKnowledgeArea");
			if(propRoh_suggestedKnowledgeArea != null && propRoh_suggestedKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_suggestedKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_suggestedKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_suggestedKnowledgeArea.Add(roh_suggestedKnowledgeArea);
					}
				}
			}
			this.Roh_linkedRO = new List<ResearchObject>();
			SemanticPropertyModel propRoh_linkedRO = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/linkedRO");
			if(propRoh_linkedRO != null && propRoh_linkedRO.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_linkedRO.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						ResearchObject roh_linkedRO = new ResearchObject(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_linkedRO.Add(roh_linkedRO);
					}
				}
			}
			this.Vcard_hasLanguage = new List<Language>();
			SemanticPropertyModel propVcard_hasLanguage = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasLanguage");
			if(propVcard_hasLanguage != null && propVcard_hasLanguage.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVcard_hasLanguage.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Language vcard_hasLanguage = new Language(propValue.RelatedEntity,idiomaUsuario);
						this.Vcard_hasLanguage.Add(vcard_hasLanguage);
					}
				}
			}
			this.Bibo_authorList = new List<BFO_0000023>();
			SemanticPropertyModel propBibo_authorList = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/authorList");
			if(propBibo_authorList != null && propBibo_authorList.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_authorList.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						BFO_0000023 bibo_authorList = new BFO_0000023(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_authorList.Add(bibo_authorList);
					}
				}
			}
			SemanticPropertyModel propRoh_project = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/project");
			if(propRoh_project != null && propRoh_project.PropertyValues.Count > 0)
			{
				this.Roh_project = new Project(propRoh_project.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_linkedDocument = new List<Document>();
			SemanticPropertyModel propRoh_linkedDocument = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/linkedDocument");
			if(propRoh_linkedDocument != null && propRoh_linkedDocument.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_linkedDocument.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document roh_linkedDocument = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_linkedDocument.Add(roh_linkedDocument);
					}
				}
			}
			this.Roh_userKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_userKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/userKnowledgeArea");
			if(propRoh_userKnowledgeArea != null && propRoh_userKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_userKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_userKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_userKnowledgeArea.Add(roh_userKnowledgeArea);
					}
				}
			}
			this.Roh_externalKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_externalKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/externalKnowledgeArea");
			if(propRoh_externalKnowledgeArea != null && propRoh_externalKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_externalKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_externalKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_externalKnowledgeArea.Add(roh_externalKnowledgeArea);
					}
				}
			}
			SemanticPropertyModel propDc_type = pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/type");
			if(propDc_type != null && propDc_type.PropertyValues.Count > 0)
			{
				this.Dc_type = new ResearchObjectType(propDc_type.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			this.Roh_enrichedKnowledgeArea = new List<CategoryPath>();
			SemanticPropertyModel propRoh_enrichedKnowledgeArea = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/enrichedKnowledgeArea");
			if(propRoh_enrichedKnowledgeArea != null && propRoh_enrichedKnowledgeArea.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_enrichedKnowledgeArea.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						CategoryPath roh_enrichedKnowledgeArea = new CategoryPath(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_enrichedKnowledgeArea.Add(roh_enrichedKnowledgeArea);
					}
				}
			}
			this.Roh_releasesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/releasesNumber"));
			this.Bibo_abstract = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/abstract"));
			this.Roh_branchesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/branchesNumber"));
			this.Roh_updatedDate = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/updatedDate"));
			this.Roh_packagesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/packagesNumber"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_forksNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/forksNumber"));
			this.Vcard_url = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#url"));
			this.Roh_idZenodo = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/idZenodo"));
			SemanticPropertyModel propRoh_userKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/userKeywords");
			this.Roh_userKeywords = new List<string>();
			if (propRoh_userKeywords != null && propRoh_userKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_userKeywords.PropertyValues)
				{
					this.Roh_userKeywords.Add(propValue.Value);
				}
			}
			this.Roh_idGit = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/idGit"));
			SemanticPropertyModel propRoh_externalKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/externalKeywords");
			this.Roh_externalKeywords = new List<string>();
			if (propRoh_externalKeywords != null && propRoh_externalKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_externalKeywords.PropertyValues)
				{
					this.Roh_externalKeywords.Add(propValue.Value);
				}
			}
			this.Roh_linkedCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/linkedCount"));
			this.Dct_license = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/license"));
			SemanticPropertyModel propVivo_freeTextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeyword");
			this.Vivo_freeTextKeyword = new List<string>();
			if (propVivo_freeTextKeyword != null && propVivo_freeTextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeyword.PropertyValues)
				{
					this.Vivo_freeTextKeyword.Add(propValue.Value);
				}
			}
			this.Roh_resolvedIssuesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/resolvedIssuesNumber"));
			this.Bibo_doi = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/doi"));
			this.Roh_issuesNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/issuesNumber"));
			this.Roh_idFigShare = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/idFigShare"));
			SemanticPropertyModel propRoh_suggestedKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/suggestedKeywords");
			this.Roh_suggestedKeywords = new List<string>();
			if (propRoh_suggestedKeywords != null && propRoh_suggestedKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_suggestedKeywords.PropertyValues)
				{
					this.Roh_suggestedKeywords.Add(propValue.Value);
				}
			}
			SemanticPropertyModel propRoh_enrichedKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/enrichedKeywords");
			this.Roh_enrichedKeywords = new List<string>();
			if (propRoh_enrichedKeywords != null && propRoh_enrichedKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_enrichedKeywords.PropertyValues)
				{
					this.Roh_enrichedKeywords.Add(propValue.Value);
				}
			}
			this.Roh_viewsNumber = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/viewsNumber"));
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
			this.Roh_isValidated= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isValidated"));
			this.Roh_citationLoadedCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/citationLoadedCount")).Value;
		}

		public virtual string RdfType { get { return "http://w3id.org/roh/ResearchObject"; } }
		public virtual string RdfsLabel { get { return "http://w3id.org/roh/ResearchObject"; } }
		[RDFProperty("http://w3id.org/roh/annotations")]
		public  List<Annotation> Roh_annotations { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/suggestedKnowledgeArea")]
		[RDFProperty("http://w3id.org/roh/suggestedKnowledgeArea")]
		public  List<CategoryPath> Roh_suggestedKnowledgeArea { get; set;}

		[RDFProperty("http://w3id.org/roh/linkedRO")]
		public  List<ResearchObject> Roh_linkedRO { get; set;}
		public List<string> IdsRoh_linkedRO { get; set;}

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#hasLanguage")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasLanguage")]
		public  List<Language> Vcard_hasLanguage { get; set;}

		[LABEL(LanguageEnum.es,"http://purl.org/ontology/bibo/authorList")]
		[RDFProperty("http://purl.org/ontology/bibo/authorList")]
		public  List<BFO_0000023> Bibo_authorList { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/project")]
		[RDFProperty("http://w3id.org/roh/project")]
		public  Project Roh_project  { get; set;} 
		public string IdRoh_project  { get; set;} 

		[RDFProperty("http://w3id.org/roh/linkedDocument")]
		public  List<Document> Roh_linkedDocument { get; set;}
		public List<string> IdsRoh_linkedDocument { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/userKnowledgeArea")]
		[RDFProperty("http://w3id.org/roh/userKnowledgeArea")]
		public  List<CategoryPath> Roh_userKnowledgeArea { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/externalKnowledgeArea")]
		[RDFProperty("http://w3id.org/roh/externalKnowledgeArea")]
		public  List<CategoryPath> Roh_externalKnowledgeArea { get; set;}

		[LABEL(LanguageEnum.es,"http://purl.org/dc/elements/1.1/type")]
		[RDFProperty("http://purl.org/dc/elements/1.1/type")]
		public  ResearchObjectType Dc_type  { get; set;} 
		public string IdDc_type  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/hasKnowledgeArea")]
		[RDFProperty("http://w3id.org/roh/hasKnowledgeArea")]
		public  List<CategoryPath> Roh_hasKnowledgeArea { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/enrichedKnowledgeArea")]
		[RDFProperty("http://w3id.org/roh/enrichedKnowledgeArea")]
		public  List<CategoryPath> Roh_enrichedKnowledgeArea { get; set;}

		[RDFProperty("http://w3id.org/roh/releasesNumber")]
		public  int? Roh_releasesNumber { get; set;}

		[LABEL(LanguageEnum.es,"http://purl.org/ontology/bibo/abstract")]
		[RDFProperty("http://purl.org/ontology/bibo/abstract")]
		public  string Bibo_abstract { get; set;}

		[RDFProperty("http://w3id.org/roh/branchesNumber")]
		public  int? Roh_branchesNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/updatedDate")]
		public  DateTime? Roh_updatedDate { get; set;}

		[RDFProperty("http://w3id.org/roh/packagesNumber")]
		public  int? Roh_packagesNumber { get; set;}

		[LABEL(LanguageEnum.es,"http://purl.org/dc/terms/issued")]
		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime? Dct_issued { get; set;}

		[RDFProperty("http://w3id.org/roh/forksNumber")]
		public  int? Roh_forksNumber { get; set;}

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#url")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#url")]
		public  string Vcard_url { get; set;}

		[RDFProperty("http://w3id.org/roh/idZenodo")]
		public  string Roh_idZenodo { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/userKeywords")]
		[RDFProperty("http://w3id.org/roh/userKeywords")]
		public  List<string> Roh_userKeywords { get; set;}

		[RDFProperty("http://w3id.org/roh/idGit")]
		public  string Roh_idGit { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/externalKeywords")]
		[RDFProperty("http://w3id.org/roh/externalKeywords")]
		public  List<string> Roh_externalKeywords { get; set;}

		[RDFProperty("http://w3id.org/roh/linkedCount")]
		public  int? Roh_linkedCount { get; set;}

		[RDFProperty("http://purl.org/dc/terms/license")]
		public  string Dct_license { get; set;}

		[LABEL(LanguageEnum.es,"http://vivoweb.org/ontology/core#freeTextKeyword")]
		[RDFProperty("http://vivoweb.org/ontology/core#freeTextKeyword")]
		public  List<string> Vivo_freeTextKeyword { get; set;}

		[RDFProperty("http://w3id.org/roh/resolvedIssuesNumber")]
		public  int? Roh_resolvedIssuesNumber { get; set;}

		[LABEL(LanguageEnum.es,"http://purl.org/ontology/bibo/doi")]
		[RDFProperty("http://purl.org/ontology/bibo/doi")]
		public  string Bibo_doi { get; set;}

		[RDFProperty("http://w3id.org/roh/issuesNumber")]
		public  int? Roh_issuesNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/idFigShare")]
		public  string Roh_idFigShare { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/suggestedKeywords")]
		[RDFProperty("http://w3id.org/roh/suggestedKeywords")]
		public  List<string> Roh_suggestedKeywords { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/enrichedKeywords")]
		[RDFProperty("http://w3id.org/roh/enrichedKeywords")]
		public  List<string> Roh_enrichedKeywords { get; set;}

		[RDFProperty("http://w3id.org/roh/viewsNumber")]
		public  int? Roh_viewsNumber { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/title")]
		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/isValidated")]
		[RDFProperty("http://w3id.org/roh/isValidated")]
		public  bool Roh_isValidated { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/citationLoadedCount")]
		[RDFProperty("http://w3id.org/roh/citationLoadedCount")]
		public  int Roh_citationLoadedCount { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new ListStringOntologyProperty("roh:linkedRO", this.IdsRoh_linkedRO));
			propList.Add(new StringOntologyProperty("roh:project", this.IdRoh_project));
			propList.Add(new ListStringOntologyProperty("roh:linkedDocument", this.IdsRoh_linkedDocument));
			propList.Add(new StringOntologyProperty("dc:type", this.IdDc_type));
			propList.Add(new StringOntologyProperty("roh:releasesNumber", this.Roh_releasesNumber.ToString()));
			propList.Add(new StringOntologyProperty("bibo:abstract", this.Bibo_abstract));
			propList.Add(new StringOntologyProperty("roh:branchesNumber", this.Roh_branchesNumber.ToString()));
			if (this.Roh_updatedDate.HasValue){
				propList.Add(new DateOntologyProperty("roh:updatedDate", this.Roh_updatedDate.Value));
				}
			propList.Add(new StringOntologyProperty("roh:packagesNumber", this.Roh_packagesNumber.ToString()));
			if (this.Dct_issued.HasValue){
				propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued.Value));
				}
			propList.Add(new StringOntologyProperty("roh:forksNumber", this.Roh_forksNumber.ToString()));
			propList.Add(new StringOntologyProperty("vcard:url", this.Vcard_url));
			propList.Add(new StringOntologyProperty("roh:idZenodo", this.Roh_idZenodo));
			propList.Add(new ListStringOntologyProperty("roh:userKeywords", this.Roh_userKeywords));
			propList.Add(new StringOntologyProperty("roh:idGit", this.Roh_idGit));
			propList.Add(new ListStringOntologyProperty("roh:externalKeywords", this.Roh_externalKeywords));
			propList.Add(new StringOntologyProperty("roh:linkedCount", this.Roh_linkedCount.ToString()));
			propList.Add(new StringOntologyProperty("dct:license", this.Dct_license));
			propList.Add(new ListStringOntologyProperty("vivo:freeTextKeyword", this.Vivo_freeTextKeyword));
			propList.Add(new StringOntologyProperty("roh:resolvedIssuesNumber", this.Roh_resolvedIssuesNumber.ToString()));
			propList.Add(new StringOntologyProperty("bibo:doi", this.Bibo_doi));
			propList.Add(new StringOntologyProperty("roh:issuesNumber", this.Roh_issuesNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:idFigShare", this.Roh_idFigShare));
			propList.Add(new ListStringOntologyProperty("roh:suggestedKeywords", this.Roh_suggestedKeywords));
			propList.Add(new ListStringOntologyProperty("roh:enrichedKeywords", this.Roh_enrichedKeywords));
			propList.Add(new StringOntologyProperty("roh:viewsNumber", this.Roh_viewsNumber.ToString()));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
			propList.Add(new BoolOntologyProperty("roh:isValidated", this.Roh_isValidated));
			propList.Add(new StringOntologyProperty("roh:citationLoadedCount", this.Roh_citationLoadedCount.ToString()));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_annotations!=null){
				foreach(Annotation prop in Roh_annotations){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityAnnotation = new OntologyEntity("http://w3id.org/roh/Annotation", "http://w3id.org/roh/Annotation", "roh:annotations", prop.propList, prop.entList);
				entList.Add(entityAnnotation);
				prop.Entity= entityAnnotation;
				}
			}
			if(Roh_suggestedKnowledgeArea!=null){
				foreach(CategoryPath prop in Roh_suggestedKnowledgeArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:suggestedKnowledgeArea", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
			if(Vcard_hasLanguage!=null){
				foreach(Language prop in Vcard_hasLanguage){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityLanguage = new OntologyEntity("http://w3id.org/roh/Language", "http://w3id.org/roh/Language", "vcard:hasLanguage", prop.propList, prop.entList);
				entList.Add(entityLanguage);
				prop.Entity= entityLanguage;
				}
			}
			if(Bibo_authorList!=null){
				foreach(BFO_0000023 prop in Bibo_authorList){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityBFO_0000023 = new OntologyEntity("http://purl.obolibrary.org/obo/BFO_0000023", "http://purl.obolibrary.org/obo/BFO_0000023", "bibo:authorList", prop.propList, prop.entList);
				entList.Add(entityBFO_0000023);
				prop.Entity= entityBFO_0000023;
				}
			}
			if(Roh_userKnowledgeArea!=null){
				foreach(CategoryPath prop in Roh_userKnowledgeArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:userKnowledgeArea", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
			if(Roh_externalKnowledgeArea!=null){
				foreach(CategoryPath prop in Roh_externalKnowledgeArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:externalKnowledgeArea", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
			if(Roh_hasKnowledgeArea!=null){
				foreach(CategoryPath prop in Roh_hasKnowledgeArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:hasKnowledgeArea", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
			if(Roh_enrichedKnowledgeArea!=null){
				foreach(CategoryPath prop in Roh_enrichedKnowledgeArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:enrichedKnowledgeArea", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
				}
			}
		} 
		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias)
		{
			return ToGnossApiResource(resourceAPI, listaDeCategorias, Guid.Empty, Guid.Empty);
		}

		public virtual ComplexOntologyResource ToGnossApiResource(ResourceApi resourceAPI, List<string> listaDeCategorias, Guid idrecurso, Guid idarticulo)
		{
			ComplexOntologyResource resource = new ComplexOntologyResource();
			Ontology ontology=null;
			GetEntities();
			GetProperties();
			if(idrecurso.Equals(Guid.Empty) && idarticulo.Equals(Guid.Empty))
			{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList);
			}
			else{
				ontology = new Ontology(resourceAPI.GraphsUrl, resourceAPI.OntologyUrl, RdfType, RdfsLabel, prefList, propList, entList,idrecurso,idarticulo);
			}
			resource.Id = GNOSSID;
			resource.Ontology = ontology;
			resource.TextCategories=listaDeCategorias;
			AddResourceTitle(resource);
			AddResourceDescription(resource);
			AddImages(resource);
			AddFiles(resource);
			return resource;
		}

		public override List<string> ToOntologyGnossTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ResearchObject>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ResearchObject\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_annotations != null)
			{
			foreach(var item0 in this.Roh_annotations)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Annotation>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Annotation\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/annotations", $"<{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{item0.Dct_issued.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(item0.Roh_annotation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/annotation", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_annotation)}\"", list, " . ");
				}
				if(item0.IdRdf_member != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Annotation_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#member", $"<{item0.IdRdf_member}>", list, " . ");
				}
			}
			}
			if(this.Roh_suggestedKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_suggestedKnowledgeArea)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/suggestedKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_userKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_userKnowledgeArea)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/userKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_externalKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_externalKnowledgeArea)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/externalKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_hasKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_hasKnowledgeArea)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/hasKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_enrichedKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_enrichedKnowledgeArea)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/enrichedKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{item2}>", list, " . ");
					}
				}
			}
			}
			if(this.Vcard_hasLanguage != null)
			{
			foreach(var item0 in this.Vcard_hasLanguage)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Language_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Language>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Language_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Language\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Language_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "https://www.w3.org/2006/vcard/ns#hasLanguage", $"<{resourceAPI.GraphsUrl}items/Language_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_percentage != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Language_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/percentage", $"{item0.Roh_percentage.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Language_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title)}\"", list, " . ");
				}
			}
			}
			if(this.Bibo_authorList != null)
			{
			foreach(var item0 in this.Bibo_authorList)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://purl.obolibrary.org/obo/BFO_0000023>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://purl.obolibrary.org/obo/BFO_0000023\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://purl.org/ontology/bibo/authorList", $"<{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick)}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
				}
				if(item0.IdRdf_member != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#member", $"<{item0.IdRdf_member}>", list, " . ");
				}
			}
			}
				if(this.IdsRoh_linkedRO != null)
				{
					foreach(var item2 in this.IdsRoh_linkedRO)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/linkedRO", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdRoh_project != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/project", $"<{this.IdRoh_project}>", list, " . ");
				}
				if(this.IdsRoh_linkedDocument != null)
				{
					foreach(var item2 in this.IdsRoh_linkedDocument)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/linkedDocument", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdDc_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/type", $"<{this.IdDc_type}>", list, " . ");
				}
				if(this.Roh_releasesNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/releasesNumber", $"{this.Roh_releasesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_abstract != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/abstract", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_abstract)}\"", list, " . ");
				}
				if(this.Roh_branchesNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/branchesNumber", $"{this.Roh_branchesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_updatedDate != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/updatedDate", $"\"{this.Roh_updatedDate.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_packagesNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/packagesNumber", $"{this.Roh_packagesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_forksNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/forksNumber", $"{this.Roh_forksNumber.Value.ToString()}", list, " . ");
				}
				if(this.Vcard_url != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#url", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_url)}\"", list, " . ");
				}
				if(this.Roh_idZenodo != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/idZenodo", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_idZenodo)}\"", list, " . ");
				}
				if(this.Roh_userKeywords != null)
				{
					foreach(var item2 in this.Roh_userKeywords)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/userKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_idGit != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/idGit", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_idGit)}\"", list, " . ");
				}
				if(this.Roh_externalKeywords != null)
				{
					foreach(var item2 in this.Roh_externalKeywords)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/externalKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_linkedCount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/linkedCount", $"{this.Roh_linkedCount.Value.ToString()}", list, " . ");
				}
				if(this.Dct_license != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://purl.org/dc/terms/license", $"\"{GenerarTextoSinSaltoDeLinea(this.Dct_license)}\"", list, " . ");
				}
				if(this.Vivo_freeTextKeyword != null)
				{
					foreach(var item2 in this.Vivo_freeTextKeyword)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_resolvedIssuesNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/resolvedIssuesNumber", $"{this.Roh_resolvedIssuesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_doi != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_doi)}\"", list, " . ");
				}
				if(this.Roh_issuesNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/issuesNumber", $"{this.Roh_issuesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_idFigShare != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/idFigShare", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_idFigShare)}\"", list, " . ");
				}
				if(this.Roh_suggestedKeywords != null)
				{
					foreach(var item2 in this.Roh_suggestedKeywords)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/suggestedKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_enrichedKeywords != null)
				{
					foreach(var item2 in this.Roh_enrichedKeywords)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}", "http://w3id.org/roh/enrichedKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_viewsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/viewsNumber", $"{this.Roh_viewsNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
				if(this.Roh_isValidated != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isValidated", $"\"{this.Roh_isValidated.ToString()}\"", list, " . ");
				}
				if(this.Roh_citationLoadedCount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ResearchObject_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/citationLoadedCount", $"{this.Roh_citationLoadedCount.ToString()}", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"researchobject\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://w3id.org/roh/ResearchObject\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
			if(this.Roh_annotations != null)
			{
			foreach(var item0 in this.Roh_annotations)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/annotations", $"<{resourceAPI.GraphsUrl}items/annotation_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/annotation_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/terms/issued", $"{item0.Dct_issued.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(item0.Roh_annotation != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/annotation_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/annotation", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_annotation).ToLower()}\"", list, " . ");
				}
				if(item0.IdRdf_member != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRdf_member;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/annotation_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#member", $"<{itemRegex}>", list, " . ");
				}
			}
			}
			if(this.Roh_suggestedKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_suggestedKnowledgeArea)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/suggestedKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_userKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_userKnowledgeArea)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/userKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_externalKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_externalKnowledgeArea)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/externalKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_hasKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_hasKnowledgeArea)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/hasKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Roh_enrichedKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_enrichedKnowledgeArea)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/enrichedKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/categorypath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{itemRegex}>", list, " . ");
					}
				}
			}
			}
			if(this.Vcard_hasLanguage != null)
			{
			foreach(var item0 in this.Vcard_hasLanguage)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://www.w3.org/2006/vcard/ns#hasLanguage", $"<{resourceAPI.GraphsUrl}items/language_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_percentage != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/language_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/percentage", $"{item0.Roh_percentage.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/language_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Bibo_authorList != null)
			{
			foreach(var item0 in this.Bibo_authorList)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/ontology/bibo/authorList", $"<{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick).ToLower()}\"", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
				}
				if(item0.IdRdf_member != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRdf_member;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#member", $"<{itemRegex}>", list, " . ");
				}
			}
			}
				if(this.IdsRoh_linkedRO != null)
				{
					foreach(var item2 in this.IdsRoh_linkedRO)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/linkedRO", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdRoh_project != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_project;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/project", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsRoh_linkedDocument != null)
				{
					foreach(var item2 in this.IdsRoh_linkedDocument)
					{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item2;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/linkedDocument", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdDc_type != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdDc_type;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/elements/1.1/type", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_releasesNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/releasesNumber", $"{this.Roh_releasesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_abstract != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/abstract", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_abstract).ToLower()}\"", list, " . ");
				}
				if(this.Roh_branchesNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/branchesNumber", $"{this.Roh_branchesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_updatedDate != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/updatedDate", $"{this.Roh_updatedDate.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_packagesNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/packagesNumber", $"{this.Roh_packagesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/terms/issued", $"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_forksNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/forksNumber", $"{this.Roh_forksNumber.Value.ToString()}", list, " . ");
				}
				if(this.Vcard_url != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#url", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_url).ToLower()}\"", list, " . ");
				}
				if(this.Roh_idZenodo != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/idZenodo", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_idZenodo).ToLower()}\"", list, " . ");
				}
				if(this.Roh_userKeywords != null)
				{
					foreach(var item2 in this.Roh_userKeywords)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/userKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_idGit != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/idGit", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_idGit).ToLower()}\"", list, " . ");
				}
				if(this.Roh_externalKeywords != null)
				{
					foreach(var item2 in this.Roh_externalKeywords)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/externalKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_linkedCount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/linkedCount", $"{this.Roh_linkedCount.Value.ToString()}", list, " . ");
				}
				if(this.Dct_license != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/terms/license", $"\"{GenerarTextoSinSaltoDeLinea(this.Dct_license).ToLower()}\"", list, " . ");
				}
				if(this.Vivo_freeTextKeyword != null)
				{
					foreach(var item2 in this.Vivo_freeTextKeyword)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_resolvedIssuesNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/resolvedIssuesNumber", $"{this.Roh_resolvedIssuesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_doi != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_doi).ToLower()}\"", list, " . ");
				}
				if(this.Roh_issuesNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/issuesNumber", $"{this.Roh_issuesNumber.Value.ToString()}", list, " . ");
				}
				if(this.Roh_idFigShare != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/idFigShare", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_idFigShare).ToLower()}\"", list, " . ");
				}
				if(this.Roh_suggestedKeywords != null)
				{
					foreach(var item2 in this.Roh_suggestedKeywords)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/suggestedKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_enrichedKeywords != null)
				{
					foreach(var item2 in this.Roh_enrichedKeywords)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/enrichedKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_viewsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/viewsNumber", $"{this.Roh_viewsNumber.Value.ToString()}", list, " . ");
				}
				if(!string.IsNullOrEmpty(this.Roh_title))
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title).ToLower()}\"", list, " . ");
					search += $"{this.Roh_title} ";
				}
				if(this.Roh_isValidated != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isValidated", $"\"{this.Roh_isValidated.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_citationLoadedCount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/citationLoadedCount", $"{this.Roh_citationLoadedCount.ToString()}", list, " . ");
				}
			if (listaSearch != null && listaSearch.Count > 0)
			{
				foreach(string valorSearch in listaSearch)
				{
					search += $"{valorSearch} ";
				}
			}
			if(!string.IsNullOrEmpty(search))
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/search", $"\"{GenerarTextoSinSaltoDeLinea(search.ToLower())}\"", list, " . ");
			}
			return list;
		}

		public override KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI)
		{

			//Insert en la tabla Documento
			string tags = "";
			foreach(string tag in tagList)
			{
				tags += $"{tag}, ";
			}
			if (!string.IsNullOrEmpty(tags))
			{
				tags = tags.Substring(0, tags.LastIndexOf(','));
			}
			string titulo = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "#COMILLA#").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/ResearchobjectOntology_{ResourceID}_{ArticleID}";
		}


		internal void AddResourceTitle(ComplexOntologyResource resource)
		{
			resource.Title = this.Roh_title;
		}

		internal void AddResourceDescription(ComplexOntologyResource resource)
		{
			resource.Description = this.Roh_title;
		}




	}
}
