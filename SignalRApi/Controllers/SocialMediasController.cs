using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.SocialMediaDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SocialMediasController : ControllerBase
    {
        private readonly ISocialMediaService _socialMediaService;
        private readonly IMapper _mapper;
        public SocialMediasController(ISocialMediaService socialMediaService, IMapper mapper)
        {
            _socialMediaService = socialMediaService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> SocialMediaList()
        {
            var values = _mapper.Map<List<ResultSocialMediaDto>>(await _socialMediaService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSocialMedia(CreateSocialMediaDto createSocialMediaDto)
        {
            if (createSocialMediaDto == null) return BadRequest("Sosyal medya verisi boş olamaz.");

            var socialMedia = _mapper.Map<SocialMedia>(createSocialMediaDto);
            await _socialMediaService.TAddAsync(socialMedia);

            return CreatedAtAction(nameof(GetSocialMedia), new { id = socialMedia.SocialMediaId }, socialMedia);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSocialMedia(int id)
        {
            var value = await _socialMediaService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile sosyal medya bulunamadı.");
            }
            await _socialMediaService.TDeleteAsync(value);
            return Ok("Sosyal Medya Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSocialMedia(UpdateSocialMediaDto updateSocialMediaDto)
        {
            if (updateSocialMediaDto == null) return BadRequest("Güncellenecek sosyal medya verisi boş olamaz.");

            var existingSocialMedia = await _socialMediaService.TGetByIdAsync(updateSocialMediaDto.SocialMediaId);
            if (existingSocialMedia == null)
            {
                return NotFound($"ID {updateSocialMediaDto.SocialMediaId} ile sosyal medya bulunamadı.");
            }

            _mapper.Map(updateSocialMediaDto, existingSocialMedia);
            await _socialMediaService.TUpdateAsync(existingSocialMedia);

            return Ok("Sosyal Medya Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSocialMedia(int id)
        {
            var value = await _socialMediaService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile sosyal medya bulunamadı.");
            }
            var result = _mapper.Map<ResultSocialMediaDto>(value);
            return Ok(result);
        }
    }
}
