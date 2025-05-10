using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using RepositoryPatternWithUOW.Core.Constants;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        T GetById(int id);
        Task<T> GetByIdAsync(int id);
        IEnumerable<T> GetAll();

        Task<T> FindAsync(Expression<Func<T, bool>> criteria , string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(string[] includes = null);
        IEnumerable<T> FindAll(string[] includes = null);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria , string[] includes = null);
        Task<IEnumerable<T>> FindAllWithOrderingAsync(string[] includes = null, 
            Expression <Func<T , object>> orderBy = null , string orderByDirection = OrderBy.Ascending);
        Task<T> AddAsync(T entity);
        //public Task<IEnumerable<T>>AddRangeAsync(IEnumerable<T> entities);
        public void Update(T entity);
        Task<T> UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> Count(Expression<Func<T, bool>> criteria = null);

    }
}
