using System;

namespace Harvester.Models.ModelsBBDD
{
    public class PosgradoBBDD:AcademicDegreeBBDD
    {
        public DateTime? fechaTitulacion { get; set; }
        public string tipoFormacion { get; set; }
        public string calificacionObtenida { get; set; }
        public bool tituloHomologado { get; set; }
        public DateTime? fechaHomologacion { get; set; }
    }
}
