using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.AppUserDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppUsersController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly IMapper _mapper;

        public AppUsersController(IAppUserService appUserService, IMapper mapper)
        {
            _appUserService = appUserService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _appUserService.TGetListAllAsync();
            var result = _mapper.Map<List<ResultAppUserDto>>(users);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _appUserService.TGetByIdAsync(id);
            if (user == null) return NotFound($"ID {id} ile kullanıcı bulunamadı.");

            var result = _mapper.Map<ResultAppUserDto>(user);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppUser([FromBody] CreateAppUserDto createDto)
        {
            if (createDto == null) return BadRequest("Kullanıcı verisi boş olamaz.");

            var user = _mapper.Map<AppUser>(createDto);
            await _appUserService.TAddAsync(user);

            var result = _mapper.Map<ResultAppUserDto>(user);
            return CreatedAtAction(nameof(GetById), new { id = user.AppUserId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAppUser(int id, [FromBody] UpdateAppUserDto updateDto)
        {
            if (updateDto == null) return BadRequest("Güncellenecek kullanıcı verisi boş olamaz.");
            if (id != updateDto.AppUserId) return BadRequest("ID uyuşmazlığı.");

            var existingUser = await _appUserService.TGetByIdAsync(id);
            if (existingUser == null) return NotFound($"ID {id} ile kullanıcı bulunamadı.");

            _mapper.Map(updateDto, existingUser);

            await _appUserService.TUpdateAsync(existingUser);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAppUser(int id)
        {
            var user = await _appUserService.TGetByIdAsync(id);
            if (user == null) return NotFound($"ID {id} ile kullanıcı bulunamadı.");

            await _appUserService.TDeleteAsync(user);
            return NoContent();
        }

        [HttpGet("online")]
        public async Task<IActionResult> GetOnlineUsers()
        {
            var onlineUsers = await _appUserService.TGetOnlineUsersAsync();
            var result = _mapper.Map<List<ResultAppUserDto>>(onlineUsers);
            return Ok(result);
        }

        [HttpGet("department/{department}")]
        public async Task<IActionResult> GetUsersByDepartment(UserDepartment department)
        {
            var users = await _appUserService.TGetUsersByDepartmentAsync(department);
            var result = _mapper.Map<List<ResultAppUserDto>>(users);
            return Ok(result);
        }
    }
}
