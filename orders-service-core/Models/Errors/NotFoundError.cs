using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Core.Models.Errors
{
    public class NotFoundError:ApplicationError
    {
        public NotFoundError():base(ErrorCode.NotFound, "Resource not found")
        {}
    }
}
