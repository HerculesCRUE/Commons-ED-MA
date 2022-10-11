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
    public class RabbitServiceReaderDenormalizer
    {
        private ConfigService _configService;
        private readonly ConnectionFactory connectionFactory;
        private readonly IConnection connection;

        /// <summary>
        /// ReceivedDelegate.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public delegate bool ReceivedDelegate(string s);

        /// <summary>
        /// ShutDownDelegate.
        /// </summary>
        public delegate void ShutDownDelegate();

        /// <summary>
        /// Contructor.
        /// </summary>
        /// <param name="configService"></param>
        public RabbitServiceReaderDenormalizer(ConfigService configService)
        {
            _configService = configService;
            connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_configService.GetrabbitConnectionString())
            };

            connection = connectionFactory.CreateConnection();
        }

        /// <summary>
        /// ListenToQueue.
        /// </summary>
        /// <param name="receivedFunction"></param>
        /// <param name="shutdownFunction"></param>
        public void ListenToQueue(ReceivedDelegate receivedFunction, ShutDownDelegate shutdownFunction, string queue)
        {
            IModel channel = connection.CreateModel();
            channel.BasicQos(0, 1, false);

            channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            EventingBasicConsumer eventingBasicConsumer = new EventingBasicConsumer(channel);

            eventingBasicConsumer.Received += (sender, basicDeliveryEventArgs) =>
            {
                try
                {
                    IBasicProperties basicProperties = basicDeliveryEventArgs.BasicProperties;
                    string body = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body.ToArray());

                    if (receivedFunction(body))
                    {
                        channel.BasicAck(basicDeliveryEventArgs.DeliveryTag, false);
                    }
                    else
                    {
                        channel.BasicNack(basicDeliveryEventArgs.DeliveryTag, false, true);
                    }
                }
                catch (Exception)
                {
                    channel.BasicNack(basicDeliveryEventArgs.DeliveryTag, false, true);
                    throw;
                }
            };

            eventingBasicConsumer.Shutdown += (sender, shutdownEventArgs) =>
            {
                shutdownFunction();
            };
            channel.BasicConsume(queue, false, eventingBasicConsumer);
        }        
    }
}

