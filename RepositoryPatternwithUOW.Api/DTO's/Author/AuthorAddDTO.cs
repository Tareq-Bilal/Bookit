using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Author
{
    public class AuthorAddDTO
    {
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Biography { get; set; }    // Short biography or description
        public DateTime DateOfBirth { get; set; }

    }
}
