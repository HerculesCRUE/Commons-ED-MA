using System;
using System.Collections.Generic;
using System.Linq;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.Model;
using Es.Riam.Gnoss.Web.MVC.Models;
using Gnoss.ApiWrapper.Interfaces;
using System.Reflection;

namespace GnossBase
{
    public class RDFPropertyAttribute : Attribute
    {
        public RDFPropertyAttribute(string pRDFA)
        {
            mRDFA = pRDFA;
        }
        protected string mRDFA;
        public string RDFProperty
        {
            get { return mRDFA; }
        }
    }

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class LABELAttribute : Attribute
    {
        private readonly GnossOCBase.LanguageEnum midioma;
        private readonly string mlabel;
        public LABELAttribute(GnossOCBase.LanguageEnum idioma, string label)
        {
            mlabel = label;
            midioma = idioma;
        }
        public string LABEL(GnossOCBase.LanguageEnum pLang)
        {
            if (midioma.Equals(pLang))
            {
                return mlabel;
            }
            return "";
        }
    }

    public class GnossOCBase : IGnossOCBase
    {
        public enum LanguageEnum
        {
            es,
            en,
            ca,
            eu,
            gl,
            fr,
        }
        internal List<OntologyEntity> entList = new();
        internal List<OntologyProperty> propList = new();
        internal List<OntologyProperty> imagePropList = new();
        internal List<string> prefList = new();
        internal string mGNOSSID;
        internal Guid resourceID;
        internal Guid articleID;
        private readonly static List<string> NoEnIdiomas = new() { "Não", "Non", "Ez", "Nein", "No" };
        public List<string> tagList = new();
        public GnossOCBase()
        {
            prefList.Add("xmlns:drm=\"http://vocab.data.gov/def/drm#\"");
            prefList.Add("xmlns:vivo=\"http://vivoweb.org/ontology/core#\"");
            prefList.Add("xmlns:rdfs=\"http://www.w3.org/2000/01/rdf-schema#\"");
            prefList.Add("xmlns:owl=\"http://www.w3.org/2002/07/owl#\"");
            prefList.Add("xmlns:qb=\"http://purl.org/linked-data/cube#\"");
            prefList.Add("xmlns:bibo=\"http://purl.org/ontology/bibo/\"");
            prefList.Add("xmlns:roh=\"http://w3id.org/roh/\"");
            prefList.Add("xmlns:dct=\"http://purl.org/dc/terms/\"");
            prefList.Add("xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"");
            prefList.Add("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema#\"");
            prefList.Add("xmlns:schema=\"http://www.schema.org/\"");
            prefList.Add("xmlns:vcard=\"https://www.w3.org/2006/vcard/ns#\"");
            prefList.Add("xmlns:dc=\"http://purl.org/dc/elements/1.1/\"");
            prefList.Add("xmlns:skos=\"http://www.w3.org/2008/05/skos#\"");
            prefList.Add("xmlns:foaf=\"http://xmlns.com/foaf/0.1/\"");
            prefList.Add("xmlns:obo=\"http://purl.obolibrary.org/obo/\"");
            prefList.Add("xmlns:gn=\"http://www.geonames.org/ontology#\"");

            this.resourceID = Guid.NewGuid();
            this.articleID = Guid.NewGuid();
        }

        public string GNOSSID
        {
            get { return mGNOSSID; }
            set
            {
                this.mGNOSSID = value;
                var GnossIDSplit = this.mGNOSSID.Split('_');
                Guid nuevoResource = Guid.Parse(GnossIDSplit[^2]);
                Guid nuevoArticle = Guid.Parse(GnossIDSplit.Last());
                if (!this.ResourceID.Equals(nuevoResource))
                {
                    this.resourceID = nuevoResource;
                }
                if (!this.ArticleID.Equals(nuevoArticle))
                {
                    this.articleID = nuevoArticle;
                }
            }
        }

        public Guid ResourceID
        {
            get { return resourceID; }
            set
            {
                this.resourceID = value;
                string primeraParte = this.mGNOSSID.Substring(0, this.mGNOSSID.LastIndexOf('/') + 1);
                string antiguoGuid = this.mGNOSSID.Substring(this.mGNOSSID.LastIndexOf('/') + 1, this.mGNOSSID.LastIndexOf('_'));
                string ultimaParte = this.mGNOSSID.Substring(this.mGNOSSID.LastIndexOf('_') + 1);
                if (!antiguoGuid.Equals(this.resourceID.ToString()))
                {
                    this.mGNOSSID = $"{primeraParte}{this.resourceID}{ultimaParte}";
                }
            }
        }

        public Guid ArticleID
        {
            get { return articleID; }
            set
            {
                this.articleID = value;
                string primeraParte = this.mGNOSSID.Substring(0, this.mGNOSSID.LastIndexOf('_') + 1);
                string antiguoGuid = this.mGNOSSID.Substring(this.mGNOSSID.LastIndexOf('_') + 1);
                if (!antiguoGuid.Equals(this.articleID.ToString()))
                {
                    this.mGNOSSID = $"{primeraParte}{this.articleID}";
                }
            }
        }

        public static string GetPropertyValueSemCms(SemanticPropertyModel pProperty)
        {
            if (pProperty != null && pProperty.PropertyValues.Count > 0)
            {
                return pProperty.PropertyValues[0].Value;
            }
            return "";
        }

        public static int? GetNumberIntPropertyValueSemCms(SemanticPropertyModel pProperty)
        {
            if (pProperty != null && pProperty.PropertyValues.Count > 0 && !string.IsNullOrEmpty(pProperty.PropertyValues[0].Value))
            {
                return int.Parse(pProperty.PropertyValues[0].Value);
            }
            return 0;
        }

        public static int? GetNumberIntPropertyMultipleValueSemCms(SemanticPropertyModel.PropertyValue pProperty)
        {
            if (pProperty != null && !string.IsNullOrEmpty(pProperty.Value))
            {
                return int.Parse(pProperty.Value);
            }
            return 0;
        }

        public static float? GetNumberFloatPropertyValueSemCms(SemanticPropertyModel pProperty)
        {
            if (pProperty != null && pProperty.PropertyValues.Count > 0 && !string.IsNullOrEmpty(pProperty.PropertyValues[0].Value))
            {
                return float.Parse(pProperty.PropertyValues[0].Value.Replace('.', ','));
            }
            return 0;
        }

        public static DateTime? GetDateValuePropertySemCms(SemanticPropertyModel pProperty)
        {
            string stringDate = GetPropertyValueSemCms(pProperty);
            if (!string.IsNullOrEmpty(stringDate))
            {
                int year;
                int month;
                int day;
                if (stringDate.Contains('/'))
                {
                    day = int.Parse(stringDate.Split('/')[0]);
                    month = int.Parse(stringDate.Split('/')[1]);
                    year = int.Parse(stringDate.Split('/')[2].Split(' ')[0]);
                }
                else
                {
                    year = int.Parse(stringDate.Substring(0, 4));
                    month = int.Parse(stringDate.Substring(4, 2));
                    day = int.Parse(stringDate.Substring(6, 2));
                }
                if (stringDate.Length == 14)
                {
                    if (month == 0 || day == 0)
                    {
                        return new DateTime(year);
                    }
                    else
                    {
                        int hour = int.Parse(stringDate.Substring(8, 2));
                        int minute = int.Parse(stringDate.Substring(10, 2));
                        int second = int.Parse(stringDate.Substring(12, 2));
                        return new DateTime(year, month, day, hour, minute, second);
                    }
                }
                else
                {
                    return new DateTime(year, month, day);
                }
            }
            return null;
        }

        public static bool GetBooleanPropertyValueSemCms(SemanticPropertyModel pProperty)
        {
            bool resultado = false;
            if (pProperty != null && pProperty.PropertyValues.Count > 0 && !string.IsNullOrEmpty(pProperty.PropertyValues[0].Value))
            {
                if (!bool.TryParse(pProperty.PropertyValues[0].Value, out resultado))
                {
                    resultado = !NoEnIdiomas.Contains(pProperty.PropertyValues[0].Value);
                }
            }
            return resultado;
        }

        internal virtual void GetProperties()
        {
        }

        internal virtual void GetEntities()
        {
        }


        public string GetPropertyURI(string nombrePropiedad)
        {
            Type type = this.GetType();
            PropertyInfo mInfo = type.GetProperty(nombrePropiedad);
            if (mInfo != null)
            {
                Attribute attr = Attribute.GetCustomAttribute(mInfo, typeof(RDFPropertyAttribute));
                if (attr != null)
                {
                    return ((RDFPropertyAttribute)attr).RDFProperty;
                }
            }
            return "";
        }



        public string GetLabel(string nombrePropiedad, LanguageEnum pLang)
        {
            Type type = this.GetType();
            PropertyInfo mInfo = type.GetProperty(nombrePropiedad);

            if (mInfo != null)
            {
                Attribute[] attr = Attribute.GetCustomAttributes(mInfo, typeof(LABELAttribute));
                {
                    foreach (Attribute atributo in attr)
                    {
                        if (atributo != null)
                        {
                            if (!((LABELAttribute)atributo).LABEL(pLang).Equals(""))
                            {
                                return ((LABELAttribute)atributo).LABEL(pLang);
                            }
                        }
                    }
                }
            }

            return "";
        }



        public virtual List<string> ToOntologyGnossTriples(ResourceApi pResourceApi) { throw new NotImplementedException(); }

        public virtual List<string> ToSearchGraphTriples(ResourceApi pResourceApi) { throw new NotImplementedException(); }

        public virtual KeyValuePair<Guid, string> ToAcidData(ResourceApi resourceAPI) { throw new NotImplementedException(); }

        public virtual string GetURI(ResourceApi resourceAPI) { throw new NotImplementedException(); }

        public int GetID() { return 0; }
    }
}
