using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Hercules.CommonsEDMA.ServicioExterno.Controllers.Utilidades;
using AnnotationOntology;
using Hercules.CommonsEDMA.ServicioExterno.Models.Quote;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones
{
    public class AccionesCitas : GnossGetMainResourceApiDataBase
    {
        /// <summary>
        /// Este método obtiene el texto de la cita desde la base de datos para llamarse desde otros métodos de la clase
        /// 
        /// </summary>
        /// <param name="pIdRecurso">Id del recurso</param>
        /// <returns>Objeto Quote</returns>
        public Quote GetQuote(string pIdRecurso)
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
