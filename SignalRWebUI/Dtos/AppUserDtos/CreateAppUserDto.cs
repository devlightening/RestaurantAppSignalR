using SignalR.EntityLayer.Entities;

namespace SignalRWebUI.Dtos.AppUserDtos
{
    public class CreateAppUserDto
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public UserDepartment UserDepartment { get; set; }
    }

}
