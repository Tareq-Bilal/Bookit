using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Authentication;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternwithUOW.Api.Validators.Register
{
    public class RegisterDTOValidator : AbstractValidator<RegisterDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public RegisterDTOValidator(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("User name is required.")
                .MaximumLength(200).WithMessage("User name must not exceed 200 characters.")
                .Must(_IsUserNameTaken).WithMessage("User Name Is Already Taken !");

            RuleFor(x => x.Email)
                 .EmailAddress().WithMessage("Invalid email format")
                 .Must(_IsUserNameTaken).WithMessage("Email Is Already Taken !");


            RuleFor(x => x.Password)
                        .NotEmpty().WithMessage("Password is required.")

                        // Rule 1: Minimum Length
                        .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")

                        // Rule 2: Maximum Length (optional, but good practice)
                        .MaximumLength(100).WithMessage("Password cannot be longer than 100 characters.")

                        // Rule 3: Must contain at least one uppercase letter
                        .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")

                        // Rule 4: Must contain at least one lowercase letter
                        .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")

                        // Rule 5: Must contain at least one number
                        .Matches("[0-9]").WithMessage("Password must contain at least one number.")

                        // Rule 6: Must contain at least one special character
                        .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character (e.g., !@#$%^&*).");

        }

        private bool _IsUserNameTaken(string userName)
        {
            var isTaken = _unitOfWork.Users.FindAsync(c => c.Name == userName);

            return (isTaken == null ? false : true);

        }

        private bool _IsEmailTaken(string email)
        {
            var isTaken = _unitOfWork.Users.FindAsync(c => c.Email == email);

            return (isTaken == null ? false : true);

        }

    }
}
