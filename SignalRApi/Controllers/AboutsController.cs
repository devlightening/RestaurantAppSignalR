using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.AboutDto;
using SignalR.EntityLayer.Entities;

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
        public IActionResult AboutList()
        {
            var values =_mapper.Map<List<ResultAboutDto>>( _aboutService.TGetListAll());
            return Ok(values);

        }

        [HttpPost]
        public IActionResult CreateAbout(CreateAboutDto createAboutDto)
        {
          
            _aboutService.TAdd(new About()
            {
                Title = createAboutDto.Title,
                Description = createAboutDto.Description,
                ImageUrl = createAboutDto.ImageUrl
            });
            return Ok("Hakkında Kısmı Başarıyla Eklendi");
        }

        [HttpDelete]
        public IActionResult DeleteAbout(int id)
        {
            var value = _aboutService.TGetById(id);
            _aboutService.TDelete(value);
            return Ok("Hakkında Kısmı Başarıyla Silindi");
        }

        [HttpPut]
        public IActionResult UpdateAbout(UpdateAboutDto updateAboutDto)
        {
            _aboutService.TUpdate(new About()
            {
                AboutId = updateAboutDto.AboutId,
                Title = updateAboutDto.Title,
                Description = updateAboutDto.Description,
                ImageUrl = updateAboutDto.ImageUrl
            });

            return Ok("Hakkında Kısmı Başarıyla Güncellendi");
        }

        [HttpGet("GetAbout")]
        public IActionResult GetAbout(int id)
        {
            var value = _aboutService.TGetById(id);
            return Ok(value);

        }
    }
}
