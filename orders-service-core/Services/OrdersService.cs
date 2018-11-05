using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrdersService.Core.Models;
using OrdersService.Core.Models.Errors;
using OrdersService.Core.Interfaces;
using Ardalis.GuardClauses;
using OrdersService.Core.Services;

namespace OrdersService.Core
{
    public class OrdersService : IOrdersService
    {
        IRepository<Order> _odersRepository;
        ITimeProvider _timeProvider;
        IOrdersMessageBus _ordersMessageBus;
        public OrdersService(IRepository<Order> orderRepository, IOrdersMessageBus messageBus, ITimeProvider timeProvider)
        {
            _odersRepository = orderRepository;
            _ordersMessageBus = messageBus;
            _timeProvider = timeProvider;
        }

        public async Task<Order> AddItem(string orderId, OrderItem item)
        {
            var order = await LoadById(orderId);
            order.AddItem(item);
            _ordersMessageBus.WriteOrder(order);
            return order;
        }

        public async Task<Order> Cancel(string orderId)
        {
            var order = await LoadById(orderId);
            order.Cancel();
            _ordersMessageBus.WriteOrder(order);
            return order;
        }

        public async Task<Order> Create(string reference, string userId, List<OrderItem> items)
        {
            var order = new Order(reference, userId, _timeProvider.Now(), items);
            _ordersMessageBus.WriteOrder(order);
            return await Task.FromResult(order);
        }

        public async Task<List<Order>> ListByUser(string userId)
        {
            Guard.Against.NullOrEmpty(userId, nameof(userId));
            return (await _odersRepository.Search(e => e.UserId == userId)).ToList();
        }

        public async Task<Order> LoadById(string orderId)
        {
            var entity = await _odersRepository.LoadById(orderId);
            if (entity == null) throw new NotFoundError();
            return entity;
        }

        public async Task<Order> RemoveItem(string orderId, string orderItemId)
        {
            Guard.Against.NullOrEmpty(orderId, nameof(orderId));
            Guard.Against.NullOrEmpty(orderItemId, nameof(orderItemId));
            var order = await LoadById(orderId);
            order.RemoveItem(orderItemId);
            _ordersMessageBus.WriteOrder(order);
            return order;
        }

        public async Task<Order> Submit(string orderId)
        {
            var order = await LoadById(orderId);
            order.Submit();
            _ordersMessageBus.WriteOrder(order);
            return order;
        }

    }
}
