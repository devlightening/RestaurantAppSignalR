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
    public class MoneyCaseManager : IMoneyCaseService
    {
        private readonly IMoneyCaseDal _moneyCaseDal;

        public MoneyCaseManager(IMoneyCaseDal moneyCaseDal)
        {
            _moneyCaseDal = moneyCaseDal;
        }

        // Asenkron ekleme metodu
        public async Task TAddAsync(MoneyCase entity)
        {
            await _moneyCaseDal.AddAsync(entity);
            await _moneyCaseDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // Asenkron silme metodu
        public async Task TDeleteAsync(MoneyCase entity)
        {
            await _moneyCaseDal.DeleteAsync(entity);
            await _moneyCaseDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        // ID'ye göre asenkron getirme metodu
        public async Task<MoneyCase> TGetByIdAsync(int id)
        {
            return await _moneyCaseDal.GetByIdAsync(id);
        }

        // Tüm listeyi asenkron getirme metodu
        public async Task<List<MoneyCase>> TGetListAllAsync()
        {
            return await _moneyCaseDal.GetListAllAsync();
        }

        // Değişiklikleri asenkron kaydetme metodu
        public async Task TSaveChangesAsync()
        {
            await _moneyCaseDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }

        // Toplam kasa miktarını asenkron getirme metodu
        public async Task<decimal> TTotalMoneyCaseAmountAsync()
        {
            return await _moneyCaseDal.TotalMoneyCaseAmountAsync();
        }

        // Asenkron güncelleme metodu
        public async Task TUpdateAsync(MoneyCase entity)
        {
            await _moneyCaseDal.UpdateAsync(entity);
            await _moneyCaseDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }
    }
}
