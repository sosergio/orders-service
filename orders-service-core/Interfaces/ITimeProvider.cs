using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Core.Interfaces
{
    public interface ITimeProvider
    {
        DateTimeOffset Now();
    }
}
