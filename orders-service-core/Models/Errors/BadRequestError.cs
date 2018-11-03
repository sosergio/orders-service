using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Core.Models.Errors
{
    public class BadRequestError:ApplicationError
    {
        public BadRequestError():base(ErrorCode.BadRequest, "Something wrong with the request")
        {}
    }
}
