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
    public class RestaurantTableManager : IRestaurantTableService
    {
        private readonly IRestaurantTableDal _restaurantTableDal;

        public RestaurantTableManager(IRestaurantTableDal restaurantTableDal)
        {
            _restaurantTableDal = restaurantTableDal;
        }

        public async Task TAddAsync(RestaurantTable entity)
        {
            await _restaurantTableDal.AddAsync(entity);
            await _restaurantTableDal.SaveChangesAsync();
        }

        public async Task<int> TAvailableTableCount()
        {
            return await _restaurantTableDal.AvailableTableCount();
        }

        public async Task TDeleteAsync(RestaurantTable entity)
        {
            await _restaurantTableDal.DeleteAsync(entity);
            await _restaurantTableDal.SaveChangesAsync();
        }

        public async Task<List<RestaurantTable>> TGetAvailableTables()
        {
            return await _restaurantTableDal.GetAvailableTables();
        }

        public async Task<RestaurantTable> TGetByIdAsync(int id)
        {
            return await _restaurantTableDal.GetByIdAsync(id);
        }

        public async Task<RestaurantTable> TGetByTableNo(int tableNo)
        {
            return await _restaurantTableDal.GetByTableNo(tableNo);
        }

        public async Task<List<RestaurantTable>> TGetListAllAsync()
        {
            return await _restaurantTableDal.GetListAllAsync();
        }

        public async Task<List<RestaurantTable>> TGetNotAvailableTables()
        {
            return await _restaurantTableDal.GetNotAvailableTables();
        }

        public async Task<List<RestaurantTable>> TGetTablesByLocation(TableLocation location)
        {
            return await _restaurantTableDal.GetTablesByLocation(location);
        }

        public async Task<List<RestaurantTable>> TGetTablesByStatus(bool status)
        {
            return await _restaurantTableDal.GetTablesByStatus(status);
        }

        public async Task<int> TNotAvailableTableCount()
        {
            return await _restaurantTableDal.NotAvailableTableCount();
        }

        public async Task TSaveChangesAsync()
        {
            await _restaurantTableDal.SaveChangesAsync();
        }

        public async Task<int> TTotalTableCount()
        {
            return await _restaurantTableDal.TotalTableCount();
        }

        public async Task TUpdateAsync(RestaurantTable entity)
        {
            await _restaurantTableDal.UpdateAsync(entity);
            await _restaurantTableDal.SaveChangesAsync();
        }
    }
}
