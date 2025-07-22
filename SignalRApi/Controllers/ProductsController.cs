using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DtoLayer.CategoryDto;
using SignalR.DtoLayer.ProductDto;
using SignalR.DtoLayer.RestaurantTableDto;
using SignalR.EntityLayer.Entities;


namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult ProductList()
        {
            var values = _mapper.Map<List<ResultProductDto>>(_productService.TGetListAll());
            return Ok(values);
        }

        [HttpGet("ProductCount")]
        public IActionResult ProductCount()
        {
            var count = _productService.TProductCount();
            return Ok(count);
        }

        [HttpGet("ProductCountByHamburger")]
        public IActionResult ProductCountByCategoryNameHamburger()
        {
            var count = _productService.TProductCountByCategoryNameHamburger();
            return Ok(count);
        }

        [HttpGet("AvarageHamburgerPrice")]
        public IActionResult AvarageHamburgerPrice()
        {
            var count = _productService.TAvarageHamburgerPrice();
            return Ok(count);
        }

        [HttpGet("ProductCountByDrink")]
        public IActionResult ProductCountByCategoryNameDrink()
        {
            var count = _productService.TProductCountByCategoryNameDrink();
            return Ok(count);
        }


        [HttpGet("AvarageProductPrice")]
        public IActionResult AvarageProductPrice()
        {
            var count = _productService.TAvarageProductPrice();
            return Ok(count);
        }

        [HttpGet("HighestPricedProduct")]
        public IActionResult HighestPricedProduct()
        {
            var name = _productService.THighestPricedProduct();
            return Ok(name);
        }

        [HttpGet("LowesPricedProduct")]
        public IActionResult LowesPricedProduct()
        {
            var count = _productService.TLowestPricedProduct();
            return Ok(count);
        }

        [HttpPost]
        public IActionResult CreateProduct(CreateProductDto createProductDto)
        {
            _productService.TAdd(new Product()
            {
                ProductName = createProductDto.ProductName,
                Price = createProductDto.Price,
                Description = createProductDto.Description,
                ProductStatus = createProductDto.ProductStatus,
                ImageUrl = createProductDto.ImageUrl,
                CategoryId = createProductDto.CategoryId
            });
            return Ok("Ürün Başarıyla Eklendi");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var value = _productService.TGetById(id);
            _productService.TDelete(value);
            return Ok("Ürün Başarıyla Silindi");
        }

        [HttpPut]
        public IActionResult UpdateProduct(UpdateProductDto updateProductDto)
        {
            _productService.TUpdate(new Product()
            {
                ProductId = updateProductDto.ProductId,
                ProductName = updateProductDto.ProductName,
                Price = updateProductDto.Price,
                Description = updateProductDto.Description,
                ProductStatus = updateProductDto.ProductStatus,
                ImageUrl = updateProductDto.ImageUrl,
                CategoryId = updateProductDto.CategoryId
            });
            return Ok("Ürün Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var value = _productService.TGetById(id);
            return Ok(value);
        }

        [HttpGet("ProductsListWithCategory")]
        public IActionResult ProductsListWithCategory()
        {
            var context = new SignalRContext();
            var values = context.Products
                .Include(p => p.Category)
                .Select(y => new ResultProductWithCategory
                {
                    Description = y.Description,
                    ImageUrl = y.ImageUrl,
                    Price = y.Price,
                    ProductId = y.ProductId,
                    ProductName = y.ProductName,
                    ProductStatus = y.ProductStatus,
                    Category = new ResultCategoryDto()
                    {
                        CategoryId = y.Category.CategoryId,
                        CategoryName = y.Category.CategoryName,
                        CategoryStatus = y.Category.CategoryStatus
                    }
                }).ToList();

            return Ok(values);
        }
    }
}
