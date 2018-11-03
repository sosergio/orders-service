using System;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OrdersService.Api.ApiModels
{

    public sealed class ApiErrorResponse
    {
        public ApiErrorResponse()
        { }

        public ApiErrorResponse(Exception exc)
        {
            Code = HttpStatusCode.InternalServerError;
            Message = "Something went wrong";
            Details = new
            {
                message = exc.Message,
                stackTrace = exc.StackTrace
            };
            Error = "INTERNAL_SERVER_ERROR";
        }

        public HttpStatusCode Code { get; set; }

        public string Error { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
        [DefaultValue(null)]
        public object Details { get; set; }

        public async Task WriteToHttpContext(HttpContext context)
        {
            var serializer = new JsonSerializer();
            serializer.ContractResolver = new CamelCasePropertyNamesContractResolver();
            context.Response.StatusCode = (int)Code;
            context.Response.ContentType = "application/json";
            using (var writer = new System.IO.StreamWriter(context.Response.Body))
            {
                serializer.Serialize(writer, this);
                await writer.FlushAsync().ConfigureAwait(false);
            }
        }

    }
}
