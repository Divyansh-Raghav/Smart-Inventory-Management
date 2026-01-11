using Microsoft.AspNetCore.Mvc;
using SmartInventoryManager.Api.DTOs;
using SmartInventoryManager.Api.Services;

namespace SmartInventoryManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder(CreateOrderDto dto)
        {
            try
            {
                var order = await _orderService.CreateOrderAsync(dto);
                return CreatedAtAction(nameof(GetOrders), order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderResponseDto>>> GetOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
    }
}
