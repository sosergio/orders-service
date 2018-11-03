using Newtonsoft.Json;
using OrdersService.Core.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OrdersService.Core.Models
{
    public class Order : IDbEntity
    {
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
        [JsonProperty("status")]
        public OrderStatus Status { get; private set; }
        
    
        public Order(string reference, string userId, DateTimeOffset date, List<OrderItem> items)
        {
            Reference = reference;
            UserId = userId;
            Date = date;
            Items = items ?? new List<OrderItem>();
            Status = OrderStatus.Draft;
        }

        public void AddItem(OrderItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(string itemId)
        {
            var item = Items.SingleOrDefault(i => i.Id == itemId);
            if (item != null) Items.Remove(item);
        }

        public void Submit()
        {
            Status = OrderStatus.Submitted;
        }

        public void Cancel()
        {
            Status = OrderStatus.Cancelled;
        }
    }
}
