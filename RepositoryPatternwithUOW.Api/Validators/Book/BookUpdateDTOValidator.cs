using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Book;
using RepositoryPatternWithUOW.Core;

namespace RepositoryPatternwithUOW.Api.Validators.Book
{
    public class BookUpdateDTOValidator : AbstractValidator<BookUpdateDTO>
    {
        private readonly IUnitOfWork _unitOfWork;
        public BookUpdateDTOValidator(IUnitOfWork unitOfWork)
        {

            _unitOfWork = unitOfWork;

            RuleFor(x => x.Title)
          .NotEmpty().WithMessage("Title is required")
          .MaximumLength(150);

            RuleFor(x => x.AuthorID)
                .GreaterThan(0)
                .WithMessage("Valid AuthorId is required");

            RuleFor(x => x.AuthorID)
                .Must(IsAuthorExist)
                .WithMessage("Author Does Not Exist");





        }

        private bool IsAuthorExist(int authorID)
        {
            var author = _unitOfWork.Authors.GetById(authorID);

            return author != null;
        }
    
    }
}
