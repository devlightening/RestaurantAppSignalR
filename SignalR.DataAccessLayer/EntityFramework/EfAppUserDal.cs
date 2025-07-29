using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfAppUserDal : GenericRepository<AppUser>, IAppUserDal
    {
        private readonly SignalRContext _context;
        public EfAppUserDal(SignalRContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AppUser>> GetOnlineUsersAsync()
        {
           return await _context.AppUsers
                .Where(user => user.IsOnline)
                .ToListAsync();
        }

        public async Task<IEnumerable<AppUser>> GetUsersByDepartmentAsync(UserDepartment department)
        {
            return await _context.AppUsers
                .Where(user => user.UserDepartment == department)
                .ToListAsync();
        }
        public async Task<AppUser> GetUserByFullNameAsync(string name, string surname)
        {
            return await _context.AppUsers
                .FirstOrDefaultAsync(user => user.Name.ToLower() == name.ToLower() &&
                                             user.Surname.ToLower() == surname.ToLower());
        }
    }
}
