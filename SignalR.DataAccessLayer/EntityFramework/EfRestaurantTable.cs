using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfRestaurantTable : GenericRepository<RestaurantTable>, IRestaurantTableDal
    {
        public EfRestaurantTable(SignalRContext context) : base(context)
        {
        }

        public int AvailableTableCount()
        {
            using var context = new SignalRContext();
            return context.RestaurantTables.Count(t => t.Status == true);
        }


        public List<RestaurantTable> GetAvailableTables()
        {
           using var context = new SignalRContext();
            return context.RestaurantTables.Where(t => t.Status == true).ToList();
        }

        public RestaurantTable GetByName(string tableName)
        {
            using var context = new SignalRContext();
            return context.RestaurantTables.FirstOrDefault(t => t.TableName == tableName);
        }

        public List<RestaurantTable> GetNotAvailableTables()
        {
            using var context = new SignalRContext();
            return context.RestaurantTables.Where(t => t.Status == false).ToList();
        }

        public List<RestaurantTable> GetTablesByStatus(bool status)
        {
            using var context = new SignalRContext();
            return context.RestaurantTables.Where(t => t.Status == status).ToList();
        }

        public int NotAvailableTableCount()
        {
            using var context = new SignalRContext();
            return context.RestaurantTables.Count(t => t.Status == false);

        }

        public int TotalTableCount()
        {
            using var context = new SignalRContext();
            return context.RestaurantTables.Count();
        }
    }
}
