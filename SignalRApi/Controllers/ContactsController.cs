using AutoMapper;
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
        private readonly IMapper _mapper;
        public ContactsController(IContactService contactService, IMapper mapper)
        {
            _contactService = contactService;
            _mapper = mapper;
        }


        [HttpGet]
        public IActionResult ContactList()
        {
            var values =_mapper.Map<List<ResultContactDto>>(_contactService.TGetListAll());
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateContact(CreateContactDto createContactDto)
        {
            
            _contactService.TAdd(new Contact()
            {
                Location = createContactDto.Location,
                Mail = createContactDto.Mail,
                Phone = createContactDto.Phone,
                FooterDescription = createContactDto.FooterDescription
            });
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
            
            _contactService.TUpdate(new Contact()
            {
                ContactId = updateContactDto.ContactId,
                Location = updateContactDto.Location,
                Mail = updateContactDto.Mail,
                Phone = updateContactDto.Phone,
                FooterDescription = updateContactDto.FooterDescription,
            });
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
