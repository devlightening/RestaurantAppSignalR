using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfProductDal : GenericRepository<Product>, IProductDal
    {
        public EfProductDal(SignalRContext context) : base(context)
        {
        }

        public decimal AvarageHamburgerPrice()
        {
            using var context = new SignalRContext();
            return context.Products.Where(z => z.CategoryId == (context.Categories.Where(y => y.CategoryName == "Hamburger").
            Select(x => x.CategoryId).FirstOrDefault())).Average(a=>a.Price);

        }

        public decimal AvarageProductPrice()
        {
            using var context = new SignalRContext();
            return context.Products.Average(x=>x.Price);
        }

        public List<Product> GetProductsWithCategory()
        {
            var context = new SignalRContext();
            var values= context.Products.Include(p => p.Category).ToList();
            return values;

        }

        public string HighestPricedProduct()
        {
            using var context = new SignalRContext();
            return context.Products.Where(x => x.Price == (context.Products.Max(y => y.Price))).Select(z=>z.ProductName).FirstOrDefault();
        }

        public string LowesPricedProduct()
        {
            using var context = new SignalRContext();
            return context.Products.Where(x => x.Price == (context.Products.Min(y => y.Price))).Select(z => z.ProductName).FirstOrDefault();


        }

        public int ProductCount()
        {
            using var context = new SignalRContext();
            return context.Products.Count();
        }

        public int ProductCountByCategoryNameDrink()
        {
            using var context = new SignalRContext();

            return context.Products
                .Where(p => p.CategoryId == context.Categories
                    .Where(y => y.CategoryName == "İçecek")
                    .Select(z => z.CategoryId)
                    .FirstOrDefault())
                .Count();

        }

        public int ProductCountByCategoryNameHamburger()
        {
            using var context = new SignalRContext();

            return context.Products
                .Where(p => p.CategoryId == context.Categories
                    .Where(y => y.CategoryName == "Hamburger")
                    .Select(z => z.CategoryId)
                    .FirstOrDefault())
                .Count();
        }

    
    }
}
