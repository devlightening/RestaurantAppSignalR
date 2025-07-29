using SignalR.EntityLayer.Entities;
using System.Text.Json.Serialization;

namespace SignalRWebUI.Dtos.AppUserDtos
{
    public class ResultAppUserDto
    {
        public int AppUserId { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName => Name + " " + Surname;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public bool IsOnline { get; set; }
        public DateTime CreatedDate { get; set; }
        public UserDepartment UserDepartment { get; set; }
    }

}
