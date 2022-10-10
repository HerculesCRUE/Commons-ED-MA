
using Es.Riam.Gnoss.Recursos;
using Es.Riam.Gnoss.Util.General;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Gnoss.Web.Login
{
    public class GnossMiddleware
    {
        private IHostingEnvironment mEnv;
        private readonly RequestDelegate _next;

        public GnossMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            _next = next;
            mEnv = env;
        }

        public async Task Invoke(HttpContext context, LoggingService loggingService)
        {
            //Application_BeginRequest();
            await _next(context);
            Application_EndRequest(loggingService);
        }



        protected string ObtenerRutaTraza()
        {
            string ruta = Path.Combine(mEnv.ContentRootPath, "trazas");

            if (!Directory.Exists(ruta))
            {
                Directory.CreateDirectory(ruta);
            }

            ruta += "\\traza_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

            return ruta;
        }



        protected void Application_EndRequest(LoggingService pLoggingService)
        {
            ControladorConexiones.CerrarConexiones();

            pLoggingService.GuardarTraza(ObtenerRutaTraza());
        }


        
    }


    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseGnossMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GnossMiddleware>();
        }
    }
}