using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Dtos.Product;
using SignalR.DtoLayer.BasketDto;
using SignalR.EntityLayer.Entities;

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
        public IActionResult GetBasketsByRestaurantTableNumber(int id)
        {
            var values = _mapper.Map<List<ResultBasketDto>>(_basketService.TGetBasketsByRestaurantTableNumber(id));
            return Ok(values);

        }


        [HttpPost]
        public IActionResult CreateBasket(CreateBasketDto createBasketDto)
        {
            using var context = new SignalRContext();

            if (createBasketDto != null)
            {
                var existingBasket = context.Baskets
                    .FirstOrDefault(x => x.ProductId == createBasketDto.ProductId && x.RestaurantTableId == 4 && x.Status == true);

                if (existingBasket != null)
                {
                    // Aynı ürün varsa, adet ve toplam fiyat güncellenir
                    existingBasket.Count += 1;
                    existingBasket.TotalPrice = existingBasket.Price * existingBasket.Count;

                    context.Baskets.Update(existingBasket);
                }
                else
                {
                    // Ürün sepette yoksa, yeni olarak eklenir
                    var price = context.Products
                        .Where(x => x.ProductId == createBasketDto.ProductId)
                        .Select(a => a.Price)
                        .FirstOrDefault();

                    var newBasket = new Basket()
                    {
                        ProductId = createBasketDto.ProductId,
                        Count = 1,
                        RestaurantTableId = 4,
                        Price = price,
                        TotalPrice = price,
                        Status = true
                    };

                    context.Baskets.Add(newBasket);
                }

                context.SaveChanges();
                return Ok("Sepet başarıyla eklendi");
            }

            return NotFound("Sepet başarıyla eklenemedi");
        }

        [HttpGet("{id}")]
        public IActionResult GetBasketById(int id)
        {
            var value = _basketService.TGetById(id);
            return Ok(value);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBasket(int id)
        {
            var value = _basketService.TGetById(id);
            if (value != null)
            {
                _basketService.TDelete(value);
                return Ok("Ürün başarıyla Sepetten Çıkarıldı");
            }
            return NotFound("Ürün Sepetten Çıkarılamadı");
        }

        [HttpGet("BasketListByMenuTableWithProductName")]
        public IActionResult BasketListByMenuTableWithProductName(int id)
        {
            using var context = new SignalRContext();

            var baskets = context.Baskets
                .Include(b => b.Product)
                .Include(b => b.RestaurantTable)
                .Where(b => b.RestaurantTableId == id)
                .ToList();

            var result = _mapper.Map<List<ResultBasketWithProductDto>>(baskets);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCount(int id, [FromBody] CountUpdateDto countUpdateDto)
        {
            var basket = _basketService.TGetById(id);
            if (basket == null)
                return NotFound();

            basket.Count = countUpdateDto.Count;
            basket.TotalPrice = basket.Count * basket.Price;
            _basketService.TUpdate(basket);

            await _hubContext.Clients.All.SendAsync("ReceiveBasketUpdate");

            return Ok("Güncellendi");
        }

        [HttpGet("BasketCount")]
        public IActionResult BasketCount()
        {
            var count = _basketService.TBasketCount();
            return Ok(count);
        }
    }
}
