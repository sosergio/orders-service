using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace OrdersService.ApiClient.Models
{
    public class RemoveOrderItemRequest : OrderRequest
    {
        [Required]
        public string ItemId { get; set; }
    }
}
