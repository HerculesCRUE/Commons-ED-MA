using Newtonsoft.Json;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.Project;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OAI_PMH.Services
{
    public class Project
    {
        public static Dictionary<string, DateTime> GetModifiedProjects(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request, true);

            if (!String.IsNullOrEmpty(response.Content))
            {
                idList = response.Content[1..^1].Split(',').ToList();
                foreach (string id in idList)
                {
                    idDictionary.Add("Proyecto_" + id, DateTime.UtcNow);
                }
            }
            return idDictionary;
        }

        public static Proyecto GetProyecto(string id, ConfigService pConfig)
        {
            string identifier = id.Split('_')[1];

            Proyecto proyectoAux = GetDatosProyecto(identifier, pConfig);

            if (proyectoAux == null)
            {
                return null;
            }

            ContextoProyecto contexto = GetContexto(identifier, pConfig);
            List<ProyectoEquipo> equipo = GetEquipo(identifier, pConfig);
            List<ProyectoEntidadGestora> entidadesGestoras= GetEntidadesGestoras(identifier, pConfig);
            List<ProyectoEntidadConvocante> entidadesConvocantes = GetEntidadesConvocantes(identifier, pConfig);
            List<ProyectoEntidadFinanciadora> entidadesFinanciadoras  = GetEntidadesFinanciadoras(identifier, pConfig);
            List<ProyectoAnualidadResumen> resumenAnualidades  = GetAnualidades(identifier, pConfig);
            ProyectoPresupuestoTotales presupuestosTotales  = GetPresupuestosProyecto(identifier, pConfig);
            List<ProyectoClasificacion> proyectoClasificaciones  = GetProyectoClasificaciones(identifier, pConfig);
            List<ProyectoAreaConocimiento> areaConocimiento = GetAreasConocimiento(identifier, pConfig);
            List<PalabraClave> palabrasClave = GetPalabrasClave(identifier, pConfig);
            List<NotificacionProyectoExternoCVN> notificacionProyectoExternoCVN = GetNotificacionProyectoExternoCVN(identifier, pConfig);

            Proyecto proyecto = proyectoAux;
            proyecto.Contexto = contexto;
            proyecto.Equipo = equipo;
            proyecto.EntidadesGestoras = entidadesGestoras;
            proyecto.EntidadesConvocantes = entidadesConvocantes;
            proyecto.EntidadesFinanciadoras = entidadesFinanciadoras;
            proyecto.ResumenAnualidades = resumenAnualidades;
            proyecto.PresupuestosTotales = presupuestosTotales;
            proyecto.ProyectoClasificacion = proyectoClasificaciones;
            proyecto.AreasConocimiento = areaConocimiento;
            proyecto.PalabrasClaves = palabrasClave;
            proyecto.NotificacionProyectoExternoCVN = notificacionProyectoExternoCVN;

            return proyecto;
        }

        public static Proyecto GetDatosProyecto(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            if (string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            Proyecto proyecto = JsonConvert.DeserializeObject<Proyecto>(response.Content);
            return proyecto;
        }

        public static ContextoProyecto GetContexto(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            ContextoProyecto contexto = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/contexto");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                contexto = JsonConvert.DeserializeObject<ContextoProyecto>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return contexto;
        }

        public static List<ProyectoEquipo> GetEquipo(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<ProyectoEquipo> equipo = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/equipos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                equipo = JsonConvert.DeserializeObject<List<ProyectoEquipo>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return equipo;
        }

        public static List<ProyectoEntidadGestora> GetEntidadesGestoras(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<ProyectoEntidadGestora> entidadesGestoras = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/entidadgestoras");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                entidadesGestoras = JsonConvert.DeserializeObject<List<ProyectoEntidadGestora>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return entidadesGestoras;
        }

        public static List<ProyectoEntidadConvocante> GetEntidadesConvocantes(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<ProyectoEntidadConvocante> entidadesConvocantes = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/entidadconvocantes");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                entidadesConvocantes = JsonConvert.DeserializeObject<List<ProyectoEntidadConvocante>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return entidadesConvocantes;
        }

        public static List<ProyectoEntidadFinanciadora> GetEntidadesFinanciadoras(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<ProyectoEntidadFinanciadora> entidadesFinanciadoras = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/entidadfinanciadoras");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                entidadesFinanciadoras = JsonConvert.DeserializeObject<List<ProyectoEntidadFinanciadora>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return entidadesFinanciadoras;
        }

        public static List<ProyectoAnualidadResumen> GetAnualidades(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<ProyectoAnualidadResumen> anualidades = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/anualidades");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                anualidades = JsonConvert.DeserializeObject<List<ProyectoAnualidadResumen>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return anualidades;
        }

        public static List<ProyectoClasificacion> GetProyectoClasificaciones(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<ProyectoClasificacion> proyectoClasificaciones = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/proyectoclasificaciones");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                proyectoClasificaciones = JsonConvert.DeserializeObject<List<ProyectoClasificacion>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return proyectoClasificaciones;
        }

        public static List<NotificacionProyectoExternoCVN> GetNotificacionProyectoExternoCVN(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<NotificacionProyectoExternoCVN> notificacionProyectos = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/notificacionesproyectos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                notificacionProyectos = JsonConvert.DeserializeObject<List<NotificacionProyectoExternoCVN>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return notificacionProyectos;
        }

        public static ProyectoPresupuestoTotales GetPresupuestosProyecto(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            ProyectoPresupuestoTotales presupuestos = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/presupuesto-totales");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                presupuestos = JsonConvert.DeserializeObject<ProyectoPresupuestoTotales>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return presupuestos;
        }

        public static List<ProyectoAreaConocimiento> GetAreasConocimiento(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<ProyectoAreaConocimiento> areasConocimiento = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/areasconocimento");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                areasConocimiento = JsonConvert.DeserializeObject<List<ProyectoAreaConocimiento>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return areasConocimiento;
        }

        public static List<PalabraClave> GetPalabrasClave(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<PalabraClave> palabrasClave = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/palabrasclave");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                palabrasClave = JsonConvert.DeserializeObject<List<PalabraClave>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return palabrasClave;
        }

        public static List<EstadoProyecto> GetEstadoProyecto(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            List<EstadoProyecto> palabrasClave = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgicsp/proyectos/" + id + "/estadoproyectos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
            try
            {
                palabrasClave = JsonConvert.DeserializeObject<List<EstadoProyecto>>(response.Content);
            }
            catch (Exception)
            {
                return null;
            }
            return palabrasClave;
        }
    }
}