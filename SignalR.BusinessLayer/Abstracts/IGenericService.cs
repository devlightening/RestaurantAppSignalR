using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IGenericService<T> where T : class
    {
        Task TAddAsync(T entity); // Asenkron ekleme
        Task TDeleteAsync(T entity); // Asenkron silme
        Task TUpdateAsync(T entity); // Asenkron güncelleme
        Task<T> TGetByIdAsync(int id); // Asenkron ID'ye göre getirme
        Task<List<T>> TGetListAllAsync(); // Asenkron tüm listeyi getirme
        Task TSaveChangesAsync(); // Değişiklikleri asenkron kaydetme
    }
}
