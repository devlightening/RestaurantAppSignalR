using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IAppUserDal : IGenericDal<AppUser>
    {
        Task<IEnumerable<AppUser>> GetUsersByDepartmentAsync(UserDepartment department);

        Task<IEnumerable<AppUser>> GetOnlineUsersAsync();

        Task<AppUser> GetUserByFullNameAsync(string name, string surname);
    }
}
