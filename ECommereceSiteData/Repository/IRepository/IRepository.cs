using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommereceSiteData.Repository.IRepository
{
    public interface IRepository <T> where T : class
    {
        //T - category or any other  model that we want to perform CRUD operations
        //IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> ?filter = null,string? includeProperties = null);
        T Get(Expression<Func<T, bool>> filter, bool tracked = false);
        void Add(T entity);


        void Remove(T entity);

        void RemoveRange(IEnumerable<T> entity);
    }
}
