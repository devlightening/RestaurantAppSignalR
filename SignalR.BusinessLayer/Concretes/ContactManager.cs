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
    public class ContactManager : IContactService
    {
        private readonly IContactDal _contactDal;
        public ContactManager(IContactDal contactDal)
        {
            _contactDal = contactDal;
        }

        public async Task TAddAsync(Contact entity)
        {
            await _contactDal.AddAsync(entity);
            await _contactDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task TDeleteAsync(Contact entity)
        {
            await _contactDal.DeleteAsync(entity);
            await _contactDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task<Contact> TGetByIdAsync(int id)
        {
            return await _contactDal.GetByIdAsync(id);
        }

        public async Task<List<Contact>> TGetListAllAsync()
        {
            return await _contactDal.GetListAllAsync();
        }

        public async Task TSaveChangesAsync()
        {
            await _contactDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }

        public async Task TUpdateAsync(Contact entity)
        {
            await _contactDal.UpdateAsync(entity);
            await _contactDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }
    }
}
