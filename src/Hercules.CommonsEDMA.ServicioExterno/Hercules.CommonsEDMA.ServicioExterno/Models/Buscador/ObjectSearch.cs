using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Buscador
{
    public abstract class ObjectSearch
    {
        public class Property
        {
            public Property(HashSet<string> pTexts,int pScore,ObjectSearch pOwner)
            {
                texts = pTexts;
                score = pScore;
                owner = pOwner;
            }

            public HashSet<string> texts { get; set; }
            public int score { get; set; }
            public ObjectSearch owner{ get; set; }
        }

        public string title { get; set; }
        public string url { get; set; }
        public Guid id { get; set; }

        public string order { get; set; }

        public List<Property> properties { get; set; }

        ///// <summary>
        ///// Método de búsqueda para el autocompletar
        ///// </summary>
        ///// <param name="pInput">Texto a buscar (excepto la última palabra)</param>
        ///// <param name="pLastInput">Última palabra del texto a buscar</param>
        ///// <returns>Peso</returns>
        //public abstract long SearchAutocompletar(HashSet<string> pInput, string pLastInput);

        ///// <summary>
        ///// Método de búsqueda para determinar si la búsqueda ofrecería resultados
        ///// </summary>
        ///// <param name="pInput">Texto a buscar (excepto la última palabra)</param>
        ///// <param name="pLastInput">Última palabra del texto a buscar</param>
        ///// <returns>Peso</returns>
        //public abstract bool SearchBuscador(HashSet<string> pInput, string pLastInput);


        //public bool SearchForAutocomplete(HashSet<string> pText, HashSet<string> pInput, string pLastInput)
        //{
        //    return pText.IsSupersetOf(pInput) && pText.Any(x => x.StartsWith(pLastInput));
        //}

        //public bool SearchForSearcher(HashSet<string> pText, HashSet<string> pInput, string pLastInput)
        //{
        //    if (string.IsNullOrEmpty(pLastInput))
        //    {
        //        return pText.IsSupersetOf(pInput);
        //    }
        //    return pText.IsSupersetOf(pInput) && pText.Contains(pLastInput);
        //}
    }
}