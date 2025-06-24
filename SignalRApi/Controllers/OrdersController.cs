using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
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

        [HttpGet("TodayTotalPrice")]
        public IActionResult TodayTotalPrice()
        {
            var count = _orderService.TTodayTotalPrice();
            return Ok(count);

        }
    }
}
