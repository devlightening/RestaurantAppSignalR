using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.NotificationDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;


        public NotificationsController(INotificationService notificationService, IMapper mapper)
        {
            _notificationService = notificationService;
            _mapper = mapper;

        }

        [HttpGet]
        public IActionResult NotificationList()
        {
            var values = _notificationService.TGetListAll();
            return Ok(_mapper.Map<List<ResultNotificationDto>>(values));
        }

        [HttpGet("NotificationCountByStatusFalseCount")]
        public async  Task<IActionResult> NotificationCountByStatusFalseCount()
        {
            var values = _notificationService.TNotificationCountByStatusFalse();
            return Ok(values);
        }

        [HttpGet("GetAllNotificationByFalse")]
        public IActionResult GetAllNotificationByFalse()
        {
            return Ok(_notificationService.TGetAllNotificationByFalse());
        }

        [HttpGet("NotificationStatusTrue/{id}")]
        public IActionResult NotificationStatusTrue(int id)
        {
            _notificationService.TNotificationStatusTrue(id);
            return Ok("Bildirim durumu başarıyla değiştirildi");
        }

        [HttpGet("NotificationStatusFalse/{id}")]
        public IActionResult NotificationStatusFalse(int id)
        {
            _notificationService.TNotificationStatusFalse(id);
            return Ok("Bildirim durumu başarıyla değiştirildi");
        }

        [HttpPost]
        public IActionResult CreateNotification(CreateNotificationDto createNotificationDto)
        {
            createNotificationDto.Status = false;
            createNotificationDto.Date = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            var value = _mapper.Map<Notification>(createNotificationDto);
            _notificationService.TAdd(value);
            return Ok("Ekleme işlemi başarıyla yapıldı");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(int id)
        {
            var value = _notificationService.TGetById(id);
            _notificationService.TDelete(value);
            return Ok("Bildirim Silindi");
        }

        [HttpPut]
        public IActionResult UpdateNotification(UpdateNotificationDto updateNotificationDto)
        {
            var value = _mapper.Map<Notification>(updateNotificationDto);
            _notificationService.TUpdate(value);
            return Ok("Güncelleme işlemi başarıyla yapıldı");
        }

        [HttpGet("{id}")]
        public IActionResult GetNotification(int id)
        {
            var value = _notificationService.TGetById(id);
            return Ok(_mapper.Map<GetNotificationDto>(value));
        }
    }
}
