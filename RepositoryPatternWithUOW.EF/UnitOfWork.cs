using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using RepositoryPatternWithUOW.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IBaseRepository<Author> Authors { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public IBooksRepository Books { get; private set; }
        public IBookCopyRepository BookCopies { get; private set; }
        public IPublisherRepository Publishers { get; private set; }
        public IUserRepository Users { get; private set; }
        public ILoanRepository Loans { get; private set; }
        public ITransactionReposirtory Transactions { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;

            Authors      = new BaseRepository<Author>(_context);
            Books        = new BooksRepository(_context);
            BookCopies   = new BookCopyRepositroy(_context);
            Categories   = new CategoryRepository(_context);
            Publishers   = new PublisherRepository(_context);
            Users        = new UserRepository(_context);
            Loans        = new LoanRepository(_context);
            Transactions = new TransactionRepository(_context);
        }
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();

        public int Complete()
        {
            return _context.SaveChanges();
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
