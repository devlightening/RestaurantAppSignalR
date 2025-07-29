using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.SliderDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlidersController : ControllerBase
    {
        private readonly ISliderService _sliderService;
        private readonly IMapper _mapper;

        public SlidersController(ISliderService sliderService, IMapper mapper)
        {
            _sliderService = sliderService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> SliderList()
        {
            var values = _mapper.Map<List<ResultSliderDto>>(await _sliderService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSlider(int id)
        {
            var value = await _sliderService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile slider bulunamadı.");
            }
            var mappedValue = _mapper.Map<ResultSliderDto>(value); // Genellikle Result DTO'ya maplenir
            return Ok(mappedValue);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlider(CreateSliderDto createSliderDto)
        {
            if (createSliderDto == null) return BadRequest("Slider verisi boş olamaz.");

            var newSlider = _mapper.Map<Slider>(createSliderDto);
            await _sliderService.TAddAsync(newSlider);

            return CreatedAtAction(nameof(GetSlider), new { id = newSlider.SliderId }, newSlider);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlider(int id)
        {
            var value = await _sliderService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile slider bulunamadı.");
            }
            await _sliderService.TDeleteAsync(value);
            return Ok("Slider Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSlider(UpdateSliderDto updateSliderDto)
        {
            if (updateSliderDto == null) return BadRequest("Güncellenecek slider verisi boş olamaz.");

            var existingSlider = await _sliderService.TGetByIdAsync(updateSliderDto.SliderId);
            if (existingSlider == null)
            {
                return NotFound($"ID {updateSliderDto.SliderId} ile slider bulunamadı.");
            }

            _mapper.Map(updateSliderDto, existingSlider);
            await _sliderService.TUpdateAsync(existingSlider);

            return Ok("Slider Başarıyla Güncellendi");
        }
    }
}
