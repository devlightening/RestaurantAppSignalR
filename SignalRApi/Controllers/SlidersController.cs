using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.SliderDto;
using SignalR.EntityLayer.Entities;

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
        public IActionResult SliderList()
        {
            var values = _mapper.Map<List<ResultSliderDto>>(_sliderService.TGetListAll());
            return Ok(values);
        }

        [HttpGet("{id}")]
        public IActionResult GetSlider(int id)
        {
            var value = _sliderService.TGetById(id);
            var mappedValue = _mapper.Map<UpdateSliderDto>(value);
            return Ok(mappedValue);
        }

        [HttpPost]
        public IActionResult CreateSlider(CreateSliderDto createSliderDto)
        {
            _sliderService.TAdd(new Slider()
            {
                Title1 = createSliderDto.Title1,
                Title2 = createSliderDto.Title2,
                Title3 = createSliderDto.Title3,
                Description1 = createSliderDto.Description1,
                Description2 = createSliderDto.Description2,
                Description3 = createSliderDto.Description3
            });
            return Ok("Slider Başarıyla Eklendi");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSlider(int id)
        {
            var value = _sliderService.TGetById(id);
            _sliderService.TDelete(value);
            return Ok("Slider Başarıyla Silindi");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateSlider(int id, UpdateSliderDto updateSliderDto)
        {
            _sliderService.TUpdate(new Slider()
            {
                SliderId = id,
                Title1 = updateSliderDto.Title1,
                Title2 = updateSliderDto.Title2,
                Title3 = updateSliderDto.Title3,
                Description1 = updateSliderDto.Description1,
                Description2 = updateSliderDto.Description2,
                Description3 = updateSliderDto.Description3
            });
            return Ok("Slider Başarıyla Güncellendi");
        }
    }
}