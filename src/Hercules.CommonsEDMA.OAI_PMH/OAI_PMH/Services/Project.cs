using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.Project;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OAI_PMH.Services
{
    public class Project
    {
        public static Dictionary<string, DateTime> GetModifiedProjects(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();
            List<string> idList = new();
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/modificados-ids?q=fechaModificacion=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

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
            List<Thread> hilos = new List<Thread>();

            Proyecto proyectoAux = null;
            hilos.Add(new Thread(() => proyectoAux = GetDatosProyecto(identifier, pConfig)));

            ContextoProyecto contexto = null;
            hilos.Add(new Thread(() => contexto = GetContexto(identifier, pConfig)));

            List<ProyectoEquipo> equipo = null;
            hilos.Add(new Thread(() => equipo = GetEquipo(identifier, pConfig)));

            List<ProyectoEntidadGestora> entidadesGestoras = null;
            hilos.Add(new Thread(() => entidadesGestoras = GetEntidadesGestoras(identifier, pConfig)));

            List<ProyectoEntidadConvocante> entidadesConvocantes = null;
            hilos.Add(new Thread(() => entidadesConvocantes = GetEntidadesConvocantes(identifier, pConfig)));

            List<ProyectoEntidadFinanciadora> entidadesFinanciadoras = null;
            hilos.Add(new Thread(() => entidadesFinanciadoras = GetEntidadesFinanciadoras(identifier, pConfig)));

            List<ProyectoAnualidadResumen> resumenAnualidades = null;
            hilos.Add(new Thread(() => resumenAnualidades = GetAnualidades(identifier, pConfig)));

            ProyectoPresupuestoTotales presupuestosTotales = null;
            hilos.Add(new Thread(() => presupuestosTotales = GetPresupuestosProyecto(identifier, pConfig)));

            List<ProyectoClasificacion> proyectoClasificaciones = null;
            hilos.Add(new Thread(() => proyectoClasificaciones = GetProyectoClasificaciones(identifier, pConfig)));

            List<ProyectoAreaConocimiento> areaConocimiento = null;
            hilos.Add(new Thread(() => areaConocimiento = GetAreasConocimiento(identifier, pConfig)));

            List<PalabraClave> palabrasClave = null;
            hilos.Add(new Thread(() => palabrasClave = GetPalabrasClave(identifier, pConfig)));

            List<NotificacionProyectoExternoCVN> notificacionProyectoExternoCVN = null;
            hilos.Add(new Thread(() => notificacionProyectoExternoCVN = GetNotificacionProyectoExternoCVN(identifier, pConfig)));

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
            Proyecto proyecto = new();
            List<Thread> hilos = new List<Thread>();
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);            
            try
            {
                var json = JObject.Parse(response.Content);
                proyecto = JsonConvert.DeserializeObject<Proyecto>(json.ToString());
            }
            catch
            {
                return null;
            }
            return proyecto;
        }

        public static ContextoProyecto GetContexto(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            ContextoProyecto contexto = new();
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/contexto");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/equipos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/entidadgestoras");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/entidadconvocantes");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/entidadfinanciadoras");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/anualidades");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/proyectoclasificaciones");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/notificacionesproyectos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/presupuesto-totales");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/areasconocimento");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/palabrasclave");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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
            RestClient client = new(pConfig.GetUrlBaseProyecto() + "proyectos/" + id + "/estadoproyectos");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
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