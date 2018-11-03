using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OrdersService.ApiClient.Models
{
    public class AddOrderItemRequest : OrderRequest
    {
        [Required]
        public string ItemId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
    }
}
