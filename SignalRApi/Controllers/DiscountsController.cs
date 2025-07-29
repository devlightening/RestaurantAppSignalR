using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.DiscountDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;
        public DiscountsController(IDiscountService discountService, IMapper mapper)
        {
            _discountService = discountService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> DiscountList()
        {
            var values = _mapper.Map<List<ResultDiscountDto>>(await _discountService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscount(CreateDiscountDto createDiscountDto)
        {
            if (createDiscountDto == null) return BadRequest("İndirim verisi boş olamaz.");

            var discount = _mapper.Map<Discount>(createDiscountDto);
            await _discountService.TAddAsync(discount);

            return CreatedAtAction(nameof(GetDiscount), new { id = discount.DiscountId }, discount);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var value = await _discountService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile indirim bulunamadı.");
            }
            await _discountService.TDeleteAsync(value);
            return Ok("İndirim Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateDiscount(UpdateDiscountDto updateDiscountDto)
        {
            if (updateDiscountDto == null) return BadRequest("Güncellenecek indirim verisi boş olamaz.");

            var existingDiscount = await _discountService.TGetByIdAsync(updateDiscountDto.DiscountId);
            if (existingDiscount == null)
            {
                return NotFound($"ID {updateDiscountDto.DiscountId} ile indirim bulunamadı.");
            }

            _mapper.Map(updateDiscountDto, existingDiscount);
            await _discountService.TUpdateAsync(existingDiscount);

            return Ok("İndirim Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscount(int id)
        {
            var value = await _discountService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile indirim bulunamadı.");
            }
            var result = _mapper.Map<ResultDiscountDto>(value);
            return Ok(result);
        }

        [HttpGet("ChangeStatusToTrue/{id}")]
        public async Task<IActionResult> ChangeStatusToTrue(int id)
        {
            await _discountService.TChangeStatusToTrueAsync(id);
            return Ok();
        }

        [HttpGet("ChangeStatusToFalse/{id}")]
        public async Task<IActionResult> ChangeStatusToFalse(int id)
        {
            await _discountService.TChangeStatusToFalseAsync(id);
            return Ok();
        }
    }
}
