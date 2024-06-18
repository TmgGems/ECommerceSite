using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommereceSiteData.Repository.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepositroy Category { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }

        IShoppingCartRepository ShoppingCart { get; }

        IApplicationUserRepository ApplicationUser { get; }

        IOrderHeaderRepositroy OrderHeader { get; }

        IOrderDetailRepository OrderDetail { get; }
        void Save();
    }
}
