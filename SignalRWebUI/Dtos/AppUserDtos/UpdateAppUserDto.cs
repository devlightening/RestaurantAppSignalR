using SignalR.EntityLayer.Entities;
namespace SignalRWebUI.Dtos.AppUserDtos
{
    public class UpdateAppUserDto
    {
        public int AppUserId { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public bool IsOnline { get; set; }
        public UserDepartment UserDepartment { get; set; }
    }
}
