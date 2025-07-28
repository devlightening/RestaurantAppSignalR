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

        public void TAdd(AppUser entity)
        {
            _appUserDal.Add(entity);
        }

        public void TDelete(AppUser entity)
        {
           _appUserDal.Delete(entity);
        }

        public AppUser TGetById(int id)
        {
            return _appUserDal.GetById(id);
        }

        public List<AppUser> TGetListAll()
        {
           return _appUserDal.GetListAll();
        }

        public Task<IEnumerable<AppUser>> TGetOnlineUsersAsync()
        {
            return _appUserDal.GetOnlineUsersAsync();
        }

        public Task<AppUser> TGetUserByFullNameAsync(string name, string surname)
        {
            return _appUserDal.GetUserByFullNameAsync(name, surname);
        }

        public Task<IEnumerable<AppUser>> TGetUsersByDepartmentAsync(UserDepartment department)
        {
           return _appUserDal.GetUsersByDepartmentAsync(department);
        }

        public void TUpdate(AppUser entity)
        {
           _appUserDal.Update(entity);
        }

      
    }
}
