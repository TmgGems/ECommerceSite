using ECommereceSiteModels.Models;
using ECommereceSiteModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommereceSiteData.Repository.IRepository
{
    public interface IOrderHeaderRepositroy : IRepository <OrderHeader>
    {
        void Update(OrderHeader obj);
    }
}
