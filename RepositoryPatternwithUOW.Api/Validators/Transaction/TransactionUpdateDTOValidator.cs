using FluentValidation;
using RepositoryPatternwithUOW.Api.DTO_s.Transaction;

namespace RepositoryPatternwithUOW.Api.Validators.Transaction
{
    public class TransactionUpdateDTOValidator : AbstractValidator<TransactionUpdateDTO>
    {
        public TransactionUpdateDTOValidator()
        {

            #region Amount Validations
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than 0.");

            RuleFor(x => x.Amount)
                .Must(_IsAmountInRange).WithMessage("Amount must be in Rnage Grater Than 0.01 and Less Than 10000.");
            #endregion

        }
        private bool _IsAmountInRange(decimal Amount)
        {
            return Amount is >= (decimal)0.01 and <= 10000;
        }

    }
}
