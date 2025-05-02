using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class BookCopyRepositroy : BaseRepository<BookCopy>, IBookCopyRepository
    {
        private readonly ApplicationDbContext _context;

        public BookCopyRepositroy(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookCopy>> GetBookCopiesByTitle(string title)
        {
            int bookId = await _context.Books.Where(b => b.Title == title).Select(b => b.Id).SingleOrDefaultAsync();
           
            return await _context.BookCopies.Where(bc => bc.BookId == bookId).ToListAsync();

        }

        // In BookCopyRepository
        public async Task<IEnumerable<BookCopy>> GetBookCopiesByTitleWithBooks(string title)
        {
            return await _context.BookCopies
                .Include(bc => bc.Book)
                .Where(bc => bc.Book.Title.Contains(title))
                .ToListAsync();
        }

    }
}
