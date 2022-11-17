using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.GraficaBarras
{
    public class GraficaSectores
    {
        public string type
        {
            get
            {
                return "pie";
            }
        }
        public DataGraficaSectores data { get; set; }
        public GraficaSectores(DataGraficaSectores pData)
        {
            this.data = pData;
        }
    }

    public class DataGraficaSectores
    {
        public List<string> labels { get; set; }
        public List<DatosSector> datasets { get; set; }
        public DataGraficaSectores(List<string> pLabels, List<DatosSector> pDatasets)
        {
            this.labels = pLabels;
            this.datasets = pDatasets;
        }
    }

    public class DatosSector
    {
        public List<int> data { get; set; }
        public List<string> backgroundColor { get; set; }
        public List<string> borderColor { get; set; }
        public int borderWidth
        {
            get
            {
                return 1;
            }
        }
        public DatosSector(List<string> pBackgroundColor, List<string> pBorderColor, List<int> pData)
        {
            this.backgroundColor = pBackgroundColor;
            this.borderColor = pBorderColor;
            this.data = pData;
        }
    }
}
