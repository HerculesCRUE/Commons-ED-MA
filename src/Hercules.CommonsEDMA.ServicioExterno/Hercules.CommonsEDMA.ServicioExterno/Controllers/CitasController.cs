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
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class CitasController : ControllerBase
    {
        /// <summary>
        /// Este método obtiene un archivo en función de la publicación y formato de cita indicado.
        /// </summary>
        /// <param name="pIdRecurso"></param>
        /// <param name="pFormato"></param>
        /// <returns></returns>
        [HttpGet("GetQuoteDownload")]
        public ActionResult GetQuoteDownload(string pIdRecurso, string pFormato)
        {
            AccionesCitas accionesCitas = new();
            try
            {
                Quote quote = accionesCitas.GetQuote(pIdRecurso);
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
        ///     - APA: autor, autor y autor. (año). título. revista, volumen. doi
        ///     - BibTeX: @article{autor_autor_año, title={titulo}, volume={volumen}, ISSN={issn}, DOI={doi}, journal={revista}, publisher={editorial}, author={autor and autor and autor}, year={año} }
        ///     - Chicago: autor, autor and autor. “titulo” revista volumen (año). doi:doi.
        ///     - CSE: 1. autor, autor, autor, titulo volumen (año), doi:doi
        ///     - IEEE: [1]autor, autor and autor “titulo”, revista, vol. volumen, año.
        ///     - MLA: autor et al. “titulo” revista volumen (año): paginaInicial-paginaFinal
        ///     
        /// </summary>
        /// <param name="pIdRecurso"></param>
        /// <param name="pFormato"></param>
        /// <returns>String</returns>
        [HttpGet("GetQuoteText")]
        public string GetQuoteText(string pIdRecurso, string pFormato)
        {
            AccionesCitas accionesCitas = new();
            try
            {
                Quote quote = accionesCitas.GetQuote(pIdRecurso);

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
    }
}
