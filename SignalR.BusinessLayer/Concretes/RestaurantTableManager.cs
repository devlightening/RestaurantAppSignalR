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
        private readonly IRestaurantTableDal? _restaurantTableDal;

        public RestaurantTableManager(IRestaurantTableDal? restaurantTableDal)
        {
            _restaurantTableDal = restaurantTableDal;
        }

        public void TAdd(RestaurantTable entity)
        {
            _restaurantTableDal.Add(entity);

        }

        public int TAvailableTableCount()
        {

            return _restaurantTableDal.AvailableTableCount();
        }

        public void TDelete(RestaurantTable entity)
        {

            _restaurantTableDal.Delete(entity);
        }

        public List<RestaurantTable> TGetAvailableTables()
        {
            return _restaurantTableDal.GetAvailableTables();
        }

        public RestaurantTable TGetById(int id)
        {
            return _restaurantTableDal.GetById(id);
        }

        public RestaurantTable TGetByName(string tableName)
        {
            return _restaurantTableDal.GetByName(tableName);

        }

        public List<RestaurantTable> TGetListAll()
        {

            return _restaurantTableDal.GetListAll();
        }

        public List<RestaurantTable> TGetNotAvailableTables()
        {
            return _restaurantTableDal.GetNotAvailableTables();

        }

        public List<RestaurantTable> TGetTablesByLocation(TableLocation location)
        {
            return _restaurantTableDal.GetTablesByLocation(location);
        }

        public List<RestaurantTable> TGetTablesByStatus(bool status)
        {
            return _restaurantTableDal.GetTablesByStatus(status);

        }

        public int TNotAvailableTableCount() 
        { 
            return _restaurantTableDal.NotAvailableTableCount();

        }

        public int TTotalTableCount()
        {
            return _restaurantTableDal.TotalTableCount();

        }

        public void TUpdate(RestaurantTable entity)
        {

            _restaurantTableDal.Update(entity);
        }
    }
}
