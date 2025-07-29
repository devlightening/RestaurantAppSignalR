using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.DataAccessLayer.Abstracts
{
    public interface ICategoryDal : IGenericDal<Category>
    {
        // Kategori sayısını asenkron olarak döndürür
        Task<int> CategoryCountAsync();

        // Aktif kategori sayısını asenkron olarak döndürür
        Task<int> ActiveCategoryCountAsync();

        // Pasif kategori sayısını asenkron olarak döndürür
        Task<int> PassiveCategoryCountAsync();
    }
}