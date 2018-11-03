using OrdersService.Core.Models;
using System.Threading.Tasks;

namespace OrdersService.MessageReceiver
{
    public interface IMessageReceiver
    {
        Task<Order> OnWriteOrder(Order order);
    }
}
