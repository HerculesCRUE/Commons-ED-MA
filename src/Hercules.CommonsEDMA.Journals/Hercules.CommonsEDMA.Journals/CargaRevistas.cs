using ExcelDataReader;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Hercules.CommonsEDMA.Journals.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.Journals
{
    internal class CargaRevistas
    {
        private static ResourceApi mResourceApi = new ResourceApi($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config");

        //Número de hilos para el paralelismo.
        private static int NUM_HILOS = 6;

        //Número máximo de intentos de subida
        private static int MAX_INTENTOS = 10;

        public static void CargarRevistas()
        {
            string nombreExcel = "Revistas";
            string nombreHoja = "revistas";

            // Diccionario de revistas.
            List<Journal> listaRevistas = new List<Journal>();

            Console.WriteLine("1/7.- Leemos las revistas del Excel");
            DataSet dataSet = LecturaExcel($@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Dataset{Path.DirectorySeparatorChar}{nombreExcel}.xlsx");

            if (ComprobarColumnasExcel(dataSet, nombreHoja))
            {
                listaRevistas = GetRevistas(dataSet, nombreHoja, listaRevistas);
            }
            else
            {
                Console.WriteLine("Se ha producido un error al comprobar el Excel");
                throw new Exception("Se ha producido un error al comprobar el Excel");
            }

            // Ordenación de por Key.
            listaRevistas = listaRevistas.OrderBy(obj => obj.titulo).ToList();

            // Comprobar si hay datos duplicados.
            ComprobarErrores(listaRevistas);

            // Carga/Modificación/Borrado de datos de BBDD. 
            ModificarRevistas(listaRevistas);
            Console.WriteLine("El proceso ha terminado");
        }


        private static void ModificarRevistas(List<Journal> pRevistas)
        {
            // Obtenemos las revistas de BBDD
            List<string> idRecursosRevistas = ObtenerIDsRevistas();
            List<Journal> dicRevistasBBDD = ObtenerRevistaPorID(idRecursosRevistas).Values.ToList();

            mResourceApi.ChangeOntoly("maindocument");

            // Creación 
            List<Journal> revistasCargar = pRevistas.Where(x => string.IsNullOrEmpty(x.idJournal)).ToList();
            List<ComplexOntologyResource> listaRecursosCargar = new List<ComplexOntologyResource>();
            ObtenerRevistas(revistasCargar, listaRecursosCargar);
            CargarDatos(listaRecursosCargar);

            Console.WriteLine($"6/7.- Comprobando borrados");

            // Borrado
            foreach (Journal journal in dicRevistasBBDD)
            {
                if (!pRevistas.Exists(x => x.idJournal == journal.idJournal))
                {
                    try
                    {
                        mResourceApi.PersistentDelete(mResourceApi.GetShortGuid(journal.idJournal));
                    }
                    catch
                    {
                        // Si entra por aquí, es error de CORE.
                    }
                }
            }

            Console.WriteLine($"7/7.- Comprobando modificaciones");

            // Modificación            
            foreach (Journal journalBBDD in dicRevistasBBDD)
            {
                Journal journalCargar = pRevistas.FirstOrDefault(x => x.idJournal == journalBBDD.idJournal);
                if (journalCargar != null)
                {
                    if (!journalBBDD.Equals(journalCargar))
                    {
                        List<ComplexOntologyResource> listaRecursosModificar = new List<ComplexOntologyResource>();
                        ObtenerRevistas(new List<Journal>() { journalCargar }, listaRecursosModificar);
                        ModificarDatos(listaRecursosCargar);
                    }
                }
            }
        }

        /// <summary>
        /// Contruye el objeto de Revista.
        /// </summary>
        /// <param name="pDataSet"></param>
        /// <param name="pNombreHoja"></param>
        /// <param name="pListaRevistas"></param>
        /// <returns></returns>
        private static List<Journal> GetRevistas(DataSet pDataSet, string pNombreHoja, List<Journal> pListaRevistas)
        {
            Console.WriteLine("2/7.- Montamos los objetos a cargar");
            // Obtención de la hoja a leer.
            DataTable tabla = pDataSet.Tables[$@"{pNombreHoja}"];

            // Contadores.
            int revistasSinTitulo = 0;
            int revistasSinIdentificadores = 0;

            // Ordenar DATASET
            DataTable tablaRevistas = pDataSet.Tables[tabla.TableName];
            tablaRevistas.DefaultView.Sort = "TITLE ASC, PUBLISHER_NAME ASC, ISSN ASC, EISSN ASC, SOURCE ASC, YEAR ASC";
            tablaRevistas = pDataSet.Tables[tabla.TableName].DefaultView.ToTable();

            int numRevista = 0;
            int numRevistas = tablaRevistas.Rows.Count;
            Console.Write($"3/7.- Procesando fila del excel {0}/{numRevistas}");
            foreach (DataRow fila in tablaRevistas.Rows)
            {
                numRevista++;
                Console.Write($"\r3/7.- Procesando fila del excel {numRevista}/{numRevistas}");

                // Si la revista no tiene título, no es válida.
                if (string.IsNullOrEmpty(fila["TITLE"].ToString()))
                {
                    revistasSinTitulo++;
                    continue;
                }

                // Si la revista no tiene ISSN ni EISSN, no es válida.
                if (string.IsNullOrEmpty(fila["ISSN"].ToString()) && string.IsNullOrEmpty(fila["EISSN"].ToString()))
                {
                    revistasSinIdentificadores++;
                    continue;
                }

                // Datos 
                string titleAux = fila["TITLE"].ToString();
                string editorialAux = fila["PUBLISHER_NAME"].ToString();
                string issnAux = LimpiarIdentificador(fila["ISSN"].ToString());
                string eissnAux = LimpiarIdentificador(fila["EISSN"].ToString());

                // Si tienen el mismo ISSN e EISSN, únicamente tienen EISSN.
                if (!string.IsNullOrEmpty(issnAux) && !string.IsNullOrEmpty(eissnAux) && issnAux == eissnAux)
                {
                    issnAux = null;
                }

                Journal revista = ComprobarRevista(pListaRevistas, titleAux, editorialAux, issnAux, eissnAux);

                if (revista == null)
                {
                    revista = new Journal();
                    revista.indicesImpacto = new HashSet<IndiceImpacto>();
                    pListaRevistas.Add(revista);
                }

                // Año.
                int anyo = Int32.Parse(fila["YEAR"].ToString());

                // Título.
                revista.titulo = titleAux;

                if (!string.IsNullOrEmpty(editorialAux))
                {
                    // Publicador.
                    revista.publicador = editorialAux;
                }

                if (!string.IsNullOrEmpty(issnAux))
                {
                    // ISSN.
                    revista.issn = issnAux;
                }

                if (!string.IsNullOrEmpty(eissnAux))
                {
                    // EISSN.
                    revista.eissn = eissnAux;
                }

                // Índice de impacto.
                bool encontrado = false;
                foreach (IndiceImpacto item in revista.indicesImpacto)
                {
                    if (item.fuente == fila["SOURCE"].ToString() && item.anyo == anyo)
                    {
                        encontrado = true;
                        break;
                    }
                }

                if (!encontrado && !string.IsNullOrEmpty(fila["IMPACT_FACTOR"].ToString()))
                {
                    revista.indicesImpacto.Add(CrearIndiceImpacto(fila, anyo));
                }

                // Categorías.
                if (!string.IsNullOrEmpty(fila["CATEGORY_DESCRIPTION"].ToString()) && !string.IsNullOrEmpty(fila["IMPACT_FACTOR"].ToString()))
                {
                    HashSet<Categoria> categorias = revista.indicesImpacto.First(x => x.anyo == anyo && x.fuente == fila["SOURCE"].ToString()).categorias;
                    Categoria categoria = CrearCategoria(fila, anyo);

                    if (!categorias.Any(x => x.nomCategoria == categoria.nomCategoria))
                    {
                        revista.indicesImpacto.First(x => x.anyo == anyo && x.fuente == fila["SOURCE"].ToString()).categorias.Add(categoria);
                    }
                    else
                    {
                        Categoria categoriaCargada = categorias.First(x => x.nomCategoria == categoria.nomCategoria);
                        float rangoNuevo = (float)categoria.posicionPublicacion / (float)categoria.numCategoria;
                        float rangoCargado = (float)categoriaCargada.posicionPublicacion / (float)categoriaCargada.numCategoria;

                        if ((rangoNuevo < rangoCargado) || (rangoNuevo == rangoCargado && categoria.numCategoria > categoriaCargada.numCategoria))
                        {
                            categorias.Remove(categoriaCargada);
                            categorias.Add(categoria);
                            revista.indicesImpacto.First(x => x.anyo == anyo && x.fuente == fila["SOURCE"].ToString()).categorias = categorias;
                        }
                    }
                }
            }
            Console.WriteLine($"\r3/7.- Procesando fila del excel {numRevista}/{numRevistas}");

            return pListaRevistas;
        }

        /// <summary>
        /// Permite crear un índice de impacto con los datos de Scopus.
        /// </summary>
        /// <param name="pFila">Fila con los datos pertenecientes al excel.</param>
        /// <param name="pAnyo">Año.</param>
        /// <returns>Índice de impacto.</returns>
        private static IndiceImpacto CrearIndiceImpacto(DataRow pFila, int pAnyo)
        {
            IndiceImpacto indiceImpacto = new IndiceImpacto();
            indiceImpacto.categorias = new HashSet<Categoria>();

            // Fuente.
            indiceImpacto.fuente = pFila["SOURCE"].ToString();

            // Año.
            indiceImpacto.anyo = pAnyo;

            // Índice de impacto.
            if (!string.IsNullOrEmpty(pFila["IMPACT_FACTOR"].ToString()))
            {
                indiceImpacto.indiceImpacto = float.Parse(pFila["IMPACT_FACTOR"].ToString().Replace(",", "."), CultureInfo.InvariantCulture);
            }

            return indiceImpacto;
        }

        /// <summary>
        /// Permite crear una categoría con los datos de WoS.
        /// </summary>
        /// <param name="pFila">Fila con los datos pertenecientes al excel.</param>
        /// <param name="pAnyo">Año.</param>
        /// <returns>Categoría.</returns>
        private static Categoria CrearCategoria(DataRow pFila, int pAnyo)
        {
            Categoria categoria = new Categoria();

            // Fuente.
            categoria.fuente = pFila["SOURCE"].ToString();

            // Año.
            categoria.anyo = pAnyo;

            // Nombre categoría.
            if (!string.IsNullOrEmpty(pFila["CATEGORY_DESCRIPTION"].ToString()))
            {
                categoria.nomCategoria = pFila["CATEGORY_DESCRIPTION"].ToString();
            }

            // Ranking y Posición en ranking.
            if (!string.IsNullOrEmpty(pFila["RANK"].ToString()) && !string.IsNullOrEmpty(pFila["RANK_OUT_OF"].ToString()) && pFila["RANK"].ToString() != "--" && pFila["RANK_OUT_OF"].ToString() != "--")
            {
                categoria.posicionPublicacion = Int32.Parse(pFila["RANK"].ToString());
                categoria.numCategoria = Int32.Parse(pFila["RANK_OUT_OF"].ToString());
            }

            // Cuartil.
            if (!string.IsNullOrEmpty(pFila["QUARTILE_RANK"].ToString()))
            {
                switch (pFila["QUARTILE_RANK"].ToString().ToLower())
                {
                    case "q1":
                        categoria.cuartil = 1;
                        break;
                    case "q2":
                        categoria.cuartil = 2;
                        break;
                    case "q3":
                        categoria.cuartil = 3;
                        break;
                    case "q4":
                        categoria.cuartil = 4;
                        break;
                }
            }

            return categoria;
        }

        /// <summary>
        /// Dado un ISSN o E-ISSN, lo construye en buen formato.
        /// </summary>
        /// <param name="pId">Identificador a formatear.</param>
        /// <returns>Identificador formateado.</returns>
        private static string LimpiarIdentificador(string pId)
        {
            // Comprobar si el ISSN/EISSN está bien formado.
            if (pId == "****-****")
            {
                return null;
            }

            // Comprobar si el ISSN/EISSN está bien formado.
            if (pId.Length == 9 && pId.Contains("-") && pId.Split("-")[0].Length == 4 && pId.Split("-")[1].Length == 4)
            {
                return pId;
            }

            string idFinal = null;

            if (!string.IsNullOrEmpty(pId) && pId.Length <= 8)
            {
                // Los ISSN y E-ISSN se componen por 8 caracteres.
                int numDiferencia = 8 - pId.Length;

                // Rellenamos con 0s en primera posición.
                if (numDiferencia != 0)
                {
                    for (int i = 0; i < numDiferencia; i++)
                    {
                        pId = $@"0{pId}";
                    }
                }

                // Agregamos el guión en medio.
                string parte1 = pId.Substring(0, 4);
                string parte2 = pId.Substring(4);
                idFinal = $@"{parte1}-{parte2}";
            }

            return idFinal;
        }

        /// <summary>
        /// Verifica si hay revistas duplicadas con diversas condiciones.
        /// </summary>
        /// <param name="pListaRevistas">Listado dónde se almacenan las revistas.</param>
        /// <param name="pTitulo">Título.</param>
        /// <param name="pEditorial">Editorial.</param>
        /// <param name="pIssn">ISSN</param>
        /// <returns>Revista si la encuentra o null si no la encuentra.</returns>
        /// <exception cref="Exception"></exception>
        private static Journal ComprobarRevista(List<Journal> pListaRevistas, string pTitulo = null, string pEditorial = null, string pIssn = null, string pEissn = null)
        {
            // No consideramos el EISSN como identificador porque diferentes revistas (con diferentes ISSN que sean tengan algún parentesco) pueden tener la misma versión online.
            Journal revista = null;
            List<Journal> revistasMismoTituloYEditorial = pListaRevistas.Where(x => x.titulo.ToLower() == pTitulo.ToLower() && x.publicador?.ToLower() == pEditorial?.ToLower()).ToList();
            List<Journal> revistasMismoISSN = pListaRevistas.Where(x => x.issn == pIssn && !string.IsNullOrEmpty(pIssn)).Except(revistasMismoTituloYEditorial).ToList();
            List<Journal> revistasMismoTituloYEISSN = pListaRevistas.Where(x => x.titulo.ToLower() == pTitulo.ToLower() && x.eissn == pEissn && !string.IsNullOrEmpty(pEissn)).Except(revistasMismoISSN).Except(revistasMismoTituloYEditorial).ToList();

            if (revistasMismoTituloYEditorial.Count > 1 || revistasMismoISSN.Count > 1 || revistasMismoTituloYEISSN.Count > 1)
            {
                throw new Exception("Datos duplicados.");
            }

            if (revistasMismoISSN.Count > 0 && revistasMismoTituloYEditorial.Count == 0 && revistasMismoTituloYEISSN.Count == 0)
            {
                revista = revistasMismoISSN[0];
            }
            else if (revistasMismoTituloYEditorial.Count > 0 && revistasMismoISSN.Count == 0 && revistasMismoTituloYEISSN.Count == 0)
            {
                revista = revistasMismoTituloYEditorial[0];
            }
            else if (revistasMismoTituloYEISSN.Count > 0 && revistasMismoTituloYEditorial.Count == 0 && revistasMismoISSN.Count == 0)
            {
                revista = revistasMismoTituloYEISSN[0];
            }
            else if (revistasMismoTituloYEditorial.Count > 0 || revistasMismoISSN.Count > 0 || revistasMismoTituloYEISSN.Count > 0)
            {
                // No debería entrar por aquí, porque significaría que hay una revista con mismo título/editorial y otra revista diferente con el mismo ISSN.
                // Nos quedamos con la que tenga el mismo ISSN.
                if (revistasMismoISSN.Count > 0)
                {
                    revista = revistasMismoISSN[0];

                    if (revistasMismoTituloYEditorial.Count > 0)
                    {
                        pListaRevistas.Remove(revistasMismoTituloYEditorial[0]);
                    }

                    if (revistasMismoTituloYEISSN.Count > 0)
                    {
                        pListaRevistas.Remove(revistasMismoTituloYEISSN[0]);
                    }
                }

                if (revistasMismoTituloYEISSN.Count > 0)
                {
                    revista = revistasMismoTituloYEISSN[0];

                    if (revistasMismoTituloYEditorial.Count > 0)
                    {
                        pListaRevistas.Remove(revistasMismoTituloYEditorial[0]);
                    }
                }
            }

            return revista;
        }

        /// <summary>
        /// Control de errores.
        /// </summary>
        /// <param name="pListaRevistas">Lista de revistas.</param>
        /// <exception cref="Exception"></exception>
        private static void ComprobarErrores(List<Journal> pListaRevistas)
        {
            #region --- Comprobar que no haya revistas con el mismo ISSN.
            Dictionary<string, Journal> issns = new Dictionary<string, Journal>();
            foreach (Journal revista in pListaRevistas)
            {
                if (!string.IsNullOrEmpty(revista.issn))
                {
                    try
                    {
                        issns.Add(revista.issn, revista);
                    }
                    catch
                    {
                        throw new Exception("Hay revistas con el mismo ISSN.");
                    }
                }
            }
            #endregion

            #region --- Comprobar que no haya revistas con el mismo título y editorial.
            Dictionary<string, Journal> titulos = new Dictionary<string, Journal>();
            foreach (Journal revista in pListaRevistas)
            {
                try
                {
                    titulos.Add(revista.titulo.ToLower() + "|||" + revista.publicador.ToLower(), revista);
                }
                catch
                {
                    throw new Exception("Hay revistas con el mismo título y publicador.");
                }
            }
            #endregion

            #region --- Comprobar que no haya revistas con el mismo título y EISSN.
            Dictionary<string, Journal> eissns = new Dictionary<string, Journal>();
            foreach (Journal revista in pListaRevistas)
            {
                if (!string.IsNullOrEmpty(revista.eissn))
                {
                    try
                    {
                        eissns.Add(revista.titulo.ToLower() + "|||" + revista.eissn, revista);
                    }
                    catch
                    {
                        throw new Exception("Hay revistas con el mismo título y EISSN.");
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// Obtiene todos los identificadores de los recursos de las revistas cargadas.
        /// </summary>
        /// <returns>Lista con los identificadores.</returns>
        private static List<string> ObtenerIDsRevistas()
        {
            List<string> idsRecursos = new List<string>();
            int limit = 10000;
            int offset = 0;
            bool salirBucle = false;

            do
            {
                // Consulta sparql.
                string select = "SELECT * WHERE { SELECT ?revista ";
                string where = $@"WHERE {{
                                ?revista a <http://w3id.org/roh/MainDocument>.
                                }} ORDER BY DESC(?revista) }} LIMIT {limit} OFFSET {offset} ";

                SparqlObject resultadoQuery = mResourceApi.VirtuosoQuery(select, where, "maindocument");
                if (resultadoQuery != null && resultadoQuery.results != null && resultadoQuery.results.bindings != null && resultadoQuery.results.bindings.Count > 0)
                {
                    offset += limit;

                    foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQuery.results.bindings)
                    {
                        idsRecursos.Add(fila["revista"].value);
                    }

                    if (resultadoQuery.results.bindings.Count < limit)
                    {
                        salirBucle = true;
                    }
                }
                else
                {
                    salirBucle = true;
                }

            } while (!salirBucle);

            return idsRecursos;
        }

        /// <summary>
        /// Obtiene los datos de las revistas por el id del recurso.
        /// </summary>
        /// <param name="pListaRevistasIds"></param>
        /// <returns></returns>
        private static Dictionary<string, Journal> ObtenerRevistaPorID(List<string> pListaRevistasIds)
        {
            Dictionary<string, Journal> dicResultado = new Dictionary<string, Journal>();
            List<List<string>> listaSplit = SplitList(pListaRevistasIds, 1000).ToList();

            #region --- MainDocument
            foreach (List<string> listaSpliteada in listaSplit)
            {
                int limit = 10000;
                int offset = 0;
                bool salirBucle = false;

                while(true)
                {
                    // Consulta sparql.
                    string selectMainDocument = $@"SELECT * WHERE {{ SELECT ?revista ?titulo ?issn ?eissn ?editor";
                    string whereMainDocument = $@"WHERE {{
                                ?revista a <http://w3id.org/roh/MainDocument>. 
                                ?revista <http://w3id.org/roh/format> <{mResourceApi.GraphsUrl}items/documentformat_057>. 
                                ?revista <http://w3id.org/roh/title> ?titulo. 
                                OPTIONAL{{?revista <http://purl.org/ontology/bibo/issn> ?issn. }} 
                                OPTIONAL{{?revista <http://purl.org/ontology/bibo/eissn> ?eissn. }} 
                                OPTIONAL{{?revista <http://purl.org/ontology/bibo/editor> ?editor. }}                     
                                FILTER(?revista IN (<{string.Join(">,<", listaSpliteada)}>)) 
                                }} ORDER BY DESC(?revista) }} LIMIT {limit} OFFSET {offset} ";

                    SparqlObject resultadoQueryMainDocument = mResourceApi.VirtuosoQueryMultipleGraph(selectMainDocument, whereMainDocument, new List<string>() { "maindocument", "documentformat", "referencesource", "impactindexcategory" });
                    if (resultadoQueryMainDocument != null && resultadoQueryMainDocument.results != null &&
                        resultadoQueryMainDocument.results.bindings != null && resultadoQueryMainDocument.results.bindings.Count > 0)
                    {
                        offset += limit;

                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryMainDocument.results.bindings)
                        {
                            // Valores.
                            string revistaId = fila["revista"].value;
                            string titulo = fila["titulo"].value;
                            string issn = null;
                            string eissn = null;
                            string editor = null;
                            if (fila.ContainsKey("issn"))
                            {
                                issn = fila["issn"].value;
                            }
                            if (fila.ContainsKey("eissn"))
                            {
                                eissn = fila["eissn"].value;
                            }
                            if (fila.ContainsKey("editor"))
                            {
                                editor = fila["editor"].value;
                            }

                            // Creación del objeto.
                            Journal revista = new Journal();
                            revista.idJournal = revistaId;
                            revista.titulo = titulo;
                            revista.issn = issn;
                            revista.eissn = eissn;
                            revista.publicador = editor;
                            revista.indicesImpacto = new HashSet<IndiceImpacto>();
                            dicResultado.Add(revista.idJournal, revista);
                        }

                        if (resultadoQueryMainDocument.results.bindings.Count < limit)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
            #endregion

            #region --- ImpactIndex
            foreach (List<string> listaSpliteada in listaSplit)
            {
                int limit = 10000;
                int offset = 0;
                bool salirBucle = false;

                // ImpactIndex
                while(true)
                {
                    // Consulta sparql.
                    string selectImpactIndex = $@"SELECT * WHERE {{ SELECT ?revista ?impactIndex ?fuente ?year ?impactIndexInYear ";
                    string whereImpactIndex = $@"WHERE {{
                                ?revista a <http://w3id.org/roh/MainDocument>.
                                ?revista <http://w3id.org/roh/format> <{mResourceApi.GraphsUrl}items/documentformat_057>. 
                                ?revista <http://w3id.org/roh/impactIndex> ?impactIndex.
                                OPTIONAL{{?impactIndex <http://w3id.org/roh/impactSource> ?fuente. }} 
                                ?impactIndex <http://w3id.org/roh/year> ?year.
                                ?impactIndex <http://w3id.org/roh/impactIndexInYear> ?impactIndexInYear.    
                                FILTER(?revista IN (<{string.Join(">,<", listaSpliteada)}>)) 
                                }} ORDER BY DESC(?revista) DESC(?impactIndex) }} LIMIT {limit} OFFSET {offset} ";

                    SparqlObject resultadoQueryImpactIndex = mResourceApi.VirtuosoQueryMultipleGraph(selectImpactIndex, whereImpactIndex, new List<string>() { "maindocument", "documentformat", "referencesource", "impactindexcategory" });
                    if (resultadoQueryImpactIndex != null && resultadoQueryImpactIndex.results != null &&
                        resultadoQueryImpactIndex.results.bindings != null && resultadoQueryImpactIndex.results.bindings.Count > 0)
                    {
                        offset += limit;

                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryImpactIndex.results.bindings)
                        {
                            string revistaId = fila["revista"].value;
                            string impactIndexId = fila["impactIndex"].value;
                            int year = Int32.Parse(fila["year"].value);
                            float impactIndexInYear = float.Parse(fila["impactIndexInYear"].value.Replace(",", "."), CultureInfo.InvariantCulture);
                            string fuente = null;

                            if (fila.ContainsKey("fuente"))
                            {
                                switch (fila["fuente"].value)
                                {
                                    case "http://gnoss.com/items/referencesource_000":
                                        fuente = "wos";
                                        break;
                                    case "http://gnoss.com/items/referencesource_010":
                                        fuente = "scopus";
                                        break;
                                    case "http://gnoss.com/items/referencesource_020":
                                        fuente = "inrecs";
                                        break;
                                }
                            }

                            dicResultado[revistaId].indicesImpacto.Add(new IndiceImpacto()
                            {
                                idImpactIndex = impactIndexId,
                                categorias = new HashSet<Categoria>(),
                                fuente = fuente,
                                indiceImpacto = impactIndexInYear,
                                anyo = year
                            });
                        }

                        if (resultadoQueryImpactIndex.results.bindings.Count < limit)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                } 
            }
            #endregion

            #region --- ImpactCategory
            foreach (List<string> listaSpliteada in listaSplit)
            {
                int limit = 10000;
                int offset = 0;

                // ImpactCategory
                while(true)
                {
                    // Consulta sparql.
                    string selectImpactCategory = $@"SELECT * WHERE {{ SELECT ?revista ?impactIndex ?impactCategory ?year ?nombreCategoria ?posicion ?numCategoria ?cuartil ";
                    string whereImpactCategory = $@"WHERE {{
                                ?revista a <http://w3id.org/roh/MainDocument>.
                                ?revista <http://w3id.org/roh/format> <{mResourceApi.GraphsUrl}items/documentformat_057>. 
                                ?revista <http://w3id.org/roh/impactIndex> ?impactIndex.
                                ?impactIndex <http://w3id.org/roh/year> ?year.
                                ?impactIndex <http://w3id.org/roh/impactCategory> ?impactCategory. 
                                ?impactCategory <http://w3id.org/roh/quartile> ?cuartil. 
                                ?impactCategory <http://w3id.org/roh/impactIndexCategory> ?categoria.
                                ?categoria <http://purl.org/dc/elements/1.1/title> ?nombreCategoria. 
                                FILTER (LANG(?nombreCategoria) = 'es')
                                OPTIONAL{{?impactCategory <http://w3id.org/roh/publicationPosition> ?posicion. }} 
                                OPTIONAL{{?impactCategory <http://w3id.org/roh/journalNumberInCat> ?numCategoria. }} 
                                FILTER(?revista IN (<{string.Join(">,<", listaSpliteada)}>)) 
                                }} ORDER BY DESC(?revista) DESC(?impactIndex) DESC(?impactCategory) }} LIMIT {limit} OFFSET {offset} ";

                    SparqlObject resultadoQueryImpactCategory = mResourceApi.VirtuosoQueryMultipleGraph(selectImpactCategory, whereImpactCategory, new List<string>() { "maindocument", "documentformat", "referencesource", "impactindexcategory" });
                    if (resultadoQueryImpactCategory != null && resultadoQueryImpactCategory.results != null &&
                        resultadoQueryImpactCategory.results.bindings != null && resultadoQueryImpactCategory.results.bindings.Count > 0)
                    {
                        offset += limit;

                        foreach (Dictionary<string, SparqlObject.Data> fila in resultadoQueryImpactCategory.results.bindings)
                        {
                            string revistaId = fila["revista"].value;
                            string impactIndexId = fila["impactIndex"].value;
                            string impactCategoryId = fila["impactIndex"].value;
                            int year = Int32.Parse(fila["year"].value);
                            string nombreCategoria = fila["nombreCategoria"].value;
                            int cuartil = Int32.Parse(fila["cuartil"].value);
                            int posicion = 0;
                            int numCategoria = 0;
                            if (fila.ContainsKey("posicion"))
                            {
                                posicion = Int32.Parse(fila["posicion"].value);
                            }
                            if (fila.ContainsKey("numCategoria"))
                            {
                                numCategoria = Int32.Parse(fila["numCategoria"].value);
                            }

                            dicResultado[revistaId].indicesImpacto.First(x => x.idImpactIndex == impactIndexId).categorias.Add(new Categoria()
                            {
                                idImpactCategory = impactCategoryId,
                                nomCategoria = nombreCategoria,
                                cuartil = cuartil,
                                posicionPublicacion = posicion,
                                numCategoria = numCategoria,
                                anyo = year
                            });

                        }

                        if (resultadoQueryImpactCategory.results.bindings.Count < limit)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }
            #endregion

            return dicResultado;
        }

        /// <summary>
        /// Método para dividir listas
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pItems">Listado</param>
        /// <param name="pSize">Tamaño</param>
        /// <returns></returns>
        private static IEnumerable<List<T>> SplitList<T>(List<T> pItems, int pSize)
        {
            for (int i = 0; i < pItems.Count; i += pSize)
            {
                yield return pItems.GetRange(i, Math.Min(pSize, pItems.Count - i));
            }
        }

        /// <summary>
        /// Comprueba que el Excel genérico esté bien formado.
        /// </summary>
        /// <param name="pDataSet">Dataset.</param>
        /// <param name="pNombreHoja">Nombre de la hoja.</param>
        /// <returns></returns>
        private static bool ComprobarColumnasExcel(DataSet pDataSet, string pNombreHoja)
        {
            DataTable tabla = pDataSet.Tables[$@"{pNombreHoja}"];
            if (tabla == null)
            {
                Console.WriteLine($@"{DateTime.Now} Hoja excel inválida. El excel de no contiene el título '{pNombreHoja}'.");
                return false;
            }

            // Comprobación de los nombres de las columnas. 
            if (!tabla.Columns.Contains("TITLE"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'TITLE'.");
                return false;
            }

            if (!tabla.Columns.Contains("PUBLISHER_NAME"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'PUBLISHER_NAME'.");
                return false;
            }

            if (!tabla.Columns.Contains("ISSN"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'ISSN'.");
                return false;
            }

            if (!tabla.Columns.Contains("EISSN"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'EISSN'.");
                return false;
            }

            if (!tabla.Columns.Contains("IMPACT_FACTOR"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'IMPACT_FACTOR'.");
                return false;
            }

            if (!tabla.Columns.Contains("CATEGORY_DESCRIPTION"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'CATEGORY_DESCRIPTION'.");
                return false;
            }

            if (!tabla.Columns.Contains("RANK"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'RANK'.");
                return false;
            }

            if (!tabla.Columns.Contains("RANK_OUT_OF"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'RANK_OUT_OF'.");
                return false;
            }

            if (!tabla.Columns.Contains("QUARTILE_RANK"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'QUARTILE_RANK'.");
                return false;
            }

            if (!tabla.Columns.Contains("SOURCE"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'SOURCE'.");
                return false;
            }

            if (!tabla.Columns.Contains("YEAR"))
            {
                Console.WriteLine($@"{DateTime.Now} Revista inválida. No contiene la columna 'YEAR'.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Lectura del excel.
        /// </summary>
        /// <param name="pRuta">Ruta del fichero.</param>
        /// <returns></returns>
        private static DataSet LecturaExcel(string pRuta)
        {
            DataSet dataSet = new DataSet();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(pRuta, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true,
                        }
                    });
                }
            }

            return dataSet;
        }

        /// <summary>
        /// Permite crear el objeto a cargar de las revistas.
        /// </summary>
        /// <param name="pListaRevistas">Listado de revistas a cargar.</param>
        /// <param name="pListaRecursos">Listado de recursos a cargar.</param>
        /// <returns></returns>
        private static void ObtenerRevistas(List<Journal> pListaRevistas, List<ComplexOntologyResource> pListaRecursos)
        {
            int numRevista = 0;
            int numRevistas = pListaRevistas.Count;
            Console.Write($"4/7.- Creando objetos de carga {0}/{numRevistas}");
            foreach (Journal revista in pListaRevistas)
            {
                numRevista++;
                Console.Write($"\r4/7.- Creando objetos de carga {numRevista}/{numRevistas}");
                MaindocumentOntology.MainDocument revistaCargar = new MaindocumentOntology.MainDocument();
                revistaCargar.Roh_title = revista.titulo;
                revistaCargar.Bibo_issn = revista.issn;
                revistaCargar.Bibo_eissn = revista.eissn;
                revistaCargar.Bibo_editor = revista.publicador;
                revistaCargar.IdRoh_format = $@"{mResourceApi.GraphsUrl}items/documentformat_057";
                revistaCargar.Roh_impactIndex = new List<MaindocumentOntology.ImpactIndex>();
                foreach (IndiceImpacto indice in revista.indicesImpacto)
                {
                    MaindocumentOntology.ImpactIndex indiceCargar = new MaindocumentOntology.ImpactIndex();
                    switch (indice.fuente)
                    {
                        case "wos":
                            indiceCargar.IdRoh_impactSource = $@"{mResourceApi.GraphsUrl}items/referencesource_000";
                            break;
                        case "scopus":
                            indiceCargar.IdRoh_impactSource = $@"{mResourceApi.GraphsUrl}items/referencesource_010";
                            break;
                        case "inrecs":
                            indiceCargar.IdRoh_impactSource = $@"{mResourceApi.GraphsUrl}items/referencesource_020";
                            break;
                    }
                    indiceCargar.Roh_impactIndexInYear = indice.indiceImpacto;
                    indiceCargar.Roh_year = indice.anyo;

                    indiceCargar.Roh_impactCategory = new List<MaindocumentOntology.ImpactCategory>();
                    foreach (Categoria categoria in indice.categorias)
                    {
                        MaindocumentOntology.ImpactCategory categoriaCargar = new MaindocumentOntology.ImpactCategory();
                        categoriaCargar.Roh_title = categoria.nomCategoria;
                        categoriaCargar.Roh_publicationPosition = categoria.posicionPublicacion;
                        categoriaCargar.Roh_journalNumberInCat = categoria.numCategoria;
                        categoriaCargar.Roh_quartile = categoria.cuartil;
                        indiceCargar.Roh_impactCategory.Add(categoriaCargar);
                    }

                    revistaCargar.Roh_impactIndex.Add(indiceCargar);
                }

                //Creamos el recurso.
                ComplexOntologyResource resource = revistaCargar.ToGnossApiResource(mResourceApi, null);
                pListaRecursos.Add(resource);
            }
            Console.WriteLine($"\r4/7.- Creando objetos de carga {numRevista}/{numRevistas}");
        }

        /// <summary>
        /// Permite cargar los recursos.
        /// </summary>
        /// <param name="pListaRecursosCargar">Lista de recursos a cargar.</param>
        private static void CargarDatos(List<ComplexOntologyResource> pListaRecursosCargar)
        {
            // Carga.
            int numRevista = 0;
            int numRevistas = pListaRecursosCargar.Count;
            Console.Write($"5/7.- Cargando revistas {0}/{numRevistas}");
            foreach (ComplexOntologyResource recursoCargar in pListaRecursosCargar)
            {
                numRevista++;
                Console.Write($"\r5/7.- Cargando revistas {numRevista}/{numRevistas}");
                int numIntentos = 0;
                while (!recursoCargar.Uploaded)
                {
                    numIntentos++;

                    if (numIntentos > MAX_INTENTOS)
                    {
                        break;
                    }

                    if (pListaRecursosCargar.Last() == recursoCargar)
                    {
                        mResourceApi.LoadComplexSemanticResource(recursoCargar, false, true);
                    }
                    else
                    {
                        mResourceApi.LoadComplexSemanticResource(recursoCargar);
                    }

                    if (!recursoCargar.Uploaded)
                    {
                        Thread.Sleep(1000 * numIntentos);
                    }
                }
            }

            Console.WriteLine($"\r5/7.- Cargando revistas {numRevista}/{numRevistas}");
        }

        /// <summary>
        /// Permite modificar los recursos.
        /// </summary>
        /// <param name="pListaRecursosModificar">Lista de recursos a modificar.</param>
        private static void ModificarDatos(List<ComplexOntologyResource> pListaRecursosModificar)
        {
            // Modificación.
            Parallel.ForEach(pListaRecursosModificar, new ParallelOptions { MaxDegreeOfParallelism = NUM_HILOS }, recursoModificar =>
            {
                int numIntentos = 0;
                while (!recursoModificar.Modified)
                {
                    numIntentos++;

                    if (numIntentos > MAX_INTENTOS)
                    {
                        break;
                    }

                    if (pListaRecursosModificar.Last() == recursoModificar)
                    {
                        mResourceApi.ModifyComplexOntologyResource(recursoModificar, false, true);
                    }
                    else
                    {
                        mResourceApi.ModifyComplexOntologyResource(recursoModificar, false, false);
                    }

                    if (!recursoModificar.Modified)
                    {
                        Thread.Sleep(1000 * numIntentos);
                    }
                }
            });
        }
    }
}
