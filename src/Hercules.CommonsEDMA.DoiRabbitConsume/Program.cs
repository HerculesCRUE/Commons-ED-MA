using Gnoss.Web.ReprocessData.Models.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.DoiRabbitConsume
{
    public class Program
    {
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
                    services.AddScoped(typeof(ReadRabbitService));
                    services.AddHostedService<Worker>();
                });

        /// <summary>
        /// Clase FileLogger.
        /// </summary>
        public static class FileLogger
        {
            private const string FilePath = "/app/logs/log.txt"; // --- TODO: Sacarlo a archivo de configuración.

            /// <summary>
            /// Sobreescribe el método Log para pintar el mensaje de error en un fichero.
            /// </summary>
            /// <param name="messsage"></param>
            public static void Log(string messsage)
            {
                using var fileStream = new FileStream(FilePath, FileMode.Append);
                using var writter = new StreamWriter(fileStream);
                writter.WriteLine(messsage);
            }
        }
    }
}
