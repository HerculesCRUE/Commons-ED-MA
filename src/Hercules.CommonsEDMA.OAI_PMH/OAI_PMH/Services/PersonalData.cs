using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.ActividadDocente;
using OAI_PMH.Models.SGI.FormacionAcademica;
using OAI_PMH.Models.SGI.PersonalData;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OAI_PMH.Services
{
    /// <summary>
    /// Datos personales
    /// </summary>
    public class PersonalData
    {
        public static Dictionary<string, DateTime> GetModifiedPeople(string from, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Dictionary<string, DateTime> idDictionary = new();

            #region --- Personal Data
            List<string> idList = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/personas/modificadas-ids?q=fechaModificacion=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (!String.IsNullOrEmpty(response.Content))
            {
                idList = response.Content[1..^1].Split(',').ToList();
                foreach (string id in idList)
                {
                    string idPersona = "Persona_" + id.Replace("\"", "").Substring(0, id.Replace("\"", "").Length - 1);
                    if (!idDictionary.ContainsKey(idPersona))
                    {
                        idDictionary.Add(idPersona, DateTime.UtcNow);
                    }
                }
            }
            #endregion

            #region --- Formación Académica
            Dictionary<string, DateTime> dicFormacionAcademica = AcademicFormation.GetModifiedCiclos(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicFormacionAcademica)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
                }
                else
                {
                    if (DateTime.Compare(item.Value, idDictionary[idPersona]) == 1)
                    {
                        idDictionary[idPersona] = item.Value;
                    }
                }
            }

            dicFormacionAcademica = AcademicFormation.GetModifiedDoctorados(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicFormacionAcademica)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
                }
                else
                {
                    if (DateTime.Compare(item.Value, idDictionary[idPersona]) == 1)
                    {
                        idDictionary[idPersona] = item.Value;
                    }
                }
            }

            dicFormacionAcademica = AcademicFormation.GetModifiedPosgrado(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicFormacionAcademica)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
                }
                else
                {
                    if (DateTime.Compare(item.Value, idDictionary[idPersona]) == 1)
                    {
                        idDictionary[idPersona] = item.Value;
                    }
                }
            }

            dicFormacionAcademica = AcademicFormation.GetModifiedEspecializada(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicFormacionAcademica)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
                }
                else
                {
                    if (DateTime.Compare(item.Value, idDictionary[idPersona]) == 1)
                    {
                        idDictionary[idPersona] = item.Value;
                    }
                }
            }
            #endregion

            #region --- Actividad Docente
            Dictionary<string, DateTime> dicActividadDocente = DocentActivity.GetModifiedTesis(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicActividadDocente)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
                }
                else
                {
                    if (DateTime.Compare(item.Value, idDictionary[idPersona]) == 1)
                    {
                        idDictionary[idPersona] = item.Value;
                    }
                }
            }

            dicActividadDocente = DocentActivity.GetModifiedAcademicFormationProvided(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicActividadDocente)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
                }
                else
                {
                    if (DateTime.Compare(item.Value, idDictionary[idPersona]) == 1)
                    {
                        idDictionary[idPersona] = item.Value;
                    }
                }
            }

            dicActividadDocente = DocentActivity.GetModifiedSeminars(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicActividadDocente)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
                }
                else
                {
                    if (DateTime.Compare(item.Value, idDictionary[idPersona]) == 1)
                    {
                        idDictionary[idPersona] = item.Value;
                    }
                }
            }
            #endregion

            return idDictionary;
        }

        /// <summary>
        /// Devuelve la persona con el id <paramref name="id"/>
        /// </summary>
        /// <param name="id">Identificador de la persona</param>
        /// <param name="pConfig">ConfigService</param>
        /// <returns>Devuelve la persona</returns>
        public static Persona GetPersona(string id, ConfigService pConfig)
        {
            string identifier = id.Split('_')[1];
            List<Thread> hilos = new List<Thread>();

            Persona dataPersona = GetDatosPersona(identifier, pConfig);

            DatosPersonales datosPersonales = null;
            DatosContacto datosContacto = null;
            Vinculacion vinculacion = null;
            DatosAcademicos datosAcademicos = null;
            Fotografia fotografia = null;
            Sexenio sexenio = null;
            List<FormacionAcademicaImpartida> listaFormacionAcademica = null;
            List<SeminariosCursos> listaSeminarios = null;
            List<Tesis> listaTesis = null;
            List<Ciclos> listaCiclos = null;
            List<Doctorados> listaDoctorados = null;
            List<FormacionEspecializada> listaEspecializada = null;
            List<Posgrado> listaPosgrado = null;

            if (dataPersona.Activo.HasValue && dataPersona.Activo.Value)
            {
                hilos.Add(new Thread(() =>
                    datosPersonales = GetDatosPersonales(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    datosContacto = GetDatosContacto(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    vinculacion = GetVinculacion(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    datosAcademicos = GetDatosAcademicos(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    fotografia = GetFotografia(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    sexenio = GetSexenios(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    listaFormacionAcademica = DocentActivity.GetAcademicFormationProvided(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    listaSeminarios = DocentActivity.GetSeminars(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    listaTesis = DocentActivity.GetTesis(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    listaCiclos = AcademicFormation.GetFormacionAcademicaCiclos(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    listaDoctorados = AcademicFormation.GetFormacionAcademicaDoctorados(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    listaEspecializada = AcademicFormation.GetFormacionAcademicaEspecializada(identifier, pConfig)
                ));


                hilos.Add(new Thread(() =>
                    listaPosgrado = AcademicFormation.GetFormacionAcademicaPosgrado(identifier, pConfig)
                ));

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
            }

            Persona persona = dataPersona;
            persona.DatosPersonales = datosPersonales;
            persona.DatosContacto = datosContacto;
            persona.Vinculacion = vinculacion;
            persona.DatosAcademicos = datosAcademicos;
            persona.Fotografia = fotografia;
            persona.Sexenios = sexenio;
            persona.FormacionAcademicaImpartida = listaFormacionAcademica;
            persona.SeminariosCursos = listaSeminarios;
            persona.Tesis = listaTesis;
            persona.Ciclos = listaCiclos;
            persona.Doctorados = listaDoctorados;
            persona.FormacionEspecializada = listaEspecializada;
            persona.Posgrado = listaPosgrado;

            return persona;
        }

        /// <summary>
        /// Obtiene los datos básicos de la persona.
        /// </summary>
        /// <param name="id">Identificador de la persona</param>
        /// <param name="pConfig">ConfigService</param>
        /// <param name="accessToken">Token de acceso</param>
        /// <returns>Devuelve el objeto persona.</returns>
        private static Persona GetDatosPersona(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/personas/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            var json = JObject.Parse(response.Content);
            return JsonConvert.DeserializeObject<Persona>(json.ToString());
        }

        /// <summary>
        /// Devuelve los datos personales de la persona con id <paramref name="id"/>
        /// </summary>
        /// <param name="id">Identificador de la persona</param>
        /// <param name="pConfig">ConfigService</param>
        /// <param name="accessToken">Token de acceso</param>
        /// <returns>Devuelve los datos personales de la persona</returns>
        private static DatosPersonales GetDatosPersonales(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            DatosPersonales datosPersonales = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/datos-personales/persona/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                datosPersonales = JsonConvert.DeserializeObject<DatosPersonales>(response.Content);
            }
            catch
            {
                return null;
            }
            return datosPersonales;
        }

        private static DatosContacto GetDatosContacto(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            DatosContacto datosContacto = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/datos-contacto/persona/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                datosContacto = JsonConvert.DeserializeObject<DatosContacto>(response.Content);
            }
            catch
            {
                return null;
            }
            return datosContacto;
        }

        private static Vinculacion GetVinculacion(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Vinculacion vinculacion = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/vinculaciones/persona/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                vinculacion = JsonConvert.DeserializeObject<Vinculacion>(response.Content);
            }
            catch
            {
                return null;
            }
            return vinculacion;
        }

        private static VinculacionCategoriaProfesional GetVinculacionCategoriaProfesional(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            VinculacionCategoriaProfesional vinculacion = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/vinculaciones/persona/" + id + "/vinculaciones-categorias-profesionales");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                vinculacion = JsonConvert.DeserializeObject<VinculacionCategoriaProfesional>(response.Content);
            }
            catch
            {
                return null;
            }
            return vinculacion;
        }

        private static DatosAcademicos GetDatosAcademicos(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            DatosAcademicos datosAcademicos = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/datos-academicos/persona/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                datosAcademicos = JsonConvert.DeserializeObject<DatosAcademicos>(response.Content);
            }
            catch
            {
                return null;
            }
            return datosAcademicos;
        }

        private static Fotografia GetFotografia(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Fotografia fotografia = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/personas/" + id + "/fotografia");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                fotografia = JsonConvert.DeserializeObject<Fotografia>(response.Content);
            }
            catch
            {
                return null;
            }
            return fotografia;
        }

        private static Sexenio GetSexenios(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            Sexenio sexenios = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/sexenios/persona/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            try
            {
                sexenios = JsonConvert.DeserializeObject<Sexenio>(response.Content);
            }
            catch
            {
                return null;
            }
            return sexenios;
        }
    }
}
