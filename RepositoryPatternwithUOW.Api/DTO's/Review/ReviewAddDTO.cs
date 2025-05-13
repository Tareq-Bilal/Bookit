using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Review
{
    public class ReviewAddDTO
    {
        public int BookId { get; set; }
        public int UserId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } // 1-5 stars

        [MaxLength(300)]
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}
