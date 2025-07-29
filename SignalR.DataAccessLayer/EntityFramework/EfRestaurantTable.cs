using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfRestaurantTable : GenericRepository<RestaurantTable>, IRestaurantTableDal
    {
        private readonly SignalRContext _context;

        public EfRestaurantTable(SignalRContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> AvailableTableCount()
        {
            return await _context.RestaurantTables.CountAsync(t => t.Status == false);
        }

        public async Task<List<RestaurantTable>> GetAvailableTables()
        {
            return await _context.RestaurantTables.Where(t => t.Status == false).ToListAsync();
        }

        public async Task<RestaurantTable> GetByTableNo(int tableNo)
        {
            return await _context.RestaurantTables.FirstOrDefaultAsync(t => t.TableNo == tableNo);
        }

        public async Task<List<RestaurantTable>> GetNotAvailableTables()
        {
            return await _context.RestaurantTables.Where(t => t.Status == true).ToListAsync();
        }

        public async Task<List<RestaurantTable>> GetTablesByLocation(TableLocation location)
        {
            return await _context.RestaurantTables.Where(t => t.Location == location).ToListAsync();
        }

        public async Task<List<RestaurantTable>> GetTablesByStatus(bool status)
        {
            return await _context.RestaurantTables.Where(t => t.Status == status).ToListAsync();
        }

        public async Task<int> NotAvailableTableCount()
        {
            return await _context.RestaurantTables.CountAsync(t => t.Status == true);
        }

        public async Task<int> TotalTableCount()
        {
            return await _context.RestaurantTables.CountAsync();
        }
    }
}
