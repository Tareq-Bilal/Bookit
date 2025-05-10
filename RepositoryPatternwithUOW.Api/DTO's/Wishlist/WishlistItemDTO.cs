namespace RepositoryPatternwithUOW.Api.DTO_s.Wishlist
{
    public class WishlistItemDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime AddedDate { get; set; }

        // Basic user information
        public string UserName { get; set; }

        // Basic book information
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
    }
}
