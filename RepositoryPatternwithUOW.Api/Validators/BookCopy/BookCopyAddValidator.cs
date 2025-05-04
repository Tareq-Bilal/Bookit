using FluentValidation;
using Microsoft.VisualBasic;
using RepositoryPatternwithUOW.Api.DTO_s.BookCopy;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Constants;

namespace RepositoryPatternwithUOW.Api.Validators.BookCopy
{
    public class BookCopyAddValidator : AbstractValidator<BookCopyAddDTO>
    {
        private readonly IUnitOfWork _unitOfWork ;
        public BookCopyAddValidator(IUnitOfWork unitOfWork) {

            _unitOfWork = unitOfWork ;

            RuleFor(x => x.BookId)
                .NotNull().WithMessage("Book Id is required");

            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("Book Id Should Be Positive");


            RuleFor(x => x.BookId)
                .Must(_IsExistBookID).WithMessage($"Not Found Book ");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status Id is required");
             
            RuleFor(x => x.Status)
                .Must(_IsValidStatus).
                WithMessage("Copy Status Is NOT Valid! , Choose One Of { Available , Loaned , Lost }");


        }

        private bool _IsValidStatus(string status)
        {
             return BookCopyStatus.Statuses.Any(s => s == status) ? true : false;
        }

        private bool _IsExistBookID(int bookId)
        {
            return _unitOfWork.Books.GetById(bookId) != null;
        }

    }
}
