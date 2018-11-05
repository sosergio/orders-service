using System;
using System.Threading.Tasks;
using OrdersService.Core.Interfaces;
using OrdersService.Core.Models;

namespace OrdersService.Core.Services
{
    public class OrdersMessageBus : IOrdersMessageBus
    {
        IMessageBusProvider _messageBus;
        private const string WriteOrderQueueName = "write.order";
        private const string DefaultExchangeName = "default";
        private const string AllRoutingKey = "all";
        
        public OrdersMessageBus(IMessageBusProvider messageBus)
        {
            _messageBus = messageBus;
        }

        public void ReceiveWriteOrder(Func<Order, Task<Order>> onReceive)
        {
            _messageBus.ReceiveFromTopicQueue<Order>(WriteOrderQueueName, DefaultExchangeName, AllRoutingKey, onReceive);
        }

        public void WriteOrder(Order order)
        {
            _messageBus.SendToTopicQueue(WriteOrderQueueName, DefaultExchangeName, AllRoutingKey, order);
        }
    }
}
