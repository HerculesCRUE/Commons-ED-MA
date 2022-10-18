namespace QuickType
{
    using Newtonsoft.Json;

    public partial class MainPaises
    {
        [JsonProperty("PAISES")]
        public Paises Paises { get; set; }
    }

    public partial class Paises
    {
        [JsonProperty("DATA_RECORD")]
        public DataRecord[] DataRecord { get; set; }
    }

    public partial class DataRecord
    {
        [JsonProperty("PAI_CODIGO")]
        public string PaiCodigo { get; set; }

        [JsonProperty("PAI_NOMBRE")]
        public string PaiNombre { get; set; }

        [JsonProperty("PAI_ISO31661_A3")]
        public string PaiIso31661A3 { get; set; }
    }
}