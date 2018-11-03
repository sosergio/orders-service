using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Api.ApiModels;
using OrdersService.Core;

namespace OrdersService.Api.Controllers
{
    [Route("api/orders/{orderId}/items")]
    public class OrderItemsController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly IMapper _mapper;

        public OrderItemsController(IOrdersService ordersService, IMapper mapper)
        {
            _mapper = mapper;
            _ordersService = ordersService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiModels.Order), 201)]
        public async Task<IActionResult> AddItem(
            [XReference] string reference, 
            [XUserId] string userId, 
            string orderId, 
            [FromBody]AddOrderItemRequest orderRequest)
        {
            var item = _mapper.Map<Core.Models.OrderItem>(orderRequest);
            var order = await _ordersService.AddItem(orderId, item);
            return Created($"/{order.Id}", _mapper.Map<ApiModels.Order>(order));
        }

        [HttpDelete("{itemId}")]
        [ProducesResponseType(typeof(ApiModels.Order), 200)]
        public async Task<IActionResult> RemoveItem(
            [XReference] string reference,
            [XUserId] string userId, 
            string orderId, 
            string itemId)
        {
            return Ok(_mapper.Map<ApiModels.Order>(await _ordersService.RemoveItem(orderId, itemId)));
        }
    }
}
