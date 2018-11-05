using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrdersService.Api.ApiModels
{
    public class AddOrderItemRequest
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("price")]
        public decimal Price { get; set; }
    }
}
