using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.BookingDto;
using SignalR.EntityLayer.Entities;

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
        public IActionResult BookingList()
        {
            var values = _mapper.Map<List<ResultBookingDto>>(_bookingService.TGetListAll());
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateBooking(CreateBookingDto createBookingDto)
        {
          
            _bookingService.TAdd(new Booking()
            {
                Name=createBookingDto.Name,
                Mail = createBookingDto.Mail,
                Phone = createBookingDto.Phone,
                Date = createBookingDto.Date,
                PersonCount = createBookingDto.PersonCount
            });
            return Ok("Rezervasyon Başarıyla Eklendi");
        }

        [HttpDelete]
        public IActionResult DeleteBooking(int id)
        {
            var value = _bookingService.TGetById(id);
            _bookingService.TDelete(value);
            return Ok("Rezervasyon Başarıyla Silindi");
        }

        [HttpPut]
        public IActionResult UpdateBooking(UpdateBookingDto updateBookingDto)
        {

            _bookingService.TUpdate(new Booking()
            {
                Name = updateBookingDto.Name,
                Mail = updateBookingDto.Mail,
                Phone = updateBookingDto.Phone,
                Date = updateBookingDto.Date,
                PersonCount = updateBookingDto.PersonCount
            });
            return Ok("Rezervasyon Başarıyla Güncellendi");
        }

        [HttpGet("GetBooking")]
        public IActionResult GetBooking(int id)
        {
            var value = _bookingService.TGetById(id);
            return Ok(value);
        }
    }
}
