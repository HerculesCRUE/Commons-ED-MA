using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.Organization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAI_PMH.Services
{
    class Organization
    {
        public static Dictionary<string, DateTime> GetModifiedOrganizations(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetUrlBaseOrganizacion() + "empresas/modificadas-ids?q=fechaModificacion=ge=" + from);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (!String.IsNullOrEmpty(response.Content))
            {
                var sinComillas= response.Content[1..^1].Replace("\"", "");

                idList = sinComillas.Split(',').ToList();
                foreach (string id in idList)
                {
                    idDictionary.Add($"Organizacion_{id}", DateTime.UtcNow);
                }
            }
            return idDictionary;
        }

        public static Empresa GetEmpresa(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            string identifier = id.Split('_')[1];
            Empresa empresa = new();
            RestClient client = new(pConfig.GetUrlBaseOrganizacion() + "empresas/" + identifier);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var json = JObject.Parse(response.Content);
            empresa = JsonConvert.DeserializeObject<Empresa>(json.ToString());
            empresa.DatosContacto = GetDatosContacto(identifier, pConfig);
            return empresa;
        }

        private static DatosContacto GetDatosContacto(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            DatosContacto datosContacto = new();
            RestClient client = new(pConfig.GetUrlBaseOrganizacion() + "datos-contacto/empresa/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string datosLimpios = response.Content;
            datosContacto = JsonConvert.DeserializeObject<DatosContacto>(datosLimpios);
            return datosContacto;
        }

        private static EmpresaClasificacion GetEmpresaClasificacion(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            EmpresaClasificacion datos = new();
            RestClient client = new(pConfig.GetUrlBaseOrganizacion() + "empresas-clasificaciones/empresa/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string datosLimpios = response.Content;
            datos = JsonConvert.DeserializeObject<EmpresaClasificacion>(datosLimpios);
            return datos;
        }

        private static List<TipoIdentificador> GetTiposIdentificadorFiscal(ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<TipoIdentificador> tiposIdentificador = new();
            RestClient client = new(pConfig.GetUrlBaseOrganizacion() + "/tipos-identificador");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            tiposIdentificador = JsonConvert.DeserializeObject<List<TipoIdentificador>>(response.Content);
            return tiposIdentificador;
        }

        private static DatosTipoEmpresa GetDatosTipoEmpresa(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            DatosTipoEmpresa datosTipoEmpresa = new();
            RestClient client = new(pConfig.GetUrlBaseOrganizacion() + "datos-tipo-empresa/empresa/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            datosTipoEmpresa = JsonConvert.DeserializeObject<DatosTipoEmpresa>(response.Content);
            return datosTipoEmpresa;
        }
    }
}
