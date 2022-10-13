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
            RestClient client = new(pConfig.GetUrlBasePersona() + "personas/modificadas-ids?q=fechaModificacion=ge=\"" + from + "\"");
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

            dicFormacionAcademica = AcademicFormation.GetModifiedDoctorados(from, pConfig, accessToken);
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

            dicFormacionAcademica = AcademicFormation.GetModifiedPosgrado(from, pConfig, accessToken);
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

            dicFormacionAcademica = AcademicFormation.GetModifiedEspecializada(from, pConfig, accessToken);
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
            Dictionary<string, DateTime> dicActividadDocente = DocentActivity.GetModifiedTesis(from, pConfig, accessToken);
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

            dicActividadDocente = DocentActivity.GetModifiedAcademicFormationProvided(from, pConfig, accessToken);
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

            dicActividadDocente = DocentActivity.GetModifiedSeminars(from, pConfig, accessToken);
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
            string accessToken = Token.CheckToken(pConfig);
            string identifier = id.Split('_')[1];
            List<Thread> hilos = new List<Thread>();

            Persona dataPersona = new();
            hilos.Add(new Thread(() =>
                dataPersona = GetDatosPersona(identifier, pConfig, accessToken)
            ));

            DatosPersonales datosPersonales = null;
            hilos.Add(new Thread(() =>
                datosPersonales = GetDatosPersonales(identifier, pConfig, accessToken)
            ));

            DatosContacto datosContacto = null;
            hilos.Add(new Thread(() =>
                datosContacto = GetDatosContacto(identifier, pConfig, accessToken)
            ));

            Vinculacion vinculacion = null;
            hilos.Add(new Thread(() =>
                vinculacion = GetVinculacion(identifier, pConfig, accessToken)
            ));

            DatosAcademicos datosAcademicos = null;
            hilos.Add(new Thread(() =>
                datosAcademicos = GetDatosAcademicos(identifier, pConfig, accessToken)
            ));

            Fotografia fotografia = null;
            hilos.Add(new Thread(() =>
                fotografia = GetFotografia(identifier, pConfig, accessToken)
            ));

            Sexenio sexenio = null;
            hilos.Add(new Thread(() =>
                sexenio = GetSexenios(identifier, pConfig, accessToken)
            ));

            List<FormacionAcademicaImpartida> listaFormacionAcademica = null;
            hilos.Add(new Thread(() =>
                listaFormacionAcademica = DocentActivity.GetAcademicFormationProvided(identifier, pConfig, accessToken)
            ));

            List<SeminariosCursos> listaSeminarios = null;
            hilos.Add(new Thread(() =>
                listaSeminarios = DocentActivity.GetSeminars(identifier, pConfig, accessToken)
            ));

            List<Tesis> listaTesis = null;
            hilos.Add(new Thread(() =>
                listaTesis = DocentActivity.GetTesis(identifier, pConfig, accessToken)
            ));

            List<Ciclos> listaCiclos = null;
            hilos.Add(new Thread(() =>
                listaCiclos = AcademicFormation.GetFormacionAcademicaCiclos(identifier, pConfig, accessToken)
            ));

            List<Doctorados> listaDoctorados = null;
            hilos.Add(new Thread(() =>
                listaDoctorados = AcademicFormation.GetFormacionAcademicaDoctorados(identifier, pConfig, accessToken)
            ));

            List<FormacionEspecializada> listaEspecializada = null;
            hilos.Add(new Thread(() =>
                listaEspecializada = AcademicFormation.GetFormacionAcademicaEspecializada(identifier, pConfig, accessToken)
            ));

            List<Posgrado> listaPosgrado = null;
            hilos.Add(new Thread(() =>
                listaPosgrado = AcademicFormation.GetFormacionAcademicaPosgrado(identifier, pConfig, accessToken)
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
        private static Persona GetDatosPersona(string id, ConfigService pConfig, string accessToken)
        {
            RestClient client = new(pConfig.GetUrlBasePersona() + "personas/" + id);
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
        private static DatosPersonales GetDatosPersonales(string id, ConfigService pConfig, string accessToken)
        {
            DatosPersonales datosPersonales = new();
            RestClient client = new(pConfig.GetUrlBasePersona() + "datos-personales/persona/" + id);
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

        private static DatosContacto GetDatosContacto(string id, ConfigService pConfig, string accessToken)
        {
            DatosContacto datosContacto = new();
            RestClient client = new(pConfig.GetUrlBasePersona() + "datos-contacto/persona/" + id);
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

        private static Vinculacion GetVinculacion(string id, ConfigService pConfig, string accessToken)
        {
            Vinculacion vinculacion = new();
            RestClient client = new(pConfig.GetUrlBasePersona() + "vinculaciones/persona/" + id);
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

        private static VinculacionCategoriaProfesional GetVinculacionCategoriaProfesional(string id, ConfigService pConfig, string accessToken)
        {
            VinculacionCategoriaProfesional vinculacion = new();
            RestClient client = new(pConfig.GetUrlBasePersona() + "vinculaciones/persona/" + id + "/vinculaciones-categorias-profesionales");
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

        private static DatosAcademicos GetDatosAcademicos(string id, ConfigService pConfig, string accessToken)
        {
            DatosAcademicos datosAcademicos = new();
            RestClient client = new(pConfig.GetUrlBasePersona() + "datos-academicos/persona/" + id);
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

        private static Fotografia GetFotografia(string id, ConfigService pConfig, string accessToken)
        {
            Fotografia fotografia = new();
            RestClient client = new(pConfig.GetUrlBasePersona() + "personas/" + id + "/fotografia");
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

        private static Sexenio GetSexenios(string id, ConfigService pConfig, string accessToken)
        {
            Sexenio sexenios = new();
            RestClient client = new(pConfig.GetUrlBasePersona() + "sexenios/persona/" + id);
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
