using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.User;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternwithUOW.Api.Validators.User
{
    public class UserAddDTOValidator : AbstractValidator<UserAddDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserAddDTOValidator(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("User name is required.")
                .MaximumLength(200).WithMessage("User name must not exceed 200 characters.")
                .Must(_IsUserNameTaken).WithMessage("User Name Is Already Taken !");

            RuleFor(x => x.Email)
                 .EmailAddress().WithMessage("Invalid email format");
        }

        private bool _IsUserNameTaken(string userName)
        {
            var isTaken = _unitOfWork.Users.FindAsync(c => c.Name == userName);

            return (isTaken == null ? false : true);

        }
    }
}
