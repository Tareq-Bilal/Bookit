using RepositoryPatternwithUOW.Api.DTO_s.Book;
using RepositoryPatternWithUOW.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Publisher
{
    public class PublisherGetDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BookGetDTO> Books { get; set; }
    }

}
