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
    public class DiscountManager : IDiscountService
    {
        private readonly IDiscountDal _discountDal;
        public DiscountManager(IDiscountDal discountDal)
        {
            _discountDal = discountDal;
        }

        public async Task TAddAsync(Discount entity)
        {
            await _discountDal.AddAsync(entity);
            await _discountDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task TChangeStatusToFalseAsync(int id)
        {
            // ID'ye göre indirimi bul
            var discount = await _discountDal.GetByIdAsync(id);
            if (discount != null)
            {
                discount.Status = false; // Durumu pasif yap
                await _discountDal.UpdateAsync(discount); // Güncelle
                await _discountDal.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            // Hata yönetimi eklenebilir (örn: indirim bulunamazsa loglama)
        }

        public async Task TChangeStatusToTrueAsync(int id)
        {
            // ID'ye göre indirimi bul
            var discount = await _discountDal.GetByIdAsync(id);
            if (discount != null)
            {
                discount.Status = true; // Durumu aktif yap
                await _discountDal.UpdateAsync(discount); // Güncelle
                await _discountDal.SaveChangesAsync(); // Değişiklikleri kaydet
            }
            // Hata yönetimi eklenebilir
        }

        public async Task TDeleteAsync(Discount entity)
        {
            await _discountDal.DeleteAsync(entity);
            await _discountDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task<Discount> TGetByIdAsync(int id)
        {
            return await _discountDal.GetByIdAsync(id);
        }

        public async Task<List<Discount>> TGetListAllAsync()
        {
            return await _discountDal.GetListAllAsync();
        }

        public async Task TSaveChangesAsync()
        {
            await _discountDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }

        public async Task TUpdateAsync(Discount entity)
        {
            await _discountDal.UpdateAsync(entity);
            await _discountDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }
    }
}
