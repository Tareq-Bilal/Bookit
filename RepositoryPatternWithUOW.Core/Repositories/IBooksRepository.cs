using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface IBooksRepository : IBaseRepository<Book>
    {
        public Task<IEnumerable<Book>> GetBooksByAuthor(int authorId);
        public Task<bool> IsBookTitleExist(string Tiltle);  

    }
}
