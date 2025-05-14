using Microsoft.IdentityModel.Tokens;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsBookReviewdByTheUser(int userId , int bookId)
        {
            var reviewId = _context.Reviews.Where(r => r.UserId == userId && r.BookId == bookId).Select(r => r.Id);

            return reviewId != null;

        }
    }
}
