using SignalR.DataAccessLayer.EntityFramework;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalR.BusinessLayer.Abstracts
{
    public interface IMoneyCaseService : IGenericService<MoneyCase>
    {
        Task<decimal> TTotalMoneyCaseAmountAsync();
    }
}
