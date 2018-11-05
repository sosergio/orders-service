using System;
using System.Collections;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OrdersService.Api.ApiModels;
using OrdersService.Api.Middleware;
using OrdersService.Core;
using OrdersService.Core.Config;
using OrdersService.Core.Interfaces;
using OrdersService.Core.Services;
using OrdersService.Infrastructure.Providers;
using OrdersService.Infrastructure.Repositories;
using Swashbuckle.AspNetCore.Swagger;

namespace OrdersService.Api
{
    public class Startup
    {
        IServiceCollection _services;

        public static string ReadString(IDictionary dic, string key)
        {
            if (dic[key] == null)
            {
                throw new Exception($"{key} is missing.");
            }

            return dic[key].ToString();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var envVars = Environment.GetEnvironmentVariables();
            var dbConfig = new DbConfig()
            {
                ConnectionString = ReadString(envVars, "ConnectionString"),
                DatabaseName = ReadString(envVars, "DatabaseName")
            };
            services.AddSingleton(dbConfig);
            services.AddSingleton(new MessagingConfig()
            {
                HostName = ReadString(envVars, "MessageBusConnection"),
                UserName = ReadString(envVars, "MessageBusUserName"),
                Password = ReadString(envVars, "MessageBusPassword")
            });

            Console.WriteLine($"RabbitMq HostName: {ReadString(envVars, "MessageBusConnection")}");
            Console.WriteLine($"Db ConnectionString: {ReadString(envVars, "ConnectionString")}");

            services.AddSingleton(BuildMapper());
            services.AddTransient<IRepository<Core.Models.Order>, OrdersRepository>();
            services.AddTransient<IOrdersService, Core.OrdersService>();
            //if (dbConfig.DatabaseName == "fake")
            //{
            //    services.AddTransient<IDbProvider, InMemoryDbProvider>();
            //}
            //else
            //{
                services.AddTransient<IDbProvider, MongoDbProvider>();
            //}
            services.AddTransient<IMessageBusProvider, RabbitMqProvider>();
            services.AddTransient<ITimeProvider, UtcTimeProvider>();
            services.AddTransient<IOrdersMessageBus, OrdersMessageBus>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc("v1", new Info
                {
                    Title = "Orders Service",
                    Version = "v1",
                    Description = "The Order Service HTTP API",
                    TermsOfService = "Read our terms of service"
                });
            });

            _services = services;
        }

        private void ListAllRegisteredServices(IApplicationBuilder app)
        {
            app.Map("/allservices", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>All Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (var svc in _services)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            }));
        }

        public IMapper BuildMapper()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ApiModelsMappingProfile());
            }).CreateMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                ListAllRegisteredServices(app);
            }

            app.UseMiddleware<JsonExceptionMiddleware>();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "The Orders Service Api | v1"));

        }
    }
}
