using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Concretes
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public async Task TAddAsync(Product entity)
        {
            await _productDal.AddAsync(entity);
            await _productDal.SaveChangesAsync();
        }

        public async Task<decimal> TAvarageHamburgerPrice()
        {
            return await _productDal.AvarageHamburgerPrice();
        }

        public async Task<decimal> TAvarageProductPrice()
        {
            return await _productDal.AvarageProductPrice();
        }

        public async Task TDeleteAsync(Product entity)
        {
            await _productDal.DeleteAsync(entity);
            await _productDal.SaveChangesAsync();
        }

        public async Task<Product> TGetByIdAsync(int id)
        {
            return await _productDal.GetByIdAsync(id);
        }

        public async Task<List<Product>> TGetListAllAsync()
        {
            return await _productDal.GetListAllAsync();
        }

        public async Task<List<Product>> TGetProductsWithCategory()
        {
            return await _productDal.GetProductsWithCategory();
        }

        public async Task<string> THighestPricedProduct()
        {
            return await _productDal.HighestPricedProduct();
        }

        public async Task<string> TLowestPricedProduct()
        {
            return await _productDal.LowesPricedProduct();
        }

        public async Task<int> TProductCount()
        {
            return await _productDal.ProductCount();
        }

        public async Task<int> TProductCountByCategoryNameDrink()
        {
            return await _productDal.ProductCountByCategoryNameDrink();
        }

        public async Task<int> TProductCountByCategoryNameHamburger()
        {
            return await _productDal.ProductCountByCategoryNameHamburger();
        }

        public async Task TSaveChangesAsync()
        {
            await _productDal.SaveChangesAsync();
        }

        public async Task TUpdateAsync(Product entity)
        {
            await _productDal.UpdateAsync(entity);
            await _productDal.SaveChangesAsync();
        }
    }
}
