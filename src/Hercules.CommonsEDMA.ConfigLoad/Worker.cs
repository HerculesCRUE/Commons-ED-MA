using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Helpers;
using Gnoss.ApiWrapper.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Hercules.CommonsEDMA.ConfigLoad
{
    public class Worker : BackgroundService
    {
        private static ConfigService configService;
        private static string bdType;

        /// <summary>
        /// Contructor.
        /// </summary>
        public Worker()
        {
        }

        /// <summary>
        /// Tarea asincrona.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                configService = new ConfigService();
                string nombreProy = configService.ObtenerNombreCortoComunidad();
                string loginAdmin = configService.ObtenerLoginAdmin();
                string passAdmin = configService.ObtenerPassAdmin();
                SubirConfiguraciones(nombreProy, loginAdmin, passAdmin);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error no controlado: " + ex.Message);
                Console.WriteLine("Pila de llamadas: " + ex.StackTrace);
                Finalizar();
            }
        }


        /// <summary>
        /// Permite lanzar la escucha despu�s de leer. 
        /// Contiene un sleep de 30 segundos.
        /// </summary>
        private void OnShutDown()
        {
            Thread.Sleep(30000);
        }

        private static void SubirConfiguraciones(string pNombreProy, string pLoginUsuario, string pPasswordUsuario)
        {
            Console.WriteLine("8.- Subimos configuraciones");
            string rutaBase = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Files{Path.DirectorySeparatorChar}";
            Console.WriteLine("8.1- Subimos ontolog�as");
            Despliegue(rutaBase + "Ontologias.zip", "Ontologias", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.2- Subimos Objetos de conocimiento");
            Despliegue(rutaBase + "ObjetosConocimiento.zip", "ObjetosConocimiento", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.3- Subimos Facetas");
            Despliegue(rutaBase + "Facetas.zip", "Facetas", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.4- Subimos Componentes del CMS");
            Despliegue(rutaBase + "ComponentesCMS.zip", "ComponentesCMS", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.5- Subimos Pesta�as");
            Despliegue(rutaBase + "Pestanyas.zip", "Pestanyas", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.6- Subimos Paginas del CMS");
            Despliegue(rutaBase + "PaginasCMS.zip", "PaginasCMS", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.7- Subimos Utilidades");
            Despliegue(rutaBase + "Utilidades.zip", "Utilidades", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.8- Subimos Opciones avanzadas");
            Despliegue(rutaBase + "OpcionesAvanzadas.zip", "OpcionesAvanzadas", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.9- Subimos Estilos");
            Despliegue(rutaBase + "Estilos.zip", "Estilos", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.10- Subimos Par�metros de b�squeda personalizados");
            Despliegue(rutaBase + "SearchPersonalizado.zip", "SearchPersonalizado", pNombreProy, pLoginUsuario, pPasswordUsuario);
            Console.WriteLine("8.11- Subimos Vistas");
            Despliegue(rutaBase + "Vistas.zip", "Vistas", pNombreProy, pLoginUsuario, pPasswordUsuario);
        }

        private static void Despliegue(string pRutaFichero, string pMetodo, string pNombreProy, string pLoginUsuario, string pPasswordUsuario)
        {
            string sWebAddress = $"{configService.ObtenerUrlAPIDespliegues()}Upload?tipoPeticion={pMetodo}&usuario={pLoginUsuario}&password={pPasswordUsuario}&nombreProy={pNombreProy}";

            HttpContent contentData = null;

            byte[] data = File.ReadAllBytes(pRutaFichero);
            ByteArrayContent bytes = new ByteArrayContent(data);
            bytes.Headers.Add("Content-Type", "application/zip");
            contentData = new MultipartFormDataContent();
            ((MultipartFormDataContent)contentData).Add(bytes, "ficheroZip", ".zip");

            string result = "";
            HttpResponseMessage response = null;

            try
            {
                HttpClient client = new HttpClient();
                response = client.PostAsync($"{sWebAddress}", contentData).Result;
                response.EnsureSuccessStatusCode();
                result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"{pMetodo} {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{pMetodo} Error:{ex.Message}");
            }
        }

        private static void Finalizar()
        {
            Console.WriteLine("Pulsa intro para finalizar");
            Console.ReadLine();
        }
    }
}
