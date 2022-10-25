using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using System.Text;
using System.IO;
using System.Threading;
using Hercules.CommonsEDMA.ServicioExterno.Models.Quote;

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
                Quote quote = GetQuote(pIdRecurso);
                string texto = "";
                switch (pFormato)
                {
                    case "BIBTEX":
                        texto = "@article{" + string.Join('_', quote.autores) + "_" + quote.anio + ", title={" + quote.titulo + "}, volume={" + quote.volumen + "}, ISSN={" + quote.issn + "}, DOI={" + quote.doi + "}, journal={" + quote.revista + "}, publisher={" + quote.publisher + "}, author={" + string.Join(" and ", quote.autores) + "}, year={" + quote.anio + "} }";
                        return File(Encoding.Latin1.GetBytes(texto), "application/BIB", "bibtex.bib");
                    case "REFMAN":
                        string textoPaginas = "";
                        if (!string.IsNullOrEmpty(quote.paginas))
                        {
                            textoPaginas = "\nSP  - " + quote.paginas.Split('-')[0] + "\nEP  - " + quote.paginas.Split('-')[1];
                        }
                        texto = "TY  - JOUR\nAU  - " + string.Join("\nAU  - ", quote.autores) + "\nDA  - " + quote.anio + "\nPY  - " + quote.anio + "\nDO  - " + quote.doi + "\nSN  - " + quote.issn + textoPaginas + "\nT2  - " + quote.revista + "\nT1  - " + quote.titulo + "\nVL  - " + quote.volumen + "\nER  - ";
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
        /// <returns>String</returns>
        [HttpGet("GetQuoteText")]
        public string GetQuoteText(string pIdRecurso, string pFormato)
        {
            try
            {
                Quote quote = GetQuote(pIdRecurso);

                string texto = "";
                string textoAutor = "";
                switch (pFormato)
                {
                    case "APA":
                        textoAutor = "";
                        for (int i = 0; i < quote.autores.Count; i++)
                        {
                            if (i > 0)
                            {
                                if (i == quote.autores.Count - 1)
                                {
                                    textoAutor += " & ";
                                }
                                else
                                {
                                    textoAutor += ", ";
                                }
                            }
                            textoAutor += quote.autores[i];
                        }
                        texto = $"{textoAutor}. ({quote.anio}). {quote.titulo}. {quote.revista}, {quote.volumen}. https://doi.org/{quote.doi}";
                        break;
                    case "BIBTEX":
                        texto = "@article{" + string.Join('_', quote.autores) + "_" + quote.anio + ", title={" + quote.titulo + "}, volume={" + quote.volumen + "}, ISSN={" + quote.issn + "}, DOI={" + quote.doi + "}, journal={" + quote.revista + "}, publisher={" + quote.publisher + "}, author={" + string.Join(" and ", quote.autores) + "}, year={" + quote.anio + "} }";
                        break;
                    case "CHICAGO":
                        textoAutor = "";
                        for (int i = 0; i < quote.autores.Count; i++)
                        {
                            if (i > 0)
                            {
                                if (i == quote.autores.Count - 1)
                                {
                                    textoAutor += " and ";
                                }
                                else
                                {
                                    textoAutor += ", ";
                                }
                            }
                            textoAutor += quote.autores[i];
                        }
                        texto = $"{textoAutor}. “{quote.titulo}” {quote.revista} {quote.volumen} ({quote.anio}). doi:{quote.doi}.";
                        break;
                    case "CSE":
                        texto = $"1. {string.Join(", ", quote.autores)}, {quote.titulo} {quote.volumen} ({quote.anio}), doi:{quote.doi}";
                        break;
                    case "IEEE":
                        textoAutor = "";
                        for (int i = 0; i < quote.autores.Count; i++)
                        {
                            if (i > 0)
                            {
                                if (i == quote.autores.Count - 1)
                                {
                                    textoAutor += " and ";
                                }
                                else
                                {
                                    textoAutor += ", ";
                                }
                            }
                            textoAutor += quote.autores[i];
                        }
                        texto = $"[1]{textoAutor}, “{quote.titulo},” {quote.revista}, vol. {quote.volumen}, {quote.anio}.";
                        break;
                    case "MLA":
                        if (!string.IsNullOrEmpty(quote.paginas))
                        {
                            quote.paginas = ": " + quote.paginas;
                        }
                        texto = $"{quote.autores[0]} et al. “{quote.titulo}” {quote.revista} {quote.volumen} ({quote.anio}){quote.paginas}";
                        break;
                }

                return texto;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Este método obtiene el texto de la cita desde la base de datos para llamarse desde otros métodos de la clase
        /// 
        /// </summary>
        /// <param name="pIdRecurso">Id del recurso</param>
        /// <returns>Objeto Quote</returns>
        private Quote GetQuote (string pIdRecurso)
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
            SparqlObject sparqlObject = resourceApi.VirtuosoQueryMultipleGraph(select, where, new List<string> { "document", "maindocument" });

            Quote quote = new();
            quote.autores = new();

            foreach (Dictionary<string, SparqlObject.Data> fila in sparqlObject.results.bindings)
            {
                if (quote.autores.Count == 0)
                {
                    // Título
                    quote.titulo = fila["titulo"].value;
                    // Año
                    if (fila.ContainsKey("anio"))
                    {
                        quote.anio = fila["anio"].value;
                    }
                    // Revista
                    if (fila.ContainsKey("revista"))
                    {
                        quote.revista = fila["revista"].value;
                    }
                    // Publisher
                    if (fila.ContainsKey("publisher"))
                    {
                        quote.publisher = fila["publisher"].value;
                    }
                    // ISSN
                    if (fila.ContainsKey("issn"))
                    {
                        quote.issn = fila["issn"].value;
                    }
                    // Volumen
                    if (fila.ContainsKey("volumen"))
                    {
                        quote.volumen = fila["volumen"].value;
                    }
                    // DOI
                    if (fila.ContainsKey("doi"))
                    {
                        quote.doi = fila["doi"].value;
                    }
                    // Pagina Inicio
                    if (fila.ContainsKey("paginaInicio") && fila.ContainsKey("paginaFin"))
                    {
                        quote.paginas = fila["paginaInicio"].value + "-" + fila["paginaFin"].value;
                    }
                }
                // Autores
                quote.autores.Add(fila["autores"].value);
            }

            return quote;
        }
    }
}
