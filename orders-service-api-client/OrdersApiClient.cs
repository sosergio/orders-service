using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using OrdersService.ApiClient.Models;
using Ardalis.GuardClauses;
using Newtonsoft.Json;
using System.Linq;

namespace OrdersService.ApiClient
{
    public class OrdersApiClient : IOrdersApiClient
    {
        private readonly OrdersApiConfig _config;

        public OrdersApiClient(OrdersApiConfig appConfig)
        {
            _config = appConfig;
            if (_config.UseSslCertificate)
            {
                if (string.IsNullOrEmpty(_config.CertificateThumbprint)) throw new OrdersApiError("Missing certificate thumbprint", null);
                FlurlHttp.Configure(settings =>
                {
                    settings.HttpClientFactory = new SslHttpClientFactory(_config.CertificateThumbprint);
                });
            }
        }

        private IFlurlRequest BaseOrdersRequest(string userId)
        {
            var baseUrl = _config.BaseUrl;
            if (!String.IsNullOrEmpty(baseUrl))
            {
                if (baseUrl.EndsWith('/'))
                {
                    baseUrl = baseUrl.Substring(0, baseUrl.Length - 2);
                }
            }
            return $"{baseUrl}".WithHeaders(new
            {
                Accept = "application/json",
                X_Reference = _config.Reference,
                X_User_Id = userId,
                Content_Type = "application/json"
            }).AppendPathSegment("orders");
        }

        #region Little Helpers

        private void ValidateModel(object data)
        {
            var context = new ValidationContext(data);
            var validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(data, context, validationResults, true);

            if (!isValid)
            {
                throw new OrdersApiError(validationResults);
            }
        }

        private async Task<T> RunAndCatchAsync<T>(Func<Task<T>> fn)
        {
            try
            {
                return await fn();
            }
            catch (FlurlHttpException ex)
            {
                throw new OrdersApiError($"Http Error: {ex.Message}", ex);
            }
            catch (ArgumentNullException ex)
            {
                throw new OrdersApiError($"Argument Error: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new OrdersApiError(ex.Message, ex);
            }
        }
        #endregion

        public async Task<Order> CreateOrder(CreateOrderRequest request)
        {
            return await RunAndCatchAsync(() =>
            {
                Guard.Against.Null(request, nameof(CreateOrderRequest));
                ValidateModel(request);
                request.items.ForEach(item => {
                    ValidateModel(item);
                });
                return BaseOrdersRequest(request.UserId)
                    .PostJsonAsync(request)
                    .ReceiveJson<Order>();
            });
        }
        
        public async Task<List<Order>> ListByUser(OrderRequest request)
        {
            return await RunAndCatchAsync(async () =>
             {
                 Guard.Against.Null(request, nameof(CreateOrderRequest));
                 ValidateModel(request);
                 var items = await BaseOrdersRequest(request.UserId)
                     .GetJsonAsync<List<Order>>();
                 return items;
             });
        }

        public async Task<Order> LoadById(OrderRequest request)
        {
            return await RunAndCatchAsync(() =>
            {
                Guard.Against.Null(request, nameof(OrderRequest));
                return BaseOrdersRequest(request.UserId)
                    .AppendPathSegment(request.OrderId)
                    .GetJsonAsync<Order>();
            });
        }

        public async Task<Order> Cancel(OrderRequest request)
        {
            return await RunAndCatchAsync(() =>
            {
                Guard.Against.Null(request, nameof(OrderRequest));
                return BaseOrdersRequest(request.UserId)
                    .AppendPathSegments(request.OrderId, "cancel")
                    .PutAsync(null)
                    .ReceiveJson<Order>();
            });
        }

        public async Task<Order> AddItem(AddOrderItemRequest request)
        {
            return await RunAndCatchAsync(() =>
            {
                Guard.Against.Null(request, nameof(OrderRequest));
                return BaseOrdersRequest(request.UserId)
                    .AppendPathSegments(request.OrderId, "items")
                    .PostJsonAsync(request)
                    .ReceiveJson<Order>();
            });
        }

        public async Task<Order> RemoveItem(RemoveOrderItemRequest request)
        {
            return await RunAndCatchAsync(() =>
            {
                Guard.Against.Null(request, nameof(OrderRequest));
                return BaseOrdersRequest(request.UserId)
                    .AppendPathSegments(request.OrderId, "items", request.ItemId)
                    .DeleteAsync()
                    .ReceiveJson<Order>();
            });
        }

        public async Task<Order> Submit(OrderRequest request)
        {
            return await RunAndCatchAsync(() => 
            {
                Guard.Against.Null(request, nameof(OrderRequest));
                return BaseOrdersRequest(request.UserId)
                    .AppendPathSegments(request.OrderId, "submit")
                    .PutAsync(null)
                    .ReceiveJson<Order>();
            });
        }
    }
}