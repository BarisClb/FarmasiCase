using FarmasiCase.Domain.Entities;
using FarmasiCase.Service.Dtos.Read;
using FarmasiCase.Service.Dtos.Redis;
using FarmasiCase.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmasiCase.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _orderService.Get());
        }

        [HttpPost("Order")]
        public async Task<IActionResult> Post()
        {
            string? jwt = Request.Cookies[$"jwtUser"];
            if (jwt == null)
                return Ok(new { success = false, message = "You need to login to Order." });

            await _orderService.Create(jwt);
            return Ok(new { success = true, message = $"Order created." });
        }

        [HttpDelete("DeleteOrder/{orderId}")]
        public async Task<IActionResult> Delete(string orderId)
        {
            await _orderService.DeleteAsync(orderId);

            return Ok("Order deleted.");
        }
    }
}
