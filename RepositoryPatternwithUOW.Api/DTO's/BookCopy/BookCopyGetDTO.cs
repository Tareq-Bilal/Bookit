using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.BookCopy
{
    public class BookCopyGetDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public string Status { get; set; } // Available, Loaned, Lost
        public DateTime AcquisitionDate { get; set; }
        public bool IsAvailable => Status == "Available";
    }
}
