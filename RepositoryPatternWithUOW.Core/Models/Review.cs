using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatternWithUOW.Core.Models
{
    // Review/Rating entity
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; } // 1-5 stars

        [MaxLength(300)]
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        // Navigation properties
        public Book Book { get; set; }
        public User User { get; set; }
    }

}
