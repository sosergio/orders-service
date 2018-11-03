using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Core.Models.Errors
{
    public class ApplicationError : Exception
    {
        public ErrorCode Error { get; }
        public string Description { get;  }
        public object Details { get; }

        public ApplicationError() { }
        
        protected ApplicationError(ErrorCode error, string description)
        {
            Error = error;
            Description = description;
            Details = StackTrace;
        }

        public ApplicationError(ErrorCode error, string description, object details): this(error, description)
        {
            Details = details;
        }
    }
}
