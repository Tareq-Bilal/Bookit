using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Authentication
{
    public class RegisterDTO
    {
        [Required]
        public string Name { get; set; } = string.Empty;


        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

    }
}
