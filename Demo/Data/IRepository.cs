using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Demo.Data
{
    public interface IRepository<T> where T : class
    {
        Task AddRange(IEnumerable<T> entities);
        void Delete(object id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        T GetByID(object id);
        void Insert(T entity);
        void Save();
        Task SaveAsync();
        void Update(T entity);
        void UpdateAsync(IEnumerable<T> entities);
    }
}
