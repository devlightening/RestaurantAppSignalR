using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IBasketDal : IGenericDal<Basket>
    {
        Task<List<Basket>> GetBasketsByRestaurantTableNumberAsync(int id);
        Task<decimal> TotalBasketAmountAsync();
    }
}
