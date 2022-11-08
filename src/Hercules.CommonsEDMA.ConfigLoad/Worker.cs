using Hercules.CommonsEDMA.ConfigLoad.Models.Services;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.ConfigLoad
{
    public class Worker : BackgroundService
    {
        private static ConfigService configService;

        /// <summary>
        /// Contructor.
        /// </summary>
        public Worker() { }

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

        private static void SubirConfiguraciones()
        {
            List<Tuple<string, string, string>> listaPasos = new();
            listaPasos.Add(Tuple.Create("1- Subimos ontologías", "Ontologias.zip", "Ontologias"));
            listaPasos.Add(Tuple.Create("2- Subimos Objetos de conocimiento", "ObjetosConocimiento.zip", "ObjetosConocimiento"));
            listaPasos.Add(Tuple.Create("3- Subimos Facetas", "Facetas.zip", "Facetas"));
            listaPasos.Add(Tuple.Create("4- Subimos Componentes del CMS", "ComponentesCMS.zip", "ComponentesCMS"));
            listaPasos.Add(Tuple.Create("5- Subimos Pestañas", "Pestanyas.zip", "Pestanyas"));
            listaPasos.Add(Tuple.Create("6- Subimos Paginas del CMS", "PaginasCMS.zip", "PaginasCMS"));
            listaPasos.Add(Tuple.Create("7- Subimos Utilidades", "Utilidades.zip", "Utilidades"));
            listaPasos.Add(Tuple.Create("8- Subimos Opciones avanzadas", "OpcionesAvanzadas.zip", "OpcionesAvanzadas"));
            listaPasos.Add(Tuple.Create("9- Subimos Estilos", "Estilos.zip", "Estilos"));
            listaPasos.Add(Tuple.Create("10- Subimos Parámetros de búsqueda personalizados", "SearchPersonalizado.zip", "SearchPersonalizado"));
            listaPasos.Add(Tuple.Create("11- Subimos Vistas", "Vistas.zip", "Vistas"));

            Console.WriteLine("Subimos configuraciones");
            string rutaBase = $@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Files{Path.DirectorySeparatorChar}";

            Console.WriteLine("Escribe el login del usuario administrador:");
            string loginAdmin = Console.ReadLine();
            Console.WriteLine("Escribe el password del usuario administrador:");
            string passAdmin = Console.ReadLine();

            foreach (Tuple<string, string, string> step in listaPasos)
            {
                Console.WriteLine(step.Item1);
                Despliegue(rutaBase + step.Item2, step.Item3, loginAdmin, passAdmin);
            }
        }

        private static void Despliegue(string pRutaFichero, string pMetodo, string pLoginAdmin, string pPassAdmin)
        {
            string nombreProy = configService.ObtenerNombreCortoComunidad();
            string sWebAddress = $"{configService.ObtenerUrlAPIDespliegues()}Upload?tipoPeticion={pMetodo}&usuario={pLoginAdmin}&password={pPassAdmin}&nombreProy={nombreProy}";

            HttpContent contentData;

            byte[] data = File.ReadAllBytes(pRutaFichero);
            ByteArrayContent bytes = new(data);
            bytes.Headers.Add("Content-Type", "application/zip");
            contentData = new MultipartFormDataContent();
            ((MultipartFormDataContent)contentData).Add(bytes, "ficheroZip", ".zip");

            string result;
            HttpResponseMessage response;

            try
            {
                HttpClient client = new();
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
