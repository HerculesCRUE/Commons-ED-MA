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
using Feature = FeatureOntology.Feature;
using DocumentFormat = DocumentformatOntology.DocumentFormat;
using Organization = OrganizationOntology.Organization;
using OrganizationType = OrganizationtypeOntology.OrganizationType;
using EventType = EventtypeOntology.EventType;
using Project = ProjectOntology.Project;
using Language = LanguageOntology.Language;
using GeographicRegion = GeographicregionOntology.GeographicRegion;
using Group = GroupOntology.Group;
using PublicationType = PublicationtypeOntology.PublicationType;
using SeminarEventType = SeminareventtypeOntology.SeminarEventType;
using Document = DocumentOntology.Document;
using MainDocument = MaindocumentOntology.MainDocument;
using ScientificActivityDocument = ScientificactivitydocumentOntology.ScientificActivityDocument;

namespace DocumentOntology
{
	[ExcludeFromCodeCoverage]
	public class Document : GnossOCBase
	{

		public Document() : base() { } 

		public Document(SemanticResourceModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.RootEntities[0].Entity.Uri;
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
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_supportType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/supportType");
			if(propRoh_supportType != null && propRoh_supportType.PropertyValues.Count > 0)
			{
				this.Roh_supportType = new DocumentFormat(propRoh_supportType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtOrganizer = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizer");
			if(propRoh_presentedAtOrganizer != null && propRoh_presentedAtOrganizer.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtOrganizer = new Organization(propRoh_presentedAtOrganizer.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtOrganizerType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerType");
			if(propRoh_presentedAtOrganizerType != null && propRoh_presentedAtOrganizerType.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtOrganizerType = new OrganizationType(propRoh_presentedAtOrganizerType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtOrganizerHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerHasCountryName");
			if(propRoh_presentedAtOrganizerHasCountryName != null && propRoh_presentedAtOrganizerHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtOrganizerHasCountryName = new Feature(propRoh_presentedAtOrganizerHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtType");
			if(propRoh_presentedAtType != null && propRoh_presentedAtType.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtType = new EventType(propRoh_presentedAtType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_projectAux = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectAux");
			if(propRoh_projectAux != null && propRoh_projectAux.PropertyValues.Count > 0)
			{
				this.Roh_projectAux = new Project(propRoh_projectAux.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_presentedAtGeographicFocus = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtGeographicFocus");
			if(propRoh_presentedAtGeographicFocus != null && propRoh_presentedAtGeographicFocus.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtGeographicFocus = new GeographicRegion(propRoh_presentedAtGeographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			this.Roh_isProducedBy = new List<Group>();
			SemanticPropertyModel propRoh_isProducedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isProducedBy");
			if(propRoh_isProducedBy != null && propRoh_isProducedBy.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_isProducedBy.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Group roh_isProducedBy = new Group(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_isProducedBy.Add(roh_isProducedBy);
					}
				}
			}
			SemanticPropertyModel propRoh_presentedAtOrganizerHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerHasRegion");
			if(propRoh_presentedAtOrganizerHasRegion != null && propRoh_presentedAtOrganizerHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtOrganizerHasRegion = new Feature(propRoh_presentedAtOrganizerHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			this.Roh_references = new List<Reference>();
			SemanticPropertyModel propRoh_references = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/references");
			if(propRoh_references != null && propRoh_references.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_references.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Reference roh_references = new Reference(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_references.Add(roh_references);
					}
				}
			}
			SemanticPropertyModel propDc_type = pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/type");
			if(propDc_type != null && propDc_type.PropertyValues.Count > 0)
			{
				this.Dc_type = new PublicationType(propDc_type.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Vivo_freeTextKeyword = new List<KeyWord>();
			SemanticPropertyModel propVivo_freeTextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeyword");
			if(propVivo_freeTextKeyword != null && propVivo_freeTextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeyword.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWord vivo_freeTextKeyword = new KeyWord(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_freeTextKeyword.Add(vivo_freeTextKeyword);
					}
				}
			}
			SemanticPropertyModel propRoh_presentedAtHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtHasCountryName");
			if(propRoh_presentedAtHasCountryName != null && propRoh_presentedAtHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtHasCountryName = new Feature(propRoh_presentedAtHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtSeminarType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtSeminarType");
			if(propRoh_presentedAtSeminarType != null && propRoh_presentedAtSeminarType.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtSeminarType = new SeminarEventType(propRoh_presentedAtSeminarType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_identifier = new List<fDocument>();
			SemanticPropertyModel propBibo_identifier = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/identifier");
			if(propBibo_identifier != null && propBibo_identifier.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_identifier.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						fDocument bibo_identifier = new fDocument(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_identifier.Add(bibo_identifier);
					}
				}
			}
			this.Roh_impactIndex = new List<ImpactIndex>();
			SemanticPropertyModel propRoh_impactIndex = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndex");
			if(propRoh_impactIndex != null && propRoh_impactIndex.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_impactIndex.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						ImpactIndex roh_impactIndex = new ImpactIndex(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_impactIndex.Add(roh_impactIndex);
					}
				}
			}
			this.Roh_i_doc_references = new List<Document>();
			SemanticPropertyModel propRoh_i_doc_references = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/i_doc_references");
			if(propRoh_i_doc_references != null && propRoh_i_doc_references.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_i_doc_references.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document roh_i_doc_references = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_i_doc_references.Add(roh_i_doc_references);
					}
				}
			}
			this.Roh_enrichedKeywords = new List<EnrichedKeyWord>();
			SemanticPropertyModel propRoh_enrichedKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/enrichedKeywords");
			if(propRoh_enrichedKeywords != null && propRoh_enrichedKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_enrichedKeywords.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						EnrichedKeyWord roh_enrichedKeywords = new EnrichedKeyWord(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_enrichedKeywords.Add(roh_enrichedKeywords);
					}
				}
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_hasMetric = new List<PublicationMetric>();
			SemanticPropertyModel propRoh_hasMetric = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasMetric");
			if(propRoh_hasMetric != null && propRoh_hasMetric.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_hasMetric.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PublicationMetric roh_hasMetric = new PublicationMetric(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_hasMetric.Add(roh_hasMetric);
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
			SemanticPropertyModel propRoh_presentedAtHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtHasRegion");
			if(propRoh_presentedAtHasRegion != null && propRoh_presentedAtHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtHasRegion = new Feature(propRoh_presentedAtHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propVivo_hasPublicationVenue = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#hasPublicationVenue");
			if(propVivo_hasPublicationVenue != null && propVivo_hasPublicationVenue.PropertyValues.Count > 0)
			{
				this.Vivo_hasPublicationVenue = new MainDocument(propVivo_hasPublicationVenue.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_impactIndexInYear = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndexInYear"));
			this.Roh_isbn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isbn"));
			this.Roh_presentedAtLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtLocality"));
			this.Roh_presentedAtGeographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtGeographicFocusOther"));
			this.Roh_year = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/year"));
			this.Roh_presentedAtSeminarTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtSeminarTypeOther"));
			this.Roh_presentedAtEnd = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtEnd"));
			this.Roh_presentedAtStart = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtStart"));
			this.Roh_semanticScholarCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/semanticScholarCitationCount"));
			this.Bibo_abstract = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/abstract"));
			this.Roh_presentedAtOrganizerTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerTypeOther"));
			this.Roh_openAccess= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/openAccess"));
			this.Roh_wosCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/wosCitationCount"));
			this.Roh_presentedAtWithExternalAdmissionsCommittee= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtWithExternalAdmissionsCommittee"));
			this.Bibo_issue = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issue"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_typeOthers = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/typeOthers"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vcard_url = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#url"));
			SemanticPropertyModel propRoh_userKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/userKeywords");
			this.Roh_userKeywords = new List<string>();
			if (propRoh_userKeywords != null && propRoh_userKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_userKeywords.PropertyValues)
				{
					this.Roh_userKeywords.Add(propValue.Value);
				}
			}
			this.Roh_scopusCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scopusCitationCount"));
			this.Bibo_publisher = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/publisher"));
			this.Roh_legalDeposit = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/legalDeposit"));
			this.Roh_hasFile = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasFile"));
			this.Roh_positionIP = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/positionIP"));
			this.Roh_publicationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationTitle"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Bibo_presentedAt = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/presentedAt"));
			this.Roh_reviewsNumber = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/reviewsNumber"));
			SemanticPropertyModel propRoh_externalKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/externalKeywords");
			this.Roh_externalKeywords = new List<string>();
			if (propRoh_externalKeywords != null && propRoh_externalKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_externalKeywords.PropertyValues)
				{
					this.Roh_externalKeywords.Add(propValue.Value);
				}
			}
			this.Roh_presentedAtTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtTypeOther"));
			this.Roh_inrecsCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/inrecsCitationCount"));
			this.Bibo_pmid = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pmid"));
			this.Roh_validationStatusPRC = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/validationStatusPRC"));
			this.Roh_hasPublicationVenueText = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasPublicationVenueText"));
			this.Bibo_handle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/handle"));
			this.Roh_presentedAtOrganizerTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerTitle"));
			this.Bibo_doi = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/doi"));
			SemanticPropertyModel propRoh_suggestedKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/suggestedKeywords");
			this.Roh_suggestedKeywords = new List<string>();
			if (propRoh_suggestedKeywords != null && propRoh_suggestedKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_suggestedKeywords.PropertyValues)
				{
					this.Roh_suggestedKeywords.Add(propValue.Value);
				}
			}
			this.Bibo_volume = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/volume"));
			this.Roh_quartile = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/quartile"));
			this.Bibo_issn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issn"));
			this.Bibo_pageStart = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pageStart"));
			this.Roh_congressProceedingsPublication= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/congressProceedingsPublication"));
			this.Roh_presentedAtOrganizerLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerLocality"));
			this.Roh_genderIP = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/genderIP"));
			this.Roh_getKeyWords= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/getKeyWords"));
			this.Bibo_pageEnd = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pageEnd"));
			this.Roh_collection = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collection"));
			SemanticPropertyModel propRoh_scientificActivityDocument = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificActivityDocument");
			if(propRoh_scientificActivityDocument != null && propRoh_scientificActivityDocument.PropertyValues.Count > 0)
			{
				this.Roh_scientificActivityDocument = new ScientificActivityDocument(propRoh_scientificActivityDocument.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isValidated= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isValidated"));
			this.Roh_citationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/citationCount")).Value;
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public Document(SemanticEntityModel pSemCmsModel, LanguageEnum idiomaUsuario) : base()
		{
			this.mGNOSSID = pSemCmsModel.Entity.Uri;
			this.mURL = pSemCmsModel.Properties.FirstOrDefault(p => p.PropertyValues.Any(prop => prop.DownloadUrl != null))?.FirstPropertyValue.DownloadUrl;
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
			SemanticPropertyModel propVcard_hasCountryName = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasCountryName");
			if(propVcard_hasCountryName != null && propVcard_hasCountryName.PropertyValues.Count > 0)
			{
				this.Vcard_hasCountryName = new Feature(propVcard_hasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_supportType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/supportType");
			if(propRoh_supportType != null && propRoh_supportType.PropertyValues.Count > 0)
			{
				this.Roh_supportType = new DocumentFormat(propRoh_supportType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtOrganizer = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizer");
			if(propRoh_presentedAtOrganizer != null && propRoh_presentedAtOrganizer.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtOrganizer = new Organization(propRoh_presentedAtOrganizer.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtOrganizerType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerType");
			if(propRoh_presentedAtOrganizerType != null && propRoh_presentedAtOrganizerType.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtOrganizerType = new OrganizationType(propRoh_presentedAtOrganizerType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtOrganizerHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerHasCountryName");
			if(propRoh_presentedAtOrganizerHasCountryName != null && propRoh_presentedAtOrganizerHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtOrganizerHasCountryName = new Feature(propRoh_presentedAtOrganizerHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtType");
			if(propRoh_presentedAtType != null && propRoh_presentedAtType.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtType = new EventType(propRoh_presentedAtType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_projectAux = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/projectAux");
			if(propRoh_projectAux != null && propRoh_projectAux.PropertyValues.Count > 0)
			{
				this.Roh_projectAux = new Project(propRoh_projectAux.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propRoh_presentedAtGeographicFocus = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtGeographicFocus");
			if(propRoh_presentedAtGeographicFocus != null && propRoh_presentedAtGeographicFocus.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtGeographicFocus = new GeographicRegion(propRoh_presentedAtGeographicFocus.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			this.Roh_isProducedBy = new List<Group>();
			SemanticPropertyModel propRoh_isProducedBy = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isProducedBy");
			if(propRoh_isProducedBy != null && propRoh_isProducedBy.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_isProducedBy.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Group roh_isProducedBy = new Group(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_isProducedBy.Add(roh_isProducedBy);
					}
				}
			}
			SemanticPropertyModel propRoh_presentedAtOrganizerHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerHasRegion");
			if(propRoh_presentedAtOrganizerHasRegion != null && propRoh_presentedAtOrganizerHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtOrganizerHasRegion = new Feature(propRoh_presentedAtOrganizerHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			this.Roh_references = new List<Reference>();
			SemanticPropertyModel propRoh_references = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/references");
			if(propRoh_references != null && propRoh_references.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_references.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Reference roh_references = new Reference(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_references.Add(roh_references);
					}
				}
			}
			SemanticPropertyModel propDc_type = pSemCmsModel.GetPropertyByPath("http://purl.org/dc/elements/1.1/type");
			if(propDc_type != null && propDc_type.PropertyValues.Count > 0)
			{
				this.Dc_type = new PublicationType(propDc_type.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Vivo_freeTextKeyword = new List<KeyWord>();
			SemanticPropertyModel propVivo_freeTextKeyword = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#freeTextKeyword");
			if(propVivo_freeTextKeyword != null && propVivo_freeTextKeyword.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propVivo_freeTextKeyword.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						KeyWord vivo_freeTextKeyword = new KeyWord(propValue.RelatedEntity,idiomaUsuario);
						this.Vivo_freeTextKeyword.Add(vivo_freeTextKeyword);
					}
				}
			}
			SemanticPropertyModel propRoh_presentedAtHasCountryName = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtHasCountryName");
			if(propRoh_presentedAtHasCountryName != null && propRoh_presentedAtHasCountryName.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtHasCountryName = new Feature(propRoh_presentedAtHasCountryName.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			SemanticPropertyModel propRoh_presentedAtSeminarType = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtSeminarType");
			if(propRoh_presentedAtSeminarType != null && propRoh_presentedAtSeminarType.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtSeminarType = new SeminarEventType(propRoh_presentedAtSeminarType.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Bibo_identifier = new List<fDocument>();
			SemanticPropertyModel propBibo_identifier = pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/identifier");
			if(propBibo_identifier != null && propBibo_identifier.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propBibo_identifier.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						fDocument bibo_identifier = new fDocument(propValue.RelatedEntity,idiomaUsuario);
						this.Bibo_identifier.Add(bibo_identifier);
					}
				}
			}
			this.Roh_impactIndex = new List<ImpactIndex>();
			SemanticPropertyModel propRoh_impactIndex = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndex");
			if(propRoh_impactIndex != null && propRoh_impactIndex.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_impactIndex.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						ImpactIndex roh_impactIndex = new ImpactIndex(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_impactIndex.Add(roh_impactIndex);
					}
				}
			}
			this.Roh_i_doc_references = new List<Document>();
			SemanticPropertyModel propRoh_i_doc_references = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/i_doc_references");
			if(propRoh_i_doc_references != null && propRoh_i_doc_references.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_i_doc_references.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						Document roh_i_doc_references = new Document(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_i_doc_references.Add(roh_i_doc_references);
					}
				}
			}
			this.Roh_enrichedKeywords = new List<EnrichedKeyWord>();
			SemanticPropertyModel propRoh_enrichedKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/enrichedKeywords");
			if(propRoh_enrichedKeywords != null && propRoh_enrichedKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_enrichedKeywords.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						EnrichedKeyWord roh_enrichedKeywords = new EnrichedKeyWord(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_enrichedKeywords.Add(roh_enrichedKeywords);
					}
				}
			}
			SemanticPropertyModel propVcard_hasRegion = pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#hasRegion");
			if(propVcard_hasRegion != null && propVcard_hasRegion.PropertyValues.Count > 0)
			{
				this.Vcard_hasRegion = new Feature(propVcard_hasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_hasMetric = new List<PublicationMetric>();
			SemanticPropertyModel propRoh_hasMetric = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasMetric");
			if(propRoh_hasMetric != null && propRoh_hasMetric.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_hasMetric.PropertyValues)
				{
					if(propValue.RelatedEntity!=null){
						PublicationMetric roh_hasMetric = new PublicationMetric(propValue.RelatedEntity,idiomaUsuario);
						this.Roh_hasMetric.Add(roh_hasMetric);
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
			SemanticPropertyModel propRoh_presentedAtHasRegion = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtHasRegion");
			if(propRoh_presentedAtHasRegion != null && propRoh_presentedAtHasRegion.PropertyValues.Count > 0)
			{
				this.Roh_presentedAtHasRegion = new Feature(propRoh_presentedAtHasRegion.PropertyValues[0].RelatedEntity,idiomaUsuario);
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
			SemanticPropertyModel propVivo_hasPublicationVenue = pSemCmsModel.GetPropertyByPath("http://vivoweb.org/ontology/core#hasPublicationVenue");
			if(propVivo_hasPublicationVenue != null && propVivo_hasPublicationVenue.PropertyValues.Count > 0)
			{
				this.Vivo_hasPublicationVenue = new MainDocument(propVivo_hasPublicationVenue.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_impactIndexInYear = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/impactIndexInYear"));
			this.Roh_isbn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isbn"));
			this.Roh_presentedAtLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtLocality"));
			this.Roh_presentedAtGeographicFocusOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtGeographicFocusOther"));
			this.Roh_year = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/year"));
			this.Roh_presentedAtSeminarTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtSeminarTypeOther"));
			this.Roh_presentedAtEnd = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtEnd"));
			this.Roh_presentedAtStart = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtStart"));
			this.Roh_semanticScholarCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/semanticScholarCitationCount"));
			this.Bibo_abstract = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/abstract"));
			this.Roh_presentedAtOrganizerTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerTypeOther"));
			this.Roh_openAccess= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/openAccess"));
			this.Roh_wosCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/wosCitationCount"));
			this.Roh_presentedAtWithExternalAdmissionsCommittee= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtWithExternalAdmissionsCommittee"));
			this.Bibo_issue = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issue"));
			this.Dct_issued = GetDateValuePropertySemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/dc/terms/issued"));
			this.Roh_typeOthers = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/typeOthers"));
			this.Roh_crisIdentifier = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/crisIdentifier"));
			this.Vcard_url = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#url"));
			SemanticPropertyModel propRoh_userKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/userKeywords");
			this.Roh_userKeywords = new List<string>();
			if (propRoh_userKeywords != null && propRoh_userKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_userKeywords.PropertyValues)
				{
					this.Roh_userKeywords.Add(propValue.Value);
				}
			}
			this.Roh_scopusCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scopusCitationCount"));
			this.Bibo_publisher = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/publisher"));
			this.Roh_legalDeposit = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/legalDeposit"));
			this.Roh_hasFile = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasFile"));
			this.Roh_positionIP = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/positionIP"));
			this.Roh_publicationTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/publicationTitle"));
			this.Vcard_locality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("https://www.w3.org/2006/vcard/ns#locality"));
			this.Bibo_presentedAt = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/presentedAt"));
			this.Roh_reviewsNumber = GetNumberFloatPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/reviewsNumber"));
			SemanticPropertyModel propRoh_externalKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/externalKeywords");
			this.Roh_externalKeywords = new List<string>();
			if (propRoh_externalKeywords != null && propRoh_externalKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_externalKeywords.PropertyValues)
				{
					this.Roh_externalKeywords.Add(propValue.Value);
				}
			}
			this.Roh_presentedAtTypeOther = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtTypeOther"));
			this.Roh_inrecsCitationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/inrecsCitationCount"));
			this.Bibo_pmid = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pmid"));
			this.Roh_validationStatusPRC = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/validationStatusPRC"));
			this.Roh_hasPublicationVenueText = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/hasPublicationVenueText"));
			this.Bibo_handle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/handle"));
			this.Roh_presentedAtOrganizerTitle = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerTitle"));
			this.Bibo_doi = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/doi"));
			SemanticPropertyModel propRoh_suggestedKeywords = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/suggestedKeywords");
			this.Roh_suggestedKeywords = new List<string>();
			if (propRoh_suggestedKeywords != null && propRoh_suggestedKeywords.PropertyValues.Count > 0)
			{
				foreach (SemanticPropertyModel.PropertyValue propValue in propRoh_suggestedKeywords.PropertyValues)
				{
					this.Roh_suggestedKeywords.Add(propValue.Value);
				}
			}
			this.Bibo_volume = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/volume"));
			this.Roh_quartile = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/quartile"));
			this.Bibo_issn = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/issn"));
			this.Bibo_pageStart = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pageStart"));
			this.Roh_congressProceedingsPublication= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/congressProceedingsPublication"));
			this.Roh_presentedAtOrganizerLocality = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/presentedAtOrganizerLocality"));
			this.Roh_genderIP = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/genderIP"));
			this.Roh_getKeyWords= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/getKeyWords"));
			this.Bibo_pageEnd = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://purl.org/ontology/bibo/pageEnd"));
			this.Roh_collection = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/collection"));
			SemanticPropertyModel propRoh_scientificActivityDocument = pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/scientificActivityDocument");
			if(propRoh_scientificActivityDocument != null && propRoh_scientificActivityDocument.PropertyValues.Count > 0)
			{
				this.Roh_scientificActivityDocument = new ScientificActivityDocument(propRoh_scientificActivityDocument.PropertyValues[0].RelatedEntity,idiomaUsuario);
			}
			this.Roh_isValidated= GetBooleanPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/isValidated"));
			this.Roh_citationCount = GetNumberIntPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/citationCount")).Value;
			this.Roh_title = GetPropertyValueSemCms(pSemCmsModel.GetPropertyByPath("http://w3id.org/roh/title"));
		}

		public virtual string RdfType { get { return "http://purl.org/ontology/bibo/Document"; } }
		public virtual string RdfsLabel { get { return "http://purl.org/ontology/bibo/Document"; } }
		[RDFProperty("http://w3id.org/roh/suggestedKnowledgeArea")]
		public  List<CategoryPath> Roh_suggestedKnowledgeArea { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasCountryName")]
		public  Feature Vcard_hasCountryName  { get; set;} 
		public string IdVcard_hasCountryName  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/supportType")]
		[RDFProperty("http://w3id.org/roh/supportType")]
		public  DocumentFormat Roh_supportType  { get; set;} 
		public string IdRoh_supportType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/presentedAtOrganizer")]
		public  Organization Roh_presentedAtOrganizer  { get; set;} 
		public string IdRoh_presentedAtOrganizer  { get; set;} 

		[RDFProperty("http://w3id.org/roh/presentedAtOrganizerType")]
		public  OrganizationType Roh_presentedAtOrganizerType  { get; set;} 
		public string IdRoh_presentedAtOrganizerType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/presentedAtOrganizerHasCountryName")]
		public  Feature Roh_presentedAtOrganizerHasCountryName  { get; set;} 
		public string IdRoh_presentedAtOrganizerHasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/presentedAtType")]
		public  EventType Roh_presentedAtType  { get; set;} 
		public string IdRoh_presentedAtType  { get; set;} 

		[RDFProperty("http://w3id.org/roh/projectAux")]
		public  Project Roh_projectAux  { get; set;} 
		public string IdRoh_projectAux  { get; set;} 

		[LABEL(LanguageEnum.es,"https://www.w3.org/2006/vcard/ns#hasLanguage")]
		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasLanguage")]
		public  List<Language> Vcard_hasLanguage { get; set;}
		public List<string> IdsVcard_hasLanguage { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtGeographicFocus")]
		public  GeographicRegion Roh_presentedAtGeographicFocus  { get; set;} 
		public string IdRoh_presentedAtGeographicFocus  { get; set;} 

		[RDFProperty("http://purl.org/ontology/bibo/authorList")]
		public  List<BFO_0000023> Bibo_authorList { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/project")]
		[RDFProperty("http://w3id.org/roh/project")]
		public  Project Roh_project  { get; set;} 
		public string IdRoh_project  { get; set;} 

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/isProducedBy")]
		[RDFProperty("http://w3id.org/roh/isProducedBy")]
		public  List<Group> Roh_isProducedBy { get; set;}
		public List<string> IdsRoh_isProducedBy { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtOrganizerHasRegion")]
		public  Feature Roh_presentedAtOrganizerHasRegion  { get; set;} 
		public string IdRoh_presentedAtOrganizerHasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/externalKnowledgeArea")]
		public  List<CategoryPath> Roh_externalKnowledgeArea { get; set;}

		[RDFProperty("http://w3id.org/roh/userKnowledgeArea")]
		public  List<CategoryPath> Roh_userKnowledgeArea { get; set;}

		[RDFProperty("http://w3id.org/roh/references")]
		public  List<Reference> Roh_references { get; set;}

		[LABEL(LanguageEnum.es,"http://purl.org/dc/elements/1.1/type")]
		[RDFProperty("http://purl.org/dc/elements/1.1/type")]
		public  PublicationType Dc_type  { get; set;} 
		public string IdDc_type  { get; set;} 

		[RDFProperty("http://vivoweb.org/ontology/core#freeTextKeyword")]
		public  List<KeyWord> Vivo_freeTextKeyword { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtHasCountryName")]
		public  Feature Roh_presentedAtHasCountryName  { get; set;} 
		public string IdRoh_presentedAtHasCountryName  { get; set;} 

		[RDFProperty("http://w3id.org/roh/presentedAtSeminarType")]
		public  SeminarEventType Roh_presentedAtSeminarType  { get; set;} 
		public string IdRoh_presentedAtSeminarType  { get; set;} 

		[RDFProperty("http://purl.org/ontology/bibo/identifier")]
		public  List<fDocument> Bibo_identifier { get; set;}

		[RDFProperty("http://w3id.org/roh/impactIndex")]
		public  List<ImpactIndex> Roh_impactIndex { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/i_doc_references")]
		[RDFProperty("http://w3id.org/roh/i_doc_references")]
		public  List<Document> Roh_i_doc_references { get; set;}
		public List<string> IdsRoh_i_doc_references { get; set;}

		[RDFProperty("http://w3id.org/roh/enrichedKeywords")]
		public  List<EnrichedKeyWord> Roh_enrichedKeywords { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#hasRegion")]
		public  Feature Vcard_hasRegion  { get; set;} 
		public string IdVcard_hasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/hasMetric")]
		public  List<PublicationMetric> Roh_hasMetric { get; set;}

		[RDFProperty("http://w3id.org/roh/hasKnowledgeArea")]
		public  List<CategoryPath> Roh_hasKnowledgeArea { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtHasRegion")]
		public  Feature Roh_presentedAtHasRegion  { get; set;} 
		public string IdRoh_presentedAtHasRegion  { get; set;} 

		[RDFProperty("http://w3id.org/roh/enrichedKnowledgeArea")]
		public  List<CategoryPath> Roh_enrichedKnowledgeArea { get; set;}

		[LABEL(LanguageEnum.es,"http://vivoweb.org/ontology/core#hasPublicationVenue")]
		[RDFProperty("http://vivoweb.org/ontology/core#hasPublicationVenue")]
		public  MainDocument Vivo_hasPublicationVenue  { get; set;} 
		public string IdVivo_hasPublicationVenue  { get; set;} 

		[RDFProperty("http://w3id.org/roh/impactIndexInYear")]
		public  float? Roh_impactIndexInYear { get; set;}

		[RDFProperty("http://w3id.org/roh/isbn")]
		public  string Roh_isbn { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtLocality")]
		public  string Roh_presentedAtLocality { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtGeographicFocusOther")]
		public  string Roh_presentedAtGeographicFocusOther { get; set;}

		[RDFProperty("http://w3id.org/roh/year")]
		public  int? Roh_year { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtSeminarTypeOther")]
		public  string Roh_presentedAtSeminarTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtEnd")]
		public  DateTime? Roh_presentedAtEnd { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtStart")]
		public  DateTime? Roh_presentedAtStart { get; set;}

		[RDFProperty("http://w3id.org/roh/semanticScholarCitationCount")]
		public  int? Roh_semanticScholarCitationCount { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/abstract")]
		public  string Bibo_abstract { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtOrganizerTypeOther")]
		public  string Roh_presentedAtOrganizerTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/openAccess")]
		public  bool Roh_openAccess { get; set;}

		[RDFProperty("http://w3id.org/roh/wosCitationCount")]
		public  int? Roh_wosCitationCount { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtWithExternalAdmissionsCommittee")]
		public  bool Roh_presentedAtWithExternalAdmissionsCommittee { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/issue")]
		public  string Bibo_issue { get; set;}

		[RDFProperty("http://purl.org/dc/terms/issued")]
		public  DateTime? Dct_issued { get; set;}

		[RDFProperty("http://w3id.org/roh/typeOthers")]
		public  string Roh_typeOthers { get; set;}

		[RDFProperty("http://w3id.org/roh/crisIdentifier")]
		public  string Roh_crisIdentifier { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#url")]
		public  string Vcard_url { get; set;}

		[RDFProperty("http://w3id.org/roh/userKeywords")]
		public  List<string> Roh_userKeywords { get; set;}

		[RDFProperty("http://w3id.org/roh/scopusCitationCount")]
		public  int? Roh_scopusCitationCount { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/publisher")]
		public  string Bibo_publisher { get; set;}

		[RDFProperty("http://w3id.org/roh/legalDeposit")]
		public  string Roh_legalDeposit { get; set;}

		[RDFProperty("http://w3id.org/roh/hasFile")]
		public  string Roh_hasFile { get; set;}

		[RDFProperty("http://w3id.org/roh/positionIP")]
		public  string Roh_positionIP { get; set;}

		[RDFProperty("http://w3id.org/roh/publicationTitle")]
		public  string Roh_publicationTitle { get; set;}

		[RDFProperty("https://www.w3.org/2006/vcard/ns#locality")]
		public  string Vcard_locality { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/presentedAt")]
		public  string Bibo_presentedAt { get; set;}

		[RDFProperty("http://w3id.org/roh/reviewsNumber")]
		public  float? Roh_reviewsNumber { get; set;}

		[RDFProperty("http://w3id.org/roh/externalKeywords")]
		public  List<string> Roh_externalKeywords { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtTypeOther")]
		public  string Roh_presentedAtTypeOther { get; set;}

		[RDFProperty("http://w3id.org/roh/inrecsCitationCount")]
		public  int? Roh_inrecsCitationCount { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/pmid")]
		public  string Bibo_pmid { get; set;}

		[RDFProperty("http://w3id.org/roh/validationStatusPRC")]
		public  string Roh_validationStatusPRC { get; set;}

		[RDFProperty("http://w3id.org/roh/hasPublicationVenueText")]
		public  string Roh_hasPublicationVenueText { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/handle")]
		public  string Bibo_handle { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtOrganizerTitle")]
		public  string Roh_presentedAtOrganizerTitle { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/doi")]
		public  string Bibo_doi { get; set;}

		[RDFProperty("http://w3id.org/roh/suggestedKeywords")]
		public  List<string> Roh_suggestedKeywords { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/volume")]
		public  string Bibo_volume { get; set;}

		[RDFProperty("http://w3id.org/roh/quartile")]
		public  int? Roh_quartile { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/issn")]
		public  string Bibo_issn { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/pageStart")]
		public  string Bibo_pageStart { get; set;}

		[RDFProperty("http://w3id.org/roh/congressProceedingsPublication")]
		public  bool Roh_congressProceedingsPublication { get; set;}

		[RDFProperty("http://w3id.org/roh/presentedAtOrganizerLocality")]
		public  string Roh_presentedAtOrganizerLocality { get; set;}

		[RDFProperty("http://w3id.org/roh/genderIP")]
		public  string Roh_genderIP { get; set;}

		[RDFProperty("http://w3id.org/roh/getKeyWords")]
		public  bool Roh_getKeyWords { get; set;}

		[RDFProperty("http://purl.org/ontology/bibo/pageEnd")]
		public  string Bibo_pageEnd { get; set;}

		[RDFProperty("http://w3id.org/roh/collection")]
		public  string Roh_collection { get; set;}

		[LABEL(LanguageEnum.es,"http://w3id.org/roh/scientificActivityDocument")]
		[RDFProperty("http://w3id.org/roh/scientificActivityDocument")]
		[Required]
		public  ScientificActivityDocument Roh_scientificActivityDocument  { get; set;} 
		public string IdRoh_scientificActivityDocument  { get; set;} 

		[RDFProperty("http://w3id.org/roh/isValidated")]
		public  bool Roh_isValidated { get; set;}

		[RDFProperty("http://w3id.org/roh/citationCount")]
		public  int Roh_citationCount { get; set;}

		[RDFProperty("http://w3id.org/roh/title")]
		public  string Roh_title { get; set;}


		internal override void GetProperties()
		{
			base.GetProperties();
			propList.Add(new StringOntologyProperty("vcard:hasCountryName", this.IdVcard_hasCountryName));
			propList.Add(new StringOntologyProperty("roh:supportType", this.IdRoh_supportType));
			propList.Add(new StringOntologyProperty("roh:presentedAtOrganizer", this.IdRoh_presentedAtOrganizer));
			propList.Add(new StringOntologyProperty("roh:presentedAtOrganizerType", this.IdRoh_presentedAtOrganizerType));
			propList.Add(new StringOntologyProperty("roh:presentedAtOrganizerHasCountryName", this.IdRoh_presentedAtOrganizerHasCountryName));
			propList.Add(new StringOntologyProperty("roh:presentedAtType", this.IdRoh_presentedAtType));
			propList.Add(new StringOntologyProperty("roh:projectAux", this.IdRoh_projectAux));
			propList.Add(new ListStringOntologyProperty("vcard:hasLanguage", this.IdsVcard_hasLanguage));
			propList.Add(new StringOntologyProperty("roh:presentedAtGeographicFocus", this.IdRoh_presentedAtGeographicFocus));
			propList.Add(new StringOntologyProperty("roh:project", this.IdRoh_project));
			propList.Add(new ListStringOntologyProperty("roh:isProducedBy", this.IdsRoh_isProducedBy));
			propList.Add(new StringOntologyProperty("roh:presentedAtOrganizerHasRegion", this.IdRoh_presentedAtOrganizerHasRegion));
			propList.Add(new StringOntologyProperty("dc:type", this.IdDc_type));
			propList.Add(new StringOntologyProperty("roh:presentedAtHasCountryName", this.IdRoh_presentedAtHasCountryName));
			propList.Add(new StringOntologyProperty("roh:presentedAtSeminarType", this.IdRoh_presentedAtSeminarType));
			propList.Add(new ListStringOntologyProperty("roh:i_doc_references", this.IdsRoh_i_doc_references));
			propList.Add(new StringOntologyProperty("vcard:hasRegion", this.IdVcard_hasRegion));
			propList.Add(new StringOntologyProperty("roh:presentedAtHasRegion", this.IdRoh_presentedAtHasRegion));
			propList.Add(new StringOntologyProperty("vivo:hasPublicationVenue", this.IdVivo_hasPublicationVenue));
			propList.Add(new StringOntologyProperty("roh:impactIndexInYear", this.Roh_impactIndexInYear.ToString()));
			propList.Add(new StringOntologyProperty("roh:isbn", this.Roh_isbn));
			propList.Add(new StringOntologyProperty("roh:presentedAtLocality", this.Roh_presentedAtLocality));
			propList.Add(new StringOntologyProperty("roh:presentedAtGeographicFocusOther", this.Roh_presentedAtGeographicFocusOther));
			propList.Add(new StringOntologyProperty("roh:year", this.Roh_year.ToString()));
			propList.Add(new StringOntologyProperty("roh:presentedAtSeminarTypeOther", this.Roh_presentedAtSeminarTypeOther));
			if (this.Roh_presentedAtEnd.HasValue){
				propList.Add(new DateOntologyProperty("roh:presentedAtEnd", this.Roh_presentedAtEnd.Value));
				}
			if (this.Roh_presentedAtStart.HasValue){
				propList.Add(new DateOntologyProperty("roh:presentedAtStart", this.Roh_presentedAtStart.Value));
				}
			propList.Add(new StringOntologyProperty("roh:semanticScholarCitationCount", this.Roh_semanticScholarCitationCount.ToString()));
			propList.Add(new StringOntologyProperty("bibo:abstract", this.Bibo_abstract));
			propList.Add(new StringOntologyProperty("roh:presentedAtOrganizerTypeOther", this.Roh_presentedAtOrganizerTypeOther));
			propList.Add(new BoolOntologyProperty("roh:openAccess", this.Roh_openAccess));
			propList.Add(new StringOntologyProperty("roh:wosCitationCount", this.Roh_wosCitationCount.ToString()));
			propList.Add(new BoolOntologyProperty("roh:presentedAtWithExternalAdmissionsCommittee", this.Roh_presentedAtWithExternalAdmissionsCommittee));
			propList.Add(new StringOntologyProperty("bibo:issue", this.Bibo_issue));
			if (this.Dct_issued.HasValue){
				propList.Add(new DateOntologyProperty("dct:issued", this.Dct_issued.Value));
				}
			propList.Add(new StringOntologyProperty("roh:typeOthers", this.Roh_typeOthers));
			propList.Add(new StringOntologyProperty("roh:crisIdentifier", this.Roh_crisIdentifier));
			propList.Add(new StringOntologyProperty("vcard:url", this.Vcard_url));
			propList.Add(new ListStringOntologyProperty("roh:userKeywords", this.Roh_userKeywords));
			propList.Add(new StringOntologyProperty("roh:scopusCitationCount", this.Roh_scopusCitationCount.ToString()));
			propList.Add(new StringOntologyProperty("bibo:publisher", this.Bibo_publisher));
			propList.Add(new StringOntologyProperty("roh:legalDeposit", this.Roh_legalDeposit));
			propList.Add(new StringOntologyProperty("roh:hasFile", this.Roh_hasFile));
			propList.Add(new StringOntologyProperty("roh:positionIP", this.Roh_positionIP));
			propList.Add(new StringOntologyProperty("roh:publicationTitle", this.Roh_publicationTitle));
			propList.Add(new StringOntologyProperty("vcard:locality", this.Vcard_locality));
			propList.Add(new StringOntologyProperty("bibo:presentedAt", this.Bibo_presentedAt));
			propList.Add(new StringOntologyProperty("roh:reviewsNumber", this.Roh_reviewsNumber.ToString()));
			propList.Add(new ListStringOntologyProperty("roh:externalKeywords", this.Roh_externalKeywords));
			propList.Add(new StringOntologyProperty("roh:presentedAtTypeOther", this.Roh_presentedAtTypeOther));
			propList.Add(new StringOntologyProperty("roh:inrecsCitationCount", this.Roh_inrecsCitationCount.ToString()));
			propList.Add(new StringOntologyProperty("bibo:pmid", this.Bibo_pmid));
			propList.Add(new StringOntologyProperty("roh:validationStatusPRC", this.Roh_validationStatusPRC));
			propList.Add(new StringOntologyProperty("roh:hasPublicationVenueText", this.Roh_hasPublicationVenueText));
			propList.Add(new StringOntologyProperty("bibo:handle", this.Bibo_handle));
			propList.Add(new StringOntologyProperty("roh:presentedAtOrganizerTitle", this.Roh_presentedAtOrganizerTitle));
			propList.Add(new StringOntologyProperty("bibo:doi", this.Bibo_doi));
			propList.Add(new ListStringOntologyProperty("roh:suggestedKeywords", this.Roh_suggestedKeywords));
			propList.Add(new StringOntologyProperty("bibo:volume", this.Bibo_volume));
			propList.Add(new StringOntologyProperty("roh:quartile", this.Roh_quartile.ToString()));
			propList.Add(new StringOntologyProperty("bibo:issn", this.Bibo_issn));
			propList.Add(new StringOntologyProperty("bibo:pageStart", this.Bibo_pageStart));
			propList.Add(new BoolOntologyProperty("roh:congressProceedingsPublication", this.Roh_congressProceedingsPublication));
			propList.Add(new StringOntologyProperty("roh:presentedAtOrganizerLocality", this.Roh_presentedAtOrganizerLocality));
			propList.Add(new StringOntologyProperty("roh:genderIP", this.Roh_genderIP));
			propList.Add(new BoolOntologyProperty("roh:getKeyWords", this.Roh_getKeyWords));
			propList.Add(new StringOntologyProperty("bibo:pageEnd", this.Bibo_pageEnd));
			propList.Add(new StringOntologyProperty("roh:collection", this.Roh_collection));
			propList.Add(new StringOntologyProperty("roh:scientificActivityDocument", this.IdRoh_scientificActivityDocument));
			propList.Add(new BoolOntologyProperty("roh:isValidated", this.Roh_isValidated));
			propList.Add(new StringOntologyProperty("roh:citationCount", this.Roh_citationCount.ToString()));
			propList.Add(new StringOntologyProperty("roh:title", this.Roh_title));
		}

		internal override void GetEntities()
		{
			base.GetEntities();
			if(Roh_suggestedKnowledgeArea!=null){
				foreach(CategoryPath prop in Roh_suggestedKnowledgeArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:suggestedKnowledgeArea", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
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
			if(Roh_externalKnowledgeArea!=null){
				foreach(CategoryPath prop in Roh_externalKnowledgeArea){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityCategoryPath = new OntologyEntity("http://w3id.org/roh/CategoryPath", "http://w3id.org/roh/CategoryPath", "roh:externalKnowledgeArea", prop.propList, prop.entList);
				entList.Add(entityCategoryPath);
				prop.Entity= entityCategoryPath;
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
			if(Roh_references!=null){
				foreach(Reference prop in Roh_references){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityReference = new OntologyEntity("http://w3id.org/roh/Reference", "http://w3id.org/roh/Reference", "roh:references", prop.propList, prop.entList);
				entList.Add(entityReference);
				prop.Entity= entityReference;
				}
			}
			if(Vivo_freeTextKeyword!=null){
				foreach(KeyWord prop in Vivo_freeTextKeyword){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityKeyWord = new OntologyEntity("http://w3id.org/roh/KeyWord", "http://w3id.org/roh/KeyWord", "vivo:freeTextKeyword", prop.propList, prop.entList);
				entList.Add(entityKeyWord);
				prop.Entity= entityKeyWord;
				}
			}
			if(Bibo_identifier!=null){
				foreach(fDocument prop in Bibo_identifier){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityDocument = new OntologyEntity("http://xmlns.com/foaf/0.1/Document", "http://xmlns.com/foaf/0.1/Document", "bibo:identifier", prop.propList, prop.entList);
				entList.Add(entityDocument);
				prop.Entity= entityDocument;
				}
			}
			if(Roh_impactIndex!=null){
				foreach(ImpactIndex prop in Roh_impactIndex){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityImpactIndex = new OntologyEntity("http://w3id.org/roh/ImpactIndex", "http://w3id.org/roh/ImpactIndex", "roh:impactIndex", prop.propList, prop.entList);
				entList.Add(entityImpactIndex);
				prop.Entity= entityImpactIndex;
				}
			}
			if(Roh_enrichedKeywords!=null){
				foreach(EnrichedKeyWord prop in Roh_enrichedKeywords){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityEnrichedKeyWord = new OntologyEntity("http://w3id.org/roh/EnrichedKeyWord", "http://w3id.org/roh/EnrichedKeyWord", "roh:enrichedKeywords", prop.propList, prop.entList);
				entList.Add(entityEnrichedKeyWord);
				prop.Entity= entityEnrichedKeyWord;
				}
			}
			if(Roh_hasMetric!=null){
				foreach(PublicationMetric prop in Roh_hasMetric){
					prop.GetProperties();
					prop.GetEntities();
					OntologyEntity entityPublicationMetric = new OntologyEntity("http://w3id.org/roh/PublicationMetric", "http://w3id.org/roh/PublicationMetric", "roh:hasMetric", prop.propList, prop.entList);
				entList.Add(entityPublicationMetric);
				prop.Entity= entityPublicationMetric;
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
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://purl.org/ontology/bibo/Document>", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://purl.org/ontology/bibo/Document\"", list, " . ");
			AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}>", list, " . ");
			if(this.Roh_suggestedKnowledgeArea != null)
			{
			foreach(var item0 in this.Roh_suggestedKnowledgeArea)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/CategoryPath>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/CategoryPath\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/suggestedKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/externalKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/userKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/hasKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/enrichedKnowledgeArea", $"<{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdsRoh_categoryNode != null)
				{
					foreach(var item2 in item0.IdsRoh_categoryNode)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/CategoryPath_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/categoryNode", $"<{item2}>", list, " . ");
					}
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
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://purl.org/ontology/bibo/authorList", $"<{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick)}\"", list, " . ");
				}
				if(item0.IdRdf_member != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#member", $"<{item0.IdRdf_member}>", list, " . ");
				}
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/BFO_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
				}
			}
			}
			if(this.Roh_references != null)
			{
			foreach(var item0 in this.Roh_references)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/Reference>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/Reference\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/references", $"<{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Roh_authorList != null)
			{
			foreach(var item1 in item0.Roh_authorList)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ReferenceAuthor_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ReferenceAuthor>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ReferenceAuthor_{ResourceID}_{item1.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ReferenceAuthor\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ReferenceAuthor_{ResourceID}_{item1.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/authorList", $"<{resourceAPI.GraphsUrl}items/ReferenceAuthor_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_semanticScholarId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ReferenceAuthor_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/semanticScholarId", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_semanticScholarId)}\"", list, " . ");
				}
				if(item1.Foaf_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ReferenceAuthor_{ResourceID}_{item1.ArticleID}",  "http://xmlns.com/foaf/0.1/name", $"\"{GenerarTextoSinSaltoDeLinea(item1.Foaf_name)}\"", list, " . ");
				}
			}
			}
				if(item0.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{item0.Dct_issued.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(item0.Vcard_url != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#url", $"\"{GenerarTextoSinSaltoDeLinea(item0.Vcard_url)}\"", list, " . ");
				}
				if(item0.Roh_hasPublicationVenueText != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/hasPublicationVenueText", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_hasPublicationVenueText)}\"", list, " . ");
				}
				if(item0.Bibo_doi != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(item0.Bibo_doi)}\"", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Reference_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title)}\"", list, " . ");
				}
			}
			}
			if(this.Vivo_freeTextKeyword != null)
			{
			foreach(var item0 in this.Vivo_freeTextKeyword)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWord_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/KeyWord>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWord_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/KeyWord\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/KeyWord_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"<{resourceAPI.GraphsUrl}items/KeyWord_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRoh_keyWordConcept != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWord_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/keyWordConcept", $"<{item0.IdRoh_keyWordConcept}>", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/KeyWord_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title)}\"", list, " . ");
				}
			}
			}
			if(this.Bibo_identifier != null)
			{
			foreach(var item0 in this.Bibo_identifier)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://xmlns.com/foaf/0.1/Document>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://xmlns.com/foaf/0.1/Document\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://purl.org/ontology/bibo/identifier", $"<{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Dc_title)}\"", list, " . ");
				}
				if(item0.Foaf_topic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/topic", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_topic)}\"", list, " . ");
				}
			}
			}
			if(this.Roh_impactIndex != null)
			{
			foreach(var item0 in this.Roh_impactIndex)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/ImpactIndex>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/ImpactIndex\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/impactIndex", $"<{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRoh_impactIndexCategory != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactIndexCategory", $"<{item0.IdRoh_impactIndexCategory}>", list, " . ");
				}
				if(item0.IdRoh_impactSource != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactSource", $"<{item0.IdRoh_impactSource}>", list, " . ");
				}
				if(item0.Roh_impactIndexInYear != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactIndexInYear", $"{item0.Roh_impactIndexInYear.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(item0.Roh_impactSourceOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactSourceOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_impactSourceOther)}\"", list, " . ");
				}
				if(item0.Roh_journalNumberInCat != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/journalNumberInCat", $"{item0.Roh_journalNumberInCat.Value.ToString()}", list, " . ");
				}
				if(item0.Roh_publicationPosition != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/publicationPosition", $"{item0.Roh_publicationPosition.Value.ToString()}", list, " . ");
				}
				if(item0.Roh_quartile != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/ImpactIndex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/quartile", $"{item0.Roh_quartile.Value.ToString()}", list, " . ");
				}
			}
			}
			if(this.Roh_enrichedKeywords != null)
			{
			foreach(var item0 in this.Roh_enrichedKeywords)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/EnrichedKeyWord_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/EnrichedKeyWord>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/EnrichedKeyWord_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/EnrichedKeyWord\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/EnrichedKeyWord_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/enrichedKeywords", $"<{resourceAPI.GraphsUrl}items/EnrichedKeyWord_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_score != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/EnrichedKeyWord_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/score", $"{item0.Roh_score.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/EnrichedKeyWord_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title)}\"", list, " . ");
				}
			}
			}
			if(this.Roh_hasMetric != null)
			{
			foreach(var item0 in this.Roh_hasMetric)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PublicationMetric_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"<http://w3id.org/roh/PublicationMetric>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PublicationMetric_{ResourceID}_{item0.ArticleID}", "http://www.w3.org/2000/01/rdf-schema#label", $"\"http://w3id.org/roh/PublicationMetric\"", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}{ResourceID}", "http://gnoss/hasEntidad", $"<{resourceAPI.GraphsUrl}items/PublicationMetric_{ResourceID}_{item0.ArticleID}>", list, " . ");
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/hasMetric", $"<{resourceAPI.GraphsUrl}items/PublicationMetric_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_metricName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PublicationMetric_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/metricName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_metricName)}\"", list, " . ");
				}
				if(item0.Roh_citationCount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/PublicationMetric_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/citationCount", $"{item0.Roh_citationCount.ToString()}", list, " . ");
				}
			}
			}
				if(this.IdVcard_hasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{this.IdVcard_hasCountryName}>", list, " . ");
				}
				if(this.IdRoh_supportType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/supportType", $"<{this.IdRoh_supportType}>", list, " . ");
				}
				if(this.IdRoh_presentedAtOrganizer != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtOrganizer", $"<{this.IdRoh_presentedAtOrganizer}>", list, " . ");
				}
				if(this.IdRoh_presentedAtOrganizerType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtOrganizerType", $"<{this.IdRoh_presentedAtOrganizerType}>", list, " . ");
				}
				if(this.IdRoh_presentedAtOrganizerHasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtOrganizerHasCountryName", $"<{this.IdRoh_presentedAtOrganizerHasCountryName}>", list, " . ");
				}
				if(this.IdRoh_presentedAtType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtType", $"<{this.IdRoh_presentedAtType}>", list, " . ");
				}
				if(this.IdRoh_projectAux != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/projectAux", $"<{this.IdRoh_projectAux}>", list, " . ");
				}
				if(this.IdsVcard_hasLanguage != null)
				{
					foreach(var item2 in this.IdsVcard_hasLanguage)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "https://www.w3.org/2006/vcard/ns#hasLanguage", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdRoh_presentedAtGeographicFocus != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtGeographicFocus", $"<{this.IdRoh_presentedAtGeographicFocus}>", list, " . ");
				}
				if(this.IdRoh_project != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/project", $"<{this.IdRoh_project}>", list, " . ");
				}
				if(this.IdsRoh_isProducedBy != null)
				{
					foreach(var item2 in this.IdsRoh_isProducedBy)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/isProducedBy", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdRoh_presentedAtOrganizerHasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtOrganizerHasRegion", $"<{this.IdRoh_presentedAtOrganizerHasRegion}>", list, " . ");
				}
				if(this.IdDc_type != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/dc/elements/1.1/type", $"<{this.IdDc_type}>", list, " . ");
				}
				if(this.IdRoh_presentedAtHasCountryName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtHasCountryName", $"<{this.IdRoh_presentedAtHasCountryName}>", list, " . ");
				}
				if(this.IdRoh_presentedAtSeminarType != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtSeminarType", $"<{this.IdRoh_presentedAtSeminarType}>", list, " . ");
				}
				if(this.IdsRoh_i_doc_references != null)
				{
					foreach(var item2 in this.IdsRoh_i_doc_references)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/i_doc_references", $"<{item2}>", list, " . ");
					}
				}
				if(this.IdVcard_hasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{this.IdVcard_hasRegion}>", list, " . ");
				}
				if(this.IdRoh_presentedAtHasRegion != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtHasRegion", $"<{this.IdRoh_presentedAtHasRegion}>", list, " . ");
				}
				if(this.IdVivo_hasPublicationVenue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://vivoweb.org/ontology/core#hasPublicationVenue", $"<{this.IdVivo_hasPublicationVenue}>", list, " . ");
				}
				if(this.Roh_impactIndexInYear != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/impactIndexInYear", $"{this.Roh_impactIndexInYear.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_isbn != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isbn", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_isbn)}\"", list, " . ");
				}
				if(this.Roh_presentedAtLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtLocality)}\"", list, " . ");
				}
				if(this.Roh_presentedAtGeographicFocusOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtGeographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtGeographicFocusOther)}\"", list, " . ");
				}
				if(this.Roh_year != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/year", $"{this.Roh_year.Value.ToString()}", list, " . ");
				}
				if(this.Roh_presentedAtSeminarTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtSeminarTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtSeminarTypeOther)}\"", list, " . ");
				}
				if(this.Roh_presentedAtEnd != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtEnd", $"\"{this.Roh_presentedAtEnd.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_presentedAtStart != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtStart", $"\"{this.Roh_presentedAtStart.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_semanticScholarCitationCount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/semanticScholarCitationCount", $"{this.Roh_semanticScholarCitationCount.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_abstract != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/abstract", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_abstract)}\"", list, " . ");
				}
				if(this.Roh_presentedAtOrganizerTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtOrganizerTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtOrganizerTypeOther)}\"", list, " . ");
				}
				if(this.Roh_openAccess != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/openAccess", $"\"{this.Roh_openAccess.ToString()}\"", list, " . ");
				}
				if(this.Roh_wosCitationCount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/wosCitationCount", $"{this.Roh_wosCitationCount.Value.ToString()}", list, " . ");
				}
				if(this.Roh_presentedAtWithExternalAdmissionsCommittee != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtWithExternalAdmissionsCommittee", $"\"{this.Roh_presentedAtWithExternalAdmissionsCommittee.ToString()}\"", list, " . ");
				}
				if(this.Bibo_issue != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/issue", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issue)}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/dc/terms/issued", $"\"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}\"", list, " . ");
				}
				if(this.Roh_typeOthers != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/typeOthers", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_typeOthers)}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier)}\"", list, " . ");
				}
				if(this.Vcard_url != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#url", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_url)}\"", list, " . ");
				}
				if(this.Roh_userKeywords != null)
				{
					foreach(var item2 in this.Roh_userKeywords)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/userKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_scopusCitationCount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/scopusCitationCount", $"{this.Roh_scopusCitationCount.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_publisher != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/publisher", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_publisher)}\"", list, " . ");
				}
				if(this.Roh_legalDeposit != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/legalDeposit", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_legalDeposit)}\"", list, " . ");
				}
				if(this.Roh_hasFile != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/hasFile", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasFile)}\"", list, " . ");
				}
				if(this.Roh_positionIP != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/positionIP", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_positionIP)}\"", list, " . ");
				}
				if(this.Roh_publicationTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/publicationTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_publicationTitle)}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality)}\"", list, " . ");
				}
				if(this.Bibo_presentedAt != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/presentedAt", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_presentedAt)}\"", list, " . ");
				}
				if(this.Roh_reviewsNumber != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/reviewsNumber", $"{this.Roh_reviewsNumber.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_externalKeywords != null)
				{
					foreach(var item2 in this.Roh_externalKeywords)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/externalKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Roh_presentedAtTypeOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtTypeOther)}\"", list, " . ");
				}
				if(this.Roh_inrecsCitationCount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/inrecsCitationCount", $"{this.Roh_inrecsCitationCount.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_pmid != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/pmid", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pmid)}\"", list, " . ");
				}
				if(this.Roh_validationStatusPRC != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/validationStatusPRC", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_validationStatusPRC)}\"", list, " . ");
				}
				if(this.Roh_hasPublicationVenueText != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/hasPublicationVenueText", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasPublicationVenueText)}\"", list, " . ");
				}
				if(this.Bibo_handle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/handle", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_handle)}\"", list, " . ");
				}
				if(this.Roh_presentedAtOrganizerTitle != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtOrganizerTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtOrganizerTitle)}\"", list, " . ");
				}
				if(this.Bibo_doi != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_doi)}\"", list, " . ");
				}
				if(this.Roh_suggestedKeywords != null)
				{
					foreach(var item2 in this.Roh_suggestedKeywords)
					{
						AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}", "http://w3id.org/roh/suggestedKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2)}\"", list, " . ");
					}
				}
				if(this.Bibo_volume != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/volume", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_volume)}\"", list, " . ");
				}
				if(this.Roh_quartile != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/quartile", $"{this.Roh_quartile.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_issn != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/issn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issn)}\"", list, " . ");
				}
				if(this.Bibo_pageStart != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/pageStart", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pageStart)}\"", list, " . ");
				}
				if(this.Roh_congressProceedingsPublication != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/congressProceedingsPublication", $"\"{this.Roh_congressProceedingsPublication.ToString()}\"", list, " . ");
				}
				if(this.Roh_presentedAtOrganizerLocality != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/presentedAtOrganizerLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtOrganizerLocality)}\"", list, " . ");
				}
				if(this.Roh_genderIP != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/genderIP", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_genderIP)}\"", list, " . ");
				}
				if(this.Roh_getKeyWords != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/getKeyWords", $"\"{this.Roh_getKeyWords.ToString()}\"", list, " . ");
				}
				if(this.Bibo_pageEnd != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://purl.org/ontology/bibo/pageEnd", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pageEnd)}\"", list, " . ");
				}
				if(this.Roh_collection != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/collection", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_collection)}\"", list, " . ");
				}
				if(this.IdRoh_scientificActivityDocument != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/scientificActivityDocument", $"<{this.IdRoh_scientificActivityDocument}>", list, " . ");
				}
				if(this.Roh_isValidated != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/isValidated", $"\"{this.Roh_isValidated.ToString()}\"", list, " . ");
				}
				if(this.Roh_citationCount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/citationCount", $"{this.Roh_citationCount.ToString()}", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/Document_{ResourceID}_{ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
				}
			return list;
		}

		public override List<string> ToSearchGraphTriples(ResourceApi resourceAPI)
		{
			List<string> list = new List<string>();
			List<string> listaSearch = new List<string>();
			AgregarTags(list);
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://www.w3.org/1999/02/22-rdf-syntax-ns#type", $"\"document\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/type", $"\"http://purl.org/ontology/bibo/Document\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechapublicacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hastipodoc", "\"5\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasfechamodificacion", $"{DateTime.Now.ToString("yyyyMMddHHmmss")}", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnumeroVisitas", "0", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasprivacidadCom", "\"publico\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://xmlns.com/foaf/0.1/firstName", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://gnoss/hasnombrecompleto", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title)}\"", list, " . ");
			string search = string.Empty;
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
			if(this.Bibo_authorList != null)
			{
			foreach(var item0 in this.Bibo_authorList)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/ontology/bibo/authorList", $"<{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Foaf_nick != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/nick", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_nick).ToLower()}\"", list, " . ");
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
				if(item0.Rdf_comment != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/bfo_0000023_{ResourceID}_{item0.ArticleID}",  "http://www.w3.org/1999/02/22-rdf-syntax-ns#comment", $"{item0.Rdf_comment.ToString()}", list, " . ");
				}
			}
			}
			if(this.Roh_references != null)
			{
			foreach(var item0 in this.Roh_references)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/references", $"<{resourceAPI.GraphsUrl}items/reference_{ResourceID}_{item0.ArticleID}>", list, " . ");
			if(item0.Roh_authorList != null)
			{
			foreach(var item1 in item0.Roh_authorList)
			{
				AgregarTripleALista($"{resourceAPI.GraphsUrl}items/reference_{ResourceID}_{item0.ArticleID}", "http://w3id.org/roh/authorList", $"<{resourceAPI.GraphsUrl}items/referenceauthor_{ResourceID}_{item1.ArticleID}>", list, " . ");
				if(item1.Roh_semanticScholarId != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/referenceauthor_{ResourceID}_{item1.ArticleID}",  "http://w3id.org/roh/semanticScholarId", $"\"{GenerarTextoSinSaltoDeLinea(item1.Roh_semanticScholarId).ToLower()}\"", list, " . ");
				}
				if(item1.Foaf_name != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/referenceauthor_{ResourceID}_{item1.ArticleID}",  "http://xmlns.com/foaf/0.1/name", $"\"{GenerarTextoSinSaltoDeLinea(item1.Foaf_name).ToLower()}\"", list, " . ");
				}
			}
			}
				if(item0.Dct_issued != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/reference_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/terms/issued", $"{item0.Dct_issued.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(item0.Vcard_url != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/reference_{ResourceID}_{item0.ArticleID}",  "https://www.w3.org/2006/vcard/ns#url", $"\"{GenerarTextoSinSaltoDeLinea(item0.Vcard_url).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_hasPublicationVenueText != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/reference_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/hasPublicationVenueText", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_hasPublicationVenueText).ToLower()}\"", list, " . ");
				}
				if(item0.Bibo_doi != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/reference_{ResourceID}_{item0.ArticleID}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(item0.Bibo_doi).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/reference_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Vivo_freeTextKeyword != null)
			{
			foreach(var item0 in this.Vivo_freeTextKeyword)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://vivoweb.org/ontology/core#freeTextKeyword", $"<{resourceAPI.GraphsUrl}items/keyword_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRoh_keyWordConcept != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_keyWordConcept;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/keyword_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/keyWordConcept", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/keyword_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Bibo_identifier != null)
			{
			foreach(var item0 in this.Bibo_identifier)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://purl.org/ontology/bibo/identifier", $"<{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Dc_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item0.ArticleID}",  "http://purl.org/dc/elements/1.1/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Dc_title).ToLower()}\"", list, " . ");
				}
				if(item0.Foaf_topic != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/document_{ResourceID}_{item0.ArticleID}",  "http://xmlns.com/foaf/0.1/topic", $"\"{GenerarTextoSinSaltoDeLinea(item0.Foaf_topic).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Roh_impactIndex != null)
			{
			foreach(var item0 in this.Roh_impactIndex)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/impactIndex", $"<{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.IdRoh_impactIndexCategory != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_impactIndexCategory;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactIndexCategory", $"<{itemRegex}>", list, " . ");
				}
				if(item0.IdRoh_impactSource != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = item0.IdRoh_impactSource;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactSource", $"<{itemRegex}>", list, " . ");
				}
				if(item0.Roh_impactIndexInYear != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactIndexInYear", $"{item0.Roh_impactIndexInYear.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(item0.Roh_impactSourceOther != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/impactSourceOther", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_impactSourceOther).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_journalNumberInCat != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/journalNumberInCat", $"{item0.Roh_journalNumberInCat.Value.ToString()}", list, " . ");
				}
				if(item0.Roh_publicationPosition != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/publicationPosition", $"{item0.Roh_publicationPosition.Value.ToString()}", list, " . ");
				}
				if(item0.Roh_quartile != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/impactindex_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/quartile", $"{item0.Roh_quartile.Value.ToString()}", list, " . ");
				}
			}
			}
			if(this.Roh_enrichedKeywords != null)
			{
			foreach(var item0 in this.Roh_enrichedKeywords)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/enrichedKeywords", $"<{resourceAPI.GraphsUrl}items/enrichedkeyword_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_score != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/enrichedkeyword_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/score", $"{item0.Roh_score.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(item0.Roh_title != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/enrichedkeyword_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_title).ToLower()}\"", list, " . ");
				}
			}
			}
			if(this.Roh_hasMetric != null)
			{
			foreach(var item0 in this.Roh_hasMetric)
			{
				AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/hasMetric", $"<{resourceAPI.GraphsUrl}items/publicationmetric_{ResourceID}_{item0.ArticleID}>", list, " . ");
				if(item0.Roh_metricName != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/publicationmetric_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/metricName", $"\"{GenerarTextoSinSaltoDeLinea(item0.Roh_metricName).ToLower()}\"", list, " . ");
				}
				if(item0.Roh_citationCount != null)
				{
					AgregarTripleALista($"{resourceAPI.GraphsUrl}items/publicationmetric_{ResourceID}_{item0.ArticleID}",  "http://w3id.org/roh/citationCount", $"{item0.Roh_citationCount.ToString()}", list, " . ");
				}
			}
			}
				if(this.IdVcard_hasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVcard_hasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#hasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_supportType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_supportType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/supportType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_presentedAtOrganizer != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_presentedAtOrganizer;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtOrganizer", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_presentedAtOrganizerType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_presentedAtOrganizerType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtOrganizerType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_presentedAtOrganizerHasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_presentedAtOrganizerHasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtOrganizerHasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_presentedAtType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_presentedAtType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_projectAux != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_projectAux;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/projectAux", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsVcard_hasLanguage != null)
				{
					foreach(var item2 in this.IdsVcard_hasLanguage)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "https://www.w3.org/2006/vcard/ns#hasLanguage", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdRoh_presentedAtGeographicFocus != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_presentedAtGeographicFocus;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtGeographicFocus", $"<{itemRegex}>", list, " . ");
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
				if(this.IdsRoh_isProducedBy != null)
				{
					foreach(var item2 in this.IdsRoh_isProducedBy)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/isProducedBy", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdRoh_presentedAtOrganizerHasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_presentedAtOrganizerHasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtOrganizerHasRegion", $"<{itemRegex}>", list, " . ");
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
				if(this.IdRoh_presentedAtHasCountryName != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_presentedAtHasCountryName;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtHasCountryName", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_presentedAtSeminarType != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_presentedAtSeminarType;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtSeminarType", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdsRoh_i_doc_references != null)
				{
					foreach(var item2 in this.IdsRoh_i_doc_references)
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
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/i_doc_references", $"<{itemRegex}>", list, " . ");
					}
				}
				if(this.IdVcard_hasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVcard_hasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#hasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdRoh_presentedAtHasRegion != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_presentedAtHasRegion;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtHasRegion", $"<{itemRegex}>", list, " . ");
				}
				if(this.IdVivo_hasPublicationVenue != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdVivo_hasPublicationVenue;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://vivoweb.org/ontology/core#hasPublicationVenue", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_impactIndexInYear != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/impactIndexInYear", $"{this.Roh_impactIndexInYear.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_isbn != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isbn", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_isbn).ToLower()}\"", list, " . ");
				}
				if(this.Roh_presentedAtLocality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtLocality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_presentedAtGeographicFocusOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtGeographicFocusOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtGeographicFocusOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_year != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/year", $"{this.Roh_year.Value.ToString()}", list, " . ");
				}
				if(this.Roh_presentedAtSeminarTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtSeminarTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtSeminarTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_presentedAtEnd != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtEnd", $"{this.Roh_presentedAtEnd.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_presentedAtStart != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtStart", $"{this.Roh_presentedAtStart.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_semanticScholarCitationCount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/semanticScholarCitationCount", $"{this.Roh_semanticScholarCitationCount.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_abstract != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/abstract", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_abstract).ToLower()}\"", list, " . ");
				}
				if(this.Roh_presentedAtOrganizerTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtOrganizerTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtOrganizerTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_openAccess != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/openAccess", $"\"{this.Roh_openAccess.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_wosCitationCount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/wosCitationCount", $"{this.Roh_wosCitationCount.Value.ToString()}", list, " . ");
				}
				if(this.Roh_presentedAtWithExternalAdmissionsCommittee != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtWithExternalAdmissionsCommittee", $"\"{this.Roh_presentedAtWithExternalAdmissionsCommittee.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Bibo_issue != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/issue", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issue).ToLower()}\"", list, " . ");
				}
				if(this.Dct_issued != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/dc/terms/issued", $"{this.Dct_issued.Value.ToString("yyyyMMddHHmmss")}", list, " . ");
				}
				if(this.Roh_typeOthers != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/typeOthers", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_typeOthers).ToLower()}\"", list, " . ");
				}
				if(this.Roh_crisIdentifier != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/crisIdentifier", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_crisIdentifier).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_url != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#url", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_url).ToLower()}\"", list, " . ");
				}
				if(this.Roh_userKeywords != null)
				{
					foreach(var item2 in this.Roh_userKeywords)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/userKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_scopusCitationCount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/scopusCitationCount", $"{this.Roh_scopusCitationCount.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_publisher != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/publisher", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_publisher).ToLower()}\"", list, " . ");
				}
				if(this.Roh_legalDeposit != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/legalDeposit", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_legalDeposit).ToLower()}\"", list, " . ");
				}
				if(this.Roh_hasFile != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/hasFile", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasFile).ToLower()}\"", list, " . ");
				}
				if(this.Roh_positionIP != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/positionIP", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_positionIP).ToLower()}\"", list, " . ");
				}
				if(this.Roh_publicationTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/publicationTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_publicationTitle).ToLower()}\"", list, " . ");
				}
				if(this.Vcard_locality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "https://www.w3.org/2006/vcard/ns#locality", $"\"{GenerarTextoSinSaltoDeLinea(this.Vcard_locality).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_presentedAt != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/presentedAt", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_presentedAt).ToLower()}\"", list, " . ");
				}
				if(this.Roh_reviewsNumber != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/reviewsNumber", $"{this.Roh_reviewsNumber.Value.ToString(new CultureInfo("en-US"))}", list, " . ");
				}
				if(this.Roh_externalKeywords != null)
				{
					foreach(var item2 in this.Roh_externalKeywords)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/externalKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Roh_presentedAtTypeOther != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtTypeOther", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtTypeOther).ToLower()}\"", list, " . ");
				}
				if(this.Roh_inrecsCitationCount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/inrecsCitationCount", $"{this.Roh_inrecsCitationCount.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_pmid != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/pmid", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pmid).ToLower()}\"", list, " . ");
				}
				if(this.Roh_validationStatusPRC != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/validationStatusPRC", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_validationStatusPRC).ToLower()}\"", list, " . ");
				}
				if(this.Roh_hasPublicationVenueText != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/hasPublicationVenueText", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_hasPublicationVenueText).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_handle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/handle", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_handle).ToLower()}\"", list, " . ");
				}
				if(this.Roh_presentedAtOrganizerTitle != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtOrganizerTitle", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtOrganizerTitle).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_doi != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/doi", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_doi).ToLower()}\"", list, " . ");
				}
				if(this.Roh_suggestedKeywords != null)
				{
					foreach(var item2 in this.Roh_suggestedKeywords)
					{
						AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://w3id.org/roh/suggestedKeywords", $"\"{GenerarTextoSinSaltoDeLinea(item2).ToLower()}\"", list, " . ");
					}
				}
				if(this.Bibo_volume != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/volume", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_volume).ToLower()}\"", list, " . ");
				}
				if(this.Roh_quartile != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/quartile", $"{this.Roh_quartile.Value.ToString()}", list, " . ");
				}
				if(this.Bibo_issn != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/issn", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_issn).ToLower()}\"", list, " . ");
				}
				if(this.Bibo_pageStart != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/pageStart", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pageStart).ToLower()}\"", list, " . ");
				}
				if(this.Roh_congressProceedingsPublication != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/congressProceedingsPublication", $"\"{this.Roh_congressProceedingsPublication.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_presentedAtOrganizerLocality != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/presentedAtOrganizerLocality", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_presentedAtOrganizerLocality).ToLower()}\"", list, " . ");
				}
				if(this.Roh_genderIP != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/genderIP", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_genderIP).ToLower()}\"", list, " . ");
				}
				if(this.Roh_getKeyWords != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/getKeyWords", $"\"{this.Roh_getKeyWords.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Bibo_pageEnd != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://purl.org/ontology/bibo/pageEnd", $"\"{GenerarTextoSinSaltoDeLinea(this.Bibo_pageEnd).ToLower()}\"", list, " . ");
				}
				if(this.Roh_collection != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/collection", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_collection).ToLower()}\"", list, " . ");
				}
				if(this.IdRoh_scientificActivityDocument != null)
				{
					Regex regex = new Regex(@"\/items\/.+_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}_[0-9A-Fa-f]{8}[-]?(?:[0-9A-Fa-f]{4}[-]?){3}[0-9A-Fa-f]{12}");
					string itemRegex = this.IdRoh_scientificActivityDocument;
					if (regex.IsMatch(itemRegex))
					{
						itemRegex = $"http://gnoss/{resourceAPI.GetShortGuid(itemRegex).ToString().ToUpper()}";
					}
					else
					{
						itemRegex = itemRegex.ToLower();
					}
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/scientificActivityDocument", $"<{itemRegex}>", list, " . ");
				}
				if(this.Roh_isValidated != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/isValidated", $"\"{this.Roh_isValidated.ToString().ToLower()}\"", list, " . ");
				}
				if(this.Roh_citationCount != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/citationCount", $"{this.Roh_citationCount.ToString()}", list, " . ");
				}
				if(this.Roh_title != null)
				{
					AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}",  "http://w3id.org/roh/title", $"\"{GenerarTextoSinSaltoDeLinea(this.Roh_title).ToLower()}\"", list, " . ");
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
			string titulo = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string descripcion = $"{this.Roh_title.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\"", "\"\"").Replace("'", "''").Replace("|", "#PIPE#")}";
			string tablaDoc = $"'{titulo}', '{descripcion}', '{resourceAPI.GraphsUrl}', '{tags}'";
			KeyValuePair<Guid, string> valor = new KeyValuePair<Guid, string>(ResourceID, tablaDoc);

			return valor;
		}

		public override string GetURI(ResourceApi resourceAPI)
		{
			return $"{resourceAPI.GraphsUrl}items/DocumentOntology_{ResourceID}_{ArticleID}";
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
