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
        List<Product> GetProductsWithCategory();
        public int ProductCount();
        public int ProductCountByCategoryNameHamburger();
        public int ProductCountByCategoryNameDrink();
    }
}
