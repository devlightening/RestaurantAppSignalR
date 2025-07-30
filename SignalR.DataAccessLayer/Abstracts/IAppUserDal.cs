using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IAppUserDal : IGenericDal<AppUser>
    {
        Task<IEnumerable<AppUser>> GetUsersByDepartmentAsync(UserDepartment department);

        Task<IEnumerable<AppUser>> GetOnlineUsersAsync();

        Task<AppUser> GetUserByFullNameAsync(string name, string surname);

        Task UpdateUserOnlineStatusAsync(int userId, bool isOnline);
    }
}
