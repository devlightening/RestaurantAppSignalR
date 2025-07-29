using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IProductService : IGenericService<Product>
    {
        Task<List<Product>> TGetProductsWithCategory();
        Task<int> TProductCount();
        Task<int> TProductCountByCategoryNameHamburger();
        Task<int> TProductCountByCategoryNameDrink();
        Task<decimal> TAvarageProductPrice();
        Task<string> TLowestPricedProduct();
        Task<string> THighestPricedProduct();
        Task<decimal> TAvarageHamburgerPrice();
    }
}
