using System;

namespace Harvester.Models.ModelsBBDD
{
    public class CiclosBBDD : AcademicDegreeBBDD
    {
        public DateTime? fechaTitulacion { get; set; }
        public string titulacionUni { get; set; }
        public string tituloExtranjero { get; set; }
        public string tituloHomologado { get; set; }
        public DateTime? fechaHomologacion { get; set; }
        public string notaMedia { get; set; }
        public string premio { get; set; }
    }
}
