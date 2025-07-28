using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.AppUserDto;
using SignalR.EntityLayer.Entities;

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

        // GET: api/AppUsers
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await Task.Run(() => _appUserService.TGetListAll());
            var result = _mapper.Map<List<ResultAppUserDto>>(users);
            return Ok(result);
        }

        // GET: api/AppUsers/5
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            var user = _appUserService.TGetById(id);
            if (user == null) return NotFound($"ID {id} ile kullanıcı bulunamadı.");

            var result = _mapper.Map<ResultAppUserDto>(user);
            return Ok(result);
        }

        // POST: api/AppUsers
        [HttpPost]
        public IActionResult Create([FromBody] CreateAppUserDto createDto)
        {
            if (createDto == null) return BadRequest("Kullanıcı verisi boş olamaz.");

            var user = _mapper.Map<AppUser>(createDto);
            _appUserService.TAdd(user);

            var result = _mapper.Map<ResultAppUserDto>(user);
            return CreatedAtAction(nameof(GetById), new { id = user.AppUserId }, result);
        }

        // PUT: api/AppUsers/5
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] CreateAppUserDto updateDto)
        {
            if (updateDto == null) return BadRequest("Güncellenecek kullanıcı verisi boş olamaz.");

            var existingUser = _appUserService.TGetById(id);
            if (existingUser == null) return NotFound($"ID {id} ile kullanıcı bulunamadı.");

            var updatedUser = _mapper.Map<AppUser>(updateDto);
            updatedUser.AppUserId = id;

            _appUserService.TUpdate(updatedUser);
            return NoContent();
        }

        // DELETE: api/AppUsers/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var user = _appUserService.TGetById(id);
            if (user == null) return NotFound($"ID {id} ile kullanıcı bulunamadı.");

            _appUserService.TDelete(user);
            return NoContent();
        }

        // Ekstra: Online kullanıcıları listele
        // GET: api/AppUsers/online
        [HttpGet("online")]
        public async Task<IActionResult> GetOnlineUsers()
        {
            var onlineUsers = await _appUserService.TGetOnlineUsersAsync();
            var result = _mapper.Map<List<ResultAppUserDto>>(onlineUsers);
            return Ok(result);
        }

        // Ekstra: Departmana göre kullanıcıları listele
        // GET: api/AppUsers/department/{department}
        [HttpGet("department/{department}")]
        public async Task<IActionResult> GetUsersByDepartment(UserDepartment department)
        {
            var users = await _appUserService.TGetUsersByDepartmentAsync(department);
            var result = _mapper.Map<List<ResultAppUserDto>>(users);
            return Ok(result);
        }
    }
}
