using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.CategoryDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> CategoryList()
        {
            var values = _mapper.Map<List<ResultCategoryDto>>(await _categoryService.TGetListAllAsync());
            return Ok(values);
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null) return BadRequest("Kategori verisi boş olamaz.");

            var category = _mapper.Map<Category>(createCategoryDto);
            await _categoryService.TAddAsync(category);

            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var value = await _categoryService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile kategori bulunamadı.");
            }
            await _categoryService.TDeleteAsync(value);
            return Ok("Kategori Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryDto updateCategoryDto)
        {
            if (updateCategoryDto == null) return BadRequest("Güncellenecek kategori verisi boş olamaz.");

            var existingCategory = await _categoryService.TGetByIdAsync(updateCategoryDto.CategoryId);
            if (existingCategory == null)
            {
                return NotFound($"ID {updateCategoryDto.CategoryId} ile kategori bulunamadı.");
            }

            _mapper.Map(updateCategoryDto, existingCategory);
            await _categoryService.TUpdateAsync(existingCategory);

            return Ok("Kategori Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var value = await _categoryService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile kategori bulunamadı.");
            }
            var result = _mapper.Map<ResultCategoryDto>(value);
            return Ok(result);
        }

        [HttpGet("CategoryCount")]
        public async Task<IActionResult> CategoryCount()
        {
            return Ok(await _categoryService.TCategoryCountAsync());
        }

        [HttpGet("ActiveCategoryCount")]
        public async Task<IActionResult> ActiveCategoryCount()
        {
            return Ok(await _categoryService.TActiveCategoryCountAsync());
        }

        [HttpGet("PassiveCategoryCount")]
        public async Task<IActionResult> PassiveCategoryCount()
        {
            return Ok(await _categoryService.TPassiveCategoryCountAsync());
        }
    }
}
