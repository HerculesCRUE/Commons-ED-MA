using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaPublicaciones
{
    public class DataGraficaPublicaciones
    {
        public string type { get; set; }
        public Data data { get; set; }
        public Options options { get; set; }
        public DataGraficaPublicaciones(string pType, Data pData, Options pOptions)
        {
            this.type = pType;
            this.data = pData;
            this.options = pOptions;
        }
    }
    public class Data
    {
        public List<string> labels { get; set; }
        public List<Datasets> datasets { get; set; }
        public Data(List<string> pLabels, List<Datasets> pDatasets)
        {
            this.labels = pLabels;
            this.datasets = pDatasets;
        }
    }
    public class Datasets
    {
        public string label { get; set; }
        public string type { get; set; }
        public string stack { get; set; }
        public float maxBarThickness { get; set; }
        public List<int> data { get; set; }
        public List<string> backgroundColor { get; set; }
        public List<string> borderColor { get; set; }
        public int borderWidth { get; set; }
        public string yAxisID { get; set; }
        public Datasets(string pLabel, List<int> pData, List<string> pBackgroundColor, List<string> pBorderColor, int pBorderWidth, string pStack = null, string pType = "bar", string pYAxisID = null)
        {
            this.label = pLabel;
            this.data = pData;
            this.backgroundColor = pBackgroundColor;
            this.maxBarThickness = 100;
            this.borderColor = pBorderColor;
            this.borderWidth = pBorderWidth;
            this.stack = pStack;
            this.type = pType;
            this.yAxisID = pYAxisID;
        }
    }
    public class Options
    {
        public Scales scales { get; set; }
        public Plugins plugins { get; set; }
        public Options(Scales pScales, Plugins pPlugins)
        {
            this.scales = pScales;
            this.plugins = pPlugins;
        }
    }
    public class Scales
    {
        public Y y { get; set; }
        public Scales(Y pY)
        {
            this.y = pY;
        }
    }
    public class Y
    {
        public bool beginAtZero { get; set; }
        public Y(bool pBeginAtZero)
        {
            this.beginAtZero = pBeginAtZero;
        }
    }
    public class Plugins
    {
        public Title title { get; set; }
        public Legend legend { get; set; }
        public Plugins(Title pTitle, Legend pLegend)
        {
            this.title = pTitle;
            this.legend = pLegend;
        }
    }
    public class Title
    {
        public bool display { get; set; }
        public string text { get; set; }
        public Title(bool pDisplay, string pText)
        {
            this.display = pDisplay;
            this.text = pText;
        }
    }
    public class Legend
    {
        public Labels labels { get; set; }
        public string position { get; set; }
        public string align { get; set; }
        public Legend(Labels pLabels, string pPosition, string pAlign)
        {
            this.labels = pLabels;
            this.position = pPosition;
            this.align = pAlign;
        }
    }
    public class Labels
    {
        public bool usePointStyle { get; set; }
        public Labels(bool pUsePointStyle)
        {
            this.usePointStyle = pUsePointStyle;
        }
    }
}
