using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        public  Task<bool> ExistsAsync(int id);
        public  Task<bool> ExistsAsync(string name);

        public Task<string> GetCategoryNameByID(int id); 

    }
}
