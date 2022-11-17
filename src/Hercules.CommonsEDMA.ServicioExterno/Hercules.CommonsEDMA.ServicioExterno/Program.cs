using Hercules.CommonsEDMA.ServicioExterno.Controllers.Acciones;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Hercules.CommonsEDMA.ServicioExterno
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    GenerateMetaSearch();
                });


        public static void GenerateMetaSearch()
        {
            AccionesMetaBusqueda accmt = new();
            accmt.GenerateMetaShearch();
        }
    }
}
