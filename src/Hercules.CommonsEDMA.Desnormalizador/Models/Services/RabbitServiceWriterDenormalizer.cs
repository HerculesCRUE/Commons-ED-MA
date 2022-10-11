using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading;

namespace Hercules.CommonsEDMA.Desnormalizador.Models.Services
{
    public class RabbitServiceWriterDenormalizer
    {
        private ConfigService _configService;
        private readonly ConnectionFactory connectionFactory;


        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="configService"></param>
        public RabbitServiceWriterDenormalizer(ConfigService configService)
        {
            _configService = configService;
            connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_configService.GetrabbitConnectionString())
            };
        }


        /// <summary>
        /// Encola un objeto en Rabbit.
        /// </summary>
        /// <param name="message">Objeto a encolar</param>
        /// <param name="queue">Cola</param>
        public void PublishMessage(DenormalizerItemQueue item)
        {
            using (var conn = connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: _configService.GetDenormalizerQueueRabbit(),
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var jsonPayload = JsonConvert.SerializeObject(item);
                    var body = Encoding.UTF8.GetBytes(jsonPayload);

                    channel.BasicPublish(exchange: string.Empty,
                        routingKey: _configService.GetDenormalizerQueueRabbit(),
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }
    }
}

