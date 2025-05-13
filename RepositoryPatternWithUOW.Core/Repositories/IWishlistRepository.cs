using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface IWishlistRepository : IBaseRepository<Wishlist>
    {
        // In your Wishlist repository implementation
        public Task<Wishlist> GetWishlistWithDetailsAsync(int id);

        public Task AddBookToWishlist(int userId, int bookId);
        public Task RemoveBookFromWishlist(int userId, int bookId);

        // In your WishlistRepository class
        public Task<int> DeleteWhereAsync(Expression<Func<Wishlist, bool>> predicate);
    }
}
