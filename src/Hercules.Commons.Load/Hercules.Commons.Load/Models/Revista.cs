using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.Commons.Load.Models
{
    public class Revista
    {
        public string fuente { get; set; }
        public string titulo { get; set; }
        public string issn { get; set; }
        public string eissn { get; set; }
        public string publicador { get; set; }
        public string categoria { get; set; }
        public int posicionPublicacion { get; set; }
        public int numCategoria { get; set; }
        public int cuartil { get; set; }
        public float indiceImpacto { get; set; }
        public DateTime anyo { get; set; }
    }
}
