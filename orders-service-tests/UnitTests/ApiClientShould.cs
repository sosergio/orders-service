using AutoFixture;
using AutoFixture.AutoNSubstitute;
using FluentAssertions;
using Flurl.Http.Testing;
using NSubstitute.ExceptionExtensions;
using OrdersService.Api.ApiModels;
using OrdersService.ApiClient;
using OrdersService.ApiClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace OrdersService.Tests.UnitTests
{
    public class ApiClientShould
    {
        IOrdersApiClient _sut;
        OrdersApiConfig ApiConfig { get; } = new OrdersApiConfig()
        {
            BaseUrl = "http://api-url.com",
            UseSslCertificate = false,
            Reference = "reference-abc"
        };
        IFixture _fixture;
        private readonly ITestOutputHelper _output;

        public ApiClientShould(ITestOutputHelper output)
        {
            _output = output;
            _sut = new OrdersApiClient(ApiConfig);
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        [Fact]
        async void PostToOrders_WhenCreateOrder()
        {
            using (var httpTest = new HttpTest())
            {
                // arrange
                var request = _fixture.Create<ApiClient.Models.CreateOrderRequest>();
                var response = _fixture.Create<ApiClient.Models.Order>();
                httpTest.RespondWithJson(response, 201);

                // act
                var result = await _sut.CreateOrder(request);
                LogCalls(httpTest);

                // assert
                httpTest.ShouldHaveCalled($"{ApiConfig.BaseUrl}/orders")
                    .WithVerb(HttpMethod.Post)
                    .WithRequestJson(request)
                    .WithHeader("x-user-id", request.UserId)
                    .WithHeader("x-reference", ApiConfig.Reference)
                    .WithContentType("application/json")
                    .Times(1);
            }
        }

        [Fact]
        async Task Throw_WhenCreateOrderIsInvalid()
        {
            // act
            await Assert.ThrowsAsync<OrdersApiError>(async() => await _sut.CreateOrder(null));
        }

        [Fact]
        async void PostToOrderItems_WhenAddOrderItem()
        {
            using (var httpTest = new HttpTest())
            {
                // arrange
                var request = _fixture.Create<ApiClient.Models.AddOrderItemRequest>();
                var response = _fixture.Create<ApiClient.Models.Order>();
                httpTest.RespondWithJson(response, 201);

                // act
                var result = await _sut.AddItem(request);
                LogCalls(httpTest);

                // assert
                httpTest.ShouldHaveCalled($"{ApiConfig.BaseUrl}/orders/{request.OrderId}/items")
                    .WithVerb(HttpMethod.Post)
                    .WithRequestJson(request)
                    .WithHeader("x-user-id", request.UserId)
                    .WithHeader("x-reference", ApiConfig.Reference)
                    .WithContentType("application/json")
                    .Times(1);
            }
        }

        [Fact]
        async void PutToOrderWithId_WhenSubmitOrder()
        {
            using (var httpTest = new HttpTest())
            {
                // arrange
                var request = _fixture.Create<ApiClient.Models.OrderRequest>();
                var response = _fixture.Create<ApiClient.Models.Order>();
                httpTest.RespondWithJson(response, 200);

                // act
                var result = await _sut.Submit(request);
                LogCalls(httpTest);

                // assert
                httpTest.ShouldHaveCalled($"{ApiConfig.BaseUrl}/orders/{request.OrderId}/submit")
                    .WithVerb(HttpMethod.Put)
                    .WithHeader("x-user-id", request.UserId)
                    .WithHeader("x-reference", ApiConfig.Reference)
                    .WithContentType("application/json")
                    .Times(1);
            }
        }

        [Fact]
        async void PutToOrderWithId_WhenCancelOrder()
        {
            using (var httpTest = new HttpTest())
            {
                // arrange
                var request = _fixture.Create<ApiClient.Models.OrderRequest>();
                var response = _fixture.Create<ApiClient.Models.Order>();
                httpTest.RespondWithJson(response, 200);

                // act
                var result = await _sut.Cancel(request);
                LogCalls(httpTest);

                // assert
                httpTest.ShouldHaveCalled($"{ApiConfig.BaseUrl}/orders/{request.OrderId}/cancel")
                    .WithVerb(HttpMethod.Put)
                    .WithHeader("x-user-id", request.UserId)
                    .WithHeader("x-reference", ApiConfig.Reference)
                    .WithContentType("application/json")
                    .Times(1);
            }
        }

        [Fact]
        async void DeleteToOrderItemsWithId_WhenRemoveOrderItem()
        {
            using (var httpTest = new HttpTest())
            {
                // arrange
                var request = _fixture.Create<ApiClient.Models.RemoveOrderItemRequest>();
                var response = _fixture.Create<ApiClient.Models.Order>();
                httpTest.RespondWithJson(response, 200);

                // act
                var result = await _sut.RemoveItem(request);
                LogCalls(httpTest);

                // assert
                httpTest.ShouldHaveCalled($"{ApiConfig.BaseUrl}/orders/{request.OrderId}/items/{request.ItemId}")
                    .WithVerb(HttpMethod.Delete)
                    .WithHeader("x-user-id", request.UserId)
                    .WithHeader("x-reference", ApiConfig.Reference)
                    .WithContentType("application/json")
                    .Times(1);
            }
        }

        [Fact]
        async void GetToOrders_WhenListOrderByUser()
        {
            using (var httpTest = new HttpTest())
            {
                // arrange
                var request = _fixture.Create<ApiClient.Models.OrderRequest>();
                var response = _fixture.Create<List<ApiClient.Models.Order>>();
                httpTest.RespondWithJson(response, 200);

                // act
                var result = await _sut.ListByUser(request);
                LogCalls(httpTest);

                // assert
                httpTest.ShouldHaveCalled($"{ApiConfig.BaseUrl}/orders")
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("x-user-id", request.UserId)
                    .WithHeader("x-reference", ApiConfig.Reference)
                    .WithContentType("application/json")
                    .Times(1);
            }
        }

        [Fact]
        async void GetToOrderWithId_WhenLoadOrdeById()
        {
            using (var httpTest = new HttpTest())
            {
                // arrange
                var request = _fixture.Create<ApiClient.Models.OrderRequest>();
                var response = _fixture.Create<ApiClient.Models.Order>();
                httpTest.RespondWithJson(response, 200);

                // act
                var result = await _sut.LoadById(request);
                LogCalls(httpTest);

                // assert
                httpTest.ShouldHaveCalled($"{ApiConfig.BaseUrl}/orders/{request.OrderId}")
                    .WithVerb(HttpMethod.Get)
                    .WithHeader("x-user-id", request.UserId)
                    .WithHeader("x-reference", ApiConfig.Reference)
                    .WithContentType("application/json")
                    .Times(1);
            }
        }

        private void LogCalls(HttpTest httpTest)
        {
            _output.WriteLine($"Urls: {string.Join(" ", httpTest.CallLog.Select(l => l.Request))}");
        }
    }
}
