using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.MessageDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public MessagesController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        // GET: api/Messages/GetAllMessages
        [HttpGet("GetAllMessages")]
        public async Task<IActionResult> GetAllMessages()
        {
            var messages = await _messageService.TGetAllMessages();
            var result = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(result);
        }

        // GET: api/Messages/GetMessageById/5
        [HttpGet("GetMessageById/{id:int}")]
        public async Task<IActionResult> GetMessageById(int id)
        {
            var message = await _messageService.TGetMessageById(id);
            if (message == null)
                return NotFound($"ID {id} ile mesaj bulunamadı.");

            var result = _mapper.Map<ResultMessageDto>(message);
            return Ok(result);
        }

        // GET: api/Messages/GetMessagesByUserId/3
        [HttpGet("GetMessagesByUserId/{userId:int}")]
        public async Task<IActionResult> GetMessagesByUserId(int userId)
        {
            var messages = await _messageService.TGetMessagesByUserId(userId);
            var result = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(result);
        }

        // ✅ GET: api/Messages/GetMessagesByDateRange?start=2024-01-01&end=2024-12-31
        [HttpGet("GetMessagesByDateRange")]
        public async Task<IActionResult> GetMessagesByDateRange(DateTime start, DateTime end)
        {
            var messages = await _messageService.TGetMessagesByDateRange(start, end);
            var result = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(result);
        }

        // ✅ GET: api/Messages/GetConversationBetweenUsers?userId1=1&userId2=2
        [HttpGet("GetConversationBetweenUsers")]
        public async Task<IActionResult> GetConversation(int userId1, int userId2)
        {
            var messages = await _messageService.TGetConversation(userId1, userId2);
            var result = _mapper.Map<List<ResultMessageDto>>(messages);
            return Ok(result);
        }

        // POST: api/Messages/CreateMessage
        [HttpPost("CreateMessage")]
        public async Task<IActionResult> CreateMessage([FromBody] CreateMessageDto createMessageDto)
        {
            if (createMessageDto == null)
                return BadRequest("Mesaj verisi boş olamaz.");

            var message = _mapper.Map<Message>(createMessageDto);
             _messageService.TAdd(message); // await zorunlu
            var result = _mapper.Map<ResultMessageDto>(message);
            return CreatedAtAction(nameof(GetMessageById), new { id = message.MessageId }, result);
        }

        
        // DELETE: api/Messages/DeleteMessage/5
        [HttpDelete("DeleteMessage/{id:int}")]
        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _messageService.TGetMessageById(id);
            if (message == null)
                return NotFound($"ID {id} ile mesaj bulunamadı.");

            _messageService.TDelete(message); // await zorunlu
            return NoContent();
        }
    }
}
