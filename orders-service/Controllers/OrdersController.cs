using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrdersService.Api.ApiModels;
using OrdersService.Core;

namespace OrdersService.Api.Controllers
{
    [Route("api/orders")]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;
        private readonly IMapper _mapper;

        public OrdersController(IOrdersService ordersService, IMapper mapper)
        {
            _mapper = mapper;
            _ordersService = ordersService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ApiModels.Order>), 200)]
        public async Task<IActionResult> ListByUser(
            [XReference] string reference,
            [XUserId] string userId)
        {
            return Ok(_mapper.Map<List<ApiModels.Order>>(await _ordersService.ListByUser(userId)));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiModels.Order), 200)]
        public async Task<IActionResult> LoadById(
            [XReference] string reference,
            [XUserId] string userId,
            string id)
        {
            return Ok(_mapper.Map<ApiModels.Order>(await _ordersService.LoadById(id)));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ApiModels.Order), 201)]
        public async Task<IActionResult> Create(
            [XReference] string reference,
            [XUserId] string userId,
            [FromBody]CreateOrderRequest orderRequest)
        {
            var items = _mapper.Map<List<Core.Models.OrderItem>>(orderRequest.items);
            var order = await _ordersService.Create(reference, userId, items);
            return Created($"/{order.Id}", _mapper.Map<ApiModels.Order>(order));
        }
        
        [HttpPut("{id}/submit")]
        [ProducesResponseType(typeof(ApiModels.Order), 202)]
        public async Task<IActionResult> Submit(
            [XReference] string reference,
            [XUserId] string userId,
            string id)
        {
            return Accepted(_mapper.Map<ApiModels.Order>(await _ordersService.Submit(id)));
        }
        
        [HttpPut("{id}/cancel")]
        [ProducesResponseType(typeof(ApiModels.Order), 202)]
        public async Task<IActionResult> Cancel(
            [XReference] string reference,
            [XUserId] string userId,
            string id)
        {
            return Accepted(_mapper.Map<ApiModels.Order>(await _ordersService.Cancel(id)));
        }
    }
}
