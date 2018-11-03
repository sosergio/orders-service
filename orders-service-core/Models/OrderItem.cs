﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersService.Core.Models
{
    public class OrderItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("description")]
        public string Description { get; private set; }
        [JsonProperty("price")]
        public decimal Price { get; private set; }

        public OrderItem()
        {}

    }
}
