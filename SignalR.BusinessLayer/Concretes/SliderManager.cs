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

        public SliderManager(ISliderDal sliderDal)
        {
            _sliderDal = sliderDal;
        }

        public async Task TAddAsync(Slider entity)
        {
            await _sliderDal.AddAsync(entity);
            await _sliderDal.SaveChangesAsync();
        }

        public async Task TDeleteAsync(Slider entity)
        {
            await _sliderDal.DeleteAsync(entity);
            await _sliderDal.SaveChangesAsync();
        }

        public async Task<Slider> TGetByIdAsync(int id)
        {
            return await _sliderDal.GetByIdAsync(id);
        }

        public async Task<List<Slider>> TGetListAllAsync()
        {
            return await _sliderDal.GetListAllAsync();
        }

        public async Task TUpdateAsync(Slider entity)
        {
            await _sliderDal.UpdateAsync(entity);
            await _sliderDal.SaveChangesAsync();
        }

        public async Task TSaveChangesAsync()
        {
            await _sliderDal.SaveChangesAsync();
        }
    }
}
