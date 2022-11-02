using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Similarity
{
    public class UtilsSimilarity
    {
        private readonly string mUrlSimilarity;
        private readonly ResourceApi mResourceApi;
        private readonly string mType;
        private readonly string mRdfType;
        private readonly string mGraph;
        public UtilsSimilarity(string pUrlSimilarity, ResourceApi pResourceApi, string pType)
        {
            mUrlSimilarity = pUrlSimilarity;
            mResourceApi = pResourceApi;
            mType = pType;
            switch (mType)
            {
                case "research_paper":
                    mRdfType = "http://purl.org/ontology/bibo/Document";
                    mGraph = "document";
                    break;
                case "code_project":
                    mRdfType = "http://w3id.org/roh/ResearchObject";
                    mGraph = "researchobject";
                    break;
                default:
                    throw new ArgumentException("El tipo " + pType + " no está permitido, sólo está permitido 'research_paper' o 'code_project'");
            }
        }
        public List<string> GetItemsLoaded()
        {
            List<string> idsLoaded = new List<string>();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(1);
            var responseGet = client.GetAsync(mUrlSimilarity + "ro-collection?ro_type_target=" + mType).Result;
            if (responseGet.IsSuccessStatusCode)
            {
                idsLoaded = responseGet.Content.ReadAsAsync<List<string>>().Result;
            }
            return idsLoaded;
        }

        public List<string> GetItemsToLoad()
        {
            List<string> idsToLoad = new List<string>();

            string select = "select distinct ?doc";
            string where = $@"
where{{
    ?doc a <{mRdfType}>.
    ?doc <http://w3id.org/roh/isValidated> 'true'.
}}";
            var response = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { mGraph , "curriculumvitae" });
            return response.results.bindings.Select(x => x["doc"].value).ToList();

        }

        public EnrichmentSimilarityItem GetItemToLoad(string pId)
        {
            return GetItemsToLoad(new List<string>() { pId })[pId];
        }

        public Dictionary<string, EnrichmentSimilarityItem> GetItemsToLoad(List<string> pIds)
        {
            //TODO limit 10000
            Dictionary<string, EnrichmentSimilarityItem> respuesta = new Dictionary<string, EnrichmentSimilarityItem>();
            foreach (string id in pIds)
            {
                respuesta[id] = null;
            }

            //Obtenemos título y descripción
            string select = "select ?doc ?title ?abstract";
            string where = $@"
where{{
    ?doc a <{mRdfType}>.
    ?doc <http://w3id.org/roh/title> ?title.
    FILTER(?doc in (<{string.Join(">,<", pIds)}>))
    OPTIONAL{{?doc <http://purl.org/ontology/bibo/abstract> ?abstract}}
    {{ ?doc <http://w3id.org/roh/isValidated> 'true'.}}
    UNION
    {{  
        ?cv a <http://w3id.org/roh/CV>.
        ?cv ?p1 ?o1.
        ?o1 ?p2 ?item.
        ?item <http://vivoweb.org/ontology/core#relatedBy> ?doc.
        ?item <http://w3id.org/roh/isPublic> 'true'.
    }}
}}";
            SparqlObject response = mResourceApi.VirtuosoQuery(select, where, mGraph);
            foreach (Dictionary<string, SparqlObject.Data> fila in response.results.bindings)
            {
                string text = fila["title"].value.Trim();
                if (fila.ContainsKey("abstract"))
                {
                    text += " " + fila["abstract"].value.Trim();
                }
                respuesta[fila["doc"].value] = new EnrichmentSimilarityItem();
                respuesta[fila["doc"].value].ro_id = fila["doc"].value;
                respuesta[fila["doc"].value].ro_type = mType;
                respuesta[fila["doc"].value].text = text;
                respuesta[fila["doc"].value].authors = new List<string>();
                respuesta[fila["doc"].value].specific_descriptors = new List<List<object>>();
                respuesta[fila["doc"].value].thematic_descriptors = new List<List<object>>();
            }

            pIds = respuesta.Where(x => x.Value != null).Select(x => x.Key).ToList();

            if (pIds.Count > 0)
            {
                #region Obtenemos autores
                select = "select ?doc ?orden ?authorName";
                where = $@"
where
{{
	?doc a <{mRdfType}>.
	?doc <http://purl.org/ontology/bibo/authorList> ?author.
    FILTER(?doc in(<{string.Join(">,<", pIds)}>))
	?author <http://www.w3.org/1999/02/22-rdf-syntax-ns#comment> ?ordenAux.
    ?author <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
    ?person <http://xmlns.com/foaf/0.1/name> ?authorName
	BIND(xsd:int(?ordenAux) as ?orden)
}}order by asc(?orden) ";
                response = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { mGraph ,"person"});
                foreach (Dictionary<string, SparqlObject.Data> fila in response.results.bindings)
                {
                    string id = fila["doc"].value;
                    string authorName = fila["authorName"].value.Trim();
                    EnrichmentSimilarityItem enrichmentSimilarityItem = respuesta[id];
                    if (enrichmentSimilarityItem != null)
                    {
                        enrichmentSimilarityItem.authors.Add(authorName);
                    }
                }
                #endregion

                #region Obtenemos etiquetas

                switch (mType)
                {
                    case "research_paper":
                        select = "select ?doc ?tag";
                        where = $@"
where
{{
	?doc a <{mRdfType}>.
	?doc <http://vivoweb.org/ontology/core#freeTextKeyword> ?tagAux.
    ?tagAux <http://w3id.org/roh/title> ?tag
    FILTER(?doc in(<{string.Join(">,<", pIds)}>))
}} ";
                        response = mResourceApi.VirtuosoQuery(select, where, mGraph);
                        foreach (Dictionary<string, SparqlObject.Data> fila in response.results.bindings)
                        {
                            string id = fila["doc"].value;
                            string tag = fila["tag"].value.Trim();
                            EnrichmentSimilarityItem enrichmentSimilarityItem = respuesta[id];
                            if (enrichmentSimilarityItem != null)
                            {
                                enrichmentSimilarityItem.specific_descriptors.Add(new List<object>() { tag, 1 });
                            }
                        }
                        break;
                    case "code_project":
                        select = "select ?doc ?tag";
                        where = $@"
where
{{
	?doc a <{mRdfType}>.
	?doc <http://vivoweb.org/ontology/core#freeTextKeyword> ?tag.
    FILTER(?doc in(<{string.Join(">,<", pIds)}>))
}} ";
                        response = mResourceApi.VirtuosoQuery(select, where, mGraph);
                        foreach (Dictionary<string, SparqlObject.Data> fila in response.results.bindings)
                        {
                            string id = fila["doc"].value;
                            string tag = fila["tag"].value.Trim();
                            EnrichmentSimilarityItem enrichmentSimilarityItem = respuesta[id];
                            if (enrichmentSimilarityItem != null)
                            {
                                enrichmentSimilarityItem.specific_descriptors.Add(new List<object>() { tag, 1 });
                            }
                        }
                        break;
                }



                select = "select ?doc ?tag ?score";
                where = $@"
where
{{
	?doc a <{mGraph}>.
	?doc <http://w3id.org/roh/enrichedKeywords> ?enrichedKeywords.
    ?enrichedKeywords <http://w3id.org/roh/title> ?tag.
    ?enrichedKeywords <http://w3id.org/roh/score> ?score.
    FILTER(?doc in(<{string.Join(">,<", pIds)}>))
}} ";
                response = mResourceApi.VirtuosoQuery(select, where, mGraph);
                foreach (Dictionary<string, SparqlObject.Data> fila in response.results.bindings)
                {
                    string id = fila["doc"].value;
                    string tag = fila["tag"].value.Trim();
                    float score = float.Parse(fila["score"].value.Trim().Replace(",", "."), CultureInfo.InvariantCulture);
                    EnrichmentSimilarityItem enrichmentSimilarityItem = respuesta[id];
                    if (enrichmentSimilarityItem != null)
                    {
                        if (enrichmentSimilarityItem.specific_descriptors.Exists(x => (string)x[0] == tag))
                        {
                            enrichmentSimilarityItem.specific_descriptors.First(x => (string)x[0] == tag)[1] = score;
                        }
                        else
                        {
                            enrichmentSimilarityItem.specific_descriptors.Add(new List<object>() { tag, score });
                        }
                    }
                }
                #endregion

                #region Obtenemos categorías

                select = "select ?doc ?category";
                where = $@"
where
{{
	?doc a <{mRdfType}>.
    ?doc  <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaAux.
    ?hasKnowledgeAreaAux <http://w3id.org/roh/categoryNode> ?categoryNode.
    ?categoryNode <http://www.w3.org/2008/05/skos#prefLabel> ?category
    MINUS{{
        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
    }}
    FILTER(?doc in(<{string.Join(">,<", pIds)}>))
}} ";
                response = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { mGraph , "taxonomy" });
                foreach (Dictionary<string, SparqlObject.Data> fila in response.results.bindings)
                {
                    string id = fila["doc"].value;
                    string category = fila["category"].value.Trim();
                    EnrichmentSimilarityItem enrichmentSimilarityItem = respuesta[id];
                    if (enrichmentSimilarityItem != null)
                    {
                        enrichmentSimilarityItem.thematic_descriptors.Add(new List<object>() { category, 1 });
                    }
                }
                #endregion
        
            }
            return respuesta;
        }

        public EnrichmentSimilarityItem GetItemLoaded(string pId)
        {
            EnrichmentSimilarityItem respuesta = null;

            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(1);
            var responseGet = client.GetAsync(mUrlSimilarity + "ro?ro_id=" + pId).Result;
            if (responseGet.IsSuccessStatusCode)
            {
                respuesta = responseGet.Content.ReadAsAsync<EnrichmentSimilarityItem>().Result;
            }
            return respuesta;
        }

        public bool LoadItem(EnrichmentSimilarityItem pEnrichmentSimilarityItem)
        {
            bool ok = true;
            if (pEnrichmentSimilarityItem != null)
            {

                // Cliente.
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromDays(1);

                // Conversión de los datos.
                string informacion = JsonConvert.SerializeObject(pEnrichmentSimilarityItem, new DecimalFormatConverter());
                StringContent contentData = new StringContent(informacion, Encoding.UTF8, "application/json");

                var responsePut = client.PutAsync(mUrlSimilarity + "ro", contentData).Result;
                if (!responsePut.IsSuccessStatusCode)
                {
                    ok = false;
                }
            }
            return ok;
        }

        public bool DeleteItem(string pId)
        {
            bool ok = true;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(1);
            var responseDelete = client.DeleteAsync(mUrlSimilarity + "ro?ro_id=" + pId).Result;
            if (!responseDelete.IsSuccessStatusCode)
            {
                ok = false;
            }
            return ok;
        }

        public List<KeyValuePair<Guid, Dictionary<string, float>>> GetSimilars(string pId)
        {
            string rdfType = "";
            string graph = "";
            switch (mType)
            {
                case "research_paper":
                    rdfType = "http://purl.org/ontology/bibo/Document";
                    graph = "document";
                    break;
                case "code_project":
                    rdfType = "http://w3id.org/roh/ResearchObject";
                    graph = "researchobject";
                    break;
            }

            Dictionary<string, Dictionary<string, float>> dicSimilarsAux = new Dictionary<string, Dictionary<string, float>>();

            // Cliente.
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromDays(1);

            var responseGet = client.GetAsync(mUrlSimilarity + "similar?ro_id=" + pId + "&ro_type_target=" + mType).Result;
            if (responseGet.IsSuccessStatusCode)
            {
                EnrichmentSimilarityGetResponse responseGetObject = responseGet.Content.ReadAsAsync<EnrichmentSimilarityGetResponse>().Result;
                dicSimilarsAux = responseGetObject.similar_ros_calculado;
                dicSimilarsAux.Remove(pId);
            }

            if (dicSimilarsAux.Count > 0)
            {
                //Hacemos una verificación para que sólo se devuelvan validados
                string select = "select distinct ?id";
                string where = $@"
where{{
    FILTER(?id in (<{string.Join(">,<", dicSimilarsAux.Keys)}>))
    ?id a <{rdfType}>.
    ?id <http://w3id.org/roh/isValidated> 'true'.
}}";
                List<string> listID = mResourceApi.VirtuosoQueryMultipleGraph(select, where,new List<string> { graph , "curriculumvitae" }).results.bindings.Select(x => x["id"].value).ToList();
                dicSimilarsAux = dicSimilarsAux.Where(x => listID.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            }
            List<KeyValuePair<Guid, Dictionary<string, float>>> dicSimilars = dicSimilarsAux.ToDictionary(x => mResourceApi.GetShortGuid(x.Key), x => x.Value).ToList();
            return dicSimilars;
        }

    }

    public class EnrichmentSimilarityItem
    {
        public string ro_id { get; set; }
        public string ro_type { get; set; }
        public string text { get; set; }
        public List<string> authors { get; set; }
        public List<List<object>> thematic_descriptors { get; set; }
        public List<List<object>> specific_descriptors { get; set; }
        public Dictionary<string, double> GetThematic_descriptors_transform()
        {
            Dictionary<string, double> thematics = new Dictionary<string, double>();
            foreach (List<object> thematic in thematic_descriptors)
            {
                string name = (string)thematic[0];
                double score;
                if (thematic[1] is int)
                {
                    score = (int)thematic[1];
                }
                else
                {
                    score = (double)thematic[1];
                }
                thematics[name] = score;
            }
            return thematics;
        }
        public Dictionary<string, double> GetSpecific_descriptors_transform()
        {
            Dictionary<string, double> specifics = new Dictionary<string, double>();
            foreach (List<object> specific in specific_descriptors)
            {
                string name = (string)specific[0];
                double score;
                if (specific[1] is int)
                {
                    score = (int)specific[1];
                }
                else
                {
                    score = (double)specific[1];
                }
                specifics[name] = score;
            }
            return specifics;
        }

        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                EnrichmentSimilarityItem p = (EnrichmentSimilarityItem)obj;
                bool equals = true;
                equals = equals && ro_id == p.ro_id;
                equals = equals && ro_type == p.ro_type;
                equals = equals && text == p.text;
                equals = equals && string.Join(",", authors).Equals(string.Join(",", p.authors));
                equals = equals && GetThematic_descriptors_transform().Count == p.GetThematic_descriptors_transform().Count;
                equals = equals && GetSpecific_descriptors_transform().Count == p.GetSpecific_descriptors_transform().Count;
                if (equals)
                {
                    foreach (KeyValuePair<string, double> thematic in GetThematic_descriptors_transform())
                    {
                        if (!p.GetThematic_descriptors_transform().ContainsKey(thematic.Key) ||
                            p.GetThematic_descriptors_transform()[thematic.Key] != GetThematic_descriptors_transform()[thematic.Key])
                        {
                            equals = false;
                        }
                    }
                }
                if (equals)
                {
                    foreach (KeyValuePair<string, double> specific in GetSpecific_descriptors_transform())
                    {
                        if (!p.GetSpecific_descriptors_transform().ContainsKey(specific.Key) ||
                            p.GetSpecific_descriptors_transform()[specific.Key] != GetSpecific_descriptors_transform()[specific.Key])
                        {
                            equals = false;
                        }
                    }
                }
                return equals;
            }
        }

        public override int GetHashCode()
        {
            return text.GetHashCode();
        }
    }

    public class EnrichmentSimilarityGetResponse
    {
        public List<object> similar_ros { get; set; }
        public Dictionary<string, Dictionary<string, float>> similar_ros_calculado
        {
            get
            {
                Dictionary<string, Dictionary<string, float>> dic = new Dictionary<string, Dictionary<string, float>>();
                foreach (var item in similar_ros)
                {
                    string id = (string)((Newtonsoft.Json.Linq.JArray)item)[0];
                    if (!dic.ContainsKey(id))
                    {
                        dic.Add(id, new Dictionary<string, float>());
                    }
                    var tags = ((Newtonsoft.Json.Linq.JArray)item)[1];
                    foreach (var tag in tags)
                    {
                        string tagNombre = (string)((Newtonsoft.Json.Linq.JArray)tag)[0];
                        float tagPeso = (float)((Newtonsoft.Json.Linq.JArray)tag)[1];
                        if (!dic[id].ContainsKey(tagNombre))
                        {
                            dic[id].Add(tagNombre, tagPeso);
                        }
                    }
                }

                return dic;
            }
        }
    }

    public class DecimalFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(int));
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {
            if (value.GetType() == 1.GetType())
            {
                double x = (int)value;
                writer.WriteValue(x);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType,
                                     object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
