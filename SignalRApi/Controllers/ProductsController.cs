using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.CategoryDto; // ResultCategoryDto için
using SignalR.DtoLayer.ProductDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq; // Select için
using System.Threading.Tasks; // async/await için

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
        public async Task<IActionResult> ProductList()
        {
            var values = _mapper.Map<List<ResultProductDto>>(await _productService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpGet("ProductCount")]
        public async Task<IActionResult> ProductCount()
        {
            var count = await _productService.TProductCount();
            return Ok(count);
        }

        [HttpGet("ProductCountByHamburger")]
        public async Task<IActionResult> ProductCountByCategoryNameHamburger()
        {
            var count = await _productService.TProductCountByCategoryNameHamburger();
            return Ok(count);
        }

        [HttpGet("AvarageHamburgerPrice")]
        public async Task<IActionResult> AvarageHamburgerPrice()
        {
            var count = await _productService.TAvarageHamburgerPrice();
            return Ok(count);
        }

        [HttpGet("ProductCountByDrink")]
        public async Task<IActionResult> ProductCountByCategoryNameDrink()
        {
            var count = await _productService.TProductCountByCategoryNameDrink();
            return Ok(count);
        }

        [HttpGet("AvarageProductPrice")]
        public async Task<IActionResult> AvarageProductPrice()
        {
            var count = await _productService.TAvarageProductPrice();
            return Ok(count);
        }

        [HttpGet("HighestPricedProduct")]
        public async Task<IActionResult> HighestPricedProduct()
        {
            var name = await _productService.THighestPricedProduct();
            return Ok(name);
        }

        [HttpGet("LowesPricedProduct")]
        public async Task<IActionResult> LowesPricedProduct()
        {
            var count = await _productService.TLowestPricedProduct();
            return Ok(count);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductDto createProductDto)
        {
            if (createProductDto == null) return BadRequest("Ürün verisi boş olamaz.");

            var product = _mapper.Map<Product>(createProductDto);
            await _productService.TAddAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var value = await _productService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile ürün bulunamadı.");
            }
            await _productService.TDeleteAsync(value);
            return Ok("Ürün Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null) return BadRequest("Güncellenecek ürün verisi boş olamaz.");

            var existingProduct = await _productService.TGetByIdAsync(updateProductDto.ProductId);
            if (existingProduct == null)
            {
                return NotFound($"ID {updateProductDto.ProductId} ile ürün bulunamadı.");
            }

            _mapper.Map(updateProductDto, existingProduct);
            await _productService.TUpdateAsync(existingProduct);

            return Ok("Ürün Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var value = await _productService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile ürün bulunamadı.");
            }
            var result = _mapper.Map<ResultProductDto>(value);
            return Ok(result);
        }

        [HttpGet("ProductsListWithCategory")]
        public async Task<IActionResult> ProductsListWithCategory()
        {
            // TGetProductsWithCategory metodu zaten ProductManager'da Include ile kategori getiriyor olmalı
            var values = await _productService.TGetProductsWithCategory();
            var result = _mapper.Map<List<ResultProductWithCategory>>(values);
            return Ok(result);
        }
    }
}
