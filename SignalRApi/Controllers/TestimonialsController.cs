using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.TestimonialDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestimonialsController : ControllerBase
    {
        private readonly ITestimonialService _testimonialService;
        private readonly IMapper _mapper;
        public TestimonialsController(ITestimonialService testimonialService, IMapper mapper)
        {
            _testimonialService = testimonialService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> TestimonialList()
        {
            var values = _mapper.Map<List<ResultTestimonialDto>>(await _testimonialService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTestimonial(CreateTestimonialDto createTestimonialDto)
        {
            if (createTestimonialDto == null) return BadRequest("Referans verisi boş olamaz.");

            var testimonial = _mapper.Map<Testimonial>(createTestimonialDto);
            await _testimonialService.TAddAsync(testimonial);

            return CreatedAtAction(nameof(GetTestimonial), new { id = testimonial.TestimonialId }, testimonial);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestimonial(int id)
        {
            var value = await _testimonialService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile referans bulunamadı.");
            }
            await _testimonialService.TDeleteAsync(value);
            return Ok("Referans Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTestimonial(UpdateTestimonialDto updateTestimonialDto)
        {
            if (updateTestimonialDto == null) return BadRequest("Güncellenecek referans verisi boş olamaz.");

            var existingTestimonial = await _testimonialService.TGetByIdAsync(updateTestimonialDto.TestimonialId);
            if (existingTestimonial == null)
            {
                return NotFound($"ID {updateTestimonialDto.TestimonialId} ile referans bulunamadı.");
            }

            _mapper.Map(updateTestimonialDto, existingTestimonial);
            await _testimonialService.TUpdateAsync(existingTestimonial);

            return Ok("Referans Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestimonial(int id)
        {
            var value = await _testimonialService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile referans bulunamadı.");
            }
            var result = _mapper.Map<ResultTestimonialDto>(value);
            return Ok(result);
        }
    }
}
