using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OrdersService.ApiClient.Models
{
    public class CreateOrderItem
    {
        [Required]
        public string ItemId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
    }
}
