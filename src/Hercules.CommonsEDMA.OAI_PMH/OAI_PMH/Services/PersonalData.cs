using Newtonsoft.Json;
using OAI_PMH.Controllers;
using OAI_PMH.Models.SGI.ActividadDocente;
using OAI_PMH.Models.SGI.FormacionAcademica;
using OAI_PMH.Models.SGI.PersonalData;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

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

            #region IDs candidatos
            HashSet<string> idListCandidatos = new();
            RestClient clientCandidatos = new(pConfig.GetConfigSGI() + "/api/sgp/personas/modificadas-ids?q=fechaModificacion=ge=\"1500-01-01T00:00:00Z\"");
            clientCandidatos.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var requestCandidatos = new RestRequest(Method.GET);
            IRestResponse responseCandidatos = Token.httpCall(clientCandidatos, requestCandidatos);

            if (!String.IsNullOrEmpty(responseCandidatos.Content))
            {
                List<string> idListCandidatosAux = responseCandidatos.Content[1..^1].Split(',').Distinct().ToList();
                foreach (string id in idListCandidatosAux)
                {
                    string idPersona = "Persona_" + id.Replace("\"", "").Substring(0, id.Replace("\"", "").Length - 1);
                    idListCandidatos.Add(idPersona);
                }
            }
            #endregion

            #region --- Personal Data
            List<string> idList = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/personas/modificadas-ids?q=fechaModificacion=ge=\"" + from + "\"");
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);

            if (!String.IsNullOrEmpty(response.Content))
            {
                idList = response.Content[1..^1].Split(',').ToList();
                foreach (string id in idList)
                {
                    string idPersona = "Persona_" + id.Replace("\"", "").Substring(0, id.Replace("\"", "").Length - 1);
                    if (!idDictionary.ContainsKey(idPersona))
                    {
                        idDictionary.Add(idPersona, DateTime.UtcNow);
                        idListCandidatos.Add(idPersona);
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
            }

            dicFormacionAcademica = AcademicFormation.GetModifiedDoctorados(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicFormacionAcademica)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
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
            }

            dicFormacionAcademica = AcademicFormation.GetModifiedEspecializada(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicFormacionAcademica)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
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
            }

            dicActividadDocente = DocentActivity.GetModifiedAcademicFormationProvided(from, pConfig);
            foreach (KeyValuePair<string, DateTime> item in dicActividadDocente)
            {
                string idPersona = "Persona_" + item.Key.Split("_")[1];
                if (!idDictionary.ContainsKey(idPersona))
                {
                    idDictionary.Add(idPersona, DateTime.UtcNow);
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
            }
            #endregion

            idDictionary = idDictionary.Where(x => idListCandidatos.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);

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

            Persona dataPersona = GetDatosPersona(identifier, pConfig);
            if (dataPersona == null)
            {
                return null;
            }

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
                datosPersonales = GetDatosPersonales(identifier, pConfig);
                datosContacto = GetDatosContacto(identifier, pConfig);
                vinculacion = GetVinculacion(identifier, pConfig);
                datosAcademicos = GetDatosAcademicos(identifier, pConfig);
                fotografia = GetFotografia(identifier, pConfig);
                sexenio = GetSexenios(identifier, pConfig);
                listaFormacionAcademica = DocentActivity.GetAcademicFormationProvided(identifier, pConfig);
                listaSeminarios = DocentActivity.GetSeminars(identifier, pConfig);
                listaTesis = DocentActivity.GetTesis(identifier, pConfig);
                listaCiclos = AcademicFormation.GetFormacionAcademicaCiclos(identifier, pConfig);
                listaDoctorados = AcademicFormation.GetFormacionAcademicaDoctorados(identifier, pConfig);
                listaEspecializada = AcademicFormation.GetFormacionAcademicaEspecializada(identifier, pConfig);
                listaPosgrado = AcademicFormation.GetFormacionAcademicaPosgrado(identifier, pConfig);
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
            IRestResponse response = Token.httpCall(client, request);
            if (string.IsNullOrEmpty(response.Content))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Persona>(response.Content);
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
            IRestResponse response = Token.httpCall(client, request);
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
            IRestResponse response = Token.httpCall(client, request);
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
            IRestResponse response = Token.httpCall(client, request);
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

        private static DatosAcademicos GetDatosAcademicos(string id, ConfigService pConfig)
        {
            string accessToken = Token.CheckToken(pConfig);
            DatosAcademicos datosAcademicos = new();
            RestClient client = new(pConfig.GetConfigSGI() + "/api/sgp/datos-academicos/persona/" + id);
            client.AddDefaultHeader("Authorization", "Bearer " + accessToken);
            var request = new RestRequest(Method.GET);
            IRestResponse response = Token.httpCall(client, request);
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
            IRestResponse response = Token.httpCall(client, request);
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
            IRestResponse response = Token.httpCall(client, request);
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
