using RepositoryPatternWithUOW.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Repositories
{
    public interface IWishlistRepository : IBaseRepository<Wishlist>
    {
        public Task AddBookToWishlist(int userId, int bookId);
        public Task RemoveBookFromWishlist(int userId, int bookId);

    }
}
