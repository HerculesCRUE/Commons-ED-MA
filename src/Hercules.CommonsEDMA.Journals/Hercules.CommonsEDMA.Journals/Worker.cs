
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.Journals
{
    public class Worker : BackgroundService
    {

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
                CargaRevistas.CargarRevistas();
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

        private static void Finalizar()
        {
            Console.WriteLine("Pulsa intro para finalizar");
            Console.ReadLine();
        }
    }
}
