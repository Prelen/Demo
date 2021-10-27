using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Demo.Data
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<T> entity;

        public Repository(ApplicationContext context)
        {
            _context = context;
            entity = context.Set<T>();

        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await this.entity.AddRangeAsync(entities);
        }

        public void Delete(object id)
        {
            T table = this.GetByID(id);
            if (table != null)
                _context.Remove(table);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await entity.Where(predicate).ToListAsync();
        }

        public async Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate)
        {
            return await entity.Where(predicate).FirstOrDefaultAsync();
        }

        public IEnumerable<T> GetAll()
        {
            return entity.ToList();
        }

        public T GetByID(object id)
        {
            return entity.Find(id);
        }

        public void Insert(T entity)
        {
            this.entity.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            try
            {
                _context.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
            }
            catch 
            {

               
            }
           
        }

        public void UpdateAsync(IEnumerable<T> entities)
        {
            _context.AttachRange(entities);
            _context.Entry(entities).State = EntityState.Modified;
        }
    }
}
