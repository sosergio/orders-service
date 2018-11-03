using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Core.Models.Errors
{
    public enum ErrorCode
    {
        NotFound,
        BadRequest,
        ProxyError,
        Unknown
    }
}
