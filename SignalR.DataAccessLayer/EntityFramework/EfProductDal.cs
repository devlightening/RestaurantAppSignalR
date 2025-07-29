using Microsoft.EntityFrameworkCore;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.Repositories;
using SignalR.EntityLayer.Entities;

namespace SignalR.DataAccessLayer.EntityFramework
{
    public class EfProductDal : GenericRepository<Product>, IProductDal
    {
        private readonly SignalRContext _context;

        public EfProductDal(SignalRContext context) : base(context)
        {
            _context = context; // Dependency Injection ile gelen context'i kullanıyoruz
        }

        // Ortalama Hamburger Fiyatını asenkron olarak döndürür
        public async Task<decimal> AvarageHamburgerPrice()
        {
            // Hamburger kategorisinin ID'sini bulur ve o kategorideki ürünlerin ortalama fiyatını hesaplar
            var hamburgerCategoryId = await _context.Categories
                                                    .Where(y => y.CategoryName == "Hamburger")
                                                    .Select(x => x.CategoryId)
                                                    .FirstOrDefaultAsync();

            if (hamburgerCategoryId == 0) // Eğer "Hamburger" kategorisi bulunamazsa
            {
                return 0; // Veya başka bir varsayılan değer
            }

            return await _context.Products
                                 .Where(z => z.CategoryId == hamburgerCategoryId)
                                 .AverageAsync(a => a.Price);
        }

        // Ortalama Ürün Fiyatını asenkron olarak döndürür
        public async Task<decimal> AvarageProductPrice()
        {
            // Tüm ürünlerin ortalama fiyatını hesaplar
            return await _context.Products.AverageAsync(x => x.Price);
        }

        // Kategorileri ile birlikte ürün listesini asenkron olarak döndürür
        public async Task<List<Product>> GetProductsWithCategory()
        {
            // Ürünleri Category navigation property'si ile birlikte yükler
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        // En pahalı ürünün adını asenkron olarak döndürür
        public async Task<string> HighestPricedProduct()
        {
            // En yüksek fiyatlı ürünü bulur ve adını döndürür
            var maxPrice = await _context.Products.MaxAsync(y => y.Price);
            return await _context.Products
                                 .Where(x => x.Price == maxPrice)
                                 .Select(z => z.ProductName)
                                 .FirstOrDefaultAsync();
        }

        // En ucuz ürünün adını asenkron olarak döndürür
        public async Task<string> LowesPricedProduct()
        {
            // En düşük fiyatlı ürünü bulur ve adını döndürür
            var minPrice = await _context.Products.MinAsync(y => y.Price);
            return await _context.Products
                                 .Where(x => x.Price == minPrice)
                                 .Select(z => z.ProductName)
                                 .FirstOrDefaultAsync();
        }

        // Toplam ürün sayısını asenkron olarak döndürür
        public async Task<int> ProductCount()
        {
            // Tüm ürünlerin sayısını döner
            return await _context.Products.CountAsync();
        }

        // İçecek kategorisindeki ürün sayısını asenkron olarak döndürür
        public async Task<int> ProductCountByCategoryNameDrink()
        {
            // "İçecek" kategorisinin ID'sini bulur ve o kategorideki ürünlerin sayısını hesaplar
            var drinkCategoryId = await _context.Categories
                                                .Where(y => y.CategoryName == "İçecek")
                                                .Select(z => z.CategoryId)
                                                .FirstOrDefaultAsync();
            if (drinkCategoryId == 0)
            {
                return 0;
            }
            return await _context.Products
                                 .Where(p => p.CategoryId == drinkCategoryId)
                                 .CountAsync();
        }

        // Hamburger kategorisindeki ürün sayısını asenkron olarak döndürür
        public async Task<int> ProductCountByCategoryNameHamburger()
        {
            // "Hamburger" kategorisinin ID'sini bulur ve o kategorideki ürünlerin sayısını hesaplar
            var hamburgerCategoryId = await _context.Categories
                                                    .Where(y => y.CategoryName == "Hamburger")
                                                    .Select(z => z.CategoryId)
                                                    .FirstOrDefaultAsync();
            if (hamburgerCategoryId == 0)
            {
                return 0;
            }
            return await _context.Products
                                 .Where(p => p.CategoryId == hamburgerCategoryId)
                                 .CountAsync();
        }
    }
}