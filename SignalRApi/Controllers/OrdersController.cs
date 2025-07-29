using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.OrderDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> OrdersList()
        {
            var values = _mapper.Map<List<ResultOrderDto>>(await _orderService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null) return BadRequest("Sipariş verisi boş olamaz.");

            var order = _mapper.Map<Order>(createOrderDto);
            await _orderService.TAddAsync(order);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var value = await _orderService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile sipariş bulunamadı.");
            }
            await _orderService.TDeleteAsync(value);
            return Ok("Sipariş Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder(UpdateOrderDto updateOrderDto)
        {
            if (updateOrderDto == null) return BadRequest("Güncellenecek sipariş verisi boş olamaz.");

            var existingOrder = await _orderService.TGetByIdAsync(updateOrderDto.OrderId);
            if (existingOrder == null)
            {
                return NotFound($"ID {updateOrderDto.OrderId} ile sipariş bulunamadı.");
            }

            _mapper.Map(updateOrderDto, existingOrder);
            await _orderService.TUpdateAsync(existingOrder);

            return Ok("Sipariş Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var value = await _orderService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile sipariş bulunamadı.");
            }
            var result = _mapper.Map<ResultOrderDto>(value);
            return Ok(result);
        }

        [HttpGet("OrdersListWithOrderDetails")]
        public async Task<IActionResult> OrdersListWithOrderDetails()
        {
            var values = _mapper.Map<List<ResultOrderDto>>(await _orderService.TGetListWithOrderDetails());
            return Ok(values);
        }

        [HttpGet("TotalOrderNumber")]
        public async Task<IActionResult> TotalOrderNumber()
        {
            var count = await _orderService.TTotalOrderNumber();
            return Ok(count);
        }

        [HttpGet("ActiveOrderNumber")]
        public async Task<IActionResult> ActiveOrderNumber()
        {
            var count = await _orderService.TActiveOrderNumber();
            return Ok(count);
        }

        [HttpGet("LastOrderPrice")]
        public async Task<IActionResult> LastOrderPrice()
        {
            var count = await _orderService.TLastOrderPrice();
            return Ok(count);
        }

        [HttpGet("TodayTotalPrice")]
        public async Task<IActionResult> TodayTotalPrice()
        {
            var count = await _orderService.TTodayTotalPrice();
            return Ok(count);
        }
    }
}
