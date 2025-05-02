namespace RepositoryPatternwithUOW.Api.DTO_s.User
{
    public class UserGetDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateOnly RegistrationDate { get; set; }
        public bool IsActive { get; set; }

    }
}
