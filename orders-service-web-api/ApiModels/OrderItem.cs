using Newtonsoft.Json;

namespace OrdersService.Api.ApiModels
{
    public class OrderItem
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
