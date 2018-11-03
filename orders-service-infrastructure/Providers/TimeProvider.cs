using OrdersService.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Infrastructure.Providers
{
    public class UtcTimeProvider : ITimeProvider
    {
        public DateTimeOffset Now()
        {
            return DateTimeOffset.UtcNow;
        }
    }
}
