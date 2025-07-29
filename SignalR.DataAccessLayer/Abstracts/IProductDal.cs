using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IProductDal : IGenericDal<Product>
    {
        Task<List<Product>> GetProductsWithCategory();
        Task<int> ProductCount();
        Task<int> ProductCountByCategoryNameHamburger();
        Task<int> ProductCountByCategoryNameDrink();
        Task<decimal> AvarageProductPrice();
        Task<string> LowesPricedProduct();
        Task<string> HighestPricedProduct();
        Task<decimal> AvarageHamburgerPrice();
    }
}
