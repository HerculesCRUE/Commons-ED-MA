using OAI_PMH.Controllers;
using System;
using System.IO;

namespace OAI_PMH.Middlewares
{
    public class FileLogger
    {
        // Configuración.
        readonly ConfigService _Configuracion;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="pConfig">Configuración.</param>
        public FileLogger(ConfigService pConfig)
        {
            _Configuracion = pConfig;
        }

        /// <summary>
        /// Pinta el mensaje del log.
        /// </summary>
        /// <param name="pFechaInicio">Fecha de inicio.</param>
        /// <param name="pFechaFin">Fecha de fin.</param>
        /// <param name="pUrl">URL de la petición.</param>
        /// <param name="pTipo">Tipo de log.</param>
        public void Log(DateTime pFechaInicio, DateTime pFechaFin, string pUrl, string pTipo)
        {
            string fecha = DateTime.Now.ToString().Split(" ")[0].Replace("/", "-");
            string ruta = $@"{_Configuracion.GetLogPath()}OAI-PMH_{fecha}.log";

            string fechaInicio = pFechaInicio.ToString("HH':'mm':'ss");
            string fechaFin = pFechaFin.ToString("HH':'mm':'ss");
            string tiempoSegundos = (pFechaFin - pFechaInicio).TotalSeconds.ToString("0.00");

            if (!File.Exists(ruta))
            {
                using (FileStream fs = File.Create(ruta)) { }
            }

            File.AppendAllText(ruta, $@"{pTipo} || {tiempoSegundos}' || {fechaInicio} || {fechaFin} || {pUrl}{Environment.NewLine}");
        }
    }
}
