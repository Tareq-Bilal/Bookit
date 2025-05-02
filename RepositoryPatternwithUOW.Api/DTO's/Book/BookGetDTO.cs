using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Book
{
    public class BookGetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string? Category { get; set; }
        public DateOnly? PublicationDate { get; set; }

    }
}
