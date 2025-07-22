using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IRestaurantTableService :IGenericService<RestaurantTable>
    {
        public int TTotalTableCount(); // Toplam masa sayısı
        public int TAvailableTableCount(); // Boş masa sayısı
        public int TNotAvailableTableCount(); // Dolu masa sayısı
        public RestaurantTable TGetByName(string tableName); // Belirli ada sahip masayı getirme
        public List<RestaurantTable> TGetAvailableTables(); // Boş masaları listeleme
        public List<RestaurantTable> TGetNotAvailableTables(); // Dolu masaları listeleme
        public List<RestaurantTable> TGetTablesByStatus(bool status); // Duruma göre masaları listeleme
        public List<RestaurantTable> TGetTablesByLocation(TableLocation location); // Konuma göre masaları listeleme
    }
}
