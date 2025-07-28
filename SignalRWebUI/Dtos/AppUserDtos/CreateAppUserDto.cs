using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
