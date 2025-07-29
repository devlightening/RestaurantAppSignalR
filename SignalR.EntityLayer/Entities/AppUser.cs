using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.EntityLayer.Entities
{
    public class AppUser
    {
        public int AppUserId { get; set; }

        public string Name { get; set; } = null!;          // Kullanıcı adı
        public string Surname { get; set; } = null!;          // Tam adı      /
        public string? ProfileImageUrl { get; set; }           // (İsteğe bağlı) Profil resmi

        public bool IsOnline { get; set; } = false;            // (Opsiyonel) Anlık çevrimiçi durumu
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public UserDepartment UserDepartment { get; set; }

        // Navigation
        public ICollection<Message> SentMessages { get; set; } = new HashSet<Message>();
        public ICollection<Message> ReceivedMessages { get; set; } = new HashSet<Message>();
    }

    public enum UserDepartment
    {
        Garson,
        Mutfak,
        Bar,
        Yönetici,
        Temizlik,
        Kasa
    }
}
