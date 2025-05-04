using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Loan;
using RepositoryPatternWithUOW.Core.Constants;

namespace RepositoryPatternwithUOW.Api.Validators.Loan
{
    public class LoanUpdateValidator : AbstractValidator<LoanUpdateDTO>
    {
        public LoanUpdateValidator()
        {
            RuleFor(x => x.ReturnDate)
                .Must((dto, returnDate) => !returnDate.HasValue || returnDate >= dto.BorrowDate)
                .WithMessage("ReturnDate cannot be before BorrowDate.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => LoanStatus.Statuses.Contains(status))
                .WithMessage("Status must be one of the predefined values.");

        }
    }
}
