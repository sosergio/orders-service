using System;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.Core.Interfaces;
using OrdersService.Infrastructure.Repositories;
using OrdersService.Core.Config;
using OrdersService.Infrastructure.Providers;

namespace OrdersService.MessageReceiver
{
    class Program
    {
        public static IMessageReceiver Reference { get; set; }
        static void Main(string[] args)
        {
            try
            {
                var config = new MessagingConfig()
                {
                    HostName = "localhost"
                };
                var serviceProvider = new ServiceCollection()
                    .AddSingleton(new DbConfig() { ConnectionString = "your-connection-string" })
                    .AddSingleton(config)
                    .AddScoped<IRepository<Core.Models.Order>, OrdersRepository>()
                    .AddScoped<IDbProvider, InMemoryDbProvider>()
                    .AddScoped<IMessageBusProvider, RabbitMqProvider>()
                    .AddSingleton<IMessageReceiver, MessageReceiver>()
                    .BuildServiceProvider();


                var dep1 = serviceProvider.GetService<IMessageBusProvider>();
                var dep2 = serviceProvider.GetService<IRepository<Core.Models.Order>>();
                Reference = new MessageReceiver(dep1, dep2);

                Console.WriteLine("Job Host Started at: {0:o}", DateTimeOffset.UtcNow);
                Console.WriteLine("Press Q to exit");

                var quit = false;
                while (!quit)
                {
                    var key = Console.ReadKey();
                    quit = key.ToString() == "Q";
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Job Host Stopped at: {0:o}", DateTimeOffset.UtcNow);
                throw;
            }
        }
    }
}
