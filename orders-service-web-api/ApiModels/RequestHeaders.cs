using Microsoft.AspNetCore.Mvc;

namespace OrdersService.Api.ApiModels
{
    public class XUserIdAttribute : FromHeaderAttribute
    {
        public XUserIdAttribute()
        {
            Name = "x-user-id";
        }
    }
    public class XReferenceAttribute : FromHeaderAttribute
    {
        public XReferenceAttribute()
        {
            Name = "x-reference";
        }
    }
}
