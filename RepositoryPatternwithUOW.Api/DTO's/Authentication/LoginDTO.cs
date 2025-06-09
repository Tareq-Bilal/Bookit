using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.Authentication
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
