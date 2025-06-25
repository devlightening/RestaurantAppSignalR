using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.OrderDto;
using SignalR.EntityLayer.Entities;

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
        public IActionResult OrdersList()
        {
            var values = _mapper.Map<List<ResultOrderDto>>(_orderService.TGetListAll());
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDto createOrderDto)
        {
            _orderService.TAdd(new Order()
            {
                TableNumber = createOrderDto.TableNumber,
                Description = createOrderDto.Description,
                OrderDate = createOrderDto.OrderDate,
                TotalOrderPrice = createOrderDto.TotalOrderPrice

            });
            return Ok("Sipariş Başarıyla Eklendi");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var value = _orderService.TGetById(id);
            _orderService.TDelete(value);
            return Ok("Sipariş Başarıyla Silindi");
        }

        [HttpPut]
        public IActionResult UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            _orderService.TUpdate(new Order()
            {
                OrderId = updateOrderDto.OrderId,
                TableNumber = updateOrderDto.TableNumber,
                Description = updateOrderDto.Description,
                OrderDate = updateOrderDto.OrderDate,
                TotalOrderPrice = updateOrderDto.TotalOrderPrice
            });
            return Ok("Sipariş Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var value = _orderService.TGetById(id);
            var result = _mapper.Map<ResultOrderDto>(value);
            return Ok(result);
        }

        [HttpGet("OrdersListWithOrderDetails")]
        public IActionResult OrdersListWithOrderDetails()
        {
            var values = _mapper.Map<List<ResultOrderDto>>(_orderService.TGetListWithOrderDetails());
            return Ok(values);
        }






        [HttpGet("TotalOrderNumber")]
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
