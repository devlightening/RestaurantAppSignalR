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
    public class AboutManager : IAboutService
    {
        private readonly IAboutDal _aboutDal;

        public AboutManager(IAboutDal aboutDal)
        {
            _aboutDal = aboutDal;
        }

        public async Task TAddAsync(About entity)
        {
            await _aboutDal.AddAsync(entity);
            await _aboutDal.SaveChangesAsync();
        }

        public async Task TDeleteAsync(About entity)
        {
           await _aboutDal.DeleteAsync(entity);
        }
        public async Task<List<About>> TGetListAllAsync()
        {
            return await _aboutDal.GetListAllAsync();
        }

        public Task TSaveChangesAsync()
        {
            return _aboutDal.SaveChangesAsync();
        }

        public async Task TUpdateAsync(About entity)
        {
            await _aboutDal.AddAsync(entity);
            await _aboutDal.SaveChangesAsync();
        }

        Task<About> IGenericService<About>.TGetByIdAsync(int id)
        {
            return _aboutDal.GetByIdAsync(id);

        }
    }
}
