using System;
using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Buscador
{
    public abstract class ObjectSearch
    {
        public class Property
        {
            public Property(HashSet<string> pTexts, int pScore, ObjectSearch pOwner)
            {
                texts = pTexts;
                score = pScore;
                owner = pOwner;
            }

            public HashSet<string> texts { get; set; }
            public int score { get; set; }
            public ObjectSearch owner { get; set; }
        }

        public string title { get; set; }
        public string url { get; set; }
        public Guid id { get; set; }

        public string order { get; set; }

        public List<Property> properties { get; set; }

    }
}