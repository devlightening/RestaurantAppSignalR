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
    public class SliderManager : ISliderService
    {
        private readonly ISliderDal _sliderDal;

        public void TAdd(Slider entity)
        {
            _sliderDal.Add(entity);

        }

        public void TDelete(Slider entity)
        {
           _sliderDal.Delete(entity);
        }

        public Slider TGetById(int id)
        {
            return _sliderDal.GetById(id);
        }

        public List<Slider> TGetListAll()
        {
            return _sliderDal.GetListAll();
        }

        public void TUpdate(Slider entity)
        {
           _sliderDal.Update(entity);
        }
    }
}
