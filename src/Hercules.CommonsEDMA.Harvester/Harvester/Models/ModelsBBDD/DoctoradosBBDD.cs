using System;
using System.Collections.Generic;

namespace Harvester.Models.ModelsBBDD
{
    public class DoctoradosBBDD:AcademicDegreeBBDD
    {

        public DateTime? fechaTitulacion { get; set; }
        public string entidadTitDEA { get; set; }
        public DateTime? obtencionDEA { get; set; }
        public string tituloTesis { get; set; }
        public string calificacionObtenida { get; set; }
        public string firmaDirector { get; set; }
        public string nombreDirector { get; set; }
        public string primApeDirector { get; set; }
        public string segunApeDirector { get; set; }
        public List<Codirector> codirectorTesis { get; set; }
        public string doctoradoEuropeo { get; set; }
        public string mencionCalidad { get; set; }
        public string premioExtraordinarioDoctor { get; set; }
        public string tituloHomologado { get; set; }

    }
}
