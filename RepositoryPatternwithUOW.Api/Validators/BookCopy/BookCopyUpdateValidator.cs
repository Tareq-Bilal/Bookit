using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.BookCopy;
using RepositoryPatternWithUOW.Core.Constants;

namespace RepositoryPatternwithUOW.Api.Validators.BookCopy
{
    public class BookCopyUpdateValidator : AbstractValidator<BookCopyUpdateDTO>
    {

        public BookCopyUpdateValidator() {

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status Id is required");

            RuleFor(x => x.Status)
                .Must(_IsValidStatus).
                WithMessage("Copy Status Is NOT Valid! , Choose One Of {\"Available\", \"Loaned\" , \"Lost\"}");

        }
        private bool _IsValidStatus(string status)
        {
            return BookCopyStatus.Status.Any(s => s == status) ? true : false;
        }

    }
}
