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
    public class BookingManager : IBookingService
    {
        private readonly IBookingDal _bookingDal;

        public BookingManager(IBookingDal bookingDal)
        {
            _bookingDal = bookingDal;
        }

        public async Task TAddAsync(Booking entity)
        {
            await _bookingDal.AddAsync(entity);
            await _bookingDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task TDeleteAsync(Booking entity)
        {
            await _bookingDal.DeleteAsync(entity);
            await _bookingDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task<Booking> TGetByIdAsync(int id)
        {
            return await _bookingDal.GetByIdAsync(id);
        }

        public async Task<List<Booking>> TGetListAllAsync()
        {
            return await _bookingDal.GetListAllAsync();
        }

        public async Task TSaveChangesAsync()
        {
            await _bookingDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }

        public async Task TUpdateAsync(Booking entity)
        {
            await _bookingDal.UpdateAsync(entity);
            await _bookingDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }
    }
}
