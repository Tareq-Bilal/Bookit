using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Book
{
    public class BookUpdateDTO
    {
        [Required, MaxLength(150)]
        public string Title { get; set; }
        public int AuthorID { get; set; }
        public int CategoryId { get; set; }
        public DateOnly? PublicationDate { get; set; }

    }
}
