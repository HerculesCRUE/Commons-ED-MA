using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Hercules.CommonsEDMA.ServicioExterno.Controllers
{
    public class ReadRabbitService
    {
        protected Dictionary<string, string> headers = new();

        /// <summary>
        /// ReceivedDelegate
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public delegate bool ReceivedDelegate(string s);

        /// <summary>
        /// ShutDownDelegate
        /// </summary>
        public delegate void ShutDownDelegate();
        private ConfigService _configService;
        private readonly ConnectionFactory connectionFactory;
        private readonly IConnection connection;

        public ReadRabbitService(ConfigService configService)
        {
            _configService = configService;
            connectionFactory = new ConnectionFactory
            {
                Uri = new Uri(_configService.GetRabbitConnectionString())
            };

            connection = connectionFactory.CreateConnection();
        }


        /// <summary>
        /// Encola un objeto en Rabbit
        /// </summary>
        /// <param name="message">Objeto a encolar</param>
        /// <param name="queue">Cola</param>
        public void PublishMessage(object message, string queue)
        {
            using (var conn = connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: queue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var jsonPayload = JsonConvert.SerializeObject(message);
                    var body = Encoding.UTF8.GetBytes(jsonPayload);

                    channel.BasicPublish(exchange: "",
                        routingKey: queue,
                        basicProperties: null,
                        body: body
                    );
                }
            }
        }

        /// <summary>
        /// ListenToQueue
        /// </summary>
        /// <param name="receivedFunction"></param>
        /// <param name="shutdownFunction"></param>
        /// <param name="queue"></param>
        public void ListenToQueue(ReceivedDelegate receivedFunction, ShutDownDelegate shutdownFunction, string queue)
        {
            IModel channel = connection.CreateModel();
            channel.BasicQos(0, 1, false);

            channel.QueueDeclare(queue: queue,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            EventingBasicConsumer eventingBasicConsumer = new(channel);

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

        /// <summary>
        /// A Http calls function
        /// </summary>
        /// <param name="url">the http call url</param>
        /// <param name="method">Crud method for the call</param>
        /// <param name="headers"></param>
        /// <returns></returns>
        protected async Task<string> httpCall(string url, string method = "GET", Dictionary<string, string> headers = null)
        {
            HttpResponseMessage response;
            using (var httpClient = new HttpClient())
            {
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
                    try
                    {
                        response = await httpClient.SendAsync(request);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            if (response.Content != null)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return "";
            }
        }
    }
}
