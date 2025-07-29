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
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        // Kategori sayısını asenkron olarak döner
        public async Task<int> TCategoryCountAsync()
        {
            return await _categoryDal.CategoryCountAsync();
        }

        // Aktif kategori sayısını asenkron olarak döner
        public async Task<int> TActiveCategoryCountAsync()
        {
            return await _categoryDal.ActiveCategoryCountAsync();
        }

        // Yeni kategori ekler ve değişiklikleri kaydeder
        public async Task TAddAsync(Category entity)
        {
            await _categoryDal.AddAsync(entity);
            await _categoryDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // Kategoriyi siler ve değişiklikleri kaydeder
        public async Task TDeleteAsync(Category entity)
        {
            await _categoryDal.DeleteAsync(entity);
            await _categoryDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // ID'ye göre kategoriyi asenkron olarak getirir
        public async Task<Category> TGetByIdAsync(int id)
        {
            return await _categoryDal.GetByIdAsync(id);
        }

        // Tüm kategorilerin listesini asenkron olarak getirir
        public async Task<List<Category>> TGetListAllAsync()
        {
            return await _categoryDal.GetListAllAsync();
        }

        // Pasif kategori sayısını asenkron olarak döner
        public async Task<int> TPassiveCategoryCountAsync()
        {
            return await _categoryDal.PassiveCategoryCountAsync();
        }

        // Değişiklikleri asenkron olarak kaydeder
        public async Task TSaveChangesAsync()
        {
            await _categoryDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }

        // Kategoriyi günceller ve değişiklikleri kaydeder
        public async Task TUpdateAsync(Category entity)
        {
            await _categoryDal.UpdateAsync(entity);
            await _categoryDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }
    }
}
