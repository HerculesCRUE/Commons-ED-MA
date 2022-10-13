using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using System.Threading;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class CitasController : ControllerBase
    {
        #region --- Constantes   
        private static string RUTA_OAUTH = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config";
        private static ResourceApi mResourceAPI = null;
        #endregion

        private static ResourceApi resourceApi
        {
            get
            {
                while (mResourceAPI == null)
                {
                    try
                    {
                        mResourceAPI = new ResourceApi(RUTA_OAUTH);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("No se ha podido iniciar ResourceApi");
                        Console.WriteLine($"Contenido OAuth: {System.IO.File.ReadAllText(RUTA_OAUTH)}");
                        Thread.Sleep(10000);
                    }
                }
                return mResourceAPI;
            }
        }

        /// <summary>
        /// Este método obtiene un archivo en función de la publicación y formato de cita indicado.
        /// </summary>
        /// <param name="pIdRecurso"></param>
        /// <param name="pFormato"></param>
        /// <returns></returns>
        [HttpGet("GetQuoteDownload")]
        public ActionResult GetQuoteDownload(string pIdRecurso, string pFormato)
        {
            try
            {
                string select = "SELECT DISTINCT ?titulo ?autores ?anio ?revista ?publisher ?issn ?volumen ?doi ?paginaInicio ?paginaFin";
                string where =
                $@"WHERE {{
                ?s <http://w3id.org/roh/title> ?titulo FILTER(?s=<{pIdRecurso}>).
                ?s <http://purl.org/ontology/bibo/authorList> ?autoresAux.
                ?autoresAux <http://xmlns.com/foaf/0.1/nick> ?autores.
                OPTIONAL {{ ?s <http://w3id.org/roh/year> ?anio. }}
                OPTIONAL {{ ?s <http://w3id.org/roh/hasPublicationVenueJournalText> ?revista. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/publisher> ?publisher. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/issn> ?issn. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/volume> ?volumen. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/doi> ?doi. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/pageStart> ?paginaInicio. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/pageEnd> ?paginaFin. }}
            }}";
                SparqlObject sparqlObject = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "document" , "maindocument" });

                string titulo = string.Empty;
                List<string> autores = new List<string>();
                string anio = string.Empty;
                string revista = string.Empty;
                string publisher = string.Empty;
                string issn = string.Empty;
                string volumen = string.Empty;
                string doi = string.Empty;
                string paginas = string.Empty;
                foreach (Dictionary<string, SparqlObject.Data> fila in sparqlObject.results.bindings)
                {
                    if (autores.Count == 0)
                    {
                        // Título
                        titulo = fila["titulo"].value;
                        // Año
                        if (fila.ContainsKey("anio"))
                        {
                            anio = fila["anio"].value;
                        }
                        // Revista
                        if (fila.ContainsKey("revista"))
                        {
                            revista = fila["revista"].value;
                        }
                        // Publisher
                        if (fila.ContainsKey("publisher"))
                        {
                            publisher = fila["publisher"].value;
                        }
                        // ISSN
                        if (fila.ContainsKey("issn"))
                        {
                            issn = fila["issn"].value;
                        }
                        // Volumen
                        if (fila.ContainsKey("volumen"))
                        {
                            volumen = fila["volumen"].value;
                        }
                        // DOI
                        if (fila.ContainsKey("doi"))
                        {
                            doi = fila["doi"].value;
                        }
                        // Pagina Inicio
                        if (fila.ContainsKey("paginaInicio") && fila.ContainsKey("paginaFin"))
                        {
                            paginas = fila["paginaInicio"].value + "-" + fila["paginaFin"].value;
                        }
                    }
                    // Autores
                    autores.Add(fila["autores"].value);
                }
                string texto = "";
                switch (pFormato)
                {
                    case "BIBTEX":
                        texto = "@article{" + string.Join('_', autores) + "_" + anio + ", title={" + titulo + "}, volume={" + volumen + "}, ISSN={" + issn + "}, DOI={" + doi + "}, journal={" + revista + "}, publisher={" + publisher + "}, author={" + string.Join(" and ", autores) + "}, year={" + anio + "} }";
                        return File(Encoding.Latin1.GetBytes(texto), "application/BIB", "bibtex.bib");
                    case "REFMAN":
                        string textoPaginas = "";
                        if (!string.IsNullOrEmpty(paginas))
                        {
                            textoPaginas = "\nSP  - " + paginas.Split('-')[0] + "\nEP  - " + paginas.Split('-')[1];
                        }
                        texto = "TY  - JOUR\nAU  - " + string.Join("\nAU  - ", autores) + "\nDA  - " + anio + "\nPY  - " + anio + "\nDO  - " + doi + "\nSN  - " + issn + textoPaginas + "\nT2  - " + revista + "\nT1  - " + titulo + "\nVL  - " + volumen + "\nER  - ";
                        return File(Encoding.Latin1.GetBytes(texto), "application/RIS", "refman.ris");
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Este método obtiene el texto de la cita en función de la publicación y el formato de cita indicado.
        /// 
        /// Hay muchos estilos/formatos para citar y solo están contemplados 6.
        /// 
        ///     - APA: autor, autor & autor. (año). título. revista, volumen. doi
        ///     - BibTeX: @article{autor_autor_año, title={titulo}, volume={volumen}, ISSN={issn}, DOI={doi}, journal={revista}, publisher={editorial}, author={autor and autor and autor}, year={año} }
        ///     - Chicago: autor, autor and autor. “titulo” revista volumen (año). doi:doi.
        ///     - CSE: 1. autor, autor, autor, titulo volumen (año), doi:doi
        ///     - IEEE: [1]autor, autor and autor “titulo”, revista, vol. volumen, año.
        ///     - MLA: autor et al. “titulo” revista volumen (año): paginaInicial-paginaFinal
        ///     
        /// 
        /// 
        /// </summary>
        /// <param name="pIdRecurso"></param>
        /// <param name="pFormato"></param>
        /// <returns></returns>
        [HttpGet("GetQuoteText")]
        public string GetQuoteText(string pIdRecurso, string pFormato)
        {
            try
            {
                string select = "SELECT DISTINCT ?titulo ?autores ?anio ?revista ?publisher ?issn ?volumen ?doi ?paginaInicio ?paginaFin ";
                string where =
                $@"WHERE {{
                ?s <http://w3id.org/roh/title> ?titulo FILTER(?s=<{pIdRecurso}>).
                ?s <http://purl.org/ontology/bibo/authorList> ?autoresAux.
                ?autoresAux <http://xmlns.com/foaf/0.1/nick> ?autores.
                OPTIONAL {{ ?s <http://w3id.org/roh/year> ?anio. }}                
                OPTIONAL {{ ?s <http://w3id.org/roh/hasPublicationVenueJournalText> ?revista. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/publisher> ?publisher. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/issn> ?issn. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/volume> ?volumen. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/doi> ?doi. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/pageStart> ?paginaInicio. }}
                OPTIONAL {{ ?s <http://purl.org/ontology/bibo/pageEnd> ?paginaFin. }}
            }}";
                SparqlObject sparqlObject = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "document", "maindocument" });

                string titulo = string.Empty;
                List<string> autores = new List<string>();
                string anio = string.Empty;
                string revista = string.Empty;
                string publisher = string.Empty;
                string issn = string.Empty;
                string volumen = string.Empty;
                string doi = string.Empty;
                string paginas = string.Empty;
                foreach (Dictionary<string, SparqlObject.Data> fila in sparqlObject.results.bindings)
                {
                    if (autores.Count == 0)
                    {
                        // Título
                        titulo = fila["titulo"].value;
                        // Año
                        if (fila.ContainsKey("anio"))
                        {
                            anio = fila["anio"].value;
                        }
                        // Revista
                        if (fila.ContainsKey("revista"))
                        {
                            revista = fila["revista"].value;
                        }
                        // Publisher
                        if (fila.ContainsKey("publisher"))
                        {
                            publisher = fila["publisher"].value;
                        }
                        // ISSN
                        if (fila.ContainsKey("issn"))
                        {
                            issn = fila["issn"].value;
                        }
                        // Volumen
                        if (fila.ContainsKey("volumen"))
                        {
                            volumen = fila["volumen"].value;
                        }
                        // DOI
                        if (fila.ContainsKey("doi"))
                        {
                            doi = fila["doi"].value;
                        }
                        // Pagina Inicio
                        if (fila.ContainsKey("paginaInicio") && fila.ContainsKey("paginaFin"))
                        {
                            paginas = fila["paginaInicio"].value + "-" + fila["paginaFin"].value;
                        }
                    }
                    // Autores
                    autores.Add(fila["autores"].value);
                }
                string texto = "";
                string textoAutor = "";
                switch (pFormato)
                {
                    case "APA":
                        textoAutor = "";
                        for (int i = 0; i < autores.Count; i++)
                        {
                            if (i > 0)
                            {
                                if (i == autores.Count - 1)
                                {
                                    textoAutor += " & ";
                                }
                                else
                                {
                                    textoAutor += ", ";
                                }
                            }
                            textoAutor += autores[i];
                        }
                        texto = $"{textoAutor}. ({anio}). {titulo}. {revista}, {volumen}. https://doi.org/{doi}";
                        break;
                    case "BIBTEX":
                        texto = "@article{" + string.Join('_', autores) + "_" + anio + ", title={" + titulo + "}, volume={" + volumen + "}, ISSN={" + issn + "}, DOI={" + doi + "}, journal={" + revista + "}, publisher={" + publisher + "}, author={" + string.Join(" and ", autores) + "}, year={" + anio + "} }";
                        break;
                    case "CHICAGO":
                        textoAutor = "";
                        for (int i = 0; i < autores.Count; i++)
                        {
                            if (i > 0)
                            {
                                if (i == autores.Count - 1)
                                {
                                    textoAutor += " and ";
                                }
                                else
                                {
                                    textoAutor += ", ";
                                }
                            }
                            textoAutor += autores[i];
                        }
                        texto = $"{textoAutor}. “{titulo}” {revista} {volumen} ({anio}). doi:{doi}.";
                        break;
                    case "CSE":
                        texto = $"1. {string.Join(", ", autores)}, {titulo} {volumen} ({anio}), doi:{doi}";
                        break;
                    case "IEEE":
                        textoAutor = "";
                        for (int i = 0; i < autores.Count; i++)
                        {
                            if (i > 0)
                            {
                                if (i == autores.Count - 1)
                                {
                                    textoAutor += " and ";
                                }
                                else
                                {
                                    textoAutor += ", ";
                                }
                            }
                            textoAutor += autores[i];
                        }
                        texto = $"[1]{textoAutor}, “{titulo},” {revista}, vol. {volumen}, {anio}.";
                        break;
                    case "MLA":
                        if (!string.IsNullOrEmpty(paginas))
                        {
                            paginas = ": " + paginas;
                        }
                        texto = $"{autores[0]} et al. “{titulo}” {revista} {volumen} ({anio}){paginas}";
                        break;
                }

                return texto;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
