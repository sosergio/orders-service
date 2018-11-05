using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OrdersService.Api.ApiModels;
using OrdersService.Core.Models.Errors;
using System.Net;

namespace OrdersService.Api.Middleware
{
    public class JsonExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public JsonExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHostingEnvironment env)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, env, ex);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, IHostingEnvironment env, Exception exception)
        {
            ApiErrorResponse error;
            var appError = exception as ApplicationError;
            if (appError != null)
            {
                var httpCode = HttpStatusCode.InternalServerError;
                switch (appError.Error)
                {
                    case ErrorCode.BadRequest:
                        httpCode = HttpStatusCode.BadRequest;
                        break;
                    case ErrorCode.NotFound:
                        httpCode = HttpStatusCode.NotFound;
                        break;
                    case ErrorCode.ProxyError:
                        httpCode = HttpStatusCode.ServiceUnavailable;
                        break;
                }
                error = new ApiErrorResponse()
                {
                    Code = httpCode,
                    Details = appError.Details,
                    Message = appError.Description,
                    Error = appError.Error.ToString()
                };
            }
            else
            {
                error = new ApiErrorResponse(exception);
            };

            if (!env.IsDevelopment())
            {
                error.Details = null;
            }

            await error.WriteToHttpContext(context);
        }
    }
}
