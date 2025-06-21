using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IProductService : IGenericService<Product>
    {
        List<Product> TGetProductsWithCategory();
        public int TProductCount();
        public int TProductCountByCategoryNameHamburger();
        public int TProductCountByCategoryNameDrink();
    }
}
