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
    public class FeatureManager : IFeatureService
    {
        private readonly IFeatureDal _featureDal;
        public FeatureManager(IFeatureDal featureDal)
        {
            _featureDal = featureDal;
        }

        // Asenkron ekleme metodu
        public async Task TAddAsync(Feature entity)
        {
            await _featureDal.AddAsync(entity);
            await _featureDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // Asenkron silme metodu
        public async Task TDeleteAsync(Feature entity)
        {
            await _featureDal.DeleteAsync(entity);
            await _featureDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // ID'ye göre asenkron getirme metodu
        public async Task<Feature> TGetByIdAsync(int id)
        {
            return await _featureDal.GetByIdAsync(id);
        }

        // Tüm listeyi asenkron getirme metodu
        public async Task<List<Feature>> TGetListAllAsync()
        {
            return await _featureDal.GetListAllAsync();
        }

        // Asenkron güncelleme metodu
        public async Task TUpdateAsync(Feature entity)
        {
            await _featureDal.UpdateAsync(entity);
            await _featureDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // Değişiklikleri asenkron kaydetme metodu
        public async Task TSaveChangesAsync()
        {
            await _featureDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }
    }
}
