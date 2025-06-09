using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Authentication;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternwithUOW.Api.Validators.Login
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginDTOValidator(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

            RuleFor(x => x.Email)
     .NotEmpty().WithMessage("Email is required.")
     .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }

    }
}
