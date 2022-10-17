using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.Organization;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgemp/empresas/modificadas-ids?q=fechaModificacion=ge=" + from);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (!String.IsNullOrEmpty(response.Content))
            {
                var sinComillas = response.Content[1..^1].Replace("\"", "");

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
            List<Thread> hilos = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgemp/empresas/" + identifier);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var json = JObject.Parse(response.Content);
            empresa = JsonConvert.DeserializeObject<Empresa>(json.ToString());
            DatosContacto datosContacto = null;
            hilos.Add(new Thread(() => datosContacto = GetDatosContacto(identifier, pConfig)));
            // Inicio hilos.
            foreach (Thread th in hilos)
            {
                th.Start();
            }

            // Espero a que estén listos.
            foreach (Thread th in hilos)
            {
                th.Join();
            }
            empresa.DatosContacto = datosContacto;
            return empresa;
        }

        private static DatosContacto GetDatosContacto(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            DatosContacto datosContacto = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgemp/datos-contacto/empresa/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string datosLimpios = response.Content;
            try
            {
                datosContacto = JsonConvert.DeserializeObject<DatosContacto>(datosLimpios);
            }
            catch (Exception)
            {
                return null;
            }
            return datosContacto;
        }

        private static EmpresaClasificacion GetEmpresaClasificacion(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            EmpresaClasificacion datos = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgemp/empresas-clasificaciones/empresa/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            string datosLimpios = response.Content;
            try
            {
                datos = JsonConvert.DeserializeObject<EmpresaClasificacion>(datosLimpios);
            }
            catch (Exception)
            {
                return null;
            }
            return datos;
        }

        private static List<TipoIdentificador> GetTiposIdentificadorFiscal(ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<TipoIdentificador> tiposIdentificador = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgemp/tipos-identificador");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                tiposIdentificador = JsonConvert.DeserializeObject<List<TipoIdentificador>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return tiposIdentificador;
        }

        private static DatosTipoEmpresa GetDatosTipoEmpresa(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            DatosTipoEmpresa datosTipoEmpresa = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgemp/datos-tipo-empresa/empresa/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                datosTipoEmpresa = JsonConvert.DeserializeObject<DatosTipoEmpresa>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return datosTipoEmpresa;
        }
    }
}
