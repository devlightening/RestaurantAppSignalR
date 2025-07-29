using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.ContactDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly IMapper _mapper;
        public ContactsController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> ContactList()
        {
            var values = _mapper.Map<List<ResultContactDto>>(await _contactService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactDto createContactDto)
        {
            if (createContactDto == null) return BadRequest("İletişim verisi boş olamaz.");

            var contact = _mapper.Map<Contact>(createContactDto);
            await _contactService.TAddAsync(contact);

            return CreatedAtAction(nameof(GetContact), new { id = contact.ContactId }, contact);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var value = await _contactService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile iletişim bilgisi bulunamadı.");
            }
            await _contactService.TDeleteAsync(value);
            return Ok("İletişim Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateContact(UpdateContactDto updateContactDto)
        {
            if (updateContactDto == null) return BadRequest("Güncellenecek iletişim verisi boş olamaz.");

            var existingContact = await _contactService.TGetByIdAsync(updateContactDto.ContactId);
            if (existingContact == null)
            {
                return NotFound($"ID {updateContactDto.ContactId} ile iletişim bilgisi bulunamadı.");
            }

            _mapper.Map(updateContactDto, existingContact);
            await _contactService.TUpdateAsync(existingContact);

            return Ok("İletişim Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(int id)
        {
            var value = await _contactService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile iletişim bilgisi bulunamadı.");
            }
            var result = _mapper.Map<ResultContactDto>(value);
            return Ok(result);
        }
    }
}
