using Newtonsoft.Json;

namespace OrdersService.Api.ApiModels
{
    public class OrderItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
