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
        Task<int> TotalTableCount();
        Task<int> AvailableTableCount(); // Boş masa sayısı
        Task<int> NotAvailableTableCount(); //Dolu masa sayısı
         Task<RestaurantTable> GetByTableNo(int tableNo); // Belirli ada sahip masayı getirme
        Task<List<RestaurantTable>> GetAvailableTables(); // Boş masaları listeleme
        Task<List<RestaurantTable>> GetNotAvailableTables(); //Dolu masaları listeleme
        Task<List<RestaurantTable>> GetTablesByStatus(bool status); // Duruma göre masaları listeleme
        Task<List<RestaurantTable>> GetTablesByLocation(TableLocation location); // Duruma göre masaları listeleme

    }
}
