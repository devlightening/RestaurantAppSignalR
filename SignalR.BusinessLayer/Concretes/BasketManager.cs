using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Concretes
{
    public class BasketManager : IBasketService
    {
        private readonly IBasketDal _basketDal;

        public BasketManager(IBasketDal basketDal)
        {
            _basketDal = basketDal;
        }

        public async Task TAddAsync(Basket entity)
        {
            await _basketDal.AddAsync(entity);
            await _basketDal.SaveChangesAsync();
        }

        // Metot adı ve çağrısı asenkron hale getirildi
        public async Task<decimal> TotalBasketAmountAsync() // IBasketService'deki TotalBasketAmountAsync'e uygun
        {
            return await _basketDal.TotalBasketAmountAsync(); // IBasketDal'daki TotalBasketAmountAsync'i çağır
        }

        public async Task TDeleteAsync(Basket entity)
        {
            await _basketDal.DeleteAsync(entity);
            await _basketDal.SaveChangesAsync();
        }

        public async Task<Basket> TGetByIdAsync(int id)
        {
            return await _basketDal.GetByIdAsync(id);
        }

        public async Task<List<Basket>> TGetListAllAsync()
        {
            return await _basketDal.GetListAllAsync();
        }

        public async Task TSaveChangesAsync()
        {
            await _basketDal.SaveChangesAsync();
        }

        public async Task TUpdateAsync(Basket entity)
        {
            await _basketDal.UpdateAsync(entity);
            await _basketDal.SaveChangesAsync();
        }

        // Metot adı ve çağrısı asenkron hale getirildi
        public async Task<List<Basket>> TGetBasketsByRestaurantTableNumberAsync(int id) // IBasketService'deki Async versiyona uygun
        {
            return await _basketDal.GetBasketsByRestaurantTableNumberAsync(id); // IBasketDal'daki Async versiyonu çağır
        }
    }
}
