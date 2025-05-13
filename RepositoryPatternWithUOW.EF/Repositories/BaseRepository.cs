using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOW.Core.Constants;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected ApplicationDbContext _context { get; set; }

        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            
            if (includes != null)
                foreach(var include in includes)
                    query = query.Include(include);

            return await query.SingleOrDefaultAsync(criteria);
        }

        public IEnumerable<T> FindAll(string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.ToList();
        }
        public async Task<IEnumerable<T>> FindAllAsync(string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllWithOrderingAsync(string[] includes = null,
            Expression<Func<T, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<T> query = _context.Set<T>();

            if (orderBy != null)
            {
                if(orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
            
                else
                    query = query.OrderByDescending(orderBy);

            
            }

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.ToListAsync();

        }
        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().AddAsync(entity);
            return entity;
        }
        //public async Task<bool> ExistsAsync(int id)
        //{
        //    // Use AnyAsync which is more efficient for existence checks
        //    // And crucially, ConfigureAwait(false) to avoid deadlocks
        //    return await _context.Set<T>()
        //        .AnyAsync(c => c. == id)
        //        .ConfigureAwait(false);
        //}

        //public async Task<bool> ExistsAsync(string name)
        //{
        //    return await _context.Set<Category>()
        //    .AnyAsync(c => c.Name == name);
        //}

        //public T Update(T entity)
        //{
        //    _context.Update(entity);
        //    return entity;
        //}
        public void Update(T entity)
        {
            // Just mark as modified, don't query the database
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Update(entity);
            return  entity;
        }
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public async Task<int> DeleteWhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ExecuteDeleteAsync();
        }

        public async Task<int> Count(Expression<Func<T, bool>> criteria = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if(criteria != null)
                query = query.Where(criteria);

            return await query.CountAsync();
        }

    }
}
