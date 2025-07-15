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
    public class DiscountManager : IDiscountService
    {
        private readonly IDiscountDal _discountDal;
        public DiscountManager(IDiscountDal discountDal)
        {
            _discountDal = discountDal;
        }
        public void TAdd(Discount entity)
        {
            _discountDal.Add(entity);
        }

        public void TChangeStatusToTrue(int id)
        {
            var discount = _discountDal.GetById(id);
            if (discount != null)
            {
                discount.Status = true;
                _discountDal.Update(discount);
            }
        }

        public void TChangeStatusToFalse(int id)
        {
            var discount = _discountDal.GetById(id);
            if (discount != null)
            {
                discount.Status = false;
                _discountDal.Update(discount);
            }
        }

        public void TDelete(Discount entity)
        {
            _discountDal.Delete(entity);
        }

        public Discount TGetById(int id)
        {
            return _discountDal.GetById(id);
        }

        public List<Discount> TGetListAll()
        {
            return _discountDal.GetListAll();
        }

        public void TUpdate(Discount entity)
        {
            _discountDal.Update(entity);
        }
    }
}
