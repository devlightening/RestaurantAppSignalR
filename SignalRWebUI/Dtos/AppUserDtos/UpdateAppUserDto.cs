using SignalR.EntityLayer.Entities;

namespace SignalRWebUI.Dtos.AppUserDtos
{
    public class UpdateAppUserDto
    {
        public int AppUserId { get; set; }                 // Kullanıcının Id'si
        public string Name { get; set; } = null!;          // Ad
        public string Surname { get; set; } = null!;       // Soyad
        public string Department { get; set; } = null!;     // String olarak bölüm
        public string? ProfileImageUrl { get; set; }       // Profil fotoğrafı (opsiyonel)
        public bool IsOnline { get; set; }                 // Çevrimiçi durumu
        public UserDepartment UserDepartment { get; set; } // Enum olarak bölüm
    }
}
