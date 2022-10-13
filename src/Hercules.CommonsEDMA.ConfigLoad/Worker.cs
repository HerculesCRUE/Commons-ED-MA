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
                SubirConfiguraciones();
                Console.WriteLine("Ha finalizado la carga de configuraciones");
                Finalizar();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error no controlado: " + ex.Message);
                Console.WriteLine("Pila de llamadas: " + ex.StackTrace);
                Finalizar();
            }
        }


        /// <summary>
        /// Permite lanzar la escucha después de leer. 
        /// Contiene un sleep de 30 segundos.
        /// </summary>
        private void OnShutDown()
        {
            Thread.Sleep(30000);
        }

        private void SubirConfiguraciones()
        {
            Console.WriteLine("Subimos configuraciones");
            string rutaBase = $@"{System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Files{Path.DirectorySeparatorChar}";
            Console.WriteLine("1- Subimos ontologías");
            Despliegue(rutaBase + "Ontologias.zip", "Ontologias");
            Console.WriteLine("2- Subimos Objetos de conocimiento");
            Despliegue(rutaBase + "ObjetosConocimiento.zip", "ObjetosConocimiento");
            Console.WriteLine("3- Subimos Facetas");
            Despliegue(rutaBase + "Facetas.zip", "Facetas");
            Console.WriteLine("4- Subimos Componentes del CMS");
            Despliegue(rutaBase + "ComponentesCMS.zip", "ComponentesCMS");
            Console.WriteLine("5- Subimos Pestañas");
            Despliegue(rutaBase + "Pestanyas.zip", "Pestanyas");
            Console.WriteLine("6- Subimos Paginas del CMS");
            Despliegue(rutaBase + "PaginasCMS.zip", "PaginasCMS");
            Console.WriteLine("7- Subimos Utilidades");
            Despliegue(rutaBase + "Utilidades.zip", "Utilidades");
            Console.WriteLine("8- Subimos Opciones avanzadas");
            Despliegue(rutaBase + "OpcionesAvanzadas.zip", "OpcionesAvanzadas");
            Console.WriteLine("9- Subimos Estilos");
            Despliegue(rutaBase + "Estilos.zip", "Estilos");
            Console.WriteLine("10- Subimos Parámetros de búsqueda personalizados");
            Despliegue(rutaBase + "SearchPersonalizado.zip", "SearchPersonalizado");
            Console.WriteLine("11- Subimos Vistas");
            Despliegue(rutaBase + "Vistas.zip", "Vistas");
        }

        private void CambiarURLs(string rutaZip)
        {
            string rutaCarpeta = rutaZip.Substring(0, rutaZip.Length - 4);
            if (Directory.Exists(rutaCarpeta))
            {
                Directory.Delete(rutaCarpeta, true);
            }
            System.IO.Compression.ZipFile.ExtractToDirectory(rutaZip, rutaCarpeta);
            foreach (string file in Directory.EnumerateFiles(rutaCarpeta, "*.*", SearchOption.AllDirectories))
            {
                string contenidoOriginal = File.ReadAllText(file);
                string contenidoModificado = contenidoOriginal;
                contenidoModificado=contenidoModificado.Replace(configService.ObtenerUrlServiciosBase(), configService.ObtenerUrlServiciosInstalacion());
                if (contenidoOriginal != contenidoModificado)
                {
                    System.IO.File.WriteAllText(file, contenidoModificado);
                }
            }
            if (File.Exists(rutaZip))
            {
                File.Delete(rutaZip);
            }
            System.IO.Compression.ZipFile.CreateFromDirectory(rutaCarpeta,rutaZip);
            if (Directory.Exists(rutaCarpeta))
            {
                System.IO.Directory.Delete(rutaCarpeta, true);
            }
        }


        private void Despliegue(string pRutaFichero, string pMetodo)
        {
            string nombreProy = configService.ObtenerNombreCortoComunidad();
            string loginAdmin = configService.ObtenerLoginAdmin();
            string passAdmin = configService.ObtenerPassAdmin();
            CambiarURLs(pRutaFichero);
            string sWebAddress = $"{configService.ObtenerUrlAPIDespliegues()}Upload?tipoPeticion={pMetodo}&usuario={loginAdmin}&password={passAdmin}&nombreProy={nombreProy}";

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
