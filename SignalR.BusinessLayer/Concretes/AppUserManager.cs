using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Concretes
{
    public class AppUserManager : IAppUserService
    {
        private readonly IAppUserDal _appUserDal;

        public AppUserManager(IAppUserDal appUserDal)
        {
            _appUserDal = appUserDal;
        }

        public async Task TAddAsync(AppUser entity)
        {
            await _appUserDal.AddAsync(entity);
            await _appUserDal.SaveChangesAsync(); // DÜZELTİLDİ: await eklendi
        }

        public async Task TDeleteAsync(AppUser entity)
        {
            await _appUserDal.DeleteAsync(entity);
            await _appUserDal.SaveChangesAsync();
        }

        public async Task<AppUser> TGetByIdAsync(int id)
        {
            return await _appUserDal.GetByIdAsync(id);
        }

        public Task<List<AppUser>> TGetListAllAsync()
        {
            return _appUserDal.GetListAllAsync();
        }

        public async Task<IEnumerable<AppUser>> TGetOnlineUsersAsync()
        {
            return (await _appUserDal.GetOnlineUsersAsync()).ToList();

        }

        public async Task<AppUser> TGetUserByFullNameAsync(string name, string surname)
        {
            return await _appUserDal.GetUserByFullNameAsync(name, surname);
        }

        public async Task<IEnumerable<AppUser>> TGetUsersByDepartmentAsync(UserDepartment department)
        {
            return (await _appUserDal.GetUsersByDepartmentAsync(department)).ToList();

        }

        public async Task TSaveChangesAsync()
        {
            await _appUserDal.SaveChangesAsync();
        }

        public async Task TUpdateAsync(AppUser entity)
        {
            await _appUserDal.UpdateAsync(entity);
            await _appUserDal.SaveChangesAsync();
        }

        public async Task TUpdateUserOnlineStatusAsync(int userId, bool isOnline)
        {
             await _appUserDal.UpdateUserOnlineStatusAsync(userId, isOnline);
        }
    }
}