using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfBasketDal : GenericRepository<Basket>, IBasketDal
    {
        private readonly SignalRContext _context;
        public EfBasketDal(SignalRContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Basket>> GetBasketsByRestaurantTableNumberAsync(int id)
        {

            return await _context.Baskets 
                .Include(b => b.RestaurantTable)
                .Include(b => b.Product)
                .Where(b => b.RestaurantTableId == id)
                .ToListAsync(); 
        }

        public async Task<decimal> TotalBasketAmountAsync()
        {
            return await _context.Baskets 
                .Where(b => b.Status == true)
                .SumAsync(b => b.TotalPrice); 

        }
    }
}
