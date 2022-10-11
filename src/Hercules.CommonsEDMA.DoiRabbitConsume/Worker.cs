using Gnoss.Web.ReprocessData.Models.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Hercules.CommonsEDMA.DoiRabbitConsume.Program;

namespace Hercules.CommonsEDMA.DoiRabbitConsume
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceScopeFactory"></param>
        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <summary>
        /// Tarea asincrona.
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ListenToQueue();
        }

        /// <summary>
        /// Lectura de una cola Rabbit.
        /// </summary>
        private void ListenToQueue()
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                ConfigService configService = scope.ServiceProvider.GetRequiredService<ConfigService>();
                ReadRabbitService rabbitMQService = scope.ServiceProvider.GetRequiredService<ReadRabbitService>();
                rabbitMQService.ListenToQueue(new ReadRabbitService.ReceivedDelegate(rabbitMQService.ProcessItem), new ReadRabbitService.ShutDownDelegate(OnShutDown), configService.GetQueueRabbit());
            }
        }

        /// <summary>
        /// Permite lanzar la escucha después de leer. 
        /// Contiene un sleep de 30 segundos.
        /// </summary>
        private void OnShutDown()
        {
            Thread.Sleep(30000);
            ListenToQueue();
        }
    }
}
