using PersonOntology;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harvester.Models.ModelsBBDD
{
    public class MetricPageBBDD
    {
        public string title { get; set; }
        public int order { get; set; }
        public List<string> idsMetricGraphic { get; set; }
        public List<MetricGraphic> listaGraficas { get; set; }
    }
}
