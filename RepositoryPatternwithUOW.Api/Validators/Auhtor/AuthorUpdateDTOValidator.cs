using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Author;

namespace RepositoryPatternwithUOW.Api.Validators.Auhtor
{
    public class AuthorUpdateDTOValidator : AbstractValidator<AuthorUpdateDTO>
    {
        public AuthorUpdateDTOValidator()
        {
            RuleFor(x => x.Name)
             .NotEmpty().WithMessage("Name is required.")
             .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.Biography)
                .MaximumLength(500).WithMessage("Biography must not exceed 500 characters.");

            RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of birth is required.")
                .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.")
                .Must(BeAtLeast18YearsOld).WithMessage("Author must be at least 18 years old.");
        }

        private bool BeAtLeast18YearsOld(DateTime dateOfBirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age)) age--; // Adjust if birthday hasn't occurred yet this year

            return age >= 18;
        }
    }
}
