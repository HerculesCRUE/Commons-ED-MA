using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.ServicioExterno.Models
{
    public class DataQueryRelaciones
    {
        public string nombreRelacion { get; set; }
        public List<Datos> idRelacionados { get; set; }
        public DataQueryRelaciones()
        { }
        public DataQueryRelaciones(string pNombreRelacion, List<Datos> pIdRelacionados)
        {
            nombreRelacion = pNombreRelacion;
            idRelacionados = pIdRelacionados;
        }
    }
    public class Datos
    {
        public string idRelacionado { get; set; }
        public int numVeces { get; set; }
        public Datos()
        { }
        public Datos(string pIdRelacionado, int pNumVeces)
        {
            idRelacionado = pIdRelacionado;
            numVeces = pNumVeces;
        }
    }
}
