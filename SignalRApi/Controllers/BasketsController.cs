using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public BasketsController(IBasketService basketservice, IMapper mapper)
        {
            _basketService = basketservice;
              _mapper = mapper;
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
                _basketService.TAdd(new Basket()
                {
                    ProductId = createBasketDto.ProductId,
                    Count = 1,
                    RestaurantTableId = 4,
                    Price = context.Products.Where(x => x.ProductId == createBasketDto.ProductId).Select(a => a.Price).FirstOrDefault(),
                    TotalPrice = 0,
                    Status = true
                });
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
                return Ok("Sepet başarıyla eklendi");
            }
            return NotFound("Sepet başarıyla eklenemedi");
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




    }
}
