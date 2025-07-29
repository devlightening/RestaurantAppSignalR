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
    public class TestimonialManager : ITestimonialService
    {
        private readonly ITestimonialDal _testimonialDal;
        public TestimonialManager(ITestimonialDal testimonialDal)
        {
            _testimonialDal = testimonialDal;
        }

        // Asenkron ekleme metodu
        public async Task TAddAsync(Testimonial entity)
        {
            await _testimonialDal.AddAsync(entity);
            await _testimonialDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // Asenkron silme metodu
        public async Task TDeleteAsync(Testimonial entity)
        {
            await _testimonialDal.DeleteAsync(entity);
            await _testimonialDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // ID'ye göre asenkron getirme metodu
        public async Task<Testimonial> TGetByIdAsync(int id)
        {
            return await _testimonialDal.GetByIdAsync(id);
        }

        // Tüm listeyi asenkron getirme metodu
        public async Task<List<Testimonial>> TGetListAllAsync()
        {
            return await _testimonialDal.GetListAllAsync();
        }

        // Asenkron güncelleme metodu
        public async Task TUpdateAsync(Testimonial entity)
        {
            await _testimonialDal.UpdateAsync(entity);
            await _testimonialDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // Değişiklikleri asenkron kaydetme metodu
        public async Task TSaveChangesAsync()
        {
            await _testimonialDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }
    }
}
