using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.TestimonialDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimionalsController : ControllerBase
    {
        private readonly ITestimonialService _testimonialService;
        private readonly IMapper _mapper;
        public TestimionalsController(ITestimonialService testimonialService, IMapper mapper)
        {
            _testimonialService = testimonialService;
            _mapper = mapper;
        }
        
        [HttpGet]
        public IActionResult TestimonialList()
        {
            var values =_mapper.Map<List<ResultTestimonialDto>>(_testimonialService.TGetListAll());
            return Ok(values);
        }
        [HttpPost]
        public IActionResult CreateTestimonial(CreateTestimonialDto createTestimonialDto)
        {
            _testimonialService.TAdd(new Testimonial()
            {
                Name = createTestimonialDto.Name,
                Comment = createTestimonialDto.Comment,
                ImageUrl = createTestimonialDto.ImageUrl,
                Title = createTestimonialDto.Title,
                Status = createTestimonialDto.Status
            });
            return Ok("Testimonial Başarıyla Eklendi");
        }

        [HttpDelete]
        public IActionResult DeleteTestimonial(int id)
        {
            var value = _testimonialService.TGetById(id);
            _testimonialService.TDelete(value);
            return Ok("Testimonial Başarıyla Silindi");
        }

        [HttpPut]
        public IActionResult UpdateTestimonial(UpdateTestimonialDto updateTestimonialDto)
        {
            _testimonialService.TUpdate(new Testimonial()
            {
                TestimonialId = updateTestimonialDto.TestimonialId,
                Name = updateTestimonialDto.Name,
                Comment = updateTestimonialDto.Comment,
                ImageUrl = updateTestimonialDto.ImageUrl,
                Title = updateTestimonialDto.Title,
                Status = updateTestimonialDto.Status
            });
            return Ok("Testimonial Başarıyla Güncellendi");
        }

        [HttpGet("GetTestimonial")]
        public IActionResult GetTestimonial(int id)
        {
            var value = _testimonialService.TGetById(id);
            return Ok(value);
        }
    }
}
