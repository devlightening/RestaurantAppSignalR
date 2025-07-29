using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IDiscountService : IGenericService<Discount>
    {
        Task TChangeStatusToTrueAsync(int id);
        Task TChangeStatusToFalseAsync(int id);
    }
}