using Gnoss.ApiWrapper;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Harvester
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
                ResourceApi mResourceApi = new ResourceApi($@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config");
                CommunityApi mCommunityApi = new CommunityApi($@"{AppDomain.CurrentDomain.SetupInformation.ApplicationBase}Config{Path.DirectorySeparatorChar}ConfigOAuth{Path.DirectorySeparatorChar}OAuthV3.config");
                Loader loader = new Loader(mResourceApi);
                loader.LoadMainEntities();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se ha producido un error no controlado: " + ex.Message);
                Console.WriteLine("Pila de llamadas: " + ex.StackTrace);
                Finalizar(); 
            }
        }

        private static void Finalizar()
        {
            Console.WriteLine("Pulsa intro para finalizar");
            Console.ReadLine();
        }
    }
}
