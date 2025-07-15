using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IDiscountService : IGenericService<Discount>
    {
        public void TChangeStatusToTrue(int id);
        public void TChangeStatusToFalse(int id);
    }
}