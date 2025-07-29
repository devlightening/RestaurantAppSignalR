using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.NotificationDto;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<IActionResult> NotificationList()
        {
            var values = await _notificationService.TGetListAllAsync();
            return Ok(_mapper.Map<List<ResultNotificationDto>>(values));
        }

        [HttpGet("NotificationCountByStatusFalseCount")]
        public async Task<IActionResult> NotificationCountByStatusFalseCount()
        {
            var values = await _notificationService.TNotificationCountByStatusFalse();
            return Ok(values);
        }

        [HttpGet("GetAllNotificationByFalse")]
        public async Task<IActionResult> GetAllNotificationByFalse()
        {
            return Ok(await _notificationService.TGetAllNotificationByFalse());
        }

        [HttpGet("NotificationStatusTrue/{id}")]
        public async Task<IActionResult> NotificationStatusTrue(int id)
        {
            await _notificationService.TNotificationStatusTrue(id);
            return Ok("Bildirim durumu başarıyla değiştirildi");
        }

        [HttpGet("NotificationStatusFalse/{id}")]
        public async Task<IActionResult> NotificationStatusFalse(int id)
        {
            await _notificationService.TNotificationStatusFalse(id);
            return Ok("Bildirim durumu başarıyla değiştirildi");
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification(CreateNotificationDto createNotificationDto)
        {
            if (createNotificationDto == null) return BadRequest("Bildirim verisi boş olamaz.");

            createNotificationDto.Status = false;
            createNotificationDto.Date = DateTime.Now; // ToShortDateString() yerine doğrudan DateTime.Now kullanıldı
            var value = _mapper.Map<Notification>(createNotificationDto);
            await _notificationService.TAddAsync(value);
            return CreatedAtAction(nameof(GetNotification), new { id = value.NotificationId }, value);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var value = await _notificationService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile bildirim bulunamadı.");
            }
            await _notificationService.TDeleteAsync(value);
            return Ok("Bildirim Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNotification(UpdateNotificationDto updateNotificationDto)
        {
            if (updateNotificationDto == null) return BadRequest("Güncellenecek bildirim verisi boş olamaz.");

            var existingNotification = await _notificationService.TGetByIdAsync(updateNotificationDto.NotificationID);
            if (existingNotification == null)
            {
                return NotFound($"ID {updateNotificationDto.NotificationID} ile bildirim bulunamadı.");
            }

            // AutoMapper ile mevcut nesneye map'leme
            _mapper.Map(updateNotificationDto, existingNotification);
            await _notificationService.TUpdateAsync(existingNotification);
            return Ok("Güncelleme işlemi başarıyla yapıldı");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(int id)
        {
            var value = await _notificationService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile bildirim bulunamadı.");
            }
            return Ok(_mapper.Map<GetNotificationDto>(value));
        }
    }
}
