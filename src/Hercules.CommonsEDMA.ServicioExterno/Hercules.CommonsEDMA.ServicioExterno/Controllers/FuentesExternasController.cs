using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Threading.Tasks;
using Hercules.CommonsEDMA.ServicioExterno.Models;
namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class FuentesExternasController : ControllerBase
    {
        readonly ConfigService _Configuracion;

        public FuentesExternasController(ConfigService pConfig)
        {
            _Configuracion = pConfig;

        }

        [HttpGet("InsertToQueue")]
        public IActionResult InsertToQueueFuentesExternas(string pUserId)
        {

            if (!Security.CheckUser(new Guid(pUserId), Request))
            {
                return StatusCode(StatusCodes.Status401Unauthorized);
            }

            try
            {
                ReadRabbitService rabbitMQService = new ReadRabbitService(_Configuracion);
                string orcid = Acciones.AccionesFuentesExternas.GetORCID(pUserId);
                string idGnoss = Acciones.AccionesFuentesExternas.GetIdGnoss(pUserId);

                if (!string.IsNullOrEmpty(orcid))
                {
                    string ultimaFechaMod = Acciones.AccionesFuentesExternas.GetLastUpdatedDate(pUserId);

                    DateTime currentDate = DateTime.Now;
                    DateTime lastUpdate = new DateTime(Int32.Parse(ultimaFechaMod.Split('-')[0]), Int32.Parse(ultimaFechaMod.Split('-')[1]), Int32.Parse(ultimaFechaMod.Split('-')[2]));

                    if (lastUpdate.AddDays(1) > currentDate)
                    {
                        return BadRequest(ultimaFechaMod);
                    }

                    Dictionary<string, string> dicIDs = Acciones.AccionesFuentesExternas.GetUsersIDs(pUserId);

                    // Publicaciones.
                    List<string> listaDatos = new List<string>() { "investigador", orcid, ultimaFechaMod, idGnoss };
                    rabbitMQService.PublishMessage(listaDatos, _Configuracion.GetFuentesExternasQueueRabbit());

                    // Zenodo
                    List<string> listaDatosZenodo = new List<string>() { "zenodo", orcid };
                    rabbitMQService.PublishMessage(listaDatosZenodo, _Configuracion.GetFuentesExternasQueueRabbit());

                    // FigShare
                    if (dicIDs.ContainsKey("usuarioFigshare") && dicIDs.ContainsKey("tokenFigshare"))
                    {
                        List<string> listaDatosFigShare = new List<string>() { "figshare", dicIDs["tokenFigshare"] };
                        rabbitMQService.PublishMessage(listaDatosFigShare, _Configuracion.GetFuentesExternasQueueRabbit());
                    }

                    // GitHub
                    if (dicIDs.ContainsKey("usuarioGitHub") && dicIDs.ContainsKey("tokenGitHub"))
                    {
                        List<string> listaDatosGitHub = new List<string>() { "github", dicIDs["usuarioGitHub"], dicIDs["tokenGitHub"] };
                        rabbitMQService.PublishMessage(listaDatosGitHub, _Configuracion.GetFuentesExternasQueueRabbit());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Ok();
        }

        [HttpGet("InsertDoiToQueue")]
        public bool InsertDoiToQueueFuentesExternas([Required] string pIdentificador, [Required] string pDoi, [Required] string pIdPersona, [Required] string pNombreCompletoAutor)
        {
            try
            {
                ReadRabbitService rabbitMQService = new ReadRabbitService(_Configuracion);
                                
                if (!string.IsNullOrEmpty(pIdentificador) && !string.IsNullOrEmpty(pDoi) && !string.IsNullOrEmpty(pIdPersona) && !string.IsNullOrEmpty(pNombreCompletoAutor))
                {
                    // Inserción a la cola.
                    List<string> listaDatos = new List<string>() { pIdentificador, pDoi, pIdPersona, pNombreCompletoAutor };
                    rabbitMQService.PublishMessage(listaDatos, _Configuracion.GetDoiQueueRabbit());

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.Error($"{DateTime.Now} - {ex.Message}\n{ex.StackTrace}\n");
            }

            return false;
        }

        /// <summary>
        /// A Http calls function.
        /// </summary>
        /// <param name="url">The http call URL.</param>
        /// <param name="method">Crud method for the call.</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        protected async Task<string> httpCall(string url, string method = "GET", Dictionary<string, string> headers = null)
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
                httpClient.Timeout = TimeSpan.FromHours(24);
                using (var request = new HttpRequestMessage(new HttpMethod(method), url))
                {
                    request.Headers.TryAddWithoutValidation("Accept", "application/json");

                    if (headers != null && headers.Count > 0)
                    {
                        foreach (var item in headers)
                        {
                            request.Headers.TryAddWithoutValidation(item.Key, item.Value);
                        }
                    }
                    response = await httpClient.SendAsync(request);
                }
            }

            if (response.Content != null)
            {
                return await response.Content.ReadAsStringAsync();
            }

            else
            {
                return string.Empty;
            }
        }
    }
}
