namespace RepositoryPatternwithUOW.Api.DTO_s.Wishlist
{
    public class WishlistGetDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime AddedDate { get; set; }
    }
}
