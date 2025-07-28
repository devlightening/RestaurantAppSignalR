using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IAppUserService : IGenericService<AppUser>
    {
        Task<IEnumerable<AppUser>> TGetUsersByDepartmentAsync(UserDepartment department);
        Task<IEnumerable<AppUser>> TGetOnlineUsersAsync();
        Task<AppUser> TGetUserByFullNameAsync(string name, string surname);
    }
}
