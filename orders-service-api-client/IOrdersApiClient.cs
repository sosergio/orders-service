using OrdersService.ApiClient.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrdersService.ApiClient
{
    public interface IOrdersApiClient
    {
        Task<Order> CreateOrder(CreateOrderRequest request);
        Task<List<Order>> ListByUser(OrderRequest request);
        Task<Order> LoadById(OrderRequest request);
        Task<Order> Cancel(OrderRequest request);
        Task<Order> AddItem(AddOrderItemRequest request);
        Task<Order> RemoveItem(RemoveOrderItemRequest request);
        Task<Order> Submit(OrderRequest request);
    }
}