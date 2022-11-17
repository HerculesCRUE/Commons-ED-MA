using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.GraficaBarras
{
    public class GraficaBarras
    {
        public string type
        {
            get
            {
                return "bar";
            }
        }
        public DataGraficaBarras data { get; set; }
        public GraficaBarras(DataGraficaBarras pData)
        {
            this.data = pData;
        }
    }

    public class DataGraficaBarras
    {
        public List<string> labels { get; set; }
        public List<DatosBarra> datasets { get; set; }
        public DataGraficaBarras(List<string> pLabels, List<DatosBarra> pDatasets)
        {
            this.labels = pLabels;
            this.datasets = pDatasets;
        }
    }

    public class DatosBarra
    {
        public string label { get; set; }
        public string backgroundColor { get; set; }
        public List<int> data { get; set; }
        public float barPercentage { get; set; }
        public float maxBarThickness { get; set; }
        public string stack { get; set; }
        public DatosBarra(string pLabel, string pBackgroundColor, List<int> pData, float pBarPercentage, string pStack)
        {
            this.label = pLabel;
            this.backgroundColor = pBackgroundColor;
            this.data = pData;
            this.barPercentage = pBarPercentage;
            this.maxBarThickness = 100;
            this.stack = pStack;
        }
    }
}
