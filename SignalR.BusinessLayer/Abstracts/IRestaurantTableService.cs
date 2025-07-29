using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IRestaurantTableService : IGenericService<RestaurantTable>
    {
        Task<int> TTotalTableCount(); // Toplam masa sayısı
        Task<int> TAvailableTableCount(); // Boş masa sayısı
        Task<int> TNotAvailableTableCount(); // Dolu masa sayısı
        Task<RestaurantTable> TGetByTableNo(int tableNo); // Belirli ada sahip masayı getirme
        Task<List<RestaurantTable>> TGetAvailableTables(); // Boş masaları listeleme
        Task<List<RestaurantTable>> TGetNotAvailableTables(); // Dolu masaları listeleme
        Task<List<RestaurantTable>> TGetTablesByStatus(bool status); // Duruma göre masaları listeleme
        Task<List<RestaurantTable>> TGetTablesByLocation(TableLocation location); // Konuma göre masaları listeleme
    }
}
