using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DtoLayer.ProductDto;
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

        [HttpGet("GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var value = _productService.TGetById(id);
            return Ok(value);
        }
        [HttpGet("ProductsListWithCategory")]
        public IActionResult ProductsListWithCategory()
        {
            var context = new SignalRContext();
            var values = context.Products.Include(p => p.Category).Select(y => new ResultProductWithCategory
            {
                Description = y.Description,
                ImageUrl = y.ImageUrl,
                Price = y.Price,
                ProductId = y.ProductId,
                ProductName = y.ProductName,
                ProductStatus = y.ProductStatus,
                CategoryName = y.Category.CategoryName // Assuming Category has a property CategoryName
            });
            return Ok(values.ToList());
        }
    }  
}
