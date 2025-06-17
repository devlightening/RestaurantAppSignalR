using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IProductService : IGenericService<Product>
    {
        List<Product> TGetProductsWithCategory();
    }
}
