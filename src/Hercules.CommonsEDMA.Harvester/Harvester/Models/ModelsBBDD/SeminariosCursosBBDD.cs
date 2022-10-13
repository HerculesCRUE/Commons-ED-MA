using System;

namespace Harvester.Models.ModelsBBDD
{
    public class SeminariosCursosBBDD:AcademicDegreeBBDD
    {

        public string objetivosCurso { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaFin { get; set; }
        public int duracionHoras { get; set; }
        public int duracionAnyos { get; set; }
        public int duracionMeses { get; set; }
        public int duracionDias { get; set; }
        public string objetivoEstancia { get; set; }
        public string programaFin { get; set; }
        public string perfilDestinatario { get; set; }
        public string tareasContrastables { get; set; }
    }
}
