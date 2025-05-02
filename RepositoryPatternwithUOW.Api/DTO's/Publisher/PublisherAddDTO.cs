using RepositoryPatternWithUOW.Core.DTO_s.Book;
using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Publisher
{
    public class PublisherAddDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
