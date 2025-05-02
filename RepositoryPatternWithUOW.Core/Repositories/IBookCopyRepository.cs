using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface IBookCopyRepository : IBaseRepository<BookCopy>
    {
        public Task<IEnumerable<BookCopy>> GetBookCopiesByTitle(string title);
        public Task<IEnumerable<BookCopy>> GetBookCopiesByTitleWithBooks(string title);

    }
}
