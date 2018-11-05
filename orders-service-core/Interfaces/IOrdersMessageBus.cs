using OrdersService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Core.Services
{
    public interface IOrdersMessageBus
    {
        bool WriteOrder(Order order);
        bool ReceiveWriteOrder(Func<Order,Task<Order>> onReceive);
    }
}
