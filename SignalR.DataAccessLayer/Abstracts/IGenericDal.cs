using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface IGenericDal<T> where T : class
    {
        Task AddAsync(T entity); // Asenkron ekleme
        Task DeleteAsync(T entity); // Asenkron silme
        Task UpdateAsync(T entity); // Asenkron güncelleme
        Task<T> GetByIdAsync(int id); // Asenkron ID'ye göre getirme
        Task<List<T>> GetListAllAsync(); // Asenkron tüm listeyi getirme
        Task SaveChangesAsync(); // Değişiklikleri asenkron kaydetme
    }
}
