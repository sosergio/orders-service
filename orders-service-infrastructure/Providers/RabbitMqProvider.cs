using Newtonsoft.Json;
using OrdersService.Core.Config;
using OrdersService.Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Infrastructure.Providers
{
    public class RabbitMqProvider : IMessageBusProvider
    {
        MessagingConfig _config;
        public RabbitMqProvider(MessagingConfig config)
        {
            _config = config;
        }
        
        public void Receive<T>(string channelId, Func<T,Task<T>> function)
        {
            var factory = new ConnectionFactory() { HostName = _config.HostName, UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("orderEx", "topic");
                channel.QueueDeclare(queue: channelId,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);
                channel.QueueBind(channelId, "orderEx", "orders");

                Subscription subscription = new Subscription(channel,
                        channelId, false);
                channel.BasicQos(0, 10, false);
                while (true)
                {
                    try
                    {
                        BasicDeliverEventArgs deliveryArguments = subscription.Next();
                        var json = Encoding.Default.GetString(deliveryArguments.Body);
                        Console.WriteLine(" [x] Received {0}", json);
                        var typed = JsonConvert.DeserializeObject<T>(json);
                        function(typed);
                        subscription.Ack(deliveryArguments);
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
            }
        }

        public void Send(string channelId, object message)
        {
            var factory = new ConnectionFactory() { HostName = _config.HostName, UserName = "guest", Password = "guest" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare("orderEx", "topic");
                channel.QueueDeclare(queue: channelId,
                                    durable: false,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

                channel.QueueBind(channelId, "orderEx", "orders");
                
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

                channel.BasicPublish(exchange: "orderEx",
                                     routingKey: "orders",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
