using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using RepositoryPatternWithUOW.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository <Author> Authors { get; }
        ICategoryRepository Categories { get; }
        IBooksRepository Books { get; }
        IBookCopyRepository BookCopies { get; }
        IPublisherRepository Publishers { get; }
        IUserRepository Users { get; }
        ILoanRepository Loans { get; }
        ITransactionReposirtory Transactions { get; }
        int Complete();
        Task<int> CompleteAsync();

    }
}
