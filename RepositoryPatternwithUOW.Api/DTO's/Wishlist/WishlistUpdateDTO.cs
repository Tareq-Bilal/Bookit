using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Wishlist
{
    public class WishlistUpdateDTO
    {

        // Only allowing update of BookId as UserId should not change
        // (moving an item between users' wishlists would be deletion + creation)
        [Required]
        public int BookId { get; set; }
    }
}
