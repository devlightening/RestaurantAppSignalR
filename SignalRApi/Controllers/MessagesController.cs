using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DtoLayer.MessageDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly SignalRContext _context; 
        public MessagesController(IMessageService messageService, IMapper mapper, SignalRContext context)
        {
            _messageService = messageService;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetAllMessages()
        {
          
            var messages = await _context.Messages
                                         .Include(m => m.SenderUser)
                                         .Include(m => m.ReceiverUser) // ReceiverUser'ı da include et
                                         .OrderByDescending(m => m.Timestamp) // En yeni mesajlar en altta görünsün
                                         .ToListAsync();

            var result = messages.Select(m => new ResultMessageDto
            {
                MessageId = m.MessageId,
                Content = m.Content, // Mesaj içeriği
                Timestamp = m.Timestamp, // Zaman damgası
                SenderFullName = m.SenderUser != null ? $"{m.SenderUser.Name} {m.SenderUser.Surname}" : "Bilinmeyen Kullanıcı", // Tam Adı
                ReceiverFullName = m.ReceiverUser != null ? $"{m.ReceiverUser.Name} {m.ReceiverUser.Surname}" : "Genel Sohbet" // Tam Adı
            }).ToList();

            return Ok(result);
        }

        [HttpGet("GetMessageById/{id:int}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _messageService.TGetMessageByIdAsync(id);
            if (message == null)
                return NotFound($"ID {id} ile mesaj bulunamadı.");

            var result = _mapper.Map<ResultMessageDto>(message);
            return Ok(result);
        }

        [HttpGet("GetMessagesByUserId/{userId:int}")]
        public async Task<IActionResult> GetMessagesByUserId(int userId)
        {
            var messages = await _messageService.TGetMessagesByUserIdAsync(userId);
            var result = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(result);
        }

        [HttpGet("GetMessagesByDateRange")]
        public async Task<IActionResult> GetMessagesByDateRange(DateTime start, DateTime end)
        {
            var messages = await _messageService.TGetMessagesByDateRangeAsync(start, end);
            var result = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(result);
        }

        [HttpGet("GetConversationBetweenUsers")]
        public async Task<IActionResult> GetConversation(int userId1, int userId2)
        {
            var messages = await _messageService.TGetConversationAsync(userId1, userId2);
            var result = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(result);
        }

        [HttpPost("CreateMessage")]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDto createMessageDto)
        {
            if (createMessageDto == null)
                return BadRequest("Mesaj verisi boş olamaz.");

            var message = _mapper.Map<Message>(createMessageDto);
            await _messageService.TAddAsync(message);
            var result = _mapper.Map<ResultMessageDto>(message);
            return CreatedAtAction(nameof(GetMessageById), new { id = message.MessageId }, result);
        }

        [HttpDelete("DeleteMessage/{id:int}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _messageService.TGetMessageByIdAsync(id);
            if (message == null)
                return NotFound($"ID {id} ile mesaj bulunamadı.");

            await _messageService.TDeleteAsync(message);
            return NoContent();
        }
    }
}
