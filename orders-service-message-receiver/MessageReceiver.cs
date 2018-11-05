using OrdersService.Core.Interfaces;
using OrdersService.Core.Models;
using OrdersService.Core.Services;
using System.Threading.Tasks;

namespace OrdersService.MessageReceiver
{
    public class MessageReceiver:IMessageReceiver
    {
        IRepository<Order> _ordersRepository;
        
        IOrdersMessageBus _ordersMessageBus;

        public MessageReceiver(IOrdersMessageBus messageBusProvider, IRepository<Order> ordersRepository)
        {
            _ordersRepository = ordersRepository;
            _ordersMessageBus = messageBusProvider;
            _ordersMessageBus.ReceiveWriteOrder(OnWriteOrder);
        }

        public async Task<Order> OnWriteOrder(Order order)
        {
            return await _ordersRepository.SaveAsync(order);
        }
    }
}
