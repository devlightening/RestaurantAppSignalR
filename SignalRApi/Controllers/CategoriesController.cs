using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.CategoryDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult CategoryList()
        {
            var values = _categoryService.TGetListAll();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateCategory(CreateCategoryDto createCategoryDto)
        {
            Category category = new Category()
            {
                CategoryName = createCategoryDto.CategoryName,
                CategoryStatus = createCategoryDto.CategoryStatus
            };
            _categoryService.TAdd(category);
            return Ok("Kategori Başarıyla Eklendi");
        }

        [HttpDelete]
        public IActionResult DeleteCategory(int id)
        {
            var value = _categoryService.TGetById(id);
            _categoryService.TDelete(value);
            return Ok("Kategori Başarıyla Silindi");
        }

        [HttpPut]
        public IActionResult UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            Category category = new Category()
            {
                CategoryId = updateCategoryDto.CategoryId,
                CategoryName = updateCategoryDto.CategoryName,
                CategoryStatus = updateCategoryDto.CategoryStatus
            };
            _categoryService.TUpdate(category);
            return Ok("Kategori Başarıyla Güncellendi");
        }

        [HttpGet("GetCategory")]
        public IActionResult GetCategory(int id)
        {
            var value = _categoryService.TGetById(id);
            return Ok(value);
        }
    }
}
