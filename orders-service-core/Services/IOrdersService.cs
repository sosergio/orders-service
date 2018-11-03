using OrdersService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Core
{
    public interface IOrdersService
    {
        Task<List<Order>> ListByUser(string userId);
        Task<Order> LoadById(string orderId);
        Task<Order> Cancel(string orderId);
        Task<Order> Create(string referemce, string userId, List<OrderItem> items);
        Task<Order> AddItem(string orderId, OrderItem item);
        Task<Order> RemoveItem(string orderId, string orderItemId);
        Task<Order> Submit(string orderId);
    }
}
