using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DtoLayer.AppUserDto
{
    public class UpdateAppUserDto
    {
        public int AppUserId { get; set; }                 // Kullanıcının Id'si
        public string Name { get; set; } = null!;          // Ad
        public string Surname { get; set; } = null!;       // Soyad
        public string? ProfileImageUrl { get; set; }       // Profil fotoğrafı (opsiyonel)
        public bool IsOnline { get; set; }                 // Çevrimiçi durumu
        public UserDepartment UserDepartment { get; set; } // Enum olarak bölüm
    }
}
