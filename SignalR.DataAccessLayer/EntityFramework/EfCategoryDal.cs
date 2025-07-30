using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

using Microsoft.EntityFrameworkCore; // CountAsync için gerekli

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfCategoryDal : GenericRepository<Category>, ICategoryDal
    {
        private readonly SignalRContext _context;

        public EfCategoryDal(SignalRContext context) : base(context)
        {
            _context = context; // Enjekte edilen context'i kullanıyoruz
        }


        public async Task<int> CategoryCountAsync()
        {
            // Tüm kategorilerin sayısını asenkron olarak döner
            return await _context.Categories.CountAsync();
        }

        // ActiveCategoryCountAsync metodu dolduruldu
        public async Task<int> ActiveCategoryCountAsync()
        {
            // Aktif (Status == true) kategorilerin sayısını asenkron olarak döner
            return await _context.Categories.Where(c => c.CategoryStatus == true).CountAsync();
        }

        // PassiveCategoryCountAsync metodu dolduruldu
        public async Task<int> PassiveCategoryCountAsync()
        {
            // Pasif (Status == false) kategorilerin sayısını asenkron olarak döner
            return await _context.Categories.Where(c => c.CategoryStatus == false).CountAsync();
        }
    }
}
