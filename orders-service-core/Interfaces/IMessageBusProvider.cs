using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Core.Interfaces
{
    public interface IMessageBusProvider
    {
        void Send(string channelId, object message);
        void Receive<T>(string channelId, Func<T, Task<T>> function);
    }
}
