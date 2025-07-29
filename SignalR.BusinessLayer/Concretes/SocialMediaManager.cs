using SignalR.BusinessLayer.Abstracts;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.EntityLayer.Entities;

namespace SignalR.BusinessLayer.Concretes
{
    public class SocialMediaManager : ISocialMediaService
    {
        private readonly ISocialMediaDal _socialMediaDal;
        public SocialMediaManager(ISocialMediaDal socialMediaDal)
        {
            _socialMediaDal = socialMediaDal;
        }

        public async Task TAddAsync(SocialMedia entity)
        {
            await _socialMediaDal.AddAsync(entity);
            await _socialMediaDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task TDeleteAsync(SocialMedia entity)
        {
            await _socialMediaDal.DeleteAsync(entity);
            await _socialMediaDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }

        public async Task<SocialMedia> TGetByIdAsync(int id)
        {
            return await _socialMediaDal.GetByIdAsync(id);
        }

        public async Task<List<SocialMedia>> TGetListAllAsync()
        {
            return await _socialMediaDal.GetListAllAsync();
        }

        public async Task TSaveChangesAsync()
        {
            await _socialMediaDal.SaveChangesAsync(); // DAL katmanındaki SaveChangesAsync'i çağır
        }

        public async Task TUpdateAsync(SocialMedia entity)
        {
            await _socialMediaDal.UpdateAsync(entity);
            await _socialMediaDal.SaveChangesAsync(); // Değişiklikleri veritabanına kaydet
        }
    }
}
