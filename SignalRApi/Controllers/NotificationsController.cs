using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            return Ok(values);
        }

        [HttpGet("NotificationCountByStatusFalseCount")]
        public IActionResult NotificationCountByStatusFalseCount()
        {
            var values = _notificationService.TNotificationCountByStatusFalse();
            return Ok(values);
        }

        [HttpGet("NotificationStatusTrue")]
        public IActionResult NotificationStatusTrue(int id)
        {
            _notificationService.TNotificationStatusTrue(id);
            return Ok("Bildirim durumu başarıyla değiştirildi");
        }

        [HttpGet("NotificationStatusFalse")]
        public IActionResult NotificationStatusFalse(int id)
        {
            _notificationService.TNotificationStatusFalse(id);
            return Ok("Bildirim durumu başarıyla değiştirildi");
        }

        [HttpPost]
        public IActionResult CreateNotification(CreateNotificationDto createNotificationDto)
        {
            if (createNotificationDto != null)
            {
                _notificationService.TAdd(new Notification()
                {
                    Type = createNotificationDto.Type,
                    Description = createNotificationDto.Description,
                    Icon = createNotificationDto.Icon,
                    Date = DateTime.Now,
                    Status = createNotificationDto.Status
                });
                return Ok("Bildirim Başarıyla Oluşturuldu");
            }
            return NotFound("Bildirim Başarıyla Oluşturulamadı");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(int id)
        {
            var value = _notificationService.TGetById(id);
            if (value != null)
            {
                _notificationService.TDelete(value);
                return Ok("Bildirim Başarıyla Silindi");
            }
            return NotFound("Bildirim Silinemedi.");
        }

        [HttpPut]
        public IActionResult UpdateNotification(UpdateNotificationDto updateNotificationDto)
        {
            if (updateNotificationDto != null)
            {
                var notification = _mapper.Map<Notification>(updateNotificationDto);
                _notificationService.TUpdate(notification);
                return Ok("Bildirim Başarıyla Güncellendi");
            }
            return NotFound("Bildirim Güncellenemedi");


        }

        [HttpGet("{id}")]
        public IActionResult GetNotification(int id)
        {
            var value = _notificationService.TGetById(id);
            return Ok(value);
        }
    }
}
