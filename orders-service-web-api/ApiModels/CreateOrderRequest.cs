using Newtonsoft.Json;

namespace OrdersService.Api.ApiModels
{
    public class CreateOrderRequest
    {
        [JsonProperty("items")]
        public OrderItem[] items { get; set; }
    }
}
