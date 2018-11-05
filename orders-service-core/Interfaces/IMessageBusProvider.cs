using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrdersService.Core.Interfaces
{
    public interface IMessageBusProvider
    {
        void ReceiveFromTopicQueue<T>(string queueName, string exchangeName, string routingKey, Func<T,Task<T>> function);
        void SendToTopicQueue(string queueName, string exchangeName, string routingKey, object message); 
    }
}
