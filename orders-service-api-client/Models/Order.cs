using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace OrdersService.ApiClient.Models
{
    public class Order
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("reference")]
        public string Reference { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }
        [JsonProperty("items")]
        public List<OrderItem> Items { get; set; }
    }
}
