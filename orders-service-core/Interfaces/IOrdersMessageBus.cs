using OrdersService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Core.Services
{
    public interface IOrdersMessageBus
    {
        void WriteOrder(Order order);
        void ReceiveWriteOrder(Func<Order,Task<Order>> onReceive);
    }
}
