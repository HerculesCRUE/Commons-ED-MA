using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Hercules.CommonsEDMA.Desnormalizador.Models.Actualizadores;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Hercules.CommonsEDMA.Desnormalizador.Models.Similarity
{
    public class UtilsSimilarity
    {
        private readonly string mUrlSimilarity;
        private readonly ResourceApi mResourceApi;
        private readonly string mType;
        private readonly string mRdfType;
        private readonly string mGraph;
        private readonly string mQueryVisible = $@"{{ ?doc <http://w3id.org/roh/isValidated> 'true'.}}";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pUrlSimilarity">Url del servicio de similaridad</param>
        /// <param name="pResourceApi">ResourceApi</param>
        /// <param name="pType">Tipo ('research_paper' o 'code_project')</param>
        public UtilsSimilarity(string pUrlSimilarity, ResourceApi pResourceApi, string pType)
        {
            mUrlSimilarity = pUrlSimilarity;
            mResourceApi = pResourceApi;
            mType = pType;
            switch (mType)
            {
                case "research_paper":
                    mRdfType = $"{ActualizadorBase.GetUrlPrefix("bibo")}Document";
                    mGraph = "document";
                    break;
                case "code_project":
                    mRdfType = $"{ActualizadorBase.GetUrlPrefix("roh")}ResearchObject";
                    mGraph = "researchobject";
                    break;
                default:
                    throw new Exception("El tipo " + pType + " no está permitido, sólo está permitido 'research_paper' o 'code_project'");
            }
        }

        /// <summary>
        /// Obtiene los identificadores de los items cargados en el servicio de similaridad
        /// </summary>
        /// <returns>Listado con los IDs</returns>
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

        /// <summary>
        /// Obtiene los IDs de los elementos que deberían estar cargados
        /// </summary>
        /// <returns>IDs de los elementos que deberían estar cargados</returns>
        public List<string> GetItemsToLoad()
        {
            List<string> idsToLoad = new List<string>();
            int limit = 10000;
            int offset = 0;
            while (true)
            {
                string select = "select * where{select distinct ?doc ";
                string where = $@"
where{{
    ?doc a <{mRdfType}>.
    {mQueryVisible}
}}ORDER BY ASC(?doc)}} LIMIT {limit} OFFSET {offset}";
                SparqlObject resultadoQuery = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { mGraph, "curriculumvitae" });
                idsToLoad = idsToLoad.Union(resultadoQuery.results.bindings.Select(x => x["doc"].value)).ToList();
                offset += limit;
                if (resultadoQuery.results.bindings.Count < limit)
                {
                    break;
                }
            }
            idsToLoad = idsToLoad.Distinct().ToList();
            return idsToLoad;

        }

        /// <summary>
        /// Obtiene los datos de un elemento para cargar
        /// </summary>
        /// <param name="pId">ID del elemento</param>
        /// <returns>Elemento para cargar</returns>
        public EnrichmentSimilarityItem GetItemToLoad(string pId)
        {
            return GetItemsToLoad(new List<string>() { pId })[pId];
        }

        /// <summary>
        /// Obtiene los datos de N elementos para cargar
        /// </summary>
        /// <param name="pIds">ID del elemento</param>
        /// <returns>Elementos para cargar</returns>
        public Dictionary<string, EnrichmentSimilarityItem> GetItemsToLoad(List<string> pIds)
        {
            int numItems = 1000;
            int limit = 10000;
            List<List<string>> listIds = SplitList(pIds, numItems).ToList();
            Dictionary<string, EnrichmentSimilarityItem> respuestaFinal = new Dictionary<string, EnrichmentSimilarityItem>();

            foreach (List<string> ids in listIds)
            {
                List<string> idsAux = new List<string>(ids);

                Dictionary<string, EnrichmentSimilarityItem> respuesta = new Dictionary<string, EnrichmentSimilarityItem>();
                foreach (string id in idsAux)
                {
                    respuesta[id] = null;
                }

                #region Obtenemos título y descripción
                string selectTitDesc = "select ?doc ?title ?abstract";
                string whereTitDesc = $@"
where{{
    ?doc a <{mRdfType}>.
    ?doc <http://w3id.org/roh/title> ?title.
    FILTER(?doc in (<{string.Join(">,<", idsAux)}>))
    OPTIONAL{{?doc <http://purl.org/ontology/bibo/abstract> ?abstract}}
    {mQueryVisible}
}}";
                SparqlObject responseTitDdesc = mResourceApi.VirtuosoQuery(selectTitDesc, whereTitDesc, mGraph);
                foreach (Dictionary<string, SparqlObject.Data> fila in responseTitDdesc.results.bindings)
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
                #endregion

                //Continuamos sólo con aquellos que tengan datos
                idsAux = respuesta.Where(x => x.Value != null).Select(x => x.Key).ToList();

                if (idsAux.Count > 0)
                {
                    int offset = 0;

                    #region Obtenemos autores
                    while (true)
                    {
                        string selectAutores = "select * where{select ?doc ?orden ?authorName ";
                        string whereAutores = $@"
where
{{
	?doc a <{mRdfType}>.
	?doc <http://purl.org/ontology/bibo/authorList> ?author.
    FILTER(?doc in(<{string.Join(">,<", idsAux)}>))
	?author <http://www.w3.org/1999/02/22-rdf-syntax-ns#comment> ?ordenAux.
    ?author <http://www.w3.org/1999/02/22-rdf-syntax-ns#member> ?person.
    ?person <http://xmlns.com/foaf/0.1/name> ?authorName
	BIND(xsd:int(?ordenAux) as ?orden)
}}order by asc(?doc) asc(?orden) }} LIMIT {limit} OFFSET {offset}";
                        SparqlObject responseAutores = mResourceApi.VirtuosoQueryMultipleGraph(selectAutores, whereAutores, new List<string>() { mGraph, "person" });
                        offset += limit;
                        foreach (Dictionary<string, SparqlObject.Data> fila in responseAutores.results.bindings)
                        {
                            string id = fila["doc"].value;
                            string authorName = fila["authorName"].value.Trim();
                            EnrichmentSimilarityItem enrichmentSimilarityItem = respuesta[id];
                            if (enrichmentSimilarityItem != null)
                            {
                                enrichmentSimilarityItem.authors.Add(authorName);
                            }
                        }
                        if (responseAutores.results.bindings.Count < limit)
                        {
                            break;
                        }
                    }
                    #endregion

                    #region Obtenemos etiquetas
                    offset = 0;
                    while (true)
                    {
                        string selectTags = "select * where{select ?doc ?tag";
                        string whereTags = "";
                        switch (mType)
                        {
                            case "research_paper":
                                whereTags = $@"
                                        where
                                        {{
	                                        ?doc a <{mRdfType}>.
	                                        ?doc <http://vivoweb.org/ontology/core#freeTextKeyword> ?tagAux.
                                            ?tagAux <http://w3id.org/roh/title> ?tag
                                            FILTER(?doc in(<{string.Join(">,<", idsAux)}>))
                                        }} ORDER BY ASC(?doc)}} LIMIT {limit} OFFSET {offset}";
                                break;
                            case "code_project":
                                whereTags = $@"
                                        where
                                        {{
	                                        ?doc a <{mRdfType}>.
	                                        ?doc <http://vivoweb.org/ontology/core#freeTextKeyword> ?tag.
                                            FILTER(?doc in(<{string.Join(">,<", idsAux)}>))
                                        }}  ORDER BY ASC(?doc)}} LIMIT {limit} OFFSET {offset}";
                                break;
                        }

                        SparqlObject responseTags = mResourceApi.VirtuosoQuery(selectTags, whereTags, mGraph);
                        offset += limit;
                        foreach (Dictionary<string, SparqlObject.Data> fila in responseTags.results.bindings)
                        {
                            string id = fila["doc"].value;
                            string tag = fila["tag"].value.Trim();
                            EnrichmentSimilarityItem enrichmentSimilarityItem = respuesta[id];
                            if (enrichmentSimilarityItem != null)
                            {
                                enrichmentSimilarityItem.specific_descriptors.Add(new List<object>() { tag, 1 });
                            }
                        }
                        if (responseTags.results.bindings.Count < limit)
                        {
                            break;
                        }
                    }


                    offset = 0;
                    while (true)
                    {
                        string selectTags2 = "select * where{select ?doc ?tag ?score";
                        string whereTags2 = $@"
where
{{
	?doc a <{mRdfType}>.
	?doc <http://w3id.org/roh/enrichedKeywords> ?enrichedKeywords.
    ?enrichedKeywords <http://w3id.org/roh/title> ?tag.
    ?enrichedKeywords <http://w3id.org/roh/score> ?score.
    FILTER(?doc in(<{string.Join(">,<", idsAux)}>))
}} ORDER BY ASC(?doc)}} LIMIT {limit} OFFSET {offset}";
                        SparqlObject responseTags2 = mResourceApi.VirtuosoQuery(selectTags2, whereTags2, mGraph);
                        offset += limit;
                        foreach (Dictionary<string, SparqlObject.Data> fila in responseTags2.results.bindings)
                        {
                            string id = fila["doc"].value;
                            string tag = fila["tag"].value.Trim();
                            float score = float.Parse(fila["score"].value.Trim().Replace(",", "."), CultureInfo.InvariantCulture);
                            if (score > 1)
                            {
                                score = 1;
                            }
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
                        if (responseTags2.results.bindings.Count < limit)
                        {
                            break;
                        }
                    }
                    #endregion

                    #region Obtenemos categorías
                    offset = 0;
                    while (true)
                    {
                        string selectCat = "select * where{select ?doc ?category ";
                        string whereCat = $@"
where
{{
	?doc a <{mRdfType}>.
    ?doc  <http://w3id.org/roh/hasKnowledgeArea> ?hasKnowledgeAreaAux.
    ?hasKnowledgeAreaAux <http://w3id.org/roh/categoryNode> ?categoryNode.
    ?categoryNode <http://www.w3.org/2008/05/skos#prefLabel> ?category
    MINUS{{
        ?categoryNode <http://www.w3.org/2008/05/skos#narrower> ?hijo.
    }}
    FILTER(?doc in(<{string.Join(">,<", idsAux)}>))
}} ORDER BY ASC(?doc)}} LIMIT {limit} OFFSET {offset}";
                        SparqlObject responseCat = mResourceApi.VirtuosoQueryMultipleGraph(selectCat, whereCat, new List<string>() { mGraph, "taxonomy" });
                        offset += limit;
                        foreach (Dictionary<string, SparqlObject.Data> fila in responseCat.results.bindings)
                        {
                            string id = fila["doc"].value;
                            string category = fila["category"].value.Trim();
                            EnrichmentSimilarityItem enrichmentSimilarityItem = respuesta[id];
                            if (enrichmentSimilarityItem != null)
                            {
                                enrichmentSimilarityItem.thematic_descriptors.Add(new List<object>() { category, 1 });
                            }
                        }
                        if (responseCat.results.bindings.Count < limit)
                        {
                            break;
                        }
                    }
                    #endregion
                }
                foreach (string id in respuesta.Keys)
                {
                    respuestaFinal[id] = respuesta[id];
                }
            }
            return respuestaFinal;
        }

        /// <summary>
        /// Otiene los datos de un elemento cargado
        /// </summary>
        /// <param name="pId">ID del elemento</param>
        /// <returns>Elemento cargado</returns>
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

        /// <summary>
        /// Carga un item
        /// </summary>
        /// <param name="pEnrichmentSimilarityItem">Item cargado</param>
        /// <returns>True si se ha procesado correctamente</returns>
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

        /// <summary>
        /// Se elimina un item
        /// </summary>
        /// <param name="pId">ID del elemento</param>
        /// <returns>True si se ha procesado correctamente</returns>
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

        /// <summary>
        /// Obitiene los elementos similares al elemento pasado por parámetro
        /// </summary>
        /// <param name="pId">ID del elemento</param>
        /// <returns>Elementos similares</returns>
        public List<KeyValuePair<Guid, Dictionary<string, float>>> GetSimilars(string pId)
        {
            string rdfType = "";
            string graph = "";
            switch (mType)
            {
                case "research_paper":
                    rdfType = $"{ActualizadorBase.GetUrlPrefix("bibo")}Document";
                    graph = "document";
                    break;
                case "code_project":
                    rdfType = $"{ActualizadorBase.GetUrlPrefix("roh")}ResearchObject";
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
            else
            {

            }
            if (dicSimilarsAux.Count > 0)
            {
                //Hacemos una verificación para que sólo se devuelvan validados
                string select = "select distinct ?id ";
                string where = $@"
where{{
    FILTER(?id in (<{string.Join(">,<", dicSimilarsAux.Keys)}>))
    ?id a <{rdfType}>.
    ?id <http://w3id.org/roh/isValidated> 'true'.

}}";
                List<string> listID = mResourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string>() { graph, "curriculumvitae" }).results.bindings.Select(x => x["id"].value).ToList();
                dicSimilarsAux = dicSimilarsAux.Where(x => listID.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            }
            List<KeyValuePair<Guid, Dictionary<string, float>>> dicSimilars = dicSimilarsAux.ToDictionary(x => mResourceApi.GetShortGuid(x.Key), x => x.Value).ToList();
            return dicSimilars;
        }

        /// <summary>
        /// Trocea una lista en listas de N tamaño
        /// </summary>
        /// <typeparam name="T">Clased de los elementos de la lista</typeparam>
        /// <param name="pList">Lista</param>
        /// <param name="nSize">Tamaño de la lista</param>
        /// <returns></returns>
        private IEnumerable<List<T>> SplitList<T>(List<T> pList, int pSize)
        {
            for (int i = 0; i < pList.Count; i += pSize)
            {
                yield return pList.GetRange(i, Math.Min(pSize, pList.Count - i));
            }
        }

        /// <summary>
        /// Realiza una sincronización completa
        /// </summary>
        public void SincroComplete()
        {
            //Obtenemos los ids cargados
            List<string> idsLoaded = GetItemsLoaded();

            //Obtenemos los ids a cargar
            List<string> idsToLoad = GetItemsToLoad();

            //Eliminar
            foreach (string idEliminar in idsLoaded.Except(idsToLoad))
            {
                DeleteItem(idEliminar);
            }

            //Cargar
            List<string> cargar = idsToLoad.Except(idsLoaded).ToList();
            Dictionary<string, EnrichmentSimilarityItem> itemsCargar = GetItemsToLoad(cargar);
            foreach (string idCargar in cargar)
            {
                EnrichmentSimilarityItem enrichmentSimilarityItem = itemsCargar[idCargar];
                if (enrichmentSimilarityItem != null)
                {
                    LoadItem(enrichmentSimilarityItem);
                }
            }

            //Modificar
            List<string> modificar = idsToLoad.Intersect(idsLoaded).ToList();

            foreach (List<string> idsModificar in SplitList(modificar, 500))
            {
                Dictionary<string, EnrichmentSimilarityItem> itemsModificar = GetItemsToLoad(idsModificar);
                foreach (string idModificar in idsModificar)
                {
                    EnrichmentSimilarityItem enrichmentSimilarityItemToLoad = itemsModificar[idModificar];
                    EnrichmentSimilarityItem enrichmentSimilarityItemLoaded = GetItemLoaded(idModificar);
                    if (enrichmentSimilarityItemToLoad == null)
                    {
                        DeleteItem(idModificar);
                    }
                    else if (!enrichmentSimilarityItemToLoad.Equals(enrichmentSimilarityItemLoaded))
                    {
                        LoadItem(enrichmentSimilarityItemToLoad);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Clase para representar un objeto en el servicio de similitud
    /// </summary>
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
                float score = 1;
                if (specific[1] is int)
                {
                    score = (int)specific[1];
                }
                else if (specific[1] is float)
                {
                    score = float.Parse(specific[1].ToString().Replace(",", "."), CultureInfo.InvariantCulture);
                }
                else if (specific[1] is double)
                {
                    score = float.Parse(specific[1].ToString().Replace(",", "."), CultureInfo.InvariantCulture);
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

    /// <summary>
    /// Clase para representar la respuesta a una petición de obtención de items similares
    /// </summary>
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

    /// <summary>
    /// Conversor
    /// </summary>
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
