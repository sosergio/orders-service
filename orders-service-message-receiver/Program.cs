using System;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.Core.Interfaces;
using OrdersService.Infrastructure.Repositories;
using OrdersService.Core.Config;
using OrdersService.Infrastructure.Providers;
using System.Collections;
using OrdersService.Core.Services;
using System.Threading;

namespace OrdersService.MessageReceiver
{
    class Program
    {
        public static IMessageReceiver Reference { get; set; }

        public static string ReadString(IDictionary dic, string key)
        {
            if (dic[key] == null)
            {
                throw new Exception($"{key} is missing.");
            }

            return dic[key].ToString();
        }

        static void Main(string[] args)
        {
            try
            {
                var envVars = Environment.GetEnvironmentVariables();
                Console.WriteLine($"RabbitMq HostName: {ReadString(envVars, "MessageBusConnection")}");
                Console.WriteLine($"Db ConnectionString: {ReadString(envVars, "ConnectionString")}");

                var config = new MessagingConfig()
                {
                    HostName = ReadString(envVars, "MessageBusConnection"),
                    UserName = ReadString(envVars, "MessageBusUserName"),
                    Password = ReadString(envVars, "MessageBusPassword")
                };
                var serviceProvider = new ServiceCollection()
                    .AddSingleton(new DbConfig()
                    {
                        ConnectionString = ReadString(envVars, "ConnectionString"),
                        DatabaseName = ReadString(envVars, "DatabaseName")
                    })
                    .AddSingleton(config)
                    .AddScoped<IRepository<Core.Models.Order>, OrdersRepository>()
                    .AddScoped<IDbProvider, MongoDbProvider>()
                    .AddScoped<IMessageBusProvider, RabbitMqProvider>()
                    .AddScoped<IMessageReceiver, MessageReceiver>()
                    .AddScoped<IOrdersMessageBus, OrdersMessageBus>()
                    .BuildServiceProvider();


                var quit = false;
                try
                {
                    var attempts = 0;
                    while (attempts < 3)
                    {
                        try
                        {
                            var dep1 = serviceProvider.GetService<IOrdersMessageBus>();
                            var dep2 = serviceProvider.GetService<IRepository<Core.Models.Order>>();
                            Reference = new MessageReceiver(dep1, dep2);
                            attempts = 3;
                        }
                        catch(Exception)
                        {
                            Console.WriteLine("Couldn't connect to RabbitMq, retry in 5 sec.");
                            attempts++;
                            Thread.Sleep(5000);
                        }
                    }

                    Console.WriteLine("Job Host Started at: {0:o}", DateTimeOffset.UtcNow);
                    Console.WriteLine("Press Q to exit");


                    while (!quit)
                    {
                        var key = Console.Read();
                        quit = key.ToString() == "Q";
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    quit = true;
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
