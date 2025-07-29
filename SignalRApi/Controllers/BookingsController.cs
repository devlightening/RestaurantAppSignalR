using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.BookingDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingsController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> BookingList()
        {
            var values = _mapper.Map<List<ResultBookingDto>>(await _bookingService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking(CreateBookingDto createBookingDto)
        {
            if (createBookingDto == null) return BadRequest("Rezervasyon verisi boş olamaz.");

            var booking = _mapper.Map<Booking>(createBookingDto);
            await _bookingService.TAddAsync(booking);

            return CreatedAtAction(nameof(GetBooking), new { id = booking.BookingId }, booking);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var value = await _bookingService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile rezervasyon bulunamadı.");
            }
            await _bookingService.TDeleteAsync(value);
            return Ok("Rezervasyon Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBooking(UpdateBookingDto updateBookingDto)
        {
            if (updateBookingDto == null) return BadRequest("Güncellenecek rezervasyon verisi boş olamaz.");

            var existingBooking = await _bookingService.TGetByIdAsync(updateBookingDto.BookingId);
            if (existingBooking == null)
            {
                return NotFound($"ID {updateBookingDto.BookingId} ile rezervasyon bulunamadı.");
            }

            _mapper.Map(updateBookingDto, existingBooking);
            await _bookingService.TUpdateAsync(existingBooking);

            return Ok("Rezervasyon Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var value = await _bookingService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile rezervasyon bulunamadı.");
            }
            var result = _mapper.Map<ResultBookingDto>(value);
            return Ok(result);
        }
    }
}
