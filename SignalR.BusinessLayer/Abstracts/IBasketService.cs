using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IBasketService : IGenericService<Basket>
    {
        Task<List<Basket>> TGetBasketsByRestaurantTableNumberAsync(int id);
         Task<decimal> TotalBasketAmountAsync();
    }
}
