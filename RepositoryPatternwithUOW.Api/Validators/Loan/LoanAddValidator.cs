using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Loan;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Constants;

namespace RepositoryPatternwithUOW.Api.Validators.Loan
{
    public class LoanAddValidator : AbstractValidator<LoanAddDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoanAddValidator(IUnitOfWork unitOfWork) {

            _unitOfWork = unitOfWork;

            RuleFor(x => x.BookId)
                .GreaterThan(0).WithMessage("BookId must be greater than 0.");

            RuleFor(x => x.BookId)
                .Must(_IsBookExist).WithMessage("Book Does NOT Exist");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.UserId)
                .Must(_IsUserExist).WithMessage("User Does NOT Exist");

            RuleFor(x => x.BookCopyId)
                .GreaterThan(0).WithMessage("BookCopyId must be greater than 0.");

            RuleFor(x => x.BookCopyId)
                .Must(_IsBookCopyExist).WithMessage("Book Copy Does NOT Exist");

            RuleFor(x => x.BorrowDate)
                .NotEmpty().WithMessage("BorrowDate is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("BorrowDate cannot be in the future.");

            RuleFor(x => x.DueDate)
                .GreaterThan(x => x.BorrowDate).WithMessage("DueDate must be after BorrowDate.");

            RuleFor(x => x.ReturnDate)
                .Must((dto, returnDate) => !returnDate.HasValue || returnDate >= dto.BorrowDate)
                .WithMessage("ReturnDate cannot be before BorrowDate.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => LoanStatus.Statuses.Contains(status))
                .WithMessage("Status must be one of the predefined values.");

        }

        private bool _IsBookExist(int id)
        {
            return _unitOfWork.Books.GetById(id) != null;
        }

        private bool _IsBookCopyExist(int id)
        {
            return _unitOfWork.BookCopies.GetById(id) != null;
        }

        private bool _IsUserExist(int id)
        {
            return _unitOfWork.Users.GetById(id) != null;
        }

    }
}
