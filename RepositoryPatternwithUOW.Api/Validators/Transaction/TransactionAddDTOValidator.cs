using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Transaction;
using RepositoryPatternWithUOW.Core;
using RepositoryPatternWithUOW.Core.Constants;

namespace RepositoryPatternwithUOW.Api.Validators.Transaction
{
    public class TransactionAddDTOValidator :  AbstractValidator<TransactionAddDTO>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionAddDTOValidator(IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;

            #region User Validations
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.UserId)
                .Must(_IsUserExist).WithMessage("User Does NOT Exist");
            #endregion

            #region Loan Validations
            RuleFor(x => x.LoanId)
                .GreaterThan(0).WithMessage("Loan ID must be greater than 0.")
                .When(x => x.LoanId.HasValue);

            RuleFor(x => x.LoanId)
                .Must(_IsLoanExist).WithMessage("Loan Does NOT Exist")
                .When(x => x.LoanId.HasValue);
            #endregion

            #region TransactionDate Validatoins
            RuleFor(x => x.TransactionDate)
                .NotEmpty().WithMessage("Transaction Date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("BorrowDate cannot be in the future.");
            #endregion

            #region Amount Validations
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.Amount)
                .Must(_IsAmountInRange).WithMessage("Amount must be in Rnage Grater Than 0.01 and Less Than 10000.");
            #endregion

            #region Transaction Type Validations
            RuleFor(x => x.TransactionType)
                   .NotEmpty().WithMessage("Transactoin Type is required.")
                   .Must(type => TransactionType.TransactionTypes.Contains(type))
                   .WithMessage("Transactoin Type must be one of the predefined values { Fee , Fine , Damage , Refund }");                                                             
            #endregion                                                                   
                                                                                         
        }

        private bool _IsAmountInRange(decimal Amount)
        {
            return Amount is >= (decimal)0.01 and <= 10000;
        }
        private bool _IsLoanExist(int? id)
        {
            return _unitOfWork.Loans.GetById((int)id) != null;
        }
        private bool _IsUserExist(int id)
        {
            return _unitOfWork.Users.GetById(id) != null;
        }
    }
}
