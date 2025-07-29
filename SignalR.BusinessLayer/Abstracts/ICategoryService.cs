﻿using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface ICategoryService : IGenericService<Category>
    {
        Task<int> TCategoryCountAsync();
        Task<int> TActiveCategoryCountAsync();
        Task<int> TPassiveCategoryCountAsync();
    }
}
