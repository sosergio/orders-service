using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using OrdersService.Api;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace OrdersService.Tests.FunctionalTests
{
    public class OrdersControllerShould
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public OrdersControllerShould()
        {
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Development"));
            _client = _server.CreateClient();
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }

        [Fact]
        public async Task Index_Get_ReturnsIndexHtmlPage()
        {
            // Act
            var response = await _client.GetAsync("/api/orders");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("<title>Home Page - BlogPlayground</title>", responseString);
        }
    }

    //private readonly TestServer _server;
    //private readonly HttpClient _client;

    //public OrderGetRequestShould()
    //{
    //    // Arrange
    //    _server = new TestServer(new WebHostBuilder()
    //       .UseStartup<Startup>());
    //    _client = _server.CreateClient();
    //}

    //[Fact]
    //public async Task ReturnHelloWorld()
    //{
    //    // Act
    //    var response = await _client.GetAsync("/api/orders");
    //    response.EnsureSuccessStatusCode();
    //    var responseString = await response.Content.ReadAsStringAsync();
    //    // Assert
    //    Assert.Equal("Hello World!", responseString);
    //}

}
