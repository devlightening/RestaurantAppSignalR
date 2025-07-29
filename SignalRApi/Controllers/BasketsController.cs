using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.BasketDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly IMapper _mapper;
        private readonly IHubContext<SignalRHub> _hubContext;

        public BasketsController(IBasketService basketService, IMapper mapper, IHubContext<SignalRHub> hubContext)
        {
            _basketService = basketService;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetBasketsByRestaurantTableNumber(int id)
        {
            var values = _mapper.Map<List<ResultBasketDto>>(await _basketService.TGetBasketsByRestaurantTableNumberAsync(id));
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBasket(CreateBasketDto createBasketDto)
        {
            if (createBasketDto == null) return BadRequest("Sepet verisi boş olamaz.");

            var allBaskets = await _basketService.TGetListAllAsync();
            var existingBasket = allBaskets.FirstOrDefault(x => x.ProductId == createBasketDto.ProductId && x.RestaurantTableId == 24 && x.Status == true);

            if (existingBasket != null)
            {
                existingBasket.Count += 1;
                existingBasket.TotalPrice = existingBasket.Price * existingBasket.Count;
                await _basketService.TUpdateAsync(existingBasket);
            }
            else
            {
                var newBasket = new Basket()
                {
                    ProductId = createBasketDto.ProductId,
                    Count = 1,
                    RestaurantTableId = 24,
                    Price = createBasketDto.Price,
                    TotalPrice = createBasketDto.Price,
                    Status = true
                };
                await _basketService.TAddAsync(newBasket);
            }

            return Ok("Sepet başarıyla eklendi/güncellendi");
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasketById(int id)
        {
            var value = await _basketService.TGetByIdAsync(id);
            if (value == null) return NotFound($"ID {id} ile sepet öğesi bulunamadı.");
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var value = await _basketService.TGetByIdAsync(id);
            if (value != null)
            {
                await _basketService.TDeleteAsync(value);
                return Ok("Ürün başarıyla Sepetten Çıkarıldı");
            }
            return NotFound("Ürün Sepetten Çıkarılamadı");
        }

        [HttpGet("BasketListByMenuTableWithProductName")]
        public async Task<IActionResult> BasketListByMenuTableWithProductName(int id)
        {
            var baskets = await _basketService.TGetBasketsByRestaurantTableNumberAsync(id);
            var result = _mapper.Map<List<ResultBasketWithProductDto>>(baskets);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCount(int id, [FromBody] CountUpdateDto countUpdateDto)
        {
            var basket = await _basketService.TGetByIdAsync(id);
            if (basket == null)
                return NotFound();

            basket.Count = countUpdateDto.Count;
            basket.TotalPrice = basket.Count * basket.Price;
            await _basketService.TUpdateAsync(basket);

            await _hubContext.Clients.All.SendAsync("ReceiveBasketUpdate");

            return Ok("Güncellendi");
        }

        [HttpGet("BasketCount")]
        public async Task<IActionResult> BasketCount()
        {
            var count = await _basketService.TotalBasketAmountAsync();
            return Ok(count);
        }
    }
}
