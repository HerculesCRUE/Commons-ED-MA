using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Hercules.CommonsEDMA.DisambiguationEngine.Models
{
    /// <summary>
    /// Clase para lanzar un sólo hilo
    /// </summary>
    public class ThreadSafeSingleShotGuard
    {
        private static int NOTCALLED = 0;
        private static int CALLED = 1;
        private int _state = NOTCALLED;
        public bool CheckAndSetFirstCall
        { get { return Interlocked.Exchange(ref _state, CALLED) == NOTCALLED; } }
    }

    /// <summary>
    /// Clase para realizar tareas de desambiguación
    /// </summary>
    public static class Disambiguation
    {
        /// <summary>
        /// API
        /// </summary>
        public static ResourceApi mResourceApi = null;
        /// <summary>
        /// Frecuencia absoluta de los nombres
        /// </summary>
        private static Dictionary<string, int> mFrecuenciaNombres = null;
        /// <summary>
        /// Score de los nombres 
        /// </summary>
        private static Dictionary<string, float> mScoreNombresCalculado = null;
        /// <summary>
        /// Score para las iniciales
        /// </summary>
        private static readonly float scoreInicial = 0.1f;
        /// <summary>
        /// Score mínimo para los nombres
        /// </summary>
        private static readonly float minimoScoreNombres = 0.4f;
        /// <summary>
        /// Score máximo para los nombres
        /// </summary>
        private static readonly float maximoScoreNombres = 0.6f;
        private static ThreadSafeSingleShotGuard _loading = new ThreadSafeSingleShotGuard();

        /// <summary>
        /// Obtiene la frecuencia de mombres
        /// </summary>
        private static Dictionary<string, int> FrecuenciaNombres
        {
            get
            {
                if (mFrecuenciaNombres != null)
                {
                    return mFrecuenciaNombres;
                }

                if (_loading.CheckAndSetFirstCall)
                {
                    new Thread(delegate ()
                    {
                        try
                        {
                            while (true)
                            {
                                Dictionary<string, int> frecuenciaNombresAux = new Dictionary<string, int>();
                                Dictionary<string, float> scoreNombresCalculadoAux = new Dictionary<string, float>();
                                int limit = 10000;
                                int offset = 0;
                                int numPersonas = 0;
                                while (true)
                                {
                                    string select = $@"SELECT * WHERE {{ SELECT DISTINCT ?persona ?nombreCompleto";
                                    string where = $@"WHERE {{
                                                            ?persona a <http://xmlns.com/foaf/0.1/Person>. 
                                                            ?persona <http://w3id.org/roh/isActive> 'true'. 
                                                            ?persona <http://xmlns.com/foaf/0.1/name> ?nombreCompleto.                                
                                                        }} ORDER BY DESC(?persona) }} LIMIT {limit} OFFSET {offset}";
                                    SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, "person");
                                    if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                                    {
                                        offset += limit;
                                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                                        {
                                            numPersonas++;
                                            string nombreNormalizado = ObtenerTextosNombresNormalizados(fila["nombreCompleto"].value);
                                            foreach (string nombre in nombreNormalizado.Split(' '))
                                            {
                                                if (!frecuenciaNombresAux.ContainsKey(nombre))
                                                {
                                                    frecuenciaNombresAux[nombre] = 0;
                                                }
                                                frecuenciaNombresAux[nombre]++;
                                                if (nombre.Length > 0)
                                                {
                                                    if (!frecuenciaNombresAux.ContainsKey(nombre[0].ToString()))
                                                    {
                                                        frecuenciaNombresAux[nombre[0].ToString()] = 0;
                                                    }
                                                    frecuenciaNombresAux[nombre[0].ToString()]++;
                                                }
                                            }
                                        }
                                        if (resultadoQuery.results.bindings.Count < limit)
                                        {
                                            break;
                                        }
                                    }
                                }
                                frecuenciaNombresAux = frecuenciaNombresAux.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

                                int max = frecuenciaNombresAux.Where(x => x.Key.Length > 1).Select(x => x.Value).Max();
                                int min = frecuenciaNombresAux.Where(x => x.Key.Length > 1).Select(x => x.Value).Min();
                                //Asignamos un valor a cada frecuencia
                                int numFrecuencias = frecuenciaNombresAux.Where(x => x.Key.Length > 1).Select(x => x.Value).Distinct().Count();
                                int i = 0;
                                Dictionary<int, float> frecuenciaValor = new Dictionary<int, float>();
                                foreach (int frecuencia in frecuenciaNombresAux.Where(x => x.Key.Length > 1).Select(x => x.Value).Reverse().Distinct())
                                {
                                    var f = (float)i / numFrecuencias;
                                    frecuenciaValor.Add(frecuencia, 1 - f);
                                    i++;
                                }
                                foreach (string nombre in frecuenciaNombresAux.Keys)
                                {
                                    if (nombre.Length == 1)
                                    {
                                        scoreNombresCalculadoAux[nombre] = scoreInicial;
                                    }
                                    else
                                    {
                                        float x = minimoScoreNombres + (maximoScoreNombres - minimoScoreNombres) * frecuenciaValor[frecuenciaNombresAux[nombre]];
                                        if (x > maximoScoreNombres)
                                        {
                                            x = maximoScoreNombres;
                                        }
                                        if (x < minimoScoreNombres)
                                        {
                                            x = minimoScoreNombres;
                                        }
                                        scoreNombresCalculadoAux[nombre] = x;
                                    }
                                }
                                scoreNombresCalculadoAux = scoreNombresCalculadoAux.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                                mFrecuenciaNombres = frecuenciaNombresAux;
                                mScoreNombresCalculado = scoreNombresCalculadoAux;


                                //Se recalcula cada hora
                                Thread.Sleep(3600000);
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }).Start();
                }
                while (mFrecuenciaNombres == null)
                {
                    Thread.Sleep(1000);
                }

                return mFrecuenciaNombres;
            }
        }

        /// <summary>
        /// Obtiene el score de los nombres calculado
        /// </summary>
        private static Dictionary<string, float> ScoreNombresCalculado
        {
            get
            {
                var x = FrecuenciaNombres;
                return mScoreNombresCalculado;
            }
        }

        /// <summary>
        /// Compara la similitud con los elementos de la BBDD
        /// </summary>
        /// <param name="pItems">Items a desambiguar</param>
        /// <param name="pItemBBDD">Items de BBDD</param>
        /// <param name="pUmbral">Umbral</param>
        /// <param name="pToleranciaNombres">Tolerancia para los nombres</param>
        /// <returns></returns>
        public static Dictionary<string, string> SimilarityBBDD(List<DisambiguableEntity> pItems, List<DisambiguableEntity> pItemBBDD, float pUmbral = 0.8f, float pToleranciaNombres = 0f)
        {
            Dictionary<string, Dictionary<string, float>> listaEquivalenciasSimilarityBBDD = new Dictionary<string, Dictionary<string, float>>();
            if (pItemBBDD != null && pItemBBDD.Count > 0)
            {
                // Diccionario Nombres Personas Desnormalizadas
                Dictionary<string, string> dicNomPersonasDesnormalizadasSimilarityBBDD = new Dictionary<string, string>();
                // Diccionario Titulos Desnormalizadas
                Dictionary<string, string> dicTitulosDesnormalizadosSimilarityBBDD = new Dictionary<string, string>();

                Dictionary<DisambiguableEntity, List<DisambiguationData>> disambiguationDataItemsACargarSimilarityBBDD = GetDisambiguationData(pItems);
                Dictionary<DisambiguableEntity, List<DisambiguationData>> disambiguationDataItemsBBDDSimilarityBBDD = GetDisambiguationData(pItemBBDD);

                // Diccionario con tipo de item y su listado correspondiente de los datos a desambiguar.
                Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> itemsPorTipoToLoadSimilarityBBDD = ObtenerItemsPorTipo(disambiguationDataItemsACargarSimilarityBBDD);
                // Diccionario con tipo de item y su listado correspondiente de los datos a desambiguar.
                Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> itemsPorTipoBBDDSimilarityBBDD = ObtenerItemsPorTipo(disambiguationDataItemsBBDDSimilarityBBDD);

                // Realizamos las comprobacions para ver si el input es correcto
                RealizarComprobaciones(itemsPorTipoToLoadSimilarityBBDD, itemsPorTipoBBDDSimilarityBBDD);

                foreach (string tipo in itemsPorTipoToLoadSimilarityBBDD.Keys)
                {
                    List<KeyValuePair<DisambiguableEntity, List<DisambiguationData>>> itemsPorTipoBBDDList = itemsPorTipoBBDDSimilarityBBDD[tipo].ToList();
                    Dictionary<DisambiguableEntity, List<DisambiguationData>> itemsBBDD = null;
                    if (itemsPorTipoBBDDSimilarityBBDD != null)
                    {
                        itemsBBDD = new Dictionary<DisambiguableEntity, List<DisambiguationData>>();
                        if (itemsPorTipoBBDDSimilarityBBDD.ContainsKey(tipo))
                        {
                            itemsBBDD = itemsPorTipoBBDDSimilarityBBDD[tipo];
                        }
                    }
                    foreach (var itemA in itemsPorTipoToLoadSimilarityBBDD[tipo])
                    {
                        listaEquivalenciasSimilarityBBDD[itemA.Key.ID] = new Dictionary<string, float>();
                        for (int i = 0; i < itemsPorTipoBBDDSimilarityBBDD[tipo].Count; i++)
                        {
                            // Algoritmo de similaridad.
                            float similarity = GetSimilarity(itemA, itemsPorTipoBBDDList[i], dicNomPersonasDesnormalizadasSimilarityBBDD, dicTitulosDesnormalizadosSimilarityBBDD, null, new Dictionary<string, HashSet<string>>(), pToleranciaNombres);
                            if (similarity >= pUmbral)
                            {
                                listaEquivalenciasSimilarityBBDD[itemA.Key.ID][itemsPorTipoBBDDList[i].Key.ID] = similarity;
                            }
                        }
                    }
                }
            }

            Dictionary<string, string> listaEquivalenciasFinal = new Dictionary<string, string>();
            HashSet<string> idBBDDSeleccionados = new HashSet<string>();
            foreach (string id in listaEquivalenciasSimilarityBBDD.Keys)
            {
                listaEquivalenciasFinal[id] = "";
                if (listaEquivalenciasSimilarityBBDD[id].Count > 1)
                {
                    listaEquivalenciasSimilarityBBDD[id] = listaEquivalenciasSimilarityBBDD[id].OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                }
                foreach (string id2 in listaEquivalenciasSimilarityBBDD[id].Keys)
                {
                    //Si existe en otro sitio con mas puntuación no se añade
                    bool existeEnOtroSitio = false;
                    foreach (string idAux in listaEquivalenciasSimilarityBBDD.Keys)
                    {
                        if (idAux != id)
                        {
                            foreach (string id2Aux in listaEquivalenciasSimilarityBBDD[idAux].Keys)
                            {
                                if (id2 == id2Aux && listaEquivalenciasSimilarityBBDD[idAux][id2Aux] > listaEquivalenciasSimilarityBBDD[id][id2])
                                {
                                    existeEnOtroSitio = true;
                                }
                            }
                        }
                    }
                    //Si esta añadido no se añade
                    if (!existeEnOtroSitio && !idBBDDSeleccionados.Contains(id2))
                    {
                        listaEquivalenciasFinal[id] = id2;
                        idBBDDSeleccionados.Add(id2);
                    }
                }
            }

            foreach (DisambiguableEntity entity in pItems)
            {
                if (!listaEquivalenciasFinal.ContainsKey(entity.ID))
                {
                    listaEquivalenciasFinal[entity.ID] = "";
                }
            }
            return listaEquivalenciasFinal;
        }

        /// <summary>
        /// Compara la similitud con los elementos de la BBDD y devuelve los scores
        /// </summary>
        /// <param name="pItems">Items a desambiguar</param>
        /// <param name="pItemBBDD">Items de BBDD</param>
        /// <param name="pUmbral">Umbral</param>
        /// <param name="pToleranciaNombres">Tolerancia para los nombres</param>
        /// <returns></returns>
        public static Dictionary<string, Dictionary<string, float>> SimilarityBBDDScores(List<DisambiguableEntity> pItems, List<DisambiguableEntity> pItemBBDD, float pUmbral = 0.8f, float pToleranciaNombres = 0f)
        {
            Dictionary<string, Dictionary<string, float>> listaEquivalencias = new Dictionary<string, Dictionary<string, float>>();
            if (pItemBBDD != null && pItemBBDD.Count > 0)
            {
                // Diccionario Nombres Personas Desnormalizadas
                Dictionary<string, string> dicNomPersonasDesnormalizadas = new Dictionary<string, string>();
                // Diccionario Titulos Desnormalizadas
                Dictionary<string, string> dicTitulosDesnormalizados = new Dictionary<string, string>();

                Dictionary<DisambiguableEntity, List<DisambiguationData>> disambiguationDataItemsACargar = GetDisambiguationData(pItems);
                Dictionary<DisambiguableEntity, List<DisambiguationData>> disambiguationDataItemsBBDD = GetDisambiguationData(pItemBBDD);

                // Diccionario con tipo de item y su listado correspondiente de los datos a desambiguar.
                Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> itemsPorTipoToLoad = ObtenerItemsPorTipo(disambiguationDataItemsACargar);
                // Diccionario con tipo de item y su listado correspondiente de los datos a desambiguar.
                Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> itemsPorTipoBBDD = ObtenerItemsPorTipo(disambiguationDataItemsBBDD);

                // Realizamos las comprobacions para ver si el input es correcto
                RealizarComprobaciones(itemsPorTipoToLoad, itemsPorTipoBBDD);

                foreach (string tipo in itemsPorTipoToLoad.Keys)
                {
                    List<KeyValuePair<DisambiguableEntity, List<DisambiguationData>>> itemsPorTipoBBDDList = itemsPorTipoBBDD[tipo].ToList();
                    Dictionary<DisambiguableEntity, List<DisambiguationData>> itemsBBDD = null;
                    if (itemsPorTipoBBDD != null)
                    {
                        itemsBBDD = new Dictionary<DisambiguableEntity, List<DisambiguationData>>();
                        if (itemsPorTipoBBDD.ContainsKey(tipo))
                        {
                            itemsBBDD = itemsPorTipoBBDD[tipo];
                        }
                    }
                    foreach (var itemA in itemsPorTipoToLoad[tipo])
                    {
                        listaEquivalencias[itemA.Key.ID] = new Dictionary<string, float>();
                        for (int i = 0; i < itemsPorTipoBBDD[tipo].Count; i++)
                        {
                            // Algoritmo de similaridad.
                            float similarity = GetSimilarity(itemA, itemsPorTipoBBDDList[i], dicNomPersonasDesnormalizadas, dicTitulosDesnormalizados, null, new Dictionary<string, HashSet<string>>(), pToleranciaNombres);
                            if (similarity >= pUmbral)
                            {
                                listaEquivalencias[itemA.Key.ID][itemsPorTipoBBDDList[i].Key.ID] = similarity;
                            }
                        }
                    }
                }
            }

            HashSet<string> idBBDDSeleccionados = new HashSet<string>();
            foreach (string id in listaEquivalencias.Keys)
            {
                if (listaEquivalencias[id].Count > 1)
                {
                    listaEquivalencias[id] = listaEquivalencias[id].OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                }
            }

            foreach (DisambiguableEntity entity in pItems)
            {
                if (!listaEquivalencias.ContainsKey(entity.ID))
                {
                    listaEquivalencias[entity.ID] = new Dictionary<string, float>();
                }
            }
            return listaEquivalencias;
        }

        /// <summary>
        /// Proceso de desambiguación.
        /// </summary>
        /// <param name="pItems">Lista de datos a desambiguar.</param>        
        /// <param name="pItemBBDD">Lista de datos de BBDD.</param>
        /// <param name="pDisambiguateItems">Indica si hay que disambiguar entr los pItems</param>
        /// <param name="pUmbral">Umbral para la desambiguación</param>
        /// <param name="pToleranciaNombres">Tolerancia para los nombres de las personas (porcentaje máximo de nombres no coincidentes admitidos)</param>
        /// <returns>Lista de datos desambiguables.</returns>
        public static Dictionary<string, HashSet<string>> Disambiguate(List<DisambiguableEntity> pItems, List<DisambiguableEntity> pItemBBDD, bool pDisambiguateItems = true, float pUmbral = 0.8f, float pToleranciaNombres = 0f)
        {
            //En esta variable se almacenarán todas las equivalencias encontradas con su peso
            Dictionary<string, Dictionary<string, float>> listaEquivalencias = new Dictionary<string, Dictionary<string, float>>();
            #region Diccionarios auxiliares para la desmbiguación
            Dictionary<string, string> dicNomPersonasDesnormalizadas = new Dictionary<string, string>();
            Dictionary<string, string> dicTitulosDesnormalizados = new Dictionary<string, string>();
            #endregion

            //En esta variable se almacenarán todas las entidades que nunca podrán ser equivalentes
            //Ver que pasa cuando hay multiples
            Dictionary<string, HashSet<string>> listaDistintos = new Dictionary<string, HashSet<string>>();
            foreach (DisambiguableEntity item in pItems)
            {
                if (item.distincts != null)
                {
                    foreach (string distinct in item.distincts)
                    {
                        if (!listaDistintos.ContainsKey(item.ID))
                        {
                            listaDistintos[item.ID] = new HashSet<string>();
                        }
                        if (!listaDistintos.ContainsKey(distinct))
                        {
                            listaDistintos[distinct] = new HashSet<string>();
                        }
                        listaDistintos[item.ID].Add(distinct);
                        listaDistintos[distinct].Add(item.ID);
                    }
                }
            }


            //Obtenemos las propiedades que nos permitirán la desambiguación.
            Dictionary<DisambiguableEntity, List<DisambiguationData>> disambiguationDataItemsACargar = GetDisambiguationData(pItems);
            Dictionary<DisambiguableEntity, List<DisambiguationData>> disambiguationDataItemsBBDD = GetDisambiguationData(pItemBBDD);

            //Creamos una variable en la que estarán los datos de desambiguación de las entidades a cargar
            //se irá modificando según se vaya realizando la desambiguación
            Dictionary<DisambiguableEntity, List<DisambiguationData>> disambiguationDataItemsACargarAux = new Dictionary<DisambiguableEntity, List<DisambiguationData>>(disambiguationDataItemsACargar);

            bool cambios = true;
            while (cambios)
            {
                cambios = false;
                if (pDisambiguateItems)
                {
                    // 1º Aplicamos la desambiguación con los items a cargar
                    bool cambiosItems = true;
                    while (cambiosItems)
                    {
                        Dictionary<string, Dictionary<string, float>> listaEquivalenciasItemsACargar = ApplyDisambiguation(disambiguationDataItemsACargarAux, null, listaDistintos, dicNomPersonasDesnormalizadas, dicTitulosDesnormalizados, pToleranciaNombres);
                        cambiosItems = ProcesarEquivalencias(pItems, null, listaEquivalenciasItemsACargar, disambiguationDataItemsACargarAux, disambiguationDataItemsBBDD, listaEquivalencias, listaDistintos, pUmbral);
                        if (cambiosItems)
                        {
                            cambios = true;
                        }
                    }


                }
                if (pItemBBDD != null && pItemBBDD.Count > 0)
                {
                    //2º Aplicamos la desambiguación con los items de BBDD
                    bool cambiosBBDD = true;
                    while (cambiosBBDD)
                    {
                        Dictionary<string, Dictionary<string, float>> listaEquivalenciasItemsACargar = ApplyDisambiguation(disambiguationDataItemsACargarAux, disambiguationDataItemsBBDD, listaDistintos, dicNomPersonasDesnormalizadas, dicTitulosDesnormalizados, pToleranciaNombres);
                        cambiosBBDD = ProcesarEquivalencias(pItems, pItemBBDD, listaEquivalenciasItemsACargar, disambiguationDataItemsACargarAux, disambiguationDataItemsBBDD, listaEquivalencias, listaDistintos, pUmbral);
                        if (cambiosBBDD)
                        {
                            cambios = true;
                        }
                    }
                }
            }

            //Preparamos el objeto a devolver
            Dictionary<string, HashSet<string>> listadoEquivalencias = new Dictionary<string, HashSet<string>>();
            //Comprobamos errores
            foreach (string id in listaEquivalencias.Keys)
            {
                List<string> itemsBBDD = listaEquivalencias[id].Keys.Where(x => !Guid.TryParse(x.Split('|')[1], out Guid auxB)).ToList();
                if (itemsBBDD.Count > 1)
                {
                    throw new Exception("Error, no puede un item apuntar a más de un ítem de BBDD");
                }
            }

            //Lista para comprobar que no se carguen duplicados
            HashSet<string> cargados = new HashSet<string>();

            //Añadimos los que están vinculados con BBDD
            foreach (string id in listaEquivalencias.Keys)
            {
                bool idBBDD = !Guid.TryParse(id.Split('|')[1], out Guid auxA);
                foreach (string id2 in listaEquivalencias[id].Keys)
                {
                    bool id2BBDD = !Guid.TryParse(id2.Split('|')[1], out Guid auxB);
                    if (idBBDD && id2BBDD)
                    {
                        throw new Exception("Error, no puede un item apuntar a más de un ítem de BBDD");
                    }
                    float similitud = listaEquivalencias[id][id2];
                    if (similitud >= pUmbral)
                    {
                        if (idBBDD && !id2BBDD)
                        {
                            if (!listadoEquivalencias.ContainsKey(id.Split('|')[1]))
                            {
                                listadoEquivalencias[id.Split('|')[1]] = new HashSet<string>();
                            }
                            if (listadoEquivalencias[id.Split('|')[1]].Add(id2))
                            {
                                if (!cargados.Add(id2))
                                {
                                    throw new Exception("No puede haber 2 cargados iguales");
                                }
                            }
                        }
                        if (!idBBDD && id2BBDD)
                        {
                            if (!listadoEquivalencias.ContainsKey(id2.Split('|')[1]))
                            {
                                listadoEquivalencias[id2.Split('|')[1]] = new HashSet<string>();
                            }
                            if (listadoEquivalencias[id2.Split('|')[1]].Add(id))
                            {
                                if (!cargados.Add(id))
                                {
                                    throw new Exception("No puede haber 2 cargados iguales");
                                }
                            }
                        }
                    }
                }
            }

            //Recorremos los que no están vinculados con BBDD y buscamos si tienen alguna vinculación con las que están vinculadas con las de BBDD, si tienen más de uno nos quedamos con el mayor
            Dictionary<string, string> aniadidos = new Dictionary<string, string>();
            foreach (string id in listadoEquivalencias.Keys)
            {
                foreach (string id2 in listadoEquivalencias[id])
                {
                    aniadidos[id2] = id;
                }
            }
            foreach (string id in listaEquivalencias.Keys)
            {
                bool idBBDD = !Guid.TryParse(id.Split('|')[1], out Guid auxA);
                //Si no es de BBDD y no está cargado buscamos el más similar de los de BBDD
                if (!idBBDD && !cargados.Contains(id))
                {
                    string idEquivalenteBBDD = "";
                    float maxEquivalencia = 0;
                    int num = 0;
                    foreach (string id2 in listaEquivalencias[id].Keys)
                    {
                        bool id2BBDD = !Guid.TryParse(id2.Split('|')[1], out Guid auxB);
                        if (!id2BBDD && cargados.Contains(id2))
                        {
                            float similitud = listaEquivalencias[id][id2];
                            if (similitud >= pUmbral)
                            {
                                num++;
                                if (similitud > maxEquivalencia)
                                {
                                    idEquivalenteBBDD = aniadidos[id2];
                                    maxEquivalencia = similitud;
                                }
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(idEquivalenteBBDD))
                    {
                        //Añadimos si no se incumple 'listaDistintos'
                        bool incumple = false;
                        if (listaDistintos.ContainsKey(id.Split('|')[1]))
                        {
                            foreach (string cargadoActual in listadoEquivalencias[idEquivalenteBBDD])
                            {
                                if (listaDistintos[id.Split('|')[1]].Contains(cargadoActual.Split('|')[1]))
                                {
                                    incumple = true;
                                }
                            }
                        }
                        if (!incumple)
                        {
                            listadoEquivalencias[idEquivalenteBBDD].Add(id);
                            aniadidos[id] = idEquivalenteBBDD;
                            cargados.Add(id);
                        }
                    }
                }
            }

            //Añadimos los que no son de BBDD
            aniadidos = new Dictionary<string, string>();
            foreach (string id in listadoEquivalencias.Keys)
            {
                foreach (string id2 in listadoEquivalencias[id])
                {
                    aniadidos[id2] = id;
                }
            }
            foreach (string id in listaEquivalencias.Keys)
            {
                bool idBBDD = !Guid.TryParse(id.Split('|')[1], out Guid auxA);
                foreach (string id2 in listaEquivalencias[id].Keys)
                {
                    bool id2BBDD = !Guid.TryParse(id2.Split('|')[1], out Guid auxB);
                    if (idBBDD && id2BBDD)
                    {
                        throw new Exception("Error, no puede un item apuntar a más de un ítem de BBDD");
                    }
                    float similitud = listaEquivalencias[id][id2];
                    if (similitud >= pUmbral)
                    {
                        if (!idBBDD && !id2BBDD)
                        {
                            //Buscamos si está ya añadido                            
                            string aniadir = "";
                            if (aniadidos.ContainsKey(id))
                            {
                                aniadir = aniadidos[id];
                            }
                            if (aniadidos.ContainsKey(id2))
                            {
                                if (aniadir == "" || aniadir == aniadidos[id2])
                                {
                                    aniadir = aniadidos[id2];
                                }
                                else
                                {
                                    continue;
                                }
                            }


                            //Si los dos están añadidos no hacemos nada
                            if (aniadidos.ContainsKey(id) && aniadidos.ContainsKey(id2))
                            {
                                continue;
                            }
                            else
                            {
                                //Si no los añadimos
                                if (string.IsNullOrEmpty(aniadir))
                                {
                                    aniadir = Guid.NewGuid().ToString();
                                    listadoEquivalencias[aniadir] = new HashSet<string>();
                                }
                                //Añadimos si no se incumple 'listaDistintos'
                                bool incumple1 = false;
                                if (listaDistintos.ContainsKey(id.Split('|')[1]))
                                {
                                    foreach (string cargadoActual in listadoEquivalencias[aniadir])
                                    {
                                        if (listaDistintos[id.Split('|')[1]].Contains(cargadoActual.Split('|')[1]))
                                        {
                                            incumple1 = true;
                                        }
                                    }
                                }

                                bool incumple2 = false;
                                if (listaDistintos.ContainsKey(id2.Split('|')[1]))
                                {
                                    foreach (string cargadoActual in listadoEquivalencias[aniadir])
                                    {
                                        if (listaDistintos[id2.Split('|')[1]].Contains(cargadoActual.Split('|')[1]))
                                        {
                                            incumple2 = true;
                                        }
                                    }
                                }
                                if (!incumple1)
                                {
                                    listadoEquivalencias[aniadir].Add(id);
                                    aniadidos[id] = aniadir;
                                    cargados.Add(id);
                                }
                                if (!incumple2)
                                {
                                    listadoEquivalencias[aniadir].Add(id2);
                                    aniadidos[id2] = aniadir;
                                    cargados.Add(id2);
                                }
                            }
                        }
                    }
                }
            }

            //Añadimos las que falten            
            foreach (DisambiguableEntity item in pItems)
            {
                if (cargados.Add(item.GetType().Name + "|" + item.ID))
                {
                    listadoEquivalencias.Add(Guid.NewGuid().ToString(), new HashSet<string>() { item.GetType().Name + "|" + item.ID });
                }
            }
            {
                //Verificar que estan todos y no estan repetidos
                HashSet<string> idsResultantes = new HashSet<string>();
                foreach (string id in listadoEquivalencias.Keys)
                {
                    foreach (string iditem in listadoEquivalencias[id])
                    {
                        if (!idsResultantes.Add(iditem))
                        {
                            throw new Exception("Item repetido");
                        }
                    }
                }

                if (idsResultantes.Count != pItems.Count)
                {
                    throw new Exception("Error");
                }
            }

            return listadoEquivalencias;
        }

        /// <summary>
        /// Procesa las equivalencias
        /// </summary>
        /// <param name="pItems">Items originales</param>
        /// <param name="pItemsBBDD">Items de BBDD</param>
        /// <param name="pListaEquivalenciasItemsACargar">Resultado de la desmbiguación</param>
        /// <param name="pDisambiguationDataItemsACargar">Items con los datos de desambiguación</param>
        /// <param name="pDisambiguationDataItemsACargarBBDD">Items con los datos de desambiguación de BBDD</param>
        /// <param name="pListaEquivalencias">Lista de equivalencias</param>
        /// <param name="pUmbral">Umbral</param>
        private static bool ProcesarEquivalencias(List<DisambiguableEntity> pItems, List<DisambiguableEntity> pItemsBBDD, Dictionary<string, Dictionary<string, float>> pListaEquivalenciasItemsACargar,
            Dictionary<DisambiguableEntity, List<DisambiguationData>> pDisambiguationDataItemsACargar, Dictionary<DisambiguableEntity, List<DisambiguationData>> pDisambiguationDataItemsACargarBBDD, Dictionary<string, Dictionary<string, float>> pListaEquivalencias, Dictionary<string, HashSet<string>> pListaDistintos, float pUmbral)
        {
            Dictionary<string, DisambiguableEntity> dicItems = new Dictionary<string, DisambiguableEntity>();
            foreach (DisambiguableEntity item in pItems)
            {
                dicItems.Add(item.GetType().Name + "|" + item.ID, item);
            }
            if (pItemsBBDD != null)
            {
                foreach (DisambiguableEntity item in pItemsBBDD)
                {
                    dicItems.Add(item.GetType().Name + "|" + item.ID, item);
                }
            }

            bool cambios = false;
            Dictionary<string, string> cambiosReferencias = new Dictionary<string, string>();
            //Obtenemos las equivalencias que superan el umbral
            foreach (string idA in pListaEquivalenciasItemsACargar.Keys)
            {
                string idAtype = idA.Split('|')[0];
                string idAidentifier = idA.Split('|')[1];
                foreach (string idB in pListaEquivalenciasItemsACargar[idA].Keys)
                {
                    string idBtype = idB.Split('|')[0];
                    string idBidentifier = idB.Split('|')[1];

                    if (pListaEquivalenciasItemsACargar[idA][idB] >= pUmbral)
                    {
                        if (pListaDistintos.ContainsKey(idA) && pListaDistintos[idA].Contains(idB))
                        {
                            continue;
                        }
                        cambios = true;
                        //Obtenemos ItemA
                        DisambiguableEntity itemA = dicItems[idAtype + "|" + idAidentifier];
                        //Obtenemos ItemB
                        DisambiguableEntity itemB = dicItems[idBtype + "|" + idBidentifier];

                        FusionarEntidades(idA, idB, itemA, itemB, pDisambiguationDataItemsACargar, pDisambiguationDataItemsACargarBBDD, pListaEquivalenciasItemsACargar, pListaEquivalencias, cambiosReferencias);
                    }
                }
            }
            //Cambiamos referencias
            foreach (DisambiguableEntity item in pDisambiguationDataItemsACargar.Keys)
            {
                foreach (DisambiguationData data in pDisambiguationDataItemsACargar[item])
                {
                    foreach (string antiguo in cambiosReferencias.Keys)
                    {
                        if (data.value == antiguo)
                        {
                            data.value = cambiosReferencias[antiguo];
                        }
                        if (data.values != null && data.values.Contains(antiguo))
                        {
                            data.values.Remove(antiguo);
                            data.values.Add(cambiosReferencias[antiguo]);
                        }
                    }
                }
            }
            return cambios;
        }


        private static void FusionarEntidades(string pIDA, string pIDb, DisambiguableEntity pItemA, DisambiguableEntity pItemB, Dictionary<DisambiguableEntity, List<DisambiguationData>> pDisambiguationDataItemsACargar, Dictionary<DisambiguableEntity, List<DisambiguationData>> pDisambiguationDataItemsACargarBBDD, Dictionary<string, Dictionary<string, float>> pListaEquivalenciasItemsACargar, Dictionary<string, Dictionary<string, float>> pListaEquivalencias, Dictionary<string, string> pCambiosReferencias)
        {
            bool itemABBDD = !Guid.TryParse(pItemA.ID, out Guid aux);
            bool itemBBBDD = !Guid.TryParse(pItemB.ID, out Guid aux2);
            if (itemABBDD && itemBBBDD)
            {
                //Si los dos son items de BBDD no hay que hacer nada (no se van a fusionar)
                return;
            }

            //Nos quedamos con el 'mejor'
            DisambiguableEntity itemEliminar = null;    //Item a sustituir por el itemBueno
            DisambiguableEntity itemBueno = null;       //Item para reemplazar a itemEliminar
            if (itemABBDD || itemBBBDD)
            {
                //Si uno de los dos es de BBDD nos quedamos con él
                if (itemABBDD && pDisambiguationDataItemsACargar.ContainsKey(pItemB))
                {
                    itemEliminar = pItemB;
                    itemBueno = pItemA;
                }
                else if (itemBBBDD && pDisambiguationDataItemsACargar.ContainsKey(pItemA))
                {
                    itemEliminar = pItemA;
                    itemBueno = pItemB;
                }
                else
                {
                    return;
                }
            }
            else if (pDisambiguationDataItemsACargar.ContainsKey(pItemA) && pDisambiguationDataItemsACargar.ContainsKey(pItemB))
            {
                //Si ningunp de los dos es de BBDD
                List<DisambiguationData> dataA = pDisambiguationDataItemsACargar[pItemA];
                List<DisambiguationData> dataB = pDisambiguationDataItemsACargar[pItemB];

                int identifiersA = dataA.Where(x => x.config.type == DisambiguationDataConfigType.equalsIdentifiers).Count();
                int identifiersB = dataB.Where(x => x.config.type == DisambiguationDataConfigType.equalsIdentifiers).Count();

                int? nombreA = dataA.Where(x => x.config.type == DisambiguationDataConfigType.algoritmoNombres).FirstOrDefault()?.value.Length;
                int? nombreB = dataB.Where(x => x.config.type == DisambiguationDataConfigType.algoritmoNombres).FirstOrDefault()?.value.Length;

                int itemsA = dataA.Where(x => x.config.type == DisambiguationDataConfigType.equalsItem).Count();
                int itemsB = dataB.Where(x => x.config.type == DisambiguationDataConfigType.equalsItem).Count();

                if (identifiersA > identifiersB)
                {
                    //Nos quedamos con el que tenga más identificadores
                    itemEliminar = pItemB;
                    itemBueno = pItemA;
                }
                else if (identifiersB > identifiersA)
                {
                    itemEliminar = pItemA;
                    itemBueno = pItemB;
                }
                else if (nombreA > nombreB)
                {
                    itemEliminar = pItemB;
                    itemBueno = pItemA;
                }
                else if (nombreB > nombreA)
                {
                    itemEliminar = pItemA;
                    itemBueno = pItemB;
                }
                else if (itemsA > itemsB)
                {
                    itemEliminar = pItemB;
                    itemBueno = pItemA;
                }
                else if (itemsB > itemsA)
                {
                    itemEliminar = pItemA;
                    itemBueno = pItemB;
                }
                else
                {
                    itemEliminar = pItemB;
                    itemBueno = pItemA;
                }
            }
            else if (pDisambiguationDataItemsACargar.ContainsKey(pItemA))
            {
                itemEliminar = pItemB;
                itemBueno = pItemA;
            }
            else if (pDisambiguationDataItemsACargar.ContainsKey(pItemB))
            {
                itemEliminar = pItemA;
                itemBueno = pItemB;
            }
            else
            {
                return;
            }

            if (!pListaEquivalencias.ContainsKey(pIDA))
            {
                pListaEquivalencias.Add(pIDA, new Dictionary<string, float>());
            }
            if (!pListaEquivalencias.ContainsKey(pIDb))
            {
                pListaEquivalencias.Add(pIDb, new Dictionary<string, float>());
            }
            pListaEquivalencias[pIDA][pIDb] = pListaEquivalenciasItemsACargar[pIDA][pIDb];
            pListaEquivalencias[pIDb][pIDA] = pListaEquivalenciasItemsACargar[pIDb][pIDA];

            //Agregamos el listado del eliminado en el bueno
            List<DisambiguationData> datosBueno = null;
            if (pDisambiguationDataItemsACargar.ContainsKey(itemBueno))
            {
                datosBueno = pDisambiguationDataItemsACargar[itemBueno];
            }
            else if (pDisambiguationDataItemsACargarBBDD.ContainsKey(itemBueno))
            {
                datosBueno = pDisambiguationDataItemsACargarBBDD[itemBueno];
            }
            foreach (DisambiguationData dataBueno in datosBueno)
            {
                if (pDisambiguationDataItemsACargar.ContainsKey(itemEliminar))
                {
                    foreach (DisambiguationData dataMalo in pDisambiguationDataItemsACargar[itemEliminar])
                    {
                        if (dataBueno.property == dataMalo.property && dataBueno.config.type == DisambiguationDataConfigType.equalsItemList)
                        {
                            dataBueno.values.UnionWith(dataMalo.values);
                        }
                        if (dataBueno.property == dataMalo.property && dataBueno.config.type == DisambiguationDataConfigType.equalsIdentifiers && !string.IsNullOrEmpty(dataMalo.value))
                        {
                            dataBueno.value = dataMalo.value;
                        }
                        if (dataBueno.property == dataMalo.property && dataBueno.config.type == DisambiguationDataConfigType.equalsItem && !string.IsNullOrEmpty(dataMalo.value))
                        {
                            dataBueno.value = dataMalo.value;
                        }
                    }
                }
            }
            //Cambiamos referencias
            pCambiosReferencias[itemEliminar.ID] = itemBueno.ID;

            //Eliminamos de la lista de items el duplicado
            pDisambiguationDataItemsACargar.Remove(itemEliminar);
        }

        /// <summary>
        /// Obtiene y clasifica los datos a desambiguar.
        /// </summary>
        /// <param name="pItems"></param>
        /// <returns>Diccionario con el tipo de dato y la lista de datos a desambiguar.</returns>
        private static Dictionary<DisambiguableEntity, List<DisambiguationData>> GetDisambiguationData(List<DisambiguableEntity> pItems)
        {
            Dictionary<DisambiguableEntity, List<DisambiguationData>> disambiguationData = new Dictionary<DisambiguableEntity, List<DisambiguationData>>();
            if (pItems != null)
            {
                foreach (DisambiguableEntity item in pItems)
                {
                    disambiguationData[item] = item.GetDisambiguationData();
                }
            }
            return disambiguationData;
        }

        /// <summary>
        /// Aplica la desambiguación de los datos.
        /// </summary>
        /// <param name="pItemsToLoad">Diccionario con el tipo de dato y la lista de los datos a desambiguar.</param>
        /// <param name="pItemsBBDD">Diccionario con el tipo de dato y la lista de los datos de BBDD.</param>
        /// <param name="pListaDistintos">Diccionario con los items que no pueden ser iguales</param>
        /// <param name="pToleranciaNombres">Tolerancia para los nombres de las personas (porcentaje máximo de nombres no coincidentes admitidos)</param>
        /// <returns>Listado con las equivalencias y su peso.</returns>
        private static Dictionary<string, Dictionary<string, float>> ApplyDisambiguation(Dictionary<DisambiguableEntity, List<DisambiguationData>> pItemsToLoad, Dictionary<DisambiguableEntity, List<DisambiguationData>> pItemsBBDD, Dictionary<string, HashSet<string>> pListaDistintos, Dictionary<string, string> pDicNomPersonasDesnormalizadas, Dictionary<string, string> pDicTitulosDesnormalizados, float pToleranciaNombres)
        {
            // Respuesta: Diccionario con los IDs equivalentes.
            Dictionary<string, Dictionary<string, float>> equivalences = new Dictionary<string, Dictionary<string, float>>();

            // Diccionario con tipo de item y su listado correspondiente de los datos a desambiguar.
            Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> itemsPorTipoToLoad = ObtenerItemsPorTipo(pItemsToLoad);
            // Diccionario con tipo de item y su listado correspondiente de los datos a desambiguar.
            Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> itemsPorTipoBBDD = ObtenerItemsPorTipo(pItemsBBDD);

            // Realizamos las comprobacions para ver si el input es correcto            
            RealizarComprobaciones(itemsPorTipoToLoad, itemsPorTipoBBDD);

            for (int i = 0; i < 2; i++)
            {
                foreach (string tipo in itemsPorTipoToLoad.Keys)
                {
                    Dictionary<DisambiguableEntity, List<DisambiguationData>> itemsBBDD = null;
                    if (itemsPorTipoBBDD != null)
                    {
                        itemsBBDD = new Dictionary<DisambiguableEntity, List<DisambiguationData>>();
                        if (itemsPorTipoBBDD.ContainsKey(tipo))
                        {
                            itemsBBDD = itemsPorTipoBBDD[tipo];
                        }
                    }
                    Dictionary<string, Dictionary<string, float>> equivalencesAyuda = null;
                    if (i == 1)
                    {
                        equivalencesAyuda = new Dictionary<string, Dictionary<string, float>>();
                        foreach (string key in equivalences.Keys)
                        {
                            equivalencesAyuda.Add(key.Split('|')[1], new Dictionary<string, float>());
                            foreach (string key2 in equivalences[key].Keys)
                            {
                                equivalencesAyuda[key.Split('|')[1]].Add(key2.Split('|')[1], equivalences[key][key2]);
                            }
                        }
                    }
                    Dictionary<string, Dictionary<string, float>> equivalencesAux = BuscarEquivalencias(itemsPorTipoToLoad[tipo], itemsBBDD, tipo, equivalencesAyuda, pListaDistintos, pDicNomPersonasDesnormalizadas, pDicTitulosDesnormalizados, pToleranciaNombres);

                    foreach (string key in equivalencesAux.Keys)
                    {
                        if (!equivalences.ContainsKey(key))
                        {
                            equivalences[key] = new Dictionary<string, float>();
                        }
                        foreach (string key2 in equivalencesAux[key].Keys)
                        {
                            if (equivalences[key].ContainsKey(key2))
                            {
                                if (equivalences[key][key2] < equivalencesAux[key][key2])
                                {
                                    equivalences[key][key2] = equivalencesAux[key][key2];
                                }
                            }
                            else
                            {
                                equivalences[key][key2] = equivalencesAux[key][key2];
                            }

                        }

                    }
                }
            }
            return equivalences;
        }

        /// <summary>
        /// Búsqueda de equivalencias
        /// </summary>
        /// <param name="pItemsToLoad">Items para desambiguar</param>
        /// <param name="pItemsBBDD">Items para desambiguar de la BBDD</param>
        /// <param name="pTipo">Tipo</param>
        /// <param name="pListaDistintos">Diccionario con la lista de los IDs que no puden ser iguales</param>
        /// <param name="pToleranciaNombres">Tolerancia para los nombres de las personas (porcentaje máximo de nombres no coincidentes admitidos)</param>
        /// <returns></returns>
        private static Dictionary<string, Dictionary<string, float>> BuscarEquivalencias(Dictionary<DisambiguableEntity, List<DisambiguationData>> pItemsToLoad, Dictionary<DisambiguableEntity, List<DisambiguationData>> pItemsBBDD, string pTipo, Dictionary<string, Dictionary<string, float>> pEquivalencesAux, Dictionary<string, HashSet<string>> pListaDistintos, Dictionary<string, string> pDicNomPersonasDesnormalizadas, Dictionary<string, string> pDicTitulosDesnormalizados, float pToleranciaNombres)
        {
            //Respuesta
            Dictionary<string, Dictionary<string, float>> equivalences = new Dictionary<string, Dictionary<string, float>>();

            // Lista con los items del tipo actual
            List<KeyValuePair<DisambiguableEntity, List<DisambiguationData>>> listaItemsCompare = pItemsToLoad.ToList();
            bool compareBBDD = false;
            if (pItemsBBDD != null)
            {
                compareBBDD = true;
                listaItemsCompare = pItemsBBDD.ToList();
            }

            if (!compareBBDD)
            {
                int indice = 0;
                foreach (var itemA in pItemsToLoad)
                {
                    indice++;
                    string idA = itemA.Key.ID;
                    for (int i = indice; i < listaItemsCompare.Count; i++)
                    {
                        string idB = listaItemsCompare[i].Key.ID;
                        if (pListaDistintos.ContainsKey(idA) && pListaDistintos[idA].Contains(idB))
                        {
                            continue;
                        }
                        // Algoritmo de similaridad.
                        float similarity = GetSimilarity(itemA, listaItemsCompare[i], pDicNomPersonasDesnormalizadas, pDicTitulosDesnormalizados, pEquivalencesAux, pListaDistintos, pToleranciaNombres);

                        // Agregación de los IDs similares.
                        AddSimilarityId(pTipo, equivalences, similarity, idA, idB);
                    }
                }
            }

            if (compareBBDD)
            {
                foreach (var itemA in pItemsToLoad)
                {
                    //Si es un item de BBDD no hay que comparar
                    if (!Guid.TryParse(itemA.Key.ID, out Guid aux))
                    {
                        continue;
                    }
                    //Nos quedamos sólo con el que más se parezca
                    float similarityMax = 0;
                    string idA = itemA.Key.ID;
                    string idB = null;
                    for (int i = 0; i < listaItemsCompare.Count; i++)
                    {
                        if (pListaDistintos.ContainsKey(idA) && pListaDistintos[idA].Contains(listaItemsCompare[i].Key.ID))
                        {
                            continue;
                        }

                        // Algoritmo de similaridad.
                        float similarity = GetSimilarity(itemA, listaItemsCompare[i], pDicNomPersonasDesnormalizadas, pDicTitulosDesnormalizados, pEquivalencesAux, pListaDistintos, pToleranciaNombres);
                        if (similarity > similarityMax)
                        {
                            similarityMax = similarity;
                            idB = listaItemsCompare[i].Key.ID;
                        }
                    }
                    if (similarityMax > 0)
                    {
                        // Agregación de los IDs similares.
                        AddSimilarityId(pTipo, equivalences, similarityMax, idA, idB);
                    }
                }
            }

            return equivalences;
        }


        /// <summary>
        /// Obtiene in diccionario con los items separados por tipo
        /// </summary>
        /// <param name="pItems">Items</param>
        /// <returns></returns>
        private static Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> ObtenerItemsPorTipo(Dictionary<DisambiguableEntity, List<DisambiguationData>> pItems)
        {
            if (pItems != null)
            {
                Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> itemsPorTipo = new Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>>();
                foreach (var item in pItems)
                {
                    string name = item.Key.GetType().Name;
                    if (!itemsPorTipo.ContainsKey(name))
                    {
                        itemsPorTipo.Add(name, new Dictionary<DisambiguableEntity, List<DisambiguationData>>());
                    }
                    itemsPorTipo[name].Add(item.Key, item.Value);
                }
                return itemsPorTipo;
            }
            return null;
        }

        /// <summary>
        /// Realiza comprobaciones con los datos a desambiguar para verificar que la entrada es correcta
        /// </summary>
        /// <param name="pItemsDataToLoad">Datos de desambiguación</param>
        /// <param name="pItemsDataBBDD">Datos de desambiguación de BBDD</param>
        private static void RealizarComprobaciones(Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> pItemsDataToLoad, Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> pItemsDataBBDD)
        {
            List<Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>>> pItemsDataComprobar = new List<Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>>>();
            pItemsDataComprobar.Add(pItemsDataToLoad);
            pItemsDataComprobar.Add(pItemsDataBBDD);

            bool block = false;
            foreach (Dictionary<string, Dictionary<DisambiguableEntity, List<DisambiguationData>>> itemsData in pItemsDataComprobar)
            {
                if (itemsData != null && itemsData.Count > 0)
                {
                    //Valores de propiedades
                    HashSet<string> ids = new HashSet<string>();
                    foreach (string tipo in itemsData.Keys)
                    {
                        if (itemsData[tipo].Count > 0)
                        {
                            foreach (DisambiguableEntity entity in itemsData[tipo].Keys)
                            {
                                entity.block = block;
                                if (block)
                                {
                                    //El ID no debe contener '|' y debe empezar por http
                                    if (entity.ID.Contains("|") || !entity.ID.StartsWith("http"))
                                    {
                                        throw new Exception("Los IDs de los items de BBDD no deben contener '|' y deben empezar por http");
                                    }

                                }
                                else
                                {
                                    //El ID debe ser un guid
                                    if (!Guid.TryParse(entity.ID, out Guid aux))
                                    {
                                        throw new Exception("Los IDs de los items de BBDD no deben contener '|' y deben empezar por http");
                                    }
                                }
                            }
                            List<DisambiguationData> data = itemsData[tipo].First().Value;
                            if (data.Select(x => x.property).Distinct().Count() != data.Count)
                            {
                                throw new Exception("En los items " + tipo + " hay propiedades repetidas");
                            }
                            if (data.Select(x => x.config.type).Contains(DisambiguationDataConfigType.algoritmoNombres) && data.Select(x => x.config.type).Contains(DisambiguationDataConfigType.equalsTitle))
                            {
                                throw new Exception("En los items " + tipo + " hay configurado algoritmoNombres y equalsTitle, no puedes estar los dos de forma simultánea");
                            }
                            foreach (KeyValuePair<DisambiguableEntity, List<DisambiguationData>> item in itemsData[tipo])
                            {
                                if (!ids.Add(item.Key.ID))
                                {
                                    throw new Exception("El id " + item.Key.ID + "está repetido");
                                }
                                if (string.IsNullOrEmpty(item.Key.ID))
                                {
                                    throw new Exception("Todas las entiades deben tener ID");
                                }
                                if (data.Count != item.Value.Count)
                                {
                                    throw new Exception("Todas las entiades del mismo tipo deben tener las mismas propiedades");
                                }
                                for (int i = 0; i < data.Count; i++)
                                {
                                    if (data[i].property != item.Value[i].property)
                                    {
                                        throw new Exception("En los items " + tipo + " hay propiedades diferentes");
                                    }
                                    if (data[i].config.score != item.Value[i].config.score)
                                    {
                                        throw new Exception("Todas las entiades del mismo tipo deben tener el mismo score");
                                    }
                                    if (data[i].config.scoreMinus != item.Value[i].config.scoreMinus)
                                    {
                                        throw new Exception("Todas las entiades del mismo tipo deben tener el mismo scoreMinus");
                                    }
                                    if (data[i].config.type != item.Value[i].config.type)
                                    {
                                        throw new Exception("Todas las entiades del mismo tipo deben tener el mismo type");
                                    }
                                    switch (item.Value[i].config.type)
                                    {
                                        case DisambiguationDataConfigType.algoritmoNombres:
                                            if (item.Value[i].value == null)
                                            {
                                                item.Value[i].value = "";
                                            }
                                            if (item.Value[i].config.score <= 0 && item.Value[i].config.score > 1)
                                            {
                                                throw new Exception("La propiedad score en 'algoritmoNombres' debe ser > 0 y <=1");
                                            }
                                            if (item.Value[i].config.scoreMinus != 0)
                                            {
                                                throw new Exception("La propiedad scoreMinus en 'algoritmoNombres' no hay que configurarla");
                                            }
                                            break;
                                        case DisambiguationDataConfigType.equalsIdentifiers:
                                            if (item.Value[i].value == null)
                                            {
                                                item.Value[i].value = "";
                                            }
                                            if (item.Value[i].config.score != 0)
                                            {
                                                throw new Exception("La propiedad score en 'equalsIdentifiers' no hay que configurarla");
                                            }
                                            if (item.Value[i].config.scoreMinus != 0)
                                            {
                                                throw new Exception("La propiedad scoreMinus en 'equalsIdentifiers' no hay que configurarla");
                                            }
                                            break;
                                        case DisambiguationDataConfigType.equalsItem:
                                            if (item.Value[i].value == null)
                                            {
                                                item.Value[i].value = "";
                                            }
                                            if (item.Value[i].config.score < 0 && item.Value[i].config.score > 1)
                                            {
                                                throw new Exception("La propiedad score en 'equalsItem' debe ser >= 0 y <=1");
                                            }
                                            if (item.Value[i].config.scoreMinus < 0 && item.Value[i].config.scoreMinus > 1)
                                            {
                                                throw new Exception("La propiedad scoreMinus en 'equalsItem' debe ser >= 0 y <=1");
                                            }
                                            break;
                                        case DisambiguationDataConfigType.equalsTitle:
                                            if (item.Value[i].value == null)
                                            {
                                                item.Value[i].value = "";
                                            }
                                            if (item.Value[i].config.score <= 0 && item.Value[i].config.score > 1)
                                            {
                                                throw new Exception("La propiedad score en 'equalsTitle' debe ser > 0 y <=1");
                                            }
                                            if (item.Value[i].config.scoreMinus != 0)
                                            {
                                                throw new Exception("La propiedad scoreMinus en 'equalsTitle' no hay que configurarla");
                                            }
                                            break;
                                        case DisambiguationDataConfigType.equalsItemList:
                                            if (item.Value[i].values == null)
                                            {
                                                item.Value[i].values = new HashSet<string>();
                                            }
                                            if (item.Value[i].config.score < 0 && item.Value[i].config.score > 1)
                                            {
                                                throw new Exception("La propiedad score en 'equalsItemList' debe ser >= 0 y <=1");
                                            }
                                            if (item.Value[i].config.scoreMinus != 0)
                                            {
                                                throw new Exception("La propiedad scoreMinus en 'equalsIdentifiers' no hay que configurarla");
                                            }
                                            break;
                                        default:
                                            throw new Exception("No implementado");
                                    }
                                }
                            }
                            if (!block && pItemsDataBBDD != null && pItemsDataBBDD.Count > 1)
                            {
                                if (pItemsDataBBDD.ContainsKey(tipo))
                                {
                                    foreach (KeyValuePair<DisambiguableEntity, List<DisambiguationData>> item in pItemsDataBBDD[tipo])
                                    {
                                        if (data.Count != item.Value.Count)
                                        {
                                            throw new Exception("Todas las entiades del mismo tipo deben tener las mismas propiedades");
                                        }
                                        for (int i = 0; i < data.Count; i++)
                                        {
                                            if (data[i].property != item.Value[i].property)
                                            {
                                                throw new Exception("En los items " + tipo + " hay propiedades diferentes");
                                            }
                                            if (data[i].config.score != item.Value[i].config.score)
                                            {
                                                throw new Exception("Todas las entiades del mismo tipo deben tener el mismo score");
                                            }
                                            if (data[i].config.scoreMinus != item.Value[i].config.scoreMinus)
                                            {
                                                throw new Exception("Todas las entiades del mismo tipo deben tener el mismo scoreMinus");
                                            }
                                            if (data[i].config.type != item.Value[i].config.type)
                                            {
                                                throw new Exception("Todas las entiades del mismo tipo deben tener el mismo type");
                                            }
                                            switch (item.Value[i].config.type)
                                            {
                                                case DisambiguationDataConfigType.algoritmoNombres:
                                                    if (item.Value[i].value == null)
                                                    {
                                                        item.Value[i].value = "";
                                                    }
                                                    if (item.Value[i].config.score <= 0 && item.Value[i].config.score > 1)
                                                    {
                                                        throw new Exception("La propiedad score en 'algoritmoNombres' debe ser > 0 y <=1");
                                                    }
                                                    if (item.Value[i].config.scoreMinus != 0)
                                                    {
                                                        throw new Exception("La propiedad scoreMinus en 'algoritmoNombres' no hay que configurarla");
                                                    }
                                                    break;
                                                case DisambiguationDataConfigType.equalsIdentifiers:
                                                    if (item.Value[i].value == null)
                                                    {
                                                        item.Value[i].value = "";
                                                    }
                                                    if (item.Value[i].config.score != 0)
                                                    {
                                                        throw new Exception("La propiedad score en 'equalsIdentifiers' no hay que configurarla");
                                                    }
                                                    if (item.Value[i].config.scoreMinus != 0)
                                                    {
                                                        throw new Exception("La propiedad scoreMinus en 'equalsIdentifiers' no hay que configurarla");
                                                    }
                                                    break;
                                                case DisambiguationDataConfigType.equalsItem:
                                                    if (item.Value[i].value == null)
                                                    {
                                                        item.Value[i].value = "";
                                                    }
                                                    if (item.Value[i].config.score < 0 && item.Value[i].config.score > 1)
                                                    {
                                                        throw new Exception("La propiedad score en 'equalsItem' debe ser >= 0 y <=1");
                                                    }
                                                    if (item.Value[i].config.scoreMinus < 0 && item.Value[i].config.scoreMinus > 1)
                                                    {
                                                        throw new Exception("La propiedad scoreMinus en 'equalsItem' debe ser >= 0 y <=1");
                                                    }
                                                    break;
                                                case DisambiguationDataConfigType.equalsTitle:
                                                    if (item.Value[i].value == null)
                                                    {
                                                        item.Value[i].value = "";
                                                    }
                                                    if (item.Value[i].config.score <= 0 && item.Value[i].config.score > 1)
                                                    {
                                                        throw new Exception("La propiedad score en 'equalsTitle' debe ser > 0 y <=1");
                                                    }
                                                    if (item.Value[i].config.scoreMinus != 0)
                                                    {
                                                        throw new Exception("La propiedad scoreMinus en 'equalsTitle' no hay que configurarla");
                                                    }
                                                    break;
                                                case DisambiguationDataConfigType.equalsItemList:
                                                    if (item.Value[i].values == null)
                                                    {
                                                        item.Value[i].values = new HashSet<string>();
                                                    }
                                                    if (item.Value[i].config.score < 0 && item.Value[i].config.score > 1)
                                                    {
                                                        throw new Exception("La propiedad score en 'equalsItemList' debe ser >= 0 y <=1");
                                                    }
                                                    if (item.Value[i].config.scoreMinus != 0)
                                                    {
                                                        throw new Exception("La propiedad scoreMinus en 'equalsIdentifiers' no hay que configurarla");
                                                    }
                                                    break;
                                                default:
                                                    throw new Exception("No implementado");
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
                block = true;
            }
        }



        /// <summary>
        /// Obtiene la similitud entre dos datos del mismo tipo. 
        /// </summary>
        /// <param name="pDataA">Dato A</param>
        /// <param name="pDataB">Dato B</param>
        /// <param name="pListaDistintos">Diccionario con la lista de los IDs que no puden ser iguales</param>
        /// <param name="pToleranciaNombres">Tolerancia para los nombres de las personas (porcentaje máximo de nombres no coincidentes admitidos)</param>
        /// <returns>Número de similitud. A más cerca de 1, más similar es.</returns>
        /// <exception cref="Exception">Configuración no implementada.</exception>
        private static float GetSimilarity(KeyValuePair<DisambiguableEntity, List<DisambiguationData>> pDataA, KeyValuePair<DisambiguableEntity, List<DisambiguationData>> pDataB, Dictionary<string, string> pDicNomAutoresDesnormalizados, Dictionary<string, string> pDicTitulosDesnormalizados, Dictionary<string, Dictionary<string, float>> pEquivalencesAux, Dictionary<string, HashSet<string>> pListaDistintos, float pToleranciaNombres)
        {
            float result = 0;
            #region Con identificadores con valor
            if (pDataA.Value.Count > 0 && pDataA.Value.Exists(x => x.config.type == DisambiguationDataConfigType.equalsIdentifiers))
            {
                bool existenIdentificadores = false;
                for (int i = 0; i < pDataA.Value.Count; i++)
                {
                    DisambiguationData dataAAux = pDataA.Value[i];
                    DisambiguationData dataBAux = pDataB.Value[i];
                    if (dataAAux.config.type == DisambiguationDataConfigType.equalsIdentifiers)
                    {
                        if (!string.IsNullOrEmpty(dataAAux.value) && !string.IsNullOrEmpty(dataBAux.value))
                        {
                            existenIdentificadores = true;
                            if (dataAAux.value.Equals(dataBAux.value))
                            {
                                result = 1;
                            }
                        }
                    }
                }

                if (existenIdentificadores && result > 0)
                {
                    for (int i = 0; i < pDataA.Value.Count; i++)
                    {
                        DisambiguationData dataAAux = pDataA.Value[i];
                        DisambiguationData dataBAux = pDataB.Value[i];
                        if (dataAAux.config.type == DisambiguationDataConfigType.equalsIdentifiers)
                        {
                            if (!string.IsNullOrEmpty(dataAAux.value) && !string.IsNullOrEmpty(dataBAux.value) && !dataAAux.value.Equals(dataBAux.value))
                            {
                                result = 0;
                            }
                        }
                    }
                }
                if (existenIdentificadores)
                {
                    if (result == 0)
                    {
                        if (!pListaDistintos.ContainsKey(pDataB.Key.ID))
                        {
                            pListaDistintos[pDataB.Key.ID] = new HashSet<string>();
                        }
                        if (!pListaDistintos.ContainsKey(pDataA.Key.ID))
                        {
                            pListaDistintos[pDataA.Key.ID] = new HashSet<string>();
                        }
                        pListaDistintos[pDataB.Key.ID].Add(pDataA.Key.ID);
                        pListaDistintos[pDataA.Key.ID].Add(pDataB.Key.ID);
                    }
                    return result;
                }
            }
            #endregion

            #region Con título configurado
            if (pDataA.Value.Count > 0 && pDataA.Value.Exists(x => x.config.type == DisambiguationDataConfigType.equalsTitle))
            {
                //Títulos
                for (int i = 0; i < pDataA.Value.Count; i++)
                {
                    DisambiguationData dataAAux = pDataA.Value[i];
                    DisambiguationData dataBAux = pDataB.Value[i];
                    if (dataAAux.config.type == DisambiguationDataConfigType.equalsTitle)
                    {
                        if (GetTitleSimilarity(dataAAux.value, dataBAux.value, pDicTitulosDesnormalizados))
                        {
                            result += (1 - result) * dataAAux.config.score;
                        }
                    }
                }
                //Resto
                if (result > 0)
                {
                    //ItemA ItemB Prop
                    Dictionary<string, Dictionary<string, HashSet<string>>> dicEqualsItem = new Dictionary<string, Dictionary<string, HashSet<string>>>();
                    for (int i = 0; i < pDataA.Value.Count; i++)
                    {
                        DisambiguationData dataAAux = pDataA.Value[i];
                        DisambiguationData dataBAux = pDataB.Value[i];

                        if (dataAAux.config.score > 0)
                        {
                            switch (dataAAux.config.type)
                            {
                                case DisambiguationDataConfigType.equalsIdentifiers:
                                case DisambiguationDataConfigType.equalsTitle:
                                    //No se comprueba
                                    break;
                                case DisambiguationDataConfigType.algoritmoNombres:
                                    throw new Exception("Si tiene título no debería tener nombres");
                                case DisambiguationDataConfigType.equalsItem:
                                    if (PesoEqualsItem(ref result, dataAAux.config.score, dataAAux.value, dataBAux.value, pEquivalencesAux))
                                    {
                                        if (!dicEqualsItem.ContainsKey(dataAAux.value))
                                        {
                                            dicEqualsItem.Add(dataAAux.value, new Dictionary<string, HashSet<string>>());
                                        }
                                        if (!dicEqualsItem[dataAAux.value].ContainsKey(dataBAux.value))
                                        {
                                            dicEqualsItem[dataAAux.value].Add(dataBAux.value, new HashSet<string>());
                                        }
                                        dicEqualsItem[dataAAux.value][dataBAux.value].Add(dataAAux.property);
                                    }
                                    break;
                                case DisambiguationDataConfigType.equalsItemList:
                                    result = PesoEqualsItemList(result, dataAAux.config.score, dataAAux.values, dataBAux.values, pEquivalencesAux);
                                    break;
                                default:
                                    throw new Exception("No está implementado.");
                            }
                        }
                    }

                    if (result > 0)
                    {
                        for (int i = 0; i < pDataA.Value.Count; i++)
                        {
                            DisambiguationData dataAAux = pDataA.Value[i];
                            DisambiguationData dataBAux = pDataB.Value[i];

                            if (dataAAux.config.scoreMinus > 0)
                            {
                                switch (dataAAux.config.type)
                                {
                                    case DisambiguationDataConfigType.equalsIdentifiers:
                                    case DisambiguationDataConfigType.equalsTitle:
                                        //No se comprueba
                                        break;
                                    case DisambiguationDataConfigType.algoritmoNombres:
                                        throw new Exception("Si tiene título no debería tener nombres");
                                    case DisambiguationDataConfigType.equalsItem:
                                        if (!string.IsNullOrEmpty(dataAAux.value) && !string.IsNullOrEmpty(dataBAux.value))
                                        {
                                            if (!(dicEqualsItem.ContainsKey(dataAAux.value) &&
                                                dicEqualsItem[dataAAux.value].ContainsKey(dataBAux.value) &&
                                               dicEqualsItem[dataAAux.value][dataBAux.value].Contains(dataAAux.property)))
                                            {
                                                result -= result * dataAAux.config.scoreMinus;
                                            }
                                        }
                                        break;
                                    case DisambiguationDataConfigType.equalsItemList:
                                        throw new Exception("No hay negativo para equalsItemList");
                                    default:
                                        throw new Exception("No está implementado.");
                                }
                            }
                        }
                    }
                }

                return result;
            }
            #endregion

            #region Con nombre configurado
            if (pDataA.Value.Count > 0 && pDataA.Value.Exists(x => x.config.type == DisambiguationDataConfigType.algoritmoNombres))
            {
                //Nombres
                for (int i = 0; i < pDataA.Value.Count; i++)
                {
                    DisambiguationData dataAAux = pDataA.Value[i];
                    DisambiguationData dataBAux = pDataB.Value[i];
                    if (dataAAux.config.type == DisambiguationDataConfigType.algoritmoNombres)
                    {
                        float nameSimilarity = GetNameSimilarity(dataAAux.value, dataBAux.value, pDicNomAutoresDesnormalizados, pToleranciaNombres);
                        result += (1 - result) * nameSimilarity * dataAAux.config.score;
                    }
                }
                //Resto
                if (result > 0)
                {
                    Dictionary<string, Dictionary<string, HashSet<string>>> dicEqualsItem = new Dictionary<string, Dictionary<string, HashSet<string>>>();
                    for (int i = 0; i < pDataA.Value.Count; i++)
                    {
                        DisambiguationData dataAAux = pDataA.Value[i];
                        DisambiguationData dataBAux = pDataB.Value[i];

                        if (dataAAux.config.score > 0)
                        {
                            switch (dataAAux.config.type)
                            {
                                case DisambiguationDataConfigType.equalsIdentifiers:
                                case DisambiguationDataConfigType.algoritmoNombres:
                                    //No se comprueba
                                    break;
                                case DisambiguationDataConfigType.equalsTitle:
                                    throw new Exception("Si tiene nombre no debería tener titulo");
                                case DisambiguationDataConfigType.equalsItem:
                                    if (PesoEqualsItem(ref result, dataAAux.config.score, dataAAux.value, dataBAux.value, pEquivalencesAux))
                                    {
                                        if (!dicEqualsItem.ContainsKey(dataAAux.value))
                                        {
                                            dicEqualsItem.Add(dataAAux.value, new Dictionary<string, HashSet<string>>());
                                        }
                                        if (!dicEqualsItem[dataAAux.value].ContainsKey(dataBAux.value))
                                        {
                                            dicEqualsItem[dataAAux.value].Add(dataBAux.value, new HashSet<string>());
                                        }
                                        dicEqualsItem[dataAAux.value][dataBAux.value].Add(dataAAux.property);
                                    }
                                    break;
                                case DisambiguationDataConfigType.equalsItemList:
                                    result = PesoEqualsItemList(result, dataAAux.config.score, dataAAux.values, dataBAux.values, pEquivalencesAux);
                                    break;
                                default:
                                    throw new Exception("No está implementado.");
                            }
                        }
                    }

                    if (result > 0)
                    {
                        for (int i = 0; i < pDataA.Value.Count; i++)
                        {
                            DisambiguationData dataAAux = pDataA.Value[i];
                            DisambiguationData dataBAux = pDataB.Value[i];

                            if (dataAAux.config.scoreMinus > 0)
                            {
                                switch (dataAAux.config.type)
                                {
                                    case DisambiguationDataConfigType.equalsIdentifiers:
                                    case DisambiguationDataConfigType.algoritmoNombres:
                                        //No se comprueba
                                        break;
                                    case DisambiguationDataConfigType.equalsTitle:
                                        throw new Exception("Si tiene nombre no debería tener titulo");
                                    case DisambiguationDataConfigType.equalsItem:
                                        if (!string.IsNullOrEmpty(dataAAux.value) && !string.IsNullOrEmpty(dataBAux.value))
                                        {
                                            if (!(dicEqualsItem.ContainsKey(dataAAux.value) &&
                                                dicEqualsItem[dataAAux.value].ContainsKey(dataBAux.value) &&
                                               dicEqualsItem[dataAAux.value][dataBAux.value].Contains(dataAAux.property)))
                                            {
                                                result -= result * dataAAux.config.scoreMinus;
                                            }
                                        }
                                        break;
                                    case DisambiguationDataConfigType.equalsItemList:
                                        throw new Exception("No hay negativo para equalsItemList");
                                    default:
                                        throw new Exception("No está implementado.");
                                }
                            }
                        }
                    }
                }
                else
                {
                    result = 0;
                }

                return result;
            }
            #endregion

            #region resto
            Dictionary<string, Dictionary<string, HashSet<string>>> dicEqualsItemResto = new Dictionary<string, Dictionary<string, HashSet<string>>>();
            for (int i = 0; i < pDataA.Value.Count; i++)
            {
                DisambiguationData dataAAux = pDataA.Value[i];
                DisambiguationData dataBAux = pDataB.Value[i];

                if (dataAAux.config.score > 0)
                {
                    switch (dataAAux.config.type)
                    {
                        case DisambiguationDataConfigType.equalsTitle:
                        case DisambiguationDataConfigType.equalsIdentifiers:
                            //No se comprueba
                            break;
                        case DisambiguationDataConfigType.algoritmoNombres:
                            float similaridad = GetNameSimilarity(dataAAux.value, dataBAux.value, pDicNomAutoresDesnormalizados, pToleranciaNombres);
                            result += (1 - result) * similaridad * dataAAux.config.score;
                            break;
                        case DisambiguationDataConfigType.equalsItem:
                            if (PesoEqualsItem(ref result, dataAAux.config.score, dataAAux.value, dataBAux.value, pEquivalencesAux))
                            {
                                if (!dicEqualsItemResto.ContainsKey(dataAAux.value))
                                {
                                    dicEqualsItemResto.Add(dataAAux.value, new Dictionary<string, HashSet<string>>());
                                }
                                if (!dicEqualsItemResto[dataAAux.value].ContainsKey(dataBAux.value))
                                {
                                    dicEqualsItemResto[dataAAux.value].Add(dataBAux.value, new HashSet<string>());
                                }
                                dicEqualsItemResto[dataAAux.value][dataBAux.value].Add(dataAAux.property);
                            }
                            break;
                        default:
                            throw new Exception("No está implementado.");
                    }
                }
            }

            if (result > 0)
            {
                for (int i = 0; i < pDataA.Value.Count; i++)
                {
                    DisambiguationData dataAAux = pDataA.Value[i];
                    DisambiguationData dataBAux = pDataB.Value[i];

                    if (dataAAux.config.scoreMinus > 0)
                    {
                        switch (dataAAux.config.type)
                        {
                            case DisambiguationDataConfigType.equalsIdentifiers:
                            case DisambiguationDataConfigType.equalsTitle:
                            case DisambiguationDataConfigType.algoritmoNombres:
                                //No se comprueba
                                break;
                            case DisambiguationDataConfigType.equalsItem:
                                if (!string.IsNullOrEmpty(dataAAux.value) && !string.IsNullOrEmpty(dataBAux.value))
                                {
                                    if (!(dicEqualsItemResto.ContainsKey(dataAAux.value) &&
                                        dicEqualsItemResto[dataAAux.value].ContainsKey(dataBAux.value) &&
                                       dicEqualsItemResto[dataAAux.value][dataBAux.value].Contains(dataAAux.property)))
                                    {
                                        result -= result * dataAAux.config.scoreMinus;
                                    }
                                }
                                break;
                            default:
                                throw new Exception("No está implementado.");
                        }
                    }
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// Procesar el peso para los EqualsItemList
        /// </summary>
        /// <param name="pPesoInicial">Peso inicial</param>
        /// <param name="pScore">Score de la propiedad</param>
        /// <param name="pValoresA">Valores A</param>
        /// <param name="pValoresB">Valores B</param>
        /// <param name="pEquivalencias">Equivalencias</param>
        /// <returns></returns>
        private static float PesoEqualsItemList(float pPesoInicial, float pScore, HashSet<string> pValoresA, HashSet<string> pValoresB, Dictionary<string, Dictionary<string, float>> pEquivalencias)
        {
            Dictionary<string, Dictionary<string, float>> dicFinal = new Dictionary<string, Dictionary<string, float>>();
            foreach (string id in pValoresA)
            {
                foreach (string id2 in pValoresB)
                {
                    if (id == id2)
                    {
                        if (!dicFinal.ContainsKey(id))
                        {
                            dicFinal.Add(id, new Dictionary<string, float>());
                        }
                        dicFinal[id][id2] = 1;
                    }
                }
            }
            if (pEquivalencias != null)
            {
                foreach (string id in pValoresA)
                {
                    if (pEquivalencias.ContainsKey(id))
                    {
                        foreach (string id2 in pValoresB)
                        {
                            if (pEquivalencias[id].ContainsKey(id2))
                            {
                                if (!dicFinal.ContainsKey(id))
                                {
                                    dicFinal.Add(id, new Dictionary<string, float>());
                                }
                                if (dicFinal[id].ContainsKey(id2))
                                {
                                    if (pEquivalencias[id][id2] > dicFinal[id][id2])
                                    {
                                        dicFinal[id][id2] = pEquivalencias[id][id2];
                                    }
                                }
                                else
                                {
                                    dicFinal[id][id2] = pEquivalencias[id][id2];
                                }
                            }
                        }
                    }
                }
            }
            HashSet<string> id2Agnadidos = new HashSet<string>();
            foreach (string id1 in dicFinal.Keys)
            {
                foreach (string id2 in dicFinal[id1].Keys)
                {
                    if (id2Agnadidos.Add(id2))
                    {
                        pPesoInicial += (1 - pPesoInicial) * dicFinal[id1][id2] * pScore;
                    }
                }
            }
            return pPesoInicial;
        }

        /// <summary>
        /// Procesar el peso para los EqualsItem
        /// </summary>
        /// <param name="pPesoInicial">Peso inicial</param>
        /// <param name="pScore">Score de la propiedad</param>
        /// <param name="pValorA">Valor A</param>
        /// <param name="pValorB">Valor B</param>
        /// <param name="pEquivalencias">Equivalencias</param>
        /// <returns></returns>
        private static bool PesoEqualsItem(ref float pPesoInicial, float pScore, string pValorA, string pValorB, Dictionary<string, Dictionary<string, float>> pEquivalencias)
        {
            bool cambio = false;
            if (!string.IsNullOrEmpty(pValorA) && !string.IsNullOrEmpty(pValorB))
            {
                float peso = 0;
                if (pValorA.ToLower().Equals(pValorB.ToLower()))
                {
                    peso = 1;
                }
                else if (pEquivalencias != null && pEquivalencias.ContainsKey(pValorA))
                {
                    foreach (string equivalence in pEquivalencias[pValorA].Keys)
                    {
                        if (equivalence == pValorB)
                        {
                            if (pEquivalencias[pValorA][equivalence] > peso)
                            {
                                peso = pEquivalencias[pValorA][equivalence];
                            }
                        }
                        if (pEquivalencias.ContainsKey(pValorB))
                        {
                            foreach (string equivalence2 in pEquivalencias[pValorB].Keys)
                            {
                                if (equivalence == equivalence2)
                                {
                                    if (pEquivalencias[pValorB][equivalence2] > peso)
                                    {
                                        peso = pEquivalencias[pValorB][equivalence2];
                                    }
                                }
                            }
                        }
                    }
                }
                else if (pEquivalencias != null && pEquivalencias.ContainsKey(pValorB) && !pEquivalencias.ContainsKey(pValorA))
                {
                    foreach (string equivalence2 in pEquivalencias[pValorB].Keys)
                    {
                        if (equivalence2 == pValorA)
                        {
                            if (pEquivalencias[pValorB][equivalence2] > peso)
                            {
                                peso = pEquivalencias[pValorB][equivalence2];
                            }
                        }
                    }
                }
                if (peso > 0)
                {
                    cambio = true;
                    pPesoInicial += (1 - pPesoInicial) * peso * pScore;
                }
            }
            return cambio;
        }


        /// <summary>
        /// Método para normalizar los títulos
        /// </summary>
        /// <param name="pTitle">Título</param>
        /// <returns></returns>
        private static string NormalizeTitle(string pTitle)
        {
            string normalizedString = pTitle.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            foreach (char charin in normalizedString)
            {
                if (char.IsLetterOrDigit(charin))
                {
                    sb.Append(charin);
                }
            }
            normalizedString = sb.ToString().Normalize(NormalizationForm.FormD).ToLower();
            return normalizedString;
        }

        /// <summary>
        /// Agrega los IDs con su ID similar y su similaridad.
        /// </summary>
        /// <param name="pEquivalences"></param>
        /// <param name="pSimilaridad"></param>
        /// <param name="pIdA"></param>
        /// <param name="pIdB"></param>
        private static void AddSimilarityId(string pTipo, Dictionary<string, Dictionary<string, float>> pEquivalences, float pSimilaridad, string pIdA, string pIdB)
        {
            if (pSimilaridad > 0) // TODO: Umbral.
            {
                if (!pEquivalences.ContainsKey($"{pTipo}|{pIdA}"))
                {
                    pEquivalences.Add(pTipo + "|" + pIdA, new Dictionary<string, float>());
                }
                if (!pEquivalences.ContainsKey($"{pTipo}|{pIdB}"))
                {
                    pEquivalences.Add(pTipo + "|" + pIdB, new Dictionary<string, float>());
                }
                pEquivalences[$"{pTipo}|{pIdA}"][$"{pTipo}|{pIdB}"] = pSimilaridad;
                pEquivalences[$"{pTipo}|{pIdB}"][$"{pTipo}|{pIdA}"] = pSimilaridad;
            }
        }

        public static float GetNameSimilarity(string pSource, string pTarget, Dictionary<string, string> pDicNomAutoresDesnormalizados, float pToleranciaNombres)
        {
            pSource = ObtenerTextosNombresNormalizados(pSource, pDicNomAutoresDesnormalizados);
            pTarget = ObtenerTextosNombresNormalizados(pTarget, pDicNomAutoresDesnormalizados);

            string[] pFirmaNormalizadoSplitAux = pSource.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string[] pTargetNormalizadoSplitAux = pTarget.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            List<string> pFirmaNormalizadoSplitAux2 = new List<string>();
            List<string> pTargetNormalizadoSplitAux2 = new List<string>();

            foreach (string word in pFirmaNormalizadoSplitAux)
            {
                if (word.Length == 2)
                {
                    pFirmaNormalizadoSplitAux2.Add(word[0].ToString());
                    pFirmaNormalizadoSplitAux2.Add(word[1].ToString());
                }
                else
                {
                    pFirmaNormalizadoSplitAux2.Add(word);
                }
            }

            foreach (string word in pTargetNormalizadoSplitAux)
            {
                if (word.Length == 2)
                {
                    pTargetNormalizadoSplitAux2.Add(word[0].ToString());
                    pTargetNormalizadoSplitAux2.Add(word[1].ToString());
                }
                else
                {
                    pTargetNormalizadoSplitAux2.Add(word);
                }
            }

            string[] pFirmaNormalizadoSplit = pFirmaNormalizadoSplitAux2.ToArray();
            string[] pTargetNormalizadoSplit = pTargetNormalizadoSplitAux2.ToArray();



            string[] source1 = pFirmaNormalizadoSplit;
            string[] target1 = pTargetNormalizadoSplit;

            List<string[]> sources = new List<string[]>();
            List<string[]> targets = new List<string[]>();

            sources.Add(source1);
            targets.Add(target1);

            int sourceLength = source1.Length;
            if (sourceLength > 1 && source1.Last().Length == 1)
            {
                string[] source2 = new string[sourceLength];
                source2[0] = source1.Last();
                for (int i = 0; i < sourceLength - 1; i++)
                {
                    source2[i + 1] = source1[i];
                }
                sources.Add(source2);
            }
            int targetLength = target1.Length;
            if (targetLength > 1 && target1.Last().Length == 1)
            {
                string[] target2 = new string[targetLength];
                target2[0] = target1.Last();
                for (int i = 0; i < targetLength - 1; i++)
                {
                    target2[i + 1] = target1[i];
                }
                targets.Add(target2);
            }
            float scoreMax = 0;
            foreach (string[] source in sources)
            {
                foreach (string[] target in targets)
                {
                    //Almacenamos los scores de cada una de las palabras
                    List<float> scores = new List<float>();

                    int indexTarget = 0;
                    bool coincidenciaNombreNoinicial = false;
                    for (int i = 0; i < source.Length; i++)
                    {
                        //Similitud real
                        float score = 0;
                        string wordSource = source[i];
                        bool wordSourceInicial = wordSource.Length == 1;
                        //int desplazamiento = 0;
                        for (int j = indexTarget; j < target.Length; j++)
                        {
                            string wordTarget = target[j];
                            bool wordTargetInicial = wordTarget.Length == 1;
                            //Alguna de las dos es inicial
                            if (wordSourceInicial || wordTargetInicial)
                            {
                                float scoreWord = scoreInicial;
                                if (ScoreNombresCalculado.ContainsKey(wordSource[0].ToString()))
                                {
                                    scoreWord = ScoreNombresCalculado[wordSource[0].ToString()];
                                }
                                if (wordSource[0] == wordTarget[0])
                                {
                                    score = scoreWord;
                                    indexTarget = j + 1;
                                    break;
                                }
                            }
                            //Ninguna de las dos es inicial
                            if (wordSource.Equals(wordTarget))
                            {
                                coincidenciaNombreNoinicial = true;
                                float scoreWord = minimoScoreNombres;
                                if (ScoreNombresCalculado.ContainsKey(wordTarget))
                                {
                                    scoreWord = ScoreNombresCalculado[wordTarget];
                                }
                                score = scoreWord;
                                indexTarget = j + 1;
                                break;
                            }
                        }
                        if (score > 0)
                        {
                            scores.Add(score);
                        }
                    }

                    //Coincide algo que no sea inicial y hay algun asimilitud
                    if (coincidenciaNombreNoinicial && scores.Count > 0)
                    {
                        int longMin = Math.Min(source.Length, target.Length);
                        if (1 - (scores.Count / (float)longMin) > pToleranciaNombres)
                        {
                            //return 0;
                            continue;
                        }

                        //Calculamos las coincidencias
                        float scoreFinal = 0;
                        foreach (float score in scores)
                        {
                            scoreFinal += (1 - scoreFinal) * score;
                        }
                        //Si hay varias no coincidencias dividimos
                        if (scores.Count < longMin)
                        {
                            scoreFinal = scoreFinal / (1 + longMin - scores.Count);
                        }
                        scoreMax = Math.Max(scoreMax, scoreFinal);
                    }
                }
            }
            return scoreMax;
        }

        public static bool GetTitleSimilarity(string pTituloA, string pTituloB, Dictionary<string, string> pDicTitulosDesnormalizados)
        {
            string tituloAAux = ObtenerTextosTitulosNormalizados(pTituloA, pDicTitulosDesnormalizados);
            string tituloBAux = ObtenerTextosTitulosNormalizados(pTituloB, pDicTitulosDesnormalizados);
            if (!string.IsNullOrEmpty(tituloAAux) && !string.IsNullOrEmpty(tituloBAux) && tituloAAux.Equals(tituloBAux))
            {
                return true;
            }
            return false;
        }

        public static string ObtenerTextosNombresNormalizados(string pText, Dictionary<string, string> pDicNomAutoresDesnormalizados = null)
        {
            // Comprobación si tenemos guardado el nombre.
            string textoAux = pText;
            if (pDicNomAutoresDesnormalizados != null && pDicNomAutoresDesnormalizados.ContainsKey(textoAux))
            {
                return pDicNomAutoresDesnormalizados[textoAux];
            }

            pText = pText.Replace(".", " ").ToLower();
            pText = pText.Trim();
            if (pText.Contains(","))
            {
                pText = pText.Substring(pText.IndexOf(",") + 1).Trim() + " " + (pText.Substring(0, pText.IndexOf(","))).Trim();
            }
            pText = pText.Replace("-", " ");
            string textoNormalizado = pText.Normalize(NormalizationForm.FormD);
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z ]");
            string textoSinAcentos = reg.Replace(textoNormalizado, "");
            while (textoSinAcentos.Contains(" del "))
            {
                textoSinAcentos = textoSinAcentos.Replace(" del ", " ");
            }
            while (textoSinAcentos.Contains(" de "))
            {
                textoSinAcentos = textoSinAcentos.Replace(" de ", " ");
            }
            while (textoSinAcentos.Contains(" la "))
            {
                textoSinAcentos = textoSinAcentos.Replace(" la ", " ");
            }
            while (textoSinAcentos.Contains(" von "))
            {
                textoSinAcentos = textoSinAcentos.Replace(" von ", " ");
            }
            while (textoSinAcentos.Contains(" al "))
            {
                textoSinAcentos = textoSinAcentos.Replace(" al ", " ");
            }
            while (textoSinAcentos.Contains("  "))
            {
                textoSinAcentos = textoSinAcentos.Replace("  ", " ");
            }

            if (pDicNomAutoresDesnormalizados != null)
            {
                pDicNomAutoresDesnormalizados.Add(textoAux, textoSinAcentos.Trim());
            }

            return textoSinAcentos.Trim();
        }

        private static string ObtenerTextosTitulosNormalizados(string pText, Dictionary<string, string> pDicTitulosDesnormalizados)
        {
            // Comprobación si tenemos guardado el nombre.
            string textoAux = pText;
            if (pDicTitulosDesnormalizados.ContainsKey(textoAux))
            {
                return pDicTitulosDesnormalizados[textoAux];
            }
            textoAux = NormalizeTitle(pText);
            pDicTitulosDesnormalizados[pText] = textoAux;
            return textoAux.Trim();
        }

        private static HashSet<string> GetNGramas(string pText, int pNgramSize)
        {
            HashSet<string> ngramas = new HashSet<string>();
            int textLength = pText.Length;
            if (pNgramSize == 1)
            {
                for (int i = 0; i < textLength; i++)
                {
                    ngramas.Add(pText[i].ToString());
                }
                return ngramas;
            }

            HashSet<string> ngramasaux = new HashSet<string>();
            for (int i = 0; i < textLength; i++)
            {
                foreach (string ngram in ngramasaux.ToList())
                {
                    string ngamaux = ngram + pText[i];
                    if (ngamaux.Length == pNgramSize)
                    {
                        ngramas.Add(ngamaux);
                    }
                    else
                    {
                        ngramasaux.Add(ngamaux);
                    }
                    ngramasaux.Remove(ngram);
                }
                ngramasaux.Add(pText[i].ToString());
                if (i < pNgramSize)
                {
                    foreach (string ngrama in ngramasaux)
                    {
                        if (ngrama.Length == i + 1)
                        {
                            ngramas.Add(ngrama);
                        }
                    }
                }
            }
            for (int i = (textLength - pNgramSize) + 1; i < textLength; i++)
            {
                if (i >= pNgramSize)
                {
                    ngramas.Add(pText.Substring(i));
                }
            }
            return ngramas;
        }
    }
}
