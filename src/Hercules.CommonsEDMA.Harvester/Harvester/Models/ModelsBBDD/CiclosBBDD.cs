using System;

namespace Harvester.Models.ModelsBBDD
{
    public class CiclosBBDD : AcademicDegreeBBDD
    {
        public DateTime? fechaTitulacion { get; set; }
        public string titulacionUni { get; set; }
        public string titulacionUniOtros { get; set; }
        public string tituloExtranjero { get; set; }
        public bool tituloHomologado { get; set; }
        public DateTime? fechaHomologacion { get; set; }
        public string notaMedia { get; set; }
        public string premio { get; set; }
        public string premioOther { get; set; }
    }
}
