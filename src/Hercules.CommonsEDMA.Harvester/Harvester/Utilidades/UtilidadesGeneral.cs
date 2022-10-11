using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Utilidades
{
    class UtilidadesGeneral
    {
        public static Dictionary<string, string> dicPaises = new Dictionary<string, string>();
        public static Dictionary<string, string> dicRegiones = new Dictionary<string, string>();

        private static string RUTA_PAISES = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Utilidades/paises.json";
        private static string RUTA_REGIONES = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Utilidades/regiones.json";

        public static void IniciadorDiccionarioPaises()
        {
            List<Pais> listaPaises = JsonConvert.DeserializeObject<List<Pais>>(File.ReadAllText(RUTA_PAISES));
            foreach (Pais pais in listaPaises)
            {
                dicPaises.Add(pais.country_code, pais.name);
            }
        }
        public static void IniciadorDiccionarioRegion()
        {
            List<Region> listaRegiones = JsonConvert.DeserializeObject<List<Region>>(File.ReadAllText(RUTA_REGIONES));
            foreach (Region region in listaRegiones)
            {
                dicRegiones.Add(region.ID, region.ID_CVN);
            }
        }

        public static bool DicPaisesContienePais(string pais)
        {
            return dicPaises.ContainsKey(pais);
        }
        public static bool DicRegionesContieneRegion(string region)
        {
            return dicRegiones.ContainsKey(region);
        }

    }

    class Region
    {
        public string ID { get; set; }
        public string ID_CVN { get; set; }
        string NOMBRE { get; set; }
        string PAIS_ID { get; set; }
        string NOM_PAIS { get; set; }
        string ISO_31661 { get; set; }
        string ISO_31661_A2 { get; set; }
        string ISO_31661_A3 { get; set; }
    }

    class Pais
    {
        public string name { get; set; }
        string alpha_2 { get; set; }
        string alpha_3 { get; set; }
        public string country_code { get; set; }
        string iso_3166_2 { get; set; }
        string region { get; set; }
        string sub_region { get; set; }
        string intermediate_region { get; set; }
        string region_code { get; set; }
        string sub_region_code { get; set; }
        string intermediate_region_code { get; set; }
        Pais()
        {
            name = "";
            alpha_2 = "";
            alpha_3 = "";
            country_code = "";
            iso_3166_2 = "";
            region = "";
            sub_region = "";
            intermediate_region = "";
            region_code = "";
            sub_region_code = "";
            intermediate_region_code = "";
        }
    }
}
