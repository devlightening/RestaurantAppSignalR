using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult TotalOrderNumber()
        {
            var count = _orderService.TTotalOrderNumber();
            return Ok(count);

        }

        [HttpGet("ActiveOrderNumber")]
        public IActionResult ActiveOrderNumber()
        {
            var count = _orderService.TActiveOrderNumber();
            return Ok(count);

        }

        [HttpGet("LastOrderPrice")]
        public IActionResult LastOrderPrice()
        {
            var count = _orderService.TLastOrderPrice();
            return Ok(count);

        }
    }
}
