using OrdersService.Core.Interfaces;
using OrdersService.Core.Models;
using System.Threading.Tasks;

namespace OrdersService.MessageReceiver
{
    public class MessageReceiver:IMessageReceiver
    {
        IRepository<Order> _ordersRepository;
        IMessageBusProvider _messageBusProvider;

        public MessageReceiver(IMessageBusProvider messageBusProvider, IRepository<Order> ordersRepository)
        {
            _ordersRepository = ordersRepository;
            _messageBusProvider = messageBusProvider;
            _messageBusProvider.Receive<Order>("write-order", OnWriteOrder);
        }

        public async Task<Order> OnWriteOrder(Order order)
        {
            return await _ordersRepository.SaveAsync(order);
        }
    }
}
