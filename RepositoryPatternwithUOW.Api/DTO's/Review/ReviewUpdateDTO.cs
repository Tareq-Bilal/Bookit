using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Review
{
    public class ReviewUpdateDTO
    {
        /// <summary>
        /// The Controller Should Pick The User && Book Id to Reach The Review ...
        /// </summary>
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } // 1-5 stars

        [MaxLength(300)]
        public string? Comment { get; set; }
    }
}
