using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class BooksRepository : BaseRepository<Book>, IBooksRepository
    {
        private readonly ApplicationDbContext _context;

        public BooksRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthor(int authorId)
        {

            return await _context.Books.Where(b => b.AuthorID == authorId)
                .Include(b => b.Author).ToListAsync();
        }

        public async Task<bool> IsBookTitleExist(string Tiltle)
        {
            var isExist = await _context.Books.SingleOrDefaultAsync(b => b.Title == Tiltle);
            
            return (isExist == null ? false : true);
        }


    }
}
