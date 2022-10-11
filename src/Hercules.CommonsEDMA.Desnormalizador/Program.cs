using Hercules.CommonsEDMA.Desnormalizador.Models;
using Hercules.CommonsEDMA.Desnormalizador.Models.Services;
using Gnoss.ApiWrapper;
using Gnoss.ApiWrapper.ApiModel;
using Gnoss.ApiWrapper.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace Hercules.CommonsEDMA.Desnormalizador
{
    class Program
    {
        private string _LogPath;

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// CreateHostBuilder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(typeof(ConfigService));
                    services.AddScoped(typeof(RabbitServiceReaderDenormalizer));
                    services.AddHostedService<Worker>();
                });

        /// <summary>
        /// Clase FileLogger.
        /// </summary>
        public static class FileLogger
        {
            private const string FilePath = "/app/logs/"; // --- TODO: Sacarlo a archivo de configuración.

            /// <summary>
            /// Sobreescribe el método Log para pintar el mensaje de error en un fichero.
            /// </summary>
            /// <param name="messsage"></param>
            public static void Log(Exception ex)
            {
                string filePath = FilePath + "error_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                using var fileStream = new FileStream(filePath, FileMode.Append);
                using var writer = new StreamWriter(fileStream);
                writer.WriteLine(ex.Message);
                writer.WriteLine(ex.StackTrace);
            }

            /// <summary>
            /// Sobreescribe el método Log para pintar el mensaje de error en un fichero.
            /// </summary>
            /// <param name="messsage"></param>
            public static void Log(string messsage)
            {
                string filePath = FilePath + "error_"+ DateTime.Now.ToString("yyyy-MM-dd")+".txt";
                using var fileStream = new FileStream(filePath, FileMode.Append);
                using var writer = new StreamWriter(fileStream);
                writer.WriteLine(messsage);
            }
        }
    }
}
