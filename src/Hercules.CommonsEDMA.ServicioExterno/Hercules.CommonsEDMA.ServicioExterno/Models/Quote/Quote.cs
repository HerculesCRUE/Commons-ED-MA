using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Quote
{

    public class Quote
    {
        public string titulo { get; set; }
        public List<string> autores { get; set; }
        public string anio { get; set; }
        public string revista { get; set; }
        public string publisher { get; set; }
        public string issn { get; set; }
        public string volumen { get; set; }
        public string doi { get; set; }
        public string paginas { get; set; }
    }
}
