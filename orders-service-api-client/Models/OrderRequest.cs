using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrdersService.ApiClient.Models
{
    public class OrderRequest
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string OrderId { get; set; }
    }
}
