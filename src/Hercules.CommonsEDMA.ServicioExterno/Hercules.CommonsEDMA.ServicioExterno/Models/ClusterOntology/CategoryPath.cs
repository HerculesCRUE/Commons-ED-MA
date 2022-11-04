using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gnoss.ApiWrapper.Model;
using GnossBase;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ClusterOntology
{
    [ExcludeFromCodeCoverage]
    public class CategoryPath : GnossOCBase
    {

        public CategoryPath() : base() { }
        public virtual string RdfType { get { return "http://w3id.org/roh/CategoryPath"; } }
        public virtual string RdfsLabel { get { return "http://w3id.org/roh/CategoryPath"; } }
        public OntologyEntity Entity { get; set; }

        [LABEL(LanguageEnum.es, "http://w3id.org/roh/categoryNode")]
        [RDFProperty("http://w3id.org/roh/categoryNode")]
        [MinLength(1)]
        public List<string> IdsRoh_categoryNode { get; set; }


        internal override void GetProperties()
        {
            base.GetProperties();
            propList.Add(new ListStringOntologyProperty("roh:categoryNode", this.IdsRoh_categoryNode));
        }

        protected List<object> ObtenerObjetosDePropiedad(object propiedad)
        {
            List<object> lista = new();
            if (propiedad is IList)
            {
                foreach (object item in (IList)propiedad)
                {
                    lista.Add(item);
                }
            }
            else
            {
                lista.Add(propiedad);
            }
            return lista;
        }
        protected List<string> ObtenerStringDePropiedad(object propiedad)
        {
            List<string> lista = new();
            if (propiedad is IList)
            {
                foreach (string item in (IList)propiedad)
                {
                    lista.Add(item);
                }
            }
            else if (propiedad is IDictionary)
            {
                foreach (object key in ((IDictionary)propiedad).Keys)
                {
                    if (((IDictionary)propiedad)[key] is IList)
                    {
                        List<string> listaValores = (List<string>)((IDictionary)propiedad)[key];
                        foreach (string valor in listaValores)
                        {
                            lista.Add(valor);
                        }
                    }
                    else
                    {
                        lista.Add((string)((IDictionary)propiedad)[key]);
                    }
                }
            }
            else if (propiedad is string)
            {
                lista.Add((string)propiedad);
            }
            return lista;
        }

        private static string GenerarTextoSinSaltoDeLinea(string pTexto)
        {
            return pTexto.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\"", "\\\"");
        }



        private static void AgregarTripleALista(string pSujeto, string pPredicado, string pObjeto, List<string> pLista, string pDatosExtra)
        {
            if (!string.IsNullOrEmpty(pObjeto) && !pObjeto.Equals("\"\"") && !pObjeto.Equals("<>"))
            {
                pLista.Add($"<{pSujeto}> <{pPredicado}> {pObjeto}{pDatosExtra}");
            }
        }

        private void AgregarTags(List<string> pListaTriples)
        {
            foreach (string tag in tagList)
            {
                AgregarTripleALista($"http://gnoss/{ResourceID.ToString().ToUpper()}", "http://rdfs.org/sioc/types#Tag", tag.ToLower(), pListaTriples, " . ");
            }
        }


    }
}
