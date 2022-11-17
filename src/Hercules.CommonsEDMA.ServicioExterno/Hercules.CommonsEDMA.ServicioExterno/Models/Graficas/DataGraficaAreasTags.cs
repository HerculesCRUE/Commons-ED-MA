using System.Collections.Generic;

namespace Hercules.CommonsEDMA.ServicioExterno.Models.Graficas.DataGraficaAreasTags
{
    public class DataGraficaAreasTags
    {
        public string type { get; set; }
        public Data data { get; set; }
        public Options options { get; set; }
        public DataGraficaAreasTags(string pType, Data pData, Options pOptions)
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
        public List<double> data { get; set; }
        public List<string> backgroundColor { get; set; }
        public Datasets(List<double> pData, List<string> pBackgroundColor)
        {
            this.data = pData;
            this.backgroundColor = pBackgroundColor;
        }
    }
    public class Options
    {
        public string indexAxis { get; set; }
        public Plugins plugins { get; set; }
        public Scales scales { get; set; }
        public Options(string pIndexAxis, Plugins pPlugins, Scales pScales)
        {
            this.indexAxis = pIndexAxis;
            this.plugins = pPlugins;
            this.scales = pScales;
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
        public bool display { get; set; }
        public Legend(bool pDisplay)
        {
            this.display = pDisplay;
        }
    }
    public class Scales
    {
        public x x { get; set; }
        public Scales(x pY)
        {
            this.x = pY;
        }
    }
    public class x
    {
        public Ticks ticks { get; set; }
        public ScaleLabel scaleLabel { get; set; }
        public x(Ticks pTicks, ScaleLabel pScaleLabel)
        {
            this.ticks = pTicks;
            this.scaleLabel = pScaleLabel;
        }
    }
    public class Ticks
    {
        public int min { get; set; }
        public int max { get; set; }
        public Ticks(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }
    public class ScaleLabel
    {
        public bool display { get; set; }
        public string labelString { get; set; }
        public ScaleLabel(bool pDisplay, string pLabelString)
        {
            this.display = pDisplay;
            this.labelString = pLabelString;
        }
    }
}
