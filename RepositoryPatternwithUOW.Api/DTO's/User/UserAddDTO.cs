using System.ComponentModel.DataAnnotations;

namespace RepositoryPatternwithUOW.Api.DTO_s.User
{
    public class UserAddDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
