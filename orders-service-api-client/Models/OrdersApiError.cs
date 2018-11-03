using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OrdersService.ApiClient.Models
{
    public class OrdersApiError : Exception
    {
        List<ValidationResult> ValidationErrors { get; set; }

        public OrdersApiError()
        {

        }

        public OrdersApiError(string message, Exception exc) : base(message, exc)
        { }

        public OrdersApiError(List<ValidationResult> errors) : base("Invalid input")
        {
            ValidationErrors = errors;
        }
    }
}
