using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
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
            var values =_mapper.Map<List<ResultProductDto>>(_productService.TGetListAll());
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
                ImageUrl = createProductDto.ImageUrl
            });
            return Ok("Ürün Başarıyla Eklendi");
        }

        [HttpDelete]
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
                ImageUrl = updateProductDto.ImageUrl
            });
            return Ok("Ürün Başarıyla Güncellendi");
        }

        [HttpGet("GetProduct")]
        public IActionResult GetProduct(int id)
        {
            var value = _productService.TGetById(id);
            return Ok(value);
        }
    }
}
