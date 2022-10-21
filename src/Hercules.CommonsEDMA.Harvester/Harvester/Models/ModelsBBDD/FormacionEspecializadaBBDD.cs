using System;

namespace Harvester.Models.ModelsBBDD
{
    public class FormacionEspecializadaBBDD:AcademicDegreeBBDD
    {
        public DateTime? fechaFinalizacion { get; set; }
        public float duracionHoras { get; set; }
        public string tipoFormacion { get; set; }
        public string objetivosEntidad { get; set; }
        public string firma { get; set; }
        public string nombre { get; set; }
        public string primApe { get; set; }
        public string segunApe { get; set; }
    }
}
