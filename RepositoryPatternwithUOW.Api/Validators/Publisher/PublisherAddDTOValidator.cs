using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Category;
using RepositoryPatternwithUOW.Api.DTO_s.Publisher;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternwithUOW.Api.Validators.Publisher
{
    public class PublisherAddDTOValidator : AbstractValidator<PublisherAddDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public PublisherAddDTOValidator(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Publisher name is required.")
                .MaximumLength(120).WithMessage("Publisher name must not exceed 120 characters.")
                .Must(_IsPublisherNameTaken).WithMessage("Publisher Name Is Already Taken !");

           RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");
        }

        private bool _IsPublisherNameTaken(string publisherName)
        {
            var isTaken = _unitOfWork.Publishers.FindAsync(c => c.Name == publisherName);

            return (isTaken == null ? false : true);

        }
    
    }
}
