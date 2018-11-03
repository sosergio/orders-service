using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrdersService.ApiClient.Models
{
    public class CreateOrderRequest
    {
        [Required]
        public string UserId { get; set; }
        public List<CreateOrderItem> items { get; set; }
    }
}
