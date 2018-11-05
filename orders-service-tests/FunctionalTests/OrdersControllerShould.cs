using AutoFixture;
using AutoFixture.AutoNSubstitute;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NSubstitute;
using OrdersService.Api;
using OrdersService.Api.ApiModels;
using OrdersService.Core.Interfaces;
using OrdersService.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace OrdersService.Tests.FunctionalTests
{
    public class OrdersControllerShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        IFixture _fixture;
        public OrdersControllerShould()
        {
            Environment.SetEnvironmentVariable("ConnectionString", "fake");
            Environment.SetEnvironmentVariable("DatabaseName", "fake");
            Environment.SetEnvironmentVariable("MessageBusConnection", "fake");
            Environment.SetEnvironmentVariable("MessageBusUserName", "fake");
            Environment.SetEnvironmentVariable("MessageBusPassword", "fake");

            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
            var eventBusMock = Substitute.For<IMessageBusProvider>();
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development")
                .ConfigureTestServices(s => s
                    .AddTransient<IDbProvider, InMemoryDbProvider>()
                    .AddTransient<IMessageBusProvider>(x => eventBusMock)
                    ));
            _client = _server.CreateClient();
            
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        [Fact]
        public async Task ReturnError_WhenUserIdIsMissing()
        {
            // Act
            var response = await _client.GetAsync("/api/orders");

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task GetOrdersByUserId()
        {
            // Act
            _client.DefaultRequestHeaders.Add("x-reference", "1");
            _client.DefaultRequestHeaders.Add("x-user-id", "1");
            var response = await _client.GetAsync("/api/orders");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseString = await response.Content.ReadAsStringAsync();
            var orders = JsonConvert.DeserializeObject<List<Order>>(responseString);
            Assert.NotNull(orders);
        }
    }
}
