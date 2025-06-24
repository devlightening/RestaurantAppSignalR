using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IRestaurantTableDal : IGenericDal<RestaurantTable>
    {
        public int TotalTableCount();
        public int AvailableTableCount(); // Boş masa sayısı
        public int NotAvailableTableCount(); //Dolu masa sayısı
        public RestaurantTable GetByName(string tableName); // Belirli ada sahip masayı getirme
        public List<RestaurantTable> GetAvailableTables(); // Boş masaları listeleme
        public List<RestaurantTable> GetNotAvailableTables(); //Dolu masaları listeleme
        public List<RestaurantTable> GetTablesByStatus(bool status); // Duruma göre masaları listeleme

    }
}
