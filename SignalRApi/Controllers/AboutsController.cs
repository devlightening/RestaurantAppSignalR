using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.AboutDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private readonly IAboutService _aboutService;
        private readonly IMapper _mapper;

        public AboutsController(IAboutService aboutService, IMapper mapper)
        {
            _aboutService = aboutService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> AboutList()
        {
            var values = _mapper.Map<List<ResultAboutDto>>(await _aboutService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAbout(CreateAboutDto createAboutDto)
        {
            var about = _mapper.Map<About>(createAboutDto);
            await _aboutService.TAddAsync(about);

            return CreatedAtAction(nameof(GetAbout), new { id = about.AboutId }, about);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAbout(int id)
        {
            var value = await _aboutService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile 'Hakkında' bilgisi bulunamadı.");
            }
            await _aboutService.TDeleteAsync(value);
            return Ok("Hakkında Kısmı Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            var about = _mapper.Map<About>(updateAboutDto);
            await _aboutService.TUpdateAsync(about);

            return Ok("Hakkında Kısmı Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAbout(int id)
        {
            var value = await _aboutService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile 'Hakkında' bilgisi bulunamadı.");
            }
            var result = _mapper.Map<ResultAboutDto>(value);
            return Ok(result);
        }
    }
}
