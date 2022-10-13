using System;
using System.Collections.Generic;

namespace Harvester.Models.ModelsBBDD
{
    public class DoctoradosBBDD : AcademicDegreeBBDD
    {
        public DateTime? fechaTitulacion { get; set; }
        public string entidadTitDEA { get; set; }
        public string entidadTitDEATitulo { get; set; }
        public DateTime? obtencionDEA { get; set; }
        public string tituloTesis { get; set; }
        public string calificacionObtenida { get; set; }
        public string firmaDirector { get; set; }
        public string nombreDirector { get; set; }
        public string primApeDirector { get; set; }
        public string segunApeDirector { get; set; }
        public List<Codirector> codirectorTesis { get; set; }
        public bool doctoradoEuropeo { get; set; }
        public DateTime? fechaDoctorado { get; set; }
        public bool mencionCalidad { get; set; }
        public bool premioExtraordinarioDoctor { get; set; }
        public DateTime? fechaPremioDoctor { get; set; }
        public bool tituloHomologado { get; set; }
        public DateTime? fechaHomologado { get; set; }

        public class Codirector
        {
            public int comment { get; set; }
            public string nick { get; set; }
            public string firstName { get; set; }
            public string familyName { get; set; }
            public string secondFamilyName { get; set; }
        }
    }
}
