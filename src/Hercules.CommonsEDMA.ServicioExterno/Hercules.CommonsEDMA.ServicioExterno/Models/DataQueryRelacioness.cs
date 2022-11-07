using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models
{
    public class DataQueryRelaciones
    {
        public string nombreRelacion { get; set; }
        public List<Datos> idRelacionados { get; set; }
        public DataQueryRelaciones()
        { }
        public DataQueryRelaciones(string nombreRelacion)
        {
            this.nombreRelacion = nombreRelacion;
            this.idRelacionados = idRelacionados;
        }
        public DataQueryRelaciones(string nombreRelacion, List<Datos> idRelacionados)
        {
            this.nombreRelacion = nombreRelacion;
            this.idRelacionados = idRelacionados;
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
