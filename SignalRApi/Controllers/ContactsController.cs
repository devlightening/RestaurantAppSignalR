using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.ContactDto;
using SignalR.EntityLayer.Entities;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpGet]
        public IActionResult ContactList()
        {
            var values = _contactService.TGetListAll();
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateContact(CreateContactDto createContactDto)
        {
            Contact contact = new Contact()
            {
                Location = createContactDto.Location,
                Mail = createContactDto.Mail,
                Phone = createContactDto.Phone,
                FooterDescription = createContactDto.FooterDescription
            };
            _contactService.TAdd(contact);
            return Ok("İletişim Başarıyla Eklendi");
        }

        [HttpDelete]
        public IActionResult DeleteContact(int id)
        {
            var value = _contactService.TGetById(id);
            _contactService.TDelete(value);
            return Ok("İletişim Başarıyla Silindi");
        }

        [HttpPut]
        public IActionResult UpdateContact(UpdateContactDto updateContactDto)
        {
            Contact contact = new Contact()
            {
                ContactId = updateContactDto.ContactId,
                Location = updateContactDto.Location,
                Mail = updateContactDto.Mail,
                Phone = updateContactDto.Phone,
                FooterDescription = updateContactDto.FooterDescription
            };
            _contactService.TUpdate(contact);
            return Ok("İletişim Başarıyla Güncellendi");
        }

        [HttpGet("GetContact")]
        public IActionResult GetContact(int id)
        {
            var value = _contactService.TGetById(id);
            return Ok(value);
        }
    }
}
