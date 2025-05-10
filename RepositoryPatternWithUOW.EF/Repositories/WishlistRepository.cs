using Microsoft.EntityFrameworkCore;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class WishlistRepository : BaseRepository<Wishlist>, IWishlistRepository
    {
        private readonly ApplicationDbContext _context;

        public WishlistRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Add a book to a user's wishlist
        public async Task AddBookToWishlist(int userId, int bookId)
        {
            /* ======================== Validations SHould Seperated Into Vlaidation Class ========================
             * 
            // Check if user and book exist
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            var bookExists = await _context.Books.AnyAsync(b => b.Id == bookId);

            if (!userExists || !bookExists)
            {
                throw new ArgumentException("User or book not found");
            }

            // Check if the book is already in the user's wishlist
            var existingItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);

            if (existingItem != null)
            {
                throw new InvalidOperationException("Book already in wishlist");
            }

            */

            // Add the book to wishlist
            var wishlistItem = new Wishlist
            {
                UserId = userId,
                BookId = bookId,
                AddedDate = DateTime.UtcNow
            };

            _context.Wishlists.Add(wishlistItem);
            await _context.SaveChangesAsync();
        }

        // Helper class for returning book with its added date
        public class BookWithAddedDate
        {
            public Book Book { get; set; }
            public DateTime AddedDate { get; set; }
        }

        // Remove a book from wishlist
        public async Task RemoveBookFromWishlist(int userId, int bookId)
        {
            var wishlistItem = await _context.Wishlists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.BookId == bookId);

            if (wishlistItem == null)
            {
                throw new InvalidOperationException("Book not found in wishlist");
            }

            _context.Wishlists.Remove(wishlistItem);
            await _context.SaveChangesAsync();
        }

    }
}
